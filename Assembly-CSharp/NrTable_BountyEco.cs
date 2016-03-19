using System;
using TsLibs;

public class NrTable_BountyEco : NrTableBase
{
	public NrTable_BountyEco() : base(CDefinePath.BOUNTYECO_URL, true)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			BountyEcoData bountyEcoData = new BountyEcoData();
			bountyEcoData.SetData(data);
			for (int i = 0; i < 11; i++)
			{
				bountyEcoData.i32CharKind[i] = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(bountyEcoData.strCharCode[i]);
			}
			NrTSingleton<BountyHuntManager>.Instance.AddBountyEcoData(bountyEcoData);
		}
		return true;
	}
}
