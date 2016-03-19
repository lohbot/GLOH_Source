using System;
using System.Collections.Generic;
using UnityEngine;

public class NumberControl
{
	private Texture2D[] mTexNums = new Texture2D[10];

	private float mWidth;

	private float mHeight;

	private float mSpacing;

	private Color TintColor = Color.white;

	private int mEmptyUnit = -1;

	public void Create10(string _TexCountineRes, float _Width, float _Height, float _Spacing)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < 10; i++)
		{
			list.Add(_TexCountineRes + i.ToString());
		}
		this.Create(list.ToArray(), _Width, _Height, _Spacing);
	}

	public void Create(string[] _TexRes, float _Width, float _Height, float _Spacing)
	{
		int num = _TexRes.Length;
		for (int i = 0; i < num; i++)
		{
			this.mTexNums[i] = (Texture2D)CResources.Load(_TexRes[i]);
		}
		this.mWidth = _Width;
		this.mHeight = _Height;
		this.mSpacing = _Spacing;
	}

	public void CreateIndex(string _TexRes, long[] _Number, float _Width, float _Height, float _Spacing)
	{
		int num = _Number.Length;
		for (int i = 0; i < num; i++)
		{
			long num2 = _Number[i];
			this.mTexNums[(int)(checked((IntPtr)num2))] = (Texture2D)CResources.Load(_TexRes + num2);
		}
		this.mWidth = _Width;
		this.mHeight = _Height;
		this.mSpacing = _Spacing;
	}

	public static long[] SeparateNum(long _Number)
	{
		return NumberControl.SeparateNum(_Number, -1);
	}

	public static long[] SeparateNum(long _Number, int _Unit)
	{
		string text = _Number.ToString();
		int num = Mathf.Max(text.Length, _Unit);
		long[] array = new long[num];
		int i = 0;
		int num2 = 0;
		while (i < num)
		{
			array[i] = 0L;
			if (num - i <= text.Length)
			{
				array[i] = (long)(text[num2++] - '0');
			}
			i++;
		}
		return array;
	}

	public void SetAlpha(float _Alpha)
	{
		this.TintColor.a = _Alpha;
	}

	public void SetEmptyUnit(int emptyUnit)
	{
		this.mEmptyUnit = emptyUnit;
	}

	public void DrawNumber(Vector2 _Pos, long _Number)
	{
		this.DrawNumber(_Pos, _Number, this.mWidth, this.mHeight);
	}

	public void DrawNumber(Vector2 _Pos, long _Number, float _Width, float _Height)
	{
		long[] array = NumberControl.SeparateNum(_Number, this.mEmptyUnit);
		int num = array.Length;
		Rect position = new Rect(_Pos.x, _Pos.y, _Width, _Height);
		for (int i = 0; i < num; i++)
		{
			position.x = _Pos.x + (float)i * _Width + (float)i * this.mSpacing - _Width;
			long num2 = array[i];
			Texture2D texture2D = this.mTexNums[(int)(checked((IntPtr)num2))];
			if (texture2D)
			{
				GUI.color = this.TintColor;
				GUI.DrawTexture(position, texture2D);
			}
		}
	}
}
