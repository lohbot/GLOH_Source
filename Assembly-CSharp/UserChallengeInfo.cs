using PROTOCOL.GAME;
using System;
using System.Collections.Generic;

public class UserChallengeInfo
{
	private Dictionary<short, Challenge_Info> m_kChallengeInfo = new Dictionary<short, Challenge_Info>();

	public void Init()
	{
		this.m_kChallengeInfo.Clear();
	}

	public void SetUserChallengeInfo(Challenge_Info info)
	{
		if (!this.m_kChallengeInfo.ContainsKey(info.m_nUnique))
		{
			this.m_kChallengeInfo.Add(info.m_nUnique, info);
		}
		else
		{
			this.m_kChallengeInfo[info.m_nUnique] = info;
		}
	}

	public Challenge_Info GetUserChallengeInfo(short unique)
	{
		if (this.m_kChallengeInfo.ContainsKey(unique))
		{
			return this.m_kChallengeInfo[unique];
		}
		return null;
	}
}
