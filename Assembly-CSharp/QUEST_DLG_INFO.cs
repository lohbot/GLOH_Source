using System;
using UnityForms;

public class QUEST_DLG_INFO
{
	public enum Alignment
	{
		NONE,
		CENTER,
		LEFT,
		RIGHT
	}

	public string strDialogUnique = string.Empty;

	public int i32OrderUnique;

	public string QuestDlgCharCode = string.Empty;

	public string QuestDlgCharCode2 = string.Empty;

	public string strLang_Idx = string.Empty;

	public string strUserAnswer = string.Empty;

	public string EventUnique = string.Empty;

	public string strCharAniCode = string.Empty;

	public long i64OptionFlag;

	public string strSound = string.Empty;

	public bool bTalkUser;

	public QUEST_DLG_INFO.Alignment ePosition = QUEST_DLG_INFO.Alignment.CENTER;

	public QUEST_DLG_INFO.Alignment ePosition2;

	public string strLoadImage = string.Empty;

	public bool bImageClose;

	public int nNpcName;

	public bool bShowName = true;

	public bool bShowName2;

	public bool bShowImage = true;

	public bool bShowImage2;

	public void AddDLGOption(long flag)
	{
		this.i64OptionFlag |= flag;
	}

	public bool IsDLGOption(long flag)
	{
		return (this.i64OptionFlag & flag) != 0L;
	}

	public void MakeLoadImage()
	{
		this.strLoadImage = NrTSingleton<UIDataManager>.Instance.GetString(this.strDialogUnique, this.i32OrderUnique.ToString());
	}
}
