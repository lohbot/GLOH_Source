using System;
using UnityEngine;
using WellFired.Shared;

namespace WellFired
{
	[Serializable]
	public class QuadraticSplineSolver : AbstractSplineSolver
	{
		protected float quadBezierLength(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint)
		{
			Vector3[] array = new Vector3[]
			{
				controlPoint - startPoint,
				startPoint - 2f * controlPoint + endPoint
			};
			float result;
			if (array[1] != Vector3.zero)
			{
				float num = 4f * Vector3.Dot(array[1], array[1]);
				float num2 = 8f * Vector3.Dot(array[0], array[1]);
				float num3 = 4f * Vector3.Dot(array[0], array[0]);
				float num4 = 4f * num3 * num - num2 * num2;
				float num5 = 2f * num + num2;
				float num6 = num + num2 + num3;
				float num7 = 0.25f / num;
				float num8 = num4 / (8f * Mathf.Pow(num, 1.5f));
				result = num7 * (num5 * Mathf.Sqrt(num6) - num2 * Mathf.Sqrt(num3)) + num8 * (Mathf.Log(2f * Mathf.Sqrt(num * num6) + num5) - Mathf.Log(2f * Mathf.Sqrt(num * num3) + num2));
			}
			else
			{
				result = 2f * array[0].magnitude;
			}
			return result;
		}

		public override void Close()
		{
		}

		public override Vector3 GetPosition(float time)
		{
			float num = 1f - time;
			return num * num * base.Nodes[0].Position + 2f * num * time * base.Nodes[1].Position + time * time * base.Nodes[2].Position;
		}

		public override void Display(Color splineColor)
		{
			using (new GizmosChangeColor(Color.red))
			{
				Gizmos.DrawLine(base.Nodes[0].Position, base.Nodes[1].Position);
				Gizmos.DrawLine(base.Nodes[1].Position, base.Nodes[2].Position);
			}
		}
	}
}
