using System;

namespace GAME
{
	public class SOLDIER_INFO
	{
		public long SolID;

		public byte SolPosType;

		public byte SolPosIndex;

		public byte SolStatus;

		public byte MilitaryUnique;

		public short BattlePos = -1;

		public int CharKind;

		public byte Grade;

		public short Level;

		public long Exp;

		public int HP;

		public long nFriendPersonID;

		public int nInitiativeValue;

		public SOLDIER_INFO()
		{
			this.Init();
		}

		public void Init()
		{
			this.SolID = 0L;
			this.SolPosType = 0;
			this.SolPosIndex = 0;
			this.SolStatus = 0;
			this.MilitaryUnique = 0;
			this.BattlePos = -1;
			this.CharKind = 0;
			this.Grade = 0;
			this.Level = 0;
			this.Exp = 0L;
			this.HP = 0;
			this.nFriendPersonID = 0L;
			this.nInitiativeValue = 0;
		}

		public void Set(ref SOLDIER_INFO SolData)
		{
			this.SolID = SolData.SolID;
			this.SolPosType = SolData.SolPosType;
			this.SolPosIndex = SolData.SolPosIndex;
			this.SolStatus = SolData.SolStatus;
			this.MilitaryUnique = SolData.MilitaryUnique;
			this.BattlePos = SolData.BattlePos;
			this.CharKind = SolData.CharKind;
			this.Grade = SolData.Grade;
			this.Level = SolData.Level;
			this.Exp = SolData.Exp;
			this.HP = SolData.HP;
			this.nFriendPersonID = SolData.nFriendPersonID;
			this.nInitiativeValue = SolData.nInitiativeValue;
		}
	}
}
