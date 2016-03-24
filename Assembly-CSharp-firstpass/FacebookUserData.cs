using System;

public class FacebookUserData
{
	public string m_Name = string.Empty;

	public string m_GameName = string.Empty;

	public string m_ID = string.Empty;

	public bool m_Installed;

	public int Level;

	public int nFaceCharKind;

	public string m_Email = string.Empty;

	public void init()
	{
		this.m_Name = string.Empty;
		this.m_GameName = string.Empty;
		this.m_ID = string.Empty;
		this.m_Installed = false;
		this.Level = 0;
		this.nFaceCharKind = 0;
		this.m_Email = string.Empty;
	}
}
