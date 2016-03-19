using System;
using UnityEngine;
using UnityForms;

public class CameraController : MonoBehaviour
{
	public delegate void OnChangedMovePos(Vector3 changedPos, Transform cameraTransform);

	public Quaternion QY = Quaternion.identity;

	public Quaternion QX = Quaternion.identity;

	public float Anglex;

	public float Angley;

	public float xSpeed = 250f;

	public float ySpeed = 120f;

	public float yMinLimit = 60f;

	public float yMaxLimit = 60f;

	public float mouseWheelSpeed = 2300f;

	public float distanceMin = 12f;

	public float distanceMax = 40f;

	public float distance = 35f;

	public float ffovMin = 60f;

	public float ffovMax = 60f;

	public float fFov = 60f;

	private Vector2 Mouse3DownPoint = Vector2.zero;

	private bool DragMove;

	public float Mouse3Speed = 7000f;

	public Vector3 MovePos = Vector3.zero;

	public float MoveSpeed = 1.4f;

	private Vector3 SrcPos = Vector3.zero;

	public Rect MoveAarea = new Rect(-5000f, -5000f, 5000f, 5000f);

	public static bool bFreeCameraMode;

	public Vector3 TESTVEC = Vector3.zero;

	public bool m_bMotionBlurCamera;

	public static bool EnableControll = true;

	public CameraController.OnChangedMovePos ChangedMovePos;

	public virtual void Init()
	{
		this.StartCamera();
	}

	public void Awake()
	{
		this.SetEulerAngle(base.transform.eulerAngles);
	}

	public void SetEulerAngle(Vector3 eulerAngles)
	{
		this.Anglex = eulerAngles.y;
		this.Angley = eulerAngles.x;
		this.UpdateEulerAngle(0f, 0f);
	}

	public void Start()
	{
		this.Init();
	}

	public void SetDragMove(bool bDrag)
	{
		this.DragMove = bDrag;
	}

	public void StartCamera()
	{
		if (null != Camera.main)
		{
			Camera.main.cullingMask &= ~(1 << GUICamera.UILayer);
		}
		this.UpdateEulerAngle(0f, 0f);
		this.SetMovePos(this.GetPos());
	}

	public Vector3 GetPos()
	{
		return base.transform.position;
	}

	public Vector3 GetMovePos()
	{
		return this.MovePos;
	}

	public Vector3 GetSrcPos()
	{
		return this.SrcPos;
	}

	public Quaternion GetRotation()
	{
		return base.transform.rotation;
	}

	public void InitSrcPos(Vector3 _Pos)
	{
		Transform arg_19_0 = base.transform;
		this.MovePos = _Pos;
		this.SrcPos = _Pos;
		arg_19_0.position = _Pos;
	}

	public void SetMovePos(Vector3 _Pos)
	{
		if (this.CheckMoveArea(_Pos))
		{
			this.MovePos = _Pos;
			if (this.ChangedMovePos != null)
			{
				this.ChangedMovePos(_Pos, Camera.main.transform);
			}
		}
	}

	protected virtual void SetRotation(Quaternion _Rot)
	{
		base.transform.rotation = _Rot;
	}

	protected virtual void SetPosision(Vector3 _Pos)
	{
		base.transform.position = _Pos;
	}

	public bool CheckMoveArea(Vector3 _Pos)
	{
		return true;
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (CameraController.bFreeCameraMode)
		{
			return angle;
		}
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public virtual void Update()
	{
		if (this.m_bMotionBlurCamera)
		{
			this.MotonBlurUpdate();
		}
		else if (!this.m_bMotionBlurCamera)
		{
			this.UpdateFreeCamera();
			this.UpdateMouse3();
		}
	}

	private void MotonBlurUpdate()
	{
		Vector3 vector = Vector3.zero;
		Quaternion rotation = this.QY * this.QX;
		Vector3 vector2 = rotation * new Vector3(0f, 0f, -this.distance);
		vector = this.QY * vector;
		this.SetMovePos(this.GetMovePos() + vector);
		vector2 += this.GetMovePos();
		this.SetRotation(rotation);
		this.SetPosision(vector2);
		if (Camera.main)
		{
			Camera.main.fieldOfView = this.ffovMax;
		}
	}

	public void UpdateMouse3()
	{
		if (NrTSingleton<FormsManager>.Instance.IsMouseOverForm())
		{
			if (this.DragMove)
			{
				this.SetDragMove(false);
			}
			return;
		}
		if (!CameraController.EnableControll)
		{
			this.SetDragMove(false);
			return;
		}
		bool flag = NkInputManager.GetMouseButtonDown(0) || NkInputManager.GetMouseButtonDown(2);
		bool flag2 = NkInputManager.GetMouseButtonUp(0) || NkInputManager.GetMouseButtonUp(2);
		if (flag)
		{
			this.Mouse3DownPoint = NkInputManager.mousePosition;
			this.SetDragMove(true);
		}
		if (flag2)
		{
			this.SetDragMove(false);
		}
		if (this.DragMove)
		{
			Vector2 vector = NkInputManager.mousePosition - this.Mouse3DownPoint;
			this.Mouse3DownPoint = NkInputManager.mousePosition;
			Vector3 vector2 = new Vector3(-vector.x, 0f, -vector.y);
			vector2 *= 1E-05f * this.Mouse3Speed;
			this.SetMovePos(this.GetMovePos() + this.QY * vector2);
		}
	}

	private void UpdateEulerAngle(float IncreX, float IncreY)
	{
		this.Anglex += IncreX * this.xSpeed * 0.02f;
		this.Angley -= IncreY * this.ySpeed * 0.02f;
		this.Angley = CameraController.ClampAngle(this.Angley, this.yMinLimit, this.yMaxLimit);
		this.QY = Quaternion.Euler(0f, this.Anglex, 0f);
		this.QX = Quaternion.Euler(this.Angley, 0f, 0f);
	}

	public void UpdateFreeCamera()
	{
		if (NrTSingleton<FormsManager>.Instance.IsMouseOverForm())
		{
			return;
		}
		if (CameraController.EnableControll && NkInputManager.GetButton("Fire2"))
		{
			this.UpdateEulerAngle(NkInputManager.GetAxis("Mouse X"), NkInputManager.GetAxis("Mouse Y"));
		}
		Vector3 vector = Vector3.zero;
		if (CameraController.EnableControll && NrTSingleton<UIManager>.Instance.FocusObject == null)
		{
			bool flag = NkInputManager.GetKey(KeyCode.W) || NkInputManager.GetKey(KeyCode.Keypad8) || NkInputManager.GetKey(KeyCode.UpArrow);
			bool flag2 = NkInputManager.GetKey(KeyCode.S) || NkInputManager.GetKey(KeyCode.Keypad2) || NkInputManager.GetKey(KeyCode.DownArrow);
			bool flag3 = NkInputManager.GetKey(KeyCode.A) || NkInputManager.GetKey(KeyCode.Keypad4) || NkInputManager.GetKey(KeyCode.LeftArrow);
			bool flag4 = NkInputManager.GetKey(KeyCode.D) || NkInputManager.GetKey(KeyCode.Keypad6) || NkInputManager.GetKey(KeyCode.RightArrow);
			if (flag)
			{
				vector += new Vector3(0f, 0f, this.MoveSpeed);
			}
			if (flag2)
			{
				vector += new Vector3(0f, 0f, -this.MoveSpeed);
			}
			if (flag3)
			{
				vector += new Vector3(-this.MoveSpeed, 0f, 0f);
			}
			if (flag4)
			{
				vector += new Vector3(this.MoveSpeed, 0f, 0f);
			}
		}
		Quaternion rotation = this.QY * this.QX;
		Vector3 vector2 = rotation * new Vector3(0f, 0f, -this.distance);
		vector = this.QY * vector;
		this.SetMovePos(this.GetMovePos() + vector);
		vector2 += this.GetMovePos();
		if (!this.m_bMotionBlurCamera)
		{
			Vector3 pos = vector2;
			pos = NrTSingleton<NrTerrain>.Instance.GetWorldHeight(pos);
			if (vector2.y < pos.y)
			{
				vector2.y = pos.y + 1f;
			}
		}
		this.SetRotation(rotation);
		this.SetPosision(vector2);
		if (Camera.main)
		{
			Camera.main.fieldOfView = this.ffovMax;
		}
	}

	public Vector3 CalCumAnglePos(float _Dis)
	{
		Quaternion rotation = this.QY * this.QX;
		return rotation * new Vector3(0f, 0f, _Dis);
	}

	public static void AttachCamera(GameObject _Child)
	{
		if (_Child && _Child.transform)
		{
			_Child.transform.parent = Camera.main.transform;
		}
	}

	public void SetDistance(float fDistance)
	{
		this.distance += fDistance;
		if (this.distanceMin > this.distance)
		{
			this.distance = this.distanceMin;
		}
		if (this.distanceMax < this.distance)
		{
			this.distance = this.distanceMax;
		}
	}

	public float GetDistance()
	{
		return this.distance;
	}

	public virtual float GetZoomRatePerMin()
	{
		return this.distanceMin / this.distance;
	}
}
