using System;
using System.Reflection;
using System.Text;
using Microsoft.SqlServer.Server;

namespace Plugin.MsSqlUtils.MsSqlScripting.Bll
{
	internal class SqlFunctionInfo : SqlObjectInfo<SqlFunctionAttribute>
	{
		public MethodInfo Method { get; }

		/// <summary>Scalar-value Function</summary>
		public Boolean IsSvf { get => String.IsNullOrEmpty(base.Attribute.TableDefinition); }

		/// <summary>Invoke method name</summary>
		public String MemberName { get => this.Method.Name; }

		/// <summary>Function name</summary>
		public String FunctionName { get => String.IsNullOrEmpty(base.Attribute.Name) ? this.Method.Name : this.Attribute.Name; }

		public SqlFunctionInfo(String namespaceName, SqlFunctionAttribute attribute, MethodInfo method)
			: base(namespaceName, attribute)
			=> this.Method = method;

		public String GetFunctionParameters()
		{
			StringBuilder result = new StringBuilder();
			foreach(ParameterInfo parameter in this.Method.GetParameters())
			{
				if(result.Length > 0)
					result.Append(", ");
				result.AppendFormat("@{0} {1}",
					parameter.Name,
					SqlUtils.ConvertTypeToSql(parameter.ParameterType));
			}
			return result.ToString();
		}
		public String GetReturnParameters()
		{
			if(this.IsSvf)//Scalar-value Function
				return SqlUtils.ConvertTypeToSql(this.Method.ReturnType);
			else
			{
				String[] outParams = this.Attribute.TableDefinition.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				StringBuilder result = new StringBuilder();
				foreach(String outParam in outParams)
				{
					if(result.Length > 0)
						result.Append(", ");
					result.AppendFormat("{0} NULL", outParam);
				}
				return String.Format("TABLE({0})", result.ToString());
			}
		}
	}
}
