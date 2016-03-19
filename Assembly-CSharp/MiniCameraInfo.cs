using System;
using UnityEngine;

[Serializable]
public class MiniCameraInfo
{
	public Vector3 m_Position = Vector3.zero;

	public Vector3 m_EulerAngles = Vector3.zero;

	public float m_fieldOfView = -1f;

	public void CopyCamera(Camera camera)
	{
		this.m_Position = camera.transform.localPosition;
		this.m_EulerAngles = camera.transform.localEulerAngles;
		this.m_fieldOfView = camera.fieldOfView;
	}
}
