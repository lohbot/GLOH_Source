using System;

public class MaterialSol
{
	public int i32BaseCharKind;

	public byte i8BaseGrade;

	public int[] i32MaterialCharKind = new int[5];

	public byte[] i8MaterialGrade = new byte[5];

	public long i64Money;

	public byte i8LegendBaseGrade;

	public int[] i32LegendMaterialCharKind = new int[5];

	public byte[] i8LegendMaterialGrade = new byte[5];

	public long i64LegendMoney;

	public int i32LegendItemUnique;

	public int i32LegendEssencs;

	public MaterialSol()
	{
		this.Init();
	}

	public void Init()
	{
		this.i32BaseCharKind = 0;
		this.i8BaseGrade = 0;
		this.i64Money = 0L;
		for (int i = 0; i < 5; i++)
		{
			this.i32MaterialCharKind[i] = 0;
			this.i8MaterialGrade[i] = 0;
			this.i32LegendMaterialCharKind[i] = 0;
			this.i8LegendMaterialGrade[i] = 0;
		}
		this.i8LegendBaseGrade = 0;
		this.i64LegendMoney = 0L;
		this.i32LegendItemUnique = 0;
		this.i32LegendEssencs = 0;
	}

	public void Set(int BaseCharKind, byte BaseGrade, int[] MaterialCharKind, byte[] MaterialGrade, long Money, byte LegendBaseGrade, int[] LegendMaterialCharKind, byte[] LegendMaterialGrade, long LegendMoney, int LegendItemUnique, int LegendEssencs)
	{
		this.i32BaseCharKind = BaseCharKind;
		this.i8BaseGrade = BaseGrade;
		this.i64Money = Money;
		for (int i = 0; i < 5; i++)
		{
			this.i32MaterialCharKind[i] = MaterialCharKind[i];
			this.i8MaterialGrade[i] = MaterialGrade[i];
			this.i32LegendMaterialCharKind[i] = LegendMaterialCharKind[i];
			this.i8LegendMaterialGrade[i] = LegendMaterialGrade[i];
		}
		this.i8LegendBaseGrade = LegendBaseGrade;
		this.i64LegendMoney = LegendMoney;
		this.i32LegendItemUnique = LegendItemUnique;
		this.i32LegendEssencs = LegendEssencs;
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
			i32CharKind = this.i32MaterialCharKind[(int)bCharIndex];
			bCharRank = this.i8MaterialGrade[(int)bCharIndex];
		}
	}

	public void GetLegendCharData(byte bCharIndex, ref int i32CharKind, ref byte bCharRank)
	{
		if (bCharIndex < 0 || bCharIndex > 5)
		{
			i32CharKind = 0;
			bCharRank = 0;
		}
		else
		{
			i32CharKind = this.i32LegendMaterialCharKind[(int)bCharIndex];
			bCharRank = this.i8LegendMaterialGrade[(int)bCharIndex];
		}
	}
}
