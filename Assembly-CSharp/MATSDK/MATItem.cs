using System;

namespace MATSDK
{
	public struct MATItem
	{
		public string name;

		public double? unitPrice;

		public int? quantity;

		public double? revenue;

		public string attribute1;

		public string attribute2;

		public string attribute3;

		public string attribute4;

		public string attribute5;

		public MATItem(string name)
		{
			this.name = name;
			this.unitPrice = null;
			this.quantity = null;
			this.revenue = null;
			this.attribute1 = null;
			this.attribute2 = null;
			this.attribute3 = null;
			this.attribute4 = null;
			this.attribute5 = null;
		}
	}
}
