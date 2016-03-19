using System;

namespace PROTOCOL
{
	public class GS_COMMUNITY_MESSAGE_BATTLE_RESULT
	{
		public enum BATTELE_RESULT
		{
			eVICTORY,
			eDEFEAT
		}

		public enum BATTLE_TYPE
		{
			eBATTLE_NORMAL,
			eBATTLE_PLUNDER,
			eBATTLE_COLOSSEUM,
			eBATTLE_BABEL
		}

		public byte i8Result;

		public byte i8BattleType;

		public int i32Param1;

		public int i32Param2;

		public int i32Param3;
	}
}
