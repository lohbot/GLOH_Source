using GAME;
using System;
using TsLibs;

public class NrTableItemSkillInfo : NrTableBase
{
	public NrTableItemSkillInfo() : base(CDefinePath.ITEMSKILL_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ITEMSKILL_INFO iTEMSKILL_INFO = new ITEMSKILL_INFO();
			iTEMSKILL_INFO.SetData(data);
			iTEMSKILL_INFO.m_eItemType = (eITEM_TYPE)NrTSingleton<ItemManager>.Instance.GetItemType(iTEMSKILL_INFO.m_strItemType);
			NrTSingleton<NrItemSkillInfoManager>.Instance.Set_Value(iTEMSKILL_INFO);
		}
		return true;
	}
}
