using GAME;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NrCharAnimation
{
	private string m_szCharCode = string.Empty;

	private Nr3DCharBase m_pkParent3DChar;

	private eCharAnimationType m_eCurrentAniType;

	private NrCharDefine.eCharFaicalAnimationType m_eCurrentFacialAniType;

	private eCharAnimationType m_ePrevAniType;

	private bool m_bLoop;

	private List<NrCharNextAniInfo> m_eNextAniTypeList;

	private bool m_bProcessStatus;

	private int m_nSetAniCount;

	private bool m_bFinishAnimationFrame;

	private bool m_bBattleState;

	private AnimationState m_CurrentAniState;

	private float m_fAniPlayingTime;

	private float m_fAnimationLength;

	public NrCharAnimation()
	{
		this.m_eNextAniTypeList = new List<NrCharNextAniInfo>();
		this.Init(string.Empty, null, eCharKindType.CKT_USER);
	}

	public void Init(string charcode, Nr3DCharBase k3DChar, eCharKindType chartype)
	{
		this.m_szCharCode = charcode;
		switch (chartype)
		{
		case eCharKindType.CKT_USER:
			this.m_pkParent3DChar = (k3DChar as Nr3DCharActor);
			break;
		case eCharKindType.CKT_SOLDIER:
		case eCharKindType.CKT_MONSTER:
		case eCharKindType.CKT_NPC:
			this.m_pkParent3DChar = (k3DChar as Nr3DCharNonePart);
			break;
		case eCharKindType.CKT_OBJECT:
			this.m_pkParent3DChar = (k3DChar as Nr3DCharObject);
			break;
		default:
			this.m_pkParent3DChar = k3DChar;
			break;
		}
		this.m_eCurrentAniType = eCharAnimationType.Stay1;
		this.m_ePrevAniType = eCharAnimationType.Stay1;
		this.m_bLoop = false;
		this.m_eNextAniTypeList.Clear();
		this.m_bProcessStatus = true;
		this.m_nSetAniCount = 0;
		this.m_bFinishAnimationFrame = false;
		this.m_bBattleState = false;
		this.m_CurrentAniState = null;
		this.m_fAniPlayingTime = 0f;
		this.m_fAnimationLength = 0f;
		this.AttachFacialAnimation();
	}

	public void SetBattleState()
	{
		this.m_bBattleState = NrTSingleton<NkClientLogic>.Instance.IsBattleScene();
	}

	public void SetCurrentAniType(eCharAnimationType sourceanitype, float fBlendTime)
	{
		if (!NrCharAnimation.IsForcePlayAnimation(sourceanitype) && this.m_eCurrentAniType == sourceanitype && this.IsSameAniPlaying())
		{
			return;
		}
		AnimationClip animationClip = this.m_pkParent3DChar.SetAnimation(sourceanitype, fBlendTime, this.IsLoopAnimation(sourceanitype));
		if (animationClip == null)
		{
			if (this.m_pkParent3DChar is Nr3DCharActor)
			{
				string text = "[" + sourceanitype.ToString() + "] Character Animation Setting failed. Contact me(SSH).";
				NrTSingleton<NrWebAlert>.Instance.PushMsg(text);
				Debug.Log(text);
			}
			return;
		}
		this.m_bLoop = (animationClip.wrapMode == WrapMode.Loop);
		string value = this.m_pkParent3DChar.MakeAnimationKey(sourceanitype);
		if (!animationClip.name.Equals(value))
		{
			sourceanitype = this.FindAniTypeByCode(this.m_pkParent3DChar.GetCurrentAniType());
		}
		this.m_ePrevAniType = this.m_eCurrentAniType;
		this.m_eCurrentAniType = sourceanitype;
		this._SetCurrentAniState(sourceanitype);
		this.m_bProcessStatus = false;
		this.SetFinishAnimation(false);
		this.m_fAniPlayingTime = 0f;
		this.m_fAnimationLength = animationClip.length;
		if (!this.m_pkParent3DChar.IsBattleChar())
		{
			NkCharPartControl parentCharPartControl = this.m_pkParent3DChar.GetParentCharPartControl();
			if (parentCharPartControl != null)
			{
				eCharAnimationType prevAniType = this.GetPrevAniType();
				bool flag = NrCharAnimation.IsBattleAnimation(prevAniType);
				bool flag2 = NrCharAnimation.IsBattleAnimation(sourceanitype);
				if (flag != flag2)
				{
					parentCharPartControl.ChangeWeaponTarget();
				}
			}
		}
	}

	public eCharAnimationType GetCurrentAniType()
	{
		return this.m_eCurrentAniType;
	}

	private string GetCurrentAniKey()
	{
		string text = this.m_pkParent3DChar.MakeAnimationKey(this.m_eCurrentAniType);
		return text.ToLower();
	}

	public eCharAnimationType GetPrevAniType()
	{
		return this.m_ePrevAniType;
	}

	public int GetNextAniCount()
	{
		return this.m_eNextAniTypeList.Count;
	}

	public void ClearNextAni()
	{
		this.m_eNextAniTypeList.Clear();
	}

	private void _SetCurrentAniState(eCharAnimationType _anitype)
	{
		if (this.m_pkParent3DChar == null)
		{
			return;
		}
		GameObject baseObject = this.m_pkParent3DChar.GetBaseObject();
		if (baseObject == null)
		{
			return;
		}
		string name = this.m_pkParent3DChar.MakeAnimationKey(_anitype);
		this.m_CurrentAniState = baseObject.animation[name];
		if (this.m_bBattleState)
		{
			if (this.m_eCurrentAniType == eCharAnimationType.BRun1)
			{
				this.m_CurrentAniState.speed = 1.5f;
			}
			else
			{
				this.m_CurrentAniState.speed = 1f;
			}
		}
	}

	public void SetSlowMotion()
	{
	}

	public void RestoreSlowMotion()
	{
	}

	public bool IsValidAniState(eCharAnimationType _anitype)
	{
		Nr3DCharBase pkParent3DChar = this.m_pkParent3DChar;
		if (pkParent3DChar == null)
		{
			return false;
		}
		GameObject baseObject = pkParent3DChar.GetBaseObject();
		if (baseObject == null)
		{
			return false;
		}
		string name = this.m_pkParent3DChar.MakeAnimationKey(_anitype);
		return baseObject.animation.GetClip(name) != null;
	}

	public float PlayTimeAniState(eCharAnimationType _anitype)
	{
		Nr3DCharBase pkParent3DChar = this.m_pkParent3DChar;
		if (pkParent3DChar == null)
		{
			return 0f;
		}
		GameObject baseObject = pkParent3DChar.GetBaseObject();
		if (baseObject == null)
		{
			return 0f;
		}
		string name = this.m_pkParent3DChar.MakeAnimationKey(_anitype);
		if (this.IsValidAniState(_anitype))
		{
			return baseObject.animation[name].length;
		}
		return 0f;
	}

	public float PushNextAniType(eCharAnimationType anitype, bool bForceAction, bool bForceReserved, bool bBlend)
	{
		if (!bForceAction && this.m_eCurrentAniType == anitype && this.m_bLoop)
		{
			return this.m_fAnimationLength;
		}
		this.m_nSetAniCount = 0;
		bool flag = bForceReserved || (!bForceAction && this.GetNextAniCount() > 0);
		float crossFade = this.GetCrossFade(anitype, bBlend);
		if (flag)
		{
			this.m_eNextAniTypeList.Add(new NrCharNextAniInfo(anitype, bBlend));
			this.SetNextAnimation(anitype);
			this.m_bProcessStatus = true;
			return 0f;
		}
		if (NrCharAnimation.IsBattleAnimation(anitype))
		{
			eCharAnimationType randomAniType = this.GetRandomAniType(anitype);
			if (this.m_pkParent3DChar != null && this.m_pkParent3DChar.IsHaveAnimation(randomAniType))
			{
				anitype = randomAniType;
			}
		}
		if (!bForceReserved && this.m_pkParent3DChar is Nr3DCharNonePart && !this.m_pkParent3DChar.IsHaveAnimation(anitype))
		{
			anitype = this.GetSafeAniType(anitype);
		}
		this.m_eNextAniTypeList.Clear();
		this.SetCurrentAniType(anitype, crossFade);
		if (this.m_pkParent3DChar.IsBattleChar())
		{
			if (NrCharAnimation.IsIdleAnimation(this.GetNextAniType(anitype)) && !this.m_pkParent3DChar.GetParentBattleChar().IsLastAttacker)
			{
				eCharAnimationType stayAni = this.m_pkParent3DChar.GetParentBattleChar().GetStayAni();
				this.m_eNextAniTypeList.Add(new NrCharNextAniInfo(stayAni, true));
				this.SetNextAnimation(stayAni);
			}
		}
		else
		{
			this.SetNextAnimation(anitype);
		}
		return this.m_fAnimationLength;
	}

	public void SetNextAnimation(eCharAnimationType anitype)
	{
		eCharAnimationType nextAniType = this.GetNextAniType(anitype);
		if (nextAniType != eCharAnimationType.END_ANITYPE)
		{
			this.m_nSetAniCount++;
			this.m_eNextAniTypeList.Add(new NrCharNextAniInfo(nextAniType, true));
			this.SetNextAnimation(nextAniType);
		}
	}

	public NrCharNextAniInfo PopNextAniType()
	{
		if (this.m_eNextAniTypeList.Count == 0)
		{
			return null;
		}
		NrCharNextAniInfo nrCharNextAniInfo = this.m_eNextAniTypeList[0];
		if (nrCharNextAniInfo == null)
		{
			return null;
		}
		this.m_eNextAniTypeList.Remove(nrCharNextAniInfo);
		return nrCharNextAniInfo;
	}

	public string GetFacialAniKey(NrCharDefine.eCharFaicalAnimationType anitype)
	{
		string text = this.m_szCharCode + anitype.ToString();
		return text.ToLower();
	}

	public void AttachFacialAnimation()
	{
		if (this.m_pkParent3DChar == null)
		{
			return;
		}
		GameObject faceObject = this.m_pkParent3DChar.GetFaceObject();
		if (faceObject == null)
		{
			return;
		}
		if (faceObject.animation == null)
		{
			faceObject.AddComponent<Animation>();
		}
		if (faceObject.animation != null)
		{
			faceObject.animation.playAutomatically = false;
			faceObject.animation.Stop();
		}
	}

	public void SetFacialAnimation(NrCharDefine.eCharFaicalAnimationType anitype)
	{
		string facialAniKey = this.GetFacialAniKey(anitype);
		WrapMode facialWrapMode = NrCharDefine.GetFacialWrapMode(anitype);
		this.m_pkParent3DChar.SetFacialAnimation(facialAniKey, facialWrapMode);
		this.m_eCurrentFacialAniType = anitype;
	}

	public void StopAnimation()
	{
		this.SetFacialAnimation(NrCharDefine.eCharFaicalAnimationType.FStay1);
		this.SetCurrentAniType(this.m_pkParent3DChar.GetIdleAnimation(), 0f);
	}

	public bool IsPlaying()
	{
		return this.m_pkParent3DChar.IsAniPlay();
	}

	public bool IsSameAniPlaying()
	{
		return this.m_pkParent3DChar.GetCurrentAniType().Equals(this.m_eCurrentAniType.ToString());
	}

	private bool IsCrossFadeProcess()
	{
		if (this.m_fAniPlayingTime > this.m_fAnimationLength)
		{
			return false;
		}
		float crossFade = this.GetCrossFade();
		float num = this.m_fAnimationLength - crossFade;
		return this.m_fAniPlayingTime > num;
	}

	private float GetCrossFade()
	{
		if (this.m_eNextAniTypeList.Count != 0)
		{
			NrCharNextAniInfo nrCharNextAniInfo = this.m_eNextAniTypeList[0];
			return this.GetCrossFade(nrCharNextAniInfo.eAnimationType, nrCharNextAniInfo.bBlend);
		}
		return 0f;
	}

	private float GetCrossFade(eCharAnimationType eNextType, bool bBlend)
	{
		if (this.m_pkParent3DChar != null && bBlend)
		{
			string text = this.m_pkParent3DChar.MakeAnimationKey(this.m_eCurrentAniType);
			string text2 = this.m_pkParent3DChar.MakeAnimationKey(eNextType);
			return NrTSingleton<Nr3DCharSystem>.Instance.GetBlend(this.m_szCharCode, text.ToLower(), text2.ToLower());
		}
		if (null != this.m_CurrentAniState && this.m_CurrentAniState.wrapMode == WrapMode.Loop)
		{
			return 0f;
		}
		return 0f;
	}

	public bool CheckProcessStatus()
	{
		if (this.IsCrossFadeProcess())
		{
			return true;
		}
		if (!this.m_bLoop)
		{
			if (this.IsSameAniPlaying())
			{
				if (!this.IsPlaying())
				{
					this.m_bProcessStatus = true;
				}
				else if (this.m_eCurrentFacialAniType != NrCharDefine.eCharFaicalAnimationType.FStay1 && !this.IsTalkAnimationPlaying())
				{
					this.SetFacialAnimation(NrCharDefine.eCharFaicalAnimationType.FStay1);
				}
			}
		}
		else if (this.m_fAniPlayingTime > this.m_fAnimationLength)
		{
			eCharAnimationType randomAniType = this.GetRandomAniType(this.m_eCurrentAniType);
			this.SetCurrentAniType(randomAniType, 0.3f);
			this.m_fAniPlayingTime = 0f;
		}
		return this.m_bProcessStatus;
	}

	public bool IsTalkAnimationPlaying()
	{
		if (this.m_pkParent3DChar == null)
		{
			return false;
		}
		GameObject baseObject = this.m_pkParent3DChar.GetBaseObject();
		return !(baseObject == null) && baseObject.animation.IsPlaying(this.GetCurrentAniKey());
	}

	public void ProcessAnimation(float deltatime)
	{
		this.m_fAniPlayingTime += deltatime;
		if (!this.m_bProcessStatus && !this.CheckProcessStatus())
		{
			return;
		}
		NrCharNextAniInfo nrCharNextAniInfo = this.PopNextAniType();
		if (nrCharNextAniInfo != null)
		{
			float crossFade = this.GetCrossFade(nrCharNextAniInfo.eAnimationType, nrCharNextAniInfo.bBlend);
			this.SetCurrentAniType(nrCharNextAniInfo.eAnimationType, crossFade);
		}
		else if (this.GetNextAniCount() == 0 && !this.m_bBattleState && !NrCharAnimation.IsIdleAnimation(this.m_eCurrentAniType))
		{
			NrCharBase parentChar = this.m_pkParent3DChar.GetParentChar();
			if (parentChar != null && parentChar.m_kCharMove.IsMoving())
			{
				return;
			}
			this.SetCurrentAniType(this.m_pkParent3DChar.GetIdleAnimation(), 0.3f);
		}
	}

	public eCharAnimationType FindAniTypeByCode(string anitype)
	{
		eCharAnimationType eCharAnimationType;
		for (eCharAnimationType = eCharAnimationType.Stay1; eCharAnimationType < eCharAnimationType.END_ANITYPE; eCharAnimationType++)
		{
			if (anitype.Equals(eCharAnimationType.ToString()))
			{
				break;
			}
		}
		return eCharAnimationType;
	}

	public eCharAnimationType GetSafeAniType(eCharAnimationType sourceanitype)
	{
		eCharAnimationType result = eCharAnimationType.Stay1;
		switch (sourceanitype)
		{
		case eCharAnimationType.Attack2:
		case eCharAnimationType.Attack3:
		case eCharAnimationType.AttackLeft1:
		case eCharAnimationType.AttackRight1:
			result = eCharAnimationType.Attack1;
			return result;
		case eCharAnimationType.BStay2:
			result = eCharAnimationType.BStay1;
			return result;
		case eCharAnimationType.Damage1:
			result = eCharAnimationType.BStay1;
			return result;
		case eCharAnimationType.CriDamage1:
			result = eCharAnimationType.Damage1;
			return result;
		case eCharAnimationType.Evade1:
			result = eCharAnimationType.BStay1;
			return result;
		case eCharAnimationType.Tired1:
			result = eCharAnimationType.BStay1;
			return result;
		case eCharAnimationType.EcoAction2:
			result = eCharAnimationType.EcoAction1;
			return result;
		case eCharAnimationType.TalkStart1:
			result = eCharAnimationType.TalkAction1;
			return result;
		case eCharAnimationType.TalkAction1:
			result = eCharAnimationType.TalkStay1;
			return result;
		case eCharAnimationType.TalkEnd1:
			result = eCharAnimationType.EcoAction1;
			return result;
		case eCharAnimationType.Respawn1:
		case eCharAnimationType.Event1:
		case eCharAnimationType.Cinema_000:
		case eCharAnimationType.Cinema_001:
		case eCharAnimationType.Cinema_002:
		case eCharAnimationType.Cinema_003:
		case eCharAnimationType.Cinema_004:
			result = eCharAnimationType.BStay1;
			return result;
		}
		if (this.IsMovingAnimation(sourceanitype))
		{
			result = eCharAnimationType.Run1;
		}
		return result;
	}

	public eCharAnimationType GetNextAniType(eCharAnimationType sourceanitype)
	{
		eCharAnimationType result = eCharAnimationType.END_ANITYPE;
		switch (sourceanitype)
		{
		case eCharAnimationType.Attack1:
		case eCharAnimationType.Attack2:
		case eCharAnimationType.Attack3:
		case eCharAnimationType.ExtAttack1:
		case eCharAnimationType.AttackLeft1:
		case eCharAnimationType.AttackRight1:
		case eCharAnimationType.Skill1:
		case eCharAnimationType.Skill2:
		case eCharAnimationType.Skill3:
		case eCharAnimationType.Damage1:
		case eCharAnimationType.CriDamage1:
		case eCharAnimationType.Evade1:
		case eCharAnimationType.Respawn1:
		case eCharAnimationType.Event1:
		case eCharAnimationType.Cinema_000:
		case eCharAnimationType.Cinema_001:
		case eCharAnimationType.Cinema_002:
		case eCharAnimationType.Cinema_003:
		case eCharAnimationType.Cinema_004:
			result = eCharAnimationType.BStay1;
			break;
		case eCharAnimationType.SitS1:
			result = eCharAnimationType.SitL1;
			break;
		case eCharAnimationType.CollectS1:
			result = eCharAnimationType.CollectL1;
			break;
		case eCharAnimationType.TalkStart1:
			result = eCharAnimationType.TalkAction1;
			break;
		case eCharAnimationType.TalkAction1:
			result = eCharAnimationType.TalkStay1;
			break;
		case eCharAnimationType.TalkStay1:
			result = eCharAnimationType.Stay1;
			break;
		case eCharAnimationType.TalkEnd1:
			result = eCharAnimationType.EcoAction1;
			break;
		case eCharAnimationType.ActionStart1:
			result = eCharAnimationType.ActionLoop1;
			break;
		}
		return result;
	}

	public eCharAnimationType GetRandomAniType(eCharAnimationType sourceanitype)
	{
		eCharAnimationType result = sourceanitype;
		switch (sourceanitype)
		{
		case eCharAnimationType.SitL1:
			if (UnityEngine.Random.value < 0.5f)
			{
				result = eCharAnimationType.SitL2;
			}
			return result;
		case eCharAnimationType.SitL2:
			result = eCharAnimationType.SitL1;
			return result;
		case eCharAnimationType.SitE1:
		case eCharAnimationType.CollectS1:
		case eCharAnimationType.CollectL1:
		case eCharAnimationType.CollectE1:
			IL_2D:
			if (sourceanitype == eCharAnimationType.Stay1)
			{
				if (!this.m_pkParent3DChar.GetMiniDramaChar() && UnityEngine.Random.value < 0.5f)
				{
					result = eCharAnimationType.Stay2;
				}
				else
				{
					result = eCharAnimationType.Stay1;
				}
				return result;
			}
			if (sourceanitype == eCharAnimationType.Stay2)
			{
				return eCharAnimationType.Stay1;
			}
			if (sourceanitype == eCharAnimationType.BStay1)
			{
				return result;
			}
			if (sourceanitype != eCharAnimationType.BStay2)
			{
				return result;
			}
			return eCharAnimationType.BStay1;
		case eCharAnimationType.EcoAction1:
			if (UnityEngine.Random.value < 0.5f)
			{
				result = eCharAnimationType.EcoAction2;
			}
			return result;
		case eCharAnimationType.EcoAction2:
			return eCharAnimationType.EcoAction1;
		}
		goto IL_2D;
	}

	public bool IsLoopAnimation(eCharAnimationType sourceanitype)
	{
		switch (sourceanitype)
		{
		case eCharAnimationType.BStay1:
		case eCharAnimationType.BStay2:
		case eCharAnimationType.BRun1:
		case eCharAnimationType.Tired1:
		case eCharAnimationType.SitL1:
		case eCharAnimationType.SitL2:
		case eCharAnimationType.CollectL1:
		case eCharAnimationType.EcoAction1:
		case eCharAnimationType.EcoAction2:
		case eCharAnimationType.TalkStay1:
			return true;
		case eCharAnimationType.Damage1:
		case eCharAnimationType.CriDamage1:
		case eCharAnimationType.Evade1:
		case eCharAnimationType.Die1:
		case eCharAnimationType.SitS1:
		case eCharAnimationType.SitE1:
		case eCharAnimationType.CollectS1:
		case eCharAnimationType.CollectE1:
		case eCharAnimationType.TalkStart1:
		case eCharAnimationType.TalkAction1:
			IL_5B:
			switch (sourceanitype)
			{
			case eCharAnimationType.Stay1:
			case eCharAnimationType.Stay2:
			case eCharAnimationType.Walk1:
			case eCharAnimationType.Run1:
				return true;
			default:
				if (sourceanitype != eCharAnimationType.ActionLoop1)
				{
					return false;
				}
				return true;
			}
			break;
		}
		goto IL_5B;
	}

	public bool IsMovingAnimation(eCharAnimationType sourceanitype)
	{
		return sourceanitype == eCharAnimationType.Walk1 || sourceanitype == eCharAnimationType.Run1 || sourceanitype == eCharAnimationType.BRun1;
	}

	public bool IsDontMoveAnimation()
	{
		switch (this.m_eCurrentAniType)
		{
		case eCharAnimationType.SitS1:
		case eCharAnimationType.SitL1:
		case eCharAnimationType.SitE1:
		case eCharAnimationType.CollectS1:
		case eCharAnimationType.CollectL1:
		case eCharAnimationType.CollectE1:
		case eCharAnimationType.EcoAction1:
		case eCharAnimationType.EcoAction2:
			return true;
		}
		return false;
	}

	public void SetFinishAnimation(bool bFinish)
	{
		this.m_bFinishAnimationFrame = bFinish;
	}

	public bool GetFinishAnimation()
	{
		return this.m_bFinishAnimationFrame;
	}

	public static bool IsIdleAnimation(eCharAnimationType anitype)
	{
		switch (anitype)
		{
		case eCharAnimationType.EcoAction1:
		case eCharAnimationType.EcoAction2:
		case eCharAnimationType.TalkStay1:
		case eCharAnimationType.ActionLoop1:
			return true;
		case eCharAnimationType.TalkStart1:
		case eCharAnimationType.TalkAction1:
		case eCharAnimationType.TalkEnd1:
		case eCharAnimationType.ActionStart1:
			IL_2B:
			switch (anitype)
			{
			case eCharAnimationType.Tired1:
			case eCharAnimationType.SitL1:
			case eCharAnimationType.SitL2:
				return true;
			case eCharAnimationType.SitS1:
				IL_44:
				if (anitype != eCharAnimationType.Stay1 && anitype != eCharAnimationType.Stay2 && anitype != eCharAnimationType.BStay1 && anitype != eCharAnimationType.BStay2)
				{
					return false;
				}
				return true;
			}
			goto IL_44;
		}
		goto IL_2B;
	}

	public static bool IsBattleAnimation(eCharAnimationType anitype)
	{
		switch (anitype)
		{
		case eCharAnimationType.Attack1:
		case eCharAnimationType.Attack2:
		case eCharAnimationType.Attack3:
		case eCharAnimationType.ExtAttack1:
		case eCharAnimationType.AttackLeft1:
		case eCharAnimationType.AttackRight1:
		case eCharAnimationType.Skill1:
		case eCharAnimationType.Skill2:
		case eCharAnimationType.Skill3:
		case eCharAnimationType.BStay1:
		case eCharAnimationType.BStay2:
		case eCharAnimationType.BRun1:
		case eCharAnimationType.Damage1:
		case eCharAnimationType.CriDamage1:
		case eCharAnimationType.Evade1:
		case eCharAnimationType.Die1:
		case eCharAnimationType.Tired1:
		case eCharAnimationType.Respawn1:
		case eCharAnimationType.Event1:
			return true;
		}
		return false;
	}

	public static bool IsOnlyNormalAnimation(eCharAnimationType anitype, bool bMagic)
	{
		switch (anitype)
		{
		case eCharAnimationType.SitS1:
		case eCharAnimationType.SitL1:
		case eCharAnimationType.SitL2:
		case eCharAnimationType.SitE1:
		case eCharAnimationType.CollectS1:
		case eCharAnimationType.CollectL1:
		case eCharAnimationType.CollectE1:
			return true;
		default:
			switch (anitype)
			{
			case eCharAnimationType.Stay1:
			case eCharAnimationType.Stay2:
			case eCharAnimationType.Walk1:
			case eCharAnimationType.Run1:
				return !bMagic;
			default:
				return false;
			}
			break;
		}
	}

	public static bool IsCollectAnimation(eCharAnimationType anitype)
	{
		switch (anitype)
		{
		case eCharAnimationType.CollectS1:
		case eCharAnimationType.CollectL1:
		case eCharAnimationType.CollectE1:
			return true;
		default:
			return false;
		}
	}

	public static bool IsSitDownAnimation(eCharAnimationType anitype)
	{
		switch (anitype)
		{
		case eCharAnimationType.SitS1:
		case eCharAnimationType.SitL1:
		case eCharAnimationType.SitL2:
		case eCharAnimationType.SitE1:
			return true;
		default:
			return false;
		}
	}

	public static bool IsForcePlayAnimation(eCharAnimationType anitype)
	{
		switch (anitype)
		{
		case eCharAnimationType.Damage1:
		case eCharAnimationType.CriDamage1:
		case eCharAnimationType.Evade1:
			return true;
		default:
			return false;
		}
	}
}
