using GAME;
using System;
using UnityEngine;
using UnityForms;

public class BountyHuntingDlg : Form
{
	private const short BOUNTYHUNT_MAX = 4;

	private short MAX_PAGE;

	private DrawTexture m_dtBG;

	private Label m_lbTitle;

	private Button[] m_btEpisode = new Button[4];

	private DrawTexture[] m_dtDisableBG = new DrawTexture[4];

	private DrawTexture[] m_dtDisableMark = new DrawTexture[4];

	private DrawTexture[] m_dtDisableRank = new DrawTexture[4];

	private DrawTexture[] m_dtDisableRankBG = new DrawTexture[4];

	private Button m_btPrev;

	private Button m_btNext;

	private Label m_lbBaloonText;

	private DrawTexture m_dtNPCFace;

	private string m_strText = string.Empty;

	private short m_iPage = 1;

	private string m_strBGTextureKey = "Win_T_BK";

	private string m_strNoneTextureKey = "NPC_I_QuestI12";

	private GameObject m_gbCurEffect;

	private int m_iCurIndex = -1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Bounty/DLG_Bounty", G_ID.BOUNTYHUNTING_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtBG = (base.GetControl("DrawTexture_BGIMG01") as DrawTexture);
		this.m_lbTitle = (base.GetControl("Label_PageTitleLabel01") as Label);
		for (int i = 0; i < 4; i++)
		{
			this.m_strText = string.Format("{0}{1}", "Button_EpisodeBtn0", i + 1);
			this.m_btEpisode[i] = (base.GetControl(this.m_strText) as Button);
			this.m_btEpisode[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickEpisode));
			this.m_btEpisode[i].TabIndex = i + 1;
			this.m_strText = string.Format("{0}{1}", "DrawTexture_DisableBG0", i + 1);
			this.m_dtDisableBG[i] = (base.GetControl(this.m_strText) as DrawTexture);
			this.m_strText = string.Format("{0}{1}", "DrawTexture_DisableMark0", i + 1);
			this.m_dtDisableMark[i] = (base.GetControl(this.m_strText) as DrawTexture);
			this.m_strText = string.Format("{0}{1}", "DrawTexture_Rank", i + 1);
			this.m_dtDisableRank[i] = (base.GetControl(this.m_strText) as DrawTexture);
			this.m_strText = string.Format("{0}{1}", "DrawTexture_rankbg", i + 1);
			this.m_dtDisableRankBG[i] = (base.GetControl(this.m_strText) as DrawTexture);
		}
		this.m_btPrev = (base.GetControl("Button_PrePageBtn01") as Button);
		this.m_btPrev.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrev));
		this.m_btNext = (base.GetControl("Button_NextPageBtn01") as Button);
		this.m_btNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNext));
		this.m_btPrev.EffectAni = false;
		this.m_btNext.EffectAni = false;
		this.m_lbBaloonText = (base.GetControl("BaloonText") as Label);
		this.m_dtNPCFace = (base.GetControl("NPC_Face") as DrawTexture);
		this.m_dtNPCFace.SetTextureFromUISoldierBundle(eCharImageType.LARGE, "mine");
		this.MAX_PAGE = NrTSingleton<BountyHuntManager>.Instance.MaxPage + 1;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void SetData()
	{
		short num = (short)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BOUNTY_REQUIRE);
		byte b = (byte)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_BOUNTY_REQUIRE_RANK);
		byte babelSubFloorRankInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetBabelSubFloorRankInfo(num, 4, 1);
		if (b <= babelSubFloorRankInfo)
		{
			base.ShowLayer(2);
			short num2 = NrTSingleton<BountyHuntManager>.Instance.GetPage(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique);
			if (num2 < 1)
			{
				num2 = 1;
			}
			this.ShowEpisod(num2);
		}
		else
		{
			base.ShowLayer(1);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2323"),
				"floor",
				num,
				"rank",
				BATTLE_DEFINE.RANK_STRING[(int)b]
			});
			this.m_lbBaloonText.SetText(empty);
		}
	}

	private void ShowEpisod(short iPage)
	{
		this.m_iPage = iPage;
		short bountyHuntUnique = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique;
		BountyInfoData bountyInfoData = NrTSingleton<BountyHuntManager>.Instance.GetBountyInfoDataFromUnique(bountyHuntUnique);
		if (bountyInfoData != null && !NrTSingleton<BountyHuntManager>.Instance.GetBountyInfoDataTime(bountyHuntUnique))
		{
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique = 0;
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ClearBountyHuntClearInfo();
		}
		for (int i = 0; i < 4; i++)
		{
			this.m_dtDisableBG[i].SetTexture(this.m_strBGTextureKey);
			this.m_dtDisableMark[i].SetTexture(this.m_strNoneTextureKey);
			this.m_dtDisableMark[i].Visible = true;
			this.m_dtDisableRank[i].Visible = false;
			this.m_dtDisableRankBG[i].Visible = false;
			bountyInfoData = NrTSingleton<BountyHuntManager>.Instance.GetBountyInfoData(iPage, (short)(i + 1));
			if (bountyInfoData != null)
			{
				if (NrTSingleton<BountyHuntManager>.Instance.GetBountyEcoData(bountyInfoData.i16EcoIndex) != null)
				{
					if (i == 0)
					{
						this.m_lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(bountyInfoData.i32WeekTitleKey.ToString()));
						this.m_strText = string.Format("UI/adventure/{0}", bountyInfoData.strWeekBG);
						this.m_dtBG.SetTextureFromBundle(this.m_strText);
					}
					switch (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetBountyHuntClearState(bountyInfoData.i16Unique))
					{
					case eBOUNTYHUNTCLEAR_STATE.eBOUNTYHUNTCLEAR_STATE_NONE:
						this.m_dtDisableBG[i].SetTexture(this.m_strBGTextureKey);
						this.m_dtDisableMark[i].SetTexture(this.m_strNoneTextureKey);
						break;
					case eBOUNTYHUNTCLEAR_STATE.eBOUNTYHUNTCLEAR_STATE_ACCEPT:
						this.m_dtDisableBG[i].SetTexture(eCharImageType.SMALL, bountyInfoData.i32NPCCharKind, 0, string.Empty);
						this.m_dtDisableMark[i].SetTexture(bountyInfoData.strMonBG);
						break;
					case eBOUNTYHUNTCLEAR_STATE.eBOUNTYHUNTCLEAR_STATE_CLEAR:
					{
						byte bountyHuntClearRank = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetBountyHuntClearRank(bountyInfoData.i16Unique);
						this.m_dtDisableBG[i].SetTexture(eCharImageType.SMALL, bountyInfoData.i32NPCCharKind, 0, string.Empty);
						this.m_dtDisableMark[i].Visible = false;
						UIBaseInfoLoader texture = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(NrTSingleton<BountyHuntManager>.Instance.GetBountyRankImgText(bountyHuntClearRank));
						this.m_dtDisableRank[i].SetTexture(texture);
						this.m_dtDisableRank[i].Visible = true;
						this.m_dtDisableRankBG[i].Visible = true;
						break;
					}
					}
					if (bountyInfoData.i16Unique == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique)
					{
						NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_EMERGENCY", this.m_btEpisode[i], this.m_btEpisode[i].GetSize());
						this.m_btEpisode[i].AddGameObjectDelegate(new EZGameObjectDelegate(this.ButtonAddEffectDelegate));
						this.m_iCurIndex = i;
					}
				}
			}
		}
		this.CheckPageButton((int)iPage);
	}

	public void CheckPageButton(int iPage)
	{
		if (!NrTSingleton<BountyHuntManager>.Instance.IsNextPage())
		{
			this.m_btPrev.Visible = false;
			this.m_btNext.Visible = false;
			return;
		}
		if (iPage == 1)
		{
			this.m_btPrev.Visible = false;
		}
		else
		{
			this.m_btPrev.Visible = true;
		}
		if ((int)(this.MAX_PAGE - 1) == iPage)
		{
			this.m_btNext.Visible = false;
		}
		else
		{
			this.m_btNext.Visible = true;
		}
	}

	public void ClickEpisode(IUIObject obj)
	{
		Button button = obj as Button;
		if (null == button)
		{
			return;
		}
		BountyCheckDlg bountyCheckDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BOUNTYCHECK_DLG) as BountyCheckDlg;
		if (bountyCheckDlg != null)
		{
			BountyInfoData bountyInfoData = NrTSingleton<BountyHuntManager>.Instance.GetBountyInfoData(this.m_iPage, (short)button.TabIndex);
			if (bountyInfoData != null)
			{
				bountyCheckDlg.SetEpisode(bountyInfoData);
			}
		}
		this.SetCurEffect(false);
	}

	public void ClickPrev(IUIObject obj)
	{
		if (1 <= this.m_iPage - 1)
		{
			this.m_iPage -= 1;
		}
		else
		{
			this.m_iPage = this.MAX_PAGE - 1;
		}
		this.ShowEpisod(this.m_iPage);
	}

	public void ClickNext(IUIObject obj)
	{
		if (this.m_iPage + 1 < this.MAX_PAGE)
		{
			this.m_iPage += 1;
		}
		else
		{
			this.m_iPage = 1;
		}
		this.ShowEpisod(this.m_iPage);
	}

	public void RefreshInfo()
	{
		this.ShowEpisod(this.m_iPage);
	}

	public void ButtonAddEffectDelegate(IUIObject control, GameObject obj)
	{
		if (null != this.m_gbCurEffect)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbCurEffect);
		}
		if (obj == null || control == null)
		{
			return;
		}
		this.m_gbCurEffect = obj;
		if (this.m_iCurIndex == 3)
		{
			this.m_gbCurEffect.transform.localScale = new Vector3(2.1f, 2.1f, 1f);
		}
		else
		{
			this.m_gbCurEffect.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		if (null != this.m_gbCurEffect)
		{
			UnityEngine.Object.DestroyImmediate(this.m_gbCurEffect);
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHALLENGE_DLG);
	}

	public void SetCurEffect(bool bActive)
	{
		if (null == this.m_gbCurEffect)
		{
			return;
		}
		if (this.m_iCurIndex == 1 || this.m_iCurIndex == 2)
		{
			this.m_gbCurEffect.SetActive(bActive);
		}
	}
}
