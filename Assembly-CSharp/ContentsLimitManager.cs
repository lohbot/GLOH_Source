using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityForms;

public class ContentsLimitManager : NrTSingleton<ContentsLimitManager>
{
	private Dictionary<eCONTENTSLIMIT, ContentsLimitData> m_LimitDataDIC = new Dictionary<eCONTENTSLIMIT, ContentsLimitData>();

	private ContentsLimitManager()
	{
	}

	public void Clear()
	{
		foreach (ContentsLimitData current in this.m_LimitDataDIC.Values)
		{
			current.Clear();
		}
		this.m_LimitDataDIC.Clear();
	}

	public void AddLimitData(CONTENTSLIMIT_DATA Data)
	{
		eCONTENTSLIMIT i32ContentsLimitType = (eCONTENTSLIMIT)Data.i32ContentsLimitType;
		if (this.m_LimitDataDIC.ContainsKey(i32ContentsLimitType))
		{
			this.m_LimitDataDIC[i32ContentsLimitType].AddLimitData(Data);
		}
		else
		{
			ContentsLimitData contentsLimitData = new ContentsLimitData();
			contentsLimitData.AddLimitData(Data);
			this.m_LimitDataDIC.Add(i32ContentsLimitType, contentsLimitData);
		}
	}

	public bool IsQuestAccept(int iQuestGroupUnique)
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_QUEST) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_QUEST].IsQuestAccept(iQuestGroupUnique);
	}

	public bool IsWarpMap(int MapIndex)
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_MAPWARP) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_MAPWARP].IsWarpMap(MapIndex);
	}

	public int GetBabelTowerLastFloor(short nFloorType = 1)
	{
		if (this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_BABEL))
		{
			return this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_BABEL].GetBabelTowerLastFloor(nFloorType);
		}
		return 0;
	}

	public bool IsShopTab(int iIndex)
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_SHOP_TAP) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_SHOP_TAP].IsShopTab(iIndex);
	}

	public bool IsShopProduct(long lItemIDX)
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_SHOP_PRODUCT) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_SHOP_PRODUCT].IsShopProduct(lItemIDX);
	}

	public bool IsWorldMapMove(int iMoveIndex)
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_WORLDMAP) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_WORLDMAP].IsWorldMapMove(iMoveIndex);
	}

	public bool IsMineApply(short iLevel)
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_MINE) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_MINE].IsMineApply(iLevel);
	}

	public bool IsValidMineGrade(byte bMineGrade)
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_MINE) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_MINE].IsValidMineGrade(bMineGrade);
	}

	public void SetReload()
	{
		BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
		if (bookmarkDlg != null)
		{
			bookmarkDlg.SetBookmarkInfo();
		}
		MainMenuDlg mainMenuDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAINMENU_DLG) as MainMenuDlg;
		if (mainMenuDlg != null)
		{
			mainMenuDlg.ShowHideButton();
		}
	}

	public bool isEmulator()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_BLUESTACKS_BLOCK) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_BLUESTACKS_BLOCK].IsBlueStacksUser();
	}

	public bool isEmulatorTest()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_EMULATOR_TEST_BLOCK) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_EMULATOR_TEST_BLOCK].IsBlueStacksUser();
	}

	public bool IsExploration()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_EXPLORATION) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_EXPLORATION].IsExploration();
	}

	public bool IsSupporter()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_SUPPORTER) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_SUPPORTER].IsSupporter();
	}

	public bool IsReincarnation()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_REINCARNATION) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_REINCARNATION].IsReincarnation();
	}

	public bool IsSolGuideCharKindInfo(int i32CharKindInfo)
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_SOLGUIDE) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_SOLGUIDE].IsSolCharKindinfo(i32CharKindInfo);
	}

	public bool IsGuildBoss()
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_GUILDBOSS) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_GUILDBOSS].IsGuildBoss();
	}

	public short GetGuildBossLastFloor()
	{
		if (this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_GUILDBOSS))
		{
			return this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_GUILDBOSS].GetGuildBossLastFloor();
		}
		return 0;
	}

	public bool IsAuctionUse()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_AUCTION) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_AUCTION].IsAuctionUse();
	}

	public short GetAuctionUseLevel()
	{
		if (this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_AUCTION))
		{
			return this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_AUCTION].GetAuctionUseLevel();
		}
		return 0;
	}

	public int GetDLGSolRecruit()
	{
		if (this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_DLG_SOLRECRUIT))
		{
			return this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_DLG_SOLRECRUIT].GetDLGSolRecruit();
		}
		return 0;
	}

	public bool IsTreasure()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_TREASURE) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_TREASURE].IsTreasure();
	}

	public bool IsInfiBattle()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_INFIBATTLE) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_INFIBATTLE].IsInfiBattle();
	}

	public bool IsCouponUse()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_COUPON) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_COUPON].IsCouponUse();
	}

	public bool IsHP_Auth()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_AUCTION) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_AUCTION].IsHP_Auth();
	}

	public bool IsPointExchage()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_POINT_EXCHANGE) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_POINT_EXCHANGE].IsPointExchage();
	}

	public bool IsTicketSell(int iCharKind)
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_TICKET_SELL) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_TICKET_SELL].IsTicketSell(iCharKind);
	}

	public bool IsXpsPromotion()
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_XPS_PROMOTION) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_XPS_PROMOTION].IsXpsPromotion();
	}

	public bool IsAwakeningUse()
	{
		return !this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_AWAKENING) || this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_AWAKENING].IsAwakeningUse();
	}

	public bool IsElementKind(int i32CharKind)
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_ELEMENTKIND) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_ELEMENTKIND].IsElementKind(i32CharKind);
	}

	public bool IsNewColosseumSupport()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_NEWCOLOSSEUM_SUPPORT) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_NEWCOLOSSEUM_SUPPORT].IsNewColosseumSupport();
	}

	public bool IsBountyHunt()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_BOUNTYHUNT) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_BOUNTYHUNT].IsBountyHunt();
	}

	public bool IsAlchemy()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_ALCHEMY) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_ALCHEMY].IsAlchemy();
	}

	public bool IsSolGuide_Season(int i32Season)
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_SOLGUIDE_SEASON) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_SOLGUIDE_SEASON].IsSolGuide_Season(i32Season);
	}

	public bool IsTradeCaralyst()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_TRADE_CATALYST) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_TRADE_CATALYST].IsTradeCaralyst();
	}

	public bool IsUseCaralyst()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_USE_CATALYST) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_USE_CATALYST].IsUseCaralyst();
	}

	public short GetLimitLevel(long i64atbtype)
	{
		if (this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_LEVELLIMIT))
		{
			return this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_LEVELLIMIT].GetLimitLevel(i64atbtype);
		}
		return 0;
	}

	public bool IsNPCLimit(int i32CharKind)
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_NPCLIMIT) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_NPCLIMIT].IsNPCLimit(i32CharKind);
	}

	public bool IsFacebookLimit()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_FACEBOOKLIMIT) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_FACEBOOKLIMIT].IsFaceBookLimit();
	}

	public bool IsExpeditionLimit()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_EXPEDITION) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_EXPEDITION].IsExpeditionLimit();
	}

	public bool IsExpeditionLevel(int iLevel)
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_EXPEDITION) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_EXPEDITION].IsExpeditionLevel(iLevel);
	}

	public int ExpeditionGradeLimit()
	{
		if (this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_EXPEDITION))
		{
			return this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_EXPEDITION].ExpeditionGradeLimit();
		}
		return 5;
	}

	public bool IsNewGuildWarLimit()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_NEWGUILD) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_NEWGUILD].IsNewGuildWarLimit();
	}

	public bool IsAgitLimit()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_NEWGUILD) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_NEWGUILD].IsAgitLimit();
	}

	public bool IsExchangeJewelry()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_EXCHANGE_JEWELRY) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_EXCHANGE_JEWELRY].IsExchangeJewelry();
	}

	public int GetLimitAdventure()
	{
		if (this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.ECONTENTSLIMIT_ADVENTURE))
		{
			return this.m_LimitDataDIC[eCONTENTSLIMIT.ECONTENTSLIMIT_ADVENTURE].GetLimitAdventure();
		}
		return 0;
	}

	public bool IsHeroBattle()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.ECONTENTSLIMIT_HEROBATTLE) && this.m_LimitDataDIC[eCONTENTSLIMIT.ECONTENTSLIMIT_HEROBATTLE].IsHeroBattle();
	}

	public bool IsVoucherLimit()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_VOUCHER) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_VOUCHER].IsVoucherLimit();
	}

	public bool IsVipExp()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_VIP) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_VIP].IsVipExp();
	}

	public bool IsLegend()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_VIP) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_VIP].IsLegend();
	}

	public short GetLimitSolGrade()
	{
		if (this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_EVOLUTION_GRADE_LIMIT))
		{
			return this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_EVOLUTION_GRADE_LIMIT].GetLimitSolGrade();
		}
		return 0;
	}

	public bool IsShowFriendInviteButton()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_CHANNEL_INVITE) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_CHANNEL_INVITE].IsShowFriendInviteButton();
	}

	public bool IsTutorialBattleStart()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_TUTORIAL_BATTLE) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_TUTORIAL_BATTLE].IsTutorialBattleStart();
	}

	public bool IsExchangeMythicSol()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_EXCHANGEMYTHICSOL) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_EXCHANGEMYTHICSOL].IsExchangeMythicSol();
	}

	public bool IsTranscendence()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_TRANSCENDENCS) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_TRANSCENDENCS].IsTranscendence();
	}

	public bool IsLegendHire()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_LEGENDHIRE) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_LEGENDHIRE].IsLegendHire();
	}

	public bool IsExtract()
	{
		return this.m_LimitDataDIC.ContainsKey(eCONTENTSLIMIT.eCONTENTSLIMIT_EXTRACT) && this.m_LimitDataDIC[eCONTENTSLIMIT.eCONTENTSLIMIT_EXTRACT].IsExtract();
	}
}
