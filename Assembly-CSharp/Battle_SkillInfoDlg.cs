using System;
using UnityForms;

public class Battle_SkillInfoDlg : Form
{
	private DrawTexture SkillTargetIcon;

	private Label SkillTargetName;

	private Label SkillTargetLevel;

	private Label SkillCurrentLevel;

	private ScrollLabel SkillCurrentExplain;

	private Label SkillAngerlyPoint;

	private Button m_btClose;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Battle/DLG_Skill_Info", G_ID.BATTLE_SKILLINFO_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.SkillTargetIcon = (base.GetControl("DrawTexture_skillicon") as DrawTexture);
		this.SkillTargetName = (base.GetControl("Label_skillname") as Label);
		this.SkillTargetLevel = (base.GetControl("Label_skill_level") as Label);
		this.SkillCurrentLevel = (base.GetControl("Label_t_level01") as Label);
		this.SkillCurrentExplain = (base.GetControl("ScrollLabel_Explanation01") as ScrollLabel);
		this.SkillAngerlyPoint = (base.GetControl("Label_skill_angerlypoint") as Label);
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		if (TsPlatform.IsMobile)
		{
			base.SetScreenCenter();
		}
	}

	public override void InitData()
	{
	}

	public void SetLocationByForm(Form pkTargetDlg)
	{
		float x = 100f;
		float y = 100f;
		if (pkTargetDlg != null)
		{
			x = pkTargetDlg.GetLocationX() + (pkTargetDlg.GetSizeX() - base.GetSizeX()) / 2f - 15f;
			y = pkTargetDlg.GetLocationY();
		}
		base.SetLocation(x, y);
	}

	public override void Update()
	{
	}

	public void SetDataForBattle(NkSoldierInfo pkSolinfo, int skillunique, int skillLevel)
	{
		if (pkSolinfo == null)
		{
			return;
		}
		if (skillunique <= 0 || skillLevel <= 0)
		{
			return;
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillunique);
		BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(skillunique, skillLevel);
		if (battleSkillBase == null || battleSkillDetail == null)
		{
			return;
		}
		string empty = string.Empty;
		UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase.m_nSkillUnique);
		this.SkillTargetIcon.SetTexture(battleSkillIconTexture);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1293"),
			"skillname",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey)
		});
		this.SkillTargetName.Text = empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1885"),
			"skilllevel",
			skillLevel.ToString()
		});
		this.SkillTargetLevel.Text = empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1868"),
			"skilllevel",
			skillLevel.ToString()
		});
		this.SkillCurrentLevel.Text = empty;
		NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail.m_nSkillTooltip), battleSkillDetail, pkSolinfo, -1);
		this.SkillCurrentExplain.SetScrollLabel(empty);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2153"),
			"count",
			battleSkillDetail.m_nSkillNeedAngerlyPoint.ToString()
		});
		this.SkillAngerlyPoint.Text = empty;
	}
}
