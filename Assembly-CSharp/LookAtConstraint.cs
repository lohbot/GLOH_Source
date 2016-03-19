using System;
using System.Collections.Generic;
using UnityEngine;

public class LookAtConstraint : MonoBehaviour
{
	public enum lookType
	{
		Camera,
		Nodes
	}

	public enum upType
	{
		Camera,
		Node,
		World
	}

	public enum axisType
	{
		X,
		Y,
		Z
	}

	public enum upCtrType
	{
		LootAt,
		AxisAlignment
	}

	public LookAtConstraint.lookType lookAtType;

	public List<Transform> lookAtNodeList = new List<Transform>();

	public LookAtConstraint.axisType lookAtAxis = LookAtConstraint.axisType.Z;

	public bool lookAtFilp;

	public LookAtConstraint.upType upAxisType = LookAtConstraint.upType.World;

	public Transform upNode;

	public LookAtConstraint.upCtrType upControl = LookAtConstraint.upCtrType.AxisAlignment;

	public LookAtConstraint.axisType sourceAxis = LookAtConstraint.axisType.Y;

	public bool sourceFilp;

	public LookAtConstraint.axisType alignedToUpnodeAxis = LookAtConstraint.axisType.Y;

	private Vector3 GetUpVector()
	{
		Vector3 result = Vector3.zero;
		switch (this.upAxisType)
		{
		case LookAtConstraint.upType.Camera:
			if (Camera.main != null)
			{
				result = Camera.main.transform.position - base.transform.position;
			}
			break;
		case LookAtConstraint.upType.Node:
			if (this.upControl == LookAtConstraint.upCtrType.LootAt)
			{
				result = this.upNode.transform.position - base.transform.position;
			}
			else if (!(this.upNode == null))
			{
				switch (this.alignedToUpnodeAxis)
				{
				case LookAtConstraint.axisType.X:
					result = this.upNode.right;
					break;
				case LookAtConstraint.axisType.Y:
					result = this.upNode.up;
					break;
				case LookAtConstraint.axisType.Z:
					result = this.upNode.forward;
					break;
				}
			}
			break;
		case LookAtConstraint.upType.World:
			switch (this.alignedToUpnodeAxis)
			{
			case LookAtConstraint.axisType.X:
				result = Vector3.right;
				break;
			case LookAtConstraint.axisType.Y:
				result = Vector3.up;
				break;
			case LookAtConstraint.axisType.Z:
				result = Vector3.forward;
				break;
			}
			break;
		}
		return result;
	}

	private void LookAtQuat(Vector3 xvec, Vector3 yvec, Vector3 zvec)
	{
		float num = 1f + xvec.x + yvec.y + zvec.z;
		if (num == 0f)
		{
			return;
		}
		float num2 = Mathf.Sqrt(num) / 2f;
		float num3 = 4f * num2;
		float x = (yvec.z - zvec.y) / num3;
		float y = (zvec.x - xvec.z) / num3;
		float z = (xvec.y - yvec.x) / num3;
		Quaternion rotation = new Quaternion(x, y, z, num2);
		base.transform.rotation = rotation;
	}

	private void Update()
	{
		Vector3 upVector = this.GetUpVector();
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		Vector3 vector3 = Vector3.zero;
		LookAtConstraint.lookType lookType = this.lookAtType;
		if (lookType != LookAtConstraint.lookType.Camera)
		{
			if (lookType == LookAtConstraint.lookType.Nodes)
			{
				Vector3 a = Vector3.zero;
				int num = 0;
				foreach (Transform current in this.lookAtNodeList)
				{
					if (current != null)
					{
						num++;
						a += current.position;
					}
				}
				a /= (float)num;
				vector = Vector3.Normalize(a - base.transform.position);
				vector2 = Vector3.Normalize(Vector3.Cross(upVector, vector));
				vector3 = Vector3.Cross(vector, vector2);
			}
		}
		else
		{
			if (Camera.main == null)
			{
				return;
			}
			vector = Vector3.Normalize(Camera.main.transform.position - base.transform.position);
			vector2 = Vector3.Normalize(Vector3.Cross(upVector, vector));
			vector3 = Vector3.Cross(vector, vector2);
		}
		Vector3 vector4 = Vector3.zero;
		Vector3 vector5 = Vector3.zero;
		Vector3 vector6 = Vector3.zero;
		if (this.lookAtFilp)
		{
			vector = -vector;
			vector2 = -vector2;
		}
		switch (this.lookAtAxis)
		{
		case LookAtConstraint.axisType.X:
			vector4 = vector;
			if (this.sourceAxis == LookAtConstraint.axisType.Y)
			{
				vector5 = vector3;
				vector6 = -vector2;
			}
			else if (this.sourceAxis == LookAtConstraint.axisType.Z)
			{
				vector5 = vector2;
				vector6 = vector3;
			}
			if (this.sourceFilp)
			{
				vector5 = -vector5;
				vector6 = -vector6;
			}
			break;
		case LookAtConstraint.axisType.Y:
			vector5 = vector;
			if (this.sourceAxis == LookAtConstraint.axisType.X)
			{
				vector4 = vector3;
				vector6 = vector2;
			}
			else if (this.sourceAxis == LookAtConstraint.axisType.Z)
			{
				vector4 = -vector2;
				vector6 = vector3;
			}
			if (this.sourceFilp)
			{
				vector4 = -vector4;
				vector6 = -vector6;
			}
			break;
		case LookAtConstraint.axisType.Z:
			vector6 = vector;
			if (this.sourceAxis == LookAtConstraint.axisType.X)
			{
				vector4 = vector3;
				vector5 = -vector2;
			}
			else if (this.sourceAxis == LookAtConstraint.axisType.Y)
			{
				vector4 = vector2;
				vector5 = vector3;
			}
			if (this.sourceFilp)
			{
				vector4 = -vector4;
				vector5 = -vector5;
			}
			break;
		}
		this.LookAtQuat(vector4, vector5, vector6);
	}
}
