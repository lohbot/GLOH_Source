using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class ItemSellDlg : Form
{
	private ItemTexture m_iItemIcon;

	private DrawTexture m_dIconBack;

	private Label m_lItemName;

	private Label m_lSellGold;

	private Label m_lItemCount;

	private Label m_lExpectNum;

	private Label m_lTitle;

	private Label m_lBubbleText;

	private DrawTexture m_dNpcImg;

	private Button m_bSellOK;

	private Button m_btClose;

	private ITEM m_cItem;

	private List<ITEM> m_cAllItem;

	private int m_nAllItemMoney;

	private bool m_bRankItem;

	private UIButton _GuideItem;

	private float _ButtonZ;

	private int m_nWinID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Item/DLG_ItemSell", G_ID.ITEMSELL_DLG, false);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_iItemIcon = (base.GetControl("ItemTexture_ItemIcon") as ItemTexture);
		this.m_dIconBack = (base.GetControl("DrawTexture_ItemBG") as DrawTexture);
		this.m_lItemName = (base.GetControl("Label_ItemName") as Label);
		this.m_lSellGold = (base.GetControl("Label_Gold") as Label);
		this.m_lItemCount = (base.GetControl("Label_ItemNum") as Label);
		this.m_lExpectNum = (base.GetControl("Label_ExpectNum") as Label);
		this.m_dNpcImg = (base.GetControl("DrawTexture_NPCIMG") as DrawTexture);
		this.m_lTitle = (base.GetControl("Label_title") as Label);
		this.m_lBubbleText = (base.GetControl("Label_BubbleText") as Label);
		this.m_bSellOK = (base.GetControl("Button_OK") as Button);
		Button expr_E2 = this.m_bSellOK;
		expr_E2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E2.Click, new EZValueChangedDelegate(this.OnSellOKCheck));
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		base.SetScreenCenter();
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
	}

	public void SetMultiItemSellInfo(List<ITEM> l_cItem, int nAllMoney, ITEM TopItem, bool bRankItem)
	{
		if (TopItem == null || TopItem.m_nItemUnique <= 0)
		{
			return;
		}
		this.m_cAllItem = l_cItem;
		this.m_nAllItemMoney = nAllMoney;
		this.m_bRankItem = bRankItem;
		this.m_cItem = null;
		this.m_iItemIcon.SetItemTexture(TopItem);
		this.m_iItemIcon.c_cItemTooltip = TopItem;
		this.m_dIconBack.SetTexture(TopItem.GetRankImage());
		this.m_lItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(TopItem);
		this.m_lSellGold.Text = Protocol_Item.Money_Format((long)nAllMoney);
		string empty = string.Empty;
		int num = l_cItem.Count - 1;
		if (num > 0)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("295"),
				"count",
				num.ToString()
			});
			this.m_lItemCount.SetText(empty);
		}
		else
		{
			this.m_lItemCount.SetText(string.Empty);
		}
		this.m_dNpcImg.SetTexture(eCharImageType.LARGE, 242, -1, string.Empty);
	}

	public void SetMultiItemBreakInfo(List<ITEM> l_cItem, ITEM TopItem, bool bRankItem, int ItemUnique, int ExpectMin, int ExpectMax)
	{
		if (TopItem == null || TopItem.m_nItemUnique <= 0)
		{
			return;
		}
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		this.m_bSellOK.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("420"));
		this.m_bSellOK.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnSellOKCheck));
		this.m_bSellOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemBreakOK));
		this.m_lTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2958"));
		this.m_lBubbleText.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2959"));
		this.m_cAllItem = l_cItem;
		this.m_bRankItem = bRankItem;
		this.m_cItem = null;
		this.m_iItemIcon.SetItemTexture(TopItem);
		this.m_iItemIcon.c_cItemTooltip = TopItem;
		this.m_dIconBack.SetTexture(TopItem.GetRankImage());
		this.m_lItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(TopItem);
		string empty = string.Empty;
		int num = l_cItem.Count - 1;
		if (num > 0)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("295"),
				"count",
				num.ToString()
			});
			this.m_lItemCount.SetText(empty);
		}
		else
		{
			this.m_lItemCount.SetText(string.Empty);
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2951"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(ItemUnique),
			"min",
			ExpectMin,
			"max",
			ExpectMax
		});
		this.m_lExpectNum.SetText(empty);
		this.m_dNpcImg.SetTexture(eCharImageType.LARGE, 242, -1, string.Empty);
	}

	public void SetItemBreak(ITEM cItem, int ItemUnique, int ExpectMin, int ExpectMax)
	{
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		this.m_bSellOK.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("420"));
		this.m_bSellOK.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnSellOKCheck));
		this.m_bSellOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemBreakOK));
		this.m_lTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2958"));
		this.m_lBubbleText.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2959"));
		this.m_cItem = cItem;
		this.m_iItemIcon.SetItemTexture(this.m_cItem);
		this.m_iItemIcon.c_cItemTooltip = this.m_cItem;
		this.m_dIconBack.SetTexture(this.m_cItem.GetRankImage());
		this.m_lItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_cItem);
		this.m_lItemCount.Visible = false;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2951"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(ItemUnique),
			"min",
			ExpectMin,
			"max",
			ExpectMax
		});
		this.m_lExpectNum.SetText(empty);
		this.m_dNpcImg.SetTexture(eCharImageType.LARGE, 242, -1, string.Empty);
	}

	public void SetItemSellInfo(ITEM cItem, int nMoney)
	{
		this.m_cItem = cItem;
		this.m_iItemIcon.SetItemTexture(this.m_cItem);
		this.m_iItemIcon.c_cItemTooltip = this.m_cItem;
		this.m_dIconBack.SetTexture(this.m_cItem.GetRankImage());
		this.m_lItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_cItem);
		this.m_lSellGold.Text = Protocol_Item.Money_Format((long)nMoney);
		this.m_lItemCount.Visible = false;
		this.m_dNpcImg.SetTexture(eCharImageType.LARGE, 242, -1, string.Empty);
	}

	public override void InitData()
	{
	}

	public void OnSellOKCheck(IUIObject obj)
	{
		if (this.m_cItem != null && this.m_cItem.IsLock())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("726"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			return;
		}
		if (this.m_bRankItem)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI == null)
			{
				return;
			}
			msgBoxUI.SetMsg(new YesDelegate(this.OnSellOKSend), null, new NoDelegate(this.OnSellCancelSend), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("57"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("67"), eMsgType.MB_OK_CANCEL);
		}
		else
		{
			this.OnSellOK();
		}
	}

	public void OnSellCancelSend(object a_oObject)
	{
		this.Close();
	}

	public void OnSellOKSend(object a_oObject)
	{
		this.OnSellOK();
	}

	public void OnSellOK()
	{
		if (this.m_cItem != null)
		{
			if (Protocol_Market.s_lsSellItem.Count >= 30)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("237");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			Protocol_Item.Send_AutoItemSell(this.m_cItem.m_nItemID);
		}
		else
		{
			GS_ITEM_SELL_MULTI_REQ gS_ITEM_SELL_MULTI_REQ = new GS_ITEM_SELL_MULTI_REQ();
			for (int i = 0; i < this.m_cAllItem.Count; i++)
			{
				if (this.m_cAllItem[i] != null && this.m_cAllItem[i].m_nItemUnique > 0)
				{
					gS_ITEM_SELL_MULTI_REQ.i64ItemID[i] = this.m_cAllItem[i].m_nItemID;
				}
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SELL_MULTI_REQ, gS_ITEM_SELL_MULTI_REQ);
		}
		this.HideUIGuide();
		this.Close();
	}

	public void OnItemBreakOK(object a_oObject)
	{
		Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
		if (inventory_Dlg != null)
		{
			inventory_Dlg.ActionItemBreak();
		}
		this.HideUIGuide();
		this.Close();
	}

	public override void OnClose()
	{
		Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
		if (inventory_Dlg != null && this.m_nAllItemMoney != 0)
		{
			inventory_Dlg.InitMultiItem();
		}
		this.HideUIGuide();
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		this._GuideItem = (base.GetControl(param1) as UIButton);
		this.m_nWinID = winID;
		if (null != this._GuideItem)
		{
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				this._GuideItem.EffectAni = false;
				Vector2 vector = new Vector2(base.GetLocationX() + this._GuideItem.GetLocationX() + 80f, base.GetLocationY() + this._GuideItem.GetLocationY() - 10f);
				uI_UIGuide.Move(vector, vector);
				this._ButtonZ = this._GuideItem.GetLocation().z;
				this._GuideItem.SetLocationZ(uI_UIGuide.GetLocation().z - base.GetLocation().z - 1f);
				this._GuideItem.AlphaAni(1f, 0.8f, -0.5f);
			}
		}
		else
		{
			Debug.LogError("_GuideItem == NULL");
		}
	}

	public void HideUIGuide()
	{
		if (null != this._GuideItem)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			this._GuideItem.SetLocationZ(this._ButtonZ);
			this._GuideItem.StopAni();
			this._GuideItem.AlphaAni(1f, 1f, 0f);
		}
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
			uI_UIGuide.Close();
		}
		this._GuideItem = null;
	}
}
