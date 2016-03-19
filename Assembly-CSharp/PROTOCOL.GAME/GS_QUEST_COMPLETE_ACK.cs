using GAME;
using System;
using System.Runtime.InteropServices;

namespace PROTOCOL.GAME
{
	public class GS_QUEST_COMPLETE_ACK
	{
		public int Result;

		public byte bGm;

		public short i16CharUnique;

		public char[] strQuestUnique = new char[33];

		public int i32GroupUnique;

		public int i32Grade;

		public int i32CurGrade;

		public byte bCleared;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
		public byte[] byCompleteQuest = new byte[25];

		public long i64CurrentMoney;

		public int i32RewardType;

		public ITEM[] item = new ITEM[3];

		public SOLDIER_INFO kSolinfo = new SOLDIER_INFO();

		public long i64GeneralID_Enhance;

		public byte ui8EnhanceRank;

		public long[] i64rItemID_Remove = new long[3];

		public int[] nItemUnique_Remove = new int[3];

		public int[] i32rPosType_Remove = new int[3];

		public int[] i32rItemPos_Remove = new int[3];

		public int[] i32rItemNum_Remove = new int[3];

		public byte m_ui8IsRemoveSubCharacter;

		public long[] i64rGeneralID = new long[5];

		public long[] i64rGeneralExp = new long[5];

		public int[] i32rGeneralLevel = new int[5];

		public int i32ActionID;

		public int i32Flag;

		public GS_QUEST_COMPLETE_ACK()
		{
			this.item[0] = new ITEM();
			this.item[1] = new ITEM();
			this.item[2] = new ITEM();
		}
	}
}
