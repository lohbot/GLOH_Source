using Ndoors.Memory;
using System;
using UnityEngine;

namespace Statistics
{
	public static class MemoryMonitor
	{
		private static string _GAME_OBJECT_NAME = "@MemoryMonitor";

		private static string _RES_NAME = "MemoryAlert";

		private static int m_cycleTime = PlayerPrefs.GetInt("MemoryMonitor_Cycle", 5);

		private static int m_growUpAllowedSize = PlayerPrefs.GetInt("MemoryMonitor_GrowUpAllowed", 100);

		public static int cycleTime
		{
			get
			{
				return MemoryMonitor.m_cycleTime;
			}
			set
			{
				MemoryMonitor.m_cycleTime = value;
				PlayerPrefs.SetInt("MemoryMonitor_Cycle", value);
			}
		}

		public static int growUpAllowedSize
		{
			get
			{
				return MemoryMonitor.m_growUpAllowedSize;
			}
			set
			{
				MemoryMonitor.m_growUpAllowedSize = value;
				PlayerPrefs.SetInt("MemoryMonitor_GrowUpAllowed", value);
			}
		}

		public static int growUpAllowedBytes
		{
			get
			{
				return MemoryMonitor.m_growUpAllowedSize * 1024 * 1024;
			}
		}

		public static bool Start()
		{
			GameObject gameObject = GameObject.Find(MemoryMonitor._GAME_OBJECT_NAME);
			if (gameObject == null)
			{
				gameObject = new GameObject(MemoryMonitor._GAME_OBJECT_NAME);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				MemoryMonitorBehaviour memoryMonitorBehaviour = gameObject.AddComponent<MemoryMonitorBehaviour>();
				GameObject gameObject2 = ResourceCache.LoadFromResourcesImmediate(MemoryMonitor._RES_NAME) as GameObject;
				if (gameObject2 != null)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2) as GameObject;
					if (gameObject3)
					{
						gameObject3.transform.parent = gameObject.transform;
						memoryMonitorBehaviour.RedRing = gameObject3.GetComponent<GUITexture>();
						return true;
					}
				}
				else
				{
					TsLog.LogWarning("[Memory] \"{0}\" 리소스를 읽어들이지 못 했습니다.", new object[]
					{
						MemoryMonitor._RES_NAME
					});
				}
			}
			return false;
		}

		public static bool Stop()
		{
			GameObject gameObject = GameObject.Find(MemoryMonitor._GAME_OBJECT_NAME);
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
				return true;
			}
			return false;
		}

		public static bool Show(float displaySec = 0f)
		{
			GameObject gameObject = GameObject.Find(MemoryMonitor._GAME_OBJECT_NAME);
			if (gameObject != null)
			{
				MemoryMonitorBehaviour component = gameObject.GetComponent<MemoryMonitorBehaviour>();
				if (component != null)
				{
					if (displaySec == 0f)
					{
						component.ShowAlert(30f);
					}
					else
					{
						component.ShowAlert(displaySec);
					}
					return true;
				}
			}
			return false;
		}
	}
}
