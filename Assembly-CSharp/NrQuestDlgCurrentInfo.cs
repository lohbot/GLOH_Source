using System;

public class NrQuestDlgCurrentInfo
{
	public E_EVENT_TYPE bCheck;

	public string strDlgIndex = string.Empty;

	public int iDlg32Count;

	public E_NPC_TALK_STEP step;

	public NPC_TALK_QUEST_STATE state = new NPC_TALK_QUEST_STATE();

	public CNpcUI kNpcUI;

	public NrCharKindInfo kCurNpc;

	public short i16CharUnique;

	public int i32MenuCount;

	public string strCharCode = string.Empty;

	public long nPersonID;

	public long i64uid;

	public int i32SessionKey;
}
