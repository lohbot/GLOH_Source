using System;

namespace NXBand
{
	public class NXBandResultWrite : NXBandResult
	{
		public NXBandTypeQuota quota;

		public NXBandResultWrite()
		{
			this._resultType = BandResultType.Write;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.ToString(),
				", quota : <",
				this.quota,
				">"
			});
		}
	}
}
