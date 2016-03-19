using System;
using TsLibs;

public class QUEST_COMMON
{
	public string strQuestUnique = string.Empty;

	public int nQuestGroupUnique;

	public short[] i16RequireLevel = new short[5];

	public short nReputeUnique;

	public int nReputeRate;

	public string strTextKey = string.Empty;

	public string strQuestSummaryTextKey = string.Empty;

	public string strQuestHintTextKey = string.Empty;

	public int i32QuestLevel;

	public string strAccecptCheck = string.Empty;

	public int nChapterEndCount;

	public string strPreQuestUnique = string.Empty;

	public string strNextQuestUnique = string.Empty;

	public int i32QuestCharKind;

	public string GiveQuestCharCode = string.Empty;

	public int iCastleUnique;

	public int iSectionUnique;

	public float fSourceX;

	public float fSourceZ;

	public QUEST_CONDITION[] cQuestCondition = new QUEST_CONDITION[3];

	public int i32EndType;

	public long i64EndTypeVal;

	public int i32EndCasteUnique;

	public int i32EndSectionUnique;

	public float fEndPosX;

	public float fEndPosY;

	public int i32QuestTime;

	public short nRepeat;

	public float fRandomVal;

	public int i32ScenarioUnique;

	public int i32CharKind;

	public bool bIsLimit;

	public QEUST_REWARD_ITEM[] cQuestRewardItem = new QEUST_REWARD_ITEM[5];

	public QUEST_COMMON_SUB[] kQuestCommonSub = new QUEST_COMMON_SUB[3];

	public string szTellDoingKey = string.Empty;

	public bool bTellDoingType;

	public string szTellCompleteKey = string.Empty;

	public bool bTellCompleteType;

	public short nLimitLevel;

	public int nHelpText;

	public void SetData(TsDataReader.Row row)
	{
		this.strQuestUnique = string.Empty;
		this.nQuestGroupUnique = 0;
		this.i16RequireLevel[0] = 0;
		this.i16RequireLevel[1] = 0;
		this.i16RequireLevel[2] = 0;
		this.i16RequireLevel[3] = 0;
		this.i16RequireLevel[4] = 0;
		this.nReputeUnique = 0;
		this.nReputeRate = 0;
		this.i32QuestCharKind = 0;
		this.iCastleUnique = 0;
		this.iSectionUnique = 0;
		this.fSourceX = 0f;
		this.fSourceZ = 0f;
		this.i32QuestLevel = 0;
		this.strAccecptCheck = string.Empty;
		this.nChapterEndCount = 0;
		this.strPreQuestUnique = string.Empty;
		this.strNextQuestUnique = string.Empty;
		this.i32QuestTime = 0;
		this.nRepeat = 0;
		this.i32EndType = 0;
		this.i64EndTypeVal = 0L;
		this.fRandomVal = 0f;
		this.strTextKey = string.Empty;
		this.i32EndCasteUnique = 0;
		this.i32EndSectionUnique = 0;
		this.fEndPosX = 0f;
		this.fEndPosY = 0f;
		this.strQuestSummaryTextKey = string.Empty;
		this.strQuestHintTextKey = string.Empty;
		this.szTellDoingKey = string.Empty;
		this.bTellDoingType = false;
		this.szTellCompleteKey = string.Empty;
		this.bTellCompleteType = false;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		string empty5 = string.Empty;
		for (int i = 0; i < 3; i++)
		{
			this.cQuestCondition[i] = new QUEST_CONDITION();
		}
		for (int j = 0; j < 5; j++)
		{
			this.cQuestRewardItem[j] = new QEUST_REWARD_ITEM();
		}
		for (int k = 0; k < 3; k++)
		{
			this.kQuestCommonSub[k] = new QUEST_COMMON_SUB();
		}
		string empty6 = string.Empty;
		this.nLimitLevel = 0;
		int num = 0;
		row.GetColumn(num++, out this.strQuestUnique);
		row.GetColumn(num++, out this.nQuestGroupUnique);
		row.GetColumn(num++, out this.i16RequireLevel[0]);
		row.GetColumn(num++, out this.i16RequireLevel[1]);
		row.GetColumn(num++, out this.i16RequireLevel[2]);
		row.GetColumn(num++, out this.i16RequireLevel[3]);
		row.GetColumn(num++, out this.i16RequireLevel[4]);
		row.GetColumn(num++, out this.nReputeUnique);
		row.GetColumn(num++, out this.nReputeRate);
		row.GetColumn(num++, out this.strAccecptCheck);
		row.GetColumn(num++, out this.nChapterEndCount);
		row.GetColumn(num++, out this.strPreQuestUnique);
		row.GetColumn(num++, out this.strNextQuestUnique);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.cQuestCondition[0].i32QuestCode);
		row.GetColumn(num++, out this.cQuestCondition[0].szCodeTextKey);
		row.GetColumn(num++, out this.cQuestCondition[0].i64Param);
		row.GetColumn(num++, out this.cQuestCondition[0].i64ParamVal);
		row.GetColumn(num++, out empty2);
		row.GetColumn(num++, out this.cQuestCondition[1].i32QuestCode);
		row.GetColumn(num++, out this.cQuestCondition[1].szCodeTextKey);
		row.GetColumn(num++, out this.cQuestCondition[1].i64Param);
		row.GetColumn(num++, out this.cQuestCondition[1].i64ParamVal);
		row.GetColumn(num++, out empty3);
		row.GetColumn(num++, out this.cQuestCondition[2].i32QuestCode);
		row.GetColumn(num++, out this.cQuestCondition[2].szCodeTextKey);
		row.GetColumn(num++, out this.cQuestCondition[2].i64Param);
		row.GetColumn(num++, out this.cQuestCondition[2].i64ParamVal);
		row.GetColumn(num++, out empty4);
		row.GetColumn(num++, out this.i32EndType);
		row.GetColumn(num++, out empty5);
		row.GetColumn(num++, out this.i32QuestTime);
		row.GetColumn(num++, out this.nRepeat);
		row.GetColumn(num++, out this.fRandomVal);
		row.GetColumn(num++, out this.i32ScenarioUnique);
		row.GetColumn(num++, out this.i32CharKind);
		row.GetColumn(num++, out empty6);
		row.GetColumn(num++, out this.nLimitLevel);
		row.GetColumn(num++, out this.nHelpText);
		this.strTextKey = this.strQuestUnique + "_title";
		this.strQuestSummaryTextKey = this.strQuestUnique + "_summary";
		this.strQuestHintTextKey = this.strQuestUnique + "_hint";
		int num2 = -2;
		this.SetPlace(empty, ref this.iCastleUnique, ref this.iSectionUnique, ref this.fSourceX, ref this.fSourceZ, ref this.i32QuestCharKind);
		this.SetPlace(empty2, ref this.cQuestCondition[0].nMapUnique, ref this.cQuestCondition[0].i32SectonUnique, ref this.cQuestCondition[0].fTargetPosX, ref this.cQuestCondition[0].fTargetPosZ, ref this.cQuestCondition[0].i32CharKind);
		this.SetPlace(empty3, ref this.cQuestCondition[1].nMapUnique, ref this.cQuestCondition[1].i32SectonUnique, ref this.cQuestCondition[1].fTargetPosX, ref this.cQuestCondition[1].fTargetPosZ, ref this.cQuestCondition[1].i32CharKind);
		this.SetPlace(empty4, ref this.cQuestCondition[2].nMapUnique, ref this.cQuestCondition[2].i32SectonUnique, ref this.cQuestCondition[2].fTargetPosX, ref this.cQuestCondition[2].fTargetPosZ, ref this.cQuestCondition[2].i32CharKind);
		this.SetPlace(empty5, ref this.i32EndCasteUnique, ref this.i32EndSectionUnique, ref this.fEndPosX, ref this.fEndPosY, ref num2);
		this.i64EndTypeVal = (long)num2;
		this.ParseTellInfo(empty6, ref this.szTellDoingKey, ref this.bTellDoingType, ref this.szTellCompleteKey, ref this.bTellCompleteType);
	}

	public void SetPlace(string strPlace, ref int i32CastleUnique, ref int i32SectionUnique, ref float fX, ref float fZ, ref int i32SrcCharKind)
	{
		if (strPlace.Equals("off"))
		{
			i32CastleUnique = -1;
			return;
		}
		char[] array = TKString.StringChar(strPlace);
		byte b = 0;
		byte b2 = 0;
		int charkind = 0;
		string text = string.Empty;
		bool flag = false;
		while ((int)b < strPlace.Length)
		{
			if (array[(int)b] == ' ')
			{
				b += 1;
			}
			else if (array[(int)b] == '_')
			{
				if (b2 == 0)
				{
					if (text == "pos")
					{
						flag = true;
					}
					else
					{
						int.TryParse(text, out i32CastleUnique);
					}
					text = string.Empty;
					b2 += 1;
				}
				else if (b2 == 1)
				{
					if (flag)
					{
						int.TryParse(text, out i32CastleUnique);
					}
					else
					{
						int.TryParse(text, out i32SectionUnique);
					}
					text = string.Empty;
					b2 += 1;
				}
				else if (b2 == 2)
				{
					if (flag)
					{
						float.TryParse(text, out fX);
					}
					text = string.Empty;
					b2 += 1;
				}
				b += 1;
			}
			else if (array[(int)b] == '/')
			{
				text += '_';
				b += 1;
			}
			else
			{
				text += array[(int)b];
				b += 1;
			}
		}
		if (flag)
		{
			float.TryParse(text, out fZ);
		}
		else
		{
			int.TryParse(text, out i32SrcCharKind);
			int.TryParse(text, out charkind);
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(charkind);
			if (charKindInfo != null)
			{
				QUEST_AUTO_PATH_POS_CHARCODE autoPath = NrTSingleton<NkQuestManager>.Instance.GetAutoPath((int)((short)i32CastleUnique), 0, charKindInfo.GetCode());
				if (autoPath != null)
				{
					fX = autoPath.fDesX;
					fZ = autoPath.fDesY;
				}
			}
		}
	}

	private void ParseTellInfo(string data, ref string doingKey, ref bool doingType, ref string completeKey, ref bool completeType)
	{
		if (data == null || data.Equals("0"))
		{
			return;
		}
		char[] separator = new char[]
		{
			'+'
		};
		string[] array = data.Split(separator);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Contains("g"))
			{
				int startIndex = array[i].IndexOf("g");
				array[i] = array[i].Remove(startIndex, 1);
				if (array[i].Contains("a"))
				{
					doingType = true;
					doingKey = array[i].Remove(array[i].Length - 1);
				}
				else
				{
					doingKey = array[i];
				}
			}
			else if (array[i].Contains("p"))
			{
				int startIndex2 = array[i].IndexOf("p");
				array[i] = array[i].Remove(startIndex2, 1);
				if (array[i].Contains("a"))
				{
					completeType = true;
					completeKey = array[i].Remove(array[i].Length - 1);
				}
				else
				{
					completeKey = array[i];
				}
			}
		}
	}
}
