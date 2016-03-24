using System;
using UnityEngine;

namespace WellFired
{
	public struct LerpedQuaternion
	{
		private bool slerp;

		public float duration;

		private Quaternion currentValue;

		private Quaternion target;

		private Quaternion source;

		private float startTime;

		public float x
		{
			get
			{
				return this.currentValue.x;
			}
			set
			{
				this.SmoothValue = new Quaternion(value, this.target.y, this.target.z, this.target.w);
			}
		}

		public float y
		{
			get
			{
				return this.currentValue.y;
			}
			set
			{
				this.SmoothValue = new Quaternion(this.target.x, value, this.target.y, this.target.w);
			}
		}

		public float z
		{
			get
			{
				return this.currentValue.z;
			}
			set
			{
				this.SmoothValue = new Quaternion(this.target.x, this.target.y, value, this.target.w);
			}
		}

		public float w
		{
			get
			{
				return this.currentValue.w;
			}
			set
			{
				this.SmoothValue = new Quaternion(this.target.x, this.target.y, this.target.z, value);
			}
		}

		public Quaternion SmoothValue
		{
			get
			{
				float t = (Time.realtimeSinceStartup - this.startTime) / this.duration;
				if (!this.slerp)
				{
					this.currentValue = Quaternion.Lerp(this.source, this.target, t);
				}
				else
				{
					this.currentValue = Quaternion.Slerp(this.source, this.target, t);
				}
				return this.currentValue;
			}
			set
			{
				this.source = this.SmoothValue;
				this.startTime = Time.realtimeSinceStartup;
				this.target = value;
			}
		}

		public LerpedQuaternion(Quaternion quaternion)
		{
			this.slerp = true;
			this.currentValue = quaternion;
			this.source = quaternion;
			this.target = quaternion;
			this.startTime = Time.realtimeSinceStartup;
			this.duration = 0.2f;
		}
	}
}
