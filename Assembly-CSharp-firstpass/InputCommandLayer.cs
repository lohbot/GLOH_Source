using System;

public class InputCommandLayer
{
	private KeyInputDelegate keyInputDelegate;

	private InputDelegate noChangeDelegate;

	private InputDelegate moveDelegate;

	private InputDelegate pressInputDelegate;

	private InputDelegate doublePressInputDelegate;

	private InputDelegate middlePressInputDelegate;

	private InputDelegate rightPressInputDelegate;

	private InputDelegate bothPressInputDelegate;

	private InputDelegate holdPressInputDelegate;

	private InputDelegate releaseInputDelegate;

	private InputDelegate rightReleaseTapInputDelegate;

	private InputDelegate tapInputDelegate;

	private InputDelegate rightTapInputDelegate;

	private InputDelegate dragInputDelegate;

	private InputDelegate mouseWheelInputDelegate;

	private InputDelegate longTapInputDelegate;

	private InputDelegate rotationInputDelegate;

	private InputDelegate twoTouchDragInputDelegate;

	private InputDelegate twoTouchTapInputDelegate;

	private InputDelegate touchDragLeftInputDelegate;

	private InputDelegate touchDragRightInputDelegate;

	private InputDelegate touchDragUpInputDelegate;

	private InputDelegate touchDragDownInputDelegate;

	public virtual void InitCommandLayer()
	{
	}

	public virtual bool PreCheckUpdate()
	{
		return true;
	}

	public virtual bool Update(INPUT_INFO curInput)
	{
		if (this.PreCheckUpdate())
		{
			this.ExcuteKeyInputDelegate();
			this.ExcuteInputDelegate(curInput);
		}
		return false;
	}

	public void ExcuteKeyInputDelegate()
	{
		if (this.keyInputDelegate != null)
		{
			this.keyInputDelegate();
		}
	}

	public void ExcuteInputDelegate(INPUT_INFO curInput)
	{
		switch (curInput.evt)
		{
		case INPUT_INFO.INPUT_EVENT.NO_CHANGE:
			if (this.noChangeDelegate != null)
			{
				this.noChangeDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.PRESS:
			if (this.pressInputDelegate != null)
			{
				this.pressInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.DOUBLE_PRESS:
			if (this.doublePressInputDelegate != null)
			{
				this.doublePressInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.MIDDLE_PRESS:
			if (this.middlePressInputDelegate != null)
			{
				this.middlePressInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.RIGHT_PRESS:
			if (this.rightPressInputDelegate != null)
			{
				this.rightPressInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.BOTH_PRESS:
			if (this.bothPressInputDelegate != null)
			{
				this.bothPressInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.HOLD_PRESS:
			if (this.holdPressInputDelegate != null)
			{
				this.holdPressInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.RELEASE:
			if (this.releaseInputDelegate != null)
			{
				this.releaseInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.RIGHT_RELEASE:
			if (this.rightReleaseTapInputDelegate != null)
			{
				this.rightReleaseTapInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.TAP:
			if (this.tapInputDelegate != null)
			{
				this.tapInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.RIGHT_TAP:
			if (this.rightTapInputDelegate != null)
			{
				this.rightTapInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.MOVE:
			if (this.moveDelegate != null)
			{
				this.moveDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.DRAG:
			if (this.dragInputDelegate != null)
			{
				this.dragInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.MOUSE_WHEEL:
			if (this.mouseWheelInputDelegate != null)
			{
				this.mouseWheelInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.TOUCH_DRAG_DOWN:
			if (this.touchDragDownInputDelegate != null)
			{
				this.touchDragDownInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.TOUCH_DRAG_UP:
			if (this.touchDragUpInputDelegate != null)
			{
				this.touchDragUpInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.TOUCH_DRAG_LEFT:
			if (this.touchDragLeftInputDelegate != null)
			{
				this.touchDragLeftInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.TOUCH_DRAG_RIGHT:
			if (this.touchDragRightInputDelegate != null)
			{
				this.touchDragRightInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.LONG_TAP:
			if (this.longTapInputDelegate != null)
			{
				this.longTapInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.ROTATION:
			if (this.rotationInputDelegate != null)
			{
				this.rotationInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.TWO_TOUCH_DRAG:
			if (this.twoTouchDragInputDelegate != null)
			{
				this.twoTouchDragInputDelegate();
			}
			break;
		case INPUT_INFO.INPUT_EVENT.TWO_TOUCH_TAP:
			if (this.twoTouchTapInputDelegate != null)
			{
				this.twoTouchTapInputDelegate();
			}
			break;
		}
	}

	public void AddKeyInputDelegate(KeyInputDelegate del)
	{
		this.keyInputDelegate = (KeyInputDelegate)Delegate.Combine(this.keyInputDelegate, del);
	}

	public void AddNoChangeInputDelegate(InputDelegate del)
	{
		this.noChangeDelegate = (InputDelegate)Delegate.Combine(this.noChangeDelegate, del);
	}

	public void AddMoveInputDelegate(InputDelegate del)
	{
		this.moveDelegate = (InputDelegate)Delegate.Combine(this.moveDelegate, del);
	}

	public void AddPressInputDelegate(InputDelegate del)
	{
		this.pressInputDelegate = (InputDelegate)Delegate.Combine(this.pressInputDelegate, del);
	}

	public void AddDoublePressInputDelegate(InputDelegate del)
	{
		this.doublePressInputDelegate = (InputDelegate)Delegate.Combine(this.doublePressInputDelegate, del);
	}

	public void AddMiddlePressInputDelegate(InputDelegate del)
	{
		this.middlePressInputDelegate = (InputDelegate)Delegate.Combine(this.middlePressInputDelegate, del);
	}

	public void AddRightPressInputDelegate(InputDelegate del)
	{
		this.rightPressInputDelegate = (InputDelegate)Delegate.Combine(this.rightPressInputDelegate, del);
	}

	public void AddBothPressInputDelegate(InputDelegate del)
	{
		this.bothPressInputDelegate = (InputDelegate)Delegate.Combine(this.bothPressInputDelegate, del);
	}

	public void AddHoldPressInputDelegate(InputDelegate del)
	{
		this.holdPressInputDelegate = (InputDelegate)Delegate.Combine(this.holdPressInputDelegate, del);
	}

	public void AddReleaseInputDelegate(InputDelegate del)
	{
		this.releaseInputDelegate = (InputDelegate)Delegate.Combine(this.releaseInputDelegate, del);
	}

	public void AddRightReleaseInputDelegate(InputDelegate del)
	{
		this.rightReleaseTapInputDelegate = (InputDelegate)Delegate.Combine(this.rightReleaseTapInputDelegate, del);
	}

	public void AddTapInputDelegate(InputDelegate del)
	{
		this.tapInputDelegate = (InputDelegate)Delegate.Combine(this.tapInputDelegate, del);
	}

	public void AddRightTapInputDelegate(InputDelegate del)
	{
		this.rightTapInputDelegate = (InputDelegate)Delegate.Combine(this.rightTapInputDelegate, del);
	}

	public void AddDragInputDelegate(InputDelegate del)
	{
		this.dragInputDelegate = (InputDelegate)Delegate.Combine(this.dragInputDelegate, del);
	}

	public void AddMouseWheelInputDelegate(InputDelegate del)
	{
		this.mouseWheelInputDelegate = (InputDelegate)Delegate.Combine(this.mouseWheelInputDelegate, del);
	}

	public void AddLongTapInputDelegate(InputDelegate del)
	{
		this.longTapInputDelegate = (InputDelegate)Delegate.Combine(this.longTapInputDelegate, del);
	}

	public void AddRotationInputDelegate(InputDelegate del)
	{
		this.rotationInputDelegate = (InputDelegate)Delegate.Combine(this.rotationInputDelegate, del);
	}

	public void AddTwoTouchDragInputDelegate(InputDelegate del)
	{
		this.twoTouchDragInputDelegate = (InputDelegate)Delegate.Combine(this.twoTouchDragInputDelegate, del);
	}

	public void AddTwoTouchTapInputDelegate(InputDelegate del)
	{
		this.twoTouchTapInputDelegate = (InputDelegate)Delegate.Combine(this.twoTouchTapInputDelegate, del);
	}

	public void AddTouchDragLeftInputDelegate(InputDelegate del)
	{
		this.touchDragLeftInputDelegate = (InputDelegate)Delegate.Combine(this.touchDragLeftInputDelegate, del);
	}

	public void AddTouchDragRightInputDelegate(InputDelegate del)
	{
		this.touchDragRightInputDelegate = (InputDelegate)Delegate.Combine(this.touchDragRightInputDelegate, del);
	}

	public void AddTouchDragUpInputDelegate(InputDelegate del)
	{
		this.touchDragUpInputDelegate = (InputDelegate)Delegate.Combine(this.touchDragUpInputDelegate, del);
	}

	public void AddTouchDragDownInputDelegate(InputDelegate del)
	{
		this.touchDragDownInputDelegate = (InputDelegate)Delegate.Combine(this.touchDragDownInputDelegate, del);
	}
}
