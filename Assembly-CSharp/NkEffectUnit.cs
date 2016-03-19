using Ndoors.Framework.Stage;
using System;
using UnityEngine;

public class NkEffectUnit
{
	public delegate void DeleteCallBack();

	private EFFECT_INFO m_kEffectInfo;

	private uint m_nRegistNum;

	private eEFFECT_TARGET m_eEffectTarget;

	public GameObject m_goParent;

	public Vector3 m_v3Target = Vector3.zero;

	public TrailRenderer m_TrailerRenderer;

	public GameObject m_goEffect;

	private NkBattleChar m_CasterChar;

	private float m_fScale = 1f;

	private float m_fStartTime;

	private float m_fLifeTime;

	private Scene.Type m_eMakeScene;

	private bool m_CheckScale = true;

	private NkEffectUnit.DeleteCallBack m_pkDeleteCallBack;

	private string szEffectName = string.Empty;

	public NkEffectUnit.DeleteCallBack DelCallBack
	{
		get
		{
			return this.m_pkDeleteCallBack;
		}
		set
		{
			this.m_pkDeleteCallBack = value;
		}
	}

	public string EffectKind
	{
		get
		{
			return this.m_kEffectInfo.EFFECT_KIND;
		}
		set
		{
		}
	}

	public eEFFECT_TYPE EffectType
	{
		get
		{
			return this.m_kEffectInfo.EFFECT_TYPE;
		}
		set
		{
		}
	}

	public float LifeTime
	{
		get
		{
			return this.m_fLifeTime;
		}
		set
		{
			this.m_fLifeTime = value;
		}
	}

	public float StartTime
	{
		get
		{
			return this.m_fStartTime;
		}
		set
		{
		}
	}

	public eEFFECT_TARGET EffectTarget
	{
		get
		{
			return this.m_eEffectTarget;
		}
		set
		{
		}
	}

	public NkBattleChar CasterChar
	{
		get
		{
			return this.m_CasterChar;
		}
		set
		{
		}
	}

	public uint RegistNum
	{
		get
		{
			return this.m_nRegistNum;
		}
		set
		{
			this.m_nRegistNum = value;
		}
	}

	public float Scale
	{
		get
		{
			return this.m_fScale;
		}
		set
		{
			this.m_fScale = value;
		}
	}

	public string EffectName
	{
		get
		{
			return this.szEffectName;
		}
		set
		{
			this.szEffectName = value;
		}
	}

	public Scene.Type MakeScene
	{
		get
		{
			return this.m_eMakeScene;
		}
		set
		{
			this.m_eMakeScene = value;
		}
	}

	public NkEffectUnit(EFFECT_INFO effectInfo, NkBattleChar OrderChar)
	{
		GameObject gameObject = null;
		Transform transform = null;
		Nr3DCharBase nr3DCharBase = OrderChar.Get3DChar();
		if (nr3DCharBase != null)
		{
			gameObject = nr3DCharBase.GetRootGameObject();
			transform = nr3DCharBase.GetEffectPos(effectInfo.EFFECT_POS);
		}
		Vector3 v3Target = Vector3.zero;
		if (null != transform)
		{
			v3Target = transform.position;
		}
		if (effectInfo.EFFECT_POS == eEFFECT_POS.BONE && nr3DCharBase != null)
		{
			gameObject = nr3DCharBase.GetBoneRootObject();
			if (gameObject == null)
			{
				gameObject = nr3DCharBase.GetRootGameObject();
			}
			v3Target = Vector3.zero;
			v3Target.y = -1f;
		}
		this.m_CasterChar = OrderChar;
		this.InitBase(eEFFECT_TARGET.POSITION, effectInfo, gameObject, v3Target);
	}

	public NkEffectUnit(EFFECT_INFO effectInfo, NkBattleChar OrderChar, bool bAttachEffectPos, bool CheckScale)
	{
		GameObject gameObject = null;
		Nr3DCharBase nr3DCharBase = OrderChar.Get3DChar();
		if (nr3DCharBase != null)
		{
			gameObject = nr3DCharBase.GetRootGameObject();
		}
		Vector3 v3Target = Vector3.zero;
		Transform effectPos = nr3DCharBase.GetEffectPos(effectInfo.EFFECT_POS);
		if (null != effectPos)
		{
			v3Target = effectPos.position;
		}
		if (effectInfo.EFFECT_POS == eEFFECT_POS.BONE && nr3DCharBase != null)
		{
			gameObject = nr3DCharBase.GetBoneRootObject();
			if (gameObject == null)
			{
				gameObject = nr3DCharBase.GetRootGameObject();
			}
			v3Target = Vector3.zero;
			v3Target.y = -1f;
		}
		this.m_CasterChar = OrderChar;
		if (bAttachEffectPos && effectPos != null)
		{
			gameObject = effectPos.gameObject;
			v3Target = Vector3.zero;
		}
		this.m_CheckScale = CheckScale;
		this.InitBase(eEFFECT_TARGET.GAMEOBJECT, effectInfo, gameObject, v3Target);
	}

	public NkEffectUnit(EFFECT_INFO effectInfo, GameObject goParent, Vector3 v3Target)
	{
		this.InitBase(eEFFECT_TARGET.POSITION, effectInfo, goParent, v3Target);
	}

	public NkEffectUnit(EFFECT_INFO effectInfo, Vector3 v3Target)
	{
		this.InitBase(eEFFECT_TARGET.POSITION, effectInfo, null, v3Target);
	}

	public NkEffectUnit(EFFECT_INFO effectInfo, GameObject goTarget)
	{
		this.InitBase(eEFFECT_TARGET.GAMEOBJECT, effectInfo, goTarget, Vector3.zero);
	}

	public NkEffectUnit(EFFECT_INFO effectInfo, NkBattleChar OrderChar, Vector3 v3CenterTarget)
	{
		GameObject goParent = null;
		Nr3DCharBase nr3DCharBase = OrderChar.Get3DChar();
		if (nr3DCharBase != null)
		{
			goParent = nr3DCharBase.GetRootGameObject();
		}
		Transform effectPos = nr3DCharBase.GetEffectPos(effectInfo.EFFECT_POS);
		if (null != effectPos)
		{
			v3CenterTarget.y = effectPos.position.y;
		}
		this.m_CasterChar = OrderChar;
		this.InitBase(eEFFECT_TARGET.POSITION, effectInfo, goParent, v3CenterTarget);
	}

	private void InitBase(eEFFECT_TARGET eType, EFFECT_INFO effectInfo, GameObject goParent, Vector3 v3Target)
	{
		this.m_eEffectTarget = eType;
		this.m_kEffectInfo = effectInfo;
		if (this.m_CasterChar != null)
		{
			Nr3DCharBase nr3DCharBase = this.m_CasterChar.Get3DChar();
			if (nr3DCharBase != null)
			{
				this.m_fScale = this.m_kEffectInfo.SCALE * nr3DCharBase.GetDiffCharScale();
			}
			else
			{
				this.m_fScale = this.m_kEffectInfo.SCALE;
			}
		}
		else
		{
			this.m_fScale = this.m_kEffectInfo.SCALE;
		}
		if (this.m_fScale < this.m_kEffectInfo.SCALE)
		{
			this.m_fScale = this.m_kEffectInfo.SCALE;
		}
		this.m_fStartTime = Time.time;
		this.m_goParent = goParent;
		this.m_v3Target = v3Target + this.m_kEffectInfo.DIFFPOS;
		this.m_fLifeTime = this.m_kEffectInfo.LIFE_TIME;
		this.m_eMakeScene = Scene.CurScene;
	}

	public bool IsFinishedLifeTime()
	{
		return this.LifeTime != float.PositiveInfinity && (this.LifeTime > 0f && Time.time - this.StartTime > this.LifeTime);
	}

	public void MakeProcess()
	{
		Vector3 localScale = this.m_goEffect.transform.localScale;
		float num = 0f;
		if (this.m_goParent != null)
		{
			this.m_goEffect.transform.parent = this.m_goParent.transform;
			this.m_goEffect.transform.rotation = this.m_goParent.transform.rotation;
			if (this.m_CheckScale && this.m_goParent.transform.localScale.x < 1f)
			{
				num = (1f - this.m_goParent.transform.localScale.x) * 3f;
			}
		}
		this.m_goEffect.transform.position = this.m_v3Target;
		if (this.m_CheckScale)
		{
			this.m_goEffect.transform.localScale = localScale * (this.m_fScale + num);
		}
		if (this.m_eEffectTarget == eEFFECT_TARGET.GAMEOBJECT)
		{
			this.m_goEffect.transform.localPosition = Vector3.zero;
			this.m_goEffect.transform.localRotation = Quaternion.identity;
		}
		if (this.m_kEffectInfo.EFFECT_POS == eEFFECT_POS.BONE)
		{
			this.m_goEffect.transform.localPosition = this.m_v3Target;
		}
	}

	public void SetRotate(float yrotate)
	{
		this.m_goEffect.transform.rotation = Quaternion.AngleAxis(yrotate, Vector3.up);
	}

	public void SetShow(bool bShow)
	{
		if (null != this.m_goEffect)
		{
			NkUtil.SetAllChildActive(this.m_goEffect, bShow);
		}
	}

	public void SetDeleteCallBack(NkEffectUnit.DeleteCallBack callBack)
	{
		this.m_pkDeleteCallBack = callBack;
	}
}
