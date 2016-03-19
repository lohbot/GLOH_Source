using System;
using System.Text;
using UnityEngine;

[Serializable]
public struct TKChar
{
	private char[] mValue;

	private int mLength;

	public TKChar(int Length)
	{
		this.mLength = Length;
		this.mValue = new char[this.mLength];
	}

	public int GetLength()
	{
		return this.mLength * 2;
	}

	public void Set(char[] _Data)
	{
		TKString.CharChar(_Data, ref this.mValue);
	}

	public void Set(string _Data)
	{
		TKString.StringChar(_Data, ref this.mValue);
	}

	public TKChar SetByte(byte[] bBuffer, int iIndex, TKChar Base)
	{
		string @string = Encoding.Unicode.GetString(bBuffer, iIndex, Base.GetLength());
		TKChar result = new TKChar(Base.GetLength());
		result.Set(@string);
		return result;
	}

	public byte[] GetByte()
	{
		byte[] array = new byte[this.mValue.Length * 2];
		Encoding unicode = Encoding.Unicode;
		unicode.GetBytes(this.mValue, 0, this.mValue.Length, array, 0);
		return array;
	}

	public char[] Get()
	{
		return this.mValue;
	}

	public static implicit operator string(TKChar value)
	{
		string text = new string(value.mValue);
		Debug.Log(string.Concat(new object[]
		{
			"unshuffle string >> ",
			text,
			" L",
			value.mLength
		}));
		return text;
	}

	public static implicit operator char[](TKChar value)
	{
		Debug.Log(string.Concat(new object[]
		{
			"unshuffle char >> ",
			value.mValue,
			" L",
			value.mLength
		}));
		return value.mValue;
	}
}
