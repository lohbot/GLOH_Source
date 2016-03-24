using NLibCs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace NNetCs
{
	internal class ParallelServer
	{
		public struct ServerInfo
		{
			public int index;

			public string serverIP;

			public int serverPORT;
		}

		private List<ParallelServer.ServerInfo> m_serverList = new List<ParallelServer.ServerInfo>();

		public int m_serverListSelectIndex;

		private int m_retryCount;

		private int m_retryCountMax = 3;

		public int ServerListCount
		{
			get
			{
				return this.m_serverList.Count;
			}
		}

		public bool LoadServerList(string url, ref string errorMessage)
		{
			bool result;
			try
			{
				IL_00:
				this.m_serverListSelectIndex = 0;
				this.m_serverList.Clear();
				if (!url.StartsWith("http"))
				{
					url = string.Format("http://{0}", url);
				}
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Timeout = 5000;
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				NDataReader nDataReader = new NDataReader();
				if (nDataReader.LoadFrom(responseStream, Encoding.UTF8))
				{
					NDataSection nDataSection = nDataReader["list"];
					int dataCount = nDataSection.DataCount;
					int[] array = new int[dataCount];
					for (int i = 0; i < dataCount; i++)
					{
						array[i] = 0;
					}
					Random random = new Random(DateTime.Now.Millisecond);
					foreach (NDataReader.Row row in nDataSection)
					{
						ParallelServer.ServerInfo item = default(ParallelServer.ServerInfo);
						item.serverIP = row.GetColumn(0);
						item.serverPORT = Convert.ToInt32(row.GetColumn(1));
						int num;
						do
						{
							num = random.Next(dataCount);
						}
						while (array[num] != 0);
						item.index = num;
						array[num] = 1;
						this.m_serverList.Add(item);
					}
					this.m_serverList.Sort((ParallelServer.ServerInfo a1, ParallelServer.ServerInfo a2) => a1.index.CompareTo(a2.index));
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				if (this.m_retryCount++ < this.m_retryCountMax)
				{
					goto IL_00;
				}
				errorMessage = ex.Message;
				result = false;
			}
			return result;
		}

		public bool hasNext()
		{
			if (this.m_serverListSelectIndex + 1 >= this.m_serverList.Count)
			{
				return false;
			}
			this.m_serverListSelectIndex++;
			return true;
		}

		public string GetIP()
		{
			if (this.m_serverListSelectIndex >= this.m_serverList.Count)
			{
				return "0.0.0.0";
			}
			return this.m_serverList[this.m_serverListSelectIndex].serverIP;
		}

		public int GetPORT()
		{
			if (this.m_serverListSelectIndex >= this.m_serverList.Count)
			{
				return 0;
			}
			return this.m_serverList[this.m_serverListSelectIndex].serverPORT;
		}
	}
}
