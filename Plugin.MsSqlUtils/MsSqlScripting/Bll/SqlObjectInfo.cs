using System;

namespace Plugin.MsSqlUtils.MsSqlScripting.Bll
{
	internal class SqlObjectInfo<T> where T : Attribute
	{
		public String NamespaceName { get; }
		public T Attribute { get; }
		public SqlObjectInfo(String namespaceName, T attribute)
		{
			this.NamespaceName = namespaceName;
			this.Attribute = attribute;
		}
	}
}