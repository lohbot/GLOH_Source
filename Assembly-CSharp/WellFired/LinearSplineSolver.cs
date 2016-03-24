using System;
using System.Collections.Generic;
using UnityEngine;

namespace WellFired
{
	[Serializable]
	public class LinearSplineSolver : AbstractSplineSolver
	{
		private Dictionary<int, float> segmentStartLocations;

		private Dictionary<int, float> segmentDistances;

		private int currentSegment;

		public override void Build()
		{
			this.segmentStartLocations = new Dictionary<int, float>(base.Nodes.Count - 2);
			this.segmentDistances = new Dictionary<int, float>(base.Nodes.Count - 1);
			for (int i = 0; i < base.Nodes.Count - 1; i++)
			{
				float num = Vector3.Distance(base.Nodes[i].Position, base.Nodes[i + 1].Position);
				this.segmentDistances.Add(i, num);
				base.PathLength += num;
			}
			float num2 = 0f;
			for (int j = 0; j < this.segmentDistances.Count - 1; j++)
			{
				num2 += this.segmentDistances[j];
				this.segmentStartLocations.Add(j + 1, num2 / base.PathLength);
			}
		}

		public override void Close()
		{
			if (base.Nodes[0].Position != base.Nodes[base.Nodes.Count - 1].Position)
			{
				base.Nodes.Add(base.Nodes[0]);
			}
		}

		public override Vector3 GetPosition(float time)
		{
			return this.GetPositionOnPath(time);
		}

		public override Vector3 GetPositionOnPath(float time)
		{
			if (base.Nodes.Count < 3)
			{
				return Vector3.Lerp(base.Nodes[0].Position, base.Nodes[1].Position, time);
			}
			this.currentSegment = 0;
			foreach (KeyValuePair<int, float> current in this.segmentStartLocations)
			{
				if (current.Value >= time)
				{
					break;
				}
				this.currentSegment = current.Key;
			}
			float num = time * base.PathLength;
			for (int i = this.currentSegment - 1; i >= 0; i--)
			{
				num -= this.segmentDistances[i];
			}
			return Vector3.Lerp(base.Nodes[this.currentSegment].Position, base.Nodes[this.currentSegment + 1].Position, num / this.segmentDistances[this.currentSegment]);
		}

		public override void Display(Color splineColor)
		{
		}
	}
}
