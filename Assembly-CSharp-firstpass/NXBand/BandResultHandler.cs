using System;

namespace NXBand
{
	public interface BandResultHandler
	{
		void onResult(RequestCode reqCode, int resultCode, NXBandResult result);
	}
}
