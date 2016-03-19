using ConsoleCommand.Statistics;
using GAME;
using Global;
using Ndoors.Framework.Stage;
using PROTOCOL;
using System;
using UnityEngine;
using UnityForms;

public class Main : NrBehaviour
{
	public int nSceneStatus;

	public bool m_ResolutionWindow;

	private bool m_bSwitchMyCharInfoGUI;

	private Form m_kForm;

	public override bool Initialize()
	{
		Client.GetInstance();
		return true;
	}

	public void ChangeScene()
	{
		if (Scene.CurScene == Scene.Type.WORLD)
		{
			if (NrTSingleton<NkQuestManager>.Instance.IsCompletedFirstQuest())
			{
				BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
				if (bookmarkDlg != null)
				{
					bookmarkDlg.Show();
					bookmarkDlg.CheckHideBookmark();
				}
				this.m_kForm = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MENUICON_DLG);
				if (this.m_kForm != null)
				{
					this.m_kForm.Show();
				}
				NoticeIconDlg noticeIconDlg = (NoticeIconDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_UI_ICON);
				if (noticeIconDlg != null)
				{
					noticeIconDlg.ShowTempNotice();
					noticeIconDlg.Show();
				}
				this.m_kForm = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYCHARINFO_DLG);
				if (this.m_kForm != null)
				{
					this.m_kForm.Show();
				}
				if (NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType() == 6)
				{
					this.m_kForm = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GOOGLEPLAY_DLG);
					if (this.m_kForm != null)
					{
						this.m_kForm.Show();
					}
				}
				else if (StageLoginMobile.m_bConnectGameCenter)
				{
					this.m_kForm = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GOOGLEPLAY_DLG);
					if (this.m_kForm != null)
					{
						this.m_kForm.Show();
					}
				}
				this.m_kForm = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHAT_MAIN_DLG);
				if (this.m_kForm != null)
				{
					this.m_kForm.Show();
				}
				if (NrTSingleton<MapManager>.Instance.GetMapNameAndOST() != string.Empty)
				{
					ChatNoticeDlg chatNoticeDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHAT_NOTICE_DLG) as ChatNoticeDlg;
					if (chatNoticeDlg != null)
					{
						chatNoticeDlg.AddText(NrTSingleton<MapManager>.Instance.GetMapNameAndOST());
					}
				}
				NrTSingleton<WhisperManager>.Instance.ShowWhisperDlg();
				if (TsPlatform.IsMobile)
				{
					this.m_kForm = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.JOYSTICK_DLG);
					if (this.m_kForm != null)
					{
						this.m_kForm.Show();
					}
				}
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo.ColosseumMatching)
				{
					this.m_kForm = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMNOTICE_DLG);
					if (this.m_kForm != null)
					{
						this.m_kForm.Show();
					}
				}
				RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_QUEST);
				if (rightMenuQuestUI != null)
				{
					rightMenuQuestUI.QuestUpdate();
					rightMenuQuestUI.Show();
				}
				NrTSingleton<NkQuestManager>.Instance.ShowMsg();
				if (StageWorld.BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_MAKEUP || StageWorld.BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENCE_MAKEUP || StageWorld.BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || StageWorld.BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP)
				{
					if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERMAIN_DLG))
					{
						PlunderMainDlg plunderMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERMAIN_DLG) as PlunderMainDlg;
						if (plunderMainDlg != null)
						{
							if (StageWorld.BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_ATTACK_INFIBATTLE_MAKEUP || StageWorld.BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_DEFENSE_INFIBATTLE_MAKEUP)
							{
								plunderMainDlg.SetMode(eMODE.eMODE_INFIBATTLE);
								plunderMainDlg.SetTgValue(eMODE.eMODE_INFIBATTLE);
							}
							plunderMainDlg.Show();
						}
					}
				}
				else if (StageWorld.BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP || StageWorld.BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
				{
					if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COLOSSEUMMAIN_DLG))
					{
						ColosseumDlg colosseumDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMMAIN_DLG) as ColosseumDlg;
						if (colosseumDlg != null)
						{
							colosseumDlg.Show();
						}
					}
				}
				else if (StageWorld.BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
				{
					BabelGuildBossDlg babelGuildBossDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABEL_GUILDBOSS_MAIN_DLG) as BabelGuildBossDlg;
					if (babelGuildBossDlg != null)
					{
						babelGuildBossDlg.ShowList();
					}
				}
				else if (StageWorld.BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_EXPEDITION_MAKEUP)
				{
				}
				StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_MAX;
				if (StageWorld.MINEMSG_TYPE == eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_SUCCESS)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("328"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else if (StageWorld.MINEMSG_TYPE == eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_FAIL01)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("330"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else if (StageWorld.MINEMSG_TYPE == eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_FAIL03)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("330"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else if (StageWorld.MINEMSG_TYPE == eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_FAIL02)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("327"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else if (StageWorld.MINEMSG_TYPE == eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_FAIL04)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("429"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				if (StageWorld.PLUNDERMSG_TYPE == ePLUNDER_MESSAGE.ePLUNDER_MESSAGE_MATCH_FAIL)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("120"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					PlunderMainDlg plunderMainDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERMAIN_DLG) as PlunderMainDlg;
					if (plunderMainDlg2 != null)
					{
						plunderMainDlg2.Show();
					}
				}
				StageWorld.PLUNDERMSG_TYPE = ePLUNDER_MESSAGE.ePLUNDER_MESSAGE_DEFAULT;
				StageWorld.MINEMSG_TYPE = eMINE_MESSAGE.eMINE_MESSAGE_DEFAULT;
			}
		}
		else if (Scene.CurScene == Scene.Type.BATTLE || Scene.CurScene == Scene.Type.SOLDIER_BATCH)
		{
			this.m_kForm = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GAMEGUIDE_DLG);
			if (this.m_kForm != null)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.GAMEGUIDE_DLG);
			}
			this.m_kForm = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG);
			if (this.m_kForm != null)
			{
				this.m_kForm.Hide();
			}
			this.m_kForm = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_CHARINFO);
			if (this.m_kForm != null)
			{
				this.m_kForm.Hide();
			}
			this.m_kForm = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG);
			if (this.m_kForm != null)
			{
				this.m_kForm.Hide();
			}
			if (NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType() == 6)
			{
				this.m_kForm = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GOOGLEPLAY_DLG);
				if (this.m_kForm != null)
				{
					this.m_kForm.Hide();
				}
			}
			else if (StageLoginMobile.m_bConnectGameCenter)
			{
				this.m_kForm = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GOOGLEPLAY_DLG);
				if (this.m_kForm != null)
				{
					this.m_kForm.Hide();
				}
			}
			this.m_kForm = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_QUEST);
			if (this.m_kForm != null)
			{
				this.m_kForm.Hide();
			}
			this.m_kForm = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MENUICON_DLG);
			if (this.m_kForm != null)
			{
				this.m_kForm.Hide();
			}
			NoticeIconDlg noticeIconDlg2 = (NoticeIconDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_UI_ICON);
			if (noticeIconDlg2 != null)
			{
				noticeIconDlg2.ShowTempNotice();
				noticeIconDlg2.Show();
			}
			this.m_kForm = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
			if (this.m_kForm != null)
			{
				this.m_kForm.Hide();
			}
		}
		CustomQuality.GetInstance().ChangeCameraClipPlane(TsQualityManager.Instance.CurrLevel);
	}

	public override void Update()
	{
		NrTSingleton<NrUpdateProcessor>.Instance.MainUpdate();
		if (NkInputManager.GetKeyDown(KeyCode.Escape))
		{
			this.m_ResolutionWindow = !this.m_ResolutionWindow;
		}
		if (NkInputManager.GetKey(KeyCode.LeftShift) && NkInputManager.GetKeyUp(KeyCode.Alpha3))
		{
			TsLog.LogError("GetMonoHeapSize = {0},  GetMonoUsedSize = {1} , usedHeapSize = {2} ", new object[]
			{
				Profiler.GetMonoHeapSize(),
				Profiler.GetMonoUsedSize(),
				Profiler.usedHeapSize
			});
			MemoryCollection.Print(MemoryCollection.Mode.LoadedAll);
		}
		if (NkInputManager.GetKey(KeyCode.LeftShift) && NkInputManager.GetKeyUp(KeyCode.Alpha2))
		{
			NrTSingleton<NrMainSystem>.Instance.CleanUpImmediate();
		}
		if (NkInputManager.GetKey(KeyCode.LeftShift) && NkInputManager.GetKeyUp(KeyCode.Alpha4))
		{
			NrTSingleton<FormsManager>.Instance.DeleteAll();
		}
		if (!TsPlatform.IsEditor || !NkInputManager.GetKey(KeyCode.LeftShift) || NkInputManager.GetKeyUp(KeyCode.Alpha0))
		{
		}
		if (NkInputManager.GetKey(KeyCode.LeftShift) && NkInputManager.GetKeyUp(KeyCode.Alpha5))
		{
			TsPlatform.FileLog("AppMemory : " + NrTSingleton<NrMainSystem>.Instance.AppMemory);
		}
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor && Input.GetKeyUp(KeyCode.Escape))
		{
			bool flag = NrTSingleton<FormsManager>.Instance.CloseFormESC();
			if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.TOASTMSG_DLG))
			{
				if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOASTMSG_DLG) == null)
				{
					MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
					if (msgBoxUI != null)
					{
						msgBoxUI.SetMsg(new YesDelegate(this.EscQuitGame), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("7"), NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("38"), eMsgType.MB_OK_CANCEL);
					}
				}
			}
			else if (!flag && Scene.CurScene != Scene.Type.SELECTCHAR)
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TOASTMSG_DLG);
				MsgBoxUI msgBoxUI2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI2 != null)
				{
					msgBoxUI2.SetMsg(new YesDelegate(this.EscQuitGame), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("7"), NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("38"), eMsgType.MB_OK_CANCEL);
				}
				NrTSingleton<FiveRocksEventManager>.Instance.Placement("backbutton_click");
			}
		}
		if (NkInputManager.GetKeyDown(KeyCode.Alpha1) && NkInputManager.GetKey(KeyCode.RightShift))
		{
			this.m_bSwitchMyCharInfoGUI = !this.m_bSwitchMyCharInfoGUI;
		}
	}

	public void EscQuitGame(object obj)
	{
		NrTSingleton<NrMainSystem>.Instance.EscQuitGame();
	}

	public void OnApplicationQuit()
	{
		BaseNet_Game.GetInstance().Quit();
	}

	public void MsgBoxOKQuitGame(object a_oObject)
	{
		NrTSingleton<NrMainSystem>.Instance.QuitGame();
	}
}
