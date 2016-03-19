using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class PostResultDlg : Form
{
	private Label m_lbBattleResult;

	private Label[] m_lbGeneralName = new Label[5];

	private Label[] m_lbGeneralLevel = new Label[5];

	private Label[] m_lbGeneralExp = new Label[5];

	private Label[] m_lbGeneralDeathSOL = new Label[5];

	private Label[] m_lbGeneralinjurySOL = new Label[5];

	private Label[] m_lbExpGot = new Label[5];

	private DrawTexture[] m_dtGeneralBG = new DrawTexture[5];

	private DrawTexture[] m_dtGeneralIconBG = new DrawTexture[5];

	private DrawTexture[] m_dtExpBG = new DrawTexture[5];

	private DrawTexture[] m_dtSOLBG = new DrawTexture[5];

	private DrawTexture[] m_dtGeneralIcon = new DrawTexture[5];

	private DrawTexture[] m_dtExp = new DrawTexture[5];

	private DrawTexture[] m_dtSOL = new DrawTexture[5];

	private Button m_btClose;

	private Button m_btTakeItem;

	private ListBox m_listbox_Item;

	private DrawTexture dtBackGround;

	private DrawTexture dtSymbol_Win;

	private DrawTexture dtSymbol_Lose;

	private DrawTexture dtSymbol_Expedition;

	private bool m_bIsHistory;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Post/DLG_Post_Result", G_ID.POST_RESULT_DLG, false);
	}

	public override void SetComponent()
	{
		this.dtBackGround = (base.GetControl("Bg_Win") as DrawTexture);
		this.dtBackGround.AddBoxCollider();
		this.dtBackGround.SetUseBoxCollider(true);
		this.m_lbBattleResult = (base.GetControl("Label_Label6") as Label);
		for (int i = 0; i < 5; i++)
		{
			int num = i + 1;
			this.m_lbGeneralName[i] = (base.GetControl("Label_Slot" + num + "_Name") as Label);
			this.m_lbGeneralLevel[i] = (base.GetControl("Label_Slot" + num + "_Lv") as Label);
			this.m_lbGeneralExp[i] = (base.GetControl("Label_Slot" + num + "_Lael2") as Label);
			this.m_lbExpGot[i] = (base.GetControl("Label_Slot" + num + "_Lael1") as Label);
			this.m_lbGeneralDeathSOL[i] = (base.GetControl("Label_Slot" + num + "_Lael3") as Label);
			this.m_lbGeneralinjurySOL[i] = (base.GetControl("Label_Slot" + num + "_Lael4") as Label);
			this.m_dtGeneralBG[i] = (base.GetControl("DrawTexture_Slot" + num + "_bg") as DrawTexture);
			this.m_dtGeneralIconBG[i] = (base.GetControl("DrawTexture_slot" + num + "_GBG1") as DrawTexture);
			this.m_dtExpBG[i] = (base.GetControl("DrawTexture_slot" + num + "_GBG2") as DrawTexture);
			this.m_dtSOLBG[i] = (base.GetControl("DrawTexture_slot" + num + "_GBG3") as DrawTexture);
			this.m_dtGeneralIcon[i] = (base.GetControl("DrawTexture_slot" + num + "_Img1") as DrawTexture);
			this.m_dtExp[i] = (base.GetControl("DrawTexture_slot" + num + "_Img2") as DrawTexture);
			this.m_dtSOL[i] = (base.GetControl("DrawTexture_slot" + num + "_Img3") as DrawTexture);
		}
		this.m_btClose = (base.GetControl("Button_Button90") as Button);
		Button expr_27D = this.m_btClose;
		expr_27D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_27D.Click, new EZValueChangedDelegate(this.OnClickClose));
		if (Screen.width > 1024)
		{
			this.m_btTakeItem = (base.GetControl("Button_Close2") as Button);
			Button expr_2C9 = this.m_btTakeItem;
			expr_2C9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2C9.Click, new EZValueChangedDelegate(this.OnClickTakeItem));
			Button button = base.GetControl("Button_Close") as Button;
			button.Hide(true);
		}
		else
		{
			this.m_btTakeItem = (base.GetControl("Button_Close") as Button);
			Button expr_323 = this.m_btTakeItem;
			expr_323.Click = (EZValueChangedDelegate)Delegate.Combine(expr_323.Click, new EZValueChangedDelegate(this.OnClickTakeItem));
			Button button2 = base.GetControl("Button_Close2") as Button;
			button2.Hide(true);
		}
		this.m_listbox_Item = (base.GetControl("ListBox_ListBox84") as ListBox);
		this.m_listbox_Item.UseColumnRect = true;
		this.m_listbox_Item.ColumnNum = 12;
		this.m_listbox_Item.LineHeight = 66f;
		this.m_listbox_Item.SetColumnRect(0, 0, 0, 61, 61);
		this.m_listbox_Item.SetColumnRect(1, 0, 0, 60, 60);
		this.m_listbox_Item.SetColumnRect(2, 67, 9, 158, 20, SpriteText.Anchor_Pos.Middle_Left);
		this.m_listbox_Item.SetColumnRect(3, 68, 34, 158, 20, SpriteText.Anchor_Pos.Middle_Left);
		this.m_listbox_Item.SetColumnRect(4, 230, 0, 61, 61);
		this.m_listbox_Item.SetColumnRect(5, 230, 0, 60, 60);
		this.m_listbox_Item.SetColumnRect(6, 297, 9, 158, 20, SpriteText.Anchor_Pos.Middle_Left);
		this.m_listbox_Item.SetColumnRect(7, 297, 34, 158, 20, SpriteText.Anchor_Pos.Middle_Left);
		this.m_listbox_Item.SetColumnRect(8, 460, 0, 61, 61);
		this.m_listbox_Item.SetColumnRect(9, 460, 0, 60, 60);
		this.m_listbox_Item.SetColumnRect(10, 527, 9, 158, 20, SpriteText.Anchor_Pos.Middle_Left);
		this.m_listbox_Item.SetColumnRect(11, 527, 34, 158, 20, SpriteText.Anchor_Pos.Middle_Left);
		this.m_listbox_Item.SelectStyle = "Com_I_Transparent";
		this.dtSymbol_Win = (base.GetControl("Symbol_Win") as DrawTexture);
		this.dtSymbol_Win.SetTextureKey("Bat_I_ResultWin");
		this.dtSymbol_Win.SetAlpha(0.6f);
		this.dtSymbol_Lose = (base.GetControl("Symbol_Lose") as DrawTexture);
		this.dtSymbol_Lose.SetTexture("Bat_I_ResultLose");
		this.dtSymbol_Lose.SetAlpha(0.6f);
		this.dtSymbol_Expedition = (base.GetControl("Symbol_Expedition") as DrawTexture);
		this.dtSymbol_Expedition.SetTexture("Bat_I_Expedition");
	}

	public void InitControl()
	{
		this.m_lbBattleResult.Text = string.Empty;
		for (int i = 0; i < 5; i++)
		{
			this.m_lbGeneralName[i].Text = string.Empty;
			this.m_lbGeneralLevel[i].Text = string.Empty;
			this.m_lbGeneralExp[i].Text = string.Empty;
			this.m_lbGeneralDeathSOL[i].Text = string.Empty;
			this.m_lbGeneralinjurySOL[i].Text = string.Empty;
		}
	}

	public void SetData(bool bIsHistory, long i64MailID, eMAIL_TYPE mailtype, string strCharName_Offence, string strSendObjectName)
	{
		this.InitControl();
		this.m_bIsHistory = bIsHistory;
		this.SetLocationFromResolution(GUICamera.width, GUICamera.height);
		if (bIsHistory)
		{
			this.m_btTakeItem.Text = "닫기";
		}
	}

	public override void SetLocationFromResolution(float _width, float _height)
	{
	}

	private UIBaseInfoLoader GetLoaderImg(string Key)
	{
		UIBaseInfoLoader uIBaseInfoLoader = new UIBaseInfoLoader();
		uIBaseInfoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_3x3;
		NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(Key, ref uIBaseInfoLoader);
		return uIBaseInfoLoader;
	}

	private void SetSymbol(bool bIsMilitaryHunt, bool _Win)
	{
		if (bIsMilitaryHunt)
		{
			this.dtSymbol_Win.Hide(true);
			this.dtSymbol_Lose.Hide(true);
		}
		else
		{
			this.dtSymbol_Expedition.Hide(true);
			if (_Win)
			{
				this.dtSymbol_Lose.Hide(true);
			}
			else
			{
				this.dtSymbol_Win.Hide(true);
			}
		}
	}

	private void OnClickClose(IUIObject obj)
	{
		base.CloseNow();
	}

	private void OnClickTakeItem(IUIObject obj)
	{
		if (this.m_bIsHistory)
		{
			base.CloseNow();
			return;
		}
		GS_MAILBOX_TAKE_REPORT_REQ gS_MAILBOX_TAKE_REPORT_REQ = new GS_MAILBOX_TAKE_REPORT_REQ();
		for (int i = 0; i < 5; i++)
		{
			gS_MAILBOX_TAKE_REPORT_REQ.i32ItemUnique[i] = 0;
			gS_MAILBOX_TAKE_REPORT_REQ.i32ItemNum[i] = 0;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MAILBOX_TAKE_REPORT_REQ, gS_MAILBOX_TAKE_REPORT_REQ);
		base.CloseNow();
		PostRecvDlg postRecvDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_RECV_DLG) as PostRecvDlg;
		if (postRecvDlg != null)
		{
			postRecvDlg.CloseNow();
		}
	}
}
