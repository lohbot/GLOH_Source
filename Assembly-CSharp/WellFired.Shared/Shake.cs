using System;
using UnityEngine;

namespace WellFired.Shared
{
	[Serializable]
	public class Shake
	{
		private InterpolatedNoise interpolatedNoise;

		private ShakeType shakeType;

		private float shakeSpeedPosition = 0.38f;

		private Vector3 shakeRangePosition = new Vector3(0.4f, 0.4f, 0.4f);

		private float shakeSpeedRotation = 0.38f;

		private Vector3 shakeRangeRotation = new Vector3(4f, 4f, 4f);

		public ShakeType ShakeType
		{
			get
			{
				return this.shakeType;
			}
			set
			{
				this.shakeType = value;
				if (this.shakeType == ShakeType.None)
				{
					this.Position = Vector3.zero;
					this.EulerRotation = Vector3.zero;
				}
			}
		}

		public Vector3 Position
		{
			get;
			set;
		}

		public Vector3 EulerRotation
		{
			get;
			set;
		}

		public float ShakeSpeedPosition
		{
			get
			{
				return this.shakeSpeedPosition;
			}
			set
			{
				this.shakeSpeedPosition = value;
			}
		}

		public Vector3 ShakeRangePosition
		{
			get
			{
				return this.shakeRangePosition;
			}
			set
			{
				this.shakeRangePosition = value;
			}
		}

		public float ShakeSpeedRotation
		{
			get
			{
				return this.shakeSpeedRotation;
			}
			set
			{
				this.shakeSpeedRotation = value;
			}
		}

		public Vector3 ShakeRangeRotation
		{
			get
			{
				return this.shakeRangeRotation;
			}
			set
			{
				this.shakeRangeRotation = value;
			}
		}

		public void InitialiseShake(int seed)
		{
			this.interpolatedNoise = new InterpolatedNoise(seed);
		}

		public void InitialiseShake(int seed, ShakeType shakeType)
		{
			this.interpolatedNoise = new InterpolatedNoise(seed);
			this.ShakeType = shakeType;
			this.Position = Vector3.zero;
			this.EulerRotation = Vector3.zero;
		}

		public void InitialiseShake(int seed, ShakeType shakeType, float shakeSpeedPosition, Vector3 shakeRangePosition, float shakeSpeedRotation, Vector3 shakeRangeRotation)
		{
			this.interpolatedNoise = new InterpolatedNoise(seed);
			this.shakeSpeedPosition = shakeSpeedPosition;
			this.shakeRangePosition = shakeRangePosition;
			this.shakeSpeedRotation = shakeSpeedRotation;
			this.shakeRangeRotation = shakeRangeRotation;
			this.ShakeType = shakeType;
			this.Position = Vector3.zero;
			this.EulerRotation = Vector3.zero;
		}

		public void Process(float time, float duration)
		{
			if (this.ShakeType == ShakeType.None)
			{
				return;
			}
			if (this.ShakeType == ShakeType.Position || this.ShakeType == ShakeType.Both)
			{
				this.Position = Vector3.Scale(this.interpolatedNoise.GetVector3(this.shakeSpeedPosition, time), this.shakeRangePosition);
			}
			if (this.ShakeType == ShakeType.Rotation || this.ShakeType == ShakeType.Both)
			{
				this.EulerRotation = Vector3.Scale(this.interpolatedNoise.GetVector3(this.shakeSpeedRotation, time), this.shakeRangeRotation);
			}
		}
	}
}
