using System;
using TsLibs;

public class NrTable_ITEMSKILLREINFORCE : NrTableBase
{
	public NrTable_ITEMSKILLREINFORCE() : base(CDefinePath.s_strItemSkillReinforceURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ITEMSKILLREINFORCE iTEMSKILLREINFORCE = new ITEMSKILLREINFORCE();
			iTEMSKILLREINFORCE.SetData(data);
			NrTSingleton<ITEMSKILLREINFORCE_Manager>.Instance.SetItemSkillReinforceData(iTEMSKILLREINFORCE);
		}
		return true;
	}
}
