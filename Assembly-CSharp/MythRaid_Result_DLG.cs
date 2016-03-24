using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL.GAME;
using StageHelper;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class MythRaid_Result_DLG : Form
{
	private DrawTexture dt_Background;

	private Button bt_Test;

	private bool isStageChange;

	private bool isStageChangeEnd;

	private Label lb_RoundCount;

	private Label lb_MyDamage;

	private Label lb_MyRank;

	private Label lb_Damage_Info;

	private Label lb_NextRankInfo;

	private Label lb_Damage_Refresh_info;

	private Label lb_OK;

	private Label lb_RoundHelper;

	private DrawTexture dt_RoundBG;

	private NewListBox nlb_HeroInfo;

	private DrawTexture dt_RaidResult;

	private Label lb_Loading;

	private int rountCount;

	private long totalDamage;

	private readonly int MAX_ONECOLUMN_SOLDIER = 5;

	private byte raidType;

	private float showTime;

	private bool isParty;

	private string[] itemName = new string[7];

	private int[] itemNum = new int[7];

	private bool canReward;

	private bool isBest;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		form.Scale = false;
		instance.LoadFileAll(ref form, "mythraid/dlg_myth_result", G_ID.MYTHRAID_RESULT_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.ChangeSceneDestory = false;
		Main_UI_SystemMessage.CloseUI();
		this.showTime = Time.realtimeSinceStartup;
	}

	public override void OnClose()
	{
		NrTSingleton<MythRaidManager>.Instance.Init();
		NrTSingleton<MythRaidManager>.Instance.ShowLobbyDlg();
		base.OnClose();
	}

	public override void SetComponent()
	{
		base.SetScreenCenter();
		this.dt_Background = (base.GetControl("DT_Result_BG") as DrawTexture);
		this.dt_Background.SetTextureFromBundle("UI/mythicraid/mythic_raid_bg");
		this.dt_RaidResult = (base.GetControl("DT_RaidResult") as DrawTexture);
		this.dt_RaidResult.SetTextureFromBundle("UI/mythicraid/mythic_raid_result");
		this.lb_OK = (base.GetControl("LB_OK") as Label);
		this.lb_Loading = (base.GetControl("LB_Loading") as Label);
		this.lb_RoundCount = (base.GetControl("LB_ClearRound") as Label);
		this.bt_Test = (base.GetControl("ButtonTest") as Button);
		Button expr_B0 = this.bt_Test;
		expr_B0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_B0.Click, new EZValueChangedDelegate(this._clickClose));
		this.lb_MyDamage = (base.GetControl("LB_MyDamage") as Label);
		this.lb_Damage_Info = (base.GetControl("LB_Damage_Info") as Label);
		this.lb_MyRank = (base.GetControl("LB_MyRank") as Label);
		this.lb_NextRankInfo = (base.GetControl("LB_NextRankInfo") as Label);
		this.lb_Damage_Refresh_info = (base.GetControl("LB_Damage_Refresh_info") as Label);
		this.nlb_HeroInfo = (base.GetControl("NLB_HeroInfo") as NewListBox);
		this.nlb_HeroInfo.BackButtonAniEnable(false);
		this.dt_RoundBG = (base.GetControl("DT_ClearRound_BG") as DrawTexture);
		this.dt_RoundBG.SetTextureFromBundle("UI/mythicraid/mythic_raid_dragonframe");
		this.lb_RoundHelper = (base.GetControl("LB_RoundHelper") as Label);
		base.ShowBlackBG(1f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	public void SetBG(WWWItem _item, object _param)
	{
		if (this == null)
		{
			return;
		}
		if (_item.isCanceled)
		{
			return;
		}
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				this.dt_Background.SetTexture(texture2D);
			}
		}
	}

	public override void InitData()
	{
		base.InitData();
	}

	public override void Update()
	{
		if (Time.realtimeSinceStartup - this.showTime >= 1f && !this.isStageChange)
		{
			NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
			this.isStageChange = true;
		}
		if (Scene.CurScene != Scene.Type.BATTLE)
		{
			if (CommonTasks.IsEndOfPrework && !this.isStageChangeEnd)
			{
				if (this.canReward)
				{
					NrTSingleton<MythRaidManager>.Instance.ActiveRewardEffect(this.dt_Background);
				}
				if (this.isBest && this.raidType == 2)
				{
					this.ActiveBestEffect();
					this.isBest = false;
				}
				this.lb_OK.Visible = true;
				this.bt_Test.Visible = true;
				this.lb_Loading.Visible = false;
				this.isStageChangeEnd = true;
			}
			if (this.canReward)
			{
				Animation componentInChildren = this.dt_Background.transform.GetComponentInChildren<Animation>();
				if (componentInChildren != null && !componentInChildren.isPlaying)
				{
					this.canReward = false;
					NrTSingleton<MythRaidManager>.Instance.ActiveRewardMsgBox(this.itemName, this.itemNum);
					this.itemNum.Initialize();
					this.itemName.Initialize();
				}
			}
		}
		base.Update();
	}

	public void SetRewardInfo(string[] _itemName, int[] _itemNum)
	{
		this.canReward = true;
		this.itemName = _itemName;
		this.itemNum = _itemNum;
	}

	public override void Show()
	{
		this.lb_NextRankInfo.Visible = false;
		this.isStageChange = false;
		this.isStageChangeEnd = false;
		this.canReward = false;
		this.isBest = false;
		if (this.raidType == 2)
		{
			this.SettingHard();
		}
		else
		{
			this.SettingNormalEasy();
		}
		this.ShowList();
		this.bt_Test.Visible = false;
		this.lb_OK.Visible = false;
		this.lb_Loading.Visible = true;
		base.Show();
	}

	private void ShowList()
	{
		for (int i = 0; i < 4; i++)
		{
			if (NrTSingleton<MythRaidManager>.Instance.partyPersonID[i] > 0L)
			{
				List<GS_BATTLE_RESULT_MYTHRAID_SOLDIER> list = NrTSingleton<MythRaidManager>.Instance.dic_SolInfo[NrTSingleton<MythRaidManager>.Instance.partyPersonID[i]];
				NewListItem newListItem = new NewListItem(this.nlb_HeroInfo.ColumnNum, true, string.Empty);
				newListItem.SetListItemData(0, NrTSingleton<MythRaidManager>.Instance.partyPersonName[i], null, null, null);
				this.nlb_HeroInfo.Add(newListItem);
				if (list != null && list.Count != 0)
				{
					NewListItem newListItem2 = new NewListItem(this.nlb_HeroInfo.ColumnNum, true, string.Empty);
					for (int j = 0; j < list.Count; j++)
					{
						if (j >= 0 && j % this.MAX_ONECOLUMN_SOLDIER == 0)
						{
							newListItem2 = new NewListItem(this.nlb_HeroInfo.ColumnNum, true, string.Empty);
						}
						int num = j % this.MAX_ONECOLUMN_SOLDIER + 2;
						Texture2D portraitLeaderSol = this.GetPortraitLeaderSol(list[j].CharKind);
						if (portraitLeaderSol != null)
						{
							newListItem2.SetListItemData(num, portraitLeaderSol, null, null, null, null);
						}
						else
						{
							UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(list[j].CharKind, (int)list[j].Grade);
							if (uIBaseInfoLoader == null)
							{
								uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_T_ItemEmpty");
							}
							newListItem2.SetListItemData(num, uIBaseInfoLoader, null, null, null);
							NkListSolInfo nkListSolInfo = new NkListSolInfo();
							nkListSolInfo.SolCharKind = list[j].CharKind;
							nkListSolInfo.SolGrade = (int)list[j].Grade;
							nkListSolInfo.SolInjuryStatus = false;
							nkListSolInfo.SolLevel = list[j].Level;
							nkListSolInfo.ShowLevel = true;
							if (NrTSingleton<NrCharCostumeTableManager>.Instance.IsCostumeUniqueEqualSolKind(list[j].CostumeUnique, list[j].CharKind))
							{
								nkListSolInfo.SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(list[j].CostumeUnique);
							}
							newListItem2.SetListItemData(num + this.MAX_ONECOLUMN_SOLDIER, nkListSolInfo, null, null, null);
						}
						if (j % this.MAX_ONECOLUMN_SOLDIER == this.MAX_ONECOLUMN_SOLDIER - 1)
						{
							newListItem2.SetListItemData(1, false);
							this.nlb_HeroInfo.Add(newListItem2);
							newListItem2 = null;
						}
					}
					if (newListItem2 != null)
					{
						newListItem2.SetListItemData(1, false);
						this.nlb_HeroInfo.Add(newListItem2);
					}
				}
			}
		}
	}

	private Texture2D GetPortraitLeaderSol(int iCharKind)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null && kMyCharInfo.UserPortrait)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser.GetCharKind() == iCharKind)
			{
				return kMyCharInfo.UserPortraitTexture;
			}
		}
		return null;
	}

	private void ActiveBestEffect()
	{
		NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("ui/mythicraid/fx_myth_raid_new_record_mobile", this.dt_Background, this.dt_Background.GetSize());
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "NEW_RECORD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void SettingHard()
	{
		base.SetShowLayer(2, true);
		base.SetShowLayer(3, false);
		string text = NrTSingleton<MythRaidManager>.Instance.AddComma(this.totalDamage.ToString());
		this.lb_Damage_Info.SetText(text.ToString());
		bool flag = NrTSingleton<MythRaidManager>.Instance.IsBestRecord(this.totalDamage, this.isParty);
		this.lb_MyRank.SetText(NrTSingleton<MythRaidManager>.Instance.MyBestRank(this.isParty).ToString());
		NrTSingleton<MythRaidManager>.Instance.RequestMyInfo((eMYTHRAID_DIFFICULTY)this.raidType, true);
		if (flag)
		{
			this.isBest = true;
			this.lb_MyDamage.SetText(text);
			this.lb_Damage_Refresh_info.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3208"));
		}
		else
		{
			string text2 = NrTSingleton<MythRaidManager>.Instance.AddComma(NrTSingleton<MythRaidManager>.Instance.MyBestDamage(this.isParty).ToString());
			this.lb_MyDamage.SetText(text2);
			string empty = string.Empty;
			long num = NrTSingleton<MythRaidManager>.Instance.MyBestDamage(this.isParty) - this.totalDamage;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3207"),
				"damage",
				NrTSingleton<MythRaidManager>.Instance.AddComma(num.ToString())
			});
			this.lb_Damage_Refresh_info.SetText(empty);
		}
	}

	private void SettingNormalEasy()
	{
		base.SetShowLayer(2, false);
		base.SetShowLayer(3, true);
		int num = 3212;
		if (this.rountCount != 6)
		{
			num = 3209 + UnityEngine.Random.Range(0, 2);
		}
		this.lb_RoundHelper.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(num.ToString()));
		this.lb_RoundCount.SetText(this.rountCount.ToString());
	}

	public bool IsParty()
	{
		return this.isParty;
	}

	public void SetRank(int rank, long upRankDamage)
	{
		this.lb_MyRank.SetText(rank.ToString());
		string text = string.Empty;
		if (rank == 1)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3206");
		}
		else
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3205");
			long num = upRankDamage - NrTSingleton<MythRaidManager>.Instance.MyBestDamage(this.isParty);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				textFromInterface,
				"damage",
				NrTSingleton<MythRaidManager>.Instance.AddComma(num.ToString())
			});
		}
		this.lb_NextRankInfo.SetText(text);
	}

	public void _clickClose(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MYTHRAID_RESULT_DLG);
	}

	public void SetData(byte _raidType, float _battleTime, int _roundCount, long _totalDamage, byte _isParty)
	{
		this.raidType = _raidType;
		this.rountCount = _roundCount;
		this.totalDamage = _totalDamage;
		if (_isParty == 0)
		{
			this.isParty = false;
		}
		else
		{
			this.isParty = true;
		}
		NrTSingleton<MythRaidManager>.Instance.SetBattleContinueCount(_roundCount);
	}
}
