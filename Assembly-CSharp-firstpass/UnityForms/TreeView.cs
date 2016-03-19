using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityForms
{
	public class TreeView : UIScrollList
	{
		public class TreeNode
		{
			private bool _open;

			private bool _click;

			private List<TreeView.TreeNode> _children = new List<TreeView.TreeNode>();

			private string[] _text = new string[]
			{
				string.Empty,
				string.Empty,
				string.Empty
			};

			private long _unique;

			private int _depth;

			private string _imageKey = string.Empty;

			public object _object;

			public List<TreeView.TreeNode> Children
			{
				get
				{
					return this._children;
				}
			}

			public int Depth
			{
				get
				{
					return this._depth;
				}
			}

			public string Text
			{
				get
				{
					return this._text[0];
				}
				set
				{
					this._text[0] = value;
				}
			}

			public long Unique
			{
				get
				{
					return this._unique;
				}
				set
				{
					this._unique = value;
				}
			}

			public bool IsOpen
			{
				get
				{
					return this._open;
				}
				set
				{
					this._open = value;
				}
			}

			public bool IsClick
			{
				get
				{
					return this._click;
				}
				set
				{
					this._click = value;
				}
			}

			public object ObjectData
			{
				get
				{
					return this._object;
				}
				set
				{
					this._object = value;
				}
			}

			public string ImageKey
			{
				get
				{
					return this._imageKey;
				}
			}

			public TreeNode(string sText, long nUnique)
			{
				this._text[0] = sText;
				this._unique = nUnique;
				this._imageKey = string.Empty;
			}

			public TreeNode(string sText, long nUnique, string simageKey)
			{
				this._text[0] = sText;
				this._unique = nUnique;
				this._imageKey = string.Empty;
				this._imageKey = simageKey;
			}

			public TreeNode(int ndepth, string sText1, string sText2, string sText3, long nUnique)
			{
				this._depth = ndepth;
				this._text[0] = sText1;
				this._text[1] = sText2;
				this._text[2] = sText3;
				this._unique = nUnique;
				this._imageKey = string.Empty;
			}

			public TreeNode(int ndepth, string sText1, string sText2, string sText3, long nUnique, string simageKey)
			{
				this._depth = ndepth;
				this._text[0] = sText1;
				this._text[1] = sText2;
				this._text[2] = sText3;
				this._unique = nUnique;
				this._imageKey = simageKey;
			}

			public TreeNode(string sText, object obj)
			{
				this._text[0] = sText;
				this.ObjectData = obj;
				this._imageKey = string.Empty;
			}

			public TreeNode(string sText, object obj, string simageKey)
			{
				this._text[0] = sText;
				this.ObjectData = obj;
				this._imageKey = simageKey;
			}

			public TreeNode(int ndepth, string sText1, string sText2, string sText3, object obj)
			{
				this._depth = ndepth;
				this._text[0] = sText1;
				this._text[1] = sText2;
				this._text[2] = sText3;
				this.ObjectData = obj;
				this._imageKey = string.Empty;
			}

			public TreeNode(int ndepth, string sText1, string sText2, string sText3, object obj, string simageKey)
			{
				this._depth = ndepth;
				this._text[0] = sText1;
				this._text[1] = sText2;
				this._text[2] = sText3;
				this.ObjectData = obj;
				this._imageKey = simageKey;
			}

			public void AddChild(TreeView.TreeNode node)
			{
				this._children.Add(node);
			}

			public TreeView.TreeNode AddChild(string str, long unique)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(str, unique);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(string str, long unique, string imageKey)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(str, unique, imageKey);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, long unique)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, string.Empty, string.Empty, unique);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, long unique, string imageKey)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, string.Empty, string.Empty, unique, imageKey);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, string str2, long unique)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, str2, string.Empty, unique);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, string str2, long unique, string imageKey)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, str2, string.Empty, unique, imageKey);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, string str2, string str3, long unique)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, str2, str3, unique);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, string str2, string str3, long unique, string imageKey)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, str2, str3, unique, imageKey);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(string str, object obj)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(str, obj);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(string str, object obj, string imageKey)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(str, obj, imageKey);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, object obj)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, string.Empty, string.Empty, obj);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, object obj, string imageKey)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, string.Empty, string.Empty, obj, imageKey);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, string str2, object obj)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, str2, string.Empty, obj);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, string str2, object obj, string imageKey)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, str2, string.Empty, obj, imageKey);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, string str2, string str3, object obj)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, str2, str3, obj);
				this._children.Add(treeNode);
				return treeNode;
			}

			public TreeView.TreeNode AddChild(int depth, string str1, string str2, string str3, object obj, string imageKey)
			{
				TreeView.TreeNode treeNode = new TreeView.TreeNode(depth, str1, str2, str3, obj, imageKey);
				this._children.Add(treeNode);
				return treeNode;
			}

			public string GetString(int index)
			{
				if (TreeView.MAX_TEXT_NUM <= index)
				{
					return string.Empty;
				}
				return this._text[index];
			}

			public void SetOpenRecursively(bool val)
			{
				this.IsOpen = val;
				foreach (TreeView.TreeNode current in this._children)
				{
					current.SetOpenRecursively(val);
				}
			}
		}

		public static int MAX_DEPTH_COUNT = 5;

		public static int MAX_TEXT_NUM = 3;

		private string parentImageStyle = string.Empty;

		private string childImageStyle = string.Empty;

		private TreeView.TreeNode mTree;

		private bool useDepthCount;

		private float childStartX = 20f;

		private float depthX = 20f;

		private float addX;

		private SpriteText.Alignment_Type parentAlignment;

		private SpriteText.Alignment_Type childAlignment;

		private SpriteText.Alignment_Type[] childrenAlignment = new SpriteText.Alignment_Type[3];

		private SpriteText.Font_Effect[] fontEffect = new SpriteText.Font_Effect[TreeView.MAX_DEPTH_COUNT];

		private float[] fontSize = new float[]
		{
			15f,
			15f,
			15f,
			15f,
			15f
		};

		private float[,] coulmnX = new float[5, 3];

		private float[,] coulmnWidth = new float[5, 3];

		public bool checkButton = true;

		private string checkButtonStyle = string.Empty;

		private int addCount = 1;

		public string CheckButtonStyle
		{
			set
			{
				this.checkButtonStyle = value;
			}
		}

		public float ChildStartX
		{
			set
			{
				this.childStartX = value;
			}
		}

		public string ParentImageStyle
		{
			set
			{
				this.parentImageStyle = value;
			}
		}

		public string ChildImageStyle
		{
			set
			{
				this.childImageStyle = value;
			}
		}

		public bool UseDepthCount
		{
			set
			{
				this.useDepthCount = value;
			}
		}

		public SpriteText.Alignment_Type ParentAlignment
		{
			set
			{
				this.parentAlignment = value;
			}
		}

		public SpriteText.Alignment_Type ChildAlignment
		{
			set
			{
				this.childAlignment = value;
			}
		}

		public SpriteText.Font_Effect FontEffect
		{
			get
			{
				return this.fontEffect[0];
			}
			set
			{
				this.fontEffect[0] = value;
			}
		}

		public float LineHeight
		{
			get
			{
				return this.lineHeight;
			}
			set
			{
				this.lineHeight = value;
			}
		}

		public void SetChildDepthOption(int depth, int fontSize, SpriteText.Font_Effect fontEffect, int x1, int x2, int x3, int width1, int width2, int width3)
		{
			this.fontSize[depth] = (float)fontSize;
			this.fontEffect[depth] = fontEffect;
			this.coulmnX[depth, 0] = (float)x1;
			this.coulmnX[depth, 1] = (float)x2;
			this.coulmnX[depth, 2] = (float)x3;
			this.coulmnWidth[depth, 0] = (float)width1;
			this.coulmnWidth[depth, 1] = (float)width2;
			this.coulmnWidth[depth, 2] = (float)width3;
		}

		public void SetChildrenAlignment(SpriteText.Alignment_Type child0, SpriteText.Alignment_Type child1, SpriteText.Alignment_Type child2)
		{
			this.childrenAlignment[0] = child0;
			this.childrenAlignment[1] = child1;
			this.childrenAlignment[2] = child2;
		}

		public new static TreeView Create(string name, Vector3 pos)
		{
			return (TreeView)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(TreeView));
		}

		private void InitChildNode(TreeView.TreeNode node)
		{
			foreach (TreeView.TreeNode current in node.Children)
			{
				current.Children.Clear();
			}
		}

		public void InitTreeData()
		{
			if (this.mTree != null)
			{
				foreach (TreeView.TreeNode current in this.mTree.Children)
				{
					this.InitChildNode(current);
				}
				this.mTree.Children.Clear();
			}
			else
			{
				this.mTree = new TreeView.TreeNode("Root", 0L);
			}
			if (0 < base.Count)
			{
				base.ClearList(true);
			}
			this.clipWhenMoving = true;
		}

		public TreeView.TreeNode FindChildRoot(string sChildRootName)
		{
			foreach (TreeView.TreeNode current in this.mTree.Children)
			{
				if (current.Text == sChildRootName)
				{
					return current;
				}
			}
			return null;
		}

		public void SelectItemByUnique(long unique)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if ((long)this.items[i].Data > 0L && (long)this.items[i].Data == unique)
				{
					base.SetSelectedItem(i);
					break;
				}
			}
		}

		private UIListItemContainer MakeChildRootBase(string sChildRootName, TreeView.TreeNode node, bool haveCheckButton, float fontSize, SpriteText.Font_Effect fontEffect, string imageKey)
		{
			GameObject gameObject = new GameObject("ChildRoot_NameOnly");
			UIListItemContainer uIListItemContainer = gameObject.AddComponent<UIListItemContainer>();
			uIListItemContainer.gameObject.layer = GUICamera.UILayer;
			uIListItemContainer.Data = node;
			uIListItemContainer.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickButton));
			string name = UIScrollList.backButtonName;
			UIButton uIButton;
			if (string.Empty != this.parentImageStyle)
			{
				uIButton = UICreateControl.Button(name, this.parentImageStyle, this.viewableArea.x, this.lineHeight);
			}
			else
			{
				uIButton = UICreateControl.Button(name, "Com_B_ListBtnH", this.viewableArea.x, this.lineHeight);
			}
			uIButton.EffectAni = false;
			uIButton.IsListButton = true;
			uIButton.allwaysPlayAnim = true;
			uIListItemContainer.MakeChild(uIButton.gameObject);
			uIButton.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			DrawTexture drawTexture = null;
			float x;
			if (this.checkButton)
			{
				name = "arrow";
				CheckBox checkBox = null;
				if (haveCheckButton)
				{
					if (string.Empty != this.checkButtonStyle)
					{
						UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(this.checkButtonStyle);
						if (uIBaseInfoLoader != null)
						{
							checkBox = UICreateControl.CheckBox(name, this.checkButtonStyle, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
						}
					}
					else
					{
						checkBox = UICreateControl.CheckBox(name, "Win_I_ArrowBl", 12f, 10f);
					}
				}
				else
				{
					checkBox = UICreateControl.CheckBox(name, "Win_I_ListTreeN", 12f, 10f);
				}
				uIListItemContainer.MakeChild(checkBox.gameObject);
				checkBox.gameObject.transform.localPosition = new Vector3(11f, -8f, -0.05f);
				x = 24f;
				if (string.Empty != imageKey)
				{
					drawTexture = UICreateControl.DrawTexture("image", imageKey);
					uIListItemContainer.MakeChild(drawTexture.gameObject);
					float num = 12f + checkBox.width + 2f;
					float y = (this.LineHeight - drawTexture.height) / 2f * -1f;
					drawTexture.gameObject.transform.localPosition = new Vector3(num, y, -0.05f);
					x = num + drawTexture.width + 2f;
				}
			}
			else if (string.Empty != imageKey)
			{
				if (haveCheckButton)
				{
					UIBaseInfoLoader uIBaseInfoLoader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(this.checkButtonStyle);
					if (uIBaseInfoLoader2 != null && this.checkButtonStyle == imageKey)
					{
						UIButton uIButton2 = UICreateControl.Button(name, imageKey, uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount, uIBaseInfoLoader2.UVs.height);
						if (null != uIButton2)
						{
							uIListItemContainer.MakeChild(uIButton2.gameObject);
							uIButton2.PlayAni(true);
							float y2 = (this.LineHeight - uIButton2.height) / 2f * -1f;
							uIButton2.gameObject.transform.localPosition = new Vector3(6f, y2, -0.05f);
						}
					}
					x = 6f + uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount + 6f;
				}
				else
				{
					drawTexture = UICreateControl.DrawTexture("image", imageKey);
					if (null != drawTexture)
					{
						uIListItemContainer.MakeChild(drawTexture.gameObject);
						float y3 = (this.LineHeight - drawTexture.height) / 2f * -1f;
						drawTexture.gameObject.transform.localPosition = new Vector3(10f, y3, -0.05f);
					}
					x = 26f;
				}
			}
			else
			{
				x = 6f;
			}
			name = "text" + base.Count.ToString();
			GameObject gameObject2 = new GameObject(name);
			Label label = gameObject2.AddComponent<Label>();
			label.gameObject.layer = GUICamera.UILayer;
			uIListItemContainer.MakeChild(label.gameObject);
			label.width = this.viewableArea.x;
			label.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			label.CreateSpriteText();
			float y4 = this.lineHeight / 2f * -1f;
			if (this.parentAlignment == SpriteText.Alignment_Type.Left)
			{
				label.spriteText.SetAnchor(SpriteText.Anchor_Pos.Middle_Left);
				label.spriteText.SetAlignment(this.parentAlignment);
				label.spriteText.transform.localPosition = new Vector3(x, y4, -0.02f);
			}
			else if (this.parentAlignment == SpriteText.Alignment_Type.Right)
			{
				label.spriteText.SetAnchor(SpriteText.Anchor_Pos.Middle_Right);
				label.spriteText.SetAlignment(this.parentAlignment);
				label.spriteText.transform.localPosition = new Vector3(this.viewableArea.x - 6f, y4, -0.02f);
				if (null != drawTexture)
				{
					float num2 = this.viewableArea.x - 6f - label.spriteText.TotalWidth;
					drawTexture.gameObject.transform.localPosition = new Vector3(num2 - 10f, drawTexture.gameObject.transform.localPosition.y, -0.05f);
				}
			}
			else
			{
				label.spriteText.SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
				label.spriteText.SetAlignment(this.parentAlignment);
				label.spriteText.transform.localPosition = new Vector3(this.viewableArea.x / 2f, y4, -0.02f);
				if (null != drawTexture)
				{
					float num3 = this.viewableArea.x / 2f - label.spriteText.TotalWidth;
					drawTexture.gameObject.transform.localPosition = new Vector3(num3 - 10f, drawTexture.gameObject.transform.localPosition.y, -0.05f);
				}
			}
			label.MaxWidth = this.viewableArea.x - label.spriteText.transform.localPosition.x;
			label.MultiLine = false;
			label.SPOT = true;
			label.SetFontEffect(fontEffect);
			label.SetCharacterSize(fontSize);
			label.Text = sChildRootName;
			label.BackGroundHide(true);
			return uIListItemContainer;
		}

		private TreeView.TreeNode InsertChildRootBase(string sChildRootName, TreeView.TreeNode node, bool haveCheckButton, string imageKey)
		{
			this.mTree.AddChild(node);
			UIListItemContainer item;
			if (this.useDepthCount)
			{
				item = this.MakeChildRootBase(sChildRootName, node, haveCheckButton, this.fontSize[0], this.fontEffect[0], imageKey);
			}
			else
			{
				item = this.MakeChildRootBase(sChildRootName, node, haveCheckButton, 15f, this.FontEffect, imageKey);
			}
			base.InsertItemDonotPosionUpdate(item, base.Count, null, true);
			return node;
		}

		private TreeView.TreeNode InsertChildRootBase(string sChildRootName, TreeView.TreeNode node, bool haveCheckButton)
		{
			this.mTree.AddChild(node);
			UIListItemContainer item;
			if (this.useDepthCount)
			{
				item = this.MakeChildRootBase(sChildRootName, node, haveCheckButton, this.fontSize[0], this.fontEffect[0], string.Empty);
			}
			else
			{
				item = this.MakeChildRootBase(sChildRootName, node, haveCheckButton, 15f, this.FontEffect, string.Empty);
			}
			base.InsertItemDonotPosionUpdate(item, base.Count, null, true);
			return node;
		}

		private TreeView.TreeNode InsertChildRootBase(string sChildRootName, TreeView.TreeNode node, bool haveCheckButton, float fontSize, SpriteText.Font_Effect fontEffect, string imageKey)
		{
			this.mTree.AddChild(node);
			UIListItemContainer item = this.MakeChildRootBase(sChildRootName, node, haveCheckButton, fontSize, fontEffect, imageKey);
			base.InsertItemDonotPosionUpdate(item, base.Count, null, true);
			return node;
		}

		public void ClickButton(IUIObject obj)
		{
			UIListItemContainer selectedItem = base.SelectedItem;
			if (null != selectedItem)
			{
				Transform transform = selectedItem.transform.FindChild("arrow");
				if (transform != null)
				{
					CheckBox component = transform.GetComponent<CheckBox>();
					if (null != component)
					{
						component.ToggleState();
					}
				}
			}
		}

		public TreeView.TreeNode InsertChildRoot(string sChildRootName, bool haveCheckButton)
		{
			TreeView.TreeNode node = new TreeView.TreeNode(sChildRootName, 0L);
			return this.InsertChildRootBase(sChildRootName, node, haveCheckButton);
		}

		public TreeView.TreeNode InsertChildRoot(string sChildRootName, bool haveCheckButton, float fontSize, SpriteText.Font_Effect fontEffect)
		{
			TreeView.TreeNode node = new TreeView.TreeNode(sChildRootName, 0L);
			return this.InsertChildRootBase(sChildRootName, node, haveCheckButton, fontSize, fontEffect, string.Empty);
		}

		public TreeView.TreeNode InsertChildRoot(string sChildRootName, bool haveCheckButton, float fontSize, SpriteText.Font_Effect fontEffect, string imageKey)
		{
			TreeView.TreeNode node = new TreeView.TreeNode(sChildRootName, 0L);
			return this.InsertChildRootBase(sChildRootName, node, haveCheckButton, fontSize, fontEffect, imageKey);
		}

		public TreeView.TreeNode InsertChildRoot(string sChildRootName, long lUnique, bool haveCheckButton)
		{
			TreeView.TreeNode node = new TreeView.TreeNode(sChildRootName, lUnique);
			return this.InsertChildRootBase(sChildRootName, node, haveCheckButton);
		}

		public TreeView.TreeNode InsertChildRoot(string sChildRootName, long lUnique, bool haveCheckButton, string imageKey)
		{
			TreeView.TreeNode node = new TreeView.TreeNode(sChildRootName, lUnique);
			return this.InsertChildRootBase(sChildRootName, node, haveCheckButton, imageKey);
		}

		public TreeView.TreeNode InsertChildRoot(string sChildRootName, object obj, bool haveCheckButton)
		{
			return this.InsertChildRootBase(sChildRootName, new TreeView.TreeNode(sChildRootName, 0L)
			{
				ObjectData = obj
			}, haveCheckButton);
		}

		public TreeView.TreeNode InsertChildRoot(string sChildRootName, object obj, bool haveCheckButton, string imageKey)
		{
			return this.InsertChildRootBase(sChildRootName, new TreeView.TreeNode(sChildRootName, 0L)
			{
				ObjectData = obj
			}, haveCheckButton, imageKey);
		}

		public void ExpandNode(TreeView.TreeNode childRoot, int index)
		{
			if (0 < childRoot.Children.Count)
			{
				if (!childRoot.IsOpen)
				{
					this.addCount = 1;
					this.InsertChildNode(childRoot, index);
					base.DonotCountRepositionItems();
					childRoot.IsOpen = true;
				}
				else
				{
					this.DeleteChildNode(childRoot, index);
					childRoot.IsOpen = false;
				}
			}
		}

		public void InsertChildNode(TreeView.TreeNode childRoot, int index)
		{
			foreach (TreeView.TreeNode current in childRoot.Children)
			{
				if (0 < current.Children.Count)
				{
					string text = base.Count.ToString();
					GameObject gameObject = new GameObject("ChildNode_Index" + text);
					UIListItemContainer uIListItemContainer = gameObject.AddComponent<UIListItemContainer>();
					uIListItemContainer.gameObject.layer = GUICamera.UILayer;
					uIListItemContainer.Data = current;
					this.MakeLineItem(ref uIListItemContainer, text, current);
					base.InsertItemDonotPosionUpdate(uIListItemContainer, index + this.addCount, null, true);
					this.addCount++;
					if (current.IsOpen)
					{
						this.InsertChildNode(current, index);
					}
				}
				else if (current != null)
				{
					string text2 = base.Count.ToString();
					GameObject gameObject2 = new GameObject("ChildNode_Index" + text2);
					UIListItemContainer uIListItemContainer2 = gameObject2.AddComponent<UIListItemContainer>();
					uIListItemContainer2.gameObject.layer = GUICamera.UILayer;
					uIListItemContainer2.Data = current;
					this.MakeLineItem(ref uIListItemContainer2, text2, current);
					base.InsertItemDonotPosionUpdate(uIListItemContainer2, index + this.addCount, null, true);
					this.addCount++;
				}
			}
		}

		public void InsertChildNode(TreeView.TreeNode childRoot)
		{
			int num = 0;
			foreach (TreeView.TreeNode current in childRoot.Children)
			{
				if (current != null)
				{
					string text = num.ToString();
					GameObject gameObject = new GameObject("ChildNode_None" + text);
					UIListItemContainer uIListItemContainer = gameObject.AddComponent<UIListItemContainer>();
					uIListItemContainer.gameObject.layer = GUICamera.UILayer;
					uIListItemContainer.Data = current;
					this.MakeLineItem(ref uIListItemContainer, text, current);
					base.InsertItemDonotPosionUpdate(uIListItemContainer, base.Count + num, null, true);
					num++;
					current.IsOpen = false;
				}
			}
		}

		public void DeleteChildNode(TreeView.TreeNode childRoot, int index)
		{
			foreach (TreeView.TreeNode current in childRoot.Children)
			{
				if (0 < current.Children.Count)
				{
					base.RemoveItem(index + 1, true);
					if (current.IsOpen)
					{
						this.DeleteChildNode(current, index);
					}
				}
				else if (current != null)
				{
					base.RemoveItem(index + 1, true);
				}
			}
		}

		public TreeView.TreeNode SelectNode()
		{
			UIListItemContainer selectedItem = base.SelectedItem;
			return (TreeView.TreeNode)selectedItem.data;
		}

		public void Open_Node(int a_nIndex)
		{
			IUIListObject item = base.GetItem(a_nIndex);
			if (item == null)
			{
				return;
			}
			TreeView.TreeNode treeNode = item.Data as TreeView.TreeNode;
			if (treeNode == null)
			{
				return;
			}
			if (!treeNode.IsOpen)
			{
				base.SetSelectedItem(a_nIndex);
				this.ClickButton(null);
				this.ExpandNode(treeNode, a_nIndex);
			}
		}

		private void MakeLineItem(ref UIListItemContainer container, string index, TreeView.TreeNode node)
		{
			string backButtonName = UIScrollList.backButtonName;
			UIButton uIButton;
			if (string.Empty != this.childImageStyle)
			{
				uIButton = UICreateControl.Button(backButtonName, this.childImageStyle, base.GetSize().x, this.lineHeight);
			}
			else
			{
				uIButton = UICreateControl.Button(backButtonName, "Com_B_ListBtnH", base.GetSize().x, this.lineHeight);
			}
			uIButton.EffectAni = false;
			uIButton.IsListButton = true;
			uIButton.allwaysPlayAnim = true;
			container.MakeChild(uIButton.gameObject);
			uIButton.gameObject.transform.localPosition = Vector3.zero;
			if (string.Empty != node.ImageKey)
			{
				base.name = "image";
				CheckBox checkBox = UICreateControl.CheckBox(base.name, node.ImageKey, 10f, 10f);
				container.MakeChild(checkBox.gameObject);
				checkBox.gameObject.transform.localPosition = new Vector3((float)node.Depth * this.depthX + this.childStartX, -this.LineHeight / 2f + 5f, -0.05f);
				this.addX = 12f;
			}
			if (this.useDepthCount)
			{
				for (int i = 0; i < TreeView.MAX_TEXT_NUM; i++)
				{
					if (string.Empty != node.GetString(i))
					{
						base.name = "text" + i;
						GameObject gameObject = new GameObject(base.name);
						Label label = gameObject.AddComponent<Label>();
						label.gameObject.layer = GUICamera.UILayer;
						label.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
						container.MakeChild(label.gameObject);
						label.CreateSpriteText();
						float y = this.lineHeight / 2f * -1f;
						if (null != label.spriteText)
						{
							if (i == 0)
							{
								label.SetAnchor(SpriteText.Anchor_Pos.Middle_Left);
								label.SetAlignment(SpriteText.Alignment_Type.Left);
								label.gameObject.transform.localPosition = new Vector3((float)node.Depth * this.depthX + this.childStartX + this.addX, y, 0f);
								label.MaxWidth = this.viewableArea.x - label.gameObject.transform.localPosition.x;
							}
							else
							{
								if (this.childrenAlignment[i] == SpriteText.Alignment_Type.Left)
								{
									label.SetAnchor(SpriteText.Anchor_Pos.Middle_Left);
									label.SetAlignment(this.childrenAlignment[i]);
								}
								else if (this.childrenAlignment[i] == SpriteText.Alignment_Type.Center)
								{
									label.SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
									label.SetAlignment(this.childrenAlignment[i]);
								}
								else if (this.childrenAlignment[i] == SpriteText.Alignment_Type.Right)
								{
									label.SetAnchor(SpriteText.Anchor_Pos.Middle_Right);
									label.SetAlignment(this.childrenAlignment[i]);
								}
								label.gameObject.transform.localPosition = new Vector3(this.coulmnX[node.Depth, i], y, 0f);
								label.MaxWidth = this.viewableArea.x - label.gameObject.transform.localPosition.x;
							}
							label.width = this.coulmnWidth[node.Depth, i];
							label.MultiLine = false;
							label.SetFontEffect(this.fontEffect[node.Depth]);
							label.SetCharacterSize(this.fontSize[node.Depth]);
							label.SPOT = true;
							label.Text = node.GetString(i);
							label.BackGroundHide(true);
						}
					}
				}
			}
			else
			{
				base.name = "text" + base.Count.ToString();
				GameObject gameObject2 = new GameObject(base.name);
				Label label2 = gameObject2.AddComponent<Label>();
				label2.gameObject.layer = GUICamera.UILayer;
				label2.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
				label2.CreateSpriteText();
				container.MakeChild(label2.gameObject);
				float y2 = this.lineHeight / 2f * -1f;
				if (null != label2.spriteText)
				{
					if (this.childAlignment == SpriteText.Alignment_Type.Left)
					{
						label2.SetAnchor(SpriteText.Anchor_Pos.Middle_Left);
						label2.spriteText.transform.localPosition = new Vector3(this.childStartX + this.addX, y2, 0f);
					}
					else if (this.childAlignment == SpriteText.Alignment_Type.Right)
					{
						label2.SetAnchor(SpriteText.Anchor_Pos.Middle_Right);
						label2.spriteText.transform.localPosition = new Vector3(this.viewableArea.x - 6f, y2, 0f);
					}
					else
					{
						label2.SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
						label2.spriteText.transform.localPosition = new Vector3(this.viewableArea.x / 2f, y2, 0f);
					}
					label2.SetAlignment(this.childAlignment);
				}
				label2.MaxWidth = this.viewableArea.x - label2.spriteText.transform.localPosition.x;
				label2.MultiLine = false;
				label2.SPOT = true;
				label2.SetFontEffect(this.FontEffect);
				label2.SetCharacterSize(15f);
				label2.Text = node.Text;
				label2.BackGroundHide(true);
			}
		}
	}
}
