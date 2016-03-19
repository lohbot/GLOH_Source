using System;
using UnityEngine;

public class NrQuestCamera
{
	private NrCharBase m_kCharBase;

	private Transform m_kQuestCamera;

	private bool m_bUpdate = true;

	public void SetQuestCamera(NrCharBase pkCharBase, Transform pkCamera)
	{
		this.m_kCharBase = pkCharBase;
		this.m_kQuestCamera = pkCamera;
		this.m_bUpdate = true;
		this.Update();
	}

	public void Release()
	{
		this.m_kCharBase = null;
		this.m_kQuestCamera = null;
	}

	public void SetACtive(bool bActive)
	{
		this.m_bUpdate = bActive;
	}

	public void Update()
	{
		if (this.m_bUpdate && this.m_kCharBase != null && null != this.m_kQuestCamera && null != Camera.main)
		{
			Vector3 position = this.m_kQuestCamera.position;
			Vector3 cameraPosition = this.m_kCharBase.GetCameraPosition();
			cameraPosition.y = position.y;
			Camera.main.transform.localPosition = position;
			Camera.main.transform.LookAt(cameraPosition);
		}
	}
}
