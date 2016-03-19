using GAME;
using System;

public class AuctionSearchOption
{
	public AuctionDefine.eAUCTIONREGISTERTYPE m_eAuctionRegisterType = AuctionDefine.eAUCTIONREGISTERTYPE.eAUCTIONREGISTERTYPE_ALL;

	public eITEM_TYPE m_eItemType;

	public short m_iUseMinLevel;

	public short m_iUseMaxLevel;

	public int m_iItemSkillUnique;

	public short m_iItemSkillLevel;

	public short m_iItemTradeCount;

	public byte m_bySolSeason;

	public short m_iSolLevel = 1;

	public string m_strSolName = string.Empty;

	public short m_iSolTradeCount;

	public long m_lCostMoney;

	public long m_lDirectCostMoney;

	public int m_iCostHearts;

	public int m_iCostDirectHearts;

	public AuctionDefine.ePAYTYPE m_ePayType = AuctionDefine.ePAYTYPE.ePAYTYPE_ALL;

	public void Set(AuctionSearchOption SearchOption)
	{
		this.m_eAuctionRegisterType = SearchOption.m_eAuctionRegisterType;
		this.m_eItemType = SearchOption.m_eItemType;
		this.m_iUseMinLevel = SearchOption.m_iUseMinLevel;
		this.m_iUseMaxLevel = SearchOption.m_iUseMaxLevel;
		this.m_iItemSkillUnique = SearchOption.m_iItemSkillUnique;
		this.m_iItemSkillLevel = SearchOption.m_iItemSkillLevel;
		this.m_iItemTradeCount = SearchOption.m_iItemTradeCount;
		this.m_bySolSeason = SearchOption.m_bySolSeason;
		this.m_iSolLevel = SearchOption.m_iSolLevel;
		this.m_strSolName = SearchOption.m_strSolName;
		this.m_iSolTradeCount = SearchOption.m_iSolTradeCount;
		this.m_lCostMoney = SearchOption.m_lCostMoney;
		this.m_lDirectCostMoney = SearchOption.m_lDirectCostMoney;
		this.m_iCostHearts = SearchOption.m_iCostHearts;
		this.m_iCostDirectHearts = SearchOption.m_iCostDirectHearts;
		this.m_ePayType = SearchOption.m_ePayType;
	}
}
