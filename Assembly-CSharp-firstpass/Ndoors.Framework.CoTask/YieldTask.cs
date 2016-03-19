using System;
using System.Collections;

namespace Ndoors.Framework.CoTask
{
	public class YieldTask
	{
		protected IEnumerator _routine;

		public bool MoveNext()
		{
			YieldTask yieldTask = this._routine.Current as YieldTask;
			if (yieldTask != null)
			{
				return yieldTask.MoveNext() || this._routine.MoveNext();
			}
			return this._routine.MoveNext();
		}

		public virtual string ToStringStatus(string indent)
		{
			return string.Format("{0}{1}", indent, this._routine.GetType());
		}
	}
}
