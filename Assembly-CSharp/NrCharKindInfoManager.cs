using GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NrCharKindInfoManager : NrTSingleton<NrCharKindInfoManager>
{
	private int m_nTotalCharKindInfoNum;

	public NrCharKindInfo[] m_kCharKindInfo;

	private NrCharKindTempData m_kTempData;

	private NrCharDataCodeInfo m_kCharDataCodeInfo;

	private Dictionary<string, int> m_dicCharCodeInfo;

	private Dictionary<string, int> m_dicCharNameInfo;

	private Dictionary<string, int> m_dicBundleNameInfo;

	private Dictionary<string, long> m_dicClassTypeCodeInfo;

	private Dictionary<string, int> m_dicAttackTypeCodeInfo;

	private Dictionary<string, NkCharAniInfo> m_kCharAniInfoList;

	private Dictionary<string, NkCharAniMapInfo> m_kCharAniMapInfoList;

	private List<string> m_kPlayerCodeList;

	private NrCharKindInfoManager()
	{
		this.m_kCharKindInfo = new NrCharKindInfo[5000];
		this.m_kTempData = new NrCharKindTempData();
		this.m_kCharDataCodeInfo = new NrCharDataCodeInfo();
		this.m_kCharDataCodeInfo.LoadDataCode();
		this.m_dicCharCodeInfo = new Dictionary<string, int>();
		this.m_dicCharNameInfo = new Dictionary<string, int>();
		this.m_dicBundleNameInfo = new Dictionary<string, int>();
		this.m_dicClassTypeCodeInfo = new Dictionary<string, long>();
		this.m_dicAttackTypeCodeInfo = new Dictionary<string, int>();
		this.m_kCharAniInfoList = new Dictionary<string, NkCharAniInfo>();
		this.m_kCharAniMapInfoList = new Dictionary<string, NkCharAniMapInfo>();
		this.m_kPlayerCodeList = new List<string>();
	}

	public NrCharDataCodeInfo GetCharDataCodeInfo()
	{
		return this.m_kCharDataCodeInfo;
	}

	public byte ParseCharTribeCode(string chartribecode)
	{
		byte b = 0;
		if (chartribecode == null)
		{
			return 0;
		}
		string text = string.Empty;
		int num = 0;
		int i;
		for (i = 0; i < chartribecode.Length; i++)
		{
			char c = chartribecode[i];
			if (c != ' ')
			{
				if (c == '+')
				{
					text = chartribecode.Substring(num, i - num);
					b |= this.m_kCharDataCodeInfo.GetCharTribe(text.Trim());
					num = i + 1;
				}
			}
		}
		if (i > num + 1)
		{
			text = chartribecode.Substring(num, i - num);
			b |= this.m_kCharDataCodeInfo.GetCharTribe(text.Trim());
		}
		return b;
	}

	public void SetClassTypeCodeInfo(string classtypecode, long classtype)
	{
		if (this.m_dicClassTypeCodeInfo.ContainsKey(classtypecode))
		{
			Debug.LogWarning("SetClassTypeCodeInfo dictionary same key = " + classtypecode);
			return;
		}
		this.m_dicClassTypeCodeInfo.Add(classtypecode, classtype);
	}

	public long GetClassType(string classtypecode)
	{
		if (!this.m_dicClassTypeCodeInfo.ContainsKey(classtypecode))
		{
			return 0L;
		}
		return this.m_dicClassTypeCodeInfo[classtypecode];
	}

	public long ParseClassTypeCode(string classtypecode)
	{
		long num = 0L;
		if (classtypecode == null)
		{
			return 0L;
		}
		string text = string.Empty;
		int num2 = 0;
		int i;
		for (i = 0; i < classtypecode.Length; i++)
		{
			char c = classtypecode[i];
			if (c != ' ')
			{
				if (c == '+')
				{
					text = classtypecode.Substring(num2, i - num2);
					long classType = this.GetClassType(text.Trim());
					num |= classType;
					num2 = i + 1;
				}
			}
		}
		if (i > num2 + 1)
		{
			text = classtypecode.Substring(num2, i - num2);
			long classType = this.GetClassType(text.Trim());
			num |= classType;
		}
		return num;
	}

	public void SetAttackTypeCodeInfo(int attacktype, string attacktypecode)
	{
		this.m_dicAttackTypeCodeInfo.Add(attacktypecode, attacktype);
	}

	public int GetAttackType(string attacktypecode)
	{
		if (!this.m_dicAttackTypeCodeInfo.ContainsKey(attacktypecode))
		{
			return 0;
		}
		return this.m_dicAttackTypeCodeInfo[attacktypecode];
	}

	public bool SetCharKindInfo(ref CHARKIND_INFO pkCHARKIND_INFO)
	{
		if (pkCHARKIND_INFO == null)
		{
			return false;
		}
		int cHARKIND = pkCHARKIND_INFO.CHARKIND;
		if (cHARKIND <= 0 || cHARKIND >= 5000)
		{
			return false;
		}
		this.m_kCharKindInfo[cHARKIND] = new NrCharKindInfo(this.m_kTempData);
		this.m_kCharKindInfo[cHARKIND].SetCharKindInfo(ref pkCHARKIND_INFO, this.m_kCharDataCodeInfo.GetCharTribe(pkCHARKIND_INFO.CharTribe));
		string text = this.m_kCharKindInfo[cHARKIND].GetCode();
		text = text.ToLower();
		if (!this.m_dicCharCodeInfo.ContainsKey(text))
		{
			this.m_dicCharCodeInfo.Add(text, cHARKIND);
		}
		string name = this.m_kCharKindInfo[cHARKIND].GetName();
		if (!this.m_dicCharNameInfo.ContainsKey(name))
		{
			this.m_dicCharNameInfo.Add(name, cHARKIND);
		}
		string bundleName = this.m_kCharKindInfo[cHARKIND].GetBundleName();
		if (!this.m_dicBundleNameInfo.ContainsKey(bundleName))
		{
			this.m_dicBundleNameInfo.Add(bundleName, cHARKIND);
		}
		string aniBundleName = this.m_kCharKindInfo[cHARKIND].GetAniBundleName();
		if (!this.m_kCharAniMapInfoList.ContainsKey(aniBundleName))
		{
			NkCharAniMapInfo nkCharAniMapInfo = new NkCharAniMapInfo();
			nkCharAniMapInfo.AddCharKind(cHARKIND);
			this.m_kCharAniMapInfoList.Add(aniBundleName, nkCharAniMapInfo);
		}
		else
		{
			NkCharAniMapInfo nkCharAniMapInfo = this.m_kCharAniMapInfoList[aniBundleName];
			nkCharAniMapInfo.AddCharKind(cHARKIND);
		}
		if (this.m_kCharKindInfo[cHARKIND].IsATB(1L))
		{
			this.AddPlayerCode(this.m_kCharKindInfo[cHARKIND].GetBundlePath());
		}
		this.m_nTotalCharKindInfoNum++;
		return true;
	}

	public int GetCharKindByCode(string charcode)
	{
		if (charcode.Equals("NULL"))
		{
			return 0;
		}
		string key = charcode.ToLower();
		if (!this.m_dicCharCodeInfo.ContainsKey(key))
		{
			return 0;
		}
		return this.m_dicCharCodeInfo[key];
	}

	public int GetCharKindByName(string charname)
	{
		if (!this.m_dicCharNameInfo.ContainsKey(charname))
		{
			return 0;
		}
		return this.m_dicCharNameInfo[charname];
	}

	public int GetCharKindByBundleName(string bundlename)
	{
		if (!this.m_dicBundleNameInfo.ContainsKey(bundlename))
		{
			return 0;
		}
		return this.m_dicBundleNameInfo[bundlename];
	}

	public NkCharAniMapInfo GetCharAniMapInfo(string bundlename)
	{
		if (!this.m_kCharAniMapInfoList.ContainsKey(bundlename))
		{
			return null;
		}
		return this.m_kCharAniMapInfoList[bundlename];
	}

	public NrCharKindInfo GetCharKindInfo(int charkind)
	{
		if (charkind <= 0 || charkind >= 5000)
		{
			return null;
		}
		return this.m_kCharKindInfo[charkind];
	}

	public NrCharKindInfo GetCharKindInfoFromCode(string charcode)
	{
		int charKindByCode = this.GetCharKindByCode(charcode);
		return this.GetCharKindInfo(charKindByCode);
	}

	public NrCharKindInfo GetCharKindInfoFromBundleName(string bundlename)
	{
		int charKindByBundleName = this.GetCharKindByBundleName(bundlename);
		return this.GetCharKindInfo(charKindByBundleName);
	}

	public void SetStatInfo(int charkind, ref CHARKIND_STATINFO statinfo)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(charkind);
		if (charKindInfo != null)
		{
			charKindInfo.SetStatInfo(ref statinfo);
		}
	}

	public void SetMonsterInfo(int charkind, ref CHARKIND_MONSTERINFO monsterinfo)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(charkind);
		if (charKindInfo != null)
		{
			charKindInfo.SetMonsterInfo(ref monsterinfo);
			int num = NkUtil.MakeLong(monsterinfo.MonType, (long)monsterinfo.MINLEVEL);
			CHARKIND_MONSTATINFO charKindMonStatInfo = NrTSingleton<NrBaseTableManager>.Instance.GetCharKindMonStatInfo(num.ToString());
			if (monsterinfo == null)
			{
				Debug.LogWarning("CHARKIND_MONSTATINFO setting error!! = " + charKindInfo.GetName());
				return;
			}
			charKindInfo.SetMonStatInfo(ref charKindMonStatInfo);
		}
	}

	public void SetNPCInfo(int charkind, ref CHARKIND_NPCINFO npcinfo)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(charkind);
		if (charKindInfo != null)
		{
			charKindInfo.SetNPCInfo(ref npcinfo);
		}
	}

	public void SetAniInfo(ref CHARKIND_ANIINFO aniinfo)
	{
		int weaponkey = 0;
		NrCharKindInfo nrCharKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromBundleName(aniinfo.BUNDLENAME);
		if (nrCharKindInfo != null && nrCharKindInfo.IsATB(1L))
		{
			int weaponType = NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponType(aniinfo.WEAPONTYPE);
			if (weaponType != nrCharKindInfo.GetWeaponType())
			{
				weaponkey = 1;
			}
		}
		int charAniTypeForEvent = (int)this.m_kCharDataCodeInfo.GetCharAniTypeForEvent(aniinfo.ANITYPE);
		int charAniEvent = (int)this.m_kCharDataCodeInfo.GetCharAniEvent(aniinfo.EVENTTYPE);
		NkCharAniInfo charAniInfo = this.GetCharAniInfo(aniinfo.BUNDLENAME);
		if (charAniInfo != null)
		{
			charAniInfo.SetAniEventTime(weaponkey, charAniTypeForEvent, charAniEvent, aniinfo.EVENTTIME);
			NkCharAniMapInfo charAniMapInfo = this.GetCharAniMapInfo(aniinfo.BUNDLENAME);
			if (charAniMapInfo != null)
			{
				List<int> charKindList = charAniMapInfo.GetCharKindList();
				foreach (int current in charKindList)
				{
					nrCharKindInfo = this.GetCharKindInfo(current);
					if (nrCharKindInfo != null)
					{
						nrCharKindInfo.SetAniInfo(ref charAniInfo);
					}
				}
			}
		}
	}

	public void SetSoldierInfo(int charkind, ref CHARKIND_SOLDIERINFO solinfo)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(charkind);
		if (charKindInfo != null)
		{
			charKindInfo.SetSoldierInfo(ref solinfo);
		}
	}

	public void SetSolGradeInfo(int charkind, ref BASE_SOLGRADEINFO gradeinfo)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(charkind);
		if (charKindInfo != null)
		{
			charKindInfo.SetSolGradeInfo(ref gradeinfo);
		}
	}

	public CHARKIND_INFO GetBaseCharKindInfo(int charkind)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(charkind);
		if (charKindInfo == null)
		{
			return null;
		}
		return charKindInfo.GetCHARKIND_INFO();
	}

	public CHARKIND_INFO GetBaseCharKindInfo(string charcode)
	{
		int charKindByCode = this.GetCharKindByCode(charcode);
		return this.GetBaseCharKindInfo(charKindByCode);
	}

	public CHARKIND_STATINFO GetBaseStatInfo(int charkind)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(charkind);
		if (charKindInfo == null)
		{
			return null;
		}
		return charKindInfo.GetCHARKIND_STATINFO();
	}

	public CHARKIND_MONSTERINFO GetBaseMonsterInfo(int charkind)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(charkind);
		if (charKindInfo == null)
		{
			return null;
		}
		return charKindInfo.GetCHARKIND_MONSTERINFO();
	}

	public CHARKIND_NPCINFO GetBaseNPCInfo(int charkind)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(charkind);
		if (charKindInfo == null)
		{
			return null;
		}
		return charKindInfo.GetCHARKIND_NPCINFO();
	}

	public NkCharAniInfo GetCharAniInfo(string bundlename)
	{
		if (!this.m_kCharAniInfoList.ContainsKey(bundlename))
		{
			NkCharAniInfo value = new NkCharAniInfo();
			this.m_kCharAniInfoList.Add(bundlename, value);
		}
		NkCharAniInfo result = null;
		if (!this.m_kCharAniInfoList.TryGetValue(bundlename, out result))
		{
			return null;
		}
		return result;
	}

	public UIBaseInfoLoader GetLegendFrame(int kind, int solgrade)
	{
		if (solgrade < 0 || solgrade >= 15)
		{
			return null;
		}
		if (solgrade > 5 && solgrade < 10)
		{
			return NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_LegendFrame");
		}
		if (solgrade > 9)
		{
			return NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_MythFrame");
		}
		return null;
	}

	public short GetLegendType(int kind, int solgrade)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(kind);
		if (charKindInfo == null)
		{
			return 0;
		}
		return charKindInfo.GetLegendType(solgrade);
	}

	public string GetLegendName(int kind, int solgrade, string name)
	{
		string result = name;
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(kind);
		if (charKindInfo == null)
		{
			return name;
		}
		short legendType = charKindInfo.GetLegendType(solgrade);
		if (legendType == 1)
		{
			result = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1107"), name);
		}
		else if (legendType == 2)
		{
			result = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1106"), name);
		}
		return result;
	}

	public UIBaseInfoLoader GetSolGradeImg(int kind, int solgrade)
	{
		string key = string.Empty;
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(kind);
		if (charKindInfo == null)
		{
			return null;
		}
		eLEGENDTYPE legendType = (eLEGENDTYPE)charKindInfo.GetLegendType(solgrade);
		if (legendType == eLEGENDTYPE.LEGEND || legendType == eLEGENDTYPE.MYTHOLOGY)
		{
			if (solgrade == 10 || solgrade == 6)
			{
				key = "Win_I_LankOrbA";
			}
			else if (solgrade == 11 || solgrade == 7)
			{
				key = "Win_I_LankOrbS";
			}
			else if (solgrade == 12 || solgrade == 8)
			{
				key = "Win_I_LankOrbSS";
			}
			else if (solgrade == 13 || solgrade == 9)
			{
				key = "Win_I_LankOrbEX";
			}
		}
		else
		{
			key = "Win_I_WorrGradeS" + (solgrade + 1).ToString();
		}
		return NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(key);
	}

	public UIBaseInfoLoader GetSolLargeGradeImg(int kind, int solgrade)
	{
		string key = string.Empty;
		if (this.GetCharKindInfo(kind) == null)
		{
			return null;
		}
		if (solgrade == 6)
		{
			key = "Win_I_LegendRankA";
		}
		else if (solgrade == 7)
		{
			key = "Win_I_LegendRankS";
		}
		else if (solgrade == 8)
		{
			key = "Win_I_LegendRankSS";
		}
		else if (solgrade == 9)
		{
			key = "Win_I_LegendRankEX";
		}
		else if (solgrade == 10)
		{
			key = "Win_I_MythRankA";
		}
		else if (solgrade == 11)
		{
			key = "Win_I_MythRankS";
		}
		else if (solgrade == 12)
		{
			key = "Win_I_MythRankSS";
		}
		else if (solgrade == 13)
		{
			key = "Win_I_MythRankEX";
		}
		else
		{
			key = "Win_I_WorrGradeS" + (solgrade + 1).ToString();
		}
		return NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(key);
	}

	public UIBaseInfoLoader GetSolRankImg(int solgrade)
	{
		string key = "Win_I_WorrGradeS";
		return NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(key);
	}

	public UIBaseInfoLoader GetSolRankBackImg(int charkind)
	{
		if (this.GetCharKindInfo(charkind) == null)
		{
			return null;
		}
		string key = "Win_I_FrameS";
		return NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(key);
	}

	public long GetSolEvolutionNeedEXP(int solkind, int solgrade)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(solkind);
		if (charKindInfo == null)
		{
			return 0L;
		}
		return charKindInfo.GetEvolutionNeedExp(solgrade);
	}

	public void AddPlayerCode(string charcode)
	{
		if (this.m_kPlayerCodeList.Contains(charcode))
		{
			return;
		}
		this.m_kPlayerCodeList.Add(charcode);
	}

	public List<string> GetPlayerCodeList()
	{
		if (this.m_kPlayerCodeList.Count == 0)
		{
			return null;
		}
		return this.m_kPlayerCodeList;
	}

	public string GetName(int charKind)
	{
		NrCharKindInfo charKindInfo = this.GetCharKindInfo(charKind);
		if (charKindInfo == null)
		{
			return string.Empty;
		}
		return charKindInfo.GetName();
	}

	public bool IsUserCharKind(int iCharKind)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(iCharKind);
		return this.IsUserCharKind(charKindInfo);
	}

	public bool IsUserCharKind(NrCharKindInfo CharKindInfo)
	{
		return CharKindInfo != null && CharKindInfo.IsATB(1L);
	}

	public int GetCharKindbyMythSkillUnique(int iCharKind, int i32Idx)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(iCharKind);
		return charKindInfo.GetMythBattleSkillUniqueByIndex(i32Idx);
	}

	public int GetCharKindbyMythSkillUniqueMaxCount(int iCharKind, int i32Idx)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(iCharKind);
		return charKindInfo.GetMythBattleSkillUniqueMaxCount();
	}
}
