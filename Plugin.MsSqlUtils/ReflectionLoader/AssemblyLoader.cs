using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.SqlServer.Server;
using Plugin.MsSqlUtils.MsSqlScripting.Bll;

namespace Plugin.MsSqlUtils.ReflectionLoader
{
	[Serializable]
	internal class AssemblyLoader : MarshalByRefObject
	{
		private Assembly _loadedAssembly;

		public List<SqlReflectionDataItem> GetDataItems(String path)
		{
			List<SqlReflectionDataItem> result = new List<SqlReflectionDataItem>();
			DirectoryInfo directory = new DirectoryInfo(path);
			ResolveEventHandler resolveEventHandler = (s, e) => { return OnReflectionOnlyResolve(e, directory); };

			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;

			//Assembly reflectionOnlyAssembly = AppDomain.CurrentDomain.GetAssemblies().First();
			Assembly reflectionOnlyAssembly = _loadedAssembly;

			String assemblyName = reflectionOnlyAssembly.GetName().Name;
			foreach(Type assemblyType in reflectionOnlyAssembly.GetTypes())
			{
				SqlUserDefinedTypeAttribute type = assemblyType.GetCustomAttributes<SqlUserDefinedTypeAttribute>();
				if(type != null)
				{//Тип
					SqlTypeInfo info = new SqlTypeInfo(assemblyType, type);
					result.Add(new SqlReflectionDataItem(SqlReflectionDataItem.SqlType.Type, assemblyName, info.NamespaceName, info.TypeName, info.TypeName, info.GetTypeParameters(), null));
					continue;
				}
				SqlUserDefinedAggregateAttribute aggregate = assemblyType.GetCustomAttributes<SqlUserDefinedAggregateAttribute>();
				if(aggregate != null)
				{//Агрегат
					SqlAggregateInfo info = new SqlAggregateInfo(assemblyType, aggregate);
					result.Add(new SqlReflectionDataItem(SqlReflectionDataItem.SqlType.Aggregate, assemblyName, info.NamespaceName, info.AggregateName, info.AggregateName, info.GetAggregateParameters(), info.GetReturnParameters()));
					continue;
				}

				foreach(MethodInfo method in assemblyType.GetMethods())
				{//Поиск методов
					SqlFunctionAttribute attribute = method.GetCustomAttributes<SqlFunctionAttribute>();
					if(attribute != null)
					{
						SqlFunctionInfo info = new SqlFunctionInfo(assemblyType.Name, attribute, method);
						result.Add(new SqlReflectionDataItem(SqlReflectionDataItem.SqlType.Function, assemblyName, info.NamespaceName,info.MemberName, info.FunctionName, info.GetFunctionParameters(), info.GetReturnParameters()));
					}
				}
			}

			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;

			return result;
		}

		/// <summary>Gets namespaces for ReflectionOnly Loaded Assemblies</summary>
		/// <param name="path">The path to the Assembly</param>
		/// <returns>A List of namespace strings</returns>
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		public List<String> GetNamespaces(String path)
		{
			List<String> namespaces = new List<String>();

			DirectoryInfo directory = new DirectoryInfo(path);
			ResolveEventHandler resolveEventHandler = (s, e) => { return OnReflectionOnlyResolve(e, directory); };

			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;

			Assembly reflectionOnlyAssembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().First();

			foreach(Type type in reflectionOnlyAssembly.GetTypes())
				if(!namespaces.Contains(type.Namespace))
					namespaces.Add(type.Namespace);

			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;
			return namespaces;
		}

		/// <summary>Attempts ReflectionOnlyLoad of current Assemblies dependants</summary>
		/// <param name="args">ReflectionOnlyAssemblyResolve event args</param>
		/// <param name="directory">The current Assemblies Directory</param>
		/// <returns>ReflectionOnlyLoadFrom loaded dependant Assembly</returns>
		private Assembly OnReflectionOnlyResolve(ResolveEventArgs args, DirectoryInfo directory)
		{
			Assembly loadedAssembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(asm => String.Equals(asm.FullName, args.Name, StringComparison.OrdinalIgnoreCase));

			if(loadedAssembly != null)
				return loadedAssembly;

			AssemblyName assemblyName = new AssemblyName(args.Name);
			String dependentAssemblyFilename = Path.Combine(directory.FullName, assemblyName.Name + ".dll");

			if(File.Exists(dependentAssemblyFilename))
				return Assembly.ReflectionOnlyLoadFrom(dependentAssemblyFilename);
			return Assembly.ReflectionOnlyLoad(args.Name);
		}

		/// <summary>ReflectionOnlyLoad of single Assembly based on the assemblyPath parameter</summary>
		/// <param name="assemblyPath">The path to the Assembly</param>
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		internal void LoadAssembly(String assemblyPath)
		{
			try
			{
				this._loadedAssembly = Assembly.LoadFile(assemblyPath);
			} catch(FileNotFoundException)
			{
				/* Continue loading assemblies even if an assembly can not be loaded in the new AppDomain. */
			}
		}
	}
}