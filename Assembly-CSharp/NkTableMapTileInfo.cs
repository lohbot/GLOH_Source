using System;
using TsLibs;

public class NkTableMapTileInfo : NrTableBase
{
	private bool bWorldMap;

	public NkTableMapTileInfo(string strFileName, bool worldmap) : base(strFileName, true)
	{
		this.bWorldMap = worldmap;
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		if (this.bWorldMap)
		{
			NkWorldMapATB worldMapATB = NrTSingleton<MapManager>.Instance.GetWorldMapATB();
			if (worldMapATB != null)
			{
				return worldMapATB.ParseDataFromNDT(dr);
			}
		}
		return false;
	}
}
