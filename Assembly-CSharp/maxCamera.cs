using Ndoors.Framework.Stage;
using System;
using System.Text;
using UnityEngine;
using UnityForms;

[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
public class maxCamera : MonoBehaviour
{
	public Transform target;

	public Vector3 targetOffset;

	public float distance = 5f;

	public float maxDistance = 10f;

	public float minDistance = 5f;

	public float xSpeed = 200f;

	public float ySpeed = 200f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 20f;

	public int zoomRate = 40;

	public float panSpeed = 0.3f;

	public float zoomDampening = 10f;

	public float fieldOfView;

	public float xDeg;

	public float yDeg;

	public float currentDistance;

	public float desiredDistance;

	public float fLevelHeight = 1.5f;

	public float fTargetHeight = 1.5f;

	private bool checkbackup;

	private Quaternion rotation;

	private Vector3 position;

	private bool bCanControl = true;

	public bool m_bBattleCamera;

	public bool bFollowHero;

	public float fFollowHeroDepending = 1f;

	public int m_nCameraLevel;

	public float m_fBeforeFov;

	public float m_fBeforeYDeg;

	public float m_fBeforeCurrentDistance;

	public float m_fLerpStartTime;

	public float m_fBeforeHeight;

	private bool m_bUseCameraLevel = true;

	public bool m_bToolCamera;

	private bool m_bDistanceMove;

	private bool m_DisableContorl;

	private float m_fMobileRotate = 0.37f;

	private float m_fBackupYDeg;

	public bool bCullisionCamera = true;

	private Vector3 v3CullisionCamera = default(Vector3);

	private Vector3 v3CameraDirection = default(Vector3);

	private RaycastHit rayCullisiton = default(RaycastHit);

	private bool m_bRotate;

	public bool UseCameraLevel
	{
		get
		{
			return this.m_bUseCameraLevel;
		}
		set
		{
			this.m_bUseCameraLevel = value;
			if (this.m_bUseCameraLevel)
			{
				this.SetLevelValue();
			}
		}
	}

	public float MobileRotate
	{
		get
		{
			return this.m_fMobileRotate;
		}
		set
		{
			this.m_fMobileRotate = value;
		}
	}

	public bool CAMERA_ROTAE
	{
		get
		{
			return this.m_bRotate;
		}
	}

	private void EnableControl()
	{
		if (Scene.CurScene != Scene.Type.BATTLE)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null && @char.IsReady3DModel())
			{
				GameObject rootGameObject = @char.Get3DChar().GetRootGameObject();
				if (rootGameObject != null)
				{
					this.target = rootGameObject.transform;
				}
			}
		}
		else
		{
			GameObject gameObject = GameObject.Find("BattleCameraTarget");
			if (gameObject != null)
			{
				this.target = gameObject.transform;
			}
		}
		this.m_DisableContorl = false;
		this.CameraWork();
	}

	private void DisableControl()
	{
		this.m_DisableContorl = true;
	}

	private void Start()
	{
		this.Init();
	}

	public void Init()
	{
		if (!this.target)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null && @char.IsReady3DModel())
			{
				GameObject rootGameObject = @char.Get3DChar().GetRootGameObject();
				if (rootGameObject != null)
				{
					this.target = rootGameObject.transform;
				}
			}
			if (!this.target)
			{
				return;
			}
		}
		if (this.m_DisableContorl)
		{
			return;
		}
		this.RestoreCameraInfo();
		this.distance = Vector3.Distance(base.transform.position, this.target.position);
		this.currentDistance = this.distance;
		this.desiredDistance = this.distance;
		this.xDeg = this.target.rotation.eulerAngles.y;
		this.yDeg = 35f;
		this.yDeg = maxCamera.ClampAngle(this.yDeg, this.yMinLimit, this.yMaxLimit);
		this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
		base.transform.rotation = this.rotation;
		this.BackUpCameraInfo();
		if (!this.m_bBattleCamera)
		{
			this.maxDistance = 12f;
			this.minDistance = 1.4f;
			this.xSpeed = 4f;
			this.ySpeed = 4f;
			this.yMinLimit = -70f;
			this.yMaxLimit = 80f;
			this.zoomRate = 20;
			this.panSpeed = 0.5f;
			this.zoomDampening = 6f;
			base.camera.fieldOfView = 60f;
			if (TsPlatform.IsMobile)
			{
				this.m_nCameraLevel = 2;
			}
		}
		else
		{
			this.maxDistance = 24f;
			this.minDistance = 3f;
			this.xSpeed = 4f;
			this.ySpeed = 4f;
			this.yMinLimit = 2f;
			this.yMaxLimit = 80f;
			this.zoomRate = 25;
			this.panSpeed = 0.5f;
			this.zoomDampening = 6f;
			this.currentDistance = 200f;
			this.desiredDistance = 200f;
			Camera component = base.GetComponent<Camera>();
			if (component != null)
			{
				this.fieldOfView = 35f;
				this.xDeg = 280f;
				this.yDeg = 45f;
				this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
				base.transform.rotation = this.rotation;
				this.CameraWork();
			}
		}
		if (this.m_bUseCameraLevel)
		{
			this.SetLevelValue();
		}
		AudioListener component2 = base.GetComponent<AudioListener>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
		this.StartCameraControl();
	}

	public void StartCameraControl()
	{
		this.bCanControl = true;
		if (NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
		{
			NrTSingleton<NkCharManager>.Instance.SetBillboardScale();
		}
	}

	public void StopCameraControl()
	{
		this.bCanControl = false;
	}

	public void CameraWork()
	{
		if (null == this.target)
		{
			this.Init();
			return;
		}
		CAMERASETTING_DATA cAMERASETTING_DATA = null;
		if (this.m_bUseCameraLevel)
		{
			cAMERASETTING_DATA = NrTSingleton<NkCameraSettingsManager>.Instance.GetCameraData(this.m_nCameraLevel);
		}
		float num = 0f;
		float num2 = 0f;
		if (this.GetAxisRange(ref num, ref num2))
		{
			this.xDeg += num;
			if (this.m_bUseCameraLevel && cAMERASETTING_DATA != null)
			{
				this.yDeg -= num2;
				this.yDeg = maxCamera.ClampAngle(this.yDeg, this.yMinLimit, cAMERASETTING_DATA.m_YRotate);
			}
			else
			{
				this.yDeg -= num2;
				this.yDeg = maxCamera.ClampAngle(this.yDeg, this.yMinLimit, this.yMaxLimit);
			}
			this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			base.transform.rotation = this.rotation;
		}
		else if (this.bFollowHero)
		{
			float num3 = this.target.rotation.eulerAngles.y;
			if (Mathf.Abs(this.xDeg - num3) > 180f)
			{
				num3 += 360f;
			}
			this.fFollowHeroDepending = 1.25f;
			this.fFollowHeroDepending += 1f;
			this.fFollowHeroDepending = Time.deltaTime * this.fFollowHeroDepending;
			this.xDeg = Mathf.Lerp(this.xDeg, num3, this.fFollowHeroDepending);
			this.yDeg = Mathf.Lerp(this.yDeg, 5f, this.fFollowHeroDepending);
			this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			base.transform.rotation = this.rotation;
		}
		if (this.m_bUseCameraLevel && cAMERASETTING_DATA != null)
		{
			if (this.m_bDistanceMove)
			{
				this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
				this.currentDistance = Mathf.Lerp(this.m_fBeforeCurrentDistance, this.desiredDistance, (Time.time - this.m_fLerpStartTime) / cAMERASETTING_DATA.m_LerpTime);
				if (this.desiredDistance == this.currentDistance)
				{
					if (this.m_nCameraLevel < NrTSingleton<NkCameraSettingsManager>.Instance.GetMaxLevel() - 1)
					{
						NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
						if (@char != null && @char.IsReady3DModel())
						{
							@char.SetShowHide3DModel(true, true, true);
						}
					}
					this.m_bDistanceMove = false;
				}
			}
		}
		else
		{
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (!this.m_bToolCamera && axis != 0f && NrTSingleton<FormsManager>.Instance.IsMouseOverForm())
			{
				return;
			}
			this.desiredDistance -= axis * Time.deltaTime * (float)this.zoomRate * Mathf.Abs(this.desiredDistance);
			this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
			this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance, Time.deltaTime * this.zoomDampening);
		}
		if (this.bCullisionCamera)
		{
			this.position = this.CullisionCamera();
		}
		else
		{
			this.position = this.target.position - (base.transform.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
		}
		base.transform.position = this.position;
		Camera component = base.GetComponent<Camera>();
		if (this.m_bUseCameraLevel && cAMERASETTING_DATA != null)
		{
			this.fieldOfView = Mathf.Lerp(this.m_fBeforeFov, cAMERASETTING_DATA.GetFOV(), (Time.time - this.m_fLerpStartTime) / cAMERASETTING_DATA.m_LerpTime);
		}
		component.fieldOfView = this.fieldOfView;
	}

	private void LateUpdate()
	{
		if (!this.bCanControl)
		{
			return;
		}
		if (!this.m_DisableContorl)
		{
			if (this.m_bUseCameraLevel)
			{
				if (Input.GetAxis("Mouse ScrollWheel") > 0f && !NrTSingleton<FormsManager>.Instance.IsMouseOverForm())
				{
					this.m_nCameraLevel++;
					if (this.m_nCameraLevel >= NrTSingleton<NkCameraSettingsManager>.Instance.GetMaxLevel())
					{
						this.m_nCameraLevel = NrTSingleton<NkCameraSettingsManager>.Instance.GetMaxLevel() - 1;
					}
					this.m_fBeforeFov = this.fieldOfView;
					this.m_fBeforeYDeg = this.yDeg;
					this.m_fBeforeCurrentDistance = this.currentDistance;
					this.m_fBeforeHeight = this.fTargetHeight;
					this.m_fLerpStartTime = Time.time;
					this.SetLevelValue();
				}
				else if (Input.GetAxis("Mouse ScrollWheel") < 0f && !NrTSingleton<FormsManager>.Instance.IsMouseOverForm())
				{
					this.m_nCameraLevel--;
					if (this.m_nCameraLevel < 0)
					{
						this.m_nCameraLevel = 0;
					}
					this.m_fBeforeFov = this.fieldOfView;
					this.m_fBeforeYDeg = this.yDeg;
					this.m_fBeforeCurrentDistance = this.currentDistance;
					this.m_fBeforeHeight = this.fTargetHeight;
					this.m_fLerpStartTime = Time.time;
					this.SetLevelValue();
				}
			}
			this.CameraWork();
		}
		if (NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
		{
			NrTSingleton<NkCharManager>.Instance.SyncBillboardRotate();
		}
	}

	private static float ClampAngle(float angle, float min, float max)
	{
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

	private Vector3 CullisionCamera()
	{
		this.v3CullisionCamera = this.target.position - this.rotation * Vector3.forward * this.currentDistance;
		this.v3CameraDirection = this.v3CullisionCamera - this.target.position;
		this.v3CameraDirection = this.v3CameraDirection.normalized;
		float num = this.currentDistance;
		if (this.m_bUseCameraLevel)
		{
			CAMERASETTING_DATA cameraData = NrTSingleton<NkCameraSettingsManager>.Instance.GetCameraData(this.m_nCameraLevel);
			if (cameraData != null)
			{
				this.fTargetHeight = Mathf.Lerp(this.m_fBeforeHeight, this.fLevelHeight, (Time.time - this.m_fLerpStartTime) / cameraData.m_LerpTime);
			}
		}
		this.position = this.target.position + new Vector3(0f, this.fTargetHeight, 0f);
		TsLayerMask layerMask = TsLayer.EVERYTHING - TsLayer.PC - TsLayer.PC_DECORATION - TsLayer.PC_OTHER - TsLayer.NPC - TsLayer.FADE_OBJECT;
		if (Physics.Raycast(this.position, this.v3CameraDirection, out this.rayCullisiton, num + 1f, layerMask))
		{
			float num2 = this.rayCullisiton.distance - 0.1f;
			num2 -= this.minDistance;
			num2 /= num - this.minDistance;
			this.fTargetHeight = Mathf.Lerp(2f, this.fLevelHeight, Mathf.Clamp(num2, 0f, 1f));
			this.position = this.target.position + new Vector3(0f, this.fTargetHeight, 0f);
		}
		if (Physics.Raycast(this.position, this.v3CameraDirection, out this.rayCullisiton, num + 1f, layerMask))
		{
			num = this.rayCullisiton.distance - 0.1f;
		}
		this.v3CullisionCamera = this.position - this.rotation * Vector3.forward * num;
		return this.v3CullisionCamera;
	}

	public void BackUpCameraInfo()
	{
		if (this.m_DisableContorl)
		{
			return;
		}
		if (null == Camera.main)
		{
			return;
		}
		this.position = base.transform.position;
		this.rotation = base.transform.rotation;
		this.fieldOfView = base.camera.fieldOfView;
		this.checkbackup = true;
	}

	public void RestoreCameraInfo()
	{
		if (this.m_DisableContorl)
		{
			return;
		}
		if (!this.checkbackup)
		{
			return;
		}
		base.transform.position = this.position;
		base.transform.rotation = this.rotation;
		base.camera.fieldOfView = this.fieldOfView;
		this.checkbackup = false;
		this.StartCameraControl();
		this.SetLevelValue();
	}

	public void SetFollowHero(bool bSet)
	{
		this.bFollowHero = bSet;
		if (this.bFollowHero)
		{
			this.desiredDistance = 7f;
			this.yDeg = 5f;
		}
	}

	public void RestoreBattleCamera()
	{
		if (this.m_DisableContorl)
		{
			return;
		}
		this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
		base.transform.rotation = this.rotation;
		this.CameraWork();
	}

	public void SetLevelValue()
	{
		if (this.m_DisableContorl)
		{
			return;
		}
		CAMERASETTING_DATA cameraData = NrTSingleton<NkCameraSettingsManager>.Instance.GetCameraData(this.m_nCameraLevel);
		if (this.m_bUseCameraLevel && cameraData != null)
		{
			this.yMaxLimit = cameraData.m_YRotate;
			this.yDeg = maxCamera.ClampAngle(this.yDeg, this.yMinLimit, this.yMaxLimit);
			this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			base.transform.rotation = this.rotation;
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo != null)
			{
				NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(0);
				if (soldierInfo == null)
				{
					return;
				}
				if (soldierInfo.GetSolID() <= 0L)
				{
					return;
				}
				this.fLevelHeight = cameraData.GetTribeHeight(soldierInfo.GetCharKindInfo().GetCharTribe());
				if (this.fTargetHeight == 0f)
				{
					this.fTargetHeight = this.fLevelHeight;
				}
				if (Scene.CurScene == Scene.Type.WORLD)
				{
					this.minDistance = cameraData.GetTribeZoom(soldierInfo.GetCharKindInfo().GetCharTribe());
					this.maxDistance = cameraData.GetTribeZoom(soldierInfo.GetCharKindInfo().GetCharTribe());
				}
				else
				{
					this.minDistance = cameraData.m_Zoom;
					this.maxDistance = cameraData.m_Zoom;
				}
			}
			if (Scene.CurScene == Scene.Type.WORLD && this.m_nCameraLevel >= NrTSingleton<NkCameraSettingsManager>.Instance.GetMaxLevel() - 1)
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null && @char.IsReady3DModel())
				{
					@char.SetShowHide3DModel(false, false, false);
				}
			}
			this.m_bDistanceMove = true;
		}
	}

	public void CameraSettingGui()
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		stringBuilder.AppendFormat("X Rotate {0}  ", this.xDeg);
		stringBuilder.AppendFormat("Y Rotate {0}\r\n", this.yDeg);
		stringBuilder.AppendFormat("Y Rotate Min : {0} Max {1}\r\n", this.yMinLimit, this.yMaxLimit);
		stringBuilder.AppendFormat("Zoom {0}  ", this.currentDistance);
		stringBuilder.AppendFormat("Zoom Min : {0} Max {1}\r\n", this.minDistance, this.maxDistance);
		stringBuilder.AppendFormat("LEVEL : {0}", this.m_nCameraLevel);
		GUILayout.BeginArea(new Rect(0f, 0f, 290f, 180f));
		GUI.Box(new Rect(0f, 0f, 290f, 80f), stringBuilder.ToString());
		bool flag = GUI.Toggle(new Rect(0f, 80f, 290f, 20f), this.m_bUseCameraLevel, "USE LEVEL CAMERA");
		if (this.m_bUseCameraLevel != flag)
		{
			if (flag)
			{
				this.SetLevelValue();
			}
			this.m_bUseCameraLevel = flag;
		}
		if (GUI.Button(new Rect(0f, 100f, 100f, 20f), "<<<"))
		{
			this.yMinLimit -= 1f;
			if (this.yMinLimit <= 0f)
			{
				this.yMinLimit = 0f;
			}
		}
		GUI.Label(new Rect(105f, 100f, 80f, 20f), "Y RotateMin");
		if (GUI.Button(new Rect(190f, 100f, 100f, 20f), ">>>"))
		{
			this.yMinLimit += 1f;
			if (this.yMinLimit >= this.yMaxLimit)
			{
				this.yMinLimit = this.yMaxLimit;
			}
		}
		if (GUI.Button(new Rect(0f, 120f, 100f, 20f), "<<<"))
		{
			this.yMaxLimit -= 1f;
			if (this.yMaxLimit <= this.yMinLimit)
			{
				this.yMaxLimit = this.yMinLimit;
			}
		}
		GUI.Label(new Rect(105f, 120f, 80f, 20f), "Y RotateMax");
		if (GUI.Button(new Rect(190f, 120f, 100f, 20f), ">>>"))
		{
			this.yMaxLimit += 1f;
			if (this.yMaxLimit >= 365f)
			{
				this.yMinLimit = 365f;
			}
		}
		if (GUI.Button(new Rect(0f, 140f, 100f, 20f), "<<<"))
		{
			this.minDistance -= 0.1f;
			if (this.minDistance <= 0f)
			{
				this.minDistance = 0f;
			}
		}
		GUI.Label(new Rect(105f, 140f, 80f, 20f), "Zoom Min");
		if (GUI.Button(new Rect(190f, 140f, 100f, 20f), ">>>"))
		{
			this.minDistance += 0.1f;
			if (this.minDistance >= this.maxDistance)
			{
				this.minDistance = this.maxDistance;
			}
		}
		if (GUI.Button(new Rect(0f, 160f, 100f, 20f), "<<<"))
		{
			this.maxDistance -= 0.1f;
			if (this.maxDistance <= this.minDistance)
			{
				this.maxDistance = this.minDistance;
			}
		}
		GUI.Label(new Rect(105f, 160f, 80f, 20f), "Zoom Max");
		if (GUI.Button(new Rect(190f, 160f, 100f, 20f), ">>>"))
		{
			this.maxDistance += 0.1f;
		}
		GUILayout.EndArea();
	}

	public void SetCameraMode(int nMode, Vector3 vePos, float fAngle)
	{
		if (nMode == 1)
		{
			if (this.m_fBackupYDeg == 0f)
			{
				return;
			}
			this.yMinLimit = 2f;
			this.yMaxLimit = 80f;
			if (this.m_fBackupYDeg != 0f)
			{
				this.yDeg = this.m_fBackupYDeg;
				this.m_fBackupYDeg = 0f;
			}
			this.UseCameraLevel = true;
			this.bCullisionCamera = true;
			this.EnableControl();
			this.SetLevelValue();
		}
		else if (nMode == 2)
		{
			this.UseCameraLevel = false;
			this.m_fBackupYDeg = this.yDeg;
			this.yDeg = 40f;
			this.yMinLimit = 40f;
			this.yMaxLimit = 40f;
			this.xDeg = 150f;
			this.maxDistance = 16f;
			this.minDistance = 16f;
			this.currentDistance = 16f;
			this.desiredDistance = 16f;
			this.bCullisionCamera = false;
			this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			base.transform.rotation = this.rotation;
			this.CameraWork();
		}
		else if (nMode == 3)
		{
			this.UseCameraLevel = false;
			this.m_fBackupYDeg = this.yDeg;
			this.yDeg = 0f;
			this.yMinLimit = 0f;
			this.yMaxLimit = 0f;
			this.xDeg = fAngle;
			this.maxDistance = 16f;
			this.minDistance = 16f;
			this.currentDistance = 16f;
			this.desiredDistance = 16f;
			this.bCullisionCamera = false;
			this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			base.transform.rotation = this.rotation;
			this.CameraWork();
			this.DisableControl();
			this.position = vePos - (base.transform.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
			base.transform.position = this.position;
		}
		else if (nMode == 4)
		{
			this.UseCameraLevel = false;
			this.m_fBackupYDeg = this.yDeg;
			this.yDeg = 40f;
			this.yMinLimit = 40f;
			this.yMaxLimit = 40f;
			this.xDeg = fAngle;
			this.maxDistance = 16f;
			this.minDistance = 16f;
			this.currentDistance = 16f;
			this.desiredDistance = 16f;
			this.bCullisionCamera = false;
			this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			base.transform.rotation = this.rotation;
			this.CameraWork();
			this.DisableControl();
			this.position = vePos - (base.transform.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
			base.transform.position = this.position;
		}
		else if (nMode == 5)
		{
			this.UseCameraLevel = false;
			this.yDeg = 60f;
			this.yMinLimit = 60f;
			this.yMaxLimit = 60f;
			this.xDeg = 270f;
			this.maxDistance = 18f;
			this.minDistance = 18f;
			this.currentDistance = 18f;
			this.desiredDistance = 18f;
			this.bCullisionCamera = false;
			this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			base.transform.rotation = this.rotation;
			this.CameraWork();
		}
		else if (nMode == 6)
		{
			this.UseCameraLevel = false;
			this.yDeg = 60f;
			this.yMinLimit = 60f;
			this.yMaxLimit = 60f;
			this.xDeg = 270f;
			this.maxDistance = 25f;
			this.minDistance = 25f;
			this.currentDistance = 25f;
			this.desiredDistance = 25f;
			this.bCullisionCamera = false;
			this.rotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			base.transform.rotation = this.rotation;
			this.CameraWork();
		}
	}

	public bool GetAxisRange(ref float fXrange, ref float fYRange)
	{
		if (TsPlatform.IsMobile)
		{
			if (TsPlatform.IsEditor)
			{
				if (NkInputManager.GetMouseButton(1))
				{
					fXrange = Input.GetAxis("Mouse X");
					fYRange = Input.GetAxis("Mouse Y");
					fXrange *= this.xSpeed;
					fYRange *= this.ySpeed;
				}
				return true;
			}
			bool flag = false;
			if (Scene.CurScene == Scene.Type.WORLD)
			{
				flag = true;
			}
			if (flag && NkInputManager.IsJoystick())
			{
				int touchCount = Input.touchCount;
				if (touchCount == 0)
				{
					return false;
				}
				if (touchCount != 2)
				{
					return false;
				}
				Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
				Vector2 deltaPosition2 = Input.GetTouch(1).deltaPosition;
				Vector2 vector = Vector2.zero;
				this.m_bRotate = false;
				if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Stationary)
				{
					vector = deltaPosition;
					this.m_bRotate = true;
				}
				else if (Input.GetTouch(1).phase == TouchPhase.Moved && Input.GetTouch(0).phase == TouchPhase.Stationary)
				{
					vector = deltaPosition2;
					this.m_bRotate = true;
				}
				else if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
				{
					vector = deltaPosition2;
					this.m_bRotate = true;
				}
				if (this.m_bRotate)
				{
					fXrange = vector.x / (float)Screen.width * 1280f * this.xSpeed * this.m_fMobileRotate;
					fYRange = vector.y / (float)Screen.height * 720f * this.ySpeed * this.m_fMobileRotate;
				}
			}
			else if (flag)
			{
				if (NrTSingleton<FormsManager>.Instance.IsMouseOverForm())
				{
					return false;
				}
				int touchCount2 = Input.touchCount;
				if (touchCount2 != 1)
				{
					return false;
				}
				Vector2 deltaPosition3 = Input.GetTouch(0).deltaPosition;
				Vector2 vector2 = Vector2.zero;
				this.m_bRotate = false;
				if (Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					vector2 = deltaPosition3;
					this.m_bRotate = true;
				}
				if (this.m_bRotate)
				{
					fXrange = vector2.x / (float)Screen.width * 1280f * this.xSpeed * this.m_fMobileRotate;
					fYRange = vector2.y / (float)Screen.height * 720f * this.ySpeed * this.m_fMobileRotate;
				}
			}
			else
			{
				int touchCount3 = Input.touchCount;
				if (touchCount3 == 0)
				{
					return false;
				}
				if (touchCount3 != 2)
				{
					return false;
				}
				Vector2 deltaPosition4 = Input.GetTouch(0).deltaPosition;
				Vector2 deltaPosition5 = Input.GetTouch(1).deltaPosition;
				Vector2 vector3 = Vector2.zero;
				this.m_bRotate = false;
				if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Stationary)
				{
					vector3 = deltaPosition4;
					this.m_bRotate = true;
				}
				else if (Input.GetTouch(1).phase == TouchPhase.Moved && Input.GetTouch(0).phase == TouchPhase.Stationary)
				{
					vector3 = deltaPosition5;
					this.m_bRotate = true;
				}
				else if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
				{
					vector3 = deltaPosition5;
					this.m_bRotate = true;
				}
				if (this.m_bRotate)
				{
					fXrange = vector3.x / (float)Screen.width * 1280f * this.xSpeed * this.m_fMobileRotate;
					fYRange = vector3.y / (float)Screen.height * 720f * this.ySpeed * this.m_fMobileRotate;
				}
			}
			if (Scene.CurScene == Scene.Type.BATTLE && Battle.BATTLE != null)
			{
				Battle.BATTLE.SetCameraRotate(this.m_bRotate);
			}
			return true;
		}
		else
		{
			if (NkInputManager.GetMouseButton(1))
			{
				fXrange = Input.GetAxis("Mouse X");
				fYRange = Input.GetAxis("Mouse Y");
				fXrange *= this.xSpeed;
				fYRange *= this.ySpeed;
				return true;
			}
			return false;
		}
	}
}
