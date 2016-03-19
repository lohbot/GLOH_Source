using System;

namespace StageHelper
{
	public struct ProgressCheck
	{
		public int divide;

		public float iprogress;

		public float difference;

		private float ratio;

		public ProgressCheck(int div, float mratio)
		{
			this.divide = div;
			this.iprogress = 0f;
			this.difference = 0f;
			this.ratio = mratio;
		}

		public void Reset(int div, float mratio)
		{
			this.divide = div;
			this.iprogress = 0f;
			this.difference = 0f;
			this.ratio = mratio;
		}

		public float GetUpdateDiff(float prgrs)
		{
			TsLog.Assert(0f <= prgrs && prgrs <= 1f, "PROGRESS value invalid {0}", new object[]
			{
				prgrs
			});
			float num = prgrs * (float)this.divide;
			this.difference = num - this.iprogress;
			float num2 = 1f - this.ratio;
			if (this.difference == 0f && prgrs < num2)
			{
				num += this.ratio * (float)this.divide;
				if (num > num2)
				{
					num = num2;
				}
				this.difference = num - this.iprogress;
			}
			this.iprogress = num;
			return this.difference;
		}
	}
}
