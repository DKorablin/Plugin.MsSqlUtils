using System;
using Microsoft.SqlServer.Server;
using System.Text;
using System.Reflection;

namespace Plugin.MsSqlUtils.MsSqlScripting.Bll
{
	internal class SqlTypeInfo : SqlObjectInfo<SqlUserDefinedTypeAttribute>
	{
		public Type ObjectType { get; }

		public String TypeName { get => String.IsNullOrEmpty(base.Attribute.Name) ? base.NamespaceName : base.Attribute.Name; }

		public SqlTypeInfo(Type objectType, SqlUserDefinedTypeAttribute attribute)
			: base(objectType.Name, attribute)
			=> this.ObjectType = objectType;

		public String GetTypeParameters()
		{
			var constructors = this.ObjectType.GetConstructors();
			if(constructors.Length == 1)
			{
				StringBuilder result = new StringBuilder();
				ParameterInfo[] parameters = constructors[0].GetParameters();
				for(Int32 loop = 0;loop < parameters.Length;loop++)
				{
					ParameterInfo parameter = parameters[loop];
					result.AppendFormat("{0} {1}", parameter.ParameterType, parameter.Name);
					if(parameter.RawDefaultValue != null && parameter.RawDefaultValue != DBNull.Value)
						result.AppendFormat(" = {0}", parameter.RawDefaultValue);

					if(loop + 1 != parameters.Length)
						result.Append(", ");
				}
				return result.ToString();
			} else return String.Empty;
		}
	}
}