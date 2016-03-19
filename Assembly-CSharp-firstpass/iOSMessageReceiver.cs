using System;
using UnityEngine;

public class iOSMessageReceiver : MonoBehaviour
{
	private void OnDidReceiveMemoryWarning(string parm)
	{
		Resources.UnloadUnusedAssets();
		GC.Collect();
		if (Profiler.enabled)
		{
			TsLog.LogWarning("[{0}] ReceiveMemoryWarning => memory clean up. (TotalMemorySize={1}, UsedHeapSize={2})", new object[]
			{
				Time.realtimeSinceStartup,
				GC.GetTotalMemory(false).ToString("###,###,###,###"),
				Profiler.usedHeapSize.ToString("###,###,###,###")
			});
		}
	}
}
