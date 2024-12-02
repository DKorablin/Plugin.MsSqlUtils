using System;
using System.Reflection;
using System.Text;
using Microsoft.SqlServer.Server;

namespace Plugin.MsSqlUtils.MsSqlScripting.Bll
{
	internal class SqlAggregateInfo : SqlObjectInfo<SqlUserDefinedAggregateAttribute>
	{
		public Type ObjectType { get; }

		public String AggregateName { get => String.IsNullOrEmpty(base.Attribute.Name) ? base.NamespaceName : base.Attribute.Name; }

		public SqlAggregateInfo(Type objectType, SqlUserDefinedAggregateAttribute attribute)
			: base(objectType.Name, attribute)
			=> this.ObjectType = objectType;

		public String GetAggregateParameters()
		{
			MethodInfo method = this.ObjectType.GetMethod("Accumulate")
				?? throw new MissingMethodException(String.Format("The Accumulate method is missing from user defined aggregate {0}", this.AggregateName));

			StringBuilder result = new StringBuilder();
			foreach(ParameterInfo parameter in method.GetParameters())
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
			MethodInfo method = this.ObjectType.GetMethod("Terminate")
				?? throw new MissingMethodException(String.Format("The Terminate method is missing from user defined aggregate {0}", this.AggregateName));

			return SqlUtils.ConvertTypeToSql(method.ReturnType);
		}
	}
}