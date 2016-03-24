using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired.Shared;

namespace WellFired
{
	[Serializable]
	public class CatmullRomSplineSolver : AbstractSplineSolver
	{
		public CatmullRomSplineSolver(List<SplineKeyframe> nodes)
		{
			base.Nodes = nodes;
		}

		public override void Close()
		{
			base.Nodes.RemoveAt(0);
			base.Nodes.RemoveAt(base.Nodes.Count - 1);
			if (base.Nodes[0] != base.Nodes[base.Nodes.Count - 1])
			{
				base.Nodes.Add(base.Nodes[0]);
			}
			float num = Vector3.Distance(base.Nodes[0].Position, base.Nodes[1].Position);
			float num2 = Vector3.Distance(base.Nodes[0].Position, base.Nodes[base.Nodes.Count - 2].Position);
			float d = num2 / Vector3.Distance(base.Nodes[1].Position, base.Nodes[0].Position);
			Vector3 position = base.Nodes[0].Position + (base.Nodes[1].Position - base.Nodes[0].Position) * d;
			float d2 = num / Vector3.Distance(base.Nodes[base.Nodes.Count - 2].Position, base.Nodes[0].Position);
			Vector3 position2 = base.Nodes[0].Position + (base.Nodes[base.Nodes.Count - 2].Position - base.Nodes[0].Position) * d2;
			SplineKeyframe splineKeyframe = new SplineKeyframe();
			splineKeyframe.Position = position2;
			SplineKeyframe splineKeyframe2 = new SplineKeyframe();
			splineKeyframe2.Position = position;
			base.Nodes.Insert(0, splineKeyframe);
			base.Nodes.Add(splineKeyframe2);
		}

		public override Vector3 GetPosition(float time)
		{
			int num = base.Nodes.Count - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(time * (float)num), num - 1);
			float num3 = time * (float)num - (float)num2;
			Vector3 position = base.Nodes[num2].Position;
			Vector3 position2 = base.Nodes[num2 + 1].Position;
			Vector3 position3 = base.Nodes[num2 + 2].Position;
			Vector3 position4 = base.Nodes[num2 + 3].Position;
			return 0.5f * ((-position + 3f * position2 - 3f * position3 + position4) * (num3 * num3 * num3) + (2f * position - 5f * position2 + 4f * position3 - position4) * (num3 * num3) + (-position + position3) * num3 + 2f * position2);
		}

		public override void Display(Color splineColor)
		{
			if (base.Nodes.Count < 2)
			{
				return;
			}
			using (new GizmosChangeColor(Color.red))
			{
				Gizmos.DrawLine(base.Nodes[0].Position, base.Nodes[1].Position);
				Gizmos.DrawLine(base.Nodes[base.Nodes.Count - 1].Position, base.Nodes[base.Nodes.Count - 2].Position);
			}
		}
	}
}
