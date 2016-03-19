using System;
using UnityEngine;

namespace UnityForms
{
	public class ProgressBarUp : UIProgressBar
	{
		public new static ProgressBarUp Create(string name, Vector3 pos)
		{
			return (ProgressBarUp)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(ProgressBarUp));
		}
	}
}
