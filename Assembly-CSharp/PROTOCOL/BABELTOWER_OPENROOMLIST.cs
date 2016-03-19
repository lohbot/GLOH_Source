using System;

namespace PROTOCOL
{
	public class BABELTOWER_OPENROOMLIST
	{
		public int i32RoomIndex;

		public long i64LeaderPersonID;

		public char[] szLeaderName = new char[21];

		public short i16LeaderLevel;

		public short i16Floor;

		public short i16SubFloor;

		public byte byMaxUserNum;

		public byte byCurUserNum;

		public short MinLevel;

		public short MaxLevel;

		public short i16FloorType;
	}
}
