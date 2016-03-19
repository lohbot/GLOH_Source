using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class BountyCheckDlg : Form
{
	private DrawTexture m_dtBG;

	private Label m_lbMonsterName;

	private DrawTexture m_dtClearRank;

	private DrawTexture m_dtClearRankBG;

	private DrawTexture m_dtMonFaceBG;

	private DrawTexture m_dtMonFace;

	private ItemTexture m_itRewardItem;

	private Label m_lbReward;

	private Button m_btAccept;

	private string m_strText = string.Empty;

	private short m_iBountyHuntUnique;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Bounty/DLG_Bounty_Check", G_ID.BOUNTYCHECK_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtBG = (base.GetControl("DrawTexture_MonSlotBG") as DrawTexture);
		this.m_lbMonsterName = (base.GetControl("Label_MonsterName") as Label);
		this.m_lbMonsterName.SetText(string.Empty);
		this.m_dtClearRank = (base.GetControl("DrawTexture_Rank") as DrawTexture);
		this.m_dtClearRankBG = (base.GetControl("DrawTexture_rankbg") as DrawTexture);
		this.m_dtMonFaceBG = (base.GetControl("DrawTexture_MonFaceBG") as DrawTexture);
		this.m_dtMonFace = (base.GetControl("DrawTexture_MonFace") as DrawTexture);
		this.m_itRewardItem = (base.GetControl("ItemTexture_Reward") as ItemTexture);
		this.m_lbReward = (base.GetControl("Label_Reward") as Label);
		this.m_lbReward.SetText(string.Empty);
		this.m_btAccept = (base.GetControl("Button_StartBTN") as Button);
		this.m_btAccept.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAccept));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	private void ClickAccept(IUIObject obj)
	{
		if (0 >= this.m_iBountyHuntUnique)
		{
			return;
		}
		if (this.m_iBountyHuntUnique == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique)
		{
			NrTSingleton<BountyHuntManager>.Instance.AutoMoveClientNPC(this.m_iBountyHuntUnique);
			base.CloseNow();
			return;
		}
		if (!NrTSingleton<BountyHuntManager>.Instance.IsAllClear(NrTSingleton<BountyHuntManager>.Instance.Week) && !NrTSingleton<BountyHuntManager>.Instance.IsBountyHuntNextEpisode(this.m_iBountyHuntUnique))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("710"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		GS_BOUNTYHUNT_ACCEPT_REQ gS_BOUNTYHUNT_ACCEPT_REQ = new GS_BOUNTYHUNT_ACCEPT_REQ();
		gS_BOUNTYHUNT_ACCEPT_REQ.i16CurBountyHuntUnique = this.m_iBountyHuntUnique;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BOUNTYHUNT_ACCEPT_REQ, gS_BOUNTYHUNT_ACCEPT_REQ);
		base.CloseNow();
	}

	public void SetEpisode(BountyInfoData Data)
	{
		if (Data == null)
		{
			return;
		}
		this.m_strText = string.Format("UI/Bounty/{0}", Data.strMonBG);
		this.m_dtBG.SetTextureFromBundle(this.m_strText);
		this.m_iBountyHuntUnique = Data.i16Unique;
		if (this.m_iBountyHuntUnique == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique)
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_EMERGENCY", this.m_btAccept, this.m_btAccept.GetSize());
			this.m_btAccept.AddGameObjectDelegate(new EZGameObjectDelegate(this.ButtonAddEffectDelegate));
			this.m_btAccept.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2382"));
		}
		else
		{
			this.m_btAccept.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2327"));
		}
		byte bountyHuntClearRank = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetBountyHuntClearRank(Data.i16Unique);
		if (0 < bountyHuntClearRank)
		{
			this.m_dtClearRank.Visible = true;
			this.m_dtClearRank.SetTexture(NrTSingleton<BountyHuntManager>.Instance.GetBountyRankImgText(bountyHuntClearRank));
			this.m_dtClearRankBG.Visible = true;
		}
		else
		{
			this.m_dtClearRank.Visible = false;
			this.m_dtClearRankBG.Visible = false;
		}
		this.m_dtMonFaceBG.SetTexture(Data.strMonBG);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(Data.i32NPCCharKind);
		if (charKindInfo != null)
		{
			this.m_lbMonsterName.SetText(charKindInfo.GetName());
		}
		this.m_dtMonFace.SetTexture(eCharImageType.LARGE, Data.i32NPCCharKind, 0);
		int num = Data.i32FirstRewardUnique;
		int num2 = Data.i32FirstRewardNum;
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsBountyHuntClearUnique(Data.i16Unique))
		{
			num = Data.i32RepeatRewardUnique;
			num2 = Data.i32RepeatRewardNum;
		}
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = num;
		iTEM.m_nItemNum = num2;
		this.m_itRewardItem.SetItemTexture(iTEM);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
			"itemname",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(num),
			"count",
			num2
		});
		this.m_lbReward.SetText(this.m_strText);
	}

	public void ButtonAddEffectDelegate(IUIObject control, GameObject obj)
	{
		if (obj == null || control == null)
		{
			return;
		}
		obj.transform.localScale = new Vector3(5f, 1f, 1f);
	}

	public override void OnClose()
	{
		base.OnClose();
		BountyHuntingDlg bountyHuntingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOUNTYHUNTING_DLG) as BountyHuntingDlg;
		if (bountyHuntingDlg != null)
		{
			bountyHuntingDlg.SetCurEffect(true);
		}
	}
}
