using System;
using TsLibs;

public class NrTable_ITEM_COMPOSE : NrTableBase
{
	public NrTable_ITEM_COMPOSE() : base(CDefinePath.s_strItemComposeURL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ITEM_COMPOSE iTEM_COMPOSE = new ITEM_COMPOSE();
			iTEM_COMPOSE.SetData(data);
			NrTSingleton<ITEM_COMPOSE_Manager>.Instance.Set_Value(iTEM_COMPOSE);
		}
		return true;
	}
}
