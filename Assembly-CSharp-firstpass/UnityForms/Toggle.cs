using System;
using UnityEngine;

namespace UnityForms
{
	public class Toggle : UIRadioBtn
	{
		private bool changeColor;

		public bool ChangeColor
		{
			get
			{
				return this.changeColor;
			}
			set
			{
				this.changeColor = value;
			}
		}

		public string ChangeNormalColor
		{
			set
			{
				this.Text = value + this.Text;
			}
		}

		public EZValueChangedDelegate CheckedChanged
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

		public new static Toggle Create(string name, Vector3 pos)
		{
			return (Toggle)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(Toggle));
		}

		public void SetToggleState(bool value)
		{
			this.btnValue = value;
			if (this.btnValue)
			{
				base.PopOtherButtonsInGroup();
			}
			this.SetButtonState();
		}

		public bool GetToggleState()
		{
			return this.Value;
		}

		public void SetEnabled(bool value)
		{
			this.controlIsEnabled = value;
		}

		public bool GetEnabled()
		{
			return this.m_controlIsEnabled;
		}
	}
}
