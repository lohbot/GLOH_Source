using System;
using UnityEngine;

public struct INPUT_INFO
{
	public enum INPUT_EVENT
	{
		NO_CHANGE,
		PRESS,
		DOUBLE_PRESS,
		MIDDLE_PRESS,
		RIGHT_PRESS,
		BOTH_PRESS,
		HOLD_PRESS,
		RELEASE,
		RIGHT_RELEASE,
		TAP,
		RIGHT_TAP,
		MOVE,
		MOVE_OFF,
		RELEASE_OFF,
		DRAG,
		MOUSE_WHEEL,
		TOUCH_DRAG_DOWN,
		TOUCH_DRAG_UP,
		TOUCH_DRAG_LEFT,
		TOUCH_DRAG_RIGHT,
		LONG_TAP,
		ROTATION,
		TWO_TOUCH_DRAG,
		TWO_TOUCH_TAP,
		PINCHZOOM
	}

	public enum INPUT_TYPE
	{
		MOUSE = 1,
		TOUCHPAD
	}

	public enum ResolutionType
	{
		Normal,
		HD,
		FHD,
		QHD,
		WQXGA
	}

	public INPUT_INFO.INPUT_TYPE type;

	public int id;

	public int actionID;

	public INPUT_INFO.INPUT_EVENT evt;

	public bool active;

	public Vector3 devicePos;

	public Vector3 origPos;

	public Vector3 inputDelta;

	public bool isTap;

	public bool isGesture;

	public float clickTime;

	public bool isDrag;

	public void Copy(INPUT_INFO ptr)
	{
		this.type = ptr.type;
		this.id = ptr.id;
		this.actionID = ptr.actionID;
		this.evt = ptr.evt;
		this.active = ptr.active;
		this.devicePos = ptr.devicePos;
		this.origPos = ptr.origPos;
		this.inputDelta = ptr.inputDelta;
		this.isTap = ptr.isTap;
		this.isGesture = ptr.isGesture;
		this.clickTime = ptr.clickTime;
		this.isDrag = ptr.isDrag;
	}

	public void Reuse(INPUT_INFO ptr)
	{
		this.evt = ptr.evt;
		this.actionID = ptr.actionID;
		this.active = ptr.active;
		this.devicePos = ptr.devicePos;
		this.origPos = ptr.origPos;
		this.inputDelta = ptr.inputDelta;
		this.isTap = ptr.isTap;
		this.isGesture = ptr.isGesture;
		this.clickTime = ptr.clickTime;
		this.isDrag = ptr.isDrag;
	}

	public void Reset(int actID)
	{
		this.actionID = actID;
		this.evt = INPUT_INFO.INPUT_EVENT.NO_CHANGE;
		this.active = false;
		this.devicePos = Vector3.zero;
		this.origPos = Vector3.zero;
		this.inputDelta = Vector3.zero;
		this.isTap = true;
		this.isGesture = false;
		this.clickTime = 0f;
		this.isDrag = false;
	}

	public INPUT_INFO.ResolutionType getResolution()
	{
		if (Screen.width == 1280)
		{
			return INPUT_INFO.ResolutionType.HD;
		}
		if (Screen.width == 1920)
		{
			return INPUT_INFO.ResolutionType.FHD;
		}
		if (Screen.width != 2560)
		{
			return INPUT_INFO.ResolutionType.Normal;
		}
		if (Screen.height == 1600)
		{
			return INPUT_INFO.ResolutionType.WQXGA;
		}
		return INPUT_INFO.ResolutionType.QHD;
	}
}
