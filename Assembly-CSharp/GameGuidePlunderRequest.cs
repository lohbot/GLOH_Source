using GAME;
using System;
using UnityForms;

public class GameGuidePlunderRequest : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsHeroBattle())
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ENABLE_PLUNDER);
			if (charSubData == 1L)
			{
				return;
			}
			PlunderAgreeDlg plunderAgreeDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_AGREE_DLG) as PlunderAgreeDlg;
			if (plunderAgreeDlg != null)
			{
				plunderAgreeDlg.Show();
			}
		}
		this.Init();
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuide()
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsHeroBattle() && this.m_eCheck == GameGuideCheck.LEVELUP)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return false;
			}
			int level = kMyCharInfo.GetLevel();
			int num = 10;
			if (level >= num && kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ENABLE_PLUNDER) == 0L)
			{
				return true;
			}
		}
		return false;
	}

	public override string GetGameGuideText()
	{
		long num = (long)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PLUNDER_AGREE_GOLD);
		string empty = string.Empty;
		string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromToolTip,
			"gold",
			ANNUALIZED.Convert(num)
		});
		return empty;
	}
}
