using System;

public class UICommandLayer : InputCommandLayer
{
	~UICommandLayer()
	{
	}

	public override bool Update(INPUT_INFO curInput)
	{
		return (Battle.BATTLE == null || !Battle.BATTLE.InputControlTrigger) && NrTSingleton<UIManager>.Instance.Update();
	}
}
