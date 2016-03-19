using System;
using UnityEngine;

[AddComponentMenu("Character/Character Move Tester")]
public class NmCharMoveTester : MonoBehaviour
{
	private enum eANITYPE
	{
		stay1,
		walk1,
		brun1
	}

	private CharacterController kCharController;

	private Vector3 v3DestPos = Vector3.zero;

	private NmCharMoveTester.eANITYPE eCurrentAnyType;

	private Animation kAnimation;

	private bool bRun;

	private bool bForceRun;

	private float fWalkSpeed = 20f;

	private float fRunSpeed = 60f;

	private GameObject goCameraPos;

	public void Awake()
	{
	}

	public void Update()
	{
	}

	private void _AttachCharController()
	{
		if (null == base.gameObject)
		{
			return;
		}
		CapsuleCollider component = base.gameObject.GetComponent<CapsuleCollider>();
		if (null == component)
		{
			return;
		}
		this.kCharController = base.gameObject.AddComponent<CharacterController>();
		this.kCharController.radius = component.radius * component.transform.localScale.x;
		this.kCharController.center = component.center * component.transform.localScale.x;
		this.kCharController.height = component.height * component.transform.localScale.x;
		Vector3 center = this.kCharController.center;
		center.y = Mathf.Max(this.kCharController.height, this.kCharController.radius * 2f) * 0.5f;
		this.kCharController.center = center;
		UnityEngine.Object.DestroyObject(component);
		Collider[] componentsInChildren = base.gameObject.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Collider collider = componentsInChildren[i];
			if (!(collider is CharacterController))
			{
				UnityEngine.Object.DestroyObject(collider);
			}
		}
		this.kAnimation = base.gameObject.GetComponent<Animation>();
		this.v3DestPos = base.transform.position;
		this.goCameraPos = GameObject.Find("CameraPos");
		if (null == this.goCameraPos)
		{
			this.goCameraPos = new GameObject("CameraPos");
			this.goCameraPos.transform.position = base.transform.position;
		}
		if (null == Camera.main)
		{
			GameObject gameObject = new GameObject("Main Camera");
			gameObject.AddComponent<Camera>();
			gameObject.tag = "MainCamera";
		}
		maxCamera maxCamera = Camera.main.gameObject.AddComponent<maxCamera>();
		maxCamera.target = this.goCameraPos.transform;
		maxCamera.maxDistance = 10000f;
	}

	private void _CameraMove()
	{
		if (null == this.goCameraPos)
		{
			return;
		}
		Vector3 localPosition = this.goCameraPos.transform.localPosition;
		if (Input.GetKeyDown(KeyCode.Keypad8))
		{
			localPosition.y += 1f;
		}
		else if (Input.GetKeyDown(KeyCode.Keypad5))
		{
			localPosition.y -= 1f;
		}
		else if (Input.GetKeyDown(KeyCode.Keypad7))
		{
			localPosition.x += 1f;
		}
		else if (Input.GetKeyDown(KeyCode.Keypad4))
		{
			localPosition.x -= 1f;
		}
		else if (Input.GetKeyDown(KeyCode.Keypad9))
		{
			localPosition.z += 1f;
		}
		else if (Input.GetKeyDown(KeyCode.Keypad6))
		{
			localPosition.z -= 1f;
		}
		if (localPosition != this.goCameraPos.transform.localPosition)
		{
			this.goCameraPos.transform.localPosition = localPosition;
		}
	}

	private bool _KeyboardMove()
	{
		if (this.bForceRun || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			this.bRun = true;
		}
		else
		{
			this.bRun = false;
		}
		Vector3 a = Camera.main.transform.TransformDirection(Vector3.forward);
		a.y = 0f;
		a = a.normalized;
		Vector3 a2 = new Vector3(a.z, 0f, -a.x);
		float axisRaw = Input.GetAxisRaw("Vertical");
		float axisRaw2 = Input.GetAxisRaw("Horizontal");
		Vector3 vector = axisRaw2 * a2 + axisRaw * a;
		if (vector == Vector3.zero)
		{
			return false;
		}
		Vector3 vector2 = Vector3.zero;
		float num = 0.3f;
		vector2 = Vector3.RotateTowards(vector2, vector, num * 0.0174532924f * Time.deltaTime, 1f);
		vector2.Normalize();
		if (vector2 == Vector3.zero)
		{
			return false;
		}
		float deltaTime = Time.deltaTime;
		Debug.Log("DELTATIME : " + deltaTime);
		base.transform.rotation = Quaternion.LookRotation(vector2);
		float num2;
		if (this.bRun)
		{
			num2 = this.fRunSpeed / 10f;
		}
		else
		{
			num2 = this.fWalkSpeed / 10f;
		}
		num2 = deltaTime * num2;
		vector2 *= num2;
		this.v3DestPos = vector2 + base.transform.position;
		this.kCharController.Move(vector2);
		return true;
	}

	private bool _IsMoveKeyDown()
	{
		return Input.GetAxisRaw("Vertical") != 0f || Input.GetAxisRaw("Horizontal") != 0f;
	}

	private void _AnimationChange()
	{
		if (this.v3DestPos == base.transform.position && !this._IsMoveKeyDown())
		{
			this._PlayAnimation(NmCharMoveTester.eANITYPE.stay1);
		}
		else if (this.bRun)
		{
			this._PlayAnimation(NmCharMoveTester.eANITYPE.brun1);
		}
		else
		{
			this._PlayAnimation(NmCharMoveTester.eANITYPE.walk1);
		}
	}

	private void _PlayAnimation(NmCharMoveTester.eANITYPE ePlayAniType)
	{
		if (this.eCurrentAnyType == ePlayAniType)
		{
			return;
		}
		this.eCurrentAnyType = ePlayAniType;
		this.kAnimation.Play(ePlayAniType.ToString());
	}

	private void _OnGUI_Window(int nWindowID)
	{
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.Label("Walk Speed : " + this.fWalkSpeed, new GUILayoutOption[0]);
		this.fWalkSpeed = GUILayout.HorizontalScrollbar(this.fWalkSpeed, 1f, 1f, 250f, new GUILayoutOption[0]);
		this.fWalkSpeed -= this.fWalkSpeed % 1f;
		GUILayout.Label("Run Speed : " + this.fRunSpeed, new GUILayoutOption[0]);
		this.fRunSpeed = GUILayout.HorizontalScrollbar(this.fRunSpeed, 1f, 1f, 250f, new GUILayoutOption[0]);
		this.fRunSpeed -= this.fRunSpeed % 1f;
		this.bForceRun = GUILayout.Toggle(this.bForceRun, "무조건 뛰는 모드", new GUILayoutOption[0]);
		GUILayout.Space(10f);
		GUILayout.Label("-알림-", new GUILayoutOption[0]);
		GUILayout.Label("LeftShift/RightShift : 뛰기", new GUILayoutOption[0]);
		GUILayout.Label("NumPad7 : Camera X +1", new GUILayoutOption[0]);
		GUILayout.Label("NumPad4 : Camera X -1", new GUILayoutOption[0]);
		GUILayout.Label("NumPad8 : Camera Y +1", new GUILayoutOption[0]);
		GUILayout.Label("NumPad5 : Camera Y -1", new GUILayoutOption[0]);
		GUILayout.Label("NumPad9 : Camera Z +1", new GUILayoutOption[0]);
		GUILayout.Label("NumPad6 : Camera Z -1", new GUILayoutOption[0]);
		GUILayout.EndVertical();
	}
}
