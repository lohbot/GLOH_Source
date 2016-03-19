using System;
using UnityEngine;

public static class TsImmortal
{
	private static GameObject m_immortalGO;

	public static GameObject gameObject
	{
		get
		{
			if (null != TsImmortal.m_immortalGO)
			{
				return TsImmortal.m_immortalGO;
			}
			int num = TsImmortal._FindImmortalGO(ref TsImmortal.m_immortalGO);
			if (1 < num)
			{
				TsLog.LogWarning(string.Concat(new object[]
				{
					"You have ",
					num,
					" ",
					TsImmortal.immortalName,
					" components in runtime. Allow only on instance in the game."
				}), new object[0]);
			}
			if (null == TsImmortal.m_immortalGO)
			{
				TsImmortal.m_immortalGO = new GameObject(TsImmortal.immortalName);
				TsImmortal.m_immortalGO.AddComponent<TsImmortalMono>();
				TsImmortal.m_immortalGO.AddComponent<TsItweenTimer>();
			}
			TsImmortal.m_immortalGO.isStatic = true;
			return TsImmortal.m_immortalGO;
		}
	}

	private static string immortalName
	{
		get
		{
			return "__ImmortalGameService__";
		}
	}

	public static TsBundleService bundleService
	{
		get
		{
			TsBundleService tsBundleService = TsImmortal.gameObject.GetComponent<TsBundleService>();
			if (null == tsBundleService)
			{
				TsImmortal.bundleDbgPrint.enabled = false;
				tsBundleService = TsImmortal.gameObject.AddComponent<TsBundleService>();
			}
			return tsBundleService;
		}
	}

	public static TsBundleDbgPrint bundleDbgPrint
	{
		get
		{
			TsBundleDbgPrint tsBundleDbgPrint = TsImmortal.gameObject.GetComponent<TsBundleDbgPrint>();
			if (null == tsBundleDbgPrint)
			{
				tsBundleDbgPrint = TsImmortal.gameObject.AddComponent<TsBundleDbgPrint>();
			}
			return tsBundleDbgPrint;
		}
	}

	private static int _FindImmortalGO(ref GameObject goImm)
	{
		int num = 0;
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		UnityEngine.Object[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			UnityEngine.Object @object = array2[i];
			GameObject gameObject = @object as GameObject;
			TsImmortalMono component = gameObject.GetComponent<TsImmortalMono>();
			if (null != component)
			{
				goImm = gameObject;
				num++;
			}
		}
		return num;
	}

	public static TsBundleService WakeUpBundleService()
	{
		return TsImmortal.bundleService;
	}
}
