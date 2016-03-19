using System;
using UnityEngine;

[Serializable]
public class BendingSegment
{
	public Transform firstTransform;

	public Transform lastTransform;

	public float thresholdAngleDifference;

	public float bendingMultiplier = 0.6f;

	public float maxAngleDifference = 30f;

	public float maxBendingAngle = 80f;

	public float responsiveness = 5f;

	public float angleH;

	public float angleV;

	public Vector3 dirUp;

	public Vector3 referenceLookDir;

	public Vector3 referenceUpDir;

	public int chainLength;

	public Quaternion[] origRotations;
}
