using System;
using TsLibs;

public class NrClientNpcInfo : NrTableData
{
	public int i32MapIndex;

	public float fFixPosX;

	public float fFixPosY;

	public float i16FixPosA;

	public string strCharCode = string.Empty;

	public NrNpcClientCondition kStartCon = new NrNpcClientCondition();

	public NrNpcClientCondition kEndCon = new NrNpcClientCondition();

	public int i32Count = 1;

	public NrClientNpcInfo()
	{
		this.Init();
	}

	public void Init()
	{
		this.i32MapIndex = 0;
		this.fFixPosX = 0f;
		this.fFixPosY = 0f;
		this.i16FixPosA = 0f;
		this.strCharCode = string.Empty;
		this.i32Count = 1;
		this.kStartCon.strQuestUnique = string.Empty;
		this.kStartCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_END;
		this.kEndCon.strQuestUnique = string.Empty;
		this.kEndCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_END;
	}

	public override void SetData(TsDataReader.Row tsRow)
	{
		this.Init();
		string text = string.Empty;
		string empty = string.Empty;
		string empty2 = string.Empty;
		int num = 0;
		int num2 = 0;
		tsRow.GetColumn(num2++, out this.i32MapIndex);
		tsRow.GetColumn(num2++, out this.fFixPosX);
		tsRow.GetColumn(num2++, out this.fFixPosY);
		tsRow.GetColumn(num2++, out this.i16FixPosA);
		tsRow.GetColumn(num2++, out this.strCharCode);
		tsRow.GetColumn(num2++, out num);
		tsRow.GetColumn(num2++, out empty);
		tsRow.GetColumn(num2++, out empty2);
		text = empty + "/" + empty2;
		char[] array = TKString.StringChar(text);
		byte b = 0;
		byte b2 = 0;
		string text2 = string.Empty;
		while ((int)b < text.Length)
		{
			if (array[(int)b] == ' ')
			{
				b += 1;
			}
			else if (array[(int)b] == '+')
			{
				if (b2 == 0)
				{
					this.kStartCon.strQuestUnique = text2;
					text2 = string.Empty;
					b2 += 1;
				}
				else if (b2 == 2)
				{
					this.kEndCon.strQuestUnique = text2;
					text2 = string.Empty;
					b2 += 1;
				}
				b += 1;
			}
			else if (array[(int)b] == '/')
			{
				if (b2 == 1)
				{
					if (text2 == "a")
					{
						this.kStartCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE;
					}
					else if (text2 == "g")
					{
						this.kStartCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING;
					}
					else if (text2 == "s")
					{
						this.kStartCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE;
					}
					else if (text2 == "p")
					{
						this.kStartCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE;
					}
					text2 = string.Empty;
					b2 += 1;
				}
				else if (b2 == 3)
				{
					if (text2 == "a")
					{
						this.kEndCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE;
					}
					else if (text2 == "g")
					{
						this.kEndCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING;
					}
					else if (text2 == "s")
					{
						this.kEndCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE;
					}
					else if (text2 == "p")
					{
						this.kEndCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE;
					}
					text2 = string.Empty;
					b2 += 1;
				}
				b += 1;
			}
			else
			{
				text2 += array[(int)b];
				b += 1;
			}
		}
		if (b2 == 3)
		{
			if (text2 == "a")
			{
				this.kEndCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE;
			}
			else if (text2 == "g")
			{
				this.kEndCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING;
			}
			else if (text2 == "s")
			{
				this.kEndCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE;
			}
			else if (text2 == "p")
			{
				this.kEndCon.eQuestCase = QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE;
			}
		}
		else if (b2 == 4)
		{
			int.TryParse(text2, out this.i32Count);
		}
	}
}
