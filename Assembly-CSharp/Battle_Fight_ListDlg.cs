using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityForms;

public class Battle_Fight_ListDlg : Form
{
	private Button m_btFight;

	private NewListBox m_nlbCharList;

	private Button m_btNext;

	private Button m_btPre;

	private Box m_bxPage;

	private int m_nNumPerPage;

	private int m_nCurPage;

	private int m_nTotalPage;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Battle/FIGHT/DLG_fight_list", G_ID.BATTLE_FIGHT_LIST_DLG, false);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_nlbCharList = (base.GetControl("NewListBox_fight_list") as NewListBox);
		this.m_nlbCharList.Clear();
		this.m_nNumPerPage = (int)(this.m_nlbCharList.GetSize().y / this.m_nlbCharList.LineHeight);
		this.m_btFight = (base.GetControl("Button_match") as Button);
		Button expr_63 = this.m_btFight;
		expr_63.Click = (EZValueChangedDelegate)Delegate.Combine(expr_63.Click, new EZValueChangedDelegate(this.OnRequestAllowFight));
		this.m_btNext = (base.GetControl("Button_next1") as Button);
		Button expr_A0 = this.m_btNext;
		expr_A0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_A0.Click, new EZValueChangedDelegate(this.OnNextPage));
		this.m_btPre = (base.GetControl("Button_pre1") as Button);
		Button expr_DD = this.m_btPre;
		expr_DD.Click = (EZValueChangedDelegate)Delegate.Combine(expr_DD.Click, new EZValueChangedDelegate(this.OnPrePage));
		this.m_bxPage = (base.GetControl("Box_page1") as Box);
		this.SetCharList();
		this.SetPageNum();
		base.SetScreenCenter();
	}

	public override void InitData()
	{
		base.InitData();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "FIGHT", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		base.SetScreenCenter();
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "FIGHT", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void Update()
	{
	}

	public void OnRequestAllowFight(IUIObject obj)
	{
		if (this.m_nlbCharList.Count <= 0)
		{
			return;
		}
		if (this.m_nlbCharList.GetSelectItem() == null)
		{
			return;
		}
		string text = this.m_nlbCharList.GetSelectItem().Data as string;
		if (text == null)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("22");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("18"),
			"charname",
			text
		});
		msgBoxUI.SetMsg(new YesDelegate(this.OnFightAllowOK), text, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
	}

	public void OnFightAllowOK(object a_oObject)
	{
		string text = a_oObject as string;
		if (text == null)
		{
			return;
		}
		GS_BATTLE_FIGHT_ALLOW_REQ gS_BATTLE_FIGHT_ALLOW_REQ = new GS_BATTLE_FIGHT_ALLOW_REQ();
		TKString.StringChar(text, ref gS_BATTLE_FIGHT_ALLOW_REQ.szCharName);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_FIGHT_ALLOW_REQ, gS_BATTLE_FIGHT_ALLOW_REQ);
	}

	public void OnNextPage(IUIObject obj)
	{
		if (this.m_nCurPage + 1 >= this.m_nTotalPage)
		{
			return;
		}
		this.m_nCurPage++;
		this.SetCharList();
		this.SetPageNum();
	}

	public void OnPrePage(IUIObject obj)
	{
		if (this.m_nCurPage - 1 < 0)
		{
			return;
		}
		this.m_nCurPage--;
		this.SetCharList();
		this.SetPageNum();
	}

	private void SetCharList()
	{
		NrCharBase[] @char = NrTSingleton<NkCharManager>.Instance.Get_Char();
		NrCharBase char2 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		int num = 0;
		this.m_nlbCharList.Clear();
		NrCharBase[] array = @char;
		for (int i = 0; i < array.Length; i++)
		{
			NrCharBase nrCharBase = array[i];
			if (nrCharBase != null && nrCharBase.GetPersonID() > 0L && char2.GetPersonID() != nrCharBase.GetPersonID())
			{
				if (num >= this.m_nCurPage * this.m_nNumPerPage && num < (this.m_nCurPage + 1) * this.m_nNumPerPage)
				{
					NewListItem newListItem = new NewListItem(this.m_nlbCharList.ColumnNum, true);
					newListItem.SetListItemData(0, nrCharBase.GetCharName(), null, null, null);
					newListItem.SetListItemData(1, nrCharBase.GetPersonInfo().GetLevel(0L).ToString(), null, null, null);
					newListItem.Data = nrCharBase.GetCharName();
					this.m_nlbCharList.Add(newListItem);
					this.m_nlbCharList.RepositionItems();
					num++;
				}
				else
				{
					num++;
				}
			}
		}
		this.m_nTotalPage = num / this.m_nNumPerPage + 1;
	}

	private void SetPageNum()
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("568"),
			"Count",
			(this.m_nCurPage + 1).ToString(),
			"Count2",
			this.m_nTotalPage
		});
		this.m_bxPage.Text = empty;
	}
}
