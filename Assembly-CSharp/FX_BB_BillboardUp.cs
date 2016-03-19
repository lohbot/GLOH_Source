using System;
using UnityEngine;

public class FX_BB_BillboardUp : MonoBehaviour
{
	[SerializeField]
	public Transform targetCamera;

	private void Start()
	{
		if (this.targetCamera == null)
		{
			GameObject gameObject = GameObject.FindWithTag("MainCamera");
			if (gameObject != null)
			{
				this.targetCamera = gameObject.transform;
			}
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
			base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, this.targetCamera.transform.eulerAngles.y, base.transform.eulerAngles.z);
		}
	}
}
