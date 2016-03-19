using System;

public class InitialSearch
{
	private static int HANGUL_BEGIN_UNICODE = 44032;

	private static int HANGUL_END_UNICODE = 55203;

	private static int HANGUL_BASE_UNIT = 588;

	private static string HANGUL_INITIAL = "ㄱㄲㄴㄷㄸㄹㅁㅂㅃㅅㅆㅇㅈㅉㅊㅋㅌㅍㅎ";

	private static bool[] IsInitialHanGulKeyWorld(string SearchKeyWord)
	{
		if (SearchKeyWord == null)
		{
			return null;
		}
		if (SearchKeyWord.Length == 0)
		{
			return null;
		}
		bool[] array = new bool[SearchKeyWord.Length];
		for (int i = 0; i < SearchKeyWord.Length; i++)
		{
			char value = SearchKeyWord[i];
			bool flag = false;
			int num = InitialSearch.HANGUL_INITIAL.IndexOf(value);
			if (num >= 0)
			{
				array[i] = true;
			}
			else if (!flag)
			{
				array[i] = false;
			}
		}
		return array;
	}

	private static bool IsCheckString(string szValue, string szSearchKeyWord, bool[] bInitial)
	{
		if (bInitial == null)
		{
			return false;
		}
		int num = 0;
		int num2 = -1;
		for (int i = 0; i < szValue.Length; i++)
		{
			char c = szValue[i];
			char c2 = szSearchKeyWord[num];
			if (bInitial[num])
			{
				if (InitialSearch.HANGUL_BEGIN_UNICODE > (int)c || (int)c > InitialSearch.HANGUL_END_UNICODE)
				{
					return false;
				}
				int num3 = (int)c - InitialSearch.HANGUL_BEGIN_UNICODE;
				int index = num3 / InitialSearch.HANGUL_BASE_UNIT;
				char c3 = InitialSearch.HANGUL_INITIAL[index];
				if (c3 == c2)
				{
					if (num > 0 && num2 + 1 != i)
					{
						return false;
					}
					num++;
					num2 = i;
				}
			}
			else if (c == c2)
			{
				if (num > 0 && num2 + 1 != i)
				{
					return false;
				}
				num++;
				num2 = i;
			}
			if (num == bInitial.Length)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsCheckString(string szValue, string szSearchKeyWord)
	{
		if (szValue == null)
		{
			return false;
		}
		if (szValue.Length == 0)
		{
			return false;
		}
		if (szSearchKeyWord == null)
		{
			return false;
		}
		if (szSearchKeyWord.Length == 0)
		{
			return false;
		}
		bool[] bInitial = InitialSearch.IsInitialHanGulKeyWorld(szSearchKeyWord);
		return InitialSearch.IsCheckString(szValue, szSearchKeyWord, bInitial);
	}

	public static bool[] IsCheckString(string[] szValue, string szSearchKeyWord)
	{
		if (szValue == null)
		{
			return null;
		}
		if (szValue.Length == 0)
		{
			return null;
		}
		if (szSearchKeyWord == null)
		{
			return null;
		}
		if (szSearchKeyWord.Length == 0)
		{
			return null;
		}
		bool[] array = new bool[szValue.Length];
		bool[] bInitial = InitialSearch.IsInitialHanGulKeyWorld(szSearchKeyWord);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = InitialSearch.IsCheckString(szValue[i], szSearchKeyWord, bInitial);
		}
		return array;
	}
}
