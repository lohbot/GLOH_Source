using System;
using System.Collections;

namespace NXBand
{
	public class BandListOption
	{
		public enum Domain
		{
			BAND,
			MEMBER
		}

		public enum SortingOrder
		{
			ASC,
			DESC
		}

		private class BandAttribute
		{
			private BandListOption.Domain _domain;

			private string _attribute;

			private object _value;

			public BandAttribute(BandListOption.Domain domain, string attribute, object value)
			{
				this._domain = domain;
				this._attribute = attribute;
				this._value = value;
			}

			public string getJsonString()
			{
				return string.Concat(new object[]
				{
					"{\"domain\":",
					(int)this._domain,
					",\"attribute\":\"",
					this._attribute,
					"\",\"value\":",
					this._value.ToString(),
					"}"
				});
			}
		}

		public static string BAND_KEY = "band_key";

		public static string BAND_NAME = "name";

		public static string BAND_IS_GUILDBAND = "is_guild_band";

		public static string BAND_IS_SCHOOLBAND = "is_school_band";

		public static string BAND_MEMBERCOUNT = "member_count";

		public static string MEMBER_KEY = "user_key";

		public static string MEMBER_NAME = "name";

		public static string MEMBER_MESSAGE_ALLOWED = "message_allowed";

		public static string MEMBER_IS_APPMEMBER = "is_app_member";

		private int _currentPage = 1;

		private int _pageSize = -1;

		private BandListOption.BandAttribute _sorting;

		private ArrayList _filter;

		public BandListOption setPage(int page)
		{
			this._currentPage = page;
			return this;
		}

		public void setNextPage()
		{
			this._currentPage++;
		}

		public void setInitPage()
		{
			this._currentPage = 1;
		}

		public void setPageSize(int size)
		{
			this._pageSize = size;
		}

		public BandListOption addFilter(BandListOption.Domain domain, string attribute, object value)
		{
			if (this._filter == null)
			{
				this._filter = new ArrayList();
			}
			this._filter.Add(new BandListOption.BandAttribute(domain, attribute, value));
			return this;
		}

		public void setSorting(BandListOption.Domain domain, string attribute, BandListOption.SortingOrder order)
		{
			this._sorting = new BandListOption.BandAttribute(domain, attribute, (int)order);
		}

		public string getJsonString()
		{
			if (this._sorting == null && this._filter == null && this._currentPage == 1 && this._pageSize <= 0)
			{
				return null;
			}
			bool flag = false;
			string text = "{";
			if (this._sorting != null)
			{
				text = text + "\"sorting\":" + this._sorting.getJsonString();
				flag = true;
			}
			if (this._filter != null && this._filter.Count > 0)
			{
				if (flag)
				{
					text += ",";
				}
				text += "\"filter\":[";
				for (int i = 0; i < this._filter.Count; i++)
				{
					text += ((BandListOption.BandAttribute)this._filter[i]).getJsonString();
					if (i < this._filter.Count - 1)
					{
						text += ",";
					}
				}
				text += "]";
				flag = true;
			}
			if (this._currentPage != 1)
			{
				if (flag)
				{
					text += ",";
				}
				text = text + "\"page\":" + this._currentPage;
				flag = true;
			}
			if (this._pageSize > 0)
			{
				if (flag)
				{
					text += ",";
				}
				text = text + "\"page_size\":" + this._pageSize;
			}
			return text + "}";
		}
	}
}
