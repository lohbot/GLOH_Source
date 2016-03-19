using System;
using System.Collections.Generic;
using UnityForms;

public class PlunderSolNumDlg : Form
{
	private Label m_lHelpText;

	private Label m_lCharName;

	private Label m_lCharType;

	private Label m_lSkillInfo;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/dlg_pvp_solnum", G_ID.PLUNDERSOLNUM_DLG, false);
		base.DonotDepthChange(1000f);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_lHelpText = (base.GetControl("Label_Help") as Label);
		this.m_lCharName = (base.GetControl("Label_Charname") as Label);
		this.m_lCharType = (base.GetControl("Label_Chartype") as Label);
		this.m_lSkillInfo = (base.GetControl("Label_Skill") as Label);
		float x = 0f;
		float y = 0f;
		base.SetLocation(x, y, base.GetLocation().z);
	}

	public override void InitData()
	{
	}

	public void SetExplain()
	{
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
	}

	public void SetSeleteSol(long nSolID)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(nSolID);
		if (soldierInfoFromSolID == null)
		{
			base.SetShowLayer(1, true);
			base.SetShowLayer(2, false);
		}
		else
		{
			base.SetShowLayer(1, false);
			base.SetShowLayer(2, true);
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("567");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"targetname",
				soldierInfoFromSolID.GetName(),
				"count",
				soldierInfoFromSolID.GetLevel().ToString()
			});
			string text = string.Empty;
			NrCharKindInfo charKindInfo = soldierInfoFromSolID.GetCharKindInfo();
			if (charKindInfo != null)
			{
				if (charKindInfo.GetCHARKIND_ATTACKINFO().ATTACKTYPE == soldierInfoFromSolID.GetAttackInfo().ATTACKTYPE)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(charKindInfo.GetCHARKIND_INFO().SoldierSpec1);
				}
				else
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(charKindInfo.GetCHARKIND_INFO().SoldierSpec2);
				}
			}
			string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("992");
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				textFromInterface2,
				"type",
				text
			});
			int num = 0;
			string text2 = string.Empty;
			List<BATTLESKILL_TRAINING> battleSkillTrainingGroup = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTrainingGroup(soldierInfoFromSolID);
			if (battleSkillTrainingGroup != null)
			{
				foreach (BATTLESKILL_TRAINING current in battleSkillTrainingGroup)
				{
					int nSkillUnique = current.m_nSkillUnique;
					BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(nSkillUnique);
					if (battleSkillBase != null)
					{
						num = soldierInfoFromSolID.GetBattleSkillLevel(current.m_nSkillUnique);
						text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey);
						break;
					}
				}
			}
			string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1292");
			string empty3 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
			{
				textFromInterface3,
				"skillname",
				text2,
				"skilllevel",
				num.ToString()
			});
			this.m_lCharName.Text = empty;
			this.m_lCharType.Text = empty2;
			this.m_lSkillInfo.Text = empty3;
		}
		this.GuildBossBattleUserName();
	}

	public void GuildBossBattleUserName()
	{
		string empty = string.Empty;
		NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(SoldierBatch.GUILDBOSS_INFO.m_i64CurPlayer);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			if (memberInfoFromPersonID != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1963"),
					"targetname",
					memberInfoFromPersonID.GetCharName()
				});
				this.m_lHelpText.Visible = true;
				this.m_lHelpText.Text = empty;
			}
			else
			{
				this.m_lHelpText.Visible = true;
				this.m_lHelpText.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("991");
			}
		}
	}

	public void SetSolNum(int nSolNum)
	{
	}
}
