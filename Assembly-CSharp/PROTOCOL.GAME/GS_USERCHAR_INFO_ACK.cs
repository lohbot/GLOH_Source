using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_USERCHAR_INFO_ACK
	{
		public short CharUnique;

		public int WorldID;

		public CHAR_SHAPE_INFO kShapeInfo = new CHAR_SHAPE_INFO();

		public long GuildID;

		public char[] szCharGuildName = new char[11];

		public bool bCharGuildPortrait;

		public short iColosseumGrade;

		public bool bGuildWar;
	}
}
