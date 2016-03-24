using System;
using UnityForms;

public class Myth_Evolution_SkillDetail_DLG : Form
{
	private DrawTexture m_DT_SkillIcon;

	private Label m_Label_SkillName;

	private Label m_Label_SkillText;

	private Button m_Button_Exit;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Soldier/Evolution/DLG_Sol_MythSkillDetail", G_ID.MYTH_EVOLUTION_SKILLDETAIL_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_DT_SkillIcon = (base.GetControl("DT_SKILLICON") as DrawTexture);
		this.m_Label_SkillName = (base.GetControl("Label_SkillName") as Label);
		this.m_Label_SkillText = (base.GetControl("Label_SKILLTEXT") as Label);
		this.m_Button_Exit = (base.GetControl("Button_Exit") as Button);
		this.m_Button_Exit.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythSkillDetailClose));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	private void OnClickMythSkillDetailClose(IUIObject obj)
	{
		this.Close();
	}

	public void SetSkillUnique(int i32MythSkillUnique)
	{
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(i32MythSkillUnique);
		if (battleSkillBase != null)
		{
			string text = string.Empty;
			text = battleSkillBase.m_waSkillName;
			this.m_Label_SkillName.SetText(text);
			BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillBase.m_nSkillUnique, battleSkillBase.m_nSkillMaxLevel);
			if (battleSkillDetail != null)
			{
				this.m_Label_SkillText.SetText(battleSkillDetail.m_nSkillTooltip);
			}
			UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(i32MythSkillUnique);
			if (battleSkillIconTexture != null)
			{
				this.m_DT_SkillIcon.SetTexture(battleSkillIconTexture);
			}
		}
	}
}
