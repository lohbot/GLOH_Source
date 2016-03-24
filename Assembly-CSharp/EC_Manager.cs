using NLibCs.ErrorCollector;
using System;
using UnityEngine;

public class EC_Manager : MonoBehaviour
{
	private bool firstInit = true;

	private static GameObject m_GameObj;

	private static EC_Manager Inst;

	public static EC_Manager Instance
	{
		get
		{
			if (EC_Manager.Inst == null)
			{
				EC_Manager.m_GameObj = new GameObject("EC_Manager");
				EC_Manager.Inst = EC_Manager.m_GameObj.AddComponent<EC_Manager>();
				UnityEngine.Object.DontDestroyOnLoad(EC_Manager.m_GameObj);
			}
			return EC_Manager.Inst;
		}
	}

	private void Awake()
	{
	}

	public void Init(string server_IP, int server_PORT, ErrorCollectorHandler handler, string local_Location)
	{
		Connector.Start("ErrorCollector", "ErrorCollector", server_IP, server_PORT, handler, local_Location, false, false, true);
	}

	public void Init(string url_IP, ErrorCollectorHandler handler, string local_Location)
	{
		Connector.Start("ErrorCollector", "ErrorCollector", handler, local_Location, url_IP, false, false, true);
	}

	private void Update()
	{
		if (Connector.Instance != null)
		{
			if (this.firstInit)
			{
				Connector.Instance.Send_Off();
				this.firstInit = false;
			}
			Connector.Instance.RecvUpdate();
			Connector.Instance.SendUpdate();
		}
	}
}
