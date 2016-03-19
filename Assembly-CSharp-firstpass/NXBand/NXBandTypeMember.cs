using NetServ.Net.Json;
using System;

namespace NXBand
{
	public class NXBandTypeMember
	{
		public string userKey;

		public string profileImageUrl;

		public string name;

		public bool isAppMember;

		public bool messageAllowed;

		public int Level;

		public int nFaceCharKind;

		public string strGameName = string.Empty;

		public bool isImageCache;

		public NXBandTypeMember(JsonObject json)
		{
			if (json.ContainsKey("user_key"))
			{
				this.userKey = ((JsonString)json["user_key"]).Value;
			}
			if (json.ContainsKey("profile_image_url"))
			{
				this.profileImageUrl = ((JsonString)json["profile_image_url"]).Value;
			}
			if (json.ContainsKey("name"))
			{
				this.name = ((JsonString)json["name"]).Value;
			}
			if (json.ContainsKey("is_app_member"))
			{
				this.isAppMember = ((JsonBoolean)json["is_app_member"]).Value;
			}
			if (json.ContainsKey("message_allowed"))
			{
				this.messageAllowed = ((JsonBoolean)json["message_allowed"]).Value;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"userKey : <",
				this.userKey,
				">, profileImageUrl : <",
				this.profileImageUrl,
				">, name : <",
				this.name,
				">, isAppMember : <",
				this.isAppMember,
				">, messageAllowed : <",
				this.messageAllowed,
				">"
			});
		}
	}
}
