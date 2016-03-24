using System;
using UnityForms;

public class UICommandLayer : InputCommandLayer
{
	~UICommandLayer()
	{
	}

	public override bool Update(INPUT_INFO curInput)
	{
		return (Battle.BATTLE == null || !Battle.BATTLE.InputControlTrigger || NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_TALK_DLG)) && NrTSingleton<UIManager>.Instance.Update();
	}
}
