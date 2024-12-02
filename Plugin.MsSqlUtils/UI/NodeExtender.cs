using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plugin.MsSqlUtils.UI
{
	internal static class NodeExtender
	{
		internal static Color NullColor = Color.Gray;
		internal static Color ExceptionColor = Color.Red;
		internal static Font _nullFont;

		private static Font NullFont
		{
			get => NodeExtender._nullFont ?? (NodeExtender._nullFont = new Font(Control.DefaultFont, FontStyle.Italic));
		}

		public static void SetNull(this ListViewItem item)
		{
			item.Font = NodeExtender.NullFont;
			item.ForeColor = NodeExtender.NullColor;
		}

		public static Boolean IsNull(this ListViewItem item)
			=> item.ForeColor == NodeExtender.NullColor;

		public static void SetException(this ListViewItem item)
			=> item.ForeColor = NodeExtender.ExceptionColor;

		public static Boolean IsException(this ListViewItem item)
			=> item.ForeColor == NodeExtender.ExceptionColor;

		public static void SetNull(this TreeNode node)
		{
			node.NodeFont = NodeExtender.NullFont;
			node.ForeColor = NodeExtender.NullColor;
		}

		public static Boolean IsNull(this TreeNode node)
			=> node.ForeColor == NodeExtender.NullColor;

		public static void SetException(this TreeNode node, Exception exc)
		{
			node.ForeColor = NodeExtender.ExceptionColor;
			node.Text = exc.Message;
		}
		public static Boolean IsException(this TreeNode node)
			=> node.ForeColor == NodeExtender.ExceptionColor;

		public static Boolean IsClosedRootNode(this TreeNode node)
			=> node.Parent == null && node.Nodes.Count == 1 && (node.Nodes[0].Text.Length == 0 || node.Nodes[0].IsException());

		public static void SetNull(this ToolStripItem item)
		{
			item.Font = NodeExtender.NullFont;
			item.ForeColor = NodeExtender.NullColor;
		}

		public static Boolean IsNull(this ToolStripItem item)
			=> item.ForeColor == NodeExtender.NullColor;

		public static void SetException(this ToolStripItem item, Exception exc)
		{
			item.ForeColor = NodeExtender.ExceptionColor;
			item.Text = exc.Message;
		}

		public static Boolean IsException(this ToolStripItem item)
			=> item.ForeColor == NodeExtender.ExceptionColor;
	}
}