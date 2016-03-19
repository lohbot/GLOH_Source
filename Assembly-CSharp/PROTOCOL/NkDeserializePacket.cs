using System;
using UnityEngine;

namespace PROTOCOL
{
	public class NkDeserializePacket
	{
		private byte[] m_btBuffer;

		private int m_nStartIndex;

		private int m_nTotalReadByte;

		public int TotalReadByte
		{
			get
			{
				return this.m_nTotalReadByte;
			}
		}

		public NkDeserializePacket(byte[] btBuffer, int nStartIndex)
		{
			this.m_btBuffer = btBuffer;
			this.m_nStartIndex = nStartIndex;
			this.m_nTotalReadByte = 0;
		}

		public void SetDeserializePacket(byte[] btBuffer, int nStartIndex)
		{
			this.m_btBuffer = btBuffer;
			this.m_nStartIndex = nStartIndex;
			this.m_nTotalReadByte = 0;
		}

		public T GetPacket<T>()
		{
			int num = 0;
			T result = (T)((object)ReceivePakcet.DeserializePacket(this.m_btBuffer, this.m_nStartIndex + this.m_nTotalReadByte, out num, typeof(T)));
			this.m_nTotalReadByte += num;
			return result;
		}

		public override string ToString()
		{
			return string.Format("DeserializePacket Info - StartIndex : {0} / TotalReadByte : {1} / BufferSize : {2} / {3}", new object[]
			{
				this.m_nStartIndex,
				this.m_nTotalReadByte,
				this.m_btBuffer.Length,
				Time.time.ToString()
			});
		}
	}
}
