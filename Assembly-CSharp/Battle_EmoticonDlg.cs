using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Battle_EmoticonDlg : Form
{
	private NewListBox m_nlbEmoticon;

	private Dictionary<eBATTLE_RADIO_ALARM, string> _radioAlarmTextKeyDic;

	private float _radioAlarmDelay;

	private float _radioAlarmRecentRequestTime;

	public Battle_EmoticonDlg()
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
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_Emoticon", G_ID.BATTLE_EMOTICON_DLG, false);
		this._radioAlarmDelay = 3f;
	}

	public override void SetComponent()
	{
		this.m_nlbEmoticon = (base.GetControl("NLB_Emoticon") as NewListBox);
		this.m_nlbEmoticon.Reserve = false;
		this.m_nlbEmoticon.AddRightMouseDelegate(new EZValueChangedDelegate(this.OnClickEmoticonListBox));
		this._SetDialogPos();
	}

	public override void InitData()
	{
		this._SetDialogPos();
		this.SetEmoticonData();
	}

	public void _SetDialogPos()
	{
		float x = 0f;
		base.SetLocation(x, GUICamera.height - base.GetSizeY());
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	private void SetEmoticonData()
	{
		this.m_nlbEmoticon.Clear();
		BATTLE_EMOTICON_Manager instance = BATTLE_EMOTICON_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		for (int i = 0; i < 9; i++)
		{
			BATTLE_EMOTICON data = instance.GetData((eBATTLE_EMOTICON)i);
			if (data != null)
			{
				NewListItem newListItem = new NewListItem(this.m_nlbEmoticon.ColumnNum, true, string.Empty);
				object data2;
				if (this.IsRadioAlarmMode())
				{
					data2 = i;
				}
				else
				{
					data2 = data;
				}
				newListItem.SetListItemData(0, string.Empty, data2, new EZValueChangedDelegate(this.OnClickEmoticonListBox), null);
				newListItem.SetListItemData(1, data.m_szTexture, null, null, null);
				newListItem.SetListItemData(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(data.m_szTextKey), null, null, null);
				this.m_nlbEmoticon.Add(newListItem);
			}
		}
		this.m_nlbEmoticon.RepositionItems();
	}

	public void OnClickEmoticonListBox(IUIObject obj)
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER)
		{
			int alarmKind = (int)obj.Data;
			this.RequestRadioAlarmToParty(alarmKind);
			return;
		}
		if (this.m_nlbEmoticon.SelectedItem == null)
		{
			return;
		}
		BATTLE_EMOTICON bATTLE_EMOTICON = (BATTLE_EMOTICON)obj.Data;
		if (!Battle.BATTLE.IsEmotionSet)
		{
			Battle.BATTLE.IsEmotionSet = true;
			Battle.BATTLE.SetEmoticon = bATTLE_EMOTICON.m_eConstant;
			return;
		}
		if (Battle.BATTLE.SetEmoticon == bATTLE_EMOTICON.m_eConstant)
		{
			Battle.BATTLE.IsEmotionSet = false;
			Battle.BATTLE.SetEmoticon = eBATTLE_EMOTICON.eBATTLE_EMOTICON_MAX;
			return;
		}
		Battle.BATTLE.SetEmoticon = bATTLE_EMOTICON.m_eConstant;
	}

	public override void Update()
	{
		base.Update();
		if (this.IsRadioAlarmMode())
		{
			return;
		}
		for (int i = 0; i < 9; i++)
		{
			UIListItemContainer item = this.m_nlbEmoticon.GetItem(i);
			BATTLE_EMOTICON bATTLE_EMOTICON = (BATTLE_EMOTICON)item.GetElementObject(0);
			if (!Battle.BATTLE.IsEmotionSet)
			{
				if (item != null)
				{
					UIButton uIButton = item.GetElement(0) as UIButton;
					if (uIButton != null)
					{
						uIButton.SetControlState(UIButton.CONTROL_STATE.NORMAL);
					}
				}
			}
			else if (Battle.BATTLE.SetEmoticon == bATTLE_EMOTICON.m_eConstant)
			{
				if (item != null)
				{
					UIButton uIButton2 = item.GetElement(0) as UIButton;
					if (uIButton2 != null)
					{
						uIButton2.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
					}
				}
			}
			else if (item != null)
			{
				UIButton uIButton3 = item.GetElement(0) as UIButton;
				if (uIButton3 != null)
				{
					uIButton3.SetControlState(UIButton.CONTROL_STATE.NORMAL);
				}
			}
		}
	}

	public string GetAlarmTextKey(eBATTLE_RADIO_ALARM alarmKind)
	{
		if (this._radioAlarmTextKeyDic == null)
		{
			return string.Empty;
		}
		if (!this._radioAlarmTextKeyDic.ContainsKey(alarmKind))
		{
			Debug.LogError("ERROR, Battle_EmoticonDlg.cs, GetAlarmTextKey(), alarmKind not contain");
			return string.Empty;
		}
		return this._radioAlarmTextKeyDic[alarmKind];
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
}
