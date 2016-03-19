using GameMessage;
using System;
using UnityEngine;

namespace UnityForms
{
	public class LinkText : UIButton
	{
		public enum TYPE
		{
			NONE,
			ITEM,
			GENERAL,
			MONSTER,
			PLAYER,
			NPC,
			MESSAGE,
			HELP,
			PLUNDER_REPLAY,
			COLOSSEUM_REPLAY,
			MINE_REPLAY,
			COUPON,
			TREASUREBOX,
			INFIBATTLE_REPLAY
		}

		public LinkText.TYPE linkTextType;

		public string textKey = string.Empty;

		public override void OnInput(ref POINTER_INFO ptr)
		{
			if (this.deleted)
			{
				return;
			}
			switch (ptr.evt)
			{
			case POINTER_INFO.INPUT_EVENT.PRESS:
			case POINTER_INFO.INPUT_EVENT.DRAG:
				if (!base.IsListButton)
				{
					base.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.DOUBLE_PRESS:
				if (!base.IsListButton)
				{
					base.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
				}
				if (this.doubleClickDelegate != null)
				{
					this.doubleClickDelegate(this);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.RIGHT_PRESS:
				if (!base.IsListButton)
				{
					base.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
				}
				if (this.rightMouseDelegate != null)
				{
					this.rightMouseDelegate(this);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.TAP:
				if (ptr.type != POINTER_INFO.POINTER_TYPE.TOUCHPAD && ptr.hitInfo.collider == base.collider)
				{
					if (!base.IsListButton)
					{
						base.SetControlState(UIButton.CONTROL_STATE.OVER);
					}
				}
				else if (!base.IsListButton)
				{
					base.SetControlState(UIButton.CONTROL_STATE.NORMAL);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.RIGHT_RELEASE:
			case POINTER_INFO.INPUT_EVENT.RIGHT_TAP:
				if (ptr.type != POINTER_INFO.POINTER_TYPE.TOUCHPAD && ptr.hitInfo.collider == base.collider)
				{
					if (!base.IsListButton)
					{
						base.SetControlState(UIButton.CONTROL_STATE.OVER);
					}
				}
				else if (!base.IsListButton)
				{
					base.SetControlState(UIButton.CONTROL_STATE.NORMAL);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
				if (this.m_ctrlState != UIButton.CONTROL_STATE.OVER)
				{
					if (!base.IsListButton)
					{
						base.SetControlState(UIButton.CONTROL_STATE.OVER);
					}
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
				if (this.m_ctrlState != UIButton.CONTROL_STATE.NORMAL)
				{
					this.MouseOutEvent();
				}
				if (!base.IsListButton)
				{
					base.SetControlState(UIButton.CONTROL_STATE.NORMAL);
				}
				break;
			}
			if (!this.m_controlIsEnabled || base.IsHidden())
			{
				return;
			}
			if (ptr.evt == this.whenToInvoke)
			{
				base.Invoke("DoLinkText", this.delay);
			}
			else if (ptr.evt == POINTER_INFO.INPUT_EVENT.RIGHT_TAP)
			{
				base.Invoke("DoLinkTextRightClick", this.delay);
			}
		}

		protected void DoLinkText()
		{
			MsgHandler.Handle("DoLinkText", new object[]
			{
				this.linkTextType,
				this.Text,
				this.textKey,
				this.data
			});
		}

		protected void DoLinkTextRightClick()
		{
			MsgHandler.Handle("DoLinkTextRightClick", new object[]
			{
				this.linkTextType,
				this.Text,
				this.textKey,
				this.data
			});
		}

		public override void Copy(SpriteRoot s)
		{
			this.Copy(s, ControlCopyFlags.All);
		}

		public override void Copy(SpriteRoot s, ControlCopyFlags flags)
		{
			base.Copy(s, flags);
			if (!(s is UIBtnWWW))
			{
				return;
			}
			LinkText linkText = (LinkText)s;
			if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
			{
				this.linkTextType = linkText.linkTextType;
			}
		}

		public void MouseOutEvent()
		{
			MsgHandler.Handle("CloseToolTip", new object[0]);
		}

		public new static LinkText Create(string name, Vector3 pos)
		{
			return (LinkText)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(LinkText));
		}

		public new static LinkText Create(string name, Vector3 pos, Quaternion rotation)
		{
			return (LinkText)new GameObject(name)
			{
				transform = 
				{
					position = pos,
					rotation = rotation
				}
			}.AddComponent(typeof(LinkText));
		}
	}
}
