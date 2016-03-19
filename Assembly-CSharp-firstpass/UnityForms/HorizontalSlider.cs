using System;
using UnityEngine;

namespace UnityForms
{
	public class HorizontalSlider : UISlider
	{
		public new static HorizontalSlider Create(string name, Vector3 pos)
		{
			return (HorizontalSlider)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(HorizontalSlider));
		}
	}
}
