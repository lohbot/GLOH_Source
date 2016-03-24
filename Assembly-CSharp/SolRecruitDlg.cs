using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolRecruitDlg : Form
{
	private enum eSolTicketType
	{
		STT_CASH,
		STT_PLATINUM,
		STT_GOLD,
		STT_SILVER,
		STT_BRONZE,
		MAX_STT_NUM
	}

	private Button m_HelpButton;

	protected DrawTexture m_BackImage;

	private Button m_btPrimiumMall;

	protected NewListBox m_TicketList;

	private List<TICKET_SELL_INFO> m_TicketSellInfoList = new List<TICKET_SELL_INFO>();

	private float m_fScrollPosition = -1f;

	private short m_i16SolRecruitImageIndex;

	private ITEM m_OpenTicket;

	private bool m_ReUseTicket;

	private int m_OpenTicketCardType;

	private bool bShowReUseTicket = true;

	private float m_fRecruitPassCheckTime;

	private bool bRecruitPassCheck = true;

	private string m_backImgKey = string.Empty;

	protected int m_nWinID;

	private UIListItemContainer _GuideItem;

	private bool m_bTutorial;

	private float m_fRequestTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/Recruit/DLG_SolRecruit_New", G_ID.SOLRECRUIT_DLG, true);
		base.InteractivePanel.draggable = false;
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
		this.m_backImgKey = string.Empty;
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.depthChangeable = false;
		}
		int num = int.Parse(param1);
		int num2 = -1;
		for (int i = 0; i < this.m_TicketList.Count; i++)
		{
			if (num == (int)this.m_TicketList.GetItem(i).Data)
			{
				this._GuideItem = this.m_TicketList.GetItem(i);
				num2 = i + 1;
				break;
			}
		}
		if (null == this._GuideItem)
		{
			return;
		}
		this.m_nWinID = winID;
		this._GuideItem.gameObject.transform.localPosition = new Vector3(this._GuideItem.gameObject.transform.localPosition.x, this._GuideItem.gameObject.transform.localPosition.y, -1f);
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide == null)
		{
			return;
		}
		Vector3 v = new Vector2(base.GetLocationX() + this.m_TicketList.GetLocationX() + this.m_TicketList.GetSize().x / 2f - 80f, base.GetLocationY() + this.m_TicketList.LineHeight * (float)num2);
		Vector2 x = new Vector2(base.GetLocationX() + this.m_BackImage.GetSize().x + this.m_TicketList.GetSize().x / 4f, base.GetLocationY() + this.m_TicketList.LineHeight * (float)num2 - 50f);
		uI_UIGuide.Move(v, x);
		this.m_bTutorial = true;
	}

	public override void SetComponent()
	{
		this.m_BackImage = (base.GetControl("DrawTexture_ADImg01") as DrawTexture);
		this.m_TicketList = (base.GetControl("Listbox_TicketList") as NewListBox);
		this.m_TicketList.Reserve = false;
		this.m_btPrimiumMall = (base.GetControl("Button_premium") as Button);
		this.m_btPrimiumMall.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrimiumMall));
		this.m_HelpButton = (base.GetControl("Help_Button") as Button);
		this.m_HelpButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		base.SetScreenCenter();
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		ITEMMALL_POPUPSHOP atbToIDX = NrTSingleton<ItemMallPoPupShopManager>.Instance.GetAtbToIDX(ItemMallPoPupShopManager.ePoPupShop_Type.RecruitOpen);
		if (atbToIDX != null)
		{
			GS_ITEMSHOP_ITEMPOPUP_INFO_REQ gS_ITEMSHOP_ITEMPOPUP_INFO_REQ = new GS_ITEMSHOP_ITEMPOPUP_INFO_REQ();
			gS_ITEMSHOP_ITEMPOPUP_INFO_REQ.i64PersonID = myCharInfo.m_PersonID;
			gS_ITEMSHOP_ITEMPOPUP_INFO_REQ.i64Idx = (long)atbToIDX.m_Idx;
			gS_ITEMSHOP_ITEMPOPUP_INFO_REQ.i32ATB = 3;
			SendPacket.GetInstance().SendObject(2536, gS_ITEMSHOP_ITEMPOPUP_INFO_REQ);
		}
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("user_card");
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("solrecruitdlg_open");
		NrTSingleton<NkClientLogic>.Instance.SetCanOpenTicket(true);
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY-EMPLOYMENT", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void Update()
	{
		if (this.m_fRecruitPassCheckTime > 0f && Time.time - this.m_fRecruitPassCheckTime > 2f && this.bRecruitPassCheck)
		{
			this.m_fRecruitPassCheckTime = 0f;
			this.SetRecruitButtonEnable(true);
		}
	}

	public virtual void SetTicketList()
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2016");
		string empty = string.Empty;
		this.m_TicketList.Clear();
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsLegendHire())
		{
			textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2862");
			NewListItem newListItem = new NewListItem(this.m_TicketList.ColumnNum, true, string.Empty);
			int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ESSENCE_ITEMUNIQUE);
			int needItemNum = SolRecruitDlg.GetNeedItemNum(13);
			ITEM item = NkUserInventory.GetInstance().GetItem(value);
			if (item != null)
			{
				newListItem.SetListItemData(1, item, null, null, null);
				if (item.m_nItemNum >= needItemNum)
				{
					newListItem.SetListItemEnable(3, true);
				}
				else
				{
					newListItem.SetListItemEnable(3, false);
				}
			}
			else
			{
				newListItem.SetListItemData(1, NrTSingleton<ItemManager>.Instance.GetItemTexture(value), null, null, null);
				newListItem.SetListItemEnable(3, false);
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"targetname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(value),
				"count1",
				needItemNum,
				"count2",
				SolRecruitDlg.GetSolCount(13)
			});
			newListItem.SetListItemData(2, empty, null, null, null);
			newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("573"), eSolRecruitType.SOLRECRUIT_ESSENCE_ONE, new EZValueChangedDelegate(this.ClickTicketList), null);
			newListItem.SetListItemData(4, false);
			newListItem.SetListItemData(6, false);
			newListItem.Data = eSolRecruitType.SOLRECRUIT_ESSENCE_ONE;
			this.m_TicketList.Add(newListItem);
		}
		this.AddTicketSellInfo();
		List<ITEM> functionItemsByInvenType = NkUserInventory.instance.GetFunctionItemsByInvenType(6, eITEM_SUPPLY_FUNCTION.SUPPLY_GETSOLDIER);
		foreach (ITEM current in functionItemsByInvenType)
		{
			if (NrTSingleton<ItemManager>.Instance.IsItemATB(current.m_nItemUnique, 2048L))
			{
				NewListItem newListItem2 = new NewListItem(this.m_TicketList.ColumnNum, true, string.Empty);
				string name = NrTSingleton<ItemManager>.Instance.GetName(current);
				newListItem2.SetListItemData(1, current, null, null, null);
				newListItem2.SetListItemData(2, name, null, null, null);
				newListItem2.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("573"), current, new EZValueChangedDelegate(this.ClickTicketList), null);
				newListItem2.SetListItemData(4, false);
				newListItem2.SetListItemData(5, string.Empty, current.m_nItemID, new EZValueChangedDelegate(this.ClickTicketItemToolTip), null);
				newListItem2.SetListItemData(6, false);
				newListItem2.Data = current.m_nItemUnique;
				this.m_TicketList.Add(newListItem2);
			}
		}
		foreach (ITEM current2 in functionItemsByInvenType)
		{
			if (!NrTSingleton<ItemManager>.Instance.IsItemATB(current2.m_nItemUnique, 2048L))
			{
				NewListItem newListItem3 = new NewListItem(this.m_TicketList.ColumnNum, true, string.Empty);
				string name2 = NrTSingleton<ItemManager>.Instance.GetName(current2);
				newListItem3.SetListItemData(1, current2, null, null, null);
				newListItem3.SetListItemData(2, name2, null, null, null);
				newListItem3.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("573"), current2, new EZValueChangedDelegate(this.ClickTicketList), null);
				newListItem3.SetListItemData(4, false);
				newListItem3.SetListItemData(5, string.Empty, current2.m_nItemID, new EZValueChangedDelegate(this.ClickTicketItemToolTip), null);
				newListItem3.SetListItemData(6, false);
				this.m_TicketList.Add(newListItem3);
			}
		}
		this.m_TicketList.RepositionItems();
		this.SetLastSelectItemScrol();
	}

	protected virtual void ClickTicketList(IUIObject obj)
	{
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.UIGUIDE_DLG);
		}
		if (obj == null)
		{
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.GetCanOpenTicket())
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (obj.Data is eSolRecruitType)
		{
			eSolRecruitType eSolRecruitType = (eSolRecruitType)((int)obj.Data);
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1699");
			string text = string.Empty;
			if (eSolRecruitType != eSolRecruitType.SOLRECRUIT_VOUCHER_FREE_HEARTS_RECRUIT)
			{
				if (eSolRecruitType == eSolRecruitType.SOLRECRUIT_ESSENCE_ONE)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("296");
				}
				else
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("127");
				}
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					text,
					"count",
					SolRecruitDlg.GetNeedItemNum((int)eSolRecruitType)
				});
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("233");
			}
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), eSolRecruitType, null, null, textFromInterface, text, eMsgType.MB_OK_CANCEL);
			}
		}
		else
		{
			ITEM iTEM = (ITEM)obj.Data;
			if (iTEM != null)
			{
				this.SetGetNowOpenTicket(iTEM);
				this.SolTicketOpen();
				if (this.m_OpenTicket.m_nItemNum > 1)
				{
					this.m_ReUseTicket = true;
				}
				if (this.m_bTutorial)
				{
					this.Close();
				}
			}
		}
		this.m_fRequestTime = Time.time;
		this.m_fScrollPosition = this.m_TicketList.ScrollPosition;
	}

	public bool GetReUseTicket()
	{
		return this.bShowReUseTicket && this.m_ReUseTicket;
	}

	public void SetShowReUseTicket(bool ReUseTicket)
	{
		this.bShowReUseTicket = ReUseTicket;
	}

	private void SetGetNowOpenTicket(ITEM TicketItem)
	{
		if (TicketItem != null)
		{
			this.m_OpenTicket = TicketItem;
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(TicketItem.m_nItemUnique);
			if (itemInfo != null)
			{
				this.m_OpenTicketCardType = itemInfo.m_nCardType;
			}
		}
	}

	public int GetNowOpenTicketCardType()
	{
		return this.m_OpenTicketCardType;
	}

	public ITEM GetNowOpenTicket()
	{
		return this.m_OpenTicket;
	}

	public void SolTicketOpen()
	{
		if (this.m_OpenTicket == null)
		{
			return;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null || readySolList.GetCount() >= 100)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (this.m_OpenTicket.m_nItemNum >= 2 && this.GetNowOpenTicketCardType() == 0)
		{
			ItemBoxContinue_Dlg itemBoxContinue_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_CONTINUE_DLG) as ItemBoxContinue_Dlg;
			if (itemBoxContinue_Dlg == null)
			{
				return;
			}
			itemBoxContinue_Dlg.SetItemData(this.m_OpenTicket, ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_TICKET);
			return;
		}
		else
		{
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_OpenTicket.m_nItemUnique);
			if (itemInfo == null)
			{
				return;
			}
			NrTSingleton<NkClientLogic>.Instance.SetCanOpenTicket(false);
			if (itemInfo.IsItemATB(2048L) || itemInfo.IsItemATB(32768L))
			{
				this.SetRecruitButtonEnable(false);
				Protocol_Item.Item_Use(this.m_OpenTicket);
			}
			else
			{
				this.ExcuteTicket(this.m_OpenTicket.m_nItemUnique, itemInfo.m_nRecruitType, itemInfo.m_nParam[1], false);
			}
			return;
		}
	}

	public void SolTicketReOpen()
	{
		if (this.m_OpenTicket == null)
		{
			return;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null || readySolList.GetCount() >= 100)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_OpenTicket.m_nItemUnique) == null)
		{
			return;
		}
		this.SetRecruitButtonEnable(false);
		GS_SOLDIER_RECRUIT_REQ gS_SOLDIER_RECRUIT_REQ = default(GS_SOLDIER_RECRUIT_REQ);
		gS_SOLDIER_RECRUIT_REQ.ItemUnique = this.m_OpenTicket.m_nItemUnique;
		gS_SOLDIER_RECRUIT_REQ.RecruitType = 20;
		gS_SOLDIER_RECRUIT_REQ.SubData = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_RECRUIT_REQ, gS_SOLDIER_RECRUIT_REQ);
		this.SetShowReUseTicket(false);
	}

	public void ExcuteTicket(int itemunique, int recruittype, int season, bool bForceRecruit)
	{
		int solCount = SolRecruitDlg.GetSolCount(recruittype);
		if (!bForceRecruit)
		{
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null || readySolList.GetCount() + solCount - 1 >= 100)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
		}
		if (NkUserInventory.GetInstance().GetFirstItemByUnique(itemunique) == null)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("506");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		NrTSingleton<NkClientLogic>.Instance.SetCanOpenTicket(false);
		this.SetRecruitButtonEnable(false);
		GS_SOLDIER_RECRUIT_REQ gS_SOLDIER_RECRUIT_REQ = default(GS_SOLDIER_RECRUIT_REQ);
		gS_SOLDIER_RECRUIT_REQ.ItemUnique = itemunique;
		gS_SOLDIER_RECRUIT_REQ.RecruitType = recruittype;
		gS_SOLDIER_RECRUIT_REQ.SubData = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_RECRUIT_REQ, gS_SOLDIER_RECRUIT_REQ);
	}

	public void SetRecruitButtonEnable(bool bEnable)
	{
		if (bEnable)
		{
			if (this.m_fRecruitPassCheckTime > 0f)
			{
				this.bRecruitPassCheck = true;
				return;
			}
		}
		else
		{
			this.m_fRecruitPassCheckTime = Time.time;
		}
		for (int i = 0; i < this.m_TicketList.Count; i++)
		{
			UIListItemContainer item = this.m_TicketList.GetItem(i);
			if (item != null)
			{
				UIButton uIButton = item.GetElement(3) as UIButton;
				if (uIButton != null)
				{
					uIButton.SetEnabled(bEnable);
				}
			}
		}
	}

	public override void OnClose()
	{
		if (NrTSingleton<FormsManager>.Instance.IsPopUPDlgNotExist(base.WindowID))
		{
			NrTSingleton<UIImageBundleManager>.Instance.DeleteTexture();
		}
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY-EMPLOYMENT", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
	}

	public static int GetNeedItemNum(int recruittype)
	{
		int result = 0;
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return 0;
		}
		switch (recruittype)
		{
		case 0:
			result = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CASHCOUNT_FORONE);
			break;
		case 1:
			result = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CASHCOUNT_FORTEN);
			break;
		case 13:
			result = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ESSENCECOUNT_FORONE);
			break;
		}
		return result;
	}

	public static int GetSolCount(int recruittype)
	{
		int result = 0;
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return 0;
		}
		switch (recruittype)
		{
		case 0:
		case 2:
		case 3:
		case 4:
		case 5:
		case 7:
		case 13:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
			result = 1;
			break;
		case 1:
			result = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_SOLCOUNT_FORTEN);
			break;
		}
		return result;
	}

	private void MsgBoxOKEvent(object obj)
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		eSolRecruitType eSolRecruitType = (eSolRecruitType)((int)obj);
		int itemunique = 0;
		if (eSolRecruitType == eSolRecruitType.SOLRECRUIT_CASH_ONE || eSolRecruitType == eSolRecruitType.SOLRECRUIT_CASH_TEN)
		{
			itemunique = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CASH_ITEMUNIQUE);
		}
		else if (eSolRecruitType == eSolRecruitType.SOLRECRUIT_ESSENCE_ONE)
		{
			itemunique = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ESSENCE_ITEMUNIQUE);
		}
		bool bForceRecruit = false;
		if (eSolRecruitType == eSolRecruitType.SOLRECRUIT_CASH_TEN)
		{
			bForceRecruit = true;
		}
		this.ExcuteTicket(itemunique, (int)eSolRecruitType, 0, bForceRecruit);
	}

	private void MsgBoxOKSolRecruitCashTen(object obj)
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		eSolRecruitType recruittype = (eSolRecruitType)((int)obj);
		int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CASH_ITEMUNIQUE);
		this.ExcuteTicket(value, (int)recruittype, 0, true);
	}

	public void SetTicketSellInfo(GS_TICKET_SELL_INFO_ACK ACK, NkDeserializePacket kDeserializePacket)
	{
		this.m_TicketSellInfoList.Clear();
		for (int i = 0; i < (int)ACK.i16SolCount; i++)
		{
			this.m_TicketSellInfoList.Add(kDeserializePacket.GetPacket<TICKET_SELL_INFO>());
		}
		this.m_i16SolRecruitImageIndex = ACK.i16SolRecruitImageIndex;
		this.m_backImgKey = string.Format("UI/SolRecruit/SolRecruit_AD{0}", this.m_i16SolRecruitImageIndex);
		this.m_BackImage.SetTextureFromBundle(this.m_backImgKey);
		this.SetTicketList();
	}

	public void AddTicketSellInfo()
	{
		for (int i = 0; i < this.m_TicketSellInfoList.Count; i++)
		{
			TICKET_SELL_INFO tICKET_SELL_INFO = this.m_TicketSellInfoList[i];
			NewListItem newListItem = new NewListItem(this.m_TicketList.ColumnNum, true, string.Empty);
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(tICKET_SELL_INFO.i32CharKind);
			if (charKindInfo != null)
			{
				if (NrTSingleton<ContentsLimitManager>.Instance.IsTicketSell(tICKET_SELL_INFO.i32CharKind))
				{
					NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
					nkSoldierInfo.SetCharKind(tICKET_SELL_INFO.i32CharKind);
					byte b = tICKET_SELL_INFO.i8Grade;
					b -= 1;
					nkSoldierInfo.SetGrade(b);
					EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(tICKET_SELL_INFO.i32CharKind, tICKET_SELL_INFO.i8Grade);
					if (eventHeroCharCode != null)
					{
						newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
						newListItem.SetListItemData(4, true);
					}
					else
					{
						UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(tICKET_SELL_INFO.i32CharKind, (int)tICKET_SELL_INFO.i8Grade);
						if (legendFrame != null)
						{
							newListItem.SetListItemData(0, legendFrame, null, null, null);
						}
						newListItem.SetListItemData(4, false);
					}
					string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(tICKET_SELL_INFO.i32CharKind, (int)tICKET_SELL_INFO.i8Grade, charKindInfo.GetName());
					newListItem.SetListItemData(1, nkSoldierInfo.GetListSolInfo(false), null, null, null);
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2211"),
						"rank",
						tICKET_SELL_INFO.i8Grade,
						"solname",
						legendName,
						"count",
						tICKET_SELL_INFO.i32HeartsNum
					});
					newListItem.SetListItemData(2, empty, null, null, null);
					newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("573"), i, new EZValueChangedDelegate(this.ClickTicketSellInfo), null);
					if (NkUserInventory.GetInstance().Get_First_ItemCnt(70000) < tICKET_SELL_INFO.i32HeartsNum)
					{
						newListItem.SetListItemEnable(3, false);
					}
					else
					{
						newListItem.SetListItemEnable(3, true);
					}
					newListItem.Data = eSolRecruitType.SOLRECRUIT_TICKET_SELL_INFO;
					newListItem.SetListItemData(5, string.Empty, tICKET_SELL_INFO.i32CharKind, new EZValueChangedDelegate(this.ClickTicketSellInfoDetail), null);
					this.m_TicketList.Add(newListItem);
				}
			}
		}
	}

	private void ClickTicketSellInfo(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			return;
		}
		if (this.m_fRequestTime > 0f && Time.time - this.m_fRequestTime < 1f)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		int solCount = SolRecruitDlg.GetSolCount(7);
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null || readySolList.GetCount() + solCount - 1 >= 100)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		int num = (int)obj.Data;
		TICKET_SELL_INFO ticketSellInfo = this.GetTicketSellInfo(num);
		if (ticketSellInfo == null)
		{
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1699");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("127");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromMessageBox, new object[]
		{
			textFromMessageBox,
			"count",
			ticketSellInfo.i32HeartsNum
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKTicketSellInfo), num, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
		}
		this.m_fRequestTime = Time.time;
		this.m_fScrollPosition = this.m_TicketList.ScrollPosition;
	}

	private TICKET_SELL_INFO GetTicketSellInfo(int iIndex)
	{
		if (0 > iIndex || this.m_TicketSellInfoList.Count <= iIndex)
		{
			return null;
		}
		return this.m_TicketSellInfoList[iIndex];
	}

	private TICKET_SELL_INFO GetTicketSellInfoFromCharKind(int iCharKind)
	{
		for (int i = 0; i < this.m_TicketSellInfoList.Count; i++)
		{
			if (iCharKind == this.m_TicketSellInfoList[i].i32CharKind)
			{
				return this.m_TicketSellInfoList[i];
			}
		}
		return null;
	}

	private void MsgBoxOKTicketSellInfo(object obj)
	{
		if (COMMON_CONSTANT_Manager.GetInstance() == null)
		{
			return;
		}
		int iIndex = (int)obj;
		TICKET_SELL_INFO ticketSellInfo = this.GetTicketSellInfo(iIndex);
		if (ticketSellInfo == null)
		{
			return;
		}
		GS_TICKET_SELL_INFO_USE_REQ gS_TICKET_SELL_INFO_USE_REQ = new GS_TICKET_SELL_INFO_USE_REQ();
		gS_TICKET_SELL_INFO_USE_REQ.i32CharKind = ticketSellInfo.i32CharKind;
		gS_TICKET_SELL_INFO_USE_REQ.i8Grade = ticketSellInfo.i8Grade;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TICKET_SELL_INFO_USE_REQ, gS_TICKET_SELL_INFO_USE_REQ);
	}

	public void SetLastSelectItemScrol()
	{
		if (0f > this.m_fScrollPosition)
		{
			return;
		}
		this.m_TicketList.ScrollPosition = this.m_fScrollPosition;
	}

	private void ClickTicketSellInfoDetail(IUIObject obj)
	{
		int iCharKind = (int)obj.Data;
		TICKET_SELL_INFO ticketSellInfoFromCharKind = this.GetTicketSellInfoFromCharKind(iCharKind);
		if (ticketSellInfoFromCharKind == null)
		{
			return;
		}
		SolSlotData solSlotData = SolSlotData.GetSolSlotData(ticketSellInfoFromCharKind.i32CharKind, ticketSellInfoFromCharKind.i8Grade);
		SolDetail_Info_Dlg solDetail_Info_Dlg = (SolDetail_Info_Dlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAIL_DLG);
		if (solDetail_Info_Dlg != null && solSlotData != null)
		{
			solDetail_Info_Dlg.SetSolKind(solSlotData);
			solDetail_Info_Dlg.SetTabButtonHide(SolDetail_Info_Dlg.eSOLTOOLBAR.eEIEMENTSOL);
		}
	}

	private void ClickTicketItemToolTip(IUIObject obj)
	{
		long itemID = (long)obj.Data;
		ITEM itemFromItemID = NkUserInventory.instance.GetItemFromItemID(itemID);
		if (itemFromItemID == null)
		{
			return;
		}
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, itemFromItemID, null, false);
	}

	protected virtual void ClickPrimiumMall(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(ItemMallDlg.eMODE.eMODE_VOUCHER_HERO);
	}

	private void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Soldier_New.ToString());
		}
	}

	public void ClickRateOpenUrl(IUIObject obj)
	{
		ITEM_RATE_OPENURL_DATA itemRateOpenUrl = BASE_RATE_OPENURL_DATA.GetItemRateOpenUrl();
		NrMobileNoticeWeb nrMobileNoticeWeb = new NrMobileNoticeWeb();
		nrMobileNoticeWeb.OnRateOpenUrl(itemRateOpenUrl.strUrl);
	}
}
