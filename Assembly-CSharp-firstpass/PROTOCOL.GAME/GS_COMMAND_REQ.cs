using System;

namespace PROTOCOL.GAME
{
	public class GS_COMMAND_REQ
	{
		public long TID;

		public byte eCommandType;

		public long BuildingID;

		public int BuildingUnique;

		public short PosX;

		public short PosY;

		public byte Direction;

		public long Data;

		public long Data1;
	}
}
