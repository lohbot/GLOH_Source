using System;
using UnityEngine;

public class GUICamera : MonoBehaviour
{
	private static float m_f32Width;

	private static float m_f32Height;

	private int m_si32CurrentScreenWidth;

	private int m_si32CurrentScreenHeight;

	private float m_fFixedOrthographicSize = 361f;

	private static GameObject m_goCamera;

	protected static int ScreenWidth;

	protected static int ScreenHeight;

	public static float height
	{
		get
		{
			return GUICamera.m_f32Height;
		}
	}

	public static float width
	{
		get
		{
			return GUICamera.m_f32Width;
		}
	}

	public static int UILayer
	{
		get
		{
			return 31;
		}
	}

	public virtual void SetScreenSize()
	{
		GUICamera.ScreenWidth = Screen.width;
		GUICamera.ScreenHeight = Screen.height;
		if (Screen.width == 1280)
		{
			this.m_fFixedOrthographicSize = (float)(Screen.height / 2) + 1f;
		}
	}

	public virtual void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		this.SetScreenSize();
		this.InitCameraPosition();
	}

	public void Start()
	{
	}

	public bool InitCameraPosition()
	{
		if (this.m_si32CurrentScreenWidth != Screen.width || this.m_si32CurrentScreenHeight != Screen.height)
		{
			Camera camera = (Camera)base.gameObject.GetComponent(typeof(Camera));
			if (null != camera)
			{
				this.m_si32CurrentScreenWidth = Screen.width;
				this.m_si32CurrentScreenHeight = Screen.height;
				if (TsPlatform.IsWeb)
				{
					if (Screen.width == 1024)
					{
						camera.orthographicSize = this.m_fFixedOrthographicSize * 1.31f;
					}
					else
					{
						camera.orthographicSize = this.m_fFixedOrthographicSize;
						camera.aspect = 1.7777f;
					}
				}
				else if (TsPlatform.IsMobile)
				{
					if (TsPlatform.IsAndroid)
					{
						if (Screen.width == 1024 || Screen.width == 2048)
						{
							camera.orthographicSize = this.m_fFixedOrthographicSize * 1.31f;
						}
						else
						{
							camera.orthographicSize = this.m_fFixedOrthographicSize;
							camera.aspect = 1.7777f;
						}
					}
					else if (Screen.width == 960)
					{
						camera.orthographicSize = this.m_fFixedOrthographicSize * 1.2f;
					}
					else if (Screen.width == 1024 || Screen.width == 2048)
					{
						camera.orthographicSize = this.m_fFixedOrthographicSize * 1.35f;
					}
					else
					{
						camera.orthographicSize = this.m_fFixedOrthographicSize;
						camera.aspect = 1.7777f;
					}
				}
				Vector3 position = new Vector3(0f, 0f, camera.transform.position.z);
				position.x = camera.aspect * camera.orthographicSize;
				position.y = -camera.orthographicSize;
				camera.transform.position = position;
				GUICamera.m_f32Height = camera.orthographicSize * 2f;
				GUICamera.m_f32Width = GUICamera.m_f32Height * camera.aspect;
				return true;
			}
		}
		return false;
	}

	public virtual void Update()
	{
		if (this.InitCameraPosition())
		{
		}
	}

	public static Vector3 WorldToEZ(Vector3 Pos)
	{
		Camera main = Camera.main;
		if (null != main)
		{
			Vector3 rhs = Pos - main.transform.position;
			float num = Vector3.Dot(main.transform.forward, rhs);
			if (num < 0f)
			{
				float y = -2000f;
				Pos = Vector3.zero;
				Pos.y = y;
				return Pos;
			}
			Pos = main.WorldToScreenPoint(Pos);
		}
		Pos.y = (float)Screen.height - Pos.y;
		Pos = GUICamera.ScreenToGUIPoint(Pos);
		return Pos;
	}

	public static Vector3 ScreenToGUIPoint(Vector3 v3ScreenPoint)
	{
		Vector3 result = new Vector3(0f, 0f);
		result.x = 1f / (float)Screen.width * v3ScreenPoint.x;
		result.x *= GUICamera.width;
		result.y = 1f / (float)Screen.height * v3ScreenPoint.y;
		result.y *= GUICamera.height;
		return result;
	}

	public static void ShowUI_Toggle()
	{
		if (null == GUICamera.m_goCamera)
		{
			GUICamera.m_goCamera = GameObject.Find("UI Camera");
			if (null == GUICamera.m_goCamera)
			{
				return;
			}
		}
		GUICamera.m_goCamera.SetActive(!GUICamera.m_goCamera.activeInHierarchy);
	}
}
