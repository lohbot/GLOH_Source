using GAME;
using System;
using TsLibs;

[Serializable]
public class EventTriggerInfo : NrTableData
{
	[Flags]
	public enum EventTriggerATB
	{
		None = 0,
		Repeat = 1,
		EventArea = 2
	}

	public string Name = string.Empty;

	public int Unique;

	public int MapIdx;

	public EventTriggerFlag Flag;

	public EventTriggerInfo.EventTriggerATB ATB;

	public string QuestTextIndex = string.Empty;

	public string Comment = string.Empty;

	public POS3D[] EventAreaPos;

	public EventTrigger_Game EventTrigger;

	public EventTriggerInfo()
	{
		this.Init();
	}

	public EventTriggerInfo(EventTriggerInfo Info)
	{
		this.Init();
		this.Set(Info.Name, Info.Unique, Info.MapIdx, Info.Flag.Get(), Info.ATB, Info.QuestTextIndex, Info.Comment);
	}

	public void Init()
	{
		this.Name = string.Empty;
		this.Unique = 0;
		this.MapIdx = 0;
		this.Flag = new EventTriggerFlag(0L);
		this.ATB = EventTriggerInfo.EventTriggerATB.None;
		this.QuestTextIndex = string.Empty;
		this.Comment = string.Empty;
	}

	public void Set(string name, int unique, int mapidx, long flag, string atb, string textidx, string comment)
	{
		this.Set(name, unique, this.MapIdx, flag, EventTriggerInfo.ParseATB(atb), textidx, comment);
	}

	public void Set(string name, int unique, int mapidx, long flag, EventTriggerInfo.EventTriggerATB atb, string textidx, string comment)
	{
		this.Name = name;
		this.Unique = unique;
		this.MapIdx = mapidx;
		this.Flag.Set(flag);
		this.ATB = atb;
		this.QuestTextIndex = textidx;
		this.Comment = comment;
	}

	public override void SetData(TsDataReader.Row tsRow)
	{
		short num = 0;
		this.Init();
		tsRow.GetColumn(0, out this.Unique);
		tsRow.GetColumn(1, out this.Name);
		tsRow.GetColumn(2, out this.MapIdx);
		tsRow.GetColumn(3, out num);
		this.Flag = new EventTriggerFlag((long)num);
		string empty = string.Empty;
		tsRow.GetColumn(4, out empty);
		this.ATB = EventTriggerInfo.ParseATB(empty);
		tsRow.GetColumn(5, out this.QuestTextIndex);
		tsRow.GetColumn(6, out this.Comment);
	}

	public static bool IsATB(EventTriggerInfo.EventTriggerATB value, EventTriggerInfo.EventTriggerATB ATB)
	{
		return (value & ATB) > EventTriggerInfo.EventTriggerATB.None;
	}

	public static string ParseATBToString(EventTriggerInfo.EventTriggerATB ATB)
	{
		string text = EventTriggerInfo.EventTriggerATB.None.ToString();
		EventTriggerInfo.EventTriggerATB[] array = Enum.GetValues(typeof(EventTriggerInfo.EventTriggerATB)) as EventTriggerInfo.EventTriggerATB[];
		EventTriggerInfo.EventTriggerATB[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EventTriggerInfo.EventTriggerATB eventTriggerATB = array2[i];
			if (EventTriggerInfo.IsATB(ATB, eventTriggerATB))
			{
				if (text != EventTriggerInfo.EventTriggerATB.None.ToString())
				{
					text += "+";
				}
				else
				{
					text = string.Empty;
				}
				text += Enum.GetName(typeof(EventTriggerInfo.EventTriggerATB), eventTriggerATB);
			}
		}
		return text;
	}

	public static EventTriggerInfo.EventTriggerATB ParseATB(string strATB)
	{
		string[] array = strATB.Split(new char[]
		{
			'+'
		});
		EventTriggerInfo.EventTriggerATB eventTriggerATB = EventTriggerInfo.EventTriggerATB.None;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i];
			if (!string.IsNullOrEmpty(text))
			{
				if (!(text == "null"))
				{
					try
					{
						object obj = Enum.Parse(typeof(EventTriggerInfo.EventTriggerATB), text);
						if (obj is EventTriggerInfo.EventTriggerATB)
						{
							eventTriggerATB |= (EventTriggerInfo.EventTriggerATB)((int)obj);
						}
					}
					catch (Exception ex)
					{
						TsLog.LogWarning(ex.Message, new object[0]);
						TsLog.LogWarning(string.Format("EventTriggerInfo Parse Error : {0}", strATB), new object[0]);
					}
				}
			}
		}
		return eventTriggerATB;
	}
}
