using System;
using UnityEngine;

public static class CDefinePath
{
	private static string ms_strMobilebundlePath = "GameData/MobileBundles/";

	private static string ms_strXMLbundlePath = "GameData/XMLBundles/";

	private static string ms_strXMLPath = "GameData/XML/";

	private static string ms_strNDTPath = "GameData/NDT/";

	public static string QuestNpcGroupURL = "QuestNPCGroup";

	public static string CHARKIND_ATTACKINFO_URL = "CharInfo/CHARKIND_ATTACKINFO";

	public static string CHARKIND_CLASSINFO_URL = "CharInfo/CHARKIND_CLASSINFO";

	public static string CHARKIND_INFO_URL = "CharInfo/CHARKIND_INFO";

	public static string CHARKIND_MONSTERINFO_URL = "CharInfo/CHARKIND_MONSTERINFO";

	public static string CHARKIND_MONSTATINFO_URL = "CharInfo/CHARKIND_MONSTATINFO";

	public static string CHARKIND_NPCINFO_URL = "CharInfo/CHARKIND_NPCINFO";

	public static string CHARKIND_STATINFO_URL = "CharInfo/CHARKIND_STATINFO";

	public static string CHARKIND_ANIMATIONINFO_URL = "CharInfo/CharAniInfo";

	public static string CHARKIND_SOLGRADEINFO_URL = "CharInfo/CHARKIND_SOLGRADEINFO";

	public static string CHARKIND_LEGENDINFO_URL = "CharInfo/CHARKIND_LEGENDINFO";

	public static string CHARKIND_SOLINFO_URL = "CharInfo/CHARKIND_SOLDIERINFO";

	public static string CHARKIND_SOLGUIDE_URL = "CharInfo/SolGuide";

	public static string GMHElpInfo_URL = "CharInfo/GMHelp_Info";

	public static string TEXT_LEVEL_EXP = "CharInfo/level_exp";

	public static string TEXT_GUILD_EXP = "CharInfo/GUILD_EXP";

	public static string WEAPONTYPE_INFO_URL = "CharInfo/WEAPONTYPE_INFO";

	public static string ITEMTYPE_INFO_URL = "ItemCommon/ITEMTYPE_INFO";

	public static string QUEST_COMMON_PATH = "Quest/Quest";

	public static string QUEST_GROUP_PATH = "Quest/QuestGroup";

	public static string QUEST_GROUP_REWARD_PATH = "Quest/Group_Reward";

	public static string NPC_QUEST_MATCH_PATH = "npc_quest_match";

	public static string QUEST_DIALOG_PATH = "quest_dialog";

	public static string QUEST_DIALOG_CHAPTER_PATH = "quest_chapter";

	public static string QUEST_REWARD_PATH = "Quest/Quest_Reward";

	public static string QUEST_CLIENTNPC_INFO = "Quest/ClientNPC_Info";

	public static string QUEST_ATUOQUEST_PATH = "Quest/Auto_Quest";

	public static string QUEST_NPC_POS_PATH = "quest_npc_pos";

	public static string QUEST_DROP_ITEM_PATH = "Quest/quest_drop_items";

	public static string REPUTE_TABLE_PATH = "Quest/Repute_Table";

	public static string REPUTE_REWARD_PATH = "Quest/Repute_Reward";

	public static string CHALLENGE_TIMETABLE_PATH = "Quest/Challenge_Event";

	public static string CHALLENGE_TABLE_PATH = "Quest/Challenge";

	public static string CHALLENGE_EQUIPTABLE_PATH = "Quest/Challenge_Equip";

	public static string POINT_TABLE_PATH = "ItemCommon/pointexchange";

	public static string JEWELRY_TABLE_PATH = "ItemCommon/jewelryexchange";

	public static string POINTLIMIT_TABLE_PATH = "ItemCommon/pointexchangelimit";

	public static string MYTHICSOL_TABLE_PATH = "ItemCommon/mythicexchange";

	public static string GUILDWAR_EXCHANGE_TABLE_PATH = "Guild/GuildWar_Exchange";

	public static string EVENT_EXCHANGE_TABLE_PATH = "ItemCommon/EventExchange";

	public static string EXPLORATION_TABLE_PATH = "ItemCommon/Exploration_Info";

	public static string RESERVEDWORD_TABLE_PATH = "reservedword";

	public static string RECOMMEND_REWARD_TABLE_PATH = "recommend_reward";

	public static string SUPPORTER_REWARD_TABLE_PATH = "Supporter_reward";

	public static string TUTORIAL_BUBBLE_PATH = "quest_tutorial_text";

	public static string FONT_COLOR_PATH = "font_color";

	public static string ECO_TALK_PATH = "CharInfo/Eco_talk";

	public static string ECO_PATH = "CharInfo/Eco";

	public static string EVENTTRIGGER_MAP_PATH = "EventTrigger_MAP_";

	public static string BATTLEMAP_CELLATBINFO_PATH = "BattleMapCellInfo/BATTLEMAP_CELLINFO_";

	public static string WeaponInfoURL = "weapon";

	public static string BattleMapInfoURL = "BattleMapCellInfo/BATTLEMAPINFO";

	public static string BattleMapBundlePath = "Map/Battle/";

	public static string BattlePosURL = "BattleMapCellInfo/BattlePos";

	public static string BattleGridDataURL = "BattleMapCellInfo/battle_grid_data";

	public static string GridCellInfoURL = "battle_grid_cell";

	public static string MagicInfo_detailURL = "magic_detail";

	public static string EffectUnitInfoURL = "effect_unit";

	public static string EffectDataInfoURL = "effect_data";

	public static string ScenarioBattleInfoURL = "scenario_battle_info";

	public static string s_strBattleSkillBaseURL = "BattleSkill/BattleSkill_Base";

	public static string s_strBattleSkillDetailIncludeURL = "BattleSkill/BattleSkill_DetailInclude";

	public static string s_strBattleSkillDetailURL = "BattleSkill/BattleSkill_Detail";

	public static string s_strBattleSkillTrainingURL = "BattleSkill/BattleSkill_Training";

	public static string s_strBattleSkillTrainingIncludeURL = "BattleSkill/BattleSkill_TrainingInclude";

	public static string s_strBattleSkillIconURL = "BattleSkill/BattleSkill_Icon";

	public static string s_strMythSkillTrainingURL = "BattleSkill/MythSkill_Training";

	public static string s_strItemAccessoryURL = "ItemInfo/item_accessory";

	public static string s_strItemArmorURL = "ItemInfo/item_armor";

	public static string s_strItemBoxURL = "ItemInfo/item_box";

	public static string s_strItemMaterialURL = "ItemInfo/item_material";

	public static string s_strItemQuestURL = "ItemInfo/item_quest";

	public static string s_strItemSecondEquipURL = "ItemInfo/item_secondequip";

	public static string s_strItemSuppliesURL = "ItemInfo/item_supplies";

	public static string s_strItemTicketURL = "ItemInfo/item_ticket";

	public static string s_strItemWeaponURL = "ItemInfo/item_weapon";

	public static string s_strItemRankURL = "ItemCommon/item_rank";

	public static string s_strItemComposeURL = "ItemInfo/item_compose";

	public static string s_strItemReforgeURL = "ItemCommon/ITEM_REFORGE";

	public static string s_strItemSkillReinforceURL = "ItemCommon/ITEM_SKILL_REINFORCE";

	public static string s_strItemEvolutionURL = "ItemCommon/ITEM_EVOLUTION";

	public static string s_strExchangeEvolutionURL = "ItemCommon/EXCHANGE_EVOLUTION";

	public static string s_strItemSellURL = "ItemCommon/ITEM_SELL";

	public static string s_strItemBoxGroupURL = "ItemCommon/item_boxgroup";

	public static string s_strItemMakeRankURL = "ItemCommon/make_rank";

	public static string s_strItemGroupSolTicketURL = "ItemCommon/group_sol_ticket";

	public static string s_strLoadingTextURL = "loading_text";

	public static string s_strBuyItemURL = "ItemCommon/buy_item";

	public static string s_strPlunderSupportGoldURL = "ItemCommon/PlunderSupportGold";

	public static string s_strPlunderObjectInfoURL = "Charinfo/Plunder_ObjectInfo";

	public static string s_strBurnningEventInfoURL = "EventInfo";

	public static string s_strColosseumRankRewardURL = "ItemCommon/Colosseum_Rank_Reward";

	public static string s_strColosseumGradeInfoURL = "ItemCommon/Colosseum_GradeInfo";

	public static string COLOSSEUM_CHALLENGE_URL = "CharInfo/Colosseum_ChallengeInfo";

	public static string BATTLE_CONSTANT_URL = "battle_constant";

	public static string COMMON_CONSTANT_URL = "common_constant";

	public static string COLOSSEUM_CONSTANT_URL = "colosseum_constant";

	public static string BATTLE_EMOTICON_URL = "Babel/EMOTICON_INFO";

	public static string EventTriggerInfoURL = "eventtrigger/eventtriggerinfo";

	public static string GameDramaInfoURL = "eventtrigger/gamedramainfo";

	public static string WORLDMAP_INFO_URL = "MapInfo/WORLDMAP_INFO";

	public static string LOCALMAP_INFO_URL = "MapInfo/LOCALMAP_INFO";

	public static string MAP_INFO_URL = "MapInfo/MAP_INFO";

	public static string MAP_UNIT_URL = "MapInfo/MAP_UNIT";

	public static string GATE_INFO_URL = "MapInfo/GATE_INFO";

	public static string EFFECT_INFO_URL = "Effect/EFFECT_INFO";

	public static string SKYBOX_BASE_URL = "MAP/SKYBOX";

	public static string ROADPOINT_BASE_URL = "RoadPoint";

	public static string TILEINFO_BASE_URL = "MapTileInfo";

	public static string BULLETINFO_URL = "Effect/BULLET_INFO";

	public static string CAMERA_SETTINGS_URL = "CameraSettings";

	public static string INDUN_INFO_URL = "IndunInfo/INDUN_INFO";

	public static string BATTLE_SREWARD_URL = "ItemCommon/Special_Reward";

	public static string BATTLE_BABEL_SREWARD_URL = "ItemCommon/Special_Reward_Babel";

	public static string GAMEGUIDE_INFO_URL = "Quest/GameGuide";

	public static string ADVENTURE_INFO_URL = "Quest/Adventure_Info";

	public static string INVITE_PERSONCOUNT_URL = "Quest/FRIEND_RECOMMEND";

	public static string FACEBOOK_FEED_URL = "FaceBook/Feed";

	public static string SOLCOMPOSEEXP_URL = "CharInfo/ComposeExpTable";

	public static string SOLDIER_EVOLUTIONEXP_URL = "CharInfo/Evolution_EXP";

	public static string SOLDIER_TICKETINFO_URL = "CharInfo/Ticket_Info";

	public static string BABELTOWER_URL = "Babel/BabelTower_Info";

	public static string BABEL_GUILDBOSS_URL = "Babel/GuildBoss_Info";

	public static string s_strItemMallURL = "ItemCommon/item_Shop";

	public static string ITEM_REDUCE_URL = "ItemCommon/ITEM_REDUCE";

	public static string ITEMSKILL_URL = "ItemCommon/ITEM_SKILL";

	public static string s_strPoPupShop_URL = "ItemCommon/popup_shop";

	public static string MineConstantURL = "Mine/mine_constant";

	public static string MineDataURL = "Mine/mine_data";

	public static string MineCreateDataURL = "Mine/minecreate_data";

	public static string SOLWAREHOUSE_URL = "CharInfo/HERO_WAREHOUSE";

	public static string CHARCHANGE_URL = "CharInfo/char_spend";

	public static string REINCARNATION_URL = "CharInfo/Reincarnation_Info";

	public static string DAILY_DUNGEON_URL = "Event/Daily_Dungeon";

	public static string DAILY_GIFT_URL = "Event/dailygift";

	public static string SOL_EXTRACTRATE_URL = "charinfo/sol_extract_rate";

	public static string SEASON_EXP_PENALTY_URL = "CharInfo/Evolution_Panalty";

	public static string FriendCountLimitDataURL = "Quest/Friend_limit";

	public static string BOUNTYINFO_URL = "Bounty/Bounty_Info";

	public static string BOUNTYECO_URL = "Bounty/BountyEco";

	public static string ExpeditionDataURL = "expedition/expedition_data";

	public static string ExpeditionConstantURL = "expedition/expedition_constant";

	public static string ExpeditionCrateDataURL = "expedition/expedition_create_data";

	public static string GameWebCallURLData_URL = "gamewebcallurldata";

	public static string AGIT_INFO_URL = "guild/guild_agit_info";

	public static string AGIT_NPC_URL = "guild/guild_agit_NPC";

	public static string AGIT_MERCHANT_URL = "guild/guild_agit_merchant";

	public static string AGIT_EGG_URL = "guild/guild_agit_egg";

	public static string NEWGUILD_CONSTANT = "guild_constant";

	public static string GUILDWAR_REWARD = "guild/guildwar_reward";

	public static string TRANSCENDENCE_COST_URL = "transcendence/transcendence_cost";

	public static string TRANSCENDENCE_RATE_URL = "transcendence/transcendence_rate";

	public static string TRANSCENDENCE_SOL_URL = "transcendence/transcendence_sol";

	public static string TRANSCENDENCE_FAILREWARD_URL = "transcendence/transcendence_failreward";

	public static string LEVELUP_GUIDE_URL = "charinfo/levelupguide";

	public static string GAMEHELP_INFO_URL = "gamehelplist";

	public static string ACHIVEMENT_INFO_URL = "quest/achivement_google";

	public static string SET_ITEM_DATA_URL = "iteminfo/set_item_info";

	public static string MYTHRAID_INFO = "raid/raid_info";

	public static string SOLCOMBINE_INFO_URL = "battleskill/sol_combination_skill";

	public static string MYTHRAID_CLEAR_REWARD = "raid/raid_clear_reward";

	public static string MYTHRAID_RANK_REWARD = "raid/raid_rank_reward";

	public static string MYTHRAID_GuardianAngel = "raid/raid_guardianangel";

	public static string ITEM_BREAK_URL = "itemcommon/item_break";

	public static string ITEM_RATE_URL = "rate_openurl";

	public static string VIPSUBINFO_URL = "vip/vip_sub";

	public static string AUTOSELL_INFO_URL = "itemcommon/auto_sell_table";

	public static string CHAR_COSTUME = "charinfo/costume_info";

	public static string ATTENDANCE = "Event/attendance";

	public static string EMULATOR_INFO_URL = "emulator";

	public static string TIMESHOP_URL = "itemcommon/item_timeshop";

	public static string TIMESHOPREFRESH_URL = "itemcommon/item_timeshop_refresh";

	public static string NEWEXPLORATION_INFO_URL = "NewExploration/NewExploration_info";

	public static string NEWEXPLORATION_SREWARD_URL = "NewExploration/NewExploration_special_reward";

	public static string NEWEXPLORATION_TREASURE_URL = "NewExploration/NewExploration_treasureBox";

	public static string NEWEXPLORATION_RESET_INFO_URL = "NewExploration/NewExploration_reset_Info";

	public static string NEWEXPLORATION_RANK_REWARD_INFO_URL = "NewExploration/NewExploration_Rank_reward";

	public static string MYTH_EVOLUTION_URL = "charinfo/myth_evolution_spend";

	public static string MOVIE_URL = "movieurl";

	public static string WebData()
	{
		return NrTSingleton<NrGlobalReference>.Instance.basePath;
	}

	public static string MobileXmlDataPath()
	{
		string text = Application.dataPath + "/Mobile/";
		string text2 = "file://" + text.Substring(text.IndexOf('/'));
		Debug.Log("==============================" + text2);
		return text2;
	}

	public static string XMLBundlePath()
	{
		if (!TsPlatform.IsMobile)
		{
			return CDefinePath.ms_strXMLbundlePath;
		}
		return CDefinePath.ms_strMobilebundlePath;
	}

	public static string XMLPath()
	{
		return CDefinePath.ms_strXMLPath;
	}

	public static string NDTPath()
	{
		return CDefinePath.ms_strNDTPath;
	}

	public static bool CheckURLKeyWord(string Key)
	{
		string text = Application.dataPath;
		Key = Key.ToUpper();
		text = text.ToUpper();
		int num = text.IndexOf(Key);
		return 0 < num;
	}
}
