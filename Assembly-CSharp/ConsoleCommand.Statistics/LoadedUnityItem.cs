using Ndoors.Memory;
using System;

namespace ConsoleCommand.Statistics
{
	[ObjectPool(typeof(LoadedUnityItem), 128, 512, 20000)]
	internal sealed class LoadedUnityItem : IDisposable, IComparable<LoadedUnityItem>, IPoolObject
	{
		public int hitCount;

		public int objectBytes;

		public Type objectType;

		public string objectName;

		public string subDesc;

		private LoadedUnityItem()
		{
		}

		void IPoolObject.OnCreate(object param)
		{
		}

		void IPoolObject.OnDelete()
		{
		}

		int IComparable<LoadedUnityItem>.CompareTo(LoadedUnityItem other)
		{
			return other.objectBytes - this.objectBytes;
		}

		private static object CreateInstanceStaicPrivate()
		{
			return new LoadedUnityItem();
		}

		public static LoadedUnityItem Instantiate(Type type, string name, int size, string additonalDesc = null)
		{
			LoadedUnityItem loadedUnityItem = ObjectPoolManager.Acquire<LoadedUnityItem>();
			loadedUnityItem.objectType = type;
			loadedUnityItem.objectName = name;
			loadedUnityItem.hitCount = 0;
			loadedUnityItem.objectBytes = size;
			loadedUnityItem.subDesc = additonalDesc;
			return loadedUnityItem;
		}

		public void Dispose()
		{
			ObjectPoolManager.Release<LoadedUnityItem>(this);
		}

		public override string ToString()
		{
			return string.Format("[{0}] \"{1}\" <{2} Bytes> {3}", new object[]
			{
				this.objectType,
				this.objectName,
				(this.objectBytes != 0) ? this.objectBytes.ToString("###,###,###,###") : "?",
				(this.subDesc == null) ? string.Empty : this.subDesc
			});
		}
	}
}
