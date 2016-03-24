using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class WillChargeDlg : Form
{
	public enum eTYPE
	{
		eTYPE_GOLD,
		eTYPE_ELIXIR
	}

	private Button m_btGold;

	private Button m_btElixir;

	private Label m_lbGold;

	private Label m_lbElixir;

	private Button m_btFullElixir;

	private Button m_btFullGold;

	private Label m_lbLabel10;

	private Label m_lbLabel11;

	private Button m_btClose;

	private GS_BABELTOWER_INVITE_FRIEND_ACK m_InvitePersonInfo;

	public GS_BABELTOWER_INVITE_FRIEND_ACK BabelInvitePersonInfo
	{
		get
		{
			return this.m_InvitePersonInfo;
		}
		set
		{
			this.m_InvitePersonInfo = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Main/dlg_WillCharge", G_ID.WILLCHARGE_DLG, true);
		base.DonotDepthChange(90f);
	}

	public override void SetComponent()
	{
		this.m_btGold = (base.GetControl("Button_Gold") as Button);
		this.m_btGold.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickUseGold));
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1140"),
			"gold",
			this.GetNeedMoney()
		});
		this.m_btGold.SetText(empty);
		this.m_btElixir = (base.GetControl("Button_Elixir") as Button);
		this.m_btElixir.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickUseElixir));
		this.m_lbGold = (base.GetControl("Label_gold") as Label);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1921"),
			"gold2",
			ANNUALIZED.Convert(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money)
		});
		this.m_lbGold.SetText(empty);
		this.m_lbElixir = (base.GetControl("Label_elixir") as Label);
		int num = NkUserInventory.GetInstance().Get_First_ItemCnt(70005);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1078"),
			"count",
			ANNUALIZED.Convert(num)
		});
		this.m_lbElixir.SetText(empty);
		this.m_btFullElixir = (base.GetControl("Button_Full") as Button);
		this.m_btFullElixir.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickUseFullElixir));
		this.m_btFullGold = (base.GetControl("Button_gold_Full") as Button);
		this.m_btFullGold.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickUseFullGold));
		long maxWillMoney = this.GetMaxWillMoney();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2912"),
			"gold",
			ANNUALIZED.Convert(maxWillMoney)
		});
		this.m_btFullGold.SetText(empty);
		if (maxWillMoney > 0L)
		{
			this.m_btFullGold.SetEnabled(true);
		}
		else
		{
			this.m_btFullGold.SetEnabled(false);
		}
		this.m_lbLabel10 = (base.GetControl("Label_Label10") as Label);
		this.m_lbLabel11 = (base.GetControl("Label_Label11") as Label);
		this.m_btClose = (base.GetControl("Btn_Close") as Button);
		if (this.m_btClose != null)
		{
			Button expr_28D = this.m_btClose;
			expr_28D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_28D.Click, new EZValueChangedDelegate(this.CloseForm));
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			float num2 = kMyCharInfo.m_fCurrentActivityTime - Time.realtimeSinceStartup;
			int num3 = (int)(num2 / 3600f);
			int num4 = (int)((num2 - (float)num3 * 3600f) / 60f);
			int num5 = (int)((num2 - (float)num3 * 3600f - (float)num4 * 60f) % 60f);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2855"),
				"min",
				num4.ToString("00"),
				"sec",
				num5.ToString("00")
			});
			this.m_lbLabel10.SetText(empty);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2856"),
				"maxcount",
				kMyCharInfo.m_nMaxActivityPoint
			});
			this.m_lbLabel11.SetText(empty);
		}
		else
		{
			this.m_lbLabel10.SetText(string.Empty);
			this.m_lbLabel11.SetText(string.Empty);
		}
		base.SetScreenCenter();
	}

	public override void OnClose()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (this.BabelInvitePersonInfo != null)
		{
			USER_FRIEND_INFO friend = kMyCharInfo.m_kFriendInfo.GetFriend(this.BabelInvitePersonInfo.ReqPersonID);
			string text = string.Empty;
			string empty = string.Empty;
			string text2 = string.Empty;
			if (this.BabelInvitePersonInfo.floortype == 2)
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2784");
			}
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("96");
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("97");
			if (friend != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"charname",
					TKString.NEWString(friend.szName),
					"type",
					text2,
					"floor",
					this.BabelInvitePersonInfo.floor,
					"subfloor",
					(int)(this.BabelInvitePersonInfo.sub_floor + 1)
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"charname",
					TKString.NEWString(this.BabelInvitePersonInfo.ReqPersonName),
					"type",
					text2,
					"floor",
					this.BabelInvitePersonInfo.floor,
					"subfloor",
					(int)(this.BabelInvitePersonInfo.sub_floor + 1)
				});
			}
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			msgBoxUI.SetMsg(new YesDelegate(NrReceiveGame.OnBabelInviteAccept), this.BabelInvitePersonInfo, new NoDelegate(NrReceiveGame.OnBabelInviteCancel), this.BabelInvitePersonInfo, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
			msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("317"));
			msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("318"));
		}
		base.OnClose();
	}

	public void ClickUseGold(IUIObject obj)
	{
		this.OnWillCharOK(null);
	}

	public void ClickUseElixir(IUIObject obj)
	{
		if (!this.IsWillCharge())
		{
			return;
		}
		if (0 >= NkUserInventory.GetInstance().Get_First_ItemCnt(70005))
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("168"),
				"targetname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(70005)
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		Protocol_Item.Item_Use(NkUserInventory.GetInstance().GetItem(70005));
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WILLCHARGE_DLG);
	}

	public long GetMaxWillMoney()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return 0L;
		}
		if (!this.IsWillCharge())
		{
			return 0L;
		}
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			return 0L;
		}
		long iWillCount = kMyCharInfo.m_nMaxActivityPoint - kMyCharInfo.m_nActivityPoint;
		return kMyCharInfo.GetMaxWillChargeGold(iWillCount);
	}

	public void ClickUseFullGold(IUIObject obj)
	{
		TsAudioManager.Container.RequestAudioClip("UI_SFX", "ETC", "ENERTGY_RECHARGE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		long maxWillMoney = this.GetMaxWillMoney();
		if (maxWillMoney == 0L)
		{
			this.m_btFullGold.SetEnabled(false);
			return;
		}
		this.m_btFullGold.SetEnabled(true);
		if (maxWillMoney > kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		SendPacket.GetInstance().SendObject(46);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WILLCHARGE_DLG);
	}

	public void ClickUseFullElixir(IUIObject obj)
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			long num = kMyCharInfo.m_nMaxActivityPoint - kMyCharInfo.m_nActivityPoint;
			if (num <= 0L)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("784");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			if (0 >= NkUserInventory.GetInstance().Get_First_ItemCnt(70005))
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("168"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(70005)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			ITEM item = NkUserInventory.GetInstance().GetItem(70005);
			if (item != null)
			{
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				NkSoldierInfo userSoldierInfo = nrCharUser.GetUserSoldierInfo();
				long solID = userSoldierInfo.GetSolID();
				GS_ITEM_SUPPLY_USE_REQ gS_ITEM_SUPPLY_USE_REQ = new GS_ITEM_SUPPLY_USE_REQ();
				gS_ITEM_SUPPLY_USE_REQ.m_nItemUnique = item.m_nItemUnique;
				gS_ITEM_SUPPLY_USE_REQ.m_nDestSolID = solID;
				if ((long)item.m_nItemNum < num)
				{
					gS_ITEM_SUPPLY_USE_REQ.m_shItemNum = item.m_nItemNum;
				}
				else
				{
					gS_ITEM_SUPPLY_USE_REQ.m_shItemNum = (int)num;
				}
				gS_ITEM_SUPPLY_USE_REQ.m_byPosType = item.m_nPosType;
				gS_ITEM_SUPPLY_USE_REQ.m_shPosItem = item.m_nItemPos;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SUPPLY_USE_REQ, gS_ITEM_SUPPLY_USE_REQ);
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WILLCHARGE_DLG);
		}
	}

	public void Send_GS_CHAR_WILL_CHARGE_REQ(byte i8Type)
	{
		SendPacket.GetInstance().SendObject(45);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WILLCHARGE_DLG);
	}

	public void OnWillCharOK(object a_oObject)
	{
		TsAudioManager.Container.RequestAudioClip("UI_SFX", "ETC", "ENERTGY_RECHARGE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (!this.IsWillCharge())
		{
			return;
		}
		long willChargeGold = kMyCharInfo.GetWillChargeGold();
		if (willChargeGold > kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		this.Send_GS_CHAR_WILL_CHARGE_REQ(0);
	}

	public int GetMyLevel()
	{
		int result = 1;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(0);
			if (soldierInfo != null)
			{
				result = (int)soldierInfo.GetLevel();
			}
		}
		return result;
	}

	public bool IsWillCharge()
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			return false;
		}
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHARGE_ACTIVITY_MAX);
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_nActivityPoint >= num)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("135"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return false;
		}
		return true;
	}

	public string GetNeedMoney()
	{
		string empty = string.Empty;
		long willChargeGold = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetWillChargeGold();
		return ANNUALIZED.Convert(willChargeGold);
	}

	public string GetAllElixirNeedMoney()
	{
		string empty = string.Empty;
		long willChargeGold = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetWillChargeGold();
		return ANNUALIZED.Convert(willChargeGold);
	}

	public override void Update()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (kMyCharInfo != null)
		{
			float num = kMyCharInfo.m_fCurrentActivityTime - Time.realtimeSinceStartup;
			int num2 = (int)(num / 3600f);
			int num3 = (int)((num - (float)num2 * 3600f) / 60f);
			int num4 = (int)((num - (float)num2 * 3600f - (float)num3 * 60f) % 60f);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2855"),
				"min",
				num3.ToString("00"),
				"sec",
				num4.ToString("00")
			});
			this.m_lbLabel10.SetText(empty);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2856"),
				"maxcount",
				kMyCharInfo.m_nMaxActivityPoint
			});
			this.m_lbLabel11.SetText(empty);
		}
		else
		{
			this.m_lbLabel10.SetText(string.Empty);
			this.m_lbLabel11.SetText(string.Empty);
		}
	}
}
