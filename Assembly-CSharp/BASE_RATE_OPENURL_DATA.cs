using System;
using System.Collections.Generic;
using TsLibs;

public class BASE_RATE_OPENURL_DATA : NrTableBase
{
	public static List<ITEM_RATE_OPENURL_DATA> m_listItemRateOpenUrl = new List<ITEM_RATE_OPENURL_DATA>();

	public BASE_RATE_OPENURL_DATA() : base(CDefinePath.ITEM_RATE_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ITEM_RATE_OPENURL_DATA iTEM_RATE_OPENURL_DATA = new ITEM_RATE_OPENURL_DATA();
			iTEM_RATE_OPENURL_DATA.SetData(data);
			BASE_RATE_OPENURL_DATA.m_listItemRateOpenUrl.Add(iTEM_RATE_OPENURL_DATA);
		}
		return true;
	}

	public static ITEM_RATE_OPENURL_DATA GetItemRateOpenUrl()
	{
		NkServiceAreaInfo currentServiceAreaInfo = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceAreaInfo();
		string b = currentServiceAreaInfo.eServiceArea.ToString();
		foreach (ITEM_RATE_OPENURL_DATA current in BASE_RATE_OPENURL_DATA.m_listItemRateOpenUrl)
		{
			if (current.strService_Code == b)
			{
				return current;
			}
		}
		return null;
	}
}
