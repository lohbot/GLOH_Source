using System;
using System.Net.Sockets;
using UnityEngine;

public class ReceiveBuffer
{
	private byte[] mBufferPool;

	private int mIndex;

	private int mReceiveIndex;

	private int mTotalLen;

	public int Index
	{
		get
		{
			return this.mIndex;
		}
	}

	public int TotalLen
	{
		get
		{
			return this.mTotalLen;
		}
	}

	public byte[] Buffer
	{
		get
		{
			return this.mBufferPool;
		}
	}

	public ReceiveBuffer(int Length)
	{
		this.mBufferPool = new byte[Length];
		this.ClearBuffer();
	}

	private void ClearBuffer()
	{
		this.mIndex = (this.mReceiveIndex = 0);
		this.mTotalLen = 0;
		Array.Clear(this.mBufferPool, 0, this.mBufferPool.Length);
	}

	private void SetRewindBuffer()
	{
		Array.Copy(this.mBufferPool, this.mIndex, this.mBufferPool, 0, this.mTotalLen);
		this.mIndex = 0;
		this.mReceiveIndex = this.mIndex + this.mTotalLen;
		TsLog.Log(string.Concat(new object[]
		{
			"ReWind",
			this.mTotalLen,
			":",
			this.Print()
		}), new object[0]);
	}

	public bool Receive(Socket socket, SocketFlags option)
	{
		if (socket != null && socket.Connected && 0 < socket.Available)
		{
			int available = socket.Available;
			if (this.mReceiveIndex + available > this.mBufferPool.Length)
			{
				TsLog.LogError("Offset OVER Rewind {0}({1} + {2}) > {3} ", new object[]
				{
					this.mReceiveIndex + available,
					this.mReceiveIndex,
					available,
					this.mBufferPool.Length
				});
				this.SetRewindBuffer();
			}
			int num = socket.Receive(this.mBufferPool, this.mReceiveIndex, available, option);
			if (available != num)
			{
				Debug.LogError(string.Concat(new object[]
				{
					":NotMathch",
					available,
					", ",
					num
				}));
			}
			this.mTotalLen += num;
			this.mReceiveIndex += num;
			return true;
		}
		return false;
	}

	public bool ReceivedComplete(int CompleteLen)
	{
		this.mTotalLen -= Mathf.Abs(this.mIndex - CompleteLen);
		if (0 >= this.mTotalLen)
		{
			this.ClearBuffer();
		}
		else
		{
			this.mIndex = CompleteLen;
			this.mReceiveIndex = this.mIndex + this.mTotalLen;
			Debug.Log(this.mIndex + "Remain:" + this.mTotalLen);
		}
		return this.mTotalLen == CompleteLen;
	}

	private string Print()
	{
		string empty = string.Empty;
		return string.Format("Offset({0}) , Total({1}) , Index({2}) ", this.mReceiveIndex, this.mTotalLen, this.mIndex);
	}
}
