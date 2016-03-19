using GAME;
using System;

public class ACQUIRED_ITEM
{
	public int ItemUnique;

	public int ItemNum;

	public int ItemNumExtra;

	public short CreateType;

	public short CharUnique;

	public int nItemPos;

	public int nItemRank;

	public void Set(ITEM info, short extranum)
	{
		this.ItemUnique = info.m_nItemUnique;
		this.ItemNum = (int)((short)info.m_nItemNum);
		this.ItemNumExtra = (int)extranum;
	}

	public void Set(int itemunique, int num, int extranum, short createtype, int itemRank)
	{
		this.ItemUnique = itemunique;
		this.ItemNum = num;
		this.ItemNumExtra = extranum;
		this.CreateType = createtype;
		this.nItemRank = itemRank;
	}
}
