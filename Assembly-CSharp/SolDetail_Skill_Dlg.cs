using System;
using UnityForms;

public class SolDetail_Skill_Dlg : Form
{
	private DrawTexture m_DrawTexture_SkillIcon;

	private Label m_Label_SkillAnger;

	private Label m_Label_SkillName;

	private Label m_Label_SkillText;

	private Button m_btClose;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "SolGuide/DLG_SolGuide_SkillDetail", G_ID.SOLDETAIL_SKILLICON_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_Label_SkillAnger = (base.GetControl("Label_Anger") as Label);
		this.m_Label_SkillName = (base.GetControl("Label_SkillName") as Label);
		this.m_Label_SkillText = (base.GetControl("Label_SKILLTEXT") as Label);
		this.m_DrawTexture_SkillIcon = (base.GetControl("DT_SKILLICON") as DrawTexture);
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		base.Draggable = false;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void SetSkillData(int i32SkillUnique, int i32SkillText, bool bMyth = false)
	{
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(i32SkillUnique);
		int num;
		if (bMyth)
		{
			BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(i32SkillUnique, 1);
			if (battleSkillDetail != null)
			{
				num = int.Parse(battleSkillDetail.m_nSkillTooltip) + 10000;
			}
			else
			{
				num = i32SkillText;
			}
		}
		else
		{
			num = i32SkillText + 30000;
		}
		if (battleSkillBase != null)
		{
			BATTLESKILL_DETAIL battleSkillDetail2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillBase.m_nSkillUnique, battleSkillBase.m_nSkillMaxLevel);
			if (battleSkillDetail2 != null)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2153"),
					"count",
					battleSkillDetail2.m_nSkillNeedAngerlyPoint.ToString()
				});
				this.m_Label_SkillAnger.SetText(empty);
			}
			UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase.m_nSkillUnique);
			this.m_DrawTexture_SkillIcon.SetTexture(battleSkillIconTexture);
			this.m_Label_SkillName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey));
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(num.ToString());
			this.m_Label_SkillText.SetText(textFromInterface);
		}
		if (bMyth)
		{
			this.m_Label_SkillAnger.Visible = false;
		}
	}

	private void OnClickOK(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		base.CloseForm(obj);
	}
}
