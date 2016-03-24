using System;
using UnityEngine;

public class SoldierBatchCamera
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
	}

	public void CloseBattle()
	{
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene);
		if (gameObject == null)
		{
			return;
		}
		Transform child = NkUtil.GetChild(gameObject.transform, "Main Camera");
		if (child == null)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.m_TargetGo);
	}

	public bool SetPlunderCamera()
	{
		GameObject gameObject = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.SoldierBatchScene);
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
		if (this.m_TargetGo != null)
		{
			this.m_TargetGo.name = "SoldierBatchCameraTarget";
		}
		else
		{
			this.m_TargetGo = GameObject.Find("SoldierBatchCameraTarget");
			if (this.m_TargetGo == null)
			{
				this.m_TargetGo = new GameObject("SoldierBatchCameraTarget");
			}
		}
		Vector3 position = child.transform.position;
		this.m_TargetGo.transform.position = position;
		maxCamera component = child.GetComponent<maxCamera>();
		Transform child3 = NkUtil.GetChild(this.m_TargetGo.transform, "CameraTarget");
		if (child3 != null)
		{
			this.m_goTargetAni = child3.gameObject;
		}
		component.m_bBattleCamera = true;
		component.m_nCameraLevel = 3;
		if (this.m_goTargetAni != null)
		{
			component.target = this.m_goTargetAni.transform;
		}
		else
		{
			component.target = this.m_TargetGo.transform;
		}
		component.Init();
		this.currentDistance = this.m_TargetGo.transform.position.y;
		this.desiredDistance = this.m_TargetGo.transform.position.y;
		if (Application.isEditor)
		{
			Transform child4 = NkUtil.GetChild(gameObject.transform, "CAMERATARGETCUBE");
			GameObject gameObject2 = null;
			if (child4 != null)
			{
				gameObject2 = child4.gameObject;
			}
			if (gameObject2 == null)
			{
				gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
				Collider component2 = gameObject2.GetComponent<Collider>();
				UnityEngine.Object.Destroy(component2);
				gameObject2.transform.parent = this.m_TargetGo.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				gameObject2.name = "CAMERATARGETCUBE";
			}
			MeshRenderer component3 = gameObject2.GetComponent<MeshRenderer>();
			if (component3 != null)
			{
				component3.material = new Material(component3.sharedMaterial)
				{
					color = new Color(1f, 0f, 0f)
				};
			}
		}
		TsSceneSwitcher.Instance.Collect(TsSceneSwitcher.ESceneType.SoldierBatchScene, this.m_TargetGo);
		child.camera.useOcclusionCulling = false;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID)
		{
			component.SetCameraMode(6, Vector3.zero, 0f);
		}
		else
		{
			component.SetCameraMode(5, Vector3.zero, 0f);
		}
		component.CameraWork();
		return true;
	}

	public void CameraUpdate(ref NrBattleMap BattleMap)
	{
		if (this.m_TargetGo == null)
		{
			return;
		}
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
		if ((transform.position.x < this.m_fMoveLimitX || this.m_TargetGo.transform.position.x < this.m_fMoveLimitX) && vector2.x < 0f)
		{
			vector3 = this.m_TargetGo.transform.position;
		}
		if ((transform.position.x > this.m_fMapXSize || this.m_TargetGo.transform.position.x > this.m_fMapXSize) && vector2.x > 0f)
		{
			vector3 = this.m_TargetGo.transform.position;
		}
		if ((transform.position.z < this.m_fMoveLimitZ || this.m_TargetGo.transform.position.z < this.m_fMoveLimitZ) && vector2.z < 0f)
		{
			vector3 = this.m_TargetGo.transform.position;
		}
		if ((transform.position.z > this.m_fMapZSize || this.m_TargetGo.transform.position.z > this.m_fMapZSize) && vector2.z > 0f)
		{
			vector3 = this.m_TargetGo.transform.position;
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

	public void CameraScrollMove(float x, float z)
	{
		Vector3 vector = new Vector3(x, 0f, z);
		Quaternion rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
		vector *= this.m_fScrollViewSensitive;
		this.m_TargetGo.transform.position = this.m_TargetGo.transform.position + rotation * vector;
	}
}
