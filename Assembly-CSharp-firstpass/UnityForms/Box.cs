using System;
using UnityEngine;

namespace UnityForms
{
	public class Box : UIButton
	{
		private bool horizontalScrollText;

		public Vector2 margins = new Vector2(6f, 0f);

		protected Rect3D clientClippingRect;

		protected Vector2 marginTopLeft;

		protected Vector2 marginBottomRight;

		private float _alphaValue = 1f;

		private float addx = 0.5f;

		private float totalAdd;

		private bool startEnd;

		private Vector3 newPos = Vector3.zero;

		public bool HorizontalScrollText
		{
			set
			{
				this.horizontalScrollText = value;
			}
		}

		public float AlphaValue
		{
			get
			{
				return this._alphaValue;
			}
			set
			{
				this._alphaValue = value;
			}
		}

		public new static Box Create(string name, Vector3 pos)
		{
			return (Box)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(Box));
		}

		public override void OnInput(ref POINTER_INFO ptr)
		{
			if (this.deleted)
			{
				return;
			}
			if (!this.m_controlIsEnabled || base.IsHidden())
			{
				base.OnInput(ref ptr);
				return;
			}
			if (this.inputDelegate != null)
			{
				this.inputDelegate(ref ptr);
			}
			if (!this.m_controlIsEnabled || base.IsHidden())
			{
				base.OnInput(ref ptr);
				return;
			}
			switch (ptr.evt)
			{
			case POINTER_INFO.INPUT_EVENT.PRESS:
			case POINTER_INFO.INPUT_EVENT.DRAG:
				base.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.TAP:
				if (ptr.type != POINTER_INFO.POINTER_TYPE.TOUCHPAD && ptr.hitInfo.collider == base.collider)
				{
					base.SetControlState(UIButton.CONTROL_STATE.OVER);
				}
				else
				{
					base.SetControlState(UIButton.CONTROL_STATE.NORMAL);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
				if (this.m_ctrlState != UIButton.CONTROL_STATE.OVER)
				{
					base.SetControlState(UIButton.CONTROL_STATE.OVER);
					if (this.soundOnOver != null)
					{
						this.soundOnOver.PlayOneShot(this.soundOnOver.clip);
					}
					if (this.mouseOverDelegate != null)
					{
						this.mouseOverDelegate(this);
					}
				}
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				base.SetControlState(UIButton.CONTROL_STATE.NORMAL);
				if (this.mouseOutDelegate != null)
				{
					this.mouseOutDelegate(this);
				}
				break;
			}
			if (this.repeat)
			{
				if (this.m_ctrlState == UIButton.CONTROL_STATE.ACTIVE)
				{
					goto IL_19F;
				}
			}
			else if (ptr.evt == this.whenToInvoke)
			{
				goto IL_19F;
			}
			return;
			IL_19F:
			if (ptr.evt == this.whenToInvoke && this.soundOnClick != null)
			{
				this.soundOnClick.PlayOneShot(this.soundOnClick.clip);
			}
			if (this.scriptWithMethodToInvoke != null)
			{
				this.scriptWithMethodToInvoke.Invoke(this.methodToInvoke, this.delay);
			}
			if (this.changeDelegate != null)
			{
				this.changeDelegate(this);
			}
		}

		public void SetMargins(Vector2 marg)
		{
			this.margins = marg;
			Vector3 centerPoint = base.GetCenterPoint();
			this.marginTopLeft = new Vector3(centerPoint.x + this.margins.x - this.width * 0.5f, centerPoint.y - this.margins.y + this.height * 0.5f);
			this.marginBottomRight = new Vector3(centerPoint.x - this.margins.x + this.width * 0.5f, centerPoint.y + this.margins.y - this.height * 0.5f);
		}

		public void CalcClippingRect()
		{
			if (this.spriteText == null)
			{
				return;
			}
			Vector3 vector = this.marginTopLeft;
			Vector3 vector2 = this.marginBottomRight;
			if (this.clipped)
			{
				Vector3 vector3 = vector;
				Vector3 vector4 = vector2;
				vector.x = Mathf.Clamp(this.localClipRect.x, vector3.x, vector4.x);
				vector2.x = Mathf.Clamp(this.localClipRect.xMax, vector3.x, vector4.x);
				vector.y = Mathf.Clamp(this.localClipRect.yMax, vector4.y, vector3.y);
				vector2.y = Mathf.Clamp(this.localClipRect.y, vector4.y, vector3.y);
			}
			this.clientClippingRect.FromRect(Rect.MinMaxRect(vector.x, vector2.y, vector2.x, vector.y));
			this.clientClippingRect.MultFast(base.transform.localToWorldMatrix);
			this.spriteText.SetClippingRect(this.clientClippingRect);
		}

		public override void SetSize(float width, float height)
		{
			base.SetSize(width, height);
			if (this.spriteText)
			{
				this.SetMargins(this.margins);
			}
		}

		public override void Update()
		{
			if (!this.horizontalScrollText || !this.Visible || null == this.spriteText)
			{
				return;
			}
			this.totalAdd += this.addx;
			float num = this.spriteText.TotalWidth / 2f;
			float num2 = this.width / 2f;
			if (this.startEnd)
			{
				if (this.totalAdd > this.width + num)
				{
					this.newPos.x = num2 + num;
					this.spriteText.transform.localPosition = this.newPos;
					this.startEnd = true;
					this.totalAdd = 0f;
				}
			}
			else if (this.totalAdd > num2 + num)
			{
				this.newPos.x = num2 + num;
				this.spriteText.transform.localPosition = this.newPos;
				this.startEnd = true;
				this.totalAdd = 0f;
			}
			this.newPos.x = this.spriteText.transform.localPosition.x - this.addx;
			this.spriteText.transform.localPosition = this.newPos;
			this.CalcClippingRect();
		}
	}
}
