using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plugin.MsSqlUtils.UI
{
	internal class RootImageTreeView : TreeView
	{
		public const Int32 NOIMAGE = 99;

		public RootImageTreeView()
			: base()
		{
			// .NET Bug: Unless LineColor is set, Win32 treeview returns -1 (default), .NET returns Color.Black!
			//base.LineColor = SystemColors.GrayText;
			base.DrawMode = TreeViewDrawMode.OwnerDrawText;
		}

		protected override void OnDrawNode(DrawTreeNodeEventArgs e)
		{
			Boolean customDraw = base.ImageList != null
				&& e.Node.ImageIndex == NOIMAGE
				&& e.Bounds.X > 0;

			e.DrawDefault = !customDraw;
			base.OnDrawNode(e);

			if(customDraw)
			{
				Brush brush;
				Boolean drawSelectRect = false;
				Boolean drawFocusRect = false;
				Color textColor = e.Node.ForeColor;
				switch(e.State)
				{
				case TreeNodeStates.Selected:
					drawFocusRect = true;
					brush = SystemBrushes.ButtonFace;
					break;
				case TreeNodeStates.Focused|TreeNodeStates.Selected:
					drawFocusRect = true;
					brush = SystemBrushes.Highlight;
					textColor = SystemColors.HighlightText;
					drawSelectRect = true;
					break;
				case TreeNodeStates.Hot:
					brush = SystemBrushes.Window;
					textColor = SystemColors.HotTrack;
					break;
				case TreeNodeStates.Focused:
					brush = SystemBrushes.Highlight;
					textColor = SystemColors.HighlightText;
					drawSelectRect = true;
					drawFocusRect = true;
					break;
				default:
					brush = SystemBrushes.Window;
					break;
				}

				Int32 x = e.Bounds.X - base.ImageList.ImageSize.Width;
				if(drawFocusRect)
				{
					//Clear old back color
					e.Graphics.FillRectangle(SystemBrushes.FromSystemColor(e.Node.TreeView.BackColor),
						x,
						e.Bounds.Y,
						e.Bounds.Width + base.ImageList.ImageSize.Width + 10,
						e.Bounds.Height);

					//Fill gray focus rectangle
					e.Graphics.FillRectangle(brush, x, e.Bounds.Y, e.Bounds.Width + 5, e.Bounds.Height);
					if(drawSelectRect)//Draw black focus rectangle
						e.Graphics.DrawRectangle(SystemPens.WindowFrame, x, e.Bounds.Y, e.Bounds.Width + 5, e.Bounds.Height - 1);
				}

				TextRenderer.DrawText(e.Graphics,
					e.Node.Text,
					e.Node.NodeFont ?? e.Node.TreeView.Font,
					new Point(x, e.Bounds.Y),
					textColor,
					TextFormatFlags.GlyphOverhangPadding);
			}
		}

		protected override void OnAfterCollapse(TreeViewEventArgs e)
		{
			base.OnAfterCollapse(e);
			if(!base.CheckBoxes && base.ImageList != null && e.Node.ImageIndex == NOIMAGE)
			{
				// DrawNode event not raised: redraw node with collapsed treeline
				base.Invalidate(e.Node.Bounds);
			}
		}
	}
}