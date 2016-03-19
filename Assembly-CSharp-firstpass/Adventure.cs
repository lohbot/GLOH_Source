using System;
using System.Collections.Generic;

public class Adventure
{
	public class AdventureInfo
	{
		public string monsterCode = string.Empty;

		public int questGroupUnique;

		public int mapIndex;

		public int destX;

		public int destY;

		public AdventureInfo()
		{
			this.monsterCode = string.Empty;
			this.questGroupUnique = 0;
			this.mapIndex = 0;
			this.destX = 0;
			this.destY = 0;
		}
	}

	public int m_nAdventureUnique;

	public string m_szAdventureName = string.Empty;

	public string m_szBackImage = string.Empty;

	public List<Adventure.AdventureInfo> m_kAdventureInfo = new List<Adventure.AdventureInfo>();

	public Adventure()
	{
		this.m_nAdventureUnique = 0;
		this.m_szAdventureName = string.Empty;
		this.m_szBackImage = string.Empty;
		this.m_kAdventureInfo.Clear();
	}

	public int GetAdventureUnique()
	{
		return this.m_nAdventureUnique;
	}

	public string GetAdventureName()
	{
		return this.m_szAdventureName;
	}

	public string GetBackImage()
	{
		return this.m_szBackImage;
	}

	public Adventure.AdventureInfo GetAdventureInfo(int index)
	{
		if (index >= this.m_kAdventureInfo.Count)
		{
			return null;
		}
		return this.m_kAdventureInfo[index];
	}

	public int GetQuestGroupUnique(int index)
	{
		if (index >= this.m_kAdventureInfo.Count)
		{
			return 0;
		}
		Adventure.AdventureInfo adventureInfo = this.m_kAdventureInfo[index];
		if (adventureInfo == null)
		{
			return 0;
		}
		return adventureInfo.questGroupUnique;
	}

	public string GetMonsterKind(int index)
	{
		if (index >= this.m_kAdventureInfo.Count)
		{
			return string.Empty;
		}
		Adventure.AdventureInfo adventureInfo = this.m_kAdventureInfo[index];
		if (adventureInfo == null)
		{
			return string.Empty;
		}
		return adventureInfo.monsterCode;
	}

	public int GetAdventureInfoCount()
	{
		return this.m_kAdventureInfo.Count;
	}
}
