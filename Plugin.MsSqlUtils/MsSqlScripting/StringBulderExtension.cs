using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Plugin.MsSqlUtils.MsSqlScripting
{
	internal static class StringBulderExtension
	{
		public static StringBuilder AppendMessage(this StringBuilder item, String template, String message)
			=> item.AppendSqlTemplate(template, new KeyValuePair<String, String>("Message", message));

		public static StringBuilder AppendSqlTemplate(this StringBuilder item, String template, params KeyValuePair<String, String>[] prms)
			=> item.AppendSqlTemplate(template, null, prms);

		public static StringBuilder AppendSqlTemplate(this StringBuilder item, String template, Object reflectedItem, params KeyValuePair<String, String>[] prms)
			=> item.Append(StringBulderExtension.FormatTemplate(template, reflectedItem, prms));

		private static String FormatTemplate(String template, Object item, params KeyValuePair<String, String>[] prms)
		{
			String result = template;
			if(item != null)
			{
				Type type = item.GetType();
				foreach(MemberInfo member in type.GetMembers())
				{
					String key = "{" + member.Name + "}";
					if(result.Contains(key))
						switch(member.MemberType)
						{
						case MemberTypes.Field:
							{
								FieldInfo field = (FieldInfo)member;
								Object value = field.GetValue(item);
								result = result.Replace(key, value == null ? String.Empty : value.ToString());
							}
							break;
						case MemberTypes.Property:
							{
								PropertyInfo prop = (PropertyInfo)member;
								Object value = null;
								if(prop.CanRead)
									value = prop.GetValue(item, null);
								result = result.Replace(key, value == null ? String.Empty : value.ToString());
							}
							break;
						}
				}
			}

			foreach(KeyValuePair<String, String> param in prms)
			{
				String key = "{" + param.Key + "}";
				if(result.Contains(key))
					result = result.Replace(key, param.Value);
			}
			return result;
		}
	}
}