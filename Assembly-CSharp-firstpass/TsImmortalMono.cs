using System;
using UnityEngine;

public class TsImmortalMono : MonoBehaviour
{
	private void Awake()
	{
		if (base.gameObject == TsImmortal.gameObject)
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
		else
		{
			TsLog.LogError("TsImmortalMono component allowed only one instance! this component is in " + base.gameObject.name, new object[0]);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
