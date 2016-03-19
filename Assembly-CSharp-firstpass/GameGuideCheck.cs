using System;

[Flags]
public enum GameGuideCheck
{
	NONE = 0,
	CYCLECAL = 1,
	QUEST_COMPLETE = 2,
	LOGIN = 3,
	LEVELUP = 4,
	DEFEAT = 5,
	SCENE = 6
}
