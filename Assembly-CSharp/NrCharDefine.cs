using System;
using UnityEngine;

public class NrCharDefine
{
	public enum CharUpdateStep : byte
	{
		CHARUPDATESTEP_NONE,
		CHARUPDATESTEP_NEAR,
		CHARUPDATESTEP_AROUND,
		CHARUPDATESTEP_FAR,
		CHARUPDATESTEP_VERYFAR
	}

	public enum CharLODStep : byte
	{
		CHARLOD_STEP_1 = 1,
		CHARLOD_STEP_2,
		CHARLOD_STEP_3,
		CHARLOD_STEP_END
	}

	public enum ReserveCharUnique : short
	{
		BATTLE_CAHR = 21000,
		BATTLE_CAHR_END = 21500,
		TERRITORY_NPC = 29000,
		TERRITORY_NPC_END = 29100,
		CREATE_CHAR = 30000,
		QUEST_SUB_NPC = 31000,
		QUEST_SUB_NPC_END = 31004,
		USER_SUB_NPCMONSTER,
		USER_SUB_NPCMONSTER_END = 31200,
		CLIENT_NPC = 31300,
		CLIENT_NPC_END = 31400
	}

	public enum eCharCutImageType
	{
		_48x35,
		_58x40,
		_40x58,
		_120x26,
		TYPE3
	}

	public enum eCharFaicalAnimationType
	{
		FStay1,
		FTalk1,
		FTalk2,
		FTalk3,
		FTalk4,
		FTalkLong1,
		FSmile1,
		FCloseEye1,
		END_FACIALANITYPE,
		FOffset = 8
	}

	public enum eAT2CharPartInfo
	{
		CHARPART_HEAD,
		CHARPART_FACE,
		CHARPART_HELMET,
		CHARPART_BODY,
		CHARPART_GLOVE,
		CHARPART_BOOTS,
		MAX_CHARPART_NUM
	}

	public enum eAT2PartAssetBundle
	{
		helmet,
		body,
		glove,
		boots,
		MAX_PARTASSET_NUM
	}

	public enum eAT2ItemAssetBundle
	{
		hair,
		face,
		helmet,
		weapon1,
		weapon2,
		decoration,
		bodyitem,
		centeritem,
		MAX_ITEMASSET_NUM
	}

	public enum eMoveTargetReason
	{
		MTR_SUCCESS,
		MTR_WIDECOLL,
		MTR_CELLATB
	}

	public const float NPC_CLICK_DISTANCE = 12f;

	public const int MAX_CHAR_NUM = 300;

	public const int MAX_MAKECHAR_FRAME_COUNT = 2;

	public const int MAX_MAKECHAR_NUM_PER_FRAME = 1;

	public const float CHAR_CUSTOM_SCALE = 0.55f;

	public const float CHAR_NORMAL_SCALE = 1f;

	public const float CHAR_BASE_SCALE = 10f;

	public const float CHAR_TERRITORY_SCALE = 3f;

	public const float CHAR_FADEIN_SPEED = 5f;

	public const float ZERO_BLEND = 0f;

	public const float DEFAULT_BLEND = 0.3f;

	public const float CHAR_MOVE_MINDISTANCE = 0.7f;

	public const float CHAR_SAFECLICK_TIME = 0.5f;

	public const int MAX_ENHANCE_COUNT = 10;

	public const int MAX_CHARKIND_NPC_NUM = 100;

	public const int MAX_CHARKIND_MONSTER_NUM = 100;

	public const int MAX_CHARKIND_SOLDIER_NUM = 100;

	public const int MAX_CHARKIND_SUB_NUM = 100;

	public const int MAX_CHARKIND_LANGIDX_LENGTH = 32;

	public const int MAX_CHARKIND_ATB_LENGTH = 256;

	public const int MAX_CHARKIND_PATH_LENGTH = 128;

	public const int MAX_SAFEANI_RETRY_NUM = 3;

	public const int MAX_CHARATB_NUM = 64;

	public const short MAX_CHARNAME_CLIENT_NUM = 11;

	public const short MAX_CHAR_SLOT = 3;

	public const float WIDECOLL_HALFSIZE_NPC = 2f;

	public const int MAX_CHAR_PICKER = 3;

	public const byte MON_TYPE_BOSS = 109;

	public const byte USA_MENUCHANGE_LEVEL = 7;

	public const byte USA_MEASURE_LEVEL = 15;

	public static bool IsTerritoryChar(short charunique)
	{
		return charunique >= 29000 && charunique < 29100;
	}

	public static bool IsSubChar(short siCharUnique)
	{
		return 31005 <= siCharUnique && 31200 > siCharUnique;
	}

	public static WrapMode GetFacialWrapMode(NrCharDefine.eCharFaicalAnimationType anitype)
	{
		switch (anitype)
		{
		case NrCharDefine.eCharFaicalAnimationType.FStay1:
			return WrapMode.Loop;
		case NrCharDefine.eCharFaicalAnimationType.FTalk1:
			return WrapMode.Loop;
		case NrCharDefine.eCharFaicalAnimationType.FTalk2:
			return WrapMode.Loop;
		case NrCharDefine.eCharFaicalAnimationType.FTalk3:
			return WrapMode.Loop;
		case NrCharDefine.eCharFaicalAnimationType.FTalk4:
			return WrapMode.Loop;
		case NrCharDefine.eCharFaicalAnimationType.FTalkLong1:
			return WrapMode.Loop;
		case NrCharDefine.eCharFaicalAnimationType.FSmile1:
			return WrapMode.Once;
		case NrCharDefine.eCharFaicalAnimationType.FCloseEye1:
			return WrapMode.Once;
		default:
			return WrapMode.Loop;
		}
	}
}
