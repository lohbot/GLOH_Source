using PROTOCOL;
using System;
using System.Collections.Generic;

public class NrTimeShopInfo
{
	private List<TIMESHOP_ITEMINFO> m_lstMyTimeShopItemList = new List<TIMESHOP_ITEMINFO>();

	private short m_i16RefreshCount;

	private long m_i64RefreshTime;

	public short RefreshCount
	{
		get
		{
			return this.m_i16RefreshCount;
		}
	}

	public long RefreshTime
	{
		get
		{
			return this.m_i64RefreshTime;
		}
	}

	public void Init()
	{
		this.m_lstMyTimeShopItemList.Clear();
		this.m_i16RefreshCount = 0;
		this.m_i64RefreshTime = 0L;
	}

	public void Add_UserTimeShopItemList(TIMESHOP_ITEMINFO _pItemInfo)
	{
		if (_pItemInfo == null)
		{
			return;
		}
		if (!this.m_lstMyTimeShopItemList.Contains(_pItemInfo))
		{
			this.m_lstMyTimeShopItemList.Add(_pItemInfo);
		}
	}

	public void Clear_UserTimeShopItemList()
	{
		this.m_lstMyTimeShopItemList.Clear();
	}

	public List<TIMESHOP_ITEMINFO> Get_UserTimeShopItemList()
	{
		if (this.m_lstMyTimeShopItemList.Count <= 0)
		{
			return null;
		}
		return this.m_lstMyTimeShopItemList;
	}

	public int Get_UserTimeShopItemListCount()
	{
		return this.m_lstMyTimeShopItemList.Count;
	}

	public void Set_UserTimeShopInfo(short _i16RefreshCount, long _i64RefreshTime)
	{
		this.m_i16RefreshCount = _i16RefreshCount;
		this.m_i64RefreshTime = _i64RefreshTime;
	}

	public void Set_UserTimeShopItemBuy(long _i64IDX, byte _i8Buy)
	{
		if (_i64IDX < 0L)
		{
			return;
		}
		for (int i = 0; i < this.m_lstMyTimeShopItemList.Count; i++)
		{
			if (this.m_lstMyTimeShopItemList[i].i64IDX == _i64IDX)
			{
				this.m_lstMyTimeShopItemList[i].i8IsBuy = _i8Buy;
				break;
			}
		}
	}

	public bool Get_UserTimeShopItmeIsBuy(long _i64IDX)
	{
		if (_i64IDX < 0L)
		{
			return false;
		}
		for (int i = 0; i < this.m_lstMyTimeShopItemList.Count; i++)
		{
			if (this.m_lstMyTimeShopItemList[i].i64IDX == _i64IDX && this.m_lstMyTimeShopItemList[i].i8IsBuy == 1)
			{
				return true;
			}
		}
		return false;
	}

	public int GetIndex_byTimeShopIDX(long _i64IDX)
	{
		if (_i64IDX < 0L)
		{
			return -1;
		}
		for (int i = 0; i < this.m_lstMyTimeShopItemList.Count; i++)
		{
			if (this.m_lstMyTimeShopItemList[i].i64IDX == _i64IDX)
			{
				return i;
			}
		}
		return -1;
	}
}
