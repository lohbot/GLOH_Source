using System;

[Serializable]
public class MapInfo
{
	public int m_MapIdx;

	public string m_Name = string.Empty;

	public override bool Equals(object oj)
	{
		MapInfo mapInfo = oj as MapInfo;
		return mapInfo != null && this.m_MapIdx == mapInfo.m_MapIdx;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
