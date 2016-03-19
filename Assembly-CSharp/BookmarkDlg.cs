using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class BookmarkDlg : Form
{
	public enum TYPE
	{
		SOLINFO,
		SOLRECRUIT,
		SOLCOMPOSE,
		ADVENTURE,
		INVEN,
		COMMUNITY,
		NEWGUILD,
		WORLDMAP,
		BABEL,
		HEROBATTLE,
		INFIBATTLE,
		FIGHT,
		EXPLORATION,
		MINE,
		MAINEVENT,
		MAX_NUM
	}

	private ListBox bookmarkList;

	private Dictionary<BookmarkDlg.TYPE, FunDelegate> mapFun = new Dictionary<BookmarkDlg.TYPE, FunDelegate>();

	private Button upButton;

	private Button downButton;

	private DrawTexture up;

	private DrawTexture down;

	private Button hideButton;

	private DrawTexture hideArrow;

	private DrawTexture hideBG;

	private bool hide;

	private bool bCheckHide;

	private UIButton _Touch;

	private UIListItemContainer _GuideItem;

	private float _ButtonZ;

	private float oldZ;

	private int m_nWinID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Main/DLG_Bookmark", G_ID.BOOKMARK_DLG, false);
		base.ChangeSceneDestory = false;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		float num;
		if (NrTSingleton<UIDataManager>.Instance.ScaleMode)
		{
			num = GUICamera.height + GUICamera.height * 0.8f;
			base.SetSize(base.GetSize().x, num);
		}
		else
		{
			num = GUICamera.height;
			base.SetSize(base.GetSize().x, num);
		}
		this.bookmarkList = (base.GetControl("ListBox_ListBox0") as ListBox);
		this.bookmarkList.AutoListBox = false;
		this.bookmarkList.Reserve = false;
		this.bookmarkList.viewableArea = new Vector2(this.bookmarkList.GetSize().x, num);
		this.bookmarkList.UseColumnRect = true;
		this.bookmarkList.ColumnNum = 4;
		this.bookmarkList.itemSpacing = 30f;
		this.bookmarkList.SetColumnRect(0, new Rect(6f, 0f, 90f, 90f));
		this.bookmarkList.SetColumnRect(1, new Rect(0f, 87f, 100f, 28f), SpriteText.Anchor_Pos.Middle_Center, 22f);
		this.bookmarkList.SetColumnRect(2, new Rect(0f, 0f, 50f, 50f));
		this.bookmarkList.SetColumnRect(3, new Rect(0f, 0f, 50f, 50f), SpriteText.Anchor_Pos.Middle_Center, 28f);
		this.bookmarkList.AddSliderDelegate();
		this.bookmarkList.SetLocation(5, 0);
		this.mapFun.Add(BookmarkDlg.TYPE.SOLINFO, new FunDelegate(this.ClickSolInfo));
		this.mapFun.Add(BookmarkDlg.TYPE.SOLRECRUIT, new FunDelegate(this.ClickRecruitSol));
		this.mapFun.Add(BookmarkDlg.TYPE.SOLCOMPOSE, new FunDelegate(this.ClickCompose));
		this.mapFun.Add(BookmarkDlg.TYPE.INVEN, new FunDelegate(this.ClickInven));
		this.mapFun.Add(BookmarkDlg.TYPE.WORLDMAP, new FunDelegate(this.ClickWorldMap));
		this.mapFun.Add(BookmarkDlg.TYPE.FIGHT, new FunDelegate(this.ClickFightDlg));
		this.mapFun.Add(BookmarkDlg.TYPE.ADVENTURE, new FunDelegate(this.ClickAdventure));
		this.mapFun.Add(BookmarkDlg.TYPE.COMMUNITY, new FunDelegate(this.ClickCommunity));
		this.mapFun.Add(BookmarkDlg.TYPE.NEWGUILD, new FunDelegate(this.ClickNewGuild));
		this.mapFun.Add(BookmarkDlg.TYPE.HEROBATTLE, new FunDelegate(this.ClickHeroBattle));
		this.mapFun.Add(BookmarkDlg.TYPE.INFIBATTLE, new FunDelegate(this.ClickInfiBattle));
		this.mapFun.Add(BookmarkDlg.TYPE.BABEL, new FunDelegate(this.ClickBabel));
		this.mapFun.Add(BookmarkDlg.TYPE.EXPLORATION, new FunDelegate(this.ClickExploration));
		this.mapFun.Add(BookmarkDlg.TYPE.MINE, new FunDelegate(this.ClickMine));
		this.mapFun.Add(BookmarkDlg.TYPE.MAINEVENT, new FunDelegate(this.ClickEvent));
		this.SetBookmarkInfo();
		this.upButton = (base.GetControl("SlideGuideBTN01") as Button);
		this.upButton.SetLocation(this.upButton.GetLocation().x, this.upButton.GetLocationY(), this.upButton.GetLocation().z - 1.1f);
		this.downButton = (base.GetControl("SlideGuideBTN02") as Button);
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
		this.up = (base.GetControl("SlideGuideBG01") as DrawTexture);
		this.up.SetLocation(this.up.GetLocation().x, this.up.GetLocationY(), this.up.GetLocation().z - 1f);
		this.down = (base.GetControl("SlideGuideBtn01") as DrawTexture);
		this.down.SetLocation(this.down.GetLocation().x, num - this.down.GetSize().y, this.down.GetLocation().z - 1f);
		this.downButton.SetLocation(this.downButton.GetLocation().x, this.down.GetLocationY() + 76f, this.downButton.GetLocation().z - 1.1f);
		this.upButton.Visible = false;
		this.up.Visible = false;
		this.hideBG = (base.GetControl("DT_HideBG") as DrawTexture);
		num = (GUICamera.height - this.hideBG.GetSize().y) / 2f;
		this.hideBG.SetLocation(-this.hideBG.GetSize().x + 2f, num);
		this.hideArrow = (base.GetControl("DT_HideArrow") as DrawTexture);
		this.hideButton = (base.GetControl("Button_Hide") as Button);
		this.hideButton.SetValueChangedDelegate(new EZValueChangedDelegate(this.ClickHide));
		num = (GUICamera.height - this.hideArrow.GetSize().y) / 2f;
		this.hideArrow.SetLocation(-23f, num);
		num = (GUICamera.height - this.hideButton.GetSize().y) / 2f;
		this.hideButton.SetLocation(-this.hideButton.GetSize().x + 2f, num);
		base.SetLocation(GUICamera.width - base.GetSizeX(), 0f);
		this.ShowHideOption(true);
	}

	public void CheckHideBookmark()
	{
		if (!PlayerPrefs.HasKey(NrPrefsKey.HIDE_BOOKMARK))
		{
			PlayerPrefs.SetInt(NrPrefsKey.HIDE_BOOKMARK, 1);
		}
		else if (PlayerPrefs.GetInt(NrPrefsKey.HIDE_BOOKMARK) == 0)
		{
			this.ShowHideOption(false);
		}
		else if (PlayerPrefs.GetInt(NrPrefsKey.HIDE_BOOKMARK) == 1)
		{
			this.ShowHideOption(true);
		}
	}

	public void ShowHideOption(bool flag)
	{
		this.hideBG.Visible = flag;
		this.hideArrow.Visible = flag;
		this.hideButton.Visible = flag;
		if (this.hide && !flag)
		{
			this.Move();
		}
	}

	private void ClickHide(IUIObject obj)
	{
		this.Move();
	}

	public void ReMove()
	{
		if (this.bCheckHide)
		{
			this.bCheckHide = false;
		}
	}

	public void Move()
	{
		float num = (!this.hide) ? 1f : -1f;
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) == null)
		{
			return;
		}
		float value = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG).GetSizeX() * num;
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG);
		if (form != null)
		{
			form.Move(value);
		}
		form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_QUEST);
		if (form != null)
		{
			form.Move(value);
		}
		form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MENUICON_DLG);
		if (form != null)
		{
			form.Move(value);
		}
		form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_UI_ICON);
		if (form != null)
		{
			form.Move(value);
		}
		this.hide = !this.hide;
		this.hideArrow.Inverse(INVERSE_MODE.LEFT_TO_RIGHT);
	}

	private void ClickList(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		BookmarkDlg.TYPE key = (BookmarkDlg.TYPE)((int)obj.Data);
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.depthChangeable = true;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) != null)
		{
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				uI_UIGuide.CloseUI = true;
			}
			this.DeleteTouch();
		}
		if (this.mapFun.ContainsKey(key))
		{
			this.mapFun[key](null);
		}
		if (null != this._Touch)
		{
			this.DeleteTouch();
		}
	}

	public bool IsHide()
	{
		return this.bCheckHide;
	}

	public override void InitData()
	{
		if (!this.hide)
		{
			base.SetLocation(GUICamera.width - base.GetSizeX(), 0f);
		}
		for (int i = 0; i < this.bookmarkList.Count; i++)
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)this.bookmarkList.GetItem(i);
			if (null != uIListItemContainer)
			{
				UIButton uIButton = uIListItemContainer.GetElement(0) as UIButton;
				if (null != uIButton)
				{
					uIButton.transform.localPosition = new Vector3(6f, 0f, uIButton.transform.localPosition.z);
				}
			}
		}
	}

	public void ClickEvent(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.EVENT_MAIN))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.EVENT_MAIN);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EVENT_MAIN);
		}
	}

	public void ClickMine(IUIObject obj)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MINE_MAINSELECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.MINE_MAINSELECT_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MINE_MAINSELECT_DLG);
		}
	}

	private void ClickExploration(IUIObject obj)
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
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("180"),
				"level",
				instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_EXPLORATION_LV)
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
		{
			return;
		}
		DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
		if (directionDLG != null)
		{
			directionDLG.SetDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_EXPLORATION);
			directionDLG.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_EXPLORATION, 0);
		}
	}

	private void ClickSolInfo(IUIObject obj)
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

	private void ClickOutTerritory(IUIObject obj)
	{
		NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
		NrTSingleton<NkCharManager>.Instance.DeleteTerritoryChar();
	}

	private void ClickRecruitSol(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLRECRUIT_DLG))
		{
			GS_TICKET_SELL_INFO_REQ obj2 = new GS_TICKET_SELL_INFO_REQ();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TICKET_SELL_INFO_REQ, obj2);
			if (null != this._Touch)
			{
				NkInputManager.IsAutoBlockInputMode = true;
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLRECRUIT_DLG);
		}
	}

	private void ClickRepute(IUIObject obj)
	{
	}

	private void ClickInven(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowHide(G_ID.INVENTORY_DLG);
	}

	private void ClickMarket(IUIObject obj)
	{
	}

	private void ClickCompose(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_MAIN_DLG))
		{
			SolComposeMainDlg solComposeMainDlg = (SolComposeMainDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_MAIN_DLG);
			if (!solComposeMainDlg.Visible)
			{
				solComposeMainDlg.Show();
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLCOMPOSE_MAIN_DLG);
		}
	}

	private void ClickItemUpgrade(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG))
		{
			ReforgeMainDlg reforgeMainDlg = (ReforgeMainDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REFORGEMAIN_DLG);
			if (!reforgeMainDlg.Visible)
			{
				reforgeMainDlg.Show();
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REFORGEMAIN_DLG);
		}
	}

	private void ClickWorldMap(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WORLD_MAP))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.WORLD_MAP);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WORLD_MAP);
		}
	}

	private void ClickFightDlg(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int level = kMyCharInfo.GetLevel();
		int value = COLOSSEUM_CONSTANT_Manager.GetInstance().GetValue(eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_CHECK_LEVEL);
		if (level < value)
		{
			string empty = string.Empty;
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromNotify,
				"level",
				value.ToString()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
		{
			return;
		}
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COLOSSEUMMAIN_DLG))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMMAIN_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COLOSSEUMMAIN_DLG);
		}
	}

	private void ClickAdventure(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ADVENTURE_DLG))
		{
			AdventureDlg adventureDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ADVENTURE_DLG) as AdventureDlg;
			if (adventureDlg != null)
			{
				adventureDlg.DrawAdventure();
			}
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ADVENTURE", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			if (null != this._Touch)
			{
				NkInputManager.IsAutoBlockInputMode = true;
			}
			NrTSingleton<EventConditionHandler>.Instance.OpenUI.OnTrigger();
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ADVENTURE_DLG);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ADVENTURE", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	private void ClickCommunity(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COMMUNITY_DLG))
		{
			CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
			if (communityUI_DLG != null)
			{
				communityUI_DLG.RequestCommunityData(eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET);
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COMMUNITY_DLG);
		}
	}

	private void ClickNewGuild(IUIObject obj)
	{
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
		{
			return;
		}
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(1);
	}

	private void ClickHeroBattle(IUIObject obj)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsHeroBattle())
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			if (kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ENABLE_PLUNDER) == 0L)
			{
				PlunderAgreeDlg plunderAgreeDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_AGREE_DLG) as PlunderAgreeDlg;
				if (plunderAgreeDlg != null)
				{
					plunderAgreeDlg.Show();
					return;
				}
			}
		}
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
		{
			return;
		}
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERMAIN_DLG))
		{
			PlunderMainDlg plunderMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERMAIN_DLG) as PlunderMainDlg;
			if (plunderMainDlg != null)
			{
				plunderMainDlg.SetMode(eMODE.eMODE_PLUNDER);
				plunderMainDlg.SetTgValue(eMODE.eMODE_PLUNDER);
				plunderMainDlg.Show();
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERMAIN_DLG);
		}
	}

	private void ClickInfiBattle(IUIObject obj)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsInfiBattle())
		{
			if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
			{
				return;
			}
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERMAIN_DLG))
			{
				PlunderMainDlg plunderMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERMAIN_DLG) as PlunderMainDlg;
				if (plunderMainDlg != null)
				{
					plunderMainDlg.SetMode(eMODE.eMODE_INFIBATTLE);
					plunderMainDlg.SetTgValue(eMODE.eMODE_INFIBATTLE);
					plunderMainDlg.Show();
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERMAIN_DLG);
			}
		}
	}

	public void ClickBabel(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int level = kMyCharInfo.GetLevel();
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABELTOWER_LIMITLEVEL);
		if (level < value)
		{
			string empty = string.Empty;
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromNotify,
				"level",
				value.ToString()
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
		{
			return;
		}
		if (kMyCharInfo.m_kFriendInfo.GetFriendCount() > 0 && kMyCharInfo.m_kFriendInfo.GetFriendsBaBelDataCount() == 0)
		{
			GS_FRIENDS_BABELTOWER_CLEARINFO_REQ obj2 = new GS_FRIENDS_BABELTOWER_CLEARINFO_REQ();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIENDS_BABELTOWER_CLEARINFO_REQ, obj2);
		}
		int value2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABEL_HARD_LEVEL);
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWERMAIN_DLG))
		{
			if (level < value2)
			{
				DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
				if (directionDLG != null)
				{
					if (kMyCharInfo.GetLevel() > 20)
					{
						directionDLG.SetDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_REDUCE);
					}
					directionDLG.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_BABEL, 1);
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BABELTOWER_MODESELECT_DLG);
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWERMAIN_DLG);
		}
	}

	public void SetBookmarkInfo()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		this.bookmarkList.Clear();
		for (int i = 0; i < 15; i++)
		{
			ListItem listItem = this.GetListItem((BookmarkDlg.TYPE)i);
			if (listItem != null)
			{
				this.bookmarkList.Add(listItem);
			}
		}
		this.bookmarkList.RepositionItems();
	}

	public override void ChangedResolution()
	{
		if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
		{
			return;
		}
		if (TsPlatform.IsWeb)
		{
			float height = GUICamera.height + GUICamera.height * 0.43f;
			base.SetSize(base.GetSize().x, height);
		}
		else
		{
			float height = GUICamera.height;
			base.SetSize(base.GetSize().x, height);
		}
		base.SetLocation(GUICamera.width - base.GetSizeX(), 0f);
	}

	private ListItem GetListItem(BookmarkDlg.TYPE type)
	{
		ListItem listItem = new ListItem();
		switch (type)
		{
		case BookmarkDlg.TYPE.SOLINFO:
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo != null)
			{
				listItem.SetColumnGUIContent(0, string.Empty, "Main_B_ChaInfo", BookmarkDlg.TYPE.SOLINFO, new EZValueChangedDelegate(this.ClickList));
				listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1765"));
				if (0 < charPersonInfo.GetUpgradeBattleSkillNum())
				{
					listItem.SetColumnGUIContent(2, string.Empty, "Win_I_Notice01");
					listItem.SetColumnGUIContent(3, string.Empty, "Win_I_Notice03");
				}
				listItem.Key = type;
			}
			break;
		}
		case BookmarkDlg.TYPE.SOLRECRUIT:
		{
			int num = NkUserInventory.GetInstance().GetFunctionItemNum(eITEM_SUPPLY_FUNCTION.SUPPLY_GETSOLDIER);
			listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Employ", BookmarkDlg.TYPE.SOLRECRUIT, new EZValueChangedDelegate(this.ClickList));
			listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1704"));
			if (0 < num)
			{
				if (99 < num)
				{
					num = 99;
				}
				listItem.SetColumnGUIContent(2, string.Empty, "Win_I_Notice01");
				listItem.SetColumnStr(3, NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + num.ToString());
			}
			listItem.Key = type;
			break;
		}
		case BookmarkDlg.TYPE.SOLCOMPOSE:
			listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Synthesis", type, new EZValueChangedDelegate(this.ClickList));
			listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1722"));
			listItem.Key = type;
			break;
		case BookmarkDlg.TYPE.ADVENTURE:
			listItem.SetColumnGUIContent(0, string.Empty, "Main_B_AdvenRec.", type, new EZValueChangedDelegate(this.ClickList));
			listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("66"));
			if (NrTSingleton<NkAdventureManager>.Instance.IsAcceptQuest())
			{
				listItem.SetColumnGUIContent(2, string.Empty, "Win_I_Notice01");
				listItem.SetColumnGUIContent(3, string.Empty, "Win_I_Notice04");
			}
			listItem.Key = type;
			break;
		case BookmarkDlg.TYPE.INVEN:
			listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Inven", type, new EZValueChangedDelegate(this.ClickList));
			listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1771"));
			listItem.Key = type;
			break;
		case BookmarkDlg.TYPE.COMMUNITY:
			listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Invite", type, new EZValueChangedDelegate(this.ClickList));
			listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("210"));
			if (0 < NrTSingleton<NkCharManager>.Instance.AddExpHelpsolCount())
			{
				listItem.SetColumnGUIContent(2, string.Empty, "Win_I_Notice01");
				listItem.SetColumnStr(3, NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + string.Format("{0}", NrTSingleton<NkCharManager>.Instance.AddExpHelpsolCount()));
			}
			listItem.Key = type;
			break;
		case BookmarkDlg.TYPE.NEWGUILD:
		{
			listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Guild", BookmarkDlg.TYPE.SOLRECRUIT, new EZValueChangedDelegate(this.ClickNewGuild));
			listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("17"));
			int num2 = NrTSingleton<NewGuildManager>.Instance.GetReadyApplicantCount();
			bool guildBossCheck = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetGuildBossCheck();
			if (0 < num2 || guildBossCheck)
			{
				if (99 < num2)
				{
					num2 = 99;
				}
				if (0 < num2)
				{
					listItem.SetColumnGUIContent(2, string.Empty, "Win_I_Notice01");
					listItem.SetColumnStr(3, NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + num2.ToString());
				}
				else if (guildBossCheck)
				{
					listItem.SetColumnGUIContent(2, string.Empty, "Win_I_Notice04");
				}
			}
			listItem.Key = type;
			break;
		}
		case BookmarkDlg.TYPE.WORLDMAP:
			listItem.SetColumnGUIContent(0, string.Empty, "Main_B_WorldMap", type, new EZValueChangedDelegate(this.ClickList));
			listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1769"));
			listItem.Key = type;
			break;
		case BookmarkDlg.TYPE.BABEL:
			listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Babel", type, new EZValueChangedDelegate(this.ClickList));
			listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("630"));
			listItem.Key = type;
			break;
		case BookmarkDlg.TYPE.HEROBATTLE:
			if (NrTSingleton<ContentsLimitManager>.Instance.IsHeroBattle())
			{
				listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Pillage", type, new EZValueChangedDelegate(this.ClickList));
				listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("43"));
				listItem.Key = type;
			}
			else
			{
				listItem = null;
			}
			break;
		case BookmarkDlg.TYPE.INFIBATTLE:
			if (NrTSingleton<ContentsLimitManager>.Instance.IsInfiBattle())
			{
				listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Pillage", type, new EZValueChangedDelegate(this.ClickList));
				listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2218"));
				listItem.Key = type;
			}
			else
			{
				listItem = null;
			}
			break;
		case BookmarkDlg.TYPE.FIGHT:
		{
			listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Expedition", type, new EZValueChangedDelegate(this.ClickList));
			listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("577"));
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo.ColosseumOldRank > 0)
			{
				listItem.SetColumnGUIContent(2, string.Empty, "Win_I_Notice01");
				listItem.SetColumnGUIContent(3, string.Empty, "Win_I_Notice04");
			}
			listItem.Key = type;
			break;
		}
		case BookmarkDlg.TYPE.EXPLORATION:
			if (NrTSingleton<ContentsLimitManager>.Instance.IsExploration())
			{
				listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Exploration", type, new EZValueChangedDelegate(this.ClickList));
				listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("846"));
				listItem.Key = type;
			}
			else
			{
				listItem = null;
			}
			break;
		case BookmarkDlg.TYPE.MINE:
			if (!NrTSingleton<ContentsLimitManager>.Instance.IsMineApply((short)NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel()) || !NrTSingleton<ContentsLimitManager>.Instance.IsExpeditionLevel(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel()))
			{
				listItem = null;
			}
			else
			{
				listItem.SetColumnGUIContent(0, string.Empty, "Main_B_MineWar", type, new EZValueChangedDelegate(this.ClickList));
				listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1625"));
				listItem.Key = type;
			}
			break;
		case BookmarkDlg.TYPE.MAINEVENT:
		{
			int num3 = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.CurrentEventCount();
			listItem.SetColumnGUIContent(0, string.Empty, "Main_B_Event", type, new EZValueChangedDelegate(this.ClickList));
			listItem.SetColumnGUIContent(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("433"));
			if (0 < num3)
			{
				if (99 < num3)
				{
					num3 = 99;
				}
				listItem.SetColumnGUIContent(2, string.Empty, "Win_I_Notice01");
				listItem.SetColumnStr(3, NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + num3.ToString());
			}
			listItem.Key = type;
			break;
		}
		default:
			listItem = null;
			break;
		}
		return listItem;
	}

	public void UpdateBookmarkInfo(BookmarkDlg.TYPE type)
	{
		for (int i = 0; i < this.bookmarkList.Count; i++)
		{
			IUIObject item = this.bookmarkList.GetItem(i);
			if (item != null)
			{
				BookmarkDlg.TYPE tYPE = (BookmarkDlg.TYPE)((int)item.Data);
				if (tYPE == type)
				{
					ListItem listItem = this.GetListItem(type);
					this.bookmarkList.RemoveAdd(i, listItem);
					break;
				}
			}
		}
		this.bookmarkList.RepositionItems();
		this.bookmarkList.clipWhenMoving = true;
		if (null != this._GuideItem)
		{
			this._GuideItem.gameObject.transform.localPosition = new Vector3(this._GuideItem.gameObject.transform.localPosition.x, this._GuideItem.gameObject.transform.localPosition.y, -3f);
		}
	}

	public override void AfterShow()
	{
		this.UpdateHide();
	}

	private void ClickGameGuide(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.GAMEGUIDE_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.GAMEGUIDE_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.GAMEGUIDE_DLG);
		}
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.depthChangeable = false;
		}
		this.m_nWinID = winID;
		this.oldZ = base.GetLocation().z;
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), NrTSingleton<FormsManager>.Instance.GetTopMostZ() + 1f);
		BookmarkDlg.TYPE tYPE = (BookmarkDlg.TYPE)((int)Enum.Parse(typeof(BookmarkDlg.TYPE), param1));
		this._GuideItem = (this.bookmarkList.GetItem((int)tYPE) as UIListItemContainer);
		UIButton uIButton = this._GuideItem.GetElement(0) as UIButton;
		if (null != uIButton)
		{
			uIButton.EffectAni = false;
		}
		this._ButtonZ = this._GuideItem.gameObject.transform.localPosition.z;
		this._GuideItem.gameObject.transform.localPosition = new Vector3(this._GuideItem.gameObject.transform.localPosition.x, this._GuideItem.gameObject.transform.localPosition.y, -3f);
		if (!this.hide)
		{
			this._GuideItem.AlphaAni(1f, 0.5f, -0.5f);
		}
		this.bookmarkList.ScrollPosition = (float)tYPE / 15f;
		if (null == this._Touch)
		{
			this._Touch = UICreateControl.Button("touch", "Main_I_Touch01", 196f, 154f);
			this._Touch.PlayAni(true);
			base.InteractivePanel.MakeChild(this._Touch.gameObject);
			float y = this.bookmarkList.viewableArea.y / 2f - this._GuideItem.gameObject.transform.localPosition.y - this.bookmarkList.MoverPosY();
			this._Touch.SetLocation(-this._Touch.GetSize().x + 75f, y, this._GuideItem.gameObject.transform.localPosition.z - 1f);
			BoxCollider component = this._Touch.gameObject.GetComponent<BoxCollider>();
			if (null != component)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
		if (this.hide)
		{
			this.Move();
		}
		this.bookmarkList.touchScroll = false;
	}

	public void UpdateHide()
	{
		if (this.hide)
		{
			this.bCheckHide = true;
		}
	}

	public void HideUIGuide()
	{
		this.bookmarkList.touchScroll = true;
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), this.oldZ);
		if (null != this._GuideItem)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			this._GuideItem.StopAni();
			this._GuideItem.AlphaAni(1f, 1f, 0f);
			UIButton uIButton = this._GuideItem.GetElement(0) as UIButton;
			if (null != uIButton)
			{
				uIButton.EffectAni = true;
			}
			this._GuideItem.gameObject.transform.localPosition = new Vector3(this._GuideItem.gameObject.transform.localPosition.x, this._GuideItem.gameObject.transform.localPosition.y, this._ButtonZ);
		}
		this._GuideItem = null;
	}

	private void DeleteTouch()
	{
		if (null != this._Touch)
		{
			base.InteractivePanel.RemoveChild(this._Touch.gameObject);
			UnityEngine.Object.Destroy(this._Touch.gameObject);
			this._Touch = null;
		}
	}

	public override void Update()
	{
		if (!this.bookmarkList.changeScrollPos)
		{
			return;
		}
		if (0.01f >= this.bookmarkList.ScrollPosition)
		{
			this.upButton.Visible = false;
			this.up.Visible = false;
		}
		else if (1f <= this.bookmarkList.ScrollPosition)
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

	public bool IsListAdd(BookmarkDlg.TYPE eTYPE)
	{
		for (int i = 0; i < this.bookmarkList.Count; i++)
		{
			UIListItemContainer uIListItemContainer = this.bookmarkList.GetItem(i) as UIListItemContainer;
			if (!(null == uIListItemContainer))
			{
				if (uIListItemContainer.Data != null)
				{
					if (eTYPE == (BookmarkDlg.TYPE)((int)uIListItemContainer.Data))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void SetGuildApplicantGet(int iCount)
	{
	}
}
