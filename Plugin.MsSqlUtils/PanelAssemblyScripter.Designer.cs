namespace Plugin.MsSqlUtils
{
	partial class PanelAssemblyScripter
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ToolStrip tsMain;
			System.Windows.Forms.ToolStripButton tsbnOpen;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelAssemblyScripter));
			this.cmsAssemblyAction = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiAssemblyAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAssemblyBrowse = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAssemblyRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAssemblyScript = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAssemblyScriptInstall = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAssemblyScriptUninstall = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAssemblyScriptBinary = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.ilTree = new System.Windows.Forms.ImageList(this.components);
			this.tvAssemblies = new Plugin.MsSqlUtils.UI.RootImageTreeView();
			this.tsmiAssemblyClear = new System.Windows.Forms.ToolStripMenuItem();
			tsMain = new System.Windows.Forms.ToolStrip();
			tsbnOpen = new System.Windows.Forms.ToolStripButton();
			tsMain.SuspendLayout();
			this.cmsAssemblyAction.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsMain
			// 
			tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsbnOpen});
			tsMain.Location = new System.Drawing.Point(0, 0);
			tsMain.Name = "tsMain";
			tsMain.Size = new System.Drawing.Size(237, 25);
			tsMain.TabIndex = 0;
			tsMain.Text = "toolStrip1";
			// 
			// tsbnOpen
			// 
			tsbnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tsbnOpen.Image = global::Plugin.MsSqlUtils.Properties.Resources.iconOpen;
			tsbnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbnOpen.Name = "tsbnOpen";
			tsbnOpen.Size = new System.Drawing.Size(23, 22);
			tsbnOpen.Text = "Open...";
			tsbnOpen.Click += new System.EventHandler(this.tsbnBrowse_Click);
			// 
			// cmsAssemblyAction
			// 
			this.cmsAssemblyAction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAssemblyAdd,
            this.tsmiAssemblyBrowse,
            this.tsmiAssemblyRemove,
            this.tsmiAssemblyClear,
            this.tsmiAssemblyScript,
            this.tsmiCopy});
			this.cmsAssemblyAction.Name = "cmsAssemblyAction";
			this.cmsAssemblyAction.Size = new System.Drawing.Size(153, 158);
			this.cmsAssemblyAction.Opening += new System.ComponentModel.CancelEventHandler(this.cmsAssemblyAction_Opening);
			this.cmsAssemblyAction.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsAssemblyAction_ItemClicked);
			// 
			// tsmiAssemblyAdd
			// 
			this.tsmiAssemblyAdd.Name = "tsmiAssemblyAdd";
			this.tsmiAssemblyAdd.Size = new System.Drawing.Size(152, 22);
			this.tsmiAssemblyAdd.Text = "&Add...";
			// 
			// tsmiAssemblyBrowse
			// 
			this.tsmiAssemblyBrowse.Name = "tsmiAssemblyBrowse";
			this.tsmiAssemblyBrowse.Size = new System.Drawing.Size(152, 22);
			this.tsmiAssemblyBrowse.Text = "&Browse";
			// 
			// tsmiAssemblyRemove
			// 
			this.tsmiAssemblyRemove.Name = "tsmiAssemblyRemove";
			this.tsmiAssemblyRemove.Size = new System.Drawing.Size(152, 22);
			this.tsmiAssemblyRemove.Text = "&Remove";
			// 
			// tsmiAssemblyScript
			// 
			this.tsmiAssemblyScript.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAssemblyScriptInstall,
            this.tsmiAssemblyScriptUninstall,
            this.tsmiAssemblyScriptBinary});
			this.tsmiAssemblyScript.Name = "tsmiAssemblyScript";
			this.tsmiAssemblyScript.Size = new System.Drawing.Size(152, 22);
			this.tsmiAssemblyScript.Text = "&Script";
			this.tsmiAssemblyScript.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsAssemblyAction_ItemClicked);
			// 
			// tsmiAssemblyScriptInstall
			// 
			this.tsmiAssemblyScriptInstall.Name = "tsmiAssemblyScriptInstall";
			this.tsmiAssemblyScriptInstall.Size = new System.Drawing.Size(152, 22);
			this.tsmiAssemblyScriptInstall.Text = "&Install";
			// 
			// tsmiAssemblyScriptUninstall
			// 
			this.tsmiAssemblyScriptUninstall.Name = "tsmiAssemblyScriptUninstall";
			this.tsmiAssemblyScriptUninstall.Size = new System.Drawing.Size(152, 22);
			this.tsmiAssemblyScriptUninstall.Text = "&Uninstall";
			// 
			// tsmiAssemblyScriptBinary
			// 
			this.tsmiAssemblyScriptBinary.Name = "tsmiAssemblyScriptBinary";
			this.tsmiAssemblyScriptBinary.Size = new System.Drawing.Size(152, 22);
			this.tsmiAssemblyScriptBinary.Text = "&Binary";
			// 
			// tsmiCopy
			// 
			this.tsmiCopy.Name = "tsmiCopy";
			this.tsmiCopy.Size = new System.Drawing.Size(152, 22);
			this.tsmiCopy.Text = "&Copy";
			// 
			// ilTree
			// 
			this.ilTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTree.ImageStream")));
			this.ilTree.TransparentColor = System.Drawing.Color.Magenta;
			this.ilTree.Images.SetKeyName(0, "iconDll.bmp");
			// 
			// tvAssemblies
			// 
			this.tvAssemblies.AllowDrop = true;
			this.tvAssemblies.ContextMenuStrip = this.cmsAssemblyAction;
			this.tvAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvAssemblies.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
			this.tvAssemblies.HideSelection = false;
			this.tvAssemblies.ImageIndex = 0;
			this.tvAssemblies.ImageList = this.ilTree;
			this.tvAssemblies.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
			this.tvAssemblies.Location = new System.Drawing.Point(0, 25);
			this.tvAssemblies.Name = "tvAssemblies";
			this.tvAssemblies.SelectedImageIndex = 0;
			this.tvAssemblies.Size = new System.Drawing.Size(237, 380);
			this.tvAssemblies.TabIndex = 1;
			this.tvAssemblies.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvAssemblies_BeforeExpand);
			this.tvAssemblies.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvAssemblies_DragDrop);
			this.tvAssemblies.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvAssemblies_DragEnter);
			this.tvAssemblies.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvAssemblies_KeyDown);
			this.tvAssemblies.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvAssemblies_MouseClick);
			// 
			// tsmiAssemblyClear
			// 
			this.tsmiAssemblyClear.Name = "tsmiAssemblyClear";
			this.tsmiAssemblyClear.Size = new System.Drawing.Size(152, 22);
			this.tsmiAssemblyClear.Text = "C&lear";
			// 
			// PanelAssemblyScripter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tvAssemblies);
			this.Controls.Add(tsMain);
			this.Name = "PanelAssemblyScripter";
			this.Size = new System.Drawing.Size(237, 405);
			tsMain.ResumeLayout(false);
			tsMain.PerformLayout();
			this.cmsAssemblyAction.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripMenuItem tsmiAssemblyAdd;
		private System.Windows.Forms.ToolStripMenuItem tsmiAssemblyRemove;
		private System.Windows.Forms.ToolStripMenuItem tsmiAssemblyScript;
		private System.Windows.Forms.ToolStripMenuItem tsmiAssemblyScriptInstall;
		private System.Windows.Forms.ToolStripMenuItem tsmiAssemblyScriptUninstall;
		private Plugin.MsSqlUtils.UI.RootImageTreeView tvAssemblies;
		private System.Windows.Forms.ToolStripMenuItem tsmiAssemblyScriptBinary;
		private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
		private System.Windows.Forms.ImageList ilTree;
		private System.Windows.Forms.ContextMenuStrip cmsAssemblyAction;
		private System.Windows.Forms.ToolStripMenuItem tsmiAssemblyBrowse;
		private System.Windows.Forms.ToolStripMenuItem tsmiAssemblyClear;
	}
}
