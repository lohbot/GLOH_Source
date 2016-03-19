using System;
using System.Collections.Generic;
using System.Text;

public class FacebookBatchRequest
{
	public Dictionary<string, string> _parameters = new Dictionary<string, string>();

	private Dictionary<string, object> _requestDict = new Dictionary<string, object>();

	public FacebookBatchRequest(string relativeUrl, string method)
	{
		this._requestDict["method"] = method.ToUpper();
		this._requestDict["relative_url"] = relativeUrl;
	}

	public void addParameter(string key, string value)
	{
		this._parameters[key] = value;
	}

	public Dictionary<string, object> requestDictionary()
	{
		if (this._parameters.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> current in this._parameters)
			{
				stringBuilder.AppendFormat("{0}={1}&", current.Key, current.Value);
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			this._requestDict["body"] = stringBuilder.ToString();
		}
		return this._requestDict;
	}
}
