using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class CProgramUtil
{
	public static void SetDrawtextureSizeByResolution(DrawTexture _image)
	{
		float num = 3f;
		float num2 = (float)Screen.width + num;
		float num3 = (float)Screen.width - num;
		float num4 = (float)Screen.height + num;
		float num5 = (float)Screen.height - num;
		if ((num2 >= 1680f & num3 <= 1680f) && (num4 >= 900f & num5 <= 900f))
		{
			_image.SetSize(1688f, 1266f);
			_image.SetLocation(-4f, -113f);
		}
		else if (num2 >= 1280f && num3 <= 1280f && num4 >= 960f && num5 <= 960f)
		{
			_image.SetSize(1280f, 960f);
			_image.SetLocation(0f, 0f);
		}
		else if (num2 >= 1280f && num3 <= 1280f && num4 >= 700f && num5 <= 700f)
		{
			_image.SetSize(1280f, 960f);
			_image.SetLocation(0f, -90f);
		}
		else if (num2 >= 1024f && num3 <= 1024f && num4 >= 768f && num5 <= 768f)
		{
			_image.SetSize(1032f, 774f);
			_image.SetLocation(-4f, -3f);
		}
		else if (num2 >= 1024f && num3 <= 1024f && num4 >= 600f && num5 <= 600f)
		{
			_image.SetSize(1032f, 774f);
			_image.SetLocation(-4f, -53f);
		}
		else
		{
			float num6 = (float)(Screen.width + 10) - _image.GetSize().x;
			float num7 = 1f + num6 / _image.GetSize().x;
			float width = _image.GetSize().x * num7;
			float height = _image.GetSize().y * num7;
			_image.SetSize(width, height);
		}
	}

	public static Vector2 GetControlPositionByResolution(Vector2 OriginalSize, Vector2 ChangedSize, Vector2 fControlPosition, Vector2 szControlSize)
	{
		Vector2 result = default(Vector2);
		float num = (fControlPosition.x + szControlSize.x / 2f) * ChangedSize.x / OriginalSize.x;
		num -= szControlSize.x / 2f;
		float y = fControlPosition.y * ChangedSize.y / OriginalSize.y;
		result.x = num;
		result.y = y;
		return result;
	}

	public static string Get_History(string Key)
	{
		string result = string.Empty;
		if (PlayerPrefs.HasKey(Key))
		{
			string @string = PlayerPrefs.GetString(Key);
			result = Protocol_COMMON.Convert_Decoding(@string);
		}
		return result;
	}

	public static void Set_History<T>(Queue<string> m_quHistory, int MaxValues, string a_strText) where T : new()
	{
		if (m_quHistory.Contains(a_strText))
		{
			string[] array = m_quHistory.ToArray();
			m_quHistory.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				if (!(a_strText == array[i]))
				{
					m_quHistory.Enqueue(array[i]);
				}
			}
			m_quHistory.Enqueue(a_strText);
		}
		else if (m_quHistory.Count >= MaxValues)
		{
			m_quHistory.Dequeue();
			m_quHistory.Enqueue(a_strText);
		}
		else
		{
			m_quHistory.Enqueue(a_strText);
		}
		Type typeFromHandle = typeof(T);
		string[] array2 = m_quHistory.ToArray();
		for (int j = 0; j < array2.Length; j++)
		{
			object obj = Enum.ToObject(typeFromHandle, j);
			T t = (T)((object)obj);
			string key = t.ToString();
			string value = Protocol_COMMON.Convert_Encoding(array2[j]);
			PlayerPrefs.SetString(key, value);
		}
	}

	public static string GetTimeForm(long _Time)
	{
		string text = string.Empty;
		if (_Time == 0L)
		{
			text = "00";
		}
		else
		{
			long num = _Time / 10L;
			if (num < 1L)
			{
				text = "0";
			}
			else
			{
				text = num.ToString();
			}
			text += _Time % 10L;
		}
		return text;
	}

	public static string GetStrTimeFromSec(long _Sec)
	{
		long hourFromSec = PublicMethod.GetHourFromSec(_Sec);
		long minuteFromSec = PublicMethod.GetMinuteFromSec(_Sec);
		long time = _Sec % 60L;
		string empty = string.Empty;
		return string.Format("{0} : {1} : {2}", CProgramUtil.GetTimeForm(hourFromSec), CProgramUtil.GetTimeForm(minuteFromSec), CProgramUtil.GetTimeForm(time));
	}

	public static string GetStrTotalFromSec(int _Sec)
	{
		string text = PublicMethod.GetYearFromSec((long)_Sec).ToString();
		string text2 = PublicMethod.GetMonthFromSec((long)_Sec).ToString();
		string text3 = PublicMethod.GetDayFromSec((long)_Sec).ToString();
		string text4 = PublicMethod.GetHourFromSec((long)_Sec).ToString();
		string text5 = PublicMethod.GetMinuteFromSec((long)_Sec).ToString();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1002");
		string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1003");
		string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1004");
		string textFromInterface4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1005");
		string textFromInterface5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1006");
		string text6 = string.Empty;
		if (text != "0")
		{
			text6 = text6 + text + textFromInterface + string.Empty;
		}
		if (text2 != "0")
		{
			text6 = text6 + text2 + textFromInterface2 + " ";
		}
		if (text3 != "0")
		{
			text6 = text6 + text3 + textFromInterface3 + " ";
		}
		if (text4 != "0")
		{
			text6 = text6 + text4 + textFromInterface4 + " ";
		}
		if (text5 != "0")
		{
			text6 = text6 + text5 + textFromInterface5;
		}
		if (_Sec == 0)
		{
			text6 = "0" + textFromInterface5;
		}
		return text6;
	}
}
