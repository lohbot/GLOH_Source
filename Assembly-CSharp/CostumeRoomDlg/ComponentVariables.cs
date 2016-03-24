using System;
using UnityForms;

namespace CostumeRoomDlg
{
	public class ComponentVariables
	{
		private CostumeRoom_Dlg _owner;

		private ComponentSetter _componentSetter;

		public NewListBox _mySolKindListBox;

		public NewListBox _costumeListBox;

		public DrawTexture _skillIcon;

		public DrawTexture _costumeTouchArea;

		public DrawTexture _costumeCharView;

		public Button _skillInfoBtn;

		public Label _skillName;

		public COSTUME_STAT_LABEL[] _statLabels;

		public DrawTexture _mainBG;

		public Button _backBtn;

		public Button _btnWalk;

		public Button _btnStay;

		public Button _btnAttack;

		public Button _btnImmediatelyBuy;

		public Label _lbCostumeName;

		public CheckBox _uiHideCheckBox;

		public Label _skillTitle;

		public DrawTexture _skillBG;

		public DrawTexture _skillLine;

		public Label _lbImmediate;

		public DrawTexture _charSkillBGGradation;

		public DrawTexture _charStatBGGradation;

		public CheckBox _sameKindSolCheckBox;

		public Button _btMyCharListLeftArrow;

		public Button _btMyCharListRightArrow;

		public DrawTexture _dtMyCharListLeftArrowBG;

		public DrawTexture _dtMyCharListRightArrowBG;

		public Label _lbSoulGem;

		public Label _lbMythElixir;

		public Button _btnSoulGemLink;

		public Button _btnSoulGemLink_2;

		public Button _btnMythElixirLink;

		public Button _btnMythElixirLink_2;

		public Button _bthHelp;

		public ComponentVariables(CostumeRoom_Dlg owner)
		{
			this._owner = owner;
			this._componentSetter = new ComponentSetter();
		}

		public void SetComponent()
		{
			this._mySolKindListBox = this._componentSetter.SetMySolListBox(this._owner);
			this._costumeListBox = this._componentSetter.SetCostumeListBox(this._owner);
			this._skillIcon = (this._owner.GetControl("DrawTexture_SkillIcon") as DrawTexture);
			this._skillName = this._componentSetter.SetSkillName(this._owner);
			this._skillInfoBtn = this._componentSetter.SetSkillInfoBtn(this._owner);
			this._statLabels = this._componentSetter.SetStatLabels(this._owner);
			this._costumeTouchArea = (this._owner.GetControl("DrawTexture_TouchArea") as DrawTexture);
			this._costumeCharView = (this._owner.GetControl("DrawTexture_CharView") as DrawTexture);
			this._mainBG = this._componentSetter.SetBG(this._owner);
			this._backBtn = this._componentSetter.SetBackButton(this._owner);
			this._btnWalk = this._componentSetter.SetActionWalkBtn(this._owner);
			this._btnStay = this._componentSetter.SetActionStayBtn(this._owner);
			this._btnAttack = this._componentSetter.SetActionAttackBtn(this._owner);
			this._btnImmediatelyBuy = this._componentSetter.SetImmediatelyBuyBtn(this._owner);
			this._lbCostumeName = this._componentSetter.SetCostumeName(this._owner);
			this._uiHideCheckBox = this._componentSetter.SetCostumeUIHideCheckBox(this._owner);
			this._skillTitle = (this._owner.GetControl("Label_SkillTitle") as Label);
			this._skillBG = (this._owner.GetControl("DrawTexture_titleline") as DrawTexture);
			this._skillLine = (this._owner.GetControl("DrawTexture_SkillSlot") as DrawTexture);
			this._lbImmediate = (this._owner.GetControl("Label_Label51") as Label);
			this._charSkillBGGradation = (this._owner.GetControl("DrawTexture_SkillTitleBG") as DrawTexture);
			this._charStatBGGradation = (this._owner.GetControl("DrawTexture_C_info_bg") as DrawTexture);
			this._sameKindSolCheckBox = this._componentSetter.SetSameKindSolCheckBox(this._owner);
			this._btMyCharListLeftArrow = this._componentSetter.SetMyCharListLeftBtn(this._owner);
			this._btMyCharListRightArrow = this._componentSetter.SetMyCharListRightBtn(this._owner);
			this._dtMyCharListLeftArrowBG = this._componentSetter.SetMyCharListLeftArrowBG(this._owner);
			this._dtMyCharListRightArrowBG = this._componentSetter.SetMyCharListRightArrowBG(this._owner);
			this._lbSoulGem = this._componentSetter.SetSolGemLabel(this._owner);
			this._lbMythElixir = this._componentSetter.SetMythElixirLabel(this._owner);
			this._btnSoulGemLink = this._componentSetter.SetSoulGemLinkButton(this._owner);
			this._btnSoulGemLink_2 = this._componentSetter.SetSoulGemLinkButton_2(this._owner);
			this._btnMythElixirLink = this._componentSetter.SetMythElixirButton(this._owner);
			this._btnMythElixirLink_2 = this._componentSetter.SetMythElixirButton_2(this._owner);
			this._bthHelp = this._componentSetter.SetHelpButton(this._owner);
		}
	}
}
