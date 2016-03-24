using GAME;
using GameMessage;
using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using System.Text;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Nr3DCharActor : Nr3DCharBase
{
	private enum eProcessStep
	{
		DOWNLOADING,
		SWITCHPART,
		ATTACHITEM,
		FINISHLOAD,
		RIDERIDE,
		IDLE
	}

	private GameObject m_kRideObject;

	private Nr3DCharPartAssetBundle[] m_kPartAssetBundle;

	private Nr3DCharActor.eProcessStep m_eProcessStep;

	private bool m_bMakeOnceLoadChar;

	private bool m_bRideState;

	private bool m_bFaceSoldier;

	private Action _RemoveScriptAction;

	public override bool removeScript
	{
		set
		{
			if (value)
			{
				this._RemoveScriptAction = new Action(this.RemoveScript);
			}
		}
	}

	public Nr3DCharActor()
	{
		this.m_bMakeOnceLoadChar = false;
		this.m_bRideState = false;
		this.m_bFaceSoldier = false;
		this.m_kPartAssetBundle = new Nr3DCharPartAssetBundle[4];
	}

	public GameObject GetRideObject()
	{
		return this.m_kRideObject;
	}

	private void RemoveAllComponent()
	{
		if (null == this.m_kRootGameObj)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.m_kRootGameObj.GetComponent<CharacterController>());
		UnityEngine.Object.Destroy(this.m_kRootGameObj.GetComponent<NrCharInfoAdaptor>());
	}

	public override void InitBundleLoadFailed(bool itembundle, int paramindex)
	{
		base.InitBundleLoadFailed(itembundle, paramindex);
		if (!itembundle)
		{
			this.InitLoadPart(paramindex);
		}
	}

	public void InitLoadPart(int partindex)
	{
		this.m_kPartAssetBundle[partindex].SetLoadPartName(string.Empty);
		this.RemovePart(partindex, true);
	}

	private void RemovePart(int partindex, bool bInitInfo)
	{
		if (this.m_kPartAssetBundle[partindex] == null || !this.m_kPartAssetBundle[partindex].IsLoadedBundle())
		{
			return;
		}
		string newName = this.m_kPartAssetBundle[partindex].GetNewName();
		Transform child = NkUtil.GetChild(this.m_kBaseObject.transform, newName);
		if (child != null)
		{
			child.parent = null;
			UnityEngine.Object.DestroyImmediate(child.gameObject);
			if (bInitInfo)
			{
				this.m_kPartAssetBundle[partindex].Init();
			}
		}
	}

	private bool IsValidIllumiate()
	{
		bool flag = true;
		flag &= !Scene.IsCurScene(Scene.Type.SELECTCHAR);
		if (this.m_pkChar != null)
		{
			flag &= (this.m_pkChar.GetID() != 1);
		}
		else if (this.m_pkBattleChar != null)
		{
			flag &= (this.m_pkBattleChar.GetID() != 1);
		}
		return flag;
	}

	private void AttachControllers()
	{
		NkUtil.GuarranteeComponent<NrCharInfoAdaptor>(this.m_kRootGameObj);
		CharacterController characterController = NkUtil.GuarranteeComponent<CharacterController>(this.m_kRootGameObj);
		CapsuleCollider componentInChildren = this.m_kRootGameObj.GetComponentInChildren<CapsuleCollider>();
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
		this.m_kCharacterCtrl = characterController;
	}

	public override void Reset()
	{
		base.Reset();
		base.bStartDownloadBundle = true;
		this.RemoveAllComponent();
		this.m_eProcessStep = Nr3DCharActor.eProcessStep.DOWNLOADING;
	}

	public void SetBase()
	{
		if (this.m_pkChar != null)
		{
			NrCharKindInfo faceCharKindInfo = this.m_pkChar.GetFaceCharKindInfo();
			if (faceCharKindInfo != null)
			{
				string text = faceCharKindInfo.GetBundlePath();
				string faceCostumeBundlePath = this.GetFaceCostumeBundlePath();
				if (!string.IsNullOrEmpty(faceCostumeBundlePath))
				{
					text = faceCostumeBundlePath;
				}
				string text2 = "Char/" + text;
				text2 = text2.ToLower();
				base.bStartDownloadBundle = true;
				base.downloadCounter++;
				NrTSingleton<NkBundleCallBack>.Instance.RequestBundleRuntime(text2, NkBundleCallBack.PlayerBundleStackName, ItemType.SKIN_BONE, 0, text.ToLower(), NkBundleParam.eBundleType.BUNDLE_CHAR_NONEPART, base.GetID().ToString(), true);
				this.m_bFaceSoldier = true;
				return;
			}
		}
		if (TsPlatform.IsMobile)
		{
			string text3 = "Char/Player/" + this.m_szModelPath;
			text3 = text3.ToLower();
			base.bStartDownloadBundle = true;
			base.downloadCounter++;
			NrTSingleton<NkBundleCallBack>.Instance.RequestBundleRuntime(text3, NkBundleCallBack.PlayerBundleStackName, ItemType.SKIN_BONE, 0, this.m_szModelPath.ToLower(), NkBundleParam.eBundleType.BUNDLE_CHAR_NONEPART, base.GetID().ToString(), true);
			return;
		}
		string text4 = "Char/Player/" + this.m_szModelPath + "/";
		text4 = text4 + this.m_szModelPath + "_bone";
		base.bStartDownloadBundle = true;
		base.downloadCounter++;
		GameObject playerModelClone = NrTSingleton<Nr3DCharSystem>.Instance.GetPlayerModelClone(this.m_szModelPath);
		if (playerModelClone == null)
		{
			NrTSingleton<NkBundleCallBack>.Instance.RequestBundleRuntime(text4, NkBundleCallBack.PlayerBundleStackName, ItemType.SKIN_BONE, 0, this.m_szCharCode, NkBundleParam.eBundleType.BUNDLE_CHAR_BONE, base.GetID().ToString());
		}
		else
		{
			this._AssignBaseObject(playerModelClone);
		}
	}

	public void SetSwitchPart(NrCharDefine.eAT2PartAssetBundle eCharPart, string filename)
	{
		if (this.m_kPartAssetBundle[(int)eCharPart] == null)
		{
			this.m_kPartAssetBundle[(int)eCharPart] = new Nr3DCharPartAssetBundle();
		}
		if (this.m_kPartAssetBundle[(int)eCharPart].IsSameLoadPartName(filename))
		{
			return;
		}
		this.m_kPartAssetBundle[(int)eCharPart].InitBundleInfo();
		base.bStartDownloadBundle = true;
		string text = "Char/Player/" + this.m_szModelPath + "/";
		text += filename;
		NrTSingleton<NkBundleCallBack>.Instance.RequestBundleRuntime(text, NkBundleCallBack.PlayerBundleStackName, ItemType.SKIN_PART, (int)eCharPart, eCharPart.ToString(), NkBundleParam.eBundleType.BUNDLE_CHAR_SWITCHPART, base.GetID().ToString());
		base.downloadCounter++;
		this.m_kPartAssetBundle[(int)eCharPart].SetLoadPartName(filename);
	}

	public void SetRide(string ridecode)
	{
		ridecode = ridecode.ToLower();
		string text = "Char/Ride/";
		text += ridecode;
		NrTSingleton<NkBundleCallBack>.Instance.RequestBundleRuntime(text, NkBundleCallBack.PlayerBundleStackName, ItemType.PART_VEHICLE, 0, ridecode, NkBundleParam.eBundleType.BUNDLE_CHAR_RIDE, base.GetID().ToString());
		base.downloadCounter++;
	}

	public void SetAssignChar()
	{
		GameObject playerModelClone = NrTSingleton<Nr3DCharSystem>.Instance.GetPlayerModelClone(this.m_szModelPath);
		if (playerModelClone != null)
		{
			this._AssignBaseObject(playerModelClone);
		}
	}

	private void _AssignBaseObject(GameObject Base)
	{
		if (this.m_kBaseObject)
		{
			UnityEngine.Object.Destroy(this.m_kBaseObject);
			this.m_kBaseObject = null;
		}
		this.m_kBaseObject = Base;
		this.m_kBaseObject.name = this.m_szCharCode;
		if (null == this.m_kBaseObject)
		{
			Debug.LogError("TTTTTT - m_kBaseObj is null");
		}
		if (null == this.m_kRootGameObj)
		{
			Debug.LogError("char bone root go is null");
			this.m_kBaseObject.transform.parent = null;
		}
		else
		{
			this.m_kBaseObject.transform.parent = this.m_kRootGameObj.transform;
			this.m_kBaseObject.transform.localPosition = new Vector3(0f, -0.07f, 0f);
			this.m_kBaseObject.transform.localRotation = Quaternion.identity;
		}
		if (this._RemoveScriptAction != null)
		{
			this._RemoveScriptAction();
		}
		NkUtil.SetAllChildActive(this.m_kBaseObject, true);
		this.m_kBaseObject.animation.enabled = true;
		base.downloadCounter--;
	}

	public override void FinishDownloadBase(ref IDownloadedItem wItem)
	{
		base.downloadCounter--;
		GameObject original;
		if (!TsPlatform.IsMobile)
		{
			GameObject gameObject = wItem.mainAsset as GameObject;
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
			original = gameObject;
		}
		else
		{
			original = (wItem.mainAsset as GameObject);
		}
		this.m_kBaseObject = (UnityEngine.Object.Instantiate(original) as GameObject);
		this.m_kBaseObject.name = this.m_szCharCode;
		if (null == this.m_kBaseObject)
		{
			Debug.LogError("TTTTTT - m_kBaseObj is null");
		}
		if (null == this.m_kRootGameObj)
		{
			Debug.LogError("char bone root go is null");
			this.m_kBaseObject.transform.parent = null;
		}
		else
		{
			this.m_kBaseObject.transform.parent = this.m_kRootGameObj.transform;
			this.m_kBaseObject.transform.localPosition = new Vector3(0f, -0.07f, 0f);
			this.m_kBaseObject.transform.localRotation = Quaternion.identity;
		}
		if (this._RemoveScriptAction != null)
		{
			this._RemoveScriptAction();
		}
		NkUtil.SetAllChildActive(this.m_kBaseObject, true);
		this.m_kBaseObject.animation.enabled = true;
		base.FinishDownloadBase(ref wItem);
	}

	private string FindMainCharCode()
	{
		string empty = string.Empty;
		string charCode = base.GetCharCode();
		int num = 0;
		for (int i = 0; i < charCode.Length; i++)
		{
			char c = charCode[i];
			if (c == '_')
			{
				return charCode.Substring(num, i - num);
			}
		}
		return charCode;
	}

	private void MakeWeaponTrail()
	{
		string arg = base.GetCharCode();
		string weaponCode = base.GetParentCharSoldierInfo().GetWeaponCode();
		if (base.GetParentCharKindInfo().IsATB(1L))
		{
			arg = this.FindMainCharCode();
		}
		string str = string.Format("Effect/AttackEffect/fx_{0}_{1}{2}", arg, weaponCode, NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.EffectBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.FinishDownloadWeaponTrail), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, false);
	}

	public void FinishDownloadWeaponTrail(IDownloadedItem wItem, object kParamObj)
	{
		try
		{
			if (!(this.m_kBaseObject == null))
			{
				if (!wItem.isCanceled)
				{
					if (wItem.GetSafeBundle() == null)
					{
						TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
						{
							wItem.assetPath
						});
					}
					else
					{
						GameObject original = wItem.GetSafeBundle().mainAsset as GameObject;
						this.m_kWeaponTrailObject = (GameObject)UnityEngine.Object.Instantiate(original);
						this.m_kWeaponTrailObject.transform.parent = this.m_kBaseObject.transform;
						this.m_kWeaponTrailObject.transform.localPosition = Vector3.zero;
						this.m_kWeaponTrailObject.SetActive(true);
						NkUtil.SetAllChildActive(this.m_kWeaponTrailObject, false, true);
					}
				}
			}
		}
		catch (Exception obj)
		{
			TsLog.LogError(obj);
		}
	}

	public void FinishDownloadPart(ref IDownloadedItem wItem)
	{
		base.downloadCounter--;
		this.m_kPartAssetBundle[wItem.indexParam].SetAssetBundle(wItem.GetSafeBundle());
		this.m_kPartAssetBundle[wItem.indexParam].SetNewName(wItem.strParam);
		if (this.m_bCreated)
		{
			GameObject renderObject = this.m_kPartAssetBundle[wItem.indexParam].GetRenderObject();
			if (renderObject != null)
			{
				this.SwitchPart(wItem.indexParam, renderObject, true);
			}
		}
	}

	public void FinishDownloadRide(ref IDownloadedItem wItem)
	{
		base.downloadCounter--;
		if (this.m_kRideObject)
		{
			UnityEngine.Object.DestroyImmediate(this.m_kRideObject);
			this.m_kRideObject = null;
		}
		GameObject original = (GameObject)wItem.GetSafeBundle().mainAsset;
		this.m_kRideObject = (GameObject)UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity);
		this.m_kRideObject.transform.localPosition = Vector3.zero;
		this.m_kRideObject.name = wItem.strParam;
		this.m_kRideObject.animation.playAutomatically = false;
		this.m_kRideObject.animation.Stop();
		List<AnimationClip> list = new List<AnimationClip>();
		foreach (AnimationState animationState in this.m_kRideObject.animation)
		{
			list.Add(animationState.clip);
		}
		string text = string.Empty;
		for (int i = 0; i < list.Count; i++)
		{
			AnimationClip animationClip = list[i];
			this.m_kRideObject.animation.RemoveClip(animationClip.name);
			text = this.m_szModelPath + animationClip.name + "_" + wItem.strParam;
			this.m_kRideObject.animation.AddClip(animationClip, text.ToLower());
		}
		list.Clear();
		if (this.m_eProcessStep > Nr3DCharActor.eProcessStep.RIDERIDE)
		{
			this.m_eProcessStep = Nr3DCharActor.eProcessStep.RIDERIDE;
		}
		this.m_bRideState = true;
	}

	public override string MakeAnimationKey(eCharAnimationType anitype)
	{
		StringBuilder stringBuilder = new StringBuilder(string.Empty);
		string value = anitype.ToString();
		if (!this.m_bFaceSoldier)
		{
			bool flag = base.GetParentCharKindInfo().GetCharTribe() == 4;
			if (NrCharAnimation.IsBattleAnimation(anitype) || flag)
			{
				if (!base.IsBattleChar())
				{
					if (!NrCharAnimation.IsOnlyNormalAnimation(anitype, flag))
					{
						stringBuilder.Append(base.GetParentChar().GetWeaponCode());
					}
				}
				else
				{
					stringBuilder.Append(base.GetParentBattleChar().GetWeaponCode());
				}
			}
		}
		if (this.IsRideState())
		{
			string value2 = "Camel";
			stringBuilder.Append("_");
			stringBuilder.Append(value2);
		}
		stringBuilder.Append(value);
		return stringBuilder.ToString().ToLower();
	}

	private AnimationClip GetAnimationClip(eCharAnimationType targetani)
	{
		if (this.m_kBaseObject == null)
		{
			return null;
		}
		if (this.m_bFaceSoldier)
		{
			if (targetani == eCharAnimationType.Run1)
			{
				targetani = eCharAnimationType.BRun1;
			}
			else if (targetani == eCharAnimationType.Stay1)
			{
				targetani = eCharAnimationType.BStay1;
			}
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
			if (this.m_kBaseObject.animation.isPlaying && animationClip.wrapMode != WrapMode.Loop)
			{
				this.m_kBaseObject.animation.Stop();
			}
			if (!this.m_kBaseObject.animation.Play(name))
			{
				Debug.LogWarning("m_kBaseObject.animation.Play failed =====> " + name);
			}
		}
		if (bForceLoop)
		{
			if (animationClip.wrapMode != WrapMode.Loop)
			{
				animationClip.wrapMode = WrapMode.Loop;
			}
		}
		else if (animationClip.wrapMode != WrapMode.Once)
		{
			animationClip.wrapMode = WrapMode.Once;
		}
		bool bBlend = 0f != fBlendTime;
		this.SetRideAnimation(name, bBlend, bForceLoop);
		this.SetWeaponTrailAnimation(targetani, bBlend, bForceLoop);
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

	public void SetWeaponTrailAnimation(eCharAnimationType targetani, bool bBlend, bool bForceLoop)
	{
		if (this.m_kWeaponTrailObject == null)
		{
			return;
		}
		if (targetani < eCharAnimationType.Attack1 || targetani > eCharAnimationType.Attack3)
		{
			return;
		}
		string text = targetani.ToString();
		text = text.ToLower();
		Transform child = NkUtil.GetChild(this.m_kWeaponTrailObject.transform, text);
		if (null == child)
		{
			return;
		}
		child.gameObject.SetActive(false);
		child.gameObject.SetActive(true);
	}

	public void SetRideAnimation(string anikey, bool bBlend, bool bForceLoop)
	{
		if (this.m_kRideObject == null)
		{
			return;
		}
		if (!this.IsRideState())
		{
			this.m_kRideObject.animation.Stop();
			return;
		}
		AnimationClip clip = this.m_kBaseObject.animation.GetClip(anikey);
		if (clip == null)
		{
			return;
		}
		if (bBlend)
		{
			float fadeLength = 0.3f;
			this.m_kRideObject.animation.CrossFade(anikey, fadeLength);
		}
		else
		{
			this.m_kRideObject.animation.Play(anikey);
		}
		if (bForceLoop && clip.wrapMode != WrapMode.Loop)
		{
			clip.wrapMode = WrapMode.Loop;
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
		case Nr3DCharActor.eProcessStep.DOWNLOADING:
			if (base.bStartDownloadBundle && base.downloadCounter == 0)
			{
				this.m_bMakeOnceLoadChar = false;
				this.debugLog.Log("downloaded all.");
				base.bStartDownloadBundle = false;
				this.m_eProcessStep++;
			}
			break;
		case Nr3DCharActor.eProcessStep.SWITCHPART:
			if (!(this.m_kBaseObject == null))
			{
				this.SwitchPartProcess();
				this.debugLog.Log("processed combine meshes.");
				this.m_eProcessStep++;
			}
			break;
		case Nr3DCharActor.eProcessStep.ATTACHITEM:
			base.AttachItemProcess();
			this.debugLog.Log("processed attach item.");
			this.m_eProcessStep++;
			break;
		case Nr3DCharActor.eProcessStep.FINISHLOAD:
			this.FinishLoadProcess();
			this.m_eProcessStep++;
			break;
		case Nr3DCharActor.eProcessStep.RIDERIDE:
			this.RideProcess();
			this.m_eProcessStep++;
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
		this.debugLog.Log("Attached blob shadow.");
		if (!this.m_bMakeOnceLoadChar)
		{
			base.StartIdleAnimation(true);
			this.m_bMakeOnceLoadChar = true;
		}
		this.AttachControllers();
		base.OnFinishLoadProcess();
		if (base.IsBattleChar())
		{
			this.MakeWeaponTrail();
		}
	}

	public void SwitchPartProcess()
	{
		if (this.m_kBaseObject == null)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (this.m_kPartAssetBundle[i] != null)
			{
				if (this.m_kPartAssetBundle[i].IsValid())
				{
					GameObject renderObject = this.m_kPartAssetBundle[i].GetRenderObject();
					if (renderObject != null)
					{
						this.SwitchPart(i, renderObject, false);
					}
				}
			}
		}
	}

	private void SwitchPart(int partindex, GameObject partobject, bool bShow)
	{
		if (partobject == null)
		{
			return;
		}
		this.RemovePart(partindex, false);
		SkinnedMeshRenderer componentInChildren = partobject.GetComponentInChildren<SkinnedMeshRenderer>();
		if (componentInChildren == null)
		{
			return;
		}
		string[] boneNameList = this.m_kPartAssetBundle[partindex].GetBoneNameList();
		Transform child = NkUtil.GetChild(this.m_kBaseObject.transform, "Bone01");
		if (child == null)
		{
			return;
		}
		Transform[] componentsInChildren = child.GetComponentsInChildren<Transform>();
		List<Transform> list = new List<Transform>();
		list.Clear();
		string[] array = boneNameList;
		for (int i = 0; i < array.Length; i++)
		{
			string b = array[i];
			Transform[] array2 = componentsInChildren;
			for (int j = 0; j < array2.Length; j++)
			{
				Transform transform = array2[j];
				if (!(transform == null))
				{
					if (transform.name == b)
					{
						list.Add(transform);
					}
				}
			}
		}
		componentInChildren.bones = list.ToArray();
		componentInChildren.renderer.enabled = bShow;
		Vector3 localPosition = partobject.transform.localPosition;
		partobject.transform.parent = this.m_kBaseObject.transform;
		partobject.transform.localPosition = localPosition;
		if (base.IsBattleChar())
		{
			partobject.layer = TsLayer.PC;
		}
		else if (this.m_pkChar != null && this.m_pkChar.GetID() == 1)
		{
			partobject.layer = TsLayer.PC;
		}
		else
		{
			partobject.layer = TsLayer.PC_OTHER;
		}
		if (this.m_bCreated)
		{
			base.OnEvent3DModelPartItemChanged(partobject);
		}
		if (Scene.IsCurScene(Scene.Type.SELECTCHAR))
		{
			MsgHandler.Handle("SetCreateCharPartInfo", new object[]
			{
				false,
				true
			});
		}
		if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
		{
			componentInChildren.renderer.enabled = false;
		}
	}

	private void RideProcess()
	{
		if (this.m_kRideObject != null)
		{
			this.m_kRideObject.transform.parent = this.m_kRootGameObj.transform;
			this.m_kRideObject.transform.localRotation = Quaternion.identity;
			this.m_kRideObject.transform.localPosition = Vector3.zero;
			if (this.m_kBaseObject != null)
			{
				Transform child = NkUtil.GetChild(this.m_kRideObject.transform, "dmride");
				if (child != null)
				{
					this.m_kBaseObject.transform.parent = child;
					this.m_kBaseObject.transform.localPosition = Vector3.zero;
				}
			}
		}
		this.debugLog.Log("processed ride ride.");
	}

	public bool IsModelLoadCompleted()
	{
		return this.m_eProcessStep > Nr3DCharActor.eProcessStep.FINISHLOAD;
	}

	public void SetRideState(bool bRideState)
	{
		this.m_bRideState = bRideState;
	}

	public bool IsRideState()
	{
		return this.m_bRideState;
	}

	public override eCharAnimationType GetMoveAnimation()
	{
		eCharAnimationType result = eCharAnimationType.Run1;
		if (base.IsBattleChar())
		{
			result = eCharAnimationType.BRun1;
		}
		return result;
	}

	public override void MoveTo(Vector3 v3Pos)
	{
		this.MoveTo(v3Pos.x, v3Pos.y, v3Pos.z);
	}

	public override void MoveTo(float x, float y, float z)
	{
		this.MoveTo(x, y, z, this.GetMoveAnimation(), false);
	}

	protected override void ReachedDestProcess(bool bSetAni)
	{
		if (bSetAni)
		{
			base.SetParentCharAnimation(this.GetIdleAnimation());
		}
		this.m_bIsMoveToTarget = false;
		if (this.m_bRequestLookAt)
		{
			if (this.m_PosLookAt.x != 0f && this.m_PosLookAt.z != 0f)
			{
				base.SetLookAt(this.m_PosLookAt.x, this.m_PosLookAt.y, this.m_PosLookAt.z, false);
			}
			this.m_bRequestLookAt = false;
		}
	}

	public void KeyboardMove(Vector3 toDir, bool bSmoothTurn)
	{
		if (!base.Is3DCharActive())
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (bSmoothTurn)
		{
			Quaternion to = Quaternion.LookRotation(toDir);
			Quaternion rotation = Quaternion.Slerp(this.m_kRootGameObj.transform.rotation, to, deltaTime * 10f);
			this.m_kRootGameObj.transform.rotation = rotation;
		}
		else
		{
			this.m_kRootGameObj.transform.rotation = Quaternion.LookRotation(toDir);
		}
		float num = base.GetSpeed() / 10f;
		num = deltaTime * num;
		this.m_kCharacterCtrl.Move(toDir * num);
		base.SetParentCharAnimation(this.GetMoveAnimation());
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

	private void RemoveLookAt()
	{
	}

	private void RemoveLocoMotion()
	{
	}

	private void RemoveScript()
	{
		this.RemoveLocoMotion();
		this.RemoveLookAt();
	}

	private string GetFaceCostumeBundlePath()
	{
		if (this.m_pkChar.GetFaceSolID() == 0L)
		{
			return string.Empty;
		}
		if (base.GetParentFaceSoldierInfo() != null)
		{
			int costumeUnique = (int)base.GetParentFaceSoldierInfo().GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
			return NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeBundlePath(costumeUnique);
		}
		if (0 < this.m_pkChar.GetFaceCostumeUnique())
		{
			return NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeBundlePath(this.m_pkChar.GetFaceCostumeUnique());
		}
		return string.Empty;
	}
}
