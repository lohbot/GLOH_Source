using System;
using UnityEngine;

namespace GAME
{
	public class POS2D
	{
		public float x;

		public float y;

		public POS2D()
		{
			this.x = 0f;
			this.y = 0f;
		}

		public POS2D(ref Vector2 v2)
		{
			this.x = v2.x;
			this.y = v2.y;
		}

		public void Normalize()
		{
			if (this.x == 0f && this.y == 0f)
			{
				return;
			}
			float num = Mathf.Sqrt(this.x * this.x + this.y * this.y);
			this.x /= num;
			this.y /= num;
		}

		public float Length()
		{
			return Mathf.Sqrt(this.x * this.x + this.y * this.y);
		}
	}
}
