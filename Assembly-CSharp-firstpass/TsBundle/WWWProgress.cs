using System;
using System.Collections.Generic;

namespace TsBundle
{
	public class WWWProgress
	{
		private List<WWWItem> _wList;

		private int curCnt;

		private float totProg;

		private float curProg;

		private int downFilesize;

		public float progress
		{
			get
			{
				return (this.curProg + this.progressOneItem / this.totProg) / this.totProg;
			}
		}

		public float progressOneItem
		{
			get
			{
				float result = 0f;
				if (this._wList == null || this._wList.Count == 0)
				{
					result = 1f;
				}
				else if (this._wList.Count > this.curCnt)
				{
					try
					{
						result = this._wList[this.curCnt].progress;
					}
					catch (Exception)
					{
						result = 1f;
					}
				}
				return result;
			}
		}

		public WWWProgress(List<WWWItem> wList)
		{
			this._wList = wList;
			this.totProg = (float)wList.Count;
			this.curProg = 0f;
			this.curCnt = 0;
		}

		public WWWProgress(float totItemCnt)
		{
			this.totProg = totItemCnt;
			this.curProg = 0f;
			this.curCnt = 0;
		}

		public void AddCompletionCnt()
		{
			if (this._wList == null)
			{
				return;
			}
			if (this._wList.Count > this.curCnt)
			{
				this.downFilesize += this._wList[this.curCnt].filesize / 1024;
			}
			this.curCnt++;
			this.curProg += 1f;
		}

		public int GetDownFileSize()
		{
			return this.downFilesize;
		}

		public float GetTotalCount()
		{
			return this.totProg;
		}

		public int GetCurCount()
		{
			return this.curCnt;
		}
	}
}
