using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace ConsoleCommand.Statistics
{
	public static class MemoryCollection
	{
		public enum Mode
		{
			NewlyLeakOnly,
			AllLeaks,
			LoadedAll,
			NewObjectOnly,
			Monitoring
		}

		private delegate bool _CheckRemove(int instanceID);

		private static StringBuilder m_StringBuilder;

		private static Dictionary<int, LoadedUnityItem> m_Swapping;

		private static CollectionSummary[] m_Collections;

		private static Type[] CollectionTypes;

		static MemoryCollection()
		{
			MemoryCollection.CollectionTypes = new Type[]
			{
				typeof(Texture),
				typeof(GameObject)
			};
			MemoryCollection.m_StringBuilder = new StringBuilder(128);
			MemoryCollection.m_Swapping = new Dictionary<int, LoadedUnityItem>(128);
			MemoryCollection.m_Collections = new CollectionSummary[MemoryCollection.CollectionTypes.Length];
			for (int i = 0; i < MemoryCollection.m_Collections.Length; i++)
			{
				MemoryCollection.m_Collections[i] = new CollectionSummary(MemoryCollection.CollectionTypes[i], 128);
			}
		}

		public static void Print(MemoryCollection.Mode mode)
		{
			int num = 0;
			for (int i = 0; i < MemoryCollection.CollectionTypes.Length; i++)
			{
				num += MemoryCollection._Collection(mode, ref MemoryCollection.m_Collections[i].items, new Type[]
				{
					MemoryCollection.CollectionTypes[i]
				});
			}
			string text = string.Format("Total memory size = {0} bytes", (num != 0) ? num.ToString("#,###,###,###") : "<<unknwon>>");
			NrTSingleton<NrDebugConsole>.Instance.Print(text);
			TsLog.Log(text, new object[0]);
		}

		public static int Monitoring()
		{
			int num = 0;
			for (int i = 0; i < MemoryCollection.CollectionTypes.Length; i++)
			{
				int num2 = MemoryCollection._Collection(MemoryCollection.Mode.Monitoring, ref MemoryCollection.m_Collections[i].items, new Type[]
				{
					MemoryCollection.CollectionTypes[i]
				});
				num += num2;
				MemoryCollection.m_Collections[i].collectedSize = num2;
				MemoryCollection.m_Collections[i].Sort();
			}
			return num;
		}

		[DebuggerHidden]
		internal static IEnumerable<CollectionSummary> CollectionSummaries()
		{
			MemoryCollection.<CollectionSummaries>c__Iterator65 <CollectionSummaries>c__Iterator = new MemoryCollection.<CollectionSummaries>c__Iterator65();
			MemoryCollection.<CollectionSummaries>c__Iterator65 expr_07 = <CollectionSummaries>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}

		private static int _Collection(MemoryCollection.Mode mode, ref Dictionary<int, LoadedUnityItem> preLoadedItems, params Type[] findTypes)
		{
			if (preLoadedItems == null || MemoryCollection.m_Swapping == null)
			{
				TsLog.LogWarning("�ʿ��� ��ü�� �����Ǿ� ���� �ʽ��ϴ�.", new object[0]);
				return 0;
			}
			Dictionary<int, LoadedUnityItem> curLoadedItems = MemoryCollection.m_Swapping;
			int num = 0;
			int num2 = 0;
			string text = string.Empty;
			MemoryCollection.m_StringBuilder.Length = 0;
			for (int i = 0; i < findTypes.Length; i++)
			{
				Type type = findTypes[i];
				if (text != string.Empty)
				{
					MemoryCollection.m_StringBuilder.AppendFormat(", {0}", type.ToString());
				}
				else
				{
					MemoryCollection.m_StringBuilder.Append(type.ToString());
				}
				UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(type);
				UnityEngine.Object[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					UnityEngine.Object @object = array2[j];
					int instanceID = @object.GetInstanceID();
					int runtimeMemorySize = Profiler.GetRuntimeMemorySize(@object);
					LoadedUnityItem loadedUnityItem = null;
					if (preLoadedItems.ContainsKey(instanceID))
					{
						loadedUnityItem = preLoadedItems[instanceID];
						if (loadedUnityItem.objectType == @object.GetType() && loadedUnityItem.objectName == @object.name)
						{
							loadedUnityItem.hitCount++;
							curLoadedItems.Add(instanceID, loadedUnityItem);
						}
					}
					if (!curLoadedItems.ContainsKey(instanceID))
					{
						string additonalDesc = null;
						if (@object is Texture)
						{
							string arg;
							if (@object is RenderTexture)
							{
								arg = (@object as RenderTexture).format.ToString();
							}
							else if (@object is Texture2D)
							{
								arg = (@object as Texture2D).format.ToString();
							}
							else if (@object is Cubemap)
							{
								arg = (@object as Cubemap).format.ToString();
							}
							else
							{
								arg = "?";
							}
							Texture texture = @object as Texture;
							additonalDesc = string.Format(" ({0}x{1}, {2})", texture.width, texture.height, arg);
						}
						loadedUnityItem = LoadedUnityItem.Instantiate(@object.GetType(), string.Intern(@object.name), runtimeMemorySize, additonalDesc);
						curLoadedItems.Add(instanceID, loadedUnityItem);
					}
					if ((mode == MemoryCollection.Mode.NewlyLeakOnly && loadedUnityItem.hitCount == 1) || (mode == MemoryCollection.Mode.AllLeaks && loadedUnityItem.hitCount > 0) || mode == MemoryCollection.Mode.LoadedAll || mode == MemoryCollection.Mode.Monitoring || (mode == MemoryCollection.Mode.NewObjectOnly && loadedUnityItem.hitCount == 0))
					{
						num++;
						num2 += runtimeMemorySize;
					}
				}
			}
			if (mode != MemoryCollection.Mode.Monitoring)
			{
				text = MemoryCollection.m_StringBuilder.ToString();
				MemoryCollection.m_StringBuilder.Length = 0;
				if (preLoadedItems.Count > 0 || mode == MemoryCollection.Mode.LoadedAll)
				{
					MemoryCollection.m_StringBuilder.AppendFormat("[{0}] Count = {1} (total memory bytes = {2})\r\n", text, num, (num2 <= 0) ? "<<unknown>>" : num2.ToString("###,###,###,###"));
					MemoryCollection.m_StringBuilder.AppendLine("=====================================================================================");
					foreach (KeyValuePair<int, LoadedUnityItem> current in curLoadedItems)
					{
						LoadedUnityItem value = current.Value;
						if ((mode == MemoryCollection.Mode.NewlyLeakOnly && value.hitCount == 1) || (mode == MemoryCollection.Mode.AllLeaks && value.hitCount > 0) || mode == MemoryCollection.Mode.LoadedAll || (mode == MemoryCollection.Mode.NewObjectOnly && value.hitCount == 0))
						{
							MemoryCollection.m_StringBuilder.AppendFormat("{0} (Hits={1})\r\n", value.ToString(), value.hitCount);
						}
					}
					MemoryCollection.m_StringBuilder.AppendLine("=====================================================================================");
					MemoryCollection.m_StringBuilder.AppendLine();
				}
				else
				{
					MemoryCollection.m_StringBuilder.AppendFormat("[{0}] first hit", text);
					MemoryCollection.m_StringBuilder.AppendLine();
				}
				try
				{
					string text2 = MemoryCollection.m_StringBuilder.ToString();
					TsLog.Log(text2, new object[0]);
					NrTSingleton<NrDebugConsole>.Instance.Print(text2);
				}
				catch (Exception obj)
				{
					TsLog.LogWarning(obj);
					NrTSingleton<NrDebugConsole>.Instance.Print(string.Empty);
				}
			}
			MemoryCollection._CheckRemove checkRemove;
			if (mode == MemoryCollection.Mode.LoadedAll && preLoadedItems.Count > 0)
			{
				foreach (LoadedUnityItem current2 in curLoadedItems.Values)
				{
					if (current2.hitCount > 0)
					{
						current2.hitCount--;
					}
				}
				Dictionary<int, LoadedUnityItem> tempCollection = preLoadedItems;
				checkRemove = ((int id) => !tempCollection.ContainsKey(id));
			}
			else
			{
				MemoryCollection.m_Swapping = preLoadedItems;
				preLoadedItems = curLoadedItems;
				checkRemove = ((int id) => !curLoadedItems.ContainsKey(id));
			}
			foreach (KeyValuePair<int, LoadedUnityItem> current3 in MemoryCollection.m_Swapping)
			{
				int key = current3.Key;
				if (checkRemove(key))
				{
					LoadedUnityItem value2 = current3.Value;
					value2.Dispose();
				}
			}
			MemoryCollection.m_Swapping.Clear();
			return num2;
		}
	}
}
