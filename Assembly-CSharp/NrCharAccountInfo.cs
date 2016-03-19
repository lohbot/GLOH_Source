using System;
using System.Collections.Generic;

public class NrCharAccountInfo
{
	public string m_szUserID = string.Empty;

	public string m_szPassword = string.Empty;

	public int m_siAuthSessionKey;

	public long m_i64AccountWorldInfoKey;

	public long m_UID;

	public string m_szAuthKey = string.Empty;

	public long m_nSerialNumber;

	private List<int> m_kSlotCharIDList = new List<int>();

	private int m_nCharUniqueNum;

	public int m_nMasterLevel;

	public int m_nConfirmCheck;

	public int m_nGetConfirmItem;

	public long m_nChatBlockDate;

	public NrCharAccountInfo()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_szUserID = string.Empty;
		this.m_szPassword = string.Empty;
		this.m_siAuthSessionKey = 0;
		this.m_UID = 0L;
		this.m_szAuthKey = string.Empty;
		this.m_nSerialNumber = 0L;
		this.m_kSlotCharIDList.Clear();
		this.m_nCharUniqueNum = 0;
		this.m_i64AccountWorldInfoKey = 0L;
		this.m_nConfirmCheck = 0;
		this.m_nGetConfirmItem = 0;
		this.m_nChatBlockDate = 0L;
	}

	public void InitSlotChar()
	{
		this.m_kSlotCharIDList.Clear();
		this.m_nCharUniqueNum = 0;
	}

	public void AddSlotChar(int charid)
	{
		if (this.GetCharListNum() >= 3)
		{
			return;
		}
		this.m_kSlotCharIDList.Add(charid);
		this.m_nCharUniqueNum++;
	}

	public void DelSlotChar(int charid)
	{
		this.m_kSlotCharIDList.Remove(charid);
	}

	public int GetCharListNum()
	{
		return this.m_kSlotCharIDList.Count;
	}

	public int EnableSlot()
	{
		return this.m_kSlotCharIDList.Count + 1;
	}

	public int FindSlot(int charid)
	{
		return this.m_kSlotCharIDList.IndexOf(charid) + 1;
	}

	public int GetCharID(int slotindex)
	{
		int num = slotindex - 1;
		if (num < 0 || num >= this.GetCharListNum())
		{
			return -1;
		}
		return this.m_kSlotCharIDList[num];
	}

	public int GetCharUniqueNum()
	{
		return this.m_nCharUniqueNum;
	}

	public bool IsMaster()
	{
		return 100 <= this.m_nMasterLevel;
	}
}
