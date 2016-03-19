using System;
using UnityEngine;

public class TsItweenTimer : MonoBehaviour
{
	private static float upkeepTime;

	private static float oldUpkeepTime;

	private float revisionTime = 0.02f;

	public static float Time
	{
		get
		{
			return TsItweenTimer.upkeepTime;
		}
	}

	public static float DeltaTime
	{
		get
		{
			return TsItweenTimer.upkeepTime - TsItweenTimer.oldUpkeepTime;
		}
	}

	private void Start()
	{
		if (TsPlatform.IsMobile)
		{
			this.revisionTime = 0.04f;
		}
	}

	private void Update()
	{
		TsItweenTimer.oldUpkeepTime = TsItweenTimer.upkeepTime;
		if (UnityEngine.Time.smoothDeltaTime > this.revisionTime)
		{
			TsItweenTimer.upkeepTime += this.revisionTime;
		}
		else
		{
			TsItweenTimer.upkeepTime += UnityEngine.Time.smoothDeltaTime;
		}
	}
}
