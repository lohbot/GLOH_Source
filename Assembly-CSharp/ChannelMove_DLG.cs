using Global;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using TsBundle;
using UnityForms;

public class ChannelMove_DLG : Form
{
	private const byte CHANNEL_MAX_LIST = 20;

	private Button m_BtnMove;

	private Button m_BtnCancel;

	private DropDownList m_ChannleDropDownList;

	private ListItem[] m_Items;

	private CHANNEL_STATE_INFO m_SelectedChallenInfo;

	public override void InitializeComponent()
	{
		this.m_Items = new ListItem[20];
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "ChannelMove/DLG_ChannelMove", G_ID.CHANNEL_MOVE_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_BtnMove = (base.GetControl("Button_confirm") as Button);
		this.m_BtnCancel = (base.GetControl("Button_cancle") as Button);
		this.m_ChannleDropDownList = (base.GetControl("DropDownList_channel") as DropDownList);
		this.m_BtnMove.SetValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickMove));
		this.m_BtnCancel.SetValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickCancel));
		this.m_ChannleDropDownList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ChannleDropDownList_SelectionChange));
		this.m_SelectedChallenInfo = null;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CHANNEL", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void SetChannelInfo(CHANNEL_STATE_INFO[] channelState, int i32Count)
	{
		string str = string.Empty;
		this.m_ChannleDropDownList.SetViewArea(i32Count);
		this.m_ChannleDropDownList.Clear();
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.LogWarning("SetChannelInfo [count:{0}]", new object[]
			{
				i32Count
			});
		}
		for (int i = 0; i < i32Count; i++)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.LogWarning("SetChannelInfo [index:{0}] [channel:{1}]", new object[]
				{
					i,
					TKString.NEWString(channelState[i].ChannelName)
				});
			}
			if ((short)Client.m_MyCH == channelState[i].ChannelID)
			{
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2080");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
				{
					textFromInterface,
					"channel",
					TKString.NEWString(channelState[i].ChannelName)
				});
			}
			else
			{
				switch (channelState[i].State)
				{
				case 0:
				{
					string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("391");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
					{
						textFromInterface2,
						"channel",
						TKString.NEWString(channelState[i].ChannelName)
					});
					break;
				}
				case 1:
				{
					string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("393");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
					{
						textFromInterface3,
						"channel",
						TKString.NEWString(channelState[i].ChannelName)
					});
					break;
				}
				case 2:
				{
					string textFromInterface4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("394");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
					{
						textFromInterface4,
						"channel",
						TKString.NEWString(channelState[i].ChannelName)
					});
					break;
				}
				case 3:
				{
					string textFromInterface5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2081");
					if (textFromInterface5 == string.Empty)
					{
						str = "1";
					}
					else
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
						{
							textFromInterface5,
							"channel",
							TKString.NEWString(channelState[i].ChannelName)
						});
					}
					break;
				}
				}
			}
			this.m_Items[i] = new ListItem();
			this.m_Items[i].SetColumnStr(0, str);
			this.m_Items[i].Key = channelState[i];
			this.m_ChannleDropDownList.Add(this.m_Items[i]);
		}
		this.m_ChannleDropDownList.RepositionItems();
		this.m_ChannleDropDownList.SetFirstItem();
		if (i32Count >= 1)
		{
			this.m_SelectedChallenInfo = channelState[0];
		}
	}

	public void TESTSetChannelInfo()
	{
		this.m_ChannleDropDownList.Clear();
		for (int i = 0; i < 10; i++)
		{
			this.m_Items[i] = new ListItem();
			this.m_Items[i].SetColumnStr(0, "AAA");
			this.m_Items[i].Key = i + 10;
			this.m_ChannleDropDownList.Add(this.m_Items[i]);
		}
		this.m_ChannleDropDownList.RepositionItems();
		this.m_ChannleDropDownList.SetFirstItem();
	}

	private void BtnClickMove(IUIObject obj)
	{
		if (this.m_SelectedChallenInfo == null)
		{
			return;
		}
		if ((short)Client.m_MyCH == this.m_SelectedChallenInfo.ChannelID)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("560");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("387");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("92");
		string empty = string.Empty;
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromMessageBox,
			"targetname",
			TKString.NEWString(this.m_SelectedChallenInfo.ChannelName)
		});
		msgBoxUI.SetMsg(new YesDelegate(this.On_Channel_Move_Request), this.m_SelectedChallenInfo.ChannelID, textFromInterface, empty, eMsgType.MB_OK_CANCEL, 2);
	}

	private void BtnClickCancel(IUIObject obj)
	{
		this.Close();
	}

	private void ChannleDropDownList_SelectionChange(IUIObject obj)
	{
		DropDownList dropDownList = (DropDownList)obj;
		this.m_SelectedChallenInfo = (CHANNEL_STATE_INFO)this.m_Items[dropDownList.SelectIndex].Key;
	}

	private void On_Channel_Move_Request(object a_oObject)
	{
		GS_ACCOUNT_CHANNEL_MOVE_REQ gS_ACCOUNT_CHANNEL_MOVE_REQ = new GS_ACCOUNT_CHANNEL_MOVE_REQ();
		gS_ACCOUNT_CHANNEL_MOVE_REQ.i32ChannelID_DEST = (int)((short)a_oObject);
		SendPacket.GetInstance().SendObject(1109, gS_ACCOUNT_CHANNEL_MOVE_REQ);
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CHANNEL", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}
}
