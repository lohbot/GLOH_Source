using System;
using System.Collections.Generic;
using UnityEngine;

public class NmActivater : MonoBehaviour
{
	public List<ACTSource> mList = new List<ACTSource>();

	public static NmActivater GetActivater(GameObject go)
	{
		NmActivater nmActivater = go.GetComponent<NmActivater>();
		if (null == nmActivater)
		{
			nmActivater = go.AddComponent<NmActivater>();
		}
		return nmActivater;
	}

	private void SetInitilize(Transform kTrans)
	{
		GameObject gameObject = kTrans.gameObject;
		if (null != gameObject)
		{
			Renderer[] components = gameObject.GetComponents<Renderer>();
			Renderer[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				Renderer src = array[i];
				ACTSource_Render item = new ACTSource_Render(src);
				this.mList.Add(item);
			}
		}
	}

	private void SetActiveTarget(Transform kTrans, bool bActive)
	{
		if (!bActive)
		{
			this.SetInitilize(kTrans);
		}
		foreach (ACTSource current in this.mList)
		{
			current.SetActive(bActive);
		}
		if (bActive)
		{
			this.mList.Clear();
			UnityEngine.Object.Destroy(this);
		}
	}

	public void SetActive(bool bActive)
	{
		this.ShowHideRecursively(base.transform, bActive, false);
	}

	public void SetActiveRecursively(bool bActive)
	{
		this.ShowHideRecursively(base.transform, bActive, true);
	}

	private void ShowHideRecursively(Transform kTrans, bool bActive, bool bDeep)
	{
		if (bDeep)
		{
			for (int i = 0; i < kTrans.childCount; i++)
			{
				this.ShowHideRecursively(kTrans.GetChild(i), bActive, bDeep);
			}
		}
		this.SetActiveTarget(kTrans, bActive);
	}
}
