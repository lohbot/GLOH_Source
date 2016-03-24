using System;
using System.Collections.Generic;

public class ITEMEVOLUTION_Manager : NrTSingleton<ITEMEVOLUTION_Manager>
{
	private Dictionary<int, ITEMEVOLUTION> mHash_ItemEvolutionData;

	private ITEMEVOLUTION_Manager()
	{
		this.mHash_ItemEvolutionData = new Dictionary<int, ITEMEVOLUTION>();
	}

	public bool SetItemEvolutioneData(ITEMEVOLUTION ItemEvolutioneData)
	{
		if (this.mHash_ItemEvolutionData == null)
		{
			return false;
		}
		if (!this.mHash_ItemEvolutionData.ContainsKey(ItemEvolutioneData.nItemIndex))
		{
			this.mHash_ItemEvolutionData.Add(ItemEvolutioneData.nItemIndex, new ITEMEVOLUTION());
			this.mHash_ItemEvolutionData[ItemEvolutioneData.nItemIndex] = ItemEvolutioneData;
		}
		return true;
	}

	public ITEMEVOLUTION GetItemEvolutionData(int ItemIndex)
	{
		if (this.mHash_ItemEvolutionData.ContainsKey(ItemIndex) && this.mHash_ItemEvolutionData[ItemIndex] != null)
		{
			return this.mHash_ItemEvolutionData[ItemIndex];
		}
		return null;
	}
}
