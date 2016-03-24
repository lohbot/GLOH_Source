using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityForms;

public class Agit_GoldRecordDlg : Form
{
	private NewListBox m_nlbDonationList;

	private Button m_btClose;

	private List<NEWGUILD_FUND_USE_INFO> m_FundUseInfo = new List<NEWGUILD_FUND_USE_INFO>();

	private string m_strText = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/Agit/DLG_Agit_GoldRecord", G_ID.AGIT_GOLDRECORD_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_nlbDonationList = (base.GetControl("NLB_goldrecord") as NewListBox);
		this.m_btClose = (base.GetControl("Button_close") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickClose));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClearFundUseInfo()
	{
		this.m_FundUseInfo.Clear();
	}

	public void AddFundUseInfo(NEWGUILD_FUND_USE_INFO Data)
	{
		this.m_FundUseInfo.Add(Data);
	}

	public void RefreshInfo()
	{
		this.m_nlbDonationList.Clear();
		for (int i = 0; i < this.m_FundUseInfo.Count; i++)
		{
			this.MakeFundUseInfoItem(this.m_FundUseInfo[i]);
		}
		this.m_nlbDonationList.RepositionItems();
	}

	public void MakeFundUseInfoItem(NEWGUILD_FUND_USE_INFO Data)
	{
		if (Data == null)
		{
			return;
		}
		NewListItem newListItem = new NewListItem(this.m_nlbDonationList.ColumnNum, true, string.Empty);
		DateTime dueDate = PublicMethod.GetDueDate(Data.i64UseTime);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("602"),
			"year",
			dueDate.Year,
			"month",
			dueDate.Month,
			"day",
			dueDate.Day
		});
		newListItem.SetListItemData(0, this.m_strText, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1527"),
			"hour",
			dueDate.Hour,
			"min",
			dueDate.Minute,
			"sec",
			dueDate.Second
		});
		newListItem.SetListItemData(1, this.m_strText, null, null, null);
		newListItem.SetListItemData(2, NrTSingleton<NewGuildManager>.Instance.GetStringFromFundUseType((NewGuildDefine.eNEWGUILD_FUND_USE_TYPE)Data.ui8UseType), null, null, null);
		newListItem.SetListItemData(3, ANNUALIZED.Convert(Data.i64UseFund), null, null, null);
		newListItem.SetListItemData(4, TKString.NEWString(Data.szUseCharName), null, null, null);
		this.m_nlbDonationList.Add(newListItem);
	}

	public void ClickClose(IUIObject obj)
	{
		this.Close();
	}
}
