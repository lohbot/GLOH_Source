using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class NrXmlSerializer
{
	private static XmlDocument m_kXmlDocument = new XmlDocument();

	private static NrXmlMap m_kXmlMap = new NrXmlMap();

	private static string UTF8ByteArrayToString(byte[] characters)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(characters);
	}

	public static byte[] StringToUTF8ByteArray(string pXmlString)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetBytes(pXmlString);
	}

	public static string SerializeObject(object pObject, Type type)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlSerializer xmlSerializer = new XmlSerializer(type);
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlTextWriter.IndentChar = '\t';
		xmlTextWriter.Indentation = 1;
		xmlSerializer.Serialize(xmlTextWriter, pObject);
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
		return NrXmlSerializer.UTF8ByteArrayToString(memoryStream.ToArray());
	}

	public static object DeserializeObject(string pXmlizedString, Type type)
	{
		object result = null;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			MemoryStream stream = new MemoryStream(NrXmlSerializer.StringToUTF8ByteArray(pXmlizedString));
			result = xmlSerializer.Deserialize(stream);
		}
		catch (Exception ex)
		{
			Debug.Log("ex" + ex.Message);
			Debug.Log("ex" + ex.StackTrace);
			if (ex.InnerException != null)
			{
				Debug.Log("ex" + ex.InnerException.Message);
				Debug.Log("ex" + ex.InnerException.StackTrace);
			}
			Debug.Log("pXmlizedString==\n" + pXmlizedString + " \n -END -");
		}
		return result;
	}

	public static object DeserializeObjectFromTextReader(string pXmlizedString, Type type)
	{
		object result = null;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			MemoryStream input = new MemoryStream(NrXmlSerializer.StringToUTF8ByteArray(pXmlizedString));
			result = xmlSerializer.Deserialize(new XmlTextReader(input)
			{
				Normalization = false
			});
		}
		catch (Exception ex)
		{
			Debug.Log("ex" + ex.Message);
			Debug.Log("ex" + ex.StackTrace);
			if (ex.InnerException != null)
			{
				Debug.Log("ex" + ex.InnerException.Message);
				Debug.Log("ex" + ex.InnerException.StackTrace);
			}
			Debug.Log("pXmlizedString==\n" + pXmlizedString + " \n -END -");
		}
		return result;
	}

	public static void SaveXML(string _FilePath, string _Data, bool _NewLinePropertyOpt)
	{
		if (_NewLinePropertyOpt)
		{
			_Data = _Data.Replace(">", ">\r\n");
		}
		NrXmlSerializer.SaveXML(_FilePath, _Data);
	}

	public static void SaveXML(string _FilePath, string _Data)
	{
	}

	public static string LoadXML(string _FilePath)
	{
		StreamReader streamReader = File.OpenText(_FilePath);
		string result = streamReader.ReadToEnd();
		streamReader.Close();
		Debug.Log("File Read");
		return result;
	}

	public static XmlDocument IsNormalXML(string strData, ref string strTagName)
	{
		try
		{
			int num = strData.IndexOf('<');
			if (num < 0)
			{
				XmlDocument result = null;
				return result;
			}
			if (strData[num] == '<' && strData[num + 1] == '?')
			{
				XmlDocument xmlDocument = new XmlDocument();
				byte[] bytes = Encoding.UTF8.GetBytes(strData);
				MemoryStream inStream = new MemoryStream(bytes);
				xmlDocument.Load(inStream);
				strTagName = NrXmlSerializer.GetTagName(xmlDocument);
				XmlDocument result = xmlDocument;
				return result;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return null;
	}

	public static string[] NewSplit(string _Data, string TypeName)
	{
		XmlDocument xmlDocument = NrXmlSerializer.IsNormalXML(_Data, ref TypeName);
		if (xmlDocument != null)
		{
			try
			{
				return NrXmlSerializer.SplitByTagName(xmlDocument, TypeName);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}
		string value = "<" + TypeName + ">";
		string text = "</" + TypeName + ">";
		List<string> list = new List<string>();
		int i = 0;
		int num = 0;
		while (i < _Data.Length)
		{
			i = _Data.IndexOf(value, i);
			if (i != -1)
			{
				num = _Data.IndexOf(text, i + 1);
				string item = _Data.Substring(i, num - i + text.Length);
				list.Add(item);
			}
			if (i < 0 || num < 0)
			{
				break;
			}
			i++;
		}
		if (list.Count == 0)
		{
			Debug.LogError("NotSplit:" + TypeName);
		}
		return list.ToArray();
	}

	public static string[] NewSplit(string _Data, Type _Type)
	{
		string typeName = _Type.ToString();
		return NrXmlSerializer.NewSplit(_Data, typeName);
	}

	public static string[] Split(string _Data, Type _Type)
	{
		string text = "<" + _Type.ToString();
		string text2 = "</" + _Type.ToString() + ">";
		string[] separator = new string[]
		{
			text
		};
		string[] array = _Data.Split(separator, StringSplitOptions.None);
		List<string> list = new List<string>();
		for (int i = 0; i < array.Length; i++)
		{
			if (i != 0)
			{
				string text3 = array[i];
				int num = text3.LastIndexOf(text2);
				if (text3.Length < num + text2.Length)
				{
					Debug.LogError("SplitByTagName Error <TAG> : " + text);
				}
				else
				{
					text3 = text3.Substring(0, num + text2.Length);
					list.Add(text + text3);
				}
			}
		}
		return list.ToArray();
	}

	public static string GetTagNameCustomXML(string strfilePath)
	{
		try
		{
			using (StreamReader streamReader = new StreamReader(strfilePath, Encoding.Default, true))
			{
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					if (text.Length > 0)
					{
						if (!text.Contains("<?") && !text.Contains("<!"))
						{
							string[] array = text.Split(new char[]
							{
								'<',
								'>'
							});
							try
							{
								if (array[1].Length > 0)
								{
									return array[1];
								}
							}
							catch (Exception arg)
							{
								Debug.Log(" TagName Find Arry Failed!!!!" + arg);
							}
						}
					}
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return "(NoTag!!!!)";
	}

	public static string GetTagName(XmlDocument doc)
	{
		try
		{
			if (doc != null)
			{
				XmlElement documentElement = doc.DocumentElement;
				if (documentElement != null)
				{
					if (documentElement.FirstChild != null)
					{
						return documentElement.FirstChild.Name;
					}
				}
				else
				{
					Debug.LogWarning("GetTagName - 루트가 없음!!!");
				}
			}
			else
			{
				Debug.LogWarning("GetTagName - doc 개체 없음!!!");
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return string.Empty;
	}

	public static string[] SplitByTagName(XmlDocument doc, string _RowName)
	{
		List<string> list = new List<string>();
		try
		{
			if (doc != null)
			{
				XmlElement documentElement = doc.DocumentElement;
				if (documentElement != null)
				{
					XmlNodeList elementsByTagName = documentElement.GetElementsByTagName(_RowName);
					foreach (XmlNode xmlNode in elementsByTagName)
					{
						list.Add(xmlNode.OuterXml);
					}
				}
				else
				{
					Debug.LogWarning("SplitByTagName - 루트가 없음!!!");
				}
			}
			else
			{
				Debug.LogWarning("SplitByTagName - doc 개체 없음!!!");
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return list.ToArray();
	}

	public static string[] SplitByTagName(string _Data, string _TabName)
	{
		XmlDocument xmlDocument = NrXmlSerializer.IsNormalXML(_Data, ref _TabName);
		if (xmlDocument != null)
		{
			try
			{
				return NrXmlSerializer.SplitByTagName(xmlDocument, _TabName);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}
		string text = "<" + _TabName;
		string text2 = "</" + _TabName + ">";
		string[] separator = new string[]
		{
			text
		};
		string[] array = _Data.Split(separator, StringSplitOptions.None);
		List<string> list = new List<string>();
		for (int i = 0; i < array.Length; i++)
		{
			if (i != 0)
			{
				string text3 = array[i];
				int num = text3.LastIndexOf(text2);
				if (text3.Length < num + text2.Length)
				{
					Debug.LogError("SplitByTagName Error <TAG> : " + text);
				}
				else
				{
					text3 = text3.Substring(0, num + text2.Length);
					list.Add(text + text3);
				}
			}
		}
		return list.ToArray();
	}

	public static NrXmlMap ParseXml(string pXmlizedString, string pTagName)
	{
		try
		{
			NrXmlSerializer.m_kXmlMap.Init();
			NrXmlSerializer.m_kXmlDocument.InnerXml = pXmlizedString;
			XmlNodeList elementsByTagName = NrXmlSerializer.m_kXmlDocument.GetElementsByTagName(pTagName);
			if (elementsByTagName[0] == null)
			{
				foreach (XmlNode xmlNode in NrXmlSerializer.m_kXmlDocument.DocumentElement.ChildNodes)
				{
					NrXmlSerializer.m_kXmlMap.kXmlValList.Add(xmlNode.InnerXml);
				}
			}
			else
			{
				for (int i = 0; i < elementsByTagName[0].ChildNodes.Count; i++)
				{
					NrXmlSerializer.m_kXmlMap.kXmlValList.Add(elementsByTagName[0].ChildNodes[i].InnerText);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log("ex" + ex);
			if (ex.InnerException != null)
			{
				Debug.Log("ex" + ex.InnerException);
			}
			Debug.Log(string.Concat(new string[]
			{
				"TagName : ",
				pTagName,
				"  pXmlizedString : \r\n",
				pXmlizedString,
				" \n -END -"
			}));
		}
		return NrXmlSerializer.m_kXmlMap;
	}
}
