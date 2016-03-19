using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class Agit_GoldenEggDlg : Form
{
	private const NewGuildDefine.eNEWGUILD_NPC_TYPE NPCType = NewGuildDefine.eNEWGUILD_NPC_TYPE.eNEWGUILD_NPC_TYPE_GOLDENEGG;

	private Label m_lbSubTitle;

	private DrawTexture m_dtPortrait;

	private Label m_lbDefaultTakenInfo;

	private NewListBox m_nlbReward;

	private DrawTexture m_dtProgressBar;

	private Label m_lbProgress;

	private float m_fProgressBarMaxWidth;

	private Button m_btReward;

	private Button m_btClose;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/Agit/DLG_goldenegg", G_ID.AGIT_GOLDENEGG_DLG, true);
		GS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ gS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ = new GS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ();
		gS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ.i32GoldenEggGetCount = NrTSingleton<NewGuildManager>.Instance.GetGoldenEggGetCount();
		gS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ.i64LastGetPersonID = NrTSingleton<NewGuildManager>.Instance.GetGoldenEggGetLastPerson();
		SendPacket.GetInstance().SendObject(2321, gS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ);
	}

	public override void SetComponent()
	{
		this.m_dtProgressBar = (base.GetControl("DT_Progress") as DrawTexture);
		this.m_dtPortrait = (base.GetControl("DT_port") as DrawTexture);
		this.m_dtPortrait.SetTexture(eCharImageType.LARGE, NrTSingleton<NewGuildManager>.Instance.GetAgitNPCCharKindFromNPCType(5), 0);
		this.m_lbDefaultTakenInfo = (base.GetControl("Label_default") as Label);
		this.m_nlbReward = (base.GetControl("NLB_reward") as NewListBox);
		this.m_dtProgressBar = (base.GetControl("DT_Progress") as DrawTexture);
		this.m_fProgressBarMaxWidth = this.m_dtProgressBar.width;
		this.m_lbSubTitle = (base.GetControl("Label_number") as Label);
		this.m_lbProgress = (base.GetControl("LB_Progress") as Label);
		this.m_btReward = (base.GetControl("BT_reward") as Button);
		this.m_btReward.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickReward));
		this.m_btClose = (base.GetControl("BT_close") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickClose));
		this.SetGoldenEggGetCount();
		this.SetGoldenEggPoint(0);
	}

	public void SetInfo(int GoldenEggPoint, bool bCanReward)
	{
		this.SetGoldenEggGetCount();
		this.SetGoldenEggPoint(GoldenEggPoint);
		this.SetTakenUserInfo();
		this.m_btReward.SetEnabled(bCanReward);
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		long personID = charPersonInfo.GetPersonID();
		List<AGIT_GOLDENEGG_INFO_SUB_DATA> rewardPersonInfoList = NrTSingleton<NewGuildManager>.Instance.GetRewardPersonInfoList();
		foreach (AGIT_GOLDENEGG_INFO_SUB_DATA current in rewardPersonInfoList)
		{
			if (current.i64PersonID == personID)
			{
				this.m_btReward.SetEnabled(false);
			}
		}
	}

	public void SetGoldenEggGetCount()
	{
		int goldenEggGetCount = NrTSingleton<NewGuildManager>.Instance.GetGoldenEggGetCount();
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2753"),
			"count",
			goldenEggGetCount + 1
		});
		this.m_lbSubTitle.SetText(empty);
	}

	public void SetGoldenEggPoint(int curPoint)
	{
		int num = 0;
		AGIT_NPC_SUB_DATA agitNPCSubDataFromNPCType = NrTSingleton<NewGuildManager>.Instance.GetAgitNPCSubDataFromNPCType(5);
		if (agitNPCSubDataFromNPCType != null)
		{
			AgitNPCData agitNPCData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitNPCData(agitNPCSubDataFromNPCType.ui8NPCType.ToString());
			if (agitNPCData != null)
			{
				num = agitNPCData.i32LevelRate[(int)(agitNPCSubDataFromNPCType.i16NPCLevel - 1)];
			}
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("603"),
			"count1",
			curPoint,
			"count2",
			num
		});
		this.m_lbProgress.SetText(empty);
		float num2 = 0f;
		if (curPoint != 0 && num != 0)
		{
			num2 = (float)curPoint / (float)num;
		}
		this.m_dtProgressBar.SetSize(this.m_fProgressBarMaxWidth * num2, this.m_dtProgressBar.height);
	}

	public void SetTakenUserInfo()
	{
		this.m_nlbReward.Clear();
		List<AGIT_GOLDENEGG_INFO_SUB_DATA> rewardPersonInfoList = NrTSingleton<NewGuildManager>.Instance.GetRewardPersonInfoList();
		rewardPersonInfoList.Reverse(0, rewardPersonInfoList.Count);
		foreach (AGIT_GOLDENEGG_INFO_SUB_DATA current in rewardPersonInfoList)
		{
			this.m_lbDefaultTakenInfo.Hide(true);
			NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(current.i64PersonID);
			if (memberInfoFromPersonID == null)
			{
				return;
			}
			string charName = memberInfoFromPersonID.GetCharName();
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.i32ItemUnique);
			string text = string.Empty;
			if (current.i8GoldenEggType == 1)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2778");
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2779");
			}
			NewListItem newListItem = new NewListItem(this.m_nlbReward.ColumnNum, true);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2770"),
				"charname",
				charName,
				"type",
				text,
				"itemname",
				itemNameByItemUnique,
				"itemnum",
				current.i32ItemQuentity
			});
			newListItem.SetListItemData(0, empty, null, null, null);
			this.m_nlbReward.Add(newListItem);
		}
		this.m_nlbReward.RepositionItems();
		rewardPersonInfoList.Reverse(0, rewardPersonInfoList.Count);
	}

	public void ClickReward(IUIObject obj)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		long personID = charPersonInfo.GetPersonID();
		NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(personID);
		if (memberInfoFromPersonID == null || memberInfoFromPersonID.GetRank() <= NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_INITIATE)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("785");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			return;
		}
		GS_NEWGUILD_AGIT_GOLDENEGG_GET_REQ obj2 = new GS_NEWGUILD_AGIT_GOLDENEGG_GET_REQ();
		SendPacket.GetInstance().SendObject(2323, obj2);
	}

	public void ClickClose(IUIObject obj)
	{
		this.Close();
	}
}
