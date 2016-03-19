using Ndoors.Framework.Stage;
using System;
using System.Collections;
using System.Diagnostics;
using UnityForms;

public class StageError : AStage
{
	public StageError(string logMsg)
	{
	}

	public override Scene.Type SceneType()
	{
		return Scene.Type.ERROR;
	}

	public override void OnPrepareSceneChange()
	{
		TsLog.LogError(StageSystem.ToStringStatus(), new object[0]);
	}

	public override void OnEnter()
	{
		TsLog.Log("--- {0}.OnEnter", new object[]
		{
			base.GetType().FullName
		});
		Scene.ChangeSceneType(this.SceneType());
		NrTSingleton<FormsManager>.Instance.CloseAllExcept(G_ID.DLG_LOADINGPAGE);
	}

	public override void OnExit()
	{
		TsLog.Log("--- {0}.OnExit", new object[]
		{
			base.GetType().FullName
		});
	}

	protected override void OnUpdateAfterStagePrework()
	{
	}

	[DebuggerHidden]
	private IEnumerator SetAudioListener()
	{
		return new StageError.<SetAudioListener>c__Iterator34();
	}
}
