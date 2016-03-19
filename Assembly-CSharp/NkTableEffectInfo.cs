using System;
using TsLibs;

public class NkTableEffectInfo : NrTableBase
{
	public NkTableEffectInfo() : base(CDefinePath.EFFECT_INFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			EFFECT_INFO eFFECT_INFO = new EFFECT_INFO();
			eFFECT_INFO.SetData(data);
			eFFECT_INFO.EFFECT_TYPE = NrTSingleton<NkEffectManager>.Instance.ConvertEffectType(eFFECT_INFO.strEffectType);
			eFFECT_INFO.EFFECT_POS = NrTSingleton<NkEffectManager>.Instance.ConvertEffectPos(eFFECT_INFO.strEffectPos);
			NrTSingleton<NkEffectManager>.Instance.AddEffectInfo(eFFECT_INFO);
		}
		return true;
	}
}
