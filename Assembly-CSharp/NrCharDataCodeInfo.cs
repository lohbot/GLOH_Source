using System;

public class NrCharDataCodeInfo
{
	private NkValueParse<byte> m_kCharTribeCode;

	private NkValueParse<byte> m_kCharJobTypeCode;

	private NkValueParse<byte> m_kCharAniTypeForEvent;

	private NkValueParse<byte> m_kCharAniEvent;

	private NkValueParse<int> m_kCharAniTypeForString;

	private NkValueParse<byte> m_kBuffTypeForString;

	private NkValueParse<int> m_kAiTypeForString;

	public NrCharDataCodeInfo()
	{
		this.m_kCharTribeCode = new NkValueParse<byte>();
		this.m_kCharJobTypeCode = new NkValueParse<byte>();
		this.m_kCharAniTypeForEvent = new NkValueParse<byte>();
		this.m_kCharAniEvent = new NkValueParse<byte>();
		this.m_kCharAniTypeForString = new NkValueParse<int>();
		this.m_kBuffTypeForString = new NkValueParse<byte>();
		this.m_kAiTypeForString = new NkValueParse<int>();
	}

	public void LoadDataCode()
	{
		this.m_kCharTribeCode.InsertCodeValue("NULL", 0);
		this.m_kCharTribeCode.InsertCodeValue("HUMAN", 1);
		this.m_kCharTribeCode.InsertCodeValue("FURRY", 2);
		this.m_kCharTribeCode.InsertCodeValue("ELF", 4);
		this.m_kCharTribeCode.InsertCodeValue("MONSTER", 8);
		this.m_kCharJobTypeCode.InsertCodeValue("MELEE_PHYSICS", 1);
		this.m_kCharJobTypeCode.InsertCodeValue("MELEE_MAGIC", 2);
		this.m_kCharJobTypeCode.InsertCodeValue("RANGE_PHYSICS", 3);
		this.m_kCharJobTypeCode.InsertCodeValue("RANGE_MAGIC", 4);
		this.m_kCharAniTypeForEvent.InsertCodeValue("ATTACK1", 1);
		this.m_kCharAniTypeForEvent.InsertCodeValue("ATTACK2", 2);
		this.m_kCharAniTypeForEvent.InsertCodeValue("ATTACK3", 3);
		this.m_kCharAniTypeForEvent.InsertCodeValue("ATTACKLEFT1", 4);
		this.m_kCharAniTypeForEvent.InsertCodeValue("ATTACKRIGHT1", 5);
		this.m_kCharAniTypeForEvent.InsertCodeValue("SKILL1", 6);
		this.m_kCharAniTypeForEvent.InsertCodeValue("SKILL2", 7);
		this.m_kCharAniTypeForEvent.InsertCodeValue("SKILL3", 8);
		this.m_kCharAniEvent.InsertCodeValue("ANIMATIONEND", 0);
		this.m_kCharAniEvent.InsertCodeValue("HIT", 1);
		this.m_kCharAniEvent.InsertCodeValue("HIT1", 2);
		this.m_kCharAniEvent.InsertCodeValue("HIT2", 3);
		this.m_kCharAniEvent.InsertCodeValue("HIT3", 4);
		this.m_kCharAniEvent.InsertCodeValue("HIT4", 5);
		this.m_kCharAniEvent.InsertCodeValue("EFFECTSTART", 6);
		this.m_kCharAniEvent.InsertCodeValue("EFFECTEND", 7);
		this.m_kCharAniEvent.InsertCodeValue("SLOWSTART", 8);
		this.m_kCharAniEvent.InsertCodeValue("SLOWEND", 9);
		this.m_kCharAniTypeForString.InsertCodeValue("NONE", -1);
		this.m_kCharAniTypeForString.InsertCodeValue("STAY1", 0);
		this.m_kCharAniTypeForString.InsertCodeValue("STAY2", 1);
		this.m_kCharAniTypeForString.InsertCodeValue("WALK1", 2);
		this.m_kCharAniTypeForString.InsertCodeValue("RUN1", 3);
		this.m_kCharAniTypeForString.InsertCodeValue("ATTACK1", 4);
		this.m_kCharAniTypeForString.InsertCodeValue("ATTACK2", 5);
		this.m_kCharAniTypeForString.InsertCodeValue("ATTACK3", 6);
		this.m_kCharAniTypeForString.InsertCodeValue("ATTACKLEFT1", 8);
		this.m_kCharAniTypeForString.InsertCodeValue("ATTACKRIGHT1", 9);
		this.m_kCharAniTypeForString.InsertCodeValue("EXTATTACK1", 7);
		this.m_kCharAniTypeForString.InsertCodeValue("SKILL1", 10);
		this.m_kCharAniTypeForString.InsertCodeValue("SKILL2", 11);
		this.m_kCharAniTypeForString.InsertCodeValue("SKILL3", 12);
		this.m_kCharAniTypeForString.InsertCodeValue("BSTAY1", 13);
		this.m_kCharAniTypeForString.InsertCodeValue("BSTAY2", 14);
		this.m_kCharAniTypeForString.InsertCodeValue("BRUN1", 15);
		this.m_kCharAniTypeForString.InsertCodeValue("DAMAGE1", 16);
		this.m_kCharAniTypeForString.InsertCodeValue("CRIDAMAGE1", 17);
		this.m_kCharAniTypeForString.InsertCodeValue("EVADE1", 18);
		this.m_kCharAniTypeForString.InsertCodeValue("DIE1", 19);
		this.m_kCharAniTypeForString.InsertCodeValue("TIRED1", 20);
		this.m_kCharAniTypeForString.InsertCodeValue("SITS1", 21);
		this.m_kCharAniTypeForString.InsertCodeValue("SITL1", 22);
		this.m_kCharAniTypeForString.InsertCodeValue("SITL2", 23);
		this.m_kCharAniTypeForString.InsertCodeValue("SITE1", 24);
		this.m_kCharAniTypeForString.InsertCodeValue("COLLECTS1", 25);
		this.m_kCharAniTypeForString.InsertCodeValue("COLLECTL1", 26);
		this.m_kCharAniTypeForString.InsertCodeValue("COLLECTE1", 27);
		this.m_kCharAniTypeForString.InsertCodeValue("ECOACTION1", 28);
		this.m_kCharAniTypeForString.InsertCodeValue("ECOACTION2", 29);
		this.m_kCharAniTypeForString.InsertCodeValue("TALKSTART1", 30);
		this.m_kCharAniTypeForString.InsertCodeValue("TALKACTION1", 31);
		this.m_kCharAniTypeForString.InsertCodeValue("TALKSTAY1", 32);
		this.m_kCharAniTypeForString.InsertCodeValue("TALKEND1", 33);
		this.m_kCharAniTypeForString.InsertCodeValue("ACTIONSTART1", 34);
		this.m_kCharAniTypeForString.InsertCodeValue("ACTIONLOOP1", 35);
		this.m_kCharAniTypeForString.InsertCodeValue("ACTIONEND1", 36);
		this.m_kCharAniTypeForString.InsertCodeValue("RESPAWN1", 37);
		this.m_kCharAniTypeForString.InsertCodeValue("EVENT1", 38);
		this.m_kBuffTypeForString.InsertCodeValue("BUFF", 1);
		this.m_kBuffTypeForString.InsertCodeValue("DEBUFF", 2);
		this.m_kAiTypeForString.InsertCodeValue("AGGROTOP", 0);
		this.m_kAiTypeForString.InsertCodeValue("HPLOW", 1);
		this.m_kAiTypeForString.InsertCodeValue("HPTOP", 2);
		this.m_kAiTypeForString.InsertCodeValue("BUFFTOP", 3);
		this.m_kAiTypeForString.InsertCodeValue("DEBUFFTOP", 4);
		this.m_kAiTypeForString.InsertCodeValue("BUFFLOW", 5);
		this.m_kAiTypeForString.InsertCodeValue("DEBUFFLOW", 6);
		this.m_kAiTypeForString.InsertCodeValue("BOSSTOP", 7);
		this.m_kAiTypeForString.InsertCodeValue("NOOVERLAP_AGGROTOP", 8);
		this.m_kAiTypeForString.InsertCodeValue("NOOVERLAP_HPLOW", 9);
		this.m_kAiTypeForString.InsertCodeValue("NOOVERLAP_HPTOP", 10);
		this.m_kAiTypeForString.InsertCodeValue("BUFFTOP_AGGROTOP", 11);
		this.m_kAiTypeForString.InsertCodeValue("BOSSTOP_HPLOW", 13);
		this.m_kAiTypeForString.InsertCodeValue("NOOVERLAP_TARGETTOP", 14);
		this.m_kAiTypeForString.InsertCodeValue("HPLOW_NOOVERLAP_CHECK", 15);
		this.m_kAiTypeForString.InsertCodeValue("HPLOW_DEBUFFTOP_CHECK", 16);
		this.m_kAiTypeForString.InsertCodeValue("TARGETTOP", 17);
	}

	public byte GetCharTribe(string datacode)
	{
		return this.m_kCharTribeCode.GetValue(datacode);
	}

	public byte GetCharJobType(string datacode)
	{
		return this.m_kCharJobTypeCode.GetValue(datacode);
	}

	public byte GetCharAniTypeForEvent(string datacode)
	{
		return this.m_kCharAniTypeForEvent.GetValue(datacode);
	}

	public byte GetCharAniEvent(string datacode)
	{
		return this.m_kCharAniEvent.GetValue(datacode);
	}

	public string[] GetCharAniString()
	{
		return this.m_kCharAniTypeForString.GetKeys();
	}

	public int GetCharAniTypeForString(string AniType)
	{
		return this.m_kCharAniTypeForString.GetValue(AniType);
	}

	public int GetBuffTypeForString(string BuffType)
	{
		return (int)this.m_kBuffTypeForString.GetValue(BuffType);
	}

	public int GetAiTypeForString(string AiType)
	{
		return this.m_kAiTypeForString.GetValue(AiType);
	}
}
