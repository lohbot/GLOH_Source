using System;
using TsLibs;

public class ITEM_ARMOR : NrTableData
{
	public int ITEMUNIQUE;

	public string TYPECODE = string.Empty;

	public string TEXTKEY = string.Empty;

	public string ENG_NAME = string.Empty;

	public string ONLYUSE = string.Empty;

	public string ATTACHPART = string.Empty;

	public string ATB = string.Empty;

	public int USE_MINLV;

	public int USE_MAXLV;

	public int DEF;

	public int MAGICDEF;

	public int ADDHP;

	public short STR;

	public short DEX;

	public short INT;

	public short VIT;

	public int CRITICALPLUS;

	public int ATTACKSPEED;

	public int HITRATEPLUS;

	public int EVASIONPLUS;

	public int DURABILITY;

	public int PRUDUCT_ID;

	public int USEDATE;

	public string TEXTKEY_TOOLTIP = string.Empty;

	public int SORT_ORDER;

	public int QUALITY_LEVEL;

	public int GROUP;

	public long PRICE;

	public string MATERIALCODE = string.Empty;

	public string[] m_strOnlyUseCharCode = new string[10];

	public string m_strIconFile = string.Empty;

	public short m_shIconIndex;

	public int nNextLevel;

	public int nSetUnique;

	public byte nStarGrade;

	public ITEM_ARMOR() : base(NrTableData.eResourceType.eRT_ITEM_ARMOR)
	{
		this.Init();
	}

	public void Init()
	{
		this.ITEMUNIQUE = 0;
		this.TYPECODE = string.Empty;
		this.TEXTKEY = string.Empty;
		this.ENG_NAME = string.Empty;
		this.ONLYUSE = string.Empty;
		this.ATTACHPART = string.Empty;
		this.ATB = string.Empty;
		this.USE_MINLV = 0;
		this.USE_MAXLV = 0;
		this.DEF = 0;
		this.MAGICDEF = 0;
		this.ADDHP = 0;
		this.STR = 0;
		this.DEX = 0;
		this.INT = 0;
		this.VIT = 0;
		this.CRITICALPLUS = 0;
		this.ATTACKSPEED = 0;
		this.HITRATEPLUS = 0;
		this.EVASIONPLUS = 0;
		this.DURABILITY = 0;
		this.PRUDUCT_ID = 0;
		this.USEDATE = 0;
		this.TEXTKEY_TOOLTIP = string.Empty;
		this.SORT_ORDER = 0;
		this.QUALITY_LEVEL = 0;
		this.GROUP = 0;
		this.PRICE = 0L;
		this.MATERIALCODE = string.Empty;
		for (int i = 0; i < 10; i++)
		{
			this.m_strOnlyUseCharCode[i] = string.Empty;
		}
		this.m_strIconFile = string.Empty;
		this.m_shIconIndex = 0;
		this.nNextLevel = 0;
		this.nSetUnique = 0;
		this.nStarGrade = 0;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.ITEMUNIQUE);
		row.GetColumn(num++, out this.TYPECODE);
		row.GetColumn(num++, out this.TEXTKEY);
		row.GetColumn(num++, out this.ENG_NAME);
		row.GetColumn(num++, out this.ONLYUSE);
		row.GetColumn(num++, out this.ATTACHPART);
		row.GetColumn(num++, out this.ATB);
		row.GetColumn(num++, out this.USE_MINLV);
		row.GetColumn(num++, out this.USE_MAXLV);
		row.GetColumn(num++, out this.DEF);
		row.GetColumn(num++, out this.MAGICDEF);
		row.GetColumn(num++, out this.ADDHP);
		row.GetColumn(num++, out this.STR);
		row.GetColumn(num++, out this.DEX);
		row.GetColumn(num++, out this.INT);
		row.GetColumn(num++, out this.VIT);
		row.GetColumn(num++, out this.CRITICALPLUS);
		row.GetColumn(num++, out this.ATTACKSPEED);
		row.GetColumn(num++, out this.HITRATEPLUS);
		row.GetColumn(num++, out this.EVASIONPLUS);
		row.GetColumn(num++, out this.DURABILITY);
		row.GetColumn(num++, out this.PRUDUCT_ID);
		row.GetColumn(num++, out this.USEDATE);
		row.GetColumn(num++, out this.TEXTKEY_TOOLTIP);
		row.GetColumn(num++, out this.SORT_ORDER);
		row.GetColumn(num++, out this.QUALITY_LEVEL);
		row.GetColumn(num++, out this.GROUP);
		row.GetColumn(num++, out this.PRICE);
		row.GetColumn(num++, out this.MATERIALCODE);
		row.GetColumn(num++, out this.m_strIconFile);
		row.GetColumn(num++, out this.m_shIconIndex);
		row.GetColumn(num++, out this.nNextLevel);
		row.GetColumn(num++, out this.nSetUnique);
		row.GetColumn(num++, out this.nStarGrade);
		this.ParseCharcode(this.ONLYUSE);
	}

	public void ParseCharcode(string strCharCode)
	{
		if (strCharCode == null)
		{
			return;
		}
		string text = string.Empty;
		int num = 0;
		int num2 = 0;
		int i;
		for (i = 0; i < strCharCode.Length; i++)
		{
			char c = strCharCode[i];
			if (c != ' ')
			{
				if (c == '+')
				{
					text = strCharCode.Substring(num, i - num);
					this.m_strOnlyUseCharCode[num2] = text;
					num2++;
					num = i + 1;
				}
			}
		}
		if (i > num + 1)
		{
			text = strCharCode.Substring(num, i - num);
			this.m_strOnlyUseCharCode[num2] = text;
		}
	}
}
