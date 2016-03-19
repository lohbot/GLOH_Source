using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class BabelGuildBossDlg : Form
{
	private const int LIST_FLOOR_COUNT = 5;

	private const int LIST_BUTTON_STARTINDEX = 1;

	private const int LIST_FLOOR_FRIENDBG = 6;

	private const int LIST_FLOOR_FRIEND_TEX = 7;

	private const int LIST_FLOORTEXTIMG_STARTINDEX = 36;

	private const int LIST_FLOORTEXTIMG_STARTINDEX_TWO = 41;

	private const int LIST_BG_INDEX = 0;

	private const int SHOW_FLOOR_COUNT = 5;

	private DrawTexture m_dtEffect;

	private NewListBox m_lbFloor;

	private Button upButton;

	private Button downButton;

	private DrawTexture up;

	private DrawTexture down;

	private Button m_btClose;

	private Label m_laClose;

	private DrawTexture m_dtClose;

	private short m_SelectFloor;

	private short m_Selectindex = -1;

	public override void InitializeComponent()
	{
		NrTSingleton<FormsManager>.Instance.HideMainUI();
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		form.AlwaysUpdate = true;
		instance.LoadFileAll(ref form, "GuildBoss/dlg_GuildBoss_main", G_ID.BABEL_GUILDBOSS_MAIN_DLG, false, true);
		base.ShowBlackBG(0.5f);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		float height = GUICamera.height;
		this.m_dtEffect = (base.GetControl("DrawTexture_Effect") as DrawTexture);
		this.m_lbFloor = (base.GetControl("NewListBox_floor") as NewListBox);
		this.m_lbFloor.AddRightMouseDelegate(new EZValueChangedDelegate(this.BtClickFloor));
		this.m_lbFloor.Reserve = false;
		this.m_lbFloor.SelectStyle = "Com_B_Transparent";
		this.m_lbFloor.AutoScroll = true;
		this.upButton = (base.GetControl("Button_slideup") as Button);
		this.upButton.SetLocation(this.upButton.GetLocation().x, this.upButton.GetLocationY(), this.upButton.GetLocation().z - 1.1f);
		this.downButton = (base.GetControl("Button_slidedown") as Button);
		BoxCollider boxCollider = (BoxCollider)this.upButton.gameObject.GetComponent(typeof(BoxCollider));
		if (boxCollider != null)
		{
			boxCollider.size = new Vector3(0f, 0f, 0f);
		}
		boxCollider = (BoxCollider)this.downButton.gameObject.GetComponent(typeof(BoxCollider));
		if (boxCollider != null)
		{
			boxCollider.size = new Vector3(0f, 0f, 0f);
		}
		this.up = (base.GetControl("DrawTexture_SlideBG01") as DrawTexture);
		this.up.SetLocation(this.up.GetLocation().x, this.up.GetLocationY(), this.up.GetLocation().z - 1f);
		this.down = (base.GetControl("DrawTexture_SlideBG02") as DrawTexture);
		this.down.SetLocation(this.down.GetLocation().x, height - this.down.GetSize().y, this.down.GetLocation().z - 1f);
		this.downButton.SetLocation(this.downButton.GetLocation().x, this.down.GetLocationY() + 76f, this.downButton.GetLocation().z - 1.1f);
		this.upButton.Visible = false;
		this.up.Visible = false;
		this.m_dtClose = (base.GetControl("DrawTexture_Close") as DrawTexture);
		this.m_laClose = (base.GetControl("Label_Close") as Label);
		this.m_btClose = (base.GetControl("Button_close") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickClose));
		this.m_dtClose.SetLocation(this.m_dtClose.GetLocationX(), this.m_dtClose.GetLocationY(), this.m_dtClose.GetLocation().z - 0.7f);
		this.m_laClose.SetLocation(this.m_laClose.GetLocationX(), this.m_laClose.GetLocationY(), this.m_laClose.GetLocation().z - 0.7f);
		this.m_btClose.SetLocation(this.m_btClose.GetLocationX(), this.m_btClose.GetLocationY(), this.m_btClose.GetLocation().z - 0.7f);
		base.SetScreenCenter();
		UIDataManager.MuteSound(true);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("BGM", "CHAOSTOWER", "START", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_BABEL_MAIN", this.m_dtEffect, this.m_dtEffect.GetSize());
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(0);
	}

	public override void Show()
	{
		this.ShowList();
		base.Show();
	}

	public override void Update()
	{
		if (!this.m_lbFloor.changeScrollPos)
		{
			return;
		}
		if (0.01f >= this.m_lbFloor.ScrollPosition)
		{
			this.upButton.Visible = false;
			this.up.Visible = false;
		}
		else if (1f <= this.m_lbFloor.ScrollPosition)
		{
			this.downButton.Visible = false;
			this.down.Visible = false;
		}
		else
		{
			this.upButton.Visible = true;
			this.up.Visible = true;
			this.downButton.Visible = true;
			this.down.Visible = true;
		}
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
		NrTSingleton<FormsManager>.Instance.Main_UI_Show(FormsManager.eMAIN_UI_VISIBLE_MODE.COMMON);
		UIDataManager.MuteSound(false);
		TsAudioManager.Instance.AudioContainer.RemoveUI("CHAOSTOWER_START");
		base.OnClose();
	}

	public void ShowList()
	{
		this.m_lbFloor.Clear();
		short num = NrTSingleton<BabelTowerManager>.Instance.GetLastGuildBossFloor();
		short guildBossLastFloor = NrTSingleton<ContentsLimitManager>.Instance.GetGuildBossLastFloor();
		if (0 < guildBossLastFloor && guildBossLastFloor < num)
		{
			num = guildBossLastFloor;
		}
		short num2 = num / 5;
		if (num % 5 != 0)
		{
			num2 += 1;
		}
		for (short num3 = num2; num3 > 0; num3 -= 1)
		{
			NewListItem newListItem = this.CreateItem(num3);
			if (newListItem != null)
			{
				this.m_lbFloor.Add(newListItem);
			}
		}
		this.SetRoomStateEffect();
		this.m_lbFloor.RepositionItems();
		this.m_lbFloor.SetSelectedItem((int)this.m_Selectindex);
	}

	public void SetRoomStateEffect()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		short num = NrTSingleton<BabelTowerManager>.Instance.GetLastGuildBossFloor();
		short guildBossLastFloor = NrTSingleton<ContentsLimitManager>.Instance.GetGuildBossLastFloor();
		if (0 < guildBossLastFloor && guildBossLastFloor < num)
		{
			num = guildBossLastFloor;
		}
		short num2 = num / 5;
		if (num % 5 != 0)
		{
			num2 += 1;
		}
		bool flag = true;
		for (int i = 0; i < (int)num2; i++)
		{
			IUIListObject item = this.m_lbFloor.GetItem(i);
			for (int j = 5; j > 0; j--)
			{
				if ((int)num >= ((int)num2 - i) * 5 - (5 - j))
				{
					string effectKey = string.Empty;
					NEWGUILD_MY_BOSS_ROOMINFO guildBossMyRoomInfo = kMyCharInfo.GetGuildBossMyRoomInfo(num);
					if (guildBossMyRoomInfo != null)
					{
						effectKey = this.GetRoomStateEffect(guildBossMyRoomInfo.byRoomState);
					}
					else
					{
						effectKey = this.GetRoomStateEffect(0);
					}
					int num3 = j - 1;
					UIButton uIButton = ((UIListItemContainer)item).GetElement(num3 + 1) as UIButton;
					if (uIButton != null)
					{
						NrTSingleton<FormsManager>.Instance.AttachEffectKey(effectKey, uIButton, uIButton.GetSize());
					}
					num -= 1;
				}
			}
			flag = !flag;
		}
	}

	public string GetRoomStateEffect(byte roomstate)
	{
		string result = string.Empty;
		switch (roomstate)
		{
		case 0:
			result = "FX_UI_TOWERBOSS_OFF";
			break;
		case 1:
		case 2:
			result = "FX_UI_TOWERBOSS_ON";
			break;
		case 3:
			result = "FX_UI_TOWERBOSS_CLEAR";
			break;
		}
		return result;
	}

	public void BtClickFloor(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		short num = (short)obj.Data;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1923");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("173");
		string message = string.Empty;
		short guildBossLastFloor = NrTSingleton<ContentsLimitManager>.Instance.GetGuildBossLastFloor();
		if (0 < guildBossLastFloor && guildBossLastFloor < num)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("608"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NEWGUILD_MY_BOSS_ROOMINFO guildBossMyRoomInfo = kMyCharInfo.GetGuildBossMyRoomInfo(num);
		if (guildBossMyRoomInfo == null)
		{
			if (NrTSingleton<NewGuildManager>.Instance.IsMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID) || NrTSingleton<NewGuildManager>.Instance.IsSubMaster(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID) || NrTSingleton<NewGuildManager>.Instance.IsOfficer(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
			{
				BABEL_GUILDBOSS babelGuildBossinfo = NrTSingleton<BabelTowerManager>.Instance.GetBabelGuildBossinfo(num);
				if (babelGuildBossinfo == null)
				{
					return;
				}
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(babelGuildBossinfo.m_nBossKind);
				if (charKindInfo == null)
				{
					return;
				}
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref message, new object[]
				{
					textFromMessageBox,
					"targetname",
					charKindInfo.GetName()
				});
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI.SetMsg(new YesDelegate(BabelGuildBossDlg.OnBabelGuildBossOpen), num, new NoDelegate(BabelGuildBossDlg.OnBabelGuildBossCancel), null, textFromInterface, message, eMsgType.MB_OK_CANCEL);
				msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10"));
				msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
			}
			else
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("546");
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
		else if (guildBossMyRoomInfo.byRoomState == 1 || guildBossMyRoomInfo.byRoomState == 2 || guildBossMyRoomInfo.byRoomState == 3)
		{
			GS_NEWGUILD_BOSS_ROOMINFO_REQ gS_NEWGUILD_BOSS_ROOMINFO_REQ = new GS_NEWGUILD_BOSS_ROOMINFO_REQ();
			gS_NEWGUILD_BOSS_ROOMINFO_REQ.i16Floor = num;
			gS_NEWGUILD_BOSS_ROOMINFO_REQ.byRoomState = guildBossMyRoomInfo.byRoomState;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_BOSS_ROOMINFO_REQ, gS_NEWGUILD_BOSS_ROOMINFO_REQ);
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CHAOSTOWER", "ENTER", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void BtClickClose(IUIObject obj)
	{
		this.Close();
	}

	public void BtnClickMemberInfo(IUIObject obj)
	{
		long num = (long)obj.Data;
		if (num > 0L)
		{
			NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(num);
			if (memberInfoFromPersonID == null)
			{
				return;
			}
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("481");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromNotify,
				"targetname",
				memberInfoFromPersonID.GetCharName()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
	}

	public static void OnBabelGuildBossOpen(object EventObject)
	{
		short i16Floor = (short)EventObject;
		GS_NEWGUILD_BOSS_OPENROOM_REQ gS_NEWGUILD_BOSS_OPENROOM_REQ = new GS_NEWGUILD_BOSS_OPENROOM_REQ();
		gS_NEWGUILD_BOSS_OPENROOM_REQ.i16Floor = i16Floor;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_BOSS_OPENROOM_REQ, gS_NEWGUILD_BOSS_OPENROOM_REQ);
	}

	public static void OnBabelGuildBossCancel(object EventObject)
	{
	}

	public bool GuildBossCheck(short floor)
	{
		NEWGUILD_MY_BOSS_ROOMINFO guildBossMyRoomInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossMyRoomInfo(floor);
		return guildBossMyRoomInfo != null && guildBossMyRoomInfo.ui8PlayState > 0;
	}

	public NewListItem CreateItem(short Column)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		string text = string.Empty;
		string text2 = string.Empty;
		short num = Column * 5;
		bool flag = Column % 2 == 0;
		short guildBossLastFloor = NrTSingleton<ContentsLimitManager>.Instance.GetGuildBossLastFloor();
		string columnData = string.Empty;
		if (flag)
		{
			columnData = string.Format("Mobile/DLG/GuildBoss/newlistbox_floor_columndata{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		}
		else
		{
			columnData = string.Format("Mobile/DLG/GuildBoss/newlistbox_floor1_columndata{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		}
		this.m_lbFloor.SetColumnData(columnData);
		NewListItem newListItem = new NewListItem(this.m_lbFloor.ColumnNum, true);
		if (!flag)
		{
			newListItem.SetListItemData(0, "UI/BabelTower/boss_main1", true, null, null);
		}
		else
		{
			newListItem.SetListItemData(0, "UI/BabelTower/boss_main2", true, null, null);
		}
		for (int i = 4; i >= 0; i--)
		{
			newListItem.SetListItemData(i * 6 + 6, false);
			newListItem.SetListItemData(i * 6 + 6 + 2, false);
			newListItem.SetListItemData(i * 6 + 6 + 4, false);
			newListItem.SetListItemData(i * 6 + 7, false);
			newListItem.SetListItemData(i * 6 + 7 + 2, false);
			newListItem.SetListItemData(i * 6 + 7 + 4, false);
			newListItem.SetListItemData(i + 36, false);
			newListItem.SetListItemData(i + 41, false);
			newListItem.SetListItemData(i + 5 + 41, false);
			newListItem.SetListItemData(i + 1, false);
			newListItem.SetListItemData(51 + i, false);
			newListItem.SetListItemData(56 + i * 2, false);
			newListItem.SetListItemData(56 + i * 2 + 1, false);
			if (num <= guildBossLastFloor)
			{
				newListItem.SetListItemData(i + 1, true);
				if (num / 10 <= 0 && num % 10 > 0)
				{
					text = "Win_Number_" + num;
					newListItem.SetListItemData(i + 36, true);
					newListItem.SetListItemData(i + 36, text, null, null, null);
				}
				else
				{
					short num2 = num / 10;
					short num3 = num % 10;
					text = "Win_Number_" + num2;
					text2 = "Win_Number_" + num3;
					newListItem.SetListItemData(i + 41, true);
					newListItem.SetListItemData(i + 41, text, null, null, null);
					newListItem.SetListItemData(i + 5 + 41, true);
					newListItem.SetListItemData(i + 5 + 41, text2, null, null, null);
				}
				newListItem.SetListItemData(i + 1, string.Empty, num, new EZValueChangedDelegate(this.BtClickFloor), null);
				bool flag2 = this.GuildBossCheck(num);
				if (flag2)
				{
					newListItem.SetListItemData(51 + i, true);
					newListItem.SetListItemData(51 + i, "Win_I_Notice04", null, null, null);
				}
				NEWGUILD_MY_BOSS_ROOMINFO guildBossMyRoomInfo = kMyCharInfo.GetGuildBossMyRoomInfo(num);
				if (guildBossMyRoomInfo != null)
				{
					if (this.m_SelectFloor < num)
					{
						this.m_Selectindex = Column;
						this.m_SelectFloor = num;
					}
					if (guildBossMyRoomInfo.i64PlayPersonID > 0L)
					{
						newListItem.SetListItemData(56 + i * 2, true);
						newListItem.SetListItemData(56 + i * 2 + 1, true);
						NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(guildBossMyRoomInfo.i64PlayPersonID);
						if (memberInfoFromPersonID != null)
						{
							if (memberInfoFromPersonID.GetPortrait() != null)
							{
								newListItem.SetListItemData(56 + i * 2 + 1, memberInfoFromPersonID.GetPortrait(), guildBossMyRoomInfo.i64PlayPersonID, null, new EZValueChangedDelegate(this.BtnClickMemberInfo), null);
							}
							else
							{
								NkListSolInfo nkListSolInfo = new NkListSolInfo();
								nkListSolInfo.SolCharKind = memberInfoFromPersonID.GetFaceCharKind();
								nkListSolInfo.SolGrade = -1;
								nkListSolInfo.SolLevel = memberInfoFromPersonID.GetLevel();
								newListItem.SetListItemData(56 + i * 2 + 1, nkListSolInfo, guildBossMyRoomInfo.i64PlayPersonID, new EZValueChangedDelegate(this.BtnClickMemberInfo), null);
							}
						}
					}
				}
			}
			num -= 1;
		}
		flag = !flag;
		newListItem.Data = Column;
		return newListItem;
	}

	public void UpdateFloor(short floor)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		short num = floor / 5;
		if (floor % 5 != 0)
		{
			num += 1;
		}
		short num2 = (floor - 1) % 5;
		NewListItem newListItem = this.CreateItem(num);
		if (newListItem != null)
		{
			for (int i = 0; i < this.m_lbFloor.Count; i++)
			{
				IUIListObject item = this.m_lbFloor.GetItem(i);
				if (item != null && (short)item.Data == num)
				{
					bool flag = this.GuildBossCheck(floor);
					DrawTexture drawTexture = ((UIListItemContainer)item).GetElement((int)(51 + num2)) as DrawTexture;
					drawTexture.Visible = false;
					if (flag)
					{
						drawTexture.Visible = true;
						drawTexture.SetTexture("Win_I_Notice04");
					}
					UIButton uIButton = ((UIListItemContainer)item).GetElement((int)(num2 + 1)) as UIButton;
					DrawTexture drawTexture2 = ((UIListItemContainer)item).GetElement((int)(56 + num2 * 2)) as DrawTexture;
					if (drawTexture2 != null)
					{
						drawTexture2.Visible = false;
					}
					ItemTexture itemTexture = ((UIListItemContainer)item).GetElement((int)(56 + num2 * 2 + 1)) as ItemTexture;
					if (itemTexture != null)
					{
						itemTexture.Visible = false;
					}
					NEWGUILD_MY_BOSS_ROOMINFO guildBossMyRoomInfo = kMyCharInfo.GetGuildBossMyRoomInfo(floor);
					if (guildBossMyRoomInfo != null)
					{
						if (guildBossMyRoomInfo.i64PlayPersonID > 0L)
						{
							if (drawTexture2 != null)
							{
								drawTexture2.Visible = true;
							}
							if (itemTexture != null)
							{
								itemTexture.Visible = true;
							}
							NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(guildBossMyRoomInfo.i64PlayPersonID);
							if (memberInfoFromPersonID != null)
							{
								if (memberInfoFromPersonID.GetPortrait() != null)
								{
									itemTexture.SetTexture(memberInfoFromPersonID.GetPortrait());
									itemTexture.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickMemberInfo));
									itemTexture.Data = guildBossMyRoomInfo.i64PlayPersonID;
								}
								else
								{
									itemTexture.SetSolImageTexure(eCharImageType.SMALL, new NkListSolInfo
									{
										SolCharKind = memberInfoFromPersonID.GetFaceCharKind(),
										SolGrade = -1,
										SolLevel = memberInfoFromPersonID.GetLevel()
									}, false);
									itemTexture.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickMemberInfo));
									itemTexture.Data = guildBossMyRoomInfo.i64PlayPersonID;
								}
							}
						}
						string effectKey = string.Empty;
						if (guildBossMyRoomInfo != null)
						{
							effectKey = this.GetRoomStateEffect(guildBossMyRoomInfo.byRoomState);
						}
						else
						{
							effectKey = this.GetRoomStateEffect(0);
						}
						if (uIButton != null)
						{
							uIButton.DeleteChildEffect();
							NrTSingleton<FormsManager>.Instance.AttachEffectKey(effectKey, uIButton, uIButton.GetSize());
						}
					}
					break;
				}
			}
		}
	}
}
