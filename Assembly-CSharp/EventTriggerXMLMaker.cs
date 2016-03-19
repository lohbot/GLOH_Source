using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using TsBundle;
using UnityEngine;

public class EventTriggerXMLMaker
{
	public GameObject[] ReadXML(XmlReader Reader)
	{
		if (Reader == null)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log("Can't Read XMLFile : " + Application.dataPath + "/../Test.xml", new object[0]);
			}
			return null;
		}
		GameObject[] result;
		try
		{
			List<GameObject> list = new List<GameObject>();
			Reader.ReadStartElement("EventTrigger");
			while (Reader.ReadToNextSibling("Object"))
			{
				if (Reader.NodeType == XmlNodeType.EndElement)
				{
					break;
				}
				if (Reader.NodeType == XmlNodeType.Element)
				{
					string attribute = Reader.GetAttribute("Name");
					GameObject gameObject = EventTriggerHelper.CreateEventTriggerObject(attribute, typeof(EventTrigger_Game), null);
					EventTrigger component = gameObject.GetComponent<EventTrigger>();
					component.ReadXML(Reader);
					list.Add(gameObject);
				}
				Reader.Read();
			}
			Reader.Close();
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log("Readed XML", new object[0]);
			}
			result = list.ToArray();
		}
		catch (Exception ex)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.LogError(ex.Message + " " + ex.StackTrace, new object[0]);
			}
			result = null;
		}
		return result;
	}

	public GameObject[] ReadXML(IDownloadedItem wItem, object obj)
	{
		if (wItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
		}
		else
		{
			GameObject gameObject = wItem.mainAsset as GameObject;
			if (null != gameObject)
			{
				TsGameDataAdapter component = gameObject.GetComponent<TsGameDataAdapter>();
				TsGameData gameData = component.GameData;
				return this.ReadXML(gameData.serializeGameDatas[0]);
			}
			wItem.unloadImmediate = true;
		}
		return null;
	}

	public GameObject[] ReadXML(string strData)
	{
		MemoryStream stream = new MemoryStream(NrXmlSerializer.StringToUTF8ByteArray(strData));
		XmlReader reader = XmlReader.Create(stream);
		return this.ReadXML(reader);
	}

	public GameObject[] LoadXML(string FileName)
	{
		if (FileName == null)
		{
			return null;
		}
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.Log("Start Load XML: " + FileName, new object[0]);
		}
		XmlReader reader = XmlReader.Create(FileName);
		GameObject[] result = this.ReadXML(reader);
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.Log("Complete Load XML", new object[0]);
		}
		return result;
	}

	public bool SaveXML(string FileName, EventTrigger[] TriggerObject)
	{
		if (TriggerObject == null)
		{
			return false;
		}
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.Log("Start Save XML: " + FileName, new object[0]);
		}
		XmlWriter xmlWriter = XmlWriter.Create(FileName, new XmlWriterSettings
		{
			Encoding = Encoding.UTF8,
			Indent = true
		});
		if (xmlWriter == null)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log("Can't Create XMLFile : " + FileName, new object[0]);
			}
			return false;
		}
		xmlWriter.WriteStartDocument();
		xmlWriter.WriteStartElement("EventTrigger");
		for (int i = 0; i < TriggerObject.Length; i++)
		{
			EventTrigger eventTrigger = TriggerObject[i];
			eventTrigger.WriteXML(xmlWriter);
		}
		xmlWriter.WriteEndElement();
		xmlWriter.Close();
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.Log("Complete Save XML", new object[0]);
		}
		return true;
	}

	public bool SaveXML(string FileName)
	{
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.Log("Start Save XML: " + FileName, new object[0]);
		}
		XmlWriter xmlWriter = XmlWriter.Create(FileName, new XmlWriterSettings
		{
			Encoding = Encoding.UTF8,
			Indent = true
		});
		if (xmlWriter == null)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log("Can't Create XMLFile : " + FileName, new object[0]);
			}
			return false;
		}
		xmlWriter.WriteStartDocument();
		xmlWriter.WriteStartElement("EventTrigger");
		EventTrigger[] array = UnityEngine.Object.FindObjectsOfType(typeof(EventTrigger)) as EventTrigger[];
		EventTrigger[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EventTrigger eventTrigger = array2[i];
			eventTrigger.WriteXML(xmlWriter);
		}
		xmlWriter.WriteEndElement();
		xmlWriter.Close();
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.Log("Complete Save XML", new object[0]);
		}
		return true;
	}
}
