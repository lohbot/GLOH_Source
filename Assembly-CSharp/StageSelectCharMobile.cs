using GameMessage.Private;
using Ndoors.Framework.Stage;
using StageHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TsBundle;
using UnityEngine;
using UnityForms;

public class StageSelectCharMobile : AStage
{
	private Dictionary<Type, ASubStage> m_mapSubStage;

	private ASubStage m_kCurrentSubStage;

	public int m_nCharKind = 1;

	private string scenePath = string.Empty;

	public override Scene.Type SceneType()
	{
		return Scene.Type.SELECTCHAR;
	}

	public override void OnPrepareSceneChange()
	{
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		NrLoadPageScreen.StepUpMain(1);
		NrLoadPageScreen.ShowHideLoadingImg(true);
		NrTSingleton<NkClientLogic>.Instance.SetLoginGameServer(false);
	}

	public override void OnEnter()
	{
		TsLog.Log("====== {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		Scene.ChangeSceneType(this.SceneType());
		Scene.ChangeSubSceneType(Scene.SubType.EMPTY);
		FacadeHandler.NotifyUnityVersion();
		this._InitSubStages();
		TsSceneSwitcher.Instance.ClearAllScene();
		CommonTasks.ClearAssetBundleResources(true);
		this.scenePath = string.Format("Map/fx_charactercreate_mobile{0}", Option.extAsset);
		base.StartTaskSerial(CommonTasks.InitializeChangeScene());
		base.StartTaskSerial(CommonTasks.LoadEmptyMainScene());
		base.StartTaskSerial(CommonTasks.LoadLevelSubScene(this.scenePath, Option.defaultStackName));
		base.StartTaskSerial(this._MoveSubStageSelect());
		base.StartTaskSerial(CommonTasks.LoadEnvironment(true));
		base.StartTaskSerial(this._PostProcessSelectCharScene());
		base.StartTaskSerial(CommonTasks.MemoryCleaning(true, 8));
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(true));
		base.StartTaskSerial(this._LoginCheck());
	}

	public override void OnExit()
	{
		TsLog.Log("====== {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
		NrTSingleton<CCameraAniPlay>.Instance.Clear();
		this.m_mapSubStage.Clear();
		this.m_mapSubStage = null;
		Holder.RemoveWWWItem(this.scenePath, true);
		Resources.UnloadUnusedAssets();
		if (this.m_kCurrentSubStage != null)
		{
			this.m_kCurrentSubStage.OnExit();
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SELECTCHAR_DLG);
		NrTSingleton<NkClientLogic>.Instance.SetLoginGameServer(true);
	}

	protected override void OnUpdateAfterStagePrework()
	{
		if (this.m_kCurrentSubStage != null)
		{
			this.m_kCurrentSubStage.Update();
		}
	}

	[DebuggerHidden]
	private IEnumerator _LoginCheck()
	{
		StageSelectCharMobile.<_LoginCheck>c__Iterator46 <_LoginCheck>c__Iterator = new StageSelectCharMobile.<_LoginCheck>c__Iterator46();
		<_LoginCheck>c__Iterator.<>f__this = this;
		return <_LoginCheck>c__Iterator;
	}

	private void _InitSubStages()
	{
		this.m_mapSubStage = new Dictionary<Type, ASubStage>();
		ASubStage value = new SubStage2D();
		this.m_mapSubStage.Add(typeof(SubStage2D), value);
	}

	[DebuggerHidden]
	private IEnumerator _MoveSubStageSelect()
	{
		StageSelectCharMobile.<_MoveSubStageSelect>c__Iterator47 <_MoveSubStageSelect>c__Iterator = new StageSelectCharMobile.<_MoveSubStageSelect>c__Iterator47();
		<_MoveSubStageSelect>c__Iterator.<>f__this = this;
		return <_MoveSubStageSelect>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator _PostProcessSelectCharScene()
	{
		return new StageSelectCharMobile.<_PostProcessSelectCharScene>c__Iterator48();
	}

	protected void MoveSubStage(Type kType)
	{
		if (!this.m_mapSubStage.ContainsKey(kType))
		{
			UnityEngine.Debug.LogError("error");
			UnityEngine.Debug.Break();
		}
		this.m_kCurrentSubStage = this.m_mapSubStage[kType];
		this.m_kCurrentSubStage.Start();
	}

	public void DisplaySelectCharName(bool bShow)
	{
		if (this.m_kCurrentSubStage is SubStage2D)
		{
			SubStage2D subStage2D = (SubStage2D)this.m_kCurrentSubStage;
			subStage2D.DisplayNames(bShow);
		}
	}

	public static void OnChangeQualitySettings(TsQualityManager.Level level)
	{
		GameObject x = GameObject.Find("TLCustoApplication");
		if (x == null)
		{
			return;
		}
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(SkinnedMeshRenderer));
		UnityEngine.Object[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			UnityEngine.Object @object = array2[i];
			if (@object is SkinnedMeshRenderer)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = @object as SkinnedMeshRenderer;
				if (level == TsQualityManager.Level.LOWEST || level == TsQualityManager.Level.LOW)
				{
					skinnedMeshRenderer.enabled = false;
				}
				else
				{
					skinnedMeshRenderer.enabled = true;
				}
			}
		}
		UnityEngine.Object[] array3 = UnityEngine.Object.FindObjectsOfType(typeof(Light));
		UnityEngine.Object[] array4 = array3;
		for (int j = 0; j < array4.Length; j++)
		{
			UnityEngine.Object object2 = array4[j];
			if (object2 is Light)
			{
				Light light = object2 as Light;
				if (level == TsQualityManager.Level.LOWEST || level == TsQualityManager.Level.LOW)
				{
					light.enabled = false;
				}
				else
				{
					light.enabled = true;
				}
			}
		}
	}

	public ASubStage GetSubStage(Type kType)
	{
		if (!this.m_mapSubStage.ContainsKey(kType))
		{
			UnityEngine.Debug.LogError("error");
			UnityEngine.Debug.Break();
		}
		return this.m_mapSubStage[kType];
	}

	public ASubStage GetSubStage()
	{
		return this.m_kCurrentSubStage;
	}

	public bool OnGameServerConnected()
	{
		SubStage2D subStage2D = (SubStage2D)this.GetSubStage(typeof(SubStage2D));
		subStage2D._StartGame();
		return true;
	}

	public void DisplayAvatar(bool bShow)
	{
		SubStage2D subStage2D = (SubStage2D)this.GetSubStage(typeof(SubStage2D));
		if (subStage2D != null)
		{
			subStage2D.DisplayNames(bShow);
		}
	}

	public bool CharacterSelect(NrCharUser pkChar, bool bConnectServer)
	{
		SubStage2D subStage2D = (SubStage2D)this.GetSubStage(typeof(SubStage2D));
		subStage2D.CharacterSelect(pkChar, bConnectServer);
		return true;
	}

	public bool SetCreateCharPartInfo(bool bCustom2Create, bool bCreate2Custom)
	{
		TsLog.LogWarning("!!!!!!!!!!!SetCreateCharPartInfo", new object[0]);
		return false;
	}

	public int GetCharKind()
	{
		return this.m_nCharKind;
	}

	public bool SetSelectStep(E_CHAR_SELECT_STEP _step)
	{
		TsLog.Log("SetSelectStep : {0}", new object[]
		{
			_step
		});
		SubStage2D subStage2D = (SubStage2D)this.GetSubStage(typeof(SubStage2D));
		if (subStage2D != null)
		{
			subStage2D.SetSelectStep(_step);
		}
		return true;
	}

	public bool SetCreateChar(E_CHAR_TRIBE _tribe)
	{
		TsLog.Log("SetCreateChar : {0}", new object[]
		{
			_tribe
		});
		SubStage2D subStage2D = (SubStage2D)this.GetSubStage(typeof(SubStage2D));
		if (subStage2D != null)
		{
			subStage2D.SetCreateChar(_tribe);
		}
		return true;
	}

	public bool ConfirmCharName(bool bCheckSuccess)
	{
		DLG_CreateChar dLG_CreateChar = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEW_CREATECHAR_DLG) as DLG_CreateChar;
		if (dLG_CreateChar != null)
		{
			dLG_CreateChar.ConfirmCharName(bCheckSuccess);
		}
		return true;
	}

	public bool CreateComplete(bool bCheckSuccess)
	{
		SubStage2D subStage2D = (SubStage2D)this.GetSubStage(typeof(SubStage2D));
		if (subStage2D != null)
		{
			subStage2D._CreateCharComplete();
		}
		return true;
	}
}
