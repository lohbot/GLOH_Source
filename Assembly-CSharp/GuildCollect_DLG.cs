using GAME;
using System;
using UnityForms;

public class GuildCollect_DLG : Form
{
	private enum eGUILD_LAYER_TYPE
	{
		NONE,
		GUILD,
		MINE,
		GUILDBOSS,
		MINE_LOCK,
		GUILDBOSS_LOCK,
		MAX
	}

	private const int COUNT_BUTTON_GUILD = 10;

	private const int COUNT_BUTTON_MINE = 12;

	private const int COUNT_BUTTON_GUILDBOSS = 9;

	private DrawTexture m_dtBG;

	private Button[] m_btnGuild;

	private Button[] m_btnMine;

	private Button[] m_btnGuildBoss;

	private DrawTexture m_dtGuildNotice;

	private DrawTexture m_dtGuildBossNotice;

	private DrawTexture m_dtMineLock;

	private Label m_lbMineLock;

	private DrawTexture m_dtGuildBossLock;

	private Label m_lbGuildBossLock;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "uibutton/DLG_Guild_Main", G_ID.GUILDCOLLECT_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btnGuild = new Button[10];
		this.m_btnMine = new Button[12];
		this.m_btnGuildBoss = new Button[9];
		this.m_dtBG = (base.GetControl("DT_Back") as DrawTexture);
		this.m_dtBG.SetTextureFromBundle("ui/uibutton/guild_bg");
		for (int i = 0; i < 10; i++)
		{
			this.m_btnGuild[i] = (base.GetControl(string.Format("Btn_Guild{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnGuild[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Guild));
		}
		for (int i = 0; i < 12; i++)
		{
			this.m_btnMine[i] = (base.GetControl(string.Format("Btn_Mine{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnMine[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Mine));
		}
		for (int i = 0; i < 9; i++)
		{
			this.m_btnGuildBoss[i] = (base.GetControl(string.Format("Btn_Babel{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnGuildBoss[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_GuildBoss));
		}
		this.m_dtGuildNotice = (base.GetControl("DT_Notice1") as DrawTexture);
		this.m_dtGuildBossNotice = (base.GetControl("DT_Notice2") as DrawTexture);
		this.m_dtMineLock = (base.GetControl("DT_LockBG01") as DrawTexture);
		this.m_lbMineLock = (base.GetControl("LB_Lock1") as Label);
		this.m_dtGuildBossLock = (base.GetControl("DT_LockBG02") as DrawTexture);
		this.m_lbGuildBossLock = (base.GetControl("LB_Lock2") as Label);
		this.Set_GuildButtons();
		this.Update_Notice();
	}

	public void Set_GuildButtons()
	{
		base.SetShowLayer(4, false);
		base.SetShowLayer(5, false);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int level = kMyCharInfo.GetLevel();
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_MYTHRAID_LIMITLEVEL);
		bool flag = 0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		bool flag2 = NrTSingleton<ContentsLimitManager>.Instance.IsMineLimit();
		bool bLock = flag || level < value || flag2;
		this.Set_LockButtons(GuildCollect_DLG.eGUILD_LAYER_TYPE.MINE_LOCK, bLock, flag2, flag, value);
		flag2 = !NrTSingleton<ContentsLimitManager>.Instance.IsGuildBoss();
		bool bLock2 = flag || flag2;
		this.Set_LockButtons(GuildCollect_DLG.eGUILD_LAYER_TYPE.GUILDBOSS_LOCK, bLock2, flag2, flag, level);
	}

	private void Set_LockButtons(GuildCollect_DLG.eGUILD_LAYER_TYPE eType, bool bLock, bool bLimit, bool bNoGuild, int nLevel)
	{
		string empty = string.Empty;
		if (bLock)
		{
			if (bLimit)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3516")
				});
			}
			else if (bNoGuild)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3534")
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3466"),
					"level",
					nLevel.ToString()
				});
			}
			base.SetShowLayer((int)eType, true);
			base.SetLayerZ((int)eType, -0.4f);
		}
		if (eType != GuildCollect_DLG.eGUILD_LAYER_TYPE.MINE_LOCK)
		{
			if (eType == GuildCollect_DLG.eGUILD_LAYER_TYPE.GUILDBOSS_LOCK)
			{
				this.m_dtGuildBossLock.SetTextureFromBundle("ui/uibutton/guildbtn_lock");
				this.m_lbGuildBossLock.SetText(empty);
				for (int i = 0; i < 9; i++)
				{
					if (!(this.m_btnGuildBoss[i] == null))
					{
						this.m_btnGuildBoss[i].SetEnabled(!bLock);
					}
				}
			}
		}
		else
		{
			this.m_dtMineLock.SetTextureFromBundle("ui/uibutton/guildbtn_lock");
			this.m_lbMineLock.SetText(empty);
			for (int j = 0; j < 12; j++)
			{
				if (!(this.m_btnMine[j] == null))
				{
					this.m_btnMine[j].SetEnabled(!bLock);
				}
			}
		}
	}

	public void Update_Notice()
	{
		this.m_dtGuildNotice.Visible = false;
		this.m_dtGuildBossNotice.Visible = false;
		if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			return;
		}
		bool guildBossRewardInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossRewardInfo();
		if (guildBossRewardInfo)
		{
			this.m_dtGuildBossNotice.Visible = true;
		}
		bool guildBossCheck = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossCheck();
		if (guildBossCheck)
		{
			this.m_dtGuildBossNotice.Visible = true;
		}
		bool flag = NrTSingleton<NewGuildManager>.Instance.CanGetGoldenEggReward();
		if (flag)
		{
			this.m_dtGuildNotice.Visible = true;
		}
		int readyApplicantCount = NrTSingleton<NewGuildManager>.Instance.GetReadyApplicantCount();
		if (0 < readyApplicantCount)
		{
			this.m_dtGuildNotice.Visible = true;
		}
		if (NrTSingleton<GuildWarManager>.Instance.CanGetGuildWarReward())
		{
			this.m_dtGuildNotice.Visible = true;
		}
	}

	private void Click_Guild(IUIObject Obj)
	{
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
		{
			return;
		}
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(1);
	}

	private void Click_Mine(IUIObject Obj)
	{
		if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID() || !NrTSingleton<ContentsLimitManager>.Instance.IsMineApply((short)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel()))
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("763");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_MINE_TUTORIAL_STEP);
		if (charSubData == 1L)
		{
			MineTutorialStepDlg mineTutorialStepDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_TUTORIAL_STEP_DLG) as MineTutorialStepDlg;
			if (mineTutorialStepDlg != null)
			{
				mineTutorialStepDlg.SetStep(1L);
			}
		}
		else
		{
			NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(1, 1, 0L);
		}
	}

	private void Click_GuildBoss(IUIObject Obj)
	{
		if (NrTSingleton<NewGuildManager>.Instance.GetGuildID() <= 0L)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("545"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		BabelGuildBossDlg babelGuildBossDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABEL_GUILDBOSS_MAIN_DLG) as BabelGuildBossDlg;
		if (babelGuildBossDlg != null)
		{
			babelGuildBossDlg.ShowList();
		}
	}
}
