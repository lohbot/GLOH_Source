using System;
using System.Net.Sockets;
using UnityEngine;

public class SendBuffer
{
	public static int SEND_BUFFER_SIZE = 16384;

	private byte[] mBufferPool;

	private int mLen;

	public int TotalLen
	{
		get
		{
			return this.mLen;
		}
	}

	public byte[] Buffer
	{
		get
		{
			return this.mBufferPool;
		}
	}

	public SendBuffer(int Length)
	{
		this.mBufferPool = new byte[Length];
		this.mLen = 0;
	}

	public void InsertMerge(byte[] btHeader, byte[] btBody)
	{
		this.mLen = 0;
		this.InserBuffer(btHeader);
		this.InserBuffer(btBody);
	}

	private void InserBuffer(byte[] btArray)
	{
		if (btArray != null && btArray.Length > SendBuffer.SEND_BUFFER_SIZE)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Send Packet Size Overflow!!!  CURRENT SIZE=",
				btArray.Length,
				" BUFFER_SIZE : ",
				SendBuffer.SEND_BUFFER_SIZE
			}));
		}
		if (btArray != null)
		{
			Array.Copy(btArray, 0, this.mBufferPool, this.mLen, btArray.Length);
			this.mLen += btArray.Length;
		}
	}

	public void Send(Socket socket, ITZEncrypt Encrypt)
	{
		Encrypt.Encode(this.mBufferPool);
		socket.Send(this.mBufferPool, 0, this.mLen, SocketFlags.None);
	}
}
