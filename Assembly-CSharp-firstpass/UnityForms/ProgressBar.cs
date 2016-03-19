using System;
using UnityEngine;

namespace UnityForms
{
	public class ProgressBar : UIProgressBar
	{
		public new static ProgressBar Create(string name, Vector3 pos)
		{
			return (ProgressBar)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(ProgressBar));
		}
	}
}
