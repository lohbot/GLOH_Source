using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class MiniDramaCameraController : MonoBehaviour
{
	private GameCameraInfo _BackupCamera = new GameCameraInfo();

	private maxCamera _WorldCamera;

	private GameObject _PreAudioListener;

	private Hashtable MoveHash = new Hashtable();

	private Hashtable RotateHash = new Hashtable();

	private float _PreVal;

	public bool IsCameraAction
	{
		get
		{
			iTween component = base.gameObject.GetComponent<iTween>();
			return component != null;
		}
	}

	private void Awake()
	{
		this._WorldCamera = Camera.main.gameObject.GetComponent<maxCamera>();
		if (this._WorldCamera != null)
		{
			this._WorldCamera.enabled = false;
			this._BackupCamera.Position = this._WorldCamera.transform.position;
			this._BackupCamera.Angle = this._WorldCamera.transform.eulerAngles;
			this._BackupCamera.fieldOfView = this._WorldCamera.camera.fieldOfView;
		}
		AudioListener currentAudioListener = TsAudioManager.Instance.CurrentAudioListener;
		if (currentAudioListener.transform.parent.gameObject != base.gameObject)
		{
			this._PreAudioListener = currentAudioListener.transform.parent.gameObject;
			TsAudioListenerSwitcher tsAudioListenerSwitcher = new TsAudioListenerSwitcher(base.gameObject);
			tsAudioListenerSwitcher.Switch();
		}
	}

	private void Start()
	{
		if (this._WorldCamera == null)
		{
			this.Awake();
		}
	}

	private void Update()
	{
		NrTSingleton<NkCharManager>.Instance.SyncBillboardRotate();
	}

	public void Cut(Action Func)
	{
		base.StartCoroutine(this.Recervy(Func));
	}

	[DebuggerHidden]
	private IEnumerator Recervy(Action Func)
	{
		MiniDramaCameraController.<Recervy>c__Iterator2 <Recervy>c__Iterator = new MiniDramaCameraController.<Recervy>c__Iterator2();
		<Recervy>c__Iterator.Func = Func;
		<Recervy>c__Iterator.<$>Func = Func;
		<Recervy>c__Iterator.<>f__this = this;
		return <Recervy>c__Iterator;
	}

	public void CutEnd()
	{
		if (this._WorldCamera != null)
		{
			this._WorldCamera.transform.position = this._BackupCamera.Position;
			this._WorldCamera.transform.eulerAngles = this._BackupCamera.Angle;
			this._WorldCamera.camera.fieldOfView = this._BackupCamera.fieldOfView;
		}
	}

	private void OnDestroy()
	{
		if (this._WorldCamera != null)
		{
			this._WorldCamera.enabled = true;
		}
	}

	public void Move(Vector3 Position, float PositionMoveTime, Vector3 Angle, float AngleMoveTime, float FieldOfView, float FOVMoveTime, float ActionTime, iTween.EaseType EaseType)
	{
		if (Position != Vector3.zero)
		{
			float num = (PositionMoveTime < 0f) ? ActionTime : PositionMoveTime;
			this.MoveHash.Clear();
			this.MoveHash.Add("position", Position);
			this.MoveHash.Add("time", num);
			this.MoveHash.Add("easetype", EaseType);
			iTween.MoveTo(base.gameObject, this.MoveHash);
		}
		if (Angle != Vector3.zero)
		{
			float num2 = (AngleMoveTime < 0f) ? ActionTime : AngleMoveTime;
			this.RotateHash.Clear();
			this.RotateHash.Add("rotation", Angle);
			this.RotateHash.Add("time", num2);
			this.RotateHash.Add("easetype", EaseType);
			iTween.RotateTo(base.gameObject, this.RotateHash);
		}
		if (FieldOfView >= 0f)
		{
			float actionTime = (FOVMoveTime < 0f) ? ActionTime : FOVMoveTime;
			this.FOVTo(FieldOfView, actionTime);
		}
	}

	public void Destory()
	{
		UnityEngine.Object.Destroy(this);
	}

	public void Quake(float QuakeScaleX, float QuakeScaleY, float ActionTime)
	{
		Hashtable args = iTween.Hash(new object[]
		{
			"x",
			QuakeScaleX,
			"y",
			QuakeScaleY,
			"time",
			ActionTime
		});
		iTween.ShakePosition(base.gameObject, args);
	}

	public void DOF(bool Use)
	{
	}

	public void Fade(float Red, float Green, float Blue, float FadeInTime, float DurationTime, float FadeOutTime)
	{
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture2D.filterMode = FilterMode.Bilinear;
		texture2D.wrapMode = TextureWrapMode.Repeat;
		Color[] pixels = texture2D.GetPixels();
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i].a = 1f;
			pixels[i].r = Red;
			pixels[i].g = Green;
			pixels[i].b = Blue;
		}
		texture2D.SetPixels(pixels);
		texture2D.Apply();
	}

	public void FadeOut(float Red, float Green, float Blue, float ActionTime)
	{
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture2D.filterMode = FilterMode.Bilinear;
		texture2D.wrapMode = TextureWrapMode.Repeat;
		Color[] pixels = texture2D.GetPixels();
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i].a = 1f;
			pixels[i].r = Red;
			pixels[i].g = Green;
			pixels[i].b = Blue;
		}
		texture2D.SetPixels(pixels);
		texture2D.Apply();
	}

	public void FadeIn(float Red, float Green, float Blue, float ActionTime)
	{
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture2D.filterMode = FilterMode.Bilinear;
		texture2D.wrapMode = TextureWrapMode.Repeat;
		Color[] pixels = texture2D.GetPixels();
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i].a = 1f;
			pixels[i].r = Red;
			pixels[i].g = Green;
			pixels[i].b = Blue;
		}
		texture2D.SetPixels(pixels);
		texture2D.Apply();
	}

	public void FOVTo(float FieldOfView, float ActionTime)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", base.camera.fieldOfView);
		hashtable.Add("to", FieldOfView);
		hashtable.Add("time", ActionTime);
		hashtable.Add("onupdate", "FieldOfViewUpdate");
		iTween.ValueTo(base.gameObject, hashtable);
	}

	public void FieldOfViewUpdate(float val)
	{
		base.camera.fieldOfView = val;
	}

	public void Panning(float Angle, float ActionTime)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", 0);
		hashtable.Add("to", Angle);
		hashtable.Add("time", ActionTime);
		hashtable.Add("onupdate", "PanningUpdate");
		iTween.ValueTo(base.gameObject, hashtable);
		this._PreVal = 0f;
	}

	public void PanningUpdate(float val)
	{
		this._PreVal = val - this._PreVal;
		base.transform.Rotate(Vector3.up, this._PreVal, Space.World);
		this._PreVal = val;
	}

	public void Position(Vector3 Position)
	{
		base.gameObject.transform.localPosition = Position;
	}

	public void Angle(Vector3 Angle)
	{
		base.gameObject.transform.localEulerAngles = Angle;
	}

	public void FieldOfView(float FOV)
	{
		this.FieldOfViewUpdate(FOV);
	}
}
