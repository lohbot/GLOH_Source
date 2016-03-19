using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using NLibCs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NPatch
{
	public class FinalPatchList
	{
		protected bool m_bLoadedOrSkipped;

		public static readonly FinalPatchList Instance = new FinalPatchList();

		public int _idx_CRC = -1;

		public int _idx_PatchLevel = -1;

		public int _idx_LangCode = -1;

		private SortedDictionary<string, PatchFileInfo> m_dic_files = new SortedDictionary<string, PatchFileInfo>();

		private float m_LoadPatchListVersion;

		public static string filename
		{
			get
			{
				string text = string.Empty;
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
				return text;
			}
		}

		public SortedDictionary<string, PatchFileInfo> FilesList
		{
			get
			{
				return this.m_dic_files;
			}
		}

		public float Version
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

		public bool UsePatchLevel
		{
			get;
			set;
		}

		public bool UseFieldNames
		{
			get;
			set;
		}

		public bool UseLangCode
		{
			get;
			set;
		}

		private IUpdateCustomCacheInfo UpdateCustomCacheInfo
		{
			get;
			set;
		}

		public FinalPatchList()
		{
			this.UseFieldNames = false;
			this.UsePatchLevel = false;
			this.UseLangCode = false;
		}

		public bool RPUVersionControl(string keyPath, out float version, out int size, out string versionDir)
		{
			version = -1f;
			size = -1;
			versionDir = string.Empty;
			if (!this.m_bLoadedOrSkipped)
			{
				this._OutputDebug(string.Format("FinalPatchList - Not loaded!!! - GetPatchFileInfo Failed!({0})", keyPath));
			}
			string text = (keyPath[0] == '/') ? keyPath : ("/" + keyPath);
			PatchFileInfo patchFileInfo;
			if (this.FilesList.TryGetValue(text.ToLower(), out patchFileInfo))
			{
				version = patchFileInfo.Version;
				size = patchFileInfo.FileSize;
				versionDir = patchFileInfo.VersionString;
				return true;
			}
			return false;
		}

		public bool LoadFromComp(byte[] gzData, IUpdateCustomCacheInfo updateCustomCacheInfo)
		{
			string text = this.__open_comp_bzip2(gzData);
			if (text == null)
			{
				text = this.__open_comp_bzip2(gzData);
			}
			return this.LoadFromText(text, updateCustomCacheInfo);
		}

		private string __open_comp_zip(byte[] compbytes)
		{
			MemoryStream baseInputStream = new MemoryStream(compbytes);
			using (ZipInputStream zipInputStream = new ZipInputStream(baseInputStream))
			{
				for (ZipEntry nextEntry = zipInputStream.GetNextEntry(); nextEntry != null; nextEntry = zipInputStream.GetNextEntry())
				{
					string name = nextEntry.Name;
					if (name.ToLower().Contains("final.patchlist"))
					{
						byte[] array = new byte[nextEntry.Size];
						zipInputStream.Read(array, 0, (int)nextEntry.Size);
						return Encoding.UTF8.GetString(array);
					}
				}
			}
			return null;
		}

		private string __open_comp_bzip2(byte[] compbytes)
		{
			MemoryStream memoryStream = new MemoryStream(compbytes);
			string result = null;
			try
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
				{
					int num = binaryReader.ReadInt32();
					BZip2InputStream bZip2InputStream = new BZip2InputStream(memoryStream);
					byte[] array = new byte[num];
					bZip2InputStream.Read(array, 0, array.Length);
					bZip2InputStream.Close();
					result = Encoding.UTF8.GetString(array);
					memoryStream.Close();
					binaryReader.Close();
				}
			}
			catch (Exception arg)
			{
				this._OutputDebug(string.Format("The process failed: {0}", arg));
				return null;
			}
			return result;
		}

		public int GetVersionCount(float version)
		{
			int num = 0;
			foreach (PatchFileInfo current in this.FilesList.Values)
			{
				if (current.Version == version)
				{
					num++;
				}
			}
			return num;
		}

		public PatchFileInfo GetPatchFileInfo(string resFileName)
		{
			if (!this.m_bLoadedOrSkipped)
			{
				this._OutputDebug(string.Format("FinalPatchList - Not loaded!!! - GetPatchFileInfo Failed!({0})", resFileName));
			}
			string text = (resFileName[0] == '/') ? resFileName : ("/" + resFileName);
			text = text.Replace('\\', '/');
			PatchFileInfo result;
			if (this.FilesList.TryGetValue(text.ToLower(), out result))
			{
				return result;
			}
			return null;
		}

		public bool LoadFromText(string strText, IUpdateCustomCacheInfo updateCustomCacheInfo)
		{
			if (updateCustomCacheInfo != null)
			{
				this.UpdateCustomCacheInfo = updateCustomCacheInfo;
			}
			bool result;
			try
			{
				NDataReader nDataReader = new NDataReader();
				if (!nDataReader.LoadFrom(strText))
				{
					this._OutputDebug(string.Format("NDataReader.LoadFromText failed: {0}", strText));
					this.m_bLoadedOrSkipped = false;
					result = false;
				}
				else
				{
					this.FilesList.Clear();
					this.m_bLoadedOrSkipped = this.ReadFinalList2(ref nDataReader);
					result = this.m_bLoadedOrSkipped;
				}
			}
			catch (Exception arg)
			{
				this._OutputDebug(string.Format("The process failed: {0}", arg));
				this.m_bLoadedOrSkipped = false;
				result = false;
			}
			return result;
		}

		public bool Load(string strListFileName, IUpdateCustomCacheInfo updateCustomCacheInfo)
		{
			if (updateCustomCacheInfo != null)
			{
				this.UpdateCustomCacheInfo = updateCustomCacheInfo;
			}
			bool result;
			try
			{
				if (Path.GetExtension(strListFileName).ToLower() == ".zip")
				{
					using (FileStream fileStream = new FileStream(strListFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						using (BinaryReader binaryReader = new BinaryReader(fileStream))
						{
							byte[] gzData = binaryReader.ReadBytes((int)fileStream.Length);
							result = this.LoadFromComp(gzData, updateCustomCacheInfo);
							return result;
						}
					}
				}
				NDataReader nDataReader = new NDataReader();
				if (!nDataReader.Load(strListFileName))
				{
					this.m_bLoadedOrSkipped = false;
					result = false;
				}
				else
				{
					this.FilesList.Clear();
					this.m_bLoadedOrSkipped = this.ReadFinalList2(ref nDataReader);
					result = this.m_bLoadedOrSkipped;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("The process failed: {0}", ex.ToString());
				this.m_bLoadedOrSkipped = false;
				result = false;
			}
			return result;
		}

		private bool ReadFinalList2(ref NDataReader dr)
		{
			this.m_LoadPatchListVersion = 0f;
			string text = string.Empty;
			NDataSection nDataSection = dr["[Header]"];
			text = nDataSection["PatchVersion"];
			bool bUserReplaceWord = nDataSection["UseListCompressor"];
			if (text.ToLower().Equals("final"))
			{
				this.m_LoadPatchListVersion = PatchFileInfo.VER_FINAL;
			}
			else
			{
				float.TryParse(text, out this.m_LoadPatchListVersion);
			}
			bool result;
			try
			{
				ListCompressor listCompressor = new ListCompressor(bUserReplaceWord);
				StringBuilder stringBuilder = new StringBuilder(512);
				StringBuilder stringBuilder2 = new StringBuilder(512);
				StringBuilder stringBuilder3 = new StringBuilder(512);
				this.UseFieldNames = dr.ReadFieldNames();
				if (this.UseFieldNames)
				{
					this._idx_CRC = dr.GetFieldIndex("CRC");
					this._idx_PatchLevel = dr.GetFieldIndex("PatchLevel");
					this._idx_LangCode = dr.GetFieldIndex("LangCode");
				}
				if (dr.BeginSection("[FinalList2]"))
				{
					foreach (NDataReader.Row row in dr)
					{
						if (row.LineType == NDataReader.Row.TYPE.LINE_DATA)
						{
							stringBuilder.Length = 0;
							stringBuilder.Append(row[0].str);
							stringBuilder = listCompressor.ReplaceWord(stringBuilder, false);
							if (stringBuilder[0] == '?')
							{
								stringBuilder.Remove(0, 1);
								stringBuilder3.Length = 0;
								stringBuilder3.Append(stringBuilder.ToString());
							}
							else
							{
								PatchFileInfo patchFileInfo = new PatchFileInfo();
								patchFileInfo.SetDataFrom(this, row);
								stringBuilder2.Length = 0;
								stringBuilder2.AppendFormat("{0}/{1}", stringBuilder3, stringBuilder);
								stringBuilder2 = stringBuilder2.Replace("//", "/");
								string text2 = stringBuilder2.ToString();
								if (this.UpdateCustomCacheInfo != null)
								{
									patchFileInfo.bUseCustomCache = this.UpdateCustomCacheInfo.CheckCustomCacheInfo(text2);
								}
								if (this.FilesList.ContainsKey(text2))
								{
									PatchFileInfo patchFileInfo2 = new PatchFileInfo();
									this.FilesList.TryGetValue(text2, out patchFileInfo2);
									this._OutputDebug(string.Format("Warning - duplicated patch list item : {0} / already:{1} new:{2}", text2, patchFileInfo2.Version, patchFileInfo.Version));
									if (patchFileInfo2.Version < patchFileInfo.Version)
									{
										this.FilesList[text2] = patchFileInfo;
									}
								}
								this.FilesList.Add(text2, patchFileInfo);
							}
						}
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

		public bool ReadPatchList(ref NDataReader dr)
		{
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(512);
				StringBuilder stringBuilder2 = new StringBuilder(512);
				StringBuilder stringBuilder3 = new StringBuilder(512);
				if (dr.BeginSection("[FinalList2]"))
				{
					ListCompressor listCompressor = new ListCompressor();
					foreach (NDataReader.Row row in dr)
					{
						if (row.LineType == NDataReader.Row.TYPE.LINE_DATA)
						{
							stringBuilder.Length = 0;
							stringBuilder.Append(row[0].str);
							stringBuilder = listCompressor.ReplaceWord(stringBuilder, false);
							if (stringBuilder[0] == '?')
							{
								stringBuilder.Remove(0, 1);
								stringBuilder3.Length = 0;
								stringBuilder3.Append(stringBuilder.ToString());
							}
							else
							{
								PatchFileInfo patchFileInfo = new PatchFileInfo();
								patchFileInfo.SetDataFrom(this, row);
								stringBuilder2.Length = 0;
								stringBuilder2.AppendFormat("{0}/{1}", stringBuilder3, stringBuilder);
								stringBuilder2 = stringBuilder2.Replace("//", "/");
								string text = stringBuilder2.ToString();
								if (this.UpdateCustomCacheInfo != null)
								{
									patchFileInfo.bUseCustomCache = this.UpdateCustomCacheInfo.CheckCustomCacheInfo(text);
								}
								if (!text.Contains("duplicationfilelist"))
								{
									if (this.FilesList.ContainsKey(text))
									{
										PatchFileInfo patchFileInfo2 = new PatchFileInfo();
										this.FilesList.TryGetValue(text, out patchFileInfo2);
										this._OutputDebug(string.Format("Warning - duplicated patch list item : {0} / already:{1} new:{2}", text, patchFileInfo2.Version, patchFileInfo.Version));
										if (patchFileInfo2.Version < patchFileInfo.Version)
										{
											this.FilesList[text] = patchFileInfo;
										}
									}
									this.FilesList.Add(text, patchFileInfo);
								}
							}
						}
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

		public void _OutputDebug(string strOutput)
		{
			Console.WriteLine(strOutput);
		}

		public void SetSkipLoad()
		{
			this.m_bLoadedOrSkipped = true;
		}
	}
}
