using System;
using UnityEngine;

public class FX_OrientationConstraint : MonoBehaviour
{
	private Transform targetCamera;

	private void Start()
	{
		GameObject gameObject = GameObject.FindWithTag("MainCamera");
		if (gameObject != null)
		{
			this.targetCamera = gameObject.transform;
		}
	}

	private void Update()
	{
		if (this.targetCamera == null)
		{
			GameObject gameObject = GameObject.FindWithTag("MainCamera");
			if (gameObject != null)
			{
				this.targetCamera = gameObject.transform;
			}
		}
		else
		{
			base.transform.rotation = this.targetCamera.rotation;
		}
	}
}
