using GAME;
using System;
using System.Collections.Generic;

public class NrSetItemDataManager : NrTSingleton<NrSetItemDataManager>
{
	private Dictionary<int, SETITEM_DATA> m_ItemSetDataList;

	private NkValueParse<int> m_kSetItemOptinType;

	private NrSetItemDataManager()
	{
		this.m_ItemSetDataList = new Dictionary<int, SETITEM_DATA>();
		this.m_kSetItemOptinType = new NkValueParse<int>();
		this.SetParaDataCode();
	}

	public void SetData(SETITEM_DATA info)
	{
		if (info != null)
		{
			this.m_ItemSetDataList.Add(info.m_Idx, info);
		}
	}

	public void SetParaDataCode()
	{
		this.m_kSetItemOptinType.InsertCodeValue("DEF", 1);
		this.m_kSetItemOptinType.InsertCodeValue("MDEF", 2);
		this.m_kSetItemOptinType.InsertCodeValue("ADDHP", 3);
		this.m_kSetItemOptinType.InsertCodeValue("STR", 4);
		this.m_kSetItemOptinType.InsertCodeValue("DEX", 5);
		this.m_kSetItemOptinType.InsertCodeValue("INT", 6);
		this.m_kSetItemOptinType.InsertCodeValue("VIT", 7);
		this.m_kSetItemOptinType.InsertCodeValue("CRIT", 8);
		this.m_kSetItemOptinType.InsertCodeValue("DAMAGE_P", 9);
		this.m_kSetItemOptinType.InsertCodeValue("DAMAGE", 10);
	}

	public int GetOptionType(string OptionType)
	{
		return this.m_kSetItemOptinType.GetValue(OptionType);
	}

	public void Check(ITEM_SET pData, ref int Def, ref int mdef, ref int addHP, ref int STR, ref int DEX, ref int INT, ref int VIT, ref int CRIT, ref int Damage_p, ref int Damage)
	{
		if (this.m_ItemSetDataList.ContainsKey(pData.m_SetUnique))
		{
			SETITEM_DATA sETITEM_DATA = this.m_ItemSetDataList[pData.m_SetUnique];
			for (int i = 0; i < 6; i++)
			{
				if (i <= pData.m_nSetCount)
				{
					switch (sETITEM_DATA.m_nSetEffectCode[i])
					{
					case 1:
						Def += sETITEM_DATA.m_nValue[i];
						break;
					case 2:
						mdef += sETITEM_DATA.m_nValue[i];
						break;
					case 3:
						addHP += sETITEM_DATA.m_nValue[i];
						break;
					case 4:
						STR += sETITEM_DATA.m_nValue[i];
						break;
					case 5:
						DEX += sETITEM_DATA.m_nValue[i];
						break;
					case 6:
						INT += sETITEM_DATA.m_nValue[i];
						break;
					case 7:
						VIT += sETITEM_DATA.m_nValue[i];
						break;
					case 8:
						CRIT += sETITEM_DATA.m_nValue[i];
						break;
					case 9:
						Damage_p += sETITEM_DATA.m_nValue[i];
						break;
					case 10:
						Damage += sETITEM_DATA.m_nValue[i];
						break;
					}
				}
			}
		}
	}

	public int GetSetData(Dictionary<int, ITEM_SET> _ItemSets, eSETITEM_OPTION_TYPE _type)
	{
		if (_ItemSets == null)
		{
			return 0;
		}
		int num = 0;
		foreach (ITEM_SET current in _ItemSets.Values)
		{
			SETITEM_DATA sETITEM_DATA = this.m_ItemSetDataList[current.m_SetUnique];
			for (int i = 0; i < 6; i++)
			{
				if (i <= current.m_nSetCount && sETITEM_DATA.m_nSetEffectCode[i] == (int)_type)
				{
					num += sETITEM_DATA.m_nValue[i];
				}
			}
		}
		return num;
	}

	public int GetValue(int SetItemUnique, int Count)
	{
		eSETITEM_OPTION_TYPE type = this.GetType(SetItemUnique, Count);
		if (type == eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_NONE)
		{
			return 0;
		}
		int result;
		if (type == eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_DAMAGE_P)
		{
			result = this.m_ItemSetDataList[SetItemUnique].m_nValue[Count] / 100;
		}
		else
		{
			result = this.m_ItemSetDataList[SetItemUnique].m_nValue[Count];
		}
		return result;
	}

	public string GetString(int SetItemUnique, int Count)
	{
		eSETITEM_OPTION_TYPE type = this.GetType(SetItemUnique, Count);
		string result = string.Empty;
		switch (type)
		{
		case eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_DEF:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper("1");
			break;
		case eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_MDEF:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper("2");
			break;
		case eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_ADDHP:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper("3");
			break;
		case eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_STR:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper("4");
			break;
		case eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_DEX:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper("5");
			break;
		case eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_INT:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper("7");
			break;
		case eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_VIT:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper("6");
			break;
		case eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_CRIT:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper("10");
			break;
		case eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_DAMAGE_P:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper("8");
			break;
		case eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_DAMAGE:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper("9");
			break;
		}
		return result;
	}

	private eSETITEM_OPTION_TYPE GetType(int SetItemUnique, int Count)
	{
		eSETITEM_OPTION_TYPE result = eSETITEM_OPTION_TYPE.eSETITEM_OPTION_TYPE_NONE;
		if (this.m_ItemSetDataList.ContainsKey(SetItemUnique))
		{
			SETITEM_DATA sETITEM_DATA = this.m_ItemSetDataList[SetItemUnique];
			if (Count < sETITEM_DATA.m_nSetEffectCode.Length)
			{
				result = (eSETITEM_OPTION_TYPE)sETITEM_DATA.m_nSetEffectCode[Count];
			}
		}
		return result;
	}

	public string GetSetItemName(int SetItemUnique)
	{
		string result = string.Empty;
		if (this.m_ItemSetDataList.ContainsKey(SetItemUnique))
		{
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper(this.m_ItemSetDataList[SetItemUnique].m_strTextKey);
		}
		return result;
	}
}
