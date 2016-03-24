using GAME;
using NPatch;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class BattleCollect_DLG : Form
{
	private enum eBATTLE_LAYER_TYPE
	{
		NONE,
		MYTHRAID,
		PVP,
		COLOSSEUM,
		NEWEXPLORATION,
		MYTHRAID_LOCK,
		PVP_LOCK,
		COLOSSEUM_LOCK,
		NEWEXPLOR_LOCK,
		MAX
	}

	private const int COUNT_BUTTON_MYRAID = 10;

	private const int COUNT_BUTTON_PVP = 8;

	private const int COUNT_BUTTON_COLOSSEUM = 8;

	private const int COUNT_BUTTON_NEWEXPLOR = 9;

	private DrawTexture m_dtBG;

	private Button[] m_btnRaid;

	private Button[] m_btnPVP;

	private Button[] m_btnColosseum;

	private Button[] m_btnExploration;

	private DrawTexture m_dtRaid_Disable;

	private Label m_lbRaid_Lock;

	private DrawTexture m_dtPVP_Disable;

	private Label m_lbPVP_Lock;

	private DrawTexture m_dtColosseum_Disable;

	private Label m_lbColosseum_Lock;

	private DrawTexture m_dtNewExplor_Disable;

	private Label m_lbNewExplor_Lock;

	private Button m_btnAdventureCollect;

	private DrawTexture m_dtMythRaidNotice;

	private DrawTexture m_dtPVPNotice;

	private DrawTexture m_dtColosseumNotice;

	private DrawTexture m_dtNewExplorNotice;

	private List<int> guideWinIDList = new List<int>();

	private UIButton _Touch;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "uibutton/DLG_Battle_Main", G_ID.BATTLECOLLECT_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btnRaid = new Button[10];
		this.m_btnPVP = new Button[8];
		this.m_btnColosseum = new Button[8];
		this.m_btnExploration = new Button[9];
		this.m_dtBG = (base.GetControl("DT_Back") as DrawTexture);
		this.m_dtBG.SetTextureFromBundle("ui/uibutton/battle_bg");
		for (int i = 0; i < 10; i++)
		{
			this.m_btnRaid[i] = (base.GetControl(string.Format("Btn_Raid{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnRaid[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_MythRaid));
		}
		for (int i = 0; i < 8; i++)
		{
			this.m_btnPVP[i] = (base.GetControl(string.Format("Btn_PVP{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnPVP[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_PVP));
		}
		for (int i = 0; i < 8; i++)
		{
			this.m_btnColosseum[i] = (base.GetControl(string.Format("Btn_Colosseum{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnColosseum[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Colosseum));
		}
		for (int i = 0; i < 9; i++)
		{
			this.m_btnExploration[i] = (base.GetControl(string.Format("Btn_NewExploration{0}", (i + 1).ToString("00"))) as Button);
			this.m_btnExploration[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Exploration));
		}
		this.m_dtRaid_Disable = (base.GetControl("DT_Raid_Lock") as DrawTexture);
		this.m_lbRaid_Lock = (base.GetControl("LB_Lock1") as Label);
		this.m_dtPVP_Disable = (base.GetControl("DT_PVP_Lock") as DrawTexture);
		this.m_lbPVP_Lock = (base.GetControl("LB_Lock2") as Label);
		this.m_dtColosseum_Disable = (base.GetControl("DT_Colosseum_Lock") as DrawTexture);
		this.m_lbColosseum_Lock = (base.GetControl("LB_Lock3") as Label);
		this.m_dtNewExplor_Disable = (base.GetControl("DT_NewExploration_Lock") as DrawTexture);
		this.m_lbNewExplor_Lock = (base.GetControl("LB_Lock4") as Label);
		this.m_btnAdventureCollect = (base.GetControl("Btn_Adventure") as Button);
		this.m_btnAdventureCollect.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_AdventureCollect));
		this.m_dtMythRaidNotice = (base.GetControl("DT_Notice1") as DrawTexture);
		this.m_dtPVPNotice = (base.GetControl("DT_Notice2") as DrawTexture);
		this.m_dtColosseumNotice = (base.GetControl("DT_Notice3") as DrawTexture);
		this.m_dtNewExplorNotice = (base.GetControl("DT_Notice4") as DrawTexture);
		this.Set_BattleButtons();
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

	private void Set_BattleButtons()
	{
		base.SetShowLayer(5, false);
		base.SetShowLayer(6, false);
		base.SetShowLayer(7, false);
		base.SetShowLayer(8, false);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		int level = kMyCharInfo.GetLevel();
		int num = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_MYTHRAID_LIMITLEVEL);
		bool flag = Launcher.Instance.IsPatchLevelMax();
		bool flag2 = !NrTSingleton<ContentsLimitManager>.Instance.IsMythRaidOn();
		if (flag2 || level < num || !flag)
		{
			this.Set_LockButtons(BattleCollect_DLG.eBATTLE_LAYER_TYPE.MYTHRAID_LOCK, flag2, num, flag);
		}
		num = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_LEVEL);
		flag2 = !NrTSingleton<ContentsLimitManager>.Instance.IsInfiBattle();
		if (flag2 || level < num || !flag)
		{
			this.Set_LockButtons(BattleCollect_DLG.eBATTLE_LAYER_TYPE.PVP_LOCK, flag2, num, flag);
		}
		num = COLOSSEUM_CONSTANT_Manager.GetInstance().GetValue(eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_CHECK_LEVEL);
		flag2 = false;
		if (flag2 || level < num || !flag)
		{
			this.Set_LockButtons(BattleCollect_DLG.eBATTLE_LAYER_TYPE.COLOSSEUM_LOCK, flag2, num, flag);
		}
		num = (int)NrTSingleton<ContentsLimitManager>.Instance.NewExplorationLimitLevel();
		flag2 = NrTSingleton<ContentsLimitManager>.Instance.IsNewExplorationLimit();
		if (flag2 || level < num || !flag)
		{
			this.Set_LockButtons(BattleCollect_DLG.eBATTLE_LAYER_TYPE.NEWEXPLOR_LOCK, flag2, num, flag);
		}
	}

	private void Set_LockButtons(BattleCollect_DLG.eBATTLE_LAYER_TYPE eType, bool bLimit, int nlevel, bool bPatchLevelMax)
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
				nlevel.ToString()
			});
		}
		base.SetShowLayer((int)eType, true);
		base.SetLayerZ((int)eType, -0.4f);
		switch (eType)
		{
		case BattleCollect_DLG.eBATTLE_LAYER_TYPE.MYTHRAID_LOCK:
			this.m_dtRaid_Disable.SetTextureFromBundle("ui/uibutton/mythraidbtn_lock");
			this.m_lbRaid_Lock.SetText(empty);
			for (int i = 0; i < 10; i++)
			{
				if (!(this.m_btnRaid[i] == null))
				{
					this.m_btnRaid[i].SetEnabled(false);
				}
			}
			break;
		case BattleCollect_DLG.eBATTLE_LAYER_TYPE.PVP_LOCK:
			this.m_dtPVP_Disable.SetTextureFromBundle("ui/uibutton/btn_lock");
			this.m_lbPVP_Lock.SetText(empty);
			for (int j = 0; j < 8; j++)
			{
				if (!(this.m_btnPVP[j] == null))
				{
					this.m_btnPVP[j].SetEnabled(false);
				}
			}
			break;
		case BattleCollect_DLG.eBATTLE_LAYER_TYPE.COLOSSEUM_LOCK:
			this.m_dtColosseum_Disable.SetTextureFromBundle("ui/uibutton/btn_lock");
			this.m_lbColosseum_Lock.SetText(empty);
			for (int k = 0; k < 8; k++)
			{
				if (!(this.m_btnColosseum[k] == null))
				{
					this.m_btnColosseum[k].SetEnabled(false);
				}
			}
			break;
		case BattleCollect_DLG.eBATTLE_LAYER_TYPE.NEWEXPLOR_LOCK:
			this.m_dtNewExplor_Disable.SetTextureFromBundle("ui/uibutton/newexplorationbtn_lock");
			this.m_lbNewExplor_Lock.SetText(empty);
			for (int l = 0; l < 9; l++)
			{
				if (!(this.m_btnExploration[l] == null))
				{
					this.m_btnExploration[l].SetEnabled(false);
				}
			}
			break;
		}
	}

	public void Update_Notice()
	{
		this.m_dtMythRaidNotice.Visible = false;
		this.m_dtPVPNotice.Visible = false;
		this.m_dtColosseumNotice.Visible = false;
		this.m_dtNewExplorNotice.Visible = false;
		if (NrTSingleton<MythRaidManager>.Instance.CanGetReward)
		{
			this.m_dtMythRaidNotice.Visible = true;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			if (myCharInfo.InfiBattleReward == 0)
			{
				this.m_dtPVPNotice.Visible = true;
			}
			if (myCharInfo.ColosseumOldRank > 0)
			{
				this.m_dtColosseumNotice.Visible = true;
			}
		}
		if (NrTSingleton<NewExplorationManager>.Instance.CanGetTreasureData() != null || NrTSingleton<NewExplorationManager>.Instance.CanGetEndReward())
		{
			this.m_dtNewExplorNotice.Visible = true;
		}
	}

	private void Click_MythRaid(IUIObject Obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_MYTHRAID_LIMITLEVEL);
			if (myCharInfo.GetLevel() < value)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("908");
				if (string.IsNullOrEmpty(textFromNotify))
				{
					return;
				}
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
		}
		DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
		if (directionDLG != null)
		{
			directionDLG.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_MYTHRAID, 0);
		}
	}

	private void Click_PVP(IUIObject _obj)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsInfiBattle())
		{
			if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
			{
				return;
			}
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERMAIN_DLG))
				{
					GS_INFIBATTLE_RANK_GET_REQ gS_INFIBATTLE_RANK_GET_REQ = new GS_INFIBATTLE_RANK_GET_REQ();
					gS_INFIBATTLE_RANK_GET_REQ.i64PersonID = myCharInfo.m_PersonID;
					SendPacket.GetInstance().SendObject(2017, gS_INFIBATTLE_RANK_GET_REQ);
				}
				else
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.PLUNDERMAIN_DLG);
				}
			}
		}
	}

	private void Click_Colosseum(IUIObject _obj)
	{
		this.HideTouch(false);
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

	private void Click_Exploration(IUIObject _obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWEXPLORATION_MAIN_DLG))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWEXPLORATION_MAIN_DLG);
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWEXPLORATION_MAIN_DLG);
		}
	}

	private void Click_AdventureCollect(IUIObject _obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ADVENTURECOLLECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ADVENTURECOLLECT_DLG);
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
