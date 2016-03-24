using GAME;
using NPatch;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class AdventureCollect_DLG : Form
{
	private enum eADVENTURE_LAYER_TYPE
	{
		NONE,
		BABELTOWER,
		ADVENTURE,
		HUNT,
		WORLDMAP,
		DAILYDUNGEON,
		BABELTOWER_LOCK,
		HUNT_LOCK,
		DAILYDUNGEON_LOCK,
		MAX
	}

	private const int MAX_BUTTON_COUNT = 8;

	private const int MAX_DAILYDUNGEON_ITEM = 5;

	private DrawTexture m_dtBG;

	private Button[] m_btnBabelTower;

	private Button[] m_btnAdventure;

	private Button[] m_btnHunt;

	private Button[] m_btnWorldMap;

	private Button[] m_btnDailyDungeon;

	private DrawTexture m_dtBabel_Disable;

	private Label m_lbBabel_Lock;

	private DrawTexture m_dtHunt_Disable;

	private Label m_lbHunt_Lock;

	private DrawTexture m_dtBDailyDungeon_Disable;

	private Label m_lbDailyDungeon_Lock;

	private DrawTexture[] m_dtItemIcon_Lock;

	private Button m_dtBattleCollect;

	private DrawTexture m_dtAdventureNotice;

	private DrawTexture m_dtDailyDungeonNotice;

	private List<int> guideWinIDList = new List<int>();

	private UIButton _Touch;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "uibutton/DLG_Adventure_Main", G_ID.ADVENTURECOLLECT_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btnBabelTower = new Button[8];
		this.m_btnAdventure = new Button[8];
		this.m_btnHunt = new Button[8];
		this.m_btnWorldMap = new Button[7];
		this.m_btnDailyDungeon = new Button[7];
		this.m_dtItemIcon_Lock = new DrawTexture[5];
		this.m_dtBG = (base.GetControl("DT_Back") as DrawTexture);
		this.m_dtBG.SetTextureFromBundle("ui/uibutton/adventure_bg");
		for (int i = 0; i < 8; i++)
		{
			this.m_btnBabelTower[i] = (base.GetControl(string.Format("Btn_BabelTower0{0}", i + 1)) as Button);
			this.m_btnBabelTower[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_BabelTower));
			this.m_btnAdventure[i] = (base.GetControl(string.Format("Btn_Adventure0{0}", i + 1)) as Button);
			this.m_btnAdventure[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Adventure));
			this.m_btnHunt[i] = (base.GetControl(string.Format("Btn_Hunt_0{0}", i + 1)) as Button);
			this.m_btnHunt[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Hunt));
		}
		for (int i = 0; i < 7; i++)
		{
			this.m_btnWorldMap[i] = (base.GetControl(string.Format("Btn_WorldMap0{0}", i + 1)) as Button);
			this.m_btnWorldMap[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_WorldMap));
			this.m_btnDailyDungeon[i] = (base.GetControl(string.Format("Btn_DailyDungeon0{0}", i + 1)) as Button);
			this.m_btnDailyDungeon[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_DailyDungeon));
		}
		for (int i = 0; i < 5; i++)
		{
			this.m_dtItemIcon_Lock[i] = (base.GetControl(string.Format("DT_ItemIcon_Lock{0}", i + 1)) as DrawTexture);
		}
		this.m_dtBabel_Disable = (base.GetControl("DT_BabelTower_Lock") as DrawTexture);
		this.m_lbBabel_Lock = (base.GetControl("LB_Lock1") as Label);
		this.m_dtHunt_Disable = (base.GetControl("DT_Hunt_Lock") as DrawTexture);
		this.m_lbHunt_Lock = (base.GetControl("LB_Lock2") as Label);
		this.m_dtBDailyDungeon_Disable = (base.GetControl("DT_DailyDungeon_Lock") as DrawTexture);
		this.m_lbDailyDungeon_Lock = (base.GetControl("LB_Lock3") as Label);
		this.m_dtBattleCollect = (base.GetControl("Btn_Battle") as Button);
		this.m_dtBattleCollect.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_BattleCollect));
		this.m_dtAdventureNotice = (base.GetControl("DT_Notice1") as DrawTexture);
		this.m_dtDailyDungeonNotice = (base.GetControl("DT_Notice2") as DrawTexture);
		base.SetShowLayer(6, false);
		base.SetShowLayer(7, false);
		base.SetShowLayer(8, false);
		this.Set_AdventureButtons();
		this.Set_DailyDungeonItems();
		this.Update_Notice();
	}

	public override void OnClose()
	{
		this.HideTouch(true);
		base.OnClose();
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (string.IsNullOrEmpty(param1))
		{
			return;
		}
		if (this.guideWinIDList != null && !this.guideWinIDList.Contains(winID))
		{
			this.guideWinIDList.Add(winID);
		}
		string[] array = param1.Split(new char[]
		{
			','
		});
		if (array == null || array.Length != 4)
		{
			return;
		}
		IUIObject control = base.GetControl(array[0]);
		if (control == null)
		{
			return;
		}
		if (this._Touch == null)
		{
			this._Touch = UICreateControl.Button("touch", "Main_I_Touch01", 196f, 154f);
		}
		if (this._Touch == null)
		{
			return;
		}
		int anchor = int.Parse(array[1]);
		this._Touch.SetAnchor((SpriteRoot.ANCHOR_METHOD)anchor);
		this._Touch.PlayAni(true);
		this._Touch.gameObject.SetActive(true);
		this._Touch.gameObject.transform.parent = control.gameObject.transform;
		this._Touch.transform.position = new Vector3(control.transform.position.x, control.transform.position.y, control.transform.position.z - 3f);
		float x = float.Parse(array[2]);
		float y = float.Parse(array[3]);
		this._Touch.transform.eulerAngles = new Vector3(x, y, this._Touch.transform.eulerAngles.z);
		BoxCollider component = this._Touch.gameObject.GetComponent<BoxCollider>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	private void Set_AdventureButtons()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		bool flag = false;
		int level = kMyCharInfo.GetLevel();
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABELTOWER_LIMITLEVEL);
		bool flag2 = Launcher.Instance.IsPatchLevelMax();
		if (flag || level < value || !flag2)
		{
			this.Set_LockButtons(AdventureCollect_DLG.eADVENTURE_LAYER_TYPE.BABELTOWER_LOCK, flag, value, flag2);
		}
		value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_HUNT_LIMITLEVEL);
		flag = NrTSingleton<ContentsLimitManager>.Instance.IsBountyHunt();
		if (flag || level < value || !flag2)
		{
			this.Set_LockButtons(AdventureCollect_DLG.eADVENTURE_LAYER_TYPE.HUNT_LOCK, flag, value, flag2);
		}
		value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_DAILYDUNGEON_LIMITLEVEL);
		flag = NrTSingleton<ContentsLimitManager>.Instance.IsDailyDungeonLimit();
		if (flag || level < value || !flag2)
		{
			this.Set_LockButtons(AdventureCollect_DLG.eADVENTURE_LAYER_TYPE.DAILYDUNGEON_LOCK, flag, value, flag2);
		}
	}

	private void Set_LockButtons(AdventureCollect_DLG.eADVENTURE_LAYER_TYPE eType, bool bLimit, int i32level, bool bPatchLevelMax)
	{
		string empty = string.Empty;
		if (!bPatchLevelMax)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3718")
			});
		}
		else if (bLimit)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3516")
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3466"),
				"level",
				i32level.ToString()
			});
		}
		base.SetShowLayer((int)eType, true);
		base.SetLayerZ((int)eType, -0.4f);
		switch (eType)
		{
		case AdventureCollect_DLG.eADVENTURE_LAYER_TYPE.BABELTOWER_LOCK:
			this.m_dtBabel_Disable.SetTextureFromBundle("ui/uibutton/babeltowerbtn_lock");
			this.m_lbBabel_Lock.SetText(empty);
			for (int i = 0; i < 8; i++)
			{
				if (!(this.m_btnBabelTower[i] == null))
				{
					this.m_btnBabelTower[i].SetEnabled(false);
				}
			}
			break;
		case AdventureCollect_DLG.eADVENTURE_LAYER_TYPE.HUNT_LOCK:
			this.m_dtHunt_Disable.SetTextureFromBundle("ui/uibutton/btn_lock");
			this.m_lbHunt_Lock.SetText(empty);
			for (int j = 0; j < 8; j++)
			{
				if (!(this.m_btnHunt[j] == null))
				{
					this.m_btnHunt[j].SetEnabled(false);
				}
			}
			break;
		case AdventureCollect_DLG.eADVENTURE_LAYER_TYPE.DAILYDUNGEON_LOCK:
			this.m_dtBDailyDungeon_Disable.SetTextureFromBundle("ui/uibutton/btn_lock");
			this.m_lbDailyDungeon_Lock.SetText(empty);
			for (int k = 0; k < 7; k++)
			{
				if (!(this.m_btnDailyDungeon[k] == null))
				{
					this.m_btnDailyDungeon[k].SetEnabled(false);
				}
			}
			break;
		}
	}

	private void Set_DailyDungeonItems()
	{
		int currWeekofDay = NrTSingleton<DailyDungeonManager>.Instance.GetCurrWeekofDay();
		if (NrTSingleton<DailyDungeonManager>.Instance.GetWeekend())
		{
			for (int i = 0; i < 5; i++)
			{
				this.m_dtItemIcon_Lock[i].Visible = false;
			}
		}
		else
		{
			int num = currWeekofDay - 1;
			this.m_dtItemIcon_Lock[num].Visible = false;
		}
	}

	public void Update_Notice()
	{
		bool visible = NrTSingleton<NkAdventureManager>.Instance.IsAcceptQuest();
		this.m_dtAdventureNotice.Visible = visible;
		bool visible2 = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.IsGet_DailyDungeonReward();
		this.m_dtDailyDungeonNotice.Visible = visible2;
	}

	private void Click_BabelTower(IUIObject Obj)
	{
		this.HideTouch(false);
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
			GS_FRIENDS_BABELTOWER_CLEARINFO_REQ obj = new GS_FRIENDS_BABELTOWER_CLEARINFO_REQ();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIENDS_BABELTOWER_CLEARINFO_REQ, obj);
		}
		int value2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BABEL_HARD_LEVEL);
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWERMAIN_DLG))
		{
			if (level < value2)
			{
				DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
				if (directionDLG != null)
				{
					directionDLG.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_BABEL, 1);
				}
				this.Close();
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

	private void Click_Adventure(IUIObject Obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ADVENTURE_DLG))
		{
			AdventureDlg adventureDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ADVENTURE_DLG) as AdventureDlg;
			if (adventureDlg != null)
			{
				adventureDlg.DrawAdventure();
			}
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ADVENTURE", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ADVENTURE_DLG);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ADVENTURE", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	private void Click_Hunt(IUIObject Obj)
	{
		GS_BOUNTYHUNT_DETAILINFO_REQ obj = new GS_BOUNTYHUNT_DETAILINFO_REQ();
		SendPacket.GetInstance().SendObject(1931, obj);
	}

	private void Click_WorldMap(IUIObject Obj)
	{
		this.HideTouch(false);
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WORLD_MAP))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.WORLD_MAP);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WORLD_MAP);
		}
	}

	private void Click_DailyDungeon(IUIObject Obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_SELECT))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DAILYDUNGEON_SELECT);
		}
	}

	private void Click_BattleCollect(IUIObject Obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLECOLLECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLECOLLECT_DLG);
		}
		this.Close();
	}

	private void HideTouch(bool closeUI)
	{
		if (this._Touch != null && this._Touch.gameObject != null)
		{
			this._Touch.gameObject.SetActive(false);
		}
		if (!closeUI)
		{
			return;
		}
		if (this.guideWinIDList == null)
		{
			return;
		}
		foreach (int current in this.guideWinIDList)
		{
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)current) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				uI_UIGuide.CloseUI = true;
			}
		}
		this._Touch = null;
	}
}
