using GAME;
using PROTOCOL.GAME;
using System;

namespace PROTOCOL
{
	public class BATTLE_SOLDIER_INFO
	{
		public short CharUnique;

		public short BUID;

		public byte Ally;

		public char[] CharName = new char[21];

		public byte SolIndex;

		public long SolID;

		public int CharKind;

		public byte CharKindType;

		public byte Grade;

		public short Level;

		public long Exp;

		public int BattleCharATB;

		public POS3D CharPos = new POS3D();

		public POS3D GridStartPos = new POS3D();

		public float Speed;

		public int HP;

		public int HP_Max;

		public byte nGridID;

		public byte nStartPosIndex;

		public byte nGridPos;

		public int nGridRotate;

		public byte CharType;

		public byte bFriend;

		public NrCharPartInfo kPartInfo = new NrCharPartInfo();

		public BATTLESKILL_DATA[] BattleSkillData = new BATTLESKILL_DATA[6];

		public BATTLE_SOLDIER_INFO()
		{
			for (int i = 0; i < 6; i++)
			{
				this.BattleSkillData[i] = new BATTLESKILL_DATA();
			}
		}
	}
}
