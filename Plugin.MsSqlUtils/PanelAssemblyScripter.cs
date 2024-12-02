using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Plugin.MsSqlUtils.MsSqlScripting;
using Plugin.MsSqlUtils.MsSqlScripting.Bll;
using Plugin.MsSqlUtils.Properties;
using Plugin.MsSqlUtils.UI;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.MsSqlUtils
{
	internal partial class PanelAssemblyScripter : UserControl, IPluginSettings<PanelAssemblyScripterSettings>
	{
		private const String Caption = "Assembly Script";
		private PanelAssemblyScripterSettings _settings;

		private PluginWindows Plugin { get => (PluginWindows)this.Window.Plugin; }
		private IWindow Window { get => (IWindow)base.Parent; }

		Object IPluginSettings.Settings { get => this.Settings; }

		public PanelAssemblyScripterSettings Settings
			=> this._settings ?? (this._settings = new PanelAssemblyScripterSettings());

		private String SelectedFile
		{
			get => tvAssemblies.SelectedNode == null || tvAssemblies.SelectedNode.Parent != null ? null : tvAssemblies.SelectedNode.Tag.ToString();
		}

		public PanelAssemblyScripter()
			=> InitializeComponent();

		protected override void OnCreateControl()
		{
			this.Window.Caption = PanelAssemblyScripter.Caption;
			this.Window.SetTabPicture(Resources.iconAsmSql);

			if(this.Settings.Files != null)
				Array.ForEach(this.Settings.Files, delegate(Object filePath) { this.AddAssembly((String)filePath, false); });

			base.OnCreateControl();
		}

		private void tsbnBrowse_Click(Object sender, EventArgs e)
		{
			using(OpenFileDialog dlg = new OpenFileDialog() { CheckFileExists = true, Multiselect = true, Filter = "Assemblies (*.dll)|*.dll|All files (*.*)|*.*", Title = "Add assembly", })
				if(dlg.ShowDialog() == DialogResult.OK)
					Array.ForEach(dlg.FileNames, delegate(String filePath) { this.AddAssembly(filePath, true); });
		}

		private void cmsAssemblyAction_Opening(Object sender, CancelEventArgs e)
			=> tsmiAssemblyScript.Visible = tsmiAssemblyRemove.Visible = tsmiAssemblyBrowse.Visible = this.SelectedFile != null;

		private void cmsAssemblyAction_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(e.ClickedItem == tsmiAssemblyScript)
				return;

			cmsAssemblyAction.Close(ToolStripDropDownCloseReason.ItemClicked);

			if(e.ClickedItem == tsmiAssemblyAdd)
				this.tsbnBrowse_Click(sender, e);
			else if(e.ClickedItem == tsmiAssemblyRemove)
				this.RemoveAssembly(tvAssemblies.SelectedNode);
			else if(e.ClickedItem == tsmiAssemblyClear)
			{
				if(MessageBox.Show("Are you shure you want to remove selected assemblies from list?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					while(tvAssemblies.Nodes.Count > 0)
						this.RemoveAssembly(tvAssemblies.Nodes[0]);
			} else if(e.ClickedItem == tsmiCopy)
				Clipboard.SetText(tvAssemblies.SelectedNode.Text);
			else if(e.ClickedItem == tsmiAssemblyBrowse)
				Process.Start(Path.GetDirectoryName(tvAssemblies.SelectedNode.Text));
			else if(e.ClickedItem == tsmiAssemblyScriptInstall)
			{
				String fileName = GetSaveFilePath(
					String.Format("{0}_Install.sql", Path.GetFileNameWithoutExtension(this.SelectedFile)),
					Path.GetDirectoryName(this.SelectedFile),
				"Save install script",
				"SQL files (*.sql)|*.sql");

				if(fileName != null)
					using(MsSqlAssembly asm = new MsSqlAssembly(this.SelectedFile))
						File.WriteAllText(fileName, this.CreateInstallScript(asm));
			} else if(e.ClickedItem == tsmiAssemblyScriptUninstall)
			{
				String fileName = GetSaveFilePath(
					String.Format("{0}_Uninstall.sql", Path.GetFileNameWithoutExtension(this.SelectedFile)),
					Path.GetDirectoryName(this.SelectedFile),
				"Save Uninstall script",
				"SQL files (*.sql)|*.sql");

				if(fileName != null)
					using(MsSqlAssembly asm = new MsSqlAssembly(this.SelectedFile))
						File.WriteAllText(fileName, this.CreateUninstallScript(asm));
			} else if(e.ClickedItem == tsmiAssemblyScriptBinary)
			{
				String fileName = GetSaveFilePath(
					String.Format("{0}_Bin.txt", Path.GetFileNameWithoutExtension(this.SelectedFile)),
					Path.GetDirectoryName(this.SelectedFile),
				"Save Uninstall script",
				"Text files (*.txt)|*.txt");

				if(fileName != null)
					using(MsSqlAssembly asm = new MsSqlAssembly(this.SelectedFile))
						File.WriteAllText(fileName, asm.GetHexAssembly());
			} else
				throw new NotImplementedException(e.ClickedItem.ToString());
		}
		private void tvAssemblies_MouseClick(Object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				TreeViewHitTestInfo info = tvAssemblies.HitTest(e.Location);
				if(info.Node != null)
				{
					tvAssemblies.SelectedNode = info.Node;
					cmsAssemblyAction.Show(tvAssemblies, e.Location);
				}
			}
		}

		private void tvAssemblies_KeyDown(Object sender, KeyEventArgs e)
		{
			switch(e.KeyData)
			{
			case Keys.O | Keys.Control://Открытие файла
				this.tsbnBrowse_Click(null, e);
				e.Handled = true;
				break;
			case Keys.Delete://Закрытие файла
			case Keys.Back:
				if(this.SelectedFile != null)
				{
					e.Handled = true;
					this.cmsAssemblyAction_ItemClicked(null, new ToolStripItemClickedEventArgs(tsmiAssemblyRemove));
				}
				break;
			}
		}
		private void tvAssemblies_BeforeExpand(Object sender, TreeViewCancelEventArgs e)
		{
			if(e.Action == TreeViewAction.Expand && e.Node.IsClosedRootNode())
			{
				String filePath = (String)e.Node.Tag;
				TreeNode nAggregate = new TreeNode(Resources.TreeAggregates) { ImageIndex = RootImageTreeView.NOIMAGE, SelectedImageIndex = RootImageTreeView.NOIMAGE, NodeFont = new Font(Control.DefaultFont, FontStyle.Bold) };
				TreeNode nFunction = new TreeNode(Resources.TreeFunctions) { ImageIndex = RootImageTreeView.NOIMAGE, SelectedImageIndex = RootImageTreeView.NOIMAGE, NodeFont = new Font(Control.DefaultFont, FontStyle.Bold) };
				TreeNode nType = new TreeNode(Resources.TreeTypes) { ImageIndex = RootImageTreeView.NOIMAGE, SelectedImageIndex = RootImageTreeView.NOIMAGE, NodeFont = new Font(Control.DefaultFont, FontStyle.Bold) };

				try
				{
					using(MsSqlAssembly asm = new MsSqlAssembly(filePath))
					{
						foreach(SqlReflectionDataItem dataItem in asm)
						{
							switch(dataItem.Type)
							{
							case SqlReflectionDataItem.SqlType.Aggregate:
							case SqlReflectionDataItem.SqlType.Function:
								nAggregate.Nodes.Add(new TreeNode($"{dataItem.ReturnType} {dataItem.NamespaceName}.{dataItem.Name}({dataItem.Parameters})")
									{
										ImageIndex = RootImageTreeView.NOIMAGE,
										SelectedImageIndex = RootImageTreeView.NOIMAGE,
									});
								break;
							case SqlReflectionDataItem.SqlType.Type:
								nType.Nodes.Add(new TreeNode($"{dataItem.Name}({dataItem.Parameters})")
									{
										ImageIndex = RootImageTreeView.NOIMAGE,
										SelectedImageIndex = RootImageTreeView.NOIMAGE,
									});
								break;
							default:
								throw new NotSupportedException();
							}
						}
					}
					e.Node.Nodes.Clear();
				} catch(Exception exc)
				{
					e.Node.Nodes[0].SetException(exc);
				}
				foreach(TreeNode node in new TreeNode[] { nAggregate, nFunction, nType })
					if(node.Nodes.Count > 0)
						e.Node.Nodes.Add(node);
			}
		}

		private void tvAssemblies_DragEnter(Object sender, DragEventArgs e)
			=> e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;

		private void tvAssemblies_DragDrop(Object sender, DragEventArgs e)
		{
			foreach(String file in (String[])e.Data.GetData(DataFormats.FileDrop))
				this.AddAssembly(file, true);
		}

		private void AddAssembly(String filePath, Boolean newAssembly)
		{
			if(newAssembly)
			{
				foreach(TreeNode check in tvAssemblies.Nodes)
					if(check.Text.Equals(filePath))
					{
						tvAssemblies.SelectedNode = check;
						return;
					}

				try//При попытке открыть фиг-знает что.
				{
					var name = AssemblyName.GetAssemblyName(filePath);
				} catch(BadImageFormatException exc)
				{
					this.Plugin.Trace.TraceData(TraceEventType.Error, 10, exc);
					return;
				}
			}

			TreeNode node = new TreeNode(filePath) { Tag = filePath, ImageIndex = 0, SelectedImageIndex = 0, };
			node.Nodes.Add(new TreeNode());
			tvAssemblies.Nodes.Add(node);
			tvAssemblies.SelectedNode = node;

			if(newAssembly)
			{
				String[] files = this.Settings.Files;
				Array.Resize<String>(ref files, files.Length + 1);
				files[files.Length - 1] = filePath;
				this.Settings.Files = files;
			}
			this.Window.Caption = $"{PanelAssemblyScripter.Caption} ({this.Settings.Files.Length})";
		}

		private static String GetSaveFilePath(String fileName, String directory, String title, String filter)
		{
			String defaultExtension = filter.Substring(filter.LastIndexOf(".") + 1);
			using(SaveFileDialog dlg = new SaveFileDialog() { OverwritePrompt = true, AddExtension = true, FileName = fileName, InitialDirectory = directory, DefaultExt = defaultExtension, Filter = filter + "|All files (*.*)|*.*", Title = title, })
				if(dlg.ShowDialog() == DialogResult.OK)
					return dlg.FileName;
			return null;
		}

		private void RemoveAssembly(TreeNode node)
		{
			List<String> files = new List<String>(this.Settings.Files);
			files.Remove(node.Text);
			this.Settings.Files = files.ToArray();
			if(this.Settings.Files.Length > 0)
				this.Window.Caption = $"{PanelAssemblyScripter.Caption} ({this.Settings.Files.Length})";
			else
				this.Window.Caption = PanelAssemblyScripter.Caption;
			node.Remove();
		}

		/// <summary>Создать скрипт удаления сборки из MSSQL'я</summary>
		/// <param name="asm">Сборка</param>
		/// <returns>Скрипт</returns>
		private String CreateUninstallScript(MsSqlAssembly asm)
		{
			StringBuilder result = new StringBuilder();
			result.Append(this.Plugin.Settings.SqlUsingDatabase);

			String assemblyName = String.Empty;
			foreach(SqlReflectionDataItem dataItem in asm)
			{
				assemblyName = dataItem.AssemblyName;
				switch(dataItem.Type)
				{
				case SqlReflectionDataItem.SqlType.Aggregate://Удаление агрегатов
					result.AppendMessage(this.Plugin.Settings.SqlMessage, String.Format("Uninstalling aggregate {0}...", dataItem.Name));
					result.AppendSqlTemplate(this.Plugin.Settings.UninstallAggregate, dataItem);
					break;
				case SqlReflectionDataItem.SqlType.Function://Удаление функций
					result.AppendMessage(this.Plugin.Settings.SqlMessage, String.Format("Uninstalling function {0}...", dataItem.Name));
					result.AppendSqlTemplate(this.Plugin.Settings.UninstallFunction, dataItem);
					break;
				case SqlReflectionDataItem.SqlType.Type://Удаление пользовательских типов данных
					result.AppendMessage(this.Plugin.Settings.SqlMessage, String.Format("Uninstalling user defined type {0}...", dataItem.Name));
					result.AppendSqlTemplate(this.Plugin.Settings.UninstallType, dataItem);
					break;
				default:
					throw new NotImplementedException(dataItem.Type.ToString());
				}
			}

			//Удаление сборки
			result.AppendMessage(this.Plugin.Settings.SqlMessage, String.Format("Uninstalling CLR library {0}...", assemblyName));
			result.AppendSqlTemplate(this.Plugin.Settings.UninstallAssembly, null, new KeyValuePair<String, String>("AssemblyName", assemblyName));

			return result.ToString();
		}

		/// <summary>Создать скрипт установки сборки в MSSQL</summary>
		/// <param name="asm">Сборка</param>
		/// <returns>Скрипт</returns>
		private String CreateInstallScript(MsSqlAssembly asm)
		{
			StringBuilder result = new StringBuilder();
			result.AppendMessage(this.Plugin.Settings.SqlMessage, "Enabling CLR...");
			result.Append(@"--Начало установки возможности запуска CLR кода
sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
sp_configure 'clr enabled', 1;
GO
RECONFIGURE;
GO
--Конец установки возможности запуска CLR кода
");
			Boolean installAssemblyAdded = false;
			foreach(SqlReflectionDataItem dataItem in asm)
			{
				if(!installAssemblyAdded)
				{
					result.AppendMessage(this.Plugin.Settings.SqlMessage, String.Format("Installing CLR library {0}...", dataItem.AssemblyName));
					result.Append(this.Plugin.Settings.SqlUsingDatabase);
					result.AppendSqlTemplate(this.Plugin.Settings.InstallAssembly, dataItem, new KeyValuePair<String, String>("GetHexAssembly()", asm.GetHexAssembly()));
					installAssemblyAdded = true;
				}

				switch(dataItem.Type)
				{
				case SqlReflectionDataItem.SqlType.Aggregate://Установка агрегатов
					result.AppendMessage(this.Plugin.Settings.SqlMessage, String.Format("Installing aggregate {0}...", dataItem.Name));
					result.AppendSqlTemplate(this.Plugin.Settings.InstallAggregate, dataItem);
					break;
				case SqlReflectionDataItem.SqlType.Function://Установка функций
					result.AppendMessage(this.Plugin.Settings.SqlMessage, String.Format("Installing function {0}...", dataItem.Name));
					result.AppendSqlTemplate(this.Plugin.Settings.InstallFunction, dataItem);
					break;
				case SqlReflectionDataItem.SqlType.Type://Установка пользовательских типов данных
					result.AppendMessage(this.Plugin.Settings.SqlMessage, String.Format("Installing user defined type {0}...", dataItem.Name));
					result.AppendSqlTemplate(this.Plugin.Settings.InstallType, dataItem);
					break;
				default:
					throw new NotSupportedException(dataItem.Type.ToString());
				}
			}

			return result.ToString();
		}
	}
}