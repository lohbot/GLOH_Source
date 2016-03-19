using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
	public Vector3 headLookVector = Vector3.forward;

	public Vector3 headUpVector = Vector3.up;

	public Transform rootNode;

	public Transform targetTransform;

	public bool overrideAnimation;

	public float effect = 1f;

	public List<BendingSegment> segments;

	public List<NonAffectedJoints> nonAffectedJoints;

	public Vector3 target = Vector3.zero;

	public void SetHeadLookVector(Vector3 vec)
	{
		this.headLookVector = vec;
	}

	public void SetHeadUpVector(Vector3 vec)
	{
		this.headUpVector = vec;
	}

	public void SetTarget(GameObject obj)
	{
		this.targetTransform = obj.transform;
	}

	public BendingSegment GetSegment(int idx)
	{
		return this.segments[idx];
	}

	public void AddSegment(BendingSegment Segment)
	{
		if (this.segments == null)
		{
			this.segments = new List<BendingSegment>();
		}
		this.segments.Add(Segment);
	}

	private void Start()
	{
		this.rootNode = base.gameObject.transform.Find("bone01 pelvis");
		if (this.segments == null)
		{
			return;
		}
		this.nonAffectedJoints = new List<NonAffectedJoints>();
		if (this.rootNode == null)
		{
			this.rootNode = base.gameObject.transform;
		}
		foreach (BendingSegment current in this.segments)
		{
			Quaternion rotation = current.firstTransform.parent.rotation;
			Quaternion lhs = Quaternion.Inverse(rotation);
			current.referenceLookDir = lhs * this.rootNode.rotation * this.headLookVector.normalized;
			current.referenceUpDir = lhs * this.rootNode.rotation * this.headUpVector.normalized;
			current.angleH = 0f;
			current.angleV = 0f;
			current.dirUp = current.referenceUpDir;
			current.chainLength = 1;
			Transform transform = current.lastTransform;
			while (transform != current.firstTransform && transform != transform.root)
			{
				current.chainLength++;
				transform = transform.parent;
			}
			current.origRotations = new Quaternion[current.chainLength];
			transform = current.lastTransform;
			for (int i = current.chainLength - 1; i >= 0; i--)
			{
				current.origRotations[i] = transform.localRotation;
				transform = transform.parent;
			}
		}
	}

	private void LateUpdate()
	{
		if (Time.deltaTime == 0f)
		{
			return;
		}
		if (this.targetTransform == null)
		{
			return;
		}
		this.target = this.targetTransform.position;
		Vector3[] array = new Vector3[this.nonAffectedJoints.Count];
		for (int i = 0; i < this.nonAffectedJoints.Count; i++)
		{
			IEnumerator enumerator = this.nonAffectedJoints[i].joint.GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					array[i] = transform.position - this.nonAffectedJoints[i].joint.position;
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
		foreach (BendingSegment current in this.segments)
		{
			Transform transform2 = current.lastTransform;
			if (this.overrideAnimation)
			{
				for (int j = current.chainLength - 1; j >= 0; j--)
				{
					transform2.localRotation = current.origRotations[j];
					transform2 = transform2.parent;
				}
			}
			Quaternion rotation = current.firstTransform.parent.rotation;
			Quaternion rotation2 = Quaternion.Inverse(rotation);
			Vector3 normalized = (this.target - current.lastTransform.position).normalized;
			Vector3 vector = rotation2 * normalized;
			float num = this.AngleAroundAxis(current.referenceLookDir, vector, current.referenceUpDir);
			Vector3 axis = Vector3.Cross(current.referenceUpDir, vector);
			Vector3 dirA = vector - Vector3.Project(vector, current.referenceUpDir);
			float num2 = this.AngleAroundAxis(dirA, vector, axis);
			float f = Mathf.Max(0f, Mathf.Abs(num) - current.thresholdAngleDifference) * Mathf.Sign(num);
			float f2 = Mathf.Max(0f, Mathf.Abs(num2) - current.thresholdAngleDifference) * Mathf.Sign(num2);
			num = Mathf.Max(Mathf.Abs(f) * Mathf.Abs(current.bendingMultiplier), Mathf.Abs(num) - current.maxAngleDifference) * Mathf.Sign(num) * Mathf.Sign(current.bendingMultiplier);
			num2 = Mathf.Max(Mathf.Abs(f2) * Mathf.Abs(current.bendingMultiplier), Mathf.Abs(num2) - current.maxAngleDifference) * Mathf.Sign(num2) * Mathf.Sign(current.bendingMultiplier);
			num = Mathf.Clamp(num, -current.maxBendingAngle, current.maxBendingAngle);
			num2 = Mathf.Clamp(num2, -current.maxBendingAngle, current.maxBendingAngle);
			Vector3 axis2 = Vector3.Cross(current.referenceUpDir, current.referenceLookDir);
			current.angleH = Mathf.Lerp(current.angleH, num, Time.deltaTime * current.responsiveness);
			current.angleV = Mathf.Lerp(current.angleV, num2, Time.deltaTime * current.responsiveness);
			vector = Quaternion.AngleAxis(current.angleH, current.referenceUpDir) * Quaternion.AngleAxis(current.angleV, axis2) * current.referenceLookDir;
			Vector3 referenceUpDir = current.referenceUpDir;
			Vector3.OrthoNormalize(ref vector, ref referenceUpDir);
			Vector3 forward = vector;
			current.dirUp = Vector3.Slerp(current.dirUp, referenceUpDir, Time.deltaTime * 5f);
			Vector3.OrthoNormalize(ref forward, ref current.dirUp);
			Quaternion to = rotation * Quaternion.LookRotation(forward, current.dirUp) * Quaternion.Inverse(rotation * Quaternion.LookRotation(current.referenceLookDir, current.referenceUpDir));
			Quaternion lhs = Quaternion.Lerp(Quaternion.identity, to, this.effect / (float)current.chainLength);
			transform2 = current.lastTransform;
			for (int k = 0; k < current.chainLength; k++)
			{
				transform2.rotation = lhs * transform2.rotation;
				transform2 = transform2.parent;
			}
		}
		for (int l = 0; l < this.nonAffectedJoints.Count; l++)
		{
			Vector3 vector2 = Vector3.zero;
			IEnumerator enumerator3 = this.nonAffectedJoints[l].joint.GetEnumerator();
			try
			{
				if (enumerator3.MoveNext())
				{
					Transform transform3 = (Transform)enumerator3.Current;
					vector2 = transform3.position - this.nonAffectedJoints[l].joint.position;
				}
			}
			finally
			{
				IDisposable disposable2 = enumerator3 as IDisposable;
				if (disposable2 != null)
				{
					disposable2.Dispose();
				}
			}
			Vector3 toDirection = Vector3.Slerp(array[l], vector2, this.nonAffectedJoints[l].effect);
			this.nonAffectedJoints[l].joint.rotation = Quaternion.FromToRotation(vector2, toDirection) * this.nonAffectedJoints[l].joint.rotation;
		}
	}

	private float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA -= Vector3.Project(dirA, axis);
		dirB -= Vector3.Project(dirB, axis);
		float num = Vector3.Angle(dirA, dirB);
		return num * (float)((Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) >= 0f) ? 1 : -1);
	}
}
