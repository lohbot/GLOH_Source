using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class Myth_Evolution_Check_DLG : Form
{
	private Label m_Label_EvolutionCheck;

	private Button m_BTN_OK;

	private Button m_BTN_Cancel;

	private Button m_Button_Exit;

	private long m_i64SolID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Soldier/Evolution/DLG_SolEvolutionCheck", G_ID.MYTH_EVOLUTION_CHECK_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_Label_EvolutionCheck = (base.GetControl("Label_EvolutionCheck") as Label);
		this.m_BTN_OK = (base.GetControl("BTN_OK") as Button);
		this.m_BTN_OK.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythEvolution_OK));
		this.m_BTN_Cancel = (base.GetControl("BTN_Cancel") as Button);
		this.m_BTN_Cancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythEvolution_Cancel));
		this.m_Button_Exit = (base.GetControl("Button_Exit") as Button);
		this.m_Button_Exit.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythEvolution_Cancel));
		this.m_i64SolID = 0L;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	public void SetMythEvolutionOK(long i64SolID)
	{
		this.m_i64SolID = i64SolID;
		NkSoldierInfo soldierInfo = this.GetSoldierInfo(this.m_i64SolID);
		if (soldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		int num = soldierInfo.GetSeason() + 1;
		if (num < 0)
		{
			return;
		}
		MYTH_EVOLUTION myth_EvolutionSeason = NrTSingleton<NrTableMyth_EvolutionManager>.Instance.GetMyth_EvolutionSeason((byte)num);
		if (myth_EvolutionSeason != null)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3452"),
				"itemnum1",
				myth_EvolutionSeason.m_i32SpendItemNum1,
				"itemnum2",
				myth_EvolutionSeason.m_i32SpendItemNum2
			});
			this.m_Label_EvolutionCheck.SetText(empty);
		}
	}

	private void OnClickMythEvolution_OK(IUIObject obj)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo(this.m_i64SolID);
		if (soldierInfo == null)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_MYTH_EVOLUTION_TIME);
			long curTime = PublicMethod.GetCurTime();
			if (curTime > charSubData)
			{
				MYTH_EVOLUTION myth_EvolutionSeason = NrTSingleton<NrTableMyth_EvolutionManager>.Instance.GetMyth_EvolutionSeason((byte)(soldierInfo.GetSeason() + 1));
				if (myth_EvolutionSeason != null)
				{
					string empty = string.Empty;
					if (NkUserInventory.GetInstance().Get_First_ItemCnt(myth_EvolutionSeason.m_i32SpendItemUnique1) < myth_EvolutionSeason.m_i32SpendItemNum1)
					{
						string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(myth_EvolutionSeason.m_i32SpendItemUnique1);
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("780"),
							"target",
							itemNameByItemUnique
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
						return;
					}
					if (NkUserInventory.GetInstance().Get_First_ItemCnt(myth_EvolutionSeason.m_i32SpendItemUnique2) < myth_EvolutionSeason.m_i32SpendItemNum2)
					{
						string itemNameByItemUnique2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(myth_EvolutionSeason.m_i32SpendItemUnique2);
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("780"),
							"target",
							itemNameByItemUnique2
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
						return;
					}
					if (soldierInfo.GetGrade() >= 6 && soldierInfo.GetGrade() < 10)
					{
						GS_MYTH_EVOLUTION_SOL_REQ gS_MYTH_EVOLUTION_SOL_REQ = new GS_MYTH_EVOLUTION_SOL_REQ();
						gS_MYTH_EVOLUTION_SOL_REQ.i64SolID = this.m_i64SolID;
						SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTH_EVOLUTION_SOL_REQ, gS_MYTH_EVOLUTION_SOL_REQ);
						this.Close();
					}
				}
			}
		}
	}

	private void OnClickMythEvolution_Cancel(IUIObject obj)
	{
		this.Close();
	}

	public NkSoldierInfo GetSoldierInfo(long SoldID)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		NrSoldierList soldierList = charPersonInfo.GetSoldierList();
		NkSoldierInfo nkSoldierInfo = soldierList.GetSoldierInfoBySolID(SoldID);
		if (nkSoldierInfo == null)
		{
			nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySoldierInfoBySolID(SoldID);
		}
		return nkSoldierInfo;
	}
}
