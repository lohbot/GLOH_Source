using System;
using TsLibs;

public class NrTable_PlunderObjectInfo : NrTableBase
{
	public NrTable_PlunderObjectInfo() : base(CDefinePath.s_strPlunderObjectInfoURL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			PLUNDER_OBJECT_INFO pLUNDER_OBJECT_INFO = new PLUNDER_OBJECT_INFO();
			pLUNDER_OBJECT_INFO.SetData(data);
			NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(pLUNDER_OBJECT_INFO.szObjectKindCode);
			if (charKindInfoFromCode != null)
			{
				pLUNDER_OBJECT_INFO.nObject_Kind = charKindInfoFromCode.GetCharKind();
			}
			NrTSingleton<NrTable_PlunderObjectinfo_Manager>.Instance.Set_Value(pLUNDER_OBJECT_INFO);
		}
		return true;
	}
}
