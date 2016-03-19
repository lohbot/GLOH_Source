using GAME;
using System;
using System.Collections.Generic;

public class NrColosseum_MyGrade_UserIList
{
	private List<COLOSSEUM_MYGRADE_USERINFO> m_Colosseum_MyGrade_UserList = new List<COLOSSEUM_MYGRADE_USERINFO>();

	public NrColosseum_MyGrade_UserIList()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_Colosseum_MyGrade_UserList.Clear();
	}

	public void AddMyGrade_UserInfo(COLOSSEUM_MYGRADE_USERINFO info)
	{
		this.m_Colosseum_MyGrade_UserList.Add(info);
	}

	public void SortList()
	{
		this.m_Colosseum_MyGrade_UserList.Sort(new Comparison<COLOSSEUM_MYGRADE_USERINFO>(this.CompareGradePoint));
	}

	public int CompareGradePoint(COLOSSEUM_MYGRADE_USERINFO a, COLOSSEUM_MYGRADE_USERINFO b)
	{
		if (a.i32ColosseumGradePoint > b.i32ColosseumGradePoint)
		{
			return -1;
		}
		return 1;
	}

	public int GetMyGradeRank(int colosseum_gradePoint)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return -1;
		}
		int num = 1;
		foreach (COLOSSEUM_MYGRADE_USERINFO current in this.m_Colosseum_MyGrade_UserList)
		{
			if (charPersonInfo.GetPersonID() == current.i64PersonID)
			{
				return num;
			}
			num++;
		}
		return num;
	}

	public void UpdateMyInfo(int gradepoint)
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		foreach (COLOSSEUM_MYGRADE_USERINFO current in this.m_Colosseum_MyGrade_UserList)
		{
			if (charPersonInfo.GetPersonID() == current.i64PersonID)
			{
				current.i32ColosseumGradePoint = gradepoint;
			}
		}
	}

	public List<COLOSSEUM_MYGRADE_USERINFO> GetList()
	{
		return this.m_Colosseum_MyGrade_UserList;
	}
}
