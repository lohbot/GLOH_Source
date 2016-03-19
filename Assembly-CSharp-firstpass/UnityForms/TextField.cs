using System;
using UnityEngine;

namespace UnityForms
{
	public class TextField : UITextField
	{
		public bool Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
				this.spriteText.password = value;
			}
		}

		public EZKeyboardCommitDelegate CommitDelegate
		{
			get
			{
				return this.commitDelegate;
			}
			set
			{
				this.commitDelegate = value;
			}
		}

		public new static TextField Create(string name, Vector3 pos)
		{
			return (TextField)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(TextField));
		}
	}
}
