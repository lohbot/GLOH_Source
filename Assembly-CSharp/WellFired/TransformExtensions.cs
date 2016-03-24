using System;
using UnityEngine;

namespace WellFired
{
	public static class TransformExtensions
	{
		public static string GetFullHierarchyPath(this Transform transform)
		{
			string text = "/" + transform.name;
			while (transform.parent != null)
			{
				transform = transform.parent;
				text = "/" + transform.name + text;
			}
			return text;
		}
	}
}
