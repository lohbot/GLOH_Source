using PROTOCOL;
using System;

public class NewGuildApplicant
{
	private long m_lPersonID;

	private string m_strCharName = string.Empty;

	private short m_iLevel;

	private long m_lApplicantDate;

	private int m_iFaceCharKind;

	public NewGuildApplicant(NEWGUILDMEMBER_APPLICANT_INFO NewGuildApplicantInfo)
	{
		this.m_lPersonID = NewGuildApplicantInfo.i64PersonID;
		this.m_strCharName = TKString.NEWString(NewGuildApplicantInfo.strCharName);
		this.m_iLevel = NewGuildApplicantInfo.i16Level;
		this.m_lApplicantDate = NewGuildApplicantInfo.i64ApplicantDate;
		this.m_iFaceCharKind = NewGuildApplicantInfo.i32FaceCharKind;
	}

	private void Clear()
	{
		this.m_lPersonID = 0L;
		this.m_strCharName = string.Empty;
		this.m_iLevel = 0;
		this.m_lApplicantDate = 0L;
		this.m_iFaceCharKind = 0;
	}

	public long GetPersonID()
	{
		return this.m_lPersonID;
	}

	public string GetCharName()
	{
		return this.m_strCharName;
	}

	public short GetLevel()
	{
		return this.m_iLevel;
	}

	public long GetApplicantDate()
	{
		return this.m_lApplicantDate;
	}

	public int GetFaceCharKind()
	{
		return this.m_iFaceCharKind;
	}
}
