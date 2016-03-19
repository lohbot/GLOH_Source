using SimpleJSON;
using System;

namespace com.adjust.sdk
{
	public class ResponseData
	{
		public enum ActivityKind
		{
			UNKNOWN,
			SESSION,
			EVENT,
			REVENUE,
			REATTRIBUTION
		}

		public ResponseData.ActivityKind? activityKind
		{
			get;
			private set;
		}

		public string activityKindString
		{
			get;
			private set;
		}

		public bool? success
		{
			get;
			private set;
		}

		public bool? willRetry
		{
			get;
			private set;
		}

		public string error
		{
			get;
			private set;
		}

		public string trackerToken
		{
			get;
			private set;
		}

		public string trackerName
		{
			get;
			private set;
		}

		public string network
		{
			get;
			private set;
		}

		public string campaign
		{
			get;
			private set;
		}

		public string adgroup
		{
			get;
			private set;
		}

		public string creative
		{
			get;
			private set;
		}

		public ResponseData(string jsonString)
		{
			JSONNode jSONNode = JSON.Parse(jsonString);
			if (jSONNode == null)
			{
				return;
			}
			this.activityKind = new ResponseData.ActivityKind?(this.ParseActivityKind(this.getJsonString(jSONNode, "activityKind")));
			this.activityKindString = this.activityKind.ToString().ToLower();
			this.success = this.getJsonBool(jSONNode, "success");
			this.willRetry = this.getJsonBool(jSONNode, "willRetry");
			this.error = this.getJsonString(jSONNode, "error");
			this.trackerName = this.getJsonString(jSONNode, "trackerName");
			this.trackerToken = this.getJsonString(jSONNode, "trackerToken");
			this.network = this.getJsonString(jSONNode, "network");
			this.campaign = this.getJsonString(jSONNode, "campaign");
			this.adgroup = this.getJsonString(jSONNode, "adgroup");
			this.creative = this.getJsonString(jSONNode, "creative");
		}

		private string getJsonString(JSONNode node, string key)
		{
			JSONNode jsonValue = this.getJsonValue(node, key);
			if (jsonValue == null)
			{
				return null;
			}
			return jsonValue.Value;
		}

		private bool? getJsonBool(JSONNode node, string key)
		{
			JSONNode jsonValue = this.getJsonValue(node, key);
			if (jsonValue == null)
			{
				return null;
			}
			return new bool?(jsonValue.AsBool);
		}

		private JSONNode getJsonValue(JSONNode node, string key)
		{
			if (node == null)
			{
				return null;
			}
			JSONNode jSONNode = node[key];
			if (jSONNode.GetType() == typeof(JSONLazyCreator))
			{
				return null;
			}
			return jSONNode;
		}

		private ResponseData.ActivityKind ParseActivityKind(string sActivityKind)
		{
			if ("session" == sActivityKind)
			{
				return ResponseData.ActivityKind.SESSION;
			}
			if ("event" == sActivityKind)
			{
				return ResponseData.ActivityKind.EVENT;
			}
			if ("revenue" == sActivityKind)
			{
				return ResponseData.ActivityKind.REVENUE;
			}
			if ("reattribution" == sActivityKind)
			{
				return ResponseData.ActivityKind.REATTRIBUTION;
			}
			return ResponseData.ActivityKind.UNKNOWN;
		}
	}
}
