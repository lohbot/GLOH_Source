using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_CHAT_ACK
	{
		public byte ChatType;

		public short ColoseumGrade;

		public int RoomUnique;

		public long SenderPersonID;

		public char[] SenderName = new char[21];

		public char[] Msg = new char[100];

		public short Color;

		public ITEM LinkItem = new ITEM();

		public long i64GuildID;
	}
}
