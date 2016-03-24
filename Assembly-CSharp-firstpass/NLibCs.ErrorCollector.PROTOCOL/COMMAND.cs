using System;

namespace NLibCs.ErrorCollector.PROTOCOL
{
	public enum COMMAND : ushort
	{
		BEGIN = 7200,
		CE_Send_Error_NFY,
		CE_OFF_NFY,
		Heartbeat_NFY = 9999,
		PROTOCOL_END,
		REAL_END
	}
}
