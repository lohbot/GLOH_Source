using System;
using TsLibs;

public class QUEST_CHAPTER
{
	public short i16QuestChapterUnique;

	public short i16ChapterIdx;

	public string strChapterTextKey = string.Empty;

	public bool bIsLimit;

	public void SetData(TsDataReader.Row row)
	{
		this.i16QuestChapterUnique = 0;
		this.i16ChapterIdx = 0;
		this.strChapterTextKey = string.Empty;
		int num = 0;
		row.GetColumn(num++, out this.i16QuestChapterUnique);
		row.GetColumn(num++, out this.i16ChapterIdx);
		row.GetColumn(num++, out this.strChapterTextKey);
	}
}
