using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace Plugin.MsSqlUtils
{
	public class PluginSettings
	{
		private String _sqlMessage;
		private String _sqlUsingDatabase;
		private String _installAssembly;
		private String _installFunction;
		private String _installType;
		private String _installAggregate;
		private String _uninstallAssembly;
		private String _uninstallFunction;
		private String _uninstallType;
		private String _uninstallAggregate;

		[Category("Template")]
		[DefaultValue(Constant.Templates.SqlMessage)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[DisplayName("Show Message")]
		[Description("Шаблон отображения сообщения при написании скрипта. Доступный шаблон: {Message}")]
		public String SqlMessage
		{
			get => this._sqlMessage ?? Constant.Templates.SqlMessage;
			set => this._sqlMessage = TestTemplate(value, Constant.Templates.SqlMessage);
		}

		[Category("Template")]
		[DefaultValue(Constant.Templates.SqlUsingDatabase)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[DisplayName("Using DB")]
		[Description("Использовать БД.")]
		public String SqlUsingDatabase
		{
			get => this._sqlUsingDatabase ?? Constant.Templates.SqlUsingDatabase;
			set => this._sqlUsingDatabase = TestTemplate(value, Constant.Templates.SqlUsingDatabase);
		}

		[Category("Template - Install")]
		[DefaultValue(Constant.Templates.SqlInstall.Assembly)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[DisplayName("Assembly")]
		[Description("Шаблон установки сборки. Доступные ключи: {GetHexAssembly()}")]
		public String InstallAssembly
		{
			get => this._installAssembly ?? Constant.Templates.SqlInstall.Assembly;
			set => this._installAssembly = TestTemplate(value, Constant.Templates.SqlInstall.Assembly);
		}

		[Category("Template - Install")]
		[DefaultValue(Constant.Templates.SqlInstall.Function)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[DisplayName("Function")]
		[Description("Шаблон установки функции.")]
		public String InstallFunction
		{
			get => this._installFunction ?? Constant.Templates.SqlInstall.Function;
			set => this._installFunction = TestTemplate(value, Constant.Templates.SqlInstall.Function);
		}

		[Category("Template - Install")]
		[DefaultValue(Constant.Templates.SqlInstall.Type)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[DisplayName("Type")]
		[Description("Шаблон установки пользовательского типа данных.")]
		public String InstallType
		{
			get => this._installType ?? Constant.Templates.SqlInstall.Type;
			set => this._installType = TestTemplate(value, Constant.Templates.SqlInstall.Type);
		}

		[Category("Template - Install")]
		[DefaultValue(Constant.Templates.SqlInstall.Aggregate)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[DisplayName("Aggregate")]
		[Description("Шаблон установки агрегата.")]
		public String InstallAggregate
		{
			get => this._installAggregate ?? Constant.Templates.SqlInstall.Aggregate;
			set => this._installAggregate = TestTemplate(value, Constant.Templates.SqlInstall.Aggregate);
		}

		[Category("Template - Uninstall")]
		[DefaultValue(Constant.Templates.SqlUninstall.Assembly)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[DisplayName("Assembly")]
		[Description("Шаблон удаления сборки")]
		public String UninstallAssembly
		{
			get => this._uninstallAssembly ?? Constant.Templates.SqlUninstall.Assembly;
			set => this._uninstallAssembly = TestTemplate(value, Constant.Templates.SqlUninstall.Assembly);
		}

		[Category("Template - Uninstall")]
		[DefaultValue(Constant.Templates.SqlUninstall.Function)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[DisplayName("Function")]
		[Description("Шаблон удаления функции")]
		public String UninstallFunction
		{
			get => this._uninstallFunction ?? Constant.Templates.SqlUninstall.Function;
			set => this._uninstallFunction = TestTemplate(value, Constant.Templates.SqlUninstall.Function);
		}

		[Category("Template - Uninstall")]
		[DefaultValue(Constant.Templates.SqlUninstall.Type)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[DisplayName("Type")]
		[Description("Шаблон удаления пользовательского типа данных")]
		public String UninstallType
		{
			get => this._uninstallType ?? Constant.Templates.SqlUninstall.Type;
			set => this._uninstallType = TestTemplate(value, Constant.Templates.SqlUninstall.Type);
		}

		[Category("Template - Uninstall")]
		[DefaultValue(Constant.Templates.SqlUninstall.Aggregate)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[DisplayName("Aggregate")]
		[Description("Шаблон удаления агрегата")]
		public String UninstallAggregate
		{
			get => this._uninstallAggregate ?? Constant.Templates.SqlUninstall.Aggregate;
			set => this._uninstallAggregate = TestTemplate(value, Constant.Templates.SqlUninstall.Aggregate);
		}

		private static String TestTemplate(String value, String defaultValue)
			=> (value ?? String.Empty).Trim().Length == 0 || value.Equals(defaultValue)
				? null
				: value;
	}
}