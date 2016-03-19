using System;
using UnityEngine;

public class FX_BB_MakeBillboard : MonoBehaviour
{
	[SerializeField]
	public Transform target;

	private void Start()
	{
		if (this.target == null)
		{
			GameObject gameObject = GameObject.FindWithTag("MainCamera");
			if (gameObject != null)
			{
				this.target = gameObject.transform;
			}
		}
	}

	private void Update()
	{
		if (this.target != null)
		{
			base.transform.LookAt(this.target);
		}
		else
		{
			GameObject gameObject = GameObject.FindWithTag("MainCamera");
			if (gameObject != null)
			{
				this.target = gameObject.transform;
			}
		}
	}
}
