using System;

namespace NXBand
{
	public class NXBandResultInviter : NXBandResult
	{
		public NXBandTypeInviter inviter;

		public NXBandResultInviter()
		{
			this._resultType = BandResultType.Inviter;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.ToString(),
				", inviter : <",
				this.inviter,
				">"
			});
		}
	}
}
