using System;
using System.Collections;
using System.Collections.Generic;

public class NrBaseTableManager : NrTSingleton<NrBaseTableManager>
{
	private Dictionary<string, NrTableData>[] m_dicResourceInfo;

	private NrBaseTableManager()
	{
		int num = 1;
		int num2 = 53;
		this.m_dicResourceInfo = new Dictionary<string, NrTableData>[num2];
		for (int i = num; i < num2; i++)
		{
			this.m_dicResourceInfo[i] = new Dictionary<string, NrTableData>();
		}
	}

	public bool SetData(NrTableData kData)
	{
		NrTableData.eResourceType typeIndex = kData.GetTypeIndex();
		int iResourceType = (int)typeIndex;
		string kDataKey = string.Empty;
		switch (typeIndex)
		{
		case NrTableData.eResourceType.eRT_WEAPONTYPE_INFO:
		{
			WEAPONTYPE_INFO wEAPONTYPE_INFO = kData as WEAPONTYPE_INFO;
			int weaponType = NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponType(wEAPONTYPE_INFO.WEAPONCODE);
			kDataKey = weaponType.ToString();
			NrTSingleton<NkWeaponTypeInfoManager>.Instance.SetWeaponTypeInfo(weaponType, ref wEAPONTYPE_INFO);
			break;
		}
		case NrTableData.eResourceType.eRT_CHARKIND_ATTACKINFO:
		{
			CHARKIND_ATTACKINFO cHARKIND_ATTACKINFO = kData as CHARKIND_ATTACKINFO;
			cHARKIND_ATTACKINFO.nWeaponType = NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponType(cHARKIND_ATTACKINFO.WEAPONCODE);
			kDataKey = cHARKIND_ATTACKINFO.ATTACKTYPE.ToString();
			NrTSingleton<NrCharKindInfoManager>.Instance.SetAttackTypeCodeInfo(cHARKIND_ATTACKINFO.ATTACKTYPE, cHARKIND_ATTACKINFO.ATTACKCODE);
			NrCharDataCodeInfo charDataCodeInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharDataCodeInfo();
			if (charDataCodeInfo != null)
			{
				cHARKIND_ATTACKINFO.nJobType = charDataCodeInfo.GetCharJobType(cHARKIND_ATTACKINFO.JOBTYPE);
			}
			break;
		}
		case NrTableData.eResourceType.eRT_CHARKIND_CLASSINFO:
		{
			CHARKIND_CLASSINFO cHARKIND_CLASSINFO = kData as CHARKIND_CLASSINFO;
			long num = 1L;
			int cLASSINDEX = cHARKIND_CLASSINFO.CLASSINDEX;
			cHARKIND_CLASSINFO.CLASSTYPE = num << cLASSINDEX - 1;
			kDataKey = cHARKIND_CLASSINFO.CLASSTYPE.ToString();
			NrTSingleton<NrCharKindInfoManager>.Instance.SetClassTypeCodeInfo(cHARKIND_CLASSINFO.CLASSCODE, cHARKIND_CLASSINFO.CLASSTYPE);
			break;
		}
		case NrTableData.eResourceType.eRT_CHARKIND_INFO:
		{
			CHARKIND_INFO cHARKIND_INFO = kData as CHARKIND_INFO;
			kDataKey = cHARKIND_INFO.CHARKIND.ToString();
			cHARKIND_INFO.nClassType = NrTSingleton<NrCharKindInfoManager>.Instance.GetClassType(cHARKIND_INFO.CLASSTYPE);
			cHARKIND_INFO.nAttackType = NrTSingleton<NrCharKindInfoManager>.Instance.GetAttackType(cHARKIND_INFO.ATTACKTYPE);
			cHARKIND_INFO.nATB = NrTSingleton<NkATB_Manager>.Instance.ParseCharATB(cHARKIND_INFO.ATB);
			NrTSingleton<NrCharKindInfoManager>.Instance.SetCharKindInfo(ref cHARKIND_INFO);
			break;
		}
		case NrTableData.eResourceType.eRT_CHARKIND_STATINFO:
		{
			CHARKIND_STATINFO cHARKIND_STATINFO = kData as CHARKIND_STATINFO;
			int charKindByCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(cHARKIND_STATINFO.CharCode);
			kDataKey = charKindByCode.ToString();
			NrTSingleton<NrCharKindInfoManager>.Instance.SetStatInfo(charKindByCode, ref cHARKIND_STATINFO);
			break;
		}
		case NrTableData.eResourceType.eRT_CHARKIND_MONSTERINFO:
		{
			CHARKIND_MONSTERINFO cHARKIND_MONSTERINFO = kData as CHARKIND_MONSTERINFO;
			int charKindByCode2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(cHARKIND_MONSTERINFO.CharCode);
			kDataKey = charKindByCode2.ToString();
			NrTSingleton<NrCharKindInfoManager>.Instance.SetMonsterInfo(charKindByCode2, ref cHARKIND_MONSTERINFO);
			break;
		}
		case NrTableData.eResourceType.eRT_CHARKIND_MONSTATINFO:
		{
			CHARKIND_MONSTATINFO cHARKIND_MONSTATINFO = kData as CHARKIND_MONSTATINFO;
			kDataKey = NkUtil.MakeLong(cHARKIND_MONSTATINFO.MonType, (long)cHARKIND_MONSTATINFO.LEVEL).ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_CHARKIND_NPCINFO:
		{
			CHARKIND_NPCINFO cHARKIND_NPCINFO = kData as CHARKIND_NPCINFO;
			int charKindByCode3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(cHARKIND_NPCINFO.CHARCODE);
			kDataKey = charKindByCode3.ToString();
			NrTSingleton<NrCharKindInfoManager>.Instance.SetNPCInfo(charKindByCode3, ref cHARKIND_NPCINFO);
			break;
		}
		case NrTableData.eResourceType.eRT_CHARKIND_ANIINFO:
		{
			CHARKIND_ANIINFO cHARKIND_ANIINFO = kData as CHARKIND_ANIINFO;
			kDataKey = cHARKIND_ANIINFO.BUNDLENAME.ToString();
			NrTSingleton<NrCharKindInfoManager>.Instance.SetAniInfo(ref cHARKIND_ANIINFO);
			break;
		}
		case NrTableData.eResourceType.eRT_CHARKIND_SOLDIERINFO:
		{
			CHARKIND_SOLDIERINFO cHARKIND_SOLDIERINFO = kData as CHARKIND_SOLDIERINFO;
			for (int i = 0; i < 5; i++)
			{
				int charKindByCode4 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(cHARKIND_SOLDIERINFO.kElement_CharData[i].Element_CharCode);
				cHARKIND_SOLDIERINFO.kElement_CharData[i].SetChar(charKindByCode4);
			}
			int charKindByCode5 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(cHARKIND_SOLDIERINFO.CharCode);
			kDataKey = charKindByCode5.ToString();
			NrTSingleton<NrCharKindInfoManager>.Instance.SetSoldierInfo(charKindByCode5, ref cHARKIND_SOLDIERINFO);
			break;
		}
		case NrTableData.eResourceType.eRT_CHARKIND_SOLGRADEINFO:
		{
			BASE_SOLGRADEINFO bASE_SOLGRADEINFO = kData as BASE_SOLGRADEINFO;
			int charKindByCode6 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(bASE_SOLGRADEINFO.CharCode);
			kDataKey = charKindByCode6.ToString();
			NrTSingleton<NrCharKindInfoManager>.Instance.SetSolGradeInfo(charKindByCode6, ref bASE_SOLGRADEINFO);
			break;
		}
		case NrTableData.eResourceType.eRT_ITEMTYPE_INFO:
		{
			ITEMTYPE_INFO iTEMTYPE_INFO = kData as ITEMTYPE_INFO;
			iTEMTYPE_INFO.OPTION1 = NrTSingleton<ItemManager>.Instance.GetItemOption(iTEMTYPE_INFO.szOption1);
			iTEMTYPE_INFO.OPTION2 = NrTSingleton<ItemManager>.Instance.GetItemOption(iTEMTYPE_INFO.szOption2);
			iTEMTYPE_INFO.ITEMPART = NrTSingleton<ItemManager>.Instance.GetItemPart(iTEMTYPE_INFO.szItemPart);
			iTEMTYPE_INFO.ITEMTYPE = NrTSingleton<ItemManager>.Instance.GetItemType(iTEMTYPE_INFO.ITEMTYPECODE);
			iTEMTYPE_INFO.ATB = NrTSingleton<NkATB_Manager>.Instance.ParseItemTypeATB(iTEMTYPE_INFO.szATB);
			iTEMTYPE_INFO.ATTACKTYPE = NrTSingleton<NrCharKindInfoManager>.Instance.GetAttackType(iTEMTYPE_INFO.szAttackTypeCode);
			CHARKIND_ATTACKINFO charAttackInfo = NrTSingleton<NrBaseTableManager>.Instance.GetCharAttackInfo(iTEMTYPE_INFO.ATTACKTYPE.ToString());
			if (charAttackInfo != null)
			{
				iTEMTYPE_INFO.WEAPONTYPE = charAttackInfo.nWeaponType;
			}
			else
			{
				iTEMTYPE_INFO.WEAPONTYPE = 0;
			}
			iTEMTYPE_INFO.EQUIPCLASSTYPE = NrTSingleton<NrCharKindInfoManager>.Instance.ParseClassTypeCode(iTEMTYPE_INFO.szClassTypeCode);
			kDataKey = iTEMTYPE_INFO.ITEMTYPE.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_QUEST_NPC_POS_INFO:
		{
			QUEST_NPC_POS_INFO qUEST_NPC_POS_INFO = kData as QUEST_NPC_POS_INFO;
			kDataKey = qUEST_NPC_POS_INFO.strUnique;
			break;
		}
		case NrTableData.eResourceType.eRT_ECO_TALK:
		{
			ECO_TALK eCO_TALK = kData as ECO_TALK;
			kDataKey = eCO_TALK.strCharCode;
			break;
		}
		case NrTableData.eResourceType.eRT_ECO:
		{
			ECO eCO = kData as ECO;
			kDataKey = eCO.GroupUnique.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_MAP_INFO:
		{
			MAP_INFO mAP_INFO = kData as MAP_INFO;
			mAP_INFO.MAP_ATB = NrTSingleton<NkATB_Manager>.Instance.ParseMapATB(mAP_INFO.strMapATB);
			ICollection gateInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetGateInfo_Col();
			foreach (GATE_INFO gATE_INFO in gateInfo_Col)
			{
				if (mAP_INFO.MAP_INDEX == gATE_INFO.SRC_MAP_IDX)
				{
					mAP_INFO.AddGateInfo(gATE_INFO);
				}
				if (mAP_INFO.MAP_INDEX == gATE_INFO.DST_MAP_IDX)
				{
					mAP_INFO.AddDSTGateInfo(gATE_INFO);
				}
			}
			kDataKey = mAP_INFO.MAP_INDEX.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_MAP_UNIT:
		{
			MAP_UNIT mAP_UNIT = kData as MAP_UNIT;
			kDataKey = mAP_UNIT.MAP_UNIQUE.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_GATE_INFO:
		{
			GATE_INFO gATE_INFO2 = kData as GATE_INFO;
			kDataKey = gATE_INFO2.GATE_IDX.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_ITEM_ACCESSORY:
		{
			ITEM_ACCESSORY pkItem = kData as ITEM_ACCESSORY;
			NrTSingleton<ItemManager>.Instance.AddAccessory(pkItem);
			return true;
		}
		case NrTableData.eResourceType.eRT_ITEM_ARMOR:
		{
			ITEM_ARMOR iTEM_ARMOR = kData as ITEM_ARMOR;
			NrTSingleton<ItemManager>.Instance.AddArmor(iTEM_ARMOR);
			kDataKey = iTEM_ARMOR.ITEMUNIQUE.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_ITEM_BOX:
		{
			ITEM_BOX pkItem2 = kData as ITEM_BOX;
			NrTSingleton<ItemManager>.Instance.AddBox(pkItem2);
			return true;
		}
		case NrTableData.eResourceType.eRT_ITEM_MATERIAL:
		{
			ITEM_MATERIAL pkItem3 = kData as ITEM_MATERIAL;
			NrTSingleton<ItemManager>.Instance.AddMaterial(pkItem3);
			return true;
		}
		case NrTableData.eResourceType.eRT_ITEM_QUEST:
		{
			ITEM_QUEST pkItem4 = kData as ITEM_QUEST;
			NrTSingleton<ItemManager>.Instance.AddQuest(pkItem4);
			return true;
		}
		case NrTableData.eResourceType.eRT_ITEM_SECONDEQUIP:
		{
			ITEM_SECONDEQUIP pkItem5 = kData as ITEM_SECONDEQUIP;
			NrTSingleton<ItemManager>.Instance.AddSecondEquip(pkItem5);
			return true;
		}
		case NrTableData.eResourceType.eRT_ITEM_SUPPLIES:
		{
			ITEM_SUPPLIES pkItem6 = kData as ITEM_SUPPLIES;
			NrTSingleton<ItemManager>.Instance.AddSupply(pkItem6);
			return true;
		}
		case NrTableData.eResourceType.eRT_ITEM_WEAPON:
		{
			ITEM_WEAPON pkItem7 = kData as ITEM_WEAPON;
			NrTSingleton<ItemManager>.Instance.AddWeapon(pkItem7);
			return true;
		}
		case NrTableData.eResourceType.eRT_INDUN_INFO:
		{
			INDUN_INFO iNDUN_INFO = kData as INDUN_INFO;
			iNDUN_INFO.m_eIndun_Type = INDUN_DEFINE.GetIndunType(iNDUN_INFO.strIndunType);
			iNDUN_INFO.m_nNpcCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(iNDUN_INFO.strNpcCode);
			kDataKey = iNDUN_INFO.m_nIndunIDX.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_GAMEGUIDE:
		{
			TableData_GameGuideInfo tableData_GameGuideInfo = kData as TableData_GameGuideInfo;
			if (tableData_GameGuideInfo.gameGuideInfo.m_eType == GameGuideType.DEFAULT)
			{
				NrTSingleton<GameGuideManager>.Instance.AddDefaultGuid(tableData_GameGuideInfo.gameGuideInfo);
			}
			else
			{
				NrTSingleton<GameGuideManager>.Instance.AddGameGuide(tableData_GameGuideInfo.gameGuideInfo);
			}
			return true;
		}
		case NrTableData.eResourceType.eRT_LOCALMAP_INFO:
		{
			LOCALMAP_INFO lOCALMAP_INFO = kData as LOCALMAP_INFO;
			kDataKey = lOCALMAP_INFO.LOCALMAP_IDX.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_WORLDMAP_INFO:
		{
			WORLDMAP_INFO wORLDMAP_INFO = kData as WORLDMAP_INFO;
			if (wORLDMAP_INFO.TEXTKEY != string.Empty)
			{
				wORLDMAP_INFO.WORLDMAP_NAME = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(wORLDMAP_INFO.TEXTKEY);
			}
			kDataKey = wORLDMAP_INFO.WORLDMAP_IDX.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_ADVENTURE:
		{
			TableData_AdventureInfo tableData_AdventureInfo = kData as TableData_AdventureInfo;
			NrTSingleton<NkAdventureManager>.Instance.AddAdventure(tableData_AdventureInfo.adventure);
			return true;
		}
		case NrTableData.eResourceType.eRT_SOLDIER_EVOLUTIONEXP:
		{
			Evolution_EXP evolution_EXP = kData as Evolution_EXP;
			kDataKey = (evolution_EXP.Grade - 1).ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_SOLDIER_TICKETINFO:
		{
			Ticket_Info ticket_Info = kData as Ticket_Info;
			kDataKey = (ticket_Info.Grade - 1).ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_CHARSOL_GUIDE:
		{
			SOL_GUIDE sOL_GUIDE = kData as SOL_GUIDE;
			kDataKey = sOL_GUIDE.m_i32CharKind.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_ITEM_REDUCE:
		{
			ItemReduceInfo itemReduceInfo = kData as ItemReduceInfo;
			kDataKey = itemReduceInfo.iGroupUnique.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_RECOMMEND_REWARD:
		{
			RECOMMEND_REWARD rECOMMEND_REWARD = kData as RECOMMEND_REWARD;
			kDataKey = rECOMMEND_REWARD.i8RecommendCount.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_SUPPORTER_REWARD:
		{
			SUPPORTER_REWARD sUPPORTER_REWARD = kData as SUPPORTER_REWARD;
			kDataKey = sUPPORTER_REWARD.i8SupporterLevel.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_GMHELPINFO:
		{
			GMHELP_INFO gMHELP_INFO = kData as GMHELP_INFO;
			kDataKey = gMHELP_INFO.m_bGMKind.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_SOLWAREHOUSE:
		{
			SolWarehouseInfo solWarehouseInfo = kData as SolWarehouseInfo;
			kDataKey = solWarehouseInfo.iWarehouseNumber.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_CHARSPEND:
		{
			charSpend charSpend = kData as charSpend;
			kDataKey = charSpend.iLevel.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_REINCARNATIONINFO:
		{
			ReincarnationInfo reincarnationInfo = kData as ReincarnationInfo;
			for (int j = 0; j < 6; j++)
			{
				reincarnationInfo.iCharKind[j] = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(reincarnationInfo.strText[j]);
			}
			kDataKey = reincarnationInfo.iType.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_ITEM_BOX_GROUP:
		{
			ITEM_BOX_GROUP_DATA pkItemBoxGroupData = kData as ITEM_BOX_GROUP_DATA;
			NrTSingleton<ItemManager>.Instance.AddBoxGroup(pkItemBoxGroupData);
			return true;
		}
		case NrTableData.eResourceType.eRT_ITEM_TICKET:
		{
			ITEM_TICKET pkItem8 = kData as ITEM_TICKET;
			NrTSingleton<ItemManager>.Instance.AddTicket(pkItem8);
			return true;
		}
		case NrTableData.eResourceType.eRT_AGIT_INFO:
		{
			AgitInfoData agitInfoData = kData as AgitInfoData;
			kDataKey = agitInfoData.i16Level.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_AGIT_NPC:
		{
			AgitNPCData agitNPCData = kData as AgitNPCData;
			kDataKey = agitNPCData.ui8NPCType.ToString();
			break;
		}
		case NrTableData.eResourceType.eRT_AGIT_MERCHNAT:
		{
			AgitMerchantData agitMerchantData = kData as AgitMerchantData;
			kDataKey = agitMerchantData.i16SellType.ToString();
			break;
		}
		}
		return this.AddResourceInfo(iResourceType, kDataKey, kData);
	}

	private bool AddResourceInfo(int iResourceType, string kDataKey, NrTableData kDataInfo)
	{
		if (0 >= iResourceType || 53 <= iResourceType)
		{
			return false;
		}
		if (!this.m_dicResourceInfo[iResourceType].ContainsKey(kDataKey))
		{
			this.m_dicResourceInfo[iResourceType].Add(kDataKey, kDataInfo);
			return true;
		}
		return false;
	}

	private NrTableData GetResourceInfo(NrTableData.eResourceType eType, string kDataKey)
	{
		if (NrTableData.eResourceType.MIN_eRT_NUM >= eType || NrTableData.eResourceType.MAX_eRT_NUM <= eType)
		{
			return null;
		}
		if (!this.m_dicResourceInfo[(int)eType].ContainsKey(kDataKey))
		{
			return null;
		}
		return this.m_dicResourceInfo[(int)eType][kDataKey];
	}

	public bool RemoveResourceInfo(NrTableData.eResourceType eType)
	{
		if (NrTableData.eResourceType.MIN_eRT_NUM >= eType || NrTableData.eResourceType.MAX_eRT_NUM <= eType)
		{
			return false;
		}
		this.m_dicResourceInfo[(int)eType].Clear();
		return true;
	}

	public int GetResourceCount(NrTableData.eResourceType eType)
	{
		if (NrTableData.eResourceType.MIN_eRT_NUM >= eType || NrTableData.eResourceType.MAX_eRT_NUM <= eType)
		{
			return 0;
		}
		return this.m_dicResourceInfo[(int)eType].Count;
	}

	public WEAPONTYPE_INFO GetWeaponTypeInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_WEAPONTYPE_INFO, kDataKey) as WEAPONTYPE_INFO;
	}

	public CHARKIND_ATTACKINFO GetCharAttackInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_CHARKIND_ATTACKINFO, kDataKey) as CHARKIND_ATTACKINFO;
	}

	public CHARKIND_CLASSINFO GetCharClassInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_CHARKIND_CLASSINFO, kDataKey) as CHARKIND_CLASSINFO;
	}

	public CHARKIND_INFO GetCharKindInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_CHARKIND_INFO, kDataKey) as CHARKIND_INFO;
	}

	public CHARKIND_STATINFO GetCharKindStatInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_CHARKIND_STATINFO, kDataKey) as CHARKIND_STATINFO;
	}

	public CHARKIND_MONSTERINFO GetCharKindMonsterInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_CHARKIND_MONSTERINFO, kDataKey) as CHARKIND_MONSTERINFO;
	}

	public CHARKIND_MONSTATINFO GetCharKindMonStatInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_CHARKIND_MONSTATINFO, kDataKey) as CHARKIND_MONSTATINFO;
	}

	public CHARKIND_NPCINFO GetCharKindNPCInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_CHARKIND_NPCINFO, kDataKey) as CHARKIND_NPCINFO;
	}

	public ITEMTYPE_INFO GetItemTypeInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_ITEMTYPE_INFO, kDataKey) as ITEMTYPE_INFO;
	}

	public QUEST_NPC_POS_INFO GetQuestNPCPosInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_QUEST_NPC_POS_INFO, kDataKey) as QUEST_NPC_POS_INFO;
	}

	public ECO_TALK GetEcoTalk(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_ECO_TALK, kDataKey) as ECO_TALK;
	}

	public ECO GetEco(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_ECO, kDataKey) as ECO;
	}

	public WORLDMAP_INFO GetWorldMapInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_WORLDMAP_INFO, kDataKey) as WORLDMAP_INFO;
	}

	public int GetLocalMapCount()
	{
		return this.GetResourceCount(NrTableData.eResourceType.eRT_LOCALMAP_INFO);
	}

	public LOCALMAP_INFO GetLocalMapInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_LOCALMAP_INFO, kDataKey) as LOCALMAP_INFO;
	}

	public LOCALMAP_INFO GetLocalMapInfoFromMapIndex(int MapIndex)
	{
		if (MapIndex <= 0)
		{
			return null;
		}
		MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(MapIndex.ToString());
		if (mapInfo != null && mapInfo.PARENTS_MAP_IDX > 0)
		{
			MapIndex = mapInfo.PARENTS_MAP_IDX;
		}
		for (int i = 0; i < NrTSingleton<NrBaseTableManager>.Instance.GetLocalMapCount(); i++)
		{
			int num = i + 1;
			LOCALMAP_INFO localMapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetLocalMapInfo(num.ToString());
			if (localMapInfo != null)
			{
				for (int j = 0; j < 20; j++)
				{
					int num2 = localMapInfo.MAP_INDEX[j];
					if (MapIndex > 0)
					{
						if (num2 == MapIndex)
						{
							return this.GetResourceInfo(NrTableData.eResourceType.eRT_LOCALMAP_INFO, num.ToString()) as LOCALMAP_INFO;
						}
					}
				}
			}
		}
		return null;
	}

	public MAP_INFO GetMapInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_MAP_INFO, kDataKey) as MAP_INFO;
	}

	public MAP_UNIT GetMapUnit(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_MAP_UNIT, kDataKey) as MAP_UNIT;
	}

	public GATE_INFO GetGateInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_GATE_INFO, kDataKey) as GATE_INFO;
	}

	public INDUN_INFO GetIndunInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_INDUN_INFO, kDataKey) as INDUN_INFO;
	}

	public Evolution_EXP GetSoldierEvolutionEXP(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_SOLDIER_EVOLUTIONEXP, kDataKey) as Evolution_EXP;
	}

	public Ticket_Info GetSoldierTickInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_SOLDIER_TICKETINFO, kDataKey) as Ticket_Info;
	}

	public ItemReduceInfo GetItemReduceInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_ITEM_REDUCE, kDataKey) as ItemReduceInfo;
	}

	public IDictionaryEnumerator GetEcho_Enum()
	{
		return this.m_dicResourceInfo[15].GetEnumerator();
	}

	public ICollection GetGateInfo_Col()
	{
		return this.m_dicResourceInfo[18].Values;
	}

	public ICollection GetMapInfo_Col()
	{
		return this.m_dicResourceInfo[16].Values;
	}

	public ICollection GetIndunInfo_Col()
	{
		return this.m_dicResourceInfo[28].Values;
	}

	public ICollection GetCharInfo_Col()
	{
		return this.m_dicResourceInfo[4].Values;
	}

	public ICollection GetSolGuide_Col()
	{
		return this.m_dicResourceInfo[35].Values;
	}

	public ICollection GetRecommend_Reward_Col()
	{
		return this.m_dicResourceInfo[37].Values;
	}

	public ICollection GetSupporter_Reward_Col()
	{
		return this.m_dicResourceInfo[38].Values;
	}

	public GMHELP_INFO GetGMHelpKindInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_GMHELPINFO, kDataKey) as GMHELP_INFO;
	}

	public SolWarehouseInfo GetSolWarehouseInfo(string kDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_SOLWAREHOUSE, kDataKey) as SolWarehouseInfo;
	}

	public charSpend GetCharSpend(string strDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_CHARSPEND, strDataKey) as charSpend;
	}

	public ReincarnationInfo GetReincarnation(string strDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_REINCARNATIONINFO, strDataKey) as ReincarnationInfo;
	}

	public AgitInfoData GetAgitData(string strDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_AGIT_INFO, strDataKey) as AgitInfoData;
	}

	public AgitNPCData GetAgitNPCData(string strDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_AGIT_NPC, strDataKey) as AgitNPCData;
	}

	public AgitMerchantData GetAgitMerchantData(string strDataKey)
	{
		return this.GetResourceInfo(NrTableData.eResourceType.eRT_AGIT_MERCHNAT, strDataKey) as AgitMerchantData;
	}
}
