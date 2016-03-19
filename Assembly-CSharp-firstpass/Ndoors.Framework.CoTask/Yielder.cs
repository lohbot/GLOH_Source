using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ndoors.Framework.CoTask
{
	public class Yielder
	{
		private Queue<BaseCoTask> _coTasks = new Queue<BaseCoTask>();

		private List<BaseCoTask> _shouldRunNext = new List<BaseCoTask>();

		public int CountCoTask
		{
			get;
			private set;
		}

		public void StartTaskPararell(IEnumerator routine)
		{
			this.CountCoTask++;
			this._shouldRunNext.Add(new BaseCoTask(routine));
		}

		public void StartTaskPararell(BaseCoTask coTsk)
		{
			this.CountCoTask++;
			this._shouldRunNext.Add(coTsk);
		}

		protected bool ProcessCoTasksInternal()
		{
			int count = this._coTasks.Count;
			for (int i = 0; i < count; i++)
			{
				BaseCoTask baseCoTask = this._coTasks.Dequeue();
				if (baseCoTask.MoveNext())
				{
					this._coTasks.Enqueue(baseCoTask);
				}
			}
			foreach (BaseCoTask current in this._shouldRunNext)
			{
				this._coTasks.Enqueue(current);
			}
			this._shouldRunNext.Clear();
			this.CountCoTask = this._coTasks.Count;
			if (this._coTasks.Count > 0)
			{
				return true;
			}
			this._coTasks.Clear();
			return false;
		}

		public void ResetCoTasks()
		{
			this._coTasks.Clear();
			this._shouldRunNext.Clear();
		}

		public virtual string ToStringStatus(string indent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			indent += "\t";
			foreach (BaseCoTask current in this._coTasks)
			{
				stringBuilder.AppendLine(current.ToStringStatus(indent));
			}
			foreach (BaseCoTask current2 in this._shouldRunNext)
			{
				stringBuilder.AppendLine(current2.ToStringStatus(indent));
			}
			return stringBuilder.ToString();
		}
	}
}
