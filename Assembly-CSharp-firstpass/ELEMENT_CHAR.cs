using System;

public class ELEMENT_CHAR
{
	public string Element_CharCode = string.Empty;

	public byte Element_Rank;

	private int Element_CharKind;

	public void InitData()
	{
		this.Element_CharCode = string.Empty;
		this.Element_Rank = 0;
		this.Element_CharKind = 0;
	}

	public void SetChar(int nCharKind)
	{
		this.Element_CharKind = nCharKind;
	}

	public int GetCharCharKind()
	{
		return this.Element_CharKind;
	}

	public byte GetCharCharRank()
	{
		return this.Element_Rank;
	}
}
