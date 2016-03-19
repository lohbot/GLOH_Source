using PROTOCOL;
using System;
using System.Collections.Generic;

public class clMineInfo
{
	public long m_i64MineID;

	public byte m_nMineGrade;

	private Dictionary<int, MINE_MILITARY_USER_SOLINFO> m_dicOccupy_User_SolList = new Dictionary<int, MINE_MILITARY_USER_SOLINFO>();

	public Dictionary<int, MINE_MILITARY_USER_SOLINFO> GetUser_SolList()
	{
		return this.m_dicOccupy_User_SolList;
	}

	public void SetMineMilitarySolList(MINE_MILITARY_USER_SOLINFO[] occupy_info)
	{
		this.m_dicOccupy_User_SolList.Clear();
		for (int i = 0; i < occupy_info.Length; i++)
		{
			this.m_dicOccupy_User_SolList.Add((int)occupy_info[i].ui8BatchIndex, occupy_info[i]);
		}
	}
}
