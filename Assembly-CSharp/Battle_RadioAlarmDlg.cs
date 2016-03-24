using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Battle_RadioAlarmDlg : Form
{
	private Dictionary<eBATTLE_RADIO_ALARM, string> _radioAlarmTextKeyDic;

	private float _radioAlarmDelay;

	private float _radioAlarmRecentRequestTime;

	private bool _hide;

	public Battle_RadioAlarmDlg()
	{
		this._radioAlarmTextKeyDic = new Dictionary<eBATTLE_RADIO_ALARM, string>();
		this._radioAlarmTextKeyDic.Add(eBATTLE_RADIO_ALARM.eBATTLE_RADIO_ALARM_THANK, "900");
		this._radioAlarmTextKeyDic.Add(eBATTLE_RADIO_ALARM.eBATTLE_RADIO_ALARM_SORRY, "901");
		this._radioAlarmTextKeyDic.Add(eBATTLE_RADIO_ALARM.eBATTLE_RADIO_ALARM_ATTACK, "902");
		this._radioAlarmTextKeyDic.Add(eBATTLE_RADIO_ALARM.eBATTLE_RADIO_ALARM_GOOD, "903");
		this._radioAlarmTextKeyDic.Add(eBATTLE_RADIO_ALARM.eBATTLE_RADIO_ALARM_BYEBYE, "904");
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "MythRaid/dlg_myth_emoticon", G_ID.BATTLE_RADIOALARM_DLG, false);
		this._radioAlarmDelay = 3f;
		this._hide = false;
	}

	public override void SetComponent()
	{
		string str = "BTN_Emotion";
		string name = string.Empty;
		for (int i = 0; i < 5; i++)
		{
			name = str + (i + 1).ToString();
			Button button = base.GetControl(name) as Button;
			if (!(button == null))
			{
				Button expr_45 = button;
				expr_45.Click = (EZValueChangedDelegate)Delegate.Combine(expr_45.Click, new EZValueChangedDelegate(this.OnClickAlarmBtn));
				button.Data = i;
			}
		}
		Button button2 = base.GetControl("BTN_HideBarButton") as Button;
		button2.SetValueChangedDelegate(new EZValueChangedDelegate(this.HideOnOff));
		this.SetDialogPos();
	}

	public override void InitData()
	{
		this.SetDialogPos();
	}

	public override void ChangedResolution()
	{
		this.SetDialogPos();
	}

	public void OnClickAlarmBtn(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			Debug.LogError("ERROR, Battle_RadioAlarmDlg.cs, OnClickAlarmBtn(), alarmKind not contain");
			return;
		}
		eBATTLE_RADIO_ALARM alarmKind = (eBATTLE_RADIO_ALARM)((int)obj.Data);
		this.RequestRadioAlarmToParty((int)alarmKind);
	}

	public string GetAlarmTextKey(eBATTLE_RADIO_ALARM alarmKind)
	{
		if (this._radioAlarmTextKeyDic == null)
		{
			return string.Empty;
		}
		if (!this._radioAlarmTextKeyDic.ContainsKey(alarmKind))
		{
			Debug.LogError("ERROR, Battle_RadioAlarmDlg.cs, GetAlarmTextKey(), alarmKind not contain");
			return string.Empty;
		}
		return this._radioAlarmTextKeyDic[alarmKind];
	}

	private void SetDialogPos()
	{
		base.SetLocation(0f, GUICamera.height - base.GetSizeY());
	}

	private bool IsRadioAlarmMode()
	{
		return Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID;
	}

	private void RequestRadioAlarmToParty(int alarmKind)
	{
		if (!this.RadioAlarmDelayIsOver())
		{
			return;
		}
		this._radioAlarmRecentRequestTime = Time.realtimeSinceStartup;
		GS_BATTLE_RADIO_ALARM_REQ gS_BATTLE_RADIO_ALARM_REQ = new GS_BATTLE_RADIO_ALARM_REQ();
		gS_BATTLE_RADIO_ALARM_REQ.i8RadioAlarmKind = (byte)alarmKind;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_RADIO_ALARM_REQ, gS_BATTLE_RADIO_ALARM_REQ);
	}

	private bool RadioAlarmDelayIsOver()
	{
		return this._radioAlarmRecentRequestTime + this._radioAlarmDelay < Time.realtimeSinceStartup;
	}

	private void HideOnOff(IUIObject obj)
	{
		this.Move();
	}

	private void Move()
	{
		if (base.IsMove)
		{
			return;
		}
		float num = (!this._hide) ? -1f : 1f;
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RADIOALARM_DLG) == null)
		{
			return;
		}
		float value = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RADIOALARM_DLG).GetSizeX() * num;
		base.Move(value);
		this._hide = !this._hide;
	}
}
