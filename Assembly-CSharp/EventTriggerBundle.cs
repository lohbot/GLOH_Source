using System;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerBundle : MonoBehaviour
{
	public float m_CompletePrecent;

	public float m_DownPercent;

	private float m_CompleteOnePercent;

	public List<EventTriggerBundleInfo> m_BundleInfoList = new List<EventTriggerBundleInfo>();

	public bool m_LoadComplete
	{
		get
		{
			return this.m_CompletePrecent >= 100f;
		}
	}

	public bool m_DownComplete
	{
		get
		{
			return this.m_DownPercent >= 100f;
		}
	}

	private void Start()
	{
		if (this.m_BundleInfoList.Count > 0)
		{
			this.m_CompleteOnePercent = 100f / (float)this.m_BundleInfoList.Count;
		}
		this.CreateBundle();
	}

	private void Update()
	{
		if (this.m_LoadComplete)
		{
			return;
		}
		this.m_CompletePrecent = 0f;
		this.m_DownPercent = 0f;
		int num = 0;
		int num2 = 0;
		foreach (EventTriggerBundleInfo current in this.m_BundleInfoList)
		{
			if (current.m_LoadComplete)
			{
				this.m_CompletePrecent += this.m_CompleteOnePercent;
				num2++;
			}
			if (current.m_DownComplete)
			{
				this.m_DownPercent += this.m_CompleteOnePercent;
				num++;
			}
		}
		if (num2 == this.m_BundleInfoList.Count)
		{
			this.m_CompletePrecent = 100f;
		}
		if (num == this.m_BundleInfoList.Count)
		{
			this.m_DownPercent = 100f;
		}
	}

	public void CreateBundle()
	{
		foreach (EventTriggerBundleInfo current in this.m_BundleInfoList)
		{
			current.CreateBundle();
		}
	}

	public UnityEngine.Object GetObject(string path)
	{
		foreach (EventTriggerBundleInfo current in this.m_BundleInfoList)
		{
			if (current.IsKey(path))
			{
				return current.GetObject();
			}
		}
		return null;
	}
}
