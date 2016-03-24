using System;
using System.Collections.Generic;
using UnityEngine;

namespace CostumeDlg
{
	public class SlotDataProcessor
	{
		private Dictionary<COSTUME_SORT, Comparison<SolSlotData>> _sortDic;

		public SlotDataProcessor()
		{
			this._sortDic = new Dictionary<COSTUME_SORT, Comparison<SolSlotData>>();
			this._sortDic.Add(COSTUME_SORT.SLOTTYPE_GRADE_ASCENDING, new Comparison<SolSlotData>(this.CompareSolGradeAscend));
			this._sortDic.Add(COSTUME_SORT.SLOTTYPE_GRADE_DESCENDING, new Comparison<SolSlotData>(this.CompareSolGradeDescend));
			this._sortDic.Add(COSTUME_SORT.SLOTTYPE_NAME, new Comparison<SolSlotData>(this.CompareSolName));
		}

		public List<SolSlotData> GetSolSlotData(ref Dictionary<byte, List<SolSlotData>> slotDataDic, byte season, COSTUME_SORT sort)
		{
			List<SolSlotData> solSlotDataBySeason = this.GetSolSlotDataBySeason(ref slotDataDic, season);
			if (solSlotDataBySeason == null)
			{
				return solSlotDataBySeason;
			}
			return this.GetSortData(ref solSlotDataBySeason, sort);
		}

		private List<SolSlotData> GetSolSlotDataBySeason(ref Dictionary<byte, List<SolSlotData>> slotDataDic, byte season)
		{
			if (slotDataDic == null)
			{
				Debug.LogError("ERROR, SlotDataSortProcessor.cs, GetSolSlotDataBySeason(), slotDataDic is Null ");
				return null;
			}
			List<SolSlotData> result = null;
			slotDataDic.TryGetValue(season, out result);
			return result;
		}

		private List<SolSlotData> GetSortData(ref List<SolSlotData> slotDataList, COSTUME_SORT sort)
		{
			if (slotDataList == null || this._sortDic == null)
			{
				Debug.LogError("ERROR, SlotDataSortProcessor.cs, slotDataList(), slotDataList is Null ");
				return null;
			}
			if (!this._sortDic.ContainsKey(sort))
			{
				return slotDataList;
			}
			slotDataList.Sort(this._sortDic[sort]);
			return slotDataList;
		}

		private int CompareSolName(SolSlotData a, SolSlotData b)
		{
			return a.strSolName.CompareTo(b.strSolName);
		}

		private int CompareSolGradeAscend(SolSlotData a, SolSlotData b)
		{
			if (a.bSolGrade < b.bSolGrade)
			{
				return 1;
			}
			if (a.bSolGrade != b.bSolGrade)
			{
				return -1;
			}
			if (a.bBitFlagCount < b.bBitFlagCount)
			{
				return 1;
			}
			return 0;
		}

		private int CompareSolGradeDescend(SolSlotData a, SolSlotData b)
		{
			if (a.bSolGrade > b.bSolGrade)
			{
				return 1;
			}
			if (a.bSolGrade != b.bSolGrade)
			{
				return -1;
			}
			if (a.bBitFlagCount < b.bBitFlagCount)
			{
				return 1;
			}
			return 0;
		}
	}
}
