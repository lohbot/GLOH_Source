using System;
using System.Collections.Generic;

public class LevelupInfo
{
	private List<string> infoList = new List<string>();

	private int level;

	public LevelupInfo(int _level)
	{
		this.level = _level;
	}

	public int GetLevel()
	{
		return this.level;
	}

	public void AddTexExplain(string texture, string explain)
	{
		string item = string.Format("{0}|{1}", texture, explain);
		this.infoList.Add(item);
	}

	public int GetListCount()
	{
		return this.infoList.Count;
	}

	public void GetTexExplain(int index, out string texturePath, out string explain)
	{
		string[] array = this.infoList[index].Split(new char[]
		{
			'|'
		});
		texturePath = array[0];
		explain = array[1];
	}
}
