using System;
using UnityEngine;
using UnityForms;

public class JoyStickDlg : Form
{
	private DrawTexture backImage;

	private Button control;

	private CharMoveCommandLayer m_pkCharMove;

	private bool bDragMoveStarted;

	private Vector3 vStickPos = Vector3.zero;

	private Vector3 vStickDir = Vector3.zero;

	private float fLimitDistance;

	private Vector3 vClickedPos = Vector3.zero;

	private Rect rect;

	private Vector3 oldPos = Vector3.zero;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "System/DLG_Joystick", G_ID.JOYSTICK_DLG, false);
		base.DonotDepthChange(UIPanelManager.UI_DEPTH);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.backImage = (base.GetControl("DrawTexture_JoystickBG") as DrawTexture);
		this.backImage.Visible = false;
		this.control = (base.GetControl("DrawTexture_Joystick") as Button);
		if (!TsPlatform.IsEditor && TsPlatform.IsAndroid)
		{
			this.control.AddInputDelegate(new EZInputDelegate(this.Mobileinput));
		}
		else
		{
			this.control.AddDragDropDelegate(new EZDragDropDelegate(this.EditorDragDrop));
			this.control.AddInputDelegate(new EZInputDelegate(this.Editorinput));
			this.control.isDraggable = true;
		}
		this.control.MouseOffset = this.control.GetSize().x / 2f;
		this.control.EffectAni = false;
		this.control.SetColor(new Color(1f, 1f, 1f, 0.5f));
		this.control.DeleteSpriteText();
		this.oldPos = this.control.GetLocation();
		this.m_pkCharMove = NrTSingleton<NrMainSystem>.Instance.GetCharMoveCommandLayer();
		this.vStickPos = this.control.transform.localPosition;
		this.fLimitDistance = this.control.GetSize().x / 2f;
		base.SetLocation(100f, GUICamera.height / 2f - base.GetSizeY() / 2f + 100f);
		this.rect = new Rect(base.GetLocation().x, base.GetLocationY(), base.GetSizeX(), base.GetSizeY());
	}

	private void Mobileinput(ref POINTER_INFO ptr)
	{
		POINTER_INFO.INPUT_EVENT evt = ptr.evt;
		if (evt == POINTER_INFO.INPUT_EVENT.PRESS)
		{
			this.vClickedPos = ptr.origPos;
			this.SetJoystickEnable(true);
		}
	}

	public override void Show()
	{
		base.Show();
		base.SetLocation(100f, GUICamera.height / 2f - base.GetSizeY() / 2f + 100f);
	}

	public override void Update()
	{
		base.Update();
		if (!this.bDragMoveStarted)
		{
			return;
		}
		if (this.m_pkCharMove == null)
		{
			return;
		}
		this.UpdateJoystick();
		this.m_pkCharMove.SetMoveDir(ref this.vStickDir);
		this.m_pkCharMove.DragMoveUpdate(true);
	}

	public void SetJoystickEnable(bool bEnabled)
	{
		if (this.m_pkCharMove != null)
		{
			if (bEnabled)
			{
				this.m_pkCharMove.StartDragMove();
			}
			else
			{
				this.m_pkCharMove.InitDragMove();
				this.m_pkCharMove.NoChangeMove();
			}
		}
		if (!bEnabled)
		{
			this.control.SetColor(new Color(1f, 1f, 1f, 0.5f));
			this.control.transform.localPosition = this.oldPos;
		}
		else
		{
			this.control.SetColor(new Color(1f, 1f, 1f, 1f));
		}
		this.backImage.Visible = bEnabled;
		this.bDragMoveStarted = bEnabled;
		NkInputManager.SetJoyStick(bEnabled);
	}

	public bool ContainPos(Vector2 pos)
	{
		return this.rect.Contains(pos);
	}

	public void UpdateJoystick()
	{
		if (TsPlatform.IsEditor)
		{
			return;
		}
		if (!TsPlatform.IsAndroid)
		{
			return;
		}
		if (!this.bDragMoveStarted)
		{
			return;
		}
		Touch touchByClicked = this.GetTouchByClicked(this.vClickedPos);
		this.SetJoyStickPosition(touchByClicked.position);
		if (touchByClicked.phase == TouchPhase.Moved || touchByClicked.phase == TouchPhase.Stationary)
		{
			CharMoveCommandLayer.bDragMove = true;
			this.vStickDir = this.control.transform.localPosition - this.vStickPos;
			this.vStickDir = new Vector3(this.vStickDir.x, this.vStickDir.y, 0f);
			float num = Vector2.Distance(this.control.transform.localPosition, this.vStickPos);
			if (num > this.fLimitDistance)
			{
				Vector3 vector = this.vStickPos + this.vStickDir.normalized * this.fLimitDistance;
				this.control.SetLocation(vector.x, -vector.y);
			}
		}
		else if (touchByClicked.phase == TouchPhase.Canceled || touchByClicked.phase == TouchPhase.Ended)
		{
			this.SetJoystickEnable(false);
		}
	}

	private void SetJoyStickPosition(Vector3 touchPos)
	{
		GameObject gameObject = GameObject.Find("UI Camera");
		if (gameObject == null)
		{
			return;
		}
		Camera component = gameObject.GetComponent<Camera>();
		if (component == null)
		{
			return;
		}
		Vector3 a = component.ScreenToWorldPoint(touchPos);
		BoxCollider component2 = this.control.GetComponent<BoxCollider>();
		if (component2 == null)
		{
			return;
		}
		a -= component2.center;
		this.control.transform.position = new Vector3(a.x, a.y, 0f);
	}

	private Touch GetTouchByClicked(Vector3 clickedPos)
	{
		Touch result = default(Touch);
		for (int i = 0; i < Input.touchCount; i++)
		{
			result = Input.GetTouch(i);
			if (result.rawPosition.x == clickedPos.x && result.rawPosition.y == clickedPos.y)
			{
				break;
			}
		}
		return result;
	}

	private void Editorinput(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.PRESS:
			this.SetJoystickEnable(true);
			break;
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
		case POINTER_INFO.INPUT_EVENT.LONG_TAP:
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			this.SetJoystickEnable(false);
			break;
		}
	}

	public void EditorDragDrop(EZDragDropParams dragDropParams)
	{
		if (dragDropParams.evt == EZDragDropEvent.Begin)
		{
			this.SetJoystickEnable(true);
			this.vStickDir = this.control.transform.localPosition - this.vStickPos;
			float num = Vector3.Distance(this.control.transform.localPosition, this.vStickPos);
			if (num > this.fLimitDistance)
			{
				Vector3 vector = this.vStickPos + this.vStickDir.normalized * this.fLimitDistance;
				this.control.SetLocation(vector.x, -vector.y);
			}
		}
		else if (dragDropParams.evt == EZDragDropEvent.Dropped)
		{
			this.SetJoystickEnable(false);
		}
		else if (dragDropParams.evt == EZDragDropEvent.Cancelled)
		{
			this.SetJoystickEnable(false);
		}
		else if (dragDropParams.evt == EZDragDropEvent.Update)
		{
			this.vStickDir = this.control.transform.localPosition - this.vStickPos;
			float num2 = Vector3.Distance(this.control.transform.localPosition, this.vStickPos);
			if (num2 > this.fLimitDistance)
			{
				Vector3 vector2 = this.vStickPos + this.vStickDir.normalized * this.fLimitDistance;
				this.control.SetLocation(vector2.x, -vector2.y);
			}
		}
	}
}
