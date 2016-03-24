using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class CostumeBuyManager : NrTSingleton<CostumeBuyManager>
{
	private CostumeBuyManager()
	{
	}

	public void BuyCostume(CharCostumeInfo_Data costumeData, string giftTargetName)
	{
		if (costumeData == null)
		{
			return;
		}
		Debug.Log("Costume Buy Try");
		GS_COSTUME_BUY_REQ gS_COSTUME_BUY_REQ = new GS_COSTUME_BUY_REQ();
		gS_COSTUME_BUY_REQ.i32CostumeUnique = costumeData.m_costumeUnique;
		if (!string.IsNullOrEmpty(giftTargetName))
		{
			TKString.StringChar(giftTargetName, ref gS_COSTUME_BUY_REQ.strGiftUserName);
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COSTUME_BUY_REQ, gS_COSTUME_BUY_REQ);
	}

	public void BuyCostumeEnd(GS_COSTUME_BUY_ACK ack)
	{
		if (ack == null)
		{
			return;
		}
		if (ack.bIsGift)
		{
			this.ShowCostumeGiftSuccessMessage(ack);
		}
		else
		{
			this.UpdateMyCostumeInfo(ack);
		}
	}

	public void BuyCostumeByMsgBox(CharCostumeInfo_Data costumeData)
	{
		if (costumeData == null)
		{
			return;
		}
		if (this.IsCostumeKindSolNotExist(costumeData))
		{
			this.ShowReConfirmMsgBox(costumeData, "355");
		}
		else if (this.IsCostumeAlreadyBuy(costumeData))
		{
			this.ShowReConfirmMsgBox(costumeData, "356");
		}
		else
		{
			this.ShowRealBuyCostumeMsgBox(costumeData);
		}
	}

	private void ShowRealBuyCostumeMsgBox(object data)
	{
		if (data == null)
		{
			return;
		}
		CharCostumeInfo_Data charCostumeInfo_Data = (CharCostumeInfo_Data)data;
		if (charCostumeInfo_Data == null)
		{
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("78"),
			"targetname",
			NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeName(charCostumeInfo_Data.m_costumeUnique)
		});
		CostumeBuyMsgBox_Dlg costumeBuyMsgBox_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COSTUME_BUY_MSG_BOX) as CostumeBuyMsgBox_Dlg;
		if (costumeBuyMsgBox_Dlg == null)
		{
			return;
		}
		costumeBuyMsgBox_Dlg.ShowMsgBox(new YesDelegate(this.OnClickCostumeBuyButton), charCostumeInfo_Data, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty);
	}

	private void OnClickCostumeBuyButton(object EventObject)
	{
		CharCostumeInfo_Data charCostumeInfo_Data = (CharCostumeInfo_Data)EventObject;
		if (charCostumeInfo_Data == null)
		{
			return;
		}
		this.BuyCostume(charCostumeInfo_Data, string.Empty);
	}

	private void UpdateMyCostumeInfo(GS_COSTUME_BUY_ACK ack)
	{
		if (ack == null)
		{
			return;
		}
		this.ShowBuyCostumeMessage(ack.i32CostumeUnique);
		NrTSingleton<NrCharCostumeTableManager>.Instance.UpdateCostumeCount(ack.i32CostumeUnique, 1, 1);
		CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
		if (costumeRoom_Dlg != null)
		{
			costumeRoom_Dlg.Refresh(true, true);
		}
		Debug.Log("BuyCostumeUnique : " + ack.i32CostumeUnique);
	}

	private void ShowBuyCostumeMessage(int costumeUnique)
	{
		string text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("861");
		text = text.Replace("@costumename@", NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeName(costumeUnique));
		Main_UI_SystemMessage.ADDMessage(text);
	}

	private void ShowCostumeGiftSuccessMessage(GS_COSTUME_BUY_ACK ack)
	{
		if (ack == null)
		{
			return;
		}
		string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("706");
		string text = TKString.NEWString(ack.strGiftUserName);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromNotify, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("706"),
			"Product",
			NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeName(ack.i32CostumeUnique),
			"targetname",
			text
		});
		Main_UI_SystemMessage.ADDMessage(textFromNotify);
	}

	private bool IsCostumeKindSolNotExist(CharCostumeInfo_Data costumeData)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return true;
		}
		List<int> ownAllSolKindList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetOwnAllSolKindList();
		if (ownAllSolKindList == null)
		{
			return true;
		}
		int charKindByCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(costumeData.m_CharCode);
		foreach (int current in ownAllSolKindList)
		{
			if (current == charKindByCode)
			{
				return false;
			}
		}
		return true;
	}

	private bool IsCostumeAlreadyBuy(CharCostumeInfo_Data costumeData)
	{
		if (costumeData == null)
		{
			return false;
		}
		COSTUME_INFO costumeInfo = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeInfo(costumeData.m_costumeUnique);
		return costumeInfo != null && costumeInfo.i32CostumeCount > 0;
	}

	private void ShowReConfirmMsgBox(CharCostumeInfo_Data costumeData, string msgKey)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox(msgKey)
		});
		msgBoxUI.SetMsg(new YesDelegate(this.ShowRealBuyCostumeMsgBox), costumeData, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("845"), empty, eMsgType.MB_OK_CANCEL, 2);
	}
}
