using System;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
	private float fStartTime;

	private float fRotateTime;

	private Quaternion q1 = Quaternion.identity;

	private Quaternion q2 = Quaternion.identity;

	public void Set(Transform Dest, float RotateTime)
	{
		this.fRotateTime = RotateTime;
		this.fStartTime = Time.time;
		this.q1 = new Quaternion(base.gameObject.transform.localRotation.x, base.gameObject.transform.localRotation.y, base.gameObject.transform.localRotation.z, base.gameObject.transform.localRotation.w);
		this.q2 = new Quaternion(Dest.localRotation.x, Dest.localRotation.y, Dest.localRotation.z, Dest.localRotation.w);
	}

	private void FixedUpdate()
	{
		if (Math.Abs(this.fStartTime - Time.time) >= this.fRotateTime)
		{
			base.gameObject.transform.localRotation = this.q2;
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			float t = (Time.time - this.fStartTime) / this.fRotateTime;
			base.gameObject.transform.localRotation = Quaternion.Lerp(this.q1, this.q2, t);
		}
	}
}
