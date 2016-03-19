using System;
using UnityEngine;

namespace UnityForms
{
	public class CheckBox : UIStateToggleBtn
	{
		private DrawTexture m_dtDisable0;

		private DrawTexture m_dtDisable1;

		private bool changeColor;

		public DrawTexture DTDesable0
		{
			get
			{
				return this.m_dtDisable0;
			}
			set
			{
				this.m_dtDisable0 = value;
			}
		}

		public DrawTexture DTDesable1
		{
			get
			{
				return this.m_dtDisable1;
			}
			set
			{
				this.m_dtDisable1 = value;
			}
		}

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

		public override bool controlIsEnabled
		{
			get
			{
				if (!this.IsChecked())
				{
					return null != this.m_dtDisable0 && !this.m_dtDisable0.Visible;
				}
				return null != this.m_dtDisable1 && !this.m_dtDisable1.Visible;
			}
			set
			{
				if (null != this.m_dtDisable0 && null != this.m_dtDisable1)
				{
					this.m_dtDisable0.Visible = false;
					this.m_dtDisable1.Visible = false;
					if (!this.IsChecked())
					{
						this.m_dtDisable0.Visible = !value;
					}
					else
					{
						this.m_dtDisable1.Visible = !value;
					}
				}
			}
		}

		public new static CheckBox Create(string name, Vector3 pos)
		{
			return (CheckBox)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(CheckBox));
		}

		public bool IsChecked()
		{
			return base.StateNum == 1;
		}

		public void SetEnabled(bool value)
		{
			this.controlIsEnabled = value;
		}

		public bool GetEnabled()
		{
			return this.controlIsEnabled;
		}
	}
}
