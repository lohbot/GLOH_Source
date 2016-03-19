using System;

public class BABELTOWER_PERSON
{
	public long nPartyPersonID;

	public string strCharName = string.Empty;

	public int nLevel;

	public bool bReady;

	public byte nSlotType;

	public int nCharKind;

	public void Init()
	{
		this.nPartyPersonID = 0L;
		this.strCharName = string.Empty;
		this.nLevel = 0;
		this.bReady = false;
		this.nSlotType = 0;
		this.nCharKind = 0;
	}

	public void SetInfo(long nPersonID, string CharName, int Level, bool ready, byte slottype, int charkind)
	{
		this.nPartyPersonID = nPersonID;
		this.strCharName = CharName;
		this.nLevel = Level;
		this.bReady = ready;
		this.nSlotType = slottype;
		this.nCharKind = charkind;
	}
}
