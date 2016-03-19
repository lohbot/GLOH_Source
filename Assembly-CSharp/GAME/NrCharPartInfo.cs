using System;

namespace GAME
{
	public class NrCharPartInfo
	{
		public NrCharBasePart m_kBasePart;

		public NrCharEquipPart m_kEquipPart;

		public NrCharPartInfo()
		{
			this.m_kBasePart = new NrCharBasePart();
			this.m_kEquipPart = new NrCharEquipPart();
			this.Init();
		}

		public void Init()
		{
			this.m_kBasePart.Init();
			this.m_kEquipPart.Init();
		}

		public void Set(NrCharPartInfo pkInfo)
		{
			this.m_kBasePart.SetData(pkInfo.m_kBasePart);
			this.m_kEquipPart.SetData(pkInfo.m_kEquipPart);
		}
	}
}
