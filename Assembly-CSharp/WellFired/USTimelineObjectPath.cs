using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired.Shared;

namespace WellFired
{
	[Serializable]
	public class USTimelineObjectPath : USTimelineBase
	{
		[SerializeField]
		private Easing.EasingType easingType;

		[SerializeField]
		private ShakeType shakeType;

		[SerializeField]
		private SplineOrientationMode splineOrientationMode = SplineOrientationMode.LookAhead;

		[SerializeField]
		private Transform lookAtTarget;

		[SerializeField]
		private string lookAtTargetPath = string.Empty;

		[SerializeField]
		private Vector3 sourcePosition;

		[SerializeField]
		private Quaternion sourceRotation;

		[SerializeField]
		private Spline objectSpline;

		[SerializeField]
		private float startTime;

		[SerializeField]
		private float endTime;

		[SerializeField]
		private List<SplineKeyframe> keyframes;

		[SerializeField]
		private WellFired.Shared.Shake Shake;

		[SerializeField]
		private float shakeSpeedPosition = 0.38f;

		[SerializeField]
		private Vector3 shakeRangePosition = new Vector3(0.4f, 0.4f, 0.4f);

		[SerializeField]
		private float shakeSpeedRotation = 0.38f;

		[SerializeField]
		private Vector3 shakeRangeRotation = new Vector3(4f, 4f, 4f);

		[SerializeField]
		private int seed;

		private LerpedQuaternion SmoothedQuaternion;

		public Easing.EasingType EasingType
		{
			get
			{
				return this.easingType;
			}
			set
			{
				this.easingType = value;
			}
		}

		public ShakeType ShakeType
		{
			get
			{
				return this.shakeType;
			}
			set
			{
				this.shakeType = value;
			}
		}

		public SplineOrientationMode SplineOrientationMode
		{
			get
			{
				return this.splineOrientationMode;
			}
			set
			{
				this.splineOrientationMode = value;
			}
		}

		public Transform LookAtTarget
		{
			get
			{
				return this.lookAtTarget;
			}
			set
			{
				this.lookAtTarget = value;
				this.lookAtTargetPath = this.lookAtTarget.GetFullHierarchyPath();
			}
		}

		public Vector3 SourcePosition
		{
			get
			{
				return this.sourcePosition;
			}
			set
			{
				this.sourcePosition = value;
			}
		}

		public Quaternion SourceRotation
		{
			get
			{
				return this.sourceRotation;
			}
			set
			{
				this.sourceRotation = value;
			}
		}

		public Spline ObjectSpline
		{
			get
			{
				return this.objectSpline;
			}
			set
			{
				this.objectSpline = value;
			}
		}

		public float StartTime
		{
			get
			{
				return this.startTime;
			}
			set
			{
				this.startTime = value;
			}
		}

		public float EndTime
		{
			get
			{
				return this.endTime;
			}
			set
			{
				this.endTime = value;
			}
		}

		public int Seed
		{
			private get
			{
				return this.seed;
			}
			set
			{
				this.seed = value;
			}
		}

		public List<SplineKeyframe> Keyframes
		{
			get
			{
				return this.keyframes;
			}
			private set
			{
				this.keyframes = value;
				this.BuildCurveFromKeyframes();
			}
		}

		public SplineKeyframe FirstNode
		{
			get
			{
				return this.ObjectSpline.Nodes[0];
			}
			private set
			{
			}
		}

		public SplineKeyframe LastNode
		{
			get
			{
				return this.ObjectSpline.Nodes[this.ObjectSpline.Nodes.Count - 1];
			}
			private set
			{
			}
		}

		public Color PathColor
		{
			get
			{
				return this.ObjectSpline.SplineColor;
			}
			set
			{
				this.ObjectSpline.SplineColor = value;
			}
		}

		public float DisplayResolution
		{
			get
			{
				return this.ObjectSpline.DisplayResolution;
			}
			set
			{
				this.ObjectSpline.DisplayResolution = value;
			}
		}

		private void OnEnable()
		{
			this.Build();
		}

		public void SetKeyframes(List<SplineKeyframe> keyframes)
		{
			this.Keyframes = keyframes;
			this.Build();
		}

		public void Build()
		{
			if (this.keyframes == null)
			{
				this.CreateEmpty();
			}
			else
			{
				this.BuildCurveFromKeyframes();
			}
		}

		public void BuildShake()
		{
			this.Shake = new WellFired.Shared.Shake();
			this.Shake.InitialiseShake(this.Seed);
		}

		public void AddKeyframe(SplineKeyframe keyframe)
		{
			this.keyframes.Add(keyframe);
			this.BuildCurveFromKeyframes();
		}

		public void AddAfterKeyframe(SplineKeyframe keyframe, int index)
		{
			this.keyframes.Insert(index + 1, keyframe);
			this.BuildCurveFromKeyframes();
		}

		public void AddBeforeKeyframe(SplineKeyframe keyframe, int index)
		{
			this.keyframes.Insert(index - 1, keyframe);
			this.BuildCurveFromKeyframes();
		}

		public void AlterKeyframe(Vector3 position, int keyframeIndex)
		{
			this.keyframes[keyframeIndex].Position = position;
			this.BuildCurveFromKeyframes();
		}

		public void RemoveKeyframe(SplineKeyframe keyframe)
		{
			this.keyframes.Remove(keyframe);
			this.BuildCurveFromKeyframes();
		}

		public void BuildCurveFromKeyframes()
		{
			this.ObjectSpline.BuildFromKeyframes(this.Keyframes);
		}

		private void CreateEmpty()
		{
			this.ObjectSpline = new Spline();
			this.Keyframes = new List<SplineKeyframe>
			{
				ScriptableObject.CreateInstance<SplineKeyframe>(),
				ScriptableObject.CreateInstance<SplineKeyframe>()
			};
			this.Keyframes[0].Position = base.AffectedObject.transform.position;
			this.Keyframes[1].Position = base.AffectedObject.transform.position;
			this.StartTime = 0f;
			this.EndTime = base.Sequence.Duration;
		}

		public void SetStartingOrientation()
		{
			switch (this.SplineOrientationMode)
			{
			case SplineOrientationMode.ManualOrientation:
				base.AffectedObject.position = this.FirstNode.Position;
				break;
			case SplineOrientationMode.LookAtTransform:
				base.AffectedObject.position = this.FirstNode.Position;
				base.AffectedObject.LookAt(this.LookAtTarget, Vector3.up);
				break;
			case SplineOrientationMode.LookAhead:
			{
				Vector3 positionOnPath = this.ObjectSpline.GetPositionOnPath((base.Sequence.PlaybackRate <= 0f) ? (-USSequencer.SequenceUpdateRate) : USSequencer.SequenceUpdateRate);
				base.AffectedObject.position = this.FirstNode.Position;
				base.AffectedObject.LookAt(positionOnPath, Vector3.up);
				break;
			}
			}
		}

		public override void StartTimeline()
		{
			this.SourcePosition = base.AffectedObject.transform.position;
			this.SourceRotation = base.AffectedObject.transform.rotation;
			this.SmoothedQuaternion = new LerpedQuaternion(base.AffectedObject.rotation);
			this.BuildShake();
		}

		public override void StopTimeline()
		{
			base.AffectedObject.transform.position = this.SourcePosition;
			base.AffectedObject.transform.rotation = this.SourceRotation;
		}

		public override void SkipTimelineTo(float time)
		{
			this.Process(time, 1f);
		}

		public override void Process(float sequencerTime, float playbackRate)
		{
			if (!base.AffectedObject)
			{
				return;
			}
			if (sequencerTime < this.StartTime || sequencerTime > this.EndTime)
			{
				return;
			}
			if (this.SplineOrientationMode == SplineOrientationMode.LookAtTransform && this.LookAtTarget == null)
			{
				throw new Exception("Spline Orientation Mode is look at object, but there is no LookAtTarget");
			}
			float num = (sequencerTime - this.StartTime) / (this.EndTime - this.StartTime);
			DoubleEasing.EasingFunction easingFunctionFor = DoubleEasing.GetEasingFunctionFor(this.easingType);
			num = (float)easingFunctionFor((double)num, 0.0, 1.0, 1.0);
			Quaternion rotation = this.sourceRotation;
			switch (this.SplineOrientationMode)
			{
			case SplineOrientationMode.ManualOrientation:
				base.AffectedObject.position = this.ObjectSpline.GetPositionOnPath(num);
				break;
			case SplineOrientationMode.LookAtTransform:
				base.AffectedObject.position = this.ObjectSpline.GetPositionOnPath(num);
				base.AffectedObject.LookAt(this.LookAtTarget, Vector3.up);
				rotation = base.AffectedObject.rotation;
				break;
			case SplineOrientationMode.LookAhead:
			{
				Vector3 positionOnPath = this.ObjectSpline.GetPositionOnPath((base.Sequence.PlaybackRate <= 0f) ? (num - USSequencer.SequenceUpdateRate) : (num + USSequencer.SequenceUpdateRate));
				Vector3 positionOnPath2 = this.ObjectSpline.GetPositionOnPath((base.Sequence.PlaybackRate <= 0f) ? (num + USSequencer.SequenceUpdateRate) : (num - USSequencer.SequenceUpdateRate));
				bool flag = positionOnPath.Equals(positionOnPath2);
				this.SmoothedQuaternion.SmoothValue = ((!flag) ? Quaternion.LookRotation((positionOnPath - positionOnPath2).normalized) : Quaternion.identity);
				base.AffectedObject.rotation = this.SmoothedQuaternion.SmoothValue;
				base.AffectedObject.position = this.ObjectSpline.GetPositionOnPath(num);
				rotation = base.AffectedObject.rotation;
				break;
			}
			}
			this.Shake.ShakeType = this.shakeType;
			this.Shake.ShakeSpeedPosition = this.shakeSpeedPosition;
			this.Shake.ShakeRangePosition = this.shakeRangePosition;
			this.Shake.ShakeSpeedRotation = this.shakeSpeedRotation;
			this.Shake.ShakeRangeRotation = this.shakeRangeRotation;
			this.Shake.Process(sequencerTime, base.Sequence.Duration);
			Quaternion quaternion = Quaternion.Euler(rotation.eulerAngles + this.Shake.EulerRotation);
			float min = 0f;
			float num2 = base.Sequence.Duration * 0.1f;
			num2 = Mathf.Clamp(num2, 0.1f, 1f);
			float t = Mathf.Clamp(sequencerTime, min, num2) / num2;
			Vector3 b = Vector3.Slerp(Vector3.zero, this.Shake.Position, t);
			quaternion = Quaternion.Slerp(base.AffectedObject.localRotation, quaternion, t);
			base.AffectedObject.localPosition += b;
			base.AffectedObject.localRotation = quaternion;
		}

		private void OnDrawGizmos()
		{
			if (!base.ShouldRenderGizmos)
			{
				return;
			}
			if (this.ObjectSpline == null)
			{
				return;
			}
			this.ObjectSpline.OnDrawGizmos();
		}

		public void FixupAdditionalObjects()
		{
			if (!string.IsNullOrEmpty(this.lookAtTargetPath))
			{
				GameObject gameObject = GameObject.Find(this.lookAtTargetPath);
				if (gameObject != null)
				{
					this.LookAtTarget = gameObject.transform;
				}
			}
			if (this.LookAtTarget == null && !string.IsNullOrEmpty(this.lookAtTargetPath))
			{
				Debug.LogWarning(string.Format("Tried to fixup a lookat target for this object path timeline, but it doesn't exist in this scene. (Target = {0}, ObjectPathTimeline = {1})", this.lookAtTargetPath, this), this);
			}
		}

		public void RecordAdditionalObjects()
		{
			if (this.LookAtTarget != null)
			{
				this.lookAtTargetPath = this.LookAtTarget.GetFullHierarchyPath();
			}
		}
	}
}
