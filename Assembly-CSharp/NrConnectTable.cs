using System;
using System.Collections.Generic;
using TsLibs;

public class NrConnectTable
{
	public List<NrServerInfo> m_arServerInfo = new List<NrServerInfo>();

	public static NrConnectTable Instance
	{
		get;
		set;
	}

	public NrConnectTable()
	{
		NrConnectTable.Instance = this;
	}

	public void AddServerList(string strFileName)
	{
		using (TsDataReader tsDataReader = new TsDataReader())
		{
			if (tsDataReader.Load(strFileName))
			{
				if (tsDataReader.BeginSection("[Table]"))
				{
					foreach (TsDataReader.Row data in tsDataReader)
					{
						NrServerInfo nrServerInfo = new NrServerInfo();
						nrServerInfo.SetData(data);
						this.m_arServerInfo.Add(nrServerInfo);
					}
				}
			}
			else
			{
				TsLog.LogError("Not ServerList~!!", new object[0]);
			}
		}
	}
}
