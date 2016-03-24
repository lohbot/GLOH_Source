using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

namespace CostumeDlg
{
	public class InitProcessor
	{
		public void InitCostumeSlotData(CostumeGuide_Dlg owner, ref Dictionary<byte, List<SolSlotData>> slotDataDic)
		{
			if (slotDataDic == null || owner == null)
			{
				return;
			}
			List<int> costumeKindList = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeKindList();
			foreach (int current in costumeKindList)
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current);
				if (charKindInfo != null)
				{
					SOL_GUIDE solGuild = NrTSingleton<NrTableSolGuideManager>.Instance.GetSolGuild(current);
					if (solGuild != null)
					{
						SolSlotData item = new SolSlotData(charKindInfo.GetName(), solGuild.m_i32CharKind, (byte)solGuild.m_iSolGrade, solGuild.m_bFlagSet, solGuild.m_bFlagSetCount - 1, solGuild.m_bSeason, solGuild.m_i32SkillUnique, solGuild.m_i32SkillText);
						if (!slotDataDic.ContainsKey(solGuild.m_bSeason))
						{
							slotDataDic.Add(solGuild.m_bSeason, new List<SolSlotData>());
						}
						if (!slotDataDic.ContainsKey(owner.ENTIRE_SEASON))
						{
							slotDataDic.Add(owner.ENTIRE_SEASON, new List<SolSlotData>());
						}
						slotDataDic[owner.ENTIRE_SEASON].Add(item);
						slotDataDic[solGuild.m_bSeason].Add(item);
					}
				}
			}
		}

		public void InitDropDownList_Season(CostumeGuide_Dlg owner, ref DropDownList DDL_Season)
		{
			List<byte> seasonList = NrTSingleton<NrTableSolGuideManager>.Instance.GetSeasonList();
			if (seasonList == null || DDL_Season == null || owner == null)
			{
				Debug.LogError("ERROR, CostumeGuide_Dlg.cs, SetDropDownList_Season(), solSesonList or DDL_Season or owner is Null");
				return;
			}
			DDL_Season.SetViewArea(seasonList.Count + 1);
			DDL_Season.Clear();
			ListItem listItem = new ListItem();
			listItem.Key = owner.ENTIRE_SEASON;
			listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1943"));
			DDL_Season.Add(listItem);
			string empty = string.Empty;
			foreach (byte current in seasonList)
			{
				ListItem listItem2 = new ListItem();
				listItem2.Key = current;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2302"),
					"count",
					current.ToString()
				});
				listItem2.SetColumnStr(0, empty);
				DDL_Season.Add(listItem2);
			}
			DDL_Season.RepositionItems();
			DDL_Season.SetFirstItem();
		}

		public void InitDropDownList_Setorder(CostumeGuide_Dlg owner, ref DropDownList DDL_Setorder)
		{
			DDL_Setorder.SetViewArea(3);
			DDL_Setorder.Clear();
			ListItem listItem = new ListItem();
			listItem.Key = SolGuideSlot.SLOTTYPE_GRADE_ASCENDING;
			listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1891"));
			DDL_Setorder.Add(listItem);
			ListItem listItem2 = new ListItem();
			listItem2.Key = SolGuideSlot.SLOTTYPE_GRADE_DESCENDING;
			listItem2.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1892"));
			DDL_Setorder.Add(listItem2);
			ListItem listItem3 = new ListItem();
			listItem3.Key = SolGuideSlot.SLOTTYPE_NAME;
			listItem3.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1890"));
			DDL_Setorder.Add(listItem3);
			DDL_Setorder.RepositionItems();
			DDL_Setorder.SetFirstItem();
			owner._currentSort = COSTUME_SORT.SLOTTYPE_GRADE_ASCENDING;
		}

		public void InitViewCostumeGuide(CostumeGuide_Dlg owner, COSTUMEGUIDE_SLOT[] slotList, List<SolSlotData> seasonSlotDataList, byte page)
		{
			this.ClearCostumeSlots(ref slotList);
			if (seasonSlotDataList == null || slotList == null)
			{
				return;
			}
			int num = (int)((page - 1) * 27);
			int count = seasonSlotDataList.Count;
			owner._maxPage = (byte)(count / 27);
			if (count % 27 > 0)
			{
				owner._maxPage += 1;
			}
			for (int i = 0; i < 27; i++)
			{
				if (seasonSlotDataList.Count <= num)
				{
					break;
				}
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(seasonSlotDataList[num].i32KindInfo);
				if (charKindInfo != null)
				{
					slotList[i].IT_Slot.SetSolImageTexure(eCharImageType.SMALL, charKindInfo.GetCharKind(), (int)(seasonSlotDataList[num].bSolGrade - 1));
				}
				else
				{
					slotList[i].IT_Slot.SetTexture("Win_T_ItemEmpty");
				}
				slotList[i].DT_BlackSlot.Hide(true);
				bool flag = NrTSingleton<NrCharCostumeTableManager>.Instance.IsNewCostumeExistByCode(charKindInfo.GetCode());
				slotList[i].Box_New.Hide(!flag);
				slotList[i].DT_Event.Hide(true);
				UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(charKindInfo.GetCharKind(), (int)seasonSlotDataList[num].bSolGrade);
				if (legendFrame != null)
				{
					slotList[i].DT_Slot.SetTexture(legendFrame);
				}
				else
				{
					slotList[i].DT_Slot.SetTexture("Win_T_ItemEmpty");
				}
				if (charKindInfo != null)
				{
					slotList[i].BT_Slot.Data = charKindInfo.GetCharKind();
				}
				num++;
			}
			owner.ChangePageText();
		}

		private void ClearCostumeSlots(ref COSTUMEGUIDE_SLOT[] slotList)
		{
			if (slotList == null)
			{
				return;
			}
			if (slotList.Length != 27)
			{
				return;
			}
			for (int i = 0; i < 27; i++)
			{
				slotList[i].IT_Slot.ClearData();
				slotList[i].IT_Slot.SetTexture("Win_T_ItemEmpty");
				slotList[i].DT_BlackSlot.Hide(false);
				slotList[i].Box_New.Hide(true);
				slotList[i].DT_Event.Hide(true);
				slotList[i].DT_Slot.SetTexture("Win_T_ItemEmpty");
				slotList[i].BT_Slot.Data = -1;
			}
		}
	}
}
