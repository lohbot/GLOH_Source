using NetServ.Net.Json;
using System;
using System.Collections.Generic;

namespace NXBand
{
	public class NXBandTypeBand
	{
		public string bandKey;

		public string name;

		public int memberCount;

		public bool isGuildBand;

		public bool isSchoolBand;

		public string coverImageUrl;

		public List<NXBandTypeMember> members;

		public NXBandTypeBand(JsonObject json)
		{
			if (json.ContainsKey("band_key"))
			{
				this.bandKey = ((JsonString)json["band_key"]).Value;
			}
			if (json.ContainsKey("name"))
			{
				this.name = ((JsonString)json["name"]).Value;
			}
			if (json.ContainsKey("member_count"))
			{
				this.memberCount = (int)((JsonNumber)json["member_count"]).Value;
			}
			if (json.ContainsKey("is_guild_band"))
			{
				this.isGuildBand = ((JsonBoolean)json["is_guild_band"]).Value;
			}
			if (json.ContainsKey("is_school_band"))
			{
				this.isSchoolBand = ((JsonBoolean)json["is_school_band"]).Value;
			}
			if (json.ContainsKey("cover_image_url"))
			{
				this.coverImageUrl = ((JsonString)json["cover_image_url"]).Value;
			}
			if (json.ContainsKey("members"))
			{
				JsonArray jsonArray = (JsonArray)json["members"];
				this.members = new List<NXBandTypeMember>();
				using (IEnumerator<IJsonType> enumerator = jsonArray.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonObject json2 = (JsonObject)enumerator.Current;
						this.members.Add(new NXBandTypeMember(json2));
					}
				}
			}
		}

		public override string ToString()
		{
			string text = string.Concat(new object[]
			{
				"bandKey : <",
				this.bandKey,
				">, name : <",
				this.name,
				">, memberCount : <",
				this.memberCount,
				">, isGuildBand : <",
				this.isGuildBand,
				">, isSchoolBand : <",
				this.isSchoolBand,
				">, coverImageUrl : <",
				this.coverImageUrl,
				">, members : <"
			});
			if (this.members != null)
			{
				foreach (NXBandTypeMember current in this.members)
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"<",
						current,
						">, "
					});
				}
			}
			text += ">";
			return text;
		}
	}
}
