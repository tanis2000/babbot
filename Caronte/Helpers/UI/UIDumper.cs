using System;
using System.Globalization;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Text;
using Glider.Common.Objects;
using System.Windows.Forms;
using System.Drawing;

using Pather.Parser;
using Pather.Tasks;
using Pather.Activities;

namespace Pather.Helpers.UI
{
    public class UIDumper
    {
        private BindingFlags p_DumpFlags;

        public enum DumpType
        {
            Unknown, UIChildren, UIGlobals
        }

        public UIDumper()
        {
            p_DumpFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        }

        public struct DumpUIGlobalsArgs
        {
            public string FilterText;
            public bool OnlyVisible;

            public DumpUIGlobalsArgs(string filterText, bool onlyVisible)
            {
                FilterText = filterText;
                OnlyVisible = onlyVisible;
            }
        }

        public struct DumpUIChildrenArgs
        {
            public string ObjectName;
            public UIDumperTreeNode RootNode;
            
            public DumpUIChildrenArgs(UIDumperTreeNode rootNode, string objectName)
            {
                ObjectName = objectName;
                RootNode = rootNode;
            }
        }

        public void Dump(DumpType dumpType, object dumpArgs)
        {
            Thread t = null;

            switch (dumpType)
            {
                case DumpType.UIGlobals:
                    t = new Thread(new ParameterizedThreadStart(Dumper_UIGlobals));
                    break;

                case DumpType.UIChildren:
                    t = new Thread(new ParameterizedThreadStart(Dumper_UIChildren));
                    break;

                default:
                    return;
            }
            
            if (t != null)
            {
                t.IsBackground = true;
                t.Start(dumpArgs);
                t = null;
            }
        }

        public static void Dumper_UIChildren(object param)
        {
            DumpUIChildrenArgs args = (DumpUIChildrenArgs)param;

            if (args.ObjectName == null || args.RootNode == null) return;

			// disable the Dump button on the form
			PPather.form.DumpButtonEnabled(false);

            //PPather.WriteLine(String.Format("Reading {0}..", args.ObjectName));

            List<TreeNode> tnc = new List<TreeNode>();
            GInterfaceObject root = null;

            try
            {
                root = GContext.Main.Interface.GetByName(args.ObjectName);
            }
            catch (Exception e)
            {
                PPather.WriteLine(String.Format("GetByName() on '{0}' failed: {1}",
                                                args.ObjectName, e.Message));
                root = null;
            }

            if (root != null)
            {
                try
                {
                    UIDumperTreeNode[] childNodes = Dumper_GetChildren(args, ref root);

                    if (childNodes.Length != 0)
                        tnc.AddRange(childNodes);
                }
                catch (Exception e)
                {
                    PPather.WriteLine(String.Format("GetChildren() on '{0}' failed: {1}",
                                                args.ObjectName, e.Message));
                }
            }
            else
            {
				PPather.WriteLine(String.Format("Unable to dump children, '{0}' was not found", args.ObjectName));
            }

            args.RootNode.SetChildNodes(tnc.ToArray());

            // re-enable the Dump button on the form
            PPather.form.DumpButtonEnabled(true);
        }

        public static void Dumper_UIGlobals(object param)
        {
            DumpUIGlobalsArgs args = (DumpUIGlobalsArgs)param;

            if (args.FilterText == null)
            {
                PPather.WriteLine("Error: Something is wrong with args, unable to dump object list");
                return;
            }

            if (GContext.Main.Me == null || !GContext.Main.Me.IsValid)
            {
                PPather.WriteLine("Error: IsPreWorldVisible is true, unable to dump object list");
                return;
            }

            // disable the Dump button on the form
            PPather.form.DumpButtonEnabled(false);
            //PPather.WriteLine("Dumping GObjects..");

            List<TreeNode> tnc = new List<TreeNode>();
            bool shouldCheck = (args.FilterText.Trim().Length != 0);
            bool dumpUiVisible = args.OnlyVisible;

            string[] onames = GInterfaceHelper.GetAllInterfaceObjectNames();
            
            for (int i = 0, max = onames.Length; i != max; i++)
            {
                if (onames[i] == null || onames[i].Length == 0) continue;
                if (shouldCheck && !onames[i].ToLower().Contains(args.FilterText)) continue;

                //PPather.WriteLine(String.Format("#{1}/{2}: {0}", onames[i], i, max));
				UIDumperTreeNode node = new UIDumperTreeNode(onames[i], true);
				node.OnlyVisible = args.OnlyVisible;
                tnc.Add(node);
            }

            // push those new nodes in there nicely
            PPather.form.SetDevTreeNodes(tnc.ToArray());

            tnc.Clear();
            tnc = null;

            // re-enable the Dump button on the form
            PPather.form.DumpButtonEnabled(true);
            //PPather.WriteLine("Dump complete.");
        }

        private static string GetGIOString(ref GInterfaceObject gio)
        {
            StringBuilder sb = new StringBuilder();
            string s = String.Empty;

            try
            {
                s = gio.ToString();

                if (!s.Contains("\"\""))
                {
                    s = s.Substring(s.IndexOf('"') + 1);
                    s = s.Substring(0, s.IndexOf('"'));
                }

                sb.Append(s);                
            }
            catch (Exception e)
            {
                s = String.Empty;
                sb.Append(String.Format("<Error: {0}>", e.Message));
            }

            if (s.EndsWith("Text") || s.Contains("Button") || s.EndsWith("Label") ||
                (s.Contains("Text") && !s.Contains("Texture")))
            {
                try
                {
                    sb.Append(String.Format(": {0}", gio.LabelText));
                }
                catch (Exception e)
                {
                    sb.Append(String.Format(": <Error: {0}>", e.Message));
                }
            }

            return sb.ToString();
        }

        private static UIDumperTreeNode[] Dumper_GetChildren(DumpUIChildrenArgs args, ref GInterfaceObject gio)
        {
            List<UIDumperTreeNode> nodes = new List<UIDumperTreeNode>();

            if (gio != null && gio.Children != null)
            {
                for (int i = 0, max = gio.Children.Length; i < max; i++)
                {
                    if (gio.Children[i] == null || (args.RootNode.OnlyVisible && !gio.Children[i].IsVisible)) continue;

                    string s = GetGIOString(ref gio.Children[i]);
                    UIDumperTreeNode node = new UIDumperTreeNode(s, ref gio.Children[i]);

					try
					{
						if (!gio.Children[i].IsVisible)
							node.ForeColor = Color.DarkGray;
					}
					catch
					{
						node.ForeColor = Color.DarkRed;
					}

					try
					{
						//PPather.WriteLine(String.Format("{0} - Child #{1}/{2}: {3}", rootName, i, max, s));
						UIDumperTreeNode[] childNodes = Dumper_GetChildren(args, ref gio.Children[i]);

						if (childNodes != null && childNodes.Length != 0)
							node.Nodes.AddRange(childNodes);
					}
					catch (Exception e)
					{
						PPather.WriteLine(String.Format("Error reading child object #{1}: {0}", e.Message, i));
						node.ForeColor = Color.DarkRed;
					}

					node.OnlyVisible = args.RootNode.OnlyVisible;

					nodes.Add(node);

                    node = null;
                    s = null;
                }
            }

            return nodes.ToArray();
        }
    }

    public class UIDumperTreeNode : TreeNode
    {
        private bool p_WasScanned;
        private bool p_IsRoot;
		private bool p_OnlyVisible;
        private GInterfaceObject p_GObjectRef;

        public virtual bool WasScanned
        {
            get
            {
                return p_WasScanned;
            }
			set
			{
				p_WasScanned = value;
			}
        }

        public virtual bool IsRoot
        {
            get
            {
                return p_IsRoot;
            }
        }

		public virtual bool OnlyVisible
		{
			get
			{
				return p_OnlyVisible;
			}

			set
			{
				p_OnlyVisible = value;
			}
		}

        public UIDumperTreeNode(string text)
            : base(text)
        {
            p_GObjectRef = null;
            InitializeUIDumperTreeNode(false);
        }

        public UIDumperTreeNode(string text, bool isRoot)
            : base(text)
        {
            InitializeUIDumperTreeNode(isRoot);
        }

        public UIDumperTreeNode(string text, ref GInterfaceObject objectRef)
            : base(text)
        {
            InitializeUIDumperTreeNode(false, ref objectRef);
        }

        public UIDumperTreeNode(string text, ref GInterfaceObject objectRef, bool isRoot)
            : base(text)
        {
            InitializeUIDumperTreeNode(isRoot, ref objectRef);
        }

        public void InitializeUIDumperTreeNode(bool isRoot)
        {
            p_WasScanned = false;
            p_IsRoot = isRoot;
        }

        public void InitializeUIDumperTreeNode(bool isRoot, ref GInterfaceObject objectRef)
        {
            InitializeUIDumperTreeNode(isRoot);
            p_GObjectRef = objectRef;
        }

        public void Scan()
        {
            //PPather.WriteLine(String.Format("Reading Object: {0}", Text));

            if (IsRoot && !WasScanned)
            {
                UIDumper dumper = new UIDumper();
                dumper.Dump(UIDumper.DumpType.UIChildren, new UIDumper.DumpUIChildrenArgs(this, Text));
                dumper = null;
                WasScanned = true;
            }
        }

        public delegate void SetChildNodesDelegate(TreeNode[] nodes);
        public void SetChildNodes(TreeNode[] nodes)
        {
            if (!this.TreeView.InvokeRequired)
            {
                this.TreeView.SuspendLayout();
                Nodes.Clear();

                if (nodes != null && nodes.Length != 0)
                    Nodes.AddRange(nodes);
				this.ExpandAll();
                this.TreeView.ResumeLayout();
            }
            else
            {
                this.TreeView.BeginInvoke(new SetChildNodesDelegate(SetChildNodes), new object[] { nodes });
            }
        }
    }
}
