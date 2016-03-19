using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NewGuildWarConditionDlg : Form
{
	private const int PAGE_COUNT = 4;

	private const float DELAY_TIME = 2f;

	private Label m_lbApplyNum;

	private Label m_lbMyGuildName;

	private Label m_lbState;

	private Label m_lbEnemyGuildName;

	private NewListBox m_nlbGuildWarList;

	private Button m_btReward;

	private Button m_btPrev;

	private Box m_bxPage;

	private Button m_btNext;

	private Button m_btRefresh;

	private string m_strText = string.Empty;

	private string m_strTime = string.Empty;

	private string m_strTemp_1 = string.Empty;

	private int m_iCurPage;

	private int m_iMaxPage = 1;

	private float m_fDelayTime;

	private List<GUILDWAR_APPLY_INFO> m_GuildWarApplyInfo = new List<GUILDWAR_APPLY_INFO>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/dlg_GuildWar_condition", G_ID.GUILDWAR_CONDITION_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_lbApplyNum = (base.GetControl("LB_join_count") as Label);
		this.m_lbMyGuildName = (base.GetControl("LB_MyGuildName1") as Label);
		this.m_lbState = (base.GetControl("LB_State") as Label);
		this.m_lbState.SetText(string.Empty);
		this.m_lbEnemyGuildName = (base.GetControl("LB_EnemyGuildName2") as Label);
		this.m_nlbGuildWarList = (base.GetControl("NLB_GuildWarCondition") as NewListBox);
		this.m_btReward = (base.GetControl("Button_Reward") as Button);
		this.m_btReward.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickReward));
		this.m_btPrev = (base.GetControl("BT_Page01") as Button);
		this.m_btPrev.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrev));
		this.m_bxPage = (base.GetControl("Box_Page") as Box);
		this.m_btNext = (base.GetControl("BT_Page02") as Button);
		this.m_btNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNext));
		this.m_btRefresh = (base.GetControl("BT_Refresh02") as Button);
		this.m_btRefresh.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickRefresh));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClickReward(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_REWARDINFO_REQ();
		this.SetControlEnable(false);
	}

	public void ClickPrev(IUIObject obj)
	{
		int num = this.m_iCurPage;
		num--;
		if (num < 0)
		{
			num = this.m_iMaxPage - 1;
		}
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_INFO_REQ(num);
		this.SetControlEnable(false);
	}

	public void ClickNext(IUIObject obj)
	{
		int num = this.m_iCurPage;
		num++;
		if (num >= this.m_iMaxPage)
		{
			num = 0;
		}
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_INFO_REQ(num);
		this.SetControlEnable(false);
	}

	public void ClickRefresh(IUIObject obj)
	{
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_INFO_REQ(this.m_iCurPage);
		this.SetControlEnable(false);
	}

	public void ClearApplyInfo()
	{
		this.m_GuildWarApplyInfo.Clear();
	}

	public void AddApplyInfo(GUILDWAR_APPLY_INFO Info)
	{
		this.m_GuildWarApplyInfo.Add(Info);
	}

	public void RefeshGuildWarInfo(GS_GUILDWAR_APPLY_INFO_ACK ACK)
	{
		this.m_nlbGuildWarList.Clear();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2545"),
			"count1",
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildWarApplyMilitaryCount(),
			"count2",
			NrTSingleton<GuildWarManager>.Instance.GuildWarJoinCount
		});
		this.m_lbApplyNum.SetText(this.m_strText);
		this.m_lbMyGuildName.SetText(NrTSingleton<NewGuildManager>.Instance.GetGuildName());
		this.m_lbEnemyGuildName.SetText(NrTSingleton<GuildWarManager>.Instance.GuildWarGuildName);
		for (byte b = 0; b < 4; b += 1)
		{
			byte b2 = ACK.ui8RaidUnique_BEGIN + b;
			if ((short)b2 < NrTSingleton<GuildWarManager>.Instance.GuildWarRound)
			{
				this.MakeGuildWarListItem(b2);
			}
		}
		this.m_nlbGuildWarList.RepositionItems();
		this.m_iCurPage = ACK.i32CurPage;
		this.m_iMaxPage = ACK.i32MaxPage;
		this.ShowPageNum();
	}

	public void MakeGuildWarListItem(byte iRaidUnique)
	{
		NewListItem newListItem = new NewListItem(this.m_nlbGuildWarList.ColumnNum, true);
		newListItem.Data = iRaidUnique;
		bool flag = false;
		GUILDWAR_APPLY_INFO guildWarApplyInfo = this.GetGuildWarApplyInfo(NrTSingleton<NewGuildManager>.Instance.GetGuildID(), iRaidUnique);
		if (guildWarApplyInfo != null)
		{
			switch (guildWarApplyInfo.ui8MilitaryStatus)
			{
			case 0:
				newListItem.SetListItemData(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1323"), iRaidUnique, new EZValueChangedDelegate(this.ClickGuildWarApply), null);
				newListItem.SetListItemData(3, false);
				newListItem.SetListItemData(4, false);
				break;
			case 1:
				if (guildWarApplyInfo.i64MilitaryActionTime > 0L)
				{
					newListItem.SetListItemData(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615"), iRaidUnique, new EZValueChangedDelegate(this.ClickGuildWarApplyCancel), null);
					this.m_strText = this.GetTimeToString(guildWarApplyInfo.i64MilitaryActionTime);
					if (this.m_strText != string.Empty)
					{
						this.m_strTime = string.Format("{0} : {1}", NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1525"), this.m_strText);
						newListItem.SetListItemData(3, this.m_strTime, null, null, null);
					}
					else
					{
						newListItem.SetListItemData(3, false);
					}
					newListItem.SetListItemData(4, false);
				}
				else
				{
					newListItem.SetListItemData(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615"), iRaidUnique, new EZValueChangedDelegate(this.ClickGuildWarApplyCancel), null);
					newListItem.SetListItemData(3, false);
					newListItem.SetListItemData(4, false);
				}
				break;
			case 2:
				newListItem.SetListItemData(0, false);
				if (guildWarApplyInfo.i64MilitaryActionTime > 0L)
				{
					this.m_strText = this.GetTimeToString(guildWarApplyInfo.i64MilitaryActionTime);
					if (this.m_strText != string.Empty)
					{
						this.m_strTime = string.Format("{0} : {1}", NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1526"), this.m_strText);
						newListItem.SetListItemData(4, this.m_strTime, null, null, null);
					}
					else
					{
						newListItem.SetListItemData(4, false);
					}
					newListItem.SetListItemData(3, false);
				}
				else
				{
					newListItem.SetListItemData(3, false);
					newListItem.SetListItemData(4, false);
				}
				break;
			}
			this.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1615");
			if (guildWarApplyInfo.i64MilitaryActionTime > 0L)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2549"),
					"count",
					(int)(iRaidUnique + 1)
				});
			}
		}
		else
		{
			newListItem.SetListItemData(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1323"), iRaidUnique, new EZValueChangedDelegate(this.ClickGuildWarApply), null);
			newListItem.SetListItemData(3, false);
			newListItem.SetListItemData(4, false);
		}
		if (!flag)
		{
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2549"),
			"count",
			(int)(iRaidUnique + 1)
		});
		newListItem.SetListItemData(1, this.m_strText, null, null, null);
		newListItem.SetListItemData(2, this.m_strText, null, null, null);
		newListItem.SetListItemData(5, string.Empty, iRaidUnique, new EZValueChangedDelegate(this.ClickMyMilitaryDetail), null);
		newListItem.SetListItemData(6, string.Empty, iRaidUnique, new EZValueChangedDelegate(this.ClickEnemyMilitaryDetail), null);
		int raidFaceCharKind = this.GetRaidFaceCharKind(NrTSingleton<NewGuildManager>.Instance.GetGuildID(), iRaidUnique);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(raidFaceCharKind);
		if (charKindInfo != null)
		{
			newListItem.SetListItemData(7, raidFaceCharKind, null, null, null);
		}
		else
		{
			newListItem.SetListItemData(7, true);
		}
		int raidFaceCharKind2 = this.GetRaidFaceCharKind(NrTSingleton<GuildWarManager>.Instance.GuildWarGuildID, iRaidUnique);
		charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(raidFaceCharKind2);
		if (charKindInfo != null)
		{
			newListItem.SetListItemData(8, raidFaceCharKind2, null, null, null);
		}
		else
		{
			newListItem.SetListItemData(8, true);
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1984"),
			"count1",
			this.GetRaidMilitaryCount(NrTSingleton<NewGuildManager>.Instance.GetGuildID(), iRaidUnique),
			"count2",
			9
		});
		newListItem.SetListItemData(9, this.m_strText, null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1984"),
			"count1",
			this.GetRaidMilitaryCount(NrTSingleton<GuildWarManager>.Instance.GuildWarGuildID, iRaidUnique),
			"count2",
			9
		});
		newListItem.SetListItemData(10, this.m_strText, null, null, null);
		this.m_nlbGuildWarList.Add(newListItem);
	}

	public void ShowPageNum()
	{
		this.m_strText = string.Format("{0}/{1}", (this.m_iCurPage + 1).ToString(), this.m_iMaxPage.ToString());
		this.m_bxPage.SetText(this.m_strText);
	}

	public void ClickGuildWarApply(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			return;
		}
		byte iRadeUnique = (byte)obj.Data;
		if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsGuildWarApplyUser())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("317"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_MILITARY_INFO_REQ(NrTSingleton<NewGuildManager>.Instance.GetGuildID(), iRadeUnique);
		this.SetControlEnable(false);
	}

	public void ClickGuildWarApplyCancel(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			return;
		}
		byte iRadeUnique = (byte)obj.Data;
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_CANCEL_REQ(iRadeUnique);
		this.SetControlEnable(false);
	}

	public void ClickMyMilitaryDetail(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			return;
		}
		byte iRadeUnique = (byte)obj.Data;
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_MILITARY_INFO_REQ(NrTSingleton<NewGuildManager>.Instance.GetGuildID(), iRadeUnique);
		this.SetControlEnable(false);
	}

	public void ClickEnemyMilitaryDetail(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			return;
		}
		byte iRadeUnique = (byte)obj.Data;
		NrTSingleton<GuildWarManager>.Instance.Send_GS_GUILDWAR_APPLY_MILITARY_INFO_REQ(NrTSingleton<GuildWarManager>.Instance.GuildWarGuildID, iRadeUnique);
		this.SetControlEnable(false);
	}

	public override void Update()
	{
		this.UpdateTime();
		if (this.m_fDelayTime > 0f && this.m_fDelayTime < Time.time)
		{
			this.SetControlEnable(true);
			this.m_fDelayTime = 0f;
		}
	}

	public void UpdateTime()
	{
		if (NrTSingleton<GuildWarManager>.Instance.GuildWarStartTime > 0L)
		{
			this.m_strText = this.GetTimeToString(NrTSingleton<GuildWarManager>.Instance.GuildWarStartTime);
			if (this.m_strText != string.Empty)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strTime, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1763"),
					"timestring",
					this.m_strText
				});
				this.m_lbState.SetText(this.m_strTime);
			}
			else
			{
				this.m_lbState.SetText(string.Empty);
			}
		}
		else
		{
			this.m_lbState.SetText(string.Empty);
		}
		for (int i = 0; i < this.m_nlbGuildWarList.Count; i++)
		{
			UIListItemContainer uIListItemContainer = this.m_nlbGuildWarList.GetItem(i) as UIListItemContainer;
			if (!(uIListItemContainer == null))
			{
				byte b = (byte)uIListItemContainer.Data;
				if ((short)b < NrTSingleton<GuildWarManager>.Instance.GuildWarRound)
				{
					GUILDWAR_APPLY_INFO guildWarApplyInfo = this.GetGuildWarApplyInfo(NrTSingleton<NewGuildManager>.Instance.GetGuildID(), b);
					if (guildWarApplyInfo != null)
					{
						eGUILDWAR_MILITARY_STATE ui8MilitaryStatus = (eGUILDWAR_MILITARY_STATE)guildWarApplyInfo.ui8MilitaryStatus;
						if (ui8MilitaryStatus != eGUILDWAR_MILITARY_STATE.eGUILDWAR_MILITARY_STATE_MOVE)
						{
							if (ui8MilitaryStatus == eGUILDWAR_MILITARY_STATE.eGUILDWAR_MILITARY_STATE_MOVECANCEL)
							{
								if (guildWarApplyInfo.i64MilitaryActionTime > 0L)
								{
									Label label = uIListItemContainer.GetElement(4) as Label;
									if (label != null)
									{
										this.m_strText = this.GetTimeToString(guildWarApplyInfo.i64MilitaryActionTime);
										if (this.m_strText != string.Empty)
										{
											this.m_strTime = string.Format("{0} : {1}", NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1525"), this.m_strText);
											label.SetText(this.m_strTime);
										}
										else
										{
											label.SetText(string.Empty);
										}
									}
								}
							}
						}
						else if (guildWarApplyInfo.i64MilitaryActionTime > 0L)
						{
							Label label2 = uIListItemContainer.GetElement(3) as Label;
							if (label2 != null)
							{
								this.m_strText = this.GetTimeToString(guildWarApplyInfo.i64MilitaryActionTime);
								if (this.m_strText != string.Empty)
								{
									this.m_strTime = string.Format("{0} : {1}", NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1525"), this.m_strText);
									label2.SetText(this.m_strTime);
								}
								else
								{
									label2.SetText(string.Empty);
								}
							}
						}
					}
				}
			}
		}
	}

	public GUILDWAR_APPLY_INFO GetGuildWarApplyInfo(long i64GuildID, byte iRaideUnique)
	{
		for (int i = 0; i < this.m_GuildWarApplyInfo.Count; i++)
		{
			if (this.m_GuildWarApplyInfo[i].i64GuildID == i64GuildID)
			{
				if (this.m_GuildWarApplyInfo[i].ui8RaidUnique == iRaideUnique)
				{
					return this.m_GuildWarApplyInfo[i];
				}
			}
		}
		return null;
	}

	public int GetRaidMilitaryCount(long i64GuildID, byte iRaideUnique)
	{
		for (int i = 0; i < this.m_GuildWarApplyInfo.Count; i++)
		{
			if (this.m_GuildWarApplyInfo[i].i64GuildID == i64GuildID)
			{
				if (this.m_GuildWarApplyInfo[i].ui8RaidUnique == iRaideUnique)
				{
					return this.m_GuildWarApplyInfo[i].i32MilitaryCount;
				}
			}
		}
		return 0;
	}

	public void SetControlEnable(bool bEnable)
	{
		if (!bEnable)
		{
			this.m_fDelayTime = Time.time + 2f;
		}
		this.m_btPrev.controlIsEnabled = bEnable;
		this.m_btNext.controlIsEnabled = bEnable;
		this.m_btRefresh.controlIsEnabled = bEnable;
	}

	public int GetRaidFaceCharKind(long i64GuildID, byte iRaideUnique)
	{
		for (int i = 0; i < this.m_GuildWarApplyInfo.Count; i++)
		{
			if (this.m_GuildWarApplyInfo[i].i64GuildID == i64GuildID)
			{
				if (this.m_GuildWarApplyInfo[i].ui8RaidUnique == iRaideUnique)
				{
					return this.m_GuildWarApplyInfo[i].i32rep_CharKind;
				}
			}
		}
		return 0;
	}

	public string GetTimeToString(long i64Time)
	{
		long num = i64Time - PublicMethod.GetCurTime();
		this.m_strTemp_1 = string.Empty;
		if (i64Time > 0L)
		{
			long totalHourFromSec = PublicMethod.GetTotalHourFromSec(num);
			long minuteFromSec = PublicMethod.GetMinuteFromSec(num);
			long num2 = num % 60L;
			this.m_strTemp_1 = string.Format("{0}:{1}:{2}", totalHourFromSec.ToString("00"), minuteFromSec.ToString("00"), num2.ToString("00"));
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1763"),
				"timestring",
				this.m_strTemp_1
			});
		}
		return this.m_strTemp_1;
	}
}
