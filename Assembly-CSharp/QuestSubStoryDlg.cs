using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class QuestSubStoryDlg : Form
{
	private NewListBox m_kSubStoryListBox;

	private List<CQuestGroup> m_kQuestGroupList = new List<CQuestGroup>();

	private Button m_Close;

	private int m_nAutoMoveMapIndex;

	private int m_nAutoMoveGateIndex;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestList/DLG_SubStory", G_ID.QUEST_SUBSTORY_DLG, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_Close = (base.GetControl("Button_Back") as Button);
		this.m_Close.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickClose));
		this.m_kSubStoryListBox = (base.GetControl("NewListBox_SubStoryList") as NewListBox);
		Dictionary<int, CQuestGroup> hashQuestGroup = NrTSingleton<NkQuestManager>.Instance.GetHashQuestGroup();
		foreach (CQuestGroup current in hashQuestGroup.Values)
		{
			if (current.GetQuestType() == 2)
			{
				this.m_kQuestGroupList.Add(current);
			}
		}
		this.SetQuestSubStory();
		base.SetScreenCenter();
	}

	private void ClickClose(IUIObject obj)
	{
		AdventureDlg adventureDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ADVENTURE_DLG) as AdventureDlg;
		if (adventureDlg != null)
		{
			adventureDlg.DrawAdventure();
		}
		this.Close();
	}

	private void SetQuestSubStory()
	{
		bool flag = false;
		this.m_kSubStoryListBox.Clear();
		int level = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel();
		foreach (CQuestGroup current in this.m_kQuestGroupList)
		{
			for (int i = 0; i < 200; i++)
			{
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(current.GetQuestUniqueByBit(i));
				if (questByQuestUnique != null)
				{
					if (!NrTSingleton<NkQuestManager>.Instance.IsCompletedQuest(questByQuestUnique.GetQuestUnique()))
					{
						if ((int)questByQuestUnique.GetQuestLevel(1) <= level)
						{
							if (NrTSingleton<NkQuestManager>.Instance.GetQuestState(questByQuestUnique.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
							{
								NewListItem newListItem = new NewListItem(this.m_kSubStoryListBox.ColumnNum, true, string.Empty);
								if ((int)questByQuestUnique.GetQuestLevel(1) < level)
								{
									newListItem.SetListItemData(7, false);
								}
								string empty = string.Empty;
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
								{
									NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("75"),
									"count",
									current.GetPageUnique()
								});
								newListItem.SetListItemData(1, empty, null, null, null);
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
								{
									NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("93"),
									"targetname",
									questByQuestUnique.GetQuestNpcName()
								});
								newListItem.SetListItemData(3, empty, questByQuestUnique.GetQuestUnique(), new EZValueChangedDelegate(this.ClickMove), null);
								newListItem.SetListItemData(4, current.GetGroupTitle(), null, null, null);
								newListItem.SetListItemData(5, "UI/Adventure/EpisodeBG", true, null, null);
								newListItem.SetListItemData(6, questByQuestUnique.GetQuestNpc().GetCharKind(), true, null, null);
								newListItem.SetListItemData(8, questByQuestUnique.GetQuestLevel(1).ToString(), null, null, null);
								newListItem.SetListItemData(8, false);
								this.m_kSubStoryListBox.Add(newListItem);
								flag = true;
								break;
							}
						}
					}
				}
			}
		}
		this.m_kSubStoryListBox.RepositionItems();
		if (flag)
		{
			base.SetShowLayer(1, false);
		}
	}

	private void ClickMove(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		string strQuestUnique = (string)obj.Data;
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char == null)
		{
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
		if (questByQuestUnique == null)
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
		if (questByQuestUnique != null)
		{
			num = questByQuestUnique.GetQuestCommon().iCastleUnique;
			NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(questByQuestUnique.GetQuestCommon().GiveQuestCharCode);
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
				msgBoxUI.SetMsg(new YesDelegate(this.MapWarp), num2, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("3"), empty, eMsgType.MB_OK_CANCEL, 2);
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

	private void MapWarp(object obj)
	{
		NrTSingleton<NkClientLogic>.Instance.SetWarp(true, this.m_nAutoMoveGateIndex, this.m_nAutoMoveMapIndex);
		base.CloseForm(null);
		NrTSingleton<FormsManager>.Instance.GetForm(G_ID.QUEST_SUBSTORY_DLG).CloseForm(null);
	}
}
