using StageHelper;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NrTextMgr : NrTSingleton<NrTextMgr>
{
	public enum eTEXTMGR
	{
		eTEXTMGR_NONE = -1,
		eTEXTMGR_MIN,
		eTEXTMGR_PRELOADINGTEXT = 0,
		eTEXTMGR_START,
		eTEXTMGR_MAP = 1,
		eTEXTMGR_ITEM,
		eTEXTMGR_ITEMHELPER,
		eTEXTMGR_MESSAGEBOX,
		eTEXTMGR_INTERFACE,
		eTEXTMGR_CHARINFO,
		eTEXTMGR_NOTIFY,
		eTEXTMGR_TOOLTIP,
		eTEXTMGR_QUEST_CODE,
		eTEXTMGR_TBS,
		eTEXTMGR_GAMEMASTER,
		eTEXTMGR_GAMEDRAMA,
		eTEXTMGR_QUEST_DIALOG,
		eTEXTMGR_QUEST_TITLE,
		eTEXTMGR_HELPER,
		eTEXTMGR_ECO_TALK,
		eTEXTMGR_LOG,
		eTEXTMGR_MINIDRAMA,
		eTEXTMGR_FACEBOOK,
		eTEXTMGR_CHALLENGE,
		eTEXTMGR_PUSH,
		eTEXTMGR_CAPTION,
		eTEXTMGR_OST,
		eTEXTMGR_MAX
	}

	public TsTextGroup[] m_tsTextMgr;

	private string[] m_strGroupText;

	private Dictionary<string, NrTextMgr.eTEXTMGR> m_dicGroupEnum;

	private NrTextMgr()
	{
		int num = 24;
		this._SetTextGroup();
		this.m_dicGroupEnum = new Dictionary<string, NrTextMgr.eTEXTMGR>();
		this.m_tsTextMgr = new TsTextGroup[num];
		for (int i = 0; i < num; i++)
		{
			this.m_tsTextMgr[i] = new TsTextGroup(this.m_strGroupText[i]);
			this.m_dicGroupEnum.Add(this.m_strGroupText[i], (NrTextMgr.eTEXTMGR)i);
		}
	}

	private void _SetTextGroup()
	{
		int num = 24;
		int num2 = 0;
		this.m_strGroupText = new string[num];
		this.m_strGroupText[num2++] = "Preloading";
		this.m_strGroupText[num2++] = "Map";
		this.m_strGroupText[num2++] = "Item";
		this.m_strGroupText[num2++] = "ItemHelper";
		this.m_strGroupText[num2++] = "MessageBox";
		this.m_strGroupText[num2++] = "Interface";
		this.m_strGroupText[num2++] = "CharInfo";
		this.m_strGroupText[num2++] = "Notify";
		this.m_strGroupText[num2++] = "ToolTip";
		this.m_strGroupText[num2++] = "Quest_Code";
		this.m_strGroupText[num2++] = "TBS";
		this.m_strGroupText[num2++] = "GameMaster";
		this.m_strGroupText[num2++] = "GameDrama";
		this.m_strGroupText[num2++] = "Quest_Dialog";
		this.m_strGroupText[num2++] = "Quest_Title";
		this.m_strGroupText[num2++] = "helper";
		this.m_strGroupText[num2++] = "Eco_Talk";
		this.m_strGroupText[num2++] = "Log";
		this.m_strGroupText[num2++] = "minidrama";
		this.m_strGroupText[num2++] = "facebook";
		this.m_strGroupText[num2++] = "Challenge";
		this.m_strGroupText[num2++] = "Push";
		this.m_strGroupText[num2++] = "Caption";
		this.m_strGroupText[num2++] = "Ost";
	}

	public bool Initialize()
	{
		return true;
	}

	public bool Register(ref TableInspector tableInspector, NrTableBase tbl)
	{
		try
		{
			int num = 24;
			for (int i = 1; i < num; i++)
			{
				string strFilePath = string.Format("textmanager/text_{0}", this.m_strGroupText[i]);
				NrTableBase tbl2 = new NrTextTable_ForTsTxtMgr(this.m_strGroupText[i], (NrTextMgr.eTEXTMGR)i, strFilePath);
				tableInspector.RegistWait(tbl, tbl2);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return true;
	}

	public List<NrTableBase> RequestTables()
	{
		List<NrTableBase> list = new List<NrTableBase>();
		try
		{
			int num = 24;
			for (int i = 1; i < num; i++)
			{
				if (i != 13)
				{
					string strFilePath = string.Format("textmanager/text_{0}", this.m_strGroupText[i]);
					NrTableBase item = new NrTextTable_ForTsTxtMgr(this.m_strGroupText[i], (NrTextMgr.eTEXTMGR)i, strFilePath);
					list.Add(item);
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return list;
	}

	public string GetGroupTextKey(NrTextMgr.eTEXTMGR eTextMgr)
	{
		return this.m_strGroupText[(int)eTextMgr];
	}

	private NrTextMgr.eTEXTMGR _GetEnumFromGroupKey(string strGroupKey)
	{
		NrTextMgr.eTEXTMGR result = NrTextMgr.eTEXTMGR.eTEXTMGR_NONE;
		if (!this.m_dicGroupEnum.TryGetValue(strGroupKey, out result))
		{
			return NrTextMgr.eTEXTMGR.eTEXTMGR_NONE;
		}
		return result;
	}

	public TsTextGroup GetTextGroup(NrTextMgr.eTEXTMGR eTextMgr)
	{
		return this.m_tsTextMgr[(int)eTextMgr];
	}

	public bool ClearTextGroup(NrTextMgr.eTEXTMGR eTextMgr)
	{
		this.m_tsTextMgr[(int)eTextMgr].Clear();
		return true;
	}

	public string GetTextFrom(NrTextMgr.eTEXTMGR eTextMgr, string strTextKey)
	{
		return this.m_tsTextMgr[(int)eTextMgr].GetText(strTextKey);
	}

	public string GetTextFrom(string strGroupKey, string strTextkey)
	{
		NrTextMgr.eTEXTMGR eTEXTMGR = this._GetEnumFromGroupKey(strGroupKey);
		if (eTEXTMGR == NrTextMgr.eTEXTMGR.eTEXTMGR_NONE)
		{
			return string.Empty;
		}
		return this.GetTextFrom(eTEXTMGR, strTextkey);
	}

	public string GetTextFromMap(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_START, strTextKey);
	}

	public string GetTextFromItem(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_ITEM, strTextKey);
	}

	public string GetTextFromItemHelper(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_ITEMHELPER, strTextKey);
	}

	public string GetTextFromMessageBox(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_MESSAGEBOX, strTextKey);
	}

	public string GetTextFromInterface(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_INTERFACE, strTextKey);
	}

	public string GetTextFromCharInfo(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_CHARINFO, strTextKey);
	}

	public string GetTextFromNotify(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_NOTIFY, strTextKey);
	}

	public string GetTextFromToolTip(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_TOOLTIP, strTextKey);
	}

	public string GetTextFromQuest_Code(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_QUEST_CODE, strTextKey);
	}

	public string GetTextFromTBS(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_TBS, strTextKey);
	}

	public string GetTextFromGameMaster(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_GAMEMASTER, strTextKey);
	}

	public string GetTextFromGameDrama(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_GAMEDRAMA, strTextKey);
	}

	public string GetTextFromQuest_Dialog(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_QUEST_DIALOG, strTextKey);
	}

	public string GetTextFromQuest_Title(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_QUEST_TITLE, strTextKey);
	}

	public string GetTextFromhelper(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_HELPER, strTextKey);
	}

	public string GetTextFromEco_Talk(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_ECO_TALK, strTextKey);
	}

	public string GetTextFromLog(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_LOG, strTextKey);
	}

	public string GetTextFromPreloadText(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_MIN, strTextKey);
	}

	public string GetTextFromMINIDRAMA(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_MINIDRAMA, strTextKey);
	}

	public Dictionary<string, string> GetTextGroupFromMINIDRAMA()
	{
		return this.m_tsTextMgr[18].Dic;
	}

	public string GetTextFromFacebook(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_FACEBOOK, strTextKey);
	}

	public string GetTextFromChallenge(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_CHALLENGE, strTextKey);
	}

	public string GetTextFromPush(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_PUSH, strTextKey);
	}

	public string GetTextFromCaption(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_CAPTION, strTextKey);
	}

	public string GetTextFromOST(string strTextKey)
	{
		return this.GetTextFrom(NrTextMgr.eTEXTMGR.eTEXTMGR_OST, strTextKey);
	}
}
