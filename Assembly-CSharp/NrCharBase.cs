using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class NrCharBase : INrCharInput, ICloneable
{
	public enum e3DCharStep
	{
		IDLE,
		READY,
		CREATED,
		LOADCOMPLETED,
		FADEIN,
		CHARACTION,
		DELETED,
		DIED
	}

	protected NkCharIDInfo m_kIDInfo;

	protected NrPersonInfoBase m_kPersonInfo;

	public Nr3DCharBase m_k3DChar;

	protected NrCharAnimation m_kAnimation;

	protected NrCharKindInfo m_pkCharKindInfo;

	protected int m_nFaceCharKind;

	protected int m_nFaceCharGrade;

	protected eCharKindType m_eCharKindType;

	public NrCharMove m_kCharMove;

	protected Vector3 m_vReservedMoveTarget = Vector3.zero;

	protected long m_CharState = 1L;

	protected long m_CharState_Old = 1L;

	public NrDebugLoger debugLog;

	protected bool m_bCharActive;

	private NkCharChildActive kCharChildActive;

	protected bool m_bShowChar;

	protected bool m_bLoadCompleted;

	protected uint m_nBattleEffectNum;

	protected uint m_nPlunderPrepareEffectNum;

	protected uint m_nNoticeQuestEffectNum;

	protected uint m_nFakeShadowEffectNum;

	protected uint m_nFaceSolGradeEffectNum;

	private eCharAnimationType m_eIndunTriggerAnimation;

	private eCharAnimationType m_eIndunTriggerNextAnimation;

	private bool m_bIndunTriggerAnimation;

	private int m_nIndunAniCount;

	private float m_fIndunAniEndTime;

	private int m_nIndunATB;

	private bool m_bChangedItem;

	public bool m_bSubChar;

	private bool m_bProcessMouseEvent = true;

	protected bool m_bClickedMe;

	private GameObject mPreWalkEffect;

	private string mPreEffectKey = string.Empty;

	private NkHeadUpEntity m_kHeadUpEntity;

	public bool m_bIsManagedGeneration;

	protected bool m_bCreateAction;

	private bool m_bSetFirstAction;

	private float m_fDeadTime;

	private float m_nNextSpecialAni;

	private Color m_kCharColor = Color.white;

	private bool m_bExceptHideForLoad;

	protected int m_nCharSkipFrame;

	protected float m_fCharDeltaTime;

	protected NrCharDefine.CharUpdateStep m_eCharUpdateStep;

	public eCharAnimationType LoadAfterAnimation
	{
		get;
		protected set;
	}

	public bool IsQuestSubChar
	{
		get
		{
			return this.IsReserveChar(NrCharDefine.ReserveCharUnique.QUEST_SUB_NPC, NrCharDefine.ReserveCharUnique.QUEST_SUB_NPC_END);
		}
	}

	public bool IsSubChar
	{
		get
		{
			return this.IsReserveChar(NrCharDefine.ReserveCharUnique.USER_SUB_NPCMONSTER, NrCharDefine.ReserveCharUnique.USER_SUB_NPCMONSTER_END);
		}
	}

	public bool IsClientNPC
	{
		get
		{
			return this.IsReserveChar(NrCharDefine.ReserveCharUnique.CLIENT_NPC, NrCharDefine.ReserveCharUnique.CLIENT_NPC_END);
		}
	}

	public bool ProcessMouseEvent
	{
		get
		{
			return this.m_bProcessMouseEvent;
		}
		set
		{
			this.m_bProcessMouseEvent = value;
		}
	}

	public QUEST_CONST.eQUESTSTATE m_eQuestState
	{
		get;
		set;
	}

	public NrCharBase.e3DCharStep m_e3DCharStep
	{
		get;
		protected set;
	}

	public float m_fCurrentAlphaFadeLerp
	{
		get;
		protected set;
	}

	public NrCharBase()
	{
		this.m_fCurrentAlphaFadeLerp = 0f;
		this.m_e3DCharStep = NrCharBase.e3DCharStep.IDLE;
		this.debugLog = new NrDebugLoger();
		this.m_kIDInfo = new NkCharIDInfo(-1, -1);
		this.m_kAnimation = new NrCharAnimation();
		this.m_kCharMove = new NrCharMove(this);
		this.kCharChildActive = new NkCharChildActive();
	}

	public void SetChangedItem(bool flag)
	{
		this.m_bChangedItem = flag;
	}

	public bool IsChangedItem()
	{
		return this.m_bChangedItem;
	}

	public bool IsShowChar()
	{
		return this.m_bShowChar;
	}

	public virtual void Init()
	{
		if (this.m_kIDInfo != null)
		{
			this.m_kIDInfo.Init();
		}
		this.m_kPersonInfo.Init();
		this.m_pkCharKindInfo = null;
		this.m_eCharKindType = eCharKindType.CKT_USER;
		this.m_bIndunTriggerAnimation = false;
		this.m_kAnimation.Init(string.Empty, this.m_k3DChar, this.m_eCharKindType);
		this.m_kCharMove.Init();
		this.m_vReservedMoveTarget = Vector3.zero;
		this.m_nIndunATB = 0;
		this.InitRelease();
	}

	private void InitRelease()
	{
		this.m_k3DChar = null;
		this.LoadAfterAnimation = eCharAnimationType.Stay1;
		this.m_CharState = 1L;
		this.m_CharState_Old = 1L;
		this.m_bCharActive = true;
		this.kCharChildActive.Init();
		this.m_bShowChar = false;
		this.m_bLoadCompleted = false;
		this.m_nBattleEffectNum = 0u;
		this.m_nPlunderPrepareEffectNum = 0u;
		this.m_nFaceSolGradeEffectNum = 0u;
		this.UpdateInit();
		this.HeadUp_Init();
		NrTSingleton<NrNpcPosManager>.Instance.DelWideCollArea(this.GetID());
		if (this.m_nBattleEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_nBattleEffectNum);
			this.m_nBattleEffectNum = 0u;
		}
		if (this.m_nPlunderPrepareEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_nPlunderPrepareEffectNum);
			this.m_nPlunderPrepareEffectNum = 0u;
		}
		if (this.m_nNoticeQuestEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_nNoticeQuestEffectNum);
			this.m_nNoticeQuestEffectNum = 0u;
		}
		if (this.m_nFakeShadowEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_nFakeShadowEffectNum);
			this.m_nFakeShadowEffectNum = 0u;
		}
		if (this.m_nFaceSolGradeEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_nFaceSolGradeEffectNum);
			this.m_nFaceSolGradeEffectNum = 0u;
		}
	}

	public void SetPersonInfo(NrPersonInfoBase pkPersonInfo)
	{
		if (pkPersonInfo != null)
		{
			if (!pkPersonInfo.GetLeaderSoldierInfo().IsValid())
			{
				NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
				nkSoldierInfo.SetSolID(pkPersonInfo.GetBasicInfo().m_nSolID);
				pkPersonInfo.SetSoldierInfo(0, nkSoldierInfo);
			}
			this.m_kPersonInfo.SetPersonInfo(pkPersonInfo);
			this.SetCharKind(this.m_kPersonInfo.GetKind(0), false);
			if (this.m_eCharKindType != eCharKindType.CKT_USER)
			{
				this.m_kPersonInfo.SetCharName(this.m_pkCharKindInfo.GetName());
			}
			NrPersonInfoUser nrPersonInfoUser = this.m_kPersonInfo as NrPersonInfoUser;
			if (nrPersonInfoUser != null)
			{
				nrPersonInfoUser.SetBaseCharID(this.GetID());
			}
		}
	}

	public NrPersonInfoBase GetPersonInfo()
	{
		return this.m_kPersonInfo;
	}

	public virtual void SetCharKind(int charkind, bool bChanged)
	{
		if (bChanged)
		{
			this.m_kPersonInfo.SetCharKind(0, charkind);
		}
		this.m_pkCharKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(charkind);
	}

	public int GetCharKind()
	{
		if (this.m_pkCharKindInfo == null)
		{
			return 0;
		}
		return this.m_pkCharKindInfo.GetCharKind();
	}

	public NrCharKindInfo GetCharKindInfo()
	{
		return this.m_pkCharKindInfo;
	}

	public int GetFaceCharKind()
	{
		return this.m_nFaceCharKind;
	}

	public int GetFaceCharGrade()
	{
		return this.m_nFaceCharGrade;
	}

	public NrCharKindInfo GetFaceCharKindInfo()
	{
		if (this.m_nFaceCharKind == 0)
		{
			return null;
		}
		return NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_nFaceCharKind);
	}

	public long GetCharATB()
	{
		if (this.m_pkCharKindInfo == null)
		{
			return 0L;
		}
		return this.m_pkCharKindInfo.GetATB();
	}

	public float GetCharBound()
	{
		if (this.m_pkCharKindInfo != null)
		{
			return this.m_pkCharKindInfo.GetBound();
		}
		return 1f;
	}

	public float GetCharHalfBound()
	{
		return this.GetCharBound() / 2f;
	}

	public float GetPickingBound()
	{
		float result = 0f;
		if (this.IsReady3DModel())
		{
			Vector3 pickingSize = this.m_k3DChar.GetPickingSize();
			result = Mathf.Max(pickingSize.x, pickingSize.y);
		}
		return result;
	}

	public void SetIDInfo(NkCharIDInfo kIDInfo)
	{
		this.m_kIDInfo = kIDInfo;
	}

	public NkCharIDInfo GetIDInfo()
	{
		return this.m_kIDInfo;
	}

	public int GetID()
	{
		return this.m_kIDInfo.m_nClientID;
	}

	public short GetCharUnique()
	{
		return this.m_kIDInfo.m_nCharUnique;
	}

	public int GetWorldID()
	{
		return this.m_kIDInfo.m_nWorldID;
	}

	public long GetPersonID()
	{
		return this.m_kPersonInfo.GetPersonID();
	}

	public Nr3DCharBase Get3DChar()
	{
		return this.m_k3DChar;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public void SetChildActive(bool bActive)
	{
		this.m_bCharActive = bActive;
		if (this.m_k3DChar == null)
		{
			return;
		}
		if (!this.IsReady3DModel())
		{
			return;
		}
		GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
		if (rootGameObject != null && rootGameObject.activeInHierarchy != bActive)
		{
			rootGameObject.SetActive(bActive);
		}
		if (bActive)
		{
			bool bShow = this.m_kHeadUpEntity.IsCheckShowHeadUp(this.m_eCharKindType);
			this.SetShowHeadUp(bShow, true, true);
			if (!this.m_kCharMove.IsMoving())
			{
				this.SetAnimationLoadAfter(this.GetIdleAnimation());
			}
			else
			{
				this.SetAnimationLoadAfter(this.m_k3DChar.GetMoveAnimation());
			}
			this.Set3DCharStep(NrCharBase.e3DCharStep.LOADCOMPLETED);
		}
		else
		{
			this.m_bSetFirstAction = false;
			this.m_fCurrentAlphaFadeLerp = 0f;
			NrTSingleton<Nr3DCharSystem>.Instance.OnEvent3DModelCreated(rootGameObject);
			GameObject rootGameObject2 = this.m_k3DChar.GetRootGameObject();
			if (rootGameObject2 != null)
			{
				Renderer[] componentsInChildren = rootGameObject2.GetComponentsInChildren<Renderer>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					Renderer renderer = componentsInChildren[i];
					renderer.renderer.enabled = false;
				}
			}
		}
	}

	public void SetCharKindType(eCharKindType chartype)
	{
		this.m_eCharKindType = chartype;
	}

	public eCharKindType GetCharKindType()
	{
		return this.m_eCharKindType;
	}

	public NrCharAnimation GetCharAnimation()
	{
		return this.m_kAnimation;
	}

	public string GetWeaponCode()
	{
		NkSoldierInfo leaderSoldierInfo = this.GetPersonInfo().GetLeaderSoldierInfo();
		if (leaderSoldierInfo != null)
		{
			return leaderSoldierInfo.GetWeaponCode();
		}
		if (this.m_pkCharKindInfo != null)
		{
			return this.m_pkCharKindInfo.GetWeaponCode();
		}
		return eWEAPON_TYPE.WEAPON_SWORD.ToString();
	}

	public int GetWeaponType()
	{
		NkSoldierInfo leaderSoldierInfo = this.GetPersonInfo().GetLeaderSoldierInfo();
		if (leaderSoldierInfo != null)
		{
			return leaderSoldierInfo.GetWeaponType();
		}
		if (this.m_pkCharKindInfo != null)
		{
			return this.m_pkCharKindInfo.GetWeaponType();
		}
		return 1;
	}

	public bool IsGround()
	{
		return this.m_k3DChar != null && this.m_k3DChar.IsCreated() && this.m_k3DChar.IsGround();
	}

	public float GetDiffCharScale()
	{
		if (this.m_k3DChar == null)
		{
			return 1f;
		}
		return this.m_k3DChar.GetDiffCharScale() * (float)this.GetCharKindInfo().GetScale();
	}

	public virtual void SetShowHide3DModel(bool bShow, bool bHeadUpShow, bool bNameCheck)
	{
		if (this.m_k3DChar == null)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				Debug.LogError("SetShowHide3DModel not found 3D model resource.");
			}
			return;
		}
		if (this.m_bShowChar != bShow)
		{
			this.m_k3DChar.SetShowHide(bShow);
			this.m_bShowChar = bShow;
		}
		this.SetShowHeadUp(bHeadUpShow, true, bNameCheck);
	}

	public virtual void SetShowHide3DModel(bool bShow, bool bHeadUpShow, bool bNameCheck, bool bParticleSystem)
	{
		if (this.m_k3DChar == null)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				Debug.LogError("SetShowHide3DModel not found 3D model resource.");
			}
			return;
		}
		if (this.m_bShowChar != bShow)
		{
			this.m_k3DChar.SetShowHide(bShow, bParticleSystem);
			this.m_bShowChar = bShow;
			this.SetEffectShowHide(bParticleSystem);
		}
		this.SetShowHeadUp(bHeadUpShow, true, bNameCheck);
	}

	private void SetEffectShowHide(bool bShow)
	{
		if (this.m_nBattleEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.SetActiveEffect(this.m_nBattleEffectNum, bShow);
		}
		if (this.m_nPlunderPrepareEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.SetActiveEffect(this.m_nPlunderPrepareEffectNum, bShow);
		}
		if (this.m_nNoticeQuestEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.SetActiveEffect(this.m_nNoticeQuestEffectNum, bShow);
		}
		if (this.m_nFakeShadowEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.SetActiveEffect(this.m_nFakeShadowEffectNum, bShow);
		}
		if (this.m_nFaceSolGradeEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.SetActiveEffect(this.m_nFaceSolGradeEffectNum, bShow);
		}
	}

	public virtual void Release()
	{
		if (this.m_bIsManagedGeneration)
		{
			NrTSingleton<NkCharManager>.Instance.DecreaseGenerationCount();
			this.m_bIsManagedGeneration = false;
			Debug.Log("333333 m_bIsManagedGeneration = false " + this.GetCharName());
		}
		if (this.m_k3DChar != null)
		{
			this.m_kHeadUpEntity.Dispose();
			NrTSingleton<Nr3DCharSystem>.Instance.Destroy3DChar(this.GetID());
			this.m_k3DChar = null;
		}
		this.InitRelease();
	}

	public virtual bool IsReayCreateCharInfo()
	{
		return this.m_kPersonInfo.GetKind(0) != 0;
	}

	public bool IsModelLoadCompleted()
	{
		if (this.m_k3DChar == null)
		{
			return false;
		}
		if (this.m_k3DChar is Nr3DCharActor)
		{
			Nr3DCharActor nr3DCharActor = this.m_k3DChar as Nr3DCharActor;
			return nr3DCharActor.IsModelLoadCompleted();
		}
		return true;
	}

	public bool IsCreated3DModel()
	{
		return this.m_e3DCharStep >= NrCharBase.e3DCharStep.CREATED && this.m_e3DCharStep < NrCharBase.e3DCharStep.DELETED && this.m_k3DChar != null;
	}

	public bool IsReady3DModel()
	{
		return this.m_e3DCharStep >= NrCharBase.e3DCharStep.LOADCOMPLETED && this.m_e3DCharStep < NrCharBase.e3DCharStep.DELETED && this.m_k3DChar.Is3DCharActive();
	}

	public bool IsReadyCharAction()
	{
		return this.m_e3DCharStep >= NrCharBase.e3DCharStep.FADEIN && this.m_e3DCharStep <= NrCharBase.e3DCharStep.CHARACTION;
	}

	public bool IsShaderRecovery()
	{
		return this.m_e3DCharStep == NrCharBase.e3DCharStep.CHARACTION;
	}

	public virtual void SetReadyPartInfo()
	{
	}

	public bool IsHaveAnimation(eCharAnimationType anitype)
	{
		return this.m_k3DChar != null && this.m_k3DChar.IsHaveAnimation(anitype);
	}

	public float SetAnimation(eCharAnimationType anitype)
	{
		return this.SetAnimation(anitype, true, true);
	}

	public float SetAnimation(eCharAnimationType anitype, bool bForceAction, bool bBlend)
	{
		bool bForceReserved = true;
		if (this.IsReady3DModel())
		{
			bForceReserved = false;
		}
		if (this.GetCharKindType() == eCharKindType.CKT_USER && (anitype == eCharAnimationType.Stay1 || anitype == eCharAnimationType.Run1) && this.IsCharStateAtb(548L))
		{
			return 0f;
		}
		return this.m_kAnimation.PushNextAniType(anitype, bForceAction, bForceReserved, bBlend);
	}

	public virtual void ChangeWeaponTarget()
	{
	}

	public void SetAnimationLoadAfter(eCharAnimationType anitype)
	{
		this.LoadAfterAnimation = anitype;
	}

	public eCharAnimationType GetAnimation()
	{
		return this.m_kAnimation.GetCurrentAniType();
	}

	public eCharAnimationType GetIdleAnimation()
	{
		if (this.m_k3DChar != null)
		{
			return this.m_k3DChar.GetIdleAnimation();
		}
		return eCharAnimationType.Stay1;
	}

	public void SetFacialAnimation(NrCharDefine.eCharFaicalAnimationType anitype)
	{
		if (this is NrCharUser)
		{
			if (!this.IsReadyCharAction())
			{
				return;
			}
			this.m_kAnimation.SetFacialAnimation(anitype);
		}
	}

	public void SetPosition(Vector3 vPos)
	{
		Vector3 zero = Vector3.zero;
		if (this.IsReadyCharAction())
		{
			GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
			if (rootGameObject != null)
			{
				rootGameObject.transform.localPosition = vPos;
			}
		}
		this.m_kCharMove.SetCharPos(vPos, zero);
	}

	public void SetLookAt(Vector3 v3Direction, bool bInterpolation)
	{
		this.SetLookAt(v3Direction.x, v3Direction.y, v3Direction.z, bInterpolation);
	}

	public void SetLookAt(float x, float y, float z, bool bInterpolation)
	{
		if (this.IsReady3DModel())
		{
			this.m_k3DChar.SetLookAt(x, y, z, bInterpolation);
		}
		else
		{
			this.m_kPersonInfo.GetBasicInfo().m_vDirection.x = x;
			this.m_kPersonInfo.GetBasicInfo().m_vDirection.y = y;
			this.m_kPersonInfo.GetBasicInfo().m_vDirection.z = z;
		}
	}

	protected void SetWideCollArea()
	{
		if (this.m_pkCharKindInfo.IsATB(32L) && Scene.IsCurScene(Scene.Type.WORLD))
		{
			GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
			if (rootGameObject != null)
			{
				NmClickEvent componentInChildren = rootGameObject.GetComponentInChildren<NmClickEvent>();
				if (componentInChildren != null)
				{
					BoxCollider component = componentInChildren.gameObject.GetComponent<BoxCollider>();
					if (component != null)
					{
						component.size *= 2f;
					}
				}
				rootGameObject.layer = TsLayer.NPC;
				NrTSingleton<NrNpcPosManager>.Instance.AddWideCollArea(this.GetID(), this.GetPersonInfo().GetCharPos());
			}
		}
	}

	public void SetSafeCharPos(Vector3 vStartPos)
	{
		if (!this.IsReady3DModel())
		{
			return;
		}
		GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
		if (rootGameObject == null)
		{
			return;
		}
		vStartPos.y = 1000f;
		Ray ray = new Ray(vStartPos, new Vector3(0f, -1f, 0f));
		int mask = NrTSingleton<NkClientLogic>.Instance.CharColliderLayerMask();
		NkRaycast.Raycast(ray, 2000f, mask);
		Vector3 vector = NkRaycast.POINT;
		if (vector == Vector3.zero)
		{
			vector = vStartPos;
		}
		else if (0f >= vector.y)
		{
			vector.y = vStartPos.y;
		}
		if (TsPlatform.IsMobile)
		{
			if (this.IsCharKindATB(16L))
			{
				vector.y += 1.5f;
			}
			else
			{
				vector.y += 1f;
			}
		}
		else
		{
			vector.y += 1f;
		}
		rootGameObject.transform.localPosition = vector;
		if (this.m_k3DChar.GetCharController() != null)
		{
			this.m_k3DChar.GetCharController().Move(new Vector3(0f, -0.1f, 0f));
		}
	}

	public void PickingMove()
	{
		if (this.m_kCharMove.IsKeyboardOrMouseMove())
		{
			return;
		}
		if (this.m_kCharMove.IsJoystickMove())
		{
			return;
		}
		if (!NkRaycast.Raycast())
		{
			return;
		}
		RaycastHit hIT = NkRaycast.HIT;
		this.m_kCharMove.HideMoveMark = false;
		this.PickMoveStart(hIT.point.x, hIT.point.y, hIT.point.z);
	}

	public void PickMoveStart(float x, float y, float z)
	{
		NrTSingleton<NrAutoPath>.Instance.ResetData();
		this.m_kCharMove.MoveTo(x, y, z);
		GS_CHAR_FINDPATH_REQ gS_CHAR_FINDPATH_REQ = new GS_CHAR_FINDPATH_REQ();
		gS_CHAR_FINDPATH_REQ.DestPos.x = this.m_kCharMove.GetTargetPos().x;
		gS_CHAR_FINDPATH_REQ.DestPos.y = this.m_kCharMove.GetTargetPos().y;
		gS_CHAR_FINDPATH_REQ.DestPos.z = this.m_kCharMove.GetTargetPos().z;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_FINDPATH_REQ, gS_CHAR_FINDPATH_REQ);
	}

	public void SetMoveByJoystick(RaycastHit _rayCastHit)
	{
		NrTSingleton<NrAutoPath>.Instance.ResetData();
		this.m_kCharMove.MoveToByJoystick(_rayCastHit);
	}

	public void ClearReservedMoveTarget()
	{
		this.m_vReservedMoveTarget = Vector3.zero;
	}

	public void MoveTo(float x, float y, float z, bool bStraightMove)
	{
		if (x < 0.5f || z < 0.5f)
		{
			return;
		}
		if (this.m_k3DChar != null)
		{
			if (!this.IsReadyCharAction())
			{
				this.m_vReservedMoveTarget.x = x;
				this.m_vReservedMoveTarget.y = y;
				this.m_vReservedMoveTarget.z = z;
			}
			else if (!bStraightMove)
			{
				this.m_kCharMove.MoveTo(x, y, z);
				if (this.m_vReservedMoveTarget.x > 0f || this.m_vReservedMoveTarget.z > 0f)
				{
					this.m_vReservedMoveTarget = Vector3.zero;
				}
			}
			else
			{
				this.m_k3DChar.MoveTo(x, y, z);
			}
		}
		else
		{
			this.m_vReservedMoveTarget.x = x;
			this.m_vReservedMoveTarget.y = y;
			this.m_vReservedMoveTarget.z = z;
		}
	}

	public void MoveTo(Vector3 vPos)
	{
		this.MoveTo(vPos.x, vPos.y, vPos.z, false);
	}

	public void MoveTo(POS3D ToPos)
	{
		this.MoveTo(ToPos.x, ToPos.y, ToPos.z, false);
	}

	public void MoveToFast(POS3D ToPos, POS3D NextDestPos)
	{
		this.m_kCharMove.MoveToFast(ToPos.x, ToPos.y, ToPos.z, NextDestPos.x, NextDestPos.y, NextDestPos.z);
	}

	public void SetIncreaseSpeed(float fSpeed)
	{
		this.m_kCharMove.SetIncreaseSpeed(fSpeed);
	}

	public void SetSpeed(float fSpeed)
	{
		this.m_kCharMove.SetSpeed(fSpeed);
	}

	public float GetSpeed()
	{
		return this.m_kCharMove.GetSpeed();
	}

	public bool IsMovingAnimation()
	{
		return !this.m_kAnimation.IsDontMoveAnimation();
	}

	public void DeleteChar()
	{
		this.Release();
		this.Init();
	}

	public virtual void funcReachedToDest()
	{
		this.m_kAnimation.SetFinishAnimation(true);
	}

	public void StopFacialAnimation()
	{
		this.m_kAnimation.SetFacialAnimation(NrCharDefine.eCharFaicalAnimationType.FStay1);
	}

	public bool IsAutoMove()
	{
		return NrTSingleton<NrAutoPath>.Instance.IsAutoMoving();
	}

	public bool IsCharKindATB(long kindatb)
	{
		return this.m_pkCharKindInfo != null && this.m_pkCharKindInfo.IsATB(kindatb);
	}

	public bool IsCharStateAtb(long eState)
	{
		return (this.m_CharState & eState) != 0L;
	}

	public bool IsCharStateAtb_Old(long eState)
	{
		return (this.m_CharState_Old & eState) != 0L;
	}

	public void SetCharState(long eState)
	{
		if ((this.m_CharState & eState) == 0L)
		{
			this.m_CharState_Old = this.m_CharState;
			if (this.CanChangeState(eState))
			{
				this.m_CharState = eState;
			}
		}
	}

	public void SetCharState_ALL(long eState)
	{
		this.m_CharState_Old = this.m_CharState;
		this.m_CharState = eState;
	}

	public void DelCharState(long eState)
	{
		if (this.IsCharStateAtb(eState))
		{
			this.m_CharState &= ~eState;
		}
	}

	public bool IsChangedCharState()
	{
		return this.m_CharState != this.m_CharState_Old;
	}

	public bool CanChangeState(long eTo)
	{
		if ((this.m_CharState & 1L) != 0L)
		{
			this.DelCharState(1L);
		}
		if ((eTo & 1L) != 0L)
		{
			this.m_CharState = 0L;
		}
		return true;
	}

	public long GetCharState()
	{
		return this.m_CharState;
	}

	public long GetCharState_Old()
	{
		return this.m_CharState_Old;
	}

	public bool IsSetNoticeQuestEffect()
	{
		return this.m_nNoticeQuestEffectNum > 0u;
	}

	public uint GetNoticeQuestEffectNum()
	{
		return this.m_nNoticeQuestEffectNum;
	}

	public void InitNoticeQuestEffectNum()
	{
		this.m_nNoticeQuestEffectNum = 0u;
	}

	public bool SetCharEffectFromState(bool bActiveEffect)
	{
		if (this.GetID() == 1)
		{
			return false;
		}
		if (bActiveEffect && this.GetCharKindType() == eCharKindType.CKT_NPC)
		{
			this.SetQuestSymbol();
			return true;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsEffectEnable())
		{
			return false;
		}
		if (this.m_nNoticeQuestEffectNum == 0u && !this.m_bSubChar && NrTSingleton<NkQuestManager>.Instance.CheckClickObjectChar(this.GetCharKindInfo().GetCharKind()))
		{
			this.m_nNoticeQuestEffectNum = NrTSingleton<NkEffectManager>.Instance.AddEffect("FX_NOTICE_QUESTMONSTER", this);
			return true;
		}
		if (bActiveEffect && (this.IsCharStateAtb(16384L) || this.IsCharStateAtb(524288L)))
		{
			if (this.m_nBattleEffectNum == 0u)
			{
				Transform nameDummy = this.m_kHeadUpEntity.GetNameDummy();
				if (nameDummy != null)
				{
					this.m_nBattleEffectNum = NrTSingleton<NkEffectManager>.Instance.AddEffectByTarget("BATTLEING", this, nameDummy.position);
				}
				return true;
			}
		}
		else if (this.m_nBattleEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_nBattleEffectNum);
			this.m_nBattleEffectNum = 0u;
			return true;
		}
		return false;
	}

	public void SetFakeShadowEnable(bool bEnable)
	{
		if (!this.IsReadyCharAction())
		{
			return;
		}
		if (this.GetCharKindType() == eCharKindType.CKT_OBJECT)
		{
			return;
		}
		if (bEnable && this.GetCharKindInfo().IsATB(1048576L))
		{
			return;
		}
		if (this.m_nFakeShadowEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.SetActiveEffect(this.m_nFakeShadowEffectNum, bEnable);
		}
		else
		{
			this.m_nFakeShadowEffectNum = NrTSingleton<NkEffectManager>.Instance.AddEffect("FAKE_SHADOW", this);
		}
	}

	public string GetCharName()
	{
		return this.m_kPersonInfo.GetCharName();
	}

	public GameObject GetCharObject()
	{
		if (this.m_k3DChar == null)
		{
			return null;
		}
		return this.m_k3DChar.GetRootGameObject();
	}

	public Vector3 GetCenterPosition()
	{
		if (this.m_k3DChar == null)
		{
			return Vector3.zero;
		}
		CharacterController charController = this.m_k3DChar.GetCharController();
		if (charController == null)
		{
			return Vector3.zero;
		}
		return this.GetCharObject().transform.position + charController.center;
	}

	public Vector3 GetCameraPosition()
	{
		if (this.m_k3DChar == null)
		{
			return Vector3.zero;
		}
		CharacterController charController = this.m_k3DChar.GetCharController();
		if (charController == null)
		{
			return Vector3.zero;
		}
		Vector3 center = charController.center;
		center.y += charController.height / 4f;
		return this.GetCharObject().transform.position + center;
	}

	public void SitDown(bool bNow, POS3D LookAt)
	{
		if (this.m_k3DChar != null)
		{
			this.m_k3DChar.SitDown(bNow, LookAt);
		}
	}

	public bool IsReserveChar(NrCharDefine.ReserveCharUnique eStart, NrCharDefine.ReserveCharUnique eEnd)
	{
		return this.GetCharUnique() >= (short)eStart && this.GetCharUnique() < (short)eEnd;
	}

	public void SetIndunTriggerAnimation(bool bSet, eCharAnimationType aniType, int nCount, eCharAnimationType NextAniType)
	{
		this.m_bIndunTriggerAnimation = bSet;
		this.m_eIndunTriggerAnimation = aniType;
		this.m_eIndunTriggerNextAnimation = NextAniType;
		this.m_nIndunAniCount = nCount;
		this.m_fIndunAniEndTime = 0f;
	}

	public void SetIndunCharATB(int nATB)
	{
		this.m_nIndunATB = nATB;
	}

	public virtual string GetUserGuildName()
	{
		return string.Empty;
	}

	public virtual long GetUserGuildID()
	{
		return 0L;
	}

	public virtual short GetUserColosseumGrade()
	{
		return 0;
	}

	public void SetClickMe()
	{
		this.m_bClickedMe = true;
	}

	public void CancelClickMe()
	{
		this.m_bClickedMe = false;
	}

	public virtual void MouseEvent_Enter()
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsCharEventEnable())
		{
			return;
		}
		if (TsPlatform.IsMobile && null != Camera.main)
		{
			maxCamera component = Camera.main.gameObject.GetComponent<maxCamera>();
			if (component.CAMERA_ROTAE)
			{
				return;
			}
		}
		if (this.m_k3DChar == null)
		{
			return;
		}
		NrTSingleton<NkClientLogic>.Instance.SetFocusCharID(this.GetID(), true);
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			if (this.GetID() == 1)
			{
				return;
			}
			break;
		case eCharKindType.CKT_MONSTER:
			this.SetShowHeadUp(true, false, false);
			if (this.m_bProcessMouseEvent)
			{
				this.m_k3DChar.FadeInIllumination();
			}
			break;
		case eCharKindType.CKT_NPC:
			if (!this.m_bProcessMouseEvent)
			{
				return;
			}
			this.m_k3DChar.FadeInIllumination();
			if (this.IsSubChar)
			{
				this.SetShowHeadUp(true, false, false);
			}
			break;
		case eCharKindType.CKT_OBJECT:
			if (this.IsCharKindATB(8589934592L))
			{
				this.m_k3DChar.FadeInIllumination();
				this.SetShowHeadUp(true, false, false);
			}
			else if (NrTSingleton<NkQuestManager>.Instance.CheckClickObjectChar(this.GetCharKindInfo().GetCharKind()) && this.IsCharKindATB(64L))
			{
				this.m_k3DChar.FadeInIllumination();
				this.SetShowHeadUp(true, false, false);
			}
			break;
		}
	}

	public virtual void MouseEvent_Exit()
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsCharEventEnable())
		{
			return;
		}
		if (this.m_k3DChar == null)
		{
			return;
		}
		NrTSingleton<NkClientLogic>.Instance.SetFocusCharID(this.GetID(), false);
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			if (this.GetID() == 1)
			{
				return;
			}
			break;
		case eCharKindType.CKT_MONSTER:
			this.SetShowHeadUp(false, false, false);
			if (this.m_bProcessMouseEvent)
			{
				this.m_k3DChar.FadeOutIllumination();
			}
			break;
		case eCharKindType.CKT_NPC:
			if (!this.m_bProcessMouseEvent)
			{
				return;
			}
			this.m_k3DChar.FadeOutIllumination();
			if (this.IsSubChar)
			{
				this.SetShowHeadUp(false, false, false);
			}
			break;
		case eCharKindType.CKT_OBJECT:
			if (this.IsCharKindATB(64L) || this.IsCharKindATB(8589934592L))
			{
				this.m_k3DChar.FadeOutIllumination();
				this.SetShowHeadUp(false, false, false);
			}
			break;
		}
	}

	public virtual void MouseEvent_Over()
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsCharEventEnable())
		{
			return;
		}
		if (!this.m_bProcessMouseEvent)
		{
			return;
		}
		bool flag = false;
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			if (this.GetID() == 1)
			{
				return;
			}
			break;
		case eCharKindType.CKT_MONSTER:
			flag = NkInputManager.IsLeftButtonUP();
			break;
		case eCharKindType.CKT_NPC:
			flag = NkInputManager.IsLeftButtonUP();
			break;
		case eCharKindType.CKT_OBJECT:
			if (this.IsCharKindATB(64L) || this.IsCharKindATB(8589934592L))
			{
				if (this.IsCharKindATB(268435456L))
				{
					return;
				}
				flag = NkInputManager.IsLeftButtonUP();
			}
			break;
		}
		if (flag)
		{
			NrTSingleton<NkClientLogic>.Instance.SetPickChar(this);
		}
		if (TsPlatform.IsEditor)
		{
			if (NkInputManager.IsRightButtonUP())
			{
				this.OnMouseRightClickEvent();
			}
		}
		else if (TsPlatform.IsMobile && 0 < Input.touchCount && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			this.OnMouseRightClickEvent();
		}
	}

	public void OnMouseLeftClickEvent()
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsCharEventEnable())
		{
			return;
		}
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			if (this.GetID() == 1)
			{
				return;
			}
			break;
		case eCharKindType.CKT_MONSTER:
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			if (Battle.IsDownLoadingMap())
			{
				Debug.Log("Before Map downLoad");
			}
			else
			{
				try
				{
					NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
					if (!kMyCharInfo.IsEnableBattleUseActivityPoint(1))
					{
						string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("488");
						Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					}
					else
					{
						NrPersonInfoUser personInfoUser = nrCharUser.GetPersonInfoUser();
						if (personInfoUser != null)
						{
							bool flag = false;
							int num = 0;
							int num2 = 0;
							for (int i = 0; i < 6; i++)
							{
								if (kMyCharInfo.IsAddBattleSoldier(i))
								{
									NkSoldierInfo soldierInfo = personInfoUser.GetSoldierInfo(i);
									if (soldierInfo == null || !soldierInfo.IsValid())
									{
										if (!flag)
										{
											flag = true;
										}
									}
									else
									{
										num++;
									}
									num2++;
								}
							}
							if (flag)
							{
								MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
								string empty = string.Empty;
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
								{
									NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("146"),
									"currentnum",
									num.ToString(),
									"maxnum",
									num2.ToString()
								});
								msgBoxUI.SetMsg(new YesDelegate(this.OnBattleOK), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("21"), empty, eMsgType.MB_OK_CANCEL);
							}
							else
							{
								bool flag2 = false;
								for (int i = 0; i < 6; i++)
								{
									NkSoldierInfo soldierInfo = personInfoUser.GetSoldierInfo(i);
									if (soldierInfo != null && soldierInfo.IsValid())
									{
										if (soldierInfo.IsInjuryStatus())
										{
											flag2 = true;
											break;
										}
									}
								}
								if (flag2)
								{
									MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
									msgBoxUI2.SetMsg(new YesDelegate(this.OnBattleInjuryOk), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("21"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("20"), eMsgType.MB_OK_CANCEL);
								}
								else
								{
									if (this.GetCharKindInfo().GetCHARKIND_MONSTERINFO() != null)
									{
										NrTSingleton<GameGuideManager>.Instance.MonsterLevel = (int)this.GetCharKindInfo().GetCHARKIND_MONSTERINFO().MINLEVEL;
									}
									else
									{
										NrTSingleton<GameGuideManager>.Instance.MonsterLevel = 0;
									}
									this.OnBattleOK(null);
								}
							}
						}
					}
				}
				catch (FormatException)
				{
				}
			}
			break;
		}
		case eCharKindType.CKT_NPC:
		{
			if (NrTSingleton<NkCharManager>.Instance.GetChar(1) == null)
			{
				return;
			}
			if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
			{
				return;
			}
			long num3 = (long)this.GetCharKindInfo().GetCharKind();
			NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(30, num3, 1L);
			NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(8, num3, 1L);
			NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(99, num3, 1L);
			NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(48, num3, 1L);
			if (this.IsSubChar)
			{
				return;
			}
			if (this.GetCharUnique() >= 31300 && this.GetCharUnique() <= 31400)
			{
				string text = NrTSingleton<NkQuestManager>.Instance.IsCheckCodeANDParam(QUEST_CONST.eQUESTCODE.QUESTCODE_TAKECHAR, num3);
				if (text != string.Empty)
				{
					TakeTalk_DLG takeTalk_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TAKETALK_DLG) as TakeTalk_DLG;
					if (takeTalk_DLG != null)
					{
						takeTalk_DLG.SetNpc((int)num3, this.GetCharUnique(), text);
						takeTalk_DLG.Show();
						return;
					}
				}
			}
			if (this.IsCharKindATB(562949953421312L))
			{
				GS_TREASUREBOX_CLICK_REQ gS_TREASUREBOX_CLICK_REQ = new GS_TREASUREBOX_CLICK_REQ();
				gS_TREASUREBOX_CLICK_REQ.i32CharUnique = (int)this.GetCharUnique();
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TREASUREBOX_CLICK_REQ, gS_TREASUREBOX_CLICK_REQ);
				return;
			}
			if (this.IsCharKindATB(1125899906842624L))
			{
				if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsBountyHunt())
				{
					return;
				}
				GS_BABELTOWER_GOLOBBY_REQ gS_BABELTOWER_GOLOBBY_REQ = new GS_BABELTOWER_GOLOBBY_REQ();
				gS_BABELTOWER_GOLOBBY_REQ.mode = 0;
				gS_BABELTOWER_GOLOBBY_REQ.babel_floor = 0;
				gS_BABELTOWER_GOLOBBY_REQ.babel_subfloor = 0;
				gS_BABELTOWER_GOLOBBY_REQ.nPersonID = 0L;
				gS_BABELTOWER_GOLOBBY_REQ.i16BountyHuntUnique = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_GOLOBBY_REQ, gS_BABELTOWER_GOLOBBY_REQ);
				return;
			}
			else
			{
				NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NPCTALK_DLG);
				if (npcTalkUI_DLG == null)
				{
					return;
				}
				npcTalkUI_DLG.SetNpcID((int)num3, this.GetCharUnique());
				npcTalkUI_DLG.Show();
				if (this.GetCharKindInfo().GetCode().Equals("Battle_Custodian"))
				{
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "SB", "NPC_CLICK", true, new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
				this.MouseEvent_Exit();
			}
			break;
		}
		case eCharKindType.CKT_OBJECT:
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char == null)
			{
				return;
			}
			try
			{
				if (NrTSingleton<NkQuestManager>.Instance.CheckClickObjectChar(this.GetCharKindInfo().GetCharKind()) || this.IsCharKindATB(8589934592L))
				{
					Vector3 vector = Camera.main.WorldToScreenPoint(@char.GetPersonInfo().GetCharPos());
					GS_COLLECT_START_REQ gS_COLLECT_START_REQ = new GS_COLLECT_START_REQ();
					gS_COLLECT_START_REQ.i32CharUnique = (int)this.GetCharUnique();
					gS_COLLECT_START_REQ.i32PosX = (int)vector.x;
					gS_COLLECT_START_REQ.i32PosY = (int)vector.y;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLLECT_START_REQ, gS_COLLECT_START_REQ);
				}
				@char.m_kCharMove.MoveStop(true, false);
			}
			catch (FormatException)
			{
			}
			this.MouseEvent_Exit();
			break;
		}
		}
	}

	private void OnMouseRightClickEvent()
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsCharEventEnable())
		{
			return;
		}
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			if (this.GetID() == 1)
			{
				return;
			}
			NrTSingleton<CRightClickMenu>.Instance.CreateUI(this.GetPersonID(), this.GetCharUnique(), this.m_kPersonInfo.GetCharName(), CRightClickMenu.KIND.USER_CLICK, CRightClickMenu.TYPE.NAME_SECTION_3);
			break;
		case eCharKindType.CKT_MONSTER:
			if (this.IsCharStateAtb(16384L) || this.IsCharStateAtb(32768L))
			{
				NrTSingleton<CRightClickMenu>.Instance.CreateUI(this.GetPersonID(), this.GetCharUnique(), this.m_kPersonInfo.GetCharName(), CRightClickMenu.KIND.MONSTER_CLICK, CRightClickMenu.TYPE.NAME_SECTION_2);
			}
			break;
		}
	}

	public void Update_MouseEvent()
	{
		if (!this.m_bProcessMouseEvent)
		{
			return;
		}
		if (!this.m_bClickedMe)
		{
			return;
		}
		if (!NkInputManager.IsInputMode)
		{
			return;
		}
		float distance = 1f;
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			return;
		case eCharKindType.CKT_SOLDIER:
			return;
		case eCharKindType.CKT_OBJECT:
			if (this.IsCharKindATB(64L) || this.IsCharKindATB(8589934592L))
			{
				distance = 0f;
			}
			break;
		}
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		bool flag = this.IsCloseToTalkNPC(ref @char, distance);
		if (flag)
		{
			if (TsPlatform.IsMobile)
			{
				if (NrTSingleton<NrMainSystem>.Instance.GetInputManager().GetInputInfo(0).evt == INPUT_INFO.INPUT_EVENT.TAP || NrTSingleton<NrMainSystem>.Instance.GetInputManager().GetInputInfo(0).evt == INPUT_INFO.INPUT_EVENT.NO_CHANGE)
				{
					this.OnMouseLeftClickEvent();
				}
				else
				{
					this.MouseEvent_Exit();
				}
			}
			else
			{
				this.OnMouseLeftClickEvent();
			}
			NrCharBase pickChar = NrTSingleton<NkClientLogic>.Instance.GetPickChar();
			if (pickChar != null && pickChar.GetID() == this.GetID())
			{
				NrTSingleton<NkClientLogic>.Instance.InitPickChar();
			}
		}
	}

	public bool IsCloseToTalkNPC(ref NrCharBase pkTargetChar, float distance)
	{
		if (pkTargetChar != null)
		{
			distance += this.GetPickingBound() + pkTargetChar.GetPickingBound();
			float num = Vector3.Distance(this.m_kPersonInfo.GetCharPos(), pkTargetChar.GetPersonInfo().GetCharPos());
			if (num <= distance)
			{
				return true;
			}
		}
		return false;
	}

	public void OnWalkEffect(string EffectKey, Vector3 vPos)
	{
		if (this.mPreEffectKey != EffectKey)
		{
			bool flag = EffectKey != string.Empty;
			float t = 3f;
			TBSUTIL.SetParticleEmitter(this.mPreWalkEffect, false);
			UnityEngine.Object.Destroy(this.mPreWalkEffect, t);
			if (flag)
			{
				GameObject gameObject = EffectDefine.Attach(string.Format("{0}_{1}", this.GetCharName(), EffectKey));
				gameObject.transform.parent = this.Get3DChar().GetRootGameObject().transform;
				gameObject.transform.position = vPos;
				NrTSingleton<NkEffectManager>.Instance.AddEffect(EffectKey, gameObject);
				this.mPreWalkEffect = gameObject;
			}
			this.mPreEffectKey = EffectKey;
		}
	}

	public void OnBattleOK(object a_oObject)
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char == null)
		{
			return;
		}
		GS_BATTLE_OPEN_REQ gS_BATTLE_OPEN_REQ = new GS_BATTLE_OPEN_REQ();
		gS_BATTLE_OPEN_REQ.CharUnique = (int)this.GetCharUnique();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_OPEN_REQ, gS_BATTLE_OPEN_REQ);
		@char.m_kCharMove.MoveStop(true, false);
		if (!this.IsCharKindATB(4194304L))
		{
			this.SetLookAt(@char.m_k3DChar.GetRootGameObject().transform.position, false);
		}
		@char.SetLookAt(this.m_k3DChar.GetRootGameObject().transform.position, false);
		@char.SetAnimation(eCharAnimationType.Run1);
		this.SetAnimation(eCharAnimationType.Run1);
		this.MouseEvent_Exit();
		this.CancelClickMe();
	}

	public void OnBattleInjuryOk(object a_oObject)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLMILITARYGROUP_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.SOLMILITARYGROUP_DLG);
		}
		this.MouseEvent_Exit();
		this.CancelClickMe();
	}

	public void HeadUp_Init()
	{
		if (this.m_kHeadUpEntity == null)
		{
			this.m_kHeadUpEntity = new NkHeadUpEntity();
		}
	}

	public void ReadyHeadUp(int basescale)
	{
		this.HeadUp_Init();
		this.m_kHeadUpEntity.SetLinkHeadUpRoot(this.m_k3DChar.GetBaseObject(), false, basescale);
	}

	public void SetShowHeadUp(bool bShow, bool bForce, bool bNameCheck)
	{
		if (bForce || this.m_kHeadUpEntity.IsShowHeadUp() != bShow)
		{
			if (bShow && bNameCheck)
			{
				bShow = this.m_kHeadUpEntity.IsCheckShowHeadUp(this.m_eCharKindType);
			}
			this.m_kHeadUpEntity.SetShowHeadUp(bShow);
			if (bShow)
			{
				this.SetBillboardScale();
			}
		}
	}

	public void SetBillboardScale()
	{
		if (this.m_kHeadUpEntity.IsShowHeadUp() && this.m_eCharUpdateStep >= NrCharDefine.CharUpdateStep.CHARUPDATESTEP_FAR)
		{
			return;
		}
		this.m_kHeadUpEntity.SetTargetScale();
		this.m_kHeadUpEntity.SyncRotate();
	}

	public void SyncBillboardRotate(bool bScaleUpdate)
	{
		if (this.m_eCharUpdateStep >= NrCharDefine.CharUpdateStep.CHARUPDATESTEP_FAR)
		{
			if (this.m_kHeadUpEntity.IsShowHeadUp())
			{
				this.m_kHeadUpEntity.SetShowHeadUp(false);
			}
			return;
		}
		if (!this.m_kHeadUpEntity.IsShowHeadUp() && this.m_kHeadUpEntity.IsCheckShowHeadUp(this.GetCharKindType()))
		{
			this.m_kHeadUpEntity.SetShowHeadUp(true);
		}
		this.m_kHeadUpEntity.SyncBillboardRotate(bScaleUpdate);
	}

	public void MakeCharName(bool bShowCharUnique, short iColosseumGrade)
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
		{
			return;
		}
		GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
		if (rootGameObject == null)
		{
			return;
		}
		string text = this.GetCharName();
		if (bShowCharUnique)
		{
			text = text + " (" + this.GetCharUnique().ToString() + ")";
		}
		bool ridestate = false;
		if (this.m_k3DChar is Nr3DCharActor)
		{
			Nr3DCharActor nr3DCharActor = this.m_k3DChar as Nr3DCharActor;
			ridestate = nr3DCharActor.IsRideState();
		}
		if (this.IsSubChar)
		{
			this.m_kHeadUpEntity.SetSubChar(true);
			this.m_kHeadUpEntity.SetShowHeadUp(true);
		}
		string gradeTexture = NrTSingleton<NrTable_ColosseumRankReward_Manager>.Instance.GetGradeTexture(iColosseumGrade);
		this.m_kHeadUpEntity.MakeName(this.m_eCharKindType, gradeTexture, text, ridestate);
	}

	public void MakeCharGuildNameShow(string strGuildName, long i64GuildID)
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
		{
			return;
		}
		if (this.m_k3DChar == null)
		{
			return;
		}
		if (this.m_eCharKindType != eCharKindType.CKT_USER)
		{
			return;
		}
		GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
		if (rootGameObject == null)
		{
			return;
		}
		bool ridestate = false;
		if (this.m_k3DChar is Nr3DCharActor)
		{
			Nr3DCharActor nr3DCharActor = this.m_k3DChar as Nr3DCharActor;
			ridestate = nr3DCharActor.IsRideState();
		}
		if (this.IsSubChar)
		{
			this.m_kHeadUpEntity.SetSubChar(true);
			this.m_kHeadUpEntity.SetShowHeadUp(true);
		}
		this.m_kHeadUpEntity.MakeCharGuild(this.m_eCharKindType, i64GuildID, strGuildName, new Color(0f, 1f, 0f), ridestate);
	}

	public bool MakeChatText(string chattext, bool checkshowstatus)
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
		{
			return false;
		}
		bool ridestate = false;
		if (this.m_k3DChar is Nr3DCharActor)
		{
			Nr3DCharActor nr3DCharActor = this.m_k3DChar as Nr3DCharActor;
			ridestate = nr3DCharActor.IsRideState();
		}
		if (this.m_eCharKindType == eCharKindType.CKT_USER)
		{
			string userGuildName = ((NrCharUser)this).GetUserGuildName();
			this.m_kHeadUpEntity.MakeChat(this.m_eCharKindType, chattext, userGuildName, ridestate, checkshowstatus);
		}
		else
		{
			this.m_kHeadUpEntity.MakeChat(this.m_eCharKindType, chattext, string.Empty, ridestate, checkshowstatus);
		}
		return true;
	}

	public bool MakeChatText(string colorText, string chattext, bool checkshowstatus)
	{
		this.m_kHeadUpEntity.SetColorText(colorText);
		return this.MakeChatText(chattext, checkshowstatus);
	}

	public bool MakeCharStatus(GameObject pkStatus, float fScale)
	{
		return this.m_kHeadUpEntity.MakeCharStatus(this.m_eCharKindType, pkStatus, fScale);
	}

	public void HideChatText()
	{
		this.m_kHeadUpEntity.HideChatText();
	}

	public void HideCharStatus()
	{
		this.m_kHeadUpEntity.HideCharStatus();
	}

	public void RefreshCharName(bool bShowCharUnique)
	{
		if (!this.IsReady3DModel())
		{
			return;
		}
		this.MakeCharName(bShowCharUnique, this.GetUserColosseumGrade());
	}

	public Transform GetNameDummy()
	{
		return this.m_kHeadUpEntity.GetNameDummy();
	}

	public GameObject GetCharStatusObject()
	{
		return this.m_kHeadUpEntity.GetUserStatus();
	}

	protected void SetQuestSymbol()
	{
		QUEST_CONST.eQUESTSTATE questState = StageWorld.GetQuestState(this.GetCharKindInfo().GetCharKind());
		if (this.m_eQuestState != questState)
		{
			if (questState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_NOT_VIEW && questState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE && questState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_END)
			{
				this.HideCharStatus();
				eSYMBOL_MARK eSYMBOL_MARK = eSYMBOL_MARK.END;
				if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
				{
					eSYMBOL_MARK = eSYMBOL_MARK.Yellow_Wan;
				}
				else if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING)
				{
					eSYMBOL_MARK = eSYMBOL_MARK.Gray_Wan;
				}
				else if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
				{
					eSYMBOL_MARK = eSYMBOL_MARK.Yellow_Su;
				}
				else if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_DAYQUEST_ACCEPTABLE)
				{
					eSYMBOL_MARK = eSYMBOL_MARK.Blue_Su;
				}
				else if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_DAYQUEST_COMPLETE)
				{
					eSYMBOL_MARK = eSYMBOL_MARK.Blue_Wan;
				}
				if (eSYMBOL_MARK != eSYMBOL_MARK.END)
				{
					string symbol = string.Empty;
					switch (eSYMBOL_MARK)
					{
					case eSYMBOL_MARK.Yellow_Wan:
						symbol = "QUEST_COMPLETE_POSSIBLE";
						break;
					case eSYMBOL_MARK.Yellow_Su:
						symbol = "QUEST_ACCEPT_POSSIBLE";
						break;
					case eSYMBOL_MARK.Gray_Wan:
						symbol = "QUEST_COMPLETE_IMPOSSIBLE";
						break;
					case eSYMBOL_MARK.Gray_Su:
						symbol = "QUEST_ACCEPT_IMPOSSIBLE";
						break;
					case eSYMBOL_MARK.Blue_Wan:
						symbol = "QUEST_BLUE_COMPLETE_POSSIBLE";
						break;
					case eSYMBOL_MARK.Blue_Su:
						symbol = "QUEST_BLUE_ACCEPT_POSSIBLE";
						break;
					}
					this.SetSymbol(symbol);
					this.m_eQuestState = questState;
				}
			}
			else
			{
				this.HideCharStatus();
				this.m_eQuestState = questState;
			}
		}
	}

	protected bool SetSymbol(string szSymbolPath)
	{
		if (NrCharDefine.IsSubChar(this.GetCharUnique()))
		{
			return true;
		}
		GameObject gameObject = new GameObject(szSymbolPath);
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(szSymbolPath);
		if (effectInfo != null)
		{
			NkEffectUnit nkEffectUnit = new NkEffectUnit(effectInfo, gameObject);
			float num = 3f;
			nkEffectUnit.Scale *= num;
			NrTSingleton<NkEffectManager>.Instance.ExternAddEffect(nkEffectUnit);
		}
		return this.MakeCharStatus(gameObject, 1f);
	}

	public virtual void UpdateInit()
	{
		this.m_bIsManagedGeneration = false;
		this.Set3DCharStep(NrCharBase.e3DCharStep.IDLE);
		this.m_bExceptHideForLoad = false;
		this.m_bCreateAction = false;
		this.m_bSetFirstAction = false;
		this.m_fDeadTime = 0f;
		this.m_fCurrentAlphaFadeLerp = 0f;
		this.m_nNextSpecialAni = 0f;
		this.m_nCharSkipFrame = 0;
		this.m_fCharDeltaTime = 0f;
		this.m_eCharUpdateStep = NrCharDefine.CharUpdateStep.CHARUPDATESTEP_NONE;
	}

	public void Set3DCharStep(NrCharBase.e3DCharStep charstep)
	{
		this.m_e3DCharStep = charstep;
	}

	public NrCharBase.e3DCharStep Get3DCharStep()
	{
		return this.m_e3DCharStep;
	}

	public void SetExceptHideForLoad(bool bShow)
	{
		this.m_bExceptHideForLoad = bShow;
	}

	public bool GetExceptHideForLoad()
	{
		return this.m_bExceptHideForLoad;
	}

	public virtual Nr3DCharBase Create3DGrahpicData()
	{
		this.Set3DCharStep(NrCharBase.e3DCharStep.CREATED);
		if (NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
		{
			if (this.GetID() > 1)
			{
				this.m_k3DChar.removeScript = true;
			}
		}
		else
		{
			this.m_k3DChar.removeScript = true;
		}
		return this.m_k3DChar;
	}

	public void Refresh3DChar()
	{
		this.Set3DCharStep(NrCharBase.e3DCharStep.DELETED);
	}

	public bool PostUpdate()
	{
		this.m_fCharDeltaTime = 0f;
		return true;
	}

	public virtual bool Update()
	{
		this.m_fCharDeltaTime += Time.deltaTime;
		if (!this.m_bCharActive)
		{
			return false;
		}
		switch (this.m_e3DCharStep)
		{
		case NrCharBase.e3DCharStep.IDLE:
			switch (Scene.CurScene)
			{
			case Scene.Type.EMPTY:
			case Scene.Type.ERROR:
			case Scene.Type.SYSCHECK:
			case Scene.Type.PREDOWNLOAD:
			case Scene.Type.NPATCH_DOWNLOAD:
			case Scene.Type.LOGIN:
			case Scene.Type.INITIALIZE:
			case Scene.Type.PREPAREGAME:
			case Scene.Type.JUSTWAIT:
			case Scene.Type.BATTLE:
			case Scene.Type.CUTSCENE:
			case Scene.Type.SOLDIER_BATCH:
				goto IL_BF;
			}
			this.debugLog.Log("state IDLE.");
			this.Set3DCharStep(NrCharBase.e3DCharStep.READY);
			IL_BF:
			break;
		case NrCharBase.e3DCharStep.READY:
			this.debugLog.Log("state READY.");
			if (StageSystem.IsStable)
			{
				if (this.IsReayCreateCharInfo())
				{
					this.Create3DGrahpicData();
					this.m_bSetFirstAction = false;
					this.m_bCreateAction = false;
					this.m_fCurrentAlphaFadeLerp = 0f;
				}
			}
			break;
		case NrCharBase.e3DCharStep.CREATED:
			if (!this.m_bCreateAction)
			{
				this.OnCreateAction();
			}
			break;
		case NrCharBase.e3DCharStep.LOADCOMPLETED:
			this.debugLog.Log("state LOADCOMPLETED.");
			this.SetFirstAction();
			if (!NrCharAnimation.IsIdleAnimation(this.LoadAfterAnimation))
			{
				this.m_kAnimation.ProcessAnimation(this.m_fCharDeltaTime);
			}
			this.MakeFadeIn();
			break;
		case NrCharBase.e3DCharStep.FADEIN:
			this.MakeFadeIn();
			this.SetCharAction();
			break;
		case NrCharBase.e3DCharStep.CHARACTION:
			if (this.m_nCharSkipFrame > 0)
			{
				this.m_nCharSkipFrame = Math.Max(0, this.m_nCharSkipFrame - 1);
				if (this.m_nCharSkipFrame > 0)
				{
					return false;
				}
			}
			this.SetCharAction();
			break;
		case NrCharBase.e3DCharStep.DELETED:
			this.debugLog.Log("state DELETED.");
			this.Release();
			this.m_eQuestState = QUEST_CONST.eQUESTSTATE.QUESTSTATE_END;
			this.Set3DCharStep(NrCharBase.e3DCharStep.IDLE);
			if (this.m_kCharMove.IsFastMoving())
			{
				this.m_kPersonInfo.SetCharPos(this.m_kCharMove.GetTargetPos());
				this.MoveTo(this.m_kCharMove.GetFastMoveNextTargetPos());
			}
			else if (this.m_kCharMove.IsMoving())
			{
				this.MoveTo(this.m_kCharMove.GetTargetPos());
			}
			if (this.GetID() == 1)
			{
				TsAudioManager.Instance.InitAudioListenerSwitcher();
			}
			break;
		case NrCharBase.e3DCharStep.DIED:
			this.debugLog.Log("state DIED.");
			if (this.m_fDeadTime > 0f && Time.time - this.m_fDeadTime > 3f)
			{
				NrTSingleton<NkCharManager>.Instance.DeleteChar(this.GetID());
			}
			else if (this.m_k3DChar != null && this.m_k3DChar.IsAniPlay())
			{
				this.m_kAnimation.ProcessAnimation(this.m_fCharDeltaTime);
			}
			break;
		}
		return true;
	}

	public void LateUpdate(Vector3 pkMyCharPos)
	{
		if (!this.m_bCharActive)
		{
			return;
		}
		if (this.m_nCharSkipFrame == 0)
		{
			Vector3 charPos = this.m_kCharMove.GetCharPos();
			float num = Vector3.Distance(pkMyCharPos, charPos);
			if (num > 40f)
			{
				this.m_eCharUpdateStep = NrCharDefine.CharUpdateStep.CHARUPDATESTEP_VERYFAR;
				this.m_nCharSkipFrame = 5;
			}
			else if (num > 30f)
			{
				this.m_eCharUpdateStep = NrCharDefine.CharUpdateStep.CHARUPDATESTEP_FAR;
				this.m_nCharSkipFrame = 3;
			}
			else if (num > 20f)
			{
				this.m_eCharUpdateStep = NrCharDefine.CharUpdateStep.CHARUPDATESTEP_AROUND;
				this.m_nCharSkipFrame = 2;
			}
			else if (num > 10f)
			{
				this.m_eCharUpdateStep = NrCharDefine.CharUpdateStep.CHARUPDATESTEP_NEAR;
				this.m_nCharSkipFrame = 1;
			}
			else
			{
				this.m_eCharUpdateStep = NrCharDefine.CharUpdateStep.CHARUPDATESTEP_NEAR;
				this.m_nCharSkipFrame = 0;
			}
			if (this.m_k3DChar != null && this.m_k3DChar.IsCreated())
			{
				this.m_k3DChar.Set3DCharFrameInfo(this.m_eCharUpdateStep, this.m_nCharSkipFrame);
			}
		}
		if (this.GetID() == 1 && this.GetCharKindType() == eCharKindType.CKT_USER)
		{
			if (Scene.CurScene == Scene.Type.WORLD)
			{
				float num2 = 12f;
				NrCharBase nrCharBase = null;
				if (!this.m_kCharMove.IsMoving() && !NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
				{
					NrCharBase[] @char = NrTSingleton<NkCharManager>.Instance.Get_Char();
					for (int i = 0; i < @char.Length; i++)
					{
						NrCharBase nrCharBase2 = @char[i];
						if (nrCharBase2 != null)
						{
							if (!(null == nrCharBase2.GetCharObject()))
							{
								if (nrCharBase2.GetCharObject().activeInHierarchy)
								{
									if (!(this.GetCharObject() == null))
									{
										float num3 = Vector3.Distance(this.GetCharObject().transform.position, nrCharBase2.GetCharObject().transform.position);
										if (num2 > num3)
										{
											if (nrCharBase2.GetCharKindInfo().IsATB(16777216L))
											{
												if (!nrCharBase2.m_bSubChar)
												{
													if (nrCharBase2.GetCharKindInfo().IsATB(16L))
													{
														if (!nrCharBase2.GetCharKindInfo().IsATB(8589934592L) && !NrTSingleton<NkQuestManager>.Instance.IsQuestMonster(nrCharBase2.GetCharKindInfo().GetCharKind()))
														{
															goto IL_363;
														}
														num2 = num3;
														nrCharBase = nrCharBase2;
													}
													else if (nrCharBase2.GetCharKindInfo().IsATB(4L) && !nrCharBase2.IsCharStateAtb(16384L))
													{
														if (!NrTSingleton<NkQuestManager>.Instance.IsQuestMonster(nrCharBase2.GetCharKindInfo().GetCharKind()))
														{
															goto IL_363;
														}
														num2 = num3;
														nrCharBase = nrCharBase2;
													}
													else if (nrCharBase2.GetCharKindInfo().IsATB(8L))
													{
														if (nrCharBase2.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
														{
															nrCharBase = nrCharBase2;
															break;
														}
														if (nrCharBase2.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
														{
															num2 = num3;
															nrCharBase = nrCharBase2;
														}
														else if (nrCharBase2.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING)
														{
															num2 = num3;
															nrCharBase = nrCharBase2;
														}
														else
														{
															if (nrCharBase2.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_DAYQUEST_COMPLETE)
															{
																nrCharBase = nrCharBase2;
																break;
															}
															if (nrCharBase2.m_eQuestState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_DAYQUEST_ACCEPTABLE)
															{
																num2 = num3;
																nrCharBase = nrCharBase2;
															}
															else if (NrTSingleton<NkQuestManager>.Instance.IsQuestMonster(nrCharBase2.GetCharKindInfo().GetCharKind()))
															{
																num2 = num3;
																nrCharBase = nrCharBase2;
															}
														}
													}
													if (this.IsNearNPCMenuOpen(nrCharBase2))
													{
														nrCharBase = nrCharBase2;
														break;
													}
												}
											}
										}
									}
								}
							}
						}
						IL_363:;
					}
				}
				else
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEARNPCSELECTUI_DLG);
				}
				if (nrCharBase != null)
				{
					if (!NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
					{
						NearNpcSelectUI_DLG nearNpcSelectUI_DLG;
						if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEARNPCSELECTUI_DLG))
						{
							nearNpcSelectUI_DLG = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEARNPCSELECTUI_DLG) as NearNpcSelectUI_DLG);
						}
						else
						{
							nearNpcSelectUI_DLG = (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEARNPCSELECTUI_DLG) as NearNpcSelectUI_DLG);
						}
						if (nearNpcSelectUI_DLG != null)
						{
							nearNpcSelectUI_DLG.SetCharKind(nrCharBase.GetCharUnique(), nrCharBase.GetCharKindInfo(), nrCharBase.m_eQuestState);
							nearNpcSelectUI_DLG.Show();
						}
					}
				}
				else
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEARNPCSELECTUI_DLG);
				}
				this.ShowAutoMoveUI();
			}
			else if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEARNPCSELECTUI_DLG) != null)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEARNPCSELECTUI_DLG);
			}
		}
	}

	private void ShowAutoMoveUI()
	{
		if (this.GetID() != 1 && this.GetCharKindType() != eCharKindType.CKT_USER && Scene.CurScene != Scene.Type.WORLD)
		{
			return;
		}
		if (this.m_kCharMove.IsAutoMove() && this.m_kCharMove.IsMoving())
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MAIN_UI_AUTO_MOVE))
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_UI_AUTO_MOVE);
			}
		}
		else if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MAIN_UI_AUTO_MOVE))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MAIN_UI_AUTO_MOVE);
		}
	}

	protected virtual void OnCreateAction()
	{
		this.m_k3DChar.SetEvent3DModelCreated(new Nr3DCharBase.func3DModelCreated(NrTSingleton<Nr3DCharSystem>.Instance.OnEvent3DModelCreated));
		this.m_bCreateAction = true;
	}

	private void SetFirstAction()
	{
		if (this.m_bSetFirstAction)
		{
			return;
		}
		if (this.m_k3DChar == null)
		{
			return;
		}
		if (NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
		{
			Vector3 charPos = this.m_kPersonInfo.GetCharPos();
			Vector3 direction = this.m_kPersonInfo.GetDirection();
			GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
			if (rootGameObject == null)
			{
				return;
			}
			this.SetSafeCharPos(charPos);
			if (this.GetID() == 1)
			{
				GameObject baseObject = this.m_k3DChar.GetBaseObject();
				if (!(baseObject != null) || Camera.main != null)
				{
				}
			}
			if (this.IsCharStateAtb(24L))
			{
				this.m_k3DChar.SetLookAt(direction.x, direction.y, direction.z, false);
			}
			else if (direction.y == 0f)
			{
				if (!direction.Equals(Vector3.zero))
				{
					rootGameObject.transform.localRotation = Quaternion.LookRotation(direction, Vector3.up);
				}
			}
			else
			{
				rootGameObject.transform.localRotation = Quaternion.AngleAxis(direction.y, Vector3.up);
			}
			this.m_kCharMove.SetCharPos(rootGameObject);
			this.m_k3DChar.SetCurrentAniType("Stay");
		}
		this.SetAnimation(this.LoadAfterAnimation, true, false);
		this.m_bSetFirstAction = true;
	}

	private void MakeFadeIn()
	{
		if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState() && (int)this.m_kIDInfo.m_nCharUnique != NrTSingleton<NkClientLogic>.Instance.GetNpcTalkCharUnique() && !this.m_bExceptHideForLoad)
		{
			return;
		}
		GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
		if (rootGameObject == null)
		{
			return;
		}
		this.m_bShowChar = false;
		if (this.m_fCurrentAlphaFadeLerp <= 0f)
		{
			Renderer[] componentsInChildren = rootGameObject.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Renderer renderer = componentsInChildren[i];
				if (renderer.gameObject.name.Contains("fx_"))
				{
					renderer.renderer.enabled = false;
				}
				else
				{
					renderer.renderer.enabled = true;
				}
			}
			this.m_fCurrentAlphaFadeLerp = 0.01f;
			this.m_k3DChar.SetEvent3DModelPartItemChanged(new Nr3DCharBase.func3DModelCreated(NrTSingleton<Nr3DCharSystem>.Instance.OnEvent3DModelPartItemChanged));
			this.Set3DCharStep(NrCharBase.e3DCharStep.FADEIN);
			this.OneTimeAction();
		}
		else if (this.m_fCurrentAlphaFadeLerp < 1f)
		{
			Renderer[] componentsInChildren2 = rootGameObject.GetComponentsInChildren<Renderer>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				Renderer renderer2 = componentsInChildren2[j];
				if (!(renderer2.particleEmitter != null) || !(renderer2.particleSystem != null))
				{
					Material[] materials = renderer2.materials;
					for (int k = 0; k < materials.Length; k++)
					{
						Material material = materials[k];
						if (material.HasProperty("_Color") && material.HasProperty("_Alpha"))
						{
							this.m_kCharColor = material.GetColor("_Color");
							this.m_kCharColor.a = this.m_fCurrentAlphaFadeLerp;
							material.SetColor("_Color", this.m_kCharColor);
							material.SetFloat("_Alpha", this.m_fCurrentAlphaFadeLerp);
						}
					}
				}
			}
			this.m_fCurrentAlphaFadeLerp = Mathf.Lerp(this.m_fCurrentAlphaFadeLerp, 1.05f, 5f * this.m_fCharDeltaTime);
		}
		else
		{
			string text = string.Empty;
			Renderer[] componentsInChildren3 = rootGameObject.GetComponentsInChildren<Renderer>(true);
			for (int l = 0; l < componentsInChildren3.Length; l++)
			{
				Renderer renderer3 = componentsInChildren3[l];
				if (!(renderer3.particleEmitter != null) || !(renderer3.particleSystem != null))
				{
					Material[] materials2 = renderer3.materials;
					for (int m = 0; m < materials2.Length; m++)
					{
						Material material2 = materials2[m];
						if (material2.HasProperty("_Color"))
						{
							this.m_kCharColor = material2.color;
							this.m_kCharColor.a = 1f;
							text = NrTSingleton<Nr3DCharSystem>.Instance.PopAlphaShaderRecovery(material2.name);
							material2.shader = Shader.Find(text);
							if (material2.shader == null)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"SkinnedMeshRenderer Shader not found ===> name : [",
									material2.name,
									"] (",
									text,
									")"
								}));
							}
							material2.color = this.m_kCharColor;
						}
					}
				}
			}
			this.OnRecoveryShader();
			this.Set3DCharStep(NrCharBase.e3DCharStep.CHARACTION);
			this.m_bShowChar = true;
			this.m_bLoadCompleted = true;
			this.m_k3DChar.SetStartSkipFrame(true);
		}
	}

	protected void OnRecoveryShader()
	{
		this.MakeCharName(false, this.GetUserColosseumGrade());
		this.MakeCharGuildNameShow(this.GetUserGuildName(), this.GetUserGuildID());
		this.m_k3DChar.OnRecoveryEnchantWeapon();
		if (NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
		{
			bool fakeShadowEnable = TsQualityManager.Instance.CurrLevel <= TsQualityManager.Level.LOW;
			if (TsPlatform.IsMobile)
			{
				fakeShadowEnable = true;
			}
			this.SetFakeShadowEnable(fakeShadowEnable);
		}
	}

	public void Loaded3DChar()
	{
		if (this.m_bIsManagedGeneration)
		{
			NrTSingleton<NkCharManager>.Instance.DecreaseGenerationCount();
			this.m_bIsManagedGeneration = false;
		}
		this.OnLoaded3DChar();
	}

	public virtual bool OnLoaded3DChar()
	{
		if (this.m_pkCharKindInfo == null)
		{
			return false;
		}
		if (this.m_k3DChar == null)
		{
			return false;
		}
		GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
		if (rootGameObject == null)
		{
			return false;
		}
		int scale = (int)this.m_pkCharKindInfo.GetScale();
		if (Scene.IsCurScene(Scene.Type.SELECTCHAR))
		{
			this.ProcessMouseEvent = false;
		}
		else
		{
			float num = 1f;
			if (this.m_pkCharKindInfo.IsATB(1L))
			{
				NrCharKindInfo faceCharKindInfo = this.GetFaceCharKindInfo();
				if (faceCharKindInfo != null)
				{
					num = 1f * (float)faceCharKindInfo.GetScale() / 10f;
				}
				else
				{
					num = 1f * (float)scale / 10f;
				}
			}
			else if (this.m_pkCharKindInfo.IsATB(16L))
			{
				num = 1f * (float)scale / 10f;
			}
			else if (!this.m_pkCharKindInfo.IsATB(16L))
			{
				num = 1f * (float)scale / 10f;
			}
			rootGameObject.transform.localScale = new Vector3(num, num, num);
		}
		this.m_kAnimation.Init(this.m_pkCharKindInfo.GetCode(), this.m_k3DChar, this.m_eCharKindType);
		this.m_kCharMove.Init();
		this.m_kCharMove.SetSpeed(this.m_kPersonInfo.GetMoveSpeed());
		if (NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
		{
			GameObject rootGameObject2 = this.m_k3DChar.GetRootGameObject();
			if (rootGameObject2 != null)
			{
				this.m_k3DChar.SetOnGround(true);
				NrCharInfoAdaptor nrCharInfoAdaptor = NkUtil.GuarranteeComponent<NrCharInfoAdaptor>(rootGameObject2);
				nrCharInfoAdaptor.SetCharInfoInterface(new NrCharInfoLogic(this));
				nrCharInfoAdaptor.charunique = (int)this.GetCharUnique();
				nrCharInfoAdaptor.charkind = this.GetCharKind();
				nrCharInfoAdaptor.makepos = this.m_kPersonInfo.GetCharPos();
				if (this.IsCharStateAtb(4L))
				{
					this.m_k3DChar.MoveStop(false);
					this.SetAnimationLoadAfter(this.m_k3DChar.GetIdleAnimation());
				}
			}
			if (this.IsSubChar)
			{
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser != null)
				{
					this.MakeChatText(nrCharUser.GetSubCharStartChatText((int)this.GetCharUnique()), true);
				}
			}
		}
		if (this.m_pkCharKindInfo.IsSlopeMode())
		{
			this.m_k3DChar.SetSlope(true);
		}
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			if (this.GetID() == 1)
			{
				this.m_k3DChar.SetLayer(TsLayer.PC);
			}
			else
			{
				this.m_k3DChar.SetLayer(TsLayer.PC_OTHER);
			}
			break;
		case eCharKindType.CKT_SOLDIER:
			this.m_k3DChar.SetLayer(TsLayer.NPC, TsTag.NPC_MOB.ToString());
			break;
		case eCharKindType.CKT_MONSTER:
			this.m_k3DChar.SetLayer(TsLayer.NPC, TsTag.NPC_MOB.ToString());
			break;
		case eCharKindType.CKT_NPC:
			this.m_k3DChar.SetLayer(TsLayer.NPC, TsTag.NPC_QUEST.ToString());
			break;
		case eCharKindType.CKT_OBJECT:
			this.m_k3DChar.SetLayer(TsLayer.NPC, TsTag.NPC_EXTRA.ToString());
			break;
		default:
			this.m_k3DChar.SetLayer(TsLayer.NPC, TsTag.NPC_EXTRA.ToString());
			break;
		}
		this.Set3DCharStep(NrCharBase.e3DCharStep.LOADCOMPLETED);
		this.ReadyHeadUp(scale);
		this.m_bCharActive = NrTSingleton<NkCharManager>.Instance.IsCharActive();
		if (NrTSingleton<NkClientLogic>.Instance.IsGameWorld() && !NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
		{
			this.SetChildActive(false);
		}
		this.SetAnimationLoadAfter(this.m_k3DChar.GetIdleAnimation());
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			Debug.Log(string.Concat(new object[]
			{
				"AfterLoad3dChar ID : ",
				this.GetID(),
				", CharUnique : ",
				this.GetCharUnique()
			}));
		}
		return true;
	}

	protected virtual void OneTimeAction()
	{
		if (NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
		{
			this.m_kAnimation.SetBattleState();
			if (!NrTSingleton<NkCharManager>.Instance.IsCharMoveEnable())
			{
				this.m_vReservedMoveTarget = new Vector3(0f, 0f, 0f);
			}
			this.m_kCharMove.SetCharPos(this.m_k3DChar.GetRootGameObject());
			if (this.m_vReservedMoveTarget.x > 0f || this.m_vReservedMoveTarget.z > 0f)
			{
				this.MoveTo(this.m_vReservedMoveTarget.x, this.m_vReservedMoveTarget.y, this.m_vReservedMoveTarget.z, false);
			}
			this.ChangeWeaponTarget();
		}
	}

	protected virtual void SetCharAction()
	{
		if (!this.m_bSetFirstAction)
		{
			return;
		}
		if (NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
		{
			if (this.m_kCharMove.SetCurrentPosInfo())
			{
				this.m_kCharMove.ProcessCharMove(false);
				if (this.GetID() == 1)
				{
					Vector3 position = this.Get3DChar().GetRootGameObject().transform.position;
					NrTSingleton<EventConditionHandler>.Instance.RangeMove.Value.Set(position.x, position.z);
					NrTSingleton<EventConditionHandler>.Instance.RangeMove.OnTrigger();
				}
			}
			this.SetAnimationFromState();
			this.SetCharEffectFromState(true);
			this.Update_MouseEvent();
		}
		else
		{
			this.SetCharEffectFromState(false);
		}
		this.m_kAnimation.ProcessAnimation(this.m_fCharDeltaTime);
	}

	public bool SetAnimationFromState()
	{
		if (this.IsCharStateAtb(16384L))
		{
			if (this.m_kCharMove != null && this.m_kCharMove.IsMoving())
			{
				this.m_kCharMove.MoveStop(false, false);
			}
			if (this.m_nNextSpecialAni <= 0f)
			{
				this.SetAnimation(eCharAnimationType.Attack1);
				this.m_nNextSpecialAni = 3f;
			}
			this.m_nNextSpecialAni -= this.m_fCharDeltaTime;
		}
		else if (this.IsCharStateAtb(32L))
		{
			if (!this.IsCharStateAtb_Old(32L) && this.m_kCharMove != null)
			{
				this.m_kCharMove.MoveStop(false, false);
			}
			this.SetAnimation(eCharAnimationType.Die1);
			this.m_fDeadTime = Time.time;
			this.Set3DCharStep(NrCharBase.e3DCharStep.DIED);
		}
		else if (this.IsCharStateAtb(2L))
		{
			bool flag = false;
			if (this.GetCharKindType() == eCharKindType.CKT_USER && this.GetFaceCharKind() == 0)
			{
				if (this.IsCharStateAtb(4L))
				{
					if (!NrCharAnimation.IsSitDownAnimation(this.m_kAnimation.GetCurrentAniType()))
					{
						this.SetAnimation(eCharAnimationType.SitS1);
						flag = true;
					}
				}
				else if (this.IsCharStateAtb(512L))
				{
					if (!NrCharAnimation.IsCollectAnimation(this.m_kAnimation.GetCurrentAniType()))
					{
						this.SetAnimation(eCharAnimationType.CollectS1);
						flag = true;
					}
				}
				else if (this.IsCharStateAtb_Old(4L))
				{
					this.SetAnimation(eCharAnimationType.SitE1, true, true);
					flag = true;
				}
				else if (this.IsCharStateAtb_Old(512L))
				{
					this.SetAnimation(eCharAnimationType.CollectE1, true, true);
					flag = true;
				}
			}
			if (!flag)
			{
				if (!this.m_bIndunTriggerAnimation && this.IsChangedCharState())
				{
					this.SetAnimation(this.LoadAfterAnimation);
				}
				if (this.m_bIndunTriggerAnimation)
				{
					if ((this.m_fIndunAniEndTime == 0f || this.m_fIndunAniEndTime < Time.time) && this.m_nIndunAniCount > 0)
					{
						float num = this.SetAnimation(this.m_eIndunTriggerAnimation);
						this.m_fIndunAniEndTime = Time.time + num;
						this.m_nIndunAniCount--;
					}
					if (this.m_nIndunAniCount == 0 && this.m_fIndunAniEndTime < Time.time)
					{
						if (this.m_eIndunTriggerNextAnimation != eCharAnimationType.None)
						{
							float num2 = this.SetAnimation(this.m_eIndunTriggerNextAnimation);
							this.m_fIndunAniEndTime = Time.time + num2;
						}
						else
						{
							this.m_fIndunAniEndTime = 0f;
						}
						this.m_nIndunAniCount--;
					}
					else if (this.m_nIndunAniCount < 0 && this.m_fIndunAniEndTime < Time.time)
					{
						this.SetIndunTriggerAnimation(false, eCharAnimationType.None, 0, eCharAnimationType.None);
					}
				}
				else if ((this.m_nIndunATB & 1) != 0)
				{
					this.SetAnimation(eCharAnimationType.ActionLoop1);
				}
				else if (NrTSingleton<NkClientLogic>.Instance.IsRemainFrame(30) && !this.m_kCharMove.IsMoving() && !NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState() && !NrCharAnimation.IsIdleAnimation(this.m_kAnimation.GetCurrentAniType()))
				{
					this.SetAnimation(this.m_k3DChar.GetIdleAnimation());
				}
			}
		}
		else if (this.IsCharStateAtb(128L))
		{
			if (this.m_nNextSpecialAni <= 0f)
			{
				this.SetAnimation(eCharAnimationType.Stay1);
				this.m_nNextSpecialAni = 5f;
			}
			this.m_nNextSpecialAni -= this.m_fCharDeltaTime;
		}
		else if (this.IsCharStateAtb(8L))
		{
			if (this.m_k3DChar == null)
			{
				return false;
			}
			if (!this.IsCharStateAtb_Old(8L))
			{
				this.SetAnimation(this.m_k3DChar.GetMoveAnimation());
			}
			if (NrTSingleton<NkClientLogic>.Instance.IsRemainFrame(30) && this.m_kCharMove.IsMoving() && this.m_k3DChar.GetMoveAnimation() != this.m_kAnimation.GetCurrentAniType())
			{
				this.SetAnimation(this.m_k3DChar.GetMoveAnimation());
			}
		}
		if (!this.m_bIndunTriggerAnimation && this.IsChangedCharState())
		{
			this.SetCharState_ALL(this.m_CharState);
		}
		return true;
	}

	public bool IsNearNPCMenuOpen(NrCharBase kChar)
	{
		return kChar.GetCharKindInfo().IsATB(274877906944L) || kChar.GetCharKindInfo().IsATB(549755813888L) || kChar.GetCharKindInfo().IsATB(1099511627776L) || kChar.GetCharKindInfo().IsATB(2199023255552L) || kChar.GetCharKindInfo().IsATB(512L) || kChar.GetCharKindInfo().IsATB(562949953421312L) || kChar.GetCharKindInfo().IsATB(1125899906842624L) || kChar.GetCharKindInfo().IsATB(4503599627370496L) || kChar.GetCharKindInfo().IsATB(18014398509481984L) || kChar.GetCharKindInfo().IsATB(36028797018963968L);
	}
}
