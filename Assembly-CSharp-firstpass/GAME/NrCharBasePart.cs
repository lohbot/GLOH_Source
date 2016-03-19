using System;

namespace GAME
{
	public class NrCharBasePart
	{
		public int[] m_nPartUnit;

		public NrCharBasePart()
		{
			this.m_nPartUnit = new int[2];
			this.Init();
		}

		public void Init()
		{
			this.SetData(eAT2CharBasePart.CHARBASEPART_HAIR, 0);
			this.SetData(eAT2CharBasePart.CHARBASEPART_FACE, 0);
		}

		public void SetData(NrCharBasePart pclinfo)
		{
			for (int i = 0; i < 2; i++)
			{
				this.m_nPartUnit[i] = pclinfo.m_nPartUnit[i];
			}
		}

		public void SetData(eAT2CharBasePart ePartID, int _value)
		{
			this.m_nPartUnit[(int)ePartID] = _value;
		}

		public int GetData(eAT2CharBasePart ePartID)
		{
			return this.m_nPartUnit[(int)ePartID];
		}
	}
}
