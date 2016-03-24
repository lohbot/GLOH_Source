using GAME;
using System;
using UnityForms;

public class Event_Detail_Dlg : Form
{
	private Label m_lbTitle;

	private Button m_btButton;

	private Label m_lbTime;

	private Label m_lbEventDetail;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "dlg_eventdetail", G_ID.EVENT_MAIN_EXPLAIN, false, true);
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("LB_Title") as Label);
		this.m_btButton = (base.GetControl("Button_Button12") as Button);
		this.m_btButton.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10");
		Button expr_4C = this.m_btButton;
		expr_4C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4C.Click, new EZValueChangedDelegate(this.BtnClick01));
		this.m_lbTime = (base.GetControl("Label_time2") as Label);
		this.m_lbEventDetail = (base.GetControl("Label_detail2") as Label);
		base.SetScreenCenter();
	}

	private void BtnClick01(IUIObject obj)
	{
		this.Close();
	}

	public void SetSelectInfoItem(int nEventType, bool bCurrentEvent)
	{
		string text = string.Empty;
		if (bCurrentEvent)
		{
			EVENT_INFO eventInfoFromType = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventInfoFromType(nEventType);
			if (eventInfoFromType == null)
			{
				return;
			}
			BUNNING_EVENT_INFO value = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)nEventType);
			if (value == null)
			{
				return;
			}
			int num = eventInfoFromType.m_nStartTime + eventInfoFromType.m_nEndDurationTime / 60;
			if (num >= 0 && num > 24)
			{
				num -= 24;
			}
			string text2 = this.EventWeek((int)eventInfoFromType.m_nEventInfoWeek);
			if (nEventType == 15)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2550");
			}
			else if (nEventType == 16)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2560");
			}
			else if (nEventType == 17)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2561");
			}
			else if (value.m_nYear > 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2307"),
					"month",
					value.m_nMonth.ToString(),
					"day",
					value.m_nDay.ToString(),
					"starttime",
					eventInfoFromType.m_nStartTime.ToString(),
					"endmonth",
					value.m_nEndMonth.ToString(),
					"endday",
					value.m_nEndDay.ToString(),
					"endtime",
					num.ToString()
				});
			}
			else if (eventInfoFromType.m_nEventType == 4)
			{
				text2 = this.EventWeek(-1);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
					"day",
					text2,
					"starttime",
					eventInfoFromType.m_nStartTime.ToString(),
					"endtime",
					num.ToString()
				});
			}
			else if (eventInfoFromType.m_nEventType == 19 || eventInfoFromType.m_nEventType == 20 || eventInfoFromType.m_nEventType == 21 || eventInfoFromType.m_nEventType == 22 || eventInfoFromType.m_nEventType == 23 || eventInfoFromType.m_nEventType == 24)
			{
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2543");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
					"day",
					textFromInterface,
					"starttime",
					eventInfoFromType.m_nStartTime.ToString(),
					"endtime",
					num.ToString()
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
					"day",
					text2,
					"starttime",
					eventInfoFromType.m_nStartTime.ToString(),
					"endtime",
					num.ToString()
				});
			}
			this.m_lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(eventInfoFromType.m_nTitleText.ToString()));
			this.m_lbEventDetail.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(eventInfoFromType.m_nExplain.ToString()));
		}
		else
		{
			BUNNING_EVENT_REFLASH_INFO bUNNING_EVENT_REFLASH_INFO = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.Get_Value((eBUNNING_EVENT)nEventType);
			if (bUNNING_EVENT_REFLASH_INFO == null)
			{
				return;
			}
			BUNNING_EVENT_INFO value = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)nEventType);
			if (value == null)
			{
				return;
			}
			int num2 = bUNNING_EVENT_REFLASH_INFO.m_nStartTime + bUNNING_EVENT_REFLASH_INFO.m_nEndTime / 60;
			if (num2 >= 0 && num2 > 24)
			{
				num2 -= 24;
			}
			string text3 = this.EventWeek((int)value.m_nWeek);
			if (nEventType == 15)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2550");
			}
			else if (nEventType == 16)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2560");
			}
			else if (nEventType == 17)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2561");
			}
			else if (value.m_nYear > 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text3, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2307"),
					"month",
					value.m_nMonth.ToString(),
					"day",
					value.m_nDay.ToString(),
					"starttime",
					bUNNING_EVENT_REFLASH_INFO.m_nStartTime.ToString(),
					"endmonth",
					value.m_nEndMonth.ToString(),
					"endday",
					value.m_nEndDay.ToString(),
					"endtime",
					num2.ToString()
				});
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
					"day",
					text3,
					"starttime",
					bUNNING_EVENT_REFLASH_INFO.m_nStartTime.ToString(),
					"endtime",
					num2.ToString()
				});
			}
			else if (bUNNING_EVENT_REFLASH_INFO.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_BUFFDAMAGE)
			{
				text3 = this.EventWeek(-1);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
					"day",
					text3,
					"starttime",
					bUNNING_EVENT_REFLASH_INFO.m_nStartTime.ToString(),
					"endtime",
					num2.ToString()
				});
			}
			else if (bUNNING_EVENT_REFLASH_INFO.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF1 || bUNNING_EVENT_REFLASH_INFO.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF2 || bUNNING_EVENT_REFLASH_INFO.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF3 || bUNNING_EVENT_REFLASH_INFO.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF4 || bUNNING_EVENT_REFLASH_INFO.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF5 || bUNNING_EVENT_REFLASH_INFO.m_eEventType == eBUNNING_EVENT.eBUNNING_EVENT_GMBUFF6)
			{
				string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2543");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
					"day",
					textFromInterface2,
					"starttime",
					bUNNING_EVENT_REFLASH_INFO.m_nStartTime.ToString(),
					"endtime",
					num2.ToString()
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1794"),
					"day",
					text3,
					"starttime",
					bUNNING_EVENT_REFLASH_INFO.m_nStartTime.ToString(),
					"endtime",
					num2.ToString()
				});
			}
			this.m_lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(bUNNING_EVENT_REFLASH_INFO.m_nTitleText.ToString()));
			this.m_lbEventDetail.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(bUNNING_EVENT_REFLASH_INFO.m_nExplain.ToString()));
		}
		this.m_lbTime.SetText(text);
	}

	public void SelectTabInfo(int tabindex)
	{
	}

	public string EventWeek(int nEventInfoWeek)
	{
		string result = string.Empty;
		switch (nEventInfoWeek + 1)
		{
		case 0:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2242");
			break;
		case 1:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2243");
			break;
		case 2:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2244");
			break;
		case 3:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2245");
			break;
		case 4:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2246");
			break;
		case 5:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2247");
			break;
		case 6:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2248");
			break;
		case 7:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2249");
			break;
		}
		return result;
	}
}
