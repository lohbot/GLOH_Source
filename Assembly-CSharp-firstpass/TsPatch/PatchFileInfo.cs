using NLibCs;
using System;

namespace TsPatch
{
	public class PatchFileInfo
	{
		public static readonly int VER_FINAL = -9999;

		public static readonly int VER_INVAILD = -1;

		public int nVersion = -1;

		public int nFileSize;

		public Platform.Type ePlatform;

		public bool bUseCustomCache;

		public string szMD5 = string.Empty;

		public char PlatfromChar
		{
			get
			{
				switch (this.ePlatform)
				{
				case Platform.Type.UNKOWN:
					return '?';
				case Platform.Type.COMMON:
					return 'C';
				case Platform.Type.WEB:
					return 'W';
				case Platform.Type.AND:
					return 'A';
				case Platform.Type.IOS:
					return 'I';
				default:
					return '?';
				}
			}
			set
			{
				switch (value)
				{
				case '?':
					this.ePlatform = Platform.Type.UNKOWN;
					return;
				case '@':
				case 'B':
					IL_1F:
					if (value == 'I')
					{
						this.ePlatform = Platform.Type.IOS;
						return;
					}
					if (value != 'W')
					{
						this.ePlatform = Platform.Type.UNKOWN;
						return;
					}
					this.ePlatform = Platform.Type.WEB;
					return;
				case 'A':
					this.ePlatform = Platform.Type.AND;
					return;
				case 'C':
					this.ePlatform = Platform.Type.COMMON;
					return;
				}
				goto IL_1F;
			}
		}

		public string VersionDir
		{
			get
			{
				if (this.nVersion == PatchFileInfo.VER_FINAL)
				{
					return "final";
				}
				return this.nVersion.ToString();
			}
		}

		public int Version
		{
			get
			{
				return this.nVersion;
			}
			set
			{
				this.nVersion = value;
			}
		}

		public bool IsVaild
		{
			get
			{
				return this.nVersion != PatchFileInfo.VER_INVAILD;
			}
		}

		public int FileSize
		{
			get
			{
				return this.nFileSize;
			}
			set
			{
				this.nFileSize = value;
			}
		}

		public PatchFileInfo()
		{
			this.FileSize = 0;
		}

		public PatchFileInfo(int nVer)
		{
			this.FileSize = 0;
			this.nVersion = nVer;
		}

		public PatchFileInfo(NDataReader.Row row)
		{
			this.FileSize = 0;
			char platfromChar = '?';
			if (row[1].Equals("final"))
			{
				this.nVersion = PatchFileInfo.VER_FINAL;
			}
			else
			{
				int.TryParse(row[1], out this.nVersion);
			}
			int fileSize;
			int.TryParse(row[2], out fileSize);
			this.FileSize = fileSize;
			char.TryParse(row[3], out platfromChar);
			this.PlatfromChar = platfromChar;
		}
	}
}
