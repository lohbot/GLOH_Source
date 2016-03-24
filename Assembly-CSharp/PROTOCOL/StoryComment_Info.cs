using System;

namespace PROTOCOL
{
	public class StoryComment_Info
	{
		public long nStoryCommentID;

		public char[] szName = new char[21];

		public char[] szMessage = new char[51];

		public int nCharKind;

		public short nLevel;

		public long nTime;

		public long nPersonID;

		public int nFaceCharCostumeUnique;
	}
}
