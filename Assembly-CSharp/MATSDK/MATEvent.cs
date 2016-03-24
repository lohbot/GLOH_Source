using System;

namespace MATSDK
{
	public struct MATEvent
	{
		public string name;

		public int? id;

		public double? revenue;

		public string currencyCode;

		public string advertiserRefId;

		public MATItem[] eventItems;

		public int? transactionState;

		public string receipt;

		public string receiptSignature;

		public string contentType;

		public string contentId;

		public int? level;

		public int? quantity;

		public string searchString;

		public double? rating;

		public DateTime? date1;

		public DateTime? date2;

		public string attribute1;

		public string attribute2;

		public string attribute3;

		public string attribute4;

		public string attribute5;

		private MATEvent(int dummy1, int dummy2)
		{
			this.name = null;
			this.id = null;
			this.revenue = null;
			this.currencyCode = null;
			this.advertiserRefId = null;
			this.eventItems = null;
			this.transactionState = null;
			this.receipt = null;
			this.receiptSignature = null;
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

		public MATEvent(string name)
		{
			this = new MATEvent(0, 0);
			this.name = name;
		}

		public MATEvent(int id)
		{
			this = new MATEvent(0, 0);
			this.id = new int?(id);
		}
	}
}
