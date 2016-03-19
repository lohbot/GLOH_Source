using System;

public class ElementSol
{
	public int[] Element_SolKind = new int[5];

	public long[] Element_SolID = new long[5];

	public void Init()
	{
		for (int i = 0; i < 5; i++)
		{
			this.Element_SolKind[i] = 0;
			this.Element_SolID[i] = 0L;
		}
	}

	public void SetLegendSol(int i32Kind, long i64SolID)
	{
		for (int i = 0; i < 5; i++)
		{
			if (this.Element_SolKind[i] == 0)
			{
				this.Element_SolKind[i] = i32Kind;
				this.Element_SolID[i] = i64SolID;
				break;
			}
		}
	}

	public bool GetLegendSolCheck(int i32Kind, long i64SolID)
	{
		bool result = false;
		for (int i = 0; i < 5; i++)
		{
			if (this.Element_SolKind[i] == i32Kind && this.Element_SolID[i] == i64SolID)
			{
				result = true;
				break;
			}
		}
		return result;
	}
}
