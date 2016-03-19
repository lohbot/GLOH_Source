using System;

public class EVENT_HERO
{
	public string m_strCharCode = string.Empty;

	public byte m_i8Rank;

	public int m_i32Atk;

	public int m_i32Hp;

	public int m_i32StartYear;

	public int m_i32StartMon;

	public int m_i32StartDay;

	public int m_i32EndYear;

	public int m_i32EndMon;

	public int m_i32EndDay;

	public EVENT_HERO()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_strCharCode = string.Empty;
		this.m_i8Rank = 0;
		this.m_i32Atk = 0;
		this.m_i32Hp = 0;
		this.m_i32StartYear = 0;
		this.m_i32StartMon = 0;
		this.m_i32StartDay = 0;
		this.m_i32EndYear = 0;
		this.m_i32EndMon = 0;
		this.m_i32EndDay = 0;
	}
}
