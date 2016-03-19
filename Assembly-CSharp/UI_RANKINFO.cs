using PROTOCOL;
using System;

public class UI_RANKINFO
{
	public int i32Rank;

	public string Charname = string.Empty;

	public short iCharLevel;

	public long i64MatchPoint;

	public UI_RANKINFO(string _char_name, byte _char_level, long _patch_point)
	{
		this.i32Rank = 0;
		this.Charname = _char_name;
		this.iCharLevel = (short)_char_level;
		this.i64MatchPoint = _patch_point;
	}

	public UI_RANKINFO(PLUNDER_RANKINFO info)
	{
		this.i32Rank = info.i32Rank;
		this.Charname = TKString.NEWString(info.szCharName);
		this.iCharLevel = info.iCharLevel;
		this.i64MatchPoint = info.i64MatchPoint;
	}

	public UI_RANKINFO(PLUNDER_FRIEND_RANKINFO info)
	{
		this.i32Rank = 0;
		this.Charname = TKString.NEWString(info.szCharName);
		this.iCharLevel = info.iCharLevel;
		this.i64MatchPoint = info.i64MatchPoint;
	}
}
