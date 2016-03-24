using System;
using UnityForms;

public class CostumeSkillInfo_Dlg : Form
{
	private DrawTexture _skillInfoSkillImage;

	private Label _skillInfoSkillName;

	private Label _skillInfoSkillAnger;

	private Label _skillInfoSkillText;

	private Button _btnExit;

	public override void InitializeComponent()
	{
		Form form = this;
		base.Scale = true;
		base.TopMost = true;
		NrTSingleton<UIBaseFileManager>.Instance.LoadFileAll(ref form, "SolGuide/DLG_SolGuide_SkillDetail", G_ID.COSTUME_SKILLINFO_DLG, false, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
		base.Draggable = false;
	}

	public override void SetComponent()
	{
		this._skillInfoSkillImage = (base.GetControl("DT_SKILLICON") as DrawTexture);
		this._skillInfoSkillName = (base.GetControl("Label_SkillName") as Label);
		this._skillInfoSkillAnger = (base.GetControl("Label_Anger") as Label);
		this._skillInfoSkillText = (base.GetControl("Label_SKILLTEXT") as Label);
		this._btnExit = (base.GetControl("Button_Exit") as Button);
		this._btnExit.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickExit));
	}

	public void InitCostumeSkillInfo(int skillUnique, NkSoldierInfo selectedSolInfo, int selectedCostumeUnique)
	{
		this.SetSkillName(skillUnique);
		this.SetSkillIcon(skillUnique);
		this.SetSkillExplain(skillUnique, ref selectedSolInfo, selectedCostumeUnique);
		this.SetSkillAngryPoint(skillUnique);
	}

	public void OnClickExit(IUIObject obj)
	{
		this.Close();
	}

	private void SetSkillName(int skillUnique)
	{
		if (NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique) == null)
		{
			return;
		}
		string strTextKey = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique).m_strTextKey;
		this._skillInfoSkillName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey);
	}

	private void SetSkillIcon(int skillUnique)
	{
		UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(skillUnique);
		if (battleSkillIconTexture == null)
		{
			return;
		}
		this._skillInfoSkillImage.SetTexture(battleSkillIconTexture);
	}

	private void SetSkillExplain(int skillUnique, ref NkSoldierInfo selectedSolInfo, int selectedCostumeUnique)
	{
		int num = 1;
		if (selectedSolInfo != null)
		{
			num = selectedSolInfo.GetMaxBattleSkillLevel();
		}
		if (num <= 0)
		{
			num = 1;
		}
		BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(skillUnique, num);
		if (battleSkillDetail == null)
		{
			return;
		}
		string empty = string.Empty;
		if (num == 1)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail.m_nSkillTooltip), battleSkillDetail, null, selectedCostumeUnique);
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail.m_nSkillTooltip), battleSkillDetail, selectedSolInfo, selectedCostumeUnique);
		}
		this._skillInfoSkillText.Text = empty;
	}

	private void SetSkillAngryPoint(int skillUnique)
	{
		BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(skillUnique, 1);
		if (battleSkillDetail == null)
		{
			return;
		}
		this._skillInfoSkillAnger.Text = this._skillInfoSkillAnger.Text.Replace("@count@", battleSkillDetail.m_nSkillNeedAngerlyPoint.ToString());
	}
}
