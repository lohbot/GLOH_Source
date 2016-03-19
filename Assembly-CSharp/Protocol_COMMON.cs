using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Protocol_COMMON
{
	public const long L_DATE_TIME_TICKS = 10000000L;

	public static string Convert_Encoding(string a_strString)
	{
		string text = string.Empty;
		try
		{
			Encoding unicode = Encoding.Unicode;
			byte[] bytes = unicode.GetBytes(a_strString);
			for (int i = 0; i < bytes.Length; i++)
			{
				text += bytes[i];
				if (i != bytes.Length - 1)
				{
					text += "_";
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(string.Concat(new object[]
			{
				"--------- Protocol_COMMON.cs -- Convert_Encoding -- ex -- ",
				ex,
				" -- ",
				Environment.TickCount
			}));
		}
		return text;
	}

	public static long LossSecond_To_Ticks(long a_lLossSecond)
	{
		return DateTime.Now.AddSeconds((double)a_lLossSecond).Ticks;
	}

	public static long Ticks_To_LossSecond(long a_lTicks)
	{
		return Protocol_COMMON.Ticks_To_TimeSpan(a_lTicks).Ticks / 10000000L;
	}

	public static TimeSpan Ticks_To_TimeSpan(long a_lTicks)
	{
		DateTime d = new DateTime(a_lTicks);
		TimeSpan result = d - DateTime.Now;
		if (result.Seconds < 0)
		{
			result = TimeSpan.Zero;
		}
		return result;
	}

	public static float Get_Percent(long a_lSecond, long a_lComplteSecond)
	{
		float num = (float)a_lSecond / (float)a_lComplteSecond;
		float num2 = 0.99f - num;
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		else if (num2 >= 1f)
		{
			num2 = 0.99f;
		}
		return num2;
	}

	public static string Convert_Decoding(string a_strEncoding)
	{
		string result = string.Empty;
		try
		{
			List<byte> list = new List<byte>();
			string[] array = a_strEncoding.Split(new char[]
			{
				'_'
			});
			for (int i = 0; i < array.Length; i++)
			{
				byte item = byte.Parse(array[i]);
				list.Add(item);
			}
			Encoding unicode = Encoding.Unicode;
			result = unicode.GetString(list.ToArray());
		}
		catch (Exception ex)
		{
			Debug.Log(string.Concat(new object[]
			{
				"--------- Protocol_COMMON.cs -- Convert_Decoding -- ex -- ",
				ex,
				" -- ",
				Environment.TickCount
			}));
		}
		return result;
	}

	public static bool Prev_Page(ref int a_nCurrentPage)
	{
		int num = a_nCurrentPage;
		num--;
		if (num < 1)
		{
			num = 1;
		}
		if (num != a_nCurrentPage)
		{
			a_nCurrentPage = num;
			return true;
		}
		return false;
	}

	public static bool Next_Page(ref int a_nCurrentPage, int a_nMaxPage)
	{
		int num = a_nCurrentPage;
		num++;
		if (num > a_nMaxPage)
		{
			num = a_nMaxPage;
		}
		if (num != a_nCurrentPage)
		{
			a_nCurrentPage = num;
			return true;
		}
		return false;
	}

	public static void Page_Setting(int a_nListCount, int a_nOnePage, ref int a_nCurrentPage, ref int a_nPageMax, out int a_nStart, out int a_nEnd)
	{
		if (a_nCurrentPage <= 0)
		{
			a_nCurrentPage = 1;
		}
		a_nPageMax = a_nListCount / a_nOnePage;
		if (a_nListCount % a_nOnePage > 0)
		{
			a_nPageMax++;
		}
		if (a_nPageMax < a_nCurrentPage)
		{
			if (a_nPageMax <= 0)
			{
				a_nPageMax = 1;
			}
			a_nCurrentPage = a_nPageMax;
		}
		a_nStart = (a_nCurrentPage - 1) * a_nOnePage;
		a_nEnd = a_nStart + a_nOnePage;
		if (a_nEnd > a_nListCount)
		{
			a_nEnd = a_nListCount;
		}
	}
}
