using System;

namespace PROTOCOL.GAME
{
	public class GS_CHAR_CHALLENGE_INVITESNG_REQ
	{
		public enum eSNGTYPE
		{
			eSNG_KAKAOTALK,
			eSNG_FACEBOOK
		}

		public byte invite_sngtype;
	}
}
