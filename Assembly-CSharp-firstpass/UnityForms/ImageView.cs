using GAME;
using GameMessage;
using System;
using System.Text;
using UnityEngine;

namespace UnityForms
{
	public class ImageView : UIScrollList
	{
		private int columnNum = 1;

		private int rowNum = 1;

		private int slotWidth;

		private int slotHeight;

		private int offSetX;

		private int offSetY;

		private bool dragging = true;

		private Vector3 vectorValue = Vector3.zero;

		private StringBuilder strBuilder = new StringBuilder(32);

		public int ColumnNum
		{
			get
			{
				return this.columnNum;
			}
			set
			{
				this.columnNum = value;
			}
		}

		public int RowNum
		{
			get
			{
				return this.rowNum;
			}
			set
			{
				this.rowNum = value;
			}
		}

		public int SlotWidth
		{
			get
			{
				return this.slotWidth;
			}
			set
			{
				this.slotWidth = value;
			}
		}

		public int SlotHeight
		{
			get
			{
				return this.slotHeight;
			}
			set
			{
				this.slotHeight = value;
			}
		}

		public int OffSetX
		{
			get
			{
				return this.offSetX;
			}
			set
			{
				this.offSetX = value;
			}
		}

		public int OffSetY
		{
			get
			{
				return this.offSetY;
			}
			set
			{
				this.offSetY = value;
			}
		}

		public bool isDragging
		{
			get
			{
				return this.dragging;
			}
			set
			{
				this.dragging = value;
			}
		}

		public new static ImageView Create(string name, Vector3 pos)
		{
			return (ImageView)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(ImageView));
		}

		public void SetImageView(int columnNum, int rowNum, int slotWidth, int slotHeight, int offSetX, int offSetY, int imageViewHeight)
		{
			this.columnCount = columnNum;
			this.columnNum = columnNum;
			this.rowNum = rowNum;
			this.slotWidth = slotWidth;
			this.slotHeight = slotHeight;
			this.offSetX = offSetX;
			this.offSetY = offSetY;
			this.lineHeight = (float)slotHeight;
		}

		public void Resize(int imageViewHeight)
		{
		}

		public void Clear()
		{
			base.ClearList(true);
		}

		public void SetImageSlot(ImageSlot slot, EZDragDropDelegate del, EZValueChangedDelegate mouseOverDel, EZValueChangedDelegate mouseOutDel, EZValueChangedDelegate rightMouseDel)
		{
			UIListItemContainer item = base.GetItem(slot.Index);
			if (null != item)
			{
				base.RemoveItem(slot.Index, true);
				base.InsertItem(this.MakeListItem(slot.Index, slot, del, mouseOverDel, mouseOutDel, rightMouseDel), slot.Index);
			}
			else
			{
				this.AddListItem(this.MakeListItem(slot.Index, slot, del, mouseOverDel, mouseOutDel, rightMouseDel));
			}
		}

		public UIListItemContainer SetImageSlot(int index, ImageSlot slot, EZDragDropDelegate del, EZValueChangedDelegate mouseOverDel, EZValueChangedDelegate mouseOutDel, EZValueChangedDelegate rightMouseDel)
		{
			UIListItemContainer item = base.GetItem(index);
			if (null != item)
			{
				base.RemoveItem(index, true);
				UIListItemContainer uIListItemContainer = this.MakeListItem(index, slot, del, mouseOverDel, mouseOutDel, rightMouseDel);
				base.InsertItem(uIListItemContainer, index);
				return uIListItemContainer;
			}
			UIListItemContainer uIListItemContainer2 = this.MakeListItem(index, slot, del, mouseOverDel, mouseOutDel, rightMouseDel);
			this.AddListItem(uIListItemContainer2);
			return uIListItemContainer2;
		}

		public UIListItemContainer MakeListItem(int index, ImageSlot imageSlot, EZDragDropDelegate del, EZValueChangedDelegate tapDel, EZValueChangedDelegate mouseOutDel, EZValueChangedDelegate rightMouseDel)
		{
			string text = index.ToString();
			this.strBuilder.Length = 0;
			this.strBuilder.AppendFormat("{0}{1}", "ImageSlot", index);
			GameObject gameObject = new GameObject(this.strBuilder.ToString());
			UIListItemContainer uIListItemContainer = gameObject.AddComponent<UIListItemContainer>();
			if (null == uIListItemContainer)
			{
				return null;
			}
			uIListItemContainer.Start();
			uIListItemContainer.AutoFindOuterEdges = false;
			uIListItemContainer.Data = imageSlot;
			uIListItemContainer.gameObject.layer = GUICamera.UILayer;
			if (imageSlot.itemunique > 0)
			{
				uIListItemContainer.IsDraggable = true;
			}
			else
			{
				uIListItemContainer.IsDraggable = false;
			}
			if (!this.isDragging)
			{
				uIListItemContainer.IsDraggable = false;
			}
			uIListItemContainer.MouseOffset = (float)(this.slotWidth / 2);
			uIListItemContainer.SetDragDropDelegate(del);
			uIListItemContainer.SetValueChangedDelegate(tapDel);
			this.strBuilder.Length = 0;
			this.strBuilder.AppendFormat("{0}{1}", "backImage", text);
			GameObject gameObject2 = new GameObject(this.strBuilder.ToString());
			DrawTexture drawTexture = gameObject2.AddComponent<DrawTexture>();
			drawTexture.SetUseBoxCollider(false);
			drawTexture.gameObject.layer = GUICamera.UILayer;
			drawTexture.width = (float)this.slotWidth;
			drawTexture.height = (float)this.slotHeight;
			if (imageSlot.WindowID == 260)
			{
				BoxCollider boxCollider = (BoxCollider)uIListItemContainer.gameObject.AddComponent(typeof(BoxCollider));
				if (!imageSlot.c_bDisable)
				{
					drawTexture.SetTexture("Win_T_ItemEmpty");
				}
				else if (imageSlot.p_nAddEnableSlot > 0)
				{
					drawTexture.SetTexture("Com_I_SlotUnlock");
				}
				else
				{
					drawTexture.SetTexture("Com_I_SlotLock");
				}
				boxCollider.size = new Vector3((float)this.slotWidth, (float)this.slotHeight, 0.01f);
				boxCollider.center = new Vector3((float)(this.slotWidth / 2), (float)(-(float)this.slotHeight / 2), 0f);
			}
			else if (!imageSlot.c_bDisable)
			{
				if (string.Empty != imageSlot.imageStr)
				{
					drawTexture.SetTexture(imageSlot.imageStr);
				}
				else
				{
					drawTexture.SetTexture("Com_I_Transparent");
				}
				BoxCollider boxCollider2 = (BoxCollider)uIListItemContainer.gameObject.AddComponent(typeof(BoxCollider));
				boxCollider2.size = new Vector3((float)this.slotWidth, (float)this.slotHeight, 0.01f);
				boxCollider2.center = new Vector3((float)(this.slotWidth / 2), (float)(-(float)this.slotHeight / 2), 0f);
			}
			else if (imageSlot.p_nAddEnableSlot > 0)
			{
				drawTexture.SetTexture("Com_I_SlotUnlock");
			}
			else
			{
				drawTexture.SetTexture("Com_I_SlotLock");
			}
			drawTexture.autoResize = false;
			drawTexture.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			uIListItemContainer.MakeChild(drawTexture.gameObject);
			ITEM iTEM = imageSlot.c_oItem as ITEM;
			if (iTEM != null && imageSlot.c_bDisable)
			{
				this.vectorValue.z = -0.05f;
			}
			else
			{
				this.vectorValue.z = 0f;
			}
			this.vectorValue.x = 0f;
			this.vectorValue.y = 0f;
			drawTexture.gameObject.transform.localPosition = this.vectorValue;
			if (imageSlot.ItemImage)
			{
				DrawTexture drawTexture2 = null;
				float num = 0f;
				float num2 = 0f;
				if (0 < imageSlot.itemunique)
				{
					this.strBuilder.Length = 0;
					this.strBuilder.AppendFormat("{0}{1}", "image", text);
					GameObject gameObject3 = new GameObject(this.strBuilder.ToString());
					DrawTexture drawTexture3 = gameObject3.AddComponent<DrawTexture>();
					drawTexture2 = drawTexture3;
					drawTexture3.SetUseBoxCollider(false);
					drawTexture3.gameObject.layer = GUICamera.UILayer;
					drawTexture3.autoResize = false;
					UIBaseInfoLoader texture = MsgHandler.HandleReturn<UIBaseInfoLoader>("GetItemTexture", new object[]
					{
						imageSlot.itemunique
					});
					if (100f < (float)this.slotWidth)
					{
						drawTexture3.width = 100f;
						drawTexture3.height = 100f;
						num = ((float)this.slotWidth - drawTexture3.width) / 2f;
						num2 = -((float)this.slotHeight - drawTexture3.height) / 2f;
					}
					else
					{
						drawTexture3.width = (float)this.slotWidth;
						drawTexture3.height = (float)this.slotHeight;
					}
					drawTexture3.SetTexture(texture);
					drawTexture3.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					uIListItemContainer.MakeChild(drawTexture3.gameObject);
					drawTexture3.gameObject.transform.localPosition = new Vector3(num, num2, -0.01f);
					if (imageSlot.WindowID == 260)
					{
						DrawTexture drawTexture4 = UICreateControl.DrawTexture(UIScrollList.lockImageName, "Win_I_Lock01");
						if (null != drawTexture4)
						{
							drawTexture4.UsedCollider(false);
							uIListItemContainer.MakeChild(drawTexture4.gameObject);
							drawTexture4.gameObject.transform.localPosition = new Vector3(0f, -((float)this.slotHeight - drawTexture4.height), -0.04f);
							uIListItemContainer.SetLocked(iTEM.IsLock());
						}
						DrawTexture drawTexture5 = UICreateControl.DrawTexture(UIScrollList.selectImageName, "Com_I_Check");
						if (null != drawTexture5)
						{
							drawTexture5.UsedCollider(false);
							uIListItemContainer.MakeChild(drawTexture5.gameObject);
							drawTexture5.Visible = false;
							drawTexture5.gameObject.transform.localPosition = new Vector3(((float)this.slotWidth - drawTexture5.width) / 2f, -((float)this.slotHeight - drawTexture5.height) / 2f, -0.05f);
						}
						DrawTexture drawTexture6 = UICreateControl.DrawTexture(UIScrollList.backButtonName, "Win_T_BK");
						if (null != drawTexture6)
						{
							drawTexture6.SetAlpha(0.9f);
							drawTexture6.SetSize((float)this.slotWidth, (float)this.slotHeight);
							drawTexture6.UsedCollider(true);
							uIListItemContainer.MakeChild(drawTexture6.gameObject);
							drawTexture6.Visible = false;
							drawTexture6.gameObject.transform.localPosition = new Vector3(((float)this.slotWidth - drawTexture6.width) / 2f, -((float)this.slotHeight - drawTexture6.height) / 2f, -0.12f);
						}
						DrawTexture drawTexture7 = UICreateControl.DrawTexture(UIScrollList.BreakItemImageName, "Win_T_RD");
						if (null != drawTexture7)
						{
							drawTexture7.SetAlpha(0.5f);
							drawTexture7.SetSize((float)(this.slotWidth - 10), (float)(this.slotHeight - 10));
							drawTexture7.UsedCollider(false);
							uIListItemContainer.MakeChild(drawTexture7.gameObject);
							drawTexture7.gameObject.transform.localPosition = new Vector3(((float)this.slotWidth - drawTexture7.width) / 2f, -((float)this.slotHeight - drawTexture7.height) / 2f, -0.05f);
							uIListItemContainer.SetBreak(iTEM.IsBreak());
						}
					}
					else if (imageSlot.WindowID == 82)
					{
						DrawTexture drawTexture8 = UICreateControl.DrawTexture(UIScrollList.BreakItemImageName, "Win_T_RD");
						if (null != drawTexture8)
						{
							drawTexture8.SetAlpha(0.5f);
							drawTexture8.SetSize((float)this.slotWidth, (float)this.slotHeight);
							drawTexture8.UsedCollider(false);
							uIListItemContainer.MakeChild(drawTexture8.gameObject);
							drawTexture8.gameObject.transform.localPosition = new Vector3(((float)this.slotWidth - drawTexture8.width) / 2f, -((float)this.slotHeight - drawTexture8.height) / 2f, -0.05f);
							uIListItemContainer.SetBreak(iTEM.m_nDurability == 0);
						}
					}
				}
				if (iTEM != null)
				{
					if (iTEM.m_nItemUnique != 70000 || null != drawTexture2)
					{
					}
					int num3 = iTEM.m_nOption[2];
					UIBaseInfoLoader uIBaseInfoLoader = MsgHandler.HandleReturn<UIBaseInfoLoader>("GetLegendItemGrade", new object[]
					{
						iTEM.m_nItemUnique
					});
					if (uIBaseInfoLoader != null)
					{
						if (string.Compare(MsgHandler.HandleReturn<string>("RankStateString", new object[]
						{
							num3
						}), "best") == 0 && null != drawTexture2)
						{
							drawTexture2.data = imageSlot.WindowID;
							NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_WEAPON_GOOD", drawTexture2, new Vector2(drawTexture2.width, drawTexture2.height));
							drawTexture2.AddGameObjectDelegate(new EZGameObjectDelegate(this.ItemEffect));
						}
						string @string = NrTSingleton<UIDataManager>.Instance.GetString("rank", text);
						GameObject gameObject4 = new GameObject(@string);
						DrawTexture drawTexture9 = gameObject4.AddComponent<DrawTexture>();
						drawTexture9.SetUseBoxCollider(false);
						drawTexture9.gameObject.layer = GUICamera.UILayer;
						drawTexture9.autoResize = false;
						drawTexture9.width = uIBaseInfoLoader.UVs.width;
						drawTexture9.height = uIBaseInfoLoader.UVs.height;
						drawTexture9.SetTexture(uIBaseInfoLoader);
						drawTexture9.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
						uIListItemContainer.MakeChild(drawTexture9.gameObject);
						drawTexture9.gameObject.transform.localPosition = new Vector3(2f + num, -2f + num2, -0.02f);
						drawTexture9.DeleteSpriteText();
					}
					else if ("true" == MsgHandler.HandleReturn<string>("IsRank", new object[]
					{
						iTEM.m_nItemUnique
					}) && num3 >= 1)
					{
						if (string.Compare(MsgHandler.HandleReturn<string>("RankStateString", new object[]
						{
							num3
						}), "best") == 0 && null != drawTexture2)
						{
							drawTexture2.data = imageSlot.WindowID;
							NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_WEAPON_GOOD", drawTexture2, new Vector2(drawTexture2.width, drawTexture2.height));
							drawTexture2.AddGameObjectDelegate(new EZGameObjectDelegate(this.ItemEffect));
						}
						string text2 = MsgHandler.HandleReturn<string>("ChangeRankToString", new object[]
						{
							num3
						});
						UIBaseInfoLoader uIBaseInfoLoader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_WorrGrade" + text2.ToUpper());
						if (uIBaseInfoLoader2 != null)
						{
							string string2 = NrTSingleton<UIDataManager>.Instance.GetString("rank", text);
							GameObject gameObject5 = new GameObject(string2);
							DrawTexture drawTexture10 = gameObject5.AddComponent<DrawTexture>();
							drawTexture10.SetUseBoxCollider(false);
							drawTexture10.gameObject.layer = GUICamera.UILayer;
							drawTexture10.autoResize = false;
							drawTexture10.width = 20f;
							drawTexture10.height = 20f;
							drawTexture10.SetTexture(uIBaseInfoLoader2);
							drawTexture10.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
							uIListItemContainer.MakeChild(drawTexture10.gameObject);
							drawTexture10.gameObject.transform.localPosition = new Vector3(2f + num, -2f + num2, -0.02f);
						}
					}
					if ("true" == MsgHandler.HandleReturn<string>("IsRank", new object[]
					{
						iTEM.m_nItemUnique
					}))
					{
						string string3 = NrTSingleton<UIDataManager>.Instance.GetString("num", text);
						Label label = UICreateControl.Label(string3, NrTSingleton<UIDataManager>.Instance.GetString("Lv.", MsgHandler.HandleReturn<string>("GetUseMinLevel", new object[]
						{
							iTEM
						})), false, (float)this.slotWidth - num - 2f, 20f, 20f, SpriteText.Font_Effect.Black_Shadow_Small, SpriteText.Anchor_Pos.Upper_Right, string.Empty);
						label.Visible = true;
						uIListItemContainer.MakeChild(label.gameObject);
						label.gameObject.transform.localPosition = new Vector3(0f, -((float)this.slotHeight - label.spriteText.TotalHeight + num2), -0.02f);
					}
					else
					{
						string string4 = NrTSingleton<UIDataManager>.Instance.GetString("num", text);
						Label label2 = UICreateControl.Label(string4, imageSlot.Num, false, (float)this.slotWidth - num - 2f, 20f, 20f, SpriteText.Font_Effect.Black_Shadow_Small, SpriteText.Anchor_Pos.Upper_Right, string.Empty);
						label2.Visible = true;
						uIListItemContainer.MakeChild(label2.gameObject);
						label2.gameObject.transform.localPosition = new Vector3(0f, -((float)this.slotHeight - label2.spriteText.TotalHeight + num2), -0.02f);
					}
				}
				if (imageSlot.CoolTime != Vector2.zero)
				{
					string name = "delayImage";
					GameObject gameObject6 = new GameObject(name);
					DrawTexture drawTexture11 = gameObject6.AddComponent<DrawTexture>();
					if (iTEM != null)
					{
						drawTexture11.data = imageSlot.CoolTime;
					}
					else
					{
						drawTexture11.data = Vector2.zero;
					}
					drawTexture11.SetUseBoxCollider(false);
					drawTexture11.gameObject.layer = GUICamera.UILayer;
					drawTexture11.width = (float)this.slotWidth;
					drawTexture11.height = (float)this.slotHeight;
					drawTexture11.SetTexture("Win_T_CPortDeathI");
					drawTexture11.autoResize = false;
					drawTexture11.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					uIListItemContainer.MakeChild(drawTexture11.gameObject);
					drawTexture11.gameObject.transform.localPosition = new Vector3(0f, (float)(-(float)this.slotHeight), -0.1f);
					drawTexture11.Visible = false;
				}
			}
			else if (0 < imageSlot.itemunique)
			{
				string string5 = NrTSingleton<UIDataManager>.Instance.GetString("image", text);
				GameObject gameObject7 = new GameObject(string5);
				DrawTexture drawTexture12 = gameObject7.AddComponent<DrawTexture>();
				drawTexture12.SetUseBoxCollider(false);
				drawTexture12.gameObject.layer = GUICamera.UILayer;
				drawTexture12.autoResize = false;
				UIBaseInfoLoader texture2 = MsgHandler.HandleReturn<UIBaseInfoLoader>("GetBattleSkillIconTexture", new object[]
				{
					imageSlot.itemunique
				});
				drawTexture12.width = 100f;
				drawTexture12.height = 100f;
				drawTexture12.SetTexture(texture2);
				drawTexture12.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
				uIListItemContainer.MakeChild(drawTexture12.gameObject);
				drawTexture12.gameObject.transform.localPosition = new Vector3(1f, -1f, -0.01f);
				if (imageSlot.c_bDisable || imageSlot.LevelUP)
				{
					GameObject gameObject8 = new GameObject(string5);
					DrawTexture drawTexture13 = gameObject8.AddComponent<DrawTexture>();
					drawTexture13.SetUseBoxCollider(false);
					drawTexture13.gameObject.layer = GUICamera.UILayer;
					drawTexture13.autoResize = false;
					drawTexture13.width = (float)this.slotWidth;
					drawTexture13.height = (float)this.slotHeight;
					if (imageSlot.c_bDisable)
					{
						drawTexture13.SetTexture("Win_T_CoverBL");
					}
					else if (imageSlot.LevelUP)
					{
						drawTexture13.SetTexture("Com_I_LvUPIcon");
					}
					drawTexture13.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					uIListItemContainer.MakeChild(drawTexture13.gameObject);
					drawTexture13.gameObject.transform.localPosition = new Vector3(1f, -1f, -0.02f);
				}
			}
			return uIListItemContainer;
		}

		public void AddListItem(UIListItemContainer item)
		{
			base.InsertItemDonotPosionUpdate(item, base.Count, null, false);
		}

		private void ItemEffect(IUIObject control, GameObject obj)
		{
			if (control == null || null == obj)
			{
				return;
			}
			DrawTexture drawTexture = (DrawTexture)control;
			if (null == drawTexture)
			{
				return;
			}
			if ((int)drawTexture.data == 260)
			{
				obj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
			}
			else if ((int)drawTexture.data == 86)
			{
				obj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
			}
			else if ((int)drawTexture.data == 429)
			{
				obj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
			}
			else
			{
				obj.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
			}
			obj.transform.localPosition = new Vector3(drawTexture.width / 2f, -drawTexture.height / 2f, drawTexture.GetLocation().z - 0.1f);
		}
	}
}
