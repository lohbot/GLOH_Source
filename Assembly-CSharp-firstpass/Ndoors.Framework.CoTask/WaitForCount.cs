using System;
using System.Collections;
using System.Diagnostics;

namespace Ndoors.Framework.CoTask
{
	public class WaitForCount : BaseCoTask
	{
		private int _count;

		public WaitForCount(int cnt)
		{
			this._count = cnt;
			this._routine = this.Count();
		}

		[DebuggerHidden]
		private IEnumerator Count()
		{
			WaitForCount.<Count>c__Iterator11 <Count>c__Iterator = new WaitForCount.<Count>c__Iterator11();
			<Count>c__Iterator.<>f__this = this;
			return <Count>c__Iterator;
		}

		public override string ToStringStatus(string indent)
		{
			return string.Format("{0}WaitCount: {1}", indent, this._count);
		}
	}
}
