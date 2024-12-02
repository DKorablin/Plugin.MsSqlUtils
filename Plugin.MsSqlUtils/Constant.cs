using System;

namespace Plugin.MsSqlUtils
{
	/// <summary>Константы плагина</summary>
	internal static class Constant
	{
		/// <summary>Шаблоны генерации MSSQL скриптов</summary>
		public static class Templates
		{
			/// <summary>Написать сообщение</summary>
			public const String SqlMessage = @"SELECT GetDate(),'{Message}';
GO
";
			/// <summary>Использовать БД</summary>
			public const String SqlUsingDatabase = @"USE [<Database Name,,>]
";
			/// <summary>MSSQL Install constants</summary>
			public static class SqlInstall
			{
				/// <summary>Установка сборки</summary>
				public const String Assembly = @"CREATE ASSEMBLY [{AssemblyName}]
AUTHORIZATION [dbo]
FROM {GetHexAssembly()}
WITH PERMISSION_SET = SAFE;
GO
";
				/// <summary>Установка функции</summary>
				public const String Function = @"CREATE FUNCTION [dbo].[{Name}]({Parameters})
RETURNS {ReturnType} WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [{AssemblyName}].[{NamespaceName}].[{MemberName}];
GO
";
				/// <summary>Установка пользовательского типа данных</summary>
				public const String Type = @"CREATE TYPE [dbo].[{Name}]
EXTERNAL NAME [{AssemblyName}].[{MemberName}];
GO
";
				/// <summary>Установка агрегата</summary>
				public const String Aggregate = @"CREATE AGGREGATE [dbo].[{Name}]
({Parameters})
RETURNS {ReturnType}
EXTERNAL NAME [{AssemblyName}].[{MemberName}]
GO
";
			}

			/// <summary>MSSQL Uninstall constants</summary>
			public static class SqlUninstall
			{
				/// <summary>Удаление сборки</summary>
				public const String Assembly = @"IF EXISTS (SELECT * FROM sys.assemblies asm WHERE asm.name = N'{AssemblyName}')
	DROP ASSEMBLY [{AssemblyName}];
ELSE
	SELECT GetDate(),'Assembly ''{AssemblyName}'' not found';
GO
";
				/// <summary>Удаление функции</summary>
				public const String Function = @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{Name}]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[{Name}];
ELSE
	SELECT GetDate(),'Function ''{Name}'' not found';
GO
";
				public const String Type = @"IF EXISTS (SELECT * FROM sys.assembly_types at INNER JOIN sys.schemas ss on at.schema_id = ss.schema_id WHERE at.name = N'{Name}' AND ss.name=N'dbo')
	DROP TYPE [dbo].[{Name}]
ELSE
	SELECT GetDate(),'User defined type ''{Name}'' not found';
GO
";
				/// <summary>Удаление агрегата</summary>
				public const String Aggregate = @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{Name}]') AND type = N'AF')
	DROP AGGREGATE [dbo].[{Name}]
ELSE
	SELECT GetDate(),'Aggregate ''{Name}'' not found';
GO
";
			}
		}
	}
}