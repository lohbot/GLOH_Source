using ICSharpCode.SharpZipLib.BZip2;
using NLibCs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TsPatch
{
	public class PatchFinalList
	{
		public static ReplaceItem[] REPLACE_ITEMS = new ReplaceItem[]
		{
			new ReplaceItem("mobile_and", "\"A\""),
			new ReplaceItem("mobile_ios", "\"I\""),
			new ReplaceItem("map_tileinfo", "MTI"),
			new ReplaceItem("battlemap_cellatbinfo", "BMCI"),
			new ReplaceItem("assetbundle", "AB"),
			new ReplaceItem("customizing", "CU"),
			new ReplaceItem("footstep", "FS"),
			new ReplaceItem("scimitar", "SC"),
			new ReplaceItem("greeting", "GR"),
			new ReplaceItem("monster", "MO"),
			new ReplaceItem("monarch", "MN"),
			new ReplaceItem("master", "MA"),
			new ReplaceItem("sword", "SW"),
			new ReplaceItem("spear", "SP"),
			new ReplaceItem("general", "G"),
			new ReplaceItem("greed", "R"),
			new ReplaceItem("vehicle", "V"),
			new ReplaceItem("dungeon", "D"),
			new ReplaceItem("attack", "A"),
			new ReplaceItem("event", "E"),
			new ReplaceItem("quest", "Q"),
			new ReplaceItem("sound", "S"),
			new ReplaceItem("magic", "M"),
			new ReplaceItem("normal", "O"),
			new ReplaceItem("weapon", "W"),
			new ReplaceItem("troop", "T"),
			new ReplaceItem("none", "N"),
			new ReplaceItem("hero", "H"),
			new ReplaceItem("item", "I"),
			new ReplaceItem("battle", "B"),
			new ReplaceItem("skill", "K"),
			new ReplaceItem("female", "<"),
			new ReplaceItem("male", ">"),
			new ReplaceItem("hit", "*")
		};

		private static readonly PatchFinalList instance = new PatchFinalList();

		public SortedDictionary<string, PatchFileInfo> FilesList = new SortedDictionary<string, PatchFileInfo>();

		protected bool m_bLoadedOrSkipped;

		public int m_LoadPatchListVersion;

		public static bool UseReplaceWord
		{
			get
			{
				return false;
			}
		}

		public static string filename
		{
			get
			{
				string text = NrTSingleton<NrGlobalReference>.Instance.ResourcesVer;
				if (text == "0" || text == string.Empty || text == string.Empty)
				{
					text = "final";
				}
				switch (Platform.RunPlatform)
				{
				case Platform.Type.WEB:
					text += "/final.patchlist.web.zip";
					break;
				case Platform.Type.AND:
					text += "/final.patchlist.and.zip";
					break;
				case Platform.Type.IOS:
					text += "/final.patchlist.ios.zip";
					break;
				default:
					Platform.RunPlatform = Platform.Type.WEB;
					text += "/final.patchlist.web.zip";
					break;
				}
				TsLog.LogWarning("TsPatch.PatchFinalList.filename -> {0} , Platfrom:{1}", new object[]
				{
					text,
					Platform.RunPlatform
				});
				return text;
			}
		}

		public static PatchFinalList Instance
		{
			get
			{
				return PatchFinalList.instance;
			}
		}

		public int Version
		{
			get
			{
				return this.m_LoadPatchListVersion;
			}
		}

		public bool isLoadedOrSkipped
		{
			get
			{
				return this.m_bLoadedOrSkipped;
			}
		}

		private PatchFinalList()
		{
		}

		public static string ReplaceWord(string str, bool bWordToCode)
		{
			if (!PatchFinalList.UseReplaceWord)
			{
				return str;
			}
			StringBuilder stringBuilder = new StringBuilder(512);
			if (bWordToCode)
			{
				stringBuilder.Append(str.ToLower());
				ReplaceItem[] rEPLACE_ITEMS = PatchFinalList.REPLACE_ITEMS;
				for (int i = 0; i < rEPLACE_ITEMS.Length; i++)
				{
					ReplaceItem replaceItem = rEPLACE_ITEMS[i];
					stringBuilder = stringBuilder.Replace(replaceItem.word, replaceItem.code);
				}
			}
			else
			{
				ReplaceItem[] rEPLACE_ITEMS2 = PatchFinalList.REPLACE_ITEMS;
				for (int j = 0; j < rEPLACE_ITEMS2.Length; j++)
				{
					ReplaceItem replaceItem2 = rEPLACE_ITEMS2[j];
					stringBuilder = stringBuilder.Replace(replaceItem2.code, replaceItem2.word);
				}
			}
			return stringBuilder.ToString();
		}

		public static StringBuilder ReplaceWord(StringBuilder sb, bool bWordToCode)
		{
			if (!PatchFinalList.UseReplaceWord)
			{
				return sb;
			}
			if (bWordToCode)
			{
				ReplaceItem[] rEPLACE_ITEMS = PatchFinalList.REPLACE_ITEMS;
				for (int i = 0; i < rEPLACE_ITEMS.Length; i++)
				{
					ReplaceItem replaceItem = rEPLACE_ITEMS[i];
					sb = sb.Replace(replaceItem.word, replaceItem.code);
				}
			}
			else
			{
				ReplaceItem[] rEPLACE_ITEMS2 = PatchFinalList.REPLACE_ITEMS;
				for (int j = 0; j < rEPLACE_ITEMS2.Length; j++)
				{
					ReplaceItem replaceItem2 = rEPLACE_ITEMS2[j];
					sb = sb.Replace(replaceItem2.code, replaceItem2.word);
				}
			}
			return sb;
		}

		public PatchFileInfo GetPatchFileInfo(string resFileName)
		{
			if (!this.m_bLoadedOrSkipped)
			{
				this._OutputDebug(string.Format("PatchFinalList - Not loaded!!! - GetPatchFileInfo Failed!({0})", resFileName));
			}
			string text = (resFileName[0] == '/') ? resFileName : ("/" + resFileName);
			PatchFileInfo result;
			if (this.FilesList.TryGetValue(text.ToLower(), out result))
			{
				return result;
			}
			return null;
		}

		public bool LoadFromText(string strText)
		{
			bool result;
			try
			{
				using (NDataReader nDataReader = new NDataReader())
				{
					if (!nDataReader.LoadFrom(strText))
					{
						this._OutputDebug(string.Format("NDataReader.LoadFromText failed: {0}", strText));
						this.m_bLoadedOrSkipped = false;
						result = false;
						return result;
					}
					this.FilesList.Clear();
					NDataReader nDataReader2 = nDataReader;
					this.m_bLoadedOrSkipped = this.ReadFinalList2(ref nDataReader2);
					if (!this.m_bLoadedOrSkipped)
					{
						this.m_bLoadedOrSkipped = this.ReadFinalList(ref nDataReader2);
					}
				}
				result = this.m_bLoadedOrSkipped;
			}
			catch (Exception arg)
			{
				this._OutputDebug(string.Format("The process failed: {0}", arg));
				this.m_bLoadedOrSkipped = false;
				result = false;
			}
			return result;
		}

		public bool LoadFromComp(byte[] gzData)
		{
			MemoryStream memoryStream = new MemoryStream(gzData);
			string strText = null;
			try
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
				{
					int num = binaryReader.ReadInt32();
					BZip2InputStream bZip2InputStream = new BZip2InputStream(memoryStream);
					byte[] array = new byte[num];
					bZip2InputStream.Read(array, 0, array.Length);
					bZip2InputStream.Close();
					strText = Encoding.UTF8.GetString(array);
					memoryStream.Close();
					binaryReader.Close();
				}
			}
			catch (Exception arg)
			{
				this._OutputDebug(string.Format("The process failed: {0}", arg));
				return false;
			}
			return this.LoadFromText(strText);
		}

		public bool Load(string strListFileName)
		{
			bool result;
			try
			{
				using (NDataReader nDataReader = new NDataReader())
				{
					if (!nDataReader.Load(strListFileName))
					{
						this.m_bLoadedOrSkipped = false;
						result = false;
						return result;
					}
					this.FilesList.Clear();
					NDataReader nDataReader2 = nDataReader;
					this.m_bLoadedOrSkipped = this.ReadFinalList2(ref nDataReader2);
					if (!this.m_bLoadedOrSkipped)
					{
						this.m_bLoadedOrSkipped = this.ReadFinalList(ref nDataReader2);
					}
				}
				result = this.m_bLoadedOrSkipped;
			}
			catch (Exception ex)
			{
				Console.WriteLine("The process failed: {0}", ex.ToString());
				this.m_bLoadedOrSkipped = false;
				result = false;
			}
			return result;
		}

		private bool CheckCustomCache(string url)
		{
			bool result = false;
			if (TsCaching.useCustomCacheOnly)
			{
				result = true;
			}
			else if (TsPlatform.IsMobile && url.Contains("mobile"))
			{
				result = true;
			}
			return result;
		}

		private bool ReadFinalList(ref NDataReader dr)
		{
			this._OutputDebug(string.Format("$$$SysConfig:ReadFinalPatchList! TsCaching.useCustomCacheOnly = {0}", TsCaching.useCustomCacheOnly));
			this.m_LoadPatchListVersion = 0;
			this.m_LoadPatchListVersion = dr["[Header]"]["PatchVersion"];
			TsPlatform.FileLog("test 1 m_LoadPatchListVersion = " + this.m_LoadPatchListVersion.ToString());
			if (dr.BeginSection("[FinalList]"))
			{
				while (!dr.IsEndOfSection())
				{
					NDataReader.Row currentRow = dr.GetCurrentRow();
					if (currentRow.LineType == NDataReader.Row.TYPE.LINE_DATA)
					{
						PatchFileInfo patchFileInfo = new PatchFileInfo();
						patchFileInfo.Version = ((1 >= currentRow.Values.Count) ? 0 : Convert.ToInt32(currentRow.GetColumn(1)));
						patchFileInfo.FileSize = ((2 >= currentRow.Values.Count) ? 0 : Convert.ToInt32(currentRow.GetColumn(2)));
						string text = currentRow.GetColumn(0).ToLower();
						patchFileInfo.bUseCustomCache = this.CheckCustomCache(text);
						if (this.FilesList.ContainsKey(text))
						{
							this._OutputDebug(string.Format("이미 등록된 파일명입니다!! {0}", text));
							return false;
						}
						this.FilesList.Add(text, patchFileInfo);
					}
					dr.NextLine();
				}
				return true;
			}
			return false;
		}

		private bool ReadFinalList2(ref NDataReader dr)
		{
			this._OutputDebug(string.Format("$$$SysConfig:ReadFinalPatchList2! TsCaching.useCustomCacheOnly = {0}", TsCaching.useCustomCacheOnly));
			this.m_LoadPatchListVersion = 0;
			string text = string.Empty;
			text = dr["[Header]"]["PatchVersion"];
			if (text.ToLower().Equals("final"))
			{
				this.m_LoadPatchListVersion = PatchFileInfo.VER_FINAL;
			}
			else
			{
				int.TryParse(text, out this.m_LoadPatchListVersion);
			}
			this._OutputDebug("test 3 m_LoadPatchListVersion = " + this.m_LoadPatchListVersion.ToString());
			TsPlatform.FileLog("test 3 m_LoadPatchListVersion = " + this.m_LoadPatchListVersion.ToString());
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(512);
				StringBuilder stringBuilder2 = new StringBuilder(512);
				StringBuilder stringBuilder3 = new StringBuilder(512);
				if (dr.BeginSection("[FinalList2]"))
				{
					while (!dr.IsEndOfSection())
					{
						NDataReader.Row currentRow = dr.GetCurrentRow();
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
								string text2 = stringBuilder2.ToString();
								patchFileInfo.bUseCustomCache = this.CheckCustomCache(text2);
								patchFileInfo.szMD5 = currentRow.GetColumn(4);
								if (!text2.Contains("duplicationfilelist"))
								{
									if (this.FilesList.ContainsKey(text2))
									{
										PatchFileInfo patchFileInfo2 = new PatchFileInfo();
										this.FilesList.TryGetValue(text2, out patchFileInfo2);
										this._OutputDebug(string.Format("Warning - duplicated patch list item : {0} / already:{1} new:{2}", text2, patchFileInfo2.nVersion, patchFileInfo.nVersion));
										if (patchFileInfo2.nVersion < patchFileInfo.nVersion)
										{
											this.FilesList[text2] = patchFileInfo;
										}
									}
									else
									{
										this.FilesList.Add(text2, patchFileInfo);
									}
								}
							}
						}
						dr.NextLine();
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				this._OutputDebug(ex.ToString());
				result = false;
			}
			return result;
		}

		private void _OutputDebug(string strOutput)
		{
			Console.WriteLine(strOutput);
			TsLog.LogWarning(strOutput, new object[0]);
		}

		public void SetSkipLoad()
		{
			this.m_bLoadedOrSkipped = true;
			string basePath = NrTSingleton<NrGlobalReference>.Instance.basePath;
			string message = string.Format("Skip to load \"{0}/{1}\" file.", basePath, PatchFinalList.filename);
			if (basePath.Contains("DESIGN") || !NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				TsLog.LogWarning(message, new object[0]);
			}
			else
			{
				TsLog.Assert(false, message, new object[0]);
			}
		}

		public string GetMD5(string strFilePath)
		{
			if (strFilePath == string.Empty)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string result;
			try
			{
				using (FileStream fileStream = File.OpenRead(strFilePath))
				{
					if (fileStream == null)
					{
						result = string.Empty;
					}
					else
					{
						MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
						byte[] array = mD5CryptoServiceProvider.ComputeHash(fileStream);
						byte[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							byte b = array2[i];
							stringBuilder.Append(b.ToString("x2"));
						}
						result = stringBuilder.ToString().ToLower();
					}
				}
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		public string GetMD5(byte[] arByte, string filePath)
		{
			if (filePath == string.Empty)
			{
				return string.Empty;
			}
			if (arByte == null)
			{
				return string.Empty;
			}
			if (arByte.Length != 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
				byte[] array = mD5CryptoServiceProvider.ComputeHash(arByte);
				byte[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					byte b = array2[i];
					stringBuilder.Append(b.ToString("x2"));
				}
				return stringBuilder.ToString().ToLower();
			}
			return string.Empty;
		}
	}
}
