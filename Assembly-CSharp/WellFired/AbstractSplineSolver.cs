using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired.Shared;

namespace WellFired
{
	public abstract class AbstractSplineSolver : ScriptableObject
	{
		public const int TOTAL_SUBDIVISIONS_PER_NODE = 5;

		[SerializeField]
		protected List<SplineKeyframe> nodes;

		protected Dictionary<float, float> segmentTimeForDistance;

		public List<SplineKeyframe> Nodes
		{
			get
			{
				return this.nodes;
			}
			set
			{
				this.nodes = value;
			}
		}

		[SerializeField]
		protected float PathLength
		{
			get;
			set;
		}

		private void OnEnable()
		{
			if (this.Nodes == null)
			{
				this.Nodes = new List<SplineKeyframe>();
			}
		}

		public virtual void Build()
		{
			int num = this.Nodes.Count * 5;
			this.PathLength = 0f;
			float num2 = 1f / (float)num;
			this.segmentTimeForDistance = new Dictionary<float, float>(num);
			Vector3 b = this.GetPosition(0f);
			for (int i = 1; i < num + 1; i++)
			{
				float num3 = num2 * (float)i;
				Vector3 position = this.GetPosition(num3);
				this.PathLength += Vector3.Distance(position, b);
				b = position;
				this.segmentTimeForDistance.Add(num3, this.PathLength);
			}
		}

		public virtual Vector3 GetPositionOnPath(float time)
		{
			float num = this.PathLength * time;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			foreach (KeyValuePair<float, float> current in this.segmentTimeForDistance)
			{
				if (current.Value >= num)
				{
					num4 = current.Key;
					num5 = current.Value;
					if (num2 > 0f)
					{
						num3 = this.segmentTimeForDistance[num2];
					}
					break;
				}
				num2 = current.Key;
			}
			float num6 = num4 - num2;
			float num7 = num5 - num3;
			float num8 = num - num3;
			time = num2 + num8 / num7 * num6;
			return this.GetPosition(time);
		}

		public void Reverse()
		{
			this.Nodes.Reverse();
		}

		public void OnInternalDrawGizmos(Color splineColor, float displayResolution)
		{
			this.Display(splineColor);
			using (new GizmosChangeColor(splineColor))
			{
				Vector3 to = this.GetPosition(0f);
				float num = displayResolution * (float)this.Nodes.Count;
				for (float num2 = 1f; num2 <= num; num2 += 1f)
				{
					float time = num2 / num;
					Vector3 position = this.GetPosition(time);
					Gizmos.DrawLine(position, to);
					to = position;
				}
			}
		}

		public abstract void Display(Color splineColor);

		public abstract void Close();

		public abstract Vector3 GetPosition(float time);
	}
}
