using System;

namespace GAME
{
	public static class CommonDefine
	{
		public enum USECOIN_TYPE
		{
			USE_ITEMMALL = 1,
			USE_SOLDIERRECRUIT_ONE,
			USE_SOLDIERRECRUIT_TEN,
			USE_WAREHOUSE,
			USE_TIMESHOP_REFRESH,
			USE_TIMESHOP_BUY,
			GET_EXCHANGE_ITEM = 10,
			GET_ITEMMALL
		}

		public enum REASON_MONEY
		{
			REASON_GET_MONEY_GM = 901,
			REASON_GET_MONEY_QUEST,
			REASON_GET_MONEY_BATTLE,
			REASON_GET_MONEY_INDUN,
			REASON_GET_MONEY_SELLITEM,
			REASON_GET_MONEY_SELLHERO,
			REASON_GET_MONEY_POST,
			REASON_GET_MONEY_ITEMSHOP,
			REASON_GET_MONEY_PLUNDER_SUPPORT,
			REASON_GET_MONEY_BATTLEBONUS,
			REASON_GET_MONEY_CHALLENGE,
			REASON_GET_MONEY_PLUNDER,
			REASON_GET_MONEY_EXPLORATION,
			REASON_GET_MONEY_ITEMUSE,
			REASON_GET_MONEY_NEWGUILDBOSSBATTLE,
			REASON_GET_MONEY_FRIENDHELP,
			REASON_GET_MONEY_SUPPORT,
			REASON_USE_MONEY_POSTFEE = 1001,
			REASON_USE_MONEY_HEROSKILL,
			REASON_USE_MONEY_ENHANCEITEM,
			REASON_USE_MONEY_GM,
			REASON_USE_MONEY_HERO_COMPOSE,
			REASON_USE_MONEY_HERO_ENHANCE,
			REASON_USE_MONEY_PLUNDER_REMATCH,
			REASON_USE_MONEY_PLUNDER_CHANGE,
			REASON_USE_MONEY_ACTIVEPOWER,
			REASON_USE_MONEY_ITEMSHOP,
			REASON_USE_MONEY_MINESEARCH,
			REASON_USE_MONEY_AUCTIONREGISTER,
			REASON_USE_MONEY_CHANGECLASS,
			REASON_USE_MONEY_CREATEGUILD,
			REASON_USE_MONEY_BATELE_SUPPORT,
			REASON_USE_MONEY_PLUNDER_OBJECT_BATCH,
			REASON_USE_MONEY_AUCTION = 1018,
			REASON_USE_MONEY_HERO_CURE,
			REASON_USE_MONEY_ELEMENT_SOLGET,
			REASON_USE_MONEY_REINCARNATION,
			REASON_USE_MONEY_MAILBOX_SEND_GUILD,
			REASON_USE_MONEY_EXPEDITION_SEARCH,
			REASON_USE_MONEY_ELEMENT_LEGENDSOLGET,
			REASON_USE_MONEY_TRANSCENDENCE
		}

		public const int MASTER_LEVEL = 100;

		public const int DEFAULT_CHATCHANNEL = 1;

		public const int MAX_CHATCHANNEL_NUM = 300;

		public const int BAD_CHAT_USER_CHANNEL = 301;

		public const int MAX_CHATCHANNEL_NUM_NUL = 302;

		public const int MAX_INUSER_CHANNEL_RNDNUM = 10;

		public const short TICK_PER_SECOND = 1000;

		public const int TICK_PER_MINUTE = 60000;

		public const int KEEPALIVE_DISCONNECT = 300000;

		public const int HEARTBEAT_INTERVAL = 60000;

		public const int CONGRATURATION_TIME_FRIEND = 180000;

		public const int CONGRATURATION_TIME_ALL = 300000;

		public const int CONGRATURATION_TIME_SPECIAL = 180000;

		public const int AUTHKEY_LEN = 36;

		public const int AUTHKEY_LEN_NUL = 37;

		public const int IP_LEN = 20;

		public const int IP_LEN_NUL = 21;

		public const int CHINA_COCO_USERID_LEN = 32;

		public const int CHINA_COCO_CHANNEL_LEN = 32;

		public const int CHINA_COCO_UID_LEN = 64;

		public const int CHINA_COCO_TOKEN_LEN = 128;

		public const int CHINA_SDK_KEY_NUM = 10;

		public const int CHINA_SDK_KEY_LEN = 32;

		public const int CHINA_SDK_VALUE_LEN = 256;

		public const int CHINA_SDK_PARAM_LEN = 2048;

		public const int JAPAN_LINE_USERID_LEN = 256;

		public const int JAPAN_SDK_PARAM_LEN = 2048;

		public const int JAPAN_BILLING_PARAM_LEN = 1024;

		public const int JAPAN_BILLING_PARAM_SMALL_LEN = 128;

		public const int JAPAN_BILLING_STATUS_LEN = 10;

		public const int JAPAN_BILLING_ERRORCODE_LEN = 5;

		public const int JAPAN_BILLING_ORDERID_LEN = 33;

		public const int JAPAN_BILLING_PRODUCTID_LEN = 21;

		public const int JAPAN_BILLING_CPID_LEN = 11;

		public const int JAPAN_LINE_ORDERID_LEN = 33;

		public const int JAPAN_LINE_RTNCD_LEN = 4;

		public const int JAPAN_LINE_PAYSEQ_LEN = 15;

		public const int JAPAN_LINE_HEADER_LEN = 512;

		public const int JAPAN_LINE_DATA_LEN = 1024;

		public const int MEGAPACKET_SIZE = 1048576;

		public const int MAX_LASTRECEIVEMSG_NUM = 10;

		public const short CHAT_LEN = 100;

		public const short MAILBOX_COUNT = 5;

		public const short MAILBOX_ITEM_COUNT = 5;

		public const short MAILBOX_SOLEXP_COUNT = 15;

		public const short MAILBOX_COMMENT_LEN = 256;

		public const short MAILBOX_COMMENT_LEN_NUL = 257;

		public const short MAILBOX_BINARY_DATA_SIZE = 200;

		public const short MAILBOX_SENDOBJECTNAME_LEN = 64;

		public const short MAILBOX_SENDOBJECTNAME_LEN_NUL = 65;

		public const byte RESERVED_SLOT_MAX = 20;

		public const byte MESSAGE_LEN = 200;

		public const short RECEIVE_MAIL_MAX = 32;

		public const int MAX_NOTE_COUNT = 1;

		public const byte NOTE_OWN_MAX = 5;

		public const int MAX_BULLET_NAME_LENGTH = 21;

		public const int MAX_NAME_LENGTH = 20;

		public const int MINE_CHARKIND = 242;

		public const int MAX_PLUNDER_STARTPOS_INDEX = 3;

		public const long DEFAULT_MATCH_POINT = 1000L;

		public const int DEFAULT_COLOSSEUM_GRADE_POINT = 1000;

		public const int COLOSSEUM_TOPGUILD_RANK = 3;

		public const int MAX_STORYCHAT_LENGTH = 201;

		public const int MAX_STORYCOMMENT_LENGTH = 51;

		public const int MAX_CHALLENGE_NUM = 40;

		public const short MAX_ONE_COLUMN_FLOORCOUNT = 63;

		public const short DEFAULT_BABELTOWER_FLOOR = 1;

		public const short DEFAULT_BABELTOWER_SUBFLOOR = 1;

		public const short MAX_BABELTOWER_SUBFLOORCOUNT = 5;

		public const short MAX_BABEL_TOWER_FLOOR = 101;

		public const int MAX_BABELTOWER_PARTY_NUM = 4;

		public const int MAX_BABELTOWER_HELP_FRIEND_NUM = 3;

		public const int ITEMUNIQUE_HEARTS = 70000;

		public const int ITEMUNIQUE_HEARTS_PICE = 70001;

		public const int ITEMUNIQUE_ELIXIR = 70005;

		public const int ITEMUNIQUE_BANDAGE = 70006;

		public const int ITEMUNIQUE_ORIHARCON = 50305;

		public const int ITEMUNIQUE_GOLDBOX_100 = 70014;

		public const int ITEMUNIQUE_GOLDBOX_1000 = 70015;

		public const int ITEMUNIQUE_SOULGEM = 70002;

		public const int ITEMUNIQUE_MYTHELXIR = 50311;

		public const int ITEMUNIQUE_MYTH_LEGEND_ELIXIR = 50313;

		public const int ITEMUNIQUE_MYTH_DRAGON_ELXIR = 50316;

		public const int ITEMUNIQUE_MYTH_TIME_ELXIR = 50317;

		public const int PRO_DEFULT_NUM = 10000;

		public const long AUCTION_COST_GOLD_MAX = 99999999999L;

		public const long AUCTION_COST_HEARTS_MAX = 10000L;

		public const string MONEY_ICON_KEY = "Com_I_MoneyIconM";

		public const string HEARTS_ICON_KEY = "Win_I_Hearts";

		public const int MAX_MINE_USER_NUM = 9;

		public const int MAX_MINE_USER_PER_STARTPOS = 3;

		public const int MAX_MINE_BATCH_USER_NUM = 5;

		public const int MAX_MINE_WAIT_GUILDCOUNT = 10;

		public const int MINE_RESULT_LIST_PER_PAGE = 5;

		public const int LOAD_LEGION_INFO_PER_COUNT = 20;

		public const short DEFAULT_MINE_NAME_ID = 1;

		public const int MAX_NEWEXPLORATION_BATCH_USER_NUM = 5;

		public const int MAX_NEWEXPLORATION_MONSTER_NUM = 12;

		public const int MAX_GUILDBOSS_BATCH_USER_NUM = 9;

		public const short MAX_GUILDBOSS_MAX_FLOOR = 16;

		public const int MAX_COLOSSEUM_CHALLENGE_PAGEPER_HERO = 8;

		public const int MAX_COLOSSEUM_MATCH_LIST = 1000;

		public const int MAX_LAST_MATCH_PERSONID_NUM = 3;

		public const int SHOW_DIRECTION_LEVEL = 20;

		public const int MAX_ACTIVE_EVENT_NUM = 7;

		public const int TEXTNOTIFY_MAX = 64;

		public const int AUTOBUY_SELL_MAX = 30;

		public const int MULTI_LOCK_MAX = 30;

		public const int POINT_BUY_ITEM_MAX = 10;

		public const int EVENT_DAILY_DUNGEON_ECONUM = 10;

		public const int MAX_LOG_TEXT = 50;

		public const int MAX_LOG_TEXT_NULL = 51;

		public const int MAX_BOUNTY_FIX_POS = 5;

		public const int MAX_BOUNTY_CHARKIND = 11;

		public const int MAX_BOUNTYHUNT_PARTY_NUM = 4;

		public const int MAX_BOUNTYHUNT_HELP_FRIEND_NUM = 3;

		public const int EXPEDITION_MAX_SOLNUM = 15;

		public const int MAX_EXPEDITION_ECO_NUM = 3;

		public const int MAX_EXPEDITION_BATCH_POSINDEX_NUM = 3;

		public const int MAX_EXPEDITION_BATCH_SOL_NUM = 5;

		public const int MAX_EXPEDITION_SOL_NUM = 15;

		public const int MAX_EXPEDITION_USER_PER_STARTPOS = 5;

		public const int AUCTION_TABLE_SKILL_MAX = 3;

		public const int MAX_GUILDWAR_USER_NUM = 9;

		public const int MAX_GUILDWAR_EXCHANGE_REWARD_COUNT = 30;

		public const long DAY_TO_SECOND = 86400L;

		public const long MINUTE_TO_SECOND = 60L;

		public const long HOUR_TO_SECOND = 3600L;

		public const int MAX_DAILY_ATTEND_LEVEL_CASE = 11;

		public const int MAX_SOL_EXTRACT_RATE_COUNT = 10;

		public const short MAX_GAMEHELP_LIST = 10;

		public const int MAX_MYTHRAID_PARTY_NUM = 4;

		public const int MAX_MYTHRAID_UNIQUE = 10;

		public const int MAX_MYTHRAID_RANK = 100;

		public const int MAX_MYTHRAID_RANKINFO_PAGE = 10;

		public const int MAX_MYTHRAID_REWARD = 7;

		public const short MAX_MYTHRAID_ROUND = 6;

		public const short MAX_CHALLENGE_EVENT_COUNT = 13;

		public const short MAX_CHALLENGE_EVENT_UNIQUE = 1150;

		public const int CHALLENGEEVENT_NEEDITEMUNIQUE = 50902;

		public const int CHALLENGECONTINUE_NEEDITEMUNIQUE = 78358;

		public const short ITEMMALL_POPUPSHOP_ATB_LEN = 100;

		public const short MAX_ITEMMALL_POPUPSHOP_ATB_DATA_COUNT = 10;

		public const int MAX_VIP_NUM = 11;

		public const long ATB_TARGETWEAPONTYPE_ALL = 1L;

		public const long ATB_TARGETWEAPONTYPE_SWORD = 2L;

		public const long ATB_TARGETWEAPONTYPE_SPEAR = 4L;

		public const long ATB_TARGETWEAPONTYPE_AXE = 8L;

		public const long ATB_TARGETWEAPONTYPE_BOW = 16L;

		public const long ATB_TARGETWEAPONTYPE_GUN = 32L;

		public const long ATB_TARGETWEAPONTYPE_CANNON = 64L;

		public const long ATB_TARGETWEAPONTYPE_STAFF = 128L;

		public const long ATB_TARGETWEAPONTYPE_BIBLE = 256L;

		public const short MAX_DAILYDUNGEON_LIMIT = 5;

		public const int MAX_DAILYDUNGEON_SOLIDER_NUM = 6;

		public const string DAILYDUNGEON_SOLDIERBATCH_KEY = "DailyDungeon Solpos";

		public const short MAX_MYTHEVOLUTION_NUM = 2;

		public const short MAX_MYTHEVOLUTION_UPGRADE_GRADE = 4;
	}
}
