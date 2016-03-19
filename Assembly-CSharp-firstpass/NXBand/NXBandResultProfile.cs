using System;

namespace NXBand
{
	public class NXBandResultProfile : NXBandResult
	{
		public NXBandTypeMember member;

		public NXBandResultProfile()
		{
			this._resultType = BandResultType.Profile;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.ToString(),
				", member : <",
				this.member,
				">"
			});
		}
	}
}
