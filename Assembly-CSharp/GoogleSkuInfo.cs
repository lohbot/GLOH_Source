using System;
using System.Collections.Generic;

public class GoogleSkuInfo
{
	public string title
	{
		get;
		private set;
	}

	public string price
	{
		get;
		private set;
	}

	public string type
	{
		get;
		private set;
	}

	public string description
	{
		get;
		private set;
	}

	public string productId
	{
		get;
		private set;
	}

	public string priceCurrencyCode
	{
		get;
		private set;
	}

	public string priceAmountMicros
	{
		get;
		private set;
	}

	public GoogleSkuInfo(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("title"))
		{
			this.title = (dict["title"] as string);
		}
		if (dict.ContainsKey("price"))
		{
			this.price = (dict["price"] as string);
		}
		if (dict.ContainsKey("type"))
		{
			this.type = (dict["type"] as string);
		}
		if (dict.ContainsKey("description"))
		{
			this.description = (dict["description"] as string);
		}
		if (dict.ContainsKey("productId"))
		{
			this.productId = (dict["productId"] as string);
		}
		if (dict.ContainsKey("price_currency_code"))
		{
			this.priceCurrencyCode = (dict["price_currency_code"] as string);
		}
		if (dict.ContainsKey("price_amount_micros"))
		{
			this.priceAmountMicros = (dict["price_amount_micros"] as string);
		}
	}

	public static List<GoogleSkuInfo> fromList(List<object> items)
	{
		List<GoogleSkuInfo> list = new List<GoogleSkuInfo>();
		using (List<object>.Enumerator enumerator = items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Dictionary<string, object> dict = (Dictionary<string, object>)enumerator.Current;
				list.Add(new GoogleSkuInfo(dict));
			}
		}
		return list;
	}

	public override string ToString()
	{
		return string.Format("<GoogleSkuInfo> title: {0}, price: {1}, type: {2}, description: {3}, productId: {4}, priceCurrencyCode: {5}", new object[]
		{
			this.title,
			this.price,
			this.type,
			this.description,
			this.productId,
			this.priceCurrencyCode
		});
	}
}
