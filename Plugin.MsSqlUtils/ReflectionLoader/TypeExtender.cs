using System;
using System.Reflection;

namespace Plugin.MsSqlUtils.ReflectionLoader
{
	internal static class TypeExtender
	{
		public static T GetCustomAttributes<T>(this MemberInfo info) where T : Attribute
		{
			Object[] attributes = info.GetCustomAttributes(typeof(T), false);
			if(attributes != null && attributes.Length == 1)
				return (T)attributes[0];
			return null;
		}
	}
}