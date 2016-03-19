using System;
using System.Collections;

namespace Ndoors.Framework.CoTask
{
	public class BaseCoTask : YieldTask
	{
		public BaseCoTask()
		{
			this._routine = null;
		}

		public BaseCoTask(IEnumerator routine)
		{
			this._routine = routine;
			if (this._routine != null)
			{
				routine.MoveNext();
			}
		}
	}
}
