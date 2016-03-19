using System;
using System.Text;
using UnityEngine;

public class TKString
{
	private string mValue;

	public int mLength;

	public int Legth
	{
		get
		{
			return this.mLength;
		}
	}

	public TKString(string _value, int _Length)
	{
		this.mValue = _value;
		this.mLength = _Length;
	}

	public override string ToString()
	{
		return this.mValue;
	}

	public byte[] ToBytes()
	{
		int num = this.mValue.Length * 2;
		byte[] array = new byte[num];
		Encoding.Unicode.GetBytes(this.mValue, 0, this.mValue.Length, array, 0);
		return array;
	}

	public byte[] Char2Byte(char[] _Buffer)
	{
		return null;
	}

	public static char[] StringChar(string _Src)
	{
		return _Src.ToCharArray();
	}

	public static void CharChar(char[] _Src, ref char[] _Des)
	{
		int num = Mathf.Min(_Des.Length, _Src.Length);
		for (int i = 0; i < _Des.Length; i++)
		{
			if (i < num)
			{
				_Des[i] = _Src[i];
			}
			else
			{
				_Des[i] = '\0';
			}
		}
	}

	public static void CharChar(char[] _Src, ref char[,] _Des, int count)
	{
		int num = Mathf.Min(_Des.Length, _Src.Length);
		for (int i = 0; i < _Des.Length; i++)
		{
			if (i < num)
			{
				_Des[count, i] = _Src[i];
			}
			else
			{
				_Des[count, i] = '\0';
			}
		}
	}

	public static void StringChar(string _Src, ref char[] _Des)
	{
		char[] src = _Src.ToCharArray();
		TKString.CharChar(src, ref _Des);
	}

	public static void StringChar(string _Src, ref char[] _Des, int startindex, int length)
	{
		_Src = _Src.Substring(startindex, length);
		char[] src = _Src.ToCharArray();
		TKString.CharChar(src, ref _Des);
	}

	public static void StringChar(string _Src, ref char[,] _Des, int count)
	{
		char[] src = _Src.ToCharArray();
		TKString.CharChar(src, ref _Des, count);
	}

	public static string NEWString(char[] Buffer)
	{
		int i;
		for (i = 0; i < Buffer.Length; i++)
		{
			if (Buffer[i] == '\0')
			{
				break;
			}
		}
		return new string(Buffer, 0, i);
	}

	public static implicit operator string(TKString value)
	{
		Debug.Log(string.Concat(new object[]
		{
			"unshuffle bits >> ",
			value.mValue,
			" L",
			value.mLength
		}));
		return value.mValue;
	}
}
