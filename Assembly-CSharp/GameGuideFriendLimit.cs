using GAME;
using System;
using UnityEngine;
using UnityForms;

public class GameGuideFriendLimit : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
		if (communityUI_DLG != null)
		{
			communityUI_DLG.RequestCommunityData(eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET);
		}
		this.Init();
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuide()
	{
		if (this.m_eCheck == GameGuideCheck.LEVELUP)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return false;
			}
			short level = (short)kMyCharInfo.GetLevel();
			int @int = PlayerPrefs.GetInt("FriendLimitCheckLevel");
			FRIENDCOUNTLIMIT_DATA firneCountLimitInfo = BASE_FRIENDCOUNTLIMIT_DATA.GetFirneCountLimitInfo(level);
			if (firneCountLimitInfo == null)
			{
				return false;
			}
			if ((int)firneCountLimitInfo.Level_Max > @int)
			{
				PlayerPrefs.SetInt("FriendLimitCheckLevel", (int)firneCountLimitInfo.Level_Max);
				return true;
			}
		}
		return false;
	}

	public override string GetGameGuideText()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return string.Empty;
		}
		short level = (short)kMyCharInfo.GetLevel();
		int num = 0;
		FRIENDCOUNTLIMIT_DATA firneCountLimitInfo = BASE_FRIENDCOUNTLIMIT_DATA.GetFirneCountLimitInfo(level);
		if (firneCountLimitInfo != null)
		{
			num = firneCountLimitInfo.FriendLimitCount;
		}
		string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip("2052");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromToolTip,
			"count",
			num
		});
		return empty;
	}
}
