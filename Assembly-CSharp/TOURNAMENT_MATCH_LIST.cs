using System;

public class TOURNAMENT_MATCH_LIST
{
	public int nIndex;

	public string[] m_szPlayer = new string[2];

	public string m_szObserver = string.Empty;

	public int m_nStartTurnAlly;

	public eTOURNAMENT_PLAYER_STATE[] ePlayerState = new eTOURNAMENT_PLAYER_STATE[2];

	public eTOURNAMENT_PLAYER_STATE eMatchState;

	public int[] m_nWinCount = new int[2];

	public bool bUseLoddy;
}
