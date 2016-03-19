using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace omniata
{
	public class Storage
	{
		private const string TAG = "Storage";

		private string DataFile
		{
			get;
			set;
		}

		public Storage(string persistentDataPath)
		{
			this.DataFile = persistentDataPath + "/omniata-events.txt";
		}

		public List<QueueElement> Read()
		{
			if (!this.Exist())
			{
				Log.Debug("Storage", "queue file missing");
				return new List<QueueElement>();
			}
			List<QueueElement> list = new List<QueueElement>();
			string text = this.ReadFromFile();
			if (text.Length > 0)
			{
				string[] array = text.Split(new char[]
				{
					'&'
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string s = array2[i];
					string text2 = WWW.UnEscapeURL(s);
					QueueElement queueElement = QueueElement.Deserialize(text2);
					if (queueElement == null)
					{
						Log.Error("Storage", "Invalid event found, skipping: " + text2);
					}
					else
					{
						list.Add(queueElement);
					}
				}
			}
			return list;
		}

		public void Write(List<QueueElement> elements)
		{
			List<string> list = new List<string>();
			foreach (QueueElement current in elements)
			{
				list.Add(WWW.EscapeURL(current.Serialize()));
			}
			string data = string.Join("&", list.ToArray());
			this.WriteToFile(data);
		}

		private string ReadFromFile()
		{
			StreamReader streamReader = new StreamReader(this.DataFile);
			string result = streamReader.ReadLine();
			streamReader.Close();
			return result;
		}

		private void WriteToFile(string data)
		{
			StreamWriter streamWriter = File.CreateText(this.DataFile);
			if (streamWriter == null)
			{
				Log.Error("Storage", "Creating file failed: " + this.DataFile);
				return;
			}
			streamWriter.WriteLine(data);
			streamWriter.Close();
		}

		private bool Exist()
		{
			return File.Exists(this.DataFile);
		}
	}
}
