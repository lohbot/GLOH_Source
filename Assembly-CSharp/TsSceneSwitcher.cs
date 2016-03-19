using System;
using System.Collections.Generic;
using UnityEngine;

public class TsSceneSwitcher : MonoBehaviour
{
	public class SwitchData
	{
		public enum ERender
		{
			Not_Have,
			Enable,
			Disable
		}

		public enum EAudio
		{
			Not_Have,
			Playing,
			Stop
		}

		public enum EAnimation
		{
			Not_Have,
			Playing,
			Stop
		}

		private GameObject _targetGO;

		public GameObject TargetGO
		{
			get
			{
				return this._targetGO;
			}
			set
			{
				this._targetGO = value;
				if (this._targetGO != null)
				{
					this.TargetGOName = this._targetGO.name;
				}
			}
		}

		public string TargetGOName
		{
			get;
			set;
		}

		public TsSceneSwitcher.SwitchData.ERender EnableRender
		{
			get;
			set;
		}

		public TsSceneSwitcher.SwitchData.EAudio IsPlayingAudio
		{
			get;
			set;
		}

		public TsSceneSwitcher.SwitchData.EAnimation IsPlayingAnimation
		{
			get;
			set;
		}

		public override string ToString()
		{
			return string.Format("TargetGO[{0}] Render[{1}] Audio[{2}]", this.TargetGOName, this.EnableRender, this.IsPlayingAudio);
		}
	}

	public class _SwitchDataList : List<TsSceneSwitcher.SwitchData>
	{
	}

	public enum ESceneType
	{
		None,
		WorldScene,
		BattleScene,
		SoldierBatchScene,
		TOTAL
	}

	private const string CheckSound = "SOUND";

	private Dictionary<GameObject, TsSceneSwitcher._SwitchDataList> _switchDataDic = new Dictionary<GameObject, TsSceneSwitcher._SwitchDataList>();

	private static TsSceneSwitcher _instance;

	public IDictionary<GameObject, TsSceneSwitcher._SwitchDataList> _InternalOnly_SwitchDataDic
	{
		get
		{
			return this._switchDataDic;
		}
	}

	public static TsSceneSwitcher Instance
	{
		get
		{
			if (TsSceneSwitcher._instance == null)
			{
				GameObject gameObject = GameObject.Find("@SceneSwitcher");
				if (gameObject == null)
				{
					gameObject = new GameObject("@SceneSwitcher");
					gameObject.AddComponent<TsSceneSwitcher>();
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				TsSceneSwitcher._instance = gameObject.GetComponent<TsSceneSwitcher>();
			}
			return TsSceneSwitcher._instance;
		}
	}

	public TsSceneSwitcher.ESceneType CurrentSceneType
	{
		get;
		private set;
	}

	public GameObject _GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType eSceneType)
	{
		foreach (GameObject current in this._switchDataDic.Keys)
		{
			if (current.name.Equals(eSceneType.ToString()))
			{
				return current;
			}
		}
		return null;
	}

	public GameObject _GetBundle_RootSceneGO(TsSceneSwitcher.ESceneType eSceneType)
	{
		string text = string.Empty;
		switch (eSceneType)
		{
		case TsSceneSwitcher.ESceneType.WorldScene:
			text = "World";
			break;
		case TsSceneSwitcher.ESceneType.BattleScene:
			text = "Battle";
			break;
		case TsSceneSwitcher.ESceneType.SoldierBatchScene:
			text = "SoldierBatch";
			break;
		}
		if (text.Length == 0)
		{
			return null;
		}
		return GameObject.Find(text);
	}

	private TsSceneSwitcher._SwitchDataList _GetSwitchData_List(TsSceneSwitcher.ESceneType eSceneType)
	{
		foreach (KeyValuePair<GameObject, TsSceneSwitcher._SwitchDataList> current in this._switchDataDic)
		{
			if (current.Key.name.Equals(eSceneType.ToString()))
			{
				return current.Value;
			}
		}
		return null;
	}

	public void DeleteScene(TsSceneSwitcher.ESceneType eSceneType)
	{
		GameObject gameObject = this._GetSwitchData_RootSceneGO(eSceneType);
		if (gameObject != null)
		{
			TsSceneSwitcher._SwitchDataList switchDataList = this._GetSwitchData_List(eSceneType);
			if (switchDataList == null)
			{
				TsLog.LogError("Cannot found~! key= " + eSceneType, new object[0]);
				return;
			}
			switchDataList.Clear();
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				Transform child = gameObject.transform.GetChild(i);
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
	}

	public void ClearAllScene()
	{
		this.DeleteScene(TsSceneSwitcher.ESceneType.WorldScene);
		this.DeleteScene(TsSceneSwitcher.ESceneType.BattleScene);
		this.DeleteScene(TsSceneSwitcher.ESceneType.SoldierBatchScene);
	}

	private GameObject _MakeRootSceneGO(TsSceneSwitcher.ESceneType eSceneType)
	{
		GameObject gameObject = this._GetSwitchData_RootSceneGO(eSceneType);
		if (gameObject == null)
		{
			gameObject = new GameObject(eSceneType.ToString());
			TsSceneSwitcherMark tsSceneSwitcherMark = this._AddOrGetComponent(gameObject, eSceneType);
			tsSceneSwitcherMark.RootGOName = eSceneType.ToString();
			tsSceneSwitcherMark.IsCollected = true;
			TsSceneSwitcher._SwitchDataList value = new TsSceneSwitcher._SwitchDataList();
			this._switchDataDic.Add(gameObject, value);
			gameObject.transform.parent = TsSceneSwitcher.Instance.gameObject.transform;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		return gameObject;
	}

	public static bool IsValidSceneType(TsSceneSwitcher.ESceneType eSceneType)
	{
		return eSceneType != TsSceneSwitcher.ESceneType.None && TsSceneSwitcher.ESceneType.TOTAL > eSceneType;
	}

	public void CollectAllMapGameObjects(TsSceneSwitcher.ESceneType eSceneType, bool isActive)
	{
		if (!TsSceneSwitcher.IsValidSceneType(eSceneType))
		{
			TsLog.LogWarning("Invalid SceneType= " + eSceneType, new object[0]);
			return;
		}
		GameObject rootGO = this._MakeRootSceneGO(eSceneType);
		GameObject gameObject = this._GetBundle_RootSceneGO(eSceneType);
		if (gameObject == null)
		{
			return;
		}
		this._TraverseToSetMarkAndCollect(rootGO, gameObject, eSceneType);
	}

	private void _TraverseToSetMarkAndCollect(GameObject rootGO, GameObject go, TsSceneSwitcher.ESceneType eSceneType)
	{
		this._Collect(go, rootGO, eSceneType, true);
		for (int i = 0; i < go.transform.childCount; i++)
		{
			Transform child = go.transform.GetChild(i);
			GameObject gameObject = child.gameObject;
			if (gameObject.name.Contains("SOUND"))
			{
				this._AddOrGetComponent(gameObject, eSceneType);
				this._TraverseToSetMarkAndCollect(rootGO, gameObject, eSceneType);
			}
		}
	}

	private TsSceneSwitcherMark _AddOrGetComponent(GameObject go, TsSceneSwitcher.ESceneType eSceneType)
	{
		if (go == null)
		{
			return null;
		}
		TsSceneSwitcherMark tsSceneSwitcherMark = go.GetComponent<TsSceneSwitcherMark>();
		if (tsSceneSwitcherMark == null)
		{
			tsSceneSwitcherMark = go.AddComponent<TsSceneSwitcherMark>();
		}
		tsSceneSwitcherMark.SceneType = eSceneType;
		return tsSceneSwitcherMark;
	}

	public void Collect(TsSceneSwitcher.ESceneType eSceneType, GameObject go)
	{
		if (!TsSceneSwitcher.IsValidSceneType(eSceneType) || go == null)
		{
			TsLog.LogWarning("Invalid SceneType[{0}] go[{1}]", new object[]
			{
				eSceneType,
				(!(go == null)) ? go.name : "null"
			});
			return;
		}
		GameObject gameObject = this._MakeRootSceneGO(eSceneType);
		this._AddOrGetComponent(go, eSceneType);
		this._Collect(go, gameObject, eSceneType, gameObject.activeInHierarchy);
	}

	private void _Collect(GameObject go, GameObject rootGO, TsSceneSwitcher.ESceneType eSceneType, bool isActive)
	{
		if (go == null || rootGO == null)
		{
			TsLog.LogError("Cannot be null~!!!!! ", new object[0]);
			return;
		}
		TsSceneSwitcherMark tsSceneSwitcherMark = go.GetComponent<TsSceneSwitcherMark>();
		if (tsSceneSwitcherMark == null)
		{
			tsSceneSwitcherMark = this._AddOrGetComponent(go, eSceneType);
			if (tsSceneSwitcherMark == null)
			{
				TsLog.LogError("Must Added~! Check logic~!", new object[0]);
				return;
			}
		}
		if (tsSceneSwitcherMark.IsCollected)
		{
			return;
		}
		if (go.transform.parent == null)
		{
			go.transform.parent = rootGO.transform;
		}
		TsSceneSwitcher._SwitchDataList switchDataList = this._GetSwitchData_List(eSceneType);
		if (switchDataList == null)
		{
			TsLog.LogError("Cannot found~! key= " + rootGO.name, new object[0]);
			return;
		}
		TsSceneSwitcher.SwitchData switchData = this._MakeSwitchData(tsSceneSwitcherMark, rootGO.name);
		if (switchData != null)
		{
			tsSceneSwitcherMark.RootGOName = rootGO.name;
			tsSceneSwitcherMark.IsCollected = true;
			tsSceneSwitcherMark.RefSwitchData = switchData;
			switchDataList.Add(switchData);
		}
		Light[] componentsInChildren = go.GetComponentsInChildren<Light>();
		if (componentsInChildren != null)
		{
			Light[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Light light = array[i];
				TsLayerMask layerMask = new TsLayerMask((uint)light.cullingMask);
				light.cullingMask = layerMask - TsLayer.GUI;
			}
		}
		go.SetActive(isActive);
	}

	private void _SetSwitchData(GameObject go, TsSceneSwitcher.SwitchData switchData)
	{
		switchData.TargetGO = go;
		if (go.renderer != null)
		{
			switchData.EnableRender = ((!go.renderer.enabled) ? TsSceneSwitcher.SwitchData.ERender.Disable : TsSceneSwitcher.SwitchData.ERender.Enable);
		}
		if (go.audio != null)
		{
			TsAudioAdapter component = switchData.TargetGO.GetComponent<TsAudioAdapter>();
			if (component != null)
			{
				switchData.IsPlayingAudio = ((!component.GetAudioEx().PlayOnDownload) ? TsSceneSwitcher.SwitchData.EAudio.Stop : TsSceneSwitcher.SwitchData.EAudio.Playing);
				component.GetAudioEx().Stop();
			}
		}
		if (go.animation != null)
		{
			Animation animation = switchData.TargetGO.animation;
			if (animation != null)
			{
				switchData.IsPlayingAnimation = ((!animation.playAutomatically || !(animation.clip != null)) ? TsSceneSwitcher.SwitchData.EAnimation.Stop : TsSceneSwitcher.SwitchData.EAnimation.Playing);
				animation.Stop();
			}
		}
	}

	private TsSceneSwitcher.SwitchData _MakeSwitchData(TsSceneSwitcherMark mark, string rootSceneGOName)
	{
		TsSceneSwitcher.SwitchData switchData = new TsSceneSwitcher.SwitchData();
		this._SetSwitchData(mark.gameObject, switchData);
		return switchData;
	}

	public void Switch(TsSceneSwitcher.ESceneType eSceneType)
	{
		if (!TsSceneSwitcher.IsValidSceneType(eSceneType))
		{
			TsLog.LogWarning("Invalid SceneType= " + eSceneType, new object[0]);
			return;
		}
		GameObject x = this._GetSwitchData_RootSceneGO(eSceneType);
		if (x == null)
		{
			return;
		}
		foreach (KeyValuePair<GameObject, TsSceneSwitcher._SwitchDataList> current in this._switchDataDic)
		{
			bool flag = false;
			if (x == current.Key)
			{
				flag = true;
			}
			foreach (TsSceneSwitcher.SwitchData current2 in current.Value)
			{
				try
				{
					if (current2.TargetGO)
					{
						this._SetSwitchData(current2.TargetGO, current2);
						NkUtil.SetChildActiveExceptChar(current2.TargetGO, flag);
						if (current2.TargetGO.activeInHierarchy)
						{
							if (current2.EnableRender != TsSceneSwitcher.SwitchData.ERender.Not_Have)
							{
								current2.TargetGO.renderer.enabled = (current2.EnableRender == TsSceneSwitcher.SwitchData.ERender.Enable);
							}
							if (current2.IsPlayingAudio != TsSceneSwitcher.SwitchData.EAudio.Not_Have && current2.IsPlayingAudio == TsSceneSwitcher.SwitchData.EAudio.Playing)
							{
								TsAudioAdapter component = current2.TargetGO.GetComponent<TsAudioAdapter>();
								if (component != null)
								{
									component.GetAudioEx().RestoreToPlay();
								}
							}
							if (current2.IsPlayingAnimation != TsSceneSwitcher.SwitchData.EAnimation.Not_Have && current2.IsPlayingAnimation == TsSceneSwitcher.SwitchData.EAnimation.Playing)
							{
								Animation animation = current2.TargetGO.animation;
								if (null != animation)
								{
									AnimationState animationState = current2.TargetGO.animation[animation.clip.name];
									if (null != animationState)
									{
										float time = UnityEngine.Random.Range(0f, animationState.length);
										animationState.time = time;
										animation.Play();
									}
								}
							}
							Terrain componentInChildren = current2.TargetGO.GetComponentInChildren<Terrain>();
							if (componentInChildren != null)
							{
								componentInChildren.enabled = flag;
							}
							Camera componentInChildren2 = current2.TargetGO.GetComponentInChildren<Camera>();
							if (componentInChildren2 != null)
							{
								componentInChildren2.enabled = flag;
							}
						}
					}
				}
				catch (Exception ex)
				{
					TsLog.LogError("DataName= " + current2.TargetGOName + "   Exception= " + ex.ToString(), new object[0]);
				}
			}
		}
		this.CurrentSceneType = eSceneType;
	}

	public void RemoveSwitchData(TsSceneSwitcher.ESceneType eSceneType, TsSceneSwitcher.SwitchData switchData)
	{
		TsSceneSwitcher._SwitchDataList switchDataList = this._GetSwitchData_List(eSceneType);
		if (switchDataList == null)
		{
			return;
		}
		switchDataList.Remove(switchData);
	}
}
