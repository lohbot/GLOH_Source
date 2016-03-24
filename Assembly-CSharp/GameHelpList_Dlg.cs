using GAME;
using System;
using UnityForms;

public class GameHelpList_Dlg : Form
{
	private NewListBox m_HelpList;

	private Label m_lbContext;

	private Label m_lbPage;

	private DrawTexture m_dtTexture;

	private Button m_btPrev;

	private Button m_btNext;

	private GameHelpInfo_Data m_HelpInfo;

	private int m_nCurPage = 1;

	private int m_nMaxPage = 1;

	private int m_nRealPage = -1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "System/Dlg_GameHelpList", G_ID.GAME_HELP_LIST, false, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_lbContext = (base.GetControl("Label_HelpText") as Label);
		this.m_lbPage = (base.GetControl("Label_pagecounting") as Label);
		this.m_btPrev = (base.GetControl("Button_Prev") as Button);
		this.m_btPrev.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnPrev));
		this.m_btNext = (base.GetControl("Button_Next") as Button);
		this.m_btNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnNext));
		this.m_dtTexture = (base.GetControl("DrawTexture_HelpTexture") as DrawTexture);
		this.m_HelpList = (base.GetControl("ListBox_HelpList") as NewListBox);
		this.m_HelpList.Reserve = false;
		this.m_HelpList.AutoScroll = true;
		this.m_HelpList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnListClick));
		this.SetData();
		this.m_HelpList.SetSelectedItem(0);
		base.SetScreenCenter();
	}

	public override void OnClose()
	{
		base.OnClose();
		CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
		if (costumeRoom_Dlg == null || costumeRoom_Dlg._costumeViewerSetter == null || costumeRoom_Dlg._costumeViewerSetter._costumeCharSetter == null)
		{
			return;
		}
		costumeRoom_Dlg._costumeViewerSetter._costumeCharSetter.VisibleChar(true);
	}

	public void SetViewType(string TypeName)
	{
		short selectedItem = 0;
		short listIndex = 1;
		GameHelpInfo_Data data = NrTSingleton<NrGameHelpInfoManager>.Instance.GetData(TypeName);
		if (data != null && data.m_byListOnOff == 0)
		{
			for (short num = 0; num < NrTSingleton<NrGameHelpInfoManager>.Instance.GetCount(); num += 1)
			{
				if (!(this.m_HelpList.GetItem((int)num) == null))
				{
					if ((short)this.m_HelpList.GetItem((int)num).Data == data.m_nSort)
					{
						selectedItem = num;
						listIndex = data.m_nSort;
						break;
					}
				}
			}
		}
		this.m_HelpList.SetSelectedItem((int)selectedItem);
		this.SelectList(listIndex);
	}

	public void SetData()
	{
		for (short num = 0; num < NrTSingleton<NrGameHelpInfoManager>.Instance.GetCount(); num += 1)
		{
			GameHelpInfo_Data data = NrTSingleton<NrGameHelpInfoManager>.Instance.GetData(num + 1);
			if (data != null && data.m_byListOnOff == 0)
			{
				if (NrTSingleton<ContentsLimitManager>.Instance.IsItemLevelCheckBlock() || !(data.m_HelpListName == eHELP_LIST.Gear_Grinding.ToString()))
				{
					if (!NrTSingleton<ContentsLimitManager>.Instance.IsItemNormalSkillBlock() || !(data.m_HelpListName == eHELP_LIST.Gear_Carving.ToString()))
					{
						NewListItem newListItem = new NewListItem(1, true, string.Empty);
						newListItem.SetListItemData(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(data.m_strTitle), null, null, null);
						newListItem.Data = data.m_nSort;
						this.m_HelpList.Add(newListItem);
					}
				}
			}
		}
		this.m_HelpList.RepositionItems();
	}

	public void OnListClick(IUIObject obj)
	{
		short listIndex = (short)this.m_HelpList.SelectedItem.Data;
		this.SelectList(listIndex);
	}

	public void OnNext(IUIObject obj)
	{
		if (this.m_nMaxPage > this.m_nCurPage)
		{
			this.SetContext(true);
			this.m_nCurPage++;
			this.m_lbPage.SetText(string.Format("{0} / {1}", this.m_nCurPage, this.m_nMaxPage));
		}
	}

	public void OnPrev(IUIObject obj)
	{
		if (1 < this.m_nCurPage)
		{
			this.m_nCurPage--;
			this.SetContext(false);
			this.m_lbPage.SetText(string.Format("{0} / {1}", this.m_nCurPage, this.m_nMaxPage));
		}
	}

	private void SelectList(short ListIndex)
	{
		GameHelpInfo_Data data = NrTSingleton<NrGameHelpInfoManager>.Instance.GetData(ListIndex);
		int num = 0;
		if (data != null)
		{
			this.m_nRealPage = -1;
			this.m_HelpInfo = data;
			this.SetContext(true);
			for (int i = 0; i < 10; i++)
			{
				if (data.m_byPageOnOff[i] != 1)
				{
					if (!string.IsNullOrEmpty(data.m_strText[i]) && !data.m_strText[i].Equals("0"))
					{
						if (i == 9)
						{
							break;
						}
						num++;
					}
				}
			}
			this.m_nCurPage = 1;
			this.m_nMaxPage = num;
			this.m_lbPage.SetText(string.Format("{0} / {1}", this.m_nCurPage, this.m_nMaxPage));
		}
	}

	private void SetContext(bool Next)
	{
		if (Next)
		{
			this.m_nRealPage++;
		}
		else
		{
			this.m_nRealPage--;
		}
		bool flag = false;
		while (!flag)
		{
			if (this.m_nRealPage < 0 && this.m_nRealPage >= 10)
			{
				return;
			}
			if (this.m_HelpInfo.m_byPageOnOff[this.m_nRealPage] == 1)
			{
				if (Next)
				{
					this.m_nRealPage++;
				}
				else
				{
					this.m_nRealPage--;
				}
			}
			else
			{
				flag = true;
			}
		}
		if (this.m_nRealPage >= 0 && this.m_nRealPage < 10)
		{
			this.m_lbContext.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromhelper(this.m_HelpInfo.m_strText[this.m_nRealPage]));
			if (this.m_HelpInfo.m_strTexture[this.m_nRealPage].Equals("0"))
			{
				this.m_dtTexture.SetTexture(string.Empty);
			}
			else
			{
				this.m_dtTexture.SetTextureFromBundle(string.Format("UI/Help/{0}", this.m_HelpInfo.m_strTexture[this.m_nRealPage]));
			}
		}
	}
}
