using GAME;
using System;
using UnityEngine;
using UnityForms;

public class MythRaid_ModeSelect_DLG : Form
{
	private Button bt_EasyEnter;

	private Button bt_NormalEnter;

	private Button bt_HardEnter;

	private DrawTexture dt_modeTexture1;

	private DrawTexture dt_modeTexture2;

	private DrawTexture dt_modeTexture3;

	private DrawTexture dt_Reward_Alarm;

	private DrawTexture dt_BG;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "mythraid/dlg_myth_modeselect", G_ID.MYTHRAID_MODESELECT_DLG, false);
	}

	public override void OnClose()
	{
	}

	public override void Show()
	{
		base.Show();
	}

	public override void OnLoad()
	{
		this.ShowNotice(NrTSingleton<MythRaidManager>.Instance.CanGetReward);
		base.OnLoad();
	}

	public override void Close()
	{
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
		TsAudio.RestoreMuteAllAudio();
		TsAudio.RefreshAllMuteAudio();
		base.Close();
	}

	public override void CloseForm(IUIObject obj)
	{
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
		TsAudio.RestoreMuteAllAudio();
		TsAudio.RefreshAllMuteAudio();
		base.CloseForm(obj);
	}

	public override void SetComponent()
	{
		base.SetScreenCenter();
		this.bt_EasyEnter = (base.GetControl("BTN_modeselect1") as Button);
		Button expr_22 = this.bt_EasyEnter;
		expr_22.Click = (EZValueChangedDelegate)Delegate.Combine(expr_22.Click, new EZValueChangedDelegate(this._clickEasyEnter));
		this.bt_NormalEnter = (base.GetControl("BTN_modeselect2") as Button);
		Button expr_5F = this.bt_NormalEnter;
		expr_5F.Click = (EZValueChangedDelegate)Delegate.Combine(expr_5F.Click, new EZValueChangedDelegate(this._clickNormalEnter));
		this.bt_HardEnter = (base.GetControl("BTN_modeselect3") as Button);
		Button expr_9C = this.bt_HardEnter;
		expr_9C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_9C.Click, new EZValueChangedDelegate(this._clickHardEnter));
		this.dt_BG = (base.GetControl("DT_SubBG") as DrawTexture);
		this.dt_BG.SetTextureFromBundle("UI/mythicraid/mythic_raid_bg");
		this.dt_modeTexture1 = (base.GetControl("DT_modeTexture1") as DrawTexture);
		this.dt_modeTexture2 = (base.GetControl("DT_modeTexture2") as DrawTexture);
		this.dt_modeTexture3 = (base.GetControl("DT_modeTexture3") as DrawTexture);
		this.dt_modeTexture1.SetTextureFromBundle("UI/mythicraid/mythic_raid_easymode");
		this.dt_modeTexture2.SetTextureFromBundle("UI/mythicraid/mythic_raid_normalmode");
		this.dt_modeTexture3.SetTextureFromBundle("UI/mythicraid/mythic_raid_hardmode");
		this.dt_Reward_Alarm = (base.GetControl("DT_Reward_Alarm") as DrawTexture);
		this.dt_Reward_Alarm.Visible = false;
		base.ShowBlackBG(1f);
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	public override void InitData()
	{
		base.InitData();
	}

	public override void Update()
	{
		base.Update();
	}

	private void _clickEasyEnter(IUIObject _obj)
	{
		this.SendEnterRoom(eMYTHRAID_DIFFICULTY.eMYTHRAID_EASY);
	}

	private void _clickNormalEnter(IUIObject _obj)
	{
		this.SendEnterRoom(eMYTHRAID_DIFFICULTY.eMYTHRAID_NORMAL);
	}

	private void _clickHardEnter(IUIObject _obj)
	{
		this.SendEnterRoom(eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD);
	}

	private void SendEnterRoom(eMYTHRAID_DIFFICULTY difficulty)
	{
		NrTSingleton<MythRaidManager>.Instance.RequestMyInfo(difficulty, false);
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLECOLLECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLECOLLECT_DLG);
		}
	}

	public void ShowNotice(bool isShow)
	{
		this.dt_Reward_Alarm.Visible = isShow;
	}
}
