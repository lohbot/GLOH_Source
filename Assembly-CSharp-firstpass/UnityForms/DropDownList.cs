using GAME;
using GameMessage;
using System;
using UnityEngine;

namespace UnityForms
{
	public class DropDownList : ListBox
	{
		public ListBox _ListBox;

		private UIButton _Button;

		public UIButton Button
		{
			get
			{
				return this._Button;
			}
			set
			{
				this._Button = value;
			}
		}

		public ListBox ListBox
		{
			get
			{
				return this._ListBox;
			}
			set
			{
				this._ListBox = value;
			}
		}

		public float DefaultHeight
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

		public override int ColumnNum
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
				this._ListBox.ColumnNum = value;
			}
		}

		public override bool UseColumnRect
		{
			set
			{
				base.UseColumnRect = value;
				this._ListBox.UseColumnRect = value;
			}
		}

		public override float OffsetX
		{
			set
			{
				base.OffsetX = value;
				this._ListBox.OffsetX = value;
			}
		}

		public new static DropDownList Create(string name, Vector3 pos)
		{
			return (DropDownList)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(DropDownList));
		}

		public void Add(string name)
		{
			ListItem listItem = new ListItem();
			listItem.SetColumnStr(0, name);
			this.Add(listItem);
		}

		public override void Add(ListItem item)
		{
			GameObject gameObject = new GameObject("DropDownListItem" + base.Count.ToString());
			UIListItemContainer uIListItemContainer = gameObject.AddComponent<UIListItemContainer>();
			uIListItemContainer.Data = item;
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
			uIButton.EffectAni = false;
			uIButton.IsListButton = true;
			uIButton.allwaysPlayAnim = true;
			uIListItemContainer.MakeChild(uIButton.gameObject);
			uIButton.gameObject.transform.localPosition = Vector3.zero;
			float num = this._offsetX;
			float num2 = -0.02f;
			for (int i = 0; i < this.currentColumnNum; i++)
			{
				string name = this.columnTitle + i.ToString();
				GameObject gameObject2 = new GameObject(name);
				num2 -= 0.01f;
				if (item.GetType(i) == ListItem.TYPE.IMAGE)
				{
					DrawTexture drawTexture = gameObject2.AddComponent<DrawTexture>();
					drawTexture.gameObject.layer = GUICamera.UILayer;
					if (item.ColumnKey[i] is ITEM)
					{
						drawTexture.c_cItemTooltip = (ITEM)item.ColumnKey[i];
					}
					else if (item.ColumnKey[i] is long)
					{
						drawTexture.nItemUniqueTooltip = (int)item.ColumnKey[i];
					}
					if (this.useColumnRect)
					{
						drawTexture.width = this.columnRect[i].width;
						drawTexture.height = this.columnRect[i].height;
					}
					else
					{
						drawTexture.width = (float)this.columnWidth[i];
						drawTexture.height = this.lineHeight;
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
					uIListItemContainer.MakeChild(drawTexture.gameObject);
					if (this.useColumnRect)
					{
						drawTexture.gameObject.transform.localPosition = new Vector3(this.columnRect[i].x, -this.columnRect[i].y, num2);
						BoxCollider boxCollider = (BoxCollider)drawTexture.gameObject.AddComponent(typeof(BoxCollider));
						boxCollider.size = new Vector3(this.columnRect[i].width, this.columnRect[i].height, 0f);
						boxCollider.center = new Vector3(this.columnRect[i].x / 2f, -this.columnRect[i].y / 2f, 0f);
					}
					else
					{
						drawTexture.gameObject.transform.localPosition = new Vector3(num, 0f, -0.02f);
						num += (float)this.columnWidth[i];
						BoxCollider boxCollider2 = (BoxCollider)drawTexture.gameObject.AddComponent(typeof(BoxCollider));
						boxCollider2.size = new Vector3((float)this.columnWidth[i], this.lineHeight, 0f);
						boxCollider2.center = new Vector3((float)(this.columnWidth[i] / 2), -this.lineHeight / 2f, num2);
					}
				}
				else if (item.GetType(i) == ListItem.TYPE.TEXT)
				{
					Label label = gameObject2.AddComponent<Label>();
					label.gameObject.layer = GUICamera.UILayer;
					label.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					label.Data = item.ColumnKey[i];
					float num3;
					float y;
					if (this.useColumnRect)
					{
						label.width = this.columnRect[i].width;
						label.height = this.columnRect[i].height;
						num3 = this.columnRect[i].x;
						y = -1f * this.columnRect[i].y;
					}
					else
					{
						num3 = num;
						y = 0f;
						label.width = (float)this.columnWidth[i];
						label.height = this.lineHeight;
						num += (float)this.columnWidth[i];
					}
					if (this.bUseSpotLabel)
					{
						label.MaxWidth = (float)this.columnWidth[i];
						label.multiLine = false;
					}
					label.Text = item.GetColumnStr(i);
					if (this.bUseSpotLabel)
					{
						label.SPOT = true;
					}
					label.SetFontEffect(base.FontEffect);
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
							if (i == 0)
							{
								num3 += 6f;
							}
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
					label.gameObject.transform.localPosition = new Vector3(num3, y, num2);
				}
				else if (item.GetType(i) == ListItem.TYPE.BUTTON)
				{
					UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(item.GetColumnImageStr(i));
					if (uIBaseInfoLoader != null)
					{
						UIButton uIButton2 = gameObject2.AddComponent<UIButton>();
						uIButton2.EffectAni = false;
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
						Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
						uIButton2.Setup(this.columnRect[i].width, this.columnRect[i].height, material);
						float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
						Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
						Rect uvs = new Rect(rect);
						uvs.x += pixelToUVsWidth;
						for (int j = 0; j < 4; j++)
						{
							uIButton2.States[j].spriteFrames = new CSpriteFrame[1];
							uIButton2.States[j].spriteFrames[0] = new CSpriteFrame();
							rect.x += pixelToUVsWidth;
							if ((int)uIBaseInfoLoader.ButtonCount <= j)
							{
								uIButton2.States[j].spriteFrames[0].uvs = uvs;
							}
							else
							{
								uIButton2.States[j].spriteFrames[0].uvs = rect;
							}
							uIButton2.animations[j] = new UVAnimation();
							uIButton2.animations[j].SetAnim(uIButton2.States[j], j);
						}
						uIButton2.autoResize = false;
						uIButton2.Text = item.GetColumnStr(i);
						if (this.m_faFontSize[i] > 0f)
						{
							uIButton2.spriteText.SetCharacterSize(this.m_faFontSize[i]);
						}
						uIButton2.PlayAnim(0, 0);
						uIListItemContainer.MakeChild(uIButton2.gameObject);
						uIButton2.gameObject.transform.localPosition = new Vector3(this.columnRect[i].x, -this.columnRect[i].y, num2);
					}
				}
			}
			uIListItemContainer.transform.position = Vector3.zero;
			if (base.GetItem(this.startIndex) != null)
			{
				base.RemoveItemDonotPositionUpdate(this.startIndex, true);
				base.InsertItemDonotPosionUpdate(uIListItemContainer, this.startIndex, null, true);
			}
			else
			{
				base.InsertItemDonotPosionUpdate(uIListItemContainer, this.startIndex, null, true);
			}
			this.startIndex++;
		}

		public void SetFirstItem()
		{
			this.SetIndex(0);
		}

		public object GetSelectItemData()
		{
			if (null == base.SelectedItem)
			{
				return null;
			}
			return base.SelectedItem.data;
		}

		public void SetIndex(int index)
		{
			base.SetSelectedItem(index);
			if (null != base.SelectedItem)
			{
				this._ListBox.Clear();
				ListItem item = base.SelectedItem.Data as ListItem;
				this._ListBox.Add(item);
				this._ListBox.DonotCountRepositionItems();
			}
			else
			{
				this._ListBox.Clear();
				ListItem listItem = new ListItem();
				listItem.SetColumnStr(0, " ");
				this._ListBox.Add(listItem);
				this._ListBox.DonotCountRepositionItems();
			}
			if (this.slider)
			{
				this.slider.gameObject.SetActive(false);
			}
		}

		public override void Clear()
		{
			if (this.items != null)
			{
				base.ClearList(true);
				this._ListBox.ClearList(true);
			}
		}

		public override void SetViewArea(int line)
		{
			this.viewableArea.y = this.lineHeight * (float)line;
			Vector3 zero = Vector3.zero;
			zero.x = base.transform.localPosition.x;
			zero.y = this._ListBox.transform.localPosition.y - (this.viewableArea.y / 2f + this.lineHeight * 0.5f);
			zero.z = base.transform.localPosition.z - 1f;
			base.transform.localPosition = zero;
			if (null != base.BG)
			{
				Vector3 zero2 = Vector3.zero;
				zero2.x = base.BG.transform.localPosition.x;
				zero2.y = this._ListBox.transform.localPosition.y - this.lineHeight * 0.5f;
				zero2.z = base.transform.localPosition.z + 0.05f;
				base.BG.transform.localPosition = zero2;
				base.BG.height = this.viewableArea.y;
				base.BG.Setup(base.BG.width, base.BG.height);
			}
			this.clipWhenMoving = true;
		}

		public void SetHideList()
		{
			this.bShow = false;
			this.Visible = false;
			if (null != base.BG)
			{
				base.BG.Visible = false;
			}
		}

		public void SetOpenList(bool show)
		{
			this.bShow = show;
			if (TsPlatform.IsMobile)
			{
				if (show)
				{
					MsgHandler.Handle("ShowDropDownDLG", new object[]
					{
						this
					});
				}
				else
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MOBILE_DROPDOWN_DLG);
				}
			}
			else
			{
				this.Visible = this.bShow;
				if (null != base.BG)
				{
					if (this.viewableArea.y == 0f)
					{
						base.BG.Visible = false;
					}
					else
					{
						base.BG.Visible = this.bShow;
					}
				}
				this.clipWhenMoving = true;
			}
		}

		public override void SetVisible(bool show)
		{
			this.Visible = false;
			if (null != base.BG)
			{
				base.BG.Visible = false;
			}
			this._ListBox.Visible = show;
			if (null != this._ListBox.BG)
			{
				this._ListBox.BG.Visible = show;
			}
			if (this._ListBox.slider)
			{
				this._ListBox.slider.Visible = false;
			}
			if (show)
			{
				this._ListBox.clipWhenMoving = true;
			}
		}

		public void SetFunction()
		{
			this._ListBox.AddValueChangedDelegate(new EZValueChangedDelegate(this.ShowDropDownList));
			this.AddValueChangedDelegate(new EZValueChangedDelegate(this.SelectItems));
		}

		public void ShowDropDownList(IUIObject obj)
		{
			this.bShow = !this.bShow;
			this.SetOpenList(this.bShow);
			if (this.bShow)
			{
			}
		}

		public void SelectItems(IUIObject obj)
		{
			ListItem item = base.SelectedItem.Data as ListItem;
			this._ListBox.Clear();
			this._ListBox.Add(item);
			this._ListBox.DonotCountRepositionItems();
			this.SetHideList();
		}

		public void SelectItems(ListItem item)
		{
			this._ListBox.Clear();
			this._ListBox.Add(item);
			this._ListBox.DonotCountRepositionItems();
			this.SetHideList();
		}

		public void Check(IUIObject obj)
		{
			Transform child = base.transform.GetChild(0);
			if (child.transform.FindChild(obj.name))
			{
			}
		}

		public override void SetColumnWidth(int size0, int size1, int size2, int size3, int size4)
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
			this._ListBox.SetColumnWidth(size0, size1, size2, size3, size4);
		}

		public override void SetColumnAlignment(int index, SpriteText.Anchor_Pos anchor)
		{
			if (0 > index && ListBox.MAX_COLUMN_NUM <= index)
			{
				return;
			}
			this.columnTextAnchor[index] = anchor;
			this._ListBox.SetColumnAlignment(index, anchor);
		}

		public override void SetColumnRect(int index, int x, int y, int width, int height)
		{
			this.SetColumnRect(index, x, y, width, height, SpriteText.Anchor_Pos.Middle_Center);
			this._ListBox.SetColumnRect(index, x, y, width, height, SpriteText.Anchor_Pos.Middle_Center);
		}

		public override void SetColumnRect(int index, int x, int y, int width, int height, SpriteText.Anchor_Pos anchor)
		{
			if (0 <= index && ListBox.MAX_COLUMN_NUM > index)
			{
				this.columnRect[index] = new Rect((float)x, (float)y, (float)width, (float)height);
			}
			this.columnTextAnchor[index] = anchor;
			this._ListBox.SetColumnRect(index, x, y, width, height, anchor);
		}

		public override void SetColumnRect(int index, int x, int y, int width, int height, SpriteText.Anchor_Pos anchor, float a_fFonSize)
		{
			if (0 <= index && ListBox.MAX_COLUMN_NUM > index)
			{
				this.columnRect[index] = new Rect((float)x, (float)y, (float)width, (float)height);
			}
			this.columnTextAnchor[index] = anchor;
			this.m_faFontSize[index] = a_fFonSize;
			this._ListBox.SetColumnRect(index, x, y, width, height, anchor, a_fFonSize);
		}
	}
}
