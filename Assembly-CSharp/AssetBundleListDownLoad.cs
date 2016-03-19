using NLibCs;
using System;
using System.Collections.Generic;
using System.Text;
using TsBundle;
using TsPatch;
using UnityEngine;

public class AssetBundleListDownLoad : MonoBehaviour
{
	private readonly string _BUNDLENAME = "final.bundlelist.txt";

	private void Start()
	{
		this.BundleListDown();
	}

	private void BundleListDown()
	{
		string key = string.Format("{0}{1}", PlayerPrefs.GetString(NrPrefsKey.LOCAL_WWW_PATH, "file:///d:/ndoors/at2dev/"), this._BUNDLENAME);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(key, null, true);
		wWWItem.SetItemType(ItemType.USER_STRING);
		wWWItem.SetCallback(new PostProcPerItem(this._OnCompleteDownload), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void _OnCompleteDownload(IDownloadedItem wItem, object obj)
	{
		if (wItem.canAccessString)
		{
			try
			{
				string safeString = wItem.safeString;
				NDataReader nDataReader = new NDataReader();
				if (!nDataReader.LoadFrom(safeString))
				{
					TsLog.LogWarning(string.Format("NDataReader.LoadFromText failed: {0}", safeString), new object[0]);
					return;
				}
				SortedDictionary<string, PatchFileInfo> sortedDictionary = new SortedDictionary<string, PatchFileInfo>();
				StringBuilder stringBuilder = new StringBuilder(512);
				StringBuilder stringBuilder2 = new StringBuilder(512);
				StringBuilder stringBuilder3 = new StringBuilder(512);
				if (nDataReader.BeginSection("[FinalList2]"))
				{
					while (!nDataReader.IsEndOfSection())
					{
						NDataReader.Row currentRow = nDataReader.GetCurrentRow();
						if (currentRow.LineType == NDataReader.Row.TYPE.LINE_DATA)
						{
							stringBuilder.Length = 0;
							stringBuilder.Append(currentRow.GetColumn(0));
							stringBuilder = PatchFinalList.ReplaceWord(stringBuilder, false);
							if (stringBuilder[0] == '?')
							{
								stringBuilder.Remove(0, 1);
								stringBuilder3.Length = 0;
								stringBuilder3.Append(stringBuilder.ToString());
							}
							else
							{
								PatchFileInfo patchFileInfo = new PatchFileInfo(currentRow);
								stringBuilder2.Length = 0;
								stringBuilder2.AppendFormat("{0}/{1}", stringBuilder3, stringBuilder);
								stringBuilder2 = stringBuilder2.Replace("//", "/");
								string text = stringBuilder2.ToString();
								if (!text.Contains("duplicationfilelist"))
								{
									if (sortedDictionary.ContainsKey(text))
									{
										PatchFileInfo patchFileInfo2 = new PatchFileInfo();
										sortedDictionary.TryGetValue(text, out patchFileInfo2);
										TsLog.Log(string.Format("Warning - duplicated patch list item : {0} / already:{1} new:{2}", text, patchFileInfo2.nVersion, patchFileInfo.nVersion), new object[0]);
										if (patchFileInfo2.nVersion < patchFileInfo.nVersion)
										{
											sortedDictionary[text] = patchFileInfo;
										}
									}
									else
									{
										sortedDictionary.Add(text, patchFileInfo);
									}
								}
							}
						}
						nDataReader.NextLine();
					}
				}
				NrTSingleton<AssetBundleURLInfo>.Instance.CollectBundleInfo(sortedDictionary.Keys);
			}
			catch (Exception arg)
			{
				TsLog.LogWarning(string.Format("The process failed: {0}", arg), new object[0]);
			}
		}
		NrTSingleton<AssetBundleURLInfo>.Instance.IsLoad = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
