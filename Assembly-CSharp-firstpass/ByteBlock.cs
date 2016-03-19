using System;
using System.Collections.Generic;

public class ByteBlock
{
	private List<byte[]> BufferList;

	private int TotalByteLen;

	public ByteBlock()
	{
		this.BufferList = new List<byte[]>();
		this.TotalByteLen = 0;
	}

	public void Add(byte[] _Buffer)
	{
		if (_Buffer != null)
		{
			this.BufferList.Add(_Buffer);
			this.TotalByteLen += _Buffer.Length;
		}
	}

	public byte[] GetAlloc()
	{
		byte[] array = new byte[this.TotalByteLen];
		int num = 0;
		foreach (byte[] current in this.BufferList)
		{
			Buffer.BlockCopy(current, 0, array, num, current.Length);
			num += current.Length;
		}
		return array;
	}
}
