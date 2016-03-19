using System;

public class NrLevelUpInfo
{
	public string szExpType = string.Empty;

	public long[] nExp = new long[200];

	public bool IsValidLevel(short level)
	{
		return level > 0 && level <= 200;
	}

	public void SetData(LEVEL_EXP pkLevelExp)
	{
		if (!this.IsValidLevel(pkLevelExp.LEVEL))
		{
			return;
		}
		if (!this.szExpType.Equals(pkLevelExp.EXP_TYPE))
		{
			this.szExpType = pkLevelExp.EXP_TYPE;
		}
		this.nExp[(int)(pkLevelExp.LEVEL - 1)] = pkLevelExp.EXP;
	}

	public long GetExp(short level)
	{
		if (!this.IsValidLevel(level))
		{
			return 0L;
		}
		return this.nExp[(int)(level - 1)];
	}
}
