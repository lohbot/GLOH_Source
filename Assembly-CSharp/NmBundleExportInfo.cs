using System;
using System.Globalization;
using UnityEngine;

[Serializable]
public class NmBundleExportInfo : MonoBehaviour
{
	private const string strGoName = "@BundleExportInfo";

	public string ExportInfo = string.Empty;

	public static string GameObjectName
	{
		get
		{
			return "@BundleExportInfo";
		}
	}

	public void StampNow()
	{
		DateTimeFormatInfo dateTimeFormat = new CultureInfo("ko-KR", false).DateTimeFormat;
		this.ExportInfo = DateTime.Now.ToString("F", dateTimeFormat);
	}
}
