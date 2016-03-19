using System;

namespace NXBand
{
	public class NXBandResultQuota : NXBandResult
	{
		public NXBandTypeQuota quotaMessage;

		public NXBandTypeQuota quotaInvite;

		public NXBandTypeQuota quotaPost;

		public NXBandResultQuota()
		{
			this._resultType = BandResultType.Quota;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.ToString(),
				", message : <",
				this.quotaMessage,
				">, invite : <",
				this.quotaInvite,
				">, post : <",
				this.quotaPost,
				">"
			});
		}
	}
}
