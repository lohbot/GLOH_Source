using Ndoors.Framework.Stage;
using System;
using UnityEngine;

public class EventTriggerGameHelper
{
	private const float TEMP_Y = 1000f;

	public static bool IsQuestState(string QuestUnique, QUEST_CONST.eQUESTSTATE QuestState)
	{
		QUEST_CONST.eQUESTSTATE questState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(QuestUnique);
		return questState == QuestState;
	}

	public static bool IsLoadedStage()
	{
		if (Scene.IsCurScene(Scene.Type.WORLD) && StageSystem.IsStable)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null && @char.IsReady3DModel())
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsLoadMyChar()
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		return @char != null && @char.IsReady3DModel();
	}

	public static float GetGroundPosition(float x, float z)
	{
		Vector3 origin = new Vector3(x, 1000f, z);
		Ray ray = new Ray(origin, new Vector3(0f, -1f, 0f));
		TsLayerMask layerMask = TsLayer.NOTHING + TsLayer.TERRAIN;
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 2000f, layerMask))
		{
			return raycastHit.point.y;
		}
		return 1000f;
	}
}
