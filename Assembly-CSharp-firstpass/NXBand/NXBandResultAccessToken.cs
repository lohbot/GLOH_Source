using System;

namespace NXBand
{
	public class NXBandResultAccessToken : NXBandResult
	{
		public string accessToken;

		public NXBandResultAccessToken()
		{
			this._resultType = BandResultType.AccessToken;
		}

		public override string ToString()
		{
			return base.ToString() + ", accessToken : <" + this.accessToken + ">";
		}
	}
}
