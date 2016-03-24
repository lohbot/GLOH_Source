using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class DailyDungeon_Start_Dlg : Form
{
	private Button m_btnDailyDungeonStart;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "DailyDungeon/dlg_start_button", G_ID.DAILYDUNGEON_START_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_btnDailyDungeonStart = (base.GetControl("Button_Start") as Button);
		this.m_btnDailyDungeonStart.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_DailyDungeonStart));
		this.m_btnDailyDungeonStart.Visible = false;
		this.Set_Location();
	}

	private void Set_Location()
	{
		float x = GUICamera.width / 2f - base.GetSize().x / 2f;
		float y = GUICamera.height - base.GetSize().y;
		base.SetLocation(x, y, base.GetLocation().z);
	}

	public void Set_Button(eSOLDIER_BATCH_MODE eBatchMode)
	{
		if (eBatchMode == eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON)
		{
			this.m_btnDailyDungeonStart.Visible = true;
		}
	}

	private void Click_DailyDungeonStart(IUIObject Obj)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return;
		}
		if (nrCharUser.GetPersonInfoUser() == null)
		{
			return;
		}
		if (!SoldierBatch.SOLDIERBATCH.IsHeroDailyDungeonBatch())
		{
			string empty = string.Empty;
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("124"),
				"charname",
				@char.GetCharName()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		int tempCount = SoldierBatch.SOLDIERBATCH.GetTempCount();
		int num = 0;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON)
		{
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
			{
				return;
			}
			num = 6;
		}
		if (tempCount < num)
		{
			this.ShowMessageBox_NotEnough_SolNumBatch(new YesDelegate(this.OnCompleteBatch_DailyDungeon), tempCount, num);
			return;
		}
		this.OnCompleteBatch_DailyDungeon(null);
	}

	private void OnCompleteBatch_DailyDungeon(object a_oObject)
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DAILYDUNGEON)
		{
			SoldierBatch.SOLDIERBATCH.Save_DailyDungeonBatchSolInfo();
		}
		clTempBattlePos[] tempBattlePosInfo = SoldierBatch.SOLDIERBATCH.GetTempBattlePosInfo();
		GS_CHARACTER_DAILYDUNGEON_SET_REQ gS_CHARACTER_DAILYDUNGEON_SET_REQ = new GS_CHARACTER_DAILYDUNGEON_SET_REQ();
		gS_CHARACTER_DAILYDUNGEON_SET_REQ.i8Diff = SoldierBatch.DailyDungeonDifficulty;
		gS_CHARACTER_DAILYDUNGEON_SET_REQ.i32DayOfWeek = (int)NrTSingleton<DailyDungeonManager>.Instance.GetDayOfWeek();
		gS_CHARACTER_DAILYDUNGEON_SET_REQ.i8IsReset = 0;
		int num = 0;
		for (int i = 0; i < 9; i++)
		{
			if (tempBattlePosInfo[i].m_nSolID > 0L)
			{
				gS_CHARACTER_DAILYDUNGEON_SET_REQ.clSolBatchPosInfo[num].SolID = tempBattlePosInfo[i].m_nSolID;
				byte b = 0;
				byte nBattlePos = 0;
				SoldierBatch.GetCalcBattlePos((long)tempBattlePosInfo[i].m_nBattlePos, ref b, ref nBattlePos);
				gS_CHARACTER_DAILYDUNGEON_SET_REQ.clSolBatchPosInfo[num].nBattlePos = nBattlePos;
				num++;
			}
		}
		gS_CHARACTER_DAILYDUNGEON_SET_REQ.nCombinationUnique = NrTSingleton<SolCombination_BatchSelectInfoManager>.Instance.GetUserSelectedUniqeKey(0);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHARACTER_DAILYDUNGEON_SET_REQ, gS_CHARACTER_DAILYDUNGEON_SET_REQ);
	}

	public void ShowMessageBox_NotEnough_SolNumBatch(YesDelegate a_deYes, int nCurrentSolArray, int nTotalSolArray)
	{
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
		string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("147");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromMessageBox2,
			"currentnum",
			nCurrentSolArray,
			"maxnum",
			nTotalSolArray
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(a_deYes, null, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL, 2);
	}
}
