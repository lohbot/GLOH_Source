using System;

namespace PROTOCOL.GAME
{
	public class MYTHRAID_PERSON_PACKET
	{
		public long nPartyPersonID;

		public char[] szCharName = new char[21];

		public int nLevel;

		public bool bReady;

		public byte nSlotType;

		public int nCharKind;

		public short i16GuardianAngel = -1;
	}
}
