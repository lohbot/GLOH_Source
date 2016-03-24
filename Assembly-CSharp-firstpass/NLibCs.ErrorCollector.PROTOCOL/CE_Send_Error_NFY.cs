using NLibCs.Net.Serialization;
using System;

namespace NLibCs.ErrorCollector.PROTOCOL
{
	public struct CE_Send_Error_NFY
	{
		public long userSN;

		public int platformType;

		public int authType;

		[Length(17)]
		public string bundleVerson;

		[Length(129)]
		public string message;

		[Length(2049)]
		public string stackTrace;

		[Length(65)]
		public string deviceModel;

		[Length(65)]
		public string deviceOS;
	}
}
