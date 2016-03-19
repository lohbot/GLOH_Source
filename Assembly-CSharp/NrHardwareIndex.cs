using System;
using UnityEngine;

internal static class NrHardwareIndex
{
	public static TsQualityManager.Level GetOptimizedQualityLevel()
	{
		return (!TsPlatform.IsMobile) ? NrHardwareIndex.GetLevel_Web() : NrHardwareIndex.GetLevel_Mobile();
	}

	private static TsQualityManager.Level GetLevel_Web()
	{
		int graphicsShaderLevel = SystemInfo.graphicsShaderLevel;
		int num = SystemInfo.graphicsPixelFillrate;
		int graphicsMemorySize = SystemInfo.graphicsMemorySize;
		int processorCount = SystemInfo.processorCount;
		int systemMemorySize = SystemInfo.systemMemorySize;
		int supportedRenderTargetCount = SystemInfo.supportedRenderTargetCount;
		bool supportsShadows = SystemInfo.supportsShadows;
		if (num < 0)
		{
			if (graphicsShaderLevel > 30)
			{
				num = 3000;
			}
			else
			{
				num = 2000;
			}
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
		int[] array = new int[]
		{
			3000,
			5000,
			8000,
			9000
		};
		int num2 = array.Length;
		while (--num2 > 0 && num < array[num2])
		{
		}
		if (graphicsMemorySize <= 128)
		{
			num2 = 0;
		}
		if (num < array[0])
		{
			TsLog.LogWarning("[자동 품질 설정] H/W 사양이 낮아도 너무 낮아~ (FillRate={0})", new object[]
			{
				num
			});
		}
		int num3 = Mathf.Clamp(num2, 0, 4);
		TsLog.Log("[자동 품질 설정] QualityLevel=\"{0}\" (FillRate={1}, CPU={2}, VideoRAM={3}, SystemRAM={4}, RenderTargets={5}, SupportShadow={6}, Processor=\"{7}\")", new object[]
		{
			(TsQualityManager.Level)num3,
			num,
			processorCount,
			graphicsMemorySize,
			systemMemorySize,
			supportedRenderTargetCount,
			supportsShadows,
			SystemInfo.processorType
		});
		return (TsQualityManager.Level)num3;
	}

	private static TsQualityManager.Level GetLevel_Mobile()
	{
		int num = SystemInfo.graphicsPixelFillrate;
		int graphicsMemorySize = SystemInfo.graphicsMemorySize;
		int processorCount = SystemInfo.processorCount;
		int systemMemorySize = SystemInfo.systemMemorySize;
		int supportedRenderTargetCount = SystemInfo.supportedRenderTargetCount;
		bool supportsShadows = SystemInfo.supportsShadows;
		if (num < 0)
		{
			int num2 = processorCount * 100;
			int num3 = (int)Convert.ToInt16((double)graphicsMemorySize * 0.2);
			num = num2 + num3;
		}
		int[] array = new int[]
		{
			500,
			600,
			1000,
			1200
		};
		int num4 = array.Length;
		while (--num4 > 0 && num < array[num4])
		{
		}
		if (num < array[0])
		{
			TsLog.LogWarning("[자동 품질 설정] H/W 사양이 낮아도 너무 낮아~ (FillRate={0})", new object[]
			{
				num
			});
		}
		int num5 = Mathf.Clamp(num4, 2, 4);
		TsLog.Log("[자동 품질 설정] QualityLevel=\"{0}\" (FillRate={1}, CPU={2}, VideoRAM={3}, SystemRAM={4}, RenderTargets={5}, SupportShadow={6}, Processor=\"{7}\")", new object[]
		{
			(TsQualityManager.Level)num5,
			num,
			processorCount,
			graphicsMemorySize,
			systemMemorySize,
			supportedRenderTargetCount,
			supportsShadows,
			SystemInfo.processorType
		});
		return (TsQualityManager.Level)num5;
	}
}
