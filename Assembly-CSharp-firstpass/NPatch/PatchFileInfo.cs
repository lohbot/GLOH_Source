using NLibCs;
using System;

namespace NPatch
{
	public class PatchFileInfo
	{
		public static readonly float VER_FINAL = -9999f;

		public static readonly float VER_INVAILD = -1f;

		public Platform.Type ePlatform;

		public char PlatfromChar
		{
			get
			{
				return Platform.TypeToChar(this.ePlatform);
			}
			set
			{
				this.ePlatform = Platform.CharToType(value);
			}
		}

		public string VersionString
		{
			get
			{
				if (this.Version == PatchFileInfo.VER_FINAL)
				{
					return "final";
				}
				return string.Format("{0}", this.Version);
			}
		}

		public bool IsVaild
		{
			get
			{
				return this.Version != PatchFileInfo.VER_INVAILD;
			}
		}

		public int FileSize
		{
			get;
			set;
		}

		public float Version
		{
			get;
			set;
		}

		public int PatchLevel
		{
			get;
			set;
		}

		public int LangCode
		{
			get;
			set;
		}

		public string CRC
		{
			get;
			set;
		}

		public bool bUsePrepack
		{
			get;
			set;
		}

		public bool bUseCustomCache
		{
			get;
			set;
		}

		public PatchFileInfo()
		{
			this.CRC = string.Empty;
			this.Version = PatchFileInfo.VER_INVAILD;
			this.FileSize = 0;
			this.PlatfromChar = 'c';
			this.bUseCustomCache = false;
			this.PatchLevel = -1;
			this.LangCode = -1;
			this.bUsePrepack = false;
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2} {3} {4} {5}", new object[]
			{
				this.CRC,
				this.PlatfromChar,
				this.PatchLevel,
				this.LangCode,
				this.VersionString,
				this.bUseCustomCache
			});
		}

		public void SetDataFrom(FinalPatchList owner, NDataReader.Row row)
		{
			this.Version = PatchFileInfo.VER_INVAILD;
			this.FileSize = 0;
			char platfromChar = '?';
			if (row[1].Equals("final"))
			{
				this.Version = PatchFileInfo.VER_FINAL;
			}
			else
			{
				float vER_INVAILD = PatchFileInfo.VER_INVAILD;
				float.TryParse(row[1], out vER_INVAILD);
				this.Version = vER_INVAILD;
			}
			int fileSize;
			int.TryParse(row[2], out fileSize);
			this.FileSize = fileSize;
			char.TryParse(row[3], out platfromChar);
			this.PlatfromChar = platfromChar;
			if (owner.UseFieldNames)
			{
				if (owner._idx_CRC != -1)
				{
					this.CRC = row[owner._idx_CRC];
				}
				if (owner._idx_PatchLevel != -1)
				{
					int patchLevel = 0;
					int.TryParse(row[owner._idx_PatchLevel], out patchLevel);
					this.PatchLevel = patchLevel;
				}
				if (owner._idx_LangCode != -1)
				{
					int langCode = 0;
					int.TryParse(row[owner._idx_LangCode], out langCode);
					this.LangCode = langCode;
				}
				if (owner._idx_Prepack != -1)
				{
					bool bUsePrepack = false;
					bool.TryParse(row[owner._idx_Prepack], out bUsePrepack);
					this.bUsePrepack = bUsePrepack;
				}
			}
		}
	}
}
