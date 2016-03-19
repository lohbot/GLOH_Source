using System;
using UnityEngine;

public class NrLogInfo
{
	public string m_str = string.Empty;

	public Time m_kTime;

	public float m_fPosY;

	public bool IsRemove()
	{
		return this.m_fPosY <= -22f;
	}
}
