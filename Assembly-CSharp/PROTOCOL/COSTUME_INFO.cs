using System;

namespace PROTOCOL
{
	public class COSTUME_INFO
	{
		public int i32CostumeUnique;

		public int i32CostumeCount;

		public int i32CostumePossibleToUse;

		public void UpdateCostumeCount(int updateCostumeCount, int updateCostumePossibleToUseCount)
		{
			this.i32CostumeCount += updateCostumeCount;
			this.i32CostumePossibleToUse += updateCostumePossibleToUseCount;
		}
	}
}
