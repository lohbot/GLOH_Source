using GAME;
using System;

public class BountyHuntNPCInfo
{
	private int m_iMapIndex;

	private int m_iCharID;

	private short m_iBountyHuntUnique;

	private POS3D m_vPos = new POS3D();

	private int m_iRandIndex;

	public int MapIndex
	{
		get
		{
			return this.m_iMapIndex;
		}
		set
		{
			this.m_iMapIndex = value;
		}
	}

	public int CharID
	{
		get
		{
			return this.m_iCharID;
		}
		set
		{
			this.m_iCharID = value;
		}
	}

	public short BountyHuntUnique
	{
		get
		{
			return this.m_iBountyHuntUnique;
		}
		set
		{
			this.m_iBountyHuntUnique = value;
		}
	}

	public POS3D Pos
	{
		get
		{
			return this.m_vPos;
		}
		set
		{
			this.m_vPos = value;
		}
	}

	public int RandIndex
	{
		get
		{
			return this.m_iRandIndex;
		}
		set
		{
			this.m_iRandIndex = value;
		}
	}
}
