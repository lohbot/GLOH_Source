using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NLibCs
{
	public class NDataTableManager
	{
		public delegate void OnLoadedOneDelegate(bool result);

		protected static NDataTableManager _instance;

		private Dictionary<string, NDataTable> tables = new Dictionary<string, NDataTable>();

		public event NDataTableManager.OnLoadedOneDelegate OnLoadedOne
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.OnLoadedOne = (NDataTableManager.OnLoadedOneDelegate)Delegate.Combine(this.OnLoadedOne, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.OnLoadedOne = (NDataTableManager.OnLoadedOneDelegate)Delegate.Remove(this.OnLoadedOne, value);
			}
		}

		public static NDataTableManager Instance
		{
			get
			{
				if (NDataTableManager._instance == null)
				{
					NDataTableManager._instance = new NDataTableManager();
				}
				return NDataTableManager._instance;
			}
		}

		private void Load()
		{
			foreach (KeyValuePair<string, NDataTable> current in this.tables)
			{
				string key = current.Key;
				NDataTable value = current.Value;
				bool result = value.Load(key);
				this.OnLoadedOne(result);
			}
		}
	}
}
