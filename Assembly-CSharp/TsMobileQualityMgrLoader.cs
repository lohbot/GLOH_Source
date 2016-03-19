using System;
using UnityEngine;

public class TsMobileQualityMgrLoader : MonoBehaviour
{
	private void OnEnable()
	{
		TsQualityManager.Instance.Refresh();
	}
}
