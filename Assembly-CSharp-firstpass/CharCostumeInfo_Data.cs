using System;
using TsLibs;
using UnityEngine;

public class CharCostumeInfo_Data : NrTableData
{
	public int m_costumeUnique;

	public string m_CharCode = string.Empty;

	public string m_CostumeTextKey = string.Empty;

	public string m_PortraitPath = string.Empty;

	public string m_BundlePath = string.Empty;

	public int m_SkillUnique;

	public int m_ATKBonusRate;

	public int m_DefBonusRate;

	public int m_HPBonusRate;

	public int m_CostumeType;

	public int m_PresentItemUnique;

	public bool m_Event;

	public bool m_New;

	public float m_Scale = 1f;

	public float m_MoveX;

	public float m_MoveY;

	public float m_Light = 1f;

	public int m_Price1Num;

	public int m_Price2Num;

	public override void SetData(TsDataReader.Row row)
	{
		this.SetRowDataToInfoData(row, 0, out this.m_costumeUnique);
		this.SetRowDataToInfoData(row, 1, out this.m_CharCode);
		this.SetRowDataToInfoData(row, 2, out this.m_CostumeTextKey);
		this.SetRowDataToInfoData(row, 3, out this.m_PortraitPath);
		this.SetRowDataToInfoData(row, 4, out this.m_BundlePath);
		this.SetRowDataToInfoData(row, 5, out this.m_SkillUnique);
		this.SetRowDataToInfoData(row, 6, out this.m_ATKBonusRate);
		this.SetRowDataToInfoData(row, 7, out this.m_DefBonusRate);
		this.SetRowDataToInfoData(row, 8, out this.m_HPBonusRate);
		this.SetRowDataToInfoData(row, 9, out this.m_CostumeType);
		this.SetRowDataToInfoData(row, 10, out this.m_PresentItemUnique);
		this.SetRowDataToInfoData(row, 11, out this.m_Event);
		this.SetRowDataToInfoData(row, 12, out this.m_New);
		this.SetRowDataToInfoData(row, 13, out this.m_Scale);
		this.SetRowDataToInfoData(row, 14, out this.m_MoveX);
		this.SetRowDataToInfoData(row, 15, out this.m_MoveY);
		this.SetRowDataToInfoData(row, 16, out this.m_Light);
		this.SetRowDataToInfoData(row, 17, out this.m_Price1Num);
		this.SetRowDataToInfoData(row, 18, out this.m_Price2Num);
	}

	public bool IsNormalCostume()
	{
		return this.m_CostumeType == 0;
	}

	private void SetRowDataToInfoData(TsDataReader.Row row, int dataIdx, out int infoData)
	{
		if (row == null)
		{
			Debug.LogError("Error, CharCostumeInfo_Data.cs. SetRowDataToInfoData(), row is null");
			infoData = -1;
			return;
		}
		if (!row.GetColumn(dataIdx, out infoData))
		{
			Debug.LogError("Error, CharCostumeInfo_Data.cs. SetRowDataToInfoData(), row.GetColumn Fail. dataIdx : " + dataIdx);
			return;
		}
	}

	private void SetRowDataToInfoData(TsDataReader.Row row, int dataIdx, out string infoData)
	{
		if (row == null)
		{
			Debug.LogError("Error, CharCostumeInfo_Data.cs. SetRowDataToInfoData(), row is null");
			infoData = string.Empty;
			return;
		}
		if (!row.GetColumn(dataIdx, out infoData))
		{
			Debug.LogError("Error, CharCostumeInfo_Data.cs. SetRowDataToInfoData(), row.GetColumn Fail. dataIdx : " + dataIdx);
			return;
		}
	}

	private void SetRowDataToInfoData(TsDataReader.Row row, int dataIdx, out bool infoData)
	{
		if (row == null)
		{
			Debug.LogError("Error, CharCostumeInfo_Data.cs. SetRowDataToInfoData(), row is null");
			infoData = false;
			return;
		}
		if (!row.GetColumn(dataIdx, out infoData))
		{
			Debug.LogError("Error, CharCostumeInfo_Data.cs. SetRowDataToInfoData(), row.GetColumn Fail. dataIdx : " + dataIdx);
			return;
		}
	}

	private void SetRowDataToInfoData(TsDataReader.Row row, int dataIdx, out float infoData)
	{
		if (row == null)
		{
			Debug.LogError("Error, CharCostumeInfo_Data.cs. SetRowDataToInfoData(), row is null");
			infoData = 1f;
			return;
		}
		if (!row.GetColumn(dataIdx, out infoData))
		{
			Debug.LogError("Error, CharCostumeInfo_Data.cs. SetRowDataToInfoData(), row.GetColumn Fail. dataIdx : " + dataIdx);
			return;
		}
	}

	private void DebugLog()
	{
		Debug.LogError(string.Concat(new object[]
		{
			this.m_costumeUnique,
			"\t",
			this.m_CharCode,
			"\t",
			this.m_CostumeTextKey,
			"\t",
			this.m_PortraitPath,
			"\t",
			this.m_BundlePath,
			"\t",
			this.m_SkillUnique,
			"\t",
			this.m_ATKBonusRate,
			"\t",
			this.m_DefBonusRate,
			"\t",
			this.m_HPBonusRate,
			"\t"
		}));
	}
}
