using System;
using Microsoft.SqlServer.Server;

namespace Plugin.MsSqlUtils.MsSqlScripting.Bll
{
	internal static class SqlUtils
	{
		public static String ConvertTypeToSql(Type parameterType)
		{
			switch(parameterType.Name)
			{
				case "SqlBinary":
				case "Byte[]":			return "[VarBinary](MAX)";
				case "SqlBoolean":		return "[Bit]";
				case "SqlByte":			return "[TinyInt]";
				case "SqlBytes":		return "[VarBinary](MAX)";
				case "SqlChars":		return "[NVarChar](MAX)";
				case "SqlDateTime":		return "[DateTime]";
				case "SqlDecimal":		return "[Decimal](18,0)";
				case "SqlDouble":		return "[Float]";
				case "SqlGuid":			return "[UniqueIdentifier]";
				case "SqlInt16":		return "[SmallInt]";
				case "SqlInt32":		return "[Int]";
				case "SqlInt64":		return "[BigInt]";
				case "SqlMoney":		return "[Money]";
				case "SqlSingle":		return "[Float]";
				case "SqlString":
				case "String":			return "[NVarChar](MAX)";
				default:
					Object[] attributes = parameterType.GetCustomAttributes(typeof(SqlUserDefinedTypeAttribute), false);
					if(attributes.Length == 1)
					{
						SqlUserDefinedTypeAttribute attribute = (SqlUserDefinedTypeAttribute)attributes[0];
						return "[dbo]." + (String.IsNullOrEmpty(attribute.Name) ? parameterType.Name : attribute.Name);
					}else
						throw new NotSupportedException();
			}
		}

		private static readonly Char[] Lookup = new Char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

		public static String HexToString(Byte[] data)
		{
			Int32 i = 0, p = 2, l = data.Length;
			Char[] result = new Char[l * 2 + 2];
			Byte d;
			result[0] = '0';
			result[1] = 'x';
			while(i < l)
			{
				d = data[i++];
				result[p++] = Lookup[d / 0x10];
				result[p++] = Lookup[d % 0x10];
			}
			String resultStr = new String(result, 0, result.Length);
			Byte[] test = StringToHex(resultStr.Remove(0,2));
			return resultStr;
		}

		public static Byte[] StringToHex(String hex)
		{
			if(hex.Length % 2 == 1)
				throw new ArgumentOutOfRangeException("The binary key cannot have an odd number of digits");

			Byte[] arr = new Byte[hex.Length >> 1];

			for(Int32 i = 0; i < hex.Length >> 1; ++i)
				arr[i] = (Byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));

			return arr;
		}

		private static Int32 GetHexVal(Char hex)
		{
			Int32 val = (Int32)hex;
			//For uppercase A-F letters:
			return val - (val < 58 ? 48 : 55);
			//For lowercase a-f letters:
			//return val - (val < 58 ? 48 : 87);
			//Or the two combined, but a bit slower:
			//return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
		}
	}
}
