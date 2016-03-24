using System;

public class SolCombination_BatchSelectInfoManager : NrTSingleton<SolCombination_BatchSelectInfoManager>
{
	private string _batchSolCombinationOSKey = "BATCH_SOLCOMBINATION_UNIQUEKEY";

	private SolCombination_BatchSelectInfoManager()
	{
	}

	public void SetUserSelectedUniqeKey(int solCombinationUniqeKey, int STARTPOS_INDEX)
	{
		SolCombinationKeySaveLoader.SaveSolCombinationUniqeKeyInOS(this.GetBathKey(STARTPOS_INDEX), solCombinationUniqeKey);
	}

	public int GetUserSelectedUniqeKey(int STARTPOS_INDEX)
	{
		return SolCombinationKeySaveLoader.GetSolCombinationUniqeKeyInOS(this.GetBathKey(STARTPOS_INDEX));
	}

	private string GetBathKey(int STARTPOS_INDEX)
	{
		return this._batchSolCombinationOSKey + STARTPOS_INDEX.ToString();
	}
}
