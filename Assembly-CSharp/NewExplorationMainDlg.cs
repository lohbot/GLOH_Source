using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class NewExplorationMainDlg : Form
{
	private NewListBox m_nlbStage;

	private NewListBox m_nlbFloor;

	private DrawTexture m_dtFloor;

	private Label m_lbTitle;

	private Label m_lbDragonHeart;

	private Label m_lbSoulGem;

	private Button m_btSoulGemShop;

	private Label m_lbRank;

	private Label m_lbRankPercent;

	private Button m_btEnd;

	private Button m_btAutoBattle;

	private Button m_btReset;

	private sbyte m_bSelectedFloor;

	private float m_fScrollPos;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewExploration/DLG_NewExploration_Main", G_ID.NEWEXPLORATION_MAIN_DLG, false, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_nlbFloor = (base.GetControl("NLB_basement") as NewListBox);
		this.m_dtFloor = (base.GetControl("DT_tower") as DrawTexture);
		this.m_dtFloor.SetTextureFromBundle(string.Format("ui/NewExploration/NewExploration_tower_BG", new object[0]));
		this.m_nlbStage = (base.GetControl("NLB_NewExploration") as NewListBox);
		this.m_nlbStage.Reserve = false;
		this.m_lbTitle = (base.GetControl("Label_PageTitleLabel01") as Label);
		this.m_lbDragonHeart = (base.GetControl("LB_DragonHeart") as Label);
		this.m_lbSoulGem = (base.GetControl("LB_MySoulGem") as Label);
		this.m_btSoulGemShop = (base.GetControl("Btn_SoulGemShop") as Button);
		Button expr_C7 = this.m_btSoulGemShop;
		expr_C7.Click = (EZValueChangedDelegate)Delegate.Combine(expr_C7.Click, new EZValueChangedDelegate(this.OnClickSoulGemShop));
		this.m_lbRank = (base.GetControl("LB_MyRank") as Label);
		this.m_lbRankPercent = (base.GetControl("LB_AllRank") as Label);
		this.m_btEnd = (base.GetControl("BT_End") as Button);
		Button expr_130 = this.m_btEnd;
		expr_130.Click = (EZValueChangedDelegate)Delegate.Combine(expr_130.Click, new EZValueChangedDelegate(this.OnClickEnd));
		this.m_btAutoBattle = (base.GetControl("BT_Autobattle") as Button);
		Button expr_16D = this.m_btAutoBattle;
		expr_16D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_16D.Click, new EZValueChangedDelegate(this.OnClickAutoBattle));
		this.m_btReset = (base.GetControl("BT_Reset") as Button);
		Button expr_1AA = this.m_btReset;
		expr_1AA.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1AA.Click, new EZValueChangedDelegate(this.OnClickReset));
		this.m_bSelectedFloor = NrTSingleton<NewExplorationManager>.Instance.GetFloor();
		this.SetFloorList();
		base.SetLayerZ(1, -1.5f);
		base.SetLayerZ(2, -1.5f);
		base.SetLayerZ(4, -1.5f);
		this.closeButton.SetLocationZ(this.closeButton.GetLocation().z - 1.5f);
		this.m_nlbFloor.SetLocationZ(this.m_nlbFloor.GetLocation().z - 1.7f);
		this.SetInfo();
		this.SetRankInfo(0, 0);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWEXPLORATION_RANK_GET_REQ, new GS_NEWEXPLORATION_RANK_GET_REQ());
	}

	public void SetRankInfo(int i32Rank, int i32TotalCount)
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1186"),
			"count",
			i32Rank
		});
		this.m_lbRank.SetText(empty);
		float num = 0f;
		if (i32TotalCount != 0 && i32Rank != 0)
		{
			num = (float)i32Rank / (float)i32TotalCount * 100f;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3553"),
			"Count",
			string.Format("{0:0.00}", num)
		});
		this.m_lbRankPercent.SetText(empty);
	}

	public void SetInfo()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() != eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_START)
		{
			this.m_btEnd.SetEnabled(false);
		}
		else
		{
			this.m_btEnd.SetEnabled(true);
		}
		int num = NkUserInventory.GetInstance().Get_First_ItemCnt(70002);
		this.m_lbSoulGem.SetText(ANNUALIZED.Convert(num));
		int num2 = 0;
		NEWEXPLORATION_DATA endRewardData = NrTSingleton<NewExplorationManager>.Instance.GetEndRewardData();
		if (endRewardData != null)
		{
			num2 = endRewardData.i32RewardCount;
		}
		this.m_lbDragonHeart.SetText(ANNUALIZED.Convert(num2));
	}

	public void SetFloorList()
	{
		Dictionary<short, NEWEXPLORATION_DATA> dataList = NrTSingleton<NewExplorationManager>.Instance.GetDataList();
		sbyte b = -1;
		string empty = string.Empty;
		this.m_nlbFloor.Clear();
		sbyte floor = NrTSingleton<NewExplorationManager>.Instance.GetFloor();
		sbyte b2 = -1;
		NEWEXPLORATION_DATA nEWEXPLORATION_DATA = NrTSingleton<NewExplorationManager>.Instance.CanGetTreasureData();
		if (nEWEXPLORATION_DATA != null)
		{
			b2 = nEWEXPLORATION_DATA.bFloor;
		}
		foreach (KeyValuePair<short, NEWEXPLORATION_DATA> current in dataList)
		{
			if ((int)b != (int)current.Value.bFloor)
			{
				b = current.Value.bFloor;
				NewListItem newListItem = new NewListItem(this.m_nlbFloor.ColumnNum, true, string.Empty);
				newListItem.SetListItemData(1, string.Empty, b, new EZValueChangedDelegate(this.OnClickFloorList), null);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3424"),
					"count",
					(int)b
				});
				newListItem.SetListItemData(2, empty, null, null, null);
				newListItem.SetListItemData(3, (int)b < (int)floor);
				for (int i = 0; i < 3; i++)
				{
					newListItem.SetListItemData(i + 4, false);
					newListItem.SetListItemData(i + 7, false);
				}
				newListItem.SetListItemData(10, (int)b == (int)this.m_bSelectedFloor);
				newListItem.SetListItemData(11, (int)b == (int)b2);
				this.m_nlbFloor.Add(newListItem);
			}
		}
		this.m_nlbFloor.RepositionItems();
		this.SetStageList();
	}

	private void SetStageList()
	{
		this.m_fScrollPos = this.m_nlbStage.scrollPos;
		this.m_fScrollPos = ((this.m_fScrollPos < 0f) ? 0f : this.m_fScrollPos);
		this.m_nlbStage.Clear();
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3424"),
			"count",
			(int)this.m_bSelectedFloor
		});
		this.m_lbTitle.SetText(empty);
		for (int i = 0; i < 2; i++)
		{
			string text = string.Empty;
			if (i == 0)
			{
				text = string.Format("Mobile/DLG/NewExploration/NLB_NewExploration_columndata{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			}
			else
			{
				text = string.Format("Mobile/DLG/NewExploration/NLB_NewExploration2_columndata{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			}
			this.m_nlbStage.SetColumnData(text);
			NewListItem newListItem = new NewListItem(this.m_nlbStage.ColumnNum, true, string.Empty);
			newListItem.m_szColumnData = text;
			newListItem.SetListItemData(0, string.Format("ui/NewExploration/NewExploration_{0}_{1}", (int)this.m_bSelectedFloor, i + 1), true, null, null);
			int num = 0;
			for (int j = 0; j < 5; j++)
			{
				int num2 = j * 9 + 1;
				int num3 = j + i * 5 + 1;
				newListItem.SetListItemData(num2, string.Empty, (sbyte)num3, new EZValueChangedDelegate(this.OnClickStageList), null);
				newListItem.SetListItemData(num2 + 3, string.Format("{0}-{1}", this.m_bSelectedFloor, num3), null, null, null);
				if (num3 != 10)
				{
					NEWEXPLORATION_DATA data = NrTSingleton<NewExplorationManager>.Instance.GetData(this.m_bSelectedFloor, (sbyte)num3);
					if (data != null)
					{
						NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(data.i32BossCharKind);
						if (charKindInfo == null)
						{
							break;
						}
						CostumeDrawTextureInfo costumeDrawTextureInfo = new CostumeDrawTextureInfo();
						costumeDrawTextureInfo.imageType = eCharImageType.SMALL;
						costumeDrawTextureInfo.charKind = charKindInfo.GetCharKind();
						newListItem.SetListItemData(num2 + 4, costumeDrawTextureInfo, null, null, null);
					}
				}
				else
				{
					num2--;
				}
				bool visibe = NrTSingleton<NewExplorationManager>.Instance.IsClear(this.m_bSelectedFloor, (sbyte)num3);
				newListItem.SetListItemData(num2 + 5, visibe);
				newListItem.SetListItemData(num2 + 6, visibe);
				bool flag = (int)this.m_bSelectedFloor == (int)NrTSingleton<NewExplorationManager>.Instance.GetFloor() && (int)((sbyte)num3) == (int)NrTSingleton<NewExplorationManager>.Instance.GetSubFloor();
				newListItem.SetListItemData(num2 + 7, flag);
				newListItem.SetListItemData(num2 + 8, flag);
				if (flag)
				{
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo != null && charPersonInfo.GetLeaderSoldierInfo().IsValid())
					{
						NrCharKindInfo charKindInfo2 = charPersonInfo.GetLeaderSoldierInfo().GetCharKindInfo();
						if (charKindInfo2 != null)
						{
							CostumeDrawTextureInfo costumeDrawTextureInfo2 = new CostumeDrawTextureInfo();
							costumeDrawTextureInfo2.imageType = eCharImageType.SMALL;
							costumeDrawTextureInfo2.charKind = charKindInfo2.GetCharKind();
							newListItem.SetListItemData(num2 + 8, costumeDrawTextureInfo2, null, null, null);
						}
					}
				}
				num = num2 + 8;
			}
			num++;
			int num4 = (i != 0) ? 3 : 2;
			for (int k = 0; k < num4; k++)
			{
				int num5 = (k + 1) * 2;
				if (i != 0)
				{
					num5 += 4;
				}
				bool flag2 = NrTSingleton<NewExplorationManager>.Instance.IsOpenTreasureBox(this.m_bSelectedFloor, (sbyte)num5);
				newListItem.SetListItemData(num, !flag2);
				newListItem.SetListItemData(num, string.Empty, (sbyte)num5, new EZValueChangedDelegate(this.OnClickTreasureList), null);
				newListItem.SetListItemData(num + 1, flag2);
				newListItem.SetListItemData(num + 1, string.Empty, (sbyte)num5, new EZValueChangedDelegate(this.OnClickTreasureList), null);
				newListItem.SetListItemData(num + 4, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3425"), null, null, null);
				newListItem.SetListItemData(num + 5, NrTSingleton<NewExplorationManager>.Instance.CanGetTreasure(this.m_bSelectedFloor, (sbyte)num5));
				num += 6;
			}
			this.m_nlbStage.Add(newListItem);
		}
		this.m_nlbStage.RepositionItems();
		this.m_nlbStage.ScrollPosition = this.m_fScrollPos;
	}

	public void OnClickStageList(IUIObject obj)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewExplorationLimit())
		{
			return;
		}
		sbyte bSubFloor = (sbyte)obj.Data;
		NewExploration_StagePopupDlg newExploration_StagePopupDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWEXPLORATION_STAGEPOPUP_DLG) as NewExploration_StagePopupDlg;
		if (newExploration_StagePopupDlg != null)
		{
			newExploration_StagePopupDlg.SetFloor(this.m_bSelectedFloor, bSubFloor);
		}
	}

	public void OnClickFloorList(IUIObject obj)
	{
		this.m_bSelectedFloor = (sbyte)obj.Data;
		this.m_nlbStage.scrollPos = 0f;
		this.SetFloorList();
		this.SetStageList();
	}

	public void OnClickTreasureList(IUIObject obj)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewExplorationLimit())
		{
			return;
		}
		sbyte b = (sbyte)obj.Data;
		NEWEXPLORATION_TREASURE treasureData = NrTSingleton<NewExplorationManager>.Instance.GetTreasureData(this.m_bSelectedFloor, b);
		if (treasureData == null)
		{
			return;
		}
		BonusItemInfoDlg bonusItemInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BONUS_ITEM_INFO_DLG) as BonusItemInfoDlg;
		if (bonusItemInfoDlg == null)
		{
			return;
		}
		for (int i = 0; i < treasureData.i32ItemUnique.Length; i++)
		{
			if (treasureData.i32ItemUnique[i] > 0)
			{
				bonusItemInfoDlg.AddItem(new ITEM
				{
					m_nItemUnique = treasureData.i32ItemUnique[i],
					m_nItemNum = treasureData.i32ItemNum[i]
				});
			}
		}
		bonusItemInfoDlg.ShowItem();
		string strNotButtonNotify = string.Empty;
		bool flag = NrTSingleton<NewExplorationManager>.Instance.CanGetTreasure(this.m_bSelectedFloor, b);
		if (flag)
		{
			strNotButtonNotify = string.Empty;
		}
		else if (NrTSingleton<NewExplorationManager>.Instance.IsOpenTreasureBox(this.m_bSelectedFloor, b))
		{
			strNotButtonNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3555");
		}
		else
		{
			NEWEXPLORATION_TREASURE prevTreasureData = NrTSingleton<NewExplorationManager>.Instance.GetPrevTreasureData(this.m_bSelectedFloor, b);
			if (prevTreasureData == null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref strNotButtonNotify, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3481"),
					"count1",
					this.m_bSelectedFloor,
					"count2",
					b
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref strNotButtonNotify, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3556"),
					"count1",
					prevTreasureData.bFloor,
					"count2",
					prevTreasureData.bSubFloor
				});
			}
		}
		bonusItemInfoDlg.SetMsg(new YesDelegate(this.OnClickTreasureGet), b, false, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3425"), strNotButtonNotify);
		if (flag)
		{
			bonusItemInfoDlg.OnClickOk(null);
		}
	}

	public void OnClickTreasureGet(object a_oObject)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewExplorationLimit())
		{
			return;
		}
		sbyte b = (sbyte)a_oObject;
		if (!NrTSingleton<NewExplorationManager>.Instance.CanGetTreasure(this.m_bSelectedFloor, b))
		{
			return;
		}
		GS_NEWEXPLORATION_TREASURE_REQ gS_NEWEXPLORATION_TREASURE_REQ = new GS_NEWEXPLORATION_TREASURE_REQ();
		gS_NEWEXPLORATION_TREASURE_REQ.i8Floor = this.m_bSelectedFloor;
		gS_NEWEXPLORATION_TREASURE_REQ.i8SubFloor = b;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWEXPLORATION_TREASURE_REQ, gS_NEWEXPLORATION_TREASURE_REQ);
	}

	public void OnClickEnd(IUIObject obj)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewExplorationLimit())
		{
			return;
		}
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() == eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_NONE)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("912"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() == eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_END)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("881"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (NrTSingleton<NewExplorationManager>.Instance.DoNotPlay())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("912"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWEXPLORATION_ENDBOX_DLG);
	}

	public void OnClickAutoBattle(IUIObject obj)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNewExplorationLimit())
		{
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWEXPLORATION_AUTOBATTLE_DLG);
	}

	public void OnClickReset(IUIObject obj)
	{
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() == eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_NONE)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("883"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (NrTSingleton<NewExplorationManager>.Instance.GetPlayState() != eNEWEXPLORATION_PLAYSTATE.eNEWEXPLORATION_PLAYSTATE_END)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("913"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWEXPLORATION_RESETBOX_DLG);
	}

	public void OnClickSoulGemShop(IUIObject obj)
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_HEARTS, true);
	}
}
