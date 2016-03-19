using System;

public class CHARKIND_SOLINCSTATINFO
{
	public short INC_STR;

	public short INC_DEX;

	public short INC_INT;

	public short INC_VIT;

	public CHARKIND_SOLINCSTATINFO()
	{
		this.Init();
	}

	public void Init()
	{
		this.INC_STR = 0;
		this.INC_DEX = 0;
		this.INC_INT = 0;
		this.INC_VIT = 0;
	}

	public void Set(ref CHARKIND_SOLINCSTATINFO pkSOLINCSTATINFO)
	{
		this.INC_STR = pkSOLINCSTATINFO.INC_STR;
		this.INC_DEX = pkSOLINCSTATINFO.INC_DEX;
		this.INC_INT = pkSOLINCSTATINFO.INC_INT;
		this.INC_VIT = pkSOLINCSTATINFO.INC_VIT;
	}
}
