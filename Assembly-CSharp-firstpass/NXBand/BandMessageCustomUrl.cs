using System;
using System.Collections.Generic;

namespace NXBand
{
	public class BandMessageCustomUrl
	{
		public static string KEY_ANDROID = "android";

		public static string KEY_IOS = "ios";

		public static string KEY_BTN_TEXT = "text";

		private string _btnText;

		private string _pathAndroid;

		private string _pathIos;

		public BandMessageCustomUrl()
		{
			this._btnText = null;
			this._pathAndroid = null;
			this._pathIos = null;
		}

		public BandMessageCustomUrl(string btnText, string androidPath, string iosPath = null)
		{
			this._btnText = btnText;
			this._pathAndroid = androidPath;
			this._pathIos = iosPath;
		}

		public void setButtonText(string btnText)
		{
			this._btnText = btnText;
		}

		public void setAndroidPath(string path)
		{
			this._pathAndroid = path;
		}

		public void setIosPath(string path)
		{
			this._pathIos = path;
		}

		public Dictionary<string, string> getCustomUrlDictionary()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (this._btnText != null)
			{
				dictionary.Add(BandMessageCustomUrl.KEY_BTN_TEXT, this._btnText);
			}
			if (this._pathAndroid != null)
			{
				dictionary.Add(BandMessageCustomUrl.KEY_ANDROID, this._pathAndroid);
			}
			if (this._pathIos != null)
			{
				dictionary.Add(BandMessageCustomUrl.KEY_IOS, this._pathIos);
			}
			return dictionary;
		}
	}
}
