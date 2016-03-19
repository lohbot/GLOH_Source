using System;
using System.IO;

namespace NLibCs
{
	public static class NEncrypt
	{
		private class Header
		{
			public const int SIZE = 24;

			public byte[] id = new byte[4];

			public ushort version;

			public ushort encodeType;

			public int iDataSize;

			public byte key;

			public byte checkSum;

			public byte[] reserved = new byte[10];

			public byte[] Init(NEncrypt.Header h, long lFileSize, char key = ' ')
			{
				Random random = new Random();
				int num;
				if (key == ' ')
				{
					num = random.Next() % 25;
				}
				else
				{
					num = (int)key;
				}
				h.key = (byte)num + 65;
				h.id[0] = Convert.ToByte('N');
				h.id[1] = Convert.ToByte('C');
				h.id[2] = Convert.ToByte('R');
				h.version = 1;
				h.encodeType = 3;
				h.iDataSize = (int)lFileSize;
				h.checkSum = 0;
				byte[] array = new byte[24];
				for (int i = 0; i < 4; i++)
				{
					array[i] = h.id[i];
				}
				byte[] bytes = BitConverter.GetBytes(h.version);
				int num2 = 0;
				for (int j = 4; j < 6; j++)
				{
					array[j] = bytes[num2];
					num2++;
				}
				num2 = 0;
				bytes = BitConverter.GetBytes(h.encodeType);
				for (int k = 6; k < 8; k++)
				{
					array[k] = bytes[num2];
					num2++;
				}
				num2 = 0;
				bytes = BitConverter.GetBytes(h.iDataSize);
				for (int l = 8; l < 12; l++)
				{
					array[l] = bytes[num2];
					num2++;
				}
				array[12] = h.key;
				array[13] = h.checkSum;
				num2 = 0;
				for (int m = 14; m < 24; m++)
				{
					array[m] = this.reserved[num2];
				}
				return array;
			}
		}

		public enum enc_type
		{
			ENC_NONE,
			ENC_DEFAULT,
			ENC_NIF,
			ENC_DDS,
			ENC_TGA,
			ENC_END
		}

		private struct __const_enc_info
		{
			public bool is_dec_recreate;

			public int mark_position;

			public int enc_size;

			public string header_string;

			public __const_enc_info(bool _is_dec_recreate, int _mark_position, int _enc_size, string _header_string)
			{
				this.is_dec_recreate = _is_dec_recreate;
				this.mark_position = _mark_position;
				this.enc_size = _enc_size;
				this.header_string = _header_string;
			}
		}

		public class enc_args
		{
			public NEncrypt.enc_type type;

			public byte[] pSrcBuf;

			public uint nSrcSize;

			public byte key;

			public uint uiSrcFileSize;

			public byte[] pOutBuf;

			public byte[] pEndBuf;

			public uint nOutSize;

			public uint nEndsize;

			public bool bOutRefPtr;

			public enc_args(NEncrypt.enc_type _type, byte[] _pSrc, uint _SrcSize, byte[] _pDest = null, bool _bDestRefPtr = false)
			{
				this.type = _type;
				this.pSrcBuf = _pSrc;
				this.nSrcSize = _SrcSize;
				this.pOutBuf = _pDest;
				this.bOutRefPtr = _bDestRefPtr;
			}
		}

		public enum PT
		{
			PT_ENC,
			PT_DEC,
			PT_AUTO
		}

		public enum process_type
		{
			process_enc_type,
			process_dec_type
		}

		public abstract class Crypto
		{
			public static NEncrypt.Crypto Create(NEncrypt.enc_type eType)
			{
				NEncrypt.Crypto result = null;
				switch (eType)
				{
				case NEncrypt.enc_type.ENC_DEFAULT:
					return new NEncrypt._Default();
				case NEncrypt.enc_type.ENC_NIF:
					return new NEncrypt._NIF();
				case NEncrypt.enc_type.ENC_DDS:
					return new NEncrypt._DDS();
				case NEncrypt.enc_type.ENC_TGA:
					return new NEncrypt._TGA();
				default:
					return result;
				}
			}

			public virtual bool IsForwarded(NEncrypt.enc_args _enc_args)
			{
				byte[] pSrcBuf = _enc_args.pSrcBuf;
				bool result = true;
				int type = (int)_enc_args.type;
				char[] array = NEncrypt.const_enc_info[type].header_string.ToCharArray();
				uint num = (uint)array.Length;
				for (uint num2 = 0u; num2 < num; num2 += 1u)
				{
					if ((char)pSrcBuf[(int)((UIntPtr)num2)] != array[(int)((UIntPtr)num2)])
					{
						result = false;
						break;
					}
				}
				return result;
			}

			public abstract void Forward(NEncrypt.enc_args _enc_args, char key = ' ');

			public abstract void Backward(NEncrypt.enc_args _enc_args, char key = ' ');

			public virtual bool Verify_File_Format(byte[] pBuffer)
			{
				return true;
			}
		}

		private class _Default : NEncrypt.Crypto
		{
			private const int HEADER_SIZE = 24;

			public override void Forward(NEncrypt.enc_args _enc_args, char key)
			{
				uint nSrcSize = _enc_args.nSrcSize;
				_enc_args.nSrcSize = _enc_args.uiSrcFileSize;
				_enc_args.nOutSize = _enc_args.nSrcSize + 24u;
				_enc_args.pOutBuf = new byte[_enc_args.nOutSize];
				NEncrypt.Header header = new NEncrypt.Header();
				Random random = new Random();
				int num;
				if (key == ' ')
				{
					num = random.Next() % 25;
				}
				else
				{
					num = (int)key;
				}
				_enc_args.key = (byte)num + 65;
				header.Init(header, (long)((ulong)nSrcSize), key);
				byte[] pOutBuf = _enc_args.pOutBuf;
				byte[] array = header.Init(header, (long)((ulong)nSrcSize), key);
				for (int i = 0; i < array.Length; i++)
				{
					pOutBuf[i] = array[i];
				}
				if (_enc_args.pOutBuf[6] == 3)
				{
					this.Type3_Forward(_enc_args.pSrcBuf, pOutBuf, nSrcSize, _enc_args.key);
				}
			}

			public override void Backward(NEncrypt.enc_args _enc_args, char key)
			{
				byte[] array = new byte[_enc_args.pSrcBuf.Length - 24];
				int num = 24;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = _enc_args.pSrcBuf[num];
					num++;
				}
				_enc_args.key = _enc_args.pSrcBuf[12];
				_enc_args.nOutSize = _enc_args.nSrcSize - 24u;
				_enc_args.pOutBuf = _enc_args.pSrcBuf;
				_enc_args.bOutRefPtr = true;
				if (_enc_args.pSrcBuf[6] == 3)
				{
					this.Type3_Backward(array, _enc_args.pOutBuf, _enc_args.nOutSize, _enc_args.key);
					_enc_args.pOutBuf = array;
				}
			}

			public override bool IsForwarded(NEncrypt.enc_args _enc_args)
			{
				return _enc_args.pSrcBuf.Length >= 10 && _enc_args.pSrcBuf[0] == 78 && _enc_args.pSrcBuf[1] == 67 && _enc_args.pSrcBuf[2] == 82 && _enc_args.pSrcBuf[4] == 1 && (_enc_args.pSrcBuf[6] == 1 || _enc_args.pSrcBuf[6] == 2 || _enc_args.pSrcBuf[6] == 3) && (uint)_enc_args.pSrcBuf[8] <= _enc_args.uiSrcFileSize - 24u;
			}

			private void Type3_Forward(byte[] pSrc, byte[] pDest, uint size, byte key)
			{
				int[] array = new int[]
				{
					1,
					3,
					2,
					3,
					1,
					5,
					4,
					2,
					1,
					4,
					2,
					8,
					4,
					2,
					6,
					8,
					2,
					6,
					4
				};
				int num = array.Length;
				uint num2 = 2821468417u + (uint)key;
				int num3 = 4;
				int num4 = (int)(size / (uint)num3);
				int num5 = (int)(size % 4u);
				uint num6 = 0u;
				int num7;
				if (num5 == 0)
				{
					num7 = num4;
				}
				else
				{
					num7 = num4 + 1;
				}
				uint[] array2 = new uint[num7];
				Buffer.BlockCopy(pSrc, 0, array2, 0, (int)size);
				for (int i = 0; i < num4; i++)
				{
					uint num8 = (uint)((ulong)num2 + (ulong)((long)(i * (int)key)) + (ulong)num6);
					array2[i] -= num8;
					num6 = array2[i];
				}
				for (int j = 0; j < num4; j++)
				{
					int num9 = array[j % num];
					int num10 = (j + num9) % num4;
					NEncrypt.Swap(ref array2[j], ref array2[num10]);
				}
				Buffer.BlockCopy(array2, 0, pDest, 24, (int)size);
			}

			private void Type3_Backward(byte[] pSrc, byte[] pDest, uint size, byte key)
			{
				int[] array = new int[]
				{
					1,
					3,
					2,
					3,
					1,
					5,
					4,
					2,
					1,
					4,
					2,
					8,
					4,
					2,
					6,
					8,
					2,
					6,
					4
				};
				int num = array.Length;
				uint num2 = 2821468417u + (uint)key;
				int num3 = 4;
				int num4 = (int)(size / (uint)num3);
				int num5 = (int)(size % (uint)num3);
				uint num6 = 0u;
				int num7;
				if (num5 != 0)
				{
					num7 = num4 + 1;
				}
				else
				{
					num7 = num4;
				}
				uint[] array2 = new uint[num7];
				Buffer.BlockCopy(pSrc, 0, array2, 0, pSrc.Length);
				int num8 = num4 - 1;
				while (-1 < num8)
				{
					int num9 = array[num8 % num];
					int num10 = (num8 + num9) % num4;
					NEncrypt.Swap(ref array2[num8], ref array2[num10]);
					num8--;
				}
				pDest = pSrc;
				for (int i = 0; i < num4; i++)
				{
					uint num11 = (uint)((ulong)num2 + (ulong)((long)(i * (int)key)) + (ulong)num6);
					num6 = array2[i];
					array2[i] += num11;
				}
				Buffer.BlockCopy(array2, 0, pDest, 0, (int)size);
			}
		}

		private class _NIF : NEncrypt.Crypto
		{
			public override void Forward(NEncrypt.enc_args _enc_args, char key)
			{
				byte[] array = new byte[]
				{
					78,
					68,
					83,
					78,
					73,
					70,
					46,
					46,
					46,
					46,
					64,
					46,
					46,
					46,
					46,
					64,
					46,
					46,
					46,
					46
				};
				uint[] array2 = new uint[11];
				Buffer.BlockCopy(_enc_args.pSrcBuf, 21, array2, 0, 44);
				for (int i = 0; i < 20; i++)
				{
					_enc_args.pSrcBuf[i] = array[i];
				}
				_enc_args.pOutBuf = _enc_args.pSrcBuf;
				int num = 0;
				for (int j = 20; j < 64; j++)
				{
					if (_enc_args.pSrcBuf[j] == 10)
					{
						array2[num] += 1600085855u;
						num++;
						break;
					}
				}
				Buffer.BlockCopy(array2, 0, _enc_args.pOutBuf, 21, 44);
			}

			public override void Backward(NEncrypt.enc_args _enc_args, char key)
			{
				byte[] array = new byte[]
				{
					71,
					97,
					109,
					101,
					98,
					114,
					121,
					111,
					32,
					70,
					105,
					108,
					101,
					32,
					70,
					111,
					114,
					109,
					97,
					116
				};
				uint[] array2 = new uint[11];
				Buffer.BlockCopy(_enc_args.pSrcBuf, 21, array2, 0, 44);
				for (int i = 0; i < 20; i++)
				{
					_enc_args.pSrcBuf[i] = array[i];
				}
				_enc_args.pOutBuf = _enc_args.pSrcBuf;
				int num = 0;
				for (int j = 20; j < 64; j++)
				{
					if (_enc_args.pSrcBuf[j] == 10)
					{
						array2[num] -= 1600085855u;
						num++;
						break;
					}
				}
				Buffer.BlockCopy(array2, 0, _enc_args.pOutBuf, 21, 44);
			}
		}

		private class _DDS : NEncrypt.Crypto
		{
			public override void Forward(NEncrypt.enc_args _enc_args, char key)
			{
				Random random = new Random();
				byte b = (byte)(random.Next() % 256);
				b = 64;
				_enc_args.pSrcBuf[0] = 78;
				_enc_args.pSrcBuf[1] = 68;
				_enc_args.pSrcBuf[2] = 83;
				_enc_args.pSrcBuf[3] = b;
				uint num = 4279269124u + (uint)b;
				uint[] array = new uint[7];
				Buffer.BlockCopy(_enc_args.pSrcBuf, 4, array, 0, 28);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] += num;
				}
				_enc_args.pOutBuf = _enc_args.pSrcBuf;
				Buffer.BlockCopy(array, 0, _enc_args.pOutBuf, 4, 28);
			}

			public override void Backward(NEncrypt.enc_args _enc_args, char key)
			{
				byte[] pSrcBuf = _enc_args.pSrcBuf;
				uint num = 4279269124u + (uint)pSrcBuf[3];
				uint[] array = new uint[7];
				Buffer.BlockCopy(pSrcBuf, 4, array, 0, 28);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] -= num;
				}
				pSrcBuf[0] = 68;
				pSrcBuf[1] = 68;
				pSrcBuf[2] = 83;
				pSrcBuf[3] = 32;
				Buffer.BlockCopy(array, 0, pSrcBuf, 4, 28);
				_enc_args.pOutBuf = pSrcBuf;
			}
		}

		public class _TGA : NEncrypt.Crypto
		{
			public struct _tga_header
			{
				public byte m_ucIDLength;

				public byte m_ucCoMapType;

				public byte m_ucImgType;

				public ushort m_usMinPalIndex;

				public ushort m_usPalLength;

				public byte m_ucCoSize;

				public ushort m_usXOrigin;

				public ushort m_usYOrigin;

				public ushort m_usWidth;

				public ushort m_usHeight;

				public byte m_ucPixelSize;

				public byte m_ucAttBits;
			}

			private const int TGA_HEADER_SIZE = 18;

			public override bool IsForwarded(NEncrypt.enc_args _enc_args)
			{
				byte[] array = new byte[18];
				array = _enc_args.pSrcBuf;
				return 200 <= array[2];
			}

			public override void Forward(NEncrypt.enc_args _enc_args, char key)
			{
				byte[] array = new byte[18];
				array = _enc_args.pSrcBuf;
				byte[] expr_16_cp_0 = array;
				int expr_16_cp_1 = 2;
				expr_16_cp_0[expr_16_cp_1] += 200;
				ushort[] array2 = new ushort[7];
				Buffer.BlockCopy(array, 3, array2, 0, 14);
				for (ushort num = 0; num < 7; num += 1)
				{
					ushort[] expr_40_cp_0 = array2;
					ushort expr_40_cp_1 = num;
					expr_40_cp_0[(int)expr_40_cp_1] = expr_40_cp_0[(int)expr_40_cp_1] + 41269 * num;
				}
				Buffer.BlockCopy(array2, 0, array, 3, 14);
				byte[] expr_6C_cp_0 = array;
				int expr_6C_cp_1 = 17;
				expr_6C_cp_0[expr_6C_cp_1] += array[7];
				_enc_args.pOutBuf = array;
			}

			public override void Backward(NEncrypt.enc_args _enc_args, char key)
			{
				byte[] pSrcBuf = _enc_args.pSrcBuf;
				byte[] expr_0E_cp_0 = pSrcBuf;
				int expr_0E_cp_1 = 2;
				expr_0E_cp_0[expr_0E_cp_1] -= 200;
				ushort[] array = new ushort[7];
				byte[] expr_27_cp_0 = pSrcBuf;
				int expr_27_cp_1 = 17;
				expr_27_cp_0[expr_27_cp_1] -= pSrcBuf[7];
				Buffer.BlockCopy(pSrcBuf, 3, array, 0, 14);
				for (ushort num = 0; num < 7; num += 1)
				{
					ushort[] expr_48_cp_0 = array;
					ushort expr_48_cp_1 = num;
					expr_48_cp_0[(int)expr_48_cp_1] = expr_48_cp_0[(int)expr_48_cp_1] - 41269 * num;
				}
				Buffer.BlockCopy(array, 0, pSrcBuf, 3, 14);
				_enc_args.pOutBuf = pSrcBuf;
			}
		}

		private static NEncrypt.__const_enc_info[] const_enc_info = new NEncrypt.__const_enc_info[]
		{
			new NEncrypt.__const_enc_info(false, 0, 0, "NoEnc"),
			new NEncrypt.__const_enc_info(true, 0, 0, "NCR"),
			new NEncrypt.__const_enc_info(false, 0, 128, "NDSNIF....@....@...."),
			new NEncrypt.__const_enc_info(false, 0, 128, "NDS"),
			new NEncrypt.__const_enc_info(false, 0, 128, "NDSTGA")
		};

		public static NEncrypt.enc_type IsForwarded(string filename)
		{
			for (NEncrypt.enc_type enc_type = NEncrypt.enc_type.ENC_DEFAULT; enc_type < NEncrypt.enc_type.ENC_END; enc_type++)
			{
				if (NEncrypt.IsForwarded(filename, enc_type))
				{
					return enc_type;
				}
			}
			return NEncrypt.enc_type.ENC_NONE;
		}

		public static NEncrypt.enc_type IsForwarded(Stream stream)
		{
			for (NEncrypt.enc_type enc_type = NEncrypt.enc_type.ENC_DEFAULT; enc_type < NEncrypt.enc_type.ENC_END; enc_type++)
			{
				if (NEncrypt.IsForwarded(stream, enc_type))
				{
					return enc_type;
				}
			}
			return NEncrypt.enc_type.ENC_NONE;
		}

		public static NEncrypt.enc_type IsForwarded(byte[] pBuffer, uint nBufSize)
		{
			for (NEncrypt.enc_type enc_type = NEncrypt.enc_type.ENC_DEFAULT; enc_type < NEncrypt.enc_type.ENC_END; enc_type++)
			{
				if (NEncrypt.IsForwarded(pBuffer, nBufSize, enc_type))
				{
					return enc_type;
				}
			}
			return NEncrypt.enc_type.ENC_NONE;
		}

		public static bool ProcessFile(string filename, char key = ' ', NEncrypt.PT pt = NEncrypt.PT.PT_AUTO)
		{
			bool result = false;
			NEncrypt.enc_type enc_type = NEncrypt.enc_type.ENC_NONE;
			if (pt == NEncrypt.PT.PT_DEC || pt == NEncrypt.PT.PT_AUTO)
			{
				enc_type = NEncrypt.IsForwarded(filename);
				if (enc_type != NEncrypt.enc_type.ENC_NONE)
				{
					return NEncrypt.DecryptFile(filename, enc_type, key, null);
				}
			}
			if (pt == NEncrypt.PT.PT_ENC || pt == NEncrypt.PT.PT_AUTO)
			{
				string extension = Path.GetExtension(filename);
				if (extension.Equals(".ntx") || extension.Equals(".ndt"))
				{
					enc_type = NEncrypt.enc_type.ENC_DEFAULT;
				}
				else if (extension.Equals(".nif"))
				{
					enc_type = NEncrypt.enc_type.ENC_NIF;
				}
				else if (extension.Equals(".dds"))
				{
					enc_type = NEncrypt.enc_type.ENC_DDS;
				}
				else if (extension.Equals(".tga"))
				{
					enc_type = NEncrypt.enc_type.ENC_TGA;
				}
				bool flag = true;
				if (enc_type == NEncrypt.enc_type.ENC_NONE)
				{
					if (pt == NEncrypt.PT.PT_AUTO)
					{
						if (flag)
						{
							enc_type = NEncrypt.enc_type.ENC_DEFAULT;
						}
					}
					else
					{
						flag = true;
						enc_type = NEncrypt.enc_type.ENC_DEFAULT;
					}
				}
				if (flag)
				{
					result = (NEncrypt.IsForwarded(filename, enc_type) || NEncrypt.EncryptFile(filename, enc_type, key, null));
				}
			}
			return result;
		}

		public static bool IsForwarded(string filename, NEncrypt.enc_type eType)
		{
			if (!NEncrypt.IsExistFile(filename))
			{
				return false;
			}
			FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			bool result = NEncrypt.__isforwarded(fileStream, eType);
			fileStream.Close();
			return result;
		}

		public static bool IsForwarded(Stream stream, NEncrypt.enc_type eType)
		{
			bool result = NEncrypt.__isforwarded(stream, eType);
			if (stream.CanSeek)
			{
				stream.Position = 0L;
			}
			return result;
		}

		private static bool __isforwarded(Stream stream, NEncrypt.enc_type eType)
		{
			if (!stream.CanSeek)
			{
				return false;
			}
			int num = (int)stream.Length;
			if (num == 0)
			{
				return false;
			}
			if (num < 24)
			{
				return false;
			}
			int num2 = num;
			byte[] array = new byte[num2];
			BinaryReader binaryReader = new BinaryReader(stream);
			array = binaryReader.ReadBytes(num);
			NEncrypt.Crypto crypto = NEncrypt.Crypto.Create(eType);
			bool result = false;
			NEncrypt.enc_args enc_args = new NEncrypt.enc_args(eType, array, (uint)array.Length, null, false);
			if (crypto != null)
			{
				result = crypto.IsForwarded(enc_args);
			}
			return result;
		}

		private static bool IsForwarded(byte[] pBuffer, uint nBufSize, NEncrypt.enc_type eType)
		{
			NEncrypt.Crypto crypto = NEncrypt.Crypto.Create(eType);
			NEncrypt.enc_args enc_args = new NEncrypt.enc_args(eType, pBuffer, nBufSize, null, false);
			if (eType == NEncrypt.enc_type.ENC_DEFAULT)
			{
				enc_args.uiSrcFileSize = nBufSize;
			}
			bool result = false;
			if (crypto != null)
			{
				result = crypto.IsForwarded(enc_args);
			}
			return result;
		}

		private static bool Enc_Dec_File(string filename, NEncrypt.enc_type eType, NEncrypt.process_type _process_type, char key, string newfilename)
		{
			StreamReader streamReader = new StreamReader(new FileStream(filename, FileMode.Open));
			int num = (int)streamReader.BaseStream.Length;
			streamReader.Close();
			byte[] array = new byte[num];
			NEncrypt.enc_args enc_args = new NEncrypt.enc_args(eType, array, (uint)array.Length, null, false);
			BinaryReader binaryReader = new BinaryReader(new FileStream(filename, FileMode.Open));
			enc_args.pSrcBuf = binaryReader.ReadBytes(num);
			enc_args.nSrcSize = (uint)num;
			binaryReader.Close();
			enc_args.uiSrcFileSize = (uint)num;
			bool flag = false;
			if (_process_type != NEncrypt.process_type.process_enc_type)
			{
				if (_process_type == NEncrypt.process_type.process_dec_type)
				{
					flag = NEncrypt.Decrypt(enc_args, key);
				}
			}
			else
			{
				flag = NEncrypt.Encrypt(enc_args, key);
			}
			if (!flag)
			{
				return false;
			}
			DateTime lastWriteTime = File.GetLastWriteTime(filename);
			BinaryWriter binaryWriter = new BinaryWriter(new FileStream(filename, FileMode.Create));
			binaryWriter.Write(enc_args.pOutBuf);
			binaryWriter.Close();
			File.SetLastWriteTime(filename, lastWriteTime);
			return true;
		}

		private static bool Encrypt(NEncrypt.enc_args _enc_args, char key = ' ')
		{
			bool result = false;
			NEncrypt.Crypto crypto = NEncrypt.Crypto.Create(_enc_args.type);
			if (!crypto.IsForwarded(_enc_args) && crypto.Verify_File_Format(_enc_args.pSrcBuf))
			{
				crypto.Forward(_enc_args, key);
				result = true;
			}
			return result;
		}

		public static bool Decrypt(NEncrypt.enc_args _enc_args, char key = ' ')
		{
			bool result = false;
			NEncrypt.Crypto crypto = NEncrypt.Crypto.Create(_enc_args.type);
			if (crypto == null)
			{
				return false;
			}
			if (crypto.IsForwarded(_enc_args))
			{
				crypto.Backward(_enc_args, key);
				result = true;
			}
			return result;
		}

		private static bool DecryptFile(string filename, NEncrypt.enc_type eType, char key, string newfilename = null)
		{
			return NEncrypt.Enc_Dec_File(filename, eType, NEncrypt.process_type.process_dec_type, key, newfilename);
		}

		private static bool EncryptFile(string filename, NEncrypt.enc_type eType, char key, string newfilename = null)
		{
			return NEncrypt.Enc_Dec_File(filename, eType, NEncrypt.process_type.process_enc_type, key, newfilename);
		}

		public static void Swap(ref uint a, ref uint b)
		{
			uint num = a;
			a = b;
			b = num;
		}

		public static bool IsExistFile(string filename)
		{
			return File.Exists(filename);
		}

		public static uint GetFileSize(string filename)
		{
			if (File.Exists(filename))
			{
				FileInfo fileInfo = new FileInfo(filename);
				return Convert.ToUInt32(fileInfo.Length);
			}
			return 0u;
		}

		public static bool CheckCrypt(string filename)
		{
			uint fileSize = NEncrypt.GetFileSize(filename);
			byte[] array = File.ReadAllBytes(filename);
			int num = array.Length;
			return num >= 24 && NEncrypt.Decrypt(new NEncrypt.enc_args(NEncrypt.enc_type.ENC_DEFAULT, array, fileSize, null, false)
			{
				uiSrcFileSize = fileSize
			}, ' ');
		}
	}
}
