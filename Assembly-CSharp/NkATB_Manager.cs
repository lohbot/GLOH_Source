using System;

public class NkATB_Manager : NrTSingleton<NkATB_Manager>
{
	private enum eATB_KIND
	{
		eATB_KIND_MAP,
		eATB_KIND_CHAR,
		eATB_KIND_ECO,
		eATB_KIND_RIDE,
		eATB_KIND_ITEMTYPE,
		eATB_KIND_WEAPONTYPE,
		eATB_KIND_ITEM,
		eATB_KIND_TARGETWEAPONTYPE,
		MAX_eATB_KIND
	}

	private NkATBParse[] m_kATB_Kind;

	private NkATB_Manager.eATB_KIND m_eCurrentATBKind;

	private NkATB_Manager()
	{
		int num = 8;
		this.m_kATB_Kind = new NkATBParse[num];
		for (int i = 0; i < num; i++)
		{
			this.m_kATB_Kind[i] = new NkATBParse();
		}
		this._LoadATBInfo();
	}

	private void _InsertCurrentATB(string strATB, long nATBValue)
	{
		int eCurrentATBKind = (int)this.m_eCurrentATBKind;
		if (8 <= eCurrentATBKind)
		{
			return;
		}
		this.m_kATB_Kind[eCurrentATBKind].InsertCodeValue(strATB, nATBValue);
	}

	private void _LoadATBInfo()
	{
		this.m_eCurrentATBKind = NkATB_Manager.eATB_KIND.eATB_KIND_MAP;
		this._InsertCurrentATB("DONTRIDE", 1L);
		this.m_eCurrentATBKind = NkATB_Manager.eATB_KIND.eATB_KIND_CHAR;
		this._InsertCurrentATB("USER", 1L);
		this._InsertCurrentATB("SOLDIER", 2L);
		this._InsertCurrentATB("MONSTER", 4L);
		this._InsertCurrentATB("NPC", 8L);
		this._InsertCurrentATB("OBJECT", 16L);
		this._InsertCurrentATB("WIDECOLL", 32L);
		this._InsertCurrentATB("COLLECT", 64L);
		this._InsertCurrentATB("BOSS", 128L);
		this._InsertCurrentATB("BUYITEMNPC", 512L);
		this._InsertCurrentATB("GOAL", 1024L);
		this._InsertCurrentATB("FINDGOAL", 2048L);
		this._InsertCurrentATB("INTERACTION", 4096L);
		this._InsertCurrentATB("DONTMOVE", 8192L);
		this._InsertCurrentATB("INTRUSION", 16384L);
		this._InsertCurrentATB("FIRSTSTRIKE", 32768L);
		this._InsertCurrentATB("RUN", 65536L);
		this._InsertCurrentATB("RUNAWAY", 131072L);
		this._InsertCurrentATB("TALK", 262144L);
		this._InsertCurrentATB("TALKTOUSER", 524288L);
		this._InsertCurrentATB("NOSHADOW", 1048576L);
		this._InsertCurrentATB("BLACKSMITH", 2097152L);
		this._InsertCurrentATB("MULTIANI", 4194304L);
		this._InsertCurrentATB("CHARCHANGE", 8388608L);
		this._InsertCurrentATB("NAMEUI", 16777216L);
		this._InsertCurrentATB("EVENTEXCHANGE", 33554432L);
		this._InsertCurrentATB("DIEANI", 67108864L);
		this._InsertCurrentATB("LOOPANI", 134217728L);
		this._InsertCurrentATB("ITEMUSECOLLECT", 268435456L);
		this._InsertCurrentATB("INDUNNPC", 536870912L);
		this._InsertCurrentATB("TREASURE", 1073741824L);
		this._InsertCurrentATB("MULTIWEAPON", (long)((ulong)-2147483648));
		this._InsertCurrentATB("GUILDNPC", 4294967296L);
		this._InsertCurrentATB("COLLECTFREE", 8589934592L);
		this._InsertCurrentATB("IMGTALK", 17179869184L);
		this._InsertCurrentATB("BATTLE_RUNAWAY", 34359738368L);
		this._InsertCurrentATB("BATTLE_BOSSDEF", 68719476736L);
		this._InsertCurrentATB("SUMMONER", 137438953472L);
		this._InsertCurrentATB("CLASSCHANGE", 274877906944L);
		this._InsertCurrentATB("EQUIPREDUCE", 549755813888L);
		this._InsertCurrentATB("AWAKENING", 1099511627776L);
		this._InsertCurrentATB("EQUIPSKILL", 2199023255552L);
		this._InsertCurrentATB("GMHELPSOL", 4398046511104L);
		this._InsertCurrentATB("TREASURE_BOX", 562949953421312L);
		this._InsertCurrentATB("BATTLE_NOTURN", 8796093022208L);
		this._InsertCurrentATB("BATTLE_ONLYSKILL", 17592186044416L);
		this._InsertCurrentATB("BOUNTYHUNT", 1125899906842624L);
		this._InsertCurrentATB("CC_RESIST", 2251799813685248L);
		this._InsertCurrentATB("JEWELRYEXCHANGE", 4503599627370496L);
		this._InsertCurrentATB("AGIT", 18014398509481984L);
		this._InsertCurrentATB("MYTHICEXCHANGE", 36028797018963968L);
		this._InsertCurrentATB("MYTH_DAMAGEADD", 72057594037927936L);
		this._InsertCurrentATB("BATTLE_IMMUNE_SKILL", 144115188075855872L);
		this._InsertCurrentATB("ITEMREPAIR", 288230376151711744L);
		this._InsertCurrentATB("GUILDWAREXCHANGE", 1152921504606846976L);
		this._InsertCurrentATB("ANGELPOINT", 576460752303423488L);
		this._InsertCurrentATB("IMMORTAL", 2305843009213693952L);
		this._InsertCurrentATB("MAGIC_RESIST", 4611686018427387904L);
		this._InsertCurrentATB("PHYSICAL_RESIST", 256L);
		this.m_eCurrentATBKind = NkATB_Manager.eATB_KIND.eATB_KIND_ECO;
		this._InsertCurrentATB("RANK", 1L);
		this._InsertCurrentATB("MOVINGRANK", 2L);
		this._InsertCurrentATB("RENEW", 4L);
		this._InsertCurrentATB("INTRUSION1", 8L);
		this._InsertCurrentATB("INTRUSION2", 16L);
		this._InsertCurrentATB("RANDOMTIMEREGEN", 32L);
		this._InsertCurrentATB("UNIONSERVER", 64L);
		this._InsertCurrentATB("MILITARY", 128L);
		this._InsertCurrentATB("RESTORE", 256L);
		this._InsertCurrentATB("TREASUREBOX", 512L);
		this._InsertCurrentATB("BOUNTYHUNT", 1024L);
		this.m_eCurrentATBKind = NkATB_Manager.eATB_KIND.eATB_KIND_RIDE;
		this._InsertCurrentATB("HORSE", 1L);
		this._InsertCurrentATB("CAMEL", 2L);
		this.m_eCurrentATBKind = NkATB_Manager.eATB_KIND.eATB_KIND_ITEMTYPE;
		this._InsertCurrentATB("OBJECT", 1L);
		this._InsertCurrentATB("NOTRADE", 2L);
		this._InsertCurrentATB("MAKE", 4L);
		this._InsertCurrentATB("BREAK", 8L);
		this.m_eCurrentATBKind = NkATB_Manager.eATB_KIND.eATB_KIND_WEAPONTYPE;
		this._InsertCurrentATB("ATTACK", 1L);
		this._InsertCurrentATB("DEFENSE", 2L);
		this._InsertCurrentATB("INVERSE", 4L);
		this._InsertCurrentATB("DOUBLE", 8L);
		this._InsertCurrentATB("WITHSHIELD", 16L);
		this.m_eCurrentATBKind = NkATB_Manager.eATB_KIND.eATB_KIND_ITEM;
		this._InsertCurrentATB("SHOW", 1L);
		this._InsertCurrentATB("TRADE_USER", 2L);
		this._InsertCurrentATB("TRADE_FRIEND", 4L);
		this._InsertCurrentATB("ENABLEUSE", 8L);
		this._InsertCurrentATB("RANDOMBOX", 16L);
		this._InsertCurrentATB("SELECTBOX", 32L);
		this._InsertCurrentATB("ALLBOX", 64L);
		this._InsertCurrentATB("SYSTEMUSE", 128L);
		this._InsertCurrentATB("HEROUSE", 256L);
		this._InsertCurrentATB("RANDOMSOLCASH", 512L);
		this._InsertCurrentATB("RANDOMSOLTICKET", 1024L);
		this._InsertCurrentATB("GETSOL", 2048L);
		this._InsertCurrentATB("COMPOSE", 4096L);
		this._InsertCurrentATB("CONGRATS", 8192L);
		this._InsertCurrentATB("RAREBOX", 16384L);
		this._InsertCurrentATB("GROUPSOLTICKET", 32768L);
		this._InsertCurrentATB("BOXGROUP", 65536L);
		this._InsertCurrentATB("LEGEND", 131072L);
		this._InsertCurrentATB("RANDOMHEARTSRATE", 262144L);
		this._InsertCurrentATB("ACCESSORY", 524288L);
		this._InsertCurrentATB("UNTRADABLE", 1048576L);
		this._InsertCurrentATB("ONLY_LEADER", 2097152L);
		this._InsertCurrentATB("PREMIUMRATE", 4194304L);
		this._InsertCurrentATB("LEGENDHIRE", 8388608L);
		this._InsertCurrentATB("ITEMLOCK", 16777216L);
		this._InsertCurrentATB("COSTUMEBOX", 33554432L);
		this.m_eCurrentATBKind = NkATB_Manager.eATB_KIND.eATB_KIND_TARGETWEAPONTYPE;
		this._InsertCurrentATB("ALL", 1L);
		this._InsertCurrentATB("SWORD", 2L);
		this._InsertCurrentATB("SPEAR", 4L);
		this._InsertCurrentATB("AXE", 8L);
		this._InsertCurrentATB("BOW", 16L);
		this._InsertCurrentATB("GUN", 32L);
		this._InsertCurrentATB("CANNON", 64L);
		this._InsertCurrentATB("STAFF", 128L);
		this._InsertCurrentATB("BIBLE", 256L);
	}

	public long GetMapATB(string strATBCode)
	{
		return this.m_kATB_Kind[0].GetValue(strATBCode);
	}

	public long ParseMapATB(string strATBContents)
	{
		return this.m_kATB_Kind[0].ParseCode(strATBContents);
	}

	public long GetCharATB(string strATBCode)
	{
		return this.m_kATB_Kind[1].GetValue(strATBCode);
	}

	public long ParseCharATB(string strATBContents)
	{
		return this.m_kATB_Kind[1].ParseCode(strATBContents);
	}

	public long GetECOATB(string strATBCode)
	{
		return this.m_kATB_Kind[2].GetValue(strATBCode);
	}

	public long ParseECOATB(string strATBContents)
	{
		return this.m_kATB_Kind[2].ParseCode(strATBContents);
	}

	public long GetRideATB(string strATBCode)
	{
		return this.m_kATB_Kind[3].GetValue(strATBCode);
	}

	public long ParseRideATB(string strATBContents)
	{
		return this.m_kATB_Kind[3].ParseCode(strATBContents);
	}

	public long GetItemTypeATB(string strATBCode)
	{
		return this.m_kATB_Kind[4].GetValue(strATBCode);
	}

	public long ParseItemTypeATB(string strATBContents)
	{
		return this.m_kATB_Kind[4].ParseCode(strATBContents);
	}

	public long GetWeaponTypeATB(string strATBCode)
	{
		return this.m_kATB_Kind[5].GetValue(strATBCode);
	}

	public long ParseWeaponTypeATB(string strATBContents)
	{
		return this.m_kATB_Kind[5].ParseCode(strATBContents);
	}

	public long GetItemATB(string strATBCode)
	{
		return this.m_kATB_Kind[6].GetValue(strATBCode);
	}

	public long ParseItemATB(string strATBContents)
	{
		return this.m_kATB_Kind[6].ParseCode(strATBContents);
	}

	public long ParseTargetWeaponTypeATB(string strATBContents)
	{
		return this.m_kATB_Kind[7].ParseCode(strATBContents);
	}

	public string[] GetEcoATBString()
	{
		return this.m_kATB_Kind[2].GetString();
	}
}
