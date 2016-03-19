using System;

namespace PROTOCOL
{
	public class GS_STORYCOMMENT_SET_REQ
	{
		public long m_nStoryCommentID;

		public long nStoryChatID;

		public char[] szMessage = new char[51];
	}
}
