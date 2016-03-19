using System;
using TsLibs;

public abstract class NrTableData
{
	public enum eResourceType
	{
		MIN_eRT_NUM,
		eRT_WEAPONTYPE_INFO,
		eRT_CHARKIND_ATTACKINFO,
		eRT_CHARKIND_CLASSINFO,
		eRT_CHARKIND_INFO,
		eRT_CHARKIND_STATINFO,
		eRT_CHARKIND_MONSTERINFO,
		eRT_CHARKIND_MONSTATINFO,
		eRT_CHARKIND_NPCINFO,
		eRT_CHARKIND_ANIINFO,
		eRT_CHARKIND_SOLDIERINFO,
		eRT_CHARKIND_SOLGRADEINFO,
		eRT_ITEMTYPE_INFO,
		eRT_QUEST_NPC_POS_INFO,
		eRT_ECO_TALK,
		eRT_ECO,
		eRT_MAP_INFO,
		eRT_MAP_UNIT,
		eRT_GATE_INFO,
		eRT_ITEM_ACCESSORY,
		eRT_ITEM_ARMOR,
		eRT_ITEM_BOX,
		eRT_ITEM_MATERIAL,
		eRT_ITEM_QUEST,
		eRT_ITEM_SECONDEQUIP,
		eRT_ITEM_SUPPLIES,
		eRT_ITEM_WEAPON,
		eRT_ITEM_TERRITORY,
		eRT_INDUN_INFO,
		eRT_GAMEGUIDE,
		eRT_LOCALMAP_INFO,
		eRT_WORLDMAP_INFO,
		eRT_ADVENTURE,
		eRT_SOLDIER_EVOLUTIONEXP,
		eRT_SOLDIER_TICKETINFO,
		eRT_CHARSOL_GUIDE,
		eRT_ITEM_REDUCE,
		eRT_RECOMMEND_REWARD,
		eRT_SUPPORTER_REWARD,
		eRT_GMHELPINFO,
		eRT_EVENT_HERO,
		eRT_SOLWAREHOUSE,
		eRT_CHARSPEND,
		eRT_REINCARNATIONINFO,
		eRT_ITEM_BOX_GROUP,
		eRT_ITEM_TICKET,
		eRT_AGIT_INFO,
		eRT_AGIT_NPC,
		eRT_AGIT_MERCHNAT,
		eRT_TRANSCENDENCE_COST,
		eRT_TRANSCENDENCE_SOL,
		eRT_TRANSCENDENCE_RATE,
		eRT_TRANSCENDENCE_FAILREWARD,
		MAX_eRT_NUM
	}

	private NrTableData.eResourceType m_eResourceType;

	public NrTableData()
	{
		this.m_eResourceType = NrTableData.eResourceType.MIN_eRT_NUM;
	}

	public NrTableData(NrTableData.eResourceType eType)
	{
		this.m_eResourceType = eType;
	}

	protected void SetTypeIndex(NrTableData.eResourceType eType)
	{
		this.m_eResourceType = eType;
	}

	public NrTableData.eResourceType GetTypeIndex()
	{
		return this.m_eResourceType;
	}

	public abstract void SetData(TsDataReader.Row row);

	public virtual void SetData(TsDataReader.Row row, Type objtype)
	{
	}
}
