using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Behavior_EffectTarget : EventTriggerItem_Behavior
{
	[SerializeField]
	public string _effectKind;

	[SerializeField]
	public string _charKind;

	[SerializeField]
	public float _effectDuration = -1f;

	[SerializeField]
	public Vector3 _effectControllPos = Vector3.zero;

	private float _effectDurationDefault = -1f;

	private bool _endEffectBehaviour;

	public override void Init()
	{
		this._endEffectBehaviour = false;
	}

	public override bool Excute()
	{
		if (this.m_Excute)
		{
			return !this._endEffectBehaviour;
		}
		this.m_Excute = true;
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
		return string.Concat(new string[]
		{
			" 대상 Char : ",
			this._charKind,
			" 에  ",
			this._effectKind,
			" Effect 를 터트려준다."
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
		return !string.IsNullOrEmpty(this._effectKind) && !string.IsNullOrEmpty(this._charKind);
	}

	private List<GameObject> GetGameObjectInCurrentSceneType(string name)
	{
		List<GameObject> sceneObjs = this.FindCurrentSceneTypeObjs();
		return this.FindObjsByName(sceneObjs, name);
	}

	private GameObject FindGameObject(string name)
	{
		GameObject gameObject = GameObject.Find(name);
		if (gameObject == null)
		{
			UnityEngine.Debug.LogError("ERROR,  Behavior_Test.cs, FindGameObject(), GameObject is Null : " + name);
			return null;
		}
		return gameObject;
	}

	private List<GameObject> FindCurrentSceneTypeObjs()
	{
		string name = TsSceneSwitcher.Instance.CurrentSceneType.ToString();
		GameObject gameObject = this.FindGameObject(name);
		TsSceneSwitcherMark[] componentsInChildren = gameObject.transform.GetComponentsInChildren<TsSceneSwitcherMark>();
		if (componentsInChildren == null)
		{
			UnityEngine.Debug.LogError("ERROR,  Behavior_Test.cs, FindCurrentSceneTypeObjs(), sceneObjs is Null : ");
			return null;
		}
		List<GameObject> list = new List<GameObject>();
		TsSceneSwitcherMark[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			TsSceneSwitcherMark tsSceneSwitcherMark = array[i];
			list.Add(tsSceneSwitcherMark.gameObject);
		}
		return list;
	}

	private List<GameObject> FindObjsByName(List<GameObject> sceneObjs, string objName)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (GameObject current in sceneObjs)
		{
			List<GameObject> list2 = this.FindInChilderen(current, objName);
			if (list2 != null)
			{
				list.InsertRange(list.Count, list2);
			}
		}
		return list;
	}

	private List<GameObject> FindInChilderen(GameObject obj, string objName)
	{
		if (obj == null)
		{
			UnityEngine.Debug.LogError("ERROR,  Behavior_Test.cs, FindInChilderen(), obj is Null : ");
			return null;
		}
		List<GameObject> list = new List<GameObject>();
		int childCount = obj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = obj.transform.GetChild(i);
			if (!(child == null))
			{
				if (child.name.Contains(objName))
				{
					list.Add(child.gameObject);
				}
			}
		}
		return list;
	}

	private void PlayEffect()
	{
		List<GameObject> gameObjectInCurrentSceneType = this.GetGameObjectInCurrentSceneType(this._charKind);
		if (gameObjectInCurrentSceneType == null)
		{
			return;
		}
		foreach (GameObject current in gameObjectInCurrentSceneType)
		{
			uint nEffectRegistNum = NrTSingleton<NkEffectManager>.Instance.AddEffect(this._effectKind, current);
			NkEffectUnit effectUnit = NrTSingleton<NkEffectManager>.Instance.GetEffectUnit(nEffectRegistNum);
			if (effectUnit != null && !(effectUnit.m_goEffect == null))
			{
				effectUnit.m_goEffect.transform.localPosition = this._effectControllPos;
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator ReserveDeleteEffect()
	{
		Behavior_EffectTarget.<ReserveDeleteEffect>c__Iterator0 <ReserveDeleteEffect>c__Iterator = new Behavior_EffectTarget.<ReserveDeleteEffect>c__Iterator0();
		<ReserveDeleteEffect>c__Iterator.<>f__this = this;
		return <ReserveDeleteEffect>c__Iterator;
	}
}
