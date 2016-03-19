using System;

namespace GAME
{
	public class NrCharEquipPart
	{
		public int[] m_nPartUnit;

		public NrCharEquipPart()
		{
			this.m_nPartUnit = new int[6];
			this.Init();
		}

		public void Init()
		{
			for (int i = 0; i < 6; i++)
			{
				this.m_nPartUnit[i] = 0;
			}
		}

		public void SetData(NrCharEquipPart pclinfo)
		{
			for (int i = 0; i < 6; i++)
			{
				this.m_nPartUnit[i] = pclinfo.m_nPartUnit[i];
			}
		}

		public void SetData(NrEquipItemInfo kEquipInfo)
		{
			this.m_nPartUnit[0] = kEquipInfo.GetItemUnique(0);
			this.m_nPartUnit[1] = kEquipInfo.GetItemUnique(1);
			this.m_nPartUnit[2] = kEquipInfo.GetItemUnique(2);
			this.m_nPartUnit[3] = kEquipInfo.GetItemUnique(3);
			this.m_nPartUnit[4] = kEquipInfo.GetItemUnique(4);
			this.m_nPartUnit[5] = kEquipInfo.GetItemUnique(5);
		}

		public void SetData(eAT2CharEquipPart ePartID, int _value)
		{
			this.m_nPartUnit[(int)ePartID] = _value;
		}

		public int GetData(eAT2CharEquipPart ePartID)
		{
			return this.m_nPartUnit[(int)ePartID];
		}
	}
}
