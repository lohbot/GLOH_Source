using System;
using UnityEngine;

public class EffectDefine
{
	public const uint INVALID_REGIST_NUM = 0u;

	public const uint START_REGIST_NUM = 1u;

	public const float AUTO_DESTROY_TIME = 10f;

	public const string ARROW = "ARROW";

	public const string GUN = "GUN";

	public const string HIT = "HIT";

	public const string BATTLEING = "BATTLEING";

	public const string LEVELUP = "LEVELUP";

	public const string CLICK_MOVE = "CLICK_MOVE";

	public const string WARP = "WARP";

	public const string QUEST_ACCEPT_POSSIBLE = "QUEST_ACCEPT_POSSIBLE";

	public const string QUEST_COMPLETE_POSSIBLE = "QUEST_COMPLETE_POSSIBLE";

	public const string QUEST_ACCEPT_IMPOSSIBLE = "QUEST_ACCEPT_IMPOSSIBLE";

	public const string QUEST_COMPLETE_IMPOSSIBLE = "QUEST_COMPLETE_IMPOSSIBLE";

	public const string QUEST_BLUE_ACCEPT_POSSIBLE = "QUEST_BLUE_ACCEPT_POSSIBLE";

	public const string QUEST_BLUE_COMPLETE_POSSIBLE = "QUEST_BLUE_COMPLETE_POSSIBLE";

	public const string BF_PLAYER_TURN = "FX_PLAYER_PHASE";

	public const string BF_ENEMY_TURN = "FX_ENEMY_PHASE";

	public const string BF_ATTACK_TARTGET = "FX_ARROW_MARK";

	public const string BF_CHANNGE = "FX_BATTLE_CHALLENGE";

	public const string BF_BRUN = "FX_BRUN";

	public const string BF_GRID_ACTIVE = "FX_GRID_GREEN";

	public const string BF_GRID_ACTIVE_SELECT = "FX_GRID_GREEN_SELECT";

	public const string BF_GRID_ATTACK = "FX_GRID_RED";

	public const string BF_GRID_NORMAL = "FX_GRID_WHITE";

	public const string BF_GRID_NORMAL_SELECT = "FX_GRID_WHITE_SELECT";

	public const string BF_GRID_SKILL = "FX_GRID_SKILL";

	public const string BF_GRID_BABEL_ADVANTAGE = "FX_GRID_BABELADVANTAGE";

	public const string POST_RECEIVE = "ef_getmail_001";

	public const string POST_SEND = "ef_sendmail_001";

	public const string RUN_WATER = "FX_RUN_WATER";

	public const string STAY_WATER = "FX_STAY_WATER";

	public const string NOTICE_QUESTMONSTER = "FX_NOTICE_QUESTMONSTER";

	public const string BF_COIN = "FX_COIN";

	public const string FAKESHADOW = "FAKE_SHADOW";

	public const string FX_WEAPON_GOOD = "FX_WEAPON_GOOD";

	public const string FX_WEAPON_BEST = "FX_WEAPON_BEST";

	public const string FX_UI_HEARTS_STONE = "FX_UI_HEARTS_STONE";

	public const string FX_SKILL_ACTIVE = "FX_SKILL_ACTIVE";

	public const string FX_URGENTBUTTON_UI = "fx_urgentbutton_ui";

	public const string FX_REWARD = "FX_REWARD";

	public const string FX_REWARDGET = "FX_REWARDGET";

	public const string FX_REWARDHEART_UI = "FX_REWARDHEART_UI";

	public const string FX_REWARDGLOW_UI = "FX_REWARDGLOW_UI";

	public const string FX_REWARDCOIN_UI = "FX_REWARDCOIN_UI";

	public const string ATTENDANCECHECK = "ATTENDANCECHECK";

	public const string FX_ANGERGAUGE = "FX_ANGERGAUGE";

	public const string FX_UI_BOXOPEN = "FX_UI_BOXOPEN";

	public const string FX_COLOSSEUM_UP_EF = "FX_UI_RANKUP";

	public const string FX_COLOSSEUM_DOWN_EF = "FX_UI_RANKDOWN";

	public const string FX_BABELMAIN_EF = "FX_BABEL_MAIN";

	public const string FX_BABELSUB_EF = "FX_BABEL_SUB";

	public const string FX_BABEL_SELECT = "FX_BABEL_SELECT";

	public const string FX_BABEL_MAIN_HARD = "FX_BABEL_MAIN_HARD";

	public const string FX_STARTBUTTON = "FX_STARTBUTTON_UI";

	public const string FX_UI_EVENTFONT = "FX_UI_EVENTFONT";

	public const string FX_HERO_GRADE5_SMALL = "FX_BABEL_SELECT";

	public const string FX_HERO_GRADE5_LARGE = "FX_BABEL_SELECT";

	public const string FX_HERO_GRADE6_SMALL = "FX_BABEL_SELECT";

	public const string FX_HERO_GRADE6_LARGE = "FX_BABEL_SELECT";

	public const string FX_UI_UNLOCK = "FX_UI_UNLOCK";

	public const string FX_DIRECT_UNLOCK = "FX_DIRECT_UNLOCK";

	public const string FX_BABEL_GUILDBOSS_ON = "FX_UI_TOWERBOSS_ON";

	public const string FX_BABEL_GUILDBOSS_OFF = "FX_UI_TOWERBOSS_OFF";

	public const string FX_BABEL_GUILDBOSS_CLOSE = "FX_UI_TOWERBOSS_CLEAR";

	public const string FX_BUTTON_REBIRTH = "FX_BUTTON_REBIRTH";

	public const string FX_GOLDENEGG = "fx_direct_goldegg";

	public const string FX_ANCIENTTREASURE = "fx_ancienttreasure_ui";

	public const string FX_UI_GUILDMARK = "FX_UI_GUILDMARK";

	public const string FX_SOL_COMBINATION5 = "FX_SOL_COMBINATION";

	public const string FX_SOL_COMBINATION4 = "FX_SOL_COMBINATION4";

	public const string FX_SOL_COMBINATION3 = "FX_SOL_COMBINATION3";

	public const string FX_GUILDWAR_VICTORYMARK = "FX_GUILDWAR_VICTORYMARK";

	public static string FX_ITEMMALL_PRODUCT = "FX_READMORE_UI";

	private static GameObject m_efRoot;

	public static bool IsValidParent(GameObject kTarget)
	{
		return null == kTarget || kTarget.transform.childCount == 0;
	}

	public static GameObject Attach(string strName)
	{
		if (null == EffectDefine.m_efRoot)
		{
			EffectDefine.m_efRoot = new GameObject("@effect root");
			UnityEngine.Object.DontDestroyOnLoad(EffectDefine.m_efRoot);
		}
		return new GameObject(strName)
		{
			transform = 
			{
				parent = EffectDefine.m_efRoot.transform
			}
		};
	}
}
