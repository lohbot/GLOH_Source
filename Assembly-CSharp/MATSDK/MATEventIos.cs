using System;

namespace MATSDK
{
	internal struct MATEventIos
	{
		public string name;

		public string eventId;

		public string revenue;

		public string currencyCode;

		public string advertiserRefId;

		public string transactionState;

		public string contentType;

		public string contentId;

		public string level;

		public string quantity;

		public string searchString;

		public string rating;

		public string date1;

		public string date2;

		public string attribute1;

		public string attribute2;

		public string attribute3;

		public string attribute4;

		public string attribute5;

		private MATEventIos(int dummy1, int dummy2)
		{
			this.eventId = null;
			this.name = null;
			this.revenue = null;
			this.currencyCode = null;
			this.advertiserRefId = null;
			this.transactionState = null;
			this.contentType = null;
			this.contentId = null;
			this.level = null;
			this.quantity = null;
			this.searchString = null;
			this.rating = null;
			this.date1 = null;
			this.date2 = null;
			this.attribute1 = null;
			this.attribute2 = null;
			this.attribute3 = null;
			this.attribute4 = null;
			this.attribute5 = null;
		}

		public MATEventIos(string name)
		{
			this = new MATEventIos(0, 0);
			this.name = name;
		}

		public MATEventIos(int id)
		{
			this = new MATEventIos(0, 0);
			this.eventId = id.ToString();
		}

		public MATEventIos(MATEvent matEvent)
		{
			this.name = matEvent.name;
			this.eventId = ((matEvent.name != null) ? null : matEvent.id.ToString());
			this.advertiserRefId = matEvent.advertiserRefId;
			this.attribute1 = matEvent.attribute1;
			this.attribute2 = matEvent.attribute2;
			this.attribute3 = matEvent.attribute3;
			this.attribute4 = matEvent.attribute4;
			this.attribute5 = matEvent.attribute5;
			this.contentId = ((matEvent.contentId != null) ? matEvent.contentId.ToString() : null);
			this.contentType = matEvent.contentType;
			this.currencyCode = matEvent.currencyCode;
			int? num = matEvent.level;
			this.level = (num.HasValue ? matEvent.level.ToString() : null);
			int? num2 = matEvent.quantity;
			this.quantity = (num2.HasValue ? matEvent.quantity.ToString() : null);
			double? num3 = matEvent.rating;
			this.rating = (num3.HasValue ? matEvent.rating.ToString() : null);
			double? num4 = matEvent.revenue;
			this.revenue = (num4.HasValue ? matEvent.revenue.ToString() : null);
			this.searchString = matEvent.searchString;
			int? num5 = matEvent.transactionState;
			this.transactionState = (num5.HasValue ? matEvent.transactionState.ToString() : null);
			this.date1 = null;
			this.date2 = null;
			DateTime dateTime = new DateTime(1970, 1, 1);
			if (matEvent.date1.HasValue)
			{
				TimeSpan timeSpan = new TimeSpan(matEvent.date1.Value.Ticks);
				double totalMilliseconds = timeSpan.TotalMilliseconds;
				double arg_221_0 = totalMilliseconds;
				TimeSpan timeSpan2 = new TimeSpan(dateTime.Ticks);
				this.date1 = (arg_221_0 - timeSpan2.TotalMilliseconds).ToString();
			}
			if (matEvent.date2.HasValue)
			{
				TimeSpan timeSpan3 = new TimeSpan(matEvent.date2.Value.Ticks);
				double totalMilliseconds2 = timeSpan3.TotalMilliseconds;
				double arg_27B_0 = totalMilliseconds2;
				TimeSpan timeSpan4 = new TimeSpan(dateTime.Ticks);
				this.date2 = (arg_27B_0 - timeSpan4.TotalMilliseconds).ToString();
			}
		}
	}
}
