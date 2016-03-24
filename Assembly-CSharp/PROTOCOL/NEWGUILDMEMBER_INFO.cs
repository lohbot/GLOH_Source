using System;

namespace PROTOCOL
{
	public class NEWGUILDMEMBER_INFO
	{
		public long i64PersonID;

		public long i64GuildID;

		public short i16Level;

		public byte ui8Rank;

		public long i64JoinDate;

		public long i64LoginDate;

		public short i16ChannelID;

		public int i32MapUnique;

		public int i32ConnectedWorldID;

		public int i32FaceCharKind;

		public char[] strCharName = new char[11];

		public int i32Contribute;

		public long i64LogoffTime;

		public long i64SN;

		public byte bIsGuildWarReward;

		public bool bConnected;

		public int i32CostumeUnique;

		public byte i8GuildPush;
	}
}
