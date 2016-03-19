using System;
using TsLibs;

public class BATTLE_EMOTICON : NrTableData
{
	public eBATTLE_EMOTICON m_eConstant = eBATTLE_EMOTICON.eBATTLE_EMOTICON_MAX;

	public string m_szTextKey = string.Empty;

	public string m_szTexture = string.Empty;

	public string m_szEffect = string.Empty;

	public string strConstant = string.Empty;

	public void Init()
	{
		this.m_eConstant = eBATTLE_EMOTICON.eBATTLE_EMOTICON_MAX;
		this.m_szTextKey = string.Empty;
		this.m_szTexture = string.Empty;
		this.m_szEffect = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.strConstant);
		row.GetColumn(num++, out this.m_szTextKey);
		row.GetColumn(num++, out this.m_szTexture);
		row.GetColumn(num++, out this.m_szEffect);
	}
}
