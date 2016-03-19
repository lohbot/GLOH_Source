using System;

namespace NXBand
{
	public class NXBandResultCacheInfo : NXBandResult
	{
		public NXBandTypeCache cacheInfo;

		public NXBandResultCacheInfo()
		{
			this._resultType = BandResultType.CacheInfo;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.ToString(),
				", cache info : <",
				this.cacheInfo,
				">"
			});
		}
	}
}
