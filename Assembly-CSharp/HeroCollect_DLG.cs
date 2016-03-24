using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityForms;

public class HeroCollect_DLG : Form
{
	private enum eHERO_LAYER_TYPE
	{
		NONE,
		HEROINFO,
		HEROCOMPOSE,
		EVOLUTION,
		RECRUIT,
		MAX
	}

	private const int COUNT_BUTTON_HEROINFO = 10;

	private const int COUNT_BUTTON_HEROCOMPOSE = 8;

	private const int COUNT_BUTTON_EVOLUTION = 8;

	private const int COUNT_BUTTON_RECRUIT = 9;

	private DrawTexture m_dtBG;

	private Button[] m_btnHeroInfo;

	private Button[] m_btnHeroCompose;

	private Button[] m_btnEvolution;

	private Button[] m_btnRecruit;

	private DrawTexture m_dtSkillLevelNotice;

	private DrawTexture m_dtTicketNotice;

	private Label m_lbTicketCount;

	private Button m_btnInven;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "uibutton/DLG_Hero_Main", G_ID.HEROCOLLECT_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btnHeroInfo = new Button[10];
		this.m_btnHeroCompose = new Button[8];
		this.m_btnEvolution = new Button[8];
		this.m_btnRecruit = new Button[9];
		this.m_dtBG = (base.GetControl("DT_Back") as DrawTexture);
		this.m_dtBG.SetTextureFromBundle("ui/uibutton/hero_bg");
		for (int i = 0; i < 10; i++)
		{
			this.m_btnHeroInfo[i] = (base.GetControl(string.Format("Btn_HeroInfo{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnHeroInfo[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_HeroInfo));
		}
		for (int i = 0; i < 8; i++)
		{
			this.m_btnHeroCompose[i] = (base.GetControl(string.Format("Btn_HeroCompose{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnHeroCompose[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_HeroCompose));
		}
		for (int i = 0; i < 8; i++)
		{
			this.m_btnEvolution[i] = (base.GetControl(string.Format("Btn_Evolution{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnEvolution[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Evolution));
		}
		for (int i = 0; i < 9; i++)
		{
			this.m_btnRecruit[i] = (base.GetControl(string.Format("Btn_Recruit{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnRecruit[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Recruit));
		}
		this.m_dtSkillLevelNotice = (base.GetControl("DT_Notice1") as DrawTexture);
		this.m_dtTicketNotice = (base.GetControl("DT_NoticeCount") as DrawTexture);
		this.m_lbTicketCount = (base.GetControl("LB_NoticeCount") as Label);
		this.m_btnInven = (base.GetControl("Btn_Inven") as Button);
		this.m_btnInven.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Inven));
		this.Update_Notice();
	}

	public void Update_Notice()
	{
		this.m_dtSkillLevelNotice.Visible = false;
		this.m_dtTicketNotice.Visible = false;
		this.m_lbTicketCount.Visible = false;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo != null && 0 < charPersonInfo.GetUpgradeBattleSkillNum())
		{
			this.m_dtSkillLevelNotice.Visible = true;
		}
		int num = NkUserInventory.GetInstance().GetFunctionItemNum(eITEM_SUPPLY_FUNCTION.SUPPLY_GETSOLDIER);
		if (0 < num)
		{
			if (99 < num)
			{
				num = 99;
			}
			this.m_dtTicketNotice.Visible = true;
			this.m_lbTicketCount.Visible = true;
			this.m_lbTicketCount.SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + num.ToString());
		}
	}

	protected virtual void Click_HeroInfo(IUIObject Obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowHide(G_ID.SOLMILITARYGROUP_DLG);
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG).Visible)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY-INFORMATION", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY-INFORMATION", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		SolMilitarySelectDlg solMilitarySelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYSELECT_DLG) as SolMilitarySelectDlg;
		if (solMilitarySelectDlg != null)
		{
			solMilitarySelectDlg.CloseByParent(82);
		}
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("solinfodlg_open");
	}

	protected virtual void Click_HeroCompose(IUIObject Obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_MAIN_DLG))
		{
			SolComposeMainDlg solComposeMainDlg = (SolComposeMainDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_MAIN_DLG);
			if (!solComposeMainDlg.Visible)
			{
				solComposeMainDlg.Show();
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLCOMPOSE_MAIN_DLG);
		}
	}

	protected virtual void Click_Evolution(IUIObject Obj)
	{
		GS_SOLGUIDE_INFO_REQ gS_SOLGUIDE_INFO_REQ = new GS_SOLGUIDE_INFO_REQ();
		gS_SOLGUIDE_INFO_REQ.bElementMark = true;
		gS_SOLGUIDE_INFO_REQ.i32CharKind = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLGUIDE_INFO_REQ, gS_SOLGUIDE_INFO_REQ);
	}

	protected virtual void Click_Recruit(IUIObject Obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLRECRUIT_DLG))
		{
			GS_TICKET_SELL_INFO_REQ obj = new GS_TICKET_SELL_INFO_REQ();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TICKET_SELL_INFO_REQ, obj);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLRECRUIT_DLG);
		}
	}

	private void Click_Inven(IUIObject Obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowHide(G_ID.INVENTORY_DLG);
	}
}
