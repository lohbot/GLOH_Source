using PROTOCOL;
using System;
using UnityEngine;

public class NrNetProcess : NrTSingleton<NrNetProcess>
{
	private NrNetProcess()
	{
	}

	public void RequestToGameServer(string ip, int port)
	{
		try
		{
			BaseNet_Login.GetInstance().Quit();
			if (BaseNet_Game.GetInstance().ConnectGameServer(ip, port))
			{
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
		}
	}
}
