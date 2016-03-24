using System;
using UnityEngine;

[Serializable]
public class Knot
{
	public float distanceFromStart = -1f;

	public CatmullRomSpline.SubKnot[] subKnots = new CatmullRomSpline.SubKnot[11];

	public Vector3 position;

	public Knot(Vector3 position)
	{
		this.position = position;
	}

	public void Invalidate()
	{
		this.distanceFromStart = -1f;
	}
}
