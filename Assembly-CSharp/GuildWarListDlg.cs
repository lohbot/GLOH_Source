using GAME;
using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class GuildWarListDlg : Form
{
	private Box m_bxPage;

	private NewListBox m_nlbWarList;

	private Button btPagePre;

	private Button btPageNext;

	private Label lbhelp;

	private List<GUILDWAR_MATCH_INFO> m_GuildWarList = new List<GUILDWAR_MATCH_INFO>();

	private int m_iCurPageNum = 1;

	private int m_iMaxPageNum = 1;

	private string m_strPageNum = string.Empty;

	private eGUILDWAR_TIME_STATE m_TimeState;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/dlg_GuildWar_GuildList", G_ID.GUILDWAR_LIST_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.Send_GS_GUILDWAR_MATCH_LIST_REQ(this.m_iCurPageNum);
		this.m_bxPage = (base.GetControl("Box_Page") as Box);
		this.m_nlbWarList = (base.GetControl("NLB_GuildWarList") as NewListBox);
		this.m_nlbWarList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickWarList));
		this.btPagePre = (base.GetControl("BT_Page01") as Button);
		this.btPagePre.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickPagePre));
		this.btPageNext = (base.GetControl("BT_Page02") as Button);
		this.btPageNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickPageNext));
		this.lbhelp = (base.GetControl("LB_Refresh") as Label);
		this.m_nlbWarList.Reserve = false;
		this.SetPageText(1, 1);
	}

	private void OnGuildMarkEffect(WWWItem _item, object _param)
	{
	}

	public void SetPageText(int curPage, int maxPage)
	{
		this.m_iCurPageNum = curPage;
		this.m_iMaxPageNum = maxPage;
		this.m_strPageNum = string.Format("{0}/{1}", curPage.ToString(), maxPage.ToString());
		this.m_bxPage.SetText(this.m_strPageNum);
	}

	public void AddInfo(GUILDWAR_MATCH_INFO INFO)
	{
		this.m_GuildWarList.Add(INFO);
	}

	public void ClaerList()
	{
		this.m_nlbWarList.Clear();
		this.m_GuildWarList.Clear();
	}

	public void SetList()
	{
		this.m_nlbWarList.Clear();
		foreach (GUILDWAR_MATCH_INFO current in this.m_GuildWarList)
		{
			NewListItem newListItem = new NewListItem(this.m_nlbWarList.ColumnNum, true, string.Empty);
			newListItem.Data = current;
			newListItem.SetListItemData(0, current.SUB_INFO.i32Point[0].ToString(), null, null, null);
			newListItem.SetListItemData(1, current.SUB_INFO.i32Point[1].ToString(), null, null, null);
			newListItem.SetListItemData(2, string.Empty, current.SUB_INFO.i64GuildID[0], new EZValueChangedDelegate(this.OnClickGuildMark), null);
			newListItem.SetListItemData(3, string.Empty, current.SUB_INFO.i64GuildID[1], new EZValueChangedDelegate(this.OnClickGuildMark), null);
			newListItem.SetListItemData(5, NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture(), current.SUB_INFO.i64GuildID[0], null, null);
			newListItem.SetListItemData(4, NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture(), current.SUB_INFO.i64GuildID[1], null, null);
			newListItem.SetListItemData(6, TKString.NEWString(current.szGuildName_1), null, null, null);
			newListItem.SetListItemData(7, TKString.NEWString(current.szGuildName_2), null, null, null);
			newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2843"), null, null, null);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("599"),
				"guildrank",
				current.SUB_INFO.i32GuildRank[0].ToString()
			});
			newListItem.SetListItemData(9, empty, null, null, null);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("599"),
				"guildrank",
				current.SUB_INFO.i32GuildRank[1].ToString()
			});
			newListItem.SetListItemData(10, empty, null, null, null);
			newListItem.SetListItemData(11, false);
			newListItem.SetListItemData(12, false);
			if (this.m_TimeState == eGUILDWAR_TIME_STATE.eGUILDWAR_TIME_STATE_WAREND && current.SUB_INFO.i64GuildID[0] > 0L)
			{
				if (current.SUB_INFO.i32Point[0] > current.SUB_INFO.i32Point[1])
				{
					newListItem.SetListItemData(11, true);
				}
				else if (current.SUB_INFO.i32Point[0] < current.SUB_INFO.i32Point[1])
				{
					newListItem.SetListItemData(12, true);
				}
				else if (current.SUB_INFO.i32Point[0] == current.SUB_INFO.i32Point[1])
				{
					if (current.SUB_INFO.i32GuildRank[0] < current.SUB_INFO.i32GuildRank[1])
					{
						newListItem.SetListItemData(12, true);
					}
					else
					{
						newListItem.SetListItemData(11, true);
					}
				}
			}
			this.m_nlbWarList.Add(newListItem);
		}
		this.m_nlbWarList.RepositionItems();
		this.SetGuildMark();
		this.SelectListItem_MyWarInfo();
	}

	private void SetGuildMark()
	{
		for (int i = 0; i < this.m_nlbWarList.Count; i++)
		{
			UIListItemContainer item = this.m_nlbWarList.GetItem(i);
			if (!(item == null))
			{
				GUILDWAR_MATCH_INFO gUILDWAR_MATCH_INFO = item.Data as GUILDWAR_MATCH_INFO;
				if (gUILDWAR_MATCH_INFO != null)
				{
					UIButton[] componentsInChildren = item.GetComponentsInChildren<UIButton>();
					if (gUILDWAR_MATCH_INFO.SUB_INFO.i64GuildID[0] == NrTSingleton<NewGuildManager>.Instance.GetGuildID() || gUILDWAR_MATCH_INFO.SUB_INFO.i64GuildID[1] == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
					{
						NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_GUILDMARK", componentsInChildren[0], componentsInChildren[0].GetSize());
					}
				}
				DrawTexture[] componentsInChildren2 = item.GetComponentsInChildren<DrawTexture>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					if (componentsInChildren2[j].data != null)
					{
						long num = (long)componentsInChildren2[j].data;
						if (num > 0L)
						{
							string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(num);
							WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebImageCallback), componentsInChildren2[j]);
						}
					}
				}
			}
		}
	}

	private void ReqWebImageCallback(Texture2D txtr, object _param)
	{
		DrawTexture drawTexture = (DrawTexture)_param;
		if (txtr == null)
		{
			drawTexture.SetTexture(NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture());
		}
		else
		{
			drawTexture.SetTexture(txtr);
		}
	}

	public void OnClickList(IUIObject obj)
	{
	}

	public void OnClickGuildMark(IUIObject obj)
	{
		long num = (long)obj.Data;
		if (num == 0L)
		{
			return;
		}
		GS_NEWGUILD_DETAILINFO_REQ gS_NEWGUILD_DETAILINFO_REQ = new GS_NEWGUILD_DETAILINFO_REQ();
		gS_NEWGUILD_DETAILINFO_REQ.i64GuildID = num;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_DETAILINFO_REQ, gS_NEWGUILD_DETAILINFO_REQ);
	}

	public void OnClickPagePre(IUIObject obj)
	{
		if (this.m_iCurPageNum <= 1)
		{
			return;
		}
		this.Send_GS_GUILDWAR_MATCH_LIST_REQ(this.m_iCurPageNum - 1);
	}

	public void OnClickPageNext(IUIObject obj)
	{
		if (this.m_iCurPageNum >= this.m_iMaxPageNum)
		{
			return;
		}
		this.Send_GS_GUILDWAR_MATCH_LIST_REQ(this.m_iCurPageNum + 1);
	}

	public void OnClickWarList(IUIObject obj)
	{
		if (null == this.m_nlbWarList || null == this.m_nlbWarList.SelectedItem)
		{
			return;
		}
		GUILDWAR_MATCH_INFO gUILDWAR_MATCH_INFO = this.m_nlbWarList.SelectedItem.Data as GUILDWAR_MATCH_INFO;
		if (gUILDWAR_MATCH_INFO == null)
		{
			return;
		}
		if (gUILDWAR_MATCH_INFO.SUB_INFO.i64GuildID[0] == NrTSingleton<NewGuildManager>.Instance.GetGuildID() || gUILDWAR_MATCH_INFO.SUB_INFO.i64GuildID[1] == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			NrTSingleton<MineManager>.Instance.Send_GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_REQ(1, 1, 0L);
		}
		this.SelectListItem_MyWarInfo();
	}

	public void SelectListItem_MyWarInfo()
	{
		if (null == this.m_nlbWarList)
		{
			return;
		}
		for (int i = 0; i < this.m_nlbWarList.Count; i++)
		{
			UIListItemContainer item = this.m_nlbWarList.GetItem(i);
			GUILDWAR_MATCH_INFO gUILDWAR_MATCH_INFO = item.Data as GUILDWAR_MATCH_INFO;
			if (gUILDWAR_MATCH_INFO == null)
			{
				return;
			}
			if (gUILDWAR_MATCH_INFO.SUB_INFO.i64GuildID[0] == NrTSingleton<NewGuildManager>.Instance.GetGuildID() || gUILDWAR_MATCH_INFO.SUB_INFO.i64GuildID[1] == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				this.m_nlbWarList.SetSelectedItem(i);
				break;
			}
		}
	}

	private void Send_GS_GUILDWAR_MATCH_LIST_REQ(int pageIndex)
	{
		GS_GUILDWAR_MATCH_LIST_REQ gS_GUILDWAR_MATCH_LIST_REQ = new GS_GUILDWAR_MATCH_LIST_REQ();
		gS_GUILDWAR_MATCH_LIST_REQ.i16CurPage = (short)pageIndex;
		SendPacket.GetInstance().SendObject(2200, gS_GUILDWAR_MATCH_LIST_REQ);
	}

	public void SetTimeSate(byte i8TimeState)
	{
		this.m_TimeState = (eGUILDWAR_TIME_STATE)i8TimeState;
		string text = string.Empty;
		switch (i8TimeState)
		{
		case 1:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2842");
			break;
		case 2:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2940");
			break;
		case 3:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2941");
			break;
		default:
			return;
		}
		this.lbhelp.SetText(text);
	}
}
