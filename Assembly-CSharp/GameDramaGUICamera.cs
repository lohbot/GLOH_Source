using System;
using UnityEngine;

public class GameDramaGUICamera : GUICamera
{
	private const float fAspectWIDE = 1.77777779f;

	public Camera GameDramaCamera;

	public int GameDramaViewWidth = Screen.width;

	public int GameDramaViewHeight = Screen.height;

	public float GameDramaViewXPoint;

	public float GameDramaViewYPoint;

	public int CurrentScreenWidth;

	public int CurrentScreenHeight;

	public float UICameraRectX;

	public float UICameraRectY;

	public float UICameraRectWidth = 1f;

	public float UICameraRectHeight = 1f;

	public float UICameraAspect;

	private float CorrectionCameraRectX;

	private float CorrectionCameraRectY;

	private float CorrectionCameraRectWidth = 1f;

	private float CorrectionCameraRectHeight = 1f;

	private float CorrectionCameraAspect;

	public void CameraSetting(Camera camera)
	{
		this.GameDramaCamera = camera;
		this.SetCameraViewPort(this.GameDramaCamera);
	}

	public override void Awake()
	{
		GUICamera component = base.gameObject.GetComponent<GUICamera>();
		if (component != null && !(component is GameDramaGUICamera))
		{
			UnityEngine.Object.Destroy(component);
		}
		Camera camera = base.gameObject.GetComponent(typeof(Camera)) as Camera;
		if (camera != null)
		{
			this.UICameraRectX = camera.rect.x;
			this.UICameraRectY = camera.rect.y;
			this.UICameraRectWidth = camera.rect.width;
			this.UICameraRectHeight = camera.rect.height;
			this.UICameraAspect = camera.aspect;
		}
		this.CameraCorrection();
		this.SetCameraViewPort(base.gameObject.GetComponent(typeof(Camera)) as Camera);
		GameObject gameObject = new GameObject("bgCamera");
		if (gameObject != null)
		{
			gameObject.AddComponent<GameDramaBGCamera>();
		}
		base.Awake();
	}

	public override void Update()
	{
		if (this.CurrentScreenWidth != Screen.width || this.CurrentScreenHeight != Screen.height)
		{
			this.Correction();
			base.InitCameraPosition();
		}
	}

	public override void SetScreenSize()
	{
		GUICamera.ScreenWidth = this.GameDramaViewWidth;
		GUICamera.ScreenHeight = this.GameDramaViewHeight;
		this.CurrentScreenWidth = Screen.width;
		this.CurrentScreenHeight = Screen.height;
		if (this.GameDramaViewWidth < Screen.width)
		{
			this.GameDramaViewXPoint = (float)((Screen.width - this.GameDramaViewWidth) / 2);
		}
		else
		{
			this.GameDramaViewXPoint = 0f;
		}
		if (this.GameDramaViewHeight < Screen.height)
		{
			this.GameDramaViewYPoint = (float)((Screen.height - this.GameDramaViewHeight) / 2);
		}
		else
		{
			this.GameDramaViewYPoint = 0f;
		}
	}

	private void CameraCorrection()
	{
		this.GameDramaViewWidth = Screen.width;
		this.GameDramaViewHeight = Screen.height;
		this.CorrectionCameraAspect = (float)this.GameDramaViewWidth / (float)this.GameDramaViewHeight;
		if (this.CorrectionCameraAspect != 1.77777779f)
		{
			this.GameDramaViewHeight = Screen.width * 9 / 16;
			this.GameDramaViewWidth = Screen.height * 16 / 9;
			if (this.GameDramaViewHeight > Screen.height)
			{
				this.CorrectionCameraRectWidth = (float)this.GameDramaViewWidth / (float)Screen.width;
				this.CorrectionCameraRectX = (1f - this.CorrectionCameraRectWidth) / 2f;
				this.GameDramaViewHeight = Screen.height;
			}
			else
			{
				this.CorrectionCameraRectHeight = (float)this.GameDramaViewHeight / (float)Screen.height;
				this.CorrectionCameraRectY = (1f - this.CorrectionCameraRectHeight) / 2f;
				this.GameDramaViewWidth = Screen.width;
			}
			this.SetScreenSize();
			this.CorrectionCameraAspect = (float)this.GameDramaViewWidth / (float)this.GameDramaViewHeight;
		}
	}

	private void Correction()
	{
		this.CameraCorrection();
		this.SetCameraViewPort(this.GameDramaCamera);
		this.SetCameraViewPort(base.gameObject.GetComponent(typeof(Camera)) as Camera);
	}

	private void SetCameraViewPort(Camera camera)
	{
		if (camera != null)
		{
			camera.aspect = this.CorrectionCameraAspect;
			camera.rect = new Rect(this.CorrectionCameraRectX, this.CorrectionCameraRectY, this.CorrectionCameraRectWidth, this.CorrectionCameraRectHeight);
		}
	}

	public void Recevory()
	{
		Camera camera = base.gameObject.GetComponent(typeof(Camera)) as Camera;
		if (camera != null)
		{
			camera.rect = new Rect(this.UICameraRectX, this.UICameraRectY, this.UICameraRectWidth, this.UICameraRectHeight);
			camera.aspect = this.UICameraAspect;
			camera.ResetAspect();
		}
		base.InitCameraPosition();
		base.gameObject.AddComponent<GUICamera>();
	}
}
