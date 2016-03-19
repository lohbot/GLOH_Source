using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameGuideRecommendSol : GameGuideInfo
{
	private NkSoldierInfo m_kSolInfo;

	private int CompareExp(NkSoldierInfo a, NkSoldierInfo b)
	{
		return b.GetExp().CompareTo(a.GetExp());
	}

	public override void Init()
	{
		base.Init();
		this.m_kSolInfo = null;
	}

	public override void ExcuteGameGuide()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			if (soldierInfo != null)
			{
				if (soldierInfo.GetSolID() == 0L)
				{
					if (myCharInfo.IsAddBattleSoldier(i))
					{
						num = i;
						break;
					}
				}
			}
		}
		if (num == -1)
		{
			return;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return;
		}
		if (1 > readySolList.GetCount())
		{
			return;
		}
		List<NkSoldierInfo> list = new List<NkSoldierInfo>(readySolList.GetList().Values);
		if (list == null)
		{
			return;
		}
		list.Sort(new Comparison<NkSoldierInfo>(this.CompareExp));
		for (int j = 0; j < list.Count; j++)
		{
			NkSoldierInfo nkSoldierInfo = list[j];
			if (nkSoldierInfo != null)
			{
				NrCharKindInfo charKindInfo = nkSoldierInfo.GetCharKindInfo();
				if (charKindInfo != null)
				{
					if (charKindInfo.GetCHARKIND_CLASSINFO() != null)
					{
						int num2 = 0;
						for (int k = 0; k < 6; k++)
						{
							NkSoldierInfo soldierInfo2 = charPersonInfo.GetSoldierInfo(k);
							if (soldierInfo2 != null)
							{
								if (soldierInfo2.GetSolID() != 0L)
								{
									if (soldierInfo2.GetCharKind() == nkSoldierInfo.GetCharKind())
									{
										num2++;
									}
								}
							}
						}
						if (num2 < (int)nkSoldierInfo.GetJoinCount())
						{
							this.SendSolChangeToServer(ref nkSoldierInfo, 1, 1);
							return;
						}
					}
				}
			}
		}
	}

	private void SendSolChangeToServer(ref NkSoldierInfo pkSolinfo, byte solpostype, byte militaryunique)
	{
		GS_SOLDIER_CHANGE_POSTYPE_REQ gS_SOLDIER_CHANGE_POSTYPE_REQ = new GS_SOLDIER_CHANGE_POSTYPE_REQ();
		gS_SOLDIER_CHANGE_POSTYPE_REQ.SolID = pkSolinfo.GetSolID();
		gS_SOLDIER_CHANGE_POSTYPE_REQ.SolPosType = solpostype;
		gS_SOLDIER_CHANGE_POSTYPE_REQ.MilitaryUnique = militaryunique;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_CHANGE_POSTYPE_REQ, gS_SOLDIER_CHANGE_POSTYPE_REQ);
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return false;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return false;
		}
		bool flag = false;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			if (soldierInfo != null)
			{
				if (soldierInfo.GetSolID() == 0L)
				{
					if (myCharInfo.IsAddBattleSoldier(i))
					{
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			return false;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		if (readySolList == null)
		{
			return false;
		}
		if (1 > readySolList.GetCount())
		{
			return false;
		}
		List<NkSoldierInfo> list = new List<NkSoldierInfo>(readySolList.GetList().Values);
		if (list == null)
		{
			return false;
		}
		list.Sort(new Comparison<NkSoldierInfo>(this.CompareExp));
		for (int j = 0; j < list.Count; j++)
		{
			NkSoldierInfo nkSoldierInfo = list[j];
			if (nkSoldierInfo != null)
			{
				NrCharKindInfo charKindInfo = nkSoldierInfo.GetCharKindInfo();
				if (charKindInfo != null)
				{
					if (charKindInfo.GetCHARKIND_CLASSINFO() != null)
					{
						if (nkSoldierInfo.GetSolPosType() != 2 && nkSoldierInfo.GetSolPosType() != 6)
						{
							int num = 0;
							for (int k = 0; k < 6; k++)
							{
								NkSoldierInfo soldierInfo2 = charPersonInfo.GetSoldierInfo(k);
								if (soldierInfo2 != null)
								{
									if (soldierInfo2.GetSolID() != 0L)
									{
										if (soldierInfo2.GetCharKind() == nkSoldierInfo.GetCharKind())
										{
											num++;
										}
									}
								}
							}
							if (num < (int)nkSoldierInfo.GetJoinCount())
							{
								this.m_kSolInfo = nkSoldierInfo;
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	public override bool CheckGameGuide()
	{
		if (Time.realtimeSinceStartup - this.m_nCheckTime > this.m_nDelayTime)
		{
			this.m_nCheckTime = Time.realtimeSinceStartup;
			return this.CheckGameGuideOnce();
		}
		return false;
	}

	public override string GetGameGuideText()
	{
		if (this.m_kSolInfo == null)
		{
			return string.Empty;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey),
			"count",
			this.m_kSolInfo.GetLevel(),
			"solname",
			this.m_kSolInfo.GetName()
		});
		return empty;
	}
}
