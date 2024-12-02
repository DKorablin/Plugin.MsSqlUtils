using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.SqlServer.Server;
using Plugin.MsSqlUtils.MsSqlScripting.Bll;
using Plugin.MsSqlUtils.ReflectionLoader;
using System.Security.Policy;

namespace Plugin.MsSqlUtils.MsSqlScripting
{
	[Serializable]
	internal class MsSqlAssembly : IDisposable,IEnumerable<SqlReflectionDataItem>
	{
		private readonly String _assemblyPath;
		private readonly Byte[] _rawAssembly;
		private Assembly _assembly;
		private AppDomain _domain;

		/// <summary>Домен в который загружена сборка</summary>
		private AppDomain Domain
		{
			get
			{
				if(this._domain == null)
				{
					AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
					setup.ApplicationBase = Path.GetDirectoryName(this._assemblyPath);
					this._domain = AppDomain.CreateDomain("MSSQL Assembly Loader", AppDomain.CurrentDomain.Evidence, setup);
				}
				return this._domain;
			}
		}

		/// <summary>Сборка, которую разбираем</summary>
		private Assembly Assembly
		{
			get
			{
				if(this._assembly == null)
					this._assembly = Assembly.Load(this._rawAssembly);
					//this._assembly = this.Domain.Load(this._rawAssembly);
				return this._assembly;
			}
		}

		public String AssemblyName => this.Assembly.GetName().Name;
		public MsSqlAssembly(String filePath)
		{
			if(String.IsNullOrEmpty(filePath))
				throw new ArgumentNullException(nameof(filePath));
			if(!File.Exists(filePath))
				throw new FileNotFoundException(filePath);

			this._assemblyPath = Path.GetFullPath(filePath);
			this._rawAssembly = File.ReadAllBytes(filePath);
		}

		public String GetHexAssembly()
			=> SqlUtils.HexToString(this._rawAssembly);

		public IEnumerable<SqlFunctionInfo> GetMethods()
		{
			foreach(Type assemblyType in this.Assembly.GetTypes())
				foreach(MethodInfo method in assemblyType.GetMethods())
				{
					SqlFunctionAttribute attribute = method.GetCustomAttributes<SqlFunctionAttribute>();
					if(attribute != null)
						yield return new SqlFunctionInfo(assemblyType.Name, attribute, method);
				}
		}

		public IEnumerable<SqlTypeInfo> GetTypes()
		{
			foreach(Type assemblyType in this.Assembly.GetTypes())
			{
				SqlUserDefinedTypeAttribute attribute = assemblyType.GetCustomAttributes<SqlUserDefinedTypeAttribute>();
				if(attribute != null)
					yield return new SqlTypeInfo(assemblyType, attribute);
			}
		}

		public IEnumerable<SqlAggregateInfo> GetAggregates()
		{
			foreach(Type assemblyType in this.Assembly.GetTypes())
			{
				SqlUserDefinedAggregateAttribute attribute = assemblyType.GetCustomAttributes<SqlUserDefinedAggregateAttribute>();
				if(attribute != null)
					yield return new SqlAggregateInfo(assemblyType, attribute);
			}
		}

		/*private IEnumerable<T> GetCustomAttribute<T>() where T : Attribute
		{
			foreach(Type assemblyType in this.Assembly.GetTypes())
			{
				Object[] attributes = assemblyType.GetCustomAttributes(typeof(T), false);
				if(attributes != null && attributes.Length == 1)
					yield return (T)attributes[0];
			}
		}*/

		public void Dispose()
		{
			if(this._domain != null)
			{
				AppDomain.Unload(this._domain);
				this._domain = null;
			}
		}

		public IEnumerator<SqlReflectionDataItem> GetEnumerator()
		{
			AppDomain childDomain = this.BuildChildDomain(AppDomain.CurrentDomain);
			try
			{
				Type loaderType = typeof(AssemblyLoader);
				if(loaderType.Assembly != null)
				{
					AssemblyLoader loader = (AssemblyLoader)childDomain.CreateInstanceFrom(loaderType.Assembly.Location, loaderType.FullName).Unwrap();

					loader.LoadAssembly(this._assemblyPath);
					foreach(SqlReflectionDataItem dataItem in loader.GetDataItems(this._assemblyPath))
						yield return dataItem;
				}
			} finally
			{
				AppDomain.Unload(childDomain);
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			=> this.GetEnumerator();

		/// <summary>Creates a new AppDomain based on the parent AppDomains Evidence and AppDomainSetup</summary>
		/// <param name="parentDomain">The parent AppDomain</param>
		/// <returns>A newly created AppDomain</returns>
		private AppDomain BuildChildDomain(AppDomain parentDomain)
		{
			Evidence evidence = new Evidence(parentDomain.Evidence);
			AppDomainSetup setup = parentDomain.SetupInformation;
			return AppDomain.CreateDomain("DiscoveryRegion", evidence, setup);
		}
	}
}