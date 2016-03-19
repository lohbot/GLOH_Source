using Ndoors.Memory;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class DeclareWar_GuildListDlg : Form
{
	private Box m_bxPage;

	private NewListBox m_nlbDeclareWarList;

	private Button btPagePre;

	private Button btPageNext;

	private List<DECLAREWAR_SATE_SUB_INFO> m_declareWarList = new List<DECLAREWAR_SATE_SUB_INFO>();

	private int m_iCurPageNum = 1;

	private int m_iMaxPageNum = 1;

	private string m_strPageNum = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/dlg_DeclareWar_GuildList", G_ID.DECLAREWAR_GUILDLIST_DLG, true);
	}

	public override void SetComponent()
	{
		this.Send_GS_DECLAREWAR_GET_INFOLIST_REQ(this.m_iCurPageNum);
		this.m_bxPage = (base.GetControl("Box_Page") as Box);
		this.m_nlbDeclareWarList = (base.GetControl("NLB_DeclareWarList") as NewListBox);
		this.btPagePre = (base.GetControl("BT_Page01") as Button);
		this.btPagePre.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPagePre));
		this.btPageNext = (base.GetControl("BT_Page02") as Button);
		this.btPageNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPageNext));
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

	public override void Close()
	{
	}

	public void AddInfo(DECLAREWAR_SATE_SUB_INFO INFO)
	{
		this.m_declareWarList.Add(INFO);
	}

	public void ClaerList()
	{
		this.m_nlbDeclareWarList.Clear();
	}

	public void SetList()
	{
		this.m_nlbDeclareWarList.Clear();
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		long guildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		foreach (DECLAREWAR_SATE_SUB_INFO current in this.m_declareWarList)
		{
			NewListItem newListItem = new NewListItem(this.m_nlbDeclareWarList.ColumnNum, true);
			text3 = string.Format("{0}", current.SUB_INFO.i32Point);
			text4 = string.Format("{0}", current.SUB_INFO.i32EnemyPoint);
			newListItem.SetListItemData(0, text3, null, null, null);
			newListItem.SetListItemData(1, text4, null, null, null);
			newListItem.SetListItemData(2, string.Empty, current.SUB_INFO.i64GuildID, new EZValueChangedDelegate(this.OnClickGuildMark), null);
			newListItem.SetListItemData(3, string.Empty, current.SUB_INFO.i64EnemyGuildID, new EZValueChangedDelegate(this.OnClickGuildMark), null);
			DeclareWarGuildMarkData declareWarGuildMarkData = new DeclareWarGuildMarkData();
			declareWarGuildMarkData.i64GuildID = current.SUB_INFO.i64GuildID;
			if (current.SUB_INFO.i64EnemyGuildID == guildID)
			{
				declareWarGuildMarkData.isEnemy = true;
			}
			DeclareWarGuildMarkData declareWarGuildMarkData2 = new DeclareWarGuildMarkData();
			declareWarGuildMarkData2.i64GuildID = current.SUB_INFO.i64EnemyGuildID;
			if (current.SUB_INFO.i64GuildID == guildID)
			{
				declareWarGuildMarkData.isEnemy = true;
			}
			newListItem.SetListItemData(4, NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture(), declareWarGuildMarkData, new EZValueChangedDelegate(this.OnClickGuildMark), null);
			newListItem.SetListItemData(5, NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture(), declareWarGuildMarkData2, new EZValueChangedDelegate(this.OnClickGuildMark), null);
			text = TKString.NEWString(current.szGuildName);
			text2 = TKString.NEWString(current.szEnemyGuildName);
			newListItem.SetListItemData(6, text, null, null, null);
			newListItem.SetListItemData(7, text2, null, null, null);
			newListItem.SetListItemData(8, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2843"), null, null, null);
			this.m_nlbDeclareWarList.Add(newListItem);
		}
		this.m_nlbDeclareWarList.RepositionItems();
		this.SetGuildMark();
	}

	private void SetGuildMark()
	{
		for (int i = 0; i < this.m_nlbDeclareWarList.Count; i++)
		{
			UIListItemContainer uIListItemContainer = this.m_nlbDeclareWarList.GetItem(i) as UIListItemContainer;
			DrawTexture[] componentsInChildren = uIListItemContainer.GetComponentsInChildren<DrawTexture>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				DeclareWarGuildMarkData declareWarGuildMarkData = (DeclareWarGuildMarkData)componentsInChildren[j].data;
				if (declareWarGuildMarkData.i64GuildID != 0L)
				{
					string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(declareWarGuildMarkData.i64GuildID);
					WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebImageCallback), componentsInChildren[j]);
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
		DeclareWarGuildMarkData declareWarGuildMarkData = (DeclareWarGuildMarkData)drawTexture.data;
		if (declareWarGuildMarkData.isEnemy)
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_GUILDMARK", drawTexture, drawTexture.GetSize());
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

	public void ClickPagePre(IUIObject obj)
	{
		if (this.m_iCurPageNum <= 1)
		{
			return;
		}
		this.Send_GS_DECLAREWAR_GET_INFOLIST_REQ(this.m_iCurPageNum - 1);
	}

	public void ClickPageNext(IUIObject obj)
	{
		if (this.m_iCurPageNum >= this.m_iMaxPageNum)
		{
			return;
		}
		this.Send_GS_DECLAREWAR_GET_INFOLIST_REQ(this.m_iCurPageNum + 1);
	}

	private void Send_GS_DECLAREWAR_GET_INFOLIST_REQ(int pageIndex)
	{
		GS_DECLAREWAR_GET_INFOLIST_REQ gS_DECLAREWAR_GET_INFOLIST_REQ = new GS_DECLAREWAR_GET_INFOLIST_REQ();
		gS_DECLAREWAR_GET_INFOLIST_REQ.i16PageIndex = (short)(pageIndex - 1);
		SendPacket.GetInstance().SendObject(2325, gS_DECLAREWAR_GET_INFOLIST_REQ);
	}
}
