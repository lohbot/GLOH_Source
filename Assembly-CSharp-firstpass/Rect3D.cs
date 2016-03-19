using System;
using UnityEngine;

public struct Rect3D
{
	private Vector3 m_tl;

	private Vector3 m_tr;

	private Vector3 m_bl;

	private Vector3 m_br;

	private float m_width;

	private float m_height;

	public Vector3 topLeft
	{
		get
		{
			return this.m_tl;
		}
	}

	public Vector3 topRight
	{
		get
		{
			return this.m_tr;
		}
	}

	public Vector3 bottomLeft
	{
		get
		{
			return this.m_bl;
		}
	}

	public Vector3 bottomRight
	{
		get
		{
			return this.m_br;
		}
	}

	public float width
	{
		get
		{
			if (float.IsNaN(this.m_width))
			{
				this.m_width = Vector3.Distance(this.m_tr, this.m_tl);
			}
			return this.m_width;
		}
	}

	public float height
	{
		get
		{
			if (float.IsNaN(this.m_height))
			{
				this.m_height = Vector3.Distance(this.m_tl, this.m_bl);
			}
			return this.m_height;
		}
	}

	public Rect3D(Vector3 tl, Vector3 tr, Vector3 bl)
	{
		this.m_tl = (this.m_tr = (this.m_bl = (this.m_br = Vector3.zero)));
		this.m_width = (this.m_height = 0f);
		this.FromPoints(tl, tr, bl);
	}

	public Rect3D(Rect r)
	{
		this.m_tl = (this.m_tr = (this.m_bl = (this.m_br = Vector3.zero)));
		this.m_width = (this.m_height = 0f);
		this.FromRect(r);
	}

	public void FromPoints(Vector3 tl, Vector3 tr, Vector3 bl)
	{
		this.m_tl = tl;
		this.m_tr = tr;
		this.m_bl = bl;
		this.m_br = tr + (bl - tl);
		this.m_width = (this.m_height = float.NaN);
	}

	public Rect GetRect()
	{
		return Rect.MinMaxRect(this.m_bl.x, this.m_bl.y, this.m_tr.x, this.m_tl.y);
	}

	public void FromRect(Rect r)
	{
		this.FromPoints(new Vector3(r.xMin, r.yMax), new Vector3(r.xMax, r.yMax), new Vector3(r.xMin, r.yMin));
	}

	public void MultFast(Matrix4x4 matrix)
	{
		this.m_tl = matrix.MultiplyPoint3x4(this.m_tl);
		this.m_tr = matrix.MultiplyPoint3x4(this.m_tr);
		this.m_bl = matrix.MultiplyPoint3x4(this.m_bl);
		this.m_br = matrix.MultiplyPoint3x4(this.m_br);
		this.m_width = (this.m_height = float.NaN);
	}

	public static Rect3D MultFast(Rect3D rect, Matrix4x4 matrix)
	{
		return new Rect3D(matrix.MultiplyPoint3x4(rect.m_tl), matrix.MultiplyPoint3x4(rect.m_tr), matrix.MultiplyPoint3x4(rect.m_bl));
	}
}
