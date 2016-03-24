using System;
using UnityEngine;

namespace WellFired.Shared
{
	public static class ColorExtensions
	{
		public static int PackColor(Color colorToPack)
		{
			Color32 color = colorToPack;
			int num = 0;
			num |= (int)color.r;
			num <<= 8;
			num |= (int)color.g;
			num <<= 8;
			num |= (int)color.b;
			num <<= 8;
			return num | (int)color.a;
		}

		public static Color UnPackColor(int packedColor)
		{
			byte a = (byte)(packedColor >> 24 & 255);
			byte b = (byte)(packedColor >> 16 & 255);
			byte g = (byte)(packedColor >> 8 & 255);
			byte r = (byte)(packedColor & 255);
			return new Color32(r, g, b, a);
		}
	}
}
