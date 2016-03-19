using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;

public class NrTableEvnetHeroManager : NrTSingleton<NrTableEvnetHeroManager>
{
	private List<EVENT_HERODATA> m_EventHeroList = new List<EVENT_HERODATA>();

	private NrTableEvnetHeroManager()
	{
	}

	public void AddEventHero(EVENT_HERO _EventHero)
	{
		EVENT_HERODATA eVENT_HERODATA = new EVENT_HERODATA();
		eVENT_HERODATA.szCharCode = _EventHero.m_strCharCode;
		eVENT_HERODATA.i8Rank = _EventHero.m_i8Rank;
		eVENT_HERODATA.i32Attack = _EventHero.m_i32Atk;
		eVENT_HERODATA.i32Hp = _EventHero.m_i32Hp;
		DateTime tStartTime = new DateTime(_EventHero.m_i32StartYear, _EventHero.m_i32StartMon, _EventHero.m_i32StartDay);
		DateTime tEndTime = new DateTime(_EventHero.m_i32EndYear, _EventHero.m_i32EndMon, _EventHero.m_i32EndDay);
		eVENT_HERODATA.tStartTime = tStartTime;
		eVENT_HERODATA.tEndTime = tEndTime;
		eVENT_HERODATA.i32CharKind = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(_EventHero.m_strCharCode);
		if (eVENT_HERODATA.i32CharKind != 0)
		{
			this.m_EventHeroList.Add(eVENT_HERODATA);
		}
	}

	public List<EVENT_HERODATA> GetValue()
	{
		return this.m_EventHeroList;
	}

	public EVENT_HERODATA GetEventHeroCharCode(int i32Kind, byte i8Rank)
	{
		EVENT_HERODATA result = null;
		if (i8Rank < 0)
		{
			i8Rank = 0;
		}
		for (int i = 0; i < this.m_EventHeroList.Count; i++)
		{
			if (i32Kind == this.m_EventHeroList[i].i32CharKind && i8Rank >= this.m_EventHeroList[i].i8Rank - 1)
			{
				if (i8Rank == this.m_EventHeroList[i].i8Rank - 1)
				{
					return this.m_EventHeroList[i];
				}
				if (i8Rank > this.m_EventHeroList[i].i8Rank - 1)
				{
					result = this.m_EventHeroList[i];
				}
			}
		}
		return result;
	}

	public EVENT_HERODATA GetEventHeroCharFriendCode(int i32Kind)
	{
		EVENT_HERODATA result = null;
		for (int i = 0; i < this.m_EventHeroList.Count; i++)
		{
			if (i32Kind == this.m_EventHeroList[i].i32CharKind)
			{
				result = this.m_EventHeroList[i];
				break;
			}
		}
		return result;
	}

	public void EventHeroDataClear()
	{
		this.m_EventHeroList.Clear();
	}

	public EVENT_HERODATA GetEventHeroCheck(int i32Kind, byte i8Rank)
	{
		EVENT_HERODATA result = null;
		for (int i = 0; i < this.m_EventHeroList.Count; i++)
		{
			if (i32Kind == this.m_EventHeroList[i].i32CharKind && i8Rank == this.m_EventHeroList[i].i8Rank)
			{
				result = this.m_EventHeroList[i];
			}
		}
		return result;
	}

	public void SetEventHeroCheck(EVENT_HERODATA _EventHero)
	{
		for (int i = 0; i < this.m_EventHeroList.Count; i++)
		{
			if (_EventHero.i32CharKind == this.m_EventHeroList[i].i32CharKind && this.m_EventHeroList[i].i8Rank == _EventHero.i8Rank)
			{
				this.m_EventHeroList[i].i32Attack = _EventHero.i32CharKind;
				this.m_EventHeroList[i].i8Rank = _EventHero.i8Rank;
				this.m_EventHeroList[i].i32Hp = _EventHero.i32Hp;
				this.m_EventHeroList[i].tStartTime = _EventHero.tStartTime;
				this.m_EventHeroList[i].tEndTime = _EventHero.tEndTime;
			}
		}
	}

	public void SetServerEventHero(EVENT_HEROINFO _EventHero)
	{
		EVENT_HERODATA eVENT_HERODATA = new EVENT_HERODATA();
		eVENT_HERODATA.szCharCode = TKString.NEWString(_EventHero.szCharCode);
		eVENT_HERODATA.i8Rank = _EventHero.i8Rank;
		eVENT_HERODATA.i32Attack = _EventHero.i32Attack;
		eVENT_HERODATA.i32Hp = _EventHero.i32Hp;
		DateTime dueDate = PublicMethod.GetDueDate(_EventHero.i64StartTime);
		eVENT_HERODATA.tStartTime = dueDate;
		DateTime dueDate2 = PublicMethod.GetDueDate(_EventHero.i64EndTime);
		eVENT_HERODATA.tEndTime = dueDate2;
		eVENT_HERODATA.i32CharKind = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(TKString.NEWString(_EventHero.szCharCode));
		if (eVENT_HERODATA.i32CharKind != 0)
		{
			this.m_EventHeroList.Add(eVENT_HERODATA);
		}
		else
		{
			TsLog.LogWarning("TableEventHero - > CharCode Error", new object[0]);
		}
	}

	public void AddServerEventHero(EVENT_HEROINFO _EventHero)
	{
		EVENT_HERODATA eVENT_HERODATA = new EVENT_HERODATA();
		eVENT_HERODATA.szCharCode = TKString.NEWString(_EventHero.szCharCode);
		eVENT_HERODATA.i8Rank = _EventHero.i8Rank;
		eVENT_HERODATA.i32Attack = _EventHero.i32Attack;
		eVENT_HERODATA.i32Hp = _EventHero.i32Hp;
		DateTime dueDate = PublicMethod.GetDueDate(_EventHero.i64StartTime);
		eVENT_HERODATA.tStartTime = dueDate;
		DateTime dueDate2 = PublicMethod.GetDueDate(_EventHero.i64EndTime);
		eVENT_HERODATA.tEndTime = dueDate2;
		eVENT_HERODATA.i32CharKind = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(TKString.NEWString(_EventHero.szCharCode));
		if (eVENT_HERODATA.i32CharKind != 0)
		{
			EVENT_HERODATA eventHeroCheck = this.GetEventHeroCheck(eVENT_HERODATA.i32CharKind, eVENT_HERODATA.i8Rank);
			if (eventHeroCheck != null)
			{
				this.SetEventHeroCheck(eVENT_HERODATA);
			}
			else
			{
				this.m_EventHeroList.Add(eVENT_HERODATA);
			}
		}
		else
		{
			TsLog.LogWarning("TableEventHero - > CharCode Error", new object[0]);
		}
	}

	public void DelServerEventHero(EVENT_HEROINFO _EventHero)
	{
		int charKindByCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(TKString.NEWString(_EventHero.szCharCode));
		EVENT_HERODATA eventHeroCheck = this.GetEventHeroCheck(charKindByCode, _EventHero.i8Rank);
		if (eventHeroCheck != null)
		{
			this.m_EventHeroList.Remove(eventHeroCheck);
		}
	}
}
