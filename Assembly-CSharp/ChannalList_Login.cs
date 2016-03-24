using PROTOCOL.LOGIN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChannalList_Login
{
	public static ChannalList_Login instance;

	private List<LS_CHANNEL_LIST_ACK.CHANNEL> m_ChannalList = new List<LS_CHANNEL_LIST_ACK.CHANNEL>();

	[MethodImpl(MethodImplOptions.Synchronized)]
	public static ChannalList_Login GetInstance()
	{
		if (ChannalList_Login.instance == null)
		{
			ChannalList_Login.instance = new ChannalList_Login();
		}
		return ChannalList_Login.instance;
	}

	public void ClearAll()
	{
		this.m_ChannalList.Clear();
	}

	public void SetInfo(LS_CHANNEL_LIST_ACK.CHANNEL _Channal)
	{
		this.m_ChannalList.Add(_Channal);
	}

	public void Show()
	{
		foreach (LS_CHANNEL_LIST_ACK.CHANNEL current in this.m_ChannalList)
		{
			MonoBehaviour.print("ID :" + current.CHID);
			MonoBehaviour.print("Name : " + current.Name);
			MonoBehaviour.print("State : " + current.State);
		}
	}

	public int GetCount()
	{
		return this.m_ChannalList.Count;
	}

	[DebuggerHidden]
	public IEnumerable GetValue()
	{
		ChannalList_Login.<GetValue>c__Iterator9 <GetValue>c__Iterator = new ChannalList_Login.<GetValue>c__Iterator9();
		<GetValue>c__Iterator.<>f__this = this;
		ChannalList_Login.<GetValue>c__Iterator9 expr_0E = <GetValue>c__Iterator;
		expr_0E.$PC = -2;
		return expr_0E;
	}
}
