using System;
using System.Collections.Generic;

public class NrCharAniInfoManager : NrTSingleton<NrCharAniInfoManager>
{
	private Dictionary<string, NkCharAniInfo> m_kCharAniInfoDic;

	private NrCharDataCodeInfo m_kCharDataCodeInfo;

	private NrCharAniInfoManager()
	{
		this.m_kCharAniInfoDic = new Dictionary<string, NkCharAniInfo>();
		this.m_kCharDataCodeInfo = new NrCharDataCodeInfo();
		this.m_kCharDataCodeInfo.LoadDataCode();
	}

	public float GetAnimationEvent(string bundleName, int weaponkey, int anitype, int eventype)
	{
		NkCharAniInfo charAniInfo = this.GetCharAniInfo(bundleName);
		if (charAniInfo == null)
		{
			return 1f;
		}
		return charAniInfo.GetAnimationEvent(weaponkey, anitype, eventype);
	}

	public int GetHitAniCount(string bundleName, int weaponkey, int anitype)
	{
		NkCharAniInfo charAniInfo = this.GetCharAniInfo(bundleName);
		if (charAniInfo == null)
		{
			return -1;
		}
		return charAniInfo.GetHitAniCount(weaponkey, anitype);
	}

	public void SetAniInfo(ref CHARKIND_ANIINFO aniinfo)
	{
		int weaponkey = 0;
		NrCharKindInfo charKindInfoFromBundleName = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromBundleName(aniinfo.BUNDLENAME);
		if (charKindInfoFromBundleName != null && charKindInfoFromBundleName.IsATB(1L))
		{
			int weaponType = NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponType(aniinfo.WEAPONTYPE);
			if (weaponType != charKindInfoFromBundleName.GetWeaponType())
			{
				weaponkey = 1;
			}
		}
		int charAniTypeForEvent = (int)this.m_kCharDataCodeInfo.GetCharAniTypeForEvent(aniinfo.ANITYPE);
		int charAniEvent = (int)this.m_kCharDataCodeInfo.GetCharAniEvent(aniinfo.EVENTTYPE);
		NkCharAniInfo charAniInfo = this.GetCharAniInfo(aniinfo.BUNDLENAME);
		if (charAniInfo == null)
		{
			return;
		}
		charAniInfo.SetAniEventTime(weaponkey, charAniTypeForEvent, charAniEvent, aniinfo.EVENTTIME);
	}

	private NkCharAniInfo GetCharAniInfo(string bundleName)
	{
		if (!this.m_kCharAniInfoDic.ContainsKey(bundleName))
		{
			this.m_kCharAniInfoDic.Add(bundleName, new NkCharAniInfo());
		}
		return this.m_kCharAniInfoDic[bundleName];
	}
}
