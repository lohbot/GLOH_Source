using System;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomSpline
{
	public struct SubKnot
	{
		public float distanceFromStart;

		public Vector3 position;

		public Vector3 tangent;
	}

	public class Marker
	{
		public int segmentIndex;

		public int subKnotAIndex;

		public int subKnotBIndex;

		public float lerpRatio;
	}

	public const int NbSubSegmentPerSegment = 10;

	private const int MinimumKnotNb = 4;

	private const int FirstSegmentKnotIndex = 2;

	public List<Knot> knots = new List<Knot>();

	public int NbSegments
	{
		get
		{
			return Math.Max(0, this.knots.Count - 3);
		}
	}

	private float Epsilon
	{
		get
		{
			return 0.1f;
		}
	}

	public Vector3 FindPositionFromDistance(float distance)
	{
		Vector3 result = Vector3.zero;
		CatmullRomSpline.Marker marker = new CatmullRomSpline.Marker();
		bool flag = this.PlaceMarker(marker, distance, null);
		if (flag)
		{
			result = this.GetPosition(marker);
		}
		return result;
	}

	public Vector3 FindTangentFromDistance(float distance)
	{
		Vector3 result = Vector3.zero;
		CatmullRomSpline.Marker marker = new CatmullRomSpline.Marker();
		bool flag = this.PlaceMarker(marker, distance, null);
		if (flag)
		{
			result = this.GetTangent(marker);
		}
		return result;
	}

	public static Vector3 ComputeBinormal(Vector3 tangent, Vector3 normal)
	{
		return Vector3.Cross(tangent, normal).normalized;
	}

	public float Length()
	{
		if (this.NbSegments == 0)
		{
			return 0f;
		}
		return Math.Max(0f, this.GetSegmentDistanceFromStart(this.NbSegments - 1));
	}

	public void Clear()
	{
		this.knots.Clear();
	}

	public void MoveMarker(CatmullRomSpline.Marker marker, float distance)
	{
		this.PlaceMarker(marker, distance, marker);
	}

	public Vector3 GetPosition(CatmullRomSpline.Marker marker)
	{
		Vector3 zero = Vector3.zero;
		if (this.NbSegments == 0)
		{
			return zero;
		}
		CatmullRomSpline.SubKnot[] segmentSubKnots = this.GetSegmentSubKnots(marker.segmentIndex);
		return Vector3.Lerp(segmentSubKnots[marker.subKnotAIndex].position, segmentSubKnots[marker.subKnotBIndex].position, marker.lerpRatio);
	}

	public Vector3 GetTangent(CatmullRomSpline.Marker marker)
	{
		Vector3 zero = Vector3.zero;
		if (this.NbSegments == 0)
		{
			return zero;
		}
		CatmullRomSpline.SubKnot[] segmentSubKnots = this.GetSegmentSubKnots(marker.segmentIndex);
		return Vector3.Lerp(segmentSubKnots[marker.subKnotAIndex].tangent, segmentSubKnots[marker.subKnotBIndex].tangent, marker.lerpRatio);
	}

	private CatmullRomSpline.SubKnot[] GetSegmentSubKnots(int i)
	{
		return this.knots[2 + i].subKnots;
	}

	public float GetSegmentDistanceFromStart(int i)
	{
		return this.knots[2 + i].distanceFromStart;
	}

	private bool IsSegmentValid(int i)
	{
		return this.knots[i].distanceFromStart != -1f && this.knots[i + 1].distanceFromStart != -1f && this.knots[i + 2].distanceFromStart != -1f && this.knots[i + 3].distanceFromStart != -1f;
	}

	private bool OutOfBoundSegmentIndex(int i)
	{
		return i < 0 || i >= this.NbSegments;
	}

	public void Parametrize()
	{
		this.Parametrize(0, this.NbSegments - 1);
	}

	public void Parametrize(int fromSegmentIndex, int toSegmentIndex)
	{
		if (this.knots.Count < 4)
		{
			return;
		}
		int num = Math.Min(toSegmentIndex + 1, this.NbSegments);
		fromSegmentIndex = Math.Max(0, fromSegmentIndex);
		float num2 = 0f;
		if (fromSegmentIndex > 0)
		{
			num2 = this.GetSegmentDistanceFromStart(fromSegmentIndex - 1);
		}
		for (int i = fromSegmentIndex; i < num; i++)
		{
			CatmullRomSpline.SubKnot[] segmentSubKnots = this.GetSegmentSubKnots(i);
			for (int j = 0; j < segmentSubKnots.Length; j++)
			{
				CatmullRomSpline.SubKnot subKnot = default(CatmullRomSpline.SubKnot);
				num2 = (subKnot.distanceFromStart = num2 + this.ComputeLengthOfSegment(i, (float)(j - 1) * this.Epsilon, (float)j * this.Epsilon));
				subKnot.position = this.GetPositionOnSegment(i, (float)j * this.Epsilon);
				subKnot.tangent = this.GetTangentOnSegment(i, (float)j * this.Epsilon);
				segmentSubKnots[j] = subKnot;
			}
			this.knots[2 + i].distanceFromStart = num2;
		}
	}

	public bool PlaceMarker(CatmullRomSpline.Marker result, float distance, CatmullRomSpline.Marker from = null)
	{
		int nbSegments = this.NbSegments;
		if (nbSegments == 0)
		{
			return false;
		}
		if (distance <= 0f)
		{
			result.segmentIndex = 0;
			result.subKnotAIndex = 0;
			result.subKnotBIndex = 1;
			result.lerpRatio = 0f;
			return true;
		}
		if (distance >= this.Length())
		{
			CatmullRomSpline.SubKnot[] segmentSubKnots = this.GetSegmentSubKnots(nbSegments - 1);
			result.segmentIndex = nbSegments - 1;
			result.subKnotAIndex = segmentSubKnots.Length - 2;
			result.subKnotBIndex = segmentSubKnots.Length - 1;
			result.lerpRatio = 1f;
			return true;
		}
		int num = 0;
		int num2 = 1;
		if (from != null)
		{
			num = from.segmentIndex;
		}
		for (int i = num; i < nbSegments; i++)
		{
			if (distance <= this.GetSegmentDistanceFromStart(i))
			{
				CatmullRomSpline.SubKnot[] segmentSubKnots = this.GetSegmentSubKnots(i);
				for (int j = num2; j < segmentSubKnots.Length; j++)
				{
					CatmullRomSpline.SubKnot subKnot = segmentSubKnots[j];
					if (distance <= subKnot.distanceFromStart)
					{
						result.segmentIndex = i;
						result.subKnotAIndex = j - 1;
						result.subKnotBIndex = j;
						result.lerpRatio = 1f - (subKnot.distanceFromStart - distance) / (subKnot.distanceFromStart - segmentSubKnots[j - 1].distanceFromStart);
						break;
					}
				}
				break;
			}
		}
		return true;
	}

	private float ComputeLength()
	{
		if (this.knots.Count < 4)
		{
			return 0f;
		}
		float num = 0f;
		int nbSegments = this.NbSegments;
		for (int i = 0; i < nbSegments; i++)
		{
			num += this.ComputeLengthOfSegment(i, 0f, 1f);
		}
		return num;
	}

	private float ComputeLengthOfSegment(int segmentIndex, float from, float to)
	{
		float num = 0f;
		from = Mathf.Clamp01(from);
		to = Mathf.Clamp01(to);
		Vector3 b = this.GetPositionOnSegment(segmentIndex, from);
		for (float num2 = from + this.Epsilon; num2 < to + this.Epsilon / 2f; num2 += this.Epsilon)
		{
			Vector3 positionOnSegment = this.GetPositionOnSegment(segmentIndex, num2);
			num += Vector3.Distance(positionOnSegment, b);
			b = positionOnSegment;
		}
		return num;
	}

	public void DebugDrawEquallySpacedDots()
	{
		Gizmos.color = Color.red;
		int num = 10 * this.NbSegments;
		float num2 = this.Length();
		CatmullRomSpline.Marker marker = new CatmullRomSpline.Marker();
		this.PlaceMarker(marker, 0f, null);
		for (int i = 0; i <= num; i++)
		{
			this.MoveMarker(marker, (float)i * (num2 / (float)num));
			Vector3 position = this.GetPosition(marker);
			Gizmos.DrawWireSphere(position, 0.025f);
		}
	}

	public void DebugDrawSubKnots()
	{
		Gizmos.color = Color.yellow;
		int nbSegments = this.NbSegments;
		for (int i = 0; i < nbSegments; i++)
		{
			CatmullRomSpline.SubKnot[] segmentSubKnots = this.GetSegmentSubKnots(i);
			for (int j = 0; j < segmentSubKnots.Length; j++)
			{
				Gizmos.DrawWireSphere(segmentSubKnots[j].position, 0.025f);
			}
		}
	}

	public void DebugDrawSpline()
	{
		if (this.knots.Count >= 4)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(this.knots[0].position, 0.2f);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.knots[this.knots.Count - 1].position, 0.2f);
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(this.knots[this.knots.Count - 2].position, 0.2f);
			int nbSegments = this.NbSegments;
			for (int i = 0; i < nbSegments; i++)
			{
				Vector3 vector = this.GetPositionOnSegment(i, 0f);
				Gizmos.DrawWireSphere(vector, 0.2f);
				for (float num = this.Epsilon; num < 1f + this.Epsilon / 2f; num += this.Epsilon)
				{
					Vector3 positionOnSegment = this.GetPositionOnSegment(i, num);
					Debug.DrawLine(vector, positionOnSegment, Color.white);
					vector = positionOnSegment;
				}
			}
		}
	}

	private Vector3 GetPositionOnSegment(int segmentIndex, float t)
	{
		return CatmullRomSpline.FindSplinePoint(this.knots[segmentIndex].position, this.knots[segmentIndex + 1].position, this.knots[segmentIndex + 2].position, this.knots[segmentIndex + 3].position, t);
	}

	private Vector3 GetTangentOnSegment(int segmentIndex, float t)
	{
		return CatmullRomSpline.FindSplineTangent(this.knots[segmentIndex].position, this.knots[segmentIndex + 1].position, this.knots[segmentIndex + 2].position, this.knots[segmentIndex + 3].position, t).normalized;
	}

	private static Vector3 FindSplinePoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		Vector3 result = default(Vector3);
		float num = t * t;
		float num2 = num * t;
		result.x = 0.5f * (2f * p1.x + (-p0.x + p2.x) * t + (2f * p0.x - 5f * p1.x + 4f * p2.x - p3.x) * num + (-p0.x + 3f * p1.x - 3f * p2.x + p3.x) * num2);
		result.y = 0.5f * (2f * p1.y + (-p0.y + p2.y) * t + (2f * p0.y - 5f * p1.y + 4f * p2.y - p3.y) * num + (-p0.y + 3f * p1.y - 3f * p2.y + p3.y) * num2);
		result.z = 0.5f * (2f * p1.z + (-p0.z + p2.z) * t + (2f * p0.z - 5f * p1.z + 4f * p2.z - p3.z) * num + (-p0.z + 3f * p1.z - 3f * p2.z + p3.z) * num2);
		return result;
	}

	private static Vector3 FindSplineTangent(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		Vector3 result = default(Vector3);
		float num = t * t;
		result.x = 0.5f * (-p0.x + p2.x) + (2f * p0.x - 5f * p1.x + 4f * p2.x - p3.x) * t + (-p0.x + 3f * p1.x - 3f * p2.x + p3.x) * num * 1.5f;
		result.y = 0.5f * (-p0.y + p2.y) + (2f * p0.y - 5f * p1.y + 4f * p2.y - p3.y) * t + (-p0.y + 3f * p1.y - 3f * p2.y + p3.y) * num * 1.5f;
		result.z = 0.5f * (-p0.z + p2.z) + (2f * p0.z - 5f * p1.z + 4f * p2.z - p3.z) * t + (-p0.z + 3f * p1.z - 3f * p2.z + p3.z) * num * 1.5f;
		return result;
	}
}
