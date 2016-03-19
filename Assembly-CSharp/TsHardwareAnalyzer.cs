using System;
using UnityEngine;

internal static class TsHardwareAnalyzer
{
	public static int GetSmartFPS()
	{
		int[] array = new int[]
		{
			60,
			40,
			30,
			25,
			20,
			15
		};
		int level = TsHardwareAnalyzer.GetLevel();
		int num = Mathf.Clamp(level - 1, 0, array.Length - 1);
		return array[num];
	}

	public static int GetLevel()
	{
		int graphicsShaderLevel = SystemInfo.graphicsShaderLevel;
		int num = SystemInfo.graphicsPixelFillrate;
		int graphicsMemorySize = SystemInfo.graphicsMemorySize;
		int processorCount = SystemInfo.processorCount;
		if (num < 0)
		{
			num = ((graphicsShaderLevel >= 30) ? 3000 : 2000);
			if (processorCount >= 6)
			{
				num *= 3;
			}
			else if (processorCount >= 3)
			{
				num *= 2;
			}
			if (graphicsMemorySize >= 512)
			{
				num *= 2;
			}
			else if (graphicsMemorySize <= 128)
			{
				num /= 2;
			}
		}
		int width = Screen.width;
		int height = Screen.height;
		float num2 = (float)(width * height + 480000);
		float num3 = num2 * 30f / 1000000f;
		float num4 = (float)num / num3;
		int[] array = new int[]
		{
			150,
			110,
			80,
			60,
			50,
			0
		};
		int num5 = 0;
		while (num5 < array.Length - 1 && num4 < (float)array[num5])
		{
			num5++;
		}
		if (graphicsMemorySize <= 128)
		{
			num5 = 0;
		}
		TsLog.Log("HardwareLevel=\"{0}\" (fillrate={1}, renderWidth={2}, renderHeight={3}, fillNeed={4}, fillMult={5}, vram={6})", new object[]
		{
			num5,
			SystemInfo.graphicsPixelFillrate,
			width,
			height,
			num3,
			num4,
			graphicsMemorySize
		});
		return num5 + 1;
	}
}
