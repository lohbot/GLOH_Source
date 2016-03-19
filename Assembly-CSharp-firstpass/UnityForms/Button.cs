using System;
using UnityEngine;

namespace UnityForms
{
	public class Button : UIButton
	{
		private int tabIndex;

		public int TabIndex
		{
			get
			{
				return this.tabIndex;
			}
			set
			{
				this.tabIndex = value;
			}
		}

		public int Tag
		{
			get
			{
				return this.tabIndex;
			}
			set
			{
				this.tabIndex = value;
			}
		}

		public EZValueChangedDelegate Click
		{
			get
			{
				return this.changeDelegate;
			}
			set
			{
				this.changeDelegate = value;
			}
		}

		public new static Button Create(string name, Vector3 pos)
		{
			return (Button)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(Button));
		}
	}
}
