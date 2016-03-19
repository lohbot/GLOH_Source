using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class BabelTowerMainDlg : Form
{
	private const int LIST_FLOOR_COUNT = 5;

	private const int LIST_BUTTON_STARTINDEX = 1;

	private const int LIST_FLOOR_FRIENDBG = 6;

	private const int LIST_FLOOR_FRIEND_TEX = 7;

	private const int LIST_FLOORTEXTIMG_STARTINDEX = 36;

	private const int LIST_FLOORTEXTIMG_STARTINDEX_TWO = 41;

	private const int LIST_BG_INDEX = 0;

	private const int SHOW_FLOOR_COUNT = 5;

	private const int CHANGEMODE_REQUEST_CLEAR_FLOOR = 100;

	private const int CHANGEMODE_REQUEST_CLEAR_SUBFLOOR = 5;

	private DrawTexture m_dtEffect;

	private NewListBox m_lbFloor;

	private Button m_btOpenRoomList;

	private Label m_laroomlist;

	private Button upButton;

	private Button downButton;

	private DrawTexture up;

	private DrawTexture down;

	private Button m_btClose;

	private Label m_laClose;

	private DrawTexture m_dtClose;

	private Button m_btGuildBoss;

	private Button m_btRepeat;

	private Label m_laRepeat;

	private Button m_btSolInfo;

	private Label m_laSolInfo;

	private Box m_bGuildBoosicon;

	private Label m_laTitle;

	private DrawTexture m_dtLock;

	private short m_nFloorType = 1;

	private short m_nSelectindex = -1;

	private bool m_bLoadComplete;

	public short FloorType
	{
		get
		{
			return this.m_nFloorType;
		}
		set
		{
			this.m_nFloorType = value;
		}
	}

	public override void InitializeComponent()
	{
		NrTSingleton<FormsManager>.Instance.HideMainUI();
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		form.AlwaysUpdate = true;
		instance.LoadFileAll(ref form, "BabelTower/dlg_babel_main", G_ID.BABELTOWERMAIN_DLG, false, true);
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
		this.m_laroomlist = (base.GetControl("Label_roomlist") as Label);
		this.m_btOpenRoomList = (base.GetControl("Button_roomlist") as Button);
		this.m_btOpenRoomList.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickOpenRoomList));
		this.m_btOpenRoomList.SetLocation(this.m_btOpenRoomList.GetLocationX(), this.m_btOpenRoomList.GetLocationY(), this.m_btOpenRoomList.GetLocation().z - 0.7f);
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
		this.m_btGuildBoss = (base.GetControl("Button_GuildBoss") as Button);
		Button expr_385 = this.m_btGuildBoss;
		expr_385.Click = (EZValueChangedDelegate)Delegate.Combine(expr_385.Click, new EZValueChangedDelegate(this.OnClickGuildBoss));
		this.m_btGuildBoss.SetLocation(this.m_btGuildBoss.GetLocationX(), this.m_btGuildBoss.GetLocationY(), this.m_btGuildBoss.GetLocation().z - 0.7f);
		this.m_laRepeat = (base.GetControl("Label_repeat") as Label);
		this.m_btRepeat = (base.GetControl("Button_Repeat") as Button);
		Button expr_413 = this.m_btRepeat;
		expr_413.Click = (EZValueChangedDelegate)Delegate.Combine(expr_413.Click, new EZValueChangedDelegate(this.OnClickRepeat));
		this.m_btRepeat.SetLocation(this.m_btRepeat.GetLocationX(), this.m_btRepeat.GetLocationY(), this.m_btRepeat.GetLocation().z - 0.7f);
		this.m_laSolInfo = (base.GetControl("Label_soldierinfo") as Label);
		this.m_btSolInfo = (base.GetControl("Button_soldierinfo") as Button);
		Button expr_4A1 = this.m_btSolInfo;
		expr_4A1.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4A1.Click, new EZValueChangedDelegate(this.OnClickSolInfo));
		this.m_btSolInfo.SetLocation(this.m_btSolInfo.GetLocationX(), this.m_btSolInfo.GetLocationY(), this.m_btSolInfo.GetLocation().z - 0.7f);
		this.m_bGuildBoosicon = (base.GetControl("Box_Notice") as Box);
		this.m_bGuildBoosicon.SetLocation(this.m_bGuildBoosicon.GetLocationX(), this.m_bGuildBoosicon.GetLocationY(), this.m_bGuildBoosicon.GetLocation().z - 0.2f);
		this.m_bGuildBoosicon.Hide(true);
		this.m_dtLock = (base.GetControl("DT_Lock") as DrawTexture);
		this.m_dtLock.Visible = false;
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsGuildBoss())
		{
			this.m_bGuildBoosicon.Visible = false;
			this.m_btGuildBoss.Visible = false;
		}
		else
		{
			this.m_bGuildBoosicon.Visible = true;
			this.m_btGuildBoss.Visible = true;
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_TOWERBOSS_ON", this.m_btGuildBoss, this.m_btGuildBoss.GetSize());
		}
		this.m_laroomlist.SetLocation(this.m_laroomlist.GetLocationX(), this.m_laroomlist.GetLocationY(), this.m_laroomlist.GetLocation().z - 0.7f);
		this.m_laRepeat.SetLocation(this.m_laRepeat.GetLocationX(), this.m_laRepeat.GetLocationY(), this.m_laRepeat.GetLocation().z - 0.7f);
		this.m_laSolInfo.SetLocation(this.m_laSolInfo.GetLocationX(), this.m_laSolInfo.GetLocationY(), this.m_laSolInfo.GetLocation().z - 0.7f);
		this.m_dtClose.SetLocation(this.m_dtClose.GetLocationX(), this.m_dtClose.GetLocationY(), this.m_dtClose.GetLocation().z - 0.7f);
		this.m_laClose.SetLocation(this.m_laClose.GetLocationX(), this.m_laClose.GetLocationY(), this.m_laClose.GetLocation().z - 0.7f);
		this.m_btClose.SetLocation(this.m_btClose.GetLocationX(), this.m_btClose.GetLocationY(), this.m_btClose.GetLocation().z - 0.7f);
		this.m_dtLock.SetLocation(this.m_dtLock.GetLocationX(), this.m_dtLock.GetLocationY(), this.m_dtLock.GetLocation().z - 0.7f);
		this.m_laTitle = (base.GetControl("Label_title") as Label);
		this.GuildBossCheck();
		base.SetScreenCenter();
		UIDataManager.MuteSound(true);
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("babeltower_enter");
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
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWERSUB_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWER_OPENROOMLIST_DLG);
		NrTSingleton<FormsManager>.Instance.Main_UI_Show(FormsManager.eMAIN_UI_VISIBLE_MODE.COMMON);
		UIDataManager.MuteSound(false);
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
		base.OnClose();
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
		NrTSingleton<ChallengeManager>.Instance.CalcContinueRewardNoticeCount();
		NrTSingleton<ChallengeManager>.Instance.CalcDayRewardNoticeCount();
	}

	public void ShowList()
	{
		this.m_bLoadComplete = false;
		this.m_lbFloor.Clear();
		bool flag = true;
		short num;
		if (this.m_nFloorType == 2)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("BGM", "CHAOSTOWER", "HARDMODE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), string.Empty, true);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("AMB", "CHAOSTOWER", "HARDMODE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), string.Empty, true);
			num = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR_HARD, 1);
			this.m_laTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2782"));
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_BABEL_MAIN_HARD", this.m_dtEffect, this.m_dtEffect.GetSize());
		}
		else
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("BGM", "CHAOSTOWER", "START", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay), string.Empty, true);
			num = (short)PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR, 1);
			this.m_laTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1377"));
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_BABEL_MAIN", this.m_dtEffect, this.m_dtEffect.GetSize());
		}
		short num2 = NrTSingleton<BabelTowerManager>.Instance.GetLastFloor(this.m_nFloorType);
		int babelTowerLastFloor = NrTSingleton<ContentsLimitManager>.Instance.GetBabelTowerLastFloor(this.m_nFloorType);
		if (0 < babelTowerLastFloor && babelTowerLastFloor < (int)num2)
		{
			num2 = (short)babelTowerLastFloor;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		int num3 = 0;
		if (instance != null)
		{
			num3 = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABEL_REPEAT);
		}
		if (myCharInfo.GetLevel() < num3)
		{
			this.m_dtLock.Visible = true;
		}
		else
		{
			this.m_dtLock.Visible = false;
		}
		short num4 = num2 / 5;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		for (short num5 = num4; num5 > 0; num5 -= 1)
		{
			string columnData = string.Empty;
			if (flag)
			{
				columnData = string.Format("Mobile/DLG/BabelTower/newlistbox_floor_columndata{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			}
			else
			{
				columnData = string.Format("Mobile/DLG/BabelTower/newlistbox_floor1_columndata{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			}
			this.m_lbFloor.SetColumnData(columnData);
			NewListItem newListItem = new NewListItem(this.m_lbFloor.ColumnNum, true);
			if (!flag)
			{
				if (this.m_nFloorType == 2)
				{
					newListItem.SetListItemData(0, "UI/BabelTower/babel_hard_main1", true, null, null);
				}
				else
				{
					newListItem.SetListItemData(0, "UI/BabelTower/babel_main1", true, null, null);
				}
			}
			else if (this.m_nFloorType == 2)
			{
				newListItem.SetListItemData(0, "UI/BabelTower/babel_hard_main2", true, null, null);
			}
			else
			{
				newListItem.SetListItemData(0, "UI/BabelTower/babel_main2", true, null, null);
			}
			for (int i = 4; i >= 0; i--)
			{
				List<FRIEND_BABEL_CLEARINFO> babelFloor_FriendList = kMyCharInfo.m_kFriendInfo.GetBabelFloor_FriendList(num2, this.m_nFloorType);
				if (babelFloor_FriendList != null)
				{
					for (int j = 0; j < 3; j++)
					{
						if (babelFloor_FriendList.Count >= j + 1)
						{
							USER_FRIEND_INFO friend = kMyCharInfo.m_kFriendInfo.GetFriend(babelFloor_FriendList[j].i64FriendPersonID);
							if (friend != null)
							{
								Texture2D friendTexture = kMyCharInfo.GetFriendTexture(babelFloor_FriendList[j].i64FriendPersonID);
								if (friendTexture == null)
								{
									NkListSolInfo nkListSolInfo = new NkListSolInfo();
									nkListSolInfo.SolCharKind = friend.i32FaceCharKind;
									nkListSolInfo.SolGrade = -1;
									nkListSolInfo.SolLevel = friend.i16Level;
									newListItem.SetListItemData(i * 6 + 7 + j * 2, nkListSolInfo, num2, new EZValueChangedDelegate(this.BtClickFloorFriendList), null);
								}
								else
								{
									newListItem.SetListItemData(i * 6 + 7 + j * 2, friendTexture, num2, null, new EZValueChangedDelegate(this.BtClickFloorFriendList), null);
								}
								newListItem.SetListItemData(i * 6 + 6 + j * 2, !flag);
								newListItem.SetListItemData(i * 6 + 7 + j * 2, !flag);
							}
						}
						else
						{
							newListItem.SetListItemData(i * 6 + 6 + j * 2, false);
							newListItem.SetListItemData(i * 6 + 7 + j * 2, false);
						}
					}
				}
				else
				{
					newListItem.SetListItemData(i * 6 + 6, false);
					newListItem.SetListItemData(i * 6 + 6 + 2, false);
					newListItem.SetListItemData(i * 6 + 6 + 4, false);
					newListItem.SetListItemData(i * 6 + 7, false);
					newListItem.SetListItemData(i * 6 + 7 + 2, false);
					newListItem.SetListItemData(i * 6 + 7 + 4, false);
				}
				newListItem.SetListItemData(i + 36, false);
				newListItem.SetListItemData(i + 41, false);
				newListItem.SetListItemData(i + 5 + 41, false);
				if (num2 / 100 >= 1)
				{
					short num6 = num2 / 100;
					short num7 = num2 / 10 % 10;
					short num8 = num2 % 10;
					text = "Win_Number_" + num6;
					text2 = "Win_Number_" + num7;
					text3 = "Win_Number_" + num8;
					newListItem.SetListItemData(i + 6 + 41, true);
					newListItem.SetListItemData(i + 6 + 41, text, null, null, null);
					newListItem.SetListItemData(i + 41, true);
					newListItem.SetListItemData(i + 41, text2, null, null, null);
					newListItem.SetListItemData(i + 5 + 41, true);
					newListItem.SetListItemData(i + 5 + 41, text3, null, null, null);
				}
				else if (num2 / 10 <= 0 && num2 % 10 > 0)
				{
					text = "Win_Number_" + num2;
					newListItem.SetListItemData(i + 36, true);
					newListItem.SetListItemData(i + 36, text, null, null, null);
				}
				else
				{
					short num9 = num2 / 10;
					short num10 = num2 % 10;
					text = "Win_Number_" + num9;
					text2 = "Win_Number_" + num10;
					newListItem.SetListItemData(i + 41, true);
					newListItem.SetListItemData(i + 41, text, null, null, null);
					newListItem.SetListItemData(i + 5 + 41, true);
					newListItem.SetListItemData(i + 5 + 41, text2, null, null, null);
				}
				byte babelFloorRankInfo = kMyCharInfo.GetBabelFloorRankInfo(num2, this.m_nFloorType);
				bool treasure = kMyCharInfo.IsBabelTreasure(num2, this.m_nFloorType);
				UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(NrTSingleton<BabelTowerManager>.Instance.GetBabelRankImgText(babelFloorRankInfo, treasure));
				newListItem.SetListItemData(i + 1, loader, num2, new EZValueChangedDelegate(this.BtClickFloor), null);
				if (num2 == num)
				{
					this.m_nSelectindex = num4 - num5;
				}
				num2 -= 1;
			}
			flag = !flag;
			this.m_lbFloor.Add(newListItem);
		}
		this.m_lbFloor.RepositionItems();
		this.m_lbFloor.SetSelectedItem((int)this.m_nSelectindex);
	}

	public bool IsLoadComplete()
	{
		return this.m_bLoadComplete;
	}

	public void MoveFloor(IUIObject obj)
	{
		this.m_lbFloor.RepositionItems();
		if (-1 < this.m_nSelectindex)
		{
			this.m_lbFloor.SetSelectedItem((int)this.m_nSelectindex);
		}
		this.m_bLoadComplete = true;
	}

	public void BtClickFloor(IUIObject obj)
	{
		short floor = (short)obj.Data;
		BabelTowerSubDlg babelTowerSubDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWERSUB_DLG) as BabelTowerSubDlg;
		if (babelTowerSubDlg != null)
		{
			babelTowerSubDlg.ShowSubFloor(floor, this.m_nFloorType);
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CHAOSTOWER", "ENTER", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void BtClickOpenRoomList(IUIObject obj)
	{
		GS_BABELTOWER_OPENROOMLIST_GET_REQ gS_BABELTOWER_OPENROOMLIST_GET_REQ = new GS_BABELTOWER_OPENROOMLIST_GET_REQ();
		gS_BABELTOWER_OPENROOMLIST_GET_REQ.search_startfloor = 1;
		gS_BABELTOWER_OPENROOMLIST_GET_REQ.search_finishfloor = 10;
		gS_BABELTOWER_OPENROOMLIST_GET_REQ.FloorType = this.m_nFloorType;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_OPENROOMLIST_GET_REQ, gS_BABELTOWER_OPENROOMLIST_GET_REQ);
	}

	public void BtClickFloorFriendList(IUIObject obj)
	{
		short num = (short)obj.Data;
		Debug.LogError(string.Concat(new object[]
		{
			"BtClickFloorFriendList Floor = ",
			num,
			" Type = ",
			this.m_nFloorType
		}));
		BabelTower_FriendList babelTower_FriendList = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWER_FRIENDLIST) as BabelTower_FriendList;
		if (babelTower_FriendList != null)
		{
			babelTower_FriendList.SetData(num, this.m_nFloorType);
		}
	}

	public void BtClickClose(IUIObject obj)
	{
		this.Close();
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("babeltower_out");
	}

	public void OnClickGuildBoss(IUIObject obj)
	{
		if (NrTSingleton<NewGuildManager>.Instance.GetGuildID() <= 0L)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("545"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		this.Close();
		BabelGuildBossDlg babelGuildBossDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABEL_GUILDBOSS_MAIN_DLG) as BabelGuildBossDlg;
		if (babelGuildBossDlg != null)
		{
			babelGuildBossDlg.ShowList();
		}
	}

	public void OnClickSolInfo(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowHide(G_ID.SOLMILITARYGROUP_DLG);
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG).Visible)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY-INFORMATION", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY-INFORMATION", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		SolMilitarySelectDlg solMilitarySelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYSELECT_DLG) as SolMilitarySelectDlg;
		if (solMilitarySelectDlg != null)
		{
			solMilitarySelectDlg.CloseByParent(79);
		}
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("solinfodlg_open");
	}

	public void OnClickRepeat(IUIObject obj)
	{
		if (Scene.CurScene != Scene.Type.BATTLE)
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			int num = 0;
			int num2 = 0;
			if (myCharInfo.ColosseumMatching)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("615"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				return;
			}
			if (instance != null)
			{
				if (NrTSingleton<ContentsLimitManager>.Instance.IsVipExp())
				{
					num2 = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BATTLE_REPEAT);
				}
				else
				{
					short vipLevelAddBattleRepeat = NrTSingleton<NrTableVipManager>.Instance.GetVipLevelAddBattleRepeat();
					num2 = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BATTLE_REPEAT) + (int)vipLevelAddBattleRepeat;
				}
				num = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABEL_REPEAT);
			}
			if (myCharInfo.GetLevel() < num)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("781"),
					"level",
					num
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				return;
			}
			string text = " ";
			int @int;
			int num3;
			if (this.m_nFloorType == 2)
			{
				@int = PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR_HARD, 0);
				num3 = PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR_HARD, -1);
			}
			else
			{
				@int = PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELFLOOR, 0);
				num3 = PlayerPrefs.GetInt(NrPrefsKey.LASTPLAY_BABELSUBFLOOR, -1);
			}
			if (@int <= 0 || num3 < 0)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("614");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				return;
			}
			num3++;
			MsgBoxTwoCheckUI msgBoxTwoCheckUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_TWOCHECK_DLG) as MsgBoxTwoCheckUI;
			if (this.m_nFloorType == 2)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2784");
			}
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("186"),
				"type",
				text,
				"floor",
				@int.ToString(),
				"subfloor",
				num3.ToString(),
				"count",
				num2.ToString()
			});
			msgBoxTwoCheckUI.SetCheckBoxState(1, false);
			msgBoxTwoCheckUI.SetCheckBoxState(2, false);
			msgBoxTwoCheckUI.SetMsg(new YesDelegate(BabelTowerMainDlg.RepeatBabelStart), msgBoxTwoCheckUI, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("185"), empty2, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("196"), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("263"), new CheckBox2Delegate(BabelTowerMainDlg.CheckBattleSpeedCount), eMsgType.MB_CHECK12_OK_CANCEL);
		}
	}

	public static void RepeatBabelStart(object a_oObject)
	{
		if (Scene.CurScene != Scene.Type.BATTLE)
		{
			if (a_oObject == null)
			{
				return;
			}
			Battle_ResultDlg_Content battle_ResultDlg_Content = (Battle_ResultDlg_Content)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_CONTENT_DLG);
			if (battle_ResultDlg_Content != null)
			{
				battle_ResultDlg_Content.Close();
			}
			MsgBoxTwoCheckUI msgBoxTwoCheckUI = (MsgBoxTwoCheckUI)a_oObject;
			if (msgBoxTwoCheckUI == null)
			{
				return;
			}
			NrTSingleton<NkBabelMacroManager>.Instance.Start(msgBoxTwoCheckUI.IsChecked(1), msgBoxTwoCheckUI.IsChecked(2));
		}
	}

	public static void CheckBattleSpeedCount(object a_oObject)
	{
		if (Scene.CurScene != Scene.Type.BATTLE)
		{
			if (a_oObject == null)
			{
				return;
			}
			if ((MsgBoxTwoCheckUI)a_oObject == null)
			{
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo != null)
			{
				long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_BATTLESPEED_COUNT);
				if (charSubData <= 0L)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("775");
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
			}
		}
	}

	public void GuildBossCheck()
	{
		bool guildBossCheck = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossCheck();
		if (guildBossCheck)
		{
			this.m_bGuildBoosicon.Hide(false);
		}
		else
		{
			this.m_bGuildBoosicon.Hide(true);
		}
	}

	public void CheckChangeMode(object a_oObject)
	{
		if (this.m_nFloorType == 2)
		{
			this.m_nFloorType = 1;
		}
		else
		{
			this.m_nFloorType = 2;
		}
		this.ShowList();
	}
}
