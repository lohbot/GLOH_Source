using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class WorldMapDlg : Form
{
	private const float MAP_X_ADD = 20f;

	private const float MAP_Y_ADD = 64f;

	private const float BUTTON_TO_LABEL_X = -107f;

	private const float BUTTON_TO_LABEL_Y = 84f;

	private const float MAPICON_TO_USERICON_X = -10f;

	private const float MAPICON_TO_USERICON_Y = -80f;

	private Label m_lbWorldMap_Title;

	private DrawTexture m_dtWorldMap_Map;

	private UIButton[] m_btWorldMap_AreaIcon;

	private Label[] m_lbWorldMap_AreaName;

	private DrawTexture m_dtLocalMap_Map;

	private DrawTexture m_dtLocalMap_Day;

	private DrawTexture m_dtLocalMap_Night;

	private UIButton[] m_btLocalMap_AreaIcon;

	private Label[] m_lbLocalMap_AreaName;

	private Button m_btLocalMap_ReWorld;

	private Button m_btLocalMap_Night;

	private Button m_btLocalMap_Day;

	private Button m_btLocalMap_NpcAutoMove;

	private Button m_btUser_User;

	private DrawTexture m_dtUser_Icon;

	private TreasureData[] m_pTreasure = new TreasureData[20];

	private WORLDMAP_INFO m_WorldMapInfo;

	private int m_MaxLocalMapCount;

	private LOCALMAP_INFO m_selectLocalMap;

	private MAP_INFO m_selectMap;

	private NrCharMapInfo m_pkCharMapInfo;

	private bool m_bNowNightMode;

	private bool m_bNowLocalMap;

	private UIButton _GuideItem;

	private float _ButtonZ;

	private int m_nWinID;

	public bool NowNightMode
	{
		get
		{
			return this.m_bNowNightMode;
		}
		private set
		{
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "DLG_worldmap", G_ID.WORLD_MAP, true);
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.draggable = false;
		}
		base.ShowBlackBG(1f);
	}

	public override void InitData()
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NPCTALK_DLG))
		{
			this.Close();
		}
		base.InitData();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MAP", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public override void SetComponent()
	{
		this.ResizeDlg();
		this._SetComponetBasic();
	}

	public void _SetComponetBasic()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		instance.CreateCloseButton(ref this.closeButton, UIDataManager.closeButtonName, base.Scale);
		this.m_lbWorldMap_Title = (base.GetControl("LB_Title") as Label);
		this.m_dtWorldMap_Map = (base.GetControl("DT_map") as DrawTexture);
		this.m_dtLocalMap_Map = (base.GetControl("DT_AreaBG") as DrawTexture);
		this.m_dtLocalMap_Day = (base.GetControl("DT_Day") as DrawTexture);
		this.m_dtLocalMap_Day.Visible = false;
		this.m_dtLocalMap_Night = (base.GetControl("DT_Night") as DrawTexture);
		this.m_dtLocalMap_Night.SetLocation(this.m_dtLocalMap_Night.GetLocationX(), this.m_dtLocalMap_Night.GetLocationY(), -0.004f);
		this.m_dtLocalMap_Night.Visible = false;
		this.m_btLocalMap_ReWorld = (base.GetControl("BT_WorldMap") as Button);
		this.m_btLocalMap_ReWorld.EffectAni = false;
		Button expr_F2 = this.m_btLocalMap_ReWorld;
		expr_F2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_F2.Click, new EZValueChangedDelegate(this.OnClickLocalMapReWorld));
		this.m_btLocalMap_Night = (base.GetControl("BT_Night") as Button);
		Button expr_12F = this.m_btLocalMap_Night;
		expr_12F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_12F.Click, new EZValueChangedDelegate(this.OnClickLocalMapNight));
		this.m_btLocalMap_Day = (base.GetControl("BT_Day") as Button);
		Button expr_16C = this.m_btLocalMap_Day;
		expr_16C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_16C.Click, new EZValueChangedDelegate(this.OnClickLocalMapDay));
		this.m_btLocalMap_NpcAutoMove = (base.GetControl("BT_NpcAutoMove") as Button);
		Button expr_1A9 = this.m_btLocalMap_NpcAutoMove;
		expr_1A9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1A9.Click, new EZValueChangedDelegate(this.OnClickLocalMapNpcAutoMove));
		this.m_btLocalMap_NpcAutoMove.EffectAni = false;
		this.m_btUser_User = (base.GetControl("BT_UserBG") as Button);
		this.m_btUser_User.controlIsEnabled = false;
		this.m_dtUser_Icon = (base.GetControl("DT_UserImg") as DrawTexture);
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		Texture2D texture2D = null;
		if (myCharInfo != null)
		{
			int num = myCharInfo.GetFaceCharKind();
			int solgrade = (int)myCharInfo.GetFaceSolGrade();
			if (num == 0)
			{
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser != null)
				{
					NkSoldierInfo soldierInfo = nrCharUser.GetPersonInfoUser().GetSoldierInfo(0);
					if (soldierInfo != null && soldierInfo.IsValid())
					{
						num = soldierInfo.GetCharKind();
						solgrade = (int)soldierInfo.GetGrade();
					}
				}
			}
			if (myCharInfo.UserPortrait)
			{
				texture2D = myCharInfo.UserPortraitTexture;
			}
			if (texture2D != null)
			{
				this.m_dtUser_Icon.SetTexture(texture2D);
			}
			else
			{
				string costumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(myCharInfo.GetFaceCostumeUnique());
				this.m_dtUser_Icon.SetTexture(eCharImageType.SMALL, num, solgrade, costumePortraitPath);
			}
		}
		this.m_btUser_User.Visible = false;
		this.m_dtUser_Icon.Visible = false;
		this.m_WorldMapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetWorldMapInfo("1");
		if (this.m_WorldMapInfo == null)
		{
			return;
		}
		this.m_MaxLocalMapCount = NrTSingleton<NrBaseTableManager>.Instance.GetLocalMapCount();
		this.m_btWorldMap_AreaIcon = new UIButton[this.m_MaxLocalMapCount];
		this.m_lbWorldMap_AreaName = new Label[this.m_MaxLocalMapCount];
		this.m_pkCharMapInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo;
		this.m_selectLocalMap = NrTSingleton<NrBaseTableManager>.Instance.GetLocalMapInfoFromMapIndex(this.m_pkCharMapInfo.m_nMapIndex);
		if (this.m_selectLocalMap == null)
		{
			this.Close();
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.m_selectLocalMap.LOCALMAP_NAME_TEXT_INDEX);
		this.m_lbWorldMap_Title.SetText(textFromInterface);
		this.m_dtLocalMap_Map.SetTextureFromBundle(this.m_selectLocalMap.GetBundlePath());
		MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(this.m_pkCharMapInfo.m_nMapIndex.ToString());
		if (mapInfo != null && mapInfo.MAP_NIGHTMODE == 1)
		{
			this.m_bNowNightMode = true;
		}
		string name = string.Empty;
		string name2 = string.Empty;
		this.m_btLocalMap_AreaIcon = new UIButton[20];
		this.m_lbLocalMap_AreaName = new Label[20];
		for (int i = 0; i < 20; i++)
		{
			name = "BT_LocalMapAreaIcon" + i.ToString();
			name2 = "LB_LocalMapAreaName" + i.ToString();
			this.m_btLocalMap_AreaIcon[i] = UICreateControl.Button(name, "Main_B_Map", 64f, 64f);
			this.m_lbLocalMap_AreaName[i] = UICreateControl.Label(name2, null, false, 280f, 28f, SpriteText.Font_Effect.Black_Shadow_Small, SpriteText.Anchor_Pos.Middle_Center, SpriteText.Alignment_Type.Left, Color.white);
			if (null != this.m_btLocalMap_AreaIcon[i])
			{
				this.m_btLocalMap_AreaIcon[i].Data = 0;
				this.m_btLocalMap_AreaIcon[i].SetSize(64f, 64f);
				this.m_btLocalMap_AreaIcon[i].Start();
				this.m_btLocalMap_AreaIcon[i].UseDefaultSound = false;
				this.m_lbLocalMap_AreaName[i].Visible = false;
				base.InteractivePanel.MakeChild(this.m_btLocalMap_AreaIcon[i].gameObject);
				base.InteractivePanel.MakeChild(this.m_lbLocalMap_AreaName[i].gameObject);
			}
		}
		for (int j = 0; j < 20; j++)
		{
			this.m_pTreasure[j] = new TreasureData();
			this.m_pTreasure[j].Init(j);
			this.m_pTreasure[j].TreasureShow(false);
			if (this.m_pTreasure[j].GetTexture())
			{
				base.InteractivePanel.MakeChild(this.m_pTreasure[j].GetTexture().gameObject);
			}
			if (this.m_pTreasure[j].GetDrawTexture())
			{
				base.InteractivePanel.MakeChild(this.m_pTreasure[j].GetDrawTexture().gameObject);
			}
		}
		this.SetLocalMapTitle();
		this.SetLocalMapMapIcon();
		for (int k = 0; k < this.m_MaxLocalMapCount; k++)
		{
			int num2 = k + 1;
			LOCALMAP_INFO localMapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetLocalMapInfo(num2.ToString());
			if (localMapInfo != null)
			{
				if (localMapInfo.LOCALMAP_IDX == num2)
				{
					if (localMapInfo != null)
					{
						name = "BT_WorldMapAreaIcon" + k.ToString();
						name2 = "LB_WorldMapAreaName" + k.ToString();
						this.m_btWorldMap_AreaIcon[k] = UICreateControl.Button(name, localMapInfo.LOCALMAP_ICON, 64f, 64f);
						this.m_btWorldMap_AreaIcon[k].EffectAni = false;
						string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(localMapInfo.LOCALMAP_NAME_TEXT_INDEX);
						this.m_lbWorldMap_AreaName[k] = UICreateControl.Label(name2, NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + textFromInterface2, false, 280f, 28f, SpriteText.Font_Effect.Black_Shadow_Small, SpriteText.Anchor_Pos.Middle_Center, SpriteText.Alignment_Type.Left, Color.white);
						if (null != this.m_btWorldMap_AreaIcon[k])
						{
							this.m_btWorldMap_AreaIcon[k].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickWorldMapToLocalArea));
							this.m_btWorldMap_AreaIcon[k].SetLocation(localMapInfo.LOCALMAP_X + 20f, localMapInfo.LOCALMAP_Y + 64f);
							this.m_btWorldMap_AreaIcon[k].Data = localMapInfo.LOCALMAP_IDX;
							this.m_lbWorldMap_AreaName[k].SetLocation(localMapInfo.LOCALMAP_X + 20f + -107f, localMapInfo.LOCALMAP_Y + 64f + 84f);
							base.InteractivePanel.MakeChild(this.m_btWorldMap_AreaIcon[k].gameObject);
							base.InteractivePanel.MakeChild(this.m_lbWorldMap_AreaName[k].gameObject);
						}
						this.m_btWorldMap_AreaIcon[k].Start();
						if (!NrTSingleton<ContentsLimitManager>.Instance.IsWorldMapMove(num2))
						{
							this.m_btWorldMap_AreaIcon[k].Hide(true);
							this.m_lbWorldMap_AreaName[k].Hide(true);
						}
					}
				}
			}
		}
		base.SetScreenCenter();
		this.ShowLocalMap(true);
		this.SetUserIcon();
	}

	public void ResizeDlg()
	{
		base.SetLocation(0f, 0f);
	}

	public void ShowWorldMap(bool bShow)
	{
		this.m_dtWorldMap_Map.Visible = bShow;
		for (int i = 0; i < this.m_MaxLocalMapCount; i++)
		{
			if (!bShow || NrTSingleton<ContentsLimitManager>.Instance.IsWorldMapMove(i + 1))
			{
				this.m_btWorldMap_AreaIcon[i].Hide(!bShow);
				this.m_lbWorldMap_AreaName[i].Hide(!bShow);
			}
		}
	}

	public void ShowLocalMap(bool bShow)
	{
		this.ShowWorldMap(!bShow);
		this.m_bNowLocalMap = bShow;
		if (bShow)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.m_selectLocalMap.LOCALMAP_NAME_TEXT_INDEX);
			this.m_lbWorldMap_Title.SetText(textFromInterface);
		}
		this.m_dtLocalMap_Map.Visible = bShow;
		this.SetLocalMapNightMode();
		this.m_btLocalMap_ReWorld.Visible = bShow;
		this.m_btLocalMap_Night.Visible = bShow;
		this.m_btLocalMap_Day.Visible = bShow;
		this.m_btLocalMap_NpcAutoMove.Visible = bShow;
	}

	private void SetLocalMapMapIcon()
	{
		this.UpdateTreasure();
		MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(this.m_pkCharMapInfo.m_nMapIndex.ToString());
		int num = 0;
		if (mapInfo != null)
		{
			num = mapInfo.MAP_INDEX;
		}
		for (int i = 0; i < 20; i++)
		{
			int num2 = this.m_selectLocalMap.MAP_INDEX[i];
			MAP_INFO mapInfo2 = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(num2.ToString());
			bool flag = true;
			if (num2 <= 0 || mapInfo2 == null)
			{
				flag = false;
			}
			if (!NrTSingleton<ContentsLimitManager>.Instance.IsWarpMap(num2))
			{
				flag = false;
			}
			TreasureData treasureCheck = this.GetTreasureCheck(num2);
			if (!flag)
			{
				this.m_btLocalMap_AreaIcon[i].Hide(true);
				this.m_lbLocalMap_AreaName[i].Hide(true);
				if (treasureCheck != null)
				{
					treasureCheck.TreasureShow(false);
				}
			}
			else
			{
				this.m_btLocalMap_AreaIcon[i].Hide(false);
				this.m_lbLocalMap_AreaName[i].Hide(false);
				this.m_btLocalMap_AreaIcon[i].SetLocation(mapInfo2.MAP_X + 20f, mapInfo2.MAP_Y + 64f, -0.1f);
				string textFromMap = NrTSingleton<NrTextMgr>.Instance.GetTextFromMap(mapInfo2.TEXTKEY);
				if (textFromMap != string.Empty)
				{
					this.m_lbLocalMap_AreaName[i].SetText(textFromMap);
					this.m_lbLocalMap_AreaName[i].SetLocation(mapInfo2.MAP_X + 20f + -107f, mapInfo2.MAP_Y + 64f + 84f, -0.1f);
				}
				if (mapInfo2.MAP_ICON != string.Empty)
				{
					this.m_btLocalMap_AreaIcon[i].SetButtonTextureKey(mapInfo2.MAP_ICON);
				}
				this.m_btLocalMap_AreaIcon[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickLocaldMapToMapIcon));
				this.m_btLocalMap_AreaIcon[i].Data = num2;
				this.m_btLocalMap_AreaIcon[i].Visible = true;
				if (treasureCheck != null)
				{
					if (num != 0 && treasureCheck.GetMapIndex() == num)
					{
						treasureCheck.TreasureShow(true);
						treasureCheck.SetPostion(mapInfo2.MAP_X + 40f, mapInfo2.MAP_Y - 16f);
					}
					else
					{
						treasureCheck.TreasureShow(true);
						treasureCheck.SetPostion(mapInfo2.MAP_X + 10f, mapInfo2.MAP_Y - 16f);
					}
				}
			}
		}
	}

	private void SetLocalMapNightMode()
	{
		if (this.m_selectLocalMap != null)
		{
			this.SetLocalMapTitle();
			this.UpdateTreasure();
			for (int i = 0; i < 20; i++)
			{
				int num = this.m_selectLocalMap.MAP_INDEX[i];
				if (num > 0)
				{
					MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(num.ToString());
					if (mapInfo != null)
					{
						bool flag;
						if (this.m_bNowNightMode)
						{
							flag = (mapInfo.MAP_NIGHTMODE == 1);
						}
						else
						{
							flag = (mapInfo.MAP_NIGHTMODE != 1);
						}
						TreasureData treasureCheck = this.GetTreasureCheck(num);
						if (NrTSingleton<ContentsLimitManager>.Instance.IsWarpMap(num))
						{
							this.m_btLocalMap_AreaIcon[i].Visible = flag;
							this.m_lbLocalMap_AreaName[i].Visible = flag;
							if (treasureCheck != null)
							{
								treasureCheck.TreasureShow(flag);
							}
						}
					}
				}
			}
		}
	}

	private void SetUserIcon()
	{
		if (this.m_pkCharMapInfo == null)
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		bool flag = false;
		MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(this.m_pkCharMapInfo.m_nMapIndex.ToString());
		if (this.m_bNowLocalMap)
		{
			if (mapInfo != null)
			{
				for (int i = 0; i < 20; i++)
				{
					if (this.m_btLocalMap_AreaIcon[i].Visible && (int)this.m_btLocalMap_AreaIcon[i].Data == mapInfo.MAP_INDEX)
					{
						num = this.m_btLocalMap_AreaIcon[i].GetLocationX();
						num2 = this.m_btLocalMap_AreaIcon[i].GetLocationY();
						flag = true;
						break;
					}
					if (this.m_btLocalMap_AreaIcon[i].Visible && (int)this.m_btLocalMap_AreaIcon[i].Data == mapInfo.PARENTS_MAP_IDX)
					{
						num = this.m_btLocalMap_AreaIcon[i].GetLocationX();
						num2 = this.m_btLocalMap_AreaIcon[i].GetLocationY();
						break;
					}
				}
				if (num <= 0f && num2 <= 0f)
				{
					if (this.m_bNowNightMode)
					{
						num = this.m_btLocalMap_Day.GetLocationX();
						num2 = this.m_btLocalMap_Day.GetLocationY();
					}
					else
					{
						num = this.m_btLocalMap_Night.GetLocationX();
						num2 = this.m_btLocalMap_Night.GetLocationY();
					}
				}
			}
			num += -10f;
			num2 += -80f;
		}
		else
		{
			LOCALMAP_INFO localMapInfoFromMapIndex = NrTSingleton<NrBaseTableManager>.Instance.GetLocalMapInfoFromMapIndex(this.m_pkCharMapInfo.m_nMapIndex);
			if (localMapInfoFromMapIndex != null)
			{
				num = localMapInfoFromMapIndex.LOCALMAP_X + 10f;
				num2 = localMapInfoFromMapIndex.LOCALMAP_Y - 13f;
			}
		}
		float num3 = num + 15f;
		float y = num2 + 10f;
		this.m_btUser_User.Visible = true;
		this.m_dtUser_Icon.Visible = true;
		if (mapInfo != null && flag && this.GetTreasureCheck(mapInfo.MAP_INDEX) != null)
		{
			this.m_btUser_User.SetLocation(num - 30f, num2, -0.3f);
			this.m_dtUser_Icon.SetLocation(num3 - 30f, y, -0.4f);
			return;
		}
		this.m_btUser_User.SetLocation(num, num2, -0.3f);
		this.m_dtUser_Icon.SetLocation(num3, y, -0.4f);
	}

	private void SetLocalMapTitle()
	{
		if (this.m_selectLocalMap != null)
		{
			if (this.m_bNowNightMode)
			{
				this.m_dtLocalMap_Day.SetTexture("Map_I_NightTime");
				this.m_dtLocalMap_Day.Visible = true;
				this.m_dtLocalMap_Night.Visible = true;
			}
			else
			{
				this.m_dtLocalMap_Day.SetTexture("Map_I_DayTime");
				this.m_dtLocalMap_Day.Visible = true;
				this.m_dtLocalMap_Night.Visible = false;
			}
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.m_selectLocalMap.LOCALMAP_NAME_TEXT_INDEX);
			this.m_lbWorldMap_Title.SetText(textFromInterface);
		}
	}

	private void OnClickWorldMapToLocalArea(IUIObject obj)
	{
		this.m_selectLocalMap = NrTSingleton<NrBaseTableManager>.Instance.GetLocalMapInfo(((int)obj.Data).ToString());
		this.m_selectMap = null;
		this.m_dtLocalMap_Map.SetTextureFromBundle(this.m_selectLocalMap.GetBundlePath());
		this.m_bNowNightMode = false;
		this.SetLocalMapTitle();
		this.SetLocalMapMapIcon();
		this.ShowLocalMap(true);
		this.SetUserIcon();
	}

	private void OnClickLocalMapReWorld(IUIObject obj)
	{
		this.CloseUIGuide();
		this.m_selectLocalMap = null;
		this.m_selectMap = null;
		for (int i = 0; i < 20; i++)
		{
			if ((int)this.m_btLocalMap_AreaIcon[i].Data > 0)
			{
				this.m_btLocalMap_AreaIcon[i].Visible = false;
				this.m_lbLocalMap_AreaName[i].Visible = false;
				this.m_pTreasure[i].TreasureShow(false);
			}
		}
		this.m_lbWorldMap_Title.SetText(this.m_WorldMapInfo.WORLDMAP_NAME);
		this.m_dtLocalMap_Day.Visible = false;
		this.m_dtLocalMap_Night.Visible = false;
		this.m_dtWorldMap_Map.SetTextureFromBundle(this.m_WorldMapInfo.GetBundlePath());
		this.ShowLocalMap(false);
		this.SetUserIcon();
	}

	private void OnClickLocalMapNpcAutoMove(IUIObject obj)
	{
		this.CloseUIGuide();
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NPC_AUTOMOVE_DLG))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NPC_AUTOMOVE_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NPC_AUTOMOVE_DLG);
		}
	}

	private void OnClickLocalMapNight(IUIObject obj)
	{
		this.CloseUIGuide();
		this.m_selectMap = null;
		this.m_bNowNightMode = true;
		this.SetLocalMapNightMode();
		this.SetUserIcon();
		NrTSingleton<EventConditionHandler>.Instance.WorldMapModeClick.OnTrigger();
	}

	private void OnClickLocalMapDay(IUIObject obj)
	{
		this.CloseUIGuide();
		this.m_selectMap = null;
		this.m_bNowNightMode = false;
		this.SetLocalMapNightMode();
		this.SetUserIcon();
	}

	private void OnClickLocaldMapToMapIcon(IUIObject obj)
	{
		this.m_selectMap = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(((int)obj.Data).ToString());
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, this.m_selectMap.MAP_INDEX))
		{
			return;
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MAP", "CLICK", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		string textFromMap = NrTSingleton<NrTextMgr>.Instance.GetTextFromMap(this.m_selectMap.TEXTKEY);
		if (textFromMap != string.Empty)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI == null)
			{
				return;
			}
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("4"),
				"mapname",
				textFromMap
			});
			msgBoxUI.SetMsg(new YesDelegate(this.OnOKStart), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("3"), empty, eMsgType.MB_OK_CANCEL, 2);
			msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("109"));
			msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
		}
		this.HideUIGuide();
	}

	public void OnOKStart(object a_oObject)
	{
		this.OnClickLocaldMapWarp();
	}

	private void OnClickLocaldMapWarp()
	{
		NrTSingleton<NkClientLogic>.Instance.SetWarp(true, 0, this.m_selectMap.MAP_INDEX);
		this.Close();
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MAP", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.CloseUIGuide();
	}

	public void UpdateTreasure()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			List<int> treasureMapDate = myCharInfo.GetTreasureMapDate();
			if (treasureMapDate.Count != 0)
			{
				for (int i = 0; i < 20; i++)
				{
					this.m_pTreasure[i].TreasureShow(false);
					if (i < treasureMapDate.Count)
					{
						this.m_pTreasure[i].SetTreasureData(true, treasureMapDate[i]);
					}
					else
					{
						this.m_pTreasure[i].SetTreasureData(false, 0);
					}
				}
			}
		}
	}

	public TreasureData GetTreasureCheck(int i32MapData)
	{
		for (int i = 0; i < 20; i++)
		{
			if (this.m_pTreasure[i].GetTreasureData(i32MapData))
			{
				return this.m_pTreasure[i];
			}
		}
		return null;
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		int num = 0;
		int.TryParse(param1, out num);
		if (num != 0)
		{
			for (int i = 0; i < 20; i++)
			{
				int num2 = (int)this.m_btLocalMap_AreaIcon[i].data;
				if (num2 == num)
				{
					this._GuideItem = this.m_btLocalMap_AreaIcon[i];
				}
			}
		}
		else
		{
			this._GuideItem = (base.GetControl(param1) as UIButton);
		}
		this.m_nWinID = winID;
		if (null != this._GuideItem)
		{
			this._ButtonZ = this._GuideItem.GetLocation().z;
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				uI_UIGuide.CloseUI = false;
				if (uI_UIGuide.GetLocation().z == base.GetLocation().z)
				{
					uI_UIGuide.SetLocation(uI_UIGuide.GetLocationX(), uI_UIGuide.GetLocationY(), uI_UIGuide.GetLocation().z - 10f);
				}
				this._GuideItem.EffectAni = false;
				Vector2 x = new Vector2(base.GetLocationX() + this._GuideItem.GetLocationX() + 72f, base.GetLocationY() + this._GuideItem.GetLocationY() - 2f);
				uI_UIGuide.Move(x, UI_UIGuide.eTIPPOS.LEFT);
				this._ButtonZ = this._GuideItem.gameObject.transform.localPosition.z;
				this._GuideItem.SetLocationZ(uI_UIGuide.GetLocation().z - base.GetLocation().z - 1f);
				this._GuideItem.AlphaAni(1f, 0.5f, -0.5f);
			}
		}
	}

	public void HideUIGuide()
	{
		if (null != this._GuideItem)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			this._GuideItem.SetLocationZ(this._ButtonZ);
			this._GuideItem.StopAni();
			this._GuideItem.AlphaAni(1f, 1f, 0f);
		}
		this._GuideItem = null;
	}

	public void CloseUIGuide()
	{
		if (null != this._GuideItem)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			this._GuideItem.SetLocationZ(this._ButtonZ);
			this._GuideItem.StopAni();
			this._GuideItem.AlphaAni(1f, 1f, 0f);
		}
		this._GuideItem = null;
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
			uI_UIGuide.Close();
		}
	}
}
