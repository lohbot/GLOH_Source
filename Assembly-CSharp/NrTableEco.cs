using System;
using TsLibs;
using UnityEngine;

public class NrTableEco : NrTableBase
{
	public NrTableEco(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			ECO eCO = new ECO();
			eCO.SetData(data);
			eCO.i64ATB = NrTSingleton<NkATB_Manager>.Instance.ParseECOATB(eCO.strATB);
			if (!NrTSingleton<NrBaseTableManager>.Instance.SetData(eCO))
			{
				Debug.LogError("XML Parsing Error! - " + this.m_strFilePath);
			}
			NrTSingleton<NkQuestManager>.Instance.AddQuestAutoPath(eCO);
			NrTSingleton<NrNpcPosManager>.Instance.AddNpcPos(eCO);
		}
		return true;
	}
}
