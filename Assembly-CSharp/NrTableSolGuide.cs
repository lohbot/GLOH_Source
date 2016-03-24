using System;
using TsLibs;

public class NrTableSolGuide : NrTableBase
{
	public NrTableSolGuide() : base(CDefinePath.CHARKIND_SOLGUIDE_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			SOL_GUIDE sOL_GUIDE = new SOL_GUIDE();
			sOL_GUIDE.SetData(data);
			sOL_GUIDE.m_i32CharKind = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(sOL_GUIDE.m_strCharCode);
			NrTSingleton<NrTableSolGuideManager>.Instance.AddSolGuide(sOL_GUIDE);
			NrTSingleton<NrBaseTableManager>.Instance.SetData(sOL_GUIDE);
		}
		return true;
	}
}
