using System;

public class NrNetworkSystem : NrTSingleton<NrNetworkSystem>
{
	private NrNetworkBase m_kNetworkComponent;

	private NrNetworkSystem()
	{
	}

	public void SetNetworkComponent(NrNetworkBase kNetworkComp)
	{
		this.m_kNetworkComponent = kNetworkComp;
	}

	public NrNetworkBase GetNetworkComponent()
	{
		return this.m_kNetworkComponent;
	}
}
