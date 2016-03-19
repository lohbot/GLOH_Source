using Ndoors.Framework.Stage;
using System;

public class EventCondition_SceneChange : EventTriggerItem_EventCondition
{
	public string PreScene = string.Empty;

	public string CurScene = string.Empty;

	public override void RegisterEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.SceneChange.SceneChange += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.SceneChange.SceneChange -= new EventHandler(this.IsVerify);
	}

	public override bool IsVaildValue()
	{
		return true;
	}

	public override void IsVerify(object sender, EventArgs e)
	{
		EventArgs_SceneChange eventArgs_SceneChange = e as EventArgs_SceneChange;
		if (eventArgs_SceneChange == null)
		{
			return;
		}
		Scene.Type type = (Scene.Type)((int)Enum.Parse(typeof(Scene.Type), this.PreScene));
		Scene.Type type2 = (Scene.Type)((int)Enum.Parse(typeof(Scene.Type), this.CurScene));
		if (type == eventArgs_SceneChange.PreScene && type2 == eventArgs_SceneChange.CurScene)
		{
			base.Verify = true;
		}
	}

	public override string GetComment()
	{
		return string.Format("{0} 씬에서 {1} 씬으로 전환 되었을때", this.PreScene, this.CurScene);
	}
}
