using GAME;
using MAIL_TYPE_BINARY_DATA;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections;
using TsBundle;
using UnityForms;

public class PostRecvDlg : Form
{
	private Label _l0_Label_Title;

	private ScrollLabel _l1_ScrollLabel_message;

	private Box m_SendUserName;

	private DrawTexture m_dtItemBG;

	private ItemTexture m_itItem;

	private Label _l1_Label_ItemName;

	private Label _l1_Label_ItemNum;

	private Box _l1_Label_Gold;

	private Button _l1_Button_OK1;

	private Button _l1_Button_reply1;

	private Button m_btClose;

	private ScrollLabel _l2_ScrollLabel_message;

	private Button _l2_Button_reply2;

	private Button _l2_Button_OK2;

	private GS_MAILBOX_HISTORY_ACK m_HistoryInfo = new GS_MAILBOX_HISTORY_ACK();

	private MAILBOX_BINARYDATA_INFO m_BinaryDataInfo;

	private long m_i64MailID;

	private long m_i64Money;

	private eMAIL_TYPE m_eMailType;

	private ITEM m_Item;

	private long m_lSolIDTrade;

	private int m_iCharKind;

	private short m_iLevel;

	private byte m_byGrade;

	private bool m_bIsHistory;

	private int m_Layer;

	private string m_strOK = string.Empty;

	private string m_strReplay = string.Empty;

	private string m_strSendCharName = string.Empty;

	private string m_strItemBGTextrueKey = "Win_T_ItemEmpty";

	private long m_i64LegionActionID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Post/Dlg_Post_GetMail", G_ID.POST_RECV_DLG, true);
		base.ChangeSceneDestory = false;
	}

	public override void SetComponent()
	{
		this._l0_Label_Title = (base.GetControl("Label_Title") as Label);
		this._l1_ScrollLabel_message = (base.GetControl("ScrollLabel_MailContent01") as ScrollLabel);
		this.m_SendUserName = (base.GetControl("Box_InputName") as Box);
		this.m_dtItemBG = (base.GetControl("DrawTexture_ItemSlot") as DrawTexture);
		this.m_itItem = (base.GetControl("DrawTexture_ItemIcn") as ItemTexture);
		this._l1_Label_ItemName = (base.GetControl("Label_ItemName") as Label);
		this._l1_Label_ItemNum = (base.GetControl("Label_ItemNum") as Label);
		this._l1_Label_Gold = (base.GetControl("Box_GetGold") as Box);
		this._l1_Button_OK1 = (base.GetControl("Button_OK01") as Button);
		this._l1_Button_reply1 = (base.GetControl("Button_Get") as Button);
		Button expr_E2 = this._l1_Button_OK1;
		expr_E2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E2.Click, new EZValueChangedDelegate(this.OnClickOK));
		Button expr_109 = this._l1_Button_reply1;
		expr_109.Click = (EZValueChangedDelegate)Delegate.Combine(expr_109.Click, new EZValueChangedDelegate(this.OnClickCancel));
		this._l2_ScrollLabel_message = (base.GetControl("FlashLabel_Reconnoiter") as ScrollLabel);
		this._l2_Button_OK2 = (base.GetControl("Button_OK02") as Button);
		this._l2_Button_reply2 = (base.GetControl("Button_reply") as Button);
		Button expr_172 = this._l2_Button_OK2;
		expr_172.Click = (EZValueChangedDelegate)Delegate.Combine(expr_172.Click, new EZValueChangedDelegate(this.OnClickOK));
		Button expr_199 = this._l2_Button_reply2;
		expr_199.Click = (EZValueChangedDelegate)Delegate.Combine(expr_199.Click, new EZValueChangedDelegate(this.OnClickReply));
		this.m_strOK = this._l2_Button_OK2.GetText();
		this.m_strReplay = this._l2_Button_reply2.GetText();
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this.InitControl();
		base.Draggable = false;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void InitData()
	{
		if (TsPlatform.IsMobile)
		{
			base.Draggable = false;
			base.SetScreenCenter();
			base.ShowBlackBG(0.5f);
		}
	}

	private void InitControl()
	{
		this._l0_Label_Title.Text = string.Empty;
		this.m_dtItemBG.SetTextureKey(this.m_strItemBGTextrueKey);
		this.m_itItem.Image = null;
		this._l1_Label_ItemName.Text = string.Empty;
		this._l1_Label_ItemNum.Text = string.Empty;
		this._l1_Label_Gold.Text = "0";
	}

	public void DecideControl(bool bIsHistory, bool bIsReport, bool bHaveMoney, bool bHaveItem, bool bCanReply, bool bIsSystemSend, bool bLinkedText)
	{
		this.DecideControl(bIsHistory, bIsReport, bHaveMoney, bHaveItem, bCanReply, bIsSystemSend, bLinkedText, eMAIL_TYPE.MAIL_TYPE_REGULAR_BEGIN);
	}

	public void DecideControl(bool bIsHistory, bool bIsReport, bool bHaveMoney, bool bHaveItem, bool bCanReply, bool bIsSystemSend, bool bLinkedText, eMAIL_TYPE _eMailType)
	{
		if (bIsReport)
		{
			base.ShowLayer(3);
			this.m_Layer = 2;
		}
		else if (bHaveMoney || bHaveItem)
		{
			this.m_Layer = 1;
			if (!bCanReply)
			{
				this._l1_Button_reply1.SetEnabled(false);
			}
			this._l1_Button_OK1.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10");
			this._l1_Button_OK1.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnClickOK));
			if (bIsHistory)
			{
				this._l1_Button_OK1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickConfirm));
			}
			else
			{
				this._l1_Button_OK1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickCancel));
			}
		}
		else
		{
			this.m_Layer = 2;
			if (!bCanReply && _eMailType != eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_FRIEND && _eMailType != eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_GUILD && _eMailType != eMAIL_TYPE.MAIL_TYPE_SYSTEM_SUPPORTER)
			{
				this.m_Layer = 1;
				this._l1_Button_reply1.SetEnabled(false);
			}
			this._l1_Button_OK1.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10");
			this._l1_Button_OK1.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnClickOK));
			if (bIsHistory)
			{
				this._l1_Button_OK1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickConfirm));
			}
			else
			{
				this._l1_Button_OK1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickCancel));
			}
			if (bIsSystemSend)
			{
			}
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (_eMailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_FRIEND)
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("464");
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2085");
			this._l2_Button_reply2.SetButtonTextureKey("Win_B_NewBtnBlue");
			this._l2_Button_OK2.SetButtonTextureKey("Win_B_NewBtnRed");
			Button expr_1BF = this._l2_Button_reply2;
			expr_1BF.Click = (EZValueChangedDelegate)Delegate.Remove(expr_1BF.Click, new EZValueChangedDelegate(this.OnClickReply));
			Button expr_1E6 = this._l2_Button_reply2;
			expr_1E6.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1E6.Click, new EZValueChangedDelegate(this.OnClickOK));
			Button expr_20D = this._l2_Button_OK2;
			expr_20D.Click = (EZValueChangedDelegate)Delegate.Remove(expr_20D.Click, new EZValueChangedDelegate(this.OnClickOK));
			Button expr_234 = this._l2_Button_OK2;
			expr_234.Click = (EZValueChangedDelegate)Delegate.Combine(expr_234.Click, new EZValueChangedDelegate(this.OnClickCancel));
		}
		else
		{
			this._l2_Button_reply2.SetButtonTextureKey("Win_B_NewBtnOrange");
			this._l2_Button_OK2.SetButtonTextureKey("Win_B_NewBtnOrange");
			if (_eMailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_ADD_FRIEND_FACEBOOK)
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("398");
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10");
				Button expr_2A9 = this._l2_Button_reply2;
				expr_2A9.Click = (EZValueChangedDelegate)Delegate.Remove(expr_2A9.Click, new EZValueChangedDelegate(this.OnClickReply));
				Button expr_2D0 = this._l2_Button_reply2;
				expr_2D0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2D0.Click, new EZValueChangedDelegate(this.OnClickOK));
				Button expr_2F7 = this._l2_Button_OK2;
				expr_2F7.Click = (EZValueChangedDelegate)Delegate.Remove(expr_2F7.Click, new EZValueChangedDelegate(this.OnClickOK));
				Button expr_31E = this._l2_Button_OK2;
				expr_31E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_31E.Click, new EZValueChangedDelegate(this.OnClickCancel));
			}
			else if (_eMailType == eMAIL_TYPE.MAIL_TYPE_REPORT_MINE_RESULT || _eMailType == eMAIL_TYPE.MAIL_TYPE_EXPEDITION_RESULT_REPORT)
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1088");
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("415");
				Button expr_37F = this._l2_Button_reply2;
				expr_37F.Click = (EZValueChangedDelegate)Delegate.Remove(expr_37F.Click, new EZValueChangedDelegate(this.OnClickReply));
				Button expr_3A6 = this._l2_Button_reply2;
				expr_3A6.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3A6.Click, new EZValueChangedDelegate(this.OnClickReport));
				Button expr_3CD = this._l2_Button_OK2;
				expr_3CD.Click = (EZValueChangedDelegate)Delegate.Remove(expr_3CD.Click, new EZValueChangedDelegate(this.OnClickOK));
				Button expr_3F4 = this._l2_Button_OK2;
				expr_3F4.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3F4.Click, new EZValueChangedDelegate(this.OnClickConfirm));
			}
			else if (_eMailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_GUILD)
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("317");
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("318");
				Button expr_449 = this._l2_Button_reply2;
				expr_449.Click = (EZValueChangedDelegate)Delegate.Remove(expr_449.Click, new EZValueChangedDelegate(this.OnClickReply));
				Button expr_470 = this._l2_Button_reply2;
				expr_470.Click = (EZValueChangedDelegate)Delegate.Combine(expr_470.Click, new EZValueChangedDelegate(this.OnClickOK));
				Button expr_497 = this._l2_Button_OK2;
				expr_497.Click = (EZValueChangedDelegate)Delegate.Remove(expr_497.Click, new EZValueChangedDelegate(this.OnClickOK));
				Button expr_4BE = this._l2_Button_OK2;
				expr_4BE.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4BE.Click, new EZValueChangedDelegate(this.OnClickCancel));
			}
			else
			{
				text = this.m_strOK;
				text2 = this.m_strReplay;
			}
		}
		base.ShowLayer(this.m_Layer);
		this._l2_Button_reply2.SetText(text2);
		this._l2_Button_OK2.SetText(text);
	}

	public void SetMailData(bool bIsHistory, long i64MailID, eMAIL_TYPE mailType, string strSendCharName, string strRecvCharName, long i64Money, ITEM item, byte[] binaryData, string strMsg, bool bDidISend, long i64SolID, int i32CharKind, byte i8Grade, short i16Level)
	{
		bool flag = false;
		bool bHaveMoney = false;
		bool bHaveItem = false;
		bool bCanReply = false;
		bool bIsSystemSend = false;
		bool bLinkedText = false;
		string text = string.Empty;
		this.InitControl();
		if (mailType == eMAIL_TYPE.MAIL_TYPE_REPORT_MINE_RESULT || mailType == eMAIL_TYPE.MAIL_TYPE_EXPEDITION_RESULT_REPORT)
		{
			flag = true;
		}
		if (mailType > eMAIL_TYPE.MAIL_TYPE_USER_BEGIN && mailType < eMAIL_TYPE.MAIL_TYPE_USER_END)
		{
			if (!bDidISend)
			{
				bCanReply = true;
			}
		}
		else
		{
			bIsSystemSend = true;
			if (mailType != eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_FRIEND && mailType < eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_SEND && mailType > eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_REWARD)
			{
				strSendCharName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1574");
			}
			switch (mailType)
			{
			case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD:
			case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD_CANCEL_REGISTER:
			case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD_CANCEL_TENDER:
				goto IL_103;
			case eMAIL_TYPE.MAIL_TYPE_SOLDIERGROUP_RECRUIT:
			case eMAIL_TYPE.MAIL_TYPE_GM_CREATESOL:
			case eMAIL_TYPE.MAIL_TYPE_SOLDIERGROUP_CASH_RECRUIT:
			case eMAIL_TYPE.MAIL_TYPE_INVENTORY_FULL:
				strSendCharName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2311");
				goto IL_15B;
			case eMAIL_TYPE.MAIL_TYPE_INFIBATTLE_REWARD:
			case eMAIL_TYPE.MAIL_TYPE_INFIBATTLE_WEEK_REWARD:
				strSendCharName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1574");
				goto IL_15B;
			case eMAIL_TYPE.MAIL_TYPE_BOUNTYHUNT_REWARD:
				strSendCharName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2344");
				goto IL_15B;
			case eMAIL_TYPE.MAIL_TYPE_KAKAOFRIEND_EVENT_REWARD:
			case eMAIL_TYPE.MAIL_TYPE_ITEMMALL_GIFT:
			case eMAIL_TYPE.MAIL_TYPE_INVITE_EVENT_REWARD:
			case eMAIL_TYPE.MAIL_TYPE_CHUKONG_ITEMMALL_SEND:
			case eMAIL_TYPE.MAIL_TYPE_EXPEDITION_RESULT_REPORT:
			case eMAIL_TYPE.MAIL_TYPE_EXPEDITION_GIVE_ITEM:
			case eMAIL_TYPE.MAIL_TYPE_FRIENDGIFT_ITEM:
			case eMAIL_TYPE.MAIL_TYPE_MYTHRAID_ITEM:
			case eMAIL_TYPE.MAIL_TYPE_COSTUME_GIFT:
				IL_DC:
				switch (mailType)
				{
				case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER:
				case eMAIL_TYPE.MAIL_TYPE_AUCTION_BEFORETENDER:
				case eMAIL_TYPE.MAIL_TYPE_AUCTION_TENDER:
				case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER_CANCEL:
				case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER_FAIL:
					goto IL_103;
				case eMAIL_TYPE.MAIL_TYPE_REPORT_MINE_RESULT:
					goto IL_15B;
				default:
					goto IL_15B;
				}
				break;
			}
			goto IL_DC;
			IL_103:
			strSendCharName = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1144");
		}
		IL_15B:
		if (i64Money != 0L)
		{
			bHaveMoney = true;
		}
		if (item != null && item.m_nItemUnique != 0)
		{
			bHaveItem = true;
		}
		this.m_bIsHistory = bIsHistory;
		this.DecideControl(bIsHistory, flag, bHaveMoney, bHaveItem, bCanReply, bIsSystemSend, bLinkedText, mailType);
		this.m_i64MailID = i64MailID;
		this.m_eMailType = mailType;
		this.m_i64Money = i64Money;
		this.m_Item = item;
		this.m_lSolIDTrade = 0L;
		this.m_iCharKind = 0;
		this.m_iLevel = 0;
		this.m_byGrade = 0;
		this.m_strSendCharName = strSendCharName;
		if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_SEND || mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_RECV || mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_REWARD)
		{
			this.m_SendUserName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1188");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_REPORT_MINE_RESULT || mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_MINE_GIVEITEM || mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_MINE_BACKMOVE_GETITEM || mailType == eMAIL_TYPE.MAILTYPE_SYSTEM_MINE_DELMINE_BACK || mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_MINE_FAIL_DEFENGUILD)
		{
			this.m_SendUserName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1361");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_NEWGUILD_CHANGEMASTER)
		{
			this.m_SendUserName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1339");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_USER_TO_GUILD)
		{
			this.m_SendUserName.Text = strSendCharName;
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_GMSETITEM || mailType == eMAIL_TYPE.MAIL_TYPE_CHALLENGE_REWARD || mailType == eMAIL_TYPE.MAIL_TYPE_KAKAOFRIEND_EVENT_REWARD || mailType == eMAIL_TYPE.MAIL_TYPE_INVITE_EVENT_REWARD)
		{
			string empty = string.Empty;
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1537");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1574")
			});
			this.m_SendUserName.Text = empty;
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_SOLDELITEM)
		{
			string empty2 = string.Empty;
			string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1537");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				textFromInterface2,
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1574")
			});
			this.m_SendUserName.Text = empty2;
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_NEWGUILDBOSS_REWARD)
		{
			this.m_SendUserName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1901");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_PROMOTION_EVENT)
		{
			this.m_SendUserName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1574");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_EXPEDITION_RESULT_REPORT || mailType == eMAIL_TYPE.MAIL_TYPE_EXPEDITION_GIVE_ITEM)
		{
			this.m_SendUserName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2428");
		}
		else
		{
			this.m_SendUserName.Text = strSendCharName;
		}
		if (!flag)
		{
			if (bDidISend)
			{
				this._l0_Label_Title.Text = NrTSingleton<PostUtil>.Instance.GetSendObjectName(mailType, strRecvCharName, this.m_bIsHistory, true);
				this._l0_Label_Title.Data = strRecvCharName;
			}
			else
			{
				this._l0_Label_Title.Text = NrTSingleton<PostUtil>.Instance.GetSendObjectName(mailType, strSendCharName, this.m_bIsHistory, false);
				this._l0_Label_Title.Data = strSendCharName;
			}
		}
		else
		{
			this._l0_Label_Title.Text = NrTSingleton<PostUtil>.Instance.GetSendObjectName(mailType, strSendCharName, this.m_bIsHistory, false);
			this._l0_Label_Title.Data = strSendCharName;
		}
		bool flag2 = true;
		switch (mailType)
		{
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_GAMEMASTER:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_GAMEMASTER_MESSAGE:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_GAMEMASTER_EMAIL:
		{
			string text2 = string.Empty;
			if (NrTSingleton<NkCharManager>.Instance.GetChar(1) != null)
			{
				text2 = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetCharName();
			}
			if (i64Money != 0L && item.m_nItemUnique != 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					string.Empty,
					"targetname",
					text2,
					"targetname2",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(item.m_nItemUnique, item.m_nRank),
					"targetname3",
					ANNUALIZED.Convert(i64Money)
				});
			}
			else if (i64Money != 0L)
			{
				string text3 = string.Empty;
				text3 = NrTSingleton<UIDataManager>.Instance.GetString(ANNUALIZED.Convert(i64Money), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("676"));
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1546"),
					"username",
					text2,
					"item",
					text3
				});
			}
			else if (item.m_nItemUnique != 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1546"),
					"username",
					text2,
					"item",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(item.m_nItemUnique, item.m_nRank)
				});
				if (item.m_nItemUnique == 70000)
				{
					NrTSingleton<FiveRocksEventManager>.Instance.HeartsInflow(eHEARTS_INFLOW.GM, (long)item.m_nItemNum);
				}
			}
			else
			{
				text = strMsg;
				strMsg = string.Empty;
			}
			string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			text = NrTSingleton<UIDataManager>.Instance.GetString(text, "\n \n", strMsg, "\n \n", textFromInterface3);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_MARKET_ITEM_SOLD:
		{
			int num = 0;
			string text4 = string.Empty;
			string text5 = string.Empty;
			SYSTEM_MARKET_ITEM_SOLD sYSTEM_MARKET_ITEM_SOLD = (SYSTEM_MARKET_ITEM_SOLD)ReceivePakcet.DeserializePacket(binaryData, 0, out num, typeof(SYSTEM_MARKET_ITEM_SOLD));
			text4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1547");
			text5 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(sYSTEM_MARKET_ITEM_SOLD.i32ItemUnique, sYSTEM_MARKET_ITEM_SOLD.i32Rank);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text4,
				"item",
				text5,
				"count",
				sYSTEM_MARKET_ITEM_SOLD.i32ItemNum.ToString(),
				"money",
				ANNUALIZED.Convert(i64Money)
			});
			string empty3 = string.Empty;
			string textFromInterface4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty3, "\n", text, "\n", textFromInterface4);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_MARKET_RETURN:
		{
			int num2 = 0;
			MARKET_RETURN mARKET_RETURN = (MARKET_RETURN)ReceivePakcet.DeserializePacket(binaryData, 0, out num2, typeof(MARKET_RETURN));
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(mARKET_RETURN.m_nItemUnique, mARKET_RETURN.m_nRank);
			string textFromInterface5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1548");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				textFromInterface5,
				"item",
				itemNameByItemUnique
			});
			string empty4 = string.Empty;
			string textFromInterface6 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty4, "\n", text, "\n", textFromInterface6);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_QUEST:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strMsg);
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_BUY_ITEM:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_QUEST_ITEM:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_EXTRACTED_ITEM:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_BATTLE:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_RETURN:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_DAILYEVENT:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_PLUNDERAGREE:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_DIRECTPURCHASE:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_COUPONUSE:
		case eMAIL_TYPE.MAIL_TYPE_BABEL_HIDDEN_TREASURE:
		case eMAIL_TYPE.MAIL_TYPE_BABEL_FIRSTREWARD:
		case eMAIL_TYPE.MAIL_TYPE_PROMOTION_EVENT:
		case eMAIL_TYPE.MAIL_TYPE_XPSPROMOTION_REWARD_1:
		case eMAIL_TYPE.MAIL_TYPE_CHUKONG_ITEMMALL_SEND:
		case eMAIL_TYPE.MAIL_TYPE_FRIENDGIFT_ITEM:
		case eMAIL_TYPE.MAIL_TYPE_MYTHRAID_ITEM:
			IL_5C7:
			if (mailType != eMAIL_TYPE.MAIL_TYPE_USER_TO_GUILD)
			{
				string textFromInterface7 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1544");
				string textFromInterface8 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
				text = strMsg;
				text = NrTSingleton<UIDataManager>.Instance.GetString(textFromInterface7, "\n \n", text, "\n \n", textFromInterface8);
				goto IL_1BBA;
			}
			text = strMsg;
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_FRIEND:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2086");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2086"),
				"targetname",
				strSendCharName
			});
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_ADD_FRIEND_FACEBOOK:
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("357"),
				"username",
				strSendCharName
			});
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_SEND:
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1193"),
				"targetname",
				strSendCharName
			});
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_RECV:
		{
			string text6 = string.Empty;
			if (NrTSingleton<NkCharManager>.Instance.GetChar(1) != null)
			{
				text6 = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetCharName();
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1189"),
				"targetname1",
				strSendCharName,
				"tergetname2",
				text6
			});
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_REWARD:
		{
			int num3 = 0;
			ICollection recommend_Reward_Col = NrTSingleton<NrBaseTableManager>.Instance.GetRecommend_Reward_Col();
			if (recommend_Reward_Col != null)
			{
				foreach (RECOMMEND_REWARD rECOMMEND_REWARD in recommend_Reward_Col)
				{
					if (rECOMMEND_REWARD.i64Money == i64Money && rECOMMEND_REWARD.i32ItemUnique == item.m_nItemUnique && (int)rECOMMEND_REWARD.i8ItemCount == item.m_nItemNum)
					{
						num3 = (int)rECOMMEND_REWARD.i8RecommendCount;
					}
					if (item.m_nItemUnique == 70000)
					{
						NrTSingleton<FiveRocksEventManager>.Instance.HeartsInflow(eHEARTS_INFLOW.RECOMMEND_REWARD, (long)item.m_nItemNum);
					}
				}
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1190"),
				"count",
				num3
			});
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_BEFORETENDER:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_TENDER:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER_CANCEL:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER_FAIL:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD_CANCEL_REGISTER:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD_CANCEL_TENDER:
		{
			int num4 = 0;
			AUCTIONINFO aUCTIONINFO = (AUCTIONINFO)ReceivePakcet.DeserializePacket(binaryData, 0, out num4, typeof(AUCTIONINFO));
			if (mailType != eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD && mailType != eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD_CANCEL_TENDER && mailType != eMAIL_TYPE.MAIL_TYPE_AUCTION_BEFORETENDER && mailType != eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER)
			{
				if (0 < aUCTIONINFO.i32ItemUnique)
				{
					item.m_nItemUnique = aUCTIONINFO.i32ItemUnique;
					item.m_nOption[4] = aUCTIONINFO.i32BattleSkillUnique;
					item.m_nOption[2] = aUCTIONINFO.i32MakeRank;
				}
				else if (0 < aUCTIONINFO.i32CharKind)
				{
					this.m_lSolIDTrade = aUCTIONINFO.i64SolID;
					this.m_iCharKind = aUCTIONINFO.i32CharKind;
					this.m_iLevel = aUCTIONINFO.i16Level;
					this.m_byGrade = aUCTIONINFO.i8Grade;
				}
			}
			if (0 >= item.m_nItemUnique && 0 >= this.m_iCharKind && 0L >= i64Money)
			{
				flag2 = false;
			}
			this.GetAuctionMessage(mailType, aUCTIONINFO, ref text);
			if (mailType == eMAIL_TYPE.MAIL_TYPE_AUCTION_TENDER)
			{
				NrTSingleton<FiveRocksEventManager>.Instance.HeartsConsume(eHEARTS_CONSUME.AUCTION_TEX, aUCTIONINFO.i64Commission);
			}
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_REPORT_MINE_RESULT:
		{
			int num5 = 0;
			string text7 = string.Empty;
			string text8 = string.Empty;
			string text9 = string.Empty;
			string text10 = string.Empty;
			MINE_RESULT mINE_RESULT = (MINE_RESULT)ReceivePakcet.DeserializePacket(binaryData, 0, out num5, typeof(MINE_RESULT));
			text9 = TKString.NEWString(mINE_RESULT.m_szName);
			text10 = TKString.NEWString(mINE_RESULT.m_szGuildName);
			this.m_i64LegionActionID = mINE_RESULT.m_i64LegionActionID;
			if (mINE_RESULT.m_bAttack)
			{
				if (mINE_RESULT.m_bMonsterBattle)
				{
					if (mINE_RESULT.m_bWin)
					{
						text7 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1362");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text8, new object[]
						{
							text7,
							"targetname1",
							text9,
							"targetname3",
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1318")
						});
					}
					else
					{
						text7 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1363");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text8, new object[]
						{
							text7,
							"targetname1",
							text9
						});
					}
				}
				else if (mINE_RESULT.m_bWin)
				{
					text7 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1354");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text8, new object[]
					{
						text7,
						"targetname1",
						text9,
						"targetname2",
						text10,
						"targetname3",
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1318")
					});
				}
				else
				{
					text7 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1355");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text8, new object[]
					{
						text7,
						"targetname1",
						text9,
						"targetname2",
						text10
					});
				}
			}
			else if (mINE_RESULT.m_bWin)
			{
				text7 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1356");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text8, new object[]
				{
					text7,
					"targetname1",
					text9,
					"targetname2",
					text10,
					"targetname3",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1318")
				});
			}
			else
			{
				text7 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1357");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text8, new object[]
				{
					text7,
					"targetname1",
					text9,
					"targetname2",
					text10,
					"targetname3",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1318")
				});
			}
			string empty5 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParamColor(out empty5, new string[]
			{
				strMsg
			});
			text8 += empty5;
			string empty6 = string.Empty;
			string textFromInterface9 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty6, "\n", text8, "\n", textFromInterface9);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_GUILD:
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2020"),
				"targetname",
				strMsg
			});
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_MINE_GIVEITEM:
		{
			string textFromInterface10 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1631");
			string empty7 = string.Empty;
			string empty8 = string.Empty;
			string textFromInterface11 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty7, new object[]
			{
				textFromInterface10,
				"targetname1",
				strSendCharName
			});
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty8, "\n", empty7, "\n", textFromInterface11);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_MINE_BACKMOVE_GETITEM:
		{
			string empty9 = string.Empty;
			string textFromInterface12 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			string textFromInterface13 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1708");
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty9, "\n", textFromInterface13, "\n", textFromInterface12);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_NEWGUILD_CHANGEMASTER:
		{
			int num6 = 0;
			NEWGUILD_SIMPLEINFO nEWGUILD_SIMPLEINFO = (NEWGUILD_SIMPLEINFO)ReceivePakcet.DeserializePacket(binaryData, 0, out num6, typeof(NEWGUILD_SIMPLEINFO));
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				strMsg,
				"count",
				nEWGUILD_SIMPLEINFO.i16CheckDay
			});
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_SUPPORTER:
		{
			string text11 = string.Empty;
			if (NrTSingleton<NkCharManager>.Instance.GetChar(1) != null)
			{
				text11 = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetCharName();
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1919"),
				"targetname1",
				this.m_strSendCharName,
				"targetname2",
				text11,
				"targetname3",
				this.m_strSendCharName
			});
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_SUPPORTER_REWARD:
		{
			int num7 = 0;
			ICollection supporter_Reward_Col = NrTSingleton<NrBaseTableManager>.Instance.GetSupporter_Reward_Col();
			if (supporter_Reward_Col != null)
			{
				foreach (SUPPORTER_REWARD sUPPORTER_REWARD in supporter_Reward_Col)
				{
					if (sUPPORTER_REWARD.i64Money == i64Money && sUPPORTER_REWARD.i32ItemUnique == item.m_nItemUnique && (int)sUPPORTER_REWARD.i8ItemCount == item.m_nItemNum)
					{
						num7 = (int)sUPPORTER_REWARD.i8SupporterLevel;
					}
				}
			}
			string text12 = string.Empty;
			text12 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1920");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text12,
				"targetname1",
				this.m_strSendCharName,
				"level",
				num7,
				"count",
				i64Money,
				"targetname2",
				this.m_strSendCharName
			});
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAILTYPE_SYSTEM_MINE_DELMINE_BACK:
		{
			string empty10 = string.Empty;
			string textFromInterface14 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			string text13 = string.Empty;
			string empty11 = string.Empty;
			text13 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1925");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty11, new object[]
			{
				text13,
				"targetname1",
				strSendCharName
			});
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty10, "\n", empty11, "\n", textFromInterface14);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_GMSETITEM:
		{
			string text14 = string.Empty;
			if (NrTSingleton<NkCharManager>.Instance.GetChar(1) != null)
			{
				text14 = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetCharName();
			}
			string empty12 = string.Empty;
			string text15 = string.Empty;
			text15 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1976");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty12, new object[]
			{
				text15,
				"username",
				text14
			});
			string textFromInterface15 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty12, "\n \n", strMsg, "\n \n", textFromInterface15);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_SOLDELITEM:
			if (strMsg.Length == 0)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1959");
			}
			else
			{
				this.m_iCharKind = Convert.ToInt32(strMsg);
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_iCharKind);
				if (charKindInfo != null)
				{
					string text16 = string.Empty;
					text16 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1958");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						text16,
						"targetname",
						charKindInfo.GetName()
					});
				}
				else
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1959");
				}
			}
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_NEWGUILDBOSS_REWARD:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1974");
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_MINE_FAIL_DEFENGUILD:
		{
			string empty13 = string.Empty;
			string textFromInterface16 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			string textFromInterface17 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2055");
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty13, "\n", textFromInterface17, "\n", textFromInterface16);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_CHALLENGE_REWARD:
		{
			string text17 = string.Empty;
			string empty14 = string.Empty;
			string textFromInterface18 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			if (NrTSingleton<NkCharManager>.Instance.GetChar(1) != null)
			{
				text17 = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetCharName();
			}
			int num8 = 0;
			if (int.TryParse(strMsg, out num8) && num8 == 20000)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty14, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2135"),
					"username",
					text17,
					"itemname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(item.m_nItemUnique),
					"count",
					item.m_nItemNum
				});
			}
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty14, "\r\n\r\n\r\n", textFromInterface18);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_SOLDIERGROUP_RECRUIT:
		case eMAIL_TYPE.MAIL_TYPE_SOLDIERGROUP_CASH_RECRUIT:
			this.m_lSolIDTrade = i64SolID;
			this.m_iCharKind = i32CharKind;
			this.m_iLevel = i16Level;
			this.m_byGrade = i8Grade;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2310");
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_INFIBATTLE_REWARD:
		case eMAIL_TYPE.MAIL_TYPE_INFIBATTLE_WEEK_REWARD:
		{
			string textFromInterface19 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1544");
			string textFromInterface20 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2334");
			text = NrTSingleton<UIDataManager>.Instance.GetString(textFromInterface19, "\n \n", text, "\n \n", textFromInterface20);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_BOUNTYHUNT_REWARD:
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2345"),
				"itemname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(item.m_nItemUnique),
				"count",
				item.m_nItemNum
			});
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_KAKAOFRIEND_EVENT_REWARD:
		{
			string empty15 = string.Empty;
			string textFromInterface21 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			int num9 = 0;
			if (int.TryParse(strMsg, out num9))
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty15, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("403"),
					"count",
					num9
				});
			}
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty15, "\r\n\r\n\r\n", textFromInterface21);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_GM_CREATESOL:
		{
			this.m_lSolIDTrade = i64SolID;
			this.m_iCharKind = i32CharKind;
			this.m_iLevel = i16Level;
			this.m_byGrade = i8Grade;
			string textFromInterface22 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1544");
			string textFromInterface23 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			text = strMsg;
			text = NrTSingleton<UIDataManager>.Instance.GetString(textFromInterface22, "\n \n", text, "\n \n", textFromInterface23);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_ITEMMALL_GIFT:
			if (strMsg != string.Empty)
			{
				long index = Convert.ToInt64(strMsg);
				ITEM_MALL_ITEM item2 = NrTSingleton<ItemMallItemManager>.Instance.GetItem(index);
				if (item2 != null)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2581"),
						"CharName",
						strSendCharName,
						"Product",
						NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(item2.m_strTextKey)
					});
				}
			}
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_INVITE_EVENT_REWARD:
		{
			string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("404");
			string textFromInterface24 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			text = NrTSingleton<UIDataManager>.Instance.GetString(textFromMessageBox, "\r\n\r\n\r\n", textFromInterface24);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_EXPEDITION_RESULT_REPORT:
		{
			int num10 = 0;
			string text18 = string.Empty;
			string empty16 = string.Empty;
			string text19 = string.Empty;
			string text20 = string.Empty;
			EXPEDITION_RESULT eXPEDITION_RESULT = (EXPEDITION_RESULT)ReceivePakcet.DeserializePacket(binaryData, 0, out num10, typeof(EXPEDITION_RESULT));
			this.m_i64LegionActionID = eXPEDITION_RESULT.m_i64ExpeditionBattleID;
			NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(eXPEDITION_RESULT.m_i32LeaderKind);
			if (charKindInfo2 != null)
			{
				text19 = charKindInfo2.GetName();
			}
			EXPEDITION_CREATE_DATA expedtionCreateData = BASE_EXPEDITION_CREATE_DATA.GetExpedtionCreateData(eXPEDITION_RESULT.m_i16ExpeditionCreateDataID);
			if (expedtionCreateData != null)
			{
				EXPEDITION_DATA expeditionDataFromGrade = BASE_EXPEDITION_DATA.GetExpeditionDataFromGrade(expedtionCreateData.GetGrade());
				if (expeditionDataFromGrade != null)
				{
					text20 = expeditionDataFromGrade.Expedition_Name + eXPEDITION_RESULT.m_i16ExpeditionCreateDataID.ToString();
				}
			}
			if (eXPEDITION_RESULT.m_bUserAttack)
			{
				if (eXPEDITION_RESULT.m_bWin)
				{
					text18 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2485");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty16, new object[]
					{
						text18,
						"targetname1",
						text19,
						"targetname2",
						text20
					});
				}
				else
				{
					text18 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2486");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty16, new object[]
					{
						text18,
						"targetname",
						text19
					});
				}
			}
			else if (eXPEDITION_RESULT.m_bWin)
			{
				text18 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2487");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty16, new object[]
				{
					text18,
					"targetname1",
					text19,
					"targetname2",
					text20
				});
			}
			else
			{
				text18 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2488");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty16, new object[]
				{
					text18,
					"targetname1",
					text19,
					"targetname2",
					text20
				});
			}
			string empty17 = string.Empty;
			string textFromInterface25 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty17, "\n", empty16, "\n", textFromInterface25);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_EXPEDITION_GIVE_ITEM:
		{
			string empty18 = string.Empty;
			string textFromInterface26 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			string textFromInterface27 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2675");
			text = NrTSingleton<UIDataManager>.Instance.GetString(empty18, "\n", textFromInterface27, "\n", textFromInterface26);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_COSTUME_GIFT:
			if (string.IsNullOrEmpty(strMsg))
			{
				int costumeUnique = Convert.ToInt32(strMsg);
				string costumeName = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeName(costumeUnique);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2581"),
					"CharName",
					strSendCharName,
					"Product",
					costumeName
				});
			}
			goto IL_1BBA;
		case eMAIL_TYPE.MAIL_TYPE_INVENTORY_FULL:
		{
			string textFromInterface28 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1544");
			string textFromInterface29 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1937");
			string textFromInterface30 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545");
			text = NrTSingleton<UIDataManager>.Instance.GetString(textFromInterface28, "\n \n", textFromInterface29, "\n \n", textFromInterface30);
			goto IL_1BBA;
		}
		case eMAIL_TYPE.MAIL_TYPE_GUILD_GOLDENBELL:
		{
			string text21 = string.Empty;
			string text22 = string.Empty;
			if (item != null)
			{
				text21 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(item.m_nItemUnique);
				text22 = item.m_nItemNum.ToString();
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3717"),
				"GuildName",
				strMsg,
				"CharName",
				strSendCharName,
				"ItemName",
				text21,
				"ItemNum",
				text22
			});
			goto IL_1BBA;
		}
		}
		goto IL_5C7;
		IL_1BBA:
		ScrollLabel scrollLabel = (this.m_Layer != 1) ? this._l2_ScrollLabel_message : this._l1_ScrollLabel_message;
		text = text.Replace("\r\n", "\n");
		scrollLabel.SetScrollLabel(text);
		if (flag2)
		{
			if (item != null)
			{
				if (item.m_nItemNum > 0)
				{
					this.m_itItem.SetItemTexture(item);
					this.m_itItem.c_cItemTooltip = item;
					string rankColorName = NrTSingleton<ItemManager>.Instance.GetRankColorName(item);
					int rank = item.m_nOption[2];
					this._l1_Label_ItemName.SetText(rankColorName);
					this.m_dtItemBG.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString(rank));
					if (item.m_nItemUnique == 70000)
					{
						NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_HEARTS_STONE", this.m_itItem, this.m_itItem.GetSize());
					}
					int rank2 = item.m_nOption[2];
					if (string.Compare(ItemManager.RankStateString(rank2), "best") == 0)
					{
						NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_WEAPON_GOOD", this.m_itItem, this.m_itItem.GetSize());
					}
					string empty19 = string.Empty;
					string textFromInterface31 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("889");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty19, new object[]
					{
						textFromInterface31,
						"count1",
						item.m_nItemNum
					});
					this._l1_Label_ItemNum.Text = empty19;
				}
				else if (i64Money > 0L)
				{
					this.m_itItem.SetTexture("Main_I_ExtraI01");
				}
			}
			if (0L < this.m_lSolIDTrade || 0 < this.m_iCharKind)
			{
				this.m_itItem.SetSolImageTexure(eCharImageType.SMALL, this.m_iCharKind, (int)this.m_byGrade, (int)this.m_iLevel);
				NrCharKindInfo charKindInfo3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_iCharKind);
				if (charKindInfo3 != null)
				{
					string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(this.m_iCharKind, (int)this.m_byGrade, charKindInfo3.GetName());
					this._l1_Label_ItemName.SetText(legendName);
				}
			}
		}
		else
		{
			this.m_itItem.ClearData();
		}
		this._l1_Label_Gold.Text = ANNUALIZED.Convert(i64Money);
	}

	public void ShowMailInfo(GS_MAILBOX_MAILINFO_ACK MailInfo)
	{
		this.SetMailData(false, MailInfo.nIdx, (eMAIL_TYPE)MailInfo.i32MailType, TKString.NEWString(MailInfo.szSendName), string.Empty, MailInfo.nMoney, MailInfo.UserItem, MailInfo.byrBinaryData, TKString.NEWString(MailInfo.szMsg), false, MailInfo.i64SolID, MailInfo.i32CharKind, MailInfo.ui8Grade, MailInfo.i16Level);
	}

	public void ShowHistoryInfo(GS_MAILBOX_HISTORY_ACK HistoryInfo, NkDeserializePacket kDeserializePacket)
	{
		this.InitControl();
		this.m_HistoryInfo = HistoryInfo;
		this.SetMailData(true, this.m_HistoryInfo.i64MailID, (eMAIL_TYPE)this.m_HistoryInfo.m_i32rMailType, TKString.NEWString(this.m_HistoryInfo.szrCharName_Send), TKString.NEWString(this.m_HistoryInfo.szrCharName_Recv), this.m_HistoryInfo.m_i64rCharMoney, this.m_HistoryInfo.UserItem, HistoryInfo.byBinaryData, TKString.NEWString(this.m_HistoryInfo.m_szrMSG), this.m_HistoryInfo.bDidISend, this.m_HistoryInfo.i64SolID, this.m_HistoryInfo.i32CharKind, this.m_HistoryInfo.ui8Grade, this.m_HistoryInfo.i16Level);
	}

	private void OnClickReply(IUIObject obj)
	{
		PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
		if (postDlg != null)
		{
			string text = this._l0_Label_Title.Data as string;
			if (text != null)
			{
				postDlg.ConfirmRequestByName(text);
				this.Hide();
			}
		}
	}

	private void OnClickOK(IUIObject obj)
	{
		if (this.m_eMailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_ADD_FRIEND_FACEBOOK)
		{
			GS_OTHERCHAR_INFO_PERMIT_REQ gS_OTHERCHAR_INFO_PERMIT_REQ = new GS_OTHERCHAR_INFO_PERMIT_REQ();
			TKString.StringChar(this.m_strSendCharName.Trim(), ref gS_OTHERCHAR_INFO_PERMIT_REQ.szCharName);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_OTHERCHAR_INFO_PERMIT_REQ, gS_OTHERCHAR_INFO_PERMIT_REQ);
		}
		if (this.m_eMailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_FRIEND)
		{
			this.SEND_FRIEND_APPLY(0);
		}
		this.SEND_GS_MAILBOX_TAKE_REQ(1);
		this.Close();
	}

	private void OnClickConfirm(IUIObject obj)
	{
		if (this.m_bIsHistory)
		{
			base.CloseNow();
			return;
		}
		if (this.m_eMailType == eMAIL_TYPE.MAIL_TYPE_REPORT_MINE_RESULT || this.m_eMailType == eMAIL_TYPE.MAIL_TYPE_EXPEDITION_RESULT_REPORT)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("514"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
		else
		{
			base.CloseNow();
		}
	}

	private void OnClickCancel(IUIObject obj)
	{
		if (this.m_eMailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_FRIEND)
		{
			this.SEND_FRIEND_APPLY(1);
		}
		this.SEND_GS_MAILBOX_TAKE_REQ(0);
		base.CloseNow();
	}

	private void OnClickReport(IUIObject obj)
	{
		this.SEND_GS_MAILBOX_REPORT_REQ();
	}

	private void SEND_GS_MAILBOX_TAKE_REQ(byte i8Accept)
	{
		string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(this.m_Item.m_nItemUnique);
		if (!string.IsNullOrEmpty(itemMaterialCode))
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_ITEM", itemMaterialCode, "DROP", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else if (0L < this.m_i64Money)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_ITEM", "MONEY", "GET", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		if (0L < this.m_lSolIDTrade)
		{
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null || readySolList.GetCount() >= 100)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				return;
			}
		}
		GS_MAILBOX_TAKE_REQ gS_MAILBOX_TAKE_REQ = new GS_MAILBOX_TAKE_REQ();
		gS_MAILBOX_TAKE_REQ.nIdx = this.m_i64MailID;
		gS_MAILBOX_TAKE_REQ.nMoney = this.m_i64Money;
		gS_MAILBOX_TAKE_REQ.nItem = this.m_Item;
		gS_MAILBOX_TAKE_REQ.i64SolID_Trade = this.m_lSolIDTrade;
		gS_MAILBOX_TAKE_REQ.i8Accept = i8Accept;
		gS_MAILBOX_TAKE_REQ.i32RecvMailType = (int)this.m_eMailType;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_TAKE_REQ, gS_MAILBOX_TAKE_REQ);
	}

	private void SEND_FRIEND_APPLY(byte YesNo)
	{
		if (YesNo != 0)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COMMUNITY", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		GS_FRIEND_APPLY_YES_NO_REQ gS_FRIEND_APPLY_YES_NO_REQ = new GS_FRIEND_APPLY_YES_NO_REQ();
		gS_FRIEND_APPLY_YES_NO_REQ.ui8YesNo = YesNo;
		TKString.StringChar(this.m_strSendCharName, ref gS_FRIEND_APPLY_YES_NO_REQ.Name);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_APPLY_YES_NO_REQ, gS_FRIEND_APPLY_YES_NO_REQ);
	}

	private void SEND_GS_MAILBOX_REPORT_REQ()
	{
		GS_MAILBOX_REPORT_REQ gS_MAILBOX_REPORT_REQ = new GS_MAILBOX_REPORT_REQ();
		gS_MAILBOX_REPORT_REQ.i64LegionActionID = this.m_i64LegionActionID;
		gS_MAILBOX_REPORT_REQ.i64MailID = this.m_i64MailID;
		gS_MAILBOX_REPORT_REQ.i32MailType = (int)this.m_eMailType;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_REPORT_REQ, gS_MAILBOX_REPORT_REQ);
	}

	public void GetAuctionMessage(eMAIL_TYPE eMailType, AUCTIONINFO AuctionInfo, ref string strMessage)
	{
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = AuctionInfo.i32ItemUnique;
		iTEM.m_nOption[4] = AuctionInfo.i32BattleSkillUnique;
		iTEM.m_nOption[2] = AuctionInfo.i32MakeRank;
		string text = string.Empty;
		if (0 < AuctionInfo.i32ItemUnique)
		{
			text = NrTSingleton<ItemManager>.Instance.GetRankColorName(iTEM);
		}
		else if (0 < AuctionInfo.i32CharKind)
		{
			text = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(AuctionInfo.i32CharKind);
		}
		string arg = string.Empty;
		switch (eMailType)
		{
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER:
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref arg, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1349"),
				"itemname",
				text
			});
			goto IL_1C7;
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_BEFORETENDER:
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref arg, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1156"),
				"itemname",
				text
			});
			goto IL_1C7;
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_TENDER:
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref arg, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1158"),
				"itemname",
				text
			});
			goto IL_1C7;
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER_CANCEL:
			arg = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1145");
			goto IL_1C7;
		case eMAIL_TYPE.MAIL_TYPE_REPORT_MINE_RESULT:
			IL_97:
			switch (eMailType)
			{
			case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD:
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref arg, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2276"),
					"ItemName",
					text
				});
				goto IL_1C7;
			case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD_CANCEL_REGISTER:
			case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD_CANCEL_TENDER:
				arg = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2277");
				goto IL_1C7;
			default:
				goto IL_1C7;
			}
			break;
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER_FAIL:
			arg = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1160");
			goto IL_1C7;
		}
		goto IL_97;
		IL_1C7:
		strMessage = string.Format("{0} \r\n \r\n \r\n {1}", arg, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1545"));
	}
}
