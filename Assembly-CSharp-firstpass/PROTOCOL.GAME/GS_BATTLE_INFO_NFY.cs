using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_INFO_NFY
	{
		public int BFRoomID;

		public short MAP_ID;

		public sbyte Observer;

		public sbyte i8BattleRoomType;

		public int i32ScenarioUnique;

		public byte i8CurrentTurnAlly;

		public bool bShowStartTurnAllyEffect;

		public int[] nPreLoadingKind = new int[15];

		public int i32BattlePlayRate;

		public int i32BSkillComboLevel;
	}
}
