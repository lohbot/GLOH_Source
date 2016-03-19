using System;
using UnityEngine;

public class FX_BB_PlaneBillboard : MonoBehaviour
{
	private Transform _mainCameraTransf;

	private void Start()
	{
		if (Camera.main != null)
		{
			this._mainCameraTransf = Camera.main.transform;
		}
		else
		{
			GameObject gameObject = GameObject.FindWithTag("MainCamera");
			if (gameObject != null)
			{
				this._mainCameraTransf = gameObject.transform;
			}
		}
	}

	private void Update()
	{
		if (this._mainCameraTransf != null)
		{
			Vector3 vector = this._mainCameraTransf.transform.position;
			vector = base.transform.position - (vector - base.transform.position);
			base.transform.LookAt(vector);
		}
	}
}
