using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class TimeShopInfo_DLG : Form
{
	private List<ChallengeTable> m_lstTimeShopTable = new List<ChallengeTable>();

	private bool m_bRequestReward;

	private Button m_btnClose;

	private DrawTexture m_dtBanner;

	private DrawTexture m_dtMissionIcon;

	private DrawTexture m_dtItem;

	private Label m_lbMissionTitle;

	private Label m_lbMissionContent;

	private Label m_lbItemName;

	private Label m_lbRewardInfo;

	private Button m_btnReward;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/dlg_timeshop_info", G_ID.TIMESHOP_INFO_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btnClose = (base.GetControl("Button_cancel") as Button);
		this.m_btnClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Close));
		this.m_dtBanner = (base.GetControl("DT_Banner") as DrawTexture);
		this.m_dtBanner.SetTextureFromBundle("ui/itemshop/timeshop_banner");
		this.m_dtMissionIcon = (base.GetControl("DT_MissionIcn") as DrawTexture);
		this.m_dtItem = (base.GetControl("DT_ItemImage") as DrawTexture);
		this.m_lbMissionTitle = (base.GetControl("LB_MissonTitle") as Label);
		this.m_lbMissionContent = (base.GetControl("LB_MissonContent") as Label);
		this.m_lbItemName = (base.GetControl("LB_ItemName") as Label);
		this.m_lbRewardInfo = (base.GetControl("Label_RewardInfo") as Label);
		this.m_btnReward = (base.GetControl("BT_Reward") as Button);
		this.m_btnReward.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Reward));
		this.Set_Value();
	}

	private void Set_Value()
	{
		ChallengeManager.eCHALLENGECODE[] timeShopChallengeCode = NrTSingleton<NrTableTimeShopManager>.Instance.Get_TimeShopChallengeCode();
		if (timeShopChallengeCode == null)
		{
			return;
		}
		for (int i = 0; i < timeShopChallengeCode.Length; i++)
		{
			ChallengeTable challengeTable = NrTSingleton<ChallengeManager>.Instance.GetChallengeTable((short)timeShopChallengeCode[i]);
			if (challengeTable != null)
			{
				this.m_lstTimeShopTable.Add(challengeTable);
			}
		}
		this.Set_ChallengeInfo();
	}

	public void Set_ChallengeInfo()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (kMyCharInfo.GetUserChallengeInfo() == null)
		{
			return;
		}
		int num = -1;
		string str = string.Empty;
		for (int i = 0; i < this.m_lstTimeShopTable.Count; i++)
		{
			if ((int)this.m_lstTimeShopTable[i].m_nLevel <= kMyCharInfo.GetLevel())
			{
				for (int j = 0; j < this.m_lstTimeShopTable[i].m_kRewardInfo.Count; j++)
				{
					if (kMyCharInfo.GetLevel() < this.m_lstTimeShopTable[i].m_kRewardInfo[j].m_nConditionLevel)
					{
						num = j;
						break;
					}
				}
				if (num != -1)
				{
					long charDetail = kMyCharInfo.GetCharDetail(12);
					if (1L <= (charDetail & this.m_lstTimeShopTable[i].m_nCheckRewardValue))
					{
						if (i < this.m_lstTimeShopTable.Count - 1)
						{
							goto IL_492;
						}
						this.m_btnReward.SetEnabled(false);
					}
					this.m_dtMissionIcon.SetTexture(NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(this.m_lstTimeShopTable[i].m_szIconKey));
					this.m_lbMissionTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(this.m_lstTimeShopTable[i].m_szTitleTextKey));
					long num2 = (long)kMyCharInfo.GetDayCharDetail((eCHAR_DAY_COUNT)this.m_lstTimeShopTable[i].m_nDetailInfoIndex);
					long num3;
					if (num2 >= (long)this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_nConditionCount)
					{
						str = NrTSingleton<CTextParser>.Instance.GetTextColor("1105");
						num3 = (long)this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_nConditionCount;
					}
					else
					{
						num3 = num2;
						this.m_btnReward.SetEnabled(false);
					}
					string str2 = string.Empty;
					string text = NrTSingleton<NrTextMgr>.Instance.GetTextFromChallenge(this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_szConditionTextKey);
					if (text.Contains("count"))
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str2, new object[]
						{
							text,
							"count",
							this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_nConditionCount,
							"count1",
							num3,
							"count2",
							this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_nConditionCount
						});
					}
					else
					{
						str2 = text;
					}
					this.m_lbMissionContent.SetText(str + str2);
					if (this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_nMoney > 0L)
					{
						this.m_dtItem.SetTexture(NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Main_I_ExtraI01"));
						text = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1119"),
							"count",
							this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_nMoney
						});
						this.m_lbItemName.SetText(text);
					}
					else if (this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_nItemUnique > 0)
					{
						this.m_dtItem.SetTexture(NrTSingleton<ItemManager>.Instance.GetItemTexture(this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_nItemUnique));
						text = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
							"itemname",
							NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_nItemUnique),
							"count",
							this.m_lstTimeShopTable[i].m_kRewardInfo[num].m_nItemNum
						});
						this.m_lbItemName.SetText(text);
					}
					this.m_lbRewardInfo.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3331"));
					this.m_btnReward.Data = this.m_lstTimeShopTable[i].m_nUnique;
					break;
				}
			}
			IL_492:;
		}
		this.m_bRequestReward = false;
	}

	private void Click_Close(IUIObject _obj)
	{
		this.Close();
	}

	private void Click_Reward(IUIObject _obj)
	{
		if (_obj == null)
		{
			return;
		}
		if (this.m_bRequestReward)
		{
			return;
		}
		GS_CHAR_CHALLENGE_REWARD_REQ gS_CHAR_CHALLENGE_REWARD_REQ = new GS_CHAR_CHALLENGE_REWARD_REQ();
		gS_CHAR_CHALLENGE_REWARD_REQ.m_nUnique = (int)((short)_obj.Data);
		SendPacket.GetInstance().SendObject(1031, gS_CHAR_CHALLENGE_REWARD_REQ);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "REPUTATION", "REWARD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.m_bRequestReward = true;
	}
}
