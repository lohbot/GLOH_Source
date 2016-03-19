using System;

namespace PROTOCOL
{
	public class EXPEDITION_SEARCH_INFO
	{
		public byte ui8ExpeditionGrade;

		public short i16xpeditionCreateDataID;

		public int i32ExpeditionNum;

		public short i16MonLevel;

		public int i32MonPlunderItemNum;

		public void Init()
		{
			this.ui8ExpeditionGrade = 0;
			this.i16xpeditionCreateDataID = 0;
			this.i32ExpeditionNum = 0;
			this.i16MonLevel = 0;
		}

		public void Set(byte _ui8ExpeditionGrade, short _i16ExpeditionCreateDataID, int _i32ExpeditionNum, short _i16MonLevel, int _i32MonPlunderItemNum)
		{
			this.ui8ExpeditionGrade = _ui8ExpeditionGrade;
			this.i16xpeditionCreateDataID = _i16ExpeditionCreateDataID;
			this.i32ExpeditionNum = _i32ExpeditionNum;
			this.i16MonLevel = _i16MonLevel;
			this.i32MonPlunderItemNum = _i32MonPlunderItemNum;
		}
	}
}
