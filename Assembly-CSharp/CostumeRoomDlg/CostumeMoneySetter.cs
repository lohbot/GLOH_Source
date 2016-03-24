using GAME;
using System;

namespace CostumeRoomDlg
{
	public class CostumeMoneySetter
	{
		private CostumeRoom_Dlg _owner;

		public CostumeMoneySetter(CostumeRoom_Dlg owner)
		{
			this._owner = owner;
		}

		public void Refresh()
		{
			this.InitMoneyNum();
		}

		private void InitMoneyNum()
		{
			int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_COSTUME_PRICE_ITEM_UNIQUE_1);
			int value2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_COSTUME_PRICE_ITEM_UNIQUE_2);
			ITEM item = NkUserInventory.instance.GetItem(value);
			if (item != null)
			{
				this._owner._variables._lbSoulGem.Text = string.Format("{0:##,##0}", item.m_nItemNum);
			}
			else
			{
				this._owner._variables._lbSoulGem.Text = "0";
			}
			ITEM item2 = NkUserInventory.instance.GetItem(value2);
			if (item2 != null)
			{
				this._owner._variables._lbMythElixir.Text = string.Format("{0:##,##0}", item2.m_nItemNum);
			}
			else
			{
				this._owner._variables._lbMythElixir.Text = "0";
			}
		}
	}
}
