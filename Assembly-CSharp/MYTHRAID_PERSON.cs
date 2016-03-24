using System;

public class MYTHRAID_PERSON
{
	public long nPartyPersonID;

	public string strCharName = string.Empty;

	public int nLevel;

	public bool bReady;

	public byte nSlotType;

	public int nCharKind;

	public int selectedGuardianUnique = -1;

	public void Init()
	{
		this.nPartyPersonID = 0L;
		this.strCharName = string.Empty;
		this.nLevel = 0;
		this.bReady = false;
		this.nSlotType = 0;
		this.nCharKind = 0;
		this.selectedGuardianUnique = -1;
	}

	public void SetInfo(long nPersonID, string CharName, int Level, bool ready, byte slottype, int charkind, int _selectedGuardianUnique)
	{
		this.nPartyPersonID = nPersonID;
		this.strCharName = CharName;
		this.nLevel = Level;
		this.bReady = ready;
		this.nSlotType = slottype;
		this.nCharKind = charkind;
		this.selectedGuardianUnique = _selectedGuardianUnique;
	}
}
