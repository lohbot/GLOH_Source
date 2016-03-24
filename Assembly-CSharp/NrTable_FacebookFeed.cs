using System;
using TsLibs;

public class NrTable_FacebookFeed : NrTableBase
{
	public NrTable_FacebookFeed() : base(CDefinePath.FACEBOOK_FEED_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			FacebookFeedData facebookFeedData = new FacebookFeedData();
			facebookFeedData.SetData(data);
			NrTSingleton<Facebook_Feed_Manager>.Instance.Set_Value(facebookFeedData);
		}
		return true;
	}
}
