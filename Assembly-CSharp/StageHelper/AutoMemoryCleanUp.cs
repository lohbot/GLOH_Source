using Ndoors.Framework.Stage;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace StageHelper
{
	public static class AutoMemoryCleanUp
	{
		private static float REGULAR_PREIOD;

		private static float STAY_PEROID;

		private static float MEM_CHECK_TIME;

		private static long MAX_HEAP_SIZE;

		private static long GROWING_MEM_PRERMISSION;

		private static long MAX_HEAP_UP_SIZE;

		private static float m_RegularNextChkTime;

		private static float m_StayNextChkTime;

		private static bool m_Moving;

		private static long m_LimitHeapSize;

		private static int m_LimitMemoryCount;

		private static float m_fMemCheckTime;

		private static bool m_bEnableMemoryClean;

		private static bool m_bCallMemoryClean;

		private static bool m_bReservedMemoryClean;

		private static bool m_Enable;

		private static long m_CheckingMemSize;

		public static bool Enable
		{
			get
			{
				return AutoMemoryCleanUp.m_Enable;
			}
			set
			{
				AutoMemoryCleanUp.m_Enable = value;
			}
		}

		static AutoMemoryCleanUp()
		{
			AutoMemoryCleanUp.MAX_HEAP_UP_SIZE = 50L;
			AutoMemoryCleanUp.m_Enable = true;
			AutoMemoryCleanUp.STAY_PEROID = 180f;
			AutoMemoryCleanUp.MEM_CHECK_TIME = 300f;
			if (TsPlatform.IsMobile)
			{
				AutoMemoryCleanUp.REGULAR_PREIOD = 180f;
				int systemMemorySize = SystemInfo.systemMemorySize;
				if (systemMemorySize < 1024)
				{
					AutoMemoryCleanUp.MAX_HEAP_SIZE = 300L;
				}
				else
				{
					AutoMemoryCleanUp.MAX_HEAP_SIZE = 380L;
				}
				AutoMemoryCleanUp.GROWING_MEM_PRERMISSION = 30L;
			}
			else
			{
				AutoMemoryCleanUp.REGULAR_PREIOD = 600f;
				AutoMemoryCleanUp.MAX_HEAP_SIZE = 900L;
				AutoMemoryCleanUp.GROWING_MEM_PRERMISSION = 100L;
			}
			AutoMemoryCleanUp.m_LimitMemoryCount = 0;
			AutoMemoryCleanUp.m_LimitHeapSize = AutoMemoryCleanUp.MAX_HEAP_SIZE;
			CharObserver.OnChangeMoving += new Action<bool>(AutoMemoryCleanUp.OnChangeCharacterMoving);
			AutoMemoryCleanUp.Reset();
		}

		public static void Reset()
		{
			try
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				AutoMemoryCleanUp.m_StayNextChkTime = realtimeSinceStartup + AutoMemoryCleanUp.STAY_PEROID;
				AutoMemoryCleanUp.m_RegularNextChkTime = realtimeSinceStartup + AutoMemoryCleanUp.REGULAR_PREIOD;
				AutoMemoryCleanUp.m_fMemCheckTime = realtimeSinceStartup + AutoMemoryCleanUp.MEM_CHECK_TIME;
				AutoMemoryCleanUp.m_CheckingMemSize = NrTSingleton<NrMainSystem>.Instance.AppMemory;
				AutoMemoryCleanUp.m_LimitMemoryCount = 0;
			}
			catch (Exception obj)
			{
				TsLog.LogError(obj);
			}
		}

		public static void CleanUp()
		{
			AutoMemoryCleanUp.m_bCallMemoryClean = true;
		}

		private static void MemoryClean()
		{
			Resources.UnloadUnusedAssets();
			GC.Collect();
			GC.Collect();
			AutoMemoryCleanUp.m_bEnableMemoryClean = false;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			AutoMemoryCleanUp.m_StayNextChkTime = realtimeSinceStartup + AutoMemoryCleanUp.STAY_PEROID;
			AutoMemoryCleanUp.m_RegularNextChkTime = realtimeSinceStartup + AutoMemoryCleanUp.REGULAR_PREIOD;
			TsLog.LogWarning("[Memory] AutoMemoryCleanUp.CleanUp() => call GC, UnloadUsedAsset()\n AppMemory : {0}", new object[]
			{
				NrTSingleton<NrMainSystem>.Instance.AppMemory
			});
		}

		public static void CleanUpImmediate()
		{
			AutoMemoryCleanUp.MemoryClean();
		}

		public static void CleanUpReserved()
		{
			TsLog.LogWarning("[Memory] CleanUpReserved CleanStatue : {0}", new object[]
			{
				AutoMemoryCleanUp.m_bEnableMemoryClean
			});
			AutoMemoryCleanUp.m_bCallMemoryClean = true;
			AutoMemoryCleanUp.m_bReservedMemoryClean = true;
		}

		public static void OnChangeCharacterMoving(bool move)
		{
			if (!move)
			{
				AutoMemoryCleanUp.m_StayNextChkTime = Time.realtimeSinceStartup + AutoMemoryCleanUp.STAY_PEROID;
			}
			AutoMemoryCleanUp.m_Moving = move;
		}

		private static bool IsDangerousMemoryGrowing(float curTime)
		{
			try
			{
				if (curTime > AutoMemoryCleanUp.m_RegularNextChkTime)
				{
					long appMemory = NrTSingleton<NrMainSystem>.Instance.AppMemory;
					bool flag = appMemory > AutoMemoryCleanUp.m_CheckingMemSize;
					if (flag)
					{
						AutoMemoryCleanUp.m_CheckingMemSize = appMemory + AutoMemoryCleanUp.GROWING_MEM_PRERMISSION;
						return true;
					}
				}
			}
			catch (Exception obj)
			{
				TsLog.LogError(obj);
			}
			return false;
		}

		private static void RecodeMomorySize(float curTime)
		{
			try
			{
				if (curTime > AutoMemoryCleanUp.m_fMemCheckTime)
				{
					AutoMemoryCleanUp.m_fMemCheckTime = curTime + AutoMemoryCleanUp.MEM_CHECK_TIME;
					NrTSingleton<NrMainSystem>.Instance.AppMemory = TsPlatform.Operator.GetAppMemory();
					TsPlatform.FileLog(string.Format("Stage : {0}  Memory : {1} ", Scene.CurScene, NrTSingleton<NrMainSystem>.Instance.AppMemory));
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}

		private static bool IsLackMemorySize(float curTime)
		{
			bool result;
			try
			{
				if (curTime > AutoMemoryCleanUp.m_RegularNextChkTime && NrTSingleton<NrMainSystem>.Instance.AppMemory > AutoMemoryCleanUp.m_LimitHeapSize)
				{
					TsLog.Log("[Memory] IsLackMemorySize() : Memory Over = {0} MB", new object[]
					{
						AutoMemoryCleanUp.m_LimitHeapSize
					});
					if (++AutoMemoryCleanUp.m_LimitMemoryCount > 3)
					{
						AutoMemoryCleanUp.m_LimitMemoryCount = 0;
						AutoMemoryCleanUp.m_LimitHeapSize += AutoMemoryCleanUp.MAX_HEAP_UP_SIZE;
					}
					result = true;
				}
				else
				{
					AutoMemoryCleanUp.m_LimitMemoryCount = 0;
					result = false;
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
				result = false;
			}
			return result;
		}

		private static bool IsStandALongTime(float curTime)
		{
			return !AutoMemoryCleanUp.m_Moving && curTime > AutoMemoryCleanUp.m_StayNextChkTime;
		}

		[DebuggerHidden]
		public static IEnumerator Action()
		{
			return new AutoMemoryCleanUp.<Action>c__IteratorD();
		}

		private static void MemoryCleanUpReserved()
		{
			if (AutoMemoryCleanUp.m_bEnableMemoryClean && AutoMemoryCleanUp.m_bReservedMemoryClean)
			{
				TsLog.LogWarning("[Memory] MemoryCleanUpReserved ", new object[0]);
				AutoMemoryCleanUp.m_bReservedMemoryClean = false;
				AutoMemoryCleanUp.MemoryClean();
			}
		}
	}
}
