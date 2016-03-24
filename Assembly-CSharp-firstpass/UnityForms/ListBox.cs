using GAME;
using GameMessage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityForms
{
	public class ListBox : UIScrollList
	{
		private DrawTexture _BG;

		protected string columnTitle = "Column";

		protected static int MAX_COLUMN_NUM = 15;

		protected int[] columnWidth = new int[ListBox.MAX_COLUMN_NUM];

		protected Rect[] columnRect = new Rect[ListBox.MAX_COLUMN_NUM];

		protected bool[] columSpot = new bool[ListBox.MAX_COLUMN_NUM];

		protected int currentColumnNum;

		protected bool useColumnRect;

		protected string selectStyle = string.Empty;

		protected SpriteText.Font_Effect fontEffect;

		protected SpriteText.Anchor_Pos[] columnTextAnchor = new SpriteText.Anchor_Pos[ListBox.MAX_COLUMN_NUM];

		protected int startIndex;

		protected bool autoListBox = true;

		protected float[] m_faFontSize = new float[ListBox.MAX_COLUMN_NUM];

		protected bool bUseSpotLabel;

		protected bool[] bUseMultiLine = new bool[ListBox.MAX_COLUMN_NUM];

		protected float _offsetX = 9f;

		private Queue<ListItem> reserveItems = new Queue<ListItem>();

		private float checkTime;

		private bool callRepositionItems;

		public override bool Visible
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
			set
			{
				if (base.gameObject.activeInHierarchy != value)
				{
					base.gameObject.SetActive(value);
					if (this.slider)
					{
						if (this.changeScrollPos)
						{
							this.slider.Visible = value;
						}
						else
						{
							this.slider.Visible = false;
						}
					}
					if (null != this._BG)
					{
						this._BG.Visible = value;
					}
				}
			}
		}

		public virtual float OffsetX
		{
			set
			{
				this._offsetX = value;
			}
		}

		public bool UseSpotLabel
		{
			set
			{
				this.bUseSpotLabel = value;
			}
		}

		public bool AutoListBox
		{
			set
			{
				this.autoListBox = value;
			}
		}

		public DrawTexture BG
		{
			get
			{
				return this._BG;
			}
			set
			{
				this._BG = value;
			}
		}

		public SpriteText.Font_Effect FontEffect
		{
			get
			{
				return this.fontEffect;
			}
			set
			{
				this.fontEffect = value;
			}
		}

		public string SelectStyle
		{
			set
			{
				this.selectStyle = value;
			}
		}

		public virtual bool UseColumnRect
		{
			set
			{
				this.useColumnRect = value;
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
				this.maxLine = (int)Math.Round((double)(this.viewableArea.y / this.LineHeight));
			}
		}

		public virtual int ColumnNum
		{
			get
			{
				return this.currentColumnNum;
			}
			set
			{
				if (ListBox.MAX_COLUMN_NUM <= value)
				{
					this.currentColumnNum = ListBox.MAX_COLUMN_NUM;
				}
				this.currentColumnNum = value;
			}
		}

		public int SelectIndex
		{
			get
			{
				if (base.SelectedItem == null)
				{
					return -1;
				}
				return base.SelectedItem.GetIndex();
			}
			set
			{
				base.SetSelectedItem(value);
			}
		}

		public new static ListBox Create(string name, Vector3 pos)
		{
			return (ListBox)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(ListBox));
		}

		public virtual void SetColumnRect(int index, int x, int y, int width, int height)
		{
			this.SetColumnRect(index, x, y, width, height, SpriteText.Anchor_Pos.Middle_Center);
		}

		public virtual void SetColumnRect(int index, Rect a_rcRect)
		{
			this.SetColumnRect(index, a_rcRect, SpriteText.Anchor_Pos.Middle_Center);
		}

		public virtual void SetColumnRect(int index, int x, int y, int width, int height, SpriteText.Anchor_Pos anchor)
		{
			if (0 <= index && ListBox.MAX_COLUMN_NUM > index)
			{
				this.columnRect[index] = new Rect((float)x, (float)y, (float)width, (float)height);
			}
			this.columnTextAnchor[index] = anchor;
		}

		public virtual void SetColumnRect(int index, Rect a_rcRect, SpriteText.Anchor_Pos anchor)
		{
			if (0 <= index && ListBox.MAX_COLUMN_NUM > index)
			{
				this.columnRect[index] = a_rcRect;
			}
			this.columnTextAnchor[index] = anchor;
		}

		public virtual void SetColumnRect(int index, int x, int y, int width, int height, SpriteText.Anchor_Pos anchor, float a_fFonSize)
		{
			if (0 <= index && ListBox.MAX_COLUMN_NUM > index)
			{
				this.columnRect[index] = new Rect((float)x, (float)y, (float)width, (float)height);
			}
			this.columnTextAnchor[index] = anchor;
			this.m_faFontSize[index] = a_fFonSize;
		}

		public virtual void SetColumnRect(int index, int x, int y, int width, int height, SpriteText.Anchor_Pos anchor, float a_fFonSize, bool a_bMultiLine)
		{
			if (0 <= index && ListBox.MAX_COLUMN_NUM > index)
			{
				this.columnRect[index] = new Rect((float)x, (float)y, (float)width, (float)height);
			}
			this.columnTextAnchor[index] = anchor;
			this.m_faFontSize[index] = a_fFonSize;
			this.bUseMultiLine[index] = a_bMultiLine;
		}

		public virtual void SetColumnRect(int index, Rect a_rcRect, SpriteText.Anchor_Pos anchor, float a_fFonSize)
		{
			if (0 <= index && ListBox.MAX_COLUMN_NUM > index)
			{
				this.columnRect[index] = a_rcRect;
			}
			this.columnTextAnchor[index] = anchor;
			this.m_faFontSize[index] = a_fFonSize;
		}

		public virtual void SetColumnWidth(int size0, int size1, int size2, int size3, int size4)
		{
			this.columnWidth[0] = size0;
			this.columnWidth[1] = size1;
			this.columnWidth[2] = size2;
			this.columnWidth[3] = size3;
			this.columnWidth[4] = size4;
			for (int i = 0; i < ListBox.MAX_COLUMN_NUM; i++)
			{
				this.columnTextAnchor[i] = SpriteText.Anchor_Pos.Middle_Center;
			}
		}

		public virtual void SetColumnWidth(int size0, int size1, int size2, int size3, int size4, float a_fFonSize)
		{
			this.columnWidth[0] = size0;
			this.columnWidth[1] = size1;
			this.columnWidth[2] = size2;
			this.columnWidth[3] = size3;
			this.columnWidth[4] = size4;
			for (int i = 0; i < ListBox.MAX_COLUMN_NUM; i++)
			{
				this.columnTextAnchor[i] = SpriteText.Anchor_Pos.Middle_Center;
				this.m_faFontSize[i] = a_fFonSize;
			}
		}

		public virtual void SetColumSpot(int index, bool bSpot)
		{
			if (0 > index && ListBox.MAX_COLUMN_NUM <= index)
			{
				return;
			}
			this.columSpot[index] = bSpot;
		}

		public virtual void SetColumnAlignment(int index, SpriteText.Anchor_Pos anchor)
		{
			if (0 > index && ListBox.MAX_COLUMN_NUM <= index)
			{
				return;
			}
			this.columnTextAnchor[index] = anchor;
		}

		public string GetSelectItemColumnStr(int index)
		{
			string elementName = this.columnTitle + index.ToString();
			SpriteRoot element = base.SelectedItem.GetElement(elementName);
			if (!(null != element))
			{
				return string.Empty;
			}
			UIButton component = element.gameObject.GetComponent<UIButton>();
			if (null != component)
			{
				return component.Text;
			}
			return string.Empty;
		}

		public object GetDataObject()
		{
			return base.SelectedItem.Data;
		}

		public void AddListItem(UIListItemContainer item)
		{
			base.AddItem(item);
		}

		public void AddItem(string text, object data)
		{
			ListItem listItem = new ListItem();
			listItem.SetColumnStr(0, text);
			listItem.Key = data;
			this.Add(listItem);
		}

		public void Add(UIListItemContainer container)
		{
			base.InsertItemDonotPosionUpdate(container, this.startIndex, null, true);
		}

		public virtual void RemoveAdd(int index, ListItem item)
		{
			UIListItemContainer uIListItemContainer = this.CreateContainer(item);
			if (null != uIListItemContainer)
			{
				uIListItemContainer.SetControlIsEnabled(item.enable);
				uIListItemContainer.transform.position = Vector3.zero;
				if (null != base.GetItem(index))
				{
					base.RemoveItemDonotPositionUpdate(index, true);
					base.InsertItemDonotPosionUpdate(uIListItemContainer, index, null, false);
				}
				else
				{
					base.InsertItemDonotPosionUpdate(uIListItemContainer, index, null, false);
				}
			}
		}

		public virtual void Add(ListItem item)
		{
			if (this.bLabelScroll)
			{
				char[] separator = new char[]
				{
					'\n'
				};
				string[] array = item.GetColumnStr(0).Split(separator);
				for (int i = 0; i < array.Length; i++)
				{
					GameObject gameObject = new GameObject("ListItem");
					FlashLabel flashLabel = gameObject.AddComponent<FlashLabel>();
					flashLabel.FontSize = (float)((int)this.m_faFontSize[0]);
					flashLabel.FontEffect = this.fontEffect;
					flashLabel.FontColor = string.Empty;
					flashLabel.anchor = this.columnTextAnchor[0];
					flashLabel.width = this.viewableArea.x - 20f;
					if (null != base.GetItem(this.startIndex))
					{
						base.RemoveItemDonotPositionUpdate(this.startIndex, true);
						flashLabel.SetFlashLabel(array[i]);
						base.InsertItemDonotPosionUpdate(flashLabel, this.startIndex, null, true);
					}
					else
					{
						flashLabel.SetFlashLabel(array[i]);
						base.InsertItemDonotPosionUpdate(flashLabel, this.startIndex, null, true);
					}
					this.startIndex++;
				}
			}
			else if (this.m_bReserve)
			{
				if (this.items.Count == 0)
				{
					this.MakeContainer(item);
				}
				else
				{
					this.reserveItems.Enqueue(item);
				}
			}
			else
			{
				this.MakeContainer(item);
			}
		}

		public UIListItemContainer CreateContainer(ListItem item)
		{
			GameObject gameObject = new GameObject("ListItem");
			UIListItemContainer uIListItemContainer = gameObject.AddComponent<UIListItemContainer>();
			uIListItemContainer.Data = item.Key;
			uIListItemContainer.isDraggable = true;
			uIListItemContainer.AutoFindOuterEdges = false;
			string backButtonName = UIScrollList.backButtonName;
			UIButton uIButton;
			if (this.orientation == UIScrollList.ORIENTATION.VERTICAL)
			{
				if (string.Empty != this.selectStyle)
				{
					uIButton = UICreateControl.Button(backButtonName, this.selectStyle, this.viewableArea.x, this.lineHeight);
				}
				else
				{
					uIButton = UICreateControl.Button(backButtonName, "Com_B_ListBtnH", this.viewableArea.x, this.lineHeight);
				}
			}
			else if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
			{
				if (string.Empty != this.selectStyle)
				{
					uIButton = UICreateControl.Button(backButtonName, this.selectStyle, this.lineHeight, this.viewableArea.y);
				}
				else
				{
					uIButton = UICreateControl.Button(backButtonName, "Com_B_ListBtnH", this.lineHeight, this.viewableArea.y);
				}
			}
			else if (string.Empty != this.selectStyle)
			{
				uIButton = UICreateControl.Button(backButtonName, this.selectStyle, this.viewableArea.x, this.lineHeight);
			}
			else
			{
				uIButton = UICreateControl.Button(backButtonName, "Com_B_ListBtnH", this.viewableArea.x, this.lineHeight);
			}
			uIButton.EffectAni = false;
			uIButton.IsListButton = true;
			uIButton.allwaysPlayAnim = true;
			uIListItemContainer.MakeChild(uIButton.gameObject);
			uIButton.gameObject.transform.localPosition = Vector3.zero;
			float num = this._offsetX;
			float num2 = 0f;
			for (int i = 0; i < this.currentColumnNum; i++)
			{
				string name = this.columnTitle;
				num2 -= 0.2f;
				if (item.GetType(i) == ListItem.TYPE.IMAGE)
				{
					GameObject gameObject2 = new GameObject(name);
					DrawTexture drawTexture = gameObject2.AddComponent<DrawTexture>();
					drawTexture.gameObject.layer = GUICamera.UILayer;
					drawTexture.Data = item.ColumnKey[i];
					if (this.useColumnRect)
					{
						if (item.ColumnKey[i] is float)
						{
							drawTexture.width = (float)item.ColumnKey[i];
							drawTexture.height = this.columnRect[i].height;
						}
						else
						{
							drawTexture.width = this.columnRect[i].width;
							drawTexture.height = this.columnRect[i].height;
						}
					}
					else
					{
						drawTexture.width = (float)this.columnWidth[i];
						drawTexture.height = this.lineHeight;
					}
					ITEM iTEM = item.ColumnKey[i] as ITEM;
					if (iTEM != null)
					{
						if (iTEM.m_nItemUnique == 70000)
						{
							NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_HEARTS_STONE", drawTexture, new Vector2(drawTexture.width, drawTexture.height));
						}
						int num3 = iTEM.m_nOption[2];
						if (string.Compare(MsgHandler.HandleReturn<string>("RankStateString", new object[]
						{
							num3
						}), "best") == 0)
						{
							NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_WEAPON_GOOD", drawTexture, new Vector2(drawTexture.width, drawTexture.height));
						}
					}
					if (item.ColumnKey[i] is ITEM)
					{
						drawTexture.c_cItemTooltip = (ITEM)item.ColumnKey[i];
					}
					else if (item.ColumnKey[i] is int)
					{
						drawTexture.nItemUniqueTooltip = (int)item.ColumnKey[i];
					}
					else if (item.ColumnKey[i] is string)
					{
						drawTexture.ToolTip = (string)item.ColumnKey[i];
					}
					if (item.ColumnKey2[i] is ITEM)
					{
						drawTexture.c_cItemSecondTooltip = (ITEM)item.ColumnKey2[i];
					}
					drawTexture.autoResize = false;
					drawTexture.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					UIBaseInfoLoader columnImageInfo = item.GetColumnImageInfo(i);
					if (columnImageInfo != null && string.Empty != columnImageInfo.Material)
					{
						drawTexture.BaseInfoLoderImage = columnImageInfo;
					}
					else
					{
						drawTexture.SetTextureKey(item.GetColumnImageStr(i));
					}
					float num4 = item.Get_Alpha(i);
					if (num4 > 0f)
					{
						drawTexture.SetAlpha(num4);
					}
					uIListItemContainer.MakeChild(drawTexture.gameObject);
					if (this.useColumnRect)
					{
						drawTexture.Setup(drawTexture.width, drawTexture.height);
						drawTexture.gameObject.transform.localPosition = new Vector3(this.columnRect[i].x, -this.columnRect[i].y, num2);
						BoxCollider component = drawTexture.gameObject.GetComponent<BoxCollider>();
						if (null != component)
						{
							component.size = new Vector3(this.columnRect[i].width, this.columnRect[i].height, 0f);
							component.center = new Vector3(this.columnRect[i].x / 2f, -this.columnRect[i].y / 2f, 0f);
						}
					}
					else
					{
						drawTexture.Setup((float)this.columnWidth[i], this.lineHeight);
						drawTexture.gameObject.transform.localPosition = new Vector3(num, 0f, -0.02f);
						num += (float)this.columnWidth[i];
						BoxCollider component2 = drawTexture.gameObject.GetComponent<BoxCollider>();
						if (null != component2)
						{
							component2.size = new Vector3((float)this.columnWidth[i], this.lineHeight, 0f);
							component2.center = new Vector3((float)(this.columnWidth[i] / 2), -this.lineHeight / 2f, num2);
						}
					}
					drawTexture.AddValueChangedDelegate(item.GetColumnDelegate(i));
				}
				else if (item.GetType(i) == ListItem.TYPE.TEXT)
				{
					GameObject gameObject3 = new GameObject(name);
					Label label = gameObject3.AddComponent<Label>();
					label.customCollider = true;
					label.gameObject.layer = GUICamera.UILayer;
					label.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					label.Data = item.ColumnKey[i];
					float x;
					float y;
					if (this.useColumnRect)
					{
						label.width = this.columnRect[i].width;
						label.height = this.columnRect[i].height;
						x = this.columnRect[i].x;
						y = -1f * this.columnRect[i].y;
					}
					else
					{
						x = num;
						y = 0f;
						label.width = (float)this.columnWidth[i];
						label.height = this.lineHeight;
						num += (float)this.columnWidth[i];
					}
					label.CreateSpriteText();
					label.MaxWidth = label.width;
					if (this.bUseMultiLine[i])
					{
						label.MultiLine = true;
					}
					else
					{
						label.MultiLine = false;
						if (this.bUseSpotLabel)
						{
							label.SPOT = true;
						}
						else
						{
							label.SPOT = this.columSpot[i];
						}
					}
					label.Text = item.GetColumnStr(i);
					label.SetFontEffect(this.FontEffect);
					label.BackGroundHide(true);
					if (this.m_faFontSize[i] > 0f)
					{
						label.SetCharacterSize(this.m_faFontSize[i]);
					}
					if (label.spriteText)
					{
						if (this.columnTextAnchor[i] == SpriteText.Anchor_Pos.Upper_Left || this.columnTextAnchor[i] == SpriteText.Anchor_Pos.Middle_Left || this.columnTextAnchor[i] == SpriteText.Anchor_Pos.Lower_Left)
						{
							label.SetAnchor(this.columnTextAnchor[i]);
							label.SetAlignment(SpriteText.Alignment_Type.Left);
						}
						else if (this.columnTextAnchor[i] == SpriteText.Anchor_Pos.Upper_Center || this.columnTextAnchor[i] == SpriteText.Anchor_Pos.Middle_Center || this.columnTextAnchor[i] == SpriteText.Anchor_Pos.Lower_Center)
						{
							label.SetAnchor(this.columnTextAnchor[i]);
							label.SetAlignment(SpriteText.Alignment_Type.Center);
						}
						else if (this.columnTextAnchor[i] == SpriteText.Anchor_Pos.Upper_Right || this.columnTextAnchor[i] == SpriteText.Anchor_Pos.Middle_Right || this.columnTextAnchor[i] == SpriteText.Anchor_Pos.Lower_Right)
						{
							label.SetAnchor(this.columnTextAnchor[i]);
							label.SetAlignment(SpriteText.Alignment_Type.Right);
						}
					}
					uIListItemContainer.MakeChild(label.gameObject);
					label.gameObject.transform.localPosition = new Vector3(x, y, num2);
				}
				else if (item.GetType(i) == ListItem.TYPE.BUTTON)
				{
					UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(item.GetColumnImageStr(i));
					if (uIBaseInfoLoader != null)
					{
						GameObject gameObject4 = new GameObject(name);
						UIButton uIButton2 = gameObject4.AddComponent<UIButton>();
						uIButton2.CreateSpriteText();
						uIButton2.gameObject.layer = GUICamera.UILayer;
						uIButton2.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
						uIButton2.SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
						uIButton2.SetAlignment(SpriteText.Alignment_Type.Center);
						uIButton2.AddValueChangedDelegate(item.GetColumnDelegate(i));
						uIButton2.width = this.columnRect[i].width;
						uIButton2.height = this.columnRect[i].height;
						uIButton2.Data = item.ColumnKey[i];
						uIButton2.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
						Material material = CResources.Load(uIBaseInfoLoader.Material) as Material;
						uIButton2.Setup(this.columnRect[i].width, this.columnRect[i].height, material);
						float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
						Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
						Rect uvs = new Rect(rect);
						uvs.x += pixelToUVsWidth;
						if (TsPlatform.IsMobile)
						{
							if (uIBaseInfoLoader.ButtonCount == 4)
							{
								for (int j = 0; j < 4; j++)
								{
									uIButton2.States[j].spriteFrames = new CSpriteFrame[1];
									uIButton2.States[j].spriteFrames[0] = new CSpriteFrame();
									rect.x += pixelToUVsWidth;
									uIButton2.States[j].spriteFrames[0].uvs = rect;
									uIButton2.animations[j].SetAnim(uIButton2.States[j], j);
								}
							}
							else if (uIBaseInfoLoader.ButtonCount == 3)
							{
								rect.x += pixelToUVsWidth;
								uIButton2.States[0].spriteFrames = new CSpriteFrame[1];
								uIButton2.States[0].spriteFrames[0] = new CSpriteFrame();
								uIButton2.States[0].spriteFrames[0].uvs = uvs;
								uIButton2.animations[0] = new UVAnimation();
								uIButton2.animations[0].SetAnim(uIButton2.States[0], 0);
								uIButton2.States[1].spriteFrames = new CSpriteFrame[1];
								uIButton2.States[1].spriteFrames[0] = new CSpriteFrame();
								uIButton2.States[1].spriteFrames[0].uvs = uvs;
								uIButton2.animations[1].SetAnim(uIButton2.States[1], 1);
								rect.x += pixelToUVsWidth;
								uIButton2.States[2].spriteFrames = new CSpriteFrame[1];
								uIButton2.States[2].spriteFrames[0] = new CSpriteFrame();
								uIButton2.States[2].spriteFrames[0].uvs = rect;
								uIButton2.animations[2].SetAnim(uIButton2.States[2], 2);
								rect.x += pixelToUVsWidth;
								uIButton2.States[3].spriteFrames = new CSpriteFrame[1];
								uIButton2.States[3].spriteFrames[0] = new CSpriteFrame();
								uIButton2.States[3].spriteFrames[0].uvs = rect;
								uIButton2.animations[3].SetAnim(uIButton2.States[3], 3);
							}
							else if (uIBaseInfoLoader.ButtonCount == 2)
							{
								rect.x += pixelToUVsWidth;
								uIButton2.States[0].spriteFrames = new CSpriteFrame[1];
								uIButton2.States[0].spriteFrames[0] = new CSpriteFrame();
								uIButton2.States[0].spriteFrames[0].uvs = uvs;
								uIButton2.animations[0].SetAnim(uIButton2.States[0], 0);
								rect.x += pixelToUVsWidth;
								uIButton2.States[1].spriteFrames = new CSpriteFrame[1];
								uIButton2.States[1].spriteFrames[0] = new CSpriteFrame();
								uIButton2.States[1].spriteFrames[0].uvs = uvs;
								uIButton2.animations[1].SetAnim(uIButton2.States[1], 1);
								uIButton2.States[2].spriteFrames = new CSpriteFrame[1];
								uIButton2.States[2].spriteFrames[0] = new CSpriteFrame();
								uIButton2.States[2].spriteFrames[0].uvs = rect;
								uIButton2.animations[2].SetAnim(uIButton2.States[2], 2);
								uIButton2.States[3].spriteFrames = new CSpriteFrame[1];
								uIButton2.States[3].spriteFrames[0] = new CSpriteFrame();
								uIButton2.States[3].spriteFrames[0].uvs = uvs;
								uIButton2.animations[3].SetAnim(uIButton2.States[3], 3);
							}
							else
							{
								for (int k = 0; k < 4; k++)
								{
									uIButton2.States[k].spriteFrames = new CSpriteFrame[1];
									uIButton2.States[k].spriteFrames[0] = new CSpriteFrame();
									rect.x += pixelToUVsWidth;
									if ((int)uIBaseInfoLoader.ButtonCount <= k)
									{
										uIButton2.States[k].spriteFrames[0].uvs = uvs;
									}
									else
									{
										uIButton2.States[k].spriteFrames[0].uvs = rect;
									}
									uIButton2.animations[k].SetAnim(uIButton2.States[k], k);
								}
							}
						}
						else
						{
							for (int l = 0; l < 4; l++)
							{
								uIButton2.States[l].spriteFrames = new CSpriteFrame[1];
								uIButton2.States[l].spriteFrames[0] = new CSpriteFrame();
								rect.x += pixelToUVsWidth;
								if ((int)uIBaseInfoLoader.ButtonCount <= l)
								{
									uIButton2.States[l].spriteFrames[0].uvs = uvs;
								}
								else
								{
									uIButton2.States[l].spriteFrames[0].uvs = rect;
								}
								uIButton2.animations[l].SetAnim(uIButton2.States[l], l);
							}
						}
						uIButton2.autoResize = false;
						if (string.Empty != item.GetColumnStr(i))
						{
							uIButton2.Text = item.GetColumnStr(i);
							if (this.m_faFontSize[i] > 0f)
							{
								uIButton2.SetCharacterSize(this.m_faFontSize[i]);
							}
						}
						else
						{
							uIButton2.DeleteSpriteText();
						}
						uIButton2.PlayAnim(0, 0);
						if (item.ColumnKey[i] is bool && !(bool)item.ColumnKey[i])
						{
							uIButton2.controlIsEnabled = false;
						}
						uIListItemContainer.MakeChild(uIButton2.gameObject);
						uIButton2.gameObject.transform.localPosition = new Vector3(this.columnRect[i].x, -this.columnRect[i].y, num2);
					}
				}
				else if (item.GetType(i) == ListItem.TYPE.ITEM)
				{
					GameObject gameObject5 = new GameObject(name);
					ItemTexture itemTexture = gameObject5.AddComponent<ItemTexture>();
					itemTexture.Layer = GUICamera.UILayer;
					ITEM iTEM2 = item.ColumnKey[i] as ITEM;
					if (this.useColumnRect)
					{
						itemTexture.width = this.columnRect[i].width;
						itemTexture.height = this.columnRect[i].height;
					}
					else
					{
						itemTexture.width = (float)this.columnWidth[i];
						itemTexture.height = this.lineHeight;
					}
					if (iTEM2.m_nItemUnique == 70000)
					{
						this.SetSlotEffect("FX_UI_HEARTS_STONE", itemTexture, new Vector2(itemTexture.width, itemTexture.height));
					}
					int num5 = iTEM2.m_nOption[2];
					if (string.Compare(MsgHandler.HandleReturn<string>("RankStateString", new object[]
					{
						num5
					}), "best") == 0)
					{
						this.SetSlotEffect("FX_WEAPON_GOOD", itemTexture, new Vector2(itemTexture.width, itemTexture.height));
						if (item.GetGameObjectDelegate() != null)
						{
							itemTexture.AddGameObjectDelegate(item.GetGameObjectDelegate());
						}
					}
					itemTexture.autoResize = false;
					itemTexture.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					float num6 = item.Get_Alpha(i);
					if (num6 > 0f)
					{
						itemTexture.SetAlpha(num6);
					}
					uIListItemContainer.MakeChild(itemTexture.gameObject);
					if (this.useColumnRect)
					{
						itemTexture.Setup(this.columnRect[i].width, this.columnRect[i].height);
						itemTexture.gameObject.transform.localPosition = new Vector3(this.columnRect[i].x, -this.columnRect[i].y, num2);
						BoxCollider boxCollider = (BoxCollider)itemTexture.gameObject.AddComponent(typeof(BoxCollider));
						if (boxCollider != null)
						{
							boxCollider.size = new Vector3(this.columnRect[i].width, this.columnRect[i].height, 0f);
							boxCollider.center = new Vector3(this.columnRect[i].x / 2f, -this.columnRect[i].y / 2f, 0f);
						}
					}
					else
					{
						itemTexture.Setup((float)this.columnWidth[i], this.lineHeight);
						itemTexture.gameObject.transform.localPosition = new Vector3(num, 0f, -0.02f);
						num += (float)this.columnWidth[i];
						BoxCollider boxCollider2 = (BoxCollider)itemTexture.gameObject.AddComponent(typeof(BoxCollider));
						boxCollider2.size = new Vector3((float)this.columnWidth[i], this.lineHeight, 0f);
						boxCollider2.center = new Vector3((float)(this.columnWidth[i] / 2), -this.lineHeight / 2f, num2);
					}
					itemTexture.Start();
					itemTexture.SetItemTexture(item.ColumnKey[i] as ITEM, false, true, 1f);
					if (item.p_bIsTooltip[i] && item.ColumnKey[i] is ITEM)
					{
						itemTexture.c_cItemTooltip = (ITEM)item.ColumnKey[i];
					}
					if (item.p_bIsSecondTooltip[i] && item.ColumnKey2[i] is ITEM)
					{
						itemTexture.c_cItemSecondTooltip = (ITEM)item.ColumnKey2[i];
					}
					itemTexture.AddValueChangedDelegate(item.GetColumnDelegate(i));
				}
				else if (item.GetType(i) == ListItem.TYPE.SOLDIER)
				{
					GameObject gameObject6 = new GameObject(name);
					ItemTexture itemTexture2 = gameObject6.AddComponent<ItemTexture>();
					itemTexture2.Layer = GUICamera.UILayer;
					if (this.useColumnRect)
					{
						itemTexture2.width = this.columnRect[i].width;
						itemTexture2.height = this.columnRect[i].height;
					}
					else
					{
						itemTexture2.width = (float)this.columnWidth[i];
						itemTexture2.height = this.lineHeight;
					}
					itemTexture2.autoResize = false;
					itemTexture2.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					float num7 = item.Get_Alpha(i);
					if (num7 > 0f)
					{
						itemTexture2.SetAlpha(num7);
					}
					uIListItemContainer.MakeChild(itemTexture2.gameObject);
					if (this.useColumnRect)
					{
						itemTexture2.Setup(this.columnRect[i].width, this.columnRect[i].height);
						itemTexture2.gameObject.transform.localPosition = new Vector3(this.columnRect[i].x, -this.columnRect[i].y, num2);
						BoxCollider boxCollider3 = (BoxCollider)itemTexture2.gameObject.AddComponent(typeof(BoxCollider));
						boxCollider3.size = new Vector3(this.columnRect[i].width, this.columnRect[i].height, 0f);
						boxCollider3.center = new Vector3(this.columnRect[i].x / 2f, -this.columnRect[i].y / 2f, 0f);
					}
					else
					{
						itemTexture2.Setup((float)this.columnWidth[i], this.lineHeight);
						itemTexture2.gameObject.transform.localPosition = new Vector3(num, 0f, -0.02f);
						num += (float)this.columnWidth[i];
						BoxCollider boxCollider4 = (BoxCollider)itemTexture2.gameObject.AddComponent(typeof(BoxCollider));
						boxCollider4.size = new Vector3((float)this.columnWidth[i], this.lineHeight, 0f);
						boxCollider4.center = new Vector3((float)(this.columnWidth[i] / 2), -this.lineHeight / 2f, num2);
					}
					itemTexture2.Start();
					if (item.ColumnKey[i] is int)
					{
						int charkind = (int)item.ColumnKey[i];
						itemTexture2.SetSolImageTexure(eCharImageType.SMALL, charkind, -1);
					}
					else if (item.ColumnKey[i] is NkListSolInfo)
					{
						NkListSolInfo solInfo = item.ColumnKey[i] as NkListSolInfo;
						itemTexture2.SetSolImageTexure(eCharImageType.SMALL, solInfo);
					}
				}
				else if (item.GetType(i) == ListItem.TYPE.PROGRESSBAR)
				{
					GameObject gameObject7 = new GameObject(name);
					UIProgressBar uIProgressBar = gameObject7.AddComponent<UIProgressBar>();
					uIProgressBar.gameObject.layer = GUICamera.UILayer;
					uIProgressBar.width = this.columnRect[i].width;
					uIProgressBar.height = this.columnRect[i].height;
					UIBaseInfoLoader uIBaseInfoLoader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(item.GetColumnImageStr(i));
					uIProgressBar.SetSpriteTile(uIBaseInfoLoader2.Tile, uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount, uIBaseInfoLoader2.UVs.height);
					Material material2 = CResources.Load(uIBaseInfoLoader2.Material) as Material;
					uIProgressBar.Setup(this.columnRect[i].width, this.columnRect[i].height, material2);
					Rect uvs2 = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material2, uIBaseInfoLoader2.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material2, uIBaseInfoLoader2.UVs.y + uIBaseInfoLoader2.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material2, uIBaseInfoLoader2.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material2, uIBaseInfoLoader2.UVs.height));
					uIProgressBar.States[0].spriteFrames = new CSpriteFrame[1];
					uIProgressBar.States[0].spriteFrames[0] = new CSpriteFrame();
					uIProgressBar.States[0].spriteFrames[0].uvs = uvs2;
					uIProgressBar.animations[0].SetAnim(uIProgressBar.States[0], 0);
					uIBaseInfoLoader2.Initialize();
					if (NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(item.GetColumnImageStr2(i), ref uIBaseInfoLoader2))
					{
						uvs2 = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material2, uIBaseInfoLoader2.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material2, uIBaseInfoLoader2.UVs.y + uIBaseInfoLoader2.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material2, uIBaseInfoLoader2.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material2, uIBaseInfoLoader2.UVs.height));
						uIProgressBar.States[1].spriteFrames = new CSpriteFrame[1];
						uIProgressBar.States[1].spriteFrames[0] = new CSpriteFrame();
						uIProgressBar.States[1].spriteFrames[0].uvs = uvs2;
						uIProgressBar.animations[1].SetAnim(uIProgressBar.States[1], 1);
					}
					uIProgressBar.autoResize = false;
					uIProgressBar.anchor = SpriteRoot.ANCHOR_METHOD.UPPER_LEFT;
					uIProgressBar.PlayAnim(0, 0);
					uIProgressBar.Value = (float)item.ColumnKey[i];
					uIListItemContainer.MakeChild(uIProgressBar.gameObject);
					uIProgressBar.gameObject.transform.localPosition = new Vector3(this.columnRect[i].x, -this.columnRect[i].y, num2);
				}
			}
			return uIListItemContainer;
		}

		public virtual void Clear()
		{
			this.reserveItems.Clear();
			base.ClearList(true);
			if (this.autoListBox)
			{
				for (int i = 0; i < this.maxLine; i++)
				{
					GameObject gameObject = new GameObject("ListItem");
					UIListItemContainer uIListItemContainer = gameObject.AddComponent<UIListItemContainer>();
					uIListItemContainer.isDraggable = true;
					uIListItemContainer.AutoFindOuterEdges = false;
					string backButtonName = UIScrollList.backButtonName;
					UIButton uIButton;
					if (string.Empty != this.selectStyle)
					{
						uIButton = UICreateControl.Button(backButtonName, this.selectStyle, this.viewableArea.x, this.lineHeight);
					}
					else
					{
						uIButton = UICreateControl.Button(backButtonName, "Com_B_ListBtnH", this.viewableArea.x, this.lineHeight);
					}
					uIButton.IsListButton = true;
					uIButton.allwaysPlayAnim = true;
					uIListItemContainer.MakeChild(uIButton.gameObject);
					uIListItemContainer.SetControlIsEnabled(false);
					base.InsertItemDonotPosionUpdate(uIListItemContainer, i, null, true);
				}
				base.DonotCountRepositionItems();
			}
			this.callRepositionItems = false;
			this.clipWhenMoving = true;
			this.startIndex = 0;
		}

		public virtual void SetViewArea(int count)
		{
			base.ResizeViewableArea(this.viewableArea.x, (this.lineHeight + this.itemSpacing) * (float)count);
			this._BG.SetSize(this.viewableArea.x, this.lineHeight * (float)count);
		}

		public void ResizeViewableArea()
		{
			base.ResizeViewableArea(this.viewableArea.x, this.viewableArea.y);
		}

		public virtual void SetVisible(bool value)
		{
			this.Visible = value;
			this._BG.Visible = value;
		}

		public void SetLabelScroll(float fontSize, SpriteText.Font_Effect fontEffect)
		{
			this.ColumnNum = 1;
			this.m_faFontSize[0] = fontSize;
			this.fontEffect = fontEffect;
			this.autoListBox = false;
			this.bLabelScroll = true;
			this.maxLine = 1;
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			if (boxCollider != null)
			{
				boxCollider.size = new Vector3(this.viewableArea.x, this.viewableArea.y, 0f);
				boxCollider.center = new Vector3(0f, 0f, -0.1f);
			}
		}

		public void SetLabelScroll()
		{
			this.ColumnNum = 1;
			this.autoListBox = false;
			this.bLabelScroll = true;
			this.maxLine = 1;
			for (int i = 0; i < ListBox.MAX_COLUMN_NUM; i++)
			{
				this.columnTextAnchor[i] = SpriteText.Anchor_Pos.Middle_Center;
			}
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			if (boxCollider != null)
			{
				boxCollider.size = new Vector3(this.viewableArea.x, this.viewableArea.y, 0f);
				boxCollider.center = new Vector3(0f, 0f, -0.1f);
			}
		}

		public void SetSlotEffect(string effectKey, AutoSpriteControlBase obj, Vector2 size)
		{
			if (obj == null)
			{
				return;
			}
			NrTSingleton<FormsManager>.Instance.AttachEffectKey(effectKey, obj, size);
		}

		public override void Update()
		{
			int num;
			if (this.orientation == UIScrollList.ORIENTATION.VERTICAL)
			{
				num = (int)this.viewableArea.y;
			}
			else if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
			{
				num = (int)this.viewableArea.x;
			}
			else
			{
				num = (int)this.viewableArea.y;
			}
			if (this.m_bReserve && (float)this.startIndex <= (float)num / this.lineHeight + 1f)
			{
				if (Time.realtimeSinceStartup - this.checkTime > 0.01f)
				{
					if (this.reserveItems.Count > 0)
					{
						ListItem listItem = this.reserveItems.Dequeue();
						if (listItem != null)
						{
							this.MakeContainer(listItem);
						}
					}
					this.checkTime = Time.realtimeSinceStartup;
				}
				if (!this.callRepositionItems && (float)this.startIndex >= (float)num / this.lineHeight + 1f)
				{
					this.RepositionItems();
					this.callRepositionItems = true;
				}
				else if (!this.callRepositionItems && this.reserveItems.Count == 0)
				{
					this.RepositionItems();
					this.callRepositionItems = true;
				}
			}
			base.Update();
		}

		protected override void ReserveMakeItem()
		{
			int num;
			if (this.orientation == UIScrollList.ORIENTATION.VERTICAL)
			{
				num = (int)this.viewableArea.y;
			}
			else if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
			{
				num = (int)this.viewableArea.x;
			}
			else
			{
				num = (int)this.viewableArea.y;
			}
			for (int i = 0; i < (int)((float)num / this.lineHeight) / 2 + 1; i++)
			{
				if (this.reserveItems.Count > 0)
				{
					ListItem listItem = this.reserveItems.Dequeue();
					if (listItem != null)
					{
						this.RemoveAdd(this.items.Count + i, listItem);
					}
				}
			}
			this.RepositionItems();
		}

		private void MakeContainer(ListItem item)
		{
			UIListItemContainer uIListItemContainer = this.CreateContainer(item);
			if (null == uIListItemContainer)
			{
				return;
			}
			uIListItemContainer.Start();
			uIListItemContainer.SetControlIsEnabled(item.enable);
			uIListItemContainer.transform.position = Vector3.zero;
			if (null != base.GetItem(this.startIndex))
			{
				base.RemoveItemDonotPositionUpdate(this.startIndex, true);
				base.InsertItemDonotPosionUpdate(uIListItemContainer, this.startIndex, null, this.m_bReserve);
			}
			else
			{
				base.InsertItemDonotPosionUpdate(uIListItemContainer, this.startIndex, null, this.m_bReserve);
			}
			this.startIndex++;
		}
	}
}
