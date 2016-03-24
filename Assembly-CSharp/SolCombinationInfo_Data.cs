using System;
using System.Collections.Generic;
using TsLibs;
using UnityEngine;

public class SolCombinationInfo_Data : NrTableData
{
	public int m_nCombinationUnique;

	public string m_CombinationName = string.Empty;

	public int m_nCombinationGrade;

	public int m_nCombinationSkillUnique;

	public int m_nCombinationSkillLevel;

	public string m_textTitleKey = string.Empty;

	public string m_textDetailKey = string.Empty;

	public int m_nCombinationIsShow;

	public string[] m_szCombinationIsCharCode;

	public int m_sortNum;

	public SolCombinationInfo_Data()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_szCombinationIsCharCode = new string[5];
	}

	public override void SetData(TsDataReader.Row row)
	{
		string empty = string.Empty;
		this.SetRowDataToInfoData(row, 0, out this.m_nCombinationUnique);
		this.SetRowDataToInfoData(row, 1, out this.m_CombinationName);
		this.SetRowDataToInfoData(row, 2, out this.m_nCombinationGrade);
		this.SetRowDataToInfoData(row, 3, out this.m_nCombinationSkillUnique);
		this.SetRowDataToInfoData(row, 4, out this.m_nCombinationSkillLevel);
		this.SetRowDataToInfoData(row, 5, out this.m_textTitleKey);
		this.SetRowDataToInfoData(row, 6, out this.m_textDetailKey);
		this.SetRowDataToInfoData(row, 7, out this.m_nCombinationIsShow);
		this.SetRowDataToInfoData(row, 8, out empty);
		int num = 9;
		for (int i = 0; i < 5; i++)
		{
			this.SetRowDataToInfoData(row, num + i, out this.m_szCombinationIsCharCode[i]);
		}
		this.SetRowDataToInfoData(row, 15, out this.m_sortNum);
	}

	public int GetCombinationSolCount()
	{
		if (this.m_szCombinationIsCharCode == null || this.m_szCombinationIsCharCode.Length == 0)
		{
			return 0;
		}
		int num = 0;
		string[] szCombinationIsCharCode = this.m_szCombinationIsCharCode;
		for (int i = 0; i < szCombinationIsCharCode.Length; i++)
		{
			string text = szCombinationIsCharCode[i];
			if (!string.IsNullOrEmpty(text))
			{
				if (!(text == "0"))
				{
					num++;
				}
			}
		}
		return num;
	}

	public List<string> GetSolCombinationCharCodeList()
	{
		if (this.m_szCombinationIsCharCode == null || this.m_szCombinationIsCharCode.Length == 0)
		{
			return null;
		}
		List<string> list = new List<string>();
		string[] szCombinationIsCharCode = this.m_szCombinationIsCharCode;
		for (int i = 0; i < szCombinationIsCharCode.Length; i++)
		{
			string text = szCombinationIsCharCode[i];
			if (!string.IsNullOrEmpty(text))
			{
				if (!(text == "0"))
				{
					list.Add(text);
				}
			}
		}
		return list;
	}

	private void SetRowDataToInfoData(TsDataReader.Row row, int dataIdx, out string infoData)
	{
		if (row == null)
		{
			Debug.LogError("Error, SolCombinationInfo_Data.cs. SetRowDataToInfoData(), row is null");
			infoData = string.Empty;
			return;
		}
		if (!row.GetColumn(dataIdx, out infoData))
		{
			Debug.LogError("Error, SolCombinationInfo_Data.cs. SetRowDataToInfoData(), row.GetColumn Fail. dataIdx : " + dataIdx);
			return;
		}
	}

	private void SetRowDataToInfoData(TsDataReader.Row row, int dataIdx, out int infoData)
	{
		if (row == null)
		{
			Debug.LogError("Error, SolCombinationInfo_Data.cs. SetRowDataToInfoData(), row is null");
			infoData = -1;
			return;
		}
		if (!row.GetColumn(dataIdx, out infoData))
		{
			Debug.LogError("Error, SolCombinationInfo_Data.cs. SetRowDataToInfoData(), row.GetColumn Fail. dataIdx : " + dataIdx);
			infoData = -1;
			return;
		}
	}
}
