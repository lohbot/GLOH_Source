using System;

namespace PROTOCOL
{
	public class StoryChat_Info
	{
		public long nStoryChatID;

		public char[] szName = new char[21];

		public char[] szMessage = new char[201];

		public int nCharKind;

		public short nLevel;

		public long nTime;

		public short nLikeCount;

		public short nCommentCount;

		public long nLastCommentID;

		public long nGuildID;

		public long nPersonID;

		public void Init()
		{
			this.nStoryChatID = 0L;
			this.szName = null;
			this.szMessage = null;
			this.nCharKind = 0;
			this.nLevel = 0;
			this.nTime = 0L;
			this.nLikeCount = 0;
			this.nCommentCount = 0;
			this.nLastCommentID = 0L;
			this.nGuildID = 0L;
			this.nPersonID = 0L;
		}
	}
}
