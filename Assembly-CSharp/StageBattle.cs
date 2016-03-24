using Ndoors.Framework.Stage;
using StageHelper;
using System;
using System.Collections;
using System.Diagnostics;
using TsBundle;
using UnityEngine;
using UnityForms;

public class StageBattle : AStage
{
	private static readonly string s_battleAudioListener = "_BattleAudioListener";

	private Battle BattleClient;

	private bool _bTemporalBattleUpdate;

	public override Scene.Type SceneType()
	{
		return Scene.Type.BATTLE;
	}

	public override void OnPrepareSceneChange()
	{
		Application.targetFrameRate = -1;
		NrTSingleton<NkCharManager>.Instance.DeleteBattleChar();
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		NrLoadPageScreen.StepUpMain(1);
		NrLoadPageScreen.SetSkipMainStep(1);
		NrLoadPageScreen.ShowHideLoadingImg(true);
		this.BattleClient = new Battle();
		this.BattleClient.Init();
		this.BattleClient.Send_GS_BATTLE_READY_NFY(0);
		this._bTemporalBattleUpdate = true;
	}

	public override void OnEnter()
	{
		TsLog.Log("====== {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		if (TsPlatform.IsLowSystemMemory)
		{
			TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.WorldScene);
			TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.SoldierBatchScene);
			Holder.ClearStackItem(Option.IndependentFromStageStackName, false);
			base.StartTaskSerial(CommonTasks.LoadEmptyMainScene());
			base.StartTaskSerial(CommonTasks.MemoryCleaning(true, 8));
		}
		Scene.ChangeSceneType(this.SceneType());
		base.StartTaskSerial(CommonTasks.InitializeChangeScene());
		base.StartTaskSerial(this._StageProcess());
		base.StartTaskSerial(CommonTasks.DownloadAsset(string.Format("Effect/CharEffect/cameramove{0}{1}", NrTSingleton<UIDataManager>.Instance.AddFilePath, Option.extAsset), new PostProcPerItem(this._funcDownloadedCameraTarget), null, true));
		base.StartTaskSerial(this._StageAfterBattleMapLoadProcess());
		base.StartTaskSerial(CommonTasks.DownloadAsset(string.Format("Effect/CharEffect/fx_battledamage{0}{1}", NrTSingleton<UIDataManager>.Instance.AddFilePath, Option.extAsset), new PostProcPerItem(this._funcDownloadedDamage), null, true));
		base.StartTaskSerial(CommonTasks.DownloadAsset(string.Format("Effect/Instant/fx_result{0}{1}", NrTSingleton<UIDataManager>.Instance.AddFilePath, Option.extAsset), new PostProcPerItem(this._funcDownloadedResultEffect), null, true));
		base.StartTaskSerial(CommonTasks.DownloadAsset(string.Format("Effect/Instant/fx_battlearrow{0}{1}", NrTSingleton<UIDataManager>.Instance.AddFilePath, Option.extAsset), new PostProcPerItem(this._fundBattleArrowDownload), null, true));
		base.StartTaskSerial(CommonTasks.DownloadAsset(string.Format("Effect/Instant/fx_skill_directing{0}{1}", NrTSingleton<UIDataManager>.Instance.AddFilePath, Option.extAsset), new PostProcPerItem(this._fundBattleSkillDirectingDownload), null, true));
		base.StartTaskSerial(CommonTasks.DownloadAsset(string.Format("Effect/Instant/fx_battle_fatality{0}{1}", NrTSingleton<UIDataManager>.Instance.AddFilePath, Option.extAsset), new PostProcPerItem(this._fundBattleSkillRivalDirectingDownload), null, true));
		base.StartTaskSerial(CommonTasks.DownloadAsset(string.Format("Effect/Instant/fx_angergauge{0}{1}", NrTSingleton<UIDataManager>.Instance.AddFilePath, Option.extAsset), new PostProcPerItem(this._fundBattleControlAngergaugeDownload), null, true));
		base.StartTaskSerial(this.DownLoadColosseumEffect());
		base.StartTaskSerial(CommonTasks.MemoryCleaning(true, 8));
		base.StartTaskSerial(CommonTasks.LoadEnvironment(true));
		NrLoadPageScreen.IncreaseProgress(1f);
		NrLoadPageScreen.IncreaseProgress(1f);
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(true));
		base.StartTaskSerial(this._StageStartBattle());
		base.StartTaskPararell(this._UpdateBattleTemporal());
	}

	public override void OnExit()
	{
		if (NrTSingleton<NkEffectManager>.Instance.DontMakeEffect)
		{
			NrTSingleton<NkEffectManager>.Instance.DontMakeEffect = false;
		}
		TsLog.Log("====== {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
		this.BattleClient.ClearStaticValue();
		NrTSingleton<NkBattleCharManager>.Instance.Init();
		this.BattleClient.Dispose();
		this.BattleClient = null;
		NrTSingleton<NkBattleReplayManager>.Instance.ClearReplayFlag();
		Time.timeScale = 1f;
		Resources.UnloadUnusedAssets();
		NrTSingleton<FormsManager>.Instance.ClearShowHideForms();
	}

	protected override void OnUpdateAfterStagePrework()
	{
		if (this.BattleClient != null)
		{
			this.BattleClient.Update();
		}
	}

	[DebuggerHidden]
	private IEnumerator _UpdateBattleTemporal()
	{
		StageBattle.<_UpdateBattleTemporal>c__Iterator30 <_UpdateBattleTemporal>c__Iterator = new StageBattle.<_UpdateBattleTemporal>c__Iterator30();
		<_UpdateBattleTemporal>c__Iterator.<>f__this = this;
		return <_UpdateBattleTemporal>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator _StageProcess()
	{
		StageBattle.<_StageProcess>c__Iterator31 <_StageProcess>c__Iterator = new StageBattle.<_StageProcess>c__Iterator31();
		<_StageProcess>c__Iterator.<>f__this = this;
		return <_StageProcess>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator _StageAfterBattleMapLoadProcess()
	{
		StageBattle.<_StageAfterBattleMapLoadProcess>c__Iterator32 <_StageAfterBattleMapLoadProcess>c__Iterator = new StageBattle.<_StageAfterBattleMapLoadProcess>c__Iterator32();
		<_StageAfterBattleMapLoadProcess>c__Iterator.<>f__this = this;
		return <_StageAfterBattleMapLoadProcess>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator _StageStartBattle()
	{
		StageBattle.<_StageStartBattle>c__Iterator33 <_StageStartBattle>c__Iterator = new StageBattle.<_StageStartBattle>c__Iterator33();
		<_StageStartBattle>c__Iterator.<>f__this = this;
		return <_StageStartBattle>c__Iterator;
	}

	public bool GameResult()
	{
		Battle.BATTLE.BattleCamera.SetLastAttackCamera(null, false);
		NrTSingleton<NkEffectManager>.Instance.DontMakeEffect = false;
		this.DeleteBattleAudioListener();
		this.BattleClient.CloseBattle();
		if (TsPlatform.IsLowSystemMemory)
		{
			NrTSingleton<NkEffectManager>.Instance.ClearEffectCache();
		}
		return true;
	}

	public GameObject CreateBattleAudioListener()
	{
		maxCamera component = Camera.main.gameObject.GetComponent<maxCamera>();
		if (component == null)
		{
			TsLog.LogError("Cannot Found CameraController~! at GO= " + Camera.main.gameObject.name, new object[0]);
			return null;
		}
		GameObject gameObject = GameObject.Find(StageBattle.s_battleAudioListener);
		if (gameObject != null)
		{
			if (gameObject.GetComponent<TsPositionFollower>() == null)
			{
				gameObject.AddComponent<TsPositionFollower>();
			}
		}
		else
		{
			gameObject = new GameObject(StageBattle.s_battleAudioListener, new Type[]
			{
				typeof(TsPositionFollower)
			});
		}
		TsPositionFollower component2 = gameObject.GetComponent<TsPositionFollower>();
		gameObject.transform.parent = TsSceneSwitcher.Instance._GetBundle_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene).transform;
		component2.SetPositionFollower(this.BattleClient.BattleCamera.GetTarget().transform, Camera.main);
		return gameObject;
	}

	public void DeleteBattleAudioListener()
	{
		TsAudioManager.Instance.InitAudioListenerSwitcher();
		GameObject gameObject = GameObject.Find(StageBattle.s_battleAudioListener);
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	private void SetParentBattleScene(GameObject goChild)
	{
		GameObject gameObject = TsSceneSwitcher.Instance._GetBundle_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
		if (gameObject != null && goChild != null)
		{
			goChild.transform.parent = gameObject.transform;
		}
	}

	[DebuggerHidden]
	private IEnumerator DownLoadColosseumEffect()
	{
		StageBattle.<DownLoadColosseumEffect>c__Iterator34 <DownLoadColosseumEffect>c__Iterator = new StageBattle.<DownLoadColosseumEffect>c__Iterator34();
		<DownLoadColosseumEffect>c__Iterator.<>f__this = this;
		return <DownLoadColosseumEffect>c__Iterator;
	}

	private void _funcDownloadedCameraTarget(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.CameraTarget != null)
				{
					this.BattleClient.CameraTarget = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.CameraTarget = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.CameraTarget.SetActive(false);
					this.SetParentBattleScene(this.BattleClient.CameraTarget);
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _funcDownloadedDamage(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.DamageEffect != null)
				{
					this.BattleClient.DamageEffect = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.DamageEffect = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.SetParentBattleScene(this.BattleClient.DamageEffect);
				}
				NkBattleDamage.SetStaticData();
				this.BattleClient.CreateDamageEffectFromReservationArray();
				TsPlatform.FileLog("Battle _funcDownloadedDamage Pass");
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _funcDownloadedResultEffect(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.ResultEffect != null)
				{
					this.BattleClient.ResultEffect = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.ResultEffect = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.ResultEffect.SetActive(false);
					this.SetParentBattleScene(this.BattleClient.ResultEffect);
					TsPlatform.FileLog("Battle _funcDownloadedResultEffect Pass");
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _fundBattleArrowDownload(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.MoveArrow != null)
				{
					this.BattleClient.MoveArrow = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.MoveArrow = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.MoveArrow.transform.position = new Vector3(10000f, 10000f, 10000f);
					this.SetParentBattleScene(this.BattleClient.MoveArrow);
					TsPlatform.FileLog("Battle _fundBattleArrowDownload Pass");
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _fundBattleSkillDirectingDownload(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.SkillDirecting != null)
				{
					this.BattleClient.SkillDirecting = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.SkillDirecting = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.SkillDirecting.SetActive(false);
					this.SetParentBattleScene(this.BattleClient.SkillDirecting);
					TsPlatform.FileLog("Battle _funcDownloadedSkillDirecting Pass");
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _fundBattleSkillRivalDirectingDownload(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.SkillRivalDirecting != null)
				{
					this.BattleClient.SkillRivalDirecting = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.SkillRivalDirecting = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.SkillRivalDirecting.SetActive(false);
					this.SetParentBattleScene(this.BattleClient.SkillRivalDirecting);
					TsPlatform.FileLog("Battle _funcDownloadedSkillRivalDirecting Pass");
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _fundBattleControlAngergaugeDownload(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.ControlAngergauge != null)
				{
					this.BattleClient.ControlAngergauge = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.ControlAngergauge = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.ControlAngergauge.SetActive(false);
					this.SetParentBattleScene(this.BattleClient.ControlAngergauge);
					TsPlatform.FileLog("Battle _fundBattleControlAngergaugeDownload Pass");
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _fundColosseumDirectSummonDownload(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.ColosseumCardSummons != null)
				{
					this.BattleClient.ColosseumCardSummons = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.ColosseumCardSummons = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.ColosseumCardSummons.SetActive(false);
					this.SetParentBattleScene(this.BattleClient.ColosseumCardSummons);
					TsPlatform.FileLog("Battle _fundColosseumDirectSummonDownload Pass");
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _fundColosseumKillDownload(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.ColosseumKill != null)
				{
					this.BattleClient.ColosseumKill = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.ColosseumKill = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.ColosseumKill.SetActive(false);
					this.SetParentBattleScene(this.BattleClient.ColosseumKill);
					TsPlatform.FileLog("Battle _fundColosseumKillDownload Pass");
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _fundColosseumCountDownload(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.ColosseumCount != null)
				{
					this.BattleClient.ColosseumCount = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.ColosseumCount = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.ColosseumCount.SetActive(false);
					this.SetParentBattleScene(this.BattleClient.ColosseumCount);
					TsPlatform.FileLog("Battle _fundColosseumCountDownload Pass");
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _fundColosseumCriticalDownload(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.ColosseumCritical != null)
				{
					this.BattleClient.ColosseumCritical = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.ColosseumCritical = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.ColosseumCritical.SetActive(false);
					this.SetParentBattleScene(this.BattleClient.ColosseumCritical);
					TsPlatform.FileLog("Battle _fundColosseumCriticalDownload Pass");
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	private void _fundColosseumRecallload(IDownloadedItem wItem, object obj)
	{
		try
		{
			NrLoadPageScreen.AddLoadingItemCnt(1);
			if (this.BattleClient != null)
			{
				if (this.BattleClient.ColosseumRecall != null)
				{
					this.BattleClient.ColosseumRecall = null;
				}
				GameObject gameObject = wItem.mainAsset as GameObject;
				if (gameObject != null)
				{
					this.BattleClient.ColosseumRecall = (GameObject)UnityEngine.Object.Instantiate(gameObject);
					this.BattleClient.ColosseumRecall.SetActive(false);
					this.SetParentBattleScene(this.BattleClient.ColosseumRecall);
					TsPlatform.FileLog("Battle _fundColosseumRecallload Pass");
				}
			}
			NrLoadPageScreen.IncreaseProgress(1f);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}
}
