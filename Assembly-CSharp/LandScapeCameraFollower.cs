using System;
using UnityEngine;

public class LandScapeCameraFollower : MonoBehaviour
{
	[SerializeField]
	public Transform targetCamera;

	public Vector3 veScale = Vector3.zero;

	public float fHeight;

	private float fPosY;

	private float LastUpdateFar;

	private void Start()
	{
		this.FindMainCamera();
	}

	private void Update()
	{
		if (this.targetCamera == null)
		{
			this.FindMainCamera();
		}
		else
		{
			if (this.targetCamera.camera.farClipPlane != this.LastUpdateFar && this.veScale != Vector3.zero)
			{
				base.transform.localScale = this.veScale * (this.targetCamera.camera.farClipPlane / 600f);
				this.fPosY = this.fHeight + this.fHeight * (1f - this.targetCamera.camera.farClipPlane / 600f);
				this.LastUpdateFar = this.targetCamera.camera.farClipPlane;
			}
			Vector3 position = this.targetCamera.transform.position;
			position.y = this.fPosY;
			base.transform.position = position;
		}
	}

	private void FindMainCamera()
	{
		if (this.targetCamera == null)
		{
			this.targetCamera = GameObject.FindWithTag("MainCamera").transform;
		}
	}
}
