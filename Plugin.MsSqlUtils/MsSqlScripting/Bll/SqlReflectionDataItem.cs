using System;

namespace Plugin.MsSqlUtils.MsSqlScripting.Bll
{
	[Serializable]
	internal readonly struct SqlReflectionDataItem
	{
		public enum SqlType
		{
			Unknown,
			Aggregate,
			Function,
			Type,
		}

		public readonly SqlType Type;

		public readonly String AssemblyName;

		public readonly String NamespaceName;

		public readonly String MemberName;

		public readonly String Name;

		public readonly String Parameters;

		public readonly String ReturnType;

		public SqlReflectionDataItem(SqlType type, String assemblyName, String namespaceName, String memberName, String name, String parameters, String returnType)
		{
			Type = type;
			AssemblyName = assemblyName;
			NamespaceName = namespaceName;
			Name = name;
			MemberName = memberName;
			Parameters = parameters;
			ReturnType = returnType;
		}
	}
}