using System;
using UnityEngine;

public class PositionConstraint : MonoBehaviour
{
	public Transform[] targetNodes;

	public bool keepInitialOffset;

	public bool xAxis = true;

	public bool yAxis = true;

	public bool zAxis = true;

	private Vector3 offsetPos;

	private Vector3 GetCenterPos(Transform[] nodes)
	{
		Vector3 vector = Vector3.zero;
		float num = 0f;
		for (int i = 0; i < nodes.Length; i++)
		{
			Transform transform = nodes[i];
			if (transform != null)
			{
				num += 1f;
				vector += transform.position;
			}
		}
		if (num == 0f)
		{
			return vector;
		}
		return vector / num;
	}

	private int GetNodeCount(Transform[] nodes)
	{
		int num = 0;
		for (int i = 0; i < nodes.Length; i++)
		{
			Transform x = nodes[i];
			if (x != null)
			{
				num++;
			}
		}
		return num;
	}

	private Vector3 GetOffsetPos()
	{
		Vector3 centerPos = this.GetCenterPos(this.targetNodes);
		this.offsetPos = base.transform.position - centerPos;
		return this.offsetPos;
	}

	private void Start()
	{
		this.offsetPos = this.GetOffsetPos();
	}

	private void Update()
	{
		if (this.GetNodeCount(this.targetNodes) == 0)
		{
			return;
		}
		Vector3 a = this.GetCenterPos(this.targetNodes);
		if (this.keepInitialOffset)
		{
			a += this.offsetPos;
		}
		float x = (!this.xAxis) ? base.transform.position.x : a.x;
		float y = (!this.yAxis) ? base.transform.position.y : a.y;
		float z = (!this.zAxis) ? base.transform.position.z : a.z;
		base.transform.position = new Vector3(x, y, z);
	}
}
