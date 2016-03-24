using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class AuctionSellCheckDlg : Form
{
	private DrawTexture m_dtItemBG;

	private ItemTexture m_itItem;

	private Label m_lbItemName;

	private Label m_lbTradeCount;

	private Label m_lbCost;

	private DrawTexture m_dtCost;

	private Label m_lbDirectCost;

	private DrawTexture m_dtDirectCost;

	private Label m_lbSellRegisterCost;

	private Button m_btDetailInfo;

	private Button m_btOK;

	private Button m_btCancel;

	private ITEM m_SelectItem;

	private NkSoldierInfo m_SelectSoldierInfo;

	private long m_lSellCost;

	private long m_lSellDirectCost;

	private long m_lSellRegisterCost;

	private AuctionDefine.ePAYTYPE m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD;

	private string m_strText = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Auction/DLG_AuctionSellCheck", G_ID.AUCTION_SELLCHECK_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtItemBG = (base.GetControl("DrawTexture_ItemBG") as DrawTexture);
		this.m_itItem = (base.GetControl("ItemTexture_ItemIcon") as ItemTexture);
		this.m_lbItemName = (base.GetControl("Label_ItemName") as Label);
		this.m_lbTradeCount = (base.GetControl("Label_TradeCount") as Label);
		this.m_btDetailInfo = (base.GetControl("Button_DetailInfo") as Button);
		this.m_btDetailInfo.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDetailInfo));
		this.m_lbCost = (base.GetControl("Label_Cost01") as Label);
		this.m_dtCost = (base.GetControl("Icon_Money01") as DrawTexture);
		this.m_lbDirectCost = (base.GetControl("Label_Cost02") as Label);
		this.m_dtDirectCost = (base.GetControl("Icon_Money02") as DrawTexture);
		this.m_lbSellRegisterCost = (base.GetControl("Label_Cost03") as Label);
		this.m_btOK = (base.GetControl("Button_OK") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		this.m_btCancel = (base.GetControl("Button_NO") as Button);
		this.m_btCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCancel));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClickDetailInfo(IUIObject obj)
	{
		if (this.m_SelectItem != null)
		{
			AuctionMainDlg.ShowItemDetailInfo(this.m_SelectItem, (G_ID)base.WindowID);
		}
		else if (this.m_SelectSoldierInfo != null)
		{
			AuctionMainDlg.ShowSolDetailInfo(this.m_SelectSoldierInfo, this);
		}
	}

	public void ClickOK(IUIObject obj)
	{
		if (this.m_SelectSoldierInfo != null)
		{
			if (this.m_SelectSoldierInfo.IsEquipItem())
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI == null)
				{
					return;
				}
				msgBoxUI.SetMsg(new YesDelegate(AuctionSellCheckDlg.MessageBoxEquipItem), this.m_SelectSoldierInfo, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1107"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("189"), eMsgType.MB_OK_CANCEL, 2);
				msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("980"));
				msgBoxUI.Show();
				return;
			}
			else if (0L < this.m_SelectSoldierInfo.GetFriendPersonID())
			{
				MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI2 == null)
				{
					return;
				}
				msgBoxUI2.SetMsg(new YesDelegate(this.MsgBoxOKUnsetSolHelp), this.m_SelectSoldierInfo, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("156"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("155"), eMsgType.MB_OK_CANCEL, 2);
				msgBoxUI2.Show();
				return;
			}
		}
		if (!this.IsRegister())
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_SELLCHECK_DLG);
			return;
		}
		GS_AUCTION_REGISTER_REQ gS_AUCTION_REGISTER_REQ = new GS_AUCTION_REGISTER_REQ();
		if (this.m_SelectItem != null)
		{
			gS_AUCTION_REGISTER_REQ.i32ItemPos = this.m_SelectItem.m_nItemPos;
			gS_AUCTION_REGISTER_REQ.i32PosType = this.m_SelectItem.m_nPosType;
		}
		if (this.m_SelectSoldierInfo != null)
		{
			gS_AUCTION_REGISTER_REQ.i64SolID = this.m_SelectSoldierInfo.GetSolID();
		}
		if (this.m_ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
		{
			gS_AUCTION_REGISTER_REQ.i64CostMoney = this.m_lSellCost;
			gS_AUCTION_REGISTER_REQ.i64DirectCostMoney = this.m_lSellDirectCost;
		}
		else
		{
			gS_AUCTION_REGISTER_REQ.i32CostHearts = (int)this.m_lSellCost;
			gS_AUCTION_REGISTER_REQ.i32DirectCostHearts = (int)this.m_lSellDirectCost;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUCTION_REGISTER_REQ, gS_AUCTION_REGISTER_REQ);
		this.SetEnableControl(false);
	}

	public void ClickCancel(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_SELLCHECK_DLG);
	}

	public void SetItemInfo(ITEM SelectItem, long lSellCost, long lSellDirectCoust, AuctionDefine.ePAYTYPE ePayType)
	{
		this.m_SelectItem = SelectItem;
		this.m_lSellCost = lSellCost;
		this.m_lSellDirectCost = lSellDirectCoust;
		this.m_ePayType = ePayType;
		this.m_itItem.SetItemTexture(this.m_SelectItem);
		int rank = this.m_SelectItem.m_nOption[2];
		string str = ItemManager.RankTextColor(rank);
		string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(this.m_SelectItem);
		this.m_lbItemName.SetText(str + rankColorName);
		this.m_dtItemBG.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString(rank));
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			this.m_SelectItem.m_nOption[7]
		});
		this.m_lbTradeCount.SetText(this.m_strText);
		this.ShowPayInfo();
	}

	public void SetSolInfo(NkSoldierInfo SelectSol, long lSellCost, long lSellDirectCoust, AuctionDefine.ePAYTYPE ePayType)
	{
		this.m_SelectSoldierInfo = SelectSol;
		this.m_lSellCost = lSellCost;
		this.m_lSellDirectCost = lSellDirectCoust;
		this.m_ePayType = ePayType;
		NkListSolInfo listSolInfo = this.m_SelectSoldierInfo.GetListSolInfo(true);
		listSolInfo.ShowLevel = true;
		this.m_itItem.SetSolImageTexure(eCharImageType.SMALL, listSolInfo, false);
		this.m_lbItemName.SetText(this.m_SelectSoldierInfo.GetName());
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1026"),
			"count",
			this.m_SelectSoldierInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_TRADE_COUNT)
		});
		this.m_lbTradeCount.SetText(this.m_strText);
		this.ShowPayInfo();
	}

	public bool IsRegister()
	{
		short auctionUseLevel = NrTSingleton<ContentsLimitManager>.Instance.GetAuctionUseLevel();
		if (0 >= auctionUseLevel)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		if (auctionUseLevel > (short)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel())
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("628"),
				"count",
				auctionUseLevel
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		if (0L >= this.m_lSellRegisterCost)
		{
			return false;
		}
		if (this.m_lSellRegisterCost > NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("1255"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		if (0L < this.m_lSellDirectCost)
		{
			long directCostMin = AuctionMainDlg.GetDirectCostMin(this.m_ePayType, this.m_lSellCost);
			if (this.m_lSellDirectCost < directCostMin)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("271"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
		}
		if (this.m_SelectItem != null)
		{
		}
		if (this.m_SelectSoldierInfo != null)
		{
			if (this.m_SelectSoldierInfo.GetLevel() < AuctionMainDlg.GetSolLevelLimit())
			{
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("649"),
					"level1",
					AuctionMainDlg.GetSolLevelLimit(),
					"level2",
					AuctionMainDlg.GetSolSkillLevelLimit()
				});
				Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
			if (this.m_SelectSoldierInfo.GetLevel() < AuctionMainDlg.GetSolSkillLevelLimit())
			{
				string empty3 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("649"),
					"level1",
					AuctionMainDlg.GetSolLevelLimit(),
					"level2",
					AuctionMainDlg.GetSolSkillLevelLimit()
				});
				Main_UI_SystemMessage.ADDMessage(empty3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
			if (this.m_SelectSoldierInfo.IsEquipItem())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("304"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
			if (this.m_SelectSoldierInfo.IsInjuryStatus())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("407"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
			if (this.m_SelectSoldierInfo.IsAwakening())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("814"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
			if (this.m_SelectSoldierInfo.IsAtbCommonFlag(1L))
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("879"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return false;
			}
		}
		int num = (int)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetCharDetail(19);
		if (AuctionMainDlg.GetDailySellLimit() <= num)
		{
			string empty4 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty4, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("656"),
				"count",
				num
			});
			Main_UI_SystemMessage.ADDMessage(empty4, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		return true;
	}

	public static long GetRegisterCost(ITEM Item, long lSellCost, AuctionDefine.ePAYTYPE ePayType)
	{
		long result = 0L;
		if (Item == null)
		{
			return result;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(Item.m_nItemUnique);
		if (itemInfo != null)
		{
			int num = (int)AuctionMainDlg.GetAuctionSellPrice();
			long num2 = (long)(itemInfo.m_nUseMinLevel * num);
			float auctionSellPriceRate = AuctionMainDlg.GetAuctionSellPriceRate();
			long num3 = (long)((float)lSellCost * auctionSellPriceRate);
			if (ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
			{
				int heartsValue = AuctionMainDlg.GetHeartsValue();
				if (0 < heartsValue)
				{
					num3 *= (long)heartsValue;
				}
			}
			result = num2;
			if (num2 < num3)
			{
				result = num3;
			}
		}
		return result;
	}

	public static long GetRegisterCost(NkSoldierInfo SoldierInfo, long lSellCost, AuctionDefine.ePAYTYPE ePayType)
	{
		long result = 0L;
		if (SoldierInfo == null)
		{
			return result;
		}
		int num = (int)AuctionMainDlg.GetAuctionSellPrice();
		long num2 = (long)((int)SoldierInfo.GetLevel() * num);
		float auctionSellPriceRate = AuctionMainDlg.GetAuctionSellPriceRate();
		long num3 = (long)((float)lSellCost * auctionSellPriceRate);
		if (ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
		{
			int heartsValue = AuctionMainDlg.GetHeartsValue();
			if (0 < heartsValue)
			{
				num3 *= (long)heartsValue;
			}
		}
		result = num2;
		if (num2 < num3)
		{
			result = num3;
		}
		return result;
	}

	public void SetEnableControl(bool bEnable)
	{
		this.m_btOK.SetEnabled(bEnable);
		this.m_btCancel.SetEnabled(bEnable);
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public void ShowPayInfo()
	{
		this.m_lbCost.SetText(ANNUALIZED.Convert(this.m_lSellCost));
		if (0L >= this.m_lSellDirectCost)
		{
			this.m_lbDirectCost.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("48"));
		}
		else
		{
			this.m_lbDirectCost.SetText(ANNUALIZED.Convert(this.m_lSellDirectCost));
		}
		long lSellCost = this.m_lSellCost;
		if (0L < this.m_lSellDirectCost)
		{
			lSellCost = this.m_lSellDirectCost;
		}
		if (this.m_SelectItem != null)
		{
			this.m_lSellRegisterCost = AuctionSellCheckDlg.GetRegisterCost(this.m_SelectItem, lSellCost, this.m_ePayType);
		}
		else
		{
			this.m_lSellRegisterCost = AuctionSellCheckDlg.GetRegisterCost(this.m_SelectSoldierInfo, lSellCost, this.m_ePayType);
		}
		this.m_lbSellRegisterCost.SetText(ANNUALIZED.Convert(this.m_lSellRegisterCost));
		AuctionDefine.ePAYTYPE ePayType = this.m_ePayType;
		if (ePayType != AuctionDefine.ePAYTYPE.ePAYTYPE_HEARTS)
		{
			if (ePayType == AuctionDefine.ePAYTYPE.ePAYTYPE_GOLD)
			{
				this.m_dtCost.SetTextureKey("Com_I_MoneyIconM");
				this.m_dtDirectCost.SetTextureKey("Com_I_MoneyIconM");
			}
		}
		else
		{
			this.m_dtCost.SetTextureKey("Win_I_Hearts");
			this.m_dtDirectCost.SetTextureKey("Win_I_Hearts");
		}
	}

	public void MsgBoxOKUnsetSolHelp(object a_oObject)
	{
		NkSoldierInfo nkSoldierInfo = a_oObject as NkSoldierInfo;
		if (nkSoldierInfo == null)
		{
			return;
		}
		GS_FRIEND_HELPSOL_UNSET_REQ gS_FRIEND_HELPSOL_UNSET_REQ = new GS_FRIEND_HELPSOL_UNSET_REQ();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64FriendPersonID = nkSoldierInfo.GetFriendPersonID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64SolID = nkSoldierInfo.GetSolID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64AddExp = nkSoldierInfo.AddHelpExp;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_HELPSOL_UNSET_REQ, gS_FRIEND_HELPSOL_UNSET_REQ);
	}

	public static void MessageBoxEquipItem(object a_oObject)
	{
		NkSoldierInfo nkSoldierInfo = a_oObject as NkSoldierInfo;
		if (nkSoldierInfo == null)
		{
			return;
		}
		Protocol_Item.Send_EquipSol_InvenEquip_All(nkSoldierInfo);
	}
}
