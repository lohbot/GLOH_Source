using System;
using TsLibs;

public class NrTable_BountyInfo : NrTableBase
{
	public NrTable_BountyInfo() : base(CDefinePath.BOUNTYINFO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BountyInfoData bountyInfoData = new BountyInfoData();
			bountyInfoData.SetData(data);
			bountyInfoData.i32NPCCharKind = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(bountyInfoData.strNPCCharCode);
			NrTSingleton<BountyHuntManager>.Instance.AddBountyInfoData(bountyInfoData);
		}
		return true;
	}
}
