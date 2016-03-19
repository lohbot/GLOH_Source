using System;
using System.Net;

namespace TsPatch
{
	public class Util
	{
		public static int GetContentLengthURL(string requestUrlString)
		{
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrlString);
				httpWebRequest.Method = "HEAD";
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				int num = 0;
				if (int.TryParse(httpWebResponse.Headers.Get("Content-Length"), out num))
				{
					Console.WriteLine("size !!! {0}", num);
				}
				httpWebResponse.Close();
				return num;
			}
			catch (WebException ex)
			{
				string text = "TsPatch.Util!!! 에러!!!\n\nException Message :" + ex;
				if (ex.Status == WebExceptionStatus.ProtocolError)
				{
					text = text + "Status Code : + " + ((HttpWebResponse)ex.Response).StatusCode;
					text = text + "Status Description : " + ((HttpWebResponse)ex.Response).StatusDescription;
				}
				Util._OutputDebug(text);
			}
			catch (Exception ex2)
			{
				Util._OutputDebug(ex2.ToString());
			}
			return -1;
		}

		public static bool IsVaildURL(string requestUrlString)
		{
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrlString);
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				httpWebResponse.Close();
				return true;
			}
			catch (WebException ex)
			{
				string text = "TsPatch.Util!!! 에러!!!\n\nException Message :" + ex.Message;
				if (ex.Status == WebExceptionStatus.ProtocolError)
				{
					text = text + "Status Code : + " + ((HttpWebResponse)ex.Response).StatusCode;
					text = text + "Status Description : " + ((HttpWebResponse)ex.Response).StatusDescription;
				}
				Util._OutputDebug(text);
			}
			catch (Exception ex2)
			{
				Util._OutputDebug(ex2.ToString());
			}
			return false;
		}

		private static void _OutputDebug(string strOutput)
		{
			Console.WriteLine(strOutput);
			TsLog.Log(strOutput, new object[0]);
		}
	}
}
