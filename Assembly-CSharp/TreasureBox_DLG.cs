using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class TreasureBox_DLG : Form
{
	private DrawTexture m_DT_ItemIcon;

	private DrawTexture m_DT_ItemBg;

	private Label m_Label_Item;

	private Button m_BtnGetItem;

	private Button m_BtnSetPos;

	private Label m_LbelTitle;

	private Label m_LbelItemGet;

	private short m_i16TreasureReward;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Treasure/DLG_Treasure", G_ID.TREASUREBOX_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_DT_ItemBg = (base.GetControl("DT_ItemBg") as DrawTexture);
		this.m_DT_ItemIcon = (base.GetControl("DT_ItemIcon") as DrawTexture);
		this.m_Label_Item = (base.GetControl("Label_Item") as Label);
		this.m_BtnGetItem = (base.GetControl("Button_Sel_C") as Button);
		this.m_BtnSetPos = (base.GetControl("Button_Sel") as Button);
		this.m_LbelTitle = (base.GetControl("Label_Title") as Label);
		this.m_LbelItemGet = (base.GetControl("Label_ItemGet") as Label);
		this.m_BtnGetItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickGetItem));
		this.m_BtnSetPos.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickSetPos));
		base.Draggable = false;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		this.m_BtnGetItem.SetEnabled(true);
	}

	private void SetRewardButton(bool bShow)
	{
		this.m_LbelItemGet.Hide(bShow);
		this.m_BtnGetItem.SetEnabled(bShow);
		this.m_DT_ItemBg.Hide(!bShow);
		this.m_DT_ItemIcon.Hide(!bShow);
		this.m_Label_Item.Hide(!bShow);
		if (!bShow)
		{
			Transform child = NkUtil.GetChild(this.m_DT_ItemIcon.transform, "child_effect");
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
	}

	private void SetPosButton(bool bShow)
	{
		this.m_BtnSetPos.Hide(!bShow);
	}

	public void SetRewardItem(GS_TREASUREBOX_REWARD_DLG_NFY Nfy)
	{
		string text = string.Empty;
		this.m_i16TreasureReward = Nfy.i16TreasureUnique;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2527") + "_" + Nfy.i16TreasureUnique.ToString();
		this.m_LbelTitle.SetText(text);
		this.SetRewardButton(false);
		if (Nfy.i32Result == -11)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("634");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TREASUREBOX_DLG);
		}
		else if (Nfy.i32Result == -12)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("634");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			TsLog.LogWarning("!!!!!!!!!!!!!!!!! Reward - Reward Not", new object[0]);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TREASUREBOX_DLG);
		}
		else if (Nfy.i32Result == 0)
		{
			this.SetRewardButton(true);
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697");
			string text2 = string.Empty;
			for (int i = 0; i < 5; i++)
			{
				if (Nfy.i32ItemUnique[i] > 0 && i == 0)
				{
					text = string.Empty;
					UIBaseInfoLoader itemTexture = NrTSingleton<ItemManager>.Instance.GetItemTexture(Nfy.i32ItemUnique[i]);
					this.m_DT_ItemIcon.SetTexture(itemTexture);
					text2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(Nfy.i32ItemUnique[i]);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						textFromInterface,
						"itemname",
						text2,
						"count",
						Nfy.i32ItemNum[i]
					});
					this.m_Label_Item.SetText(text);
					NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_ANCIENTTREASURE_UI", this.m_DT_ItemIcon, this.m_DT_ItemIcon.GetSize());
				}
			}
		}
		else
		{
			this.SetRewardButton(false);
			TsLog.LogWarning("!!!!!!!!!!!!!!!!! Reward - Error {0}", new object[]
			{
				Nfy.i32Result
			});
		}
	}

	public void SendRewardItem(GS_TREASUREBOX_GETREWARD_ACK Ack)
	{
		this.SetRewardButton(true);
		if (Ack.i32Result == 0)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "ANCIENTRELIC_OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			this.SetRewardButton(false);
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("631");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			GameGuideTreasureAlarm gameGuideTreasureAlarm = NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.TREASURE_ALARM) as GameGuideTreasureAlarm;
			if (gameGuideTreasureAlarm != null)
			{
				gameGuideTreasureAlarm.SetInfo(string.Empty, 0, 0);
			}
		}
		else if (Ack.i32Result == -10)
		{
			this.SetRewardButton(false);
		}
		else if (Ack.i32Result == -11)
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("633");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
		else if (Ack.i32Result == -12)
		{
			string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("632");
			Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TREASUREBOX_DLG);
		}
		else if (Ack.i32Result == -13)
		{
			string textFromNotify4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("634");
			Main_UI_SystemMessage.ADDMessage(textFromNotify4, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TREASUREBOX_DLG);
		}
		else
		{
			string textFromNotify5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("634");
			Main_UI_SystemMessage.ADDMessage(textFromNotify5, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TREASUREBOX_DLG);
			TsLog.LogWarning("!!!!!!!!!!!!!!!!! Reward - Error {0}", new object[]
			{
				Ack.i32Result
			});
		}
	}

	public void On_ClickGetItem(IUIObject a_cObject)
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsTreasure())
		{
			return;
		}
		if (this.m_i16TreasureReward != -1)
		{
			GS_TREASUREBOX_GETREWARD_REQ gS_TREASUREBOX_GETREWARD_REQ = new GS_TREASUREBOX_GETREWARD_REQ();
			gS_TREASUREBOX_GETREWARD_REQ.i16TreasureUnique = this.m_i16TreasureReward;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TREASUREBOX_GETREWARD_REQ, gS_TREASUREBOX_GETREWARD_REQ);
		}
	}

	public void On_ClickSetPos(IUIObject a_cObject)
	{
		if (this.m_i16TreasureReward != -1)
		{
			DateTime dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
			GS_TREASUREBOX_SHARING_REQ gS_TREASUREBOX_SHARING_REQ = new GS_TREASUREBOX_SHARING_REQ();
			gS_TREASUREBOX_SHARING_REQ.i32Day = dueDate.Day;
			gS_TREASUREBOX_SHARING_REQ.i16TreasureUnique = this.m_i16TreasureReward;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TREASUREBOX_SHARING_REQ, gS_TREASUREBOX_SHARING_REQ);
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("663");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}
	}
}
