using System;
using TsLibs;

public class CHARKIND_SOLDIERINFO : NrTableData
{
	public string CharCode = string.Empty;

	public byte bBaseRank;

	public long i64NeedMoney;

	public ELEMENT_CHAR[] kElement_CharData = new ELEMENT_CHAR[5];

	public CHARKIND_SOLDIERINFO() : base(NrTableData.eResourceType.eRT_CHARKIND_SOLDIERINFO)
	{
		for (int i = 0; i < 5; i++)
		{
			this.kElement_CharData[i] = new ELEMENT_CHAR();
		}
		this.Init();
	}

	public void Init()
	{
		this.CharCode = string.Empty;
		this.bBaseRank = 0;
		for (int i = 0; i < 5; i++)
		{
			this.kElement_CharData[i].InitData();
		}
		this.i64NeedMoney = 0L;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out this.CharCode);
		row.GetColumn(num++, out empty);
		row.GetColumn(num++, out this.bBaseRank);
		row.GetColumn(num++, out this.kElement_CharData[0].Element_CharCode);
		row.GetColumn(num++, out this.kElement_CharData[0].Element_Rank);
		row.GetColumn(num++, out this.kElement_CharData[1].Element_CharCode);
		row.GetColumn(num++, out this.kElement_CharData[1].Element_Rank);
		row.GetColumn(num++, out this.kElement_CharData[2].Element_CharCode);
		row.GetColumn(num++, out this.kElement_CharData[2].Element_Rank);
		row.GetColumn(num++, out this.kElement_CharData[3].Element_CharCode);
		row.GetColumn(num++, out this.kElement_CharData[3].Element_Rank);
		row.GetColumn(num++, out this.kElement_CharData[4].Element_CharCode);
		row.GetColumn(num++, out this.kElement_CharData[4].Element_Rank);
		row.GetColumn(num++, out this.i64NeedMoney);
	}

	public void GetCharData(byte bCharIndex, ref int i32CharKind, ref byte bCharRank)
	{
		if (bCharIndex < 0 || bCharIndex > 5)
		{
			i32CharKind = 0;
			bCharRank = 0;
		}
		else
		{
			i32CharKind = this.kElement_CharData[(int)bCharIndex].GetCharCharKind();
			bCharRank = this.kElement_CharData[(int)bCharIndex].GetCharCharRank();
		}
	}

	public byte GetBaseCharRank()
	{
		if (this.bBaseRank < 0)
		{
			return 0;
		}
		return this.bBaseRank;
	}

	public long GetCharMoney()
	{
		if (this.i64NeedMoney < 0L)
		{
			return 0L;
		}
		return this.i64NeedMoney;
	}
}
