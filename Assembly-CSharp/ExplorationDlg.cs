using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ExplorationDlg : Form
{
	private NewListBox m_kSolList;

	private DrawTexture m_BG;

	private Label m_lbActivityTime;

	private long m_nMaxActivity;

	private float m_fActivityUpdateTime;

	private Label m_lb_WillNum;

	private long m_nCurrentActivity;

	private Button btWillCharge1;

	private Button btStart;

	private Button btClose;

	private static bool m_bSortSolInfo;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Exploration/Exploration_Main", G_ID.EXPLORATION_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_BG = (base.GetControl("Main_BG") as DrawTexture);
		this.m_BG.SetTextureFromBundle("UI/Exploration/MainBG");
		this.m_kSolList = (base.GetControl("Member_NewList") as NewListBox);
		this.m_kSolList.touchScroll = false;
		this.m_kSolList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSoldierView));
		this.m_lbActivityTime = (base.GetControl("Will_Time_Label") as Label);
		this.m_lb_WillNum = (base.GetControl("Label_WillNum") as Label);
		this.btWillCharge1 = (base.GetControl("Button_WillCharge1") as Button);
		this.btWillCharge1.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickWillCharge));
		this.btStart = (base.GetControl("Start_Btn") as Button);
		this.btStart.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickStart));
		this.btClose = (base.GetControl("Exit_Btn") as Button);
		this.btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickClose));
		this.LoadSolInfo();
		this.CollectSolInfo();
		this.SetSolList();
		base.SetScreenCenter();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "EXPLOERE", "WAIT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), string.Empty, true);
	}

	public void OnClickStart(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		if (instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_EXPLORATION_LV) > kMyCharInfo.GetLevel())
		{
			return;
		}
		if (0L >= kMyCharInfo.m_nActivityPoint && NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("385"));
			return;
		}
		this.SaveSolInfo();
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPLORATION_PLAY_DLG);
		this.Close();
	}

	public override void OnClose()
	{
		if (null != BugFixAudio.PlayOnceRoot)
		{
			int childCount = BugFixAudio.PlayOnceRoot.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = BugFixAudio.PlayOnceRoot.transform.GetChild(i);
				if (child != null)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
		}
	}

	public void OnClickClose(IUIObject obj)
	{
		UIDataManager.MuteSound(false);
		this.Close();
	}

	public void OnClickWillCharge(IUIObject obj)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHARGE_ACTIVITY_MAX);
		if (this.m_nCurrentActivity >= num)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("135"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WILLCHARGE_DLG);
	}

	public void OnWillCharOK(MsgBoxUI a_cthis, object a_oObject)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_MONEY1);
		long num2 = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_WILL_CHARGE_MONEY2);
		long num3 = (long)((float)(num * (long)kMyCharInfo.GetLevel()) + Mathf.Pow((float)num2 / 10000f, (float)kMyCharInfo.GetLevel()));
		num3 -= num3 % 100L;
		if (num3 > kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		SendPacket.GetInstance().SendObject(45);
	}

	private bool IsValidSol(NkSoldierInfo pkSolinfo, bool checkLevel)
	{
		return pkSolinfo != null && pkSolinfo.IsValid() && (!checkLevel || 2 <= pkSolinfo.GetLevel()) && !pkSolinfo.IsInjuryStatus() && pkSolinfo.GetSolPosType() != 2 && pkSolinfo.GetSolPosType() != 3 && pkSolinfo.GetSolPosType() != 4 && pkSolinfo.GetSolPosType() != 5 && pkSolinfo.GetSolPosType() != 6;
	}

	private NkSoldierInfo FindSolInfo(long solID)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null || NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return null;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = soldierList.GetSoldierInfo(i);
			if (soldierInfo != null && soldierInfo.IsValid())
			{
				if (solID == soldierInfo.GetSolID())
				{
					return soldierInfo;
				}
			}
		}
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current != null && current.IsValid())
			{
				if (solID == current.GetSolID())
				{
					return current;
				}
			}
		}
		return null;
	}

	private void LoadSolInfo()
	{
		NrTSingleton<ExplorationManager>.Instance.GetSolInfo().Clear();
		string str = string.Format("ExplorationSoldierID_", new object[0]);
		for (int i = 0; i < 6; i++)
		{
			string @string = PlayerPrefs.GetString(str + i.ToString());
			if (!(@string == string.Empty))
			{
				long num = long.Parse(@string);
				NkSoldierInfo nkSoldierInfo = this.FindSolInfo(num);
				if (this.IsValidSol(nkSoldierInfo, false))
				{
					NrTSingleton<ExplorationManager>.Instance.AddSolInfo(nkSoldierInfo);
					TsLog.Log(string.Format("{0}: SolID: {1}, ExplorationCount: {2}", i, num, NrTSingleton<ExplorationManager>.Instance.GetSolInfo().Count), new object[0]);
				}
			}
		}
	}

	private void SaveSolInfo()
	{
		string str = string.Format("ExplorationSoldierID_", new object[0]);
		for (int i = 0; i < NrTSingleton<ExplorationManager>.Instance.GetSolInfo().Count; i++)
		{
			long num = NrTSingleton<ExplorationManager>.Instance.GetSolID(i);
			PlayerPrefs.SetString(str + i.ToString(), num.ToString());
		}
		for (int j = NrTSingleton<ExplorationManager>.Instance.GetSolInfo().Count; j < 6; j++)
		{
			long num = 0L;
			PlayerPrefs.SetString(str + j.ToString(), num.ToString());
		}
	}

	private void CollectSolInfo()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null || NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (!ExplorationDlg.m_bSortSolInfo)
		{
			for (int i = NrTSingleton<ExplorationManager>.Instance.GetSolInfo().Count; i < 6; i++)
			{
				NkSoldierInfo soldierInfo = soldierList.GetSoldierInfo(i);
				if (this.IsValidSol(soldierInfo, true))
				{
					NrTSingleton<ExplorationManager>.Instance.AddSolInfo(soldierInfo);
				}
			}
			foreach (NkSoldierInfo current in readySolList.GetList().Values)
			{
				if (this.IsValidSol(current, true))
				{
					if (6 <= NrTSingleton<ExplorationManager>.Instance.GetSolInfo().Count)
					{
						break;
					}
					NrTSingleton<ExplorationManager>.Instance.AddSolInfo(current);
				}
			}
			NrTSingleton<ExplorationManager>.Instance.SortSolInfo();
			if (6 < NrTSingleton<ExplorationManager>.Instance.GetSolInfo().Count)
			{
				NrTSingleton<ExplorationManager>.Instance.GetSolInfo().RemoveRange(6, NrTSingleton<ExplorationManager>.Instance.GetSolInfo().Count - 6);
			}
			ExplorationDlg.m_bSortSolInfo = true;
		}
		else
		{
			for (int j = 0; j < NrTSingleton<ExplorationManager>.Instance.GetSolInfo().Count; j++)
			{
				long solID = NrTSingleton<ExplorationManager>.Instance.GetSolID(j);
				NkSoldierInfo pkSolinfo = this.FindSolInfo(solID);
				if (!this.IsValidSol(pkSolinfo, false))
				{
					NrTSingleton<ExplorationManager>.Instance.RemoveSolInfo(solID);
				}
			}
		}
	}

	private void OnClickSoldierView(IUIObject obj)
	{
		NewListBox newListBox = obj as NewListBox;
		if (obj == null || null == newListBox)
		{
			return;
		}
		this.OnSoldierView(newListBox.SelectedItem);
	}

	private void OnSoldierView(IUIListObject obj)
	{
		UIListItemContainer uIListItemContainer = (UIListItemContainer)obj;
		if (null == uIListItemContainer)
		{
			return;
		}
		long num = -1L;
		if (uIListItemContainer.data != null)
		{
			num = (long)uIListItemContainer.data;
		}
		if (num != 0L)
		{
			return;
		}
		SolMilitarySelectDlg solMilitarySelectDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLMILITARYSELECT_DLG) as SolMilitarySelectDlg;
		if (solMilitarySelectDlg != null)
		{
			solMilitarySelectDlg.SetLocationByForm(this);
			solMilitarySelectDlg.SetFocus();
			solMilitarySelectDlg.SolSortType = 2;
			solMilitarySelectDlg.SetSortList();
		}
	}

	public void SetSolList()
	{
		this.m_kSolList.Clear();
		List<NkSoldierInfo> solInfo = NrTSingleton<ExplorationManager>.Instance.GetSolInfo();
		int num = 0;
		for (int i = 0; i < solInfo.Count; i++)
		{
			NkSoldierInfo nkSoldierInfo = solInfo[i];
			if (nkSoldierInfo != null)
			{
				if (num >= 6)
				{
					break;
				}
				if (nkSoldierInfo != null)
				{
					NewListItem item = new NewListItem(this.m_kSolList.ColumnNum, true, string.Empty);
					this.MakeSolListItem(ref item, nkSoldierInfo);
					this.m_kSolList.Add(item);
				}
			}
		}
		for (int j = solInfo.Count; j < 6; j++)
		{
			NewListItem item2 = new NewListItem(this.m_kSolList.ColumnNum, true, string.Empty);
			this.MakeSolListItem(ref item2, null);
			this.m_kSolList.Add(item2);
		}
		this.m_kSolList.RepositionItems();
	}

	private void MakeSolListItem(ref NewListItem item, NkSoldierInfo pkSolinfo)
	{
		string text = string.Empty;
		if (pkSolinfo != null)
		{
			item.SetListItemData(1, pkSolinfo.GetListSolInfo(false), null, null, null);
			item.SetListItemData(2, pkSolinfo.GetName(), null, null, null);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
				"count1",
				pkSolinfo.GetLevel().ToString(),
				"count2",
				pkSolinfo.GetSolMaxLevel().ToString()
			});
			item.SetListItemData(3, text, null, null, null);
			int num = pkSolinfo.GetEquipWeaponOrigin();
			if (num > 0)
			{
				item.SetListItemData(4, string.Format("Win_I_Weapon{0}", num.ToString()), null, null, null);
			}
			num = pkSolinfo.GetEquipWeaponExtention();
			if (num > 0)
			{
				item.SetListItemData(5, string.Format("Win_I_Weapon{0}", num.ToString()), null, null, null);
			}
			else
			{
				item.SetListItemData(5, false);
			}
			long exp = pkSolinfo.GetExp();
			long curBaseExp = pkSolinfo.GetCurBaseExp();
			long nextExp = pkSolinfo.GetNextExp();
			long num2 = exp - curBaseExp;
			long num3 = nextExp - curBaseExp;
			float num4 = pkSolinfo.GetExpPercent();
			if (num4 < 0f)
			{
				num4 = 0f;
			}
			item.SetListItemData(6, "Win_T_ReputelPrgBG", null, null, null);
			item.SetListItemData(7, "Com_T_GauWaPr4", 250f * num4, null, null);
			item.SetListItemData(8, string.Empty, pkSolinfo.GetSolID(), new EZValueChangedDelegate(this.OnClickSoldierDelete), null);
			if (pkSolinfo.IsMaxLevel())
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
					"exp",
					num2.ToString(),
					"maxexp",
					num3.ToString()
				});
			}
			item.SetListItemData(9, text, null, null, null);
			item.SetListItemData(10, false);
			item.Data = pkSolinfo.GetSolID();
		}
		else
		{
			for (int i = 0; i < this.m_kSolList.ColumnNum; i++)
			{
				item.SetListItemData(i, false);
			}
			item.SetListItemData(10, true);
			item.SetListItemData(10, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("859"), null, null, null);
			item.Data = 0L;
		}
	}

	public void OnClickSoldierDelete(IUIObject obj)
	{
		UIButton uIButton = (UIButton)obj;
		if (null == uIButton || uIButton.Data == null)
		{
			return;
		}
		long num = (long)uIButton.Data;
		if (num == 0L)
		{
			return;
		}
		NrTSingleton<ExplorationManager>.Instance.RemoveSolInfo(num);
		this.SetSolList();
	}

	public override void Update()
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (this.m_nCurrentActivity != kMyCharInfo.m_nActivityPoint || this.m_nMaxActivity != kMyCharInfo.m_nMaxActivityPoint)
		{
			this.m_nCurrentActivity = kMyCharInfo.m_nActivityPoint;
			this.m_nMaxActivity = kMyCharInfo.m_nMaxActivityPoint;
			this.SetActivityPointUI();
		}
		if (this.m_fActivityUpdateTime < Time.realtimeSinceStartup)
		{
			MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
			if (myCharInfoDlg == null)
			{
				return;
			}
			this.m_lbActivityTime.SetText(myCharInfoDlg.StrActivityTime);
			this.m_fActivityUpdateTime = Time.realtimeSinceStartup + 1f;
			this.SetActivityPointUI();
		}
	}

	public void SetActivityPointUI()
	{
		MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
		if (myCharInfoDlg == null)
		{
			return;
		}
		string empty = string.Empty;
		if (myCharInfoDlg.CurrentActivity > myCharInfoDlg.MaxActivity)
		{
			string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1304");
			string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2791"),
				"CurrentNum",
				textColor + myCharInfoDlg.CurrentActivity.ToString() + textColor2,
				"MaxNum",
				myCharInfoDlg.MaxActivity
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2791"),
				"CurrentNum",
				myCharInfoDlg.CurrentActivity,
				"MaxNum",
				myCharInfoDlg.MaxActivity
			});
		}
		this.m_lb_WillNum.SetText(empty);
	}
}
