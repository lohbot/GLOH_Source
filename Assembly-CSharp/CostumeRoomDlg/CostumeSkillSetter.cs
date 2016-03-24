using System;
using UnityForms;

namespace CostumeRoomDlg
{
	public class CostumeSkillSetter
	{
		private CostumeRoom_Dlg _owner;

		private int NOT_SELECTED = -1;

		private int _settingSkillUnique = -1;

		public CostumeSkillSetter(CostumeRoom_Dlg owner)
		{
			this._owner = owner;
			this.NOT_SELECTED = -1;
			this._settingSkillUnique = this.NOT_SELECTED;
		}

		public void InitCostumeSkill(int skillUnique)
		{
			this.SetSkill(skillUnique);
		}

		public void UIHide(bool hide)
		{
			if (this._settingSkillUnique <= 0)
			{
				hide = true;
			}
			this._owner._variables._skillName.Visible = !hide;
			this._owner._variables._skillIcon.Visible = !hide;
			this._owner._variables._skillLine.Visible = !hide;
			this._owner._variables._skillBG.Visible = !hide;
			this._owner._variables._skillTitle.Visible = !hide;
			this._owner._variables._charSkillBGGradation.Visible = !hide;
		}

		public void OnClickCostumeSkill(IUIObject obj)
		{
			this.PopUpCostumeSkillInfo();
		}

		private void PopUpCostumeSkillInfo()
		{
			CostumeSkillInfo_Dlg costumeSkillInfo_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUME_SKILLINFO_DLG) as CostumeSkillInfo_Dlg;
			if (costumeSkillInfo_Dlg != null && costumeSkillInfo_Dlg.Visible)
			{
				return;
			}
			if (this._settingSkillUnique == this.NOT_SELECTED)
			{
				return;
			}
			costumeSkillInfo_Dlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COSTUME_SKILLINFO_DLG) as CostumeSkillInfo_Dlg);
			costumeSkillInfo_Dlg.InitCostumeSkillInfo(this._settingSkillUnique, this._owner._myCharListSetter._SelectedSolInfo, this._owner._costumeListSetter._SelectedCostumeData.m_costumeUnique);
			costumeSkillInfo_Dlg.Show();
		}

		private void SetSkill(int skillUnique)
		{
			if (this._settingSkillUnique == skillUnique)
			{
				return;
			}
			if (!this.SetSkillName(skillUnique))
			{
				return;
			}
			if (!this.SetSkillIcon(skillUnique))
			{
				return;
			}
			this._settingSkillUnique = skillUnique;
		}

		private bool SetSkillName(int skillUnique)
		{
			if (NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique) == null)
			{
				return false;
			}
			string strTextKey = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique).m_strTextKey;
			this._owner._variables._skillName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey);
			return true;
		}

		private bool SetSkillIcon(int skillUnique)
		{
			UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(skillUnique);
			if (battleSkillIconTexture == null)
			{
				return false;
			}
			this._owner._variables._skillIcon.SetTexture(battleSkillIconTexture);
			return true;
		}
	}
}
