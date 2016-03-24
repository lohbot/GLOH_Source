using System;
using UnityEngine;

namespace CostumeRoomDlg
{
	public class CostumeViewerSetter
	{
		private CostumeRoom_Dlg _owner;

		public CharCostumeInfo_Data _settdCostumeData;

		private CostumeStatSetter _costumeStatSetter;

		public CostumeSkillSetter _costumeSkillSetter;

		public CostumeCharSetter _costumeCharSetter;

		public CostumeViewerSetter(CostumeRoom_Dlg owner)
		{
			this._owner = owner;
			this._costumeStatSetter = new CostumeStatSetter(owner);
			this._costumeSkillSetter = new CostumeSkillSetter(owner);
			this._costumeCharSetter = new CostumeCharSetter(owner);
		}

		public void Update()
		{
			this._costumeCharSetter.Update();
		}

		public void OnClose()
		{
			this._costumeCharSetter.OnClose();
		}

		public void InitCostumeView(CharCostumeInfo_Data costumeData)
		{
			if (costumeData == null)
			{
				Debug.LogError("ERROR, CostumeViewerSetter.cs, InitCostumeView(), costumeData is Null");
				return;
			}
			this._settdCostumeData = costumeData;
			this._costumeSkillSetter.InitCostumeSkill(costumeData.m_SkillUnique);
			this._costumeStatSetter.InitCostumeStat(costumeData, this._owner._myCharListSetter._SelectedSolInfo);
			this._costumeCharSetter.InitCostumeChar(costumeData);
			this.InitCostummeImmediatelyBuyButton(costumeData);
			this.InitCostumeName(costumeData);
			this.CostumeVisibleCheckProcess();
		}

		public void UIHide(bool hide)
		{
			if (this._owner._variables._uiHideCheckBox.IsChecked())
			{
				hide = true;
			}
			this._costumeStatSetter.UIHide(hide);
			this._costumeSkillSetter.UIHide(hide);
		}

		private void InitCostumeName(CharCostumeInfo_Data costumeData)
		{
			this._owner._variables._lbCostumeName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(costumeData.m_CostumeTextKey);
		}

		private void InitCostummeImmediatelyBuyButton(CharCostumeInfo_Data costumeData)
		{
			if (costumeData.IsNormalCostume())
			{
				if (this._owner._variables._lbImmediate != null)
				{
					this._owner._variables._lbImmediate.SetControlState(UIButton.CONTROL_STATE.DISABLED);
				}
				this._owner._variables._btnImmediatelyBuy.SetControlState(UIButton.CONTROL_STATE.DISABLED);
			}
			else
			{
				if (this._owner._variables._lbImmediate != null)
				{
					this._owner._variables._lbImmediate.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
				}
				this._owner._variables._btnImmediatelyBuy.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
			}
		}

		private void CostumeVisibleCheckProcess()
		{
			this.UIHide(false);
		}

		private bool IsNormalCostume()
		{
			return this._settdCostumeData == null || this._settdCostumeData.IsNormalCostume();
		}
	}
}
