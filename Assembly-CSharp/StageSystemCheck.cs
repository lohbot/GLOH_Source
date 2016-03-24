using com.adjust.sdk;
using GameMessage.Private;
using Ndoors.Framework.Stage;
using omniata;
using StageHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityForms;

public class StageSystemCheck : AStage
{
	public static GameObject pkGoOminiata;

	public override Scene.Type SceneType()
	{
		return Scene.Type.SYSCHECK;
	}

	public override void OnPrepareSceneChange()
	{
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		NrLoadPageScreen.StepUpMain(1);
		UIDataManager.MuteBGM = TsAudio.IsMuteAudio(EAudioType.BGM);
		UIDataManager.MuteEffect = TsAudio.IsMuteAudio(EAudioType.SFX);
	}

	public override void OnEnter()
	{
		StageSystem.AddCommonPararellTask(AutoMemoryCleanUp.Action());
		TsLog.Log("====== {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		Scene.ChangeSceneType(this.SceneType());
		base.StartTaskSerial(this._SDKSetting());
		base.StartTaskSerial(this._WaitMobileTextInit());
		base.StartTaskSerial(this._WaitMobileReview());
		base.StartTaskSerial(this._WaitWebCallParameter());
		base.StartTaskSerial(this._WaitDownloadFinalPatchList());
		base.StartTaskSerial(this._WaitDownloadAssetBundleURLInfo());
		base.StartTaskSerial(this._WaitDownloadPack());
		base.StartTaskSerial(this._InitializeUI());
		base.StartTaskSerial(this._InitializePlatform());
		base.StartTaskSerial(this._ClearFileLog());
		base.StartTaskSerial(CommonTasks.SetGUIBehaviourScene());
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(false));
	}

	public override void OnExit()
	{
		TsLog.Log("====== {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
	}

	protected override void OnUpdateAfterStagePrework()
	{
		FacadeHandler.MoveStage(Scene.Type.LOGIN);
	}

	[DebuggerHidden]
	private IEnumerator _WaitMobileTextInit()
	{
		return new StageSystemCheck.<_WaitMobileTextInit>c__Iterator51();
	}

	[DebuggerHidden]
	private IEnumerator _WaitMobileReview()
	{
		return new StageSystemCheck.<_WaitMobileReview>c__Iterator52();
	}

	[DebuggerHidden]
	private IEnumerator _WaitWebCallParameter()
	{
		return new StageSystemCheck.<_WaitWebCallParameter>c__Iterator53();
	}

	[DebuggerHidden]
	private IEnumerator _WaitDownloadFinalPatchList()
	{
		return new StageSystemCheck.<_WaitDownloadFinalPatchList>c__Iterator54();
	}

	[DebuggerHidden]
	private IEnumerator _WaitDownloadAssetBundleURLInfo()
	{
		return new StageSystemCheck.<_WaitDownloadAssetBundleURLInfo>c__Iterator55();
	}

	[DebuggerHidden]
	private IEnumerator _WaitDownloadPack()
	{
		return new StageSystemCheck.<_WaitDownloadPack>c__Iterator56();
	}

	[DebuggerHidden]
	private IEnumerator _InitializeUI()
	{
		return new StageSystemCheck.<_InitializeUI>c__Iterator57();
	}

	[DebuggerHidden]
	private IEnumerator _InitializePlatform()
	{
		return new StageSystemCheck.<_InitializePlatform>c__Iterator58();
	}

	[DebuggerHidden]
	private IEnumerator _SDKSetting()
	{
		return new StageSystemCheck.<_SDKSetting>c__Iterator59();
	}

	public static void OnAdjustResult(ResponseData responseDelegate)
	{
		if (StageSystemCheck.pkGoOminiata == null)
		{
			return;
		}
		OmniataComponent component = StageSystemCheck.pkGoOminiata.GetComponent<OmniataComponent>();
		if (component == null)
		{
			return;
		}
		if (responseDelegate.activityKind == ResponseData.ActivityKind.SESSION)
		{
			component.Track("om_adjust", new Dictionary<string, string>
			{
				{
					"adgroup",
					responseDelegate.adgroup
				},
				{
					"campaign",
					responseDelegate.campaign
				},
				{
					"creative",
					responseDelegate.creative
				},
				{
					"network",
					responseDelegate.network
				},
				{
					"trackerName",
					responseDelegate.trackerName
				},
				{
					"trackerToken",
					responseDelegate.trackerToken
				}
			});
		}
	}

	[DebuggerHidden]
	private IEnumerator _ClearFileLog()
	{
		return new StageSystemCheck.<_ClearFileLog>c__Iterator5A();
	}
}
