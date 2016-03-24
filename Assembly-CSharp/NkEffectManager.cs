using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class NkEffectManager : NrTSingleton<NkEffectManager>
{
	private Dictionary<uint, NkEffectUnit> m_kEffectList = new Dictionary<uint, NkEffectUnit>();

	private uint m_nCurrentRegistNum;

	private Dictionary<short, uint> m_kCharEffectList = new Dictionary<short, uint>();

	private Dictionary<string, List<NkEffectUnit>> m_kReserveEffect = new Dictionary<string, List<NkEffectUnit>>();

	private Dictionary<uint, NkEffectReservationInfo> m_kEffectReservationInfo = new Dictionary<uint, NkEffectReservationInfo>();

	private List<uint> m_kLifeTimeEffectList = new List<uint>();

	private bool m_bDontMakeEffect;

	private bool bUseEffectCache = true;

	private Dictionary<string, EFFECT_INFO> m_kEffectInfoList = new Dictionary<string, EFFECT_INFO>();

	private NkValueParse<eEFFECT_TYPE> m_kEffectTypeParse = new NkValueParse<eEFFECT_TYPE>();

	private NkValueParse<eEFFECT_POS> m_kEffectPosParse = new NkValueParse<eEFFECT_POS>();

	private GameObject m_kEffectModelRoot;

	private Dictionary<string, GameObject> m_kEffectGameObject = new Dictionary<string, GameObject>();

	private List<string> m_liForceReserve = new List<string>();

	public bool DontMakeEffect
	{
		get
		{
			return this.m_bDontMakeEffect;
		}
		set
		{
			this.m_bDontMakeEffect = value;
		}
	}

	public bool UseEffectCache
	{
		get
		{
			return this.bUseEffectCache;
		}
		set
		{
			this.bUseEffectCache = value;
		}
	}

	private NkEffectManager()
	{
		this._RegisterEffectType();
	}

	private uint _GetNextRegistNum()
	{
		if ((this.m_nCurrentRegistNum += 1u) == 0u)
		{
			this.m_nCurrentRegistNum = 1u;
		}
		return this.m_nCurrentRegistNum;
	}

	public void ClearAllEffect()
	{
		this.m_kEffectList.Clear();
		this.m_kCharEffectList.Clear();
		this.m_kReserveEffect.Clear();
		this.m_kEffectReservationInfo.Clear();
		this.m_kLifeTimeEffectList.Clear();
	}

	public uint AddEffect(string effectKind, NkBattleChar kBattleChar)
	{
		if (this.m_bDontMakeEffect && NrTSingleton<NkEffectManager>.Instance.isEffectLimit(effectKind))
		{
			return 0u;
		}
		if (kBattleChar == null)
		{
			return 0u;
		}
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return 0u;
		}
		NkEffectUnit kEffectUnit = new NkEffectUnit(effectInfo, kBattleChar);
		return this._AddEffect(kEffectUnit);
	}

	public uint AddEffect(string effectKind, NkBattleChar kBattleChar, bool bAttachEffectPos, bool CheckScale)
	{
		if (this.m_bDontMakeEffect && NrTSingleton<NkEffectManager>.Instance.isEffectLimit(effectKind))
		{
			return 0u;
		}
		if (kBattleChar == null)
		{
			return 0u;
		}
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return 0u;
		}
		NkEffectUnit kEffectUnit = new NkEffectUnit(effectInfo, kBattleChar, bAttachEffectPos, CheckScale);
		return this._AddEffect(kEffectUnit);
	}

	public uint AddCasterEffect(string effectKind, NkBattleChar kBattleChar)
	{
		if (this.m_bDontMakeEffect && NrTSingleton<NkEffectManager>.Instance.isEffectLimit(effectKind))
		{
			return 0u;
		}
		if (kBattleChar == null)
		{
			return 0u;
		}
		Nr3DCharBase nr3DCharBase = kBattleChar.Get3DChar();
		if (nr3DCharBase == null)
		{
			return 0u;
		}
		Transform effectTarget = nr3DCharBase.GetEffectTarget();
		if (effectTarget == null)
		{
			return 0u;
		}
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return 0u;
		}
		GameObject goTarget = nr3DCharBase.GetRootGameObject();
		Transform effectPos = nr3DCharBase.GetEffectPos(effectInfo.EFFECT_POS);
		if (null != effectPos && effectInfo.EFFECT_POS == eEFFECT_POS.CENTERDM)
		{
			goTarget = effectPos.gameObject;
		}
		NkEffectUnit kEffectUnit;
		if (effectInfo.EFFECT_POS == eEFFECT_POS.CENTERDM)
		{
			kEffectUnit = new NkEffectUnit(effectInfo, goTarget);
		}
		else
		{
			kEffectUnit = new NkEffectUnit(effectInfo, kBattleChar);
		}
		return this._AddEffect(kEffectUnit);
	}

	public uint AddCenterPosEffect(string effectKind, NkBattleChar kBattleChar, Vector3 v3CenterPos)
	{
		if (this.m_bDontMakeEffect && NrTSingleton<NkEffectManager>.Instance.isEffectLimit(effectKind))
		{
			return 0u;
		}
		if (kBattleChar == null)
		{
			return 0u;
		}
		Nr3DCharBase nr3DCharBase = kBattleChar.Get3DChar();
		if (nr3DCharBase == null)
		{
			return 0u;
		}
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return 0u;
		}
		Transform effectPos = nr3DCharBase.GetEffectPos(effectInfo.EFFECT_POS);
		if (null != effectPos)
		{
			v3CenterPos.y = effectPos.position.y;
		}
		NkEffectUnit kEffectUnit = new NkEffectUnit(effectInfo, nr3DCharBase.GetRootGameObject(), v3CenterPos);
		return this._AddEffect(kEffectUnit);
	}

	public uint AddEffect(string effectKind, NrCharBase kCharBase)
	{
		if (kCharBase == null)
		{
			return 0u;
		}
		Nr3DCharBase nr3DCharBase = kCharBase.Get3DChar();
		if (nr3DCharBase == null)
		{
			return 0u;
		}
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return 0u;
		}
		GameObject gameObject = nr3DCharBase.GetRootGameObject();
		Vector3 v3Target = Vector3.zero;
		Transform effectPos = nr3DCharBase.GetEffectPos(effectInfo.EFFECT_POS);
		if (null != effectPos)
		{
			v3Target = effectPos.position;
			if (effectInfo.EFFECT_POS == eEFFECT_POS.CENTERDM)
			{
				gameObject = effectPos.gameObject;
			}
		}
		if (effectInfo.EFFECT_POS == eEFFECT_POS.BONE)
		{
			gameObject = nr3DCharBase.GetBoneRootObject();
			if (gameObject == null)
			{
				gameObject = nr3DCharBase.GetRootGameObject();
			}
			v3Target = Vector3.zero;
			v3Target.y = -1f;
		}
		NkEffectUnit kEffectUnit;
		if (effectInfo.EFFECT_POS == eEFFECT_POS.CENTERDM)
		{
			kEffectUnit = new NkEffectUnit(effectInfo, gameObject);
		}
		else
		{
			kEffectUnit = new NkEffectUnit(effectInfo, gameObject, v3Target);
		}
		return this._AddEffect(kEffectUnit);
	}

	public uint AddEffectByTarget(string effectKind, NrCharBase kCharBase, Vector3 diffPos)
	{
		if (kCharBase == null)
		{
			return 0u;
		}
		Nr3DCharBase nr3DCharBase = kCharBase.Get3DChar();
		if (nr3DCharBase == null)
		{
			return 0u;
		}
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return 0u;
		}
		NkEffectUnit kEffectUnit = new NkEffectUnit(effectInfo, nr3DCharBase.GetRootGameObject(), diffPos);
		return this._AddEffect(kEffectUnit);
	}

	public uint AddEffect(string effectKind, GameObject goTarget)
	{
		if (this.m_bDontMakeEffect && NrTSingleton<NkEffectManager>.Instance.isEffectLimit(effectKind))
		{
			return 0u;
		}
		if (goTarget == null)
		{
			return 0u;
		}
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return 0u;
		}
		NkEffectUnit kEffectUnit = new NkEffectUnit(effectInfo, goTarget);
		return this._AddEffect(kEffectUnit);
	}

	public uint AddEffect(string effectKind, GameObject goTarget, NkEffectUnit.DeleteCallBack DelCallBack)
	{
		if (goTarget == null)
		{
			return 0u;
		}
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return 0u;
		}
		return this._AddEffect(new NkEffectUnit(effectInfo, goTarget)
		{
			DelCallBack = DelCallBack
		});
	}

	public uint AddEffect(string effectKind, Vector3 v3Target)
	{
		if (Vector3.zero == v3Target)
		{
			return 0u;
		}
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return 0u;
		}
		NkEffectUnit kEffectUnit = new NkEffectUnit(effectInfo, v3Target);
		return this._AddEffect(kEffectUnit);
	}

	public uint ExternAddEffect(NkEffectUnit kEffectUnit)
	{
		return this._AddEffect(kEffectUnit);
	}

	private uint _AddEffect(NkEffectUnit kEffectUnit)
	{
		uint num = this._GetNextRegistNum();
		kEffectUnit.RegistNum = num;
		if (this._IsImmediateMake(kEffectUnit.EffectKind))
		{
			this._MakeEffect(kEffectUnit);
		}
		else
		{
			this._AddReverseEffect(kEffectUnit);
		}
		return num;
	}

	private void _MakeEffect(NkEffectUnit kEffectUnit)
	{
		GameObject gameObject = this._GetEffectGameObject(kEffectUnit.EffectKind);
		if (null == gameObject)
		{
			Debug.LogError(kEffectUnit.EffectKind + "Make Fail:");
			return;
		}
		kEffectUnit.m_goEffect = gameObject;
		kEffectUnit.MakeProcess();
		this.RegistAdaptor(gameObject, kEffectUnit);
		switch (kEffectUnit.EffectType)
		{
		case eEFFECT_TYPE.INSTANT:
		case eEFFECT_TYPE.CHAREFFECT:
			if (gameObject.animation != null)
			{
				if (!gameObject.animation.enabled)
				{
					gameObject.animation.enabled = true;
				}
				gameObject.animation.Play();
			}
			break;
		case eEFFECT_TYPE.BULLET:
		{
			gameObject.SetActive(false);
			TrailRenderer componentInChildren = gameObject.GetComponentInChildren<TrailRenderer>();
			if (componentInChildren != null)
			{
				componentInChildren.enabled = false;
				kEffectUnit.m_TrailerRenderer = componentInChildren;
			}
			NkUtil.SetAllChildActive(gameObject, false);
			break;
		}
		}
		this.m_kEffectList.Add(kEffectUnit.RegistNum, kEffectUnit);
		if (kEffectUnit.LifeTime > 0f)
		{
			this.m_kLifeTimeEffectList.Add(kEffectUnit.RegistNum);
		}
	}

	private void _MakeEffect(NkEffectUnit kEffectUnit, GameObject goOriginal)
	{
		GameObject gameObject = this._GetEffectGameObject(kEffectUnit.EffectKind, goOriginal);
		if (null == gameObject)
		{
			Debug.LogError(kEffectUnit.EffectKind + "Make Fail:");
			return;
		}
		kEffectUnit.m_goEffect = gameObject;
		kEffectUnit.MakeProcess();
		this.RegistAdaptor(gameObject, kEffectUnit);
		switch (kEffectUnit.EffectType)
		{
		case eEFFECT_TYPE.INSTANT:
		case eEFFECT_TYPE.CHAREFFECT:
			if (gameObject.animation != null)
			{
				if (!gameObject.animation.enabled)
				{
					gameObject.animation.enabled = true;
				}
				gameObject.animation.Play();
			}
			break;
		case eEFFECT_TYPE.BULLET:
		{
			gameObject.SetActive(false);
			TrailRenderer componentInChildren = gameObject.GetComponentInChildren<TrailRenderer>();
			if (componentInChildren != null)
			{
				componentInChildren.enabled = false;
				kEffectUnit.m_TrailerRenderer = componentInChildren;
			}
			NkUtil.SetAllChildActive(gameObject, false);
			break;
		}
		}
		this.m_kEffectList.Add(kEffectUnit.RegistNum, kEffectUnit);
		if (kEffectUnit.LifeTime > 0f)
		{
			this.m_kLifeTimeEffectList.Add(kEffectUnit.RegistNum);
		}
	}

	private void RegistAdaptor(GameObject goEffect, NkEffectUnit kEffectUnit)
	{
		TsAnimationEventConnector componentInChildren = goEffect.GetComponentInChildren<TsAnimationEventConnector>();
		if (null != componentInChildren)
		{
			NkBattleChar casterChar = kEffectUnit.CasterChar;
			NrCharInfoAdaptor nrCharInfoAdaptor = NkUtil.GuarranteeComponent<NrCharInfoAdaptor>(goEffect);
			nrCharInfoAdaptor.SetCharInfoInterface(new NrCharInfoLogic(casterChar));
		}
	}

	public bool IsImmediateUse(string effectKind)
	{
		if (!this._IsImmediateMake(effectKind))
		{
			this.RequestEffectBundle(effectKind, false);
			return false;
		}
		return true;
	}

	private bool _IsImmediateMake(string effectKind)
	{
		return this.m_kEffectGameObject.ContainsKey(effectKind);
	}

	public NkEffectUnit GetEffectUnit(uint nEffectRegistNum)
	{
		if (!this.m_kEffectList.ContainsKey(nEffectRegistNum))
		{
			return null;
		}
		return this.m_kEffectList[nEffectRegistNum];
	}

	public void SetActiveEffect(uint nEffectRegistNum, bool bActive)
	{
		NkEffectUnit effectUnit = this.GetEffectUnit(nEffectRegistNum);
		if (effectUnit != null && effectUnit.m_goEffect != null)
		{
			effectUnit.m_goEffect.SetActive(bActive);
			Renderer[] componentsInChildren = effectUnit.m_goEffect.GetComponentsInChildren<Renderer>();
			if (componentsInChildren != null)
			{
				Renderer[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					Renderer renderer = array[i];
					renderer.enabled = bActive;
				}
			}
		}
	}

	public void DeleteEffect(string effectKind)
	{
		foreach (KeyValuePair<uint, NkEffectUnit> current in this.m_kEffectList)
		{
			if (current.Value.EffectKind.Equals(effectKind))
			{
				this._DeleteEffect(current.Value);
				this.m_kEffectList.Remove(current.Key);
				break;
			}
		}
	}

	public void DeleteEffectFromName(string szEffectName)
	{
		List<uint> list = new List<uint>();
		foreach (KeyValuePair<uint, NkEffectUnit> current in this.m_kEffectList)
		{
			if (!(current.Value.EffectName != szEffectName))
			{
				list.Add(current.Key);
			}
		}
		foreach (uint current2 in list)
		{
			this.DeleteEffect(current2);
		}
	}

	public void DeleteEffectFromKindAndTarget(string szEffectKind, string effectTarget)
	{
		List<uint> list = new List<uint>();
		foreach (KeyValuePair<uint, NkEffectUnit> current in this.m_kEffectList)
		{
			if (!(current.Value.EffectKind != szEffectKind))
			{
				if (!(current.Value.m_goParent == null))
				{
					if (current.Value.m_goParent.name.Contains(effectTarget))
					{
						list.Add(current.Key);
					}
				}
			}
		}
		foreach (uint current2 in list)
		{
			this.DeleteEffect(current2);
		}
	}

	public void DeleteEffect(GameObject ParentTarget)
	{
		if (ParentTarget == null)
		{
			return;
		}
		foreach (KeyValuePair<uint, NkEffectUnit> current in this.m_kEffectList)
		{
			if (!(null == current.Value.m_goParent))
			{
				if (current.Value.m_goParent == ParentTarget)
				{
					this._DeleteEffect(current.Value);
					this.m_kEffectList.Remove(current.Key);
					break;
				}
			}
		}
	}

	public void DeleteEffect(uint nEffectRegistNum)
	{
		NkEffectUnit effectUnit = this.GetEffectUnit(nEffectRegistNum);
		if (effectUnit != null)
		{
			this._DeleteEffect(effectUnit);
			this.m_kEffectList.Remove(nEffectRegistNum);
		}
	}

	public void DeleteAllEffect()
	{
		List<uint> list = new List<uint>();
		foreach (uint current in this.m_kEffectList.Keys)
		{
			list.Add(current);
		}
		foreach (uint current2 in list)
		{
			this.DeleteEffect(current2);
		}
	}

	private void _DeleteEffect(NkEffectUnit kEffectUnit)
	{
		if (kEffectUnit.m_goEffect)
		{
			UnityEngine.Object.Destroy(kEffectUnit.m_goEffect);
			kEffectUnit.m_goEffect = null;
		}
		if (kEffectUnit.DelCallBack != null)
		{
			kEffectUnit.DelCallBack();
		}
		this.m_kLifeTimeEffectList.Remove(kEffectUnit.RegistNum);
	}

	private void _AddReverseEffect(NkEffectUnit kEffectUnit)
	{
		if (!this.m_kReserveEffect.ContainsKey(kEffectUnit.EffectKind))
		{
			this.RequestEffectBundle(kEffectUnit.EffectKind, false);
		}
		List<NkEffectUnit> list = this.m_kReserveEffect[kEffectUnit.EffectKind];
		list.Add(kEffectUnit);
	}

	private void _ProcessReverseEffect(string effectKind)
	{
		if (!this.m_kReserveEffect.ContainsKey(effectKind))
		{
			return;
		}
		List<NkEffectUnit> list = null;
		if (!this.m_kReserveEffect.TryGetValue(effectKind, out list))
		{
			return;
		}
		foreach (NkEffectUnit current in list)
		{
			this._MakeEffect(current);
			NkEffectReservationInfo nkEffectReservationInfo = null;
			if (this.m_kEffectReservationInfo.TryGetValue(current.RegistNum, out nkEffectReservationInfo))
			{
				current.EffectName = nkEffectReservationInfo.m_szEffectName;
				if (nkEffectReservationInfo.m_fEndTime != 0f)
				{
					current.LifeTime = nkEffectReservationInfo.m_fEndTime;
				}
				current.SetRotate(nkEffectReservationInfo.m_fRotate);
				this.m_kEffectReservationInfo.Remove(current.RegistNum);
			}
		}
		list.Clear();
		this.m_kReserveEffect.Remove(effectKind);
	}

	private void _ProcessReverseEffect(string effectKind, GameObject goEffect)
	{
		List<NkEffectUnit> list = null;
		if (!this.m_kReserveEffect.TryGetValue(effectKind, out list))
		{
			return;
		}
		foreach (NkEffectUnit current in list)
		{
			this._MakeEffect(current, goEffect);
			NkEffectReservationInfo nkEffectReservationInfo = null;
			if (this.m_kEffectReservationInfo.TryGetValue(current.RegistNum, out nkEffectReservationInfo))
			{
				current.EffectName = nkEffectReservationInfo.m_szEffectName;
				if (nkEffectReservationInfo.m_fEndTime != 0f)
				{
					current.LifeTime = nkEffectReservationInfo.m_fEndTime;
				}
				current.SetRotate(nkEffectReservationInfo.m_fRotate);
				this.m_kEffectReservationInfo.Remove(current.RegistNum);
			}
		}
		list.Clear();
		this.m_kReserveEffect.Remove(effectKind);
	}

	public void SetReservationEffectData(uint nEffectRegisterNum, string szEffectName, float fEndTime, float fRotate)
	{
		if (!this.m_kEffectReservationInfo.ContainsKey(nEffectRegisterNum))
		{
			NkEffectReservationInfo nkEffectReservationInfo = new NkEffectReservationInfo();
			nkEffectReservationInfo.m_szEffectName = szEffectName;
			nkEffectReservationInfo.m_fEndTime = fEndTime;
			nkEffectReservationInfo.m_fRotate = fRotate;
			this.m_kEffectReservationInfo.Add(nEffectRegisterNum, nkEffectReservationInfo);
		}
	}

	public void Update()
	{
		if (this.m_kEffectList.Count == 0)
		{
			return;
		}
		if (this.m_kLifeTimeEffectList.Count > 0)
		{
			uint[] array = new uint[this.m_kLifeTimeEffectList.Count];
			this.m_kLifeTimeEffectList.CopyTo(array);
			uint[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				uint num = array2[i];
				NkEffectUnit effectUnit = this.GetEffectUnit(num);
				if (effectUnit == null)
				{
					this.m_kLifeTimeEffectList.Remove(num);
				}
				else if (effectUnit.m_goEffect == null || effectUnit.IsFinishedLifeTime() || effectUnit.MakeScene != Scene.CurScene)
				{
					if (effectUnit.EffectKind == "WARP")
					{
						NrTSingleton<NkClientLogic>.Instance.Warp(num);
					}
					this.DeleteEffect(num);
				}
			}
		}
	}

	public void SetSlowMotion()
	{
	}

	public void RestoreSlowMotion()
	{
	}

	public GameObject CreateEffectUI(string EffectKey, Vector2 ScreenPos, NkEffectUnit.DeleteCallBack DelCallBack)
	{
		GameObject gameObject = EffectDefine.Attach(string.Format("UI_{0}", EffectKey));
		this.AddEffect(EffectKey, gameObject, DelCallBack);
		gameObject.transform.position = NrTSingleton<UIDataManager>.Instance.GetEffectUIPos(ScreenPos);
		return gameObject;
	}

	public GameObject CreateEffectUI(string EffectKey, IUIObject UIObject)
	{
		GameObject gameObject = EffectDefine.Attach(string.Format("UI_{0}", EffectKey));
		NrTSingleton<UIDataManager>.Instance.EffectLocateUIPos(gameObject, UIObject);
		this.AddEffect(EffectKey, gameObject);
		return gameObject;
	}

	private void _RegisterEffectType()
	{
		this.m_kEffectTypeParse.InsertCodeValue("INSTANT", eEFFECT_TYPE.INSTANT);
		this.m_kEffectTypeParse.InsertCodeValue("CHAREFFECT", eEFFECT_TYPE.CHAREFFECT);
		this.m_kEffectTypeParse.InsertCodeValue("BULLET", eEFFECT_TYPE.BULLET);
		this.m_kEffectPosParse.InsertCodeValue("NONE", eEFFECT_POS.NONE);
		this.m_kEffectPosParse.InsertCodeValue("CENTER", eEFFECT_POS.CENTER);
		this.m_kEffectPosParse.InsertCodeValue("BOTTOM", eEFFECT_POS.BOTTOM);
		this.m_kEffectPosParse.InsertCodeValue("NAME", eEFFECT_POS.NAME);
		this.m_kEffectPosParse.InsertCodeValue("BONE", eEFFECT_POS.BONE);
		this.m_kEffectPosParse.InsertCodeValue("CENTERDM", eEFFECT_POS.CENTERDM);
	}

	public eEFFECT_TYPE ConvertEffectType(string strEffectType)
	{
		return this.m_kEffectTypeParse.GetValue(strEffectType);
	}

	public eEFFECT_POS ConvertEffectPos(string strEffectPos)
	{
		return this.m_kEffectPosParse.GetValue(strEffectPos);
	}

	public EFFECT_INFO GetEffectInfo(string effectKind)
	{
		if (this.m_kEffectInfoList.ContainsKey(effectKind))
		{
			return this.m_kEffectInfoList[effectKind];
		}
		return null;
	}

	public void ClearEffectCache()
	{
		if (this.m_kEffectGameObject != null)
		{
			foreach (GameObject current in this.m_kEffectGameObject.Values)
			{
				if (current != null)
				{
					UnityEngine.Object.Destroy(current);
				}
			}
			this.m_kEffectGameObject.Clear();
		}
		if (this.m_kEffectModelRoot != null)
		{
			UnityEngine.Object.Destroy(this.m_kEffectModelRoot);
		}
	}

	public void AddEffectInfo(EFFECT_INFO kEffectInfo)
	{
		if (this.m_kEffectInfoList.ContainsKey(kEffectInfo.EFFECT_KIND))
		{
			return;
		}
		this.m_kEffectInfoList.Add(kEffectInfo.EFFECT_KIND, kEffectInfo);
	}

	public static string FileName(string effectKind)
	{
		EFFECT_INFO effectInfo = NrTSingleton<NkEffectManager>.Instance.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return string.Empty;
		}
		return string.Format("Effect/{0}/{1}", effectInfo.EFFECT_TYPE.ToString(), effectInfo.BUNDLE_PATH);
	}

	public static WWWItem CreateWItem(string effectKind)
	{
		string text = NkEffectManager.FileName(effectKind);
		if (string.Empty.Equals(text))
		{
			return null;
		}
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(text + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		return wWWItem;
	}

	public bool RequestEffectBundle(string effectKind, bool bForceReserve)
	{
		if (this.m_kEffectGameObject.ContainsKey(effectKind))
		{
			return true;
		}
		if (this.m_kReserveEffect.ContainsKey(effectKind))
		{
			return true;
		}
		if (this._IsImmediateMake(effectKind))
		{
			return true;
		}
		EFFECT_INFO effectInfo = this.GetEffectInfo(effectKind);
		if (effectInfo == null)
		{
			return false;
		}
		string path = string.Format("Effect/{0}/{1}", effectInfo.EFFECT_TYPE.ToString(), effectInfo.BUNDLE_PATH);
		NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(path, NkBundleCallBack.EffectBundleStackName, new NkBundleParam.funcParamBundleCallBack(this._EffectBundleDownloadCompleted), effectInfo, true);
		this.m_kReserveEffect.Add(effectKind, new List<NkEffectUnit>());
		if (bForceReserve && !this.m_liForceReserve.Contains(effectKind))
		{
			this.m_liForceReserve.Add(effectKind);
		}
		return true;
	}

	private void _EffectBundleDownloadCompleted(ref IDownloadedItem wItem, object obj)
	{
		if (wItem.isCanceled)
		{
			return;
		}
		if (!wItem.canAccessAssetBundle)
		{
			return;
		}
		EFFECT_INFO eFFECT_INFO = obj as EFFECT_INFO;
		if (eFFECT_INFO == null)
		{
			return;
		}
		GameObject gameObject = wItem.mainAsset as GameObject;
		if (gameObject == null)
		{
			return;
		}
		gameObject.SetActive(false);
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
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref gameObject);
		}
		bool flag = this.m_liForceReserve.Contains(eFFECT_INFO.EFFECT_KIND);
		if (this.bUseEffectCache || flag)
		{
			this._RegisterEffectGameObject(gameObject, eFFECT_INFO.EFFECT_KIND);
			this._ProcessReverseEffect(eFFECT_INFO.EFFECT_KIND);
		}
		else
		{
			this._ProcessReverseEffect(eFFECT_INFO.EFFECT_KIND, gameObject);
		}
		if (flag)
		{
			this.m_liForceReserve.Remove(eFFECT_INFO.EFFECT_KIND);
		}
	}

	private void _RegisterEffectGameObject(GameObject originalEffect, string effectKind)
	{
		if (originalEffect == null)
		{
			return;
		}
		if (this.m_kEffectModelRoot == null)
		{
			this.m_kEffectModelRoot = GameObject.Find("@internal effects");
			if (this.m_kEffectModelRoot == null)
			{
				this.m_kEffectModelRoot = new GameObject("@internal effects");
				UnityEngine.Object.DontDestroyOnLoad(this.m_kEffectModelRoot);
			}
		}
		if (this.m_kEffectGameObject.ContainsKey(effectKind))
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(originalEffect) as GameObject;
		if (gameObject == null)
		{
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		try
		{
			gameObject.name = "@" + effectKind;
			gameObject.transform.parent = this.m_kEffectModelRoot.transform;
			NkUtil.SetAllChildActive(gameObject, false);
			this.m_kEffectGameObject.Add(effectKind, gameObject);
		}
		catch (ArgumentException ex)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Already registered effect key : key=",
				effectKind,
				" (",
				ex,
				")"
			}));
		}
	}

	private void RemoveRegisterEffect(string effectKind)
	{
		if (this.m_kEffectModelRoot == null)
		{
			return;
		}
		if (!this.m_kEffectGameObject.ContainsKey(effectKind))
		{
			return;
		}
		GameObject obj = this.m_kEffectGameObject[effectKind];
		UnityEngine.Object.Destroy(obj);
		this.m_kEffectGameObject.Remove(effectKind);
	}

	private GameObject _GetEffectGameObject(string effectKind)
	{
		if (!this.m_kEffectGameObject.ContainsKey(effectKind))
		{
			return null;
		}
		GameObject gameObject = this.m_kEffectGameObject[effectKind];
		if (null == gameObject)
		{
			return null;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject;
		if (null == gameObject2)
		{
			return null;
		}
		gameObject2.name = effectKind;
		gameObject2.transform.rotation = gameObject.transform.rotation;
		gameObject2.transform.localScale = gameObject.transform.localScale;
		NkUtil.SetAllChildActive(gameObject2, true);
		return gameObject2;
	}

	private GameObject _GetEffectGameObject(string effectKind, GameObject goOriginal)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(goOriginal, Vector3.zero, Quaternion.identity) as GameObject;
		if (null == gameObject)
		{
			return null;
		}
		gameObject.name = effectKind;
		gameObject.transform.rotation = goOriginal.transform.rotation;
		gameObject.transform.localScale = goOriginal.transform.localScale;
		NkUtil.SetAllChildActive(gameObject, true);
		return gameObject;
	}

	public bool isEffectLimit(string effectKind)
	{
		return false;
	}
}
