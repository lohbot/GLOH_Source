using NetServ.Net.Json;
using System;

namespace NXBand
{
	public class NXBandTypeQuota
	{
		public int total;

		public int remaining;

		public int expiredIn;

		public NXBandTypeQuota(JsonObject json)
		{
			if (json.ContainsKey("total"))
			{
				this.total = (int)((JsonNumber)json["total"]).Value;
			}
			if (json.ContainsKey("remaining"))
			{
				this.remaining = (int)((JsonNumber)json["remaining"]).Value;
			}
			if (json.ContainsKey("expired_in"))
			{
				this.expiredIn = (int)((JsonNumber)json["expired_in"]).Value;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"total : <",
				this.total,
				">, remaining : <",
				this.remaining,
				">, expired in(sec) : <",
				this.expiredIn,
				">"
			});
		}
	}
}
