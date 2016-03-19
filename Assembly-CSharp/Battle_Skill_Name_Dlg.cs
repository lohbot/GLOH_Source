using System;
using UnityEngine;
using UnityForms;

public class Battle_Skill_Name_Dlg : Form
{
	private Label m_lbSkillName;

	private float m_fStartTime;

	private float m_fAniTime = 0.1f;

	private float m_fAlphaTime = 0.5f;

	private float m_fWaitTime = 0.5f;

	public bool m_bCloseUpdate;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		this.TopMost = true;
		this.AlwaysUpdate = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_Skillname", G_ID.BATTLE_SKILL_NAME_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_lbSkillName = (base.GetControl("Label_skill") as Label);
		Vector2 location = new Vector2(GUICamera.width / 2f - base.GetSize().x / 2f, 0f);
		base.SetLocation(location);
		this.Hide();
	}

	public override void Update()
	{
		base.Update();
		if (this.m_bCloseUpdate)
		{
			this.CloseUpdate();
		}
	}

	private void CloseUpdate()
	{
		if (this.m_fStartTime == 0f)
		{
			return;
		}
		float num = Time.time - this.m_fStartTime;
		if (num >= this.m_fAniTime)
		{
			if (num <= this.m_fAniTime || num >= this.m_fAniTime + this.m_fWaitTime)
			{
				if (num > this.m_fAniTime + this.m_fWaitTime && num < this.m_fAniTime + this.m_fWaitTime + this.m_fAlphaTime)
				{
					float alpha = Mathf.Lerp(1f, 0f, (num - (this.m_fAniTime + this.m_fWaitTime)) / this.m_fAlphaTime);
					base.SetAlpha(alpha);
				}
				else
				{
					Battle_Control_Dlg battle_Control_Dlg = (Battle_Control_Dlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG);
					if (battle_Control_Dlg != null)
					{
						battle_Control_Dlg.SetAngergaugeFX_Click(false);
					}
					this.Close();
				}
			}
		}
	}

	public void SetCloseUpdate(bool bSkillNameType)
	{
		this.m_bCloseUpdate = bSkillNameType;
	}

	public void SetMagic(NkBattleChar pkTarget, int BattleSkillUnique)
	{
		if (pkTarget == null)
		{
			return;
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(BattleSkillUnique);
		if (null != this.m_lbSkillName && battleSkillBase != null)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey);
			this.m_lbSkillName.SetText(textFromInterface);
			this.m_fStartTime = Time.time;
			this.Show();
		}
	}
}
