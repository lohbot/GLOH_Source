using System;

namespace UnityForms
{
	public class NrPage
	{
		private const int INVALID_PAGE = 0;

		private Button mPreBtn;

		private Button mNextBtn;

		private int mCurrentPage = 1;

		private int mMaxPage;

		private REFRESH_VOIDFUC Refresh;

		public int CURRENT_PAGE
		{
			get
			{
				return this.mCurrentPage;
			}
			set
			{
				this.mCurrentPage = value;
			}
		}

		public int MAX_PAGE
		{
			get
			{
				return (this.mMaxPage != 0) ? this.mMaxPage : 1;
			}
			set
			{
				this.mMaxPage = value;
			}
		}

		public string PAGE_TEXT
		{
			get
			{
				return string.Format("{0}/{1}", this.CURRENT_PAGE, this.MAX_PAGE);
			}
		}

		public NrPage(Button Pre, Button Next)
		{
			this.CreatePage(Pre, Next, null);
		}

		public NrPage(Button Pre, Button Next, REFRESH_VOIDFUC ReflashEvent)
		{
			this.CreatePage(Pre, Next, ReflashEvent);
		}

		public void CreatePage(Button Pre, Button Next, REFRESH_VOIDFUC ReflashEvent)
		{
			this.mPreBtn = Pre;
			this.mNextBtn = Next;
			this.InitEvent(null, null, ReflashEvent);
		}

		public void InitEvent(EZValueChangedDelegate PreEvent, EZValueChangedDelegate NextEvent, REFRESH_VOIDFUC ReflashEvent)
		{
			Button expr_06 = this.mPreBtn;
			expr_06.Click = (EZValueChangedDelegate)Delegate.Combine(expr_06.Click, (EZValueChangedDelegate)Delegate.Combine(PreEvent, new EZValueChangedDelegate(this.Pre_DecrePage)));
			Button expr_38 = this.mNextBtn;
			expr_38.Click = (EZValueChangedDelegate)Delegate.Combine(expr_38.Click, (EZValueChangedDelegate)Delegate.Combine(NextEvent, new EZValueChangedDelegate(this.Next_IncrePage)));
			this.Refresh = ReflashEvent;
		}

		private void CallRefresh()
		{
			if (this.Refresh != null)
			{
				this.Refresh();
			}
		}

		private void Pre_DecrePage(IUIObject obj)
		{
			if (this.mCurrentPage > 1)
			{
				this.mCurrentPage--;
				this.CallRefresh();
			}
		}

		private void Next_IncrePage(IUIObject obj)
		{
			if (0 < this.mMaxPage && this.mCurrentPage < this.mMaxPage)
			{
				this.mCurrentPage++;
				this.CallRefresh();
			}
		}
	}
}
