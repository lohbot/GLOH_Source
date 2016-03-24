using System;
using System.Collections.Generic;
using UnityForms;

public class NrDailyGiftManager : NrTSingleton<NrDailyGiftManager>
{
	private List<DAILY_GIFT> m_dailyGiftDataList = new List<DAILY_GIFT>();

	private int m_idailyGroupUnique = 1;

	private NrDailyGiftManager()
	{
		this.m_dailyGiftDataList.Clear();
	}

	public DAILY_GIFT[] GetDailyGiftInfo()
	{
		return this.m_dailyGiftDataList.ToArray();
	}

	public void AddData(DAILY_GIFT data)
	{
		this.m_dailyGiftDataList.Add(data);
	}

	public void SetServerGroupUnique(int GroupUnique)
	{
		this.m_idailyGroupUnique = GroupUnique;
	}

	public bool GetDailyGiftItemInfo(byte DayCount, out int itemUnique, out short itemCount, out string bundleName)
	{
		itemUnique = 0;
		itemCount = 0;
		bundleName = string.Empty;
		if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
		{
			return false;
		}
		int level = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel();
		for (int i = 0; i < this.m_dailyGiftDataList.Count; i++)
		{
			DAILY_GIFT dAILY_GIFT = this.m_dailyGiftDataList[i];
			if (this.m_idailyGroupUnique == (int)dAILY_GIFT.GroupUnique && (short)DayCount == dAILY_GIFT.DayCount)
			{
				for (int j = 10; j >= 0; j--)
				{
					if (level > (int)dAILY_GIFT.i16Lev[j])
					{
						itemUnique = dAILY_GIFT.i32ItemUnique[j];
						itemCount = dAILY_GIFT.i16ItemCount[j];
						bundleName = dAILY_GIFT.strBundleName[j];
						return true;
					}
				}
			}
		}
		return false;
	}

	public void SetDailyAttendNotify()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		NoticeIconDlg noticeIconDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_UI_ICON) as NoticeIconDlg;
		if (noticeIconDlg != null && kMyCharInfo != null && NrTSingleton<ContentsLimitManager>.Instance.IsAttend())
		{
			return;
		}
	}
}
