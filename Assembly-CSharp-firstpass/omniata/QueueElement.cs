using System;
using System.Collections.Generic;
using UnityEngine;

namespace omniata
{
	public class QueueElement
	{
		private const string TAG = "QueueElement";

		private const string META_EVENT_TYPE = "event_type";

		private const string META_CREATION = "creation";

		private const string META_RETRIES = "retries";

		private static string META = "meta";

		private static string DATA = "data";

		private string EventType
		{
			get;
			set;
		}

		private double CreationSecondsSinceEpoch
		{
			get;
			set;
		}

		public int Retries
		{
			get;
			set;
		}

		private Dictionary<string, string> Parameters
		{
			get;
			set;
		}

		public QueueElement(string eventType, double creationSecondsSinceEpoch, int retries, Dictionary<string, string> parameters)
		{
			this.EventType = eventType;
			this.CreationSecondsSinceEpoch = creationSecondsSinceEpoch;
			this.Retries = retries;
			this.Parameters = parameters;
		}

		public string WireFormat()
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> current in this.Parameters)
			{
				list.Add(WWW.EscapeURL(current.Key) + '=' + WWW.EscapeURL(current.Value));
			}
			if (this.CreationSecondsSinceEpoch != 0.0)
			{
				double value = Utils.SecondsSinceEpoch() - this.CreationSecondsSinceEpoch;
				list.Add("om_delta=" + Utils.DoubleToIntegerString(value));
			}
			return string.Join("&", list.ToArray());
		}

		public string Serialize()
		{
			string text = WWW.EscapeURL(string.Concat(new object[]
			{
				"event_type=",
				WWW.EscapeURL(this.EventType),
				"&creation=",
				Utils.DoubleToIntegerString(this.CreationSecondsSinceEpoch),
				"&retries=",
				this.Retries
			}));
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> current in this.Parameters)
			{
				list.Add(WWW.EscapeURL(current.Key) + '=' + WWW.EscapeURL(current.Value));
			}
			return string.Concat(new object[]
			{
				QueueElement.META,
				'=',
				text,
				'&',
				QueueElement.DATA,
				'=',
				WWW.EscapeURL(string.Join("&", list.ToArray()))
			});
		}

		public static QueueElement Deserialize(string serialized)
		{
			string[] array = serialized.Split(new char[]
			{
				'&'
			});
			string text = string.Empty;
			double creationSecondsSinceEpoch = 0.0;
			int retries = 0;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				string[] array3 = text2.Split(new char[]
				{
					'='
				});
				string text3 = WWW.UnEscapeURL(array3[0]);
				string text4 = WWW.UnEscapeURL(array3[1]);
				if (text3.CompareTo(QueueElement.META) == 0)
				{
					string[] array4 = text4.Split(new char[]
					{
						'&'
					});
					string[] array5 = array4;
					for (int j = 0; j < array5.Length; j++)
					{
						string text5 = array5[j];
						string[] array6 = text5.Split(new char[]
						{
							'='
						});
						string text6 = WWW.UnEscapeURL(array6[0]);
						string text7 = WWW.UnEscapeURL(array6[1]);
						if (text6.CompareTo("event_type") == 0)
						{
							text = text7;
						}
						else if (text6.CompareTo("creation") == 0)
						{
							creationSecondsSinceEpoch = Convert.ToDouble(text7);
						}
						else if (text6.CompareTo("retries") == 0)
						{
							retries = Convert.ToInt32(text7);
						}
						else
						{
							Log.Error("QueueElement", "Unknown meta key: " + text6);
						}
					}
				}
				else if (text3.CompareTo(QueueElement.DATA) == 0)
				{
					string[] array7 = text4.Split(new char[]
					{
						'&'
					});
					string[] array8 = array7;
					for (int k = 0; k < array8.Length; k++)
					{
						string text8 = array8[k];
						string[] array9 = text8.Split(new char[]
						{
							'='
						});
						string key = WWW.UnEscapeURL(array9[0]);
						string value = WWW.UnEscapeURL(array9[1]);
						dictionary.Add(key, value);
					}
				}
			}
			if (text.CompareTo(string.Empty) == 0)
			{
				Log.Error("QueueElement", "eventType not found when deserializing, skipping event");
				return null;
			}
			return new QueueElement(text, creationSecondsSinceEpoch, retries, dictionary);
		}
	}
}
