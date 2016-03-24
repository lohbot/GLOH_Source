using System;
using UnityForms;

namespace CostumeDlg
{
	public class ComponentSetter
	{
		public DropDownList SetDDLSeason(CostumeGuide_Dlg owner, EZValueChangedDelegate changedDelegate)
		{
			DropDownList dropDownList = owner.GetControl("DDL_Season") as DropDownList;
			dropDownList.Reserve = false;
			dropDownList.AddValueChangedDelegate(changedDelegate);
			return dropDownList;
		}

		public DropDownList SetDDLOrder(CostumeGuide_Dlg owner, EZValueChangedDelegate changedDelegate)
		{
			DropDownList dropDownList = owner.GetControl("DDL_Setorder") as DropDownList;
			dropDownList.Reserve = false;
			dropDownList.AddValueChangedDelegate(changedDelegate);
			return dropDownList;
		}

		public COSTUMEGUIDE_SLOT[] SetSlotListComponent(CostumeGuide_Dlg owner, int maxSlotCount, EZValueChangedDelegate selectSlotDelegate)
		{
			COSTUMEGUIDE_SLOT[] array = new COSTUMEGUIDE_SLOT[maxSlotCount];
			for (int i = 0; i < maxSlotCount; i++)
			{
				array[i] = new COSTUMEGUIDE_SLOT();
				array[i].DT_Slot = (owner.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("DT_SlotBg", (i + 1).ToString())) as DrawTexture);
				array[i].IT_Slot = (owner.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("IT_SOL", (i + 1).ToString())) as ItemTexture);
				array[i].BT_Slot = (owner.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("BT_Slot", (i + 1).ToString())) as Button);
				array[i].DT_Event = (owner.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("DT_Event", (i + 1).ToString())) as DrawTexture);
				array[i].DT_BlackSlot = (owner.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("DT_DarkIcon", (i + 1).ToString())) as DrawTexture);
				if (i < 9)
				{
					array[i].Box_New = (owner.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("BOX_NEW0", (i + 1).ToString())) as Box);
				}
				else
				{
					array[i].Box_New = (owner.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("BOX_NEW", (i + 1).ToString())) as Box);
				}
				array[i].Box_New.DeleteSpriteText();
				array[i].BT_Slot.AddValueChangedDelegate(selectSlotDelegate);
				array[i].BT_Slot.data = i;
				array[i].BT_Slot.DeleteSpriteText();
			}
			return array;
		}

		public Button SetLeftPageButton(CostumeGuide_Dlg owner, EZValueChangedDelegate callback)
		{
			if (owner == null)
			{
				return null;
			}
			Button button = owner.GetControl("Bt_LeftArr") as Button;
			if (button == null)
			{
				return null;
			}
			button.AddValueChangedDelegate(callback);
			return button;
		}

		public Button SetRightPageButton(CostumeGuide_Dlg owner, EZValueChangedDelegate callback)
		{
			if (owner == null)
			{
				return null;
			}
			Button button = owner.GetControl("BT_RightArr") as Button;
			if (button == null)
			{
				return null;
			}
			button.AddValueChangedDelegate(callback);
			return button;
		}
	}
}
