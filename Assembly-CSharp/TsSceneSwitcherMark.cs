using System;
using UnityEngine;

public class TsSceneSwitcherMark : MonoBehaviour
{
	private string _rootGOName = string.Empty;

	[SerializeField]
	private TsSceneSwitcher.ESceneType _eSceneType;

	public string RootGOName
	{
		get
		{
			return this._rootGOName;
		}
		set
		{
			this._rootGOName = value;
		}
	}

	public TsSceneSwitcher.ESceneType SceneType
	{
		get
		{
			return this._eSceneType;
		}
		set
		{
			this._eSceneType = value;
		}
	}

	public bool IsBattleMap
	{
		get
		{
			return this.SceneType == TsSceneSwitcher.ESceneType.BattleScene;
		}
	}

	public bool IsCollected
	{
		get;
		set;
	}

	public TsSceneSwitcher.SwitchData RefSwitchData
	{
		get;
		set;
	}

	public override string ToString()
	{
		return string.Format("RootGO[{0}] isMapGO[{1}] SceneType[{2}] Data[{3}]", new object[]
		{
			this.RootGOName,
			this.SceneType,
			this.IsCollected,
			this.RefSwitchData
		});
	}

	public void Awake()
	{
	}

	public void OnDestroy()
	{
	}

	public void OnCombinedChildren(GameObject combinedGO)
	{
		TsSceneSwitcher.Instance.Collect(this.SceneType, combinedGO);
	}
}
