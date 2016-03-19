using System;
using System.Collections.Generic;

public class Friend_InvitePersonManager : NrTSingleton<Friend_InvitePersonManager>
{
	public List<Friend_InvitePersonCnt> InfoList = new List<Friend_InvitePersonCnt>();

	private static Friend_InvitePersonManager s_cInstance;

	private Friend_InvitePersonManager()
	{
	}

	public static Friend_InvitePersonManager Get_Instance()
	{
		if (Friend_InvitePersonManager.s_cInstance == null)
		{
			Friend_InvitePersonManager.s_cInstance = new Friend_InvitePersonManager();
		}
		return Friend_InvitePersonManager.s_cInstance;
	}

	public void Add(Friend_InvitePersonCnt data)
	{
		this.InfoList.Add(data);
	}

	public bool IsAble_InvitePerson(int _user_level, int _friend_count)
	{
		foreach (Friend_InvitePersonCnt current in this.InfoList)
		{
			if (_user_level <= current.CHECK_LEVEL && _friend_count >= current.CANINVITE_PERSONCOUNT)
			{
				return false;
			}
		}
		return true;
	}
}
