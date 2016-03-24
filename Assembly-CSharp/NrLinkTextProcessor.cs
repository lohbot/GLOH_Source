using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class NrLinkTextProcessor
{
	public static void LinkTextProcessor(LinkText.TYPE linkTextType, string strText, string strTextKey, object objData)
	{
		if (linkTextType == LinkText.TYPE.ITEM)
		{
			NrLinkTextProcessor.ItemLinkFunc(objData);
		}
		else if (linkTextType == LinkText.TYPE.PLAYER)
		{
			NrLinkTextProcessor.PlayerLinkFunc(strText);
		}
		else if (linkTextType == LinkText.TYPE.NPC)
		{
			NrLinkTextProcessor.NpcLinkFunc(strTextKey, strText);
		}
		else if (linkTextType == LinkText.TYPE.MESSAGE)
		{
			NrLinkTextProcessor.PostLinkFunc();
		}
		else if (linkTextType == LinkText.TYPE.HELP)
		{
			NrLinkTextProcessor.HelpLinkFunc(strTextKey);
		}
		else if (linkTextType == LinkText.TYPE.PLUNDER_REPLAY)
		{
			NrLinkTextProcessor.PlunderReplayFunc(strText);
		}
		else if (linkTextType == LinkText.TYPE.COLOSSEUM_REPLAY)
		{
			NrLinkTextProcessor.ColosseumReplayFunc(strText);
		}
		else if (linkTextType == LinkText.TYPE.MINE_REPLAY)
		{
			NrLinkTextProcessor.MineReplayFunc(strText);
		}
		else if (linkTextType == LinkText.TYPE.INFIBATTLE_REPLAY)
		{
			NrLinkTextProcessor.InfiBattleReplayFunc(strText);
		}
		else if (linkTextType == LinkText.TYPE.COUPON)
		{
			NrLinkTextProcessor.CouponFunc(strText);
		}
		else if (linkTextType == LinkText.TYPE.TREASUREBOX)
		{
			NrLinkTextProcessor.TreasureBoxFunc(strText);
		}
		else if (linkTextType == LinkText.TYPE.GUILD)
		{
			NrLinkTextProcessor.ShowGuildInfo(strText);
		}
	}

	public static void LinkTextProcessorRightClick(LinkText.TYPE linkTextType, string strText, string strTextKey, object objData)
	{
		if (linkTextType == LinkText.TYPE.PLAYER)
		{
			int startIndex = strText.IndexOf('[') + 1;
			int num = strText.LastIndexOf(']') - 1;
			if (num < 0)
			{
				num = strText.Length;
			}
			string text = strText.Substring(startIndex, num);
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			NrPersonInfoBase personInfo = nrCharUser.GetPersonInfo();
			if (personInfo == null)
			{
				return;
			}
			string charName = personInfo.GetCharName();
			if (string.IsNullOrEmpty(charName))
			{
				return;
			}
			if (text.Equals(charName))
			{
				return;
			}
			NrTSingleton<CRightClickMenu>.Instance.CreateUI(0L, 0, text, CRightClickMenu.KIND.CHAT_USER_LINK_TEXT, CRightClickMenu.TYPE.NAME_SECTION_2, false);
		}
	}

	private static void ItemLinkFunc(object obj)
	{
		ITEM iTEM = (ITEM)obj;
		if (iTEM == null)
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.TOOLTIP_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
		}
		else if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.TOOLTIP_DLG))
		{
			Tooltip_Dlg tooltip_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TOOLTIP_DLG) as Tooltip_Dlg;
			tooltip_Dlg.Set_Tooltip(G_ID.CHAT_MAIN_DLG, iTEM);
		}
	}

	private static void PlayerLinkFunc(string strText)
	{
		if (strText.Contains("[#"))
		{
			strText = strText.Remove(0, 11);
		}
		int startIndex = strText.IndexOf('[') + 1;
		int num = strText.LastIndexOf(']') - 1;
		if (num < 0)
		{
			num = strText.Length;
		}
		string text = strText.Substring(startIndex, num);
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return;
		}
		NrPersonInfoBase personInfo = nrCharUser.GetPersonInfo();
		if (personInfo == null)
		{
			return;
		}
		string charName = personInfo.GetCharName();
		if (string.IsNullOrEmpty(charName))
		{
			return;
		}
		if (text.Equals(charName))
		{
			return;
		}
		int charKindByName = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByName(strText);
		if (0 < charKindByName)
		{
			return;
		}
		if (TsPlatform.IsMobile)
		{
			NrTSingleton<CRightClickMenu>.Instance.CreateUI(0L, 0, text, CRightClickMenu.KIND.CHAT_USER_LINK_TEXT, CRightClickMenu.TYPE.NAME_SECTION_2, false);
		}
		else
		{
			GS_WHISPER_REQ gS_WHISPER_REQ = new GS_WHISPER_REQ();
			gS_WHISPER_REQ.RoomUnique = 0;
			TKString.StringChar(text, ref gS_WHISPER_REQ.Name);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WHISPER_REQ, gS_WHISPER_REQ);
		}
	}

	private static void NpcLinkFunc(string strTextKey, string strText)
	{
		int num = 0;
		if (string.IsNullOrEmpty(strTextKey))
		{
			num = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByName(strText);
		}
		else
		{
			try
			{
				num = Convert.ToInt32(strTextKey);
			}
			catch (Exception ex)
			{
				Debug.Log(ex.ToString() + "Error : {0} is not Int32." + strTextKey);
			}
		}
		if (0 < num && 5000 > num)
		{
			NrNewOpenURL.NewOpenHelp_URL(num.ToString());
		}
	}

	private static void PostLinkFunc()
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.POST_DLG);
		PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
		if (postDlg != null)
		{
			postDlg.ChangeTab_RecvList();
		}
	}

	private static void HelpLinkFunc(string strTextKey)
	{
		if (!string.IsNullOrEmpty(strTextKey))
		{
			NrNewOpenURL.NewOpenHelp_URL(strTextKey);
		}
	}

	private static void PlunderReplayFunc(string strText)
	{
		if (strText.Contains("[#"))
		{
			strText = strText.Remove(0, 11);
		}
		string s = strText.Substring(3, strText.Length - 4);
		long num = 0L;
		if (long.TryParse(s, out num) && 0L < num && !NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
		{
			NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayHttp(num);
		}
	}

	private static void ColosseumReplayFunc(string strText)
	{
		if (strText.Contains("[#"))
		{
			strText = strText.Remove(0, 11);
		}
		string s = strText.Substring(3, strText.Length - 4);
		long num = 0L;
		if (long.TryParse(s, out num) && 0L < num && !NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
		{
			NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayColosseumHttp(num);
		}
	}

	private static void MineReplayFunc(string strText)
	{
		if (strText.Contains("[#"))
		{
			strText = strText.Remove(0, 11);
		}
		string s = strText.Substring(3, strText.Length - 4);
		long num = 0L;
		if (long.TryParse(s, out num) && 0L < num && !NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
		{
			NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayMineHttp(num);
		}
	}

	private static void CouponFunc(string strText)
	{
		if (strText.Contains("[#"))
		{
			strText = strText.Remove(0, 11);
		}
		if (strText.Contains("["))
		{
			strText = strText.Replace("[", string.Empty);
		}
		if (strText.Contains("]"))
		{
			strText = strText.Replace("]", string.Empty);
		}
		CouponDlg couponDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COUPON_DLG) as CouponDlg;
		if (couponDlg != null)
		{
			couponDlg.SetCouponText(strText);
		}
	}

	private static void InfiBattleReplayFunc(string strText)
	{
		if (strText.Contains("[#"))
		{
			strText = strText.Remove(0, 11);
		}
		string s = strText.Substring(3, strText.Length - 4);
		long num = 0L;
		if (long.TryParse(s, out num) && 0L < num && !NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
		{
			NrTSingleton<NkBattleReplayManager>.Instance.RequestReplayinfiBattleHttp(num);
		}
	}

	private static void TreasureBoxFunc(string strText)
	{
		if (strText.Contains("[#"))
		{
			strText = strText.Remove(0, 11);
		}
		if (strText.Contains("["))
		{
			strText = strText.Replace("[", string.Empty);
		}
		if (strText.Contains("]"))
		{
			strText = strText.Replace("]", string.Empty);
		}
		int i32Day = 0;
		if (!NrLinkTextProcessor.TreasureRewardTimeCheck(strText, ref i32Day))
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("632");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		short num = NrLinkTextProcessor.TreasureRewardTreasureCheck(strText);
		GS_TREASUREBOX_MOVE_REQ gS_TREASUREBOX_MOVE_REQ = new GS_TREASUREBOX_MOVE_REQ();
		if (num == -1)
		{
			gS_TREASUREBOX_MOVE_REQ.i16TreasureUnique = 0;
			gS_TREASUREBOX_MOVE_REQ.i32Day = i32Day;
		}
		else
		{
			gS_TREASUREBOX_MOVE_REQ.i16TreasureUnique = num;
			gS_TREASUREBOX_MOVE_REQ.i32Day = i32Day;
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TREASUREBOX_MOVE_REQ, gS_TREASUREBOX_MOVE_REQ);
	}

	private static void ShowGuildInfo(string strText)
	{
		if (strText.Contains("[#"))
		{
			strText = strText.Remove(0, 11);
		}
		if (strText.Contains("["))
		{
			strText = strText.Replace("[", string.Empty);
		}
		if (strText.Contains("]"))
		{
			strText = strText.Replace("]", string.Empty);
		}
		GS_NEWGUILD_DETAILINFO_REQ gS_NEWGUILD_DETAILINFO_REQ = new GS_NEWGUILD_DETAILINFO_REQ();
		gS_NEWGUILD_DETAILINFO_REQ.i64GuildID = 0L;
		TKString.StringChar(strText, ref gS_NEWGUILD_DETAILINFO_REQ.strGuildName);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_NEWGUILD_DETAILINFO_REQ, gS_NEWGUILD_DETAILINFO_REQ);
	}

	private static bool TreasureRewardTimeCheck(string strText, ref int iDay)
	{
		string value = string.Empty;
		int num = strText.IndexOf("(");
		int num2 = strText.IndexOf(".");
		int num3 = strText.IndexOf(")");
		DateTime dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
		if (num != -1 && num2 != -1 && num3 != -1)
		{
			value = strText.Substring(num + 1, num2 - (num + 1));
			int num4 = (int)Convert.ToInt16(value);
			value = strText.Substring(num2 + 1, num3 - (num2 + 1));
			iDay = (int)Convert.ToInt16(value);
			return dueDate.Day == iDay && num4 == dueDate.Month;
		}
		if (100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel)
		{
			iDay = dueDate.Day;
			return true;
		}
		return false;
	}

	private static short TreasureRewardTreasureCheck(string strText)
	{
		string value = string.Empty;
		int num = strText.IndexOf("_");
		int num2 = strText.IndexOf("\r");
		if (num != -1 && num2 != -1)
		{
			value = strText.Substring(num + 1, num2 - (num + 1));
			return Convert.ToInt16(value);
		}
		if (100 <= NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel && num == -1 && num2 == -1)
		{
			value = strText.Trim();
			return Convert.ToInt16(value);
		}
		return -1;
	}
}
