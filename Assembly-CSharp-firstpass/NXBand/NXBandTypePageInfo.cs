using NetServ.Net.Json;
using System;

namespace NXBand
{
	public class NXBandTypePageInfo
	{
		public int page;

		public int pageSize;

		public int total;

		public NXBandTypePageInfo(JsonObject json)
		{
			if (json.ContainsKey("page"))
			{
				this.page = (int)((JsonNumber)json["page"]).Value;
			}
			if (json.ContainsKey("page_size"))
			{
				this.pageSize = (int)((JsonNumber)json["page_size"]).Value;
			}
			if (json.ContainsKey("total"))
			{
				this.total = (int)((JsonNumber)json["total"]).Value;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"page : <",
				this.page,
				">, pageSize : <",
				this.pageSize,
				">, total : <",
				this.total,
				">"
			});
		}
	}
}
