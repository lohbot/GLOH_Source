using System;
using UnityEngine;

namespace CostumeRoomDlg
{
	public class CostumeStatSetter
	{
		private enum COSTUME_STAT
		{
			ATTACK,
			DEFENSE,
			HP
		}

		private CostumeRoom_Dlg _owner;

		private string _attackTextKey = string.Empty;

		private string _defenseTextKey = string.Empty;

		private string _hpTextKey = string.Empty;

		public CostumeStatSetter(CostumeRoom_Dlg owner)
		{
			this._owner = owner;
			this._attackTextKey = "242";
			this._defenseTextKey = "252";
			this._hpTextKey = "1196";
		}

		public void InitCostumeStat(CharCostumeInfo_Data costumeData, NkSoldierInfo soldierInfo)
		{
			this.ClearStatLabels();
			this._owner._variables._charStatBGGradation.Visible = true;
			this.SetNormalCostumeStatInfo(ref costumeData, ref soldierInfo);
			this.SetCostumeStatInfo(ref costumeData, ref soldierInfo);
		}

		public void UIHide(bool hide)
		{
			this.HideStats(hide);
		}

		private void SetNormalCostumeStatInfo(ref CharCostumeInfo_Data costumeData, ref NkSoldierInfo soldierInfo)
		{
			if (!costumeData.IsNormalCostume())
			{
				return;
			}
			this.SetNormalCostumeStatTitle(ref costumeData);
			this.SetNormalCostumeStat(0, soldierInfo, CostumeStatSetter.COSTUME_STAT.ATTACK);
			this.SetNormalCostumeStat(1, soldierInfo, CostumeStatSetter.COSTUME_STAT.DEFENSE);
			this.SetNormalCostumeStat(2, soldierInfo, CostumeStatSetter.COSTUME_STAT.HP);
		}

		private void SetNormalCostumeStatTitle(ref CharCostumeInfo_Data costumeData)
		{
			this.SetCostumeStatTitle(0, this._attackTextKey);
			this.SetCostumeStatTitle(1, this._defenseTextKey);
			this.SetCostumeStatTitle(2, this._hpTextKey);
		}

		private void SetNormalCostumeStat(int labelIdx, NkSoldierInfo soldierInfo, CostumeStatSetter.COSTUME_STAT stat)
		{
			if (soldierInfo == null)
			{
				return;
			}
			COSTUME_STAT_LABEL[] statLabels = this._owner._variables._statLabels;
			if (statLabels == null || statLabels.Length <= labelIdx)
			{
				return;
			}
			statLabels[labelIdx]._costumeStatPercent.Visible = false;
			statLabels[labelIdx]._postStatLabel.Visible = false;
			statLabels[labelIdx]._costumeStatLabel.Visible = true;
			statLabels[labelIdx]._costumeNormalStat.Visible = true;
			if (stat == CostumeStatSetter.COSTUME_STAT.ATTACK)
			{
				statLabels[labelIdx]._costumeNormalStat.Text = soldierInfo.GetMinDamage_NotAdjustCostume().ToString() + "~" + soldierInfo.GetMaxDamage_NotAdjustCostume().ToString();
			}
			else if (stat == CostumeStatSetter.COSTUME_STAT.DEFENSE)
			{
				statLabels[labelIdx]._costumeNormalStat.Text = soldierInfo.GetPhysicalDefense_NotAdjustCostume().ToString();
			}
			else if (stat == CostumeStatSetter.COSTUME_STAT.HP)
			{
				statLabels[labelIdx]._costumeNormalStat.Text = soldierInfo.GetMaxHP_NotAdjustCostume().ToString();
			}
		}

		private void SetCostumeStatInfo(ref CharCostumeInfo_Data costumeData, ref NkSoldierInfo soldierInfo)
		{
			if (costumeData.IsNormalCostume())
			{
				return;
			}
			this.SetCostumeStatListTitle(ref costumeData);
			int num = 0;
			if (costumeData.m_ATKBonusRate != 0)
			{
				this.SetCostumeSoldierStat(num, costumeData.m_ATKBonusRate, soldierInfo, CostumeStatSetter.COSTUME_STAT.ATTACK);
				num++;
			}
			if (costumeData.m_DefBonusRate != 0)
			{
				this.SetCostumeSoldierStat(num, costumeData.m_DefBonusRate, soldierInfo, CostumeStatSetter.COSTUME_STAT.DEFENSE);
				num++;
			}
			if (costumeData.m_HPBonusRate != 0)
			{
				this.SetCostumeSoldierStat(num, costumeData.m_HPBonusRate, soldierInfo, CostumeStatSetter.COSTUME_STAT.HP);
				num++;
			}
		}

		private void SetCostumeSoldierStat(int labelIdx, int statRate, NkSoldierInfo soldierInfo, CostumeStatSetter.COSTUME_STAT stat)
		{
			COSTUME_STAT_LABEL[] statLabels = this._owner._variables._statLabels;
			statLabels[labelIdx]._costumeStatPercent.Visible = true;
			statLabels[labelIdx]._costumeStatPercent.Text = "(+" + statRate.ToString() + "%)";
			if (soldierInfo == null)
			{
				return;
			}
			if (statLabels == null || statLabels.Length <= labelIdx)
			{
				return;
			}
			statLabels[labelIdx]._postStatLabel.Visible = true;
			statLabels[labelIdx]._costumeStatLabel.Visible = true;
			statLabels[labelIdx]._costumeNormalStat.Visible = false;
			if (stat == CostumeStatSetter.COSTUME_STAT.ATTACK)
			{
				statLabels[labelIdx]._postStatLabel.Text = this.GetCalcurate((float)soldierInfo.GetMinDamage_NotAdjustCostume(), (float)statRate) + "~" + this.GetCalcurate((float)soldierInfo.GetMaxDamage_NotAdjustCostume(), (float)statRate);
			}
			else if (stat == CostumeStatSetter.COSTUME_STAT.DEFENSE)
			{
				statLabels[labelIdx]._postStatLabel.Text = this.GetCalcurate((float)soldierInfo.GetPhysicalDefense_NotAdjustCostume(), (float)statRate);
			}
			else if (stat == CostumeStatSetter.COSTUME_STAT.HP)
			{
				statLabels[labelIdx]._postStatLabel.Text = this.GetCalcurate((float)soldierInfo.GetMaxHP_NotAdjustCostume(), (float)statRate);
			}
		}

		private void SetCostumeStatListTitle(ref CharCostumeInfo_Data costumeData)
		{
			int num = 0;
			if (costumeData.m_ATKBonusRate != 0)
			{
				this.SetCostumeStatTitle(num, this._attackTextKey);
				num++;
			}
			if (costumeData.m_DefBonusRate != 0)
			{
				this.SetCostumeStatTitle(num, this._defenseTextKey);
				num++;
			}
			if (costumeData.m_HPBonusRate != 0)
			{
				this.SetCostumeStatTitle(num, this._hpTextKey);
				num++;
			}
		}

		private void SetCostumeStatTitle(int labelIdx, string textKey)
		{
			COSTUME_STAT_LABEL[] statLabels = this._owner._variables._statLabels;
			if (statLabels == null)
			{
				return;
			}
			if (statLabels == null || statLabels.Length <= labelIdx)
			{
				Debug.LogError("ERROR, CostumeViewerSetter.cs, SetCostumeStat(), _owner._statLabels.Length <= labelIdx");
				return;
			}
			statLabels[labelIdx]._costumeStatLabel.Visible = true;
			statLabels[labelIdx]._costumeStatLabel.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(textKey);
		}

		private void ClearStatLabels()
		{
			COSTUME_STAT_LABEL[] statLabels = this._owner._variables._statLabels;
			if (statLabels == null)
			{
				return;
			}
			for (int i = 0; i < statLabels.Length; i++)
			{
				statLabels[i]._costumeStatLabel.Text = string.Empty;
				statLabels[i]._costumeStatLabel.Visible = false;
				statLabels[i]._costumeStatPercent.Text = string.Empty;
				statLabels[i]._costumeStatPercent.Visible = false;
				statLabels[i]._postStatLabel.Text = string.Empty;
				statLabels[i]._postStatLabel.Visible = false;
				statLabels[i]._costumeNormalStat.Text = string.Empty;
				statLabels[i]._costumeNormalStat.Visible = false;
			}
		}

		private void HideStats(bool hide)
		{
			COSTUME_STAT_LABEL[] statLabels = this._owner._variables._statLabels;
			if (statLabels == null)
			{
				return;
			}
			for (int i = 0; i < statLabels.Length; i++)
			{
				statLabels[i]._costumeStatLabel.Visible = !hide;
				statLabels[i]._costumeStatPercent.Visible = !hide;
				statLabels[i]._postStatLabel.Visible = !hide;
				statLabels[i]._costumeNormalStat.Visible = !hide;
			}
			this._owner._variables._charStatBGGradation.Visible = !hide;
		}

		private string GetCalcurate(float original, float plusRate)
		{
			double num = (double)(original + original * plusRate / 100f);
			return ((int)num).ToString();
		}
	}
}
