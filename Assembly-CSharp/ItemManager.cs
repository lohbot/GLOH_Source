using GAME;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ItemManager : NrTSingleton<ItemManager>
{
	private Dictionary<int, UIBaseInfoLoader> m_gCollection;

	private SortedDictionary<int, ITEMINFO> m_cdItemTotal;

	private SortedDictionary<eITEM_PART, ITEMINFO[]> m_cdItemPartSort;

	private NkValueParse<eITEM_PART> m_kItemPartCodeInfo;

	private NkValueParse<int> m_kItemTypeCodeInfo;

	private NkValueParse<int> m_kItemOptionCodeInfo;

	private SortedDictionary<int, ITEM_BOX_GROUP> m_cdItemBoxGroup;

	private Dictionary<long, List<GROUPTICKET_INFO>> m_dicGroupSolTicket;

	private ItemManager()
	{
		this.m_gCollection = new Dictionary<int, UIBaseInfoLoader>();
		this.m_cdItemTotal = new SortedDictionary<int, ITEMINFO>();
		this.m_cdItemPartSort = new SortedDictionary<eITEM_PART, ITEMINFO[]>();
		this.m_kItemPartCodeInfo = new NkValueParse<eITEM_PART>();
		this.m_kItemTypeCodeInfo = new NkValueParse<int>();
		this.m_kItemOptionCodeInfo = new NkValueParse<int>();
		this.m_cdItemBoxGroup = new SortedDictionary<int, ITEM_BOX_GROUP>();
		this.m_dicGroupSolTicket = new Dictionary<long, List<GROUPTICKET_INFO>>();
		this.SetItemDataCode();
	}

	public SortedDictionary<int, ITEMINFO> Get_Total_Collection()
	{
		return this.m_cdItemTotal;
	}

	public ITEMINFO[] Get_Values(eITEM_PART nItemPart)
	{
		if (this.m_cdItemPartSort.ContainsKey(nItemPart))
		{
			return this.m_cdItemPartSort[nItemPart];
		}
		List<ITEMINFO> list = new List<ITEMINFO>();
		foreach (ITEMINFO current in this.m_cdItemTotal.Values)
		{
			if (Protocol_Item.GetStaticItemPart(current.m_nItemUnique) == nItemPart)
			{
				list.Add(current);
			}
		}
		list.Sort((ITEMINFO a_cX, ITEMINFO a_cY) => a_cX.m_nSortOrder.CompareTo(a_cY.m_nSortOrder));
		this.m_cdItemPartSort.Add(nItemPart, list.ToArray());
		return this.m_cdItemPartSort[nItemPart];
	}

	public void SetItemDataCode()
	{
		this.m_kItemPartCodeInfo.InsertCodeValue("WEAPON", eITEM_PART.ITEMPART_WEAPON);
		this.m_kItemPartCodeInfo.InsertCodeValue("ARMOR", eITEM_PART.ITEMPART_ARMOR);
		this.m_kItemPartCodeInfo.InsertCodeValue("ACCESSORY", eITEM_PART.ITEMPART_ACCESSORY);
		this.m_kItemPartCodeInfo.InsertCodeValue("SOLDIER", eITEM_PART.ITEMPART_SOLDIER);
		this.m_kItemPartCodeInfo.InsertCodeValue("MATERIAL", eITEM_PART.ITEMPART_MATERIAL);
		this.m_kItemPartCodeInfo.InsertCodeValue("BOX", eITEM_PART.ITEMPART_BOX);
		this.m_kItemPartCodeInfo.InsertCodeValue("SUPPLY", eITEM_PART.ITEMPART_SUPPLY);
		this.m_kItemPartCodeInfo.InsertCodeValue("TOOL", eITEM_PART.ITEMPART_TOOL);
		this.m_kItemPartCodeInfo.InsertCodeValue("ETC", eITEM_PART.ITEMPART_ETC);
		this.m_kItemPartCodeInfo.InsertCodeValue("QUEST", eITEM_PART.ITEMPART_QUEST);
		this.m_kItemPartCodeInfo.InsertCodeValue("CASH", eITEM_PART.ITEMPART_CASH);
		this.m_kItemTypeCodeInfo.InsertCodeValue("SWORD", 1);
		this.m_kItemTypeCodeInfo.InsertCodeValue("SPEAR", 2);
		this.m_kItemTypeCodeInfo.InsertCodeValue("AXE", 3);
		this.m_kItemTypeCodeInfo.InsertCodeValue("BOW", 4);
		this.m_kItemTypeCodeInfo.InsertCodeValue("GUN", 5);
		this.m_kItemTypeCodeInfo.InsertCodeValue("CANNON", 6);
		this.m_kItemTypeCodeInfo.InsertCodeValue("STAFF", 7);
		this.m_kItemTypeCodeInfo.InsertCodeValue("BIBLE", 8);
		this.m_kItemTypeCodeInfo.InsertCodeValue("HELMET", 9);
		this.m_kItemTypeCodeInfo.InsertCodeValue("ARMOR", 10);
		this.m_kItemTypeCodeInfo.InsertCodeValue("GLOVE", 11);
		this.m_kItemTypeCodeInfo.InsertCodeValue("BOOTS", 12);
		this.m_kItemTypeCodeInfo.InsertCodeValue("BOX", 13);
		this.m_kItemTypeCodeInfo.InsertCodeValue("HEAL", 14);
		this.m_kItemTypeCodeInfo.InsertCodeValue("QUEST", 15);
		this.m_kItemTypeCodeInfo.InsertCodeValue("MATERIAL", 16);
		this.m_kItemTypeCodeInfo.InsertCodeValue("SUPPLY", 17);
		this.m_kItemTypeCodeInfo.InsertCodeValue("TICKET", 18);
		this.m_kItemTypeCodeInfo.InsertCodeValue("RING", 19);
		this.m_kItemOptionCodeInfo.InsertCodeValue("DAMAGE", 1);
		this.m_kItemOptionCodeInfo.InsertCodeValue("DEFENSE", 2);
		this.m_kItemOptionCodeInfo.InsertCodeValue("CRITICAL", 3);
		this.m_kItemOptionCodeInfo.InsertCodeValue("HPPLUS", 4);
		this.m_kItemOptionCodeInfo.InsertCodeValue("MPPLUS", 5);
		this.m_kItemOptionCodeInfo.InsertCodeValue("HITRATE", 6);
		this.m_kItemOptionCodeInfo.InsertCodeValue("EVASION", 7);
	}

	public eITEM_PART GetItemPart(string itempartcode)
	{
		return this.m_kItemPartCodeInfo.GetValue(itempartcode);
	}

	public eITEM_PART GetItemPart(int itemunique)
	{
		ITEMINFO itemInfo = this.GetItemInfo(itemunique);
		if (itemInfo == null)
		{
			return eITEM_PART.ITEMPART_NONE;
		}
		return this.GetItemPartByItemType(itemInfo.m_nItemType);
	}

	public eITEM_PART GetItemPartByItemType(int itemType)
	{
		if (itemType <= 0)
		{
			return eITEM_PART.ITEMPART_NONE;
		}
		return NrTSingleton<NrBaseTableManager>.Instance.GetItemTypeInfo(itemType.ToString()).ITEMPART;
	}

	public int GetItemOption(string itemOptioncode)
	{
		return this.m_kItemOptionCodeInfo.GetValue(itemOptioncode);
	}

	public int GetItemOption(int itemUnique, int itemOption)
	{
		ITEMTYPE_INFO itemTypeInfo = this.GetItemTypeInfo(itemUnique);
		if (itemTypeInfo == null)
		{
			return 0;
		}
		if (itemOption == 0)
		{
			return itemTypeInfo.OPTION1;
		}
		if (itemOption == 1)
		{
			return itemTypeInfo.OPTION2;
		}
		return 0;
	}

	public int GetItemOptionByItemUnique(int itemunique, int itemoption)
	{
		return this.GetItemOption(itemunique, itemoption);
	}

	public int GetItemType(string itemtypecode)
	{
		return this.m_kItemTypeCodeInfo.GetValue(itemtypecode);
	}

	public string GetItemTypeName(eITEM_TYPE eItemType)
	{
		int num = (int)eItemType;
		if (num <= 0)
		{
			return "NULL";
		}
		ITEMTYPE_INFO itemTypeInfo = NrTSingleton<NrBaseTableManager>.Instance.GetItemTypeInfo(num.ToString());
		if (itemTypeInfo == null)
		{
			return "NULLL";
		}
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(itemTypeInfo.TEXTKEY);
	}

	public int GetItemType(int itemunique)
	{
		ITEMINFO itemInfo = this.GetItemInfo(itemunique);
		if (itemInfo == null)
		{
			return 0;
		}
		return itemInfo.m_nItemType;
	}

	public eITEM_TYPE GetItemTypeByCharJopType(eITEM_TYPE basetype, byte jobtype)
	{
		eITEM_TYPE result = basetype;
		switch (jobtype)
		{
		case 1:
			result = basetype - 2;
			break;
		case 3:
			result = basetype - 1;
			break;
		}
		return result;
	}

	public eITEM_TYPE GetItemTypeByItemUnique(int itemunique)
	{
		return (eITEM_TYPE)this.GetItemType(itemunique);
	}

	public eITEM_POSTYPE GetItemPosTypeByItemUnique(int itemunique)
	{
		return this.GetPosTypeByItemType(itemunique);
	}

	public ITEMTYPE_INFO GetItemTypeInfo(int itemunique)
	{
		if (itemunique <= 0)
		{
			return null;
		}
		int itemType = this.GetItemType(itemunique);
		if (itemType <= 0)
		{
			return null;
		}
		return NrTSingleton<NrBaseTableManager>.Instance.GetItemTypeInfo(itemType.ToString());
	}

	public eITEM_POSTYPE GetPosTypeByItemType(int itemunique)
	{
		ITEMINFO itemInfo = this.GetItemInfo(itemunique);
		if (itemInfo == null)
		{
			return eITEM_POSTYPE.INVEN_ALL;
		}
		switch (itemInfo.m_nItemType)
		{
		case 1:
		case 2:
		case 3:
			return eITEM_POSTYPE.INVEN_EQUIP_WEAPON_MELEE;
		case 4:
		case 5:
		case 6:
			return eITEM_POSTYPE.INVEN_EQUIP_WEAPON_RANGE;
		case 7:
		case 8:
			return eITEM_POSTYPE.INVEN_EQUIP_WEAPON_MAGIC;
		case 9:
		case 10:
		case 11:
		case 12:
		case 19:
			return eITEM_POSTYPE.INVEN_EQUIP_DEFENCE;
		case 13:
		case 14:
		case 17:
			return eITEM_POSTYPE.INVEN_SUPPLISE_BOX;
		case 15:
		case 16:
			return eITEM_POSTYPE.INVEN_MATERIAL_QUEST;
		case 18:
			return eITEM_POSTYPE.INVEN_TICKET;
		default:
			return eITEM_POSTYPE.INVEN_MATERIAL_QUEST;
		}
	}

	public eITEM_PART GetItemPartByItemUnique(int itemunique)
	{
		ITEMTYPE_INFO itemTypeInfo = this.GetItemTypeInfo(itemunique);
		if (itemTypeInfo != null)
		{
			return itemTypeInfo.ITEMPART;
		}
		return eITEM_PART.ITEMPART_NONE;
	}

	public eEQUIP_ITEM GetEquipItemPos(int itemunique)
	{
		ITEMINFO itemInfo = this.GetItemInfo(itemunique);
		if (itemInfo == null)
		{
			return eEQUIP_ITEM.EQUIP_ITEM_MAX;
		}
		switch (itemInfo.m_nItemType)
		{
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
			return eEQUIP_ITEM.EQUIP_WEAPON1;
		case 9:
			return eEQUIP_ITEM.EQUIP_HELMET;
		case 10:
			return eEQUIP_ITEM.EQUIP_ARMOR;
		case 11:
			return eEQUIP_ITEM.EQUIP_GLOVE;
		case 12:
			return eEQUIP_ITEM.EQUIP_BOOTS;
		case 19:
			return eEQUIP_ITEM.EQUIP_RING;
		}
		return eEQUIP_ITEM.EQUIP_ITEM_MAX;
	}

	public string GetItemNameByItemUnique(int itemunique)
	{
		string empty = string.Empty;
		ITEMINFO iTEMINFO = null;
		if (this.m_cdItemTotal.TryGetValue(itemunique, out iTEMINFO))
		{
			string textFromItem = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(iTEMINFO.m_strTextKey);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromItem
			});
		}
		return empty;
	}

	public string GetItemNameByItemUnique(int itemunique, int rank)
	{
		if (itemunique <= 0)
		{
			return null;
		}
		string result = "Miss Item Unique : " + itemunique;
		ITEMINFO iTEMINFO = null;
		if (this.m_cdItemTotal.TryGetValue(itemunique, out iTEMINFO))
		{
			if (rank <= 0)
			{
				result = this.GetItemNameByItemUnique(itemunique);
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref result, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1040"),
					"grade",
					rank.ToString(),
					"targetname",
					this.GetItemNameByItemUnique(itemunique)
				});
			}
		}
		return result;
	}

	public string GetItemNameByItemUnique(ITEM a_cItem)
	{
		if (a_cItem != null)
		{
			int num = a_cItem.m_nOption[2];
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(a_cItem.m_nItemUnique);
			string result;
			if (num == 0)
			{
				result = itemNameByItemUnique;
			}
			else
			{
				result = ItemManager.RankTextColor(num) + itemNameByItemUnique;
			}
			return result;
		}
		return "Item == null";
	}

	public UIBaseInfoLoader GetItemTexture(int itemunique)
	{
		if (this.m_gCollection.ContainsKey(itemunique))
		{
			return this.m_gCollection[itemunique];
		}
		ITEMINFO iTEMINFO = null;
		if (this.m_cdItemTotal.TryGetValue(itemunique, out iTEMINFO))
		{
			UIBaseInfoLoader uIBaseInfoLoader = new UIBaseInfoLoader();
			uIBaseInfoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_MIN;
			uIBaseInfoLoader.Material = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<UIDataManager>.Instance.FilePath, "Material/Item_Icon/", iTEMINFO.m_strIconFile + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			float left = (float)(iTEMINFO.m_nIconIndex % 20) * 51f;
			float top = (float)(iTEMINFO.m_nIconIndex / 20) * 51f;
			uIBaseInfoLoader.UVs = new Rect(left, top, 50f, 50f);
			uIBaseInfoLoader.StyleName = itemunique.ToString();
			this.m_gCollection.Add(itemunique, uIBaseInfoLoader);
			return uIBaseInfoLoader;
		}
		return null;
	}

	public ITEMINFO GetItemInfo(int itemunique)
	{
		if (itemunique <= 0)
		{
			return null;
		}
		if (this.m_cdItemTotal.ContainsKey(itemunique))
		{
			return this.m_cdItemTotal[itemunique];
		}
		TsLog.LogOnlyEditor("ItemManager.GetItemInfo() Null ItemUnique " + itemunique);
		return null;
	}

	public bool IsItemTypeATB(int itemunique, long atb)
	{
		ITEMTYPE_INFO itemTypeInfo = this.GetItemTypeInfo(itemunique);
		return itemTypeInfo != null && (itemTypeInfo.ATB & atb) > 0L;
	}

	public bool IsItemATB(int itemunique, long atb)
	{
		ITEMINFO itemInfo = this.GetItemInfo(itemunique);
		return itemInfo != null && itemInfo.IsItemATB(atb);
	}

	public bool IsUsableItem(int itemunique)
	{
		eITEM_PART itemPartByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(itemunique);
		return itemPartByItemUnique == eITEM_PART.ITEMPART_ARMOR || itemPartByItemUnique == eITEM_PART.ITEMPART_WEAPON || itemPartByItemUnique == eITEM_PART.ITEMPART_ACCESSORY || this.IsItemATB(itemunique, 8L);
	}

	public string GetItemMaterialCode(int itemunique)
	{
		ITEMINFO itemInfo = this.GetItemInfo(itemunique);
		if (itemInfo == null)
		{
			return null;
		}
		return itemInfo.m_strMaterialCode;
	}

	public long GetItemPrice(int itemunique)
	{
		ITEMINFO itemInfo = this.GetItemInfo(itemunique);
		if (itemInfo == null)
		{
			return 0L;
		}
		return itemInfo.m_nPrice;
	}

	public int GetItemQuailtyLevel(int itemunique)
	{
		ITEMINFO itemInfo = this.GetItemInfo(itemunique);
		if (itemInfo == null)
		{
			return 0;
		}
		return itemInfo.m_nQualityLevel;
	}

	public void AddWeapon(ITEM_WEAPON pkItem)
	{
		ITEMINFO iTEMINFO = new ITEMINFO();
		iTEMINFO.m_nItemUnique = pkItem.ITEMUNIQUE;
		iTEMINFO.m_nItemType = NrTSingleton<ItemManager>.Instance.GetItemType(pkItem.TYPECODE);
		iTEMINFO.m_strTextKey = pkItem.TEXTKEY;
		iTEMINFO.m_strEnglishName = pkItem.ENG_NAME;
		iTEMINFO.m_strModelPath = "1";
		iTEMINFO.m_nATB = NrTSingleton<NkATB_Manager>.Instance.ParseItemATB(pkItem.ATB);
		iTEMINFO.m_nUseDate = pkItem.USEDATE;
		iTEMINFO.m_nSortOrder = pkItem.SORT_ORDER;
		iTEMINFO.m_nPrice = pkItem.PRICE;
		iTEMINFO.m_strToolTipTextKey = pkItem.TEXTKEY_TOOLTIP;
		iTEMINFO.m_strIconFile = pkItem.m_strIconFile;
		iTEMINFO.m_nIconIndex = pkItem.m_shIconIndex;
		iTEMINFO.m_nUseMinLevel = pkItem.USE_MINLV;
		iTEMINFO.m_nUseMaxLevel = pkItem.USE_MAXLV;
		iTEMINFO.m_nMinDamage = pkItem.MIN_DAMAGE;
		iTEMINFO.m_nMaxDamage = pkItem.MAX_DAMAGE;
		iTEMINFO.m_nNextLevel = pkItem.nNextLevel;
		iTEMINFO.m_nAddHP = pkItem.ADDHP;
		iTEMINFO.m_nSTR = pkItem.STR;
		iTEMINFO.m_nDEX = pkItem.DEX;
		iTEMINFO.m_nINT = pkItem.INT;
		iTEMINFO.m_nVIT = pkItem.VIT;
		iTEMINFO.m_nCriticalPlus = pkItem.CRITICALPLUS;
		iTEMINFO.m_nAttackSpeed = pkItem.ATTACKSPEED;
		iTEMINFO.m_nHitratePlus = pkItem.HITRATEPLUS;
		iTEMINFO.m_nEvasionPlus = pkItem.EVASIONPLUS;
		iTEMINFO.m_nDurability = pkItem.DURABILITY;
		iTEMINFO.m_nPruductIndex = pkItem.PRUDUCT_IDX;
		iTEMINFO.m_nQualityLevel = pkItem.QUALITY_LEVEL;
		iTEMINFO.m_strMaterialCode = pkItem.MATERIALCODE;
		iTEMINFO.m_nSetUnique = pkItem.nSetUnique;
		iTEMINFO.m_nStarGrade = pkItem.nStarGrade;
		if (this.m_cdItemTotal.ContainsKey(iTEMINFO.m_nItemUnique))
		{
			this.m_cdItemTotal[iTEMINFO.m_nItemUnique] = iTEMINFO;
		}
		else
		{
			this.m_cdItemTotal.Add(iTEMINFO.m_nItemUnique, iTEMINFO);
		}
		iTEMINFO.m_strOnlyUse = pkItem.ONLYUSE;
		for (int i = 0; i < 10; i++)
		{
			iTEMINFO.m_strOnlyUseCharCode[i] = pkItem.m_strOnlyUseCharCode[i];
		}
	}

	public void AddArmor(ITEM_ARMOR pkItem)
	{
		ITEMINFO iTEMINFO = new ITEMINFO();
		iTEMINFO.m_nItemUnique = pkItem.ITEMUNIQUE;
		iTEMINFO.m_nItemType = NrTSingleton<ItemManager>.Instance.GetItemType(pkItem.TYPECODE);
		iTEMINFO.m_strTextKey = pkItem.TEXTKEY;
		iTEMINFO.m_strEnglishName = pkItem.ENG_NAME;
		iTEMINFO.m_nATB = NrTSingleton<NkATB_Manager>.Instance.ParseItemATB(pkItem.ATB);
		iTEMINFO.m_strModelPath = "1";
		iTEMINFO.m_nUseDate = pkItem.USEDATE;
		iTEMINFO.m_nSortOrder = pkItem.SORT_ORDER;
		iTEMINFO.m_nPrice = pkItem.PRICE;
		iTEMINFO.m_strToolTipTextKey = pkItem.TEXTKEY_TOOLTIP;
		iTEMINFO.m_strIconFile = pkItem.m_strIconFile;
		iTEMINFO.m_nIconIndex = pkItem.m_shIconIndex;
		iTEMINFO.m_nAttachPartChar = NrTSingleton<NrCharKindInfoManager>.Instance.ParseCharTribeCode(pkItem.ATTACHPART);
		iTEMINFO.m_nUseMinLevel = pkItem.USE_MINLV;
		iTEMINFO.m_nUseMaxLevel = pkItem.USE_MAXLV;
		iTEMINFO.m_nNextLevel = pkItem.nNextLevel;
		iTEMINFO.m_nDefense = pkItem.DEF;
		iTEMINFO.m_nMagicDefense = pkItem.MAGICDEF;
		iTEMINFO.m_nAddHP = pkItem.ADDHP;
		iTEMINFO.m_nSTR = pkItem.STR;
		iTEMINFO.m_nDEX = pkItem.DEX;
		iTEMINFO.m_nINT = pkItem.INT;
		iTEMINFO.m_nVIT = pkItem.VIT;
		iTEMINFO.m_nCriticalPlus = pkItem.CRITICALPLUS;
		iTEMINFO.m_nAttackSpeed = pkItem.ATTACKSPEED;
		iTEMINFO.m_nHitratePlus = pkItem.HITRATEPLUS;
		iTEMINFO.m_nEvasionPlus = pkItem.EVASIONPLUS;
		iTEMINFO.m_nDurability = pkItem.DURABILITY;
		iTEMINFO.m_nPruductIndex = pkItem.PRUDUCT_ID;
		iTEMINFO.m_nQualityLevel = pkItem.QUALITY_LEVEL;
		iTEMINFO.m_strMaterialCode = pkItem.MATERIALCODE;
		iTEMINFO.m_nSetUnique = pkItem.nSetUnique;
		iTEMINFO.m_nStarGrade = pkItem.nStarGrade;
		if (this.m_cdItemTotal.ContainsKey(iTEMINFO.m_nItemUnique))
		{
			this.m_cdItemTotal[iTEMINFO.m_nItemUnique] = iTEMINFO;
		}
		else
		{
			this.m_cdItemTotal.Add(iTEMINFO.m_nItemUnique, iTEMINFO);
		}
		iTEMINFO.m_strOnlyUse = pkItem.ONLYUSE;
		for (int i = 0; i < 10; i++)
		{
			iTEMINFO.m_strOnlyUseCharCode[i] = pkItem.m_strOnlyUseCharCode[i];
		}
	}

	public void AddSecondEquip(ITEM_SECONDEQUIP pkItem)
	{
		ITEMINFO iTEMINFO = new ITEMINFO();
		iTEMINFO.m_nItemUnique = pkItem.m_nItemUnique;
		iTEMINFO.m_strTextKey = pkItem.TEXTKEY;
		iTEMINFO.m_strEnglishName = pkItem.ENG_NAME;
		iTEMINFO.m_nATB = NrTSingleton<NkATB_Manager>.Instance.ParseItemATB(pkItem.ATB);
		iTEMINFO.m_nUseDate = pkItem.USEDATE;
		iTEMINFO.m_nSortOrder = pkItem.SORT_ORDER;
		iTEMINFO.m_nPrice = pkItem.PRICE;
		iTEMINFO.m_strToolTipTextKey = pkItem.TEXTKEY_TOOLTIP;
		iTEMINFO.m_strIconFile = pkItem.m_strIconFile;
		iTEMINFO.m_nIconIndex = pkItem.m_shIconIndex;
		iTEMINFO.m_strModelPath = pkItem.MOD;
		iTEMINFO.m_nItemType = NrTSingleton<ItemManager>.Instance.GetItemType(pkItem.TYPECODE);
		iTEMINFO.m_nUseMinLevel = pkItem.USE_MINLV;
		iTEMINFO.m_nUseMaxLevel = pkItem.USE_MAXLV;
		iTEMINFO.m_nDefense = pkItem.DEF;
		iTEMINFO.m_nMagicDefense = pkItem.MAGICDEF;
		iTEMINFO.m_nAddHP = pkItem.ADDHP;
		iTEMINFO.m_nSTR = pkItem.STR;
		iTEMINFO.m_nDEX = pkItem.DEX;
		iTEMINFO.m_nINT = pkItem.INT;
		iTEMINFO.m_nVIT = pkItem.VIT;
		iTEMINFO.m_nCriticalPlus = pkItem.CRITICALPLUS;
		iTEMINFO.m_nAttackSpeed = pkItem.ATTACKSPEED;
		iTEMINFO.m_nHitratePlus = pkItem.HITRATEPLUS;
		iTEMINFO.m_nEvasionPlus = pkItem.EVASIONPLUS;
		iTEMINFO.m_nDurability = pkItem.DURABILITY;
		iTEMINFO.m_nPruductIndex = pkItem.PRUDUCT_IDX;
		iTEMINFO.m_nQualityLevel = pkItem.QUALITY_LEVEL;
		iTEMINFO.m_strMaterialCode = pkItem.MATERIALCODE;
		if (this.m_cdItemTotal.ContainsKey(iTEMINFO.m_nItemUnique))
		{
			this.m_cdItemTotal[iTEMINFO.m_nItemUnique] = iTEMINFO;
		}
		else
		{
			this.m_cdItemTotal.Add(iTEMINFO.m_nItemUnique, iTEMINFO);
		}
	}

	public void AddAccessory(ITEM_ACCESSORY pkItem)
	{
		ITEMINFO iTEMINFO = new ITEMINFO();
		iTEMINFO.m_nItemUnique = pkItem.ITEMUNIQUE;
		iTEMINFO.m_strTextKey = pkItem.TEXTKEY;
		iTEMINFO.m_strEnglishName = pkItem.ENG_NAME;
		iTEMINFO.m_nATB = NrTSingleton<NkATB_Manager>.Instance.ParseItemATB(pkItem.ATB);
		iTEMINFO.m_nUseDate = pkItem.USEDATE;
		iTEMINFO.m_nSortOrder = pkItem.SORT_ORDER;
		iTEMINFO.m_nPrice = pkItem.PRICE;
		iTEMINFO.m_strToolTipTextKey = pkItem.TEXTKEY_TOOLTIP;
		iTEMINFO.m_strIconFile = pkItem.m_strIconFile;
		iTEMINFO.m_nIconIndex = pkItem.m_shIconIndex;
		iTEMINFO.m_strModelPath = pkItem.MOD;
		iTEMINFO.m_nItemType = NrTSingleton<ItemManager>.Instance.GetItemType(pkItem.TYPECODE);
		iTEMINFO.m_nUseMinLevel = pkItem.USE_MINLV;
		iTEMINFO.m_nUseMaxLevel = pkItem.USE_MAXLV;
		iTEMINFO.m_nNextLevel = pkItem.nNextLevel;
		iTEMINFO.m_nDefense = pkItem.DEF;
		iTEMINFO.m_nMagicDefense = pkItem.MAGICDEF;
		iTEMINFO.m_nAddHP = pkItem.ADDHP;
		iTEMINFO.m_nSTR = pkItem.STR;
		iTEMINFO.m_nDEX = pkItem.DEX;
		iTEMINFO.m_nINT = pkItem.INT;
		iTEMINFO.m_nVIT = pkItem.VIT;
		iTEMINFO.m_nCriticalPlus = pkItem.CRITICALPLUS;
		iTEMINFO.m_nAttackSpeed = pkItem.ATTACKSPEED;
		iTEMINFO.m_nHitratePlus = pkItem.HITRATEPLUS;
		iTEMINFO.m_nEvasionPlus = pkItem.EVASIONPLUS;
		iTEMINFO.m_nEnmitySword = pkItem.ENMYDMG_SWORD;
		iTEMINFO.m_nEnmityScimitar = pkItem.ENMYDMG_SCIMITAR;
		iTEMINFO.m_nEnmitySpear = pkItem.ENMYDMG_SPEAR;
		iTEMINFO.m_nEnmityAx = pkItem.ENMYDMG_AX;
		iTEMINFO.m_nEnmityBow = pkItem.ENMYDMG_BOW;
		iTEMINFO.m_nEnmityCrossbow = pkItem.ENMYDMG_CROSSBOW;
		iTEMINFO.m_nEnmityFan = pkItem.ENMYDMG_FAN;
		iTEMINFO.m_nEnmityCannon = pkItem.ENMYDMG_CANNON;
		iTEMINFO.m_nDurability = pkItem.DURABILITY;
		iTEMINFO.m_nPruductIndex = pkItem.PRUDUCT_IDX;
		iTEMINFO.m_nQualityLevel = pkItem.QUALITY_LEVEL;
		iTEMINFO.m_nItemKind = pkItem.JOB_TYPE;
		iTEMINFO.m_strMaterialCode = pkItem.MATERIALCODE;
		if (this.m_cdItemTotal.ContainsKey(iTEMINFO.m_nItemUnique))
		{
			this.m_cdItemTotal[iTEMINFO.m_nItemUnique] = iTEMINFO;
		}
		else
		{
			this.m_cdItemTotal.Add(iTEMINFO.m_nItemUnique, iTEMINFO);
		}
	}

	public void AddMaterial(ITEM_MATERIAL pkItem)
	{
		ITEMINFO iTEMINFO = new ITEMINFO();
		iTEMINFO.m_nItemUnique = pkItem.ITEMUNIQUE;
		iTEMINFO.m_nItemType = NrTSingleton<ItemManager>.Instance.GetItemType(pkItem.TYPECODE);
		iTEMINFO.m_strTextKey = pkItem.TEXTKEY;
		iTEMINFO.m_strEnglishName = pkItem.ENG_NAME;
		iTEMINFO.m_nATB = NrTSingleton<NkATB_Manager>.Instance.ParseItemATB(pkItem.ATB);
		iTEMINFO.m_nUseDate = pkItem.USEDATE;
		iTEMINFO.m_nSortOrder = pkItem.SORT_ORDER;
		iTEMINFO.m_nPrice = pkItem.PRICE;
		iTEMINFO.m_strToolTipTextKey = pkItem.TEXTKEY_TOOLTIP;
		iTEMINFO.m_strIconFile = pkItem.m_strIconFile;
		iTEMINFO.m_nIconIndex = pkItem.m_shIconIndex;
		iTEMINFO.m_nPruductIndex = pkItem.PRUDUCT_IDX;
		iTEMINFO.m_nGroupIndex = pkItem.GROUP_IDX;
		iTEMINFO.m_strMaterialCode = pkItem.MATERIALCODE;
		iTEMINFO.m_strTextColorCode = pkItem.TEXT_COLOR_CODE;
		iTEMINFO.m_nStarGrade = pkItem.STAR_GRADE;
		if (this.m_cdItemTotal.ContainsKey(iTEMINFO.m_nItemUnique))
		{
			this.m_cdItemTotal[iTEMINFO.m_nItemUnique] = iTEMINFO;
		}
		else
		{
			this.m_cdItemTotal.Add(iTEMINFO.m_nItemUnique, iTEMINFO);
		}
	}

	public void AddSupply(ITEM_SUPPLIES pkItem)
	{
		ITEMINFO iTEMINFO = new ITEMINFO();
		iTEMINFO.m_nItemUnique = pkItem.ITEMUNIQUE;
		iTEMINFO.m_nItemType = NrTSingleton<ItemManager>.Instance.GetItemType(pkItem.TYPECODE);
		iTEMINFO.m_strTextKey = pkItem.TEXTKEY;
		iTEMINFO.m_strEnglishName = pkItem.ENG_NAME;
		iTEMINFO.m_nATB = NrTSingleton<NkATB_Manager>.Instance.ParseItemATB(pkItem.ATB);
		iTEMINFO.m_nUseDate = pkItem.USEDATE;
		iTEMINFO.m_nSortOrder = pkItem.SORT_ORDER;
		iTEMINFO.m_nPrice = pkItem.PRICE;
		iTEMINFO.m_strToolTipTextKey = pkItem.TEXTKEY_TOOLTIP;
		iTEMINFO.m_strIconFile = pkItem.m_strIconFile;
		iTEMINFO.m_nIconIndex = pkItem.m_shIconIndex;
		iTEMINFO.m_nUseMinLevel = pkItem.USE_MINLV;
		iTEMINFO.m_nUseMaxLevel = pkItem.USE_MAXLV;
		iTEMINFO.m_nFunctions = pkItem.FUNCTIONS;
		iTEMINFO.m_nParam[0] = pkItem.PARAM1;
		iTEMINFO.m_nParam[1] = pkItem.PARAM2;
		iTEMINFO.m_strMaterialCode = pkItem.MATERIALCODE;
		if (this.m_cdItemTotal.ContainsKey(iTEMINFO.m_nItemUnique))
		{
			this.m_cdItemTotal[iTEMINFO.m_nItemUnique] = iTEMINFO;
		}
		else
		{
			this.m_cdItemTotal.Add(iTEMINFO.m_nItemUnique, iTEMINFO);
		}
	}

	public void AddTicket(ITEM_TICKET pkItem)
	{
		ITEMINFO iTEMINFO = new ITEMINFO();
		iTEMINFO.m_nItemUnique = pkItem.ITEMUNIQUE;
		iTEMINFO.m_nItemType = NrTSingleton<ItemManager>.Instance.GetItemType(pkItem.TYPECODE);
		iTEMINFO.m_strTextKey = pkItem.TEXTKEY;
		iTEMINFO.m_strEnglishName = pkItem.ENG_NAME;
		iTEMINFO.m_nATB = NrTSingleton<NkATB_Manager>.Instance.ParseItemATB(pkItem.ATB);
		iTEMINFO.m_nUseDate = pkItem.USEDATE;
		iTEMINFO.m_nSortOrder = pkItem.SORT_ORDER;
		iTEMINFO.m_nPrice = pkItem.PRICE;
		iTEMINFO.m_strToolTipTextKey = pkItem.TEXTKEY_TOOLTIP;
		iTEMINFO.m_strIconFile = pkItem.m_strIconFile;
		iTEMINFO.m_nIconIndex = pkItem.m_shIconIndex;
		iTEMINFO.m_nUseMinLevel = pkItem.USE_MINLV;
		iTEMINFO.m_nUseMaxLevel = pkItem.USE_MAXLV;
		iTEMINFO.m_nFunctions = pkItem.FUNCTIONS;
		iTEMINFO.m_nParam[0] = pkItem.PARAM1;
		iTEMINFO.m_nParam[1] = pkItem.PARAM2;
		iTEMINFO.m_strMaterialCode = pkItem.MATERIALCODE;
		iTEMINFO.m_nCardType = pkItem.CARD_TYPE;
		iTEMINFO.m_nRecruitType = pkItem.Recruit_Type;
		if (this.m_cdItemTotal.ContainsKey(iTEMINFO.m_nItemUnique))
		{
			this.m_cdItemTotal[iTEMINFO.m_nItemUnique] = iTEMINFO;
		}
		else
		{
			this.m_cdItemTotal.Add(iTEMINFO.m_nItemUnique, iTEMINFO);
		}
	}

	public void AddBox(ITEM_BOX pkItem)
	{
		ITEMINFO iTEMINFO = new ITEMINFO();
		iTEMINFO.m_nItemUnique = pkItem.ITEMUNIQUE;
		iTEMINFO.m_strTextKey = pkItem.TEXTKEY;
		iTEMINFO.m_strEnglishName = pkItem.ENG_NAME;
		iTEMINFO.m_nATB = NrTSingleton<NkATB_Manager>.Instance.ParseItemATB(pkItem.ATB);
		iTEMINFO.m_nUseDate = pkItem.USEDATE;
		iTEMINFO.m_nSortOrder = pkItem.SORT_ORDER;
		iTEMINFO.m_nPrice = pkItem.PRICE;
		iTEMINFO.m_strToolTipTextKey = pkItem.TEXTKEY_TOOLTIP;
		iTEMINFO.m_strIconFile = pkItem.m_strIconFile;
		iTEMINFO.m_nIconIndex = pkItem.m_shIconIndex;
		iTEMINFO.m_nUseMinLevel = pkItem.USE_MINLV;
		iTEMINFO.m_nUseMaxLevel = pkItem.USE_MAXLV;
		iTEMINFO.m_nItemType = NrTSingleton<ItemManager>.Instance.GetItemType(pkItem.TYPECODE);
		iTEMINFO.m_nBoxItemUnique[0] = pkItem.ITEMUNIQUE1;
		iTEMINFO.m_nBoxItemNumber[0] = pkItem.ITEMNUM1;
		iTEMINFO.m_nBoxItemProbability[0] = pkItem.ITEMPROB1;
		iTEMINFO.m_nBoxItemUnique[1] = pkItem.ITEMUNIQUE2;
		iTEMINFO.m_nBoxItemNumber[1] = pkItem.ITEMNUM2;
		iTEMINFO.m_nBoxItemProbability[1] = pkItem.ITEMPROB2;
		iTEMINFO.m_nBoxItemUnique[2] = pkItem.ITEMUNIQUE3;
		iTEMINFO.m_nBoxItemNumber[2] = pkItem.ITEMNUM3;
		iTEMINFO.m_nBoxItemProbability[2] = pkItem.ITEMPROB3;
		iTEMINFO.m_nBoxItemUnique[3] = pkItem.ITEMUNIQUE4;
		iTEMINFO.m_nBoxItemNumber[3] = pkItem.ITEMNUM4;
		iTEMINFO.m_nBoxItemProbability[3] = pkItem.ITEMPROB4;
		iTEMINFO.m_nBoxItemUnique[4] = pkItem.ITEMUNIQUE5;
		iTEMINFO.m_nBoxItemNumber[4] = pkItem.ITEMNUM5;
		iTEMINFO.m_nBoxItemProbability[4] = pkItem.ITEMPROB5;
		iTEMINFO.m_nBoxItemUnique[5] = pkItem.ITEMUNIQUE6;
		iTEMINFO.m_nBoxItemNumber[5] = pkItem.ITEMNUM6;
		iTEMINFO.m_nBoxItemProbability[5] = pkItem.ITEMPROB6;
		iTEMINFO.m_nBoxItemUnique[6] = pkItem.ITEMUNIQUE7;
		iTEMINFO.m_nBoxItemNumber[6] = pkItem.ITEMNUM7;
		iTEMINFO.m_nBoxItemProbability[6] = pkItem.ITEMPROB7;
		iTEMINFO.m_nBoxItemUnique[7] = pkItem.ITEMUNIQUE8;
		iTEMINFO.m_nBoxItemNumber[7] = pkItem.ITEMNUM8;
		iTEMINFO.m_nBoxItemProbability[7] = pkItem.ITEMPROB8;
		iTEMINFO.m_nBoxItemUnique[8] = pkItem.ITEMUNIQUE9;
		iTEMINFO.m_nBoxItemNumber[8] = pkItem.ITEMNUM9;
		iTEMINFO.m_nBoxItemProbability[8] = pkItem.ITEMPROB9;
		iTEMINFO.m_nBoxItemUnique[9] = pkItem.ITEMUNIQUE10;
		iTEMINFO.m_nBoxItemNumber[9] = pkItem.ITEMNUM10;
		iTEMINFO.m_nBoxItemProbability[9] = pkItem.ITEMPROB10;
		iTEMINFO.m_nBoxItemUnique[10] = pkItem.ITEMUNIQUE11;
		iTEMINFO.m_nBoxItemNumber[10] = pkItem.ITEMNUM11;
		iTEMINFO.m_nBoxItemProbability[10] = pkItem.ITEMPROB11;
		iTEMINFO.m_nBoxItemUnique[11] = pkItem.ITEMUNIQUE12;
		iTEMINFO.m_nBoxItemNumber[11] = pkItem.ITEMNUM12;
		iTEMINFO.m_nBoxItemProbability[11] = pkItem.ITEMPROB12;
		iTEMINFO.m_strMaterialCode = pkItem.MaterialCode;
		iTEMINFO.m_nBoxRank = pkItem.BOXRANK;
		iTEMINFO.m_nBoxSealInfo = pkItem.BOXSEALINFO;
		iTEMINFO.m_nNeedOpenItemUnique = pkItem.NEEDOPENITEMUNIQUE;
		iTEMINFO.m_nNeedOpenItemNum = pkItem.NEEDOPENITEMNUM;
		if (this.m_cdItemTotal.ContainsKey(iTEMINFO.m_nItemUnique))
		{
			this.m_cdItemTotal[iTEMINFO.m_nItemUnique] = iTEMINFO;
		}
		else
		{
			this.m_cdItemTotal.Add(iTEMINFO.m_nItemUnique, iTEMINFO);
		}
	}

	public void AddBoxGroup(ITEM_BOX_GROUP_DATA pkItemBoxGroupData)
	{
		ITEM_BOX_GROUP iTEM_BOX_GROUP = null;
		if (this.m_cdItemBoxGroup.TryGetValue(pkItemBoxGroupData.i32BoxUnique, out iTEM_BOX_GROUP))
		{
			iTEM_BOX_GROUP.AddGroupItemData(pkItemBoxGroupData);
			return;
		}
		ITEM_BOX_GROUP iTEM_BOX_GROUP2 = new ITEM_BOX_GROUP();
		iTEM_BOX_GROUP2.AddGroupItemData(pkItemBoxGroupData);
		this.m_cdItemBoxGroup.Add(iTEM_BOX_GROUP2.i32BoxUnique, iTEM_BOX_GROUP2);
	}

	public ITEM_BOX_GROUP GetBoxGroup(int BoxItemUnique)
	{
		if (this.m_cdItemBoxGroup.ContainsKey(BoxItemUnique))
		{
			return this.m_cdItemBoxGroup[BoxItemUnique];
		}
		return null;
	}

	public int GetPosTypeBoxContainItem(int a_lBoxItemUnique, int a_nArrayIndex)
	{
		ITEMINFO iTEMINFO = null;
		if (this.m_cdItemTotal.TryGetValue(a_lBoxItemUnique, out iTEMINFO))
		{
			return Protocol_Item.GetItemPosType(iTEMINFO.m_nBoxItemUnique[a_nArrayIndex]);
		}
		return 0;
	}

	public int GetItemContainCount(int a_lBoxItemUnique)
	{
		int num = 0;
		ITEMINFO iTEMINFO = null;
		if (this.m_cdItemTotal.TryGetValue(a_lBoxItemUnique, out iTEMINFO))
		{
			for (int i = 0; i < iTEMINFO.m_nBoxItemUnique.Length; i++)
			{
				if (iTEMINFO.m_nBoxItemUnique[i] > 0)
				{
					num++;
				}
			}
		}
		return num;
	}

	public void AddQuest(ITEM_QUEST pkItem)
	{
		ITEMINFO iTEMINFO = new ITEMINFO();
		iTEMINFO.m_nItemUnique = pkItem.ITEMUNIQUE;
		iTEMINFO.m_nItemType = NrTSingleton<ItemManager>.Instance.GetItemType(pkItem.TYPECODE);
		iTEMINFO.m_strTextKey = pkItem.TEXTKEY;
		iTEMINFO.m_strEnglishName = pkItem.ENG_NAME;
		iTEMINFO.m_nATB = NrTSingleton<NkATB_Manager>.Instance.ParseItemATB(pkItem.ATB);
		iTEMINFO.m_nUseDate = pkItem.USEDATE;
		iTEMINFO.m_nSortOrder = pkItem.SORT_ORDER;
		iTEMINFO.m_nPrice = pkItem.PRICE;
		iTEMINFO.m_strToolTipTextKey = pkItem.TEXTKEY_TOOLTIP;
		iTEMINFO.m_strIconFile = pkItem.m_strIconFile;
		iTEMINFO.m_nIconIndex = pkItem.m_shIconIndex;
		iTEMINFO.m_nQuestLink[0] = pkItem.LINK_QUEST1;
		iTEMINFO.m_nQuestLink[1] = pkItem.LINK_QUEST2;
		iTEMINFO.m_nQuestLink[2] = pkItem.LINK_QUEST3;
		iTEMINFO.m_nIsUseQuest = pkItem.IS_USE;
		iTEMINFO.m_nCallMonster = pkItem.CALL_MOB;
		iTEMINFO.m_nCallMonsterArea = pkItem.CALL_MOBAREA;
		iTEMINFO.m_nPruductIndex = pkItem.PRUDUCT_IDX;
		iTEMINFO.m_strMaterialCode = pkItem.MATERIALCODE;
		iTEMINFO.m_nFunctions = (byte)pkItem.ItemFunc;
		iTEMINFO.m_nQuestFuncParam = pkItem.FuncParam;
		iTEMINFO.m_nQuestIsDrop = pkItem.IsDrop;
		iTEMINFO.m_nQuestIsDisappear = pkItem.IsDisappear;
		if (this.m_cdItemTotal.ContainsKey(iTEMINFO.m_nItemUnique))
		{
			this.m_cdItemTotal[iTEMINFO.m_nItemUnique] = iTEMINFO;
		}
		else
		{
			this.m_cdItemTotal.Add(iTEMINFO.m_nItemUnique, iTEMINFO);
		}
	}

	public int GetItemUnique(string strItemName)
	{
		int result = 0;
		foreach (ITEMINFO current in NrTSingleton<ItemManager>.Instance.Get_Total_Collection().Values)
		{
			string textFromItem = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(current.m_strTextKey);
			if (textFromItem.Contains(strItemName))
			{
				result = current.m_nItemUnique;
				break;
			}
		}
		return result;
	}

	public int GetTotalDef(ITEM item)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(item.m_nItemUnique);
		int itemQuailtyLevel = NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(item.m_nItemUnique);
		Item_Rank item_Rank = Item_Rank_Manager.Get_Instance().Get_RankData(itemQuailtyLevel, item.m_nRank);
		if (item_Rank == null || itemInfo == null)
		{
			return 0;
		}
		float num = (float)item_Rank.ItemPerformanceRate / 100f;
		return (int)Math.Ceiling((double)((float)itemInfo.m_nDefense * num));
	}

	public string Get_Item_MaterialCode(int a_nItemUnique)
	{
		if (this.m_cdItemTotal.ContainsKey(a_nItemUnique))
		{
			return this.m_cdItemTotal[a_nItemUnique].m_strMaterialCode;
		}
		return null;
	}

	public ITEM GetBoxItemTemp(int a_nItemUnique, int a_nIndex)
	{
		if (Protocol_Item.GetStaticItemType(a_nItemUnique) != eITEM_TYPE.ITEMTYPE_BOX || a_nIndex < 0 || a_nIndex >= 12)
		{
			return null;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(a_nItemUnique);
		if (itemInfo == null)
		{
			return null;
		}
		if (itemInfo.IsItemATB(65536L))
		{
			ITEM_BOX_GROUP boxGroup = NrTSingleton<ItemManager>.Instance.GetBoxGroup(a_nItemUnique);
			if (boxGroup == null)
			{
				return null;
			}
			ITEM iTEM = new ITEM();
			iTEM.m_nItemID = -9223372036854775808L;
			iTEM.m_nItemUnique = boxGroup.i32GroupItemUnique[a_nIndex];
			iTEM.m_nItemNum = boxGroup.i32GroupItemNum[a_nIndex];
			iTEM.m_nOption[0] = 100;
			iTEM.m_nOption[1] = 100;
			iTEM.m_nOption[2] = boxGroup.i32GroupItemGrade[a_nIndex];
			iTEM.m_nOption[3] = 1;
			iTEM.m_nOption[4] = boxGroup.i32GroupItemSkillUnique[a_nIndex];
			iTEM.m_nOption[5] = boxGroup.i32GroupItemSkillLevel[a_nIndex];
			iTEM.m_nOption[7] = boxGroup.i32GroupItemTradePoint[a_nIndex];
			iTEM.m_nOption[8] = boxGroup.i32GroupItemReducePoint[a_nIndex];
			iTEM.m_nOption[6] = boxGroup.i32GroupItemSkill2Unique[a_nIndex];
			iTEM.m_nOption[9] = boxGroup.i32GroupItemSkill2Level[a_nIndex];
			iTEM.m_nDurability = 100;
			return iTEM;
		}
		else
		{
			if (this.m_cdItemTotal.ContainsKey(a_nItemUnique))
			{
				int num = this.m_cdItemTotal[a_nItemUnique].m_nBoxItemUnique[a_nIndex];
				ITEM iTEM2 = new ITEM();
				iTEM2.m_nItemID = -9223372036854775808L;
				iTEM2.m_nItemUnique = num;
				iTEM2.m_nItemNum = (int)((short)this.m_cdItemTotal[a_nItemUnique].m_nBoxItemNumber[a_nIndex]);
				if (this.m_cdItemTotal.ContainsKey(num))
				{
					iTEM2.m_nDurability = (int)((byte)this.m_cdItemTotal[num].m_nDurability);
				}
				else
				{
					iTEM2.m_nDurability = 100;
				}
				return iTEM2;
			}
			return null;
		}
	}

	public NkWeaponTypeInfo GetWeaponTypeInfo(int itemunique)
	{
		if (itemunique <= 0)
		{
			return null;
		}
		ITEMTYPE_INFO itemTypeInfo = this.GetItemTypeInfo(itemunique);
		if (itemTypeInfo != null)
		{
			return NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponTypeInfo(itemTypeInfo.WEAPONTYPE);
		}
		return null;
	}

	public bool IsWeaponATB(int itemunique, long atb)
	{
		NkWeaponTypeInfo weaponTypeInfo = this.GetWeaponTypeInfo(itemunique);
		return weaponTypeInfo != null && weaponTypeInfo.IsATB(atb);
	}

	public int GetItemMinLevel(int itemUnique)
	{
		ITEMINFO itemInfo = this.GetItemInfo(itemUnique);
		if (itemInfo == null)
		{
			return 0;
		}
		return itemInfo.m_nUseMinLevel;
	}

	public int GetItemMinLevelFromItem(ITEM Item)
	{
		ITEMINFO itemInfo = this.GetItemInfo(Item.m_nItemUnique);
		if (itemInfo == null)
		{
			return 0;
		}
		return itemInfo.GetUseMinLevel(Item);
	}

	public int GetItemMaxLevel(int itemUnique)
	{
		ITEMINFO itemInfo = this.GetItemInfo(itemUnique);
		if (itemInfo == null)
		{
			return 0;
		}
		return itemInfo.m_nUseMaxLevel;
	}

	public void PlayItemUseSound(int itemUnique, bool bForce)
	{
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemUnique);
		if (itemInfo == null)
		{
			return;
		}
		string strMaterialCode = itemInfo.m_strMaterialCode;
		if (string.IsNullOrEmpty(strMaterialCode))
		{
			return;
		}
		if (itemInfo.m_nItemType == 14)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_ITEM", strMaterialCode, "USE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public bool IsStack(int itemUnique)
	{
		ITEMTYPE_INFO itemTypeInfo = this.GetItemTypeInfo(itemUnique);
		if (itemTypeInfo == null)
		{
			return false;
		}
		switch (itemTypeInfo.ITEMPART)
		{
		case eITEM_PART.ITEMPART_MATERIAL:
		case eITEM_PART.ITEMPART_BOX:
		case eITEM_PART.ITEMPART_SUPPLY:
		case eITEM_PART.ITEMPART_QUEST:
			return true;
		}
		return false;
	}

	public void ItemSupplyUseReq(object a_oObject)
	{
		GS_ITEM_SUPPLY_USE_REQ gS_ITEM_SUPPLY_USE_REQ = (GS_ITEM_SUPPLY_USE_REQ)a_oObject;
		if (gS_ITEM_SUPPLY_USE_REQ == null)
		{
			return;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_SUPPLY_USE_REQ, gS_ITEM_SUPPLY_USE_REQ);
	}

	public static string RankTextColor(eITEM_RANK_TYPE type)
	{
		return ItemManager.RankTextColor((int)type);
	}

	public static string RankTextColor(int Rank)
	{
		string result = string.Empty;
		switch (Rank)
		{
		case 1:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");
			break;
		case 2:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1106");
			break;
		case 3:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1304");
			break;
		case 4:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1402");
			break;
		case 5:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1107");
			break;
		case 6:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1401");
			break;
		default:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1101");
			break;
		}
		return result;
	}

	public static string RankText(int Rank)
	{
		string result = string.Empty;
		switch (Rank)
		{
		case 1:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("116");
			break;
		case 2:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("115");
			break;
		case 3:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("114");
			break;
		case 4:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("113");
			break;
		case 5:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("112");
			break;
		case 6:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2044");
			break;
		default:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("116");
			break;
		}
		return result;
	}

	public static string RankStateString(int Rank)
	{
		string result = string.Empty;
		switch (Rank)
		{
		case 1:
		case 2:
			result = "normal";
			break;
		case 3:
		case 4:
			result = "good";
			break;
		case 5:
		case 6:
			result = "best";
			break;
		default:
			result = "normal";
			break;
		}
		return result;
	}

	public static string ChangeRankToString(int Rank)
	{
		string result = string.Empty;
		switch (Rank)
		{
		case 1:
			result = "D";
			break;
		case 2:
			result = "C";
			break;
		case 3:
			result = "B";
			break;
		case 4:
			result = "A";
			break;
		case 5:
			result = "S";
			break;
		case 6:
			result = "SS";
			break;
		default:
			result = "D";
			break;
		}
		return result;
	}

	public static string ChangeRankToColorString(int Rank)
	{
		string result = string.Empty;
		switch (Rank)
		{
		case 1:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1101") + "D";
			break;
		case 2:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1106") + "C";
			break;
		case 3:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1304") + "B";
			break;
		case 4:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1402") + "A";
			break;
		case 5:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + "S";
			break;
		case 6:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1401") + "SS";
			break;
		default:
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1101") + "D";
			break;
		}
		return result;
	}

	public static string GetWeaponTypeName(eWEAPON_TYPE eWeaponType)
	{
		string result = string.Empty;
		switch (eWeaponType)
		{
		case eWEAPON_TYPE.WEAPON_SWORD:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem("1");
			break;
		case eWEAPON_TYPE.WEAPON_SPEAR:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem("2");
			break;
		case eWEAPON_TYPE.WEAPON_AXE:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem("3");
			break;
		case eWEAPON_TYPE.WEAPON_BOW:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem("4");
			break;
		case eWEAPON_TYPE.WEAPON_GUN:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem("5");
			break;
		case eWEAPON_TYPE.WEAPON_CANNON:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem("6");
			break;
		case eWEAPON_TYPE.WEAPON_STAFF:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem("7");
			break;
		case eWEAPON_TYPE.WEAPON_BIBLE:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem("8");
			break;
		}
		return result;
	}

	public int GetUseMinLevel(ITEM Item)
	{
		ITEMINFO itemInfo = this.GetItemInfo(Item.m_nItemUnique);
		if (itemInfo != null)
		{
			return itemInfo.GetUseMinLevel(Item);
		}
		return 1;
	}

	public int GetItemMoveType_InvenToInven(int itemPosType)
	{
		int result = 0;
		switch (itemPosType)
		{
		case 1:
			result = 3;
			break;
		case 2:
			result = 4;
			break;
		case 3:
			result = 5;
			break;
		case 4:
			result = 6;
			break;
		case 5:
			result = 1;
			break;
		case 6:
			result = 2;
			break;
		case 7:
			result = 15;
			break;
		}
		return result;
	}

	public int GetItemMoveType_InvenToSol(int itemPosType)
	{
		int result = 0;
		switch (itemPosType)
		{
		case 1:
			result = 7;
			break;
		case 2:
			result = 8;
			break;
		case 3:
			result = 9;
			break;
		case 4:
			result = 10;
			break;
		}
		return result;
	}

	public int GetItemMoveType_SolToInven(int itemPosType)
	{
		int result = 0;
		switch (itemPosType)
		{
		case 1:
			result = 11;
			break;
		case 2:
			result = 12;
			break;
		case 3:
			result = 13;
			break;
		case 4:
			result = 14;
			break;
		}
		return result;
	}

	public bool CheckBoxNeedItem(int boxitemunique, bool bCheckInven, bool bShowMessage)
	{
		ITEMINFO itemInfo = this.GetItemInfo(boxitemunique);
		if (itemInfo == null)
		{
			return false;
		}
		if (itemInfo.m_nNeedOpenItemUnique <= 0)
		{
			return true;
		}
		ITEMINFO itemInfo2 = this.GetItemInfo(itemInfo.m_nNeedOpenItemUnique);
		if (itemInfo2 == null)
		{
			return false;
		}
		if (bCheckInven)
		{
			ITEM firstItemByUnique = NkUserInventory.GetInstance().GetFirstItemByUnique(itemInfo2.m_nItemUnique);
			if (firstItemByUnique != null && firstItemByUnique.IsValid() && firstItemByUnique.m_nItemNum >= itemInfo.m_nNeedOpenItemNum)
			{
				return true;
			}
		}
		if (bShowMessage)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
				"targetname",
				this.GetItemNameByItemUnique(itemInfo2.m_nItemUnique)
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		return false;
	}

	public string GetName(ITEM pkItem)
	{
		if (pkItem == null)
		{
			return string.Empty;
		}
		int num = pkItem.m_nOption[4];
		int num2 = pkItem.m_nOption[6];
		if (0 < pkItem.m_nRank)
		{
			return NrTSingleton<UIDataManager>.Instance.GetString("+", pkItem.m_nRank.ToString(), this.GetItemNameByItemUnique(pkItem.m_nItemUnique));
		}
		if (num != 0 && num2 == 0)
		{
			ITEMSKILL_INFO iTEMSKILL_INFO = NrTSingleton<NrItemSkillInfoManager>.Instance.Get_Value(num);
			if (iTEMSKILL_INFO != null)
			{
				return NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper(iTEMSKILL_INFO.PrefixText), " ", this.GetItemNameByItemUnique(pkItem.m_nItemUnique));
			}
		}
		return this.GetItemNameByItemUnique(pkItem.m_nItemUnique);
	}

	public string GetRankColorName(ITEM pkItem)
	{
		if (pkItem == null)
		{
			return string.Empty;
		}
		int num = pkItem.m_nOption[4];
		int num2 = pkItem.m_nOption[6];
		int rank = pkItem.m_nOption[2];
		if (0 < pkItem.m_nRank)
		{
			return NrTSingleton<UIDataManager>.Instance.GetString("+", pkItem.m_nRank.ToString(), this.GetItemNameByItemUnique(pkItem.m_nItemUnique));
		}
		if (num != 0 && num2 == 0)
		{
			ITEMSKILL_INFO iTEMSKILL_INFO = NrTSingleton<NrItemSkillInfoManager>.Instance.Get_Value(num);
			if (iTEMSKILL_INFO != null)
			{
				return NrTSingleton<UIDataManager>.Instance.GetString(ItemManager.RankTextColor(rank), NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper(iTEMSKILL_INFO.PrefixText), " ", this.GetItemNameByItemUnique(pkItem.m_nItemUnique));
			}
		}
		return ItemManager.RankTextColor(rank) + this.GetItemNameByItemUnique(pkItem.m_nItemUnique);
	}

	public string GetRankName(ITEM pkItem)
	{
		if (pkItem == null)
		{
			return string.Empty;
		}
		int rank = pkItem.m_nOption[2];
		return this.GetRankName(rank, pkItem);
	}

	public string GetRankName(int Rank, ITEM pkItem)
	{
		if (pkItem == null)
		{
			return string.Empty;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("450");
		string str = ItemManager.RankTextColor(Rank);
		string str2 = ItemManager.RankText(Rank);
		return str + str2;
	}

	public bool isItemGoldBar(int ItemUnique)
	{
		eITEM_PART itemPartByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(ItemUnique);
		if (itemPartByItemUnique == eITEM_PART.ITEMPART_SUPPLY)
		{
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(ItemUnique);
			if (itemInfo == null)
			{
				return false;
			}
			if (itemInfo.m_nFunctions == 11)
			{
				return true;
			}
		}
		return false;
	}

	public bool isExchangeItem(int ItemUnique)
	{
		eITEM_PART itemPartByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(ItemUnique);
		if (itemPartByItemUnique == eITEM_PART.ITEMPART_SUPPLY)
		{
			eITEM_SUPPLY_FUNCTION eITEM_SUPPLY_FUNCTION = Protocol_Item.Get_Item_Supplies_Function_Index(ItemUnique);
			if (eITEM_SUPPLY_FUNCTION == eITEM_SUPPLY_FUNCTION.SUPPLY_EXCHANGEITEM)
			{
				return true;
			}
		}
		return false;
	}

	public bool isBattleSpeedItem(int ItemUnique)
	{
		eITEM_PART itemPartByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(ItemUnique);
		if (itemPartByItemUnique == eITEM_PART.ITEMPART_SUPPLY)
		{
			eITEM_SUPPLY_FUNCTION eITEM_SUPPLY_FUNCTION = Protocol_Item.Get_Item_Supplies_Function_Index(ItemUnique);
			if (eITEM_SUPPLY_FUNCTION == eITEM_SUPPLY_FUNCTION.SUPPLY_BATTLESPEED)
			{
				return true;
			}
		}
		return false;
	}

	public void Add_GroupSolTicket(long _i64Unique, GROUP_SOL_TICKET _cInfo)
	{
		GROUPTICKET_INFO gROUPTICKET_INFO = new GROUPTICKET_INFO();
		gROUPTICKET_INFO.m_strSolKind = _cInfo.strCHARCODE;
		gROUPTICKET_INFO.m_i8Grade = _cInfo.i8Grade;
		if (this.m_dicGroupSolTicket.ContainsKey(_i64Unique))
		{
			this.m_dicGroupSolTicket[_i64Unique].Add(gROUPTICKET_INFO);
		}
		else
		{
			this.m_dicGroupSolTicket.Add(_i64Unique, new List<GROUPTICKET_INFO>());
			this.m_dicGroupSolTicket[_i64Unique].Add(gROUPTICKET_INFO);
		}
	}

	public byte GetTopGrade_GroupSolTicket(long _i64Unique, string _strSolKind)
	{
		if (_i64Unique < 0L)
		{
			return 0;
		}
		if (string.IsNullOrEmpty(_strSolKind))
		{
			return 0;
		}
		if (!this.m_dicGroupSolTicket.ContainsKey(_i64Unique))
		{
			return 0;
		}
		byte b = 0;
		List<GROUPTICKET_INFO> list = this.m_dicGroupSolTicket[_i64Unique];
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].m_strSolKind.Equals(_strSolKind) && b < list[i].m_i8Grade)
				{
					b = list[i].m_i8Grade;
				}
			}
		}
		return b;
	}

	public bool isEquipItem(int ItemUnique)
	{
		eITEM_POSTYPE posTypeByItemType = this.GetPosTypeByItemType(ItemUnique);
		return eITEM_POSTYPE.INVEN_EQUIP_DEFENCE <= posTypeByItemType && eITEM_POSTYPE.INVEN_EQUIP_WEAPON_MAGIC >= posTypeByItemType;
	}

	public bool IsPrivateEquip(int i32ItemUnique)
	{
		if (i32ItemUnique < 0)
		{
			return false;
		}
		ITEMINFO itemInfo = this.GetItemInfo(i32ItemUnique);
		return itemInfo != null && !(itemInfo.m_strOnlyUse == "0") && !string.IsNullOrEmpty(itemInfo.m_strOnlyUse);
	}

	public bool IsWearEquipItem(int i32ItemUnique, string strCharCode)
	{
		if (string.IsNullOrEmpty(strCharCode))
		{
			return false;
		}
		if (i32ItemUnique < 0)
		{
			return false;
		}
		ITEMINFO itemInfo = this.GetItemInfo(i32ItemUnique);
		if (itemInfo == null)
		{
			return false;
		}
		if (itemInfo.m_strOnlyUse == "0" || string.IsNullOrEmpty(itemInfo.m_strOnlyUse))
		{
			return true;
		}
		for (int i = 0; i < 10; i++)
		{
			if (itemInfo.m_strOnlyUseCharCode[i].Equals(strCharCode))
			{
				return true;
			}
		}
		return false;
	}
}
