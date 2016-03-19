using NetServ.Net.Json;
using System;
using System.Collections.Generic;

namespace NXBand
{
	public class NXBandResult
	{
		protected BandResultType _resultType;

		public string message;

		public BandResultType type
		{
			get
			{
				return this._resultType;
			}
		}

		public static NXBandResult makeFromJson(RequestCode reqCode, JsonObject resultObject)
		{
			NXBandResult nXBandResult;
			switch (reqCode)
			{
			case RequestCode.REQUEST_PROFILE:
			case RequestCode.REQUEST_MY_PROFILE:
				nXBandResult = new NXBandResultProfile
				{
					member = new NXBandTypeMember(resultObject)
				};
				break;
			case RequestCode.REQUEST_USER_KEY:
			{
				NXBandResultUserKey nXBandResultUserKey = new NXBandResultUserKey();
				if (resultObject.ContainsKey("message"))
				{
					nXBandResultUserKey.key = ((JsonString)resultObject["message"]).Value;
				}
				nXBandResult = nXBandResultUserKey;
				break;
			}
			case RequestCode.REQUEST_LIST_MEMBER:
			{
				NXBandResultListMembers nXBandResultListMembers = new NXBandResultListMembers();
				if (resultObject.ContainsKey("members"))
				{
					JsonArray jsonArray = (JsonArray)resultObject["members"];
					using (IEnumerator<IJsonType> enumerator = jsonArray.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							JsonObject json = (JsonObject)enumerator.Current;
							nXBandResultListMembers.members.Add(new NXBandTypeMember(json));
						}
					}
				}
				if (resultObject.ContainsKey("page_info"))
				{
					nXBandResultListMembers.pageInfo = new NXBandTypePageInfo((JsonObject)resultObject["page_info"]);
				}
				if (resultObject.ContainsKey("cache_info"))
				{
					nXBandResultListMembers.cache = new NXBandTypeCache((JsonObject)resultObject["cache_info"]);
				}
				nXBandResult = nXBandResultListMembers;
				break;
			}
			case RequestCode.REQUEST_LIST_BAND_MEMBER:
			{
				NXBandResultListBandMembers nXBandResultListBandMembers = new NXBandResultListBandMembers();
				if (resultObject.ContainsKey("members_of_bands"))
				{
					JsonArray jsonArray2 = (JsonArray)resultObject["members_of_bands"];
					using (IEnumerator<IJsonType> enumerator2 = jsonArray2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							JsonObject json2 = (JsonObject)enumerator2.Current;
							nXBandResultListBandMembers.bands.Add(new NXBandTypeBand(json2));
						}
					}
				}
				if (resultObject.ContainsKey("page_info"))
				{
					nXBandResultListBandMembers.pageInfo = new NXBandTypePageInfo((JsonObject)resultObject["page_info"]);
				}
				if (resultObject.ContainsKey("cache_info"))
				{
					nXBandResultListBandMembers.cache = new NXBandTypeCache((JsonObject)resultObject["cache_info"]);
				}
				nXBandResult = nXBandResultListBandMembers;
				break;
			}
			case RequestCode.REQUEST_SEND_INVITATION:
			case RequestCode.REQUEST_SEND_MESSAGE:
			case RequestCode.REQUEST_WRITE_POST:
			{
				NXBandResultWrite nXBandResultWrite = new NXBandResultWrite();
				if (resultObject.ContainsKey("quota"))
				{
					JsonObject jsonObject = (JsonObject)resultObject["quota"];
					if (jsonObject.ContainsKey("message"))
					{
						nXBandResultWrite.quota = new NXBandTypeQuota((JsonObject)jsonObject["message"]);
					}
					else if (jsonObject.ContainsKey("invite"))
					{
						nXBandResultWrite.quota = new NXBandTypeQuota((JsonObject)jsonObject["invite"]);
					}
					else if (jsonObject.ContainsKey("post"))
					{
						nXBandResultWrite.quota = new NXBandTypeQuota((JsonObject)jsonObject["post"]);
					}
				}
				nXBandResult = nXBandResultWrite;
				break;
			}
			case RequestCode.REQUEST_LIST_BAND:
			{
				NXBandResultListBands nXBandResultListBands = new NXBandResultListBands();
				if (resultObject.ContainsKey("bands"))
				{
					JsonArray jsonArray3 = (JsonArray)resultObject["bands"];
					using (IEnumerator<IJsonType> enumerator3 = jsonArray3.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							JsonObject json3 = (JsonObject)enumerator3.Current;
							nXBandResultListBands.bands.Add(new NXBandTypeBand(json3));
						}
					}
				}
				if (resultObject.ContainsKey("page_info"))
				{
					nXBandResultListBands.pageInfo = new NXBandTypePageInfo((JsonObject)resultObject["page_info"]);
				}
				if (resultObject.ContainsKey("cache_info"))
				{
					nXBandResultListBands.cache = new NXBandTypeCache((JsonObject)resultObject["cache_info"]);
				}
				nXBandResult = nXBandResultListBands;
				break;
			}
			case RequestCode.REQUEST_ACCESS_TOKEN:
			{
				NXBandResultAccessToken nXBandResultAccessToken = new NXBandResultAccessToken();
				if (resultObject.ContainsKey("access_token"))
				{
					nXBandResultAccessToken.accessToken = ((JsonString)resultObject["access_token"]).Value;
				}
				nXBandResult = nXBandResultAccessToken;
				break;
			}
			case RequestCode.REQUEST_QUOTA:
			{
				NXBandResultQuota nXBandResultQuota = new NXBandResultQuota();
				if (resultObject.ContainsKey("quota"))
				{
					JsonObject jsonObject2 = (JsonObject)resultObject["quota"];
					if (jsonObject2.ContainsKey("message"))
					{
						nXBandResultQuota.quotaMessage = new NXBandTypeQuota((JsonObject)jsonObject2["message"]);
					}
					if (jsonObject2.ContainsKey("invite"))
					{
						nXBandResultQuota.quotaInvite = new NXBandTypeQuota((JsonObject)jsonObject2["invite"]);
					}
					if (jsonObject2.ContainsKey("post"))
					{
						nXBandResultQuota.quotaPost = new NXBandTypeQuota((JsonObject)jsonObject2["post"]);
					}
				}
				nXBandResult = nXBandResultQuota;
				break;
			}
			case RequestCode.REQUEST_INVITER:
			{
				NXBandResultInviter nXBandResultInviter = new NXBandResultInviter();
				if (resultObject.ContainsKey("inviter"))
				{
					JsonObject json4 = (JsonObject)resultObject["inviter"];
					nXBandResultInviter.inviter = new NXBandTypeInviter(json4);
				}
				nXBandResult = nXBandResultInviter;
				break;
			}
			case RequestCode.REQUEST_CACHE_INFO:
			{
				NXBandResultCacheInfo nXBandResultCacheInfo = new NXBandResultCacheInfo();
				if (resultObject.ContainsKey("cache_info"))
				{
					JsonObject json5 = (JsonObject)resultObject["cache_info"];
					nXBandResultCacheInfo.cacheInfo = new NXBandTypeCache(json5);
				}
				nXBandResult = nXBandResultCacheInfo;
				break;
			}
			default:
				nXBandResult = new NXBandResult();
				break;
			}
			if (resultObject.ContainsKey("message"))
			{
				nXBandResult.message = ((JsonString)resultObject["message"]).Value;
			}
			return nXBandResult;
		}

		public override string ToString()
		{
			return "message : <" + this.message + ">";
		}
	}
}
