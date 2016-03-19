using System;
using System.Collections.Generic;

namespace omniata
{
	public class Queue
	{
		private const string TAG = "Queue";

		private int ElementCount
		{
			get;
			set;
		}

		private Storage Storage
		{
			get;
			set;
		}

		public Queue(string persistentDataPath)
		{
			this.Storage = new Storage(persistentDataPath);
			this.ElementCount = this.Storage.Read().Count;
			Log.Debug("Queue", "loaded, count: " + this.ElementCount);
		}

		public void Put(QueueElement element)
		{
			List<QueueElement> list = this.Storage.Read();
			list.Add(element);
			this.Storage.Write(list);
			this.ElementCount++;
		}

		public void Prepend(QueueElement element)
		{
			List<QueueElement> list = this.Storage.Read();
			list.Insert(0, element);
			this.Storage.Write(list);
			this.ElementCount++;
		}

		public QueueElement Peek()
		{
			if (this.ElementCount == 0)
			{
				return null;
			}
			List<QueueElement> list = this.Storage.Read();
			return list[0];
		}

		public QueueElement Take()
		{
			if (this.ElementCount == 0)
			{
				throw new InvalidOperationException("Queue is empty");
			}
			List<QueueElement> list = this.Storage.Read();
			QueueElement result = list[0];
			list.RemoveAt(0);
			this.Storage.Write(list);
			this.ElementCount--;
			return result;
		}

		public int Count()
		{
			return this.ElementCount;
		}
	}
}
