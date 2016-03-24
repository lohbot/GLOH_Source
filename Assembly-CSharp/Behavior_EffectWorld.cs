using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Behavior_EffectWorld : EventTriggerItem_Behavior
{
	[SerializeField]
	public string _effectKind;

	[SerializeField]
	public float _effectDuration = -1f;

	[SerializeField]
	public Vector3 _effectWorldPos = Vector3.zero;

	private float _effectDurationDefault = -1f;

	private bool _endEffectBehaviour;

	private string _worldEffectRootName = "WorldEffectRoot";

	private GameObject _worldEffectRootObj;

	private GameObject _playEffect;

	public override void Init()
	{
		this._endEffectBehaviour = false;
		this._worldEffectRootName = "WorldEffectRoot";
		this._worldEffectRootObj = null;
		this._playEffect = null;
	}

	public override bool Excute()
	{
		if (this.m_Excute)
		{
			return !this._endEffectBehaviour;
		}
		this.m_Excute = true;
		this.CreateWorldEffectRoot();
		this.PlayEffect();
		base.StartCoroutine(this.ReserveDeleteEffect());
		return !this._endEffectBehaviour;
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		return string.Concat(new object[]
		{
			"WorldPosition : ",
			this._effectWorldPos,
			" 에  ",
			this._effectKind,
			" Effect 를 ",
			this._effectDuration,
			" 초 동안 터트려준다."
		});
	}

	public override float ExcuteTiemSecond()
	{
		return 0f;
	}

	public override void Draw()
	{
	}

	public override Behavior._BEHAVIORTYPE GetBehaviorType()
	{
		return Behavior._BEHAVIORTYPE.DRAMA;
	}

	public override bool IsVaildValue()
	{
		return !string.IsNullOrEmpty(this._effectKind);
	}

	public override void OnDisableTrigger()
	{
		this.DeleteEffect();
	}

	private void CreateWorldEffectRoot()
	{
		this._worldEffectRootObj = this.FindGameObject(this._worldEffectRootName);
		if (this._worldEffectRootObj != null)
		{
			return;
		}
		this._worldEffectRootObj = new GameObject(this._worldEffectRootName);
		string name = TsSceneSwitcher.Instance.CurrentSceneType.ToString();
		GameObject gameObject = this.FindGameObject(name);
		this._worldEffectRootObj.transform.parent = gameObject.transform;
	}

	private GameObject FindGameObject(string name)
	{
		GameObject gameObject = GameObject.Find(name);
		if (gameObject == null)
		{
			UnityEngine.Debug.Log("NORMAL,  Behavior_EffectWorld.cs, FindGameObject(), GameObject is Null : " + name);
			return null;
		}
		return gameObject;
	}

	private void PlayEffect()
	{
		this._playEffect = new GameObject("effect");
		this._playEffect.transform.parent = this._worldEffectRootObj.transform;
		this._playEffect.transform.position = this._effectWorldPos;
		NrTSingleton<NkEffectManager>.Instance.AddEffect(this._effectKind, this._playEffect);
	}

	[DebuggerHidden]
	private IEnumerator ReserveDeleteEffect()
	{
		Behavior_EffectWorld.<ReserveDeleteEffect>c__Iterator1 <ReserveDeleteEffect>c__Iterator = new Behavior_EffectWorld.<ReserveDeleteEffect>c__Iterator1();
		<ReserveDeleteEffect>c__Iterator.<>f__this = this;
		return <ReserveDeleteEffect>c__Iterator;
	}

	private void DeleteEffect()
	{
		if (this._playEffect == null)
		{
			UnityEngine.Debug.Log("NORMAL, Behavior_EffectWorld.cs, DeleteEffect(), _playEffect is Null");
			return;
		}
		NrTSingleton<NkEffectManager>.Instance.DeleteEffectFromKindAndTarget(this._effectKind, this._playEffect.name);
		UnityEngine.Object.Destroy(this._playEffect);
		this._endEffectBehaviour = true;
	}
}
