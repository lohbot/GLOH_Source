using System;
using System.Collections.Generic;
using TsLibs;

public class BASE_EXPEDITION_CREATE_DATA : NrTableBase
{
	public static Dictionary<byte, List<EXPEDITION_CREATE_DATA>> m_dicExpeditionListData = new Dictionary<byte, List<EXPEDITION_CREATE_DATA>>();

	public static List<EXPEDITION_CREATE_DATA> m_listExpeditionListData = new List<EXPEDITION_CREATE_DATA>();

	public BASE_EXPEDITION_CREATE_DATA() : base(CDefinePath.ExpeditionCrateDataURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			EXPEDITION_CREATE_DATA eXPEDITION_CREATE_DATA = new EXPEDITION_CREATE_DATA();
			eXPEDITION_CREATE_DATA.SetData(data);
			BASE_EXPEDITION_CREATE_DATA.m_listExpeditionListData.Add(eXPEDITION_CREATE_DATA);
			if (!BASE_EXPEDITION_CREATE_DATA.m_dicExpeditionListData.ContainsKey(eXPEDITION_CREATE_DATA.EXPEDITION_GRADE))
			{
				List<EXPEDITION_CREATE_DATA> list = new List<EXPEDITION_CREATE_DATA>();
				list.Add(eXPEDITION_CREATE_DATA);
				BASE_EXPEDITION_CREATE_DATA.m_dicExpeditionListData.Add(eXPEDITION_CREATE_DATA.EXPEDITION_GRADE, list);
			}
			else
			{
				BASE_EXPEDITION_CREATE_DATA.m_dicExpeditionListData[eXPEDITION_CREATE_DATA.EXPEDITION_GRADE].Add(eXPEDITION_CREATE_DATA);
			}
		}
		return true;
	}

	public static EXPEDITION_CREATE_DATA GetExpeditionCreateDataFromID(byte Expedition_Grade, int Expedition_createid)
	{
		if (!BASE_EXPEDITION_CREATE_DATA.m_dicExpeditionListData.ContainsKey(Expedition_Grade))
		{
			return null;
		}
		for (int i = 0; i < BASE_EXPEDITION_CREATE_DATA.m_dicExpeditionListData[Expedition_Grade].Count; i++)
		{
			if ((int)BASE_EXPEDITION_CREATE_DATA.m_dicExpeditionListData[Expedition_Grade][i].EXPEDITION_CREATE_ID == Expedition_createid)
			{
				return BASE_EXPEDITION_CREATE_DATA.m_dicExpeditionListData[Expedition_Grade][i];
			}
		}
		return null;
	}

	public static EXPEDITION_CREATE_DATA GetExpedtionCreateData(short dataid)
	{
		foreach (EXPEDITION_CREATE_DATA current in BASE_EXPEDITION_CREATE_DATA.m_listExpeditionListData)
		{
			if (current.EXPEDITION_CREATE_ID == dataid)
			{
				return current;
			}
		}
		return null;
	}
}
