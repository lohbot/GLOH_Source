using GAME;
using System;

public class NrNpcPos
{
	public string strKey = string.Empty;

	public int nCharKind;

	public int nMapIndex;

	public POS3D kPos = new POS3D();

	public QUEST_CONST.eQUESTSTATE eQuestState = QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_NOT_VIEW;

	public string strName;
}
