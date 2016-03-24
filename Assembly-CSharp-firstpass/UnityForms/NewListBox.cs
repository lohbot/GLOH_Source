using GAME;
using GameMessage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityForms
{
	public class NewListBox : UIScrollList
	{
		public enum TYPE
		{
			LABEL,
			FLASHLABEL,
			BUTTON,
			DRAWTEXTURE,
			ITEMTEXTURE,
			CHECKBOX,
			TOGGLE,
			TEXTFIELD,
			SLIDER,
			SCROLLLABEL
		}

		private class ColumnData
		{
			public NewListBox.TYPE type;

			public string styleName1 = string.Empty;

			public string styleName2 = string.Empty;

			public string styleName3 = string.Empty;

			public int x;

			public int y;

			public int width;

			public int height;

			public SpriteText.Font_Effect fontEffect = SpriteText.Font_Effect.Black_Shadow_Small;

			public SpriteText.Anchor_Pos anchor = SpriteText.Anchor_Pos.Middle_Left;

			public float fontSize = 15f;

			public string fontColor = string.Empty;

			public bool multiLine;

			public string imageInverse = string.Empty;

			public float alpha = 1f;

			public bool useBox = true;
		}

		private DrawTexture _BG;

		private bool useBackButton = true;

		private string selectStyle = string.Empty;

		private int currentColumnNum;

		private bool changeLineHeight;

		private bool bShowLevel;

		private bool bShowEventMark;

		private bool bShowCombat;

		private List<NewListBox.ColumnData> coulmnDataList = new List<NewListBox.ColumnData>();

		private Queue<NewListItem> reserveItems = new Queue<NewListItem>();

		private int startIndex;

		private bool autoListBox;

		private float checkTime;

		private int m_nMaxNum = -1;

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

		public string SelectStyle
		{
			set
			{
				this.selectStyle = value;
			}
		}

		public int ColumnNum
		{
			get
			{
				return this.currentColumnNum;
			}
			set
			{
				this.currentColumnNum = value;
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

		public bool ChangeLineHeight
		{
			get
			{
				return this.changeLineHeight;
			}
			set
			{
				this.changeLineHeight = value;
			}
		}

		public bool ShowLevel
		{
			get
			{
				return this.bShowLevel;
			}
			set
			{
				this.bShowLevel = value;
			}
		}

		public bool ShowEventMark
		{
			get
			{
				return this.bShowEventMark;
			}
			set
			{
				this.bShowEventMark = value;
			}
		}

		public bool ShowCombat
		{
			get
			{
				return this.bShowCombat;
			}
			set
			{
				this.bShowCombat = value;
			}
		}

		public bool AutoListBox
		{
			set
			{
				this.autoListBox = value;
			}
		}

		public int MaxNum
		{
			get
			{
				this.CalculateMaxContainerNum();
				return this.m_nMaxNum;
			}
			set
			{
				this.m_nMaxNum = value;
			}
		}

		public new static NewListBox Create(string name, Vector3 pos)
		{
			return (NewListBox)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(NewListBox));
		}

		public void SetColumnData(string path)
		{
			this.coulmnDataList.Clear();
			TextAsset textAsset = (TextAsset)CResources.Load(path);
			if (null != textAsset)
			{
				char[] separator = new char[]
				{
					'\n'
				};
				char[] separator2 = new char[]
				{
					','
				};
				string[] array = textAsset.text.Split(separator);
				int num = 0;
				for (int i = 1; i < array.Length; i++)
				{
					if (0 < array[i].Length)
					{
						if (!(string.Empty == array[i]))
						{
							string[] array2 = array[i].Split(separator2, 40);
							this.SetColumnData(array2[1], array2[7], array2[8], array2[9], array2[3], array2[4], array2[5], array2[6], array2[14], array2[16], array2[17], array2[12], array2[23], array2[31], array2[20], UIBaseFileManager.GetFontEffect(array2[25]));
							num++;
						}
					}
				}
				this.ColumnNum = num;
			}
			Resources.UnloadAsset(textAsset);
			CResources.Delete(path);
		}

		public void SetColumnData(string controlType, string styleName1, string styleName2, string styleName3, string x, string y, string width, string height, string anchor, string fontSize, string fontColor, string multiLine, string imageInverse, string alpha, string box, SpriteText.Font_Effect fontEffect)
		{
			NewListBox.ColumnData columnData = new NewListBox.ColumnData();
			if ("Label" == controlType)
			{
				columnData.type = NewListBox.TYPE.LABEL;
			}
			else if ("FlashLabel" == controlType)
			{
				columnData.type = NewListBox.TYPE.FLASHLABEL;
			}
			else if ("Button" == controlType)
			{
				columnData.type = NewListBox.TYPE.BUTTON;
			}
			else if ("DrawTexture" == controlType)
			{
				columnData.type = NewListBox.TYPE.DRAWTEXTURE;
			}
			else if ("ItemTexture" == controlType)
			{
				columnData.type = NewListBox.TYPE.ITEMTEXTURE;
			}
			else if ("CheckBox" == controlType)
			{
				columnData.type = NewListBox.TYPE.CHECKBOX;
			}
			else if ("Toggle" == controlType)
			{
				columnData.type = NewListBox.TYPE.TOGGLE;
			}
			else if ("TextField" == controlType)
			{
				columnData.type = NewListBox.TYPE.TEXTFIELD;
			}
			else if ("HSlider" == controlType)
			{
				columnData.type = NewListBox.TYPE.SLIDER;
			}
			else if ("ScrollLabel" == controlType)
			{
				columnData.type = NewListBox.TYPE.SCROLLLABEL;
			}
			if (string.Empty != styleName1)
			{
				columnData.styleName1 = styleName1;
			}
			if (string.Empty != styleName2)
			{
				columnData.styleName2 = styleName2;
			}
			if (string.Empty != styleName3)
			{
				columnData.styleName3 = styleName3;
			}
			columnData.x = int.Parse(x);
			columnData.y = int.Parse(y);
			columnData.width = int.Parse(width);
			columnData.height = int.Parse(height);
			if ("UpperLeft" == anchor)
			{
				columnData.anchor = SpriteText.Anchor_Pos.Upper_Left;
			}
			else if ("UpperCenter" == anchor)
			{
				columnData.anchor = SpriteText.Anchor_Pos.Upper_Center;
			}
			else if ("UpperRight" == anchor)
			{
				columnData.anchor = SpriteText.Anchor_Pos.Upper_Right;
			}
			else if ("MiddleLeft" == anchor)
			{
				columnData.anchor = SpriteText.Anchor_Pos.Middle_Left;
			}
			else if ("MiddleCenter" == anchor)
			{
				columnData.anchor = SpriteText.Anchor_Pos.Middle_Center;
			}
			else if ("MiddleRight" == anchor)
			{
				columnData.anchor = SpriteText.Anchor_Pos.Middle_Right;
			}
			else if ("LowerLeft" == anchor)
			{
				columnData.anchor = SpriteText.Anchor_Pos.Lower_Left;
			}
			else if ("LowerCenter" == anchor)
			{
				columnData.anchor = SpriteText.Anchor_Pos.Lower_Center;
			}
			else if ("LowerRight" == anchor)
			{
				columnData.anchor = SpriteText.Anchor_Pos.Lower_Right;
			}
			columnData.fontEffect = fontEffect;
			columnData.fontSize = float.Parse(fontSize);
			columnData.fontColor = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
			{
				fontColor
			});
			columnData.multiLine = (multiLine == "1");
			columnData.imageInverse = imageInverse;
			columnData.useBox = (box == "1");
			columnData.alpha = float.Parse(alpha) / 100f;
			this.coulmnDataList.Add(columnData);
		}

		public void SetColumnSize(int index, int width, int height)
		{
			if (this.coulmnDataList.Count > index)
			{
				NewListBox.ColumnData columnData = this.coulmnDataList[index];
				columnData.width = width;
				columnData.height = height;
			}
		}

		public void SetColumnCenter()
		{
			NewListBox.ColumnData columnData = this.coulmnDataList[0];
			int num = ((int)this.viewableArea.x - columnData.width) / 2;
			int num2 = ((int)this.lineHeight - columnData.height) / 2;
			for (int i = 0; i < this.coulmnDataList.Count; i++)
			{
				this.coulmnDataList[i].x += num;
				this.coulmnDataList[i].y += num2;
			}
		}

		public virtual void Clear()
		{
			this.scrollPos = 0f;
			this.scrollDelta = 0f;
			this.callRepositionItems = false;
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
					uIButton.EffectAni = false;
					uIButton.IsListButton = true;
					uIButton.allwaysPlayAnim = true;
					uIListItemContainer.MakeChild(uIButton.gameObject);
					uIListItemContainer.SetControlIsEnabled(false);
					base.InsertItemDonotPosionUpdate(uIListItemContainer, i, null, true);
					if (!this.useBackButton)
					{
						uIButton.gameObject.SetActive(false);
					}
				}
				base.DonotCountRepositionItems();
			}
			this.clipWhenMoving = true;
			this.startIndex = 0;
		}

		public void BackButtonAniEnable(bool enable)
		{
			this.useBackButton = enable;
		}

		private UIListItemContainer CreateContainer(NewListItem item)
		{
			GameObject gameObject = new GameObject("ListItem");
			UIListItemContainer uIListItemContainer = gameObject.AddComponent<UIListItemContainer>();
			if (null == uIListItemContainer)
			{
				return null;
			}
			uIListItemContainer.transform.position = Vector3.zero;
			uIListItemContainer.Start();
			uIListItemContainer.Data = item.Data;
			uIListItemContainer.isDraggable = true;
			uIListItemContainer.AutoFindOuterEdges = false;
			string backButtonName = UIScrollList.backButtonName;
			UIButton uIButton = null;
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
			uIButton.EffectAni = false;
			uIButton.IsListButton = true;
			uIButton.allwaysPlayAnim = true;
			uIListItemContainer.MakeChild(uIButton.gameObject);
			uIButton.gameObject.transform.localPosition = Vector3.zero;
			if (!this.useBackButton)
			{
				uIButton.gameObject.SetActive(false);
			}
			float num = 0f;
			for (int i = 0; i < this.coulmnDataList.Count; i++)
			{
				NewListBox.ColumnData columnData = this.coulmnDataList[i];
				if (columnData != null)
				{
					NewListItem.NewListItemData data = item.GetData(i);
					if (data != null)
					{
						num -= 0.02f;
						if (columnData.type == NewListBox.TYPE.LABEL)
						{
							string str = (string)data.realData;
							Label label = UICreateControl.Label(i.ToString() + "text", str, columnData.multiLine, (float)columnData.width, (float)columnData.height, columnData.fontSize, columnData.fontEffect, columnData.anchor, columnData.fontColor);
							if (null != label)
							{
								label.Data = data.data;
								uIListItemContainer.MakeChild(label.gameObject);
								label.gameObject.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), num);
								label.Visible = data.visible;
								label.controlIsEnabled = data.enable;
								if (columnData.alpha != 1f)
								{
									label.SetAlpha(columnData.alpha);
								}
							}
						}
						else if (columnData.type == NewListBox.TYPE.FLASHLABEL)
						{
							string str2 = (string)data.realData;
							GameObject gameObject2 = new GameObject(i.ToString() + "FlashLabel");
							UIListItemContainer uIListItemContainer2 = gameObject2.AddComponent<UIListItemContainer>();
							EmoticonInfo.ParseEmoticonFlashLabel(ref uIListItemContainer2, str2, (float)columnData.width, (float)columnData.height, (int)columnData.fontSize, 1.1f, SpriteText.Anchor_Pos.Upper_Left, MsgHandler.HandleReturn<string>("GetTextColor", new object[]
							{
								"1101"
							}));
							if (null != uIListItemContainer2)
							{
								uIListItemContainer2.name = i.ToString() + "FlashLabel";
								uIListItemContainer2.Data = data.data;
								uIListItemContainer.MakeChild(uIListItemContainer2.gameObject);
								uIListItemContainer2.gameObject.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), num);
								if (this.changeLineHeight)
								{
									uIListItemContainer2.FindOuterEdges();
									float num2 = Mathf.Abs(uIListItemContainer2.BottomRightEdge().y - uIListItemContainer2.TopLeftEdge().y);
									this.lineHeight += num2;
									uIButton.SetSize(this.viewableArea.x, this.lineHeight);
								}
								uIListItemContainer2.Visible = data.visible;
								uIListItemContainer2.SetControlIsEnabled(data.enable);
								if (columnData.alpha != 1f)
								{
									uIListItemContainer2.SetAlpha(columnData.alpha);
								}
							}
						}
						else if (columnData.type == NewListBox.TYPE.BUTTON)
						{
							UIButton uIButton2 = UICreateControl.Button(i.ToString() + "button", columnData.styleName1, (float)columnData.width, (float)columnData.height);
							if (null != uIButton2)
							{
								if (data.realData is string)
								{
									uIButton2.Text = (string)data.realData;
								}
								else if (data.realData is UIBaseInfoLoader)
								{
									uIButton2.SetButtonTextureKey((UIBaseInfoLoader)data.realData);
									if (data.data2 is string)
									{
										uIButton2.Text = (string)data.data2;
									}
								}
								if (null != uIButton2.spriteText)
								{
									uIButton2.SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
									uIButton2.SetAlignment(SpriteText.Alignment_Type.Center);
									uIButton2.SetFontEffect(columnData.fontEffect);
									uIButton2.SetCharacterSize(columnData.fontSize);
								}
								if (string.Empty != columnData.imageInverse)
								{
									uIButton2.Inverse(UIBaseFileManager.GetInverse(columnData.imageInverse));
								}
								uIButton2.Data = data.data;
								uIButton2.AddValueChangedDelegate(data.eventDelegate);
								uIButton2.AddMouseDownDelegate(data.downDelegate);
								uIListItemContainer.MakeChild(uIButton2.gameObject);
								uIButton2.gameObject.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), num);
								uIButton2.Visible = data.visible;
								uIButton2.controlIsEnabled = data.enable;
								if (columnData.alpha != 1f)
								{
									uIButton2.SetAlpha(columnData.alpha);
								}
							}
						}
						else if (columnData.type == NewListBox.TYPE.DRAWTEXTURE)
						{
							if (data.data is float)
							{
								float num3 = (float)data.data;
								columnData.width = (int)num3;
							}
							DrawTexture drawTexture = UICreateControl.DrawTexture(i.ToString() + "drawtexture", columnData.styleName1, (float)columnData.width, (float)columnData.height, columnData.useBox);
							if (null != drawTexture)
							{
								if (data.realData is UIBaseInfoLoader)
								{
									UIBaseInfoLoader texture = (UIBaseInfoLoader)data.realData;
									drawTexture.SetTexture(texture);
								}
								else if (data.realData is ITEM)
								{
									ITEM iTEM = (ITEM)data.realData;
									if (iTEM.m_nItemUnique == 70000)
									{
										NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_HEARTS_STONE", drawTexture, new Vector2(drawTexture.width, drawTexture.height));
									}
									int num4 = iTEM.m_nOption[2];
									if (string.Compare(MsgHandler.HandleReturn<string>("RankStateString", new object[]
									{
										num4
									}), "best") == 0)
									{
										NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_WEAPON_GOOD", drawTexture, new Vector2(drawTexture.width, drawTexture.height));
									}
									drawTexture.SetTexture(MsgHandler.HandleReturn<UIBaseInfoLoader>("GetItemTexture", new object[]
									{
										iTEM.m_nItemUnique
									}));
								}
								else if (data.realData is int)
								{
									int num5 = (int)data.realData;
									int num6 = -1;
									if (data.data is int)
									{
										num6 = (int)data.data;
									}
									if ("false" == MsgHandler.HandleReturn<string>("IsReincarnation", new object[0]) && "true" == MsgHandler.HandleReturn<string>("CharKindIsATB", new object[]
									{
										num5,
										1L
									}))
									{
										num6 = -1;
									}
									if (item.EventMark)
									{
										if (0 < num6)
										{
											drawTexture.SetTextureEvent(eCharImageType.SMALL, num5, num6);
										}
										else
										{
											drawTexture.SetTextureEvent(eCharImageType.SMALL, num5, -1);
										}
									}
									else if (0 < num6)
									{
										drawTexture.SetTexture(eCharImageType.SMALL, num5, num6, string.Empty);
									}
									else if (data.data is bool)
									{
										drawTexture.SetTexture(eCharImageType.MIDDLE, num5, -1, string.Empty);
									}
									else
									{
										drawTexture.SetTexture(eCharImageType.SMALL, num5, -1, string.Empty);
									}
								}
								else if (data.realData is string)
								{
									if (data.data != null)
									{
										if (data.data is bool)
										{
											bool flag = (bool)data.data;
											if (flag)
											{
												drawTexture.SetTextureFromBundle((string)data.realData);
											}
											else
											{
												drawTexture.SetTexture((string)data.realData);
											}
										}
										else
										{
											drawTexture.SetTexture((string)data.realData);
										}
									}
									else
									{
										drawTexture.SetTexture((string)data.realData);
									}
								}
								else if (data.realData is Texture2D)
								{
									drawTexture.SetTexture2D((Texture2D)data.realData);
								}
								else if (data.realData is CostumeDrawTextureInfo)
								{
									CostumeDrawTextureInfo costumeDrawTextureInfo = data.realData as CostumeDrawTextureInfo;
									drawTexture.SetTexture(costumeDrawTextureInfo.imageType, costumeDrawTextureInfo.charKind, costumeDrawTextureInfo.grade, costumeDrawTextureInfo.costumePortraitPath);
								}
								if (data.data2 is string)
								{
									string path = string.Format("{0}{1}", (string)data.data2, NrTSingleton<UIDataManager>.Instance.AddFilePath);
									NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, drawTexture, new Vector2(drawTexture.width, drawTexture.height));
								}
								Vector3 vector = Vector3.zero;
								Vector3 localScale = Vector3.one;
								if (string.Empty != columnData.imageInverse)
								{
									drawTexture.gameObject.transform.localPosition = Vector3.zero;
									vector = drawTexture.Inverse(UIBaseFileManager.GetInverse(columnData.imageInverse));
									localScale = drawTexture.gameObject.transform.localScale;
								}
								drawTexture.Data = data.data;
								drawTexture.AddValueChangedDelegate(data.eventDelegate);
								drawTexture.AddMouseDownDelegate(data.downDelegate);
								uIListItemContainer.MakeChild(drawTexture.gameObject);
								if (UIBaseFileManager.GetInverse(columnData.imageInverse) == INVERSE_MODE.LEFT_TO_RIGHT)
								{
									drawTexture.gameObject.transform.localScale = localScale;
									drawTexture.gameObject.transform.localPosition = new Vector3((float)columnData.x + vector.x, (float)(-(float)columnData.y) + vector.y, num);
								}
								else
								{
									drawTexture.gameObject.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), num);
								}
								drawTexture.Visible = data.visible;
								drawTexture.controlIsEnabled = data.enable;
								if (columnData.alpha != 1f)
								{
									drawTexture.SetAlpha(columnData.alpha);
								}
							}
						}
						else if (columnData.type == NewListBox.TYPE.ITEMTEXTURE)
						{
							ItemTexture itemTexture = UICreateControl.ItemTexture(i.ToString() + "itemtexture", (float)columnData.width, (float)columnData.height, columnData.useBox);
							if (null != itemTexture)
							{
								if (data.realData is UIBaseInfoLoader)
								{
									UIBaseInfoLoader texture2 = (UIBaseInfoLoader)data.realData;
									itemTexture.SetTexture(texture2);
								}
								else if (data.realData is ITEM)
								{
									ITEM iTEM2 = (ITEM)data.realData;
									if (data.data != null && data.data is string)
									{
										if ("Material" == (string)data.data)
										{
											itemTexture.SetItemTexture(iTEM2, false, false, 1f);
										}
										else
										{
											itemTexture.SetItemTexture(iTEM2);
										}
									}
									else
									{
										itemTexture.SetItemTexture(iTEM2);
									}
									if (data.data2 != null)
									{
										ITEM c_cItemTooltip = (ITEM)data.data2;
										itemTexture.c_cItemTooltip = c_cItemTooltip;
										itemTexture.c_cItemSecondTooltip = iTEM2;
									}
									else
									{
										itemTexture.c_cItemTooltip = iTEM2;
									}
								}
								else if (data.realData is int)
								{
									int charkind = (int)data.realData;
									if (data.data is int)
									{
										int level = (int)data.data;
										itemTexture.SetSolImageTexure(eCharImageType.SMALL, charkind, -1, level);
									}
									else
									{
										itemTexture.SetSolImageTexure(eCharImageType.SMALL, charkind, -1);
									}
								}
								else if (data.realData is NkListSolInfo)
								{
									NkListSolInfo solInfo = (NkListSolInfo)data.realData;
									bool flag2 = false;
									if (data.data != null && data.data is bool)
									{
										flag2 = (bool)data.data;
									}
									itemTexture.SetSolImageTexure(eCharImageType.SMALL, solInfo, flag2);
								}
								else if (data.realData is string)
								{
									if (data.data != null)
									{
										if (data.data is bool)
										{
											bool flag3 = (bool)data.data;
											if (flag3)
											{
												itemTexture.SetTextureFromBundle((string)data.realData);
											}
											else
											{
												itemTexture.SetTexture((string)data.realData);
											}
										}
										else
										{
											itemTexture.SetTexture((string)data.realData);
										}
									}
									else
									{
										itemTexture.SetTexture((string)data.realData);
									}
								}
								else if (data.realData is Texture2D)
								{
									if (data.data2 != null)
									{
										short nLevel = (short)data.data2;
										itemTexture.SetEventImageTexure((Texture2D)data.realData, nLevel, this.bShowEventMark);
									}
									else
									{
										itemTexture.SetEventImageTexure((Texture2D)data.realData, 0, this.bShowEventMark);
									}
								}
								if (string.Empty != columnData.imageInverse)
								{
									itemTexture.Inverse(UIBaseFileManager.GetInverse(columnData.imageInverse));
								}
								itemTexture.Data = data.data;
								itemTexture.AddValueChangedDelegate(data.eventDelegate);
								itemTexture.AddMouseDownDelegate(data.downDelegate);
								uIListItemContainer.MakeChild(itemTexture.gameObject);
								itemTexture.gameObject.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), num);
								itemTexture.Visible = data.visible;
								itemTexture.controlIsEnabled = data.enable;
								if (columnData.alpha != 1f)
								{
									itemTexture.SetAlpha(columnData.alpha);
								}
							}
						}
						else if (columnData.type == NewListBox.TYPE.CHECKBOX)
						{
							CheckBox checkBox = UICreateControl.CheckBox(i.ToString() + "button", columnData.styleName1, (float)columnData.width, (float)columnData.height);
							if (null != checkBox)
							{
								int num7 = (int)data.realData;
								checkBox.SetCheckState((num7 != 1) ? 0 : 1);
								if (string.Empty != columnData.imageInverse)
								{
									checkBox.Inverse(UIBaseFileManager.GetInverse(columnData.imageInverse));
								}
								checkBox.Data = data.data;
								checkBox.AddValueChangedDelegate(data.eventDelegate);
								checkBox.AddMouseDownDelegate(data.downDelegate);
								uIListItemContainer.MakeChild(checkBox.gameObject);
								checkBox.gameObject.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), num);
								checkBox.Visible = data.visible;
								checkBox.controlIsEnabled = data.enable;
								if (columnData.alpha != 1f)
								{
									checkBox.SetAlpha(columnData.alpha);
								}
							}
						}
						else if (columnData.type == NewListBox.TYPE.TOGGLE)
						{
							string text = (string)data.realData;
							UIRadioBtn uIRadioBtn = UICreateControl.RadioBtn(i.ToString() + "button", columnData.styleName1, (float)columnData.width, (float)columnData.height);
							if (null != uIRadioBtn)
							{
								if (string.Empty != text)
								{
									uIRadioBtn.Text = text;
								}
								if (string.Empty != columnData.imageInverse)
								{
									uIRadioBtn.Inverse(UIBaseFileManager.GetInverse(columnData.imageInverse));
								}
								uIRadioBtn.Data = data.data;
								uIRadioBtn.AddValueChangedDelegate(data.eventDelegate);
								uIRadioBtn.AddMouseDownDelegate(data.downDelegate);
								uIListItemContainer.MakeChild(uIRadioBtn.gameObject);
								uIRadioBtn.gameObject.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), num);
								uIRadioBtn.Visible = data.visible;
								uIRadioBtn.controlIsEnabled = data.enable;
								if (columnData.alpha != 1f)
								{
									uIRadioBtn.SetAlpha(columnData.alpha);
								}
							}
						}
						else if (columnData.type == NewListBox.TYPE.TEXTFIELD)
						{
							string str3 = (string)data.realData;
							TextField textField = UICreateControl.TextField(i.ToString() + "text", str3, columnData.multiLine, (float)columnData.width, (float)columnData.height, columnData.fontSize, columnData.fontEffect, columnData.anchor, columnData.fontColor);
							if (null != textField)
							{
								textField.Data = data.data;
								uIListItemContainer.MakeChild(textField.gameObject);
								textField.gameObject.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), num);
								if (string.Empty != columnData.imageInverse)
								{
									textField.Inverse(UIBaseFileManager.GetInverse(columnData.imageInverse));
								}
								textField.Visible = data.visible;
								textField.controlIsEnabled = data.enable;
								if (columnData.alpha != 1f)
								{
									textField.SetAlpha(columnData.alpha);
								}
							}
						}
						else if (columnData.type == NewListBox.TYPE.SLIDER)
						{
							HorizontalSlider horizontalSlider = UICreateControl.HorizontalSlider(i.ToString() + "slider", columnData.styleName1, columnData.styleName2, columnData.styleName3, (float)columnData.width, (float)columnData.height);
							if (null != horizontalSlider)
							{
								horizontalSlider.Data = data.data;
								horizontalSlider.AddValueChangedDelegate(data.eventDelegate);
								uIListItemContainer.MakeChild(horizontalSlider.gameObject);
								horizontalSlider.gameObject.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), num);
								if (string.Empty != columnData.imageInverse)
								{
									horizontalSlider.Inverse(UIBaseFileManager.GetInverse(columnData.imageInverse));
								}
								horizontalSlider.Start();
								if (data.realData is float)
								{
									horizontalSlider.defaultValue = (float)data.realData;
									horizontalSlider.Value = (float)data.realData;
								}
								else
								{
									horizontalSlider.defaultValue = 0f;
									horizontalSlider.Value = 0f;
								}
								horizontalSlider.Visible = data.visible;
								horizontalSlider.controlIsEnabled = data.enable;
								if (columnData.alpha != 1f)
								{
									horizontalSlider.SetAlpha(columnData.alpha);
								}
							}
						}
						else if (columnData.type == NewListBox.TYPE.SCROLLLABEL)
						{
							string str4 = (string)data.realData;
							ScrollLabel scrollLabel = UICreateControl.ScrollLabel(i.ToString() + "text", str4, columnData.multiLine, (float)columnData.width, (float)columnData.height, columnData.fontSize, columnData.fontEffect, columnData.anchor, columnData.fontColor);
							if (null != scrollLabel)
							{
								scrollLabel.name = i.ToString() + "ScrollLabel";
								scrollLabel.Data = data.data;
								uIListItemContainer.MakeChild(scrollLabel.gameObject);
								scrollLabel.SetLocation((float)columnData.x, (float)columnData.y, num);
								scrollLabel.Visible = data.visible;
								scrollLabel.controlIsEnabled = data.enable;
								if (columnData.alpha != 1f)
								{
									scrollLabel.SetAlpha(columnData.alpha);
								}
							}
						}
					}
				}
			}
			uIListItemContainer.SetControlIsEnabled(item.GetEnable());
			return uIListItemContainer;
		}

		public virtual void InsertAdd(int index, NewListItem item)
		{
			UIListItemContainer uIListItemContainer = this.CreateContainer(item);
			if (null != uIListItemContainer)
			{
				base.InsertItemDonotPosionUpdate(uIListItemContainer, index, null, false);
			}
		}

		public void UpdateListItem(NewListItem newItem)
		{
			NewListItem[] array = this.reserveItems.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				NewListItem newListItem = array[i];
				if (newListItem != null)
				{
					if (newItem.Data.Equals(newListItem.Data))
					{
						this.reserveItems.Clear();
						for (int j = 0; j < array.Length; j++)
						{
							if (i == j)
							{
								this.reserveItems.Enqueue(newItem);
							}
							else
							{
								this.reserveItems.Enqueue(array[j]);
							}
						}
						break;
					}
				}
			}
		}

		public virtual void RemoveAdd(int index, NewListItem item)
		{
			UIListItemContainer uIListItemContainer = this.CreateContainer(item);
			if (null != uIListItemContainer)
			{
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

		public virtual void RemoveAddNew(int index, NewListItem item)
		{
			int num = index % this.items.Count;
			UIListItemContainer uIListItemContainer = this.CreateContainer(item);
			if (null != uIListItemContainer)
			{
				if (null != base.GetItem(num))
				{
					base.RemoveItemDonotPositionUpdate(num, true);
					base.InsertItemDonotPosionUpdate(uIListItemContainer, num, null, false);
				}
				else
				{
					base.InsertItemDonotPosionUpdate(uIListItemContainer, num, null, false);
				}
			}
		}

		public void UpdateContents(int index, NewListItem item)
		{
			int index2 = index % this.items.Count;
			UIListItemContainer uIListItemContainer = this.items[index2];
			if (null == uIListItemContainer)
			{
				return;
			}
			uIListItemContainer.Data = item.Data;
			for (int i = 0; i < this.coulmnDataList.Count; i++)
			{
				NewListBox.ColumnData columnData = this.coulmnDataList[i];
				if (columnData != null)
				{
					NewListItem.NewListItemData data = item.GetData(i);
					if (data != null)
					{
						if (columnData.type == NewListBox.TYPE.LABEL)
						{
							Label label = (Label)uIListItemContainer.GetElement(i);
							if (!(null == label))
							{
								string text = (string)data.realData;
								label.Text = text;
								label.Data = data.data;
								label.Visible = data.visible;
								label.controlIsEnabled = data.enable;
							}
						}
						else if (columnData.type != NewListBox.TYPE.FLASHLABEL)
						{
							if (columnData.type == NewListBox.TYPE.BUTTON)
							{
								UIButton uIButton = (UIButton)uIListItemContainer.GetElement(i);
								if (!(null == uIButton))
								{
									if (data.realData is string)
									{
										uIButton.Text = (string)data.realData;
									}
									else if (data.realData is UIBaseInfoLoader)
									{
										uIButton.SetButtonTextureKey((UIBaseInfoLoader)data.realData);
										if (data.data2 is string)
										{
											uIButton.Text = (string)data.data2;
										}
									}
									if (null != uIButton.spriteText)
									{
										uIButton.SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
										uIButton.SetAlignment(SpriteText.Alignment_Type.Center);
										uIButton.SetFontEffect(columnData.fontEffect);
										uIButton.SetCharacterSize(columnData.fontSize);
									}
									uIButton.Data = data.data;
									uIButton.SetValueChangedDelegate(data.eventDelegate);
									uIButton.SetMouseDownDelegate(data.downDelegate);
									uIButton.Visible = data.visible;
									uIButton.controlIsEnabled = data.enable;
									uIButton.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), uIButton.transform.localPosition.z);
								}
							}
							else if (columnData.type == NewListBox.TYPE.DRAWTEXTURE)
							{
								DrawTexture drawTexture = (DrawTexture)uIListItemContainer.GetElement(i);
								if (!(null == drawTexture))
								{
									if (data.data is float)
									{
										float num = (float)data.data;
										columnData.width = (int)num;
										drawTexture.Setup((float)columnData.width, (float)columnData.height);
									}
									if (data.realData is UIBaseInfoLoader)
									{
										UIBaseInfoLoader texture = (UIBaseInfoLoader)data.realData;
										drawTexture.SetTexture(texture);
									}
									else if (data.realData is ITEM)
									{
										ITEM iTEM = (ITEM)data.realData;
										if (iTEM.m_nItemUnique == 70000)
										{
											NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_HEARTS_STONE", drawTexture, new Vector2(drawTexture.width, drawTexture.height));
										}
										int num2 = iTEM.m_nOption[2];
										if (string.Compare(MsgHandler.HandleReturn<string>("RankStateString", new object[]
										{
											num2
										}), "best") == 0)
										{
											NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_WEAPON_GOOD", drawTexture, new Vector2(drawTexture.width, drawTexture.height));
										}
										drawTexture.SetTexture(MsgHandler.HandleReturn<UIBaseInfoLoader>("GetItemTexture", new object[]
										{
											iTEM.m_nItemUnique
										}));
									}
									else if (data.realData is int)
									{
										int num3 = (int)data.realData;
										int num4 = -1;
										if (data.data is int)
										{
											num4 = (int)data.data;
										}
										if ("false" == MsgHandler.HandleReturn<string>("IsReincarnation", new object[0]) && "true" == MsgHandler.HandleReturn<string>("CharKindIsATB", new object[]
										{
											num3,
											1L
										}))
										{
											num4 = -1;
										}
										if (item.EventMark)
										{
											if (0 < num4)
											{
												drawTexture.SetTextureEvent(eCharImageType.SMALL, num3, num4);
											}
											else
											{
												drawTexture.SetTextureEvent(eCharImageType.SMALL, num3, -1);
											}
										}
										else if (0 < num4)
										{
											drawTexture.SetTexture(eCharImageType.SMALL, num3, num4, string.Empty);
										}
										else if (data.data is bool)
										{
											drawTexture.SetTexture(eCharImageType.MIDDLE, num3, -1, string.Empty);
										}
										else
										{
											drawTexture.SetTexture(eCharImageType.SMALL, num3, -1, string.Empty);
										}
									}
									else if (data.realData is string)
									{
										if (data.data != null)
										{
											if (data.data is bool)
											{
												bool flag = (bool)data.data;
												if (flag)
												{
													drawTexture.SetTextureFromBundle((string)data.realData);
												}
												else
												{
													drawTexture.SetTexture((string)data.realData);
												}
											}
											else
											{
												drawTexture.SetTexture((string)data.realData);
											}
										}
										else
										{
											drawTexture.SetTexture((string)data.realData);
										}
									}
									else if (data.realData is CostumeDrawTextureInfo)
									{
										CostumeDrawTextureInfo costumeDrawTextureInfo = data.realData as CostumeDrawTextureInfo;
										drawTexture.SetTexture(costumeDrawTextureInfo.imageType, costumeDrawTextureInfo.charKind, costumeDrawTextureInfo.grade, costumeDrawTextureInfo.costumePortraitPath);
									}
									else if (data.realData is Texture2D)
									{
										drawTexture.SetTexture2D((Texture2D)data.realData);
									}
									if (data.data2 is string)
									{
										string path = (string)data.data2;
										NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, drawTexture, new Vector2(drawTexture.width, drawTexture.height));
									}
									drawTexture.Data = data.data;
									drawTexture.SetValueChangedDelegate(data.eventDelegate);
									drawTexture.SetMouseDownDelegate(data.downDelegate);
									drawTexture.Visible = data.visible;
									drawTexture.controlIsEnabled = data.enable;
								}
							}
							else if (columnData.type == NewListBox.TYPE.ITEMTEXTURE)
							{
								ItemTexture itemTexture = (ItemTexture)uIListItemContainer.GetElement(i);
								if (!(null == itemTexture))
								{
									if (data.realData is UIBaseInfoLoader)
									{
										UIBaseInfoLoader texture2 = (UIBaseInfoLoader)data.realData;
										itemTexture.SetTexture(texture2);
									}
									else if (data.realData is ITEM)
									{
										ITEM iTEM2 = (ITEM)data.realData;
										if (data.data != null && data.data is string)
										{
											if ("Material" == (string)data.data)
											{
												itemTexture.SetItemTexture(iTEM2, false, false, 1f);
											}
											else
											{
												itemTexture.SetItemTexture(iTEM2);
											}
										}
										else
										{
											itemTexture.SetItemTexture(iTEM2);
										}
										if (data.data2 != null)
										{
											ITEM c_cItemTooltip = (ITEM)data.data2;
											itemTexture.c_cItemTooltip = c_cItemTooltip;
											itemTexture.c_cItemSecondTooltip = iTEM2;
										}
										else
										{
											itemTexture.c_cItemTooltip = iTEM2;
										}
									}
									else if (data.realData is int)
									{
										int charkind = (int)data.realData;
										if (data.data is int)
										{
											int level = (int)data.data;
											itemTexture.SetSolImageTexure(eCharImageType.SMALL, charkind, -1, level);
										}
										else
										{
											itemTexture.SetSolImageTexure(eCharImageType.SMALL, charkind, -1);
										}
									}
									else if (data.realData is NkListSolInfo)
									{
										NkListSolInfo solInfo = (NkListSolInfo)data.realData;
										bool flag2 = false;
										if (data.data != null && data.data is bool)
										{
											flag2 = (bool)data.data;
										}
										itemTexture.SetSolImageTexure(eCharImageType.SMALL, solInfo, flag2);
									}
									else if (data.realData is string)
									{
										if (data.data != null)
										{
											if (data.data is bool)
											{
												bool flag3 = (bool)data.data;
												if (flag3)
												{
													itemTexture.SetTextureFromBundle((string)data.realData);
												}
												else
												{
													itemTexture.SetTexture((string)data.realData);
												}
											}
											else
											{
												itemTexture.SetTexture((string)data.realData);
											}
										}
										else
										{
											itemTexture.SetTexture((string)data.realData);
										}
									}
									else if (data.realData is Texture2D)
									{
										if (data.data2 != null)
										{
											short nLevel = (short)data.data2;
											itemTexture.SetEventImageTexure((Texture2D)data.realData, nLevel, this.bShowEventMark);
										}
										else
										{
											itemTexture.SetEventImageTexure((Texture2D)data.realData, 0, this.bShowEventMark);
										}
									}
									itemTexture.Data = data.data;
									itemTexture.Visible = data.visible;
									itemTexture.controlIsEnabled = data.enable;
									itemTexture.SetValueChangedDelegate(data.eventDelegate);
								}
							}
							else if (columnData.type == NewListBox.TYPE.CHECKBOX)
							{
								CheckBox checkBox = (CheckBox)uIListItemContainer.GetElement(i);
								if (!(null == checkBox))
								{
									int num5 = (int)data.realData;
									checkBox.SetCheckState((num5 != 1) ? 0 : 1);
									checkBox.Data = data.data;
									checkBox.SetValueChangedDelegate(data.eventDelegate);
									checkBox.SetMouseDownDelegate(data.downDelegate);
									checkBox.Visible = data.visible;
									checkBox.controlIsEnabled = data.enable;
								}
							}
							else if (columnData.type == NewListBox.TYPE.TOGGLE)
							{
								UIRadioBtn uIRadioBtn = (UIRadioBtn)uIListItemContainer.GetElement(i);
								if (!(null == uIRadioBtn))
								{
									string text2 = (string)data.realData;
									if (string.Empty != text2)
									{
										uIRadioBtn.Text = text2;
									}
									uIRadioBtn.Data = data.data;
									uIRadioBtn.Visible = data.visible;
									uIRadioBtn.controlIsEnabled = data.enable;
								}
							}
							else if (columnData.type == NewListBox.TYPE.TEXTFIELD)
							{
								UIRadioBtn uIRadioBtn2 = (UIRadioBtn)uIListItemContainer.GetElement(i);
								if (!(null == uIRadioBtn2))
								{
									string text3 = (string)data.realData;
									uIRadioBtn2.Data = data.data;
									uIRadioBtn2.Text = text3;
									if (!data.visible)
									{
										uIRadioBtn2.Visible = data.visible;
									}
									if (!data.enable)
									{
										uIRadioBtn2.controlIsEnabled = data.enable;
									}
								}
							}
							else if (columnData.type == NewListBox.TYPE.SLIDER)
							{
								HorizontalSlider horizontalSlider = (HorizontalSlider)uIListItemContainer.GetElement(i);
								if (!(null == horizontalSlider))
								{
									horizontalSlider.Data = data.data;
									horizontalSlider.SetValueChangedDelegate(data.eventDelegate);
									horizontalSlider.SetMouseDownDelegate(data.downDelegate);
									horizontalSlider.Start();
									if (data.realData is float)
									{
										horizontalSlider.defaultValue = (float)data.realData;
										horizontalSlider.Value = (float)data.realData;
									}
									else
									{
										horizontalSlider.defaultValue = 0f;
										horizontalSlider.Value = 0f;
									}
									horizontalSlider.Visible = data.visible;
									horizontalSlider.controlIsEnabled = data.enable;
								}
							}
						}
					}
				}
			}
			uIListItemContainer.SetControlIsEnabled(item.GetEnable());
		}

		public virtual void UpdateAdd(int index, NewListItem item)
		{
			if (null != base.GetItem(index))
			{
				UIListItemContainer uIListItemContainer = this.CreateContainer(item);
				if (null != uIListItemContainer)
				{
					base.RemoveItemDonotPositionUpdate(index, true);
					base.InsertItemDonotPosionUpdate(uIListItemContainer, index, null, false);
				}
			}
			else
			{
				this.UpdateListItem(item);
			}
		}

		public void Add(NewListItem item)
		{
			if (this.bLabelScroll)
			{
				char[] separator = new char[]
				{
					'\n'
				};
				string[] array = ((string)item.GetData(0).realData).Split(separator);
				for (int i = 0; i < array.Length; i++)
				{
					GameObject gameObject = new GameObject("ListItem");
					FlashLabel flashLabel = gameObject.AddComponent<FlashLabel>();
					flashLabel.FontSize = this.coulmnDataList[0].fontSize;
					flashLabel.FontEffect = this.coulmnDataList[0].fontEffect;
					flashLabel.FontColor = string.Empty;
					flashLabel.anchor = this.coulmnDataList[0].anchor;
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
			else if (this.m_bReserve || this.m_bReUse)
			{
				if (this.items.Count == 0)
				{
					this.MakeContainer(item);
					this.checkTime = Time.realtimeSinceStartup;
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

		public override void Update()
		{
			if (0 < this.reserveItems.Count)
			{
				if (this.m_bReserve)
				{
					if (Time.realtimeSinceStartup - this.checkTime > 0.02f)
					{
						for (int i = 0; i < 3; i++)
						{
							if (this.reserveItems.Count > 0)
							{
								NewListItem newListItem = this.reserveItems.Dequeue();
								if (newListItem != null)
								{
									this.MakeContainer(newListItem);
								}
							}
						}
						this.RepositionItems();
						this.checkTime = Time.realtimeSinceStartup;
					}
					if (this.reserveItems.Count == 0 && this.makeCompleteDelegate != null)
					{
						this.makeCompleteDelegate(this);
						this.makeCompleteDelegate = null;
					}
				}
				else if (this.m_bReUse)
				{
					this.CalculateMaxContainerNum();
					if (this.startIndex < this.m_nMaxNum && 0 < this.reserveItems.Count)
					{
						if (Time.realtimeSinceStartup - this.checkTime > 0.005f)
						{
							NewListItem newListItem2 = this.reserveItems.Dequeue();
							if (newListItem2 != null)
							{
								this.MakeContainer(newListItem2);
							}
							this.RepositionItems();
							this.limitListNum = this.items.Count + this.reserveItems.Count;
							this.checkTime = Time.realtimeSinceStartup;
							if (this.reserveItems.Count == 0)
							{
								this.MakeCompleteList();
							}
						}
					}
					else
					{
						this.MakeCompleteList();
					}
				}
			}
			base.Update();
		}

		protected override void ReserveMakeItem()
		{
		}

		private void MakeCompleteList()
		{
			if (!this.callRepositionItems)
			{
				this.RepositionItems();
				this.callRepositionItems = true;
			}
			if (this.makeCompleteDelegate != null)
			{
				this.callSlidePosChangeDelegate = true;
				this.makeCompleteDelegate(this);
				this.makeCompleteDelegate = null;
			}
		}

		public override void RepositionItems()
		{
			if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
			{
				base.PositionHorizontally(true);
			}
			else
			{
				base.PositionVertically(true);
			}
			if (this.m_bReUse && 0 < this.items.Count)
			{
				float num = this.items[0].TopLeftEdge().y - this.items[0].BottomRightEdge().y + this.itemSpacing;
				float num2 = this.items[this.items.Count - 1].transform.localPosition.y;
				for (int i = 0; i < this.reserveItems.Count; i++)
				{
					float num3 = num2 - num;
					this.listPosY.Add(num3);
					this.contentExtents += num;
					num2 = num3;
				}
				this.contentExtents -= this.itemSpacing;
			}
			base.UpdateContentExtents(0f);
			base.ClipItems();
		}

		private void MakeContainer(NewListItem item)
		{
			if (!item.m_szColumnData.Equals(string.Empty))
			{
				this.SetColumnData(item.m_szColumnData);
			}
			UIListItemContainer uIListItemContainer = this.CreateContainer(item);
			if (null != uIListItemContainer)
			{
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

		private void CalculateMaxContainerNum()
		{
			if (0 < this.m_nMaxNum)
			{
				return;
			}
			this.m_nMaxNum = (int)(this.viewableArea.y / this.lineHeight) + 2;
		}

		public bool IsReUseListItemsExcced()
		{
			if (!this.m_bReUse)
			{
				return false;
			}
			int num = this.items.Count + this.reserveItems.Count;
			return this.MaxNum < num;
		}

		public int GetReserverItemCount()
		{
			if (this.reserveItems == null)
			{
				return -1;
			}
			return this.reserveItems.Count;
		}
	}
}
