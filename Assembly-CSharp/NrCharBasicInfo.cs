using System;
using UnityEngine;

public class NrCharBasicInfo
{
	public long m_nPersonID;

	public string m_szCharName = string.Empty;

	public long m_nSolID;

	public Vector3 m_vCharPos = Vector3.zero;

	public Vector3 m_vDirection = Vector3.zero;

	public float m_fMoveSpeed;

	public NrCharBasicInfo()
	{
		this.m_vCharPos = Vector3.zero;
		this.m_vDirection = Vector3.zero;
		this.Init();
	}

	public void Init()
	{
		this.m_nPersonID = 0L;
		this.m_szCharName = string.Empty;
		this.m_nSolID = 1L;
		this.m_vCharPos = Vector3.zero;
		this.m_vDirection = Vector3.zero;
	}

	public void Set(NrCharBasicInfo pkBasicInfo)
	{
		this.m_nPersonID = pkBasicInfo.m_nPersonID;
		this.m_szCharName = pkBasicInfo.m_szCharName;
		this.m_nSolID = pkBasicInfo.m_nSolID;
		this.m_vCharPos = pkBasicInfo.m_vCharPos;
		this.m_vDirection = pkBasicInfo.m_vDirection;
	}

	public void SetCharPos(float x, float y, float z)
	{
		this.m_vCharPos.x = x;
		this.m_vCharPos.y = y;
		this.m_vCharPos.z = z;
	}

	public void SetDirection(float x, float y, float z)
	{
		this.m_vDirection.x = x;
		this.m_vDirection.y = y;
		this.m_vDirection.z = z;
	}
}
