using System;
using System.Collections.Generic;
using System.Diagnostics;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.MsSqlUtils
{
	public class PluginWindows : IPlugin,IPluginSettings<PluginSettings>
	{
		private TraceSource _trace;
		private PluginSettings _settings;
		private Dictionary<String, DockState> _documentTypes;

		internal TraceSource Trace { get => this._trace ?? (this._trace = PluginWindows.CreateTraceSource<PluginWindows>()); }

		internal IHostWindows HostWindows { get; }

		/// <summary>Настройки для взаимодействия из хоста</summary>
		Object IPluginSettings.Settings => this.Settings;

		/// <summary>Настройки для взаимодействия из плагина</summary>
		public PluginSettings Settings
		{
			get
			{
				if(this._settings == null)
				{
					this._settings = new PluginSettings();
					this.HostWindows.Plugins.Settings(this).LoadAssemblyParameters(this._settings);
				}
				return this._settings;
			}
		}

		internal IMenuItem AssemblyMenu { get; set; }
		private Dictionary<String, DockState> DocumentTypes
		{
			get
			{
				if(this._documentTypes == null)
					this._documentTypes = new Dictionary<String, DockState>()
					{
						{ typeof(PanelAssemblyScripter).ToString(), DockState.DockBottomAutoHide },
					};
				return this._documentTypes;
			}
		}

		public PluginWindows(IHostWindows hostWindows)
			=> this.HostWindows = hostWindows ?? throw new ArgumentNullException(nameof(HostWindows));

		public IWindow GetPluginControl(String typeName, Object args)
			=> this.CreateWindow(typeName, false, args);

		Boolean IPlugin.OnConnection(ConnectMode mode)
		{
			IMenuItem menuTools = this.HostWindows.MainMenu.FindMenuItem("Tools");
			if(menuTools == null)
			{
				this.Trace.TraceEvent(TraceEventType.Error, 10, "Menu item 'Tools' not found");
				return false;
			}

			IMenuItem menuSql = menuTools.FindMenuItem("SQL");
			if(menuSql == null)
			{
				menuSql = menuTools.Create("SQL");
				menuSql.Name = "Tools.SeQueL";
				menuTools.Items.Add(menuSql);
			}

			this.AssemblyMenu = menuSql.Create("Assembly Script");
			this.AssemblyMenu.Name = "Tools.SeQueL.AssemblyScript";
			this.AssemblyMenu.Click += (sender, e) => { this.CreateWindow(typeof(PanelAssemblyScripter).ToString(), false); };

			menuSql.Items.Add(this.AssemblyMenu);
			return true;
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
		{
			if(this.AssemblyMenu != null)
				this.HostWindows.MainMenu.Items.Remove(this.AssemblyMenu);
			return true;
		}

		private IWindow CreateWindow(String typeName, Boolean searchForOpened, Object args = null)
		{
			DockState state;
			return this.DocumentTypes.TryGetValue(typeName, out state)
				? this.HostWindows.Windows.CreateWindow(this, typeName, searchForOpened, state, args)
				: null;
		}

		private static TraceSource CreateTraceSource<T>(String name = null) where T : IPlugin
		{
			TraceSource result = new TraceSource(typeof(T).Assembly.GetName().Name + name);
			result.Switch.Level = SourceLevels.All;
			result.Listeners.Remove("Default");
			result.Listeners.AddRange(System.Diagnostics.Trace.Listeners);
			return result;
		}
	}
}