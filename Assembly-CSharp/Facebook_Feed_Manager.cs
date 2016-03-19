using System;
using System.Collections.Generic;

public class Facebook_Feed_Manager : NrTSingleton<Facebook_Feed_Manager>
{
	private SortedDictionary<string, FacebookFeedData> m_sdCollection = new SortedDictionary<string, FacebookFeedData>();

	private Facebook_Feed_Manager()
	{
	}

	public FacebookFeedData Get_FeedData(eFACEBOOK_FEED_TYPE FeedType)
	{
		string key = FeedType.ToString().ToLower();
		if (this.m_sdCollection.ContainsKey(key))
		{
			return this.m_sdCollection[key];
		}
		return null;
	}

	public void Set_Value(FacebookFeedData a_cValue)
	{
		if (a_cValue != null)
		{
			string key = a_cValue.Feed_Code.ToLower();
			this.m_sdCollection.Add(key, a_cValue);
		}
	}
}
