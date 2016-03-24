using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityForms;

public class InitiativeSetDlg : Form
{
	private NewListBox InitiativeNewListBox;

	private Button btnOk;

	private List<NkSoldierInfo> m_SoldierInfoSortList = new List<NkSoldierInfo>();

	private List<int> m_SolInitiativeList = new List<int>();

	private List<short> m_SolOnlySkillList = new List<short>();

	private List<float> m_oldHSInittiativeValue = new List<float>();

	private int CompareInitiative_High(NkSoldierInfo a, NkSoldierInfo b)
	{
		return this.COMPARE_INITIATIVE(a, b);
	}

	private int CompareInitiative_HighNew(NkSoldierInfo a, NkSoldierInfo b)
	{
		return this.COMPARE_INITIATIVE_NEW(a, b);
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "BabelTower/DLG_BattleSkill_Initiative", G_ID.INITIATIVE_SET_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.InitiativeNewListBox = (base.GetControl("NLB_Initiative_List") as NewListBox);
		this.btnOk = (base.GetControl("BT_OK") as Button);
		Button expr_32 = this.btnOk;
		expr_32.Click = (EZValueChangedDelegate)Delegate.Combine(expr_32.Click, new EZValueChangedDelegate(this.OnClickInitiativeAllSend));
		this.InitData();
		base.SetScreenCenter();
	}

	public override void InitData()
	{
	}

	public void SetBatchSolList(eBATTLE_ROOMTYPE eBattleRoomtype)
	{
		if (eBattleRoomtype <= eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NONE)
		{
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		if (eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER)
		{
			for (int i = 0; i < SoldierBatch.SOLDIERBATCH.GetBabelTowerTotalBatchInfoCount(); i++)
			{
				long num = SoldierBatch.SOLDIERBATCH.GetBabelTowerSolIDFromIndex(i);
				if (num > 0L)
				{
					NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(num);
					if (soldierInfoFromSolID != null)
					{
						this.m_SoldierInfoSortList.Add(soldierInfoFromSolID);
					}
				}
			}
		}
		else if (eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE)
		{
			for (int i = 0; i < 5; i++)
			{
				long num = SoldierBatch.SOLDIERBATCH.GetTempBattleSolID(i);
				if (num > 0L)
				{
					NkSoldierInfo soldierInfoFromSolID2 = charPersonInfo.GetSoldierInfoFromSolID(num);
					if (soldierInfoFromSolID2 != null)
					{
						this.m_SoldierInfoSortList.Add(soldierInfoFromSolID2);
					}
				}
			}
		}
		else if (eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS)
		{
			for (int i = 0; i < 9; i++)
			{
				long num = SoldierBatch.SOLDIERBATCH.GetTempBattleSolID(i);
				if (num > 0L)
				{
					NkSoldierInfo soldierInfoFromSolID3 = charPersonInfo.GetSoldierInfoFromSolID(num);
					if (soldierInfoFromSolID3 != null)
					{
						this.m_SoldierInfoSortList.Add(soldierInfoFromSolID3);
					}
				}
			}
		}
		else if (eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_DAILYDUNGEON)
		{
			for (int i = 0; i < 6; i++)
			{
				long num = SoldierBatch.SOLDIERBATCH.GetTempBattleSolID(i);
				if (num > 0L)
				{
					NkSoldierInfo soldierInfoFromSolID4 = charPersonInfo.GetSoldierInfoFromSolID(num);
					if (soldierInfoFromSolID4 != null)
					{
						this.m_SoldierInfoSortList.Add(soldierInfoFromSolID4);
					}
				}
			}
		}
		else if (eBattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
		{
			for (int i = 0; i < 5; i++)
			{
				long num = SoldierBatch.SOLDIERBATCH.GetTempBattleSolID(i);
				if (num > 0L)
				{
					NkSoldierInfo soldierInfoFromSolID5 = charPersonInfo.GetSoldierInfoFromSolID(num);
					if (soldierInfoFromSolID5 != null)
					{
						this.m_SoldierInfoSortList.Add(soldierInfoFromSolID5);
					}
				}
			}
		}
		if (this.m_SoldierInfoSortList.Count > 0)
		{
			this.SoltInitiativeBatch(true);
			this.SetList();
		}
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "INFORMATION", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.SendAllInitiative();
	}

	private void SendAllInitiative()
	{
		bool flag = false;
		GS_SET_SOLDIER_INITIATIVE_AND_ONLYSKILL_REQ gS_SET_SOLDIER_INITIATIVE_AND_ONLYSKILL_REQ = new GS_SET_SOLDIER_INITIATIVE_AND_ONLYSKILL_REQ();
		for (int i = 0; i < this.m_SoldierInfoSortList.Count; i++)
		{
			if (i >= 16)
			{
				break;
			}
			NkSoldierInfo nkSoldierInfo = this.m_SoldierInfoSortList[i];
			if (nkSoldierInfo != null && nkSoldierInfo.IsValid())
			{
				gS_SET_SOLDIER_INITIATIVE_AND_ONLYSKILL_REQ.nSolID[i] = nkSoldierInfo.GetSolID();
				gS_SET_SOLDIER_INITIATIVE_AND_ONLYSKILL_REQ.nInitiativeValue[i] = this.m_SolInitiativeList[i];
				gS_SET_SOLDIER_INITIATIVE_AND_ONLYSKILL_REQ.bOnlySkill[i] = this.m_SolOnlySkillList[i];
				flag = true;
			}
		}
		if (flag)
		{
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SET_SOLDIER_INITIATIVE_AND_ONLYSKILL_REQ, gS_SET_SOLDIER_INITIATIVE_AND_ONLYSKILL_REQ);
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("742"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	private int COMPARE_INITIATIVE(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetInitiativeValue().CompareTo(a.GetInitiativeValue());
	}

	private int COMPARE_INITIATIVE_NEW(NkSoldierInfo a, NkSoldierInfo b)
	{
		int index = this.m_SoldierInfoSortList.FindIndex((NkSoldierInfo value) => value.GetSolID() == a.GetSolID());
		int index2 = this.m_SoldierInfoSortList.FindIndex((NkSoldierInfo value) => value.GetSolID() == b.GetSolID());
		return this.m_SolInitiativeList[index].CompareTo(this.m_SolInitiativeList[index2]);
	}

	private void SortSoldierInfoSortList()
	{
		for (int i = 0; i < this.m_SoldierInfoSortList.Count; i++)
		{
			for (int j = i + 1; j < this.m_SoldierInfoSortList.Count; j++)
			{
				int initiativeValue = this.m_SoldierInfoSortList[i].GetInitiativeValue();
				int initiativeValue2 = this.m_SoldierInfoSortList[j].GetInitiativeValue();
				if (initiativeValue < initiativeValue2)
				{
					NkSoldierInfo value = this.m_SoldierInfoSortList[i];
					this.m_SoldierInfoSortList[i] = this.m_SoldierInfoSortList[j];
					this.m_SoldierInfoSortList[j] = value;
				}
			}
		}
	}

	public void SoltInitiativeBatch(bool OpenFirst)
	{
		if (this.m_SoldierInfoSortList.Count <= 0)
		{
			return;
		}
		if (OpenFirst)
		{
			this.m_SoldierInfoSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareInitiative_High));
			for (int i = 0; i < this.m_SoldierInfoSortList.Count; i++)
			{
				NkSoldierInfo nkSoldierInfo = this.m_SoldierInfoSortList[i];
				if (nkSoldierInfo != null)
				{
					this.m_SolInitiativeList.Add(nkSoldierInfo.GetInitiativeValue());
					short item = 0;
					if (nkSoldierInfo.IsAtbCommonFlag(8L))
					{
						item = 1;
					}
					this.m_SolOnlySkillList.Add(item);
				}
			}
		}
		else
		{
			this.m_SoldierInfoSortList.Sort(new Comparison<NkSoldierInfo>(this.CompareInitiative_HighNew));
		}
	}

	public void SetList()
	{
		if (this.m_SoldierInfoSortList.Count > 0)
		{
			NkSoldierInfo lastSolInfo = null;
			int num = 1;
			this.InitiativeNewListBox.Clear();
			for (int i = 0; i < this.m_SoldierInfoSortList.Count; i++)
			{
				NewListItem item = new NewListItem(this.InitiativeNewListBox.ColumnNum, true, string.Empty);
				if (i > 0)
				{
					lastSolInfo = this.m_SoldierInfoSortList[i - 1];
				}
				this.SetSolInitiativeColum(lastSolInfo, this.m_SoldierInfoSortList[i], i, ref item, ref num);
				this.InitiativeNewListBox.Add(item);
			}
			this.InitiativeNewListBox.RepositionItems();
		}
	}

	private void SetSolInitiativeColum(NkSoldierInfo LastSolInfo, NkSoldierInfo NowSolInfo, int index, ref NewListItem item, ref int ShowIndex)
	{
		string empty = string.Empty;
		if (LastSolInfo != null && LastSolInfo.GetInitiativeValue() != NowSolInfo.GetInitiativeValue())
		{
			ShowIndex = index + 1;
		}
		EVENT_HERODATA eventHeroCheck = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCheck(NowSolInfo.GetCharKind(), NowSolInfo.GetGrade());
		if (eventHeroCheck != null)
		{
			item.SetListItemData(1, "Win_I_EventSol", null, null, null);
		}
		else
		{
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(NowSolInfo.GetCharKind(), (int)NowSolInfo.GetGrade());
			if (legendFrame != null)
			{
				item.SetListItemData(1, legendFrame, null, null, null);
			}
			else
			{
				item.SetListItemData(1, "Win_I_Cancel", null, null, null);
			}
		}
		item.SetListItemData(5, ShowIndex.ToString(), NowSolInfo.GetInitiativeValue(), null, null);
		item.SetListItemData(4, NowSolInfo.GetListSolInfo(true), NowSolInfo, null, null);
		item.SetListItemData(2, NowSolInfo.GetName(), null, null, null);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
			"count1",
			NowSolInfo.GetLevel().ToString(),
			"count2",
			NowSolInfo.GetSolMaxLevel().ToString()
		});
		item.SetListItemData(3, empty, null, null, null);
		item.SetListItemData(8, string.Empty, NowSolInfo.GetSolID(), new EZValueChangedDelegate(this.OnClickInitiativeDown), null);
		item.SetListItemData(9, string.Empty, NowSolInfo.GetSolID(), new EZValueChangedDelegate(this.OnClickInitiativeUP), null);
		item.SetListItemData(11, NowSolInfo.GetInitiativeValue().ToString(), null, null, null);
		float num = 0f;
		if (NowSolInfo.GetInitiativeValue() >= 0)
		{
			num = (float)NowSolInfo.GetInitiativeValue() / 100f;
			this.m_oldHSInittiativeValue.Add(num);
		}
		item.SetFloatListItemData(12, num, index, new EZValueChangedDelegate(this.OnMoveSlider), null);
		int charKind = 0;
		if (NowSolInfo.IsAtbCommonFlag(8L))
		{
			charKind = 1;
		}
		item.SetListItemData(13, charKind, index, new EZValueChangedDelegate(this.ClickOnlySkillCheckBox), null);
	}

	private void ClickOnlySkillCheckBox(IUIObject obj)
	{
		CheckBox checkBox = obj as CheckBox;
		if (checkBox == null)
		{
			return;
		}
		int index = 0;
		if (checkBox.Data != null)
		{
			index = (int)checkBox.Data;
		}
		if (checkBox.IsChecked())
		{
			this.m_SolOnlySkillList[index] = 1;
		}
		else
		{
			this.m_SolOnlySkillList[index] = 0;
		}
	}

	private void OnMoveSlider(IUIObject obj)
	{
		HorizontalSlider horizontalSlider = obj as HorizontalSlider;
		if (horizontalSlider == null)
		{
			return;
		}
		int num = -1;
		if (obj.Data != null)
		{
			num = (int)obj.Data;
		}
		if (num > -1 && num < this.m_SoldierInfoSortList.Count && this.m_oldHSInittiativeValue[num] != horizontalSlider.Value)
		{
			int num2 = (int)(horizontalSlider.Value * 100f);
			if (num2 != this.m_SolInitiativeList[num])
			{
				this.m_SolInitiativeList[num] = num2;
				UIListItemContainer item = this.InitiativeNewListBox.GetItem(num);
				if (item != null)
				{
					Label label = item.GetElement(11) as Label;
					if (label != null)
					{
						label.SetText(this.m_SolInitiativeList[num].ToString());
					}
				}
			}
		}
	}

	private void OnClickInitiativeAllSend(IUIObject obj)
	{
		UIButton x = obj as UIButton;
		if (x == null)
		{
			return;
		}
		this.Close();
	}

	private void OnClickInitiativeUP(IUIObject obj)
	{
		UIButton uIButton = obj as UIButton;
		if (uIButton == null)
		{
			return;
		}
		long num = (long)uIButton.Data;
		if (0L < num)
		{
			this.CountSolInitiativeValue(num, 1);
		}
	}

	private void OnClickInitiativeDown(IUIObject obj)
	{
		UIButton uIButton = obj as UIButton;
		if (uIButton == null)
		{
			return;
		}
		long num = (long)uIButton.Data;
		if (0L < num)
		{
			this.CountSolInitiativeValue(num, -1);
		}
	}

	private void CountSolInitiativeValue(long SolID, int AddValue)
	{
		if (0L < SolID)
		{
			int index = this.m_SoldierInfoSortList.FindIndex((NkSoldierInfo value) => value.GetSolID() == SolID);
			int num = this.m_SolInitiativeList[index] + AddValue;
			if (num < 0)
			{
				num = 0;
			}
			else if (num >= 100)
			{
				num = 100;
			}
			if (num != this.m_SolInitiativeList[index])
			{
				this.m_SolInitiativeList[index] = num;
				UIListItemContainer selectItem = this.InitiativeNewListBox.GetSelectItem();
				if (selectItem != null)
				{
					Label label = selectItem.GetElement(11) as Label;
					if (label != null)
					{
						label.SetText(num.ToString());
					}
					float num2 = (float)num / 100f;
					HorizontalSlider horizontalSlider = selectItem.GetElement(12) as HorizontalSlider;
					horizontalSlider.CallChangeDelegate = false;
					horizontalSlider.defaultValue = num2;
					horizontalSlider.Value = num2;
					this.m_oldHSInittiativeValue[index] = num2;
				}
			}
		}
	}
}
