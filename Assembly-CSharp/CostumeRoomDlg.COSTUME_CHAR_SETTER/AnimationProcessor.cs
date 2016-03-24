using System;
using UnityEngine;

namespace CostumeRoomDlg.COSTUME_CHAR_SETTER
{
	public class AnimationProcessor
	{
		private CostumeCharSetter _costumeCharSetter;

		private string _idleAni = string.Empty;

		private string _attackAniName = string.Empty;

		private string _runAniName = string.Empty;

		public AnimationProcessor(CostumeCharSetter costumeCharSetter)
		{
			this._costumeCharSetter = costumeCharSetter;
			this._attackAniName = "attack1";
			this._idleAni = "bstay1";
			this._runAniName = "brun1";
		}

		public void PlayDefaultIdelAni()
		{
			if (this._costumeCharSetter._CostumeChar == null)
			{
				return;
			}
			Animation component = this._costumeCharSetter._CostumeChar.GetComponent<Animation>();
			if (component == null)
			{
				return;
			}
			if (component.isPlaying)
			{
				return;
			}
			component.Play(this._idleAni);
		}

		public void PlayAttackAni(bool stop)
		{
			if (this._costumeCharSetter._CostumeChar == null)
			{
				return;
			}
			Animation component = this._costumeCharSetter._CostumeChar.GetComponent<Animation>();
			if (component == null)
			{
				return;
			}
			if (stop)
			{
				component.Stop();
			}
			if (component.isPlaying)
			{
				return;
			}
			component.Play(this._attackAniName);
		}

		public void PlayMoveAni(bool stop)
		{
			if (this._costumeCharSetter._CostumeChar == null)
			{
				return;
			}
			Animation component = this._costumeCharSetter._CostumeChar.GetComponent<Animation>();
			if (component == null)
			{
				return;
			}
			if (stop)
			{
				component.Stop();
			}
			if (component.isPlaying)
			{
				return;
			}
			component.Play(this._runAniName);
		}

		public void PlayIdelAni(bool stop)
		{
			if (this._costumeCharSetter._CostumeChar == null)
			{
				return;
			}
			Animation component = this._costumeCharSetter._CostumeChar.GetComponent<Animation>();
			if (component == null)
			{
				return;
			}
			if (stop)
			{
				component.Stop();
			}
			if (component.isPlaying)
			{
				return;
			}
			component.Play(this._idleAni);
		}

		public bool IsAttackAniPlaying()
		{
			if (this._costumeCharSetter._CostumeChar == null)
			{
				return false;
			}
			Animation component = this._costumeCharSetter._CostumeChar.GetComponent<Animation>();
			return !(component == null) && component.IsPlaying(this._attackAniName);
		}

		public bool IsIdleAniPlaying()
		{
			if (this._costumeCharSetter._CostumeChar == null)
			{
				return false;
			}
			Animation component = this._costumeCharSetter._CostumeChar.GetComponent<Animation>();
			return !(component == null) && component.IsPlaying(this._idleAni);
		}

		public bool IsMoveAniPlaying()
		{
			if (this._costumeCharSetter._CostumeChar == null)
			{
				return false;
			}
			Animation component = this._costumeCharSetter._CostumeChar.GetComponent<Animation>();
			return !(component == null) && component.IsPlaying(this._runAniName);
		}
	}
}
