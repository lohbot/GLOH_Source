using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class ColosseumChallengeCheck : Form
{
	private Label m_LeaderName;

	private Label m_EpisodeSummary;

	private DrawTexture m_Image;

	private Button m_Start;

	private Button m_btClose;

	private int m_nSelectStep;

	private UIButton _GuideItem;

	private float _ButtonZ;

	private int m_nWinID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Challenge_Check", G_ID.COLOSSEUM_CHALLENGE_CHECK_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_LeaderName = (base.GetControl("Label_LeaderName") as Label);
		this.m_EpisodeSummary = (base.GetControl("Label_Summary") as Label);
		this.m_Image = (base.GetControl("DrawTexture_LeaderFace") as DrawTexture);
		this.m_Start = (base.GetControl("Button_StartBTN") as Button);
		this.m_Start.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickStart));
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		base.SetScreenCenter();
	}

	public void SetEpisode(int nSelectStep)
	{
		if (nSelectStep < 0)
		{
			return;
		}
		this.m_nSelectStep = nSelectStep;
		BASE_COLOSSEUM_CHALLENGE_DATA colosseumChallengeData = COLOSSEUM_CHALLENGE_DATA.GetColosseumChallengeData(nSelectStep);
		if (colosseumChallengeData != null)
		{
			this.m_LeaderName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(colosseumChallengeData.m_i32InterfaceKey.ToString()));
			this.m_EpisodeSummary.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(colosseumChallengeData.m_nSummary.ToString()));
			ECO eco = NrTSingleton<NrBaseTableManager>.Instance.GetEco(colosseumChallengeData.m_i32EcoIndex.ToString());
			if (eco != null)
			{
				this.m_Image.SetTexture(eCharImageType.LARGE, NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco.szCharCode[0]), -1, string.Empty);
			}
		}
	}

	private void ClickStart(IUIObject obj)
	{
		if (this.m_nSelectStep < 0)
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COLOSSEUM_CHALLENGE);
		if (charSubData < (long)this.m_nSelectStep)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("305"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_COLOSSEUM_DUMMY_MATCH_REQ gS_COLOSSEUM_DUMMY_MATCH_REQ = new GS_COLOSSEUM_DUMMY_MATCH_REQ();
		gS_COLOSSEUM_DUMMY_MATCH_REQ.i64PersonID = 0L;
		gS_COLOSSEUM_DUMMY_MATCH_REQ.nSelectStepIndex = this.m_nSelectStep;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_DUMMY_MATCH_REQ, gS_COLOSSEUM_DUMMY_MATCH_REQ);
	}

	public void SendBattle(MsgBoxUI msgbox, object obj)
	{
		GS_COLOSSEUM_DUMMY_MATCH_REQ obj2 = obj as GS_COLOSSEUM_DUMMY_MATCH_REQ;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_DUMMY_MATCH_REQ, obj2);
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		this._GuideItem = this.m_Start;
		this.m_nWinID = winID;
		if (null != this._GuideItem)
		{
			this._ButtonZ = this._GuideItem.gameObject.transform.localPosition.z;
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				this._GuideItem.EffectAni = false;
				Vector2 vector = new Vector2(base.GetLocationX() + this._GuideItem.GetLocationX() + 80f, base.GetLocationY() + this._GuideItem.GetLocationY() - 10f);
				uI_UIGuide.Move(vector, vector);
				this._ButtonZ = this._GuideItem.gameObject.transform.localPosition.z;
				this._GuideItem.SetLocationZ(uI_UIGuide.GetLocation().z - base.GetLocation().z - 1f);
				this._GuideItem.AlphaAni(1f, 0.5f, -0.5f);
			}
		}
		else
		{
			Debug.LogError("_GuideItem == NULL");
		}
	}

	public void HideUIGuide()
	{
		if (null != this._GuideItem)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			this._GuideItem.gameObject.transform.localPosition = new Vector3(this._GuideItem.gameObject.transform.localPosition.x, this._GuideItem.gameObject.transform.localPosition.y, -this._ButtonZ);
			this._GuideItem.StopAni();
			this._GuideItem.AlphaAni(1f, 1f, 0f);
		}
		this._GuideItem = null;
	}
}
