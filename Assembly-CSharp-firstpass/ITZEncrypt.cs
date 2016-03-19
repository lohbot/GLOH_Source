using System;

public class ITZEncrypt
{
	private const ushort PACKET_SIZE_MAX = 24576;

	private const ushort HEADER_SIZE = 9;

	private byte m_ucKey1;

	private byte m_ucKey2;

	private bool m_bKeyFixed;

	public static bool bUseEncrypt = true;

	public void SetKey(byte ucKey1, byte ucKey2, bool bKeyFixed)
	{
		this.m_ucKey1 = ucKey1;
		this.m_ucKey2 = ucKey2;
		this.m_bKeyFixed = bKeyFixed;
	}

	public void Encode(byte[] pData)
	{
		if (!ITZEncrypt.bUseEncrypt)
		{
			return;
		}
		if (!this.m_bKeyFixed)
		{
			this.m_ucKey1 += 1;
			this.m_ucKey2 += 1;
		}
		ushort num = (ushort)(BitConverter.ToInt16(pData, 0) + 9);
		if (3 < num)
		{
			byte b = pData[(int)(num - 1)] + this.m_ucKey1;
			byte b2 = this.m_ucKey1;
			byte b3 = this.m_ucKey2;
			int num2 = (int)num;
			for (int i = 3; i < num2; i++)
			{
				byte b4 = pData[i];
				b2 ^= b4;
				pData[i] = b2 + b3;
				b3 += b4;
			}
			pData[2] = b;
		}
	}

	public void Encode(byte[] pData, int index)
	{
		if (!ITZEncrypt.bUseEncrypt)
		{
			return;
		}
		if (!this.m_bKeyFixed)
		{
			this.m_ucKey1 += 1;
			this.m_ucKey2 += 1;
		}
		ushort num = (ushort)(BitConverter.ToInt16(pData, index) + 9);
		if (3 < num)
		{
			byte b = pData[index + (int)(num - 1)] + this.m_ucKey1;
			byte b2 = this.m_ucKey1;
			byte b3 = this.m_ucKey2;
			int num2 = (int)num;
			for (int i = index + 3; i < index + num2; i++)
			{
				byte b4 = pData[i];
				b2 ^= b4;
				pData[i] = b2 + b3;
				b3 += b4;
			}
			pData[index + 2] = b;
		}
	}

	public bool Decode(byte[] pData, int index)
	{
		if (!ITZEncrypt.bUseEncrypt)
		{
			return true;
		}
		if (!this.m_bKeyFixed)
		{
			this.m_ucKey1 += 1;
			this.m_ucKey2 += 1;
		}
		ushort num = (ushort)(BitConverter.ToInt16(pData, index) + 9);
		if (num > 24576)
		{
			TsLog.LogWarning(string.Format("Decode Fail Size {0} > {1}", num, 24576), new object[0]);
			return false;
		}
		if (3 >= num)
		{
			return true;
		}
		pData[index + 2] = pData[index + 2] - this.m_ucKey1;
		byte b = this.m_ucKey1;
		byte b2 = this.m_ucKey2;
		ushort num2 = num;
		byte b3 = pData[index + 2];
		for (int i = 3; i < (int)num2; i++)
		{
			byte b4 = pData[index + i];
			pData[index + i] = b4 - b2;
			pData[index + i] = (pData[index + i] ^ b);
			b ^= pData[index + i];
			b2 += pData[index + i];
		}
		if (b3 == pData[index + (int)num2 - 1])
		{
			return true;
		}
		string message = string.Format("Decode Fail CheckSum:{0} != {1} , end {2} Index {3}", new object[]
		{
			b3,
			pData[index + (int)num2 - 1],
			num2,
			index + (int)num2 - 1
		});
		TsLog.LogWarning(message, new object[0]);
		return false;
	}
}
