using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ndoors.Framework.CoTask
{
	public class WaitSerialTasks : BaseCoTask
	{
		internal Queue<BaseCoTask> _queue = new Queue<BaseCoTask>();

		public int Count
		{
			get
			{
				return this._queue.Count;
			}
		}

		public WaitSerialTasks()
		{
			this.Initialize();
		}

		public void Add(IEnumerator ie)
		{
			this._queue.Enqueue(new WaitTask(ie));
		}

		public void Add(BaseCoTask ct)
		{
			this._queue.Enqueue(ct);
		}

		public void AddFirst(IEnumerator ie)
		{
			int count = this._queue.Count;
			this._queue.Enqueue(new WaitTask(ie));
			while (count-- > 0)
			{
				this._queue.Enqueue(this._queue.Dequeue());
			}
		}

		internal void Initialize()
		{
			this._queue.Clear();
			this._routine = this.SerialDo();
			this._routine.MoveNext();
		}

		[DebuggerHidden]
		private IEnumerator SerialDo()
		{
			WaitSerialTasks.<SerialDo>c__Iterator12 <SerialDo>c__Iterator = new WaitSerialTasks.<SerialDo>c__Iterator12();
			<SerialDo>c__Iterator.<>f__this = this;
			return <SerialDo>c__Iterator;
		}

		public override string ToStringStatus(string indent)
		{
			string result;
			if (this._queue == null || this._queue.Count == 0)
			{
				result = string.Format("{0}WaitSerialTasks:0", indent);
			}
			else
			{
				BaseCoTask baseCoTask = this._queue.Peek();
				result = string.Format("{0}WaitSerialTasks:{1} Current\n{2}", indent, this._queue.Count, (baseCoTask == null) ? "NULL" : baseCoTask.ToStringStatus(indent + "\t"));
			}
			return result;
		}
	}
}
