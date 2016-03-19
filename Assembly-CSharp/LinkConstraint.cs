using System;
using UnityEngine;

public class LinkConstraint : MonoBehaviour
{
	public Transform node;

	private Matrix4x4 offsetMtrix = Matrix4x4.zero;

	private Quaternion GetRotation(Matrix4x4 matrix)
	{
		float num = Mathf.Sqrt(1f + matrix.m00 + matrix.m11 + matrix.m22) / 2f;
		float num2 = 4f * num;
		float x = (matrix.m21 - matrix.m12) / num2;
		float y = (matrix.m02 - matrix.m20) / num2;
		float z = (matrix.m10 - matrix.m01) / num2;
		return new Quaternion(x, y, z, num);
	}

	private Vector3 GetPosition(Matrix4x4 matrix)
	{
		float m = matrix.m03;
		float m2 = matrix.m13;
		float m3 = matrix.m23;
		return new Vector3(m, m2, m3);
	}

	private Vector3 GetScale(Matrix4x4 m)
	{
		float x = Mathf.Sqrt(m.m00 * m.m00 + m.m01 * m.m01 + m.m02 * m.m02);
		float y = Mathf.Sqrt(m.m10 * m.m10 + m.m11 * m.m11 + m.m12 * m.m12);
		float z = Mathf.Sqrt(m.m20 * m.m20 + m.m21 * m.m21 + m.m22 * m.m22);
		return new Vector3(x, y, z);
	}

	private void FromMatrix4x4(Transform transform, Matrix4x4 matrix)
	{
		transform.localScale = this.GetScale(matrix);
		transform.rotation = this.GetRotation(matrix);
		transform.position = this.GetPosition(matrix);
	}

	private void Start()
	{
		if (this.node != null)
		{
			this.offsetMtrix = this.node.localToWorldMatrix * base.transform.worldToLocalMatrix.inverse;
			this.FromMatrix4x4(base.transform, this.offsetMtrix);
		}
	}

	private void Update()
	{
		if (this.node != null)
		{
		}
	}
}
