using System;

public class EventConditionHandler : NrTSingleton<EventConditionHandler>
{
	public EventHandler_RangeMove RangeMove = new EventHandler_RangeMove();

	public EventHandler_ItemEquip ItemEquip = new EventHandler_ItemEquip();

	public EventHandler_QuestClose QuestClose = new EventHandler_QuestClose();

	public EventHandler_MapIn MapIn = new EventHandler_MapIn();

	public EventHandler_SceneChange SceneChange = new EventHandler_SceneChange();

	public EventHandler_ChapterClose ChapterClose = new EventHandler_ChapterClose();

	public EventHandler_CloseUI CloseUI = new EventHandler_CloseUI();

	public EventHandler_OpenUIByChallenge OpenUIByChallenge = new EventHandler_OpenUIByChallenge();

	public EventHandler_MythEvolutionSetComplete MythEvolutionSet = new EventHandler_MythEvolutionSetComplete();

	public EventHandler_MythEvolutionListSetComplete MythEvolutionListSet = new EventHandler_MythEvolutionListSetComplete();

	public EventHandler_MythEvolutionListReadyComplete MythEvolutionListReady = new EventHandler_MythEvolutionListReadyComplete();

	public EventHandler_MythElementSelectSetComplete MythElementSelectSet = new EventHandler_MythElementSelectSetComplete();

	public EventHandler_MythEvolutionInfoOpenMsgBox MythEvolutionInfoMsgBox = new EventHandler_MythEvolutionInfoOpenMsgBox();

	public EventHandler_WorldMapModeClickHandler WorldMapModeClick = new EventHandler_WorldMapModeClickHandler();

	private EventConditionHandler()
	{
	}
}
