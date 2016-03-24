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

	private Button upButton;

	private Button downButton;

	private Button m_btInventory;

	private DrawTexture up;

	private DrawTexture down;

	private Button m_btClose;

	private Button m_btGuildBoss;

	private Button m_btRepeat;

	private Button m_btSolInfo;

	private Label m_laSolInfo;

	private Box m_bGuildBoosicon;

	private Label m_laTitle;

	private DrawTexture m_dwTitleBar;

	private Button m_btReSelectMode;

	private DrawTexture m_dtLock;

	private UIButton _GuideItem;

	private short m_nFloorType = 1;

	private List<int> m_kFloorInfo = new List<int>();

	private bool m_bShowGuide;

	private short m_nSelectindex = -1;

	private bool m_bLoadComplete;

	private int m_nWinID;

	private int m_nGuideFloor;

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
		this.m_dtEffect = (base.GetControl("DrawTexture_Effect") as DrawTexture);
		this.m_lbFloor = (base.GetControl("NewListBox_floor") as NewListBox);
		this.m_lbFloor.SelectStyle = "Com_B_Transparent";
		this.m_lbFloor.AddRightMouseDelegate(new EZValueChangedDelegate(this.BtClickFloor));
		this.m_lbFloor.AddScrollDelegate(new EZScrollDelegate(this.ChangeFloorInfo));
		this.m_lbFloor.AddMakeCompleteDelegate(new EZValueChangedDelegate(this.ScrollFloor));
		this.m_lbFloor.Reserve = false;
		this.m_lbFloor.AutoScroll = true;
		this.m_btOpenRoomList = (base.GetControl("Button_roomlist") as Button);
		this.m_btOpenRoomList.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickOpenRoomList));
		this.upButton = (base.GetControl("Button_slideup") as Button);
		this.downButton = (base.GetControl("Button_slidedown") as Button);
		this.m_btInventory = (base.GetControl("BT_Inventory") as Button);
		this.m_btInventory.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickInventoyOpen));
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
		this.down = (base.GetControl("DrawTexture_SlideBG02") as DrawTexture);
		this.downButton.SetLocation(this.downButton.GetLocation().x, this.down.GetLocationY() + 36f, this.downButton.GetLocation().z - 1.1f);
		this.upButton.Visible = false;
		this.up.Visible = false;
		this.m_btClose = (base.GetControl("Button_close") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtClickClose));
		this.m_btGuildBoss = (base.GetControl("Button_GuildBoss") as Button);
		Button expr_282 = this.m_btGuildBoss;
		expr_282.Click = (EZValueChangedDelegate)Delegate.Combine(expr_282.Click, new EZValueChangedDelegate(this.OnClickGuildBoss));
		this.m_btRepeat = (base.GetControl("Button_Repeat") as Button);
		Button expr_2BF = this.m_btRepeat;
		expr_2BF.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2BF.Click, new EZValueChangedDelegate(this.OnClickRepeat));
		this.m_laSolInfo = (base.GetControl("Label_soldierinfo") as Label);
		this.m_laSolInfo.SetLocation(this.m_laSolInfo.GetLocationX(), this.m_laSolInfo.GetLocationY(), this.m_laSolInfo.GetLocation().z - 1.7f);
		this.m_btSolInfo = (base.GetControl("Button_soldierinfo") as Button);
		Button expr_34D = this.m_btSolInfo;
		expr_34D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_34D.Click, new EZValueChangedDelegate(this.OnClickSolInfo));
		this.m_bGuildBoosicon = (base.GetControl("Box_Notice") as Box);
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
		}
		this.m_laTitle = (base.GetControl("Label_title") as Label);
		this.m_dwTitleBar = (base.GetControl("DrawTexture_TitleBar") as DrawTexture);
		this.m_btReSelectMode = (base.GetControl("Button_Back") as Button);
		this.m_btReSelectMode.AddValueChangedDelegate(new EZValueChangedDelegate(this.ReSelectMode));
		this.m_btReSelectMode.Visible = false;
		this.GuildBossCheck();
		base.SetScreenCenter();
		UIDataManager.MuteSound(true);
		NrTSingleton<FiveRocksEventManager>.Instance.Placement("babeltower_enter");
		base.SetLayerZ(1, -1f);
		base.SetLayerZ(2, -1.2f);
		base.SetLayerZ(3, -1.4f);
		base.SetLayerZ(4, -1.6f);
		this.closeButton.SetLocationZ(this.m_dwTitleBar.GetLocation().z - 0.01f);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABEL_HARD_LEVEL);
		int level = kMyCharInfo.GetLevel();
		if (level > value && kMyCharInfo.GetBabelSubFloorRankInfo(100, 4, 1) > 0)
		{
			this.m_btReSelectMode.Visible = true;
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.GUILDCOLLECT_DLG);
	}

	private void ScrollFloor(IUIObject obj)
	{
		if (this.m_lbFloor.Count == this.m_lbFloor.MaxNum)
		{
			if (this.m_bShowGuide)
			{
				this.ExcuteUIGuide();
			}
			else
			{
				float scrollPosition = 0f;
				if (0 < this.m_lbFloor.LimitListNum - 1)
				{
					scrollPosition = (float)this.m_nSelectindex / (float)(this.m_lbFloor.LimitListNum - 1);
				}
				this.m_lbFloor.ScrollPosition = scrollPosition;
			}
			this.m_lbFloor.RepositionItems();
		}
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
			this.downButton.Visible = true;
			this.down.Visible = true;
		}
		else if (1f <= this.m_lbFloor.ScrollPosition)
		{
			this.upButton.Visible = true;
			this.up.Visible = true;
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
		this.HideUIGuide();
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
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
	}

	public NewListItem MakeFloorInfo(short floor)
	{
		short num = floor * 5;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		string text4 = string.Empty;
		bool flag = false;
		if (floor % 2 == 0)
		{
			flag = true;
		}
		if (flag)
		{
			text4 = string.Format("Mobile/DLG/BabelTower/newlistbox_floor_columndata{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		}
		else
		{
			text4 = string.Format("Mobile/DLG/BabelTower/newlistbox_floor1_columndata{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		}
		this.m_lbFloor.SetColumnData(text4);
		NewListItem newListItem = new NewListItem(this.m_lbFloor.ColumnNum, true, string.Empty);
		newListItem.m_szColumnData = text4;
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
			newListItem.SetListItemData(i * 6 + 6, false);
			newListItem.SetListItemData(i * 6 + 6 + 2, false);
			newListItem.SetListItemData(i * 6 + 6 + 4, false);
			newListItem.SetListItemData(i * 6 + 7, false);
			newListItem.SetListItemData(i * 6 + 7 + 2, false);
			newListItem.SetListItemData(i * 6 + 7 + 4, false);
			List<FRIEND_BABEL_CLEARINFO> babelFloor_FriendList = kMyCharInfo.m_kFriendInfo.GetBabelFloor_FriendList(num, this.m_nFloorType);
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
								nkListSolInfo.SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(friend.i32FaceCharCostumeUnique);
								newListItem.SetListItemData(i * 6 + 7 + j * 2, nkListSolInfo, num, new EZValueChangedDelegate(this.BtClickFloorFriendList), null);
							}
							else
							{
								newListItem.SetListItemData(i * 6 + 7 + j * 2, friendTexture, num, null, new EZValueChangedDelegate(this.BtClickFloorFriendList), null);
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
			if (num / 100 >= 1)
			{
				short num2 = num / 100;
				short num3 = num / 10 % 10;
				short num4 = num % 10;
				text = "Win_Number_" + num2;
				text2 = "Win_Number_" + num3;
				text3 = "Win_Number_" + num4;
				newListItem.SetListItemData(i + 6 + 41, true);
				newListItem.SetListItemData(i + 6 + 41, text, null, null, null);
				newListItem.SetListItemData(i + 41, true);
				newListItem.SetListItemData(i + 41, text2, null, null, null);
				newListItem.SetListItemData(i + 5 + 41, true);
				newListItem.SetListItemData(i + 5 + 41, text3, null, null, null);
			}
			else if (num / 10 <= 0 && num % 10 > 0)
			{
				text = "Win_Number_" + num;
				newListItem.SetListItemData(i + 36, true);
				newListItem.SetListItemData(i + 36, text, null, null, null);
				newListItem.SetListItemData(i + 6 + 41, false);
			}
			else
			{
				short num5 = num / 10;
				short num6 = num % 10;
				text = "Win_Number_" + num5;
				text2 = "Win_Number_" + num6;
				newListItem.SetListItemData(i + 41, true);
				newListItem.SetListItemData(i + 41, text, null, null, null);
				newListItem.SetListItemData(i + 5 + 41, true);
				newListItem.SetListItemData(i + 5 + 41, text2, null, null, null);
				if (num6 == 0)
				{
					newListItem.SetListItemData(i + 6 + 41, false);
				}
			}
			if (num >= 9)
			{
				newListItem.SetListItemData(50, true);
			}
			byte babelFloorRankInfo = kMyCharInfo.GetBabelFloorRankInfo(num, this.m_nFloorType);
			bool treasure = kMyCharInfo.IsBabelTreasure(num, this.m_nFloorType);
			UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(NrTSingleton<BabelTowerManager>.Instance.GetBabelRankImgText(babelFloorRankInfo, treasure));
			newListItem.SetListItemData(i + 1, loader, num, new EZValueChangedDelegate(this.BtClickFloor), null);
			num -= 1;
		}
		return newListItem;
	}

	public void ShowList()
	{
		this.m_bLoadComplete = false;
		this.m_kFloorInfo.Clear();
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
			string text4 = string.Empty;
			if (flag)
			{
				text4 = string.Format("Mobile/DLG/BabelTower/newlistbox_floor_columndata{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			}
			else
			{
				text4 = string.Format("Mobile/DLG/BabelTower/newlistbox_floor1_columndata{0}", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			}
			this.m_lbFloor.SetColumnData(text4);
			NewListItem newListItem = new NewListItem(this.m_lbFloor.ColumnNum, true, string.Empty);
			newListItem.m_szColumnData = text4;
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
									nkListSolInfo.SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(friend.i32FaceCharCostumeUnique);
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
			newListItem.Data = (int)(num5 - 1);
			flag = !flag;
			this.m_lbFloor.Add(newListItem);
			this.m_kFloorInfo.Add((int)num5);
		}
		this.m_lbFloor.RepositionItems();
		if (!this.m_lbFloor.ReUse)
		{
			this.m_lbFloor.SetSelectedItem((int)this.m_nSelectindex);
		}
	}

	private void ChangeFloorInfo(IUIObject obj, int index)
	{
		if (index >= this.m_kFloorInfo.Count)
		{
			return;
		}
		int num = this.m_kFloorInfo[index];
		NewListItem newListItem = this.MakeFloorInfo((short)num);
		this.m_lbFloor.SetColumnData(newListItem.m_szColumnData);
		this.m_lbFloor.UpdateContents(index, newListItem);
	}

	public bool IsLoadComplete()
	{
		return this.m_bLoadComplete;
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
		this.HideUIGuide();
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
			solMilitarySelectDlg.CloseByParent(82);
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
			msgBoxTwoCheckUI.SetMsg(new YesDelegate(BabelTowerMainDlg.RepeatBabelStart), msgBoxTwoCheckUI, null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("185"), empty2, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("354"), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("263"), new CheckBox2Delegate(BabelTowerMainDlg.CheckBattleSpeedCount), eMsgType.MB_CHECK12_OK_CANCEL);
		}
	}

	public void OnClickInventoyOpen(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.INVENTORY_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.INVENTORY_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.INVENTORY_DLG);
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
			if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
			{
				Battle_ResultDlg_Content battle_ResultDlg_Content = (Battle_ResultDlg_Content)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_CONTENT_DLG);
				if (battle_ResultDlg_Content != null)
				{
					battle_ResultDlg_Content.Close();
				}
			}
			MsgBoxTwoCheckUI msgBoxTwoCheckUI = (MsgBoxTwoCheckUI)a_oObject;
			if (msgBoxTwoCheckUI == null)
			{
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo != null)
			{
				long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_BATTLESPEED_COUNT);
				if (msgBoxTwoCheckUI.IsChecked(2) && charSubData > 0L)
				{
					MsgBoxAutoSellUI msgBoxAutoSellUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_AUTOSELL_DLG) as MsgBoxAutoSellUI;
					msgBoxAutoSellUI.SetLoadData(msgBoxTwoCheckUI.IsChecked(1), msgBoxTwoCheckUI.IsChecked(2), MsgBoxAutoSellUI.eMODE.BABEL_TOWER);
				}
				else
				{
					NrTSingleton<NkBabelMacroManager>.Instance.Start(msgBoxTwoCheckUI.IsChecked(1), false);
				}
			}
			else
			{
				NrTSingleton<NkBabelMacroManager>.Instance.Start(msgBoxTwoCheckUI.IsChecked(1), false);
			}
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
		bool flag = false;
		bool guildBossRewardInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossRewardInfo();
		if (guildBossRewardInfo)
		{
			flag = true;
		}
		bool guildBossCheck = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossCheck();
		if (guildBossCheck)
		{
			flag = true;
		}
		if (flag)
		{
			this.m_bGuildBoosicon.Hide(false);
		}
		else
		{
			this.m_bGuildBoosicon.Hide(true);
		}
	}

	public void GuildBossRewardCheck()
	{
		bool guildBossRewardInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossRewardInfo();
		if (guildBossRewardInfo)
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

	public void ReSelectMode(object a_oObhect)
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BABELTOWER_MODESELECT_DLG);
		this.Close();
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (!int.TryParse(param1, out this.m_nGuideFloor))
		{
			return;
		}
		this.m_bShowGuide = true;
		this.m_nWinID = winID;
		if (!this.m_lbFloor.ReUse)
		{
			this.ExcuteUIGuide();
		}
	}

	private void ExcuteUIGuide()
	{
		if (this.m_nWinID == 0 || this.m_nGuideFloor == 0)
		{
			return;
		}
		short num = NrTSingleton<BabelTowerManager>.Instance.GetLastFloor(this.m_nFloorType);
		short num2 = num / 5;
		for (short num3 = num2; num3 > 0; num3 -= 1)
		{
			for (int i = 4; i >= 0; i--)
			{
				if ((int)num == this.m_nGuideFloor)
				{
					this.m_nSelectindex = num2 - num3;
					break;
				}
				num -= 1;
			}
		}
		float scrollPosition = 0f;
		if (0 < this.m_lbFloor.LimitListNum - 1)
		{
			scrollPosition = (float)this.m_nSelectindex / (float)(this.m_lbFloor.LimitListNum - 1);
		}
		this.m_lbFloor.ScrollPosition = scrollPosition;
		this.m_lbFloor.SetSelectedItem((int)this.m_nSelectindex);
		UIListItemContainer selectItem = this.m_lbFloor.GetSelectItem();
		if (null == selectItem)
		{
			return;
		}
		int num4 = this.m_nGuideFloor % 5;
		if (num4 == 0)
		{
			num4 = 5;
		}
		this._GuideItem = (selectItem.GetElement(num4) as UIButton);
		if (null != this._GuideItem)
		{
			this._GuideItem.EffectAni = false;
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				Vector2 vector = new Vector2(base.GetLocationX() + this._GuideItem.GetLocationX() + 80f, base.GetLocationY() + this._GuideItem.GetLocationY() + 64f);
				uI_UIGuide.Move(vector, vector);
				this._GuideItem.AlphaAni(1f, 0.5f, -0.5f);
			}
			this.m_lbFloor.touchScroll = false;
		}
		this.m_bShowGuide = false;
	}

	public void HideUIGuide()
	{
		this.m_lbFloor.touchScroll = true;
		if (null != this._GuideItem)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			uI_UIGuide.Close();
		}
		this._GuideItem = null;
	}
}
