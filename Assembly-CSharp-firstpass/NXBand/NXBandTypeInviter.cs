using NetServ.Net.Json;
using System;

namespace NXBand
{
	public class NXBandTypeInviter
	{
		public enum Type
		{
			TypeChat,
			TypePost
		}

		public string userKey;

		public NXBandTypeInviter.Type type;

		public NXBandTypeInviter(JsonObject json)
		{
			if (json.ContainsKey("user_key"))
			{
				this.userKey = ((JsonString)json["user_key"]).Value;
			}
			if (json.ContainsKey("type"))
			{
				string value = ((JsonString)json["type"]).Value;
				if (value.Equals("chat"))
				{
					this.type = NXBandTypeInviter.Type.TypeChat;
				}
				else if (value.Equals("post"))
				{
					this.type = NXBandTypeInviter.Type.TypePost;
				}
			}
		}

		public bool isInvited()
		{
			return this.userKey != null;
		}

		public override string ToString()
		{
			if (this.isInvited())
			{
				return string.Concat(new object[]
				{
					"userKey : <",
					this.userKey,
					">, type : <",
					this.type,
					">"
				});
			}
			return "not invited user";
		}
	}
}
