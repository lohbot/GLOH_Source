using GAME;
using GameMessage;
using Ndoors.Framework.Stage;
using System;
using System.Runtime.CompilerServices;
using Ts;
using TsBundle;
using UnityEngine;

public abstract class Nr3DCharBase
{
	public delegate void func3DModelCreated(GameObject pkCharRoot);

	protected int m_siID = -1;

	protected GameObject m_kRootGameObj;

	protected GameObject m_kBaseObject;

	protected GameObject m_kBoneRootObject;

	protected GameObject m_kFaceObject;

	protected GameObject m_kWeapon1Object;

	protected GameObject m_kWeapon2Object;

	protected GameObject m_kWeaponTrailObject;

	protected CharacterController m_kCharacterCtrl;

	protected bool m_bSlope;

	protected float m_fAngleX;

	protected Nr3DCharItemAssetBundle[] m_kItemAssetBundle;

	protected Transform m_pkDummyCenter;

	protected Transform m_pkDummyWeapon1;

	protected Transform m_pkDummyWeapon2;

	protected Transform m_pkDummyBackWeapon1;

	protected Transform m_pkDummyBackWeapon2;

	protected Transform m_pkDummyShot;

	protected Transform m_pkPickingCollider;

	protected float m_fCharSpeed = 10f;

	protected float m_fGravity = 19.8f;

	protected bool m_bOnGround;

	protected bool m_bIsMoveToTarget;

	protected float m_fDistanceFromTarget;

	protected Vector3 m_vTargetPos = Vector3.zero;

	protected bool m_bCreated;

	protected bool m_b3DCharLoadFailed;

	protected bool m_MoveNoLookAt;

	public NrDebugLoger debugLog;

	protected NrCharBase m_pkChar;

	protected NkBattleChar m_pkBattleChar;

	protected string m_szCharCode = string.Empty;

	protected string m_szModelPath = string.Empty;

	protected string m_szCurrentAniType;

	protected bool m_bAnimationPlaying;

	protected POS3D m_PosLookAt;

	protected bool m_bRequestSit;

	protected bool m_bRequestLookAt;

	protected bool m_bIsFading;

	protected bool m_bIsFadeInState = true;

	private Color m_tempMainColor = CHAR_SHADER_COLOR.RIM_MAIN_NORMAL;

	private Color m_tempDefaultMainColor = CHAR_SHADER_COLOR.RIM_MAIN_NORMAL;

	private Color m_tempMaxMainColor = CHAR_SHADER_COLOR.RIM_MAIN_FOCUS;

	protected bool m_bStartSkipFrame;

	protected int m_n3DCharSkipFrame;

	protected float m_f3DCharDeltaTime;

	private bool m_bMiniDramaChar;

	protected Action<float> m_actionSetSpeed = delegate(float T)
	{
	};

	protected event Nr3DCharBase.func3DModelCreated m_evt3DModelCreated
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.m_evt3DModelCreated = (Nr3DCharBase.func3DModelCreated)Delegate.Combine(this.m_evt3DModelCreated, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.m_evt3DModelCreated = (Nr3DCharBase.func3DModelCreated)Delegate.Remove(this.m_evt3DModelCreated, value);
		}
	}

	protected event Nr3DCharBase.func3DModelCreated m_evt3DModelPartItemChanged
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.m_evt3DModelPartItemChanged = (Nr3DCharBase.func3DModelCreated)Delegate.Combine(this.m_evt3DModelPartItemChanged, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.m_evt3DModelPartItemChanged = (Nr3DCharBase.func3DModelCreated)Delegate.Remove(this.m_evt3DModelPartItemChanged, value);
		}
	}

	public bool bStartDownloadBundle
	{
		get;
		protected set;
	}

	protected int downloadCounter
	{
		get;
		set;
	}

	public virtual bool removeScript
	{
		set
		{
		}
	}

	public Nr3DCharBase()
	{
		this.m_kRootGameObj = new GameObject("no name");
		this.debugLog = new NrDebugLoger();
		this.bStartDownloadBundle = false;
		this.downloadCounter = 0;
		this.m_bCreated = false;
		this.m_b3DCharLoadFailed = false;
		this.m_bMiniDramaChar = false;
		this.m_kItemAssetBundle = new Nr3DCharItemAssetBundle[8];
	}

	public void SetMiniDramaChar()
	{
		this.m_bMiniDramaChar = true;
	}

	public bool GetMiniDramaChar()
	{
		return this.m_bMiniDramaChar;
	}

	public void Set3DCharFrameInfo(NrCharDefine.CharUpdateStep updatestep, int skipfram)
	{
		switch (updatestep)
		{
		case NrCharDefine.CharUpdateStep.CHARUPDATESTEP_NONE:
		case NrCharDefine.CharUpdateStep.CHARUPDATESTEP_NEAR:
		case NrCharDefine.CharUpdateStep.CHARUPDATESTEP_AROUND:
			if (this.m_pkPickingCollider != null && !this.m_pkPickingCollider.gameObject.activeInHierarchy)
			{
				this.m_pkPickingCollider.gameObject.SetActive(true);
			}
			break;
		case NrCharDefine.CharUpdateStep.CHARUPDATESTEP_FAR:
		case NrCharDefine.CharUpdateStep.CHARUPDATESTEP_VERYFAR:
			if (this.m_pkPickingCollider != null && this.m_pkPickingCollider.gameObject.activeInHierarchy)
			{
				this.m_pkPickingCollider.gameObject.SetActive(false);
			}
			break;
		}
		this.m_n3DCharSkipFrame = skipfram;
		this.m_f3DCharDeltaTime = 0f;
	}

	public void SetStartSkipFrame(bool bStart)
	{
		this.m_bStartSkipFrame = bStart;
	}

	public void SetID(int id)
	{
		this.m_siID = id;
		if (id == 1 && this.m_kRootGameObj != null)
		{
			this.m_kRootGameObj.tag = "Player";
		}
		this.m_bStartSkipFrame = false;
		this.m_n3DCharSkipFrame = 0;
		this.m_f3DCharDeltaTime = 0f;
	}

	public int GetID()
	{
		return this.m_siID;
	}

	public void SetSlope(bool bFlag)
	{
		this.m_bSlope = bFlag;
	}

	public void SetDownloadCount(int count)
	{
		this.downloadCounter += count;
		if (this.downloadCounter < 0)
		{
			this.downloadCounter = 0;
		}
	}

	public bool IsCreated()
	{
		return this.m_bCreated;
	}

	public void Set3DCharLoadFailed(bool bFailed)
	{
		this.m_b3DCharLoadFailed = bFailed;
	}

	public bool Is3DCharLoadFailed()
	{
		return this.m_b3DCharLoadFailed;
	}

	public bool Is3DCharActive()
	{
		return !(this.m_kBaseObject == null) && this.m_kBaseObject.activeInHierarchy;
	}

	public bool IsGround()
	{
		return !(null == this.m_kCharacterCtrl) && this.m_kCharacterCtrl.isGrounded;
	}

	public bool IsMoveToTarget()
	{
		return this.m_bIsMoveToTarget;
	}

	public void SetParentChar(NrCharBase pkChar)
	{
		this.m_pkChar = pkChar;
		this.SetCharModelInfo();
	}

	public void SetParentChar(NkBattleChar pkChar)
	{
		this.m_pkBattleChar = pkChar;
		this.SetCharModelInfo();
	}

	public bool IsBattleChar()
	{
		return this.m_pkBattleChar != null;
	}

	private void SetCharModelInfo()
	{
		NrCharKindInfo parentCharKindInfo = this.GetParentCharKindInfo();
		if (parentCharKindInfo == null)
		{
			this.m_szCharCode = "William";
			this.m_szModelPath = "NPC/William";
		}
		else
		{
			this.m_szCharCode = parentCharKindInfo.GetCode();
			this.m_szModelPath = parentCharKindInfo.GetBundlePath();
			string costumeBundlePath = this.GetCostumeBundlePath();
			if (!string.IsNullOrEmpty(costumeBundlePath))
			{
				this.m_szModelPath = costumeBundlePath;
			}
		}
		this.m_szModelPath = this.m_szModelPath.ToLower();
	}

	public NrCharBase GetParentChar()
	{
		return this.m_pkChar;
	}

	public NkBattleChar GetParentBattleChar()
	{
		return this.m_pkBattleChar;
	}

	public string GetCharCode()
	{
		return this.m_szCharCode;
	}

	protected void SetParentCharAnimation(eCharAnimationType anitype)
	{
		if (this.m_pkChar != null)
		{
			this.m_pkChar.SetAnimation(anitype);
		}
		else if (this.m_pkBattleChar != null)
		{
			this.m_pkBattleChar.SetAnimation(anitype);
		}
	}

	protected void SetParentCharAnimation(eCharAnimationType anitype, bool bForceAction, bool bBlend)
	{
		if (this.m_pkChar != null)
		{
			this.m_pkChar.SetAnimation(anitype, bForceAction, bBlend);
		}
		else if (this.m_pkBattleChar != null)
		{
			this.m_pkBattleChar.SetAnimation(anitype, bForceAction, bBlend);
		}
	}

	public NrCharKindInfo GetParentCharKindInfo()
	{
		if (this.m_pkChar != null)
		{
			return this.m_pkChar.GetCharKindInfo();
		}
		if (this.m_pkBattleChar != null)
		{
			return this.m_pkBattleChar.GetCharKindInfo();
		}
		return null;
	}

	public NkSoldierInfo GetParentCharSoldierInfo()
	{
		if (this.m_pkChar != null)
		{
			return this.m_pkChar.GetPersonInfo().GetSoldierInfo(0);
		}
		if (this.m_pkBattleChar != null)
		{
			return this.m_pkBattleChar.GetPersonInfo().GetSoldierInfo(0);
		}
		return null;
	}

	public NkSoldierInfo GetParentFaceSoldierInfo()
	{
		if (this.m_pkChar != null && this.m_pkChar.GetFaceSolID() != 0L)
		{
			return this.m_pkChar.GetPersonInfo().GetSoldierInfoBySolID((long)((int)this.m_pkChar.GetFaceSolID()));
		}
		if (this.m_pkBattleChar != null)
		{
			this.m_pkBattleChar.GetPersonInfo().GetSoldierInfoBySolID(0L);
		}
		return null;
	}

	public NrCharAnimation GetParentCharAnimation()
	{
		if (this.m_pkChar != null)
		{
			return this.m_pkChar.GetCharAnimation();
		}
		if (this.m_pkBattleChar != null)
		{
			return this.m_pkBattleChar.GetCharAnimation();
		}
		return null;
	}

	public eCharKindType GetParentCharType()
	{
		if (this.m_pkChar != null)
		{
			return this.m_pkChar.GetCharKindType();
		}
		if (this.m_pkBattleChar != null)
		{
			return this.m_pkBattleChar.GetCharKindType();
		}
		return eCharKindType.CKT_NPC;
	}

	public NkCharPartControl GetParentCharPartControl()
	{
		if (this.m_pkChar != null)
		{
			NrCharUser nrCharUser = this.m_pkChar as NrCharUser;
			if (nrCharUser != null)
			{
				return nrCharUser.GetPartControl();
			}
		}
		else if (this.m_pkBattleChar != null)
		{
			return this.m_pkBattleChar.GetPartControl();
		}
		return null;
	}

	public string GetBattleCharCostumeBundleName()
	{
		if (this.m_pkBattleChar == null || this.m_pkBattleChar.GetSoldierInfo() == null)
		{
			Debug.LogError("ERROR, Nr3DCharBase.cs, GetBattleCharCostumeUnique(), m_pkBattleChar = null or m_pkBattleChar.GetSoldierInfo() = null");
			return string.Empty;
		}
		int num = (int)this.m_pkBattleChar.GetSoldierInfo().GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		if (num <= 0)
		{
			return string.Empty;
		}
		CharCostumeInfo_Data costumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeData(num);
		if (costumeData == null)
		{
			return string.Empty;
		}
		if (costumeData.IsNormalCostume())
		{
			return string.Empty;
		}
		return costumeData.m_BundlePath.Replace("Soldier/", string.Empty);
	}

	public GameObject GetRootGameObject()
	{
		return this.m_kRootGameObj;
	}

	public void SetRootGameObject(GameObject goSet)
	{
		this.m_kRootGameObj = goSet;
		if (this.GetID() == 1)
		{
			this.m_kRootGameObj.tag = "Player";
		}
	}

	public GameObject GetBaseObject()
	{
		return this.m_kBaseObject;
	}

	public GameObject GetBoneRootObject()
	{
		return this.m_kBoneRootObject;
	}

	public GameObject GetFaceObject()
	{
		return this.m_kFaceObject;
	}

	public GameObject GetWeapon1Object()
	{
		return this.m_kWeapon1Object;
	}

	public GameObject GetWeapon2Object()
	{
		return this.m_kWeapon2Object;
	}

	public CharacterController GetCharController()
	{
		return this.m_kCharacterCtrl;
	}

	public float GetDiffCharScale()
	{
		if (this.m_kCharacterCtrl == null)
		{
			return 1f;
		}
		return 1f;
	}

	public Transform GetCenterDummy()
	{
		return this.m_pkDummyCenter;
	}

	public Transform GetShotDummy()
	{
		return this.m_pkDummyShot;
	}

	public Transform GetEffectPos(eEFFECT_POS ePos)
	{
		Transform transform = null;
		switch (ePos)
		{
		case eEFFECT_POS.CENTER:
			transform = this.GetEffectTarget();
			break;
		case eEFFECT_POS.BOTTOM:
			if (this.GetBaseObject() != null)
			{
				transform = this.GetBaseObject().transform;
			}
			break;
		case eEFFECT_POS.NAME:
			if (this.GetBaseObject() != null)
			{
				transform = NkUtil.GetChild(this.GetBaseObject().transform, "dmname");
			}
			break;
		case eEFFECT_POS.BONE:
			if (this.GetBoneRootObject() == null)
			{
				TsLog.LogWarning("GetEffectPos GetBoneRootObject() == null  ObjectName = {0}", new object[]
				{
					this.GetName()
				});
			}
			else
			{
				transform = this.GetBoneRootObject().transform;
			}
			break;
		case eEFFECT_POS.CENTERDM:
			transform = this.GetEffectTarget();
			break;
		default:
			transform = this.GetEffectTarget();
			break;
		}
		if (null == transform)
		{
			transform = this.GetEffectTarget();
		}
		return transform;
	}

	public Transform GetEffectTarget()
	{
		GameObject rootGameObject = this.GetRootGameObject();
		if (rootGameObject == null)
		{
			return null;
		}
		Transform transform = this.GetCenterDummy();
		if (transform == null)
		{
			GameObject boneRootObject = this.GetBoneRootObject();
			if (boneRootObject == null)
			{
				transform = rootGameObject.transform;
			}
			else
			{
				transform = boneRootObject.transform;
			}
		}
		return transform;
	}

	public void SetLayer(int si32Layer)
	{
		this._SetLayerRecursively(si32Layer, this.m_kRootGameObj.transform);
	}

	private void _SetLayerRecursively(int si32Layer, Transform t)
	{
		t.gameObject.layer = si32Layer;
		for (int i = 0; i < t.childCount; i++)
		{
			this._SetLayerRecursively(si32Layer, t.GetChild(i));
		}
	}

	public void SetLayer(int si32Layer, string tagName)
	{
		this._SetLayerRecursively(si32Layer, this.m_kRootGameObj.transform);
	}

	protected void CalcBounds()
	{
		this.OnCalcBounds(this.m_kRootGameObj.transform);
	}

	protected void OnCalcBounds(Transform t)
	{
	}

	public void SetName(string strName)
	{
		if (strName.Length > 0)
		{
			this.m_kRootGameObj.name = strName;
		}
	}

	public string GetName()
	{
		if (this.m_kRootGameObj == null)
		{
			return "NULL";
		}
		return this.m_kRootGameObj.name;
	}

	public abstract string MakeAnimationKey(eCharAnimationType anitype);

	public abstract AnimationClip SetAnimation(eCharAnimationType targetani, float fBlendTime, bool bForceLoop);

	public abstract void SetFacialAnimation(string anikey, WrapMode wrapmode);

	public bool IsHaveAnimation(eCharAnimationType anitype)
	{
		GameObject baseObject = this.GetBaseObject();
		if (baseObject == null)
		{
			return false;
		}
		if (baseObject.animation == null)
		{
			return false;
		}
		string name = this.MakeAnimationKey(anitype);
		return baseObject.animation.GetClip(name) != null;
	}

	protected AnimationClip GetSafeAnimationClip(eCharAnimationType targetani)
	{
		NrCharAnimation parentCharAnimation = this.GetParentCharAnimation();
		if (parentCharAnimation == null)
		{
			return null;
		}
		eCharAnimationType safeAniType = parentCharAnimation.GetSafeAniType(targetani);
		if (!this.IsHaveAnimation(safeAniType))
		{
			safeAniType = parentCharAnimation.GetSafeAniType(safeAniType);
			if (!this.IsHaveAnimation(safeAniType))
			{
				return null;
			}
		}
		string name = this.MakeAnimationKey(safeAniType);
		AnimationClip clip = this.m_kBaseObject.animation.GetClip(name);
		this.SetCurrentAniType(safeAniType.ToString());
		return clip;
	}

	public float GetBlendTime(string SourceAni, string TargetAni)
	{
		return NrTSingleton<Nr3DCharSystem>.Instance.GetBlend(this.GetCharCode(), SourceAni, TargetAni);
	}

	public virtual void SetShowHide(bool bShow)
	{
		if (null == this.m_kRootGameObj)
		{
			return;
		}
		NkUtil.SetShowHideRenderer(this.m_kRootGameObj, bShow, true);
		if (bShow)
		{
			this.FadeOutIllumination();
		}
	}

	public virtual void SetShowHide(bool bShow, bool bParticleSystem)
	{
		if (null == this.m_kRootGameObj)
		{
			return;
		}
		NkUtil.SetShowHideRenderer(this.m_kRootGameObj, bShow, true, true);
		if (bShow)
		{
			this.FadeOutIllumination();
		}
	}

	public virtual void Reset()
	{
		this.bStartDownloadBundle = true;
	}

	public void SetEvent3DModelCreated(Nr3DCharBase.func3DModelCreated kFunc)
	{
		this.m_evt3DModelCreated = kFunc;
	}

	public void OnEvent3DModelCreated()
	{
		if (this.m_evt3DModelCreated != null)
		{
			this.m_evt3DModelCreated(this.GetRootGameObject());
		}
		if (this.m_pkChar != null)
		{
			this.CalcBounds();
			this.m_pkChar.Loaded3DChar();
		}
		else if (this.m_pkBattleChar != null)
		{
			this.CalcBounds();
			this.m_pkBattleChar.Loaded3DChar();
		}
	}

	public void SetEvent3DModelPartItemChanged(Nr3DCharBase.func3DModelCreated kFunc)
	{
		this.m_evt3DModelPartItemChanged = kFunc;
	}

	public void OnEvent3DModelPartItemChanged(GameObject pkPartItemRoot)
	{
		if (this.m_evt3DModelPartItemChanged != null)
		{
			this.m_evt3DModelPartItemChanged(pkPartItemRoot);
		}
	}

	public bool PostUpdate()
	{
		this.m_f3DCharDeltaTime = 0f;
		return true;
	}

	public virtual bool Update()
	{
		if (this.m_kRootGameObj == null)
		{
			return false;
		}
		if (this.m_kBaseObject == null)
		{
			return false;
		}
		if (!this.Is3DCharActive())
		{
			return false;
		}
		this.m_f3DCharDeltaTime += Time.deltaTime;
		if (this.m_bStartSkipFrame && this.m_n3DCharSkipFrame > 0)
		{
			this.m_n3DCharSkipFrame = Math.Max(0, this.m_n3DCharSkipFrame - 1);
			if (this.m_n3DCharSkipFrame > 0)
			{
				return false;
			}
		}
		if (!this.IsBattleChar() && !NkClientLogic.bWorldCharUpdate)
		{
			return false;
		}
		using (new ScopeProfile("GravityProcess"))
		{
			if (this.m_bOnGround)
			{
				this.GravityProcess();
			}
		}
		using (new ScopeProfile("MoveProcess"))
		{
			if (this.m_bIsMoveToTarget)
			{
				this.MoveProcess();
			}
		}
		using (new ScopeProfile("Fade"))
		{
			if (this.m_bIsFading)
			{
				if (this.m_bIsFadeInState)
				{
					this.OnFadeInIllumination();
				}
				else
				{
					this.OnFadeOutIllumination();
				}
			}
		}
		return true;
	}

	public virtual void FinishDownloadBase(ref IDownloadedItem wItem)
	{
		Transform child = NkUtil.GetChild(this.GetRootGameObject().transform, "actioncam");
		if (null != child)
		{
			child.gameObject.SetActive(false);
		}
	}

	protected void OnFinishLoadProcess()
	{
		NrTSingleton<Nr3DCharSystem>.Instance.SetBlend(this.GetCharCode(), this.m_kBaseObject);
		this.SearchCharDummy();
		this.SetAdjustPickingCollider();
		this.m_bCreated = true;
		this.debugLog.Log("3DModel is created : " + this.m_kBaseObject.transform.name);
		this.m_szCurrentAniType = "None";
		this.m_bAnimationPlaying = false;
		if (this.m_pkChar != null)
		{
			TsSceneSwitcher.Instance.Collect(TsSceneSwitcher.ESceneType.WorldScene, this.GetRootGameObject());
		}
		if (this.m_pkBattleChar != null)
		{
			TsSceneSwitcher.Instance.Collect(TsSceneSwitcher.ESceneType.BattleScene, this.GetRootGameObject());
		}
		this.OnEvent3DModelCreated();
	}

	public void SearchWeaponDummy()
	{
		NkCharPartControl parentCharPartControl = this.GetParentCharPartControl();
		if (parentCharPartControl != null && this.m_kBaseObject != null)
		{
			string weaponTargetName = parentCharPartControl.GetWeaponTargetName(NrCharDefine.eAT2ItemAssetBundle.weapon1, true);
			this.m_pkDummyWeapon1 = NkUtil.GetChild(this.m_kBaseObject.transform, weaponTargetName);
			string weaponTargetName2 = parentCharPartControl.GetWeaponTargetName(NrCharDefine.eAT2ItemAssetBundle.weapon2, true);
			this.m_pkDummyWeapon2 = NkUtil.GetChild(this.m_kBaseObject.transform, weaponTargetName2);
			string weaponTargetName3 = parentCharPartControl.GetWeaponTargetName(NrCharDefine.eAT2ItemAssetBundle.weapon1, false);
			this.m_pkDummyBackWeapon1 = NkUtil.GetChild(this.m_kBaseObject.transform, weaponTargetName3);
			string weaponTargetName4 = parentCharPartControl.GetWeaponTargetName(NrCharDefine.eAT2ItemAssetBundle.weapon2, false);
			this.m_pkDummyBackWeapon2 = NkUtil.GetChild(this.m_kBaseObject.transform, weaponTargetName4);
		}
	}

	protected void SearchCharDummy()
	{
		Transform child = NkUtil.GetChild(this.m_kBaseObject.transform, "Bone01");
		if (child != null)
		{
			this.m_kBoneRootObject = child.gameObject;
		}
		else
		{
			this.m_kBoneRootObject = this.m_kBaseObject;
		}
		this.m_pkDummyCenter = NkUtil.GetChild(this.m_kBaseObject.transform, "dmcenter");
		if (this.m_pkDummyCenter == null && child != null)
		{
			this.m_pkDummyCenter = child;
		}
		NkCharPartControl parentCharPartControl = this.GetParentCharPartControl();
		if (parentCharPartControl != null)
		{
			string weaponTargetName = parentCharPartControl.GetWeaponTargetName(NrCharDefine.eAT2ItemAssetBundle.weapon1, true);
			this.m_pkDummyWeapon1 = NkUtil.GetChild(this.m_kBaseObject.transform, weaponTargetName);
			string weaponTargetName2 = parentCharPartControl.GetWeaponTargetName(NrCharDefine.eAT2ItemAssetBundle.weapon2, true);
			this.m_pkDummyWeapon2 = NkUtil.GetChild(this.m_kBaseObject.transform, weaponTargetName2);
			string weaponTargetName3 = parentCharPartControl.GetWeaponTargetName(NrCharDefine.eAT2ItemAssetBundle.weapon1, false);
			this.m_pkDummyBackWeapon1 = NkUtil.GetChild(this.m_kBaseObject.transform, weaponTargetName3);
			string weaponTargetName4 = parentCharPartControl.GetWeaponTargetName(NrCharDefine.eAT2ItemAssetBundle.weapon2, false);
			this.m_pkDummyBackWeapon2 = NkUtil.GetChild(this.m_kBaseObject.transform, weaponTargetName4);
		}
		this.SearchShotDummy(this.m_kBaseObject.transform);
	}

	private void SearchShotDummy(Transform pkBaseObject)
	{
		this.m_pkDummyShot = NkUtil.GetChild(pkBaseObject, "dmshot");
	}

	private void SetAdjustPickingCollider()
	{
		string strName = "PickingCollider";
		this.m_pkPickingCollider = NkUtil.GetChild(this.GetRootGameObject().transform, strName);
		if (this.m_pkPickingCollider != null)
		{
			bool flag = null == this.GetParentBattleChar();
			if (flag)
			{
				NmClickEvent nmClickEvent = NkUtil.GuarranteeComponent<NmClickEvent>(this.m_pkPickingCollider.gameObject);
				nmClickEvent.CharInput = this.GetParentChar();
			}
			else
			{
				this.m_pkPickingCollider.gameObject.SetActive(false);
			}
			BoxCollider component = this.m_pkPickingCollider.GetComponent<BoxCollider>();
			if (component != null)
			{
				Vector3 localScale = new Vector3(1f, 1f, 1f);
				GameObject boneRootObject = this.GetBoneRootObject();
				if (boneRootObject != null)
				{
					localScale = boneRootObject.transform.localScale;
				}
				Vector3 zero = Vector3.zero;
				zero.x = component.size.x * localScale.x;
				zero.y = component.size.y * localScale.y;
				zero.z = component.size.z * localScale.z;
				if (zero.x < 0.1f)
				{
					zero.x = 0.3f;
				}
				else if (zero.x < 0.3f)
				{
					zero.x = 0.5f;
				}
				else if (zero.x < 0.5f)
				{
					zero.x = 0.7f;
				}
				else
				{
					zero.x = Mathf.Max(0.7f, zero.x);
				}
				if (zero.y < 0.1f)
				{
					zero.y = 0.3f;
				}
				else if (zero.y < 0.3f)
				{
					zero.y = 0.5f;
				}
				else if (zero.y < 0.5f)
				{
					zero.y = 0.7f;
				}
				else
				{
					zero.y = Mathf.Max(0.7f, zero.y);
				}
				if (zero.z < 0.1f)
				{
					zero.z = 0.3f;
				}
				else if (zero.z < 0.3f)
				{
					zero.z = 0.5f;
				}
				else if (zero.z < 0.5f)
				{
					zero.z = 0.7f;
				}
				else
				{
					zero.z = Mathf.Max(0.7f, zero.z);
				}
				Vector3 size = zero;
				size.x = zero.x / localScale.x;
				size.y = zero.y / localScale.y;
				size.z = zero.z / localScale.z;
				component.size = size;
			}
		}
	}

	public Vector3 GetPickingSize()
	{
		if (this.m_pkPickingCollider != null)
		{
			BoxCollider component = this.m_pkPickingCollider.GetComponent<BoxCollider>();
			if (component != null)
			{
				return component.size;
			}
		}
		return Vector3.zero;
	}

	public void SetAttachItem(NrCharDefine.eAT2ItemAssetBundle eItemIndex, string targetname, string filename)
	{
		if (string.IsNullOrEmpty(targetname) || string.IsNullOrEmpty(filename))
		{
			Debug.LogError("AttachItem parameter is invalid! Target=" + targetname + " File=" + filename);
			return;
		}
		if (this.m_kItemAssetBundle[(int)eItemIndex] == null)
		{
			this.m_kItemAssetBundle[(int)eItemIndex] = new Nr3DCharItemAssetBundle();
		}
		if (this.m_kItemAssetBundle[(int)eItemIndex].IsSameLoadItemName(filename))
		{
			return;
		}
		this.m_kItemAssetBundle[(int)eItemIndex].InitBundleInfo();
		string path = string.Empty;
		if (eItemIndex == NrCharDefine.eAT2ItemAssetBundle.hair || eItemIndex == NrCharDefine.eAT2ItemAssetBundle.face || eItemIndex == NrCharDefine.eAT2ItemAssetBundle.helmet)
		{
			path = string.Format("Char/Player/{0}/{1}", this.m_szModelPath, filename);
		}
		else
		{
			path = string.Format("Char/Item/{0}", filename);
		}
		this.downloadCounter++;
		NrTSingleton<NkBundleCallBack>.Instance.RequestBundleRuntime(path, NkBundleCallBack.NPCBundleStackName, ItemType.CHAR_ITEM, (int)eItemIndex, targetname, NkBundleParam.eBundleType.BUNDLE_CHAR_ATTACHITEM, this.GetID().ToString());
		this.m_kItemAssetBundle[(int)eItemIndex].SetLoadItemName(filename);
	}

	public void FinishDownloadItem(ref IDownloadedItem wItem)
	{
		this.downloadCounter--;
		Vector3 scale = new Vector3(1f, 1f, 1f);
		this.m_kItemAssetBundle[wItem.indexParam].SetAssetBundle(wItem.GetSafeBundle());
		this.m_kItemAssetBundle[wItem.indexParam].SetTargetName(wItem.strParam);
		this.m_kItemAssetBundle[wItem.indexParam].SetScale(scale);
		if (this.m_bCreated)
		{
			AssetBundle assetBundle = this.m_kItemAssetBundle[wItem.indexParam].GetAssetBundle();
			if (assetBundle != null)
			{
				string targetName = this.m_kItemAssetBundle[wItem.indexParam].GetTargetName();
				this.AttachItem(wItem.indexParam, (GameObject)assetBundle.mainAsset, targetName, true, this.m_kItemAssetBundle[wItem.indexParam].GetScale());
			}
		}
	}

	public virtual void InitBundleLoadFailed(bool itembundle, int paramindex)
	{
		this.SetDownloadCount(-1);
		if (itembundle)
		{
			this.InitLoadItem(paramindex);
		}
	}

	public void InitLoadItem(int itemindex)
	{
		this.m_kItemAssetBundle[itemindex].SetLoadItemName(string.Empty);
	}

	protected void AttachItemProcess()
	{
		for (int i = 0; i < 8; i++)
		{
			if (this.m_kItemAssetBundle[i] != null)
			{
				if (this.m_kItemAssetBundle[i].IsValid())
				{
					AssetBundle assetBundle = this.m_kItemAssetBundle[i].GetAssetBundle();
					if (!(assetBundle == null))
					{
						if (assetBundle.mainAsset is GameObject)
						{
							string targetName = this.m_kItemAssetBundle[i].GetTargetName();
							this.AttachItem(i, (GameObject)assetBundle.mainAsset, targetName, false, this.m_kItemAssetBundle[i].GetScale());
						}
						else
						{
							Debug.LogError("Not game object asset.");
							Debug.Break();
						}
					}
				}
			}
		}
	}

	protected void AttachItem(int itemindex, GameObject kGameObj, string targetname, bool bShow, Vector3 kScale)
	{
		Transform child = NkUtil.GetChild(this.m_kBaseObject.transform, targetname);
		if (child == null)
		{
			Debug.LogWarning("==============> AttachItemProcess not found targetname = " + targetname);
			return;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(kGameObj);
		if (gameObject == null)
		{
			Debug.LogError("not found object name.");
			Debug.Break();
		}
		this.RemoveItem(itemindex, false);
		gameObject.transform.name = ((NrCharDefine.eAT2ItemAssetBundle)itemindex).ToString();
		this.AttachItemToParent(gameObject.transform, child, kScale, true);
		if (!this.m_bCreated)
		{
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Renderer renderer = componentsInChildren[i];
				renderer.renderer.enabled = false;
			}
		}
		if (itemindex == 1)
		{
			this.m_kFaceObject = gameObject;
			if (this.m_pkChar != null)
			{
				NrCharUser nrCharUser = this.m_pkChar as NrCharUser;
				nrCharUser.SetFacialAnimation();
			}
		}
		else if (itemindex == 3)
		{
			this.m_kWeapon1Object = gameObject;
			this.SearchShotDummy(this.m_kWeapon1Object.transform);
		}
		else if (itemindex == 4)
		{
			this.m_kWeapon2Object = gameObject;
			this.SearchShotDummy(this.m_kWeapon2Object.transform);
		}
		if (this.m_bCreated)
		{
			this.OnEvent3DModelPartItemChanged(gameObject);
		}
		if (Scene.IsCurScene(Scene.Type.SELECTCHAR))
		{
			MsgHandler.Handle("SetCreateCharPartInfo", new object[]
			{
				false,
				true
			});
		}
		NkCharPartControl parentCharPartControl = this.GetParentCharPartControl();
		if (parentCharPartControl != null)
		{
			NkSoldierInfo parentCharSoldierInfo = this.GetParentCharSoldierInfo();
			parentCharPartControl.OnItemChange(parentCharSoldierInfo, itemindex, gameObject);
		}
		if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
		{
			Renderer[] componentsInChildren2 = this.m_kBaseObject.GetComponentsInChildren<Renderer>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				Renderer renderer2 = componentsInChildren2[j];
				renderer2.renderer.enabled = false;
			}
		}
	}

	public void OnRecoveryEnchantWeapon()
	{
		NkCharPartControl parentCharPartControl = this.GetParentCharPartControl();
		if (parentCharPartControl != null)
		{
			NkSoldierInfo parentCharSoldierInfo = this.GetParentCharSoldierInfo();
			parentCharPartControl.OnItemChange(parentCharSoldierInfo, 3, this.GetWeapon1Object());
			parentCharPartControl.OnItemChange(parentCharSoldierInfo, 4, this.GetWeapon2Object());
		}
		if (!this.IsBattleChar())
		{
			NkUtil.PlayAnimationInChildren(this.m_kWeapon1Object, eCharAnimationType.Stay1.ToString());
			NkUtil.PlayAnimationInChildren(this.m_kWeapon2Object, eCharAnimationType.Stay1.ToString());
		}
		else
		{
			NkUtil.PlayAnimationInChildren(this.m_kWeapon1Object, eCharAnimationType.BStay1.ToString());
			NkUtil.PlayAnimationInChildren(this.m_kWeapon2Object, eCharAnimationType.BStay1.ToString());
		}
	}

	protected void AttachItemToParent(Transform from, Transform to, Vector3 scale, bool changescale)
	{
		Vector3 localPosition = from.localPosition;
		Quaternion localRotation = from.localRotation;
		from.parent = to;
		from.localPosition = localPosition;
		from.localRotation = localRotation;
		if (to != null && to.name.Contains("axe"))
		{
			changescale = true;
			if (to.name.Contains("back"))
			{
				scale = new Vector3(0.8f, 0.8f, 0.8f);
			}
			else
			{
				scale = new Vector3(1f, 1f, 1f);
			}
		}
		if (to != null && to.name.Contains("shield"))
		{
			changescale = true;
			if (to.name.Contains("back"))
			{
				scale = new Vector3(0.9f, 0.9f, 0.9f);
			}
			else
			{
				scale = new Vector3(1f, 1f, 1f);
			}
		}
		if (changescale)
		{
			from.localScale = scale;
		}
	}

	public void ChangeWeaponTarget(string targetname1, string targetname2)
	{
		if (this.m_kWeapon1Object != null && targetname1.Length > 0 && !this.m_kWeapon1Object.transform.parent.name.Equals(targetname1))
		{
			if (this.m_pkDummyWeapon1.name == targetname1)
			{
				this.AttachItemToParent(this.m_kWeapon1Object.transform, this.m_pkDummyWeapon1, Vector3.zero, false);
			}
			else
			{
				this.AttachItemToParent(this.m_kWeapon1Object.transform, this.m_pkDummyBackWeapon1, Vector3.zero, false);
			}
		}
		if (this.m_kWeapon2Object != null && targetname2.Length > 0 && !this.m_kWeapon2Object.transform.parent.name.Equals(targetname2))
		{
			if (this.m_pkDummyWeapon2.name == targetname2)
			{
				this.AttachItemToParent(this.m_kWeapon2Object.transform, this.m_pkDummyWeapon2, Vector3.zero, false);
			}
			else
			{
				this.AttachItemToParent(this.m_kWeapon2Object.transform, this.m_pkDummyBackWeapon2, Vector3.zero, false);
			}
		}
	}

	public void RemoveItem(int itemindex, bool bInitInfo)
	{
		if (this.m_kBaseObject == null)
		{
			return;
		}
		if (this.m_kItemAssetBundle[itemindex] == null || !this.m_kItemAssetBundle[itemindex].IsLoadedBundle())
		{
			return;
		}
		Transform transform = null;
		if (itemindex == 1)
		{
			if (this.m_kFaceObject != null)
			{
				transform = this.m_kFaceObject.transform;
			}
			this.m_kFaceObject = null;
		}
		else if (itemindex == 3)
		{
			if (this.m_kWeapon1Object != null)
			{
				transform = this.m_kWeapon1Object.transform;
			}
			this.m_kWeapon1Object = null;
		}
		else if (itemindex == 4)
		{
			if (this.m_kWeapon2Object != null)
			{
				transform = this.m_kWeapon2Object.transform;
			}
			this.m_kWeapon2Object = null;
		}
		if (transform == null)
		{
			string strName = ((NrCharDefine.eAT2ItemAssetBundle)itemindex).ToString();
			transform = NkUtil.GetChild(this.m_kBaseObject.transform, strName);
		}
		if (transform != null)
		{
			transform.parent = null;
			UnityEngine.Object.DestroyImmediate(transform.gameObject);
			if (bInitInfo)
			{
				this.m_kItemAssetBundle[itemindex].Init();
			}
		}
		if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
		{
			Renderer[] componentsInChildren = this.m_kBaseObject.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Renderer renderer = componentsInChildren[i];
				renderer.renderer.enabled = false;
			}
		}
	}

	public void RemoveItemAll()
	{
		int num = 8;
		for (int i = 0; i < num; i++)
		{
			this.RemoveItem(i, true);
		}
	}

	public virtual eCharAnimationType GetIdleAnimation()
	{
		if (this.IsBattleChar())
		{
			return this.m_pkBattleChar.GetStayAni();
		}
		return eCharAnimationType.Stay1;
	}

	public void StartIdleAnimation(bool bBlend)
	{
		eCharAnimationType targetani = this.GetIdleAnimation();
		if (this.IsBattleChar())
		{
			targetani = this.m_pkBattleChar.GetStayAni();
		}
		this.m_bAnimationPlaying = false;
		this.SetAnimation(targetani, 0.3f, true);
	}

	public void SetLookAt(Vector3 v3Pos)
	{
		this.SetLookAt(v3Pos.x, v3Pos.y, v3Pos.z, true);
	}

	public void SetLookAT(Vector2 v2Pos)
	{
		this.SetLookAt(v2Pos.x, 0f, v2Pos.y, true);
	}

	public void SetLookAt(float x, float y, float z, bool bInterpolation)
	{
		Vector3 a = new Vector3(x, this.m_kRootGameObj.transform.position.y, z);
		Vector3 vector = a - this.m_kRootGameObj.transform.position;
		if (vector == Vector3.zero)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				Debug.LogWarning("forward can't become zero.");
			}
			return;
		}
		Quaternion to = Quaternion.LookRotation(vector);
		Vector3 eulerAngles = to.eulerAngles;
		eulerAngles.x = 0f;
		eulerAngles.z = 0f;
		to.eulerAngles = eulerAngles;
		float t = 1f;
		if (bInterpolation)
		{
			t = 10f * this.m_f3DCharDeltaTime;
		}
		Quaternion rotation = this.m_kRootGameObj.transform.rotation;
		Quaternion localRotation = Quaternion.Slerp(rotation, to, t);
		this.m_kRootGameObj.transform.localRotation = localRotation;
	}

	public virtual eCharAnimationType GetMoveAnimation()
	{
		return eCharAnimationType.Walk1;
	}

	public void MoveTo(Vector2 v2Pos)
	{
		Vector3 hitPointFromVector = NkUtil.GetHitPointFromVector2(v2Pos);
		this.MoveTo(hitPointFromVector);
	}

	public virtual void MoveTo(Vector3 v3Pos)
	{
		this.MoveTo(v3Pos.x, v3Pos.y, v3Pos.z);
	}

	public virtual void MoveTo(float x, float y, float z)
	{
		this.MoveTo(x, y, z, this.GetMoveAnimation(), false);
	}

	public virtual void MoveTo(float x, float y, float z, eCharAnimationType anitype, bool MoveNoLookAt)
	{
		this.SetParentCharAnimation(anitype);
		this.m_vTargetPos.x = x;
		this.m_vTargetPos.y = y;
		this.m_vTargetPos.z = z;
		if (!this.m_bIsMoveToTarget)
		{
			this.m_bIsMoveToTarget = true;
		}
		this.m_MoveNoLookAt = MoveNoLookAt;
		if (x == 0f && y == 0f && z == 0f)
		{
			Debug.LogWarning(" !!! Warnning !!! Char move target position is zero. ");
		}
	}

	public void MoveStop(bool bSetAni)
	{
		this.ReachedDestProcess(bSetAni);
	}

	public void MovePos(Vector3 topos)
	{
		this.m_kRootGameObj.transform.position = topos;
	}

	private void MoveProcess()
	{
		if (this.m_kCharacterCtrl == null)
		{
			Debug.LogWarning("not found character controller.");
			return;
		}
		Vector2 vector = new Vector2(this.m_kCharacterCtrl.transform.position.x, this.m_kCharacterCtrl.transform.position.z);
		Vector2 vector2 = new Vector2(this.m_vTargetPos.x, this.m_vTargetPos.z);
		Vector2 a = vector2 - vector;
		POS2D pOS2D = new POS2D(ref a);
		pOS2D.Normalize();
		a.x = pOS2D.x;
		a.y = pOS2D.y;
		float d = this.GetSpeed() / 10f;
		Vector2 vector3 = a * this.m_f3DCharDeltaTime * d;
		pOS2D.x = vector3.x;
		pOS2D.y = vector3.y;
		float num = Vector2.Distance(vector, vector2);
		bool flag = false;
		if (pOS2D.Length() >= num)
		{
			vector3 = vector2 - vector;
			flag = true;
		}
		if (!this.m_MoveNoLookAt)
		{
			this.SetLookAt(this.m_vTargetPos);
		}
		if (!flag)
		{
			using (new ScopeProfile("UnityEngine.CharacterController.Move"))
			{
				CollisionFlags flg = this.m_kCharacterCtrl.Move(new Vector3(vector3.x, 0f, vector3.y));
				this.SafeStopProcess(flg);
			}
			if (!this.GetCurrentAniType().Equals(this.GetMoveAnimation().ToString()))
			{
				this.SetParentCharAnimation(this.GetMoveAnimation());
			}
		}
		else
		{
			if (this.IsBattleChar())
			{
				this.m_kCharacterCtrl.Move(new Vector3(vector3.x, 0f, vector3.y));
			}
			this.ReachedDestProcess(true);
		}
		if (this.m_bSlope)
		{
			this.CalcSlope();
		}
		if (this.m_kCharacterCtrl.transform.position.y <= 0f)
		{
			NrCharBase parentChar = this.GetParentChar();
			if (parentChar != null)
			{
				parentChar.SetSafeCharPos(this.m_kCharacterCtrl.transform.position);
			}
		}
	}

	protected void CalcSlope()
	{
		Vector3 a = this.m_kRootGameObj.transform.TransformDirection(Vector3.forward);
		Vector3 origin = this.m_kRootGameObj.transform.position + a * 0.1f;
		origin.y = 200f;
		Ray ray = new Ray(origin, new Vector3(0f, -1f, 0f));
		int mask = NrTSingleton<NkClientLogic>.Instance.CharColliderLayerMask();
		NkRaycast.Raycast(ray, 500f, mask);
		Vector3 pOINT = NkRaycast.POINT;
		ray = new Ray(this.m_kRootGameObj.transform.position, new Vector3(0f, -1f, 0f));
		NkRaycast.Raycast(ray, 500f, mask);
		Vector3 pOINT2 = NkRaycast.POINT;
		if (pOINT == pOINT2)
		{
			return;
		}
		Quaternion rotation = Quaternion.LookRotation(pOINT - pOINT2);
		Vector3 eulerAngles = rotation.eulerAngles;
		float num = Mathf.LerpAngle(this.m_fAngleX, eulerAngles.x, 2f * this.m_f3DCharDeltaTime);
		this.m_fAngleX = num;
		eulerAngles.x = num;
		rotation.eulerAngles = eulerAngles;
		this.m_kRootGameObj.transform.rotation = rotation;
	}

	protected void SafeStopProcess(CollisionFlags flg)
	{
		if ((flg == CollisionFlags.Sides || flg == CollisionFlags.Above) && this.m_pkChar != null && !this.m_pkChar.m_kCharMove.IsMoving())
		{
			this.ReachedDestProcess(true);
		}
	}

	protected virtual void ReachedDestProcess(bool bSetAni)
	{
		if (this.m_pkChar != null && this.m_pkChar.IsCharStateAtb(4L))
		{
			bSetAni = false;
		}
		if (this.m_bRequestSit)
		{
			this.SitDown(true, this.m_PosLookAt);
		}
		if (this.m_bRequestLookAt)
		{
			if (this.m_PosLookAt.x != 0f && this.m_PosLookAt.z != 0f)
			{
				this.SetLookAt(this.m_PosLookAt.x, this.m_PosLookAt.y, this.m_PosLookAt.z, false);
			}
			this.m_bRequestLookAt = false;
		}
		if (bSetAni)
		{
			this.SetParentCharAnimation(this.GetIdleAnimation());
		}
		this.m_bIsMoveToTarget = false;
	}

	private void GravityProcess()
	{
		if (null == this.m_kCharacterCtrl)
		{
			return;
		}
		if (!this.m_kCharacterCtrl.isGrounded)
		{
			this.m_kCharacterCtrl.Move(new Vector3(0f, -this.m_fGravity, 0f));
		}
	}

	public void SetSpeed(float fSpeed)
	{
		this.m_fCharSpeed = fSpeed;
		this.m_actionSetSpeed(fSpeed);
	}

	public float GetSpeed()
	{
		return this.m_fCharSpeed;
	}

	public void SetOnGround(bool bFlag)
	{
		this.m_bOnGround = bFlag;
	}

	public void Destroy()
	{
		UnityEngine.Object.DestroyImmediate(this.m_kRootGameObj);
		this.m_kRootGameObj = null;
		this.m_kBaseObject = null;
	}

	public void SetCurrentAniType(string anitype)
	{
		this.m_szCurrentAniType = anitype;
		this.m_bAnimationPlaying = true;
	}

	public string GetCurrentAniType()
	{
		return this.m_szCurrentAniType;
	}

	public virtual bool IsAniPlay()
	{
		return this.m_bAnimationPlaying;
	}

	public void OnFadeInIllumination()
	{
		this.m_tempMainColor += new Color(1f, 1f, 1f) * this.m_f3DCharDeltaTime;
		if (this.m_tempMainColor.r > this.m_tempMaxMainColor.r)
		{
			this.m_tempMainColor = this.m_tempMaxMainColor;
			this.m_bIsFading = false;
		}
		else
		{
			this.m_tempMainColor.a = 1f;
		}
		TBSUTIL.SetShaderPropertyColor(this.GetRootGameObject(), PMSHADER.RIM_MAIN, this.m_tempMainColor);
	}

	public void FadeInIllumination()
	{
		if (this.m_kRootGameObj == null)
		{
			this.m_bIsFading = false;
			return;
		}
		switch (this.GetParentCharType())
		{
		case eCharKindType.CKT_MONSTER:
			TBSUTIL.SetShaderPropertyColor(this.GetRootGameObject(), PMSHADER.SELECT_COLOR, CHAR_SHADER_COLOR.SELECT_COLOR_RED);
			break;
		case eCharKindType.CKT_NPC:
			TBSUTIL.SetShaderPropertyColor(this.GetRootGameObject(), PMSHADER.SELECT_COLOR, CHAR_SHADER_COLOR.SELECT_COLOR_BLUE);
			break;
		case eCharKindType.CKT_OBJECT:
			TBSUTIL.SetShaderPropertyColor(this.GetRootGameObject(), PMSHADER.SELECT_COLOR, CHAR_SHADER_COLOR.SELECT_COLOR_GREEN);
			break;
		}
	}

	public void OnFadeOutIllumination()
	{
		this.m_tempMainColor -= new Color(1f, 1f, 1f) * this.m_f3DCharDeltaTime;
		if (this.m_tempMainColor.r < this.m_tempDefaultMainColor.r)
		{
			this.m_tempMainColor = this.m_tempDefaultMainColor;
			this.m_bIsFading = false;
		}
		else
		{
			this.m_tempMainColor.a = 1f;
		}
		TBSUTIL.SetShaderPropertyColor(this.GetRootGameObject(), PMSHADER.RIM_MAIN, this.m_tempMainColor);
	}

	public void FadeOutIllumination()
	{
		if (this.m_kRootGameObj == null)
		{
			this.m_bIsFading = false;
			return;
		}
		TBSUTIL.SetShaderPropertyColor(this.GetRootGameObject(), PMSHADER.SELECT_COLOR, CHAR_SHADER_COLOR.SELECT_COLOR_NORMAL);
	}

	public void RequestLookAt(float x, float z)
	{
		this.m_bRequestLookAt = true;
		this.m_PosLookAt = new POS3D();
		this.m_PosLookAt.x = x;
		this.m_PosLookAt.y = 0f;
		this.m_PosLookAt.z = z;
	}

	public void SitDown(bool bNow, POS3D LookAt)
	{
		if (this.m_pkChar != null && bNow)
		{
			if (LookAt != null && LookAt.x != 0f && LookAt.z != 0f)
			{
				this.SetLookAt(LookAt.x, LookAt.y, LookAt.z, false);
			}
			this.m_pkChar.SetAnimation(this.GetIdleAnimation(), true, false);
			this.m_PosLookAt = null;
			this.m_bRequestSit = false;
		}
		else
		{
			this.m_PosLookAt = LookAt;
			this.m_bRequestSit = true;
		}
	}

	private string GetCostumeBundlePath()
	{
		if (this.m_pkBattleChar == null || this.m_pkBattleChar.GetSoldierInfo() == null)
		{
			return string.Empty;
		}
		if (!this.m_pkBattleChar.IsFriend && this.m_pkBattleChar.IsMonster)
		{
			return string.Empty;
		}
		int costumeUnique = (int)this.m_pkBattleChar.GetSoldierInfo().GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		CharCostumeInfo_Data costumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeData(costumeUnique);
		if (costumeData == null)
		{
			return string.Empty;
		}
		NrCharKindInfo parentCharKindInfo = this.GetParentCharKindInfo();
		if (parentCharKindInfo == null)
		{
			return string.Empty;
		}
		if (costumeData.m_CharCode != parentCharKindInfo.GetCode())
		{
			return string.Empty;
		}
		return NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeBundlePath(costumeUnique);
	}

	public string GetCostumePortraitPath()
	{
		if (this.m_pkBattleChar == null || this.m_pkBattleChar.GetSoldierInfo() == null)
		{
			return string.Empty;
		}
		int costumeUnique = (int)this.m_pkBattleChar.GetSoldierInfo().GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		CharCostumeInfo_Data costumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeData(costumeUnique);
		if (costumeData == null)
		{
			return string.Empty;
		}
		if (costumeData.IsNormalCostume())
		{
			return string.Empty;
		}
		NrCharKindInfo parentCharKindInfo = this.GetParentCharKindInfo();
		if (parentCharKindInfo == null)
		{
			return string.Empty;
		}
		if (costumeData.m_CharCode != parentCharKindInfo.GetCode())
		{
			return string.Empty;
		}
		return NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(costumeUnique);
	}
}
