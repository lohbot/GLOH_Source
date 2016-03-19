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
			SLIDER
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
					uIButton.EffectAni = false;
					uIButton.IsListButton = true;
					uIButton.allwaysPlayAnim = true;
					uIListItemContainer.MakeChild(uIButton.gameObject);
					uIListItemContainer.SetControlIsEnabled(false);
					base.InsertItemDonotPosionUpdate(uIListItemContainer, i, null, true);
				}
				base.DonotCountRepositionItems();
			}
			this.clipWhenMoving = true;
			this.startIndex = 0;
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
								if (!data.visible)
								{
									label.Visible = data.visible;
								}
								if (!data.enable)
								{
									label.controlIsEnabled = data.enable;
								}
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
								if (!data.visible)
								{
									uIListItemContainer2.Visible = data.visible;
								}
								if (!data.enable)
								{
									uIListItemContainer2.SetControlIsEnabled(data.enable);
								}
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
								if (!data.visible)
								{
									uIButton2.Visible = data.visible;
								}
								if (!data.enable)
								{
									uIButton2.controlIsEnabled = data.enable;
								}
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
										drawTexture.SetTexture(eCharImageType.SMALL, num5, num6);
									}
									else if (data.data is bool)
									{
										drawTexture.SetTexture(eCharImageType.MIDDLE, num5, -1);
									}
									else
									{
										drawTexture.SetTexture(eCharImageType.SMALL, num5, -1);
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
								if (string.Empty != columnData.imageInverse)
								{
									drawTexture.Inverse(UIBaseFileManager.GetInverse(columnData.imageInverse));
								}
								drawTexture.Data = data.data;
								drawTexture.AddValueChangedDelegate(data.eventDelegate);
								drawTexture.AddMouseDownDelegate(data.downDelegate);
								uIListItemContainer.MakeChild(drawTexture.gameObject);
								drawTexture.gameObject.transform.localPosition = new Vector3((float)columnData.x, (float)(-(float)columnData.y), num);
								if (!data.visible)
								{
									drawTexture.Visible = data.visible;
								}
								if (!data.enable)
								{
									drawTexture.controlIsEnabled = data.enable;
								}
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
									itemTexture.SetItemTexture(iTEM2);
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
								if (!data.visible)
								{
									itemTexture.Visible = data.visible;
								}
								if (!data.enable)
								{
									itemTexture.controlIsEnabled = data.enable;
								}
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
								if (!data.visible)
								{
									checkBox.Visible = data.visible;
								}
								if (!data.enable)
								{
									checkBox.controlIsEnabled = data.enable;
								}
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
								if (!data.visible)
								{
									uIRadioBtn.Visible = data.visible;
								}
								if (!data.enable)
								{
									uIRadioBtn.controlIsEnabled = data.enable;
								}
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
								if (!data.visible)
								{
									textField.Visible = data.visible;
								}
								if (!data.enable)
								{
									textField.controlIsEnabled = data.enable;
								}
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
								if (!data.visible)
								{
									horizontalSlider.Visible = data.visible;
								}
								if (!data.enable)
								{
									horizontalSlider.controlIsEnabled = data.enable;
								}
								if (columnData.alpha != 1f)
								{
									horizontalSlider.SetAlpha(columnData.alpha);
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
				if (base.GetItem(index) != null)
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

		public virtual void UpdateAdd(int index, NewListItem item)
		{
			if (base.GetItem(index) != null)
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
					if (base.GetItem(this.startIndex) != null)
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
			if (0 < this.reserveItems.Count && this.m_bReserve && Time.realtimeSinceStartup - this.checkTime > 0.02f)
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
				base.RepositionItems();
				this.checkTime = Time.realtimeSinceStartup;
			}
			base.Update();
		}

		protected override void ReserveMakeItem()
		{
		}

		private void MakeContainer(NewListItem item)
		{
			UIListItemContainer uIListItemContainer = this.CreateContainer(item);
			if (null != uIListItemContainer)
			{
				if (base.GetItem(this.startIndex) != null)
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
}
