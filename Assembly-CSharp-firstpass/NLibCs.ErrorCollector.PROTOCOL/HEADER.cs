using System;

namespace NLibCs.ErrorCollector.PROTOCOL
{
	public struct HEADER
	{
		public ushort Size;

		public byte CheckSum;

		public ushort Command;

		public HEADER(ushort size, ushort command)
		{
			this.Size = size;
			this.CheckSum = 0;
			this.Command = command;
		}
	}
}
