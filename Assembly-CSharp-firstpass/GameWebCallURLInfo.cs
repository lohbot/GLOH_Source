using System;
using TsLibs;

public class GameWebCallURLInfo : NrTableData
{
	public eGameWebCallURL m_eType;

	public string m_stWebcallUrl;

	public GameWebCallURLInfo()
	{
		this.Init();
	}

	public virtual void Init()
	{
		this.m_eType = eGameWebCallURL.DEFAULT;
		this.m_stWebcallUrl = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		string empty = string.Empty;
		row.GetColumn(num++, out empty);
		if (Enum.IsDefined(typeof(eGameWebCallURL), empty))
		{
			this.m_eType = (eGameWebCallURL)((int)Enum.Parse(typeof(eGameWebCallURL), empty));
		}
		empty = string.Empty;
		row.GetColumn(num++, out this.m_stWebcallUrl);
	}
}
