using GameMessage;
using PROTOCOL;
using PROTOCOL.LOGIN;
using System;
using UnityEngine;
using UnityForms;

public class NrReceiveLogin
{
	public static void Recv_LS_ENCRYPTKEY_NFY(NkDeserializePacket kDeserializePacket)
	{
		LS_ENCRYPTKEY_NFY packet = kDeserializePacket.GetPacket<LS_ENCRYPTKEY_NFY>();
		SendPacket.GetInstance().SetEncryptKey(packet.ui8Key1_send, packet.ui8Key2_send, false);
		TsLog.LogWarning("Recv_LS_ENCRYPTKEY_NFY", new object[0]);
		if (TsPlatform.IsEditor)
		{
			MsgHandler.Handle("LOGIN_USER_REQ", new object[0]);
		}
		else
		{
			MsgHandler.Handle("PLATFORM_LOGIN_REQ", new object[0]);
		}
	}

	public static void Recv_LS_LOGIN_USER_ACK(NkDeserializePacket kDeserializePacket)
	{
		LS_LOGIN_USER_ACK packet = kDeserializePacket.GetPacket<LS_LOGIN_USER_ACK>();
		if (packet.Result != 0)
		{
			string message = string.Empty;
			if (packet.Result == 2)
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("Message5");
			}
			else
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("Message2");
			}
			Main_UI_SystemMessage.ADDMessage(message);
			TsLog.LogError("LoginFailed = {0}", new object[]
			{
				(eRESULT)packet.Result
			});
			BaseNet_Login.GetInstance().Quit();
			return;
		}
		NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey = TKString.NEWString(packet.szAuthKey);
		NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nSerialNumber = packet.nSerialNumber;
		Debug.LogWarning("Logged in from login server.");
		MsgHandler.Handle("Rcv_LOGIN_USER_ACK", new object[0]);
	}

	public static void Recv_LS_CHANNEL_LIST_ACK(NkDeserializePacket kDeserializePacket)
	{
		LS_CHANNEL_LIST_ACK packet = kDeserializePacket.GetPacket<LS_CHANNEL_LIST_ACK>();
		ChannalList_Login.GetInstance().ClearAll();
		for (byte b = 0; b < packet.NumChannels; b += 1)
		{
			LS_CHANNEL_LIST_ACK.CHANNEL packet2 = kDeserializePacket.GetPacket<LS_CHANNEL_LIST_ACK.CHANNEL>();
			ChannalList_Login.GetInstance().SetInfo(packet2);
		}
	}

	public static void Recv_LS_CHANNEL_INFO_ACK(NkDeserializePacket kDeserializePacket)
	{
		LS_CHANNEL_INFO_ACK packet = kDeserializePacket.GetPacket<LS_CHANNEL_INFO_ACK>();
		SendPacket.GetInstance().SendIDType(2097156);
		string ip = TKString.NEWString(packet.IP);
		ushort port = packet.Port;
		NrTSingleton<NrNetProcess>.Instance.RequestToGameServer(ip, (int)port);
	}

	public static void Recv_LS_LOGOUT_USER_ACK(NkDeserializePacket kDeserializePacket)
	{
	}

	public static void Recv_LS_PLATFORM_LOGIN_ACK(NkDeserializePacket kDeserializePacket)
	{
		LS_PLATFORM_LOGIN_ACK packet = kDeserializePacket.GetPacket<LS_PLATFORM_LOGIN_ACK>();
		string text = TKString.NEWString(packet.szAuthKey);
		if (text.Equals("over"))
		{
			string textFromPreloadText = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("Message4");
			Main_UI_SystemMessage.ADDMessage(textFromPreloadText);
			BaseNet_Login.GetInstance().Quit();
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.LOGIN_SELECT_PLATFORM_DLG);
			return;
		}
		if (packet.Result != 0 || packet.nSerialNumber == 0L)
		{
			string message = string.Empty;
			if (packet.Result == 2)
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("Message5");
			}
			else if (packet.Result == -2)
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("35");
			}
			else
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("Message3");
			}
			if (packet.Result == 2 || !NrTSingleton<NrMainSystem>.Instance.m_bIsAutoLogin || packet.Result == -2)
			{
				Main_UI_SystemMessage.ADDMessage(message);
			}
			TsLog.LogError("LoginFailed = {0}", new object[]
			{
				(eRESULT)packet.Result
			});
			BaseNet_Login.GetInstance().Quit();
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.LOGIN_SELECT_PLATFORM_DLG);
			return;
		}
		NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey = text;
		PlayerPrefs.SetString(NrPrefsKey.PLAYER_PREFS_MOBILEAUTHKEY, NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey);
		NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nSerialNumber = packet.nSerialNumber;
		PlayerPrefs.SetInt(NrPrefsKey.LAST_AUTH_PLATFORM, NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType());
		NrTSingleton<NrMainSystem>.Instance.m_bIsAutoLogin = false;
		Debug.LogWarning("Logged in from login server.");
		MsgHandler.Handle("Recv_LS_PLATFORM_LOGIN_ACK", new object[0]);
	}
}
