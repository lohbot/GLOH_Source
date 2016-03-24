using System;
using UnityForms;

namespace CostumeRoomDlg
{
	public class ComponentSetter
	{
		public COSTUME_STAT_LABEL[] SetStatLabels(CostumeRoom_Dlg owner)
		{
			COSTUME_STAT_LABEL[] array = new COSTUME_STAT_LABEL[3];
			for (int i = 1; i <= 3; i++)
			{
				if (array.Length < i)
				{
					break;
				}
				array[i - 1] = new COSTUME_STAT_LABEL();
				array[i - 1]._costumeStatLabel = (owner.GetControl("Label_Stat0" + i.ToString()) as Label);
				array[i - 1]._postStatLabel = (owner.GetControl("Label_Stat0" + i.ToString() + "_After") as Label);
				array[i - 1]._costumeStatPercent = (owner.GetControl("Label_Stat0" + i.ToString() + "_P") as Label);
				array[i - 1]._costumeNormalStat = (owner.GetControl("Label_Stat0" + i.ToString() + "_AfterBasic") as Label);
			}
			return array;
		}

		public Button SetBackButton(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Button_Back") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner.OnClickBackBtn));
			return button;
		}

		public DrawTexture SetBG(CostumeRoom_Dlg owner)
		{
			DrawTexture drawTexture = owner.GetControl("DT_MainBG") as DrawTexture;
			drawTexture.SetTextureFromBundle("ui/soldier/BG_costume");
			drawTexture.AddMouseDownDelegate(new EZValueChangedDelegate(owner._costumeViewerSetter._costumeCharSetter.CostumeViewOnPress));
			return drawTexture;
		}

		public Button SetSkillInfoBtn(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Button_SkillInfo") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner._costumeViewerSetter._costumeSkillSetter.OnClickCostumeSkill));
			return button;
		}

		public Label SetSkillName(CostumeRoom_Dlg owner)
		{
			Label label = owner.GetControl("Label_SkillName") as Label;
			label.Text = string.Empty;
			return label;
		}

		public NewListBox SetCostumeListBox(CostumeRoom_Dlg owner)
		{
			NewListBox newListBox = owner.GetControl("NewListBox_CostumeList") as NewListBox;
			newListBox.Reserve = false;
			newListBox.Clear();
			newListBox.AddValueChangedDelegate(new EZValueChangedDelegate(owner._costumeListSetter.OnCostumeListClick));
			return newListBox;
		}

		public NewListBox SetMySolListBox(CostumeRoom_Dlg owner)
		{
			NewListBox newListBox = owner.GetControl("NewListBox_MySolList") as NewListBox;
			newListBox.Reserve = false;
			return newListBox;
		}

		public Button SetActionWalkBtn(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Button_Action_Walk") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner._costumeViewerSetter._costumeCharSetter.OnClickActionWalk));
			return button;
		}

		public Button SetActionStayBtn(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Button_Action_Stay") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner._costumeViewerSetter._costumeCharSetter.OnClickActionStay));
			return button;
		}

		public Button SetActionAttackBtn(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Button_Action_Attack") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner._costumeViewerSetter._costumeCharSetter.OnClickActionAttack));
			return button;
		}

		public Button SetImmediatelyBuyBtn(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Button_Buy") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner.OnClickImmediatelyBuy));
			return button;
		}

		public Label SetCostumeName(CostumeRoom_Dlg owner)
		{
			Label label = owner.GetControl("Label_costumename") as Label;
			label.Text = string.Empty;
			return label;
		}

		public CheckBox SetCostumeUIHideCheckBox(CostumeRoom_Dlg owner)
		{
			CheckBox checkBox = owner.GetControl("CheckBox_UIHide") as CheckBox;
			checkBox.SetValueChangedDelegate(new EZValueChangedDelegate(owner.OnChangeUIHideCheckBox));
			return checkBox;
		}

		public CheckBox SetSameKindSolCheckBox(CostumeRoom_Dlg owner)
		{
			CheckBox checkBox = owner.GetControl("CheckBox_samehero") as CheckBox;
			checkBox.SetCheckState(1);
			checkBox.SetValueChangedDelegate(new EZValueChangedDelegate(owner._myCharListSetter._callbackProcessor.OnClickSameHeroViewCheckBox));
			return checkBox;
		}

		public Button SetMyCharListLeftBtn(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Button_LeftArrow") as Button;
			button.SetLocation(button.GetLocationX(), button.GetLocationY(), button.GetLocation().z - 3f);
			return button;
		}

		public Button SetMyCharListRightBtn(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Button_RightArrow") as Button;
			button.SetLocation(button.GetLocationX(), button.GetLocationY(), button.GetLocation().z - 3f);
			return button;
		}

		public DrawTexture SetMyCharListLeftArrowBG(CostumeRoom_Dlg owner)
		{
			DrawTexture drawTexture = owner.GetControl("arrow_BG01") as DrawTexture;
			drawTexture.SetLocation(drawTexture.GetLocationX(), drawTexture.GetLocationY(), drawTexture.GetLocation().z - 3f);
			return drawTexture;
		}

		public DrawTexture SetMyCharListRightArrowBG(CostumeRoom_Dlg owner)
		{
			DrawTexture drawTexture = owner.GetControl("arrow_BG02") as DrawTexture;
			drawTexture.SetLocation(drawTexture.GetLocationX(), drawTexture.GetLocationY(), drawTexture.GetLocation().z - 3f);
			return drawTexture;
		}

		public Label SetSolGemLabel(CostumeRoom_Dlg owner)
		{
			Label label = owner.GetControl("LB_SoulGem") as Label;
			label.Text = string.Empty;
			return label;
		}

		public Label SetMythElixirLabel(CostumeRoom_Dlg owner)
		{
			Label label = owner.GetControl("LB_MythElixir") as Label;
			label.Text = string.Empty;
			return label;
		}

		public Button SetSoulGemLinkButton(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Btn_SoulGem_Shop") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner.OnClickCostumeMaterialLink));
			return button;
		}

		public Button SetSoulGemLinkButton_2(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Btn_SoulGem_Shop2") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner.OnClickCostumeMaterialLink));
			return button;
		}

		public Button SetMythElixirButton(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Btn_MythElixir_Shop") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner.OnClickCostumeMaterialLink));
			return button;
		}

		public Button SetMythElixirButton_2(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Btn_MythElixir_Shop2") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner.OnClickCostumeMaterialLink));
			return button;
		}

		public Button SetHelpButton(CostumeRoom_Dlg owner)
		{
			Button button = owner.GetControl("Help_Button") as Button;
			button.AddValueChangedDelegate(new EZValueChangedDelegate(owner.OnClickHelpBtn));
			return button;
		}
	}
}
