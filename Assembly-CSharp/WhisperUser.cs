using System;

public class WhisperUser
{
	public long PersonID;

	public string Name = string.Empty;

	public int nFaceKind;

	public byte byPlayState;

	public WhisperUser(long personid, string name, byte Playstate, int FaceKind)
	{
		this.PersonID = personid;
		this.Name = name;
		this.byPlayState = Playstate;
		this.nFaceKind = FaceKind;
	}
}
