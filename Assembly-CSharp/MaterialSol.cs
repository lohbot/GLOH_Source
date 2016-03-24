using System;

public class MaterialSol
{
	public int i32BaseCharKind;

	public byte i8BaseGrade;

	public int[] i32MaterialCharKind = new int[5];

	public byte[] i8MaterialGrade = new byte[5];

	public long i64Money;

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
		}
	}

	public void Set(int BaseCharKind, byte BaseGrade, int[] MaterialCharKind, byte[] MaterialGrade, long Money)
	{
		this.i32BaseCharKind = BaseCharKind;
		this.i8BaseGrade = BaseGrade;
		this.i64Money = Money;
		for (int i = 0; i < 5; i++)
		{
			this.i32MaterialCharKind[i] = MaterialCharKind[i];
			this.i8MaterialGrade[i] = MaterialGrade[i];
		}
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
}
