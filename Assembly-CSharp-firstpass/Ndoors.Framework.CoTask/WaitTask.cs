using System;
using System.Collections;

namespace Ndoors.Framework.CoTask
{
	public class WaitTask : BaseCoTask
	{
		public WaitTask(IEnumerator routine) : base(routine)
		{
		}
	}
}
