using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Congraturation_DLG : Form
{
	private enum eMESSAGE_TYPE
	{
		eMESSAGE_TYPE_CONGRATULATION,
		eMESSAGE_TYPE_RECUIT,
		eMESSAGE_TYPE_BABELSTART,
		eMESSAGE_TYPE_BATTLERESULT,
		eMESSAGE_TYPE_ITEMSKILLGET,
		eMESSAGE_TYPE_ELEMENTSOLGET,
		eMESSAGE_TYPE_ITEMGET,
		eMESSAGE_TYPE_GUILD,
		eMESSAGE_TYPE_RECUIT_LUCKY
	}

	private ItemTexture m_iSolFace;

	private DrawTexture m_dSolFrame;

	private Label m_laTitle;

	private Label m_Label_contents;

	private CONGRATULATORY_MESSAGE m_curMessage;

	private Queue<CONGRATULATORY_MESSAGE> m_MessageQue = new Queue<CONGRATULATORY_MESSAGE>();

	private bool m_bITweenFound;

	private bool m_bMoveFinish;

	private bool m_bShow;

	private bool m_bNext = true;

	private int m_i32StartTime = Environment.TickCount;

	private int m_i32EndTime = Environment.TickCount;

	private float m_fHeight = 100f;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Common/DLG_Congraturation", G_ID.CONGRATURATIONDLG, true);
		base.SetLocation(-base.GetSizeX(), this.m_fHeight);
	}

	public override void SetComponent()
	{
		if (TsPlatform.IsWeb)
		{
		}
		this.m_iSolFace = (base.GetControl("ItemTexture_sol") as ItemTexture);
		this.m_dSolFrame = (base.GetControl("DrawTexture_solframe") as DrawTexture);
		this.m_laTitle = (base.GetControl("Label_title") as Label);
		this.m_Label_contents = (base.GetControl("Label_contents") as Label);
	}

	public void PushMessage(GS_CONGRATULATORY_MESSAGE_NFY messageNfy)
	{
		CONGRATULATORY_MESSAGE cONGRATULATORY_MESSAGE = new CONGRATULATORY_MESSAGE();
		cONGRATULATORY_MESSAGE.Set(0, messageNfy);
		this.m_MessageQue.Enqueue(cONGRATULATORY_MESSAGE);
	}

	public void PushBattleResultMessage(char[] name, int facecharkind, GS_COMMUNITY_MESSAGE_BATTLE_RESULT message, byte ReceibeUerType)
	{
		if (!NrTSingleton<NkCharManager>.Instance.CheckCongraturationTime(ReceibeUerType))
		{
			return;
		}
		CONGRATULATORY_MESSAGE cONGRATULATORY_MESSAGE = new CONGRATULATORY_MESSAGE();
		cONGRATULATORY_MESSAGE.m_nMsgType = 3;
		cONGRATULATORY_MESSAGE.m_nPersonID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendPersonIDByName(TKString.NEWString(name));
		cONGRATULATORY_MESSAGE.i32params[0] = facecharkind;
		cONGRATULATORY_MESSAGE.i32params[1] = (int)message.i8BattleType;
		cONGRATULATORY_MESSAGE.i32params[2] = (int)message.i8Result;
		cONGRATULATORY_MESSAGE.i32params[3] = message.i32Param1;
		cONGRATULATORY_MESSAGE.i32params[4] = message.i32Param2;
		cONGRATULATORY_MESSAGE.i32params[5] = message.i32Param3;
		cONGRATULATORY_MESSAGE.char_name = name;
		this.m_MessageQue.Enqueue(cONGRATULATORY_MESSAGE);
	}

	public void PushRECRUITMessage(int nSolKind, char[] strCharName, byte nGrade, byte ReceibeUerType, int iItemUnique)
	{
		if (!NrTSingleton<NkCharManager>.Instance.CheckCongraturationTime(ReceibeUerType))
		{
			return;
		}
		CONGRATULATORY_MESSAGE cONGRATULATORY_MESSAGE = new CONGRATULATORY_MESSAGE();
		cONGRATULATORY_MESSAGE.m_nMsgType = 1;
		cONGRATULATORY_MESSAGE.m_nPersonID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendPersonIDByName(TKString.NEWString(strCharName));
		cONGRATULATORY_MESSAGE.i32params[0] = nSolKind;
		cONGRATULATORY_MESSAGE.i32params[1] = (int)nGrade;
		cONGRATULATORY_MESSAGE.i32params[2] = iItemUnique;
		cONGRATULATORY_MESSAGE.char_name = strCharName;
		this.m_MessageQue.Enqueue(cONGRATULATORY_MESSAGE);
	}

	public void PushBabelStartMessage(GS_COMMUNITY_MESSAGE_BABELTOWER_START message, byte ReceibeUerType)
	{
		if (!NrTSingleton<NkCharManager>.Instance.CheckCongraturationTime(ReceibeUerType))
		{
			return;
		}
		CONGRATULATORY_MESSAGE cONGRATULATORY_MESSAGE = new CONGRATULATORY_MESSAGE();
		cONGRATULATORY_MESSAGE.m_nMsgType = 2;
		cONGRATULATORY_MESSAGE.m_nPersonID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendPersonIDByName(TKString.NEWString(message.szCharName));
		cONGRATULATORY_MESSAGE.i32params[0] = message.i32FaceCharKind;
		cONGRATULATORY_MESSAGE.i32params[1] = (int)message.i16Floor;
		cONGRATULATORY_MESSAGE.i32params[2] = (int)message.i16SubFloor;
		cONGRATULATORY_MESSAGE.i32params[3] = message.i32UserCount;
		cONGRATULATORY_MESSAGE.i32params[4] = (int)message.i16FloorType;
		cONGRATULATORY_MESSAGE.char_name = message.szCharName;
		cONGRATULATORY_MESSAGE.szparam1 = message.szCharName1;
		cONGRATULATORY_MESSAGE.szparam2 = message.szCharName2;
		cONGRATULATORY_MESSAGE.szparam3 = message.szCharName3;
		this.m_MessageQue.Enqueue(cONGRATULATORY_MESSAGE);
		NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.UpdateFriendBabelData(cONGRATULATORY_MESSAGE.m_nPersonID, (short)cONGRATULATORY_MESSAGE.i32params[1], (byte)cONGRATULATORY_MESSAGE.i32params[2], (short)cONGRATULATORY_MESSAGE.i32params[4], 0);
	}

	public void PushItemSkillGetMessage(GS_COMMUNITY_MESSAGE_ITEM_SKILL_GET message, byte ReceibeUerType)
	{
		if (!NrTSingleton<NkCharManager>.Instance.CheckCongraturationTime(ReceibeUerType))
		{
			return;
		}
		CONGRATULATORY_MESSAGE cONGRATULATORY_MESSAGE = new CONGRATULATORY_MESSAGE();
		cONGRATULATORY_MESSAGE.m_nMsgType = 4;
		cONGRATULATORY_MESSAGE.m_nPersonID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendPersonIDByName(TKString.NEWString(message.szCharName));
		cONGRATULATORY_MESSAGE.m_nItemUnique = message.i32ItemUnique;
		cONGRATULATORY_MESSAGE.i32params[0] = message.i32ItemSkillUnique;
		cONGRATULATORY_MESSAGE.i32params[1] = message.i32ItemSkillLevel;
		cONGRATULATORY_MESSAGE.char_name = message.szCharName;
		this.m_MessageQue.Enqueue(cONGRATULATORY_MESSAGE);
	}

	public void PushItemGetMessage(GS_COMMUNITY_MESSAGE_ITEMGET message, byte ReceibeUerType)
	{
		if (!NrTSingleton<NkCharManager>.Instance.CheckCongraturationTime(ReceibeUerType))
		{
			return;
		}
		CONGRATULATORY_MESSAGE cONGRATULATORY_MESSAGE = new CONGRATULATORY_MESSAGE();
		cONGRATULATORY_MESSAGE.m_nMsgType = 6;
		cONGRATULATORY_MESSAGE.m_nPersonID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendPersonIDByName(TKString.NEWString(message.szCharName));
		cONGRATULATORY_MESSAGE.m_nItemUnique = message.i32ItemUnique;
		cONGRATULATORY_MESSAGE.m_nItemNum = message.i32ItemNum;
		cONGRATULATORY_MESSAGE.i32params[0] = message.i32BoxItemUnique;
		cONGRATULATORY_MESSAGE.char_name = message.szCharName;
		this.m_MessageQue.Enqueue(cONGRATULATORY_MESSAGE);
	}

	public void PushElementSolGetMessage(int nSolKind, char[] strCharName, byte nGrade, byte ReceibeUerType)
	{
		if (!NrTSingleton<NkCharManager>.Instance.CheckCongraturationTime(ReceibeUerType))
		{
			return;
		}
		CONGRATULATORY_MESSAGE cONGRATULATORY_MESSAGE = new CONGRATULATORY_MESSAGE();
		cONGRATULATORY_MESSAGE.m_nMsgType = 5;
		cONGRATULATORY_MESSAGE.m_nPersonID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendPersonIDByName(TKString.NEWString(strCharName));
		cONGRATULATORY_MESSAGE.i32params[0] = nSolKind;
		cONGRATULATORY_MESSAGE.i32params[1] = (int)nGrade;
		cONGRATULATORY_MESSAGE.char_name = strCharName;
		this.m_MessageQue.Enqueue(cONGRATULATORY_MESSAGE);
	}

	public void PushGuildMessage(GS_COMMUNITY_MESSAGE_GUILD message, byte ReceibeUerType)
	{
		CONGRATULATORY_MESSAGE cONGRATULATORY_MESSAGE = new CONGRATULATORY_MESSAGE();
		cONGRATULATORY_MESSAGE.m_nMsgType = 7;
		cONGRATULATORY_MESSAGE.m_nItemUnique = -1;
		cONGRATULATORY_MESSAGE.i32params[0] = message.i32SubMessageType;
		cONGRATULATORY_MESSAGE.i32params[1] = message.i32Param1;
		cONGRATULATORY_MESSAGE.i32params[2] = (int)message.i64Param2;
		this.m_MessageQue.Enqueue(cONGRATULATORY_MESSAGE);
	}

	public void PushRECRUIT_LUCKYMessage(int nSolKind, char[] strCharName, byte nGrade, byte ReceibeUerType, int iItemUnique)
	{
		if (!NrTSingleton<NkCharManager>.Instance.CheckCongraturationTime(ReceibeUerType))
		{
			return;
		}
		CONGRATULATORY_MESSAGE cONGRATULATORY_MESSAGE = new CONGRATULATORY_MESSAGE();
		cONGRATULATORY_MESSAGE.m_nMsgType = 8;
		cONGRATULATORY_MESSAGE.m_nPersonID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendPersonIDByName(TKString.NEWString(strCharName));
		cONGRATULATORY_MESSAGE.i32params[0] = nSolKind;
		cONGRATULATORY_MESSAGE.i32params[1] = (int)nGrade;
		cONGRATULATORY_MESSAGE.i32params[2] = iItemUnique;
		cONGRATULATORY_MESSAGE.char_name = strCharName;
		this.m_MessageQue.Enqueue(cONGRATULATORY_MESSAGE);
	}

	public void SetInfo(CONGRATULATORY_MESSAGE messageNfy)
	{
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		int charkind = messageNfy.i32params[0];
		if (messageNfy.m_nItemUnique == 0)
		{
			if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(charkind) == null)
			{
				return;
			}
			this.m_dSolFrame.SetTexture(NrTSingleton<NrCharKindInfoManager>.Instance.GetSolRankBackImg(charkind));
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (messageNfy.m_nMsgType == 1)
		{
			this.m_laTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("61");
			string text3 = TKString.NEWString(messageNfy.char_name);
			string text4 = string.Empty;
			text4 = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(messageNfy.i32params[0]);
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(messageNfy.i32params[2]);
			if (string.Empty != itemNameByItemUnique)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2559"),
					"charname",
					text3,
					"target",
					itemNameByItemUnique,
					"solname",
					text4
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1383"),
					"charname",
					text3,
					"targetname",
					text4
				});
			}
			this.m_iSolFace.SetSolImageTexure(eCharImageType.SMALL, charkind, messageNfy.i32params[1]);
		}
		else if (messageNfy.m_nMsgType == 3)
		{
			this.m_laTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("61");
			string text5 = TKString.NEWString(messageNfy.char_name);
			switch (messageNfy.i32params[1])
			{
			case 1:
				if (messageNfy.i32params[2] == 0)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("988");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text,
						"charname",
						text5
					});
				}
				break;
			case 2:
				if (messageNfy.i32params[2] == 0)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("990");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text,
						"charname",
						text5
					});
				}
				break;
			case 3:
			{
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				kMyCharInfo.m_kFriendInfo.UpdateFriendBabelData(messageNfy.m_nPersonID, (short)messageNfy.i32params[3], (byte)messageNfy.i32params[4], (short)((byte)messageNfy.i32params[5]), 0);
				if (messageNfy.i32params[2] == 0 && messageNfy.i32params[5] == 6)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("989");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text,
						"charname",
						text5,
						"floor",
						messageNfy.i32params[3],
						"subfloor",
						messageNfy.i32params[4] + 1
					});
				}
				else
				{
					this.Close();
				}
				break;
			}
			}
			this.m_iSolFace.SetSolImageTexure(eCharImageType.SMALL, charkind, -1);
		}
		else if (messageNfy.m_nMsgType == 2)
		{
			this.m_laTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("556");
			string text6 = TKString.NEWString(messageNfy.char_name);
			if (messageNfy.i32params[3] == 2)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"charname1",
					text6,
					"charname2",
					TKString.NEWString(messageNfy.szparam1),
					"floor",
					messageNfy.i32params[1],
					"subfloor",
					messageNfy.i32params[2] + 1
				});
			}
			else if (messageNfy.i32params[3] == 3)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1405");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"charname1",
					text6,
					"charname2",
					TKString.NEWString(messageNfy.szparam1),
					"charname3",
					TKString.NEWString(messageNfy.szparam2),
					"floor",
					messageNfy.i32params[1],
					"subfloor",
					messageNfy.i32params[2] + 1
				});
			}
			else if (messageNfy.i32params[3] == 4)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1406");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"charname1",
					text6,
					"charname2",
					TKString.NEWString(messageNfy.szparam1),
					"charname3",
					TKString.NEWString(messageNfy.szparam2),
					"charname4",
					TKString.NEWString(messageNfy.szparam3),
					"floor",
					messageNfy.i32params[1],
					"subfloor",
					messageNfy.i32params[2] + 1
				});
			}
			this.m_iSolFace.SetSolImageTexure(eCharImageType.SMALL, charkind, -1);
		}
		else if (messageNfy.m_nMsgType == 4)
		{
			string text7 = TKString.NEWString(messageNfy.char_name);
			string text8 = NrTSingleton<NrItemSkillInfoManager>.Instance.GetPreText(messageNfy.i32params[0]) + " " + NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(messageNfy.m_nItemUnique);
			TsLog.LogWarning("CharName={0} ItemName={1}, ItemUnique={2}, skilllevel={3}", new object[]
			{
				text7,
				text8,
				messageNfy.m_nItemUnique,
				messageNfy.i32params[1]
			});
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1364"),
				"charname",
				text7,
				"targetname",
				text8,
				"skilllevel",
				messageNfy.i32params[1]
			});
			ITEM iTEM = new ITEM();
			iTEM.m_nOption[2] = 6;
			iTEM.m_nItemUnique = messageNfy.m_nItemUnique;
			iTEM.m_nItemNum = messageNfy.m_nItemNum;
			this.m_iSolFace.SetItemTexture(iTEM);
		}
		else if (messageNfy.m_nMsgType == 6)
		{
			string text9 = TKString.NEWString(messageNfy.char_name);
			string itemNameByItemUnique2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(messageNfy.m_nItemUnique);
			if (messageNfy.i32params[0] > 0)
			{
				string itemNameByItemUnique3 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(messageNfy.i32params[0]);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1932"),
					"charname",
					text9,
					"targetname1",
					itemNameByItemUnique3,
					"targetname",
					itemNameByItemUnique2
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1383"),
					"charname",
					text9,
					"targetname",
					itemNameByItemUnique2
				});
			}
			ITEM iTEM2 = new ITEM();
			iTEM2.m_nOption[2] = 6;
			iTEM2.m_nItemUnique = messageNfy.m_nItemUnique;
			iTEM2.m_nItemNum = messageNfy.m_nItemNum;
			this.m_iSolFace.SetItemTexture(iTEM2);
		}
		else if (messageNfy.m_nMsgType == 5)
		{
			this.m_laTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("61");
			string text10 = TKString.NEWString(messageNfy.char_name);
			string text11 = string.Empty;
			text11 = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(messageNfy.i32params[0]);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1817"),
				"charname",
				text10,
				"targetname",
				text11
			});
			this.m_iSolFace.SetSolImageTexure(eCharImageType.SMALL, charkind, messageNfy.i32params[1]);
		}
		else if (messageNfy.m_nMsgType == 7)
		{
			this.m_laTitle.Text = this.GetTitle_GuildMsg(messageNfy.i32params[0]);
			text2 = this.GetExplain_GuildMsg(messageNfy.i32params[0], messageNfy.i32params[1], messageNfy.i32params[2]);
			if (messageNfy.i32params[0] == 0 || messageNfy.i32params[0] == 1 || messageNfy.i32params[0] == 2)
			{
				BABEL_GUILDBOSS babelGuildBossinfo = NrTSingleton<BabelTowerManager>.Instance.GetBabelGuildBossinfo((short)messageNfy.i32params[1]);
				if (babelGuildBossinfo == null)
				{
					return;
				}
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(babelGuildBossinfo.m_nBossKind);
				if (charKindInfo != null)
				{
					this.m_iSolFace.SetSolImageTexure(eCharImageType.SMALL, charKindInfo.GetCharKind(), -1);
				}
			}
		}
		if (messageNfy.m_nMsgType == 8)
		{
			this.m_laTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("61");
			string text12 = TKString.NEWString(messageNfy.char_name);
			string text13 = string.Empty;
			text13 = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(messageNfy.i32params[0]);
			string itemNameByItemUnique4 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(messageNfy.i32params[2]);
			if (string.Empty != itemNameByItemUnique4)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2559"),
					"charname",
					text12,
					"target",
					itemNameByItemUnique4,
					"solname",
					text13
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1383"),
					"charname",
					text12,
					"targetname",
					text13
				});
			}
			this.m_iSolFace.SetSolImageTexure(eCharImageType.SMALL, charkind, messageNfy.i32params[1]);
		}
		this.m_Label_contents.Text = text2;
	}

	private void DequeueInfo()
	{
		if (this.m_MessageQue.Count <= 0)
		{
			this.Close();
			return;
		}
		this.m_curMessage = this.m_MessageQue.Dequeue();
		this.SetInfo(this.m_curMessage);
		base.SetLocation(-base.GetSizeX(), this.m_fHeight);
		iTween.MoveBy(base.InteractivePanel.gameObject, new Vector3(base.GetSizeX(), 0f, 0f), 4.5f);
		this.m_bShow = true;
		TsAudioManager.Container.RequestAudioClip("UI_SFX", "ETC", "CELEBRATE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void OnClose()
	{
		if (this.m_MessageQue.Count <= 0)
		{
			base.OnClose();
		}
		else
		{
			this.DequeueInfo();
		}
	}

	public override void Update()
	{
		if (this.m_bNext)
		{
			this.DequeueInfo();
			this.m_bNext = false;
		}
		if (this.m_bShow)
		{
			if (!this.m_bITweenFound && null != base.InteractivePanel.gameObject.GetComponent<iTween>())
			{
				this.m_bITweenFound = true;
			}
			if (!this.m_bMoveFinish && this.m_bITweenFound && null == base.InteractivePanel.gameObject.GetComponent<iTween>())
			{
				this.m_bMoveFinish = true;
			}
			if (this.m_bMoveFinish)
			{
				if (this.m_i32StartTime - this.m_i32EndTime <= 5000)
				{
					this.m_i32StartTime = Environment.TickCount;
					return;
				}
				this.m_bShow = false;
			}
		}
		else
		{
			if (!this.m_bITweenFound && null != base.InteractivePanel.gameObject.GetComponent<iTween>())
			{
				this.m_bITweenFound = true;
			}
			if (!this.m_bMoveFinish && this.m_bITweenFound && null == base.InteractivePanel.gameObject.GetComponent<iTween>())
			{
				this.m_bMoveFinish = true;
			}
			if (this.m_bMoveFinish)
			{
				this.m_bMoveFinish = false;
				this.m_bITweenFound = false;
				this.m_bShow = true;
				this.m_bNext = true;
				this.m_i32StartTime = Environment.TickCount;
				this.m_i32EndTime = Environment.TickCount;
			}
		}
	}

	public string GetTitle_GuildMsg(int submessagetype)
	{
		if (submessagetype == 0)
		{
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1923");
		}
		if (submessagetype != 1)
		{
			return string.Empty;
		}
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1950");
	}

	public string GetExplain_GuildMsg(int submessagetype, int param1, int param2)
	{
		string text = string.Empty;
		string empty = string.Empty;
		switch (submessagetype)
		{
		case 0:
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1924");
			BABEL_GUILDBOSS babelGuildBossinfo = NrTSingleton<BabelTowerManager>.Instance.GetBabelGuildBossinfo((short)param1);
			if (babelGuildBossinfo != null)
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(babelGuildBossinfo.m_nBossKind);
				if (charKindInfo != null)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						text,
						"count",
						param1,
						"targetname",
						charKindInfo.GetName()
					});
				}
				return empty;
			}
			break;
		}
		case 1:
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1951");
			BABEL_GUILDBOSS babelGuildBossinfo2 = NrTSingleton<BabelTowerManager>.Instance.GetBabelGuildBossinfo((short)param1);
			if (babelGuildBossinfo2 != null)
			{
				NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(babelGuildBossinfo2.m_nBossKind);
				if (charKindInfo2 != null)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						text,
						"count",
						param1,
						"targetname",
						charKindInfo2.GetName()
					});
				}
				return empty;
			}
			break;
		}
		case 2:
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2210");
			BABEL_GUILDBOSS babelGuildBossinfo3 = NrTSingleton<BabelTowerManager>.Instance.GetBabelGuildBossinfo((short)param1);
			if (babelGuildBossinfo3 != null)
			{
				NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID((long)param2);
				if (memberInfoFromPersonID != null)
				{
					NrCharKindInfo charKindInfo3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(babelGuildBossinfo3.m_nBossKind);
					if (charKindInfo3 != null)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							text,
							"targetname",
							memberInfoFromPersonID.GetCharName(),
							"count",
							param1,
							"targetname2",
							charKindInfo3.GetName()
						});
					}
					return empty;
				}
			}
			break;
		}
		}
		return string.Empty;
	}
}
