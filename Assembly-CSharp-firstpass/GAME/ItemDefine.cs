using System;

namespace GAME
{
	public static class ItemDefine
	{
		public const float ITEM_IMAGE_WIDTH = 50f;

		public const float ITEM_IMAGE_HEIGHT = 50f;

		public const int IMAGE_LINE_COUNT_X = 20;

		public const int IMAGE_LINE_COUNT_Y = 20;

		public const int MAX_ITEMTYPE_CODE_LENGTH = 32;

		public const int MAX_GROUPDROP_CODE_LENGTH = 32;

		public const int MAX_WEAPON_CODE_LENGTH = 10;

		public const int MAX_CHARKIND_GRID_LENGTH = 25;

		public const int MAX_SENDDB_DURATION_COUNT = 10;

		public const byte ITEM_OPTION_MAX = 10;

		public const int ITEM_OVER_MAX_NUM = 9999999;

		public const int N_BOX_CREATE_ITEM_COUNT = 12;

		public const int MAX_ITEMUNIQUE_NUM = 5000;

		public const byte COMPOSEITEM_MAX_MATERIAL_NUM = 10;

		public const byte COMPOSEITEM_MAX_CREAT_NUM = 1;

		public const byte COMPOSEITEM_MAX_RESERVED_ITEM_NUM = 11;

		public const int N_MARKET_SELL_MAX = 30;

		public const int N_MARKET_BUY_MAX = 500;

		public const long L_RETURN_CYCLE = 30L;

		public const long L_RETURN_ITEM_CYCLE = 60L;

		public const long MAX_MARKET_MONEY = 100000000L;

		public const int N_STACK_COUNT = 30000;

		public const int N_STACK_LENGTH = 7;

		public const int N_ITEM_AREA = 100000;

		public const int N_MAKE_STACK = 1000;

		public const int N_MAKE_NO_STACK = 3;

		public const int N_OPTION_MAX = 10;

		public const int N_MAKE_MATERIAL_MAX = 10;

		public const int N_BOX_ITEM_MAX = 12;

		public const int N_QUEST_LINK_COUNT = 3;

		public const int N_SUPPLIES_PARAM_COUNT = 2;

		public const int N_DURABILITY_NORMAL = 21;

		public const int N_DURABILITY_DISABLE = 0;

		public const string STR_DURABILITY_NORMAL = "1101";

		public const string STR_DURABILITY_PROGRESS = "1106";

		public const string STR_DURABILITY_DISABLE = "1401";

		public const int N_SLOT_WIDTH_COUNT = 4;

		public const int N_SLOT_WIDTH_OFFSET = 65;

		public const int N_SLOT_WIDTH = 114;

		public const int N_SLOT_HIEGHT = 114;

		public const float N_SLOT_ITEM_IMAGE_WIDTH = 100f;

		public const float N_SLOT_ITEM_IMAGE_HEIGHT = 100f;

		public const int INVENTORY_INVENTORYNUM_MAX = 6;

		public const int INVENTORY_MULTISELL_ITEMNUM_MAX = 30;

		public const int INVENTORY_ITEMNUM_DEFAULT = 30;

		public const int N_AUTO_BUY_GRADE_COUNT = 11;

		public const int N_SOLDIER_ONLY_WEAPON_QUALITY_LEVEL = 1000;

		public const double D_HOUSER = 0.753;

		public const double D_ITEM_HOUSER = 0.5886;

		public const string STR_MAKERANK_D = "1101";

		public const string STR_MAKERANK_C = "1106";

		public const string STR_MAKERANK_B = "1304";

		public const string STR_MAKERANK_A = "1402";

		public const string STR_MAKERANK_S = "1107";

		public const string STR_MAKERANK_SS = "1401";

		public const long ATB_ITEMTYPE_OBJECT = 1L;

		public const long ATB_ITEMTYPE_NOTRADE = 2L;

		public const long ATB_ITEMTYPE_MAKE = 4L;

		public const long ATB_ITEMTYPE_BREAK = 8L;

		public const long ATB_WEAPON_ATTACK = 1L;

		public const long ATB_WEAPON_DEFENSE = 2L;

		public const long ATB_WEAPON_INVERSE = 4L;

		public const long ATB_WEAPON_DOUBLE = 8L;

		public const long ATB_WEAPON_WITHSHIELD = 16L;

		public const long ATB_ITEM_SHOW = 1L;

		public const long ATB_ITEM_TRADE_USER = 2L;

		public const long ATB_ITEM_TRADE_FRIEND = 4L;

		public const long ATB_ITEM_ENABLEUSE = 8L;

		public const long ATB_ITEM_RANDOMBOX = 16L;

		public const long ATB_ITEM_SELECTBOX = 32L;

		public const long ATB_ITEM_ALLBOX = 64L;

		public const long ATB_ITEM_SYSTEMUSE = 128L;

		public const long ATB_ITEM_HEROUSE = 256L;

		public const long ATB_ITEM_RANDOMSOLCASH = 512L;

		public const long ATB_ITEM_RANDOMSOLTICKET = 1024L;

		public const long ATB_ITEM_GETSOL = 2048L;

		public const long ATB_ITEM_COMPOSE = 4096L;

		public const long ATB_ITEM_CONGRATS = 8192L;

		public const long ATB_ITEM_RAREBOX = 16384L;

		public const long ATB_ITEM_GROUPSOLTICKET = 32768L;

		public const long ATB_ITEM_BOXGROUP = 65536L;

		public const long ATB_ITEM_LEGEND = 131072L;

		public const long ATB_ITEM_RANDOMHEARTSRATE = 262144L;

		public const long ATB_ITEM_ACCESSORY = 524288L;

		public static int INVENTORY_ITEMSLOT_MAX = ItemDefine.N_SLOT_COUNT_X * ItemDefine.N_SLOT_COUNT_Y;

		public static int N_SLOT_COUNT_X
		{
			get
			{
				return 10;
			}
		}

		public static int N_SLOT_COUNT_Y
		{
			get
			{
				return 3;
			}
		}
	}
}
