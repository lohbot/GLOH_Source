using System;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
	private Transform DestMove;

	private float fMoveTime;

	private float fStartTime;

	private Vector3 v1 = Vector3.zero;

	private Vector3 v2 = Vector3.zero;

	private Vector3 vMoveDir = Vector3.zero;

	private float MoveDistance;

	public void Set(Transform Dest, float MoveTime)
	{
		this.DestMove = Dest;
		this.fMoveTime = MoveTime;
		this.fStartTime = Time.time;
		this.v1 = new Vector3(base.gameObject.transform.localPosition.x, base.gameObject.transform.localPosition.y, base.gameObject.transform.localPosition.z);
		this.v2 = new Vector3(this.DestMove.localPosition.x, this.DestMove.localPosition.y, this.DestMove.localPosition.z);
		this.vMoveDir = this.v2 - this.v1;
		this.vMoveDir.Normalize();
		this.MoveDistance = Vector3.Distance(this.v1, this.v2);
		base.gameObject.transform.localPosition = this.v1;
	}

	private void FixedUpdate()
	{
		if (Math.Abs(this.fStartTime - Time.time) >= this.fMoveTime)
		{
			base.gameObject.transform.localPosition = this.v2;
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			base.gameObject.transform.localPosition += this.vMoveDir * (this.MoveDistance * (Time.deltaTime / this.fMoveTime));
		}
	}
}
