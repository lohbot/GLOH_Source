using System;
using UnityEngine;

public class EventTriggerMiniCamera : MonoBehaviour
{
	public static float m_Reaction = 1.5f;

	[HideInInspector]
	private static bool _bUseControll;

	public static bool m_bControllPosition = true;

	public IEventTrigger_MiniCameraMake m_TriggerItem;

	public static bool bUseControll
	{
		get
		{
			return EventTriggerMiniCamera._bUseControll;
		}
		set
		{
			if (value)
			{
				maxCamera maxCamera = UnityEngine.Object.FindObjectOfType(typeof(maxCamera)) as maxCamera;
				if (maxCamera != null)
				{
					maxCamera.StopCameraControl();
				}
			}
			else
			{
				maxCamera maxCamera2 = UnityEngine.Object.FindObjectOfType(typeof(maxCamera)) as maxCamera;
				if (maxCamera2 != null)
				{
					maxCamera2.Init();
				}
			}
			EventTriggerMiniCamera._bUseControll = value;
		}
	}

	public void Update()
	{
		if (!EventTriggerMiniCamera._bUseControll)
		{
			return;
		}
		if (NkInputManager.GetKeyUp(KeyCode.KeypadEnter))
		{
			EventTriggerMiniCamera.m_bControllPosition = !EventTriggerMiniCamera.m_bControllPosition;
		}
		if (NkInputManager.GetKey(KeyCode.Keypad8))
		{
			if (EventTriggerMiniCamera.m_bControllPosition)
			{
				base.gameObject.transform.localPosition += base.gameObject.transform.up.normalized * EventTriggerMiniCamera.m_Reaction;
			}
			else
			{
				base.gameObject.transform.localEulerAngles += new Vector3(-EventTriggerMiniCamera.m_Reaction, 0f, 0f);
			}
		}
		if (NkInputManager.GetKey(KeyCode.Keypad2))
		{
			if (EventTriggerMiniCamera.m_bControllPosition)
			{
				base.gameObject.transform.localPosition -= base.gameObject.transform.up.normalized * EventTriggerMiniCamera.m_Reaction;
			}
			else
			{
				base.gameObject.transform.localEulerAngles += new Vector3(EventTriggerMiniCamera.m_Reaction, 0f, 0f);
			}
		}
		if (NkInputManager.GetKey(KeyCode.Keypad4))
		{
			if (EventTriggerMiniCamera.m_bControllPosition)
			{
				base.gameObject.transform.localPosition -= base.gameObject.transform.right.normalized * EventTriggerMiniCamera.m_Reaction;
			}
			else
			{
				base.gameObject.transform.localEulerAngles += new Vector3(0f, -EventTriggerMiniCamera.m_Reaction, 0f);
			}
		}
		if (NkInputManager.GetKey(KeyCode.Keypad6))
		{
			if (EventTriggerMiniCamera.m_bControllPosition)
			{
				base.gameObject.transform.localPosition += base.gameObject.transform.right.normalized * EventTriggerMiniCamera.m_Reaction;
			}
			else
			{
				base.gameObject.transform.localEulerAngles += new Vector3(0f, EventTriggerMiniCamera.m_Reaction, 0f);
			}
		}
		if (NkInputManager.GetKey(KeyCode.KeypadPeriod))
		{
			if (EventTriggerMiniCamera.m_bControllPosition)
			{
				base.gameObject.transform.localPosition += base.gameObject.transform.forward.normalized * EventTriggerMiniCamera.m_Reaction;
			}
			else
			{
				base.gameObject.transform.localEulerAngles += new Vector3(0f, 0f, EventTriggerMiniCamera.m_Reaction);
			}
		}
		if (NkInputManager.GetKey(KeyCode.Keypad0))
		{
			if (EventTriggerMiniCamera.m_bControllPosition)
			{
				base.gameObject.transform.localPosition -= base.gameObject.transform.forward.normalized * EventTriggerMiniCamera.m_Reaction;
			}
			else
			{
				base.gameObject.transform.localEulerAngles += new Vector3(0f, 0f, -EventTriggerMiniCamera.m_Reaction);
			}
		}
		if (NkInputManager.GetKey(KeyCode.KeypadPlus))
		{
			EventTriggerMiniCamera.m_Reaction += 0.25f;
		}
		if (NkInputManager.GetKey(KeyCode.KeypadMinus))
		{
			EventTriggerMiniCamera.m_Reaction -= 0.25f;
		}
		if (NkInputManager.GetMouseButton(0))
		{
			base.gameObject.transform.localEulerAngles += new Vector3(-NkInputManager.GetAxisRaw("Mouse Y"), NkInputManager.GetAxisRaw("Mouse X"), 0f);
		}
		if (NkInputManager.GetMouseButton(1))
		{
			base.gameObject.transform.localEulerAngles += new Vector3(0f, 0f, NkInputManager.GetAxisRaw("Mouse X"));
		}
		if (NkInputManager.GetAxis("Mouse ScrollWheel") != 0f)
		{
			base.gameObject.transform.localPosition -= base.gameObject.transform.forward.normalized * NkInputManager.GetAxis("Mouse ScrollWheel");
		}
	}
}
