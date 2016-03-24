using System;
using System.Collections.Generic;
using UnityEngine;

public class NkInputManager : NrBehaviour
{
	public enum INPUT_TYPE
	{
		MOUSE,
		TOUCHPAD
	}

	private List<InputCommandLayer> listInputCommandLayer = new List<InputCommandLayer>();

	protected INPUT_INFO[,] pointers;

	protected int numPointers;

	protected int[] activePointers;

	protected int numActivePointers;

	protected InputPollerDelegate pointerPoller;

	private int numTouches = 1;

	private float scrollDelta;

	private float dragThreshold = 16f;

	private float touchDragThreshold = 50f;

	private int curActionID;

	private Vector3 tempVec = Vector3.zero;

	private bool dragClick;

	private bool leftMouseDown;

	private bool rightMouseDown;

	private bool middleMouseDown;

	private static bool _IsInputMode = true;

	private NkInputManager.INPUT_TYPE inputType;

	private static bool _autoBlockInputMode;

	private static float _blockTime;

	private static int MOUSE_LEFT_BUTTON;

	private static int MOUSE_RIGHT_BUTTON = 1;

	private int lastUpdateFrame;

	private bool m_bDoublePress;

	private float m_fDoublePressTime;

	private static bool m_bJoystick;

	private static float m_fStickMoveRange;

	public static IMECompositionMode imeCompositionMode
	{
		get
		{
			return Input.imeCompositionMode;
		}
		set
		{
			Input.imeCompositionMode = value;
		}
	}

	public static Vector3 mousePosition
	{
		get
		{
			return Input.mousePosition;
		}
	}

	public static string inputString
	{
		get
		{
			return Input.inputString;
		}
	}

	public static string compositionString
	{
		get
		{
			return Input.compositionString;
		}
	}

	public static int touchCount
	{
		get
		{
			return Input.touchCount;
		}
	}

	public static Touch[] touches
	{
		get
		{
			return Input.touches;
		}
	}

	public static bool anyKey
	{
		get
		{
			return Input.anyKey;
		}
	}

	public static bool anyKeyDown
	{
		get
		{
			return Input.anyKeyDown;
		}
	}

	public static bool IsInputMode
	{
		get
		{
			return NkInputManager._IsInputMode;
		}
		set
		{
			NkInputManager._IsInputMode = value;
		}
	}

	public static bool IsAutoBlockInputMode
	{
		get
		{
			return NkInputManager._autoBlockInputMode;
		}
		set
		{
			NkInputManager._autoBlockInputMode = value;
			NkInputManager._blockTime = Time.realtimeSinceStartup;
		}
	}

	public void AddInputCommandLayer(InputCommandLayer layer)
	{
		this.listInputCommandLayer.Add(layer);
	}

	public void RemoveInputCommandLayer(InputCommandLayer layer)
	{
		this.listInputCommandLayer.Remove(layer);
	}

	public override bool Initialize()
	{
		if (TsPlatform.IsWeb)
		{
			this.dragThreshold = 8f;
		}
		else if (TsPlatform.IsMobile)
		{
			this.dragThreshold = 32f;
		}
		this.SetupPointers();
		return true;
	}

	public static float GetAxisRaw(string axisName)
	{
		return Input.GetAxisRaw(axisName);
	}

	public static float GetAxis(string axisName)
	{
		return Input.GetAxis(axisName);
	}

	public static bool GetKeyDown(KeyCode eKey)
	{
		return TsPlatform.IsEditor && Input.GetKeyDown(eKey);
	}

	public static bool GetKeyUp(KeyCode eKey)
	{
		return TsPlatform.IsEditor && Input.GetKeyUp(eKey);
	}

	public static bool GetKey(KeyCode eKey)
	{
		return TsPlatform.IsEditor && Input.GetKey(eKey);
	}

	public static bool GetButton(string buttonName)
	{
		return Input.GetButton(buttonName);
	}

	public static bool GetMouseButton(int button)
	{
		return Input.GetMouseButton(button);
	}

	public static bool GetMouseButtonDown(int button)
	{
		return Input.GetMouseButtonDown(button);
	}

	public static bool GetMouseButtonUp(int button)
	{
		return Input.GetMouseButtonUp(button);
	}

	public static Touch GetTouch(int index)
	{
		return Input.GetTouch(index);
	}

	public static bool IsLeftButtonUP()
	{
		return Input.GetMouseButtonUp(NkInputManager.MOUSE_LEFT_BUTTON);
	}

	public static bool IsLeftButtonDOWN()
	{
		return Input.GetMouseButtonDown(NkInputManager.MOUSE_LEFT_BUTTON);
	}

	public static bool IsRightButtonUP()
	{
		return Input.GetMouseButtonUp(NkInputManager.MOUSE_RIGHT_BUTTON);
	}

	public static bool IsRightButtonDOWN()
	{
		return Input.GetMouseButtonDown(NkInputManager.MOUSE_RIGHT_BUTTON);
	}

	public void InitCommandLayer()
	{
		foreach (InputCommandLayer current in this.listInputCommandLayer)
		{
			current.InitCommandLayer();
		}
	}

	public override void Update()
	{
		if (NkInputManager._autoBlockInputMode)
		{
			if (Time.realtimeSinceStartup - NkInputManager._blockTime <= 0.3f)
			{
				return;
			}
			NkInputManager._autoBlockInputMode = false;
			NkInputManager._blockTime = Time.realtimeSinceStartup;
		}
		if (!NkInputManager._IsInputMode)
		{
			return;
		}
		if (this.lastUpdateFrame != Time.frameCount)
		{
			this.lastUpdateFrame = Time.frameCount;
			if (this.pointerPoller != null)
			{
				this.pointerPoller();
			}
			foreach (InputCommandLayer current in this.listInputCommandLayer)
			{
				if (current != null && current.Update(this.pointers[0, 0]))
				{
					break;
				}
			}
			return;
		}
	}

	protected void SetupPointers()
	{
		this.numTouches = 1;
		this.inputType = NkInputManager.INPUT_TYPE.MOUSE;
		if (TsPlatform.IsMobile)
		{
			if (TsPlatform.IsEditor || TsPlatform.IsAndroid)
			{
				this.inputType = NkInputManager.INPUT_TYPE.MOUSE;
			}
			else
			{
				this.inputType = NkInputManager.INPUT_TYPE.TOUCHPAD;
				this.numTouches = 5;
			}
		}
		this.pointers = new INPUT_INFO[1, this.numTouches];
		this.activePointers = new int[this.numTouches];
		NkInputManager.INPUT_TYPE iNPUT_TYPE = this.inputType;
		if (iNPUT_TYPE != NkInputManager.INPUT_TYPE.MOUSE)
		{
			if (iNPUT_TYPE == NkInputManager.INPUT_TYPE.TOUCHPAD)
			{
				this.pointerPoller = new InputPollerDelegate(this.PollTouchpad);
				for (int i = 0; i < this.numTouches; i++)
				{
					this.pointers[0, i].id = i;
					this.pointers[0, i].type = INPUT_INFO.INPUT_TYPE.TOUCHPAD;
				}
			}
		}
		else
		{
			this.pointerPoller = new InputPollerDelegate(this.PollMouse);
			this.activePointers[0] = 0;
			this.numActivePointers = 1;
			this.pointers[0, 0].id = 0;
			this.pointers[0, 0].type = INPUT_INFO.INPUT_TYPE.MOUSE;
		}
	}

	private void PollMouse()
	{
		this.pointers[0, 0].evt = INPUT_INFO.INPUT_EVENT.NO_CHANGE;
		this.PollMouse(ref this.pointers[0, 0]);
	}

	protected void PollMouse(ref INPUT_INFO curPtr)
	{
		if (this.PollMouseDown(ref curPtr))
		{
			curPtr.devicePos = Input.mousePosition;
		}
		else if (this.PollMouseUp(ref curPtr))
		{
			curPtr.devicePos = Input.mousePosition;
		}
		else if (this.PollMouseOthers(ref curPtr))
		{
			curPtr.devicePos = Input.mousePosition;
		}
	}

	protected bool PollMouseDown(ref INPUT_INFO curPtr)
	{
		this.leftMouseDown = Input.GetMouseButton(0);
		this.rightMouseDown = Input.GetMouseButton(1);
		this.middleMouseDown = Input.GetMouseButton(2);
		if (this.leftMouseDown && this.rightMouseDown)
		{
			curPtr.Reset(this.curActionID++);
			curPtr.evt = INPUT_INFO.INPUT_EVENT.BOTH_PRESS;
			curPtr.active = true;
			curPtr.inputDelta = Input.mousePosition - curPtr.devicePos;
			curPtr.origPos = Input.mousePosition;
			curPtr.isTap = true;
			curPtr.clickTime = Time.time;
			curPtr.isDrag = false;
			return true;
		}
		if (this.leftMouseDown && !curPtr.active)
		{
			if (curPtr.clickTime > 0f && Time.time - curPtr.clickTime < 0.3f)
			{
				curPtr.Reset(this.curActionID++);
				curPtr.evt = INPUT_INFO.INPUT_EVENT.DOUBLE_PRESS;
				curPtr.active = true;
				curPtr.inputDelta = Input.mousePosition - curPtr.devicePos;
				curPtr.origPos = Input.mousePosition;
				curPtr.isTap = true;
				curPtr.clickTime = Time.time;
			}
			else
			{
				curPtr.Reset(this.curActionID++);
				curPtr.evt = INPUT_INFO.INPUT_EVENT.PRESS;
				curPtr.active = true;
				curPtr.inputDelta = Input.mousePosition - curPtr.devicePos;
				curPtr.origPos = Input.mousePosition;
				curPtr.isTap = true;
				curPtr.clickTime = Time.time;
			}
			return true;
		}
		if (this.rightMouseDown && !curPtr.active)
		{
			curPtr.Reset(this.curActionID++);
			curPtr.evt = INPUT_INFO.INPUT_EVENT.RIGHT_PRESS;
			curPtr.active = true;
			curPtr.inputDelta = Input.mousePosition - curPtr.devicePos;
			curPtr.origPos = Input.mousePosition;
			curPtr.isTap = true;
			return true;
		}
		if (this.middleMouseDown && !curPtr.active)
		{
			curPtr.Reset(this.curActionID++);
			curPtr.evt = INPUT_INFO.INPUT_EVENT.MIDDLE_PRESS;
			curPtr.active = true;
			curPtr.inputDelta = Input.mousePosition - curPtr.devicePos;
			curPtr.origPos = Input.mousePosition;
			curPtr.isTap = true;
			return true;
		}
		return false;
	}

	protected bool PollMouseUp(ref INPUT_INFO curInput)
	{
		if (curInput.active)
		{
			if (Input.GetMouseButtonUp(0))
			{
				curInput.inputDelta = NkInputManager.mousePosition - curInput.devicePos;
				curInput.devicePos = NkInputManager.mousePosition;
				if (curInput.isTap)
				{
					this.tempVec = curInput.origPos - curInput.devicePos;
					if (Mathf.Abs(this.tempVec.x) > this.dragThreshold || Mathf.Abs(this.tempVec.y) > this.dragThreshold)
					{
						curInput.isTap = false;
					}
				}
				if (curInput.isTap)
				{
					curInput.evt = INPUT_INFO.INPUT_EVENT.TAP;
				}
				else if (this.dragClick)
				{
					curInput.evt = INPUT_INFO.INPUT_EVENT.TAP;
					this.dragClick = false;
				}
				else
				{
					curInput.evt = INPUT_INFO.INPUT_EVENT.RELEASE;
				}
				curInput.active = false;
				return true;
			}
			if (Input.GetMouseButtonUp(1))
			{
				curInput.inputDelta = NkInputManager.mousePosition - curInput.devicePos;
				curInput.devicePos = NkInputManager.mousePosition;
				if (curInput.isTap)
				{
					this.tempVec = curInput.origPos - curInput.devicePos;
					if (Mathf.Abs(this.tempVec.x) > this.dragThreshold || Mathf.Abs(this.tempVec.y) > this.dragThreshold)
					{
						curInput.isTap = false;
					}
				}
				if (curInput.isTap)
				{
					curInput.evt = INPUT_INFO.INPUT_EVENT.RIGHT_TAP;
				}
				else
				{
					curInput.evt = INPUT_INFO.INPUT_EVENT.RIGHT_RELEASE;
				}
				curInput.active = false;
				return true;
			}
		}
		return false;
	}

	private bool PollMouseOthers(ref INPUT_INFO curPtr)
	{
		if (Input.mousePosition != curPtr.devicePos)
		{
			if (curPtr.active && curPtr.isDrag)
			{
				curPtr.evt = INPUT_INFO.INPUT_EVENT.DRAG;
				curPtr.inputDelta = NkInputManager.mousePosition - curPtr.devicePos;
				curPtr.devicePos = NkInputManager.mousePosition;
				if (curPtr.isTap)
				{
					this.tempVec = curPtr.origPos - curPtr.devicePos;
					if (Mathf.Abs(this.tempVec.x) > this.dragThreshold || Mathf.Abs(this.tempVec.y) > this.dragThreshold)
					{
						curPtr.isTap = false;
					}
				}
			}
			else
			{
				curPtr.isDrag = true;
				Vector3 devicePos = NkInputManager.mousePosition;
				if (curPtr.getResolution() != INPUT_INFO.ResolutionType.Normal && 3f > Mathf.Abs(NkInputManager.mousePosition.x - curPtr.devicePos.x) && 3f > Mathf.Abs(NkInputManager.mousePosition.y - curPtr.devicePos.y))
				{
					curPtr.isDrag = false;
					devicePos = curPtr.devicePos;
				}
				curPtr.evt = INPUT_INFO.INPUT_EVENT.MOVE;
				curPtr.inputDelta = NkInputManager.mousePosition - curPtr.devicePos;
				curPtr.devicePos = devicePos;
			}
		}
		else if (!this.leftMouseDown || !this.rightMouseDown)
		{
			curPtr.evt = INPUT_INFO.INPUT_EVENT.NO_CHANGE;
			curPtr.inputDelta = Vector3.zero;
		}
		this.scrollDelta = Input.GetAxis("Mouse ScrollWheel");
		if (this.scrollDelta != 0f)
		{
			curPtr.evt = INPUT_INFO.INPUT_EVENT.MOUSE_WHEEL;
		}
		return true;
	}

	public INPUT_INFO GetInputInfo(int index)
	{
		if (index >= this.numActivePointers)
		{
			return default(INPUT_INFO);
		}
		return this.pointers[0, index];
	}

	public void GetInputInfo(int index, ref INPUT_INFO info)
	{
		info.Copy(this.pointers[0, index]);
	}

	private void PollTouchpad()
	{
		if (TsPlatform.IsMobile)
		{
			this.pointers[0, 0].evt = INPUT_INFO.INPUT_EVENT.NO_CHANGE;
			this.numActivePointers = Mathf.Min(this.numTouches, Input.touchCount);
			for (int i = 0; i < this.numActivePointers; i++)
			{
				Touch touch = Input.GetTouch(i);
				int num = touch.fingerId;
				if (num >= this.numTouches)
				{
					num = this.numTouches - 1;
				}
				this.activePointers[i] = num;
				switch (touch.phase)
				{
				case TouchPhase.Began:
					this.pointers[0, num].Reset(this.curActionID++);
					this.pointers[0, num].evt = INPUT_INFO.INPUT_EVENT.PRESS;
					this.pointers[0, num].active = true;
					this.pointers[0, num].inputDelta = Vector3.zero;
					this.pointers[0, num].origPos = touch.position;
					this.pointers[0, num].isTap = true;
					this.pointers[0, num].clickTime = Time.time;
					break;
				case TouchPhase.Moved:
					this.pointers[0, num].evt = INPUT_INFO.INPUT_EVENT.DRAG;
					this.pointers[0, num].inputDelta = touch.deltaPosition;
					this.pointers[0, num].devicePos = touch.position;
					if (this.pointers[0, num].isTap)
					{
						this.tempVec = this.pointers[0, num].origPos - this.pointers[0, num].devicePos;
						if (Mathf.Abs(this.tempVec.x) > this.dragThreshold || Mathf.Abs(this.tempVec.y) > this.dragThreshold)
						{
							this.pointers[0, num].isTap = false;
						}
					}
					break;
				case TouchPhase.Stationary:
					if (Time.time - this.pointers[0, num].clickTime > 0.2f)
					{
						this.pointers[0, num].evt = INPUT_INFO.INPUT_EVENT.HOLD_PRESS;
						this.pointers[0, num].inputDelta = Vector3.zero;
					}
					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					this.pointers[0, num].devicePos = touch.position;
					this.tempVec = this.pointers[0, num].origPos - this.pointers[0, num].devicePos;
					if (Mathf.Abs(this.tempVec.x) >= this.touchDragThreshold || Mathf.Abs(this.tempVec.y) > this.touchDragThreshold)
					{
						this.pointers[0, num].isGesture = true;
					}
					if (this.pointers[0, num].isTap && Input.touchCount == 1)
					{
						if (this.m_bDoublePress && Time.time - this.m_fDoublePressTime < 0.3f)
						{
							this.pointers[0, num].evt = INPUT_INFO.INPUT_EVENT.DOUBLE_PRESS;
							this.m_bDoublePress = false;
							this.m_fDoublePressTime = 0f;
						}
						else if (Time.time - this.pointers[0, num].clickTime > 2f)
						{
							this.pointers[0, num].evt = INPUT_INFO.INPUT_EVENT.LONG_TAP;
							this.m_bDoublePress = false;
							this.m_fDoublePressTime = 0f;
						}
						else
						{
							this.pointers[0, num].evt = INPUT_INFO.INPUT_EVENT.TAP;
							this.m_bDoublePress = true;
							this.m_fDoublePressTime = Time.time;
						}
					}
					else if (this.pointers[0, num].isTap && 1 < Input.touchCount)
					{
						this.pointers[0, num].evt = INPUT_INFO.INPUT_EVENT.TAP;
						this.m_bDoublePress = false;
						this.m_fDoublePressTime = 0f;
					}
					else if (!this.pointers[0, num].isTap)
					{
						this.pointers[0, num].evt = INPUT_INFO.INPUT_EVENT.RELEASE;
						this.m_bDoublePress = false;
						this.m_fDoublePressTime = 0f;
					}
					else
					{
						this.pointers[0, num].evt = INPUT_INFO.INPUT_EVENT.RELEASE;
						this.m_bDoublePress = false;
						this.m_fDoublePressTime = 0f;
					}
					this.pointers[0, num].inputDelta = touch.deltaPosition;
					this.pointers[0, num].active = false;
					break;
				}
				this.pointers[0, num].devicePos = touch.position;
			}
			if (this.numActivePointers == 1)
			{
				this.pointers[0, 0].evt = this.pointers[0, this.activePointers[0]].evt;
			}
			else if (1 < this.numActivePointers)
			{
				int num2 = this.activePointers[0];
				int num3 = this.activePointers[1];
				if (this.pointers[0, num2].evt == INPUT_INFO.INPUT_EVENT.TAP && this.pointers[0, num3].evt == INPUT_INFO.INPUT_EVENT.TAP)
				{
					this.pointers[0, 0].evt = INPUT_INFO.INPUT_EVENT.TWO_TOUCH_TAP;
				}
				else if ((this.pointers[0, num2].evt == INPUT_INFO.INPUT_EVENT.HOLD_PRESS && this.pointers[0, num3].evt == INPUT_INFO.INPUT_EVENT.DRAG && !this.pointers[0, num3].isTap) || (this.pointers[0, num2].evt == INPUT_INFO.INPUT_EVENT.DRAG && !this.pointers[0, num2].isTap && this.pointers[0, num3].evt == INPUT_INFO.INPUT_EVENT.HOLD_PRESS))
				{
					this.pointers[0, 0].evt = INPUT_INFO.INPUT_EVENT.ROTATION;
				}
				else if (this.pointers[0, num2].evt == INPUT_INFO.INPUT_EVENT.DRAG && !this.pointers[0, num2].isTap && this.pointers[0, num3].evt == INPUT_INFO.INPUT_EVENT.DRAG && !this.pointers[0, num3].isTap)
				{
					this.pointers[0, 0].evt = INPUT_INFO.INPUT_EVENT.TWO_TOUCH_DRAG;
				}
			}
		}
	}

	public static void SetJoyStick(bool bSet)
	{
		if (NkInputManager.m_bJoystick != bSet)
		{
			NkInputManager.m_bJoystick = bSet;
		}
	}

	public static bool IsJoystick()
	{
		return NkInputManager.m_bJoystick;
	}

	public static void SetStickMoveRange(float fRange)
	{
		NkInputManager.m_fStickMoveRange = fRange;
	}

	public static float GetStickMoveRange()
	{
		return NkInputManager.m_fStickMoveRange;
	}
}
