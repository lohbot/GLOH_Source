using GAME;
using System;
using System.Collections.Generic;

public class CNpcUIManager : NrTSingleton<CNpcUIManager>
{
	private Dictionary<int, CNpcUI> m_dicNPC = new Dictionary<int, CNpcUI>();

	private CNpcUIManager()
	{
	}

	public bool Initialize()
	{
		this.m_dicNPC.Clear();
		return true;
	}

	public void AddNpc(NrCharKindInfo charkindinfo)
	{
		if (charkindinfo.GetCHARKIND_NPCINFO() == null)
		{
			return;
		}
		int charKind = charkindinfo.GetCharKind();
		if (charKind <= 0)
		{
			return;
		}
		if (this.m_dicNPC.ContainsKey(charKind))
		{
			return;
		}
		if (charkindinfo.IsATB(8L))
		{
			if (charkindinfo.IsATB(2097152L))
			{
				CNpcUI cNpcUI = new CBlackSmith();
				cNpcUI.m_i32CharKind = charKind;
				this.m_dicNPC.Add(cNpcUI.m_i32CharKind, cNpcUI);
			}
			else if (charkindinfo.IsATB(512L))
			{
				if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
				{
					return;
				}
				CNpcUI cNpcUI2 = new CBuyItem();
				cNpcUI2.m_i32CharKind = charKind;
				this.m_dicNPC.Add(cNpcUI2.m_i32CharKind, cNpcUI2);
			}
			else if (charkindinfo.IsATB(536870912L))
			{
				CNpcUI cNpcUI3 = new CIndunOpen();
				cNpcUI3.m_i32CharKind = charKind;
				((CIndunOpen)cNpcUI3).SetData();
				this.m_dicNPC.Add(cNpcUI3.m_i32CharKind, cNpcUI3);
			}
			else if (charkindinfo.IsATB(4294967296L))
			{
				CNpcUI cNpcUI4 = new CGuildOpen();
				cNpcUI4.m_i32CharKind = charKind;
				((CGuildOpen)cNpcUI4).SetData();
				this.m_dicNPC.Add(cNpcUI4.m_i32CharKind, cNpcUI4);
			}
			else if (charkindinfo.IsATB(274877906944L) || charkindinfo.IsATB(1099511627776L))
			{
				CNpcUI cNpcUI5 = new CCharChange();
				cNpcUI5.m_i32CharKind = charKind;
				((CCharChange)cNpcUI5).SetData();
				this.m_dicNPC.Add(cNpcUI5.m_i32CharKind, cNpcUI5);
			}
			else if (charkindinfo.IsATB(549755813888L))
			{
				CNpcUI cNpcUI6 = new CEquipReduce();
				cNpcUI6.m_i32CharKind = charKind;
				((CEquipReduce)cNpcUI6).SetData();
				this.m_dicNPC.Add(cNpcUI6.m_i32CharKind, cNpcUI6);
			}
			else if (charkindinfo.IsATB(2199023255552L))
			{
				CNpcUI cNpcUI7 = new CItemSkill();
				cNpcUI7.m_i32CharKind = charKind;
				((CItemSkill)cNpcUI7).SetData();
				this.m_dicNPC.Add(cNpcUI7.m_i32CharKind, cNpcUI7);
			}
			else if (charkindinfo.IsATB(288230376151711744L))
			{
				CNpcUI cNpcUI8 = new CItemRepair();
				cNpcUI8.m_i32CharKind = charKind;
				((CItemRepair)cNpcUI8).SetData();
				this.m_dicNPC.Add(cNpcUI8.m_i32CharKind, cNpcUI8);
			}
			else if (charkindinfo.IsATB(4503599627370496L))
			{
				CNpcUI cNpcUI9 = new CExchangeJewelry();
				cNpcUI9.m_i32CharKind = charKind;
				this.m_dicNPC.Add(cNpcUI9.m_i32CharKind, cNpcUI9);
			}
			else if (charkindinfo.IsATB(18014398509481984L))
			{
				CNpcUI cNpcUI10 = new CAgitNPC(charKind);
				cNpcUI10.m_i32CharKind = charKind;
				this.m_dicNPC.Add(cNpcUI10.m_i32CharKind, cNpcUI10);
			}
			else if (charkindinfo.IsATB(36028797018963968L))
			{
				CNpcUI cNpcUI11 = new CExchangeMythice();
				cNpcUI11.m_i32CharKind = charKind;
				this.m_dicNPC.Add(cNpcUI11.m_i32CharKind, cNpcUI11);
			}
			else if (charkindinfo.IsATB(1152921504606846976L))
			{
				CNpcUI cNpcUI12 = new CExchangeGuildWar();
				cNpcUI12.m_i32CharKind = charKind;
				this.m_dicNPC.Add(cNpcUI12.m_i32CharKind, cNpcUI12);
			}
			else if (charkindinfo.IsATB(33554432L))
			{
				CNpcUI cNpcUI13 = new CExchangeEventItem();
				cNpcUI13.m_i32CharKind = charKind;
				this.m_dicNPC.Add(cNpcUI13.m_i32CharKind, cNpcUI13);
			}
			else
			{
				string code = charkindinfo.GetCode();
				if (charkindinfo.GetCode().Equals("TERRITORY_Custodian"))
				{
					CNpcUI cNpcUI14 = new CCustodian();
					cNpcUI14.m_i32CharKind = charKind;
					this.m_dicNPC.Add(cNpcUI14.m_i32CharKind, cNpcUI14);
				}
				else if (code.Equals("Battle_Custodian"))
				{
					CNpcUI cNpcUI15 = new CBattleCustodian();
					cNpcUI15.m_i32CharKind = charKind;
					this.m_dicNPC.Add(cNpcUI15.m_i32CharKind, cNpcUI15);
				}
				else if (code.Equals("NaGwanJoong"))
				{
					CNpcUI cNpcUI16 = new CQuestReset();
					cNpcUI16.m_i32CharKind = charKind;
					this.m_dicNPC.Add(cNpcUI16.m_i32CharKind, cNpcUI16);
				}
				else
				{
					CNpcUI cNpcUI17 = new CNpcUI();
					cNpcUI17.m_i32CharKind = charKind;
					this.m_dicNPC.Add(cNpcUI17.m_i32CharKind, cNpcUI17);
				}
			}
		}
	}

	public CNpcUI GetNpcUIByNpcKind(int i32NpcKind)
	{
		if (!this.m_dicNPC.ContainsKey(i32NpcKind))
		{
			return null;
		}
		this.m_dicNPC[i32NpcKind].InitData();
		return this.m_dicNPC[i32NpcKind];
	}

	public string GetTextGreeting(NrCharKindInfo kChar)
	{
		string strTextKey = string.Empty;
		int charKind = kChar.GetCharKind();
		if (kChar.IsATB(18014398509481984L))
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			long personID = charPersonInfo.GetPersonID();
			NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(personID);
			if (memberInfoFromPersonID == null || memberInfoFromPersonID.GetRank() <= NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_INITIATE)
			{
				if (memberInfoFromPersonID == null)
				{
					NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_INFO_REQ(0);
				}
				strTextKey = this.m_dicNPC[charKind].GetExceptionTalkTextKey();
				return NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo(strTextKey);
			}
		}
		strTextKey = kChar.GetCHARKIND_NPCINFO().GetTextGreeting();
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo(strTextKey);
	}
}
