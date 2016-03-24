using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired.Shared;

namespace WellFired
{
	[Serializable]
	public class CubicBezierSplineSolver : AbstractSplineSolver
	{
		public CubicBezierSplineSolver(List<SplineKeyframe> nodes)
		{
			base.Nodes = nodes;
		}

		public override void Close()
		{
		}

		public override Vector3 GetPosition(float time)
		{
			float num = 1f - time;
			return num * num * num * base.Nodes[0].Position + 3f * num * num * time * base.Nodes[2].Position + 3f * num * time * time * base.Nodes[3].Position + time * time * time * base.Nodes[1].Position;
		}

		public override void Display(Color splineColor)
		{
			using (new GizmosChangeColor(Color.red))
			{
				Gizmos.DrawLine(base.Nodes[0].Position, base.Nodes[2].Position);
				Gizmos.DrawLine(base.Nodes[3].Position, base.Nodes[1].Position);
			}
		}
	}
}
