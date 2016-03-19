using GAME;
using System;

public class NrPersonInfoBattle : NrPersonInfoBase
{
	protected NrCharBasePart m_kCharBasicPart;

	public NrPersonInfoBattle()
	{
		this.m_kCharBasicPart = new NrCharBasePart();
		this.Init();
	}

	public override void Init()
	{
		base.Init();
		this.m_kCharBasicPart.Init();
	}

	public void SetBasePart(NrCharBasePart pkBasicPart)
	{
		this.m_kCharBasicPart = pkBasicPart;
	}

	public NrCharBasePart GetBasePart()
	{
		return this.m_kCharBasicPart;
	}

	public override void SetPersonInfo(NrPersonInfoBase pkPersonInfo)
	{
		base.SetPersonInfo(pkPersonInfo);
		NrPersonInfoBattle nrPersonInfoBattle = pkPersonInfo as NrPersonInfoBattle;
		this.m_kCharBasicPart.SetData(nrPersonInfoBattle.GetBasePart());
	}
}
