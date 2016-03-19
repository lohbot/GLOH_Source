using NetServ.Net.Json;
using System;

namespace NXBand
{
	public class NXBandTypeCache
	{
		public bool complete;

		public int total;

		public int cached;

		public NXBandTypeCache(JsonObject json)
		{
			if (json.ContainsKey("total"))
			{
				this.total = (int)((JsonNumber)json["total"]).Value;
			}
			if (json.ContainsKey("cached"))
			{
				this.cached = (int)((JsonNumber)json["cached"]).Value;
			}
			if (json.ContainsKey("complete"))
			{
				this.complete = ((JsonBoolean)json["complete"]).Value;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"complete : <",
				this.complete,
				">, total : <",
				this.total,
				">, cached : <",
				this.cached,
				">"
			});
		}
	}
}
