using PROTOCOL;
using PROTOCOL.WORLD;
using System;

public class NrNetworkCustomizing : NrNetworkBase
{
	public void SendPacket_CheckUserName(string strUserName)
	{
		WS_NAME_DUPLICATION_CHECK_REQ wS_NAME_DUPLICATION_CHECK_REQ = new WS_NAME_DUPLICATION_CHECK_REQ();
		TKString.StringChar(strUserName, ref wS_NAME_DUPLICATION_CHECK_REQ.szCharName);
		SendPacket.GetInstance().SendObject(16777250, wS_NAME_DUPLICATION_CHECK_REQ);
	}

	public void SendPacket_CreateAvatar(NrCharUser kCharAvatar, string strSupporterName)
	{
		WS_CREATE_CHAR_REQ wS_CREATE_CHAR_REQ = new WS_CREATE_CHAR_REQ();
		NrPersonInfoUser nrPersonInfoUser = kCharAvatar.GetPersonInfo() as NrPersonInfoUser;
		if (nrPersonInfoUser == null)
		{
			return;
		}
		TKString.StringChar(nrPersonInfoUser.GetCharName(), ref wS_CREATE_CHAR_REQ.szCharName);
		TKString.StringChar(strSupporterName, ref wS_CREATE_CHAR_REQ.szSupporterName);
		wS_CREATE_CHAR_REQ.i32CharKind = nrPersonInfoUser.GetKind(0);
		wS_CREATE_CHAR_REQ.kBasePart.SetData(nrPersonInfoUser.GetBasePart());
		SendPacket.GetInstance().SendObject(16777252, wS_CREATE_CHAR_REQ);
	}

	public void SendPacket_DeleteAvatar(long snPersonID)
	{
		WS_DELETE_CHAR_REQ wS_DELETE_CHAR_REQ = new WS_DELETE_CHAR_REQ();
		wS_DELETE_CHAR_REQ.nPersonID = snPersonID;
		SendPacket.GetInstance().SendObject(16777254, wS_DELETE_CHAR_REQ);
	}

	public void SendPacket_RequestChannelList()
	{
		SendPacket.GetInstance().SendIDType(2097158);
	}
}
