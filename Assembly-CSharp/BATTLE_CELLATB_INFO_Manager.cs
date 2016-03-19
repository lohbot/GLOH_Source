using System;
using System.Collections;
using TsLibs;

public class BATTLE_CELLATB_INFO_Manager : NrTableBase
{
	public BATTLE_CELLATB_INFO m_kCellATBInfo;

	public BATTLE_CELLATB_INFO_Manager(string strFilePath) : base(strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		IEnumerator enumerator = dr.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				TsDataReader.Row data = (TsDataReader.Row)enumerator.Current;
				this.m_kCellATBInfo = new BATTLE_CELLATB_INFO();
				this.m_kCellATBInfo.SetData(data);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return true;
	}
}
