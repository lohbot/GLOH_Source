using System;

namespace PROTOCOL.GAME
{
	public class GS_BATTLE_RADIO_ALARM_ACK
	{
		public byte i8RadioAlarmKind;

		public char[] szRequestUserName = new char[21];
	}
}
