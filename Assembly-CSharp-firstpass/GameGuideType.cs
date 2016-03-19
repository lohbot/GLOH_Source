using System;

[Flags]
public enum GameGuideType
{
	DEFAULT = 0,
	EQUIP_ITEM = 1,
	FRIEND_RECOMMEND1 = 2,
	FRIEND_RECOMMEND2 = 3,
	FRIEND_RECOMMEND3 = 4,
	RECOMMEND_EQUIP = 5,
	FRIEND_POST = 6,
	EQUIP_SELL = 7,
	CHECK_FPS = 8,
	PLUNDER_INFO = 9,
	SUPPORT_GOLD = 10,
	PLUNDER_REQUEST = 11,
	SELL_ITEM = 12,
	ENCHANT_SOL = 13,
	RECOMMEND_REFORGE = 14,
	RECOMMEND_HEALER = 15,
	RECOMMEND_SKILL = 16,
	RECOMMEND_MON = 17,
	RECOMMEND_SOL = 18,
	RECOMMEND_COMPOSE = 19,
	GET_RUNESTONE = 20,
	GET_ORIHARCON = 21,
	CERTIFY_EMAIL = 22,
	MINE_ITEMGET = 23,
	MINE_PLUNDER = 24,
	REVIEW = 25,
	COLOSSEUM = 26,
	FRIEND_LIMIT = 27,
	GUESTID = 28,
	PURCHASE_RESTORE = 29,
	TREASURE_ALARM = 30,
	EXPEDITION_ITEMGET = 31
}
