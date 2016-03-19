using System;

public class NrMobileAuthSystem
{
	public bool m_RequestLogout;

	private static NrMobileAuthSystem g_instance;

	public NrMobileAuth Auth;

	public bool RequestLogout
	{
		get
		{
			return this.m_RequestLogout;
		}
		set
		{
			this.m_RequestLogout = value;
		}
	}

	public static NrMobileAuthSystem Instance
	{
		get
		{
			if (NrMobileAuthSystem.g_instance == null)
			{
				NrMobileAuthSystem.g_instance = new NrMobileAuthSystem();
				NrMobileAuthSystem.g_instance.Init();
			}
			return NrMobileAuthSystem.g_instance;
		}
	}

	private void Init()
	{
		this.Auth = new NrMobileAuthTemp();
	}
}
