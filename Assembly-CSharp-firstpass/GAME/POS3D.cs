using System;
using System.Text;
using UnityEngine;

namespace GAME
{
	public class POS3D
	{
		public float x;

		public float y;

		public float z;

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("({0}, {1}, {2})", this.x, this.y, this.z);
			return stringBuilder.ToString();
		}

		public Vector3 ToVector3()
		{
			return new Vector3(this.x, this.y, this.z);
		}
	}
}
