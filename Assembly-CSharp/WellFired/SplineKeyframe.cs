using System;
using UnityEngine;

namespace WellFired
{
	[Serializable]
	public class SplineKeyframe : ScriptableObject
	{
		[SerializeField]
		private Vector3 position = Vector3.zero;

		public Vector3 Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}
	}
}
