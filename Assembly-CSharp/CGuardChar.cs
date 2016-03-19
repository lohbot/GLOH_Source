using System;

public class CGuardChar : CQuestCondition
{
	public override bool IsServerCheck()
	{
		return false;
	}

	public override bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		int num = 0;
		NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (nrCharUser != null)
		{
			for (int i = 0; i < 10; i++)
			{
				if ((long)nrCharUser.GetSubChsrKind(i) == base.GetParam())
				{
					num++;
				}
			}
			i64ParamVal = (long)num;
			if ((long)num >= base.GetParamVal())
			{
				return true;
			}
		}
		return false;
	}

	public override string GetConditionText(long i64ParamVal)
	{
		string text = "NoName";
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo((int)base.GetParam());
		if (charKindInfo != null)
		{
			text = charKindInfo.GetName();
		}
		int num = 0;
		NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (nrCharUser != null)
		{
			for (int i = 0; i < 10; i++)
			{
				if ((long)nrCharUser.GetSubChsrKind(i) == base.GetParam())
				{
					num++;
				}
			}
		}
		string textFromQuest_Code = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Code(this.m_szTextKey);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromQuest_Code,
			"targetname",
			text,
			"count1",
			num,
			"count2",
			base.GetParamVal().ToString()
		});
		return empty;
	}
}
