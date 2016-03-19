using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class BabelTowerSubDlg : Form
{
	private DrawTexture m_dtMainBg;

	private Label m_laTitle;

	private Label m_laSubFloorNum;

	private Button m_btStart;

	private Button m_btClose;

	private ItemTexture m_itFirstRewardItem;

	private Label m_laFirstRewardItem;

	private DrawTexture m_dtFirstRewardItemBG;

	private Label m_laFristRewardItemText;

	private ItemTexture m_itSpecialRewardItem;

	private Label m_laSpecialRewardItem;

	private DrawTexture m_dtSpecialRewardItemBG;

	private Label m_laSpecialRewardItemText;

	private ItemTexture m_itRewardItem;

	private Label m_laRewardItem;

	private Label m_laRewardExp;

	private Button[][] m_btSubFloor = new Button[5][];

	private DrawTexture[][] m_dtFloorFirstNum = new DrawTexture[5][];

	private DrawTexture[][] m_dtFloorSecondNum = new DrawTexture[5][];

	private DrawTexture[][] m_dtFloorHundredNum = new DrawTexture[5][];

	private Label m_laWillSpend;

	private short m_nFloor;

	private short m_nsubFloor;

	private short m_nFloorType;

	private GameObject m_SelectOldEffect;

	private DrawTexture m_dwActivity;

	private Label m_lb_WillNum;

	private Label m_lbActivityTime;

	private Button btWillCharge1;

	private Button btWillCharge2;

	private long m_nBeforeActivity = -1L;

	private float m_fActivityUpdateTime;

	private int m_nBaseActivity;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "BabelTower/dlg_babel_subfloor", G_ID.BABELTOWERSUB_DLG, false, true);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_dtMainBg = (base.GetControl("Text_BG2") as DrawTexture);
		this.m_laTitle = (base.GetControl("label_title") as Label);
		this.m_laSubFloorNum = (base.GetControl("LB_FloorNum") as Label);
		this.m_btStart = (base.GetControl("Button_OK") as Button);
		this.m_btStart.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickStart));
		this.m_btClose = (base.GetControl("Button_Close") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickClose));
		this.m_itFirstRewardItem = (base.GetControl("ItemTexture_first_reward2") as ItemTexture);
		this.m_laFirstRewardItem = (base.GetControl("Label_first_reward2") as Label);
		this.m_dtFirstRewardItemBG = (base.GetControl("DrawTexture_DrawTexture49") as DrawTexture);
		this.m_laFristRewardItemText = (base.GetControl("Label_Label50") as Label);
		this.m_itSpecialRewardItem = (base.GetControl("ItemTexture_bonus_reward2") as ItemTexture);
		this.m_laSpecialRewardItem = (base.GetControl("Label_bouns_reward2") as Label);
		this.m_dtSpecialRewardItemBG = (base.GetControl("DrawTexture_bouns_reward") as DrawTexture);
		this.m_laSpecialRewardItemText = (base.GetControl("Label_bouns_reward") as Label);
		this.m_itRewardItem = (base.GetControl("ItemTexture_reward2") as ItemTexture);
		this.m_laRewardItem = (base.GetControl("Label_reward2") as Label);
		this.m_laRewardExp = (base.GetControl("Label_exp") as Label);
		string name = string.Empty;
		for (short num = 0; num < 5; num += 1)
		{
			this.m_btSubFloor[(int)num] = new Button[5];
			this.m_dtFloorFirstNum[(int)num] = new DrawTexture[5];
			this.m_dtFloorSecondNum[(int)num] = new DrawTexture[5];
			this.m_dtFloorHundredNum[(int)num] = new DrawTexture[5];
			for (short num2 = 0; num2 < 5; num2 += 1)
			{
				name = NrTSingleton<UIDataManager>.Instance.GetString("Button_subfloor", num.ToString(), "_", num2.ToString());
				this.m_btSubFloor[(int)num][(int)num2] = (base.GetControl(name) as Button);
				if (this.m_btSubFloor[(int)num][(int)num2] != null)
				{
					this.m_btSubFloor[(int)num][(int)num2].Data = num2;
					this.m_btSubFloor[(int)num][(int)num2].AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickSubFloor));
				}
				name = NrTSingleton<UIDataManager>.Instance.GetString("DrawTexture_Clear", num.ToString(), "_", num2.ToString());
				name = string.Format("subfloor{0}_{1}_1", num, num2);
				this.m_dtFloorFirstNum[(int)num][(int)num2] = (base.GetControl(name) as DrawTexture);
				if (this.m_dtFloorFirstNum[(int)num][(int)num2] != null)
				{
					this.m_dtFloorFirstNum[(int)num][(int)num2].Hide(true);
				}
				name = string.Format("subfloor{0}_{1}_2", num, num2);
				this.m_dtFloorSecondNum[(int)num][(int)num2] = (base.GetControl(name) as DrawTexture);
				if (this.m_dtFloorSecondNum[(int)num][(int)num2] != null)
				{
					this.m_dtFloorSecondNum[(int)num][(int)num2].Hide(true);
				}
				name = string.Format("subfloor{0}_{1}_0", num, num2);
				this.m_dtFloorHundredNum[(int)num][(int)num2] = (base.GetControl(name) as DrawTexture);
				if (this.m_dtFloorHundredNum[(int)num][(int)num2] != null)
				{
					this.m_dtFloorHundredNum[(int)num][(int)num2].Hide(true);
				}
			}
		}
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		this.m_nBaseActivity = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BASE_ACTIVITY);
		this.m_dwActivity = (base.GetControl("DrawTexture_will1") as DrawTexture);
		this.m_lb_WillNum = (base.GetControl("Label_WillNum") as Label);
		this.m_lbActivityTime = (base.GetControl("Label_Time") as Label);
		this.btWillCharge1 = (base.GetControl("Button_WillCharge1") as Button);
		Button expr_3FA = this.btWillCharge1;
		expr_3FA.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3FA.Click, new EZValueChangedDelegate(this.OnClickWillCharge));
		this.btWillCharge2 = (base.GetControl("Button_WillCharge2") as Button);
		Button expr_437 = this.btWillCharge2;
		expr_437.Click = (EZValueChangedDelegate)Delegate.Combine(expr_437.Click, new EZValueChangedDelegate(this.OnClickWillCharge));
		this.m_laWillSpend = (base.GetControl("Label_NeedWill") as Label);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public void ShowSubFloor(short floor, short FloorType)
	{
		this.m_nFloor = floor;
		this.m_nFloorType = FloorType;
		this.SetLayer();
		base.Show();
	}

	public void SetLayer()
	{
		short babelTowerLastSubFloor = NrTSingleton<BabelTowerManager>.Instance.GetBabelTowerLastSubFloor(this.m_nFloor, this.m_nFloorType);
		base.ShowLayer(0);
		for (int i = 1; i < 5; i++)
		{
			bool bShow = false;
			if (i == (int)babelTowerLastSubFloor)
			{
				bShow = true;
			}
			base.SetShowLayer(i, bShow);
		}
		this.SetSubFloorClear(babelTowerLastSubFloor, this.m_nFloorType);
		this.SetSubFloorInfo(0);
	}

	public void SetSubFloorClear(short sub_floor, short floortype = 1)
	{
		this.m_dtMainBg.SetTextureFromBundle("UI/BabelTower/Babel_sub" + sub_floor.ToString());
		NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_BABEL_SUB", this.m_dtMainBg, this.m_dtMainBg.GetSize());
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		for (short num = 0; num < 5; num += 1)
		{
			string texture = string.Empty;
			if (this.m_dtFloorHundredNum[(int)sub_floor][(int)num] != null && this.m_nFloor >= 100)
			{
				short num2 = this.m_nFloor / 100;
				if (num2 > 0)
				{
					texture = "Win_Number_" + num2;
					this.m_dtFloorHundredNum[(int)sub_floor][(int)num].Hide(false);
					this.m_dtFloorHundredNum[(int)sub_floor][(int)num].SetTexture(texture);
				}
			}
			if (this.m_dtFloorFirstNum[(int)sub_floor][(int)num] != null)
			{
				short num3 = this.m_nFloor / 10;
				if (num3 >= 10)
				{
					num3 %= 10;
				}
				if (num3 > 0 || (num3 == 0 && this.m_nFloor >= 100))
				{
					texture = "Win_Number_" + num3;
					this.m_dtFloorFirstNum[(int)sub_floor][(int)num].Hide(false);
					this.m_dtFloorFirstNum[(int)sub_floor][(int)num].SetTexture(texture);
				}
			}
			if (this.m_dtFloorSecondNum[(int)sub_floor][(int)num] != null)
			{
				short num4 = this.m_nFloor % 10;
				texture = "Win_Number_" + num4;
				this.m_dtFloorSecondNum[(int)sub_floor][(int)num].Hide(false);
				this.m_dtFloorSecondNum[(int)sub_floor][(int)num].SetTexture(texture);
			}
		}
		for (short num5 = 0; num5 < 5; num5 += 1)
		{
			byte babelSubFloorRankInfo = kMyCharInfo.GetBabelSubFloorRankInfo(this.m_nFloor, (byte)num5, floortype);
			bool treasure = kMyCharInfo.IsBabelTreasure(this.m_nFloor, (short)((byte)num5), floortype);
			if (this.m_btSubFloor[(int)sub_floor][(int)num5] != null)
			{
				this.m_btSubFloor[(int)sub_floor][(int)num5].SetButtonTextureKey(NrTSingleton<BabelTowerManager>.Instance.GetBabelRankImgText(babelSubFloorRankInfo, treasure));
			}
		}
	}

	public override void Update()
	{
		base.Update();
		MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
		if (myCharInfoDlg != null)
		{
			myCharInfoDlg.Update();
		}
		if (this.m_fActivityUpdateTime < Time.realtimeSinceStartup)
		{
			this.m_lbActivityTime.SetText(myCharInfoDlg.StrActivityTime);
			this.m_fActivityUpdateTime = Time.realtimeSinceStartup + 0.5f;
			this.SetActivityPointUI();
		}
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	private void SetSubFloorInfo(short sub_floor)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		BABELTOWER_DATA babelTowerData = NrTSingleton<BabelTowerManager>.Instance.GetBabelTowerData(this.m_nFloor, sub_floor, this.m_nFloorType);
		if (babelTowerData != null)
		{
			this.m_nsubFloor = sub_floor;
			string text = string.Empty;
			string empty = string.Empty;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2846");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"count",
				babelTowerData.m_nWillSpend
			});
			this.m_laWillSpend.SetText(empty);
			float width = this.m_laWillSpend.GetWidth();
			if (this.m_nFloorType == 2)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2786");
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("640");
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"floor",
				babelTowerData.m_nFloor
			});
			this.m_laTitle.SetText(empty);
			if (this.m_nFloorType == 2)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2787");
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("833");
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"floor",
				this.m_nFloor,
				"subfloor",
				(int)(sub_floor + 1)
			});
			this.m_laSubFloorNum.SetText(empty);
			if (!kMyCharInfo.IsBabelClear(this.m_nFloor, sub_floor, this.m_nFloorType))
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"itemname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(babelTowerData.m_nFirstReward_ItemUniq),
					"count",
					babelTowerData.m_nFirstReward_ItemNum
				});
				this.m_laFirstRewardItem.SetText(empty);
				this.m_itFirstRewardItem.Visible = true;
				this.m_itFirstRewardItem.ClearData();
				this.m_itFirstRewardItem.SetItemTexture(babelTowerData.m_nFirstReward_ItemUniq);
				this.m_dtFirstRewardItemBG.Visible = true;
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("723");
				this.m_laFristRewardItemText.SetText(text);
			}
			else if (!kMyCharInfo.IsBabelTreasure(this.m_nFloor, sub_floor, this.m_nFloorType))
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"itemname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(babelTowerData.m_i32TreasureRewardUnique),
					"count",
					babelTowerData.m_i32TreasureRewardNum
				});
				this.m_laFirstRewardItem.SetText(empty);
				this.m_itFirstRewardItem.Visible = true;
				this.m_itFirstRewardItem.ClearData();
				ITEM iTEM = new ITEM();
				iTEM.m_nItemUnique = babelTowerData.m_i32TreasureRewardUnique;
				iTEM.m_nItemNum = babelTowerData.m_i32TreasureRewardNum;
				iTEM.m_nOption[2] = babelTowerData.m_i32TreasureRewardRank;
				this.m_itFirstRewardItem.SetItemTexture(iTEM, false);
				this.m_dtFirstRewardItemBG.Visible = true;
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1931");
				this.m_laFristRewardItemText.SetText(text);
			}
			else
			{
				this.m_itFirstRewardItem.Visible = false;
				this.m_dtFirstRewardItemBG.Visible = false;
				this.m_laFirstRewardItem.SetText(string.Empty);
				this.m_laFristRewardItemText.SetText(string.Empty);
			}
			BATTLE_BABEL_SREWARD battleBabelSRewardData = NrTSingleton<BattleSReward_Manager>.Instance.GetBattleBabelSRewardData(babelTowerData.m_i32ShowSpecialReward_DataUnique);
			if (battleBabelSRewardData != null)
			{
				int num = babelTowerData.m_i32ShowSpecialReward_DataPos - 1;
				if (num < 0 || num >= 4)
				{
					num = 3;
				}
				int nRewardValue = battleBabelSRewardData.m_sRewardProduct[num].m_nRewardValue1;
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1803");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"itemname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(nRewardValue)
				});
				this.m_laSpecialRewardItem.SetText(empty);
				this.m_itSpecialRewardItem.ClearData();
				this.m_itSpecialRewardItem.SetItemTexture(nRewardValue);
			}
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"itemname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(babelTowerData.m_nReward_ItemUniq),
				"count",
				babelTowerData.m_nReward_ItemNum
			});
			this.m_laRewardItem.SetText(empty);
			this.m_itRewardItem.SetItemTexture(babelTowerData.m_nReward_ItemUniq);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("726");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"exp",
				babelTowerData.m_nReward_Exp
			});
			this.m_laRewardExp.SetText(empty);
			short babelTowerLastSubFloor = NrTSingleton<BabelTowerManager>.Instance.GetBabelTowerLastSubFloor(this.m_nFloor, this.m_nFloorType);
			if (this.m_SelectOldEffect != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_SelectOldEffect);
			}
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_BABEL_SELECT", this.m_btSubFloor[(int)babelTowerLastSubFloor][(int)this.m_nsubFloor], this.m_btSubFloor[(int)babelTowerLastSubFloor][(int)this.m_nsubFloor].GetSize());
			this.m_btSubFloor[(int)babelTowerLastSubFloor][(int)this.m_nsubFloor].AddGameObjectDelegate(new EZGameObjectDelegate(this.OldeffectDelete));
		}
	}

	public void BtnClickSubFloor(IUIObject obj)
	{
		short subFloorInfo = (short)obj.Data;
		this.SetSubFloorInfo(subFloorInfo);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CHAOSTOWER", "ROOM_SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void BtnClickStart(IUIObject obj)
	{
		bool flag = NrTSingleton<BabelTowerManager>.Instance.IsBabelStart();
		if (flag)
		{
			GS_BABELTOWER_GOLOBBY_REQ gS_BABELTOWER_GOLOBBY_REQ = new GS_BABELTOWER_GOLOBBY_REQ();
			gS_BABELTOWER_GOLOBBY_REQ.mode = 0;
			gS_BABELTOWER_GOLOBBY_REQ.babel_floor = this.m_nFloor;
			gS_BABELTOWER_GOLOBBY_REQ.babel_subfloor = this.m_nsubFloor;
			gS_BABELTOWER_GOLOBBY_REQ.nPersonID = 0L;
			gS_BABELTOWER_GOLOBBY_REQ.Babel_FloorType = this.m_nFloorType;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_GOLOBBY_REQ, gS_BABELTOWER_GOLOBBY_REQ);
		}
	}

	public void BtnClickClose(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickWillCharge(IUIObject obj)
	{
		MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
		if (myCharInfoDlg == null)
		{
			return;
		}
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHARGE_ACTIVITY_MAX);
		if (myCharInfoDlg.CurrentActivity >= num)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("135"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WILLCHARGE_DLG);
	}

	public void SetActivityPointUI()
	{
		MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
		if (myCharInfoDlg == null)
		{
			return;
		}
		if (this.m_nBeforeActivity == myCharInfoDlg.CurrentActivity)
		{
			return;
		}
		this.m_nBeforeActivity = myCharInfoDlg.CurrentActivity;
		string empty = string.Empty;
		if (myCharInfoDlg.CurrentActivity > myCharInfoDlg.MaxActivity)
		{
			string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1304");
			string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2791"),
				"CurrentNum",
				textColor + myCharInfoDlg.CurrentActivity.ToString() + textColor2,
				"MaxNum",
				myCharInfoDlg.MaxActivity
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2791"),
				"CurrentNum",
				myCharInfoDlg.CurrentActivity,
				"MaxNum",
				myCharInfoDlg.MaxActivity
			});
		}
		this.m_lb_WillNum.SetText(empty);
	}

	public void OldeffectDelete(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		this.m_SelectOldEffect = obj;
	}
}
