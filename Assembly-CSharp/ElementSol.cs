using System;

public class ElementSol
{
	public NkSoldierInfo[] Element_Sol = new NkSoldierInfo[5];

	public void Init()
	{
		for (int i = 0; i < 5; i++)
		{
			this.Element_Sol[i] = null;
		}
	}

	public void SetLegendSol(NkSoldierInfo kSol, int nCount)
	{
		if (nCount < 0 || nCount > 5)
		{
			return;
		}
		this.Element_Sol[nCount] = kSol;
	}

	public bool GetLegendSolCheck(int i32Kind, long i64SolID)
	{
		bool result = false;
		for (int i = 0; i < 5; i++)
		{
			if (this.Element_Sol[i] != null && this.Element_Sol[i].GetSolID() == i64SolID)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public NkSoldierInfo GetLegendSolInfo(int nCount)
	{
		if (nCount < 0 || nCount > 5)
		{
			return null;
		}
		return this.Element_Sol[nCount];
	}
}
