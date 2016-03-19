using System;
using UnityEngine;

public class CharacterRotate : MonoBehaviour
{
	private Transform DestRotate;

	private float fStartTime;

	private float fRotateTime;

	private Quaternion q1 = Quaternion.identity;

	private Quaternion q2 = Quaternion.identity;

	public void Set(Transform Dest, float RotateTime)
	{
		this.DestRotate = Dest;
		this.fRotateTime = RotateTime;
		this.fStartTime = Time.time;
		this.q1 = new Quaternion(base.gameObject.transform.localRotation.x, base.gameObject.transform.localRotation.y, base.gameObject.transform.localRotation.z, base.gameObject.transform.localRotation.w);
		this.q2 = Quaternion.LookRotation(this.DestRotate.forward);
		this.q2.x = this.q1.x;
		this.q2.z = this.q1.z;
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
