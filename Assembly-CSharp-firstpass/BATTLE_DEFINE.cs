using System;

public static class BATTLE_DEFINE
{
	public const int BATTLE_ROOM_MAX = 2000;

	public const short EMPTY_BATTLE_BUID = -1;

	public const int ALLY_CHAR_MAX_NUM = 60;

	public const int BATTLE_TOTAL_CHAR_NUM = 120;

	public const int BATTLE_MAP_NAME_LEN = 50;

	public const int BATTLE_ALLY_MAX = 2;

	public const int BATTLE_MOVE_POS_LIST = 40;

	public const int MAX_BATTLE_ORDER_PARA = 5;

	public const int MAX_BATTLE_CHARINFO_PARA = 10;

	public const int BATTLE_ORDER_UNIQUE_MAX = 3;

	public const float DIE_MOTION_TIME = 1f;

	public const float DIE_DELETE_TIME = 7f;

	public const float DOWN_VALUE = 0.02f;

	public const float CENTER_BACK_POS_LEN = 10f;

	public const float CENTER_FRONT_POS_LEN = 3f;

	public const int CONTINUEBATTLE_BOSS_ECO_NUM = 14;

	public const int MAX_ANGERLY_POINT = 1000;

	public const int MAX_ANGELANGERLY_POINT = 1000;

	public const int MAX_BABEL_SOLO_ANGERLY_POINT = 1500;

	public const float DELAY_RATE = 0.25f;

	public const int MAX_PLUNDER_SOLNUM = 15;

	public const int MAX_PLUNDER_SOLNUM_PER_STARTPOS = 5;

	public const int MIN_PLUNDER_LEVEL = 10;

	public const long BATTLE_REPLAY_FOLDERNUM = 1000L;

	public const int MIN_COLOSSEUM_LEVEL = 10;

	public const int MAX_COLOSSEUM_DUMMY_HERO_NUM = 6;

	public const int MAX_COLOSSEUM_DUMMY_HERO = 10;

	public const int MAX_COLOSSEUM_TUTORIAL_COUNT = 3;

	public const int MAX_COLOSSEUM_BATCH_COUNT = 3;

	public const int MAX_COLOSSEUM_GRIDPOS = 9;

	public const int MAX_BABELTOWER_ROOMPERCHAR_NUM = 16;

	public const int MAX_BABELTOWER_ONEPLAYER_NUM = 12;

	public const int MAX_MYTHRAID_ROOMPERCHAR_NUM = 12;

	public const int MAX_MYTHRAID_ONEPLAYER_NUM = 12;

	public const int MAX_MYTHRAID_ROOM_NUM = 1000;

	public const int MAX_MYTHRAID_BATCH_GRIDPOS = 20;

	public const int MAX_BABELTOWER_ROOM_NUM = 1000;

	public const int MAX_BOSSAGGRO_TARGET_NUM = 3;

	public const int TREASURE_MONSTER_KIND = 703;

	public const int HIDDEN_TREASURE_MONSTER_KIND = 995;

	public const int MAX_CHANGE_SOLDIER_NUM = 3;

	public const int MAX_ENTERUSER_SOLIDERINFO = 19;

	public const int MAX_BABELTOWER_SOLPOS = 16;

	public const int MAX_BABELTOWER_BATCH_GRIDPOS = 20;

	public const int MAX_GUILDBOSS_BATCH_GRIDPOS = 16;

	public const int MAX_PLUNDER_DEFENCE_OBJCOUNT = 3;

	public const int MAX_DAMAGE_ARRAY = 15;

	public const int MAX_ITEMSKILL_COUNT_DEF = 4;

	public const int MAX_ITEMSKILL_COUNT_WEP = 1;

	public const int MAX_BATTLECHAR_REVIVAL_COUNT = 3;

	public const int MAX_TOURNAMENT_LOBBY_SELECT_COUNT = 3;

	public const int MAX_TOURNAMENT_SELECT_STEP = 4;

	public const int TUTORIAL_BATTLE_SCENARIO = 99999999;

	public const int MAX_DAILYDUNGEON_GRIDPOS = 9;

	public static string[] RANK_STRING = new string[]
	{
		"NONE",
		"D",
		"C",
		"B",
		"A",
		"S",
		"SS"
	};

	public static int iOrderSeed = 1;

	public static G_ID[] TRIGGERDLG_ID = new G_ID[]
	{
		G_ID.NONE,
		G_ID.BATTLE_CONTROL_DLG,
		G_ID.BATTLE_BOSSAGGRO_DLG,
		G_ID.BATTLE_PLUNDER_TURNCOUNT_DLG,
		G_ID.UIGUIDE_DLG
	};

	public static int MakeBFOrderUnique(int iBFCharUnique)
	{
		int result = iBFCharUnique * 1000 + BATTLE_DEFINE.iOrderSeed;
		BATTLE_DEFINE.iOrderSeed++;
		if (BATTLE_DEFINE.iOrderSeed >= 1000)
		{
			BATTLE_DEFINE.iOrderSeed = 1;
		}
		return result;
	}
}
