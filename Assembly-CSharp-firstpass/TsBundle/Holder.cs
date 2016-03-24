using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace TsBundle
{
	public static class Holder
	{
		public class DbgBS
		{
			public string key
			{
				get;
				private set;
			}

			public int size
			{
				get;
				private set;
			}

			public DbgBS(string k, int sz)
			{
				this.key = k;
				this.size = sz;
			}

			public void SetSize(int sz)
			{
				this.size = sz;
			}
		}

		private static List<Dictionary<string, WWWItem>> m_bundleGroupStack = new List<Dictionary<string, WWWItem>>();

		private static List<string> m_bundleNameStack = new List<string>();

		private static int m_stackIP = -1;

		public static List<string> m_ChnStreamFileList_Datas = new List<string>();

		private static Dictionary<string, WWWItem> ms_UndefinedStack = new Dictionary<string, WWWItem>();

		private static StringBuilder m_DebugMessage = new StringBuilder(1024);

		private static Dictionary<string, Holder.DbgBS> m_dbgBS = new Dictionary<string, Holder.DbgBS>();

		public static int stackDepth
		{
			get
			{
				return Holder.m_bundleGroupStack.Count;
			}
		}

		public static int dbgTotAssetCount
		{
			get
			{
				int num = 0;
				for (int i = Holder.m_stackIP; i >= 0; i--)
				{
					Dictionary<string, WWWItem> dictionary = Holder.m_bundleGroupStack[i];
					num += dictionary.Count;
				}
				return num;
			}
		}

		public static int dbgGetBundleSize(int idxStack)
		{
			if (0 > idxStack || idxStack > Holder.m_stackIP)
			{
				return 0;
			}
			int num = 0;
			Dictionary<string, WWWItem> dictionary = Holder.m_bundleGroupStack[idxStack];
			foreach (WWWItem current in dictionary.Values)
			{
				num += current.safeSize;
			}
			return num;
		}

		public static int dbgGetBundleCount()
		{
			return Holder.m_dbgBS.Count;
		}

		public static string dbgGetStackName(int idxStack)
		{
			if (0 > idxStack || idxStack > Holder.m_stackIP)
			{
				return string.Empty;
			}
			return Holder.m_bundleNameStack[idxStack];
		}

		private static void _CreateDefaultStack()
		{
			Holder.m_stackIP = 0;
			TsLog.Log("[TsBundle] _CreateDefaultStack()", new object[0]);
			Holder.m_bundleGroupStack.Add(new Dictionary<string, WWWItem>());
			Holder.m_bundleNameStack.Add(Option.defaultStackName);
		}

		public static void ClearHolder()
		{
			while (0 <= Holder.m_stackIP)
			{
				Holder.PopBundleGroup();
			}
		}

		private static WWWItem TryGetBundle(string key)
		{
			if (0 > Holder.m_stackIP)
			{
				Holder._CreateDefaultStack();
			}
			key = key.ToLower();
			WWWItem result = null;
			Holder._TryGetBundle(key, out result);
			return result;
		}

		private static void _TryGetBundle(string keyLowerString, out WWWItem wItem)
		{
			wItem = null;
			for (int i = Holder.m_stackIP; i >= 0; i--)
			{
				Dictionary<string, WWWItem> dictionary = Holder.m_bundleGroupStack[i];
				dictionary.TryGetValue(keyLowerString, out wItem);
				if (wItem != null)
				{
					wItem.refCnt++;
					break;
				}
			}
		}

		public static WWWItem TryGetOrCreateBundle(string key, string stackName)
		{
			return Holder.TryGetOrCreateBundle(key, stackName, false);
		}

		public static WWWItem TryGetOrCreateBundle(string key, string stackName, bool immediatelyUrl)
		{
			if (0 > Holder.m_stackIP)
			{
				Holder._CreateDefaultStack();
			}
			if (!key.Contains("?notlow"))
			{
				key = key.ToLower();
			}
			WWWItem wWWItem = null;
			Holder._TryGetBundle(key, out wWWItem);
			if (wWWItem == null)
			{
				Dictionary<string, WWWItem> dictionary = Holder._FindStack(stackName);
				dictionary.TryGetValue(key, out wWWItem);
				if (wWWItem == null)
				{
					int index = Holder.m_bundleGroupStack.IndexOf(dictionary);
					stackName = Holder.m_bundleNameStack[index];
					wWWItem = new WWWItem(stackName);
					if (immediatelyUrl)
					{
						wWWItem.SetAnotherUrl(key);
					}
					else
					{
						wWWItem.SetAssetPath(key);
					}
					Holder._AddTo(stackName, wWWItem);
				}
			}
			wWWItem.refCnt++;
			return wWWItem;
		}

		internal static WWWItem GetPreDownloadBundle(string assetPath, ItemType type)
		{
			WWWItem wWWItem = null;
			string text = assetPath.ToLower();
			foreach (Dictionary<string, WWWItem> current in Holder.m_bundleGroupStack)
			{
				if (current.ContainsKey(text))
				{
					WWWItem result = null;
					return result;
				}
			}
			if (Holder.ms_UndefinedStack.TryGetValue(text, out wWWItem))
			{
				return null;
			}
			wWWItem = new WWWItem(Option.undefinedStackName);
			wWWItem.SetAssetPath(text);
			Holder.ms_UndefinedStack.Add(text, wWWItem);
			wWWItem.SetItemType(type);
			wWWItem.refCnt++;
			return wWWItem;
		}

		public static void PushBundleGroup(string name)
		{
			if (0 > Holder.m_stackIP)
			{
				Holder._CreateDefaultStack();
			}
			Holder.m_stackIP++;
			if (name == null)
			{
				name = Holder.m_stackIP.ToString();
			}
			Holder.m_bundleGroupStack.Add(new Dictionary<string, WWWItem>());
			Holder.m_bundleNameStack.Add(name);
		}

		private static void PopBundleGroup()
		{
			if (0 > Holder.m_stackIP)
			{
				Holder._CreateDefaultStack();
			}
			else
			{
				Dictionary<string, WWWItem> dictionary = Holder.m_bundleGroupStack[Holder.m_stackIP];
				foreach (KeyValuePair<string, WWWItem> current in dictionary)
				{
					Holder._UnloadAssetBundle(current.Value, true);
				}
				dictionary.Clear();
				Holder.m_bundleGroupStack.RemoveAt(Holder.m_stackIP);
				Holder.m_bundleNameStack.RemoveAt(Holder.m_stackIP);
				Holder.m_stackIP--;
			}
		}

		public static void ClearStackAll(bool clearMemory)
		{
			foreach (string current in Holder.m_bundleNameStack)
			{
				Holder.ClearStackItem(current, clearMemory);
			}
		}

		public static void ClearStackItem(string name, bool clearMemory)
		{
			if (0 > Holder.m_stackIP)
			{
				Holder._CreateDefaultStack();
			}
			name = name.ToLower();
			bool flag = false;
			Dictionary<string, WWWItem> dictionary = null;
			for (int i = Holder.m_stackIP; i >= 0; i--)
			{
				string b = Holder.m_bundleNameStack[i].ToLower();
				if (name == b)
				{
					dictionary = Holder.m_bundleGroupStack[i];
					foreach (KeyValuePair<string, WWWItem> current in dictionary)
					{
						Holder._UnloadAssetBundle(current.Value, clearMemory);
					}
					dictionary.Clear();
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TsLog.Log("[TsBundle] ClearStackItem() Cannot found StackName= {0}", new object[]
				{
					name
				});
			}
		}

		[Obsolete]
		private static int _GetStackIdx(string stackName)
		{
			if (!string.IsNullOrEmpty(stackName))
			{
				for (int i = 0; i <= Holder.m_stackIP; i++)
				{
					if (stackName.Equals(Holder.m_bundleNameStack[i], StringComparison.OrdinalIgnoreCase))
					{
						return i;
					}
				}
			}
			return Holder.m_stackIP;
		}

		private static Dictionary<string, WWWItem> _FindStack(string stackName)
		{
			if (stackName == Option.undefinedStackName)
			{
				return Holder.ms_UndefinedStack;
			}
			if (!string.IsNullOrEmpty(stackName))
			{
				for (int i = 0; i <= Holder.m_stackIP; i++)
				{
					if (stackName.Equals(Holder.m_bundleNameStack[i], StringComparison.OrdinalIgnoreCase))
					{
						return Holder.m_bundleGroupStack[i];
					}
				}
			}
			return Holder.m_bundleGroupStack[Holder.m_stackIP];
		}

		public static List<WWWItem> _InternalOnly_GetAllBundles()
		{
			int num = 0;
			foreach (Dictionary<string, WWWItem> current in Holder.m_bundleGroupStack)
			{
				num += current.Count;
			}
			if (num <= 0)
			{
				return null;
			}
			List<WWWItem> list = new List<WWWItem>(num);
			foreach (Dictionary<string, WWWItem> current2 in Holder.m_bundleGroupStack)
			{
				list.AddRange(current2.Values);
			}
			return list;
		}

		internal static WWW CancelPreDownload(string lowerCaseUrl)
		{
			WWWItem wWWItem = null;
			if (Holder.ms_UndefinedStack.TryGetValue(lowerCaseUrl, out wWWItem))
			{
				if (Option.EnableTrace)
				{
					TsLog.Log("[TsBundle] Cancel to download (AssetPath=\"{0}\", Stack=\"{1}\", Type={2})", new object[]
					{
						lowerCaseUrl,
						wWWItem.stackName,
						wWWItem.itemType
					});
				}
				return wWWItem.DeliveryWWW();
			}
			return null;
		}

		public static void RemoveWWWItem(string key, bool clearMemory)
		{
			key = key.ToLower();
			WWWItem wWWItem = null;
			for (int i = Holder.m_stackIP; i >= 0; i--)
			{
				Dictionary<string, WWWItem> dictionary = Holder.m_bundleGroupStack[i];
				dictionary.TryGetValue(key, out wWWItem);
				if (wWWItem != null)
				{
					wWWItem.refCnt--;
					if (clearMemory || 0 >= wWWItem.refCnt)
					{
						Holder._UnloadAssetBundle(wWWItem, clearMemory);
						dictionary.Remove(key);
					}
				}
			}
		}

		public static void RemoveWWWItem(WWWItem wItem, bool clearMemory)
		{
			if (wItem.inUndefinedStack)
			{
				string key = wItem.assetPath.ToLower();
				if (Holder.ms_UndefinedStack.ContainsKey(key))
				{
					wItem.refCnt--;
					if (clearMemory || 0 >= wItem.refCnt)
					{
						Holder._UnloadAssetBundle(wItem, clearMemory);
						Holder.ms_UndefinedStack.Remove(key);
					}
				}
			}
			else
			{
				Holder.RemoveWWWItem(wItem.assetPath, clearMemory);
			}
		}

		public static void RemoveWWWItem(IDownloadedItem item, bool clearMemory)
		{
			if (item is WWWItem)
			{
				Holder.RemoveWWWItem(item as WWWItem, clearMemory);
			}
			else
			{
				Holder.RemoveWWWItem(item.assetPath, clearMemory);
			}
		}

		private static void _AddTo(string stackName, WWWItem value)
		{
			Dictionary<string, WWWItem> dictionary = Holder._FindStack(stackName);
			if (dictionary.ContainsKey(value.assetPath))
			{
				TsPlatform.FileLog("already added key = " + value.assetPath + ", Name = " + value.assetName);
				return;
			}
			dictionary.Add(value.assetPath, value);
		}

		private static void _UnloadAssetBundle(WWWItem wItem, bool clearMemory)
		{
			if (wItem.isCreated)
			{
				try
				{
					if (wItem.canAccessAssetBundle)
					{
						wItem.UnloadSafeBundle(clearMemory);
					}
					else if (Option.EnableTrace)
					{
						TsLog.Log("[TsBundle] www unload (<<No-AssetBundle>> AssetPath=\"{0}\", Stack=\"{1}\", Type={2})", new object[]
						{
							wItem.assetPath,
							wItem.stackName,
							wItem.itemType
						});
					}
					wItem.Dispose();
				}
				catch (Exception obj)
				{
					TsLog.LogWarning(obj);
				}
			}
			wItem._InternalOnly_ChangeStateCancel();
		}

		internal static bool IsLoadedBundle(string key)
		{
			foreach (KeyValuePair<string, Dictionary<string, WWWItem>> current in Holder._EnumBundleGroupList())
			{
				if (current.Value.ContainsKey(key))
				{
					return true;
				}
			}
			return false;
		}

		public static string DbgPrint_BundleList()
		{
			return Holder.DbgPrint_BundleList(null);
		}

		private static int _GetBundleGroupCount()
		{
			return Holder.m_bundleGroupStack.Count + 1;
		}

		[DebuggerHidden]
		private static IEnumerable<KeyValuePair<string, Dictionary<string, WWWItem>>> _EnumBundleGroupList()
		{
			Holder.<_EnumBundleGroupList>c__Iterator1F <_EnumBundleGroupList>c__Iterator1F = new Holder.<_EnumBundleGroupList>c__Iterator1F();
			Holder.<_EnumBundleGroupList>c__Iterator1F expr_07 = <_EnumBundleGroupList>c__Iterator1F;
			expr_07.$PC = -2;
			return expr_07;
		}

		public static string DbgPrint_BundleList(string stackName)
		{
			int num = 0;
			Holder.m_DebugMessage.Length = 0;
			if (stackName == null)
			{
				Holder.m_DebugMessage.AppendLine("=======================================>");
				Holder.m_DebugMessage.AppendFormat("Bundle Stack Total Count({0}) CurIdx({1}) UseCache({2})\r\n", Holder._GetBundleGroupCount(), Holder.m_stackIP, Option.useCache);
			}
			else
			{
				Holder.m_DebugMessage.AppendLine("<< AssetBundles in stack >>");
			}
			foreach (KeyValuePair<string, Dictionary<string, WWWItem>> current in Holder._EnumBundleGroupList())
			{
				string key = current.Key;
				Dictionary<string, WWWItem> value = current.Value;
				if (stackName == null || stackName == key.ToLower())
				{
					Holder.m_DebugMessage.AppendLine("----------------------------------------");
					Holder.m_DebugMessage.AppendFormat("Holder = {0}, Cnt({1})\r\n", key, value.Count);
					int num2 = 0;
					foreach (KeyValuePair<string, WWWItem> current2 in value)
					{
						num2++;
						string key2 = current2.Key;
						WWWItem value2 = current2.Value;
						int safeSize = value2.safeSize;
						Holder.m_DebugMessage.AppendFormat("- {0}: Cache(Use:{1}, Hit:{2}, refCnt:{3}, Size:{4}) : {5}\r\n", new object[]
						{
							value2.stateString,
							value2.useLoadFromCacheOrDownload,
							value2.isCacheHit,
							value2.refCnt,
							(safeSize != 0) ? safeSize.ToString("###,###,###,###") : "0",
							key2
						});
						num += safeSize;
					}
				}
			}
			Holder.m_DebugMessage.AppendLine("----------------------------------------");
			Holder.m_DebugMessage.AppendFormat("Total size = {0} bytes\r\n", num.ToString("###,###,###,###"));
			return Holder.m_DebugMessage.ToString();
		}

		public static string DbgPrint_BundleCount()
		{
			int num = 0;
			Holder.m_DebugMessage.Length = 0;
			Holder.m_DebugMessage.AppendLine("=======================================>");
			Holder.m_DebugMessage.AppendFormat("Bundle Stack Total Count({0}) CurIdx({1}) UseCache({1})\r\n", Holder._GetBundleGroupCount(), Holder.m_stackIP, Option.useCache);
			foreach (KeyValuePair<string, Dictionary<string, WWWItem>> current in Holder._EnumBundleGroupList())
			{
				string key = current.Key;
				Dictionary<string, WWWItem> value = current.Value;
				num += value.Count;
				Holder.m_DebugMessage.AppendFormat("Holder=\"{0}\", Cnt({1})\r\n", key, value.Count);
			}
			Holder.m_DebugMessage.AppendLine("----------------------------------------");
			Holder.m_DebugMessage.AppendFormat("Total bundle count = {0}\r\n", (num != 0) ? num.ToString("###,###,###,###") : "0");
			return Holder.m_DebugMessage.ToString();
		}

		public static string DbgPrint_Downloaded()
		{
			Holder.m_DebugMessage.AppendFormat("Total Downloaded bundle list : {0}\r\n", Holder.m_dbgBS.Count);
			int num = 0;
			foreach (Holder.DbgBS current in Holder.m_dbgBS.Values)
			{
				num += current.size;
				Holder.m_DebugMessage.AppendFormat("- {0} (Size={1})\r\n", current.key, current.size);
			}
			Holder.m_DebugMessage.AppendFormat("Total Downloaded Size = {0}\r\n", num);
			Holder.m_DebugMessage.AppendLine("<=======================================");
			return Holder.m_DebugMessage.ToString();
		}

		public static void DbgAddWWWItemStat(string requestedURL, int size)
		{
			Holder.DbgBS dbgBS;
			if (Holder.m_dbgBS.TryGetValue(requestedURL, out dbgBS))
			{
				dbgBS.SetSize(size);
			}
			else
			{
				Holder.m_dbgBS.Add(requestedURL, new Holder.DbgBS(requestedURL, size));
			}
		}
	}
}
