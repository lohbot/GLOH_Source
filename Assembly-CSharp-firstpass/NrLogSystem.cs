using System;
using System.Collections.Generic;
using UnityEngine;

public class NrLogSystem : NrTSingleton<NrLogSystem>
{
	private List<NrLogInfo> m_arLogInfo;

	private float m_fLastPos;

	private bool m_bDebugMode;

	private GUIStyle m_LabelStyle;

	private NrLogSystem()
	{
		this.m_arLogInfo = new List<NrLogInfo>();
		this.m_LabelStyle = new GUIStyle();
		this.m_LabelStyle.normal.textColor = new Color(0f, 128f, 255f);
	}

	public void SetDebugMode(bool bDebug)
	{
		this.m_bDebugMode = bDebug;
	}

	public bool GetDebugMode()
	{
		return this.m_bDebugMode;
	}

	public void Update()
	{
		this.m_fLastPos = 0f;
		foreach (NrLogInfo current in this.m_arLogInfo)
		{
			current.m_fPosY -= Time.deltaTime * 10f;
			if (this.m_fLastPos < current.m_fPosY)
			{
				this.m_fLastPos = current.m_fPosY;
			}
		}
		this.m_arLogInfo.RemoveAll((NrLogInfo log) => log.IsRemove());
	}

	public void AddString(string str)
	{
		this.AddString(str, false);
	}

	public void AddString(string str, bool bAlways)
	{
		if (!this.m_bDebugMode && !bAlways)
		{
			return;
		}
		if (this.m_fLastPos > (float)Screen.height - 22f)
		{
			foreach (NrLogInfo current in this.m_arLogInfo)
			{
				current.m_fPosY -= 22f;
			}
		}
		NrLogInfo nrLogInfo = new NrLogInfo();
		nrLogInfo.m_str = "System Log>> " + str;
		nrLogInfo.m_fPosY = (float)Screen.height;
		this.m_arLogInfo.Add(nrLogInfo);
		Debug.Log(nrLogInfo.m_str);
		this.m_fLastPos = nrLogInfo.m_fPosY;
	}
}
