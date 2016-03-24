using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class MythRaid_Lobby_DLG : Form
{
	private const float ROTATE_SPEED = 5f;

	private const byte PAGEINDEX_MIN = 1;

	private const byte PAGEINDEX_MAX = 10;

	private const byte ROUNDCLEARREWARD_MAX = 6;

	private Button bt_Enter;

	private DrawTexture dt_RaidBoss;

	private DrawTexture dt_RaidBG;

	private NewListBox nlb_Rank;

	private Toolbar tb_Rank;

	private Label lb_Rank;

	private Label lb_RoundInfo;

	private Label lb_MyRank_Rank;

	private Label lb_MyRank_Name;

	private Label lb_MyRank_Damage;

	private Label lb_SolRank;

	private Label lb_PartyRank;

	private Label lb_SolDamage;

	private Label lb_PartyDamage;

	private Button bt_RankPage1;

	private Button bt_RankPage2;

	private Label lb_RankPageInfo;

	private DrawTexture dt_RaidBoss_BG;

	private DrawTexture dt_RaidBoss_effect;

	private Button bt_RewardInfo;

	private DrawTexture dt_RoundInfo_BG;

	private DrawTexture dt_SoloRank_BG;

	private DrawTexture dt_PartyRank_BG;

	private DrawTexture dt_RewardAlarm;

	private Button bt_PartySearch;

	private Button bt_SearchCancel;

	private DrawTexture dt_PartySearch_Waiting;

	private NewListBox nlb_ClearRewardinfo;

	private Button btn_Back;

	private Label lb_BossDESC;

	private Label lb_BossName;

	private Dictionary<int, MYTHRAID_RANK_INFO[]> dic_SoloRankInfo = new Dictionary<int, MYTHRAID_RANK_INFO[]>();

	private Dictionary<int, MYTHRAID_RANK_INFO[]> dic_PartyRankInfo = new Dictionary<int, MYTHRAID_RANK_INFO[]>();

	private bool isSoloMode = true;

	private short currentPageIndex;

	private bool isAdjust;

	private float listChangeTime;

	private bool isListChange;

	private Vector3[] enablePosition = new Vector3[3];

	private Vector3[] disablePosition = new Vector3[3];

	public bool canGetReward;

	private Vector3 partyCancelPosition;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "mythraid/dlg_myth_lobby", G_ID.MYTHRAID_LOBBY_DLG, false);
	}

	public override void Close()
	{
		if (NrTSingleton<MythRaidManager>.Instance.IsPartySearch)
		{
			NrTSingleton<MythRaidManager>.Instance.MatchStartCancel(false);
		}
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MYTHRAID_MODESELECT_DLG))
		{
			this.SoundMuteRestore();
		}
		base.Close();
	}

	public override void CloseForm(IUIObject obj)
	{
		if (NrTSingleton<MythRaidManager>.Instance.IsPartySearch)
		{
			NrTSingleton<MythRaidManager>.Instance.MatchStartCancel(false);
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MYTHRAID_MODESELECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MYTHRAID_MODESELECT_DLG);
		}
		this.SoundMuteRestore();
		base.CloseForm(obj);
	}

	public override void SetComponent()
	{
		base.SetScreenCenter();
		this.bt_Enter = (base.GetControl("BT_Start") as Button);
		Button expr_22 = this.bt_Enter;
		expr_22.Click = (EZValueChangedDelegate)Delegate.Combine(expr_22.Click, new EZValueChangedDelegate(this._clickEnter));
		this.lb_BossDESC = (base.GetControl("LB_BossDESC") as Label);
		this.lb_BossName = (base.GetControl("LB_BossName") as Label);
		int kind = 3806;
		MYTHRAIDINFO_DATA mythRaidInfoData = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidInfoData(NrTSingleton<MythRaidManager>.Instance.GetMyInfo().nRaidSeason.ToString() + NrTSingleton<MythRaidManager>.Instance.GetMyInfo().nRaidType.ToString());
		if (mythRaidInfoData == null)
		{
			Debug.LogError("MythRaid Info Load Fail");
		}
		else
		{
			kind = mythRaidInfoData.nMainBossCharKind;
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(mythRaidInfoData.nMainBossCharKind);
			this.lb_BossDESC.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(mythRaidInfoData.i32TextExplain.ToString()));
			if (charKindInfo != null)
			{
				this.lb_BossName.SetText(charKindInfo.GetName());
			}
		}
		this.dt_RaidBoss = (base.GetControl("DT_RaidBoss_Img") as DrawTexture);
		this.dt_RaidBoss.SetTexture(eCharImageType.LARGE, kind, -1, string.Empty);
		this.dt_RaidBG = (base.GetControl("DT_Raid_BG") as DrawTexture);
		this.dt_RaidBG.SetTextureFromBundle("UI/mythicraid/mythic_raid_bg");
		this.dt_RaidBoss_BG = (base.GetControl("DT_RaidBoss_BG") as DrawTexture);
		this.dt_RaidBoss_BG.SetTextureFromBundle("UI/mythicraid/mythic_raid_monsterbox");
		this.dt_RaidBoss_effect = (base.GetControl("DT_RaidBoss_effect") as DrawTexture);
		NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("ui/mythicraid/fx_myth_raid_lobby_mobile", this.dt_RaidBoss_effect, this.dt_RaidBoss_effect.GetSize());
		this.bt_RewardInfo = (base.GetControl("BT_RewardInfo") as Button);
		Button expr_1D8 = this.bt_RewardInfo;
		expr_1D8.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1D8.Click, new EZValueChangedDelegate(this.OnClickRewardGet));
		this.bt_RewardInfo.Visible = false;
		this.bt_PartySearch = (base.GetControl("BT_PartySearch") as Button);
		Button expr_221 = this.bt_PartySearch;
		expr_221.Click = (EZValueChangedDelegate)Delegate.Combine(expr_221.Click, new EZValueChangedDelegate(this.OnClickPartySearch));
		this.bt_SearchCancel = (base.GetControl("BT_SearchCancel") as Button);
		Button expr_25E = this.bt_SearchCancel;
		expr_25E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_25E.Click, new EZValueChangedDelegate(this.OnClickSearchCancel));
		this.bt_SearchCancel.Visible = false;
		this.partyCancelPosition = this.bt_SearchCancel.transform.localPosition;
		this.dt_RewardAlarm = (base.GetControl("DT_RewardAlarm") as DrawTexture);
		this.dt_RewardAlarm.Visible = false;
		this.dt_PartySearch_Waiting = (base.GetControl("DT_PartySearch_Waiting") as DrawTexture);
		this.btn_Back = (base.GetControl("BTN_Back") as Button);
		this.btn_Back.Click = new EZValueChangedDelegate(this.OnClickBack);
		this.tb_Rank = (base.GetControl("ToolBar_Rank") as Toolbar);
		this.nlb_Rank = (base.GetControl("NLB_Rank") as NewListBox);
		this.tb_Rank.Control_Tab[0].ButtonClick = new EZValueChangedDelegate(this.OnClickSoloRank);
		this.tb_Rank.Control_Tab[0].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3152"));
		this.tb_Rank.Control_Tab[1].ButtonClick = new EZValueChangedDelegate(this.OnClickPartyRank);
		this.tb_Rank.Control_Tab[1].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3153"));
		this.tb_Rank.Control_Tab[2].ButtonClick = new EZValueChangedDelegate(this.OnClickMyInfo);
		this.tb_Rank.Control_Tab[2].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3138"));
		this.enablePosition[0] = this.tb_Rank.Control_Tab[0].spriteText.transform.localPosition;
		this.enablePosition[1] = this.tb_Rank.Control_Tab[1].spriteText.transform.localPosition;
		this.enablePosition[2] = this.tb_Rank.Control_Tab[2].spriteText.transform.localPosition;
		this.disablePosition[0].x = this.enablePosition[0].x;
		this.disablePosition[0].y = this.enablePosition[0].y - 7f;
		this.disablePosition[0].z = this.enablePosition[0].z;
		this.disablePosition[1].x = this.enablePosition[1].x;
		this.disablePosition[1].y = this.enablePosition[1].y - 7f;
		this.disablePosition[1].z = this.enablePosition[1].z;
		this.disablePosition[2].x = this.enablePosition[2].x;
		this.disablePosition[2].y = this.enablePosition[2].y - 7f;
		this.disablePosition[2].z = this.enablePosition[2].z;
		this.lb_MyRank_Rank = (base.GetControl("LB_MyRank_Rank") as Label);
		this.lb_MyRank_Name = (base.GetControl("LB_MyRank_Name") as Label);
		this.lb_MyRank_Damage = (base.GetControl("LB_MyRank_Damage") as Label);
		this.TextAniSetting(this.lb_MyRank_Damage, -1f, 0.01f, 0.5f, true, true, true);
		this.bt_RankPage1 = (base.GetControl("BT_RankPage1") as Button);
		Button expr_636 = this.bt_RankPage1;
		expr_636.Click = (EZValueChangedDelegate)Delegate.Combine(expr_636.Click, new EZValueChangedDelegate(this.OnClickPagePrev));
		this.bt_RankPage2 = (base.GetControl("BT_RankPage2") as Button);
		Button expr_673 = this.bt_RankPage2;
		expr_673.Click = (EZValueChangedDelegate)Delegate.Combine(expr_673.Click, new EZValueChangedDelegate(this.OnClickPageNext));
		this.lb_RankPageInfo = (base.GetControl("LB_RankPageInfo") as Label);
		this.lb_SolRank = (base.GetControl("LB_SolRank") as Label);
		this.TextAniSetting(this.lb_SolRank, 1.2f, 0.01f, 0.4f, true, true, false);
		this.lb_PartyRank = (base.GetControl("LB_PartyRank") as Label);
		this.TextAniSetting(this.lb_PartyRank, 1.2f, 0.01f, 0.4f, true, true, false);
		this.lb_SolDamage = (base.GetControl("LB_SolDamage") as Label);
		this.TextAniSetting(this.lb_SolDamage, -1f, 0.01f, 0.5f, true, true, true);
		this.lb_PartyDamage = (base.GetControl("LB_PartyDamage") as Label);
		this.TextAniSetting(this.lb_PartyDamage, -1f, 0.01f, 0.5f, true, true, true);
		this.dt_SoloRank_BG = (base.GetControl("DT_SoloRank_BG") as DrawTexture);
		this.dt_SoloRank_BG.SetTextureFromBundle("UI/mythicraid/mythic_raid_dragonframe");
		this.dt_PartyRank_BG = (base.GetControl("DT_PartyRank_BG") as DrawTexture);
		this.dt_PartyRank_BG.SetTextureFromBundle("UI/mythicraid/mythic_raid_dragonframe");
		this.lb_RoundInfo = (base.GetControl("LB_RoundInfo") as Label);
		this.dt_RoundInfo_BG = (base.GetControl("DT_RoundInfo_BG") as DrawTexture);
		this.dt_RoundInfo_BG.SetTextureFromBundle("UI/mythicraid/mythic_raid_dragonframe");
		this.nlb_ClearRewardinfo = (base.GetControl("NLB_ClearRewardinfo") as NewListBox);
		this.nlb_ClearRewardinfo.BackButtonAniEnable(false);
		base.ShowBlackBG(1f);
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
		if (this.dt_RaidBoss_effect.transform.childCount > 0 && !this.isAdjust)
		{
			this.dt_RaidBoss_effect.transform.GetChild(0).transform.localPosition = new Vector3(this.dt_RaidBoss_effect.transform.GetChild(0).transform.localPosition.x, this.dt_RaidBoss_effect.transform.GetChild(0).transform.localPosition.y, 0f);
			this.isAdjust = true;
		}
		if (NrTSingleton<MythRaidManager>.Instance.IsPartySearch)
		{
			this.dt_PartySearch_Waiting.Rotate(5f);
		}
		if (Time.realtimeSinceStartup - this.listChangeTime >= 0.3f && !this.isListChange)
		{
			this.nlb_ClearRewardinfo.ScrollToItem(NrTSingleton<MythRaidManager>.Instance.GetMyInfo().clearRound - 1, 0.5f);
			this.isListChange = true;
		}
		base.Update();
	}

	public override void Show()
	{
		this.SetSearchVisible(false);
		switch (NrTSingleton<MythRaidManager>.Instance.GetRaidType())
		{
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_EASY:
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_NORMAL:
			this.Show_EasyNormal();
			break;
		case eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD:
			this.Show_Hard();
			break;
		}
		this.listChangeTime = Time.realtimeSinceStartup;
		this.isListChange = false;
		base.Show();
	}

	public void ShowRank()
	{
		Dictionary<int, MYTHRAID_RANK_INFO[]> dictionary;
		byte ui8Type;
		if (this.isSoloMode)
		{
			dictionary = this.dic_SoloRankInfo;
			ui8Type = 0;
		}
		else
		{
			dictionary = this.dic_PartyRankInfo;
			ui8Type = 1;
		}
		if (dictionary.ContainsKey((int)this.currentPageIndex))
		{
			this.ShowList((int)this.currentPageIndex);
		}
		else
		{
			GS_MYTHRAID_RANKINFO_REQ gS_MYTHRAID_RANKINFO_REQ = new GS_MYTHRAID_RANKINFO_REQ();
			gS_MYTHRAID_RANKINFO_REQ.i16PageIndex = this.currentPageIndex;
			gS_MYTHRAID_RANKINFO_REQ.ui8Type = ui8Type;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_RANKINFO_REQ, gS_MYTHRAID_RANKINFO_REQ);
		}
	}

	public void SetSoloRank(short pageIndex, MYTHRAID_RANK_INFO[] rankInfoArray)
	{
		this.isSoloMode = true;
		this.currentPageIndex = pageIndex;
		if (this.dic_SoloRankInfo.ContainsKey((int)pageIndex))
		{
			this.dic_SoloRankInfo[(int)pageIndex] = rankInfoArray;
		}
		else
		{
			this.dic_SoloRankInfo.Add((int)pageIndex, rankInfoArray);
		}
	}

	public void SetPartyRank(short pageIndex, MYTHRAID_RANK_INFO[] rankInfoArray)
	{
		this.isSoloMode = false;
		this.currentPageIndex = pageIndex;
		if (this.dic_PartyRankInfo.ContainsKey((int)pageIndex))
		{
			this.dic_PartyRankInfo[(int)pageIndex] = rankInfoArray;
		}
		else
		{
			this.dic_PartyRankInfo.Add((int)pageIndex, rankInfoArray);
		}
	}

	public void SetSearchVisible(bool isShow)
	{
		base.SetShowLayer(4, isShow);
		this.bt_Enter.SetEnabled(!isShow);
		if (isShow)
		{
			this.bt_SearchCancel.transform.localPosition = this.partyCancelPosition;
		}
	}

	public void SetRewardTextureVisible(bool isShow)
	{
		this.dt_RewardAlarm.Visible = isShow;
	}

	public void SetMyInfo()
	{
		this.lb_MyRank_Name.SetText(NrTSingleton<NkCharManager>.Instance.GetCharName());
		string text = "-";
		if (NrTSingleton<MythRaidManager>.Instance.GetRank(this.isSoloMode) > 0)
		{
			text = NrTSingleton<MythRaidManager>.Instance.GetRank(this.isSoloMode).ToString();
		}
		string text2 = "-";
		if (NrTSingleton<MythRaidManager>.Instance.GetDamage(this.isSoloMode) > 0L)
		{
			text2 = NrTSingleton<MythRaidManager>.Instance.GetDamage(this.isSoloMode).ToString();
		}
		this.lb_MyRank_Rank.SetText(text);
		string targetText = NrTSingleton<MythRaidManager>.Instance.AddComma(text2);
		NrTSingleton<UILabelAnimationManager>.Instance.PrevDataDelete(this.lb_MyRank_Damage);
		NrTSingleton<UILabelAnimationManager>.Instance.TextUpdateAndPlayAni(this.lb_MyRank_Damage, targetText);
	}

	private void OnClickBack(IUIObject obj)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MYTHRAID_MODESELECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTHRAID_MODESELECT_DLG);
		}
		this.Close();
	}

	private void OnClickPartySearch(IUIObject obj)
	{
		NrTSingleton<MythRaidManager>.Instance.MatchStartCancel(true);
	}

	private void OnClickSearchCancel(IUIObject obj)
	{
		NrTSingleton<MythRaidManager>.Instance.MatchStartCancel(false);
	}

	private void Show_EasyNormal()
	{
		this.lb_RoundInfo.SetText(NrTSingleton<MythRaidManager>.Instance.GetMyInfo().clearRound.ToString());
		this.tb_Rank.Visible = false;
		this.bt_RewardInfo.Visible = false;
		this.ShowRewardInfo();
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, true);
	}

	private void Show_Hard()
	{
		this.tb_Rank.Visible = true;
		this.bt_RewardInfo.Visible = true;
		this.OnClickSoloRank(null);
	}

	private void OnClickSoloRank(IUIObject obj)
	{
		this.isSoloMode = true;
		this.currentPageIndex = 1;
		this.tb_Rank.Control_Tab[0].spriteText.transform.localPosition = new Vector3(this.enablePosition[0].x, this.enablePosition[0].y, this.enablePosition[0].z - 0.1f);
		this.tb_Rank.Control_Tab[0].spriteTextShadow.transform.localPosition = this.enablePosition[0];
		this.tb_Rank.Control_Tab[1].spriteText.transform.localPosition = new Vector3(this.disablePosition[1].x, this.disablePosition[1].y, this.disablePosition[1].z - 0.1f);
		this.tb_Rank.Control_Tab[1].spriteTextShadow.transform.localPosition = this.disablePosition[1];
		this.tb_Rank.Control_Tab[2].spriteText.transform.localPosition = new Vector3(this.disablePosition[2].x, this.disablePosition[2].y, this.disablePosition[2].z - 0.1f);
		this.tb_Rank.Control_Tab[2].spriteTextShadow.transform.localPosition = this.disablePosition[2];
		this.tb_Rank.Control_Tab[0].SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + this.tb_Rank.Control_Tab[0].GetText());
		this.tb_Rank.Control_Tab[1].SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1102") + this.tb_Rank.Control_Tab[1].GetText());
		this.tb_Rank.Control_Tab[2].SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1102") + this.tb_Rank.Control_Tab[2].GetText());
		this.ShowRank();
		this.SetMyInfo();
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, false);
	}

	private void OnClickPartyRank(IUIObject obj)
	{
		this.isSoloMode = false;
		this.currentPageIndex = 1;
		this.tb_Rank.Control_Tab[0].spriteText.transform.localPosition = new Vector3(this.disablePosition[0].x, this.disablePosition[0].y, this.disablePosition[0].z - 0.1f);
		this.tb_Rank.Control_Tab[0].spriteTextShadow.transform.localPosition = this.disablePosition[0];
		this.tb_Rank.Control_Tab[1].spriteText.transform.localPosition = new Vector3(this.enablePosition[1].x, this.enablePosition[1].y, this.enablePosition[1].z - 0.1f);
		this.tb_Rank.Control_Tab[1].spriteTextShadow.transform.localPosition = this.enablePosition[1];
		this.tb_Rank.Control_Tab[2].spriteText.transform.localPosition = new Vector3(this.disablePosition[2].x, this.disablePosition[2].y, this.disablePosition[2].z - 0.1f);
		this.tb_Rank.Control_Tab[2].spriteTextShadow.transform.localPosition = this.disablePosition[2];
		this.tb_Rank.Control_Tab[0].SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1102") + this.tb_Rank.Control_Tab[0].GetText());
		this.tb_Rank.Control_Tab[1].SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + this.tb_Rank.Control_Tab[1].GetText());
		this.tb_Rank.Control_Tab[2].SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1102") + this.tb_Rank.Control_Tab[2].GetText());
		this.ShowRank();
		this.SetMyInfo();
		base.SetShowLayer(1, true);
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, false);
	}

	private void OnClickMyInfo(IUIObject obj)
	{
		this.tb_Rank.Control_Tab[0].spriteText.transform.localPosition = new Vector3(this.disablePosition[0].x, this.disablePosition[0].y, this.disablePosition[0].z - 0.1f);
		this.tb_Rank.Control_Tab[0].spriteTextShadow.transform.localPosition = this.disablePosition[0];
		this.tb_Rank.Control_Tab[1].spriteText.transform.localPosition = new Vector3(this.disablePosition[1].x, this.disablePosition[1].y, this.disablePosition[1].z - 0.1f);
		this.tb_Rank.Control_Tab[1].spriteTextShadow.transform.localPosition = this.disablePosition[1];
		this.tb_Rank.Control_Tab[2].spriteText.transform.localPosition = new Vector3(this.enablePosition[2].x, this.enablePosition[2].y, this.enablePosition[2].z - 0.1f);
		this.tb_Rank.Control_Tab[2].spriteTextShadow.transform.localPosition = this.enablePosition[2];
		this.tb_Rank.Control_Tab[0].SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1102") + this.tb_Rank.Control_Tab[0].GetText());
		this.tb_Rank.Control_Tab[1].SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1102") + this.tb_Rank.Control_Tab[1].GetText());
		this.tb_Rank.Control_Tab[2].SetText(NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + this.tb_Rank.Control_Tab[2].GetText());
		GS_MYTHRAID_CHARINFO_ACK myInfo = NrTSingleton<MythRaidManager>.Instance.GetMyInfo();
		string targetText = "-";
		string targetText2 = "-";
		string targetText3 = "-";
		string targetText4 = "-";
		if (myInfo.soloRank > 0)
		{
			targetText = myInfo.soloRank.ToString();
		}
		if (myInfo.partyRank > 0)
		{
			targetText2 = myInfo.partyRank.ToString();
		}
		if (myInfo.soloDamage > 0L)
		{
			targetText3 = myInfo.soloDamage.ToString();
		}
		if (myInfo.partyDamage > 0L)
		{
			targetText4 = myInfo.partyDamage.ToString();
		}
		NrTSingleton<UILabelAnimationManager>.Instance.TextUpdateAndPlayAni(this.lb_SolRank, targetText);
		NrTSingleton<UILabelAnimationManager>.Instance.PrevDataDelete(this.lb_SolRank);
		NrTSingleton<UILabelAnimationManager>.Instance.TextUpdateAndPlayAni(this.lb_PartyRank, targetText2);
		NrTSingleton<UILabelAnimationManager>.Instance.PrevDataDelete(this.lb_PartyRank);
		NrTSingleton<UILabelAnimationManager>.Instance.TextUpdateAndPlayAni(this.lb_SolDamage, targetText3);
		NrTSingleton<UILabelAnimationManager>.Instance.PrevDataDelete(this.lb_SolDamage);
		NrTSingleton<UILabelAnimationManager>.Instance.TextUpdateAndPlayAni(this.lb_PartyDamage, targetText4);
		NrTSingleton<UILabelAnimationManager>.Instance.PrevDataDelete(this.lb_PartyDamage);
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, true);
		base.SetShowLayer(3, false);
	}

	private void OnClickPagePrev(IUIObject obj)
	{
		this.currentPageIndex -= 1;
		if (this.currentPageIndex <= 1)
		{
			this.currentPageIndex = 1;
		}
		else if (this.currentPageIndex >= 10)
		{
			this.currentPageIndex = 10;
		}
		this.ShowRank();
	}

	private void OnClickPageNext(IUIObject obj)
	{
		this.currentPageIndex += 1;
		if (this.currentPageIndex <= 1)
		{
			this.currentPageIndex = 1;
		}
		else if (this.currentPageIndex >= 10)
		{
			this.currentPageIndex = 10;
		}
		this.ShowRank();
	}

	private void ShowList(int pageIndex)
	{
		this.lb_RankPageInfo.SetText(pageIndex.ToString());
		this.nlb_Rank.Clear();
		MYTHRAID_RANK_INFO[] array;
		if (this.isSoloMode)
		{
			array = this.dic_SoloRankInfo[pageIndex];
		}
		else
		{
			array = this.dic_PartyRankInfo[pageIndex];
		}
		for (int i = 0; i < array.Length; i++)
		{
			NewListItem newListItem = new NewListItem(this.nlb_Rank.ColumnNum, true, string.Empty);
			string text = NrTSingleton<MythRaidManager>.Instance.AddComma(array[i].i64Damage.ToString());
			newListItem.SetListItemData(4, array[i].i32Rank.ToString(), null, null, null);
			newListItem.SetListItemData(1, TKString.NEWString(array[i].strName), null, null, null);
			newListItem.SetListItemData(2, text, null, null, null);
			newListItem.Data = 123;
			this.nlb_Rank.Add(newListItem);
		}
		this.nlb_Rank.RepositionItems();
	}

	private void _clickEnter(IUIObject _obj)
	{
		this.SoundMuteRestore();
		NrTSingleton<MythRaidManager>.Instance.OpenBatchMode();
	}

	private void OnClickRewardGet(IUIObject obj)
	{
		NrTSingleton<MythRaidManager>.Instance.RewardGet();
	}

	public void SetRewardUI(bool value)
	{
		this.dt_RewardAlarm.Visible = value;
	}

	private void ShowRewardInfo()
	{
		if (NrTSingleton<MythRaidManager>.Instance.GetRaidType() == eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD)
		{
			Debug.LogError("하드 모드일 때는 이 함수가 사용되지 않습니다.");
			return;
		}
		this.nlb_ClearRewardinfo.Clear();
		for (int i = 0; i < 6; i++)
		{
			int num = i + 1;
			NewListItem newListItem = new NewListItem(this.nlb_ClearRewardinfo.ColumnNum, true, string.Empty);
			MYTHRAID_CLEAR_REWARD_INFO mythRaidClearRewardData = NrTSingleton<NrBaseTableManager>.Instance.GetMythRaidClearRewardData((int)((byte)NrTSingleton<MythRaidManager>.Instance.GetRaidType()), num);
			string text = string.Empty;
			for (int j = 0; j < MYTHRAID_CLEAR_REWARD_INFO.MAX_REWARD_COUNT; j++)
			{
				if (mythRaidClearRewardData.REWARD_UNIQUE[j] == 0)
				{
					break;
				}
				if (j >= 1)
				{
					text += " + ";
				}
				string textFromItem = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(mythRaidClearRewardData.REWARD_UNIQUE[j].ToString());
				text = string.Format("{0}{1} X {2}", text, textFromItem, mythRaidClearRewardData.REWARD_NUMBER[j]);
			}
			string str = string.Empty;
			if (num <= NrTSingleton<MythRaidManager>.Instance.GetMyInfo().clearRound)
			{
				str = NrTSingleton<CTextParser>.Instance.GetTextColor("1102");
			}
			else
			{
				str = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
			}
			newListItem.SetListItemData(0, str + text, null, null, null);
			string empty = string.Empty;
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3253");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"count",
				num.ToString()
			});
			newListItem.SetListItemData(2, str + empty, null, null, null);
			this.nlb_ClearRewardinfo.Add(newListItem);
		}
		this.nlb_ClearRewardinfo.RepositionItems();
	}

	private void TextAniSetting(Label text, float loopTime, float loopInterval, float nextValueStopInterval, bool reverse, bool changePartUpdate, bool useComma)
	{
		if (text == null)
		{
			return;
		}
		UILabelStepByStepAniInfo aniInfo = default(UILabelStepByStepAniInfo);
		aniInfo.loopTime = loopTime;
		aniInfo.loopInterval = loopInterval;
		aniInfo.nextValueStopInterval = nextValueStopInterval;
		aniInfo.reverse = reverse;
		aniInfo.changePartUpdate = changePartUpdate;
		aniInfo.useComma = useComma;
		NrTSingleton<UILabelAnimationManager>.Instance.TextAniSetting(text, aniInfo);
	}

	private void SoundMuteRestore()
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
	}
}
