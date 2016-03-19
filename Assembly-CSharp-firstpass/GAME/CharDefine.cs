using System;

namespace GAME
{
	public static class CharDefine
	{
		public const int ACCOUNT_LEN = 64;

		public const int ACCOUNT_LEN_NUL = 65;

		public const int PASSWORD_LEN = 40;

		public const int PASSWORD_LEN_NUL = 41;

		public const int ACCESSTOKEN_LEN = 100;

		public const int ACCESSTOKEN_LEN_NUL = 101;

		public const int ACCOUNT_ENCRYPT_LEN = 255;

		public const int ACCOUNT_ENCRYPT_LEN_NUL = 256;

		public const int MIN_ACCOUNT_LEN = 6;

		public const int MOBILEDEVICETOKEN_LEN = 512;

		public const int MOBILEDEVICETOKEN_LEN_NUL = 513;

		public const int APKVERSION_LENGTH = 16;

		public const int DEVICEID_LENGTH = 32;

		public const long INVALID_UID = -1L;

		public const long INVALID_PersonID = -1L;

		public const short INVALID_CHARUNIQUE = -1;

		public const long INVALID_SOLID = 0L;

		public const byte CHAR_NAME_LEN = 20;

		public const byte CHAR_NAME_LEN_NUL = 21;

		public const byte INTRO_MSG_LEN = 40;

		public const byte INTRO_MSG_LEN_NUL = 41;

		public const int MAX_SOL_NUM = 6;

		public const int MAX_READY_SOL_NUM = 50;

		public const int MAX_SOLGRADE_NUM = 15;

		public const int DEFAULT_BATTLEPOS_WIDTH = 3;

		public const int DEFAULT_BATTLEPOS_HEIGHT = 3;

		public const int MAX_BATTLEPOS_NUM = 9;

		public const byte UPGRADE_GRADE_UP_TYPE = 1;

		public const byte UPGRADE_COMPOSE_UP_TYPE = 2;

		public const byte UPGRADE_LEVEL_UP_TYPE = 3;

		public const float COMPOSE_LEVEL_PER_CONST = 20f;

		public const float COMPOSE_EXP_SUCCESSION = 0.5f;

		public const int COMPOSE_MAX_SOL_NUM = 50;

		public const int MAX_SOL_RECRUIT_NUM = 11;

		public const int LEGEND_GRADE_START = 6;

		public const int LEGEND_GRADE_END = 9;

		public const short SOLDIER_LEVEL_MAX = 200;

		public const byte PREMIUM_SOLDIER_GRADE = 4;

		public const int MAX_SOL_SEASON_NUM = 10;

		public const byte NPC_MENU_MAX = 6;

		public const int MAX_SOL_EQUIP_PACKET_SIZE = 20;

		public const int MAX_MILITARY_NUM = 10;

		public const int LEADER_SOL_INDEX = 0;

		public const float DEFAULT_ANIEVENT_TIME = 0f;

		public const byte MAX_CHARSPEED = 250;

		public const byte MIN_CHARSPEED = 10;

		public const float ADJUST_CHARSPEED_DIV = 10f;

		public const float SERVERCHAR_SPEEDRATIO = 1.5f;

		public const int CHARMOVE_CHECK_INTERVAL = 10000;

		public const int MAX_SOL_HP = 1000000;

		public const int MAX_SOL_MP = 1000000;

		public const long DEFAULT_MAX_FOOD_NUM = 500L;

		public const byte MAX_ACTIVITY_NUM = 20;

		public const int INIT_ACTIVITY_HOUR = 6;

		public const int MAX_REPUTE_NUM = 20;

		public const int MAX_REPUTEREWARD_ITEM_NUM = 3;

		public const int MAX_REPUTERE_QUESTUNIQUE_NUM = 10;

		public const int MAX_SOL_SUBDATA_PACKET_NUM = 10;

		public const long ATB_CHAR_INACTIVE = 1L;

		public const long ATB_CHAR_STATE_IDLE = 2L;

		public const long ATB_CHAR_STATE_SIT = 4L;

		public const long ATB_CHAR_STATE_MOVEING = 8L;

		public const long ATB_CHAR_STATE_MOVEINGFORBATTLE = 16L;

		public const long ATB_CHAR_STATE_DIED = 32L;

		public const long ATB_CHAR_STATE_LOADING = 64L;

		public const long ATB_CHAR_STATE_TRAINING = 128L;

		public const long ATB_CHAR_STATE_LEAGUE = 256L;

		public const long ATB_CHAR_STATE_COLLECT = 512L;

		public const long ATB_CHAR_STATE_TERRITORY = 1024L;

		public const long ATB_CHAR_STATE_DRAMA = 2048L;

		public const long ATB_CHAR_STATE_CLOSING = 4096L;

		public const long ATB_CHAR_ETC_ALLYPC = 8192L;

		public const long ATB_CHAR_BATTLE_ING = 16384L;

		public const long ATB_CHAR_BATTLE_OBSERVER = 32768L;

		public const long ATB_CHAR_INDUN = 65536L;

		public const long ATB_CHAR_INDUN_UPDATE_STOP = 131072L;

		public const long ATB_CHAR_PLUNDER_PREPARE = 262144L;

		public const long ATB_CHAR_REPLAY_PLAY = 524288L;

		public const long ATB_CHAR_STATE_MOVESTOP = 7392L;

		public const int MAX_RIDE_CODE_LENGTH = 32;

		public const int DEFAULT_USER_BATTLEGRID_ID = 0;

		public const int DEFAULT_PLUNDER_BATTLEGRID_ID = 2;

		public const int PLUNDER_TREASURE_BOX_KIND = 916;

		public const byte PLUNDER_TREASURE_BOX_GRID_POS = 10;

		public const int PLUNDER_SOLARRAY_RATE = 3;

		public const int DEFAULT_BABEL_TOWER_GRID_ID = 4;

		public const int DEFAULT_GUILD_BOSS_GRID_ID = 3;

		public const int MAX_CHARKIND_NUM = 5000;

		public const int MAX_CHARKIND_CODE_LENGTH = 32;

		public const int MAX_CHARKIND_NAME_LENGTH = 32;

		public const int MAX_CLASSTYPE_CODE_LENGTH = 32;

		public const int MAX_EXPTYPE_CODE_LENGTH = 32;

		public const int CHARKIND_HERO = 9999;

		public const int CHARKIND_NULL = 0;

		public const long CLS_ALL = 9223372036854775807L;

		public const int MAX_SOL_CHARKIND_NUM = 500;

		public const int MAX_RIDEKIND_NUM = 50;

		public const long ATB_USER = 1L;

		public const long ATB_SOLDIER = 2L;

		public const long ATB_MONSTER = 4L;

		public const long ATB_NPC = 8L;

		public const long ATB_OBJECT = 16L;

		public const long ATB_WIDECOLL = 32L;

		public const long ATB_COLLECT = 64L;

		public const long ATB_BOSS = 128L;

		public const long ATB_NOLOOTING = 256L;

		public const long ATB_BUYITEMNPC = 512L;

		public const long ATB_GOAL = 1024L;

		public const long ATB_FINDGOAL = 2048L;

		public const long ATB_INTERACTION = 4096L;

		public const long ATB_DONTMOVE = 8192L;

		public const long ATB_INTRUSION = 16384L;

		public const long ATB_FIRSTSTRIKE = 32768L;

		public const long ATB_RUN = 65536L;

		public const long ATB_RUNAWAY = 131072L;

		public const long ATB_TALK = 262144L;

		public const long ATB_TALKTOUSER = 524288L;

		public const long ATB_NOSHADOW = 1048576L;

		public const long ATB_BLACKSMITH = 2097152L;

		public const long ATB_MULTIANI = 4194304L;

		public const long ATB_CHARCHANGE = 8388608L;

		public const long ATB_NAMEUI = 16777216L;

		public const long ATB_TERRITORYITEMBUY = 33554432L;

		public const long ATB_DIEANI = 67108864L;

		public const long ATB_LOOPANI = 134217728L;

		public const long ATB_ITEMUSECOLLECT = 268435456L;

		public const long ATB_INDUNNPC = 536870912L;

		public const long ATB_TREASURE = 1073741824L;

		public const long ATB_MULTIWEAPON = 2147483648L;

		public const long ATB_GUILDNPC = 4294967296L;

		public const long ATB_COLLECTFREE = 8589934592L;

		public const long ATB_IMGTALK = 17179869184L;

		public const long ATB_BATTLE_RUNAWAY = 34359738368L;

		public const long ATB_BATTLE_BOSSDEF = 68719476736L;

		public const long ATB_SUMMONER = 137438953472L;

		public const long ATB_CLASSCHANGE = 274877906944L;

		public const long ATB_EQUIPREDUCE = 549755813888L;

		public const long ATB_AWAKENING = 1099511627776L;

		public const long ATB_EQUIPSKILL = 2199023255552L;

		public const long ATB_GMHELPSOL = 4398046511104L;

		public const long ATB_BATTLE_NOTURN = 8796093022208L;

		public const long ATB_BATTLE_ONLYSKILL = 17592186044416L;

		public const long ATB_BATTLE_IMMUNE_NOTURN_ON = 35184372088832L;

		public const long ATB_BATTLE_IMMUNE_NOSKILL_ON = 70368744177664L;

		public const long ATB_BATTLE_NO_AI_ATTACK = 140737488355328L;

		public const long ATB_BATTLE_ONLY_SKILL = 281474976710656L;

		public const long ATB_TREASURE_BOX = 562949953421312L;

		public const long ATB_BOUNTYHUNT = 1125899906842624L;

		public const long ATB_CC_RESIST = 2251799813685248L;

		public const long ATB_JEWELRYEXCHANGE = 4503599627370496L;

		public const long ATB_BATTLE_IMMUNE_NOCONTROL_ON = 9007199254740992L;

		public const long ATB_AGIT_NPC = 18014398509481984L;

		public const long ATB_MYTHICEXCHANGE = 36028797018963968L;

		public const long ATB_RIDE_HORSE = 1L;

		public const long ATB_RIDE_CAMEL = 2L;

		public const int MAX_SUBCHAR_NUM = 10;

		public const int MAX_MOVEINFO_BUFFER_NUM = 100;

		public const int MAX_STATUSINFO_BUFFER_NUM = 100;

		public const int MAX_MAKEINFO_BUFFER_NUM = 144;

		public const int MAX_DELETEINFO_BUFFER_NUM = 144;

		public const int MAX_SEND_NEARCHAR_NUM = 144;

		public const int MAX_ECO_FIXPOS_NUM = 5;

		public const int MAX_ECO_RANPOS_NUM = 5;

		public const int MAX_ECO_MOVEPOS_NUM = 5;

		public const int MAX_ECO_LEVEL_NUM = 5;

		public const int MAX_ECO_CHAR_NUM = 6;

		public const float PATROL_POINT_RANGE = 2f;

		public const int ECO_MOVE_DELAYTIME = 10;

		public const byte BATTLE_LIMIT_LEVEL = 20;

		public const byte BATTLE_LIMIT_SOLDIER = 4;

		public const short SOL_ITEMREQUEST_LEVELGROUP_MAX = 100;

		public const int BATTLESKILL_WEAPON_COUNT = 3;

		public const int BATTLESKILL_OWN_MAX = 6;

		public const int BATTLESKILL_BUF_MAX = 12;

		public const int MAX_BATTLESKILL_TRAINING_CHAR_NUM = 10;

		public const int MAX_BATTLE_SREWARD_PRODUCT = 4;

		public const int MAX_COUNT_BATTLESREWARD_LEVEL = 20;

		public const short MAX_PUSHTOKEN_LEN = 512;

		public const short MAX_PUSHTOKEN_LEN_NULL = 513;

		public const short MAX_PUSHTOKEN_SHA1_LEN = 40;

		public const short MAX_PUSHTOKEN_SHA1_LEN_NULL = 41;

		public const short MAX_RIVAL_TARGET_NUM = 3;

		public const short MAX_RIVALGROUP_TARGET_NUM = 50;

		public const int FACEBOOK_FRIEND_INFO_COUNT = 5;

		public const int PLUNDER_SHIELD_TIME = 180;

		public const short MAX_DUCTILE_ENGINEERING = 5;

		public const int REINCARNATION_AFTER_GRADE = 0;

		public const int MAX_SEASON = 6;

		public const int MAX_TREASURE_ITEM_REWARD = 5;

		public const byte MAX_GRADE = 5;

		public const byte MAX_INFIBATTLE_REWARD_INFO = 9;

		public const long ATB_COMMONFLAG_COLOSSEUM_INVITE = 1L;

		public const int MAX_EXPEDITION_MILITARY_NUM = 3;

		public const byte MAX_GUILDWAR_MILITARY_NUM = 10;

		public const long SOL_ATB_COMMONFLAG_LOCK = 1L;

		public const long SOL_ATB_COMMONFLAG_RINGSLOT_LOCK = 2L;

		public const long SOL_ATB_COMMONFLAG_AWAKENING = 4L;

		public const long SOL_ATB_COMMONFLAG_ONLYSKILL = 8L;

		public const byte MAX_TRANSCENDENCE_BASE_SEASON = 10;

		public static int MAX_HIT_COUNT = 5;
	}
}
