using System;

public class TsAudioEventKeyParser
{
	public static readonly string QUERY = "?";

	public static readonly string[] SEPARATOR = new string[]
	{
		"#"
	};

	public static TsAudioEventKeyParser parser = new TsAudioEventKeyParser();

	public string DomainKey
	{
		get;
		private set;
	}

	public string CategoryKey
	{
		get;
		private set;
	}

	public string AudioKey
	{
		get;
		private set;
	}

	public string BundleKey
	{
		get;
		private set;
	}

	public bool HasBundleKey
	{
		get
		{
			return !string.IsNullOrEmpty(this.BundleKey);
		}
	}

	private TsAudioEventKeyParser()
	{
	}

	public static TsAudioEventKeyParser Create(string eventKeyString, NrCharInfoAdaptor charInfoAdaptor)
	{
		if (string.IsNullOrEmpty(eventKeyString))
		{
			return null;
		}
		string[] array = eventKeyString.Split(TsAudioEventKeyParser.SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 3 && array.Length != 4)
		{
			return null;
		}
		TsAudioEventKeyParser.parser.DomainKey = array[0];
		TsAudioEventKeyParser.parser.CategoryKey = array[1];
		TsAudioEventKeyParser.parser.AudioKey = TsAudioEventKeyParser._QueryCharInfo(charInfoAdaptor, array[2]);
		TsAudioEventKeyParser.parser.BundleKey = string.Empty;
		if (array.Length == 3)
		{
			TsAudioEventKeyParser.parser.BundleKey = string.Empty;
		}
		else if (array.Length == 4)
		{
			TsAudioEventKeyParser.parser.BundleKey = array[3];
		}
		return TsAudioEventKeyParser.parser;
	}

	public static TsAudioEventKeyParser CreateOld(string domainKey, string eventKeyString, NrCharInfoAdaptor charInfoAdaptor)
	{
		if (string.IsNullOrEmpty(domainKey))
		{
			return null;
		}
		if (string.IsNullOrEmpty(eventKeyString))
		{
			return null;
		}
		string[] array = eventKeyString.Split(TsAudioEventKeyParser.SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 2 && array.Length != 3)
		{
			return null;
		}
		TsAudioEventKeyParser.parser.DomainKey = domainKey;
		TsAudioEventKeyParser.parser.CategoryKey = array[0];
		TsAudioEventKeyParser.parser.AudioKey = TsAudioEventKeyParser._QueryCharInfo(charInfoAdaptor, array[1]);
		TsAudioEventKeyParser.parser.BundleKey = string.Empty;
		if (array.Length == 2)
		{
			TsAudioEventKeyParser.parser.BundleKey = string.Empty;
		}
		else if (array.Length == 3)
		{
			TsAudioEventKeyParser.parser.BundleKey = array[2];
		}
		return TsAudioEventKeyParser.parser;
	}

	public static string MakeEventKey(string domainkey, string categoryKey, string audioKey, string bundleKey, bool useMethod)
	{
		string text = domainkey + TsAudioEventKeyParser.SEPARATOR[0] + categoryKey;
		if (!useMethod)
		{
			text = text + TsAudioEventKeyParser.SEPARATOR[0] + audioKey;
		}
		else
		{
			text = text + TsAudioEventKeyParser.SEPARATOR[0] + TsAudioEventKeyParser.QUERY + audioKey;
		}
		if (!string.IsNullOrEmpty(bundleKey))
		{
			text = text + TsAudioEventKeyParser.SEPARATOR[0] + bundleKey;
		}
		return text;
	}

	public override string ToString()
	{
		return string.Format("DomainKey( {0} )   Category( {1} )    Audio( {2} )    Bundle( {3} )", new object[]
		{
			this.DomainKey,
			this.CategoryKey,
			this.AudioKey,
			this.BundleKey
		});
	}

	private static string _QueryCharInfo(NrCharInfoAdaptor charInfoAdaptor, string queryString)
	{
		if (charInfoAdaptor == null || string.IsNullOrEmpty(queryString))
		{
			return queryString;
		}
		if (TsAudioEventKeyParser.QUERY[0] != queryString[0])
		{
			return queryString;
		}
		string methodName = queryString.Remove(0, 1);
		return charInfoAdaptor.GetMethodValue(methodName);
	}
}
