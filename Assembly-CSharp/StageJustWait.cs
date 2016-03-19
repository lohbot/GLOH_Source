using Ndoors.Framework.Stage;
using StageHelper;
using System;

public class StageJustWait : AStage
{
	public override Scene.Type SceneType()
	{
		return Scene.Type.JUSTWAIT;
	}

	public override void OnPrepareSceneChange()
	{
		NrLoadPageScreen.DecideLoadingType(Scene.CurScene, this.SceneType());
		NrLoadPageScreen.CustomLoadingProgress = true;
		NrLoadPageScreen.StepUpMain(3);
		NrLoadPageScreen.ShowHideLoadingImg(true);
	}

	public override void OnEnter()
	{
		TsLog.Log("--- {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		Scene.ChangeSceneType(this.SceneType());
		CommonTasks.ClearAssetBundleResources(false);
		base.StartTaskSerial(CommonTasks.InitializeChangeScene());
		base.StartTaskSerial(CommonTasks.FinalizeChangeScene(false));
	}

	public override void OnExit()
	{
		TsLog.Log("--- {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
		NrLoadPageScreen.CustomLoadingProgress = false;
	}

	protected override void OnUpdateAfterStagePrework()
	{
	}
}
