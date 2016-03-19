using GAME;
using System;
using TsBundle;
using UnityEngine;

public class Nr3DCharObject : Nr3DCharBase
{
	private enum eProcessStep
	{
		DOWNLOADING,
		ATTACHITEM,
		FINISHLOAD,
		IDLE
	}

	private Nr3DCharObject.eProcessStep m_eProcessStep;

	public void SwitchModelMesh()
	{
		if (null == this.m_kRootGameObj)
		{
			Debug.LogError("Nr3DCharNonePart root go is null at Request! " + this.m_kRootGameObj.name);
		}
		else
		{
			string path = "Char/" + this.m_szModelPath;
			base.bStartDownloadBundle = true;
			base.downloadCounter++;
			NrTSingleton<NkBundleCallBack>.Instance.RequestBundleRuntime(path, NkBundleCallBack.NPCBundleStackName, ItemType.SKIN_BONE, 0, this.m_szCharCode.ToLower(), NkBundleParam.eBundleType.BUNDLE_CHAR_NONEPART, base.GetID().ToString());
		}
	}

	public override void FinishDownloadBase(ref IDownloadedItem wItem)
	{
		base.downloadCounter--;
		GameObject gameObject = (GameObject)wItem.mainAsset;
		AudioSource[] components = gameObject.GetComponents<AudioSource>();
		AudioSource[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			AudioSource audioSource = array[i];
			if (audioSource.clip == null)
			{
				audioSource.clip = TsAudioManager.Instance.GetTempClip();
			}
		}
		this.m_kBaseObject = (GameObject)UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
		this.m_kBaseObject.transform.parent = this.m_kRootGameObj.transform;
		this.m_kBaseObject.transform.localPosition = new Vector3(0f, -0.07f, 0f);
		this.m_kBaseObject.transform.localRotation = Quaternion.identity;
		SkinnedMeshRenderer component = this.m_kBaseObject.GetComponent<SkinnedMeshRenderer>();
		if (component != null)
		{
			component.renderer.enabled = false;
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
	}

	public override bool Update()
	{
		if (!base.Update())
		{
			return false;
		}
		switch (this.m_eProcessStep)
		{
		case Nr3DCharObject.eProcessStep.DOWNLOADING:
			if (base.bStartDownloadBundle && 0 >= base.downloadCounter && null == base.GetRootGameObject().GetComponentInChildren<WBundle>())
			{
				base.bStartDownloadBundle = false;
				this.m_eProcessStep++;
			}
			break;
		case Nr3DCharObject.eProcessStep.ATTACHITEM:
			this.m_eProcessStep++;
			break;
		case Nr3DCharObject.eProcessStep.FINISHLOAD:
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
		base.StartIdleAnimation(true);
		this.AttachControllers();
		this.m_eProcessStep++;
		base.OnFinishLoadProcess();
	}

	private void AttachControllers()
	{
		CharacterController characterController = NkUtil.GuarranteeComponent<CharacterController>(this.m_kRootGameObj);
		CapsuleCollider componentInChildren = base.GetRootGameObject().GetComponentInChildren<CapsuleCollider>();
		if (null != componentInChildren)
		{
			characterController.radius = 0.1f;
			characterController.height = 1f;
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
		this.m_kCharacterCtrl = characterController;
	}

	public override bool IsAniPlay()
	{
		if (null == this.m_kBaseObject || null == this.m_kBaseObject.animation)
		{
			return false;
		}
		this.m_bAnimationPlaying = this.m_kBaseObject.animation.isPlaying;
		return this.m_bAnimationPlaying;
	}
}
