using System;
using TsLibs;
using UnityEngine;

public class NrTextTable_ForTsTxtMgr : NrTableBase
{
	private bool m_IsQuestDlg;

	private string m_strGroupKey;

	private TsTextGroup m_tsTextGroup;

	private NrTextMgr.eTEXTMGR m_eTextMgr;

	public NrTextTable_ForTsTxtMgr(string strGroupKey, NrTextMgr.eTEXTMGR eTextMgr, string strFilePath) : base(strFilePath, true)
	{
		this.m_strGroupKey = strGroupKey;
		this.m_eTextMgr = eTextMgr;
		this.m_IsQuestDlg = (eTextMgr == NrTextMgr.eTEXTMGR.eTEXTMGR_QUEST_DIALOG);
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		this.m_tsTextGroup = NrTSingleton<NrTextMgr>.Instance.GetTextGroup(this.m_eTextMgr);
		if (this.m_tsTextGroup == null)
		{
			Debug.LogError(string.Format("Error - NrTextTable_ForTsTxtMgr! No Regist Group({0}) - {1}", this.m_strGroupKey, this.m_strFilePath));
		}
		int num = 0;
		foreach (TsDataReader.Row row in dr)
		{
			bool flag = this.SetText(row.GetColumn(1), row.GetColumn(2, true));
			if (flag)
			{
				num++;
				if (this.m_IsQuestDlg)
				{
					this.Set_Quest_Dialog_Info(row.GetColumn(1), row.GetColumn(2, true));
				}
			}
		}
		return true;
	}

	public override void ParseRowData(TsDataReader.Row tsRow)
	{
		this.m_tsTextGroup = NrTSingleton<NrTextMgr>.Instance.GetTextGroup(this.m_eTextMgr);
		if (this.m_tsTextGroup == null)
		{
			TsLog.LogError(string.Format("Error - NrTextTable_ForTsTxtMgr! No Regist Group({0}) - {1}", this.m_strGroupKey, this.m_strFilePath), new object[0]);
		}
		TEXT tEXT = new TEXT();
		tEXT.SetData(tsRow);
		if (!this.SetText(tEXT.TEXTKEY, tEXT.DATA))
		{
			TsLog.LogError("Error! Parsing - " + this.m_strFilePath, new object[0]);
		}
		if (this.m_IsQuestDlg)
		{
			this.Set_Quest_Dialog_Info(tEXT.TEXTKEY, tEXT.DATA);
		}
	}

	private bool SetText(string strTextKey, string strText)
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParamColor(out empty, new string[]
		{
			strText
		});
		return this.m_tsTextGroup.SetText(strTextKey, empty);
	}

	private bool Set_Quest_Dialog_Info(string strTextKey, string strText)
	{
		QUEST_DLG_INFO qUEST_DLG_INFO = new QUEST_DLG_INFO();
		NrTSingleton<CTextParser>.Instance.ParsingQuestDlg(ref qUEST_DLG_INFO, strTextKey + strText);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParamColor(out empty, new string[]
		{
			qUEST_DLG_INFO.strLang_Idx
		});
		qUEST_DLG_INFO.strLang_Idx = empty;
		NrTSingleton<NkQuestManager>.Instance.AddQuestDlg(qUEST_DLG_INFO);
		QUEST_NPC_POS_INFO qUEST_NPC_POS_INFO = NrTSingleton<NrBaseTableManager>.Instance.GetQuestNPCPosInfo(qUEST_DLG_INFO.strDialogUnique);
		if (qUEST_NPC_POS_INFO == null)
		{
			qUEST_NPC_POS_INFO = new QUEST_NPC_POS_INFO();
			qUEST_NPC_POS_INFO.strUnique = qUEST_DLG_INFO.strDialogUnique;
			qUEST_NPC_POS_INFO.CHAR_CODE[0] = qUEST_DLG_INFO.QuestDlgCharCode;
		}
		else
		{
			bool flag = false;
			for (byte b = 0; b < 5; b += 1)
			{
				if (qUEST_NPC_POS_INFO.CHAR_CODE[(int)b] == qUEST_DLG_INFO.QuestDlgCharCode)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (byte b2 = 0; b2 < 5; b2 += 1)
				{
					if (qUEST_NPC_POS_INFO.CHAR_CODE[(int)b2] == string.Empty)
					{
						qUEST_NPC_POS_INFO.CHAR_CODE[(int)b2] = qUEST_DLG_INFO.QuestDlgCharCode;
						break;
					}
				}
			}
		}
		return NrTSingleton<NrBaseTableManager>.Instance.SetData(qUEST_NPC_POS_INFO);
	}
}
