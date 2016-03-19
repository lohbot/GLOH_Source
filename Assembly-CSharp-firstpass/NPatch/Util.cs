using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace NPatch
{
	public class Util
	{
		public static string GetMD5(string strFilePath)
		{
			if (strFilePath == string.Empty)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string result;
			using (FileStream fileStream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				if (fileStream == null)
				{
					result = string.Empty;
				}
				else
				{
					MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
					byte[] array = mD5CryptoServiceProvider.ComputeHash(fileStream);
					byte[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						byte b = array2[i];
						stringBuilder.Append(b.ToString("x2"));
					}
					result = stringBuilder.ToString().ToLower();
				}
			}
			return result;
		}

		public static string GetMD5(byte[] arByte, string filePath)
		{
			if (filePath == string.Empty)
			{
				return string.Empty;
			}
			if (arByte == null)
			{
				return string.Empty;
			}
			if (arByte.Length != 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
				byte[] array = mD5CryptoServiceProvider.ComputeHash(arByte);
				byte[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					byte b = array2[i];
					stringBuilder.Append(b.ToString("x2"));
				}
				return stringBuilder.ToString().ToLower();
			}
			return string.Empty;
		}

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
				string text = "NPatch.Util!!! 에러!!!\n\nException Message :" + ex;
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

		public static int GetLastVersionFromCacheList(string localRoot)
		{
			string path = string.Format("{0}/cachelist.txt", localRoot);
			if (!File.Exists(path))
			{
				return -1;
			}
			int num = 0;
			using (Stream stream = File.Open(path, FileMode.Open))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8))
				{
					while (binaryReader.PeekChar() != -1)
					{
						binaryReader.ReadString();
						int num2 = binaryReader.ReadInt32();
						binaryReader.ReadInt64();
						if (num2 > num)
						{
							num = num2;
						}
					}
				}
			}
			return num;
		}

		public static float GetPackStartVersion(string packName)
		{
			string fileName = Path.GetFileName(packName);
			string s = fileName.Substring(0, fileName.LastIndexOf('_'));
			float result = 0f;
			if (float.TryParse(s, out result))
			{
				return result;
			}
			return 0f;
		}

		public static float GetPackEndVersion(string packName)
		{
			string text = packName.ToLower().Replace(".zip", string.Empty);
			string text2 = text.Substring(text.LastIndexOf('_') + 1);
			string text3 = string.Empty;
			string text4 = string.Empty;
			string s = string.Empty;
			if (text2.Contains(","))
			{
				text3 = text2.Substring(0, text2.IndexOf(','));
			}
			else
			{
				text3 = text2;
			}
			if (text3.Contains("["))
			{
				text4 = text3.Substring(0, text3.IndexOf('['));
			}
			else
			{
				text4 = text3;
			}
			if (text4.Contains("("))
			{
				s = text4.Substring(0, text3.IndexOf('('));
			}
			else
			{
				s = text4;
			}
			float result = 0f;
			if (float.TryParse(s, out result))
			{
				return result;
			}
			return 0f;
		}

		public static int GetPackLevel(string packName)
		{
			string text = packName.ToLower().Replace(".zip", string.Empty);
			string text2 = text.Substring(text.IndexOf('_') + 1);
			string text3 = string.Empty;
			string s = string.Empty;
			if (text2.Contains(","))
			{
				text3 = text2.Substring(0, text2.IndexOf(','));
			}
			else
			{
				text3 = text2;
			}
			if (!text3.Contains("["))
			{
				return -1;
			}
			s = text3.Substring(text3.IndexOf('[') + 1, text3.Length - text3.IndexOf(']'));
			int result = 0;
			if (int.TryParse(s, out result))
			{
				return result;
			}
			return -1;
		}

		public static int GetPackLangCode(string packName)
		{
			string text = packName.ToLower().Replace(".zip", string.Empty);
			string text2 = text.Substring(text.IndexOf('_') + 1);
			string text3 = string.Empty;
			string s = string.Empty;
			if (text2.Contains(","))
			{
				text3 = text2.Substring(0, text2.IndexOf(','));
			}
			else
			{
				text3 = text2;
			}
			if (!text3.Contains("("))
			{
				return -1;
			}
			s = text3.Substring(text3.IndexOf('(') + 1, text3.Length - text3.IndexOf(')'));
			int result = 0;
			if (int.TryParse(s, out result))
			{
				return result;
			}
			return -1;
		}

		public static float GetInfoFileVersion(string filePath)
		{
			string fileName = Path.GetFileName(filePath);
			string text = fileName.ToLower().Replace(".txt", string.Empty);
			string text2 = string.Empty;
			string s = string.Empty;
			if (text.Contains("["))
			{
				text2 = text.Substring(0, text.IndexOf('['));
			}
			else
			{
				text2 = text;
			}
			if (text2.Contains("("))
			{
				s = text2.Substring(0, text2.IndexOf('('));
			}
			else
			{
				s = text2;
			}
			float result = 0f;
			if (float.TryParse(s, out result))
			{
				return result;
			}
			return -1f;
		}

		public static int GetInfoFileLevel(string filePath)
		{
			string text = filePath.ToLower().Replace(".txt", string.Empty);
			string s = string.Empty;
			if (!text.Contains("["))
			{
				return -1;
			}
			s = text.Substring(text.IndexOf('[') + 1, text.Length - text.IndexOf(']'));
			int result = 0;
			if (int.TryParse(s, out result))
			{
				return result;
			}
			return -1;
		}

		public static bool IsFirstPatch(string localPath, string urlPath)
		{
			if (!Path.IsPathRooted(localPath))
			{
				localPath = Path.GetFullPath(localPath);
			}
			string path = string.Format("{0}/PatchedDate.txt", localPath);
			return !File.Exists(path);
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
				string text = "NPatch.Util!!! 에러!!!\n\nException Message :" + ex.Message;
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
		}
	}
}
