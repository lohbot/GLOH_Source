using System;
using System.Collections.Generic;
using UnityEngine;

public class FTPTreeGUI : MonoBehaviour
{
	public class TreeNode
	{
		private bool mbOpen;

		private List<FTPTreeGUI.TreeNode> mlstChildren = new List<FTPTreeGUI.TreeNode>();

		private string msText;

		public string UsePATH;

		private string FULLPATH;

		private string Extension;

		public List<FTPTreeGUI.TreeNode> Children
		{
			get
			{
				return this.mlstChildren;
			}
		}

		public string Text
		{
			get
			{
				return this.msText;
			}
			set
			{
				this.msText = value;
			}
		}

		public bool IsOpen
		{
			get
			{
				return this.mbOpen;
			}
			set
			{
				this.mbOpen = value;
			}
		}

		public string URL
		{
			get
			{
				return this.FULLPATH;
			}
			set
			{
				this.FULLPATH = value;
			}
		}

		public string EXT
		{
			get
			{
				return this.Extension;
			}
			set
			{
				this.Extension = value;
			}
		}

		public TreeNode(string sText)
		{
			this.msText = sText;
		}

		public void AddChild(FTPTreeGUI.TreeNode node)
		{
			this.mlstChildren.Add(node);
		}

		public void SetOpenRecursively(bool val)
		{
			this.IsOpen = val;
			foreach (FTPTreeGUI.TreeNode current in this.mlstChildren)
			{
				current.SetOpenRecursively(val);
			}
		}

		public bool IsLoadType()
		{
			return this.Extension != null && this.Extension.Length != 0 && (this.Extension.Equals(".assetbundle") || this.Extension.Equals(".unity"));
		}

		public bool IsPath()
		{
			return this.Extension == null || this.Extension.Length == 0;
		}

		public void LoadURL()
		{
			AssetViewer instance = AssetViewer.GetInstance();
			instance.LoadObject(this.UsePATH);
		}

		public void UnLoadURL()
		{
			Debug.Log("Un" + this.URL);
		}
	}

	public GUIStyle scrollViewStyle;

	public GUIStyle treeHandleStyle;

	public GUIStyle textStyle;

	public GUIStyle resizeStyle;

	public GUIStyle btnStyle;

	public GUILayoutOption[] options = new GUILayoutOption[]
	{
		GUILayout.ExpandWidth(false)
	};

	public static bool isVisible = true;

	public Rect position;

	public GUISkin theSkin;

	private Vector2 mScrollPosition = Vector2.zero;

	private bool mbDragging;

	private bool mbResizing;

	private Vector2 mLastMousePosition;

	private FTPTreeGUI.TreeNode mTree;

	public string BaseURL = "ftp://119.205.210.14";

	public string SEARCH_PATH = "/SAM/DATA/BUNDLE";

	public GUIContent resizeContent = new GUIContent("/", "resize");

	private void Start()
	{
	}

	private void GetFTPSubPath(FTPTreeGUI.TreeNode _Node)
	{
	}

	private void Awake()
	{
		this.mTree = new FTPTreeGUI.TreeNode("FTP VIEWER");
		this.mTree.URL = this.BaseURL + this.SEARCH_PATH;
		this.GetFTPSubPath(this.mTree);
	}

	private FTPTreeGUI.TreeNode GenerateTree(int depth, int breadth, string sText)
	{
		FTPTreeGUI.TreeNode treeNode = new FTPTreeGUI.TreeNode(sText);
		if (depth > 0)
		{
			for (int i = 0; i < breadth; i++)
			{
				treeNode.AddChild(this.GenerateTree(depth - 1, breadth, string.Concat(new object[]
				{
					"Node ",
					depth,
					":",
					i
				})));
			}
		}
		return treeNode;
	}

	private void CheckExpandAll(Event evt, FTPTreeGUI.TreeNode node)
	{
		if (evt.button == 0 && evt.clickCount == 2)
		{
			node.SetOpenRecursively(!node.IsOpen);
		}
	}

	private void GUIRenderTreeNode(FTPTreeGUI.TreeNode node)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button((!node.IsOpen) ? " + " : " - ", this.treeHandleStyle, new GUILayoutOption[0]))
		{
			node.IsOpen = !node.IsOpen;
			if (node.Children.Count == 0 && node.IsPath())
			{
				this.GetFTPSubPath(node);
			}
		}
		if (node.IsOpen)
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			if (GUILayout.Button(node.Text, this.textStyle, new GUILayoutOption[0]))
			{
				this.CheckExpandAll(Event.current, node);
			}
			foreach (FTPTreeGUI.TreeNode current in node.Children)
			{
				this.GUIRenderTreeNode(current);
			}
			GUILayout.EndVertical();
		}
		else if (GUILayout.Button(node.Text, this.textStyle, new GUILayoutOption[0]))
		{
			this.CheckExpandAll(Event.current, node);
		}
		if (node.Children.Count == 0 && node.IsLoadType() && GUILayout.Button("Load", this.btnStyle, new GUILayoutOption[0]))
		{
			node.LoadURL();
		}
		GUILayout.EndHorizontal();
	}

	private void ShowTreeWindow(int idWindow)
	{
		if (this.theSkin != null)
		{
			GUI.skin = this.theSkin;
		}
		else
		{
			this.theSkin = GUI.skin;
		}
		GUILayoutOption[] array = new GUILayoutOption[]
		{
			GUILayout.Width(this.position.width - 20f),
			GUILayout.Height(this.position.height - 55f)
		};
		this.mScrollPosition = GUILayout.BeginScrollView(this.mScrollPosition, false, false, this.theSkin.horizontalScrollbar, this.theSkin.verticalScrollbar, this.scrollViewStyle, array);
		this.GUIRenderTreeNode(this.mTree);
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.position.width - 40f);
		GUILayout.Label(this.resizeContent, this.resizeStyle, new GUILayoutOption[0]);
		if (Event.current.type == EventType.MouseDown && !this.mbDragging)
		{
			this.mbDragging = true;
			this.mLastMousePosition = Event.current.mousePosition;
			this.mbResizing = (Vector2.Distance(new Vector2(this.position.width, this.position.height), Event.current.mousePosition) < 20f);
		}
		else if (Event.current.type == EventType.MouseUp)
		{
			this.mbDragging = false;
			this.mbResizing = false;
		}
		else if (this.mbDragging && Event.current.type == EventType.MouseDrag)
		{
			Vector2 vector = Event.current.mousePosition - this.mLastMousePosition;
			if (this.mbResizing)
			{
				this.position.width = this.position.width + vector.x;
				this.position.height = this.position.height + vector.y;
				this.mLastMousePosition = Event.current.mousePosition;
			}
			else
			{
				this.position.x = this.position.x + vector.x;
				this.position.y = this.position.y + vector.y;
			}
		}
		GUILayout.EndHorizontal();
		GUI.DragWindow();
	}
}
