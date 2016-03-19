using System;

namespace NXBand
{
	public class NXBandResultUserKey : NXBandResult
	{
		public string key;

		public NXBandResultUserKey()
		{
			this._resultType = BandResultType.UserKey;
		}

		public override string ToString()
		{
			return base.ToString() + ", userKey : <" + this.key + ">";
		}
	}
}
