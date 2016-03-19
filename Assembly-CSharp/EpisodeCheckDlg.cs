using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections;
using TsBundle;
using UnityEngine;
using UnityForms;

public class EpisodeCheckDlg : Form
{
	private Label m_EpisodeNum;

	private Label m_EpisodeName;

	private Label m_EpisodeSummary;

	private DrawTexture m_BackImage;

	private DrawTexture m_Image;

	private Button m_Start;

	private Adventure.AdventureInfo m_CurrentAdventureInfo;

	private CQuest m_CurrentQuest;

	private int m_nAutoMoveMapIndex;

	private int m_nAutoMoveGateIndex;

	private int m_nWinID;

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.depthChangeable = false;
		}
		this.m_nWinID = winID;
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 1f);
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide == null)
		{
			return;
		}
		Vector2 x = new Vector2(base.GetLocationX() + this.m_Start.GetLocationX() + 60f, base.GetLocationY() + this.m_Start.GetLocationY());
		Vector2 x2 = new Vector2(base.GetLocationX() + this.m_Start.GetLocationX() + this.m_Start.GetSize().x + 30f, base.GetLocationY() + this.m_Start.GetLocationY() + 35f);
		uI_UIGuide.Move(x, x2);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.Visible = false;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestList/DLG_Episode_Check", G_ID.EPISODECHECK_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_EpisodeNum = (base.GetControl("Label_EpisodeNum") as Label);
		this.m_EpisodeName = (base.GetControl("Label_EpisodeName") as Label);
		this.m_EpisodeSummary = (base.GetControl("Label_Summary") as Label);
		this.m_BackImage = (base.GetControl("DrawTexture_NPCFaceBG") as DrawTexture);
		this.m_Image = (base.GetControl("DrawTexture_NPCFace") as DrawTexture);
		this.m_Start = (base.GetControl("Button_StartBTN") as Button);
		this.m_Start.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickStart));
		base.SetScreenCenter();
		this.m_BackImage.SetTextureFromBundle("UI/Adventure/EpisodeBG");
	}

	private void ClickStart(IUIObject obj)
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char == null)
		{
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		NrTSingleton<NrAutoPath>.Instance.ResetData();
		Vector3 lhs = Vector3.zero;
		Vector2 zero = Vector2.zero;
		if (@char.m_kCharMove == null)
		{
			return;
		}
		int num = 0;
		if (this.m_CurrentQuest != null)
		{
			num = this.m_CurrentQuest.GetQuestCommon().iCastleUnique;
			NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(this.m_CurrentQuest.GetQuestCommon().GiveQuestCharCode);
			if (charKindInfoFromCode != null)
			{
				NrClientNpcPosList clientNpcPosList = NrTSingleton<NkQuestManager>.Instance.GetClientNpcPosList(num);
				if (clientNpcPosList != null)
				{
					for (int i = 0; i < clientNpcPosList.ClientNpcPosList.Count; i++)
					{
						NrClientNpcInfo nrClientNpcInfo = clientNpcPosList.ClientNpcPosList[i];
						if (nrClientNpcInfo != null && NrTSingleton<NkQuestManager>.Instance.ClinetNpcCreateCheck(nrClientNpcInfo.kStartCon, nrClientNpcInfo.kEndCon) && charKindInfoFromCode.GetCode() == nrClientNpcInfo.strCharCode && num == nrClientNpcInfo.i32MapIndex)
						{
							NrCharBase char2 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
							if (char2 != null && char2.m_kCharMove != null)
							{
								lhs = char2.m_kCharMove.FindFirstPath(num, (short)nrClientNpcInfo.fFixPosX, (short)nrClientNpcInfo.fFixPosY, false);
								zero.x = nrClientNpcInfo.fFixPosX;
								zero.y = nrClientNpcInfo.fFixPosY;
							}
						}
					}
				}
				if (lhs == Vector3.zero)
				{
					NrNpcPos npcPos = NrTSingleton<NrNpcPosManager>.Instance.GetNpcPos(charKindInfoFromCode.GetPosKey(), charKindInfoFromCode.GetCharKind(), num);
					if (npcPos != null && @char.m_kCharMove != null)
					{
						lhs = @char.m_kCharMove.FindFirstPath(npcPos.nMapIndex, (short)npcPos.kPos.x, (short)npcPos.kPos.z, false);
						zero.x = npcPos.kPos.x;
						zero.y = npcPos.kPos.z;
					}
				}
			}
		}
		else
		{
			num = this.m_CurrentAdventureInfo.mapIndex;
			lhs = @char.m_kCharMove.FindFirstPath(num, (short)this.m_CurrentAdventureInfo.destX, (short)this.m_CurrentAdventureInfo.destY, false);
			zero.x = (float)this.m_CurrentAdventureInfo.destX;
			zero.y = (float)this.m_CurrentAdventureInfo.destY;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, num))
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		int mapIndex = myCharInfo.m_kCharMapInfo.MapIndex;
		if (num == mapIndex)
		{
			if (lhs != Vector3.zero)
			{
				GS_CHAR_FINDPATH_REQ gS_CHAR_FINDPATH_REQ = new GS_CHAR_FINDPATH_REQ();
				gS_CHAR_FINDPATH_REQ.DestPos.x = lhs.x;
				gS_CHAR_FINDPATH_REQ.DestPos.y = lhs.y;
				gS_CHAR_FINDPATH_REQ.DestPos.z = lhs.z;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_FINDPATH_REQ, gS_CHAR_FINDPATH_REQ);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "AUTOMOVE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
			base.CloseForm(null);
			NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ADVENTURE_DLG).CloseForm(null);
		}
		else
		{
			string mapName = NrTSingleton<MapManager>.Instance.GetMapName(num);
			if (mapName != string.Empty)
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI == null)
				{
					return;
				}
				ICollection gateInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetGateInfo_Col();
				if (gateInfo_Col == null)
				{
					return;
				}
				int num2 = 0;
				foreach (GATE_INFO gATE_INFO in gateInfo_Col)
				{
					if (num == gATE_INFO.DST_MAP_IDX)
					{
						num2 = gATE_INFO.GATE_IDX;
					}
				}
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("4"),
					"mapname",
					mapName
				});
				msgBoxUI.SetMsg(new YesDelegate(this.MapWarp), num2, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("3"), empty, eMsgType.MB_OK_CANCEL);
				msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("109"));
				msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
				this.m_nAutoMoveMapIndex = num;
				this.m_nAutoMoveGateIndex = num2;
				NrTSingleton<NkQuestManager>.Instance.AutoMoveMapIndex = this.m_nAutoMoveMapIndex;
				NrTSingleton<NkQuestManager>.Instance.AutoMove = true;
				NrTSingleton<NkQuestManager>.Instance.AutoMoveDestPos = zero;
			}
		}
	}

	public void SetEpisode(Adventure.AdventureInfo adventureInfo, CQuest quest)
	{
		this.m_CurrentAdventureInfo = adventureInfo;
		this.m_CurrentQuest = quest;
		if (this.m_CurrentAdventureInfo == null)
		{
			return;
		}
		CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(this.m_CurrentAdventureInfo.questGroupUnique);
		string empty = string.Empty;
		if (questGroupByGroupUnique != null)
		{
			string text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("75");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count",
				questGroupByGroupUnique.GetPage()
			});
			this.m_EpisodeNum.Text = empty;
			this.m_EpisodeName.Text = questGroupByGroupUnique.GetGroupTitle();
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Title(questGroupByGroupUnique.GetGroupUnique().ToString() + "_Group_Sum");
			if (text != null)
			{
				this.m_EpisodeSummary.Visible = true;
				this.m_EpisodeSummary.Text = text;
			}
			else
			{
				this.m_EpisodeSummary.Visible = false;
			}
		}
		NrCharKindInfo nrCharKindInfo;
		if (this.m_CurrentQuest != null)
		{
			nrCharKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_CurrentQuest.GetQuestCommon().i32QuestCharKind);
			if (nrCharKindInfo == null)
			{
				return;
			}
			this.m_Image.SetTexture(eCharImageType.LARGE, nrCharKindInfo.GetCharKind(), -1);
		}
		else
		{
			nrCharKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(this.m_CurrentAdventureInfo.monsterCode);
			if (nrCharKindInfo == null)
			{
				return;
			}
			this.m_Image.SetTexture(eCharImageType.LARGE, nrCharKindInfo.GetCharKind(), -1);
		}
		empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("93"),
			"targetname",
			nrCharKindInfo.GetName()
		});
		this.m_Start.Text = empty;
		NrTSingleton<EventConditionHandler>.Instance.OpenUI.OnTrigger();
	}

	private void MapWarp(object obj)
	{
		NrTSingleton<NkClientLogic>.Instance.SetWarp(true, this.m_nAutoMoveGateIndex, this.m_nAutoMoveMapIndex);
		base.CloseForm(null);
		NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ADVENTURE_DLG).CloseForm(null);
	}

	public override void OnClose()
	{
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		base.OnClose();
	}
}
