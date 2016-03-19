using Ndoors.Memory;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class MineWaitMiltaryInfoDlg : Form
{
	private const int MAX_MINE_WAIT_GUILDCOUNT = 10;

	public Label m_lTitle;

	public NewListBox m_nlbWaitGuildInfo;

	public int m_nTotalCount;

	public NewListItem item;

	public List<MINE_WAIT_MILITARY_INFO> m_TempGuildInfo = new List<MINE_WAIT_MILITARY_INFO>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Mine/dlg_mine_battlewait", G_ID.MINE_WAITMILTARYINFO_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_lTitle = (base.GetControl("Label_title") as Label);
		this.m_nlbWaitGuildInfo = (base.GetControl("Newlistbox_mine_battlewait") as NewListBox);
		this.m_nlbWaitGuildInfo.touchScroll = true;
		this.m_nlbWaitGuildInfo.Clear();
		base.SetScreenCenter();
	}

	public override void OnClose()
	{
		if (this.m_TempGuildInfo != null)
		{
			this.m_TempGuildInfo.Clear();
		}
		base.OnClose();
	}

	public void SetWaitGuildInfo(string Guildname, long GuildId)
	{
		MINE_WAIT_MILITARY_INFO mINE_WAIT_MILITARY_INFO = new MINE_WAIT_MILITARY_INFO();
		mINE_WAIT_MILITARY_INFO.nGuildID = GuildId;
		mINE_WAIT_MILITARY_INFO.strGuildName = Guildname;
		if (GuildId <= 0L)
		{
			return;
		}
		if (!this.m_TempGuildInfo.Contains(mINE_WAIT_MILITARY_INFO))
		{
			this.m_TempGuildInfo.Add(mINE_WAIT_MILITARY_INFO);
		}
	}

	public void GuildImageSetting()
	{
		this.m_nlbWaitGuildInfo.Clear();
		long guildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		this.m_nTotalCount = 0;
		for (int i = 0; i < this.m_TempGuildInfo.Count; i++)
		{
			NewListItem newListItem = new NewListItem(this.m_nlbWaitGuildInfo.ColumnNum, true);
			if (this.m_TempGuildInfo[i].nGuildID == guildID)
			{
				newListItem.SetListItemData(0, NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + this.m_TempGuildInfo[i].strGuildName, null, null, null);
			}
			else
			{
				newListItem.SetListItemData(0, this.m_TempGuildInfo[i].strGuildName, null, null, null);
			}
			newListItem.SetListItemData(1, NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture(), true, null, null);
			this.m_nlbWaitGuildInfo.Add(newListItem);
		}
		this.m_nlbWaitGuildInfo.RepositionItems();
		for (int j = 0; j < this.m_TempGuildInfo.Count; j++)
		{
			string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(this.m_TempGuildInfo[j].nGuildID);
			WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebImageCallback), j);
		}
	}

	private void ReqWebImageCallback(Texture2D txtr, object _param)
	{
		long guildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		int num = (int)_param;
		if (num < 0)
		{
			return;
		}
		if (txtr != null)
		{
			IUIListObject iUIListObject = this.m_nlbWaitGuildInfo.GetItem(num);
			if (iUIListObject != null)
			{
				NewListItem newListItem = new NewListItem(this.m_nlbWaitGuildInfo.ColumnNum, true);
				if (this.m_TempGuildInfo[num].nGuildID == guildID)
				{
					newListItem.SetListItemData(0, NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + this.m_TempGuildInfo[num].strGuildName, null, null, null);
				}
				else
				{
					newListItem.SetListItemData(0, this.m_TempGuildInfo[num].strGuildName, null, null, null);
				}
				newListItem.SetListItemData(1, txtr, true, null, null, null);
				this.m_nlbWaitGuildInfo.RemoveAdd(num, newListItem);
			}
		}
		this.m_nTotalCount++;
		if (this.m_nTotalCount == this.m_TempGuildInfo.Count)
		{
			this.m_nlbWaitGuildInfo.RepositionItems();
		}
	}
}
