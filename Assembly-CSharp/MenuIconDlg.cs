using GAME;
using NPatch;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class MenuIconDlg : Form
{
	private Button openStoryChat;

	private Box newNotice;

	private Button openMenu;

	private Button closeMenu;

	private Box menuNotice;

	private Button btShopMenu;

	private Label lbShopMenu;

	private Box ShopNotice;

	private Button btPatchDownload;

	private Label lbPatchDownload;

	private float bookmarkUIWidth;

	private Vector3 oldOpenMenuPos = Vector3.zero;

	private Vector3 oldCloseMenuPos = Vector3.zero;

	public int m_nChallengeEvent_Count;

	private bool bDown = true;

	private int count;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Main/DLG_MenuIcon", G_ID.MENUICON_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.openMenu = (base.GetControl("Button_HideMenu") as Button);
		this.openMenu.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOpenMenu));
		this.oldOpenMenuPos = this.openMenu.GetLocation();
		this.openMenu.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1829");
		this.closeMenu = (base.GetControl("Button_ReleaseMenu") as Button);
		this.closeMenu.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCloseMenu));
		this.oldCloseMenuPos = this.closeMenu.GetLocation();
		this.closeMenu.ToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1829");
		this.openStoryChat = (base.GetControl("Button_Chat") as Button);
		this.openStoryChat.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickStoryChat));
		this.newNotice = (base.GetControl("Box_New") as Box);
		this.menuNotice = (base.GetControl("Box_Notice") as Box);
		this.menuNotice.Visible = false;
		this.ShopNotice = (base.GetControl("Box_Notice1") as Box);
		this.ShopNotice.Visible = false;
		this.btShopMenu = (base.GetControl("Button_Shop") as Button);
		this.btShopMenu.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickShopMenu));
		this.lbShopMenu = (base.GetControl("Label_ItemShop") as Label);
		this.newNotice.Visible = NrTSingleton<UIDataManager>.Instance.NoticeStoryChat;
		this.btPatchDownload = (base.GetControl("Button_Download") as Button);
		this.btPatchDownload.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickDownLoad));
		this.lbPatchDownload = (base.GetControl("Label_Download") as Label);
		this.lbPatchDownload.Visible = false;
		this.btPatchDownload.Visible = false;
		this.btShopMenu.Visible = true;
		this.ShowDownButton(true);
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.VOUCHER_ITEM, false);
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		GS_CHAR_CHALLENGE_EVENT_REWARD_REQ gS_CHAR_CHALLENGE_EVENT_REWARD_REQ = new GS_CHAR_CHALLENGE_EVENT_REWARD_REQ();
		gS_CHAR_CHALLENGE_EVENT_REWARD_REQ.i64PersonID = @char.GetPersonID();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_CHALLENGE_EVENT_REWARD_REQ, gS_CHAR_CHALLENGE_EVENT_REWARD_REQ);
	}

	public void ShowDownButton(bool flag)
	{
		if (Launcher.Instance.LocalPatchLevel == Launcher.Instance.PatchLevelMax)
		{
			this.btPatchDownload.Visible = false;
			this.lbPatchDownload.Visible = false;
			this.btShopMenu.Visible = true;
			this.lbShopMenu.Visible = true;
		}
		else
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo.GetLevel() < 7)
			{
				this.btPatchDownload.Visible = false;
				this.lbPatchDownload.Visible = false;
				this.btShopMenu.Visible = true;
				this.lbShopMenu.Visible = true;
			}
			else
			{
				this.btPatchDownload.Visible = true;
				this.lbPatchDownload.Visible = true;
				this.btShopMenu.Visible = false;
				this.lbShopMenu.Visible = false;
				this.ShopNotice.Visible = false;
			}
		}
	}

	private void ClickShopMenu(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(ItemMallDlg.FirstTab, true);
	}

	private void ClickDownLoad(IUIObject obj)
	{
		if (!this.bDown)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONVER_PLATFORMID_DLG);
		}
		else
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI == null)
			{
				return;
			}
			msgBoxUI.SetMsg(new YesDelegate(this.OnOKDownStart), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2458"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("213"), eMsgType.MB_OK_CANCEL, 2);
			msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10"));
			msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
		}
	}

	public void OnOKDownStart(object a_oObject)
	{
		Launcher.Instance.SavePatchLevel(Launcher.Instance.PatchLevelMax);
		NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
	}

	public void ShowNotice()
	{
		this.newNotice.Visible = NrTSingleton<UIDataManager>.Instance.NoticeStoryChat;
	}

	private void ClickStoryChat(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.STORYCHAT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.STORYCHAT_DLG);
			if (this.newNotice.Visible)
			{
				this.newNotice.Visible = false;
				NrTSingleton<UIDataManager>.Instance.NoticeStoryChat = false;
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.STORYCHAT_DLG);
		}
	}

	private void RePosition()
	{
		BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
		if (bookmarkDlg != null)
		{
			if (bookmarkDlg.IsHide())
			{
				this.bookmarkUIWidth = 0f;
			}
			else
			{
				this.bookmarkUIWidth = bookmarkDlg.GetSizeX();
			}
			base.SetLocation(GUICamera.width - this.bookmarkUIWidth - base.GetSizeX(), GUICamera.height - base.GetSizeY());
		}
		else
		{
			base.SetLocation(GUICamera.width - base.GetSizeX(), GUICamera.height - base.GetSizeY());
		}
	}

	public override void ChangedResolution()
	{
		this.RePosition();
	}

	public override void InitData()
	{
		this.RePosition();
		this.openMenu.Visible = true;
		this.closeMenu.Visible = false;
	}

	public void ClickOpenMenu(IUIObject obj)
	{
		this.openMenu.transform.localScale = Vector3.one;
		this.openMenu.transform.localPosition = this.oldOpenMenuPos;
		this.openMenu.Visible = false;
		this.closeMenu.Visible = true;
		Form form = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAINMENU_DLG);
		form.Show();
	}

	public void ClickCloseMenu(IUIObject obj)
	{
		this.closeMenu.transform.localScale = Vector3.one;
		this.closeMenu.transform.localPosition = this.oldCloseMenuPos;
		this.openMenu.Visible = true;
		this.closeMenu.Visible = false;
		NrTSingleton<FormsManager>.Instance.Hide(G_ID.MAINMENU_DLG);
	}

	public override void Update()
	{
		int noticeCount = this.GetNoticeCount();
		if (0 < noticeCount)
		{
			if (this.count != noticeCount)
			{
				int num = noticeCount;
				this.menuNotice.Visible = true;
				this.menuNotice.Text = num.ToString();
				this.count = num;
			}
		}
		else if (this.menuNotice.Visible)
		{
			this.menuNotice.Visible = false;
		}
		List<ITEM_MALL_ITEM> group = NrTSingleton<ItemMallItemManager>.Instance.GetGroup(50);
		if (group == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < group.Count; i++)
		{
			ITEM_MALL_ITEM iTEM_MALL_ITEM = group[i];
			if (iTEM_MALL_ITEM != null)
			{
				ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(iTEM_MALL_ITEM.m_Idx);
				if (itemVoucherDataFromItemID != null)
				{
					eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType;
					long voucherRemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTime(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
					if (voucherRemainTime > 0L && NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsUseVoucher(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID))
					{
						flag = true;
						break;
					}
				}
			}
		}
		if (!flag)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_PREMINUM_TIME);
			if (PublicMethod.GetCurTime() > charSubData)
			{
				flag = true;
			}
			charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_HEARTS_TIME);
			if (PublicMethod.GetCurTime() > charSubData)
			{
				flag = true;
			}
		}
		if (flag && this.btShopMenu.Visible)
		{
			this.ShopNotice.Visible = true;
		}
		else
		{
			this.ShopNotice.Visible = false;
		}
	}

	private int GetNoticeCount()
	{
		int num = 0;
		num += NrTSingleton<ChallengeManager>.Instance.GetRewardNoticeCount();
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo != null)
		{
			num += NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.CustomerAnswerCount;
		}
		return num;
	}
}
