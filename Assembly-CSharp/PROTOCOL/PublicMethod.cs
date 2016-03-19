using System;
using UnityEngine;

namespace PROTOCOL
{
	public class PublicMethod
	{
		public static long SERVERTIME;

		public static float CLIENTTIME;

		public static long GetDueDateTick(long _time)
		{
			DateTime dateTime = default(DateTime);
			DateTime dateTime2 = new DateTime(1970, 1, 1, 9, 0, 0);
			dateTime = DateTime.Now;
			dateTime2 = dateTime2.AddSeconds((double)_time);
			return (dateTime2.Ticks - dateTime.Ticks) / 10000000L;
		}

		public static DateTime GetDueDate(long _time)
		{
			DateTime result = new DateTime(1970, 1, 1, 9, 0, 0);
			result = result.AddSeconds((double)_time);
			return result;
		}

		public static long GetYearFromSec(long iSec)
		{
			long num = 31104000L;
			long result;
			if (iSec >= num)
			{
				result = iSec / num;
			}
			else
			{
				result = 0L;
			}
			return result;
		}

		public static long GetMonthFromSec(long iSec)
		{
			long num = 2592000L;
			long num2;
			if (iSec >= num)
			{
				num2 = iSec / num;
				num2 %= 12L;
			}
			else
			{
				num2 = 0L;
			}
			return num2;
		}

		public static long GetDayFromSec(long iSec)
		{
			long num = 86400L;
			long num2;
			if (iSec >= num)
			{
				num2 = iSec / num;
				num2 %= 30L;
			}
			else
			{
				num2 = 0L;
			}
			return num2;
		}

		public static long GetTotalDayFromSec(long iSec)
		{
			long num = 86400L;
			long result;
			if (iSec >= num)
			{
				result = iSec / num;
			}
			else
			{
				result = 0L;
			}
			return result;
		}

		public static long GetHourFromSec(long iSec)
		{
			long num = 3600L;
			long num2;
			if (iSec >= num)
			{
				num2 = iSec / num;
				num2 %= 24L;
			}
			else
			{
				num2 = 0L;
			}
			return num2;
		}

		public static long GetTotalHourFromSec(long iSec)
		{
			long num = 3600L;
			long result;
			if (iSec >= num)
			{
				result = iSec / num;
			}
			else
			{
				result = 0L;
			}
			return result;
		}

		public static long GetMinuteFromSec(long iSec)
		{
			long num = 60L;
			long num2 = 0L;
			if (iSec >= num2)
			{
				num2 = iSec / num;
				num2 %= 60L;
			}
			else
			{
				num2 = 0L;
			}
			return num2;
		}

		public static DateTime GetNowTime()
		{
			DateTime dateTime = default(DateTime);
			return DateTime.Now;
		}

		public static long GetCurTime()
		{
			long num = (long)(Time.realtimeSinceStartup - PublicMethod.CLIENTTIME);
			return PublicMethod.SERVERTIME + num;
		}

		public static long GetDiffSecond(long beforeSec)
		{
			long curTime = PublicMethod.GetCurTime();
			return curTime - beforeSec;
		}

		public static int GetDiffMinute(long beforeSec)
		{
			long diffSecond = PublicMethod.GetDiffSecond(beforeSec);
			return (int)(diffSecond / 60L);
		}

		public static string PassTime(long _time)
		{
			DateTime dueDate = PublicMethod.GetDueDate(_time);
			DateTime nowTime = PublicMethod.GetNowTime();
			long num = (long)((nowTime.Year - dueDate.Year) * 365 + (nowTime.Month - dueDate.Month) * 30 + (nowTime.Day - dueDate.Day));
			string result = string.Empty;
			if (num > 0L)
			{
				result = num.ToString() + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("961");
			}
			else
			{
				result = string.Concat(new string[]
				{
					dueDate.Hour.ToString("00"),
					":",
					dueDate.Minute.ToString("00"),
					":",
					dueDate.Second.ToString("00")
				});
			}
			return result;
		}

		public static string ConvertTime(int _time)
		{
			string str = string.Empty;
			uint num = (uint)(_time / 3600);
			uint num2 = (uint)(((long)_time - (long)((ulong)(num * 3600u))) / 60L);
			uint num3 = (uint)((long)_time - (long)((ulong)(num * 3600u)) - (long)((ulong)(num2 * 60u)));
			str = num.ToString("#,#00") + " : ";
			return str + num2.ToString("00") + " : " + num3.ToString("00");
		}

		public static string ConvertTime(long _time)
		{
			string str = string.Empty;
			uint num = (uint)(_time / 3600L);
			uint num2 = (uint)((_time - (long)((ulong)(num * 3600u))) / 60L);
			uint num3 = (uint)(_time - (long)((ulong)(num * 3600u)) - (long)((ulong)(num2 * 60u)));
			str = num.ToString("#,#00") + " : ";
			return str + num2.ToString("00") + " : " + num3.ToString("00");
		}

		public static long GetTick()
		{
			if (TsPlatform.IsIPhone)
			{
				return (long)Environment.TickCount;
			}
			return (long)(Environment.TickCount & 2147483647);
		}

		public static int GetTickMaxValue()
		{
			return Environment.TickCount & 2147483647;
		}
	}
}
