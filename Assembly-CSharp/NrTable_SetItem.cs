using System;
using TsLibs;

public class NrTable_SetItem : NrTableBase
{
	public NrTable_SetItem() : base(CDefinePath.SET_ITEM_DATA_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			SETITEM_DATA sETITEM_DATA = new SETITEM_DATA();
			sETITEM_DATA.SetData(data);
			for (int i = 0; i < 6; i++)
			{
				sETITEM_DATA.m_nSetEffectCode[i] = NrTSingleton<NrSetItemDataManager>.Instance.GetOptionType(sETITEM_DATA.m_strSetEffectCode[i]);
			}
			NrTSingleton<NrSetItemDataManager>.Instance.SetData(sETITEM_DATA);
		}
		return true;
	}
}
