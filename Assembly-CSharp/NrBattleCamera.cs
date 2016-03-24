using System;
using UnityEngine;

public class NrBattleCamera
{
	private GameObject m_TargetGo;

	private GameObject m_goTargetAni;

	private float currentDistance;

	private float desiredDistance;

	private float zoomDampening = 20f;

	private bool m_bCalcheight = true;

	private bool m_bScrollView;

	private bool m_bSetCameraTrigger;

	private float m_fActionTime;

	private float m_fDurningTime;

	private float m_fTriggerStartTime;

	private Vector3 m_veTriggerStartPos = Vector3.zero;

	private Vector3 m_veTriggerEndPos = Vector3.zero;

	private float m_fMapXSize;

	private float m_fMapZSize;

	private float m_fMoveLimitX = 30f;

	private float m_fMoveLimitZ = 30f;

	public float m_fScrollViewSensitive = 0.019f;

	private Material m_SkyBoxMaterial;

	public int m_nCurrentCameraMode = 1;

	public static NkBattleCameraBackupData m_BackupCameraData = new NkBattleCameraBackupData();

	public GameObject GetTarget()
	{
		return this.m_TargetGo;
	}

	public void SetScrollView(bool bset)
	{
		this.m_bScrollView = bset;
	}

	public void Init()
	{
		this.m_TargetGo = null;
		this.m_nCurrentCameraMode = 1;
	}

	public void CloseBattle()
	{
		this.SetLastAttackCamera(null, false);
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
		if (gameObject == null)
		{
			return;
		}
		Transform child = NkUtil.GetChild(gameObject.transform, "Main Camera");
		if (child == null)
		{
			return;
		}
		maxCamera component = child.GetComponent<maxCamera>();
		if (this.m_nCurrentCameraMode != 1)
		{
			component.SetCameraMode(1, Vector3.zero, 0f);
		}
		if (!component.enabled && NrBattleCamera.m_BackupCameraData.trParent != null)
		{
			child.parent = NrBattleCamera.m_BackupCameraData.trParent;
			component.enabled = true;
			NrBattleCamera.m_BackupCameraData.trParent = null;
		}
		UnityEngine.Object.Destroy(this.m_TargetGo);
		component.m_bBattleCamera = false;
		NrBattleCamera.m_BackupCameraData.distance = component.distance;
		NrBattleCamera.m_BackupCameraData.xDeg = component.xDeg;
		NrBattleCamera.m_BackupCameraData.yDeg = component.yDeg;
		NrBattleCamera.m_BackupCameraData.currentDistance = component.currentDistance;
		NrBattleCamera.m_BackupCameraData.desiredDistance = component.desiredDistance;
		NrBattleCamera.m_BackupCameraData.CameraLevel = component.m_nCameraLevel;
		NrBattleCamera.m_BackupCameraData.checkbackup = true;
		NrBattleCamera.m_BackupCameraData.trParent = null;
	}

	public void CameraDataRestore()
	{
		if (!NrBattleCamera.m_BackupCameraData.checkbackup)
		{
			return;
		}
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
		if (gameObject == null)
		{
			return;
		}
		Transform child = NkUtil.GetChild(gameObject.transform, "Main Camera");
		if (child == null)
		{
			return;
		}
		maxCamera component = child.GetComponent<maxCamera>();
		component.distance = NrBattleCamera.m_BackupCameraData.distance;
		component.xDeg = NrBattleCamera.m_BackupCameraData.xDeg;
		component.yDeg = NrBattleCamera.m_BackupCameraData.yDeg;
		component.currentDistance = NrBattleCamera.m_BackupCameraData.currentDistance;
		component.desiredDistance = NrBattleCamera.m_BackupCameraData.desiredDistance;
		component.m_nCameraLevel = NrBattleCamera.m_BackupCameraData.CameraLevel;
		NrBattleCamera.m_BackupCameraData.trParent = null;
		component.RestoreBattleCamera();
	}

	public void SetCameraLevel(int nLevel)
	{
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
		if (gameObject == null)
		{
			return;
		}
		Transform child = NkUtil.GetChild(gameObject.transform, "Main Camera");
		if (child == null)
		{
			return;
		}
		maxCamera component = child.GetComponent<maxCamera>();
		component.m_nCameraLevel = nLevel;
		component.RestoreBattleCamera();
	}

	public bool SetBattleCamera(ref NrBattleMap battleMap)
	{
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
		if (gameObject == null)
		{
			return false;
		}
		Transform child = NkUtil.GetChild(gameObject.transform, "Main Camera");
		if (child == null)
		{
			return false;
		}
		Transform child2 = NkUtil.GetChild(gameObject.transform, "battle_terrain");
		if (child2 == null)
		{
			return false;
		}
		Transform child3 = NkUtil.GetChild(gameObject.transform, "@battlemap");
		if (child3 != null)
		{
			Transform child4 = NkUtil.GetChild(child3, "normal1");
			if (child4 != null)
			{
				child4.gameObject.SetActive(true);
			}
		}
		if (Battle.BATTLE.CameraTarget != null)
		{
			this.m_TargetGo = (UnityEngine.Object.Instantiate(Battle.BATTLE.CameraTarget) as GameObject);
			this.m_TargetGo.SetActive(true);
		}
		if (this.m_TargetGo != null)
		{
			this.m_TargetGo.name = "BattleCameraTarget";
		}
		else
		{
			this.m_TargetGo = GameObject.Find("BattleCameraTarget");
			if (this.m_TargetGo == null)
			{
				this.m_TargetGo = new GameObject("BattleCameraTarget");
			}
		}
		Vector3 vector = child.position;
		vector.y = battleMap.GetBattleMapHeight(vector);
		if (vector.y == 0f)
		{
			vector = battleMap.GetBattleMapCenterPos();
		}
		this.m_TargetGo.transform.position = vector;
		maxCamera component = child.GetComponent<maxCamera>();
		Transform child5 = NkUtil.GetChild(this.m_TargetGo.transform, "CameraTarget");
		if (child5 != null)
		{
			this.m_goTargetAni = child5.gameObject;
		}
		component.m_bBattleCamera = true;
		if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			component.m_nCameraLevel = 3;
		}
		else if (Battle.BabelPartyCount > 1)
		{
			component.m_nCameraLevel = 1;
		}
		else
		{
			component.m_nCameraLevel = 3;
		}
		if (this.m_goTargetAni != null)
		{
			component.target = this.m_goTargetAni.transform;
		}
		else
		{
			component.target = this.m_TargetGo.transform;
		}
		component.Init();
		this.CameraAniStop();
		GameObject gameObject2 = GameObject.Find("bp1");
		GameObject gameObject3 = GameObject.Find("bp2");
		if (gameObject2 != null && gameObject3 != null)
		{
			this.m_fMoveLimitX = gameObject2.transform.position.x;
			this.m_fMoveLimitZ = gameObject2.transform.position.z;
			this.m_fMapXSize = gameObject3.transform.position.x;
			this.m_fMapZSize = gameObject3.transform.position.z;
		}
		else
		{
			this.m_fMapXSize = (float)battleMap.GetSizeX() - this.m_fMoveLimitX;
			this.m_fMapZSize = (float)battleMap.GetSizeY() - this.m_fMoveLimitZ;
		}
		this.currentDistance = this.m_TargetGo.transform.position.y;
		this.desiredDistance = this.m_TargetGo.transform.position.y;
		if (Application.isEditor)
		{
			Transform child6 = NkUtil.GetChild(gameObject.transform, "CAMERATARGETCUBE");
			GameObject gameObject4 = null;
			if (child6 != null)
			{
				gameObject4 = child6.gameObject;
			}
			if (gameObject4 == null)
			{
				gameObject4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
				Collider component2 = gameObject4.GetComponent<Collider>();
				UnityEngine.Object.Destroy(component2);
				gameObject4.transform.parent = this.m_TargetGo.transform;
				gameObject4.transform.localPosition = Vector3.zero;
				gameObject4.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				gameObject4.name = "CAMERATARGETCUBE";
			}
			MeshRenderer component3 = gameObject4.GetComponent<MeshRenderer>();
			if (component3 != null)
			{
				component3.material = new Material(component3.sharedMaterial)
				{
					color = new Color(1f, 0f, 0f)
				};
			}
		}
		TsSceneSwitcher.Instance.Collect(TsSceneSwitcher.ESceneType.BattleScene, this.m_TargetGo);
		child.camera.useOcclusionCulling = false;
		Camera component4 = child.GetComponent<Camera>();
		if (component4 != null && component4.renderingPath != RenderingPath.Forward)
		{
			component4.renderingPath = RenderingPath.Forward;
		}
		return true;
	}

	public void CameraUpdate(ref NrBattleMap BattleMap)
	{
		if (this.m_TargetGo == null)
		{
			return;
		}
		NrTSingleton<NkBattleCharManager>.Instance.SyncBillboardRotate();
		if (this.m_bSetCameraTrigger)
		{
			if (Time.time - this.m_fTriggerStartTime < this.m_fActionTime)
			{
				Vector3 pos = Vector3.zero;
				pos = Vector3.Lerp(this.m_veTriggerStartPos, this.m_veTriggerEndPos, (Time.time - this.m_fTriggerStartTime) / this.m_fActionTime);
				if (this.m_bCalcheight)
				{
					this.currentDistance = this.m_TargetGo.transform.position.y;
					this.desiredDistance = BattleMap.GetBattleMapHeight(pos);
					pos.y = Mathf.Lerp(this.currentDistance, this.desiredDistance, Time.deltaTime * 4f);
				}
				this.SetcameraPos(pos);
			}
			else if (Time.time - this.m_fTriggerStartTime > this.m_fDurningTime)
			{
				this.m_bSetCameraTrigger = false;
				this.SetcameraPos(this.m_veTriggerStartPos);
			}
			return;
		}
		bool flag = false;
		if (NkInputManager.GetKey(KeyCode.UpArrow) || NkInputManager.GetKey(KeyCode.W))
		{
			flag = true;
		}
		if (NkInputManager.GetKey(KeyCode.DownArrow) || NkInputManager.GetKey(KeyCode.S))
		{
			flag = true;
		}
		if (NkInputManager.GetKey(KeyCode.LeftArrow) || NkInputManager.GetKey(KeyCode.A))
		{
			flag = true;
		}
		if (NkInputManager.GetKey(KeyCode.RightArrow) || NkInputManager.GetKey(KeyCode.D))
		{
			flag = true;
		}
		if (NkInputManager.GetKeyUp(KeyCode.RightBracket))
		{
			this.m_fScrollViewSensitive += 0.001f;
			if (this.m_fScrollViewSensitive < 0f)
			{
				this.m_fScrollViewSensitive = 0.01f;
			}
			Debug.Log("Set ScrollView Sensitive " + this.m_fScrollViewSensitive);
		}
		if (NkInputManager.GetKeyUp(KeyCode.LeftBracket))
		{
			this.m_fScrollViewSensitive -= 0.001f;
			if (this.m_fScrollViewSensitive > 0.07f)
			{
				this.m_fScrollViewSensitive = 0.07f;
			}
			Debug.Log("Set ScrollView Sensitive " + this.m_fScrollViewSensitive);
		}
		if (!flag && !this.m_bCalcheight)
		{
			return;
		}
		if (this.m_bScrollView)
		{
			return;
		}
		if (Camera.main == null)
		{
			return;
		}
		Transform transform = Camera.main.transform;
		Vector3 a = transform.TransformDirection(Vector3.forward);
		a.y = 0f;
		a = a.normalized;
		Vector3 normalized = new Vector3(a.z, 0f, -a.x);
		normalized = normalized.normalized;
		float d = 0f;
		float d2 = 0f;
		if (flag)
		{
			d = NkInputManager.GetAxisRaw("Vertical");
			d2 = NkInputManager.GetAxisRaw("Horizontal");
		}
		Vector3 vector = d2 * normalized + d * a;
		Vector3 vector2 = Vector3.zero;
		if (vector != Vector3.zero)
		{
			float num = 0.3f;
			vector2 = Vector3.RotateTowards(vector2, vector, num * 0.0174532924f * Time.deltaTime, 1f).normalized;
		}
		if (vector2 == Vector3.zero && !this.m_bCalcheight)
		{
			return;
		}
		Vector3 vector3 = Vector3.zero;
		vector3 = this.m_TargetGo.transform.position + vector2 * Time.deltaTime * this.zoomDampening;
		if (this.m_TargetGo.transform.position.x < this.m_fMoveLimitX && vector2.x != 0f)
		{
			vector3.x = this.m_fMoveLimitX;
		}
		if (this.m_TargetGo.transform.position.x > this.m_fMapXSize && vector2.x != 0f)
		{
			vector3.x = this.m_fMapXSize;
		}
		if (this.m_TargetGo.transform.position.z < this.m_fMoveLimitZ && vector2.z != 0f)
		{
			vector3.z = this.m_fMoveLimitZ;
		}
		if (this.m_TargetGo.transform.position.z > this.m_fMapZSize && vector2.z != 0f)
		{
			vector3.z = this.m_fMapZSize;
		}
		if (this.m_bCalcheight)
		{
			this.currentDistance = this.m_TargetGo.transform.position.y;
			this.desiredDistance = BattleMap.GetBattleMapHeight(vector3);
			vector3.y = Mathf.Lerp(this.currentDistance, this.desiredDistance, Time.deltaTime * 4f);
		}
		this.m_TargetGo.transform.position = vector3;
	}

	public void SetcameraPos(Vector3 pos)
	{
		if (pos == Vector3.zero)
		{
			Debug.Log("Set CAmeraPos Vector3.Zero");
			return;
		}
		this.m_TargetGo.transform.position = pos;
	}

	public void SetTriggerAction(Vector3 pos, float fActionTime, float fDurningTime)
	{
		this.m_bSetCameraTrigger = true;
		this.m_fTriggerStartTime = Time.time;
		this.m_fActionTime = fActionTime;
		this.m_fDurningTime = fDurningTime;
		this.m_veTriggerStartPos = this.m_TargetGo.transform.position;
		this.m_veTriggerEndPos = pos;
	}

	public void SetSlowMotionCamera(NkBattleChar pkTarget, bool bSet)
	{
		if (bSet)
		{
			if (NrBattleCamera.m_BackupCameraData.checkbackup && NrBattleCamera.m_BackupCameraData.trParent != null)
			{
				return;
			}
			if (pkTarget == null)
			{
				return;
			}
			GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
			if (gameObject == null)
			{
				return;
			}
			Transform child = NkUtil.GetChild(gameObject.transform, "Main Camera");
			if (child == null)
			{
				return;
			}
			maxCamera component = child.GetComponent<maxCamera>();
			if (component == null)
			{
				return;
			}
			NrBattleCamera.m_BackupCameraData.CameraLevel = component.m_nCameraLevel;
			NrBattleCamera.m_BackupCameraData.checkbackup = true;
			component.m_nCameraLevel = 4;
			component.SetLevelValue();
			this.m_veTriggerStartPos = this.m_TargetGo.transform.position;
			this.SetcameraPos(pkTarget.GetCharPos());
		}
		else if (NrBattleCamera.m_BackupCameraData.checkbackup && NrBattleCamera.m_BackupCameraData.trParent == null)
		{
			GameObject gameObject2 = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
			if (gameObject2 == null)
			{
				return;
			}
			Transform child2 = NkUtil.GetChild(gameObject2.transform, "Main Camera");
			if (child2 == null)
			{
				return;
			}
			maxCamera component2 = child2.GetComponent<maxCamera>();
			if (component2 == null)
			{
				return;
			}
			component2.m_nCameraLevel = NrBattleCamera.m_BackupCameraData.CameraLevel;
			component2.SetLevelValue();
			NrBattleCamera.m_BackupCameraData.checkbackup = false;
			this.SetcameraPos(this.m_veTriggerStartPos);
		}
	}

	public void SetLastAttackCamera(NkBattleChar pkTarget, bool bSet)
	{
		if (bSet)
		{
			if (pkTarget == null)
			{
				return;
			}
			if (pkTarget.Get3DChar() == null)
			{
				Debug.LogError("ERROR, SetLastAttackCamera(), pkTarget.Get3DChar() is Null");
				return;
			}
			if (pkTarget.Get3DChar().GetRootGameObject() == null)
			{
				Debug.LogError("ERROR, SetLastAttackCamera(), pkTarget.Get3DChar().GetRootGameObject() is Null");
				return;
			}
			GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
			if (gameObject == null)
			{
				return;
			}
			Transform child = NkUtil.GetChild(gameObject.transform, "Main Camera");
			if (child == null)
			{
				return;
			}
			maxCamera component = child.GetComponent<maxCamera>();
			if (component == null)
			{
				return;
			}
			Transform child2 = NkUtil.GetChild(pkTarget.Get3DChar().GetRootGameObject().transform, "dmaction1");
			if (child2 != null)
			{
				Transform child3 = NkUtil.GetChild(child2, "actioncam");
				if (child3 != null)
				{
					Camera component2 = child3.GetComponent<Camera>();
					if (component2 == null)
					{
						return;
					}
					if (component2.renderingPath != RenderingPath.Forward)
					{
						component2.renderingPath = RenderingPath.Forward;
					}
					component2.backgroundColor = new Color(0f, 0f, 0f);
					if (component2 != null)
					{
						component.enabled = false;
						Camera component3 = child.GetComponent<Camera>();
						if (component3 == null)
						{
							return;
						}
						int cullingMask = component3.cullingMask;
						component3.CopyFrom(component2);
						component3.cullingMask = cullingMask;
						if (NrBattleCamera.m_BackupCameraData == null)
						{
							return;
						}
						NrBattleCamera.m_BackupCameraData.trParent = child.parent;
						child.parent = child2;
						NrBattleCamera.m_BackupCameraData.CameraLevel = component.m_nCameraLevel;
						NrBattleCamera.m_BackupCameraData.checkbackup = true;
						if (this.m_TargetGo != null)
						{
							this.m_veTriggerStartPos = this.m_TargetGo.transform.position;
						}
						this.SetcameraPos(pkTarget.GetCharPos());
						Transform child4 = NkUtil.GetChild(gameObject.transform, "@battlemap");
						if (child4 != null)
						{
							Transform child5 = NkUtil.GetChild(child4, "normal1");
							if (child5 != null)
							{
								child5.gameObject.SetActive(false);
								this.m_SkyBoxMaterial = RenderSettings.skybox;
								RenderSettings.skybox = null;
								NrTSingleton<NkBattleCharManager>.Instance.ShowHideAlly(pkTarget.Ally, pkTarget.GetBUID(), false, true);
								Battle.BATTLE.GRID_MANAGER.ShowHideGrid(false);
							}
						}
						GameObject gameObject2 = GameObject.Find("UI Camera");
						if (gameObject2 != null)
						{
							Camera componentInChildren = gameObject2.GetComponentInChildren<Camera>();
							if (componentInChildren != null)
							{
								componentInChildren.enabled = false;
							}
						}
						Battle.BATTLE.InputControlTrigger = true;
					}
				}
			}
		}
		else
		{
			if (NrBattleCamera.m_BackupCameraData == null)
			{
				return;
			}
			if (NrBattleCamera.m_BackupCameraData.checkbackup && NrBattleCamera.m_BackupCameraData.trParent != null)
			{
				GameObject gameObject3 = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
				if (gameObject3 == null)
				{
					return;
				}
				Transform child6 = NkUtil.GetChild(gameObject3.transform, "Main Camera");
				if (child6 == null)
				{
					return;
				}
				maxCamera component4 = child6.GetComponent<maxCamera>();
				if (component4 == null)
				{
					return;
				}
				if (!component4.enabled && NrBattleCamera.m_BackupCameraData.trParent != null)
				{
					child6.parent = NrBattleCamera.m_BackupCameraData.trParent;
					component4.enabled = true;
					NrBattleCamera.m_BackupCameraData.trParent = null;
				}
				component4.m_nCameraLevel = NrBattleCamera.m_BackupCameraData.CameraLevel;
				component4.SetLevelValue();
				NrBattleCamera.m_BackupCameraData.checkbackup = false;
				this.SetcameraPos(this.m_veTriggerStartPos);
				Battle.BATTLE.InputControlTrigger = false;
				Transform child7 = NkUtil.GetChild(gameObject3.transform, "@battlemap");
				if (child7 != null)
				{
					Transform child8 = NkUtil.GetChild(child7, "normal1");
					if (child8 != null)
					{
						child8.gameObject.SetActive(true);
						if (this.m_SkyBoxMaterial != null)
						{
							RenderSettings.skybox = this.m_SkyBoxMaterial;
							this.m_SkyBoxMaterial = null;
						}
						NrTSingleton<NkBattleCharManager>.Instance.ShowHideAlly(pkTarget.Ally, pkTarget.GetBUID(), true, true);
						Battle.BATTLE.GRID_MANAGER.ShowHideGrid(true);
					}
				}
				GameObject gameObject4 = GameObject.Find("UI Camera");
				if (gameObject4 != null)
				{
					Camera componentInChildren2 = gameObject4.GetComponentInChildren<Camera>();
					if (componentInChildren2 != null)
					{
						componentInChildren2.enabled = true;
					}
				}
			}
		}
	}

	public void CameraScrollMove(float x, float z)
	{
		if (Camera.main == null)
		{
			return;
		}
		Vector3 vector = new Vector3(x, 0f, z);
		Quaternion rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
		vector *= this.m_fScrollViewSensitive;
		Vector3 position = Vector3.zero;
		position = this.m_TargetGo.transform.position + rotation * vector;
		if (this.m_TargetGo.transform.position.x < this.m_fMoveLimitX && vector.x != 0f)
		{
			position.x = this.m_fMoveLimitX;
		}
		if (this.m_TargetGo.transform.position.x > this.m_fMapXSize && vector.x != 0f)
		{
			position.x = this.m_fMapXSize;
		}
		if (this.m_TargetGo.transform.position.z < this.m_fMoveLimitZ && vector.z != 0f)
		{
			position.z = this.m_fMoveLimitZ;
		}
		if (this.m_TargetGo.transform.position.z > this.m_fMapZSize && vector.z != 0f)
		{
			position.z = this.m_fMapZSize;
		}
		this.m_TargetGo.transform.position = position;
	}

	public void CameraAnimationPlay(string AniName)
	{
		if (this.m_goTargetAni == null)
		{
			return;
		}
		Animation component = this.m_goTargetAni.GetComponent<Animation>();
		if (component)
		{
			if (component.isPlaying)
			{
				component.Stop();
			}
			component.Play(AniName);
		}
	}

	public void CameraAniStop()
	{
		if (this.m_goTargetAni == null)
		{
			return;
		}
		Animation component = this.m_goTargetAni.GetComponent<Animation>();
		if (component)
		{
			component.Stop();
		}
	}

	public void SetCameraMode(int nMode, Vector3 vePos, float fAngle)
	{
		if (this.m_nCurrentCameraMode == nMode)
		{
			return;
		}
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
		if (gameObject == null)
		{
			return;
		}
		Transform child = NkUtil.GetChild(gameObject.transform, "Main Camera");
		if (child == null)
		{
			return;
		}
		maxCamera component = child.GetComponent<maxCamera>();
		if (nMode != 1)
		{
			if (!NrBattleCamera.m_BackupCameraData.checkbackup)
			{
				NrBattleCamera.m_BackupCameraData.distance = component.distance;
				NrBattleCamera.m_BackupCameraData.xDeg = component.xDeg;
				NrBattleCamera.m_BackupCameraData.yDeg = component.yDeg;
				NrBattleCamera.m_BackupCameraData.currentDistance = component.currentDistance;
				NrBattleCamera.m_BackupCameraData.desiredDistance = component.desiredDistance;
				NrBattleCamera.m_BackupCameraData.CameraLevel = component.m_nCameraLevel;
				NrBattleCamera.m_BackupCameraData.checkbackup = true;
			}
			component.SetCameraMode(nMode, vePos, fAngle);
		}
		else
		{
			component.SetCameraMode(nMode, vePos, fAngle);
			this.CameraDataRestore();
			NrBattleCamera.m_BackupCameraData.checkbackup = false;
		}
		this.m_nCurrentCameraMode = nMode;
	}
}
