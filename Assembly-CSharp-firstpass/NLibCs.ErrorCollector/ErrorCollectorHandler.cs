using NLibCs.ErrorCollector.PROTOCOL;
using System;

namespace NLibCs.ErrorCollector
{
	public class ErrorCollectorHandler
	{
		public virtual void Occur_PacketError(PACKETERRORTYPE type)
		{
		}

		public virtual void Occur_SettingError(string msg)
		{
		}
	}
}
