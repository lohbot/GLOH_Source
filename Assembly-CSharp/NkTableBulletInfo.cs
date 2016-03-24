using System;
using TsLibs;

public class NkTableBulletInfo : NrTableBase
{
	public NkTableBulletInfo() : base(CDefinePath.BULLETINFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BULLET_INFO bULLET_INFO = new BULLET_INFO();
			bULLET_INFO.SetData(data);
			bULLET_INFO.MOVE_TYPE = NrTSingleton<NkBulletManager>.Instance.ConvertBulletMoveType(bULLET_INFO.szMOVE_TYPE);
			bULLET_INFO.HIT_TYPE = NrTSingleton<NkBulletManager>.Instance.ConvertBulletHitType(bULLET_INFO.szHIT_TYPE);
			NrTSingleton<NkBulletManager>.Instance.AddBulletInfo(bULLET_INFO);
		}
		return true;
	}
}
