using System;
using TsBundle;
using UnityForms;

public class CAcceptBringChar : CQuestCondition
{
	private int m_i32SubChar;

	public override bool IsServerCheck()
	{
		return false;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		if (i64Param == base.GetParam() && i64ParamVal >= base.GetParamVal())
		{
			NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetChar(1);
			i64ParamVal = (long)nrCharUser.GetSubChsrKind(0);
			return true;
		}
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_i32SubChar);
		string text = "NoName1";
		if (charKindInfo != null)
		{
			text = charKindInfo.GetName();
		}
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			text
		});
		return empty;
	}

	public override void AfterAutoPath()
	{
		QuestAutoPathInfo autoPathInfo = NrTSingleton<NkQuestManager>.Instance.GetAutoPathInfo();
		if (autoPathInfo != null)
		{
			NrCharNPC nrCharNPC = (NrCharNPC)NrTSingleton<NkCharManager>.Instance.GetCharByCharKind(autoPathInfo.m_nCharKind);
			if (nrCharNPC != null && autoPathInfo.m_nCharKind > 0 && nrCharNPC != null)
			{
				NrCharKindInfo charKindInfo = nrCharNPC.GetCharKindInfo();
				if (charKindInfo != null)
				{
					NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NPCTALK_DLG);
					npcTalkUI_DLG.SetNpcID(charKindInfo.GetCharKind(), nrCharNPC.GetCharUnique());
					npcTalkUI_DLG.Show();
					CHARKIND_NPCINFO cHARKIND_NPCINFO = charKindInfo.GetCHARKIND_NPCINFO();
					if (cHARKIND_NPCINFO != null && !string.IsNullOrEmpty(cHARKIND_NPCINFO.SOUND_GREETING))
					{
						TsAudioManager.Instance.AudioContainer.RequestAudioClip("NPC", cHARKIND_NPCINFO.SOUND_GREETING, "GREETING", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
					}
				}
			}
		}
	}

	public void SetSuChar(int i32SubID)
	{
		this.m_i32SubChar = i32SubID;
	}
}
