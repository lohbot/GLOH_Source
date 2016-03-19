using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class BabelGuildBossInfoDlg : Form
{
	private const float DAMAGE_BAR_WIDTH = 340f;

	private float BOSSHP_BAR_WIDTH = 500f;

	private DrawTexture m_dtBossImg;

	private DrawTexture m_dtBossCurHP;

	private DrawTexture m_dtBossVictoyImg;

	private Label m_laGuildBossName;

	private Label m_laGuildBossHp;

	private Label m_laBasicReward;

	private Label m_lbRewardBaseItemNum;

	private Button m_btGuildBossStart;

	private Button m_btBasicRewardExplain;

	private Button m_btRankRewardExplain;

	private NewListBox m_lbGuildMemberList;

	private DrawTexture m_dtRewardBaseItem;

	private ItemTexture m_itClearUserFace;

	private Label m_lbClearUserName;

	private DrawTexture m_dtClearUserBG;

	private DrawTexture m_dtClearBG;

	private List<NEWGUILD_BOSS_PLAYER_INFO> m_listMemberInfo = new List<NEWGUILD_BOSS_PLAYER_INFO>();

	private short m_GuildBossFloor;

	private byte m_byRoomState;

	private int m_i32CurBossHp;

	private byte m_byReward;

	private long m_i64ClearPersonID;

	public byte Reward
	{
		get
		{
			return this.m_byReward;
		}
		set
		{
			this.m_byReward = value;
		}
	}

	public long ClearPersonID
	{
		get
		{
			return this.m_i64ClearPersonID;
		}
		set
		{
			this.m_i64ClearPersonID = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "GuildBoss/DLG_GuildBossInfo", G_ID.BABEL_GUILDBOSS_INFO_DLG, false, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_dtBossImg = (base.GetControl("DrawTexture_character") as DrawTexture);
		this.m_dtBossCurHP = (base.GetControl("DrawTexture_BG2") as DrawTexture);
		this.m_dtBossVictoyImg = (base.GetControl("DrawTexture_Victory") as DrawTexture);
		this.m_dtBossVictoyImg.Visible = false;
		this.m_laGuildBossName = (base.GetControl("Label_BossName") as Label);
		this.m_laGuildBossHp = (base.GetControl("Label_BossHp") as Label);
		this.m_laBasicReward = (base.GetControl("Label_BaseGoldCount") as Label);
		this.m_lbRewardBaseItemNum = (base.GetControl("LB_BaseItemCount") as Label);
		this.m_btGuildBossStart = (base.GetControl("btn_PLUS") as Button);
		this.m_btBasicRewardExplain = (base.GetControl("btn_HELP1") as Button);
		Button expr_D8 = this.m_btBasicRewardExplain;
		expr_D8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_D8.Click, new EZValueChangedDelegate(this.OnClickBasicRewardExplain));
		this.m_btRankRewardExplain = (base.GetControl("btn_HELP2") as Button);
		Button expr_115 = this.m_btRankRewardExplain;
		expr_115.Click = (EZValueChangedDelegate)Delegate.Combine(expr_115.Click, new EZValueChangedDelegate(this.OnClickRankRewardExplain));
		this.m_lbGuildMemberList = (base.GetControl("NewListBox_guildmember") as NewListBox);
		this.m_dtRewardBaseItem = (base.GetControl("DT_BaseItem") as DrawTexture);
		this.m_itClearUserFace = (base.GetControl("ItemTexture_Winner") as ItemTexture);
		this.m_itClearUserFace.Visible = false;
		this.m_lbClearUserName = (base.GetControl("Label_WinnerName") as Label);
		this.m_lbClearUserName.Visible = false;
		this.m_dtClearUserBG = (base.GetControl("DrawTexture_VictoryBG") as DrawTexture);
		this.m_dtClearUserBG.Visible = false;
		this.m_dtClearBG = (base.GetControl("DrawTexture_Victory2") as DrawTexture);
		this.m_dtClearBG.Visible = false;
		this.BOSSHP_BAR_WIDTH = this.m_dtBossCurHP.GetSize().x;
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), base.GetLocation().z - 8f);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EXPLAIN_TOOLTIP_DLG);
		base.OnClose();
	}

	public override void Show()
	{
		this.SetInfo();
		if (!base.Visible)
		{
			base.Show();
		}
	}

	public void InitInfo()
	{
		this.m_GuildBossFloor = 0;
		this.m_byRoomState = 0;
		this.m_i32CurBossHp = 0;
		this.m_listMemberInfo.Clear();
	}

	public void SetBossInfo(short floor, byte roomstate, int bosshp, byte reward, long i64ClearPersonID)
	{
		this.m_GuildBossFloor = floor;
		this.m_byRoomState = roomstate;
		this.m_i32CurBossHp = bosshp;
		this.m_byReward = reward;
		this.m_i64ClearPersonID = i64ClearPersonID;
	}

	public void AddPlayerInfo(NEWGUILD_BOSS_PLAYER_INFO player_info)
	{
		this.m_listMemberInfo.Add(player_info);
	}

	public void SetInfo()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		BABEL_GUILDBOSS babelGuildBossinfo = NrTSingleton<BabelTowerManager>.Instance.GetBabelGuildBossinfo(this.m_GuildBossFloor);
		if (babelGuildBossinfo == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(babelGuildBossinfo.m_nBossKind);
		this.m_laGuildBossName.Text = charKindInfo.GetName();
		this.m_dtBossImg.SetTexture(eCharImageType.LARGE, charKindInfo.GetCharKind(), -1);
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1808");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"count1",
			ANNUALIZED.Convert(this.m_i32CurBossHp),
			"count2",
			ANNUALIZED.Convert(babelGuildBossinfo.m_nBossMaxHP)
		});
		this.m_laGuildBossHp.Text = text2;
		float num = (float)this.m_i32CurBossHp / (float)babelGuildBossinfo.m_nBossMaxHP;
		this.m_dtBossCurHP.SetSize(this.BOSSHP_BAR_WIDTH * num, this.m_dtBossCurHP.GetSize().y);
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
		{
			text,
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(babelGuildBossinfo.m_nBaseReward_ItemUnique),
			"count",
			babelGuildBossinfo.m_nBaseReward_ItemNum
		});
		this.m_lbRewardBaseItemNum.Text = text2;
		int num2 = (babelGuildBossinfo.m_nReward_BaseMoney * kMyCharInfo.GetLevel() >= babelGuildBossinfo.m_nReward_BaseMoney_Max) ? babelGuildBossinfo.m_nReward_BaseMoney_Max : (babelGuildBossinfo.m_nReward_BaseMoney * kMyCharInfo.GetLevel());
		this.m_laBasicReward.Text = ANNUALIZED.Convert(num2);
		this.m_dtRewardBaseItem.SetTexture(NrTSingleton<ItemManager>.Instance.GetItemTexture(babelGuildBossinfo.m_nBaseReward_ItemUnique));
		this.SortMemberInfo();
		int num3 = 1;
		bool flag = false;
		this.m_lbGuildMemberList.Clear();
		for (int i = 0; i < this.m_listMemberInfo.Count; i++)
		{
			NEWGUILD_BOSS_PLAYER_INFO nEWGUILD_BOSS_PLAYER_INFO = this.m_listMemberInfo[i];
			NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(nEWGUILD_BOSS_PLAYER_INFO.i64PersonID);
			if (memberInfoFromPersonID != null)
			{
				bool flag2 = false;
				NewListItem newListItem = new NewListItem(this.m_lbGuildMemberList.ColumnNum, true);
				Texture2D portrait = memberInfoFromPersonID.GetPortrait();
				if (this.m_byRoomState == 3)
				{
					if (nEWGUILD_BOSS_PLAYER_INFO.i64PersonID == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
					{
						flag = true;
					}
					if (nEWGUILD_BOSS_PLAYER_INFO.i64PersonID == this.ClearPersonID)
					{
						this.m_itClearUserFace.Visible = true;
						if (portrait != null)
						{
							this.m_itClearUserFace.SetTexture(portrait);
						}
						else
						{
							NkListSolInfo nkListSolInfo = new NkListSolInfo();
							nkListSolInfo.SolCharKind = memberInfoFromPersonID.GetFaceCharKind();
							nkListSolInfo.SolLevel = memberInfoFromPersonID.GetLevel();
							nkListSolInfo.SolGrade = -1;
							this.m_itClearUserFace.SetSolImageTexure(eCharImageType.SMALL, nkListSolInfo, false);
						}
						this.m_lbClearUserName.SetText(memberInfoFromPersonID.GetCharName());
						this.m_lbClearUserName.Visible = true;
						newListItem.SetListItemData(11, true);
						flag2 = true;
					}
				}
				if (!flag2)
				{
					newListItem.SetListItemData(11, false);
				}
				newListItem.SetListItemData(2, memberInfoFromPersonID.GetCharName(), null, null, null);
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1915");
				newListItem.SetListItemData(3, text2, null, null, null);
				float num4 = (float)nEWGUILD_BOSS_PLAYER_INFO.i32Damage / (float)babelGuildBossinfo.m_nBossMaxHP;
				float num5 = num4 * 340f;
				newListItem.SetListItemData(5, string.Empty, num5, null, null);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1916");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"count1",
					ANNUALIZED.Convert(nEWGUILD_BOSS_PLAYER_INFO.i32Damage),
					"count2",
					(int)(num4 * 100f)
				});
				newListItem.SetListItemData(6, text2, null, null, null);
				if (portrait != null)
				{
					newListItem.SetListItemData(7, portrait, null, null, null, null);
				}
				else
				{
					newListItem.SetListItemData(7, new NkListSolInfo
					{
						SolCharKind = memberInfoFromPersonID.GetFaceCharKind(),
						SolLevel = memberInfoFromPersonID.GetLevel(),
						SolGrade = -1
					}, null, null, null);
				}
				if (!flag2)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1186");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text,
						"count",
						num3
					});
					newListItem.SetListItemData(8, text2, null, null, null);
				}
				else
				{
					newListItem.SetListItemData(8, false);
				}
				newListItem.SetListItemData(9, memberInfoFromPersonID.GetRankText(), null, null, null);
				this.m_lbGuildMemberList.Add(newListItem);
				num3++;
			}
		}
		this.m_lbGuildMemberList.RepositionItems();
		if (!flag)
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("633");
			this.m_btGuildBossStart.SetText(text2);
			Button expr_4F9 = this.m_btGuildBossStart;
			expr_4F9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4F9.Click, new EZValueChangedDelegate(this.OnClickStart));
		}
		else
		{
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("493");
			this.m_btGuildBossStart.SetText(text2);
			if (this.m_byReward == 0)
			{
				Button expr_54C = this.m_btGuildBossStart;
				expr_54C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_54C.Click, new EZValueChangedDelegate(this.OnClickRankReward));
			}
			else
			{
				this.m_btGuildBossStart.SetEnabled(false);
			}
		}
		if (this.m_byRoomState == 3)
		{
			this.m_dtBossVictoyImg.Visible = true;
			this.m_dtClearUserBG.Visible = true;
			this.m_dtClearBG.Visible = true;
		}
	}

	public void SortMemberInfo()
	{
		this.m_listMemberInfo.Sort(new Comparison<NEWGUILD_BOSS_PLAYER_INFO>(BabelGuildBossInfoDlg.CompareInfo));
	}

	private static int CompareInfo(NEWGUILD_BOSS_PLAYER_INFO x, NEWGUILD_BOSS_PLAYER_INFO y)
	{
		if (x.i32Rank > 0 && y.i32Rank > 0)
		{
			if (x.i32Rank < y.i32Rank)
			{
				return -1;
			}
		}
		else if (x.i32Damage > y.i32Damage)
		{
			return -1;
		}
		return 1;
	}

	public void OnClickBasicRewardExplain(IUIObject obj)
	{
		ExplainTooltipDlg explainTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPLAIN_TOOLTIP_DLG) as ExplainTooltipDlg;
		if (explainTooltipDlg != null)
		{
			explainTooltipDlg.SetExplainType(ExplainTooltipDlg.eEXPLAIN_TYPE.eEXPLAIN_GUILDBOSS_BASICREWARD, this);
			explainTooltipDlg.Show();
		}
	}

	public void OnClickRankRewardExplain(IUIObject obj)
	{
		ExplainTooltipDlg explainTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPLAIN_TOOLTIP_DLG) as ExplainTooltipDlg;
		if (explainTooltipDlg != null)
		{
			explainTooltipDlg.SetExplainType(ExplainTooltipDlg.eEXPLAIN_TYPE.eEXPLAIN_GUILDBOSS_RANKREWARD, this);
			explainTooltipDlg.Show();
		}
	}

	public void OnClickStart(IUIObject obj)
	{
		short guildBossLastFloor = NrTSingleton<ContentsLimitManager>.Instance.GetGuildBossLastFloor();
		if (0 < guildBossLastFloor && guildBossLastFloor < this.m_GuildBossFloor)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("608"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_NEWGUILD_BOSS_ROOMCHECK_REQ gS_NEWGUILD_BOSS_ROOMCHECK_REQ = new GS_NEWGUILD_BOSS_ROOMCHECK_REQ();
		gS_NEWGUILD_BOSS_ROOMCHECK_REQ.i16Floor = this.m_GuildBossFloor;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_BOSS_ROOMCHECK_REQ, gS_NEWGUILD_BOSS_ROOMCHECK_REQ);
	}

	public void OnClickRankReward(IUIObject obj)
	{
		short guildBossLastFloor = NrTSingleton<ContentsLimitManager>.Instance.GetGuildBossLastFloor();
		if (0 < guildBossLastFloor && guildBossLastFloor < this.m_GuildBossFloor)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("608"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_NEWGUILD_BOSS_GETREWARD_REQ gS_NEWGUILD_BOSS_GETREWARD_REQ = new GS_NEWGUILD_BOSS_GETREWARD_REQ();
		gS_NEWGUILD_BOSS_GETREWARD_REQ.i16Floor = this.m_GuildBossFloor;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_BOSS_GETREWARD_REQ, gS_NEWGUILD_BOSS_GETREWARD_REQ);
	}
}
