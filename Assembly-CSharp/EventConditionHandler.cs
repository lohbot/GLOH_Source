using System;

public class EventConditionHandler : NrTSingleton<EventConditionHandler>
{
	public EventHandler_RangeMove RangeMove = new EventHandler_RangeMove();

	public EventHandler_ItemEquip ItemEquip = new EventHandler_ItemEquip();

	public EventHandler_QuestClose QuestClose = new EventHandler_QuestClose();

	public EventHandler_MapIn MapIn = new EventHandler_MapIn();

	public EventHandler_SceneChange SceneChange = new EventHandler_SceneChange();

	public EventHandler_ChapterClose ChapterClose = new EventHandler_ChapterClose();

	public EventHandler_OpenUI OpenUI = new EventHandler_OpenUI();

	public EventHandler_CloseUI CloseUI = new EventHandler_CloseUI();

	private EventConditionHandler()
	{
	}
}
