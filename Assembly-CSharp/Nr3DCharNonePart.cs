using GAME;
using System;
using TsBundle;
using UnityEngine;

public class Nr3DCharNonePart : Nr3DCharBase
{
	private enum eProcessStep
	{
		DOWNLOADING,
		COMBINEMESHS,
		ATTACHITEM,
		FINISHLOAD,
		IDLE
	}

	private Nr3DCharNonePart.eProcessStep m_eProcessStep;

	private void SetTrailActive(bool bTurnOff)
	{
		if (this.m_kBaseObject == null)
		{
			return;
		}
		int childCount = this.m_kBaseObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = this.m_kBaseObject.transform.GetChild(i);
			if (!(child == null))
			{
				if (child.name.Contains("trail"))
				{
					child.gameObject.SetActive(bTurnOff);
				}
			}
		}
	}

	private string GenerateKey_()
	{
		return ("char/" + this.m_szModelPath).ToLower();
	}

	public void SwitchModelMesh()
	{
		if (null == this.m_kRootGameObj)
		{
			Debug.LogError("Nr3DCharNonePart root go is null at Request! " + this.m_kRootGameObj.name);
		}
		else
		{
			string text = this.GenerateKey_();
			if (!NpcCache.TryCloneObject(text, out this.m_kBaseObject))
			{
				base.bStartDownloadBundle = true;
				base.downloadCounter++;
				NrTSingleton<NkBundleCallBack>.Instance.RequestBundleRuntime(text, NkBundleCallBack.NPCBundleStackName, ItemType.SKIN_BONE, 0, this.m_szCharCode.ToLower(), NkBundleParam.eBundleType.BUNDLE_CHAR_NONEPART, base.GetID().ToString(), true);
			}
			else
			{
				this.FinishDownloadModelSetting();
			}
		}
	}

	public override void FinishDownloadBase(ref IDownloadedItem wItem)
	{
		base.downloadCounter--;
		if (NpcCache.Enabled)
		{
			this.m_kBaseObject = NpcCache.AddAndClone(this.GenerateKey_(), wItem);
		}
		else
		{
			GameObject original = wItem.mainAsset as GameObject;
			this.m_kBaseObject = (UnityEngine.Object.Instantiate(original) as GameObject);
		}
		this.FinishDownloadModelSetting();
		base.FinishDownloadBase(ref wItem);
	}

	public void FinishDownloadModelSetting()
	{
		if (this.m_kBaseObject.animation != null && this.m_kBaseObject.animation.playAutomatically)
		{
			this.m_kBaseObject.animation.playAutomatically = false;
		}
		this.m_kBaseObject.transform.parent = this.m_kRootGameObj.transform;
		this.m_kBaseObject.transform.localPosition = new Vector3(0f, -0.07f, 0f);
		this.m_kBaseObject.transform.localRotation = Quaternion.identity;
		if (!base.IsBattleChar())
		{
			this.SetTrailActive(false);
		}
		else if (this.m_kBaseObject.animation != null && this.m_kBaseObject.animation.cullingType != AnimationCullingType.AlwaysAnimate)
		{
			this.m_kBaseObject.animation.cullingType = AnimationCullingType.AlwaysAnimate;
		}
	}

	public override string MakeAnimationKey(eCharAnimationType anitype)
	{
		string text = anitype.ToString();
		return text.ToLower();
	}

	private AnimationClip GetAnimationClip(eCharAnimationType targetani)
	{
		if (this.m_kBaseObject == null)
		{
			return null;
		}
		if (this.m_kBaseObject.animation == null)
		{
			return null;
		}
		string name = this.MakeAnimationKey(targetani);
		AnimationClip clip = this.m_kBaseObject.animation.GetClip(name);
		if (!(clip == null))
		{
			base.SetCurrentAniType(targetani.ToString());
		}
		return clip;
	}

	public override AnimationClip SetAnimation(eCharAnimationType targetani, float fBlendTime, bool bForceLoop)
	{
		if (this.m_kBaseObject == null || this.m_kBaseObject.animation == null)
		{
			return null;
		}
		AnimationClip animationClip = this.GetAnimationClip(targetani);
		if (animationClip == null)
		{
			animationClip = base.GetSafeAnimationClip(targetani);
			if (animationClip == null)
			{
				return null;
			}
		}
		string name = animationClip.name;
		if (bForceLoop && animationClip.wrapMode != WrapMode.Loop)
		{
			animationClip.wrapMode = WrapMode.Loop;
		}
		if (base.IsBattleChar() && (targetani == eCharAnimationType.Damage1 || targetani == eCharAnimationType.CriDamage1))
		{
			this.m_kBaseObject.animation.Stop();
		}
		if (fBlendTime != 0f)
		{
			this.m_kBaseObject.animation.CrossFade(name, fBlendTime);
		}
		else
		{
			this.m_kBaseObject.animation.Play(name);
		}
		return animationClip;
	}

	public override void SetFacialAnimation(string anikey, WrapMode wrapmode)
	{
		if (this.m_kFaceObject == null)
		{
			return;
		}
		if (this.m_kFaceObject.animation == null)
		{
			return;
		}
		if (anikey.Equals(string.Empty))
		{
			this.m_kFaceObject.animation.Stop();
		}
		else
		{
			if (this.m_kFaceObject.animation[anikey] == null)
			{
				return;
			}
			this.m_kFaceObject.animation[anikey].wrapMode = wrapmode;
			this.m_kFaceObject.animation.Play(anikey);
		}
	}

	public override bool Update()
	{
		if (!base.Update())
		{
			return false;
		}
		switch (this.m_eProcessStep)
		{
		case Nr3DCharNonePart.eProcessStep.DOWNLOADING:
			if (base.bStartDownloadBundle && base.downloadCounter <= 0)
			{
				base.bStartDownloadBundle = false;
				this.m_eProcessStep++;
			}
			break;
		case Nr3DCharNonePart.eProcessStep.COMBINEMESHS:
			this.CombineMeshes();
			this.m_eProcessStep++;
			break;
		case Nr3DCharNonePart.eProcessStep.ATTACHITEM:
			base.AttachItemProcess();
			this.m_eProcessStep++;
			break;
		case Nr3DCharNonePart.eProcessStep.FINISHLOAD:
			this.FinishLoadProcess();
			break;
		}
		return base.PostUpdate();
	}

	private void FinishLoadProcess()
	{
		if (this.m_bCreated)
		{
			return;
		}
		if (this.m_kRootGameObj != null)
		{
			base.StartIdleAnimation(true);
			this.AttachControllers();
		}
		if (this.m_kBaseObject != null && this.m_kBaseObject.animation != null)
		{
			this.m_kBaseObject.animation.playAutomatically = true;
			this.m_kBaseObject.animation.cullingType = AnimationCullingType.AlwaysAnimate;
		}
		this.m_eProcessStep++;
		base.OnFinishLoadProcess();
	}

	private void AttachControllers()
	{
		CharacterController characterController = NkUtil.GuarranteeComponent<CharacterController>(this.m_kRootGameObj);
		CapsuleCollider componentInChildren = this.m_kBaseObject.GetComponentInChildren<CapsuleCollider>();
		if (null != componentInChildren)
		{
			characterController.radius = 0.1f;
			characterController.height = componentInChildren.height;
			characterController.center = new Vector3(0f, characterController.height / 2f, 0f);
			characterController.slopeLimit = 45f;
			characterController.stepOffset = 0.8f;
			UnityEngine.Object.DestroyObject(componentInChildren);
		}
		else
		{
			characterController.radius = 0.1f;
			this.m_pkPickingCollider = NkUtil.GetChild(this.m_kRootGameObj.transform, "PickingCollider");
			if (this.m_pkPickingCollider != null)
			{
				BoxCollider component = this.m_pkPickingCollider.GetComponent<BoxCollider>();
				if (component != null)
				{
					characterController.height = component.size.y;
				}
				else
				{
					characterController.height = 1f;
				}
			}
			else
			{
				characterController.height = 1f;
			}
			characterController.center = new Vector3(0f, characterController.height / 2f, 0f);
			characterController.slopeLimit = 45f;
			characterController.stepOffset = 0.8f;
		}
		if (this.m_kBaseObject != null)
		{
			Transform child = NkUtil.GetChild(this.m_kBaseObject.transform, "Camera");
			if (child == null)
			{
				GameObject gameObject = new GameObject("Camera");
				Vector3 a = characterController.center;
				a.y += characterController.height / 4f;
				a += this.m_kBaseObject.transform.position;
				Vector3 vector = this.m_kBaseObject.transform.position;
				vector += this.m_kBaseObject.transform.forward * 2f;
				vector.y = Mathf.Max(0.3f, a.y);
				gameObject.transform.localPosition = vector;
				gameObject.transform.parent = this.m_kBaseObject.transform;
			}
		}
		this.m_kCharacterCtrl = characterController;
	}

	public override bool IsAniPlay()
	{
		if (null == this.m_kBaseObject)
		{
			return false;
		}
		if (null == this.m_kBaseObject.animation)
		{
			return false;
		}
		this.m_bAnimationPlaying = this.m_kBaseObject.animation.isPlaying;
		return this.m_bAnimationPlaying;
	}

	private void CombineMeshes()
	{
		if (this.m_kBaseObject == null)
		{
			return;
		}
		this.SetShowHide(false);
	}

	public override eCharAnimationType GetIdleAnimation()
	{
		eCharAnimationType eCharAnimationType = eCharAnimationType.Stay1;
		if (base.IsBattleChar())
		{
			return this.m_pkBattleChar.GetStayAni();
		}
		NrCharBase parentChar = base.GetParentChar();
		if (parentChar != null && !parentChar.IsSubChar)
		{
			eCharKindType charKindType = parentChar.GetCharKindType();
			if (charKindType == eCharKindType.CKT_NPC)
			{
				if (parentChar.LoadAfterAnimation != eCharAnimationType.TalkStart1 && parentChar.LoadAfterAnimation != eCharAnimationType.TalkStay1)
				{
					eCharAnimationType = eCharAnimationType.EcoAction1;
				}
				if (!base.IsHaveAnimation(eCharAnimationType))
				{
					eCharAnimationType = eCharAnimationType.Stay1;
				}
				NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(base.GetCharCode());
				if (charKindInfoFromCode != null && charKindInfoFromCode.IsATB(1125899906842624L))
				{
					eCharAnimationType = eCharAnimationType.Stay1;
					if (!base.IsHaveAnimation(eCharAnimationType))
					{
						eCharAnimationType = eCharAnimationType.BStay1;
					}
				}
			}
		}
		return eCharAnimationType;
	}

	public override eCharAnimationType GetMoveAnimation()
	{
		eCharAnimationType eCharAnimationType = eCharAnimationType.Walk1;
		if (base.GetSpeed() > 50f)
		{
			eCharAnimationType = eCharAnimationType.Run1;
		}
		if (base.IsBattleChar())
		{
			eCharAnimationType = eCharAnimationType.BRun1;
		}
		if (!base.IsHaveAnimation(eCharAnimationType))
		{
			eCharAnimationType = eCharAnimationType.Walk1;
		}
		return eCharAnimationType;
	}
}
