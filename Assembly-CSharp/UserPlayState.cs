using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;

public class UserPlayState : MonoBehaviour
{
	private int i32CheckTime;

	private int i32MemCheckTime;

	private bool m_bUserPlayState = true;

	protected int i32PlayStateChangeTIme = 300000;

	protected int i32MemChaeckPeriodTIme = 60000;

	public bool bUserPlayState
	{
		get
		{
			return this.m_bUserPlayState;
		}
		set
		{
			this.m_bUserPlayState = value;
		}
	}

	private void Start()
	{
		this.i32CheckTime = Environment.TickCount;
	}

	private void Update()
	{
		int tickCount = Environment.TickCount;
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (NkInputManager.anyKey)
		{
			this.i32CheckTime = tickCount;
		}
		else if (@char != null && @char.IsAutoMove())
		{
			this.i32CheckTime = tickCount;
		}
		if (tickCount > this.i32CheckTime + this.i32PlayStateChangeTIme)
		{
			this.SetChangeUserPlayState(false);
			if (tickCount > this.i32MemCheckTime + this.i32MemChaeckPeriodTIme)
			{
				NrTSingleton<NrMainSystem>.Instance.MemoryCleanUP();
				this.i32MemCheckTime = tickCount;
			}
		}
		else
		{
			this.SetChangeUserPlayState(true);
		}
	}

	private void SetChangeUserPlayState(bool bState)
	{
		if (bState != this.bUserPlayState)
		{
			this.bUserPlayState = bState;
			this.ServerRequest_UserPlayState();
		}
	}

	public void ServerRequest_UserPlayState()
	{
		GS_SET_USERPLAYSTATE_CHANGE_REQ gS_SET_USERPLAYSTATE_CHANGE_REQ = new GS_SET_USERPLAYSTATE_CHANGE_REQ();
		byte i8UserPlayState = (!this.m_bUserPlayState) ? 0 : 1;
		gS_SET_USERPLAYSTATE_CHANGE_REQ.i8UserPlayState = i8UserPlayState;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SET_USERPLAYSTATE_CHANGE_REQ, gS_SET_USERPLAYSTATE_CHANGE_REQ);
	}
}
