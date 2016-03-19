using System;

public class ANNUALIZED
{
	public static string Convert(int _Num)
	{
		return string.Format("{0:###,###,###,##0}", _Num);
	}

	public static string Convert(long _Num)
	{
		return string.Format("{0:###,###,###,##0}", _Num);
	}
}
