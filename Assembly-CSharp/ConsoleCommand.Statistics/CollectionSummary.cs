using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleCommand.Statistics
{
	internal class CollectionSummary
	{
		internal Dictionary<int, LoadedUnityItem> items;

		private List<LoadedUnityItem> sortedItems;

		public int collectedSize
		{
			get;
			internal set;
		}

		public Type collectionType
		{
			get;
			private set;
		}

		public int cllectedCount
		{
			get
			{
				return this.items.Count;
			}
		}

		public CollectionSummary(Type type, int capacity)
		{
			this.collectionType = type;
			this.items = new Dictionary<int, LoadedUnityItem>(capacity);
		}

		public void Sort()
		{
			if (this.sortedItems == null)
			{
				this.sortedItems = new List<LoadedUnityItem>();
			}
			this.sortedItems.Clear();
			this.sortedItems.Capacity = this.items.Count;
			foreach (LoadedUnityItem current in this.items.Values)
			{
				this.sortedItems.Add(current);
			}
			this.sortedItems.Sort();
		}

		[DebuggerHidden]
		public IEnumerable<LoadedUnityItem> TopRanker(int count, bool newlyItemOnly = false)
		{
			CollectionSummary.<TopRanker>c__Iterator62 <TopRanker>c__Iterator = new CollectionSummary.<TopRanker>c__Iterator62();
			<TopRanker>c__Iterator.count = count;
			<TopRanker>c__Iterator.newlyItemOnly = newlyItemOnly;
			<TopRanker>c__Iterator.<$>count = count;
			<TopRanker>c__Iterator.<$>newlyItemOnly = newlyItemOnly;
			<TopRanker>c__Iterator.<>f__this = this;
			CollectionSummary.<TopRanker>c__Iterator62 expr_2A = <TopRanker>c__Iterator;
			expr_2A.$PC = -2;
			return expr_2A;
		}

		public int GetNewlyObjectTotalSize()
		{
			int num = 0;
			foreach (LoadedUnityItem current in this.items.Values)
			{
				if (current.hitCount == 0)
				{
					num += current.objectBytes;
				}
			}
			return num;
		}

		public int GetLeakObjectTotalSize()
		{
			int num = 0;
			foreach (LoadedUnityItem current in this.items.Values)
			{
				if (current.hitCount > 0)
				{
					num += current.objectBytes;
				}
			}
			return num;
		}
	}
}
