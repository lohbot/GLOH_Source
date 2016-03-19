using System;

namespace PROTOCOL.GAME
{
	public class BABELTOWER_PERSON_PACKET
	{
		public long nPartyPersonID;

		public char[] szCharName = new char[21];

		public int nLevel;

		public bool bReady;

		public byte nSlotType;

		public int nCharKind;
	}
}
