using System;
using UnityEngine;

namespace UnityForms
{
	public class VerticalSlider : UISlider
	{
		public new static VerticalSlider Create(string name, Vector3 pos)
		{
			return (VerticalSlider)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(VerticalSlider));
		}
	}
}
