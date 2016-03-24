using System;
using TsLibs;

public class NrTable_BurnningEvent : NrTableBase
{
	public NrTable_BurnningEvent() : base(CDefinePath.s_strBurnningEventInfoURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BUNNING_EVENT_INFO bUNNING_EVENT_INFO = new BUNNING_EVENT_INFO();
			bUNNING_EVENT_INFO.SetData(data);
			bUNNING_EVENT_INFO.m_eEventType = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetBurnningEventCode(bUNNING_EVENT_INFO.strEventType);
			NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.Set_Value(bUNNING_EVENT_INFO.m_eEventType, bUNNING_EVENT_INFO);
		}
		return true;
	}
}
