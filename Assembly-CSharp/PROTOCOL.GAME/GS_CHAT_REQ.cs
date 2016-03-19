using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_CHAT_REQ
	{
		public byte ChatType;

		public short ColoseumGrade;

		public int RoomUnique;

		public long nBabelLeaderPersonID;

		public char[] szChatStr = new char[100];

		public short Color;

		public ITEM LinkItem;

		public GS_CHAT_REQ()
		{
			this.LinkItem = new ITEM();
		}
	}
}
