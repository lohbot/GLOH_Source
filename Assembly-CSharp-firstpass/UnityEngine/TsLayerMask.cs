using System;

namespace UnityEngine
{
	public struct TsLayerMask
	{
		private uint m_bits;

		public uint Bits
		{
			get
			{
				return this.m_bits;
			}
		}

		public uint InverseBits
		{
			get
			{
				return ~this.m_bits;
			}
		}

		public TsLayerMask(uint bit)
		{
			this.m_bits = bit;
		}

		public TsLayerMask(TsLayer layer)
		{
			this.m_bits = layer.Bit;
		}

		public uint Inverse()
		{
			this.m_bits = ~this.m_bits;
			return this.m_bits;
		}

		public bool Contains(TsLayer layer)
		{
			return (this.m_bits & layer.Bit) != 0u;
		}

		public override string ToString()
		{
			string text = string.Empty;
			for (int i = 0; i < 32; i++)
			{
				if (((ulong)this.m_bits & (ulong)(1L << (i & 31))) != 0uL)
				{
					text += LayerMask.LayerToName(i);
					text += ";";
				}
			}
			return text;
		}

		public static TsLayerMask Parse(string layerNames)
		{
			string[] array = layerNames.Split(new char[]
			{
				';'
			});
			uint num = 0u;
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = LayerMask.NameToLayer(array[i]);
				num |= 1u << num2;
			}
			return new TsLayerMask(num);
		}

		public static implicit operator uint(TsLayerMask layerMask)
		{
			return layerMask.m_bits;
		}

		public static implicit operator int(TsLayerMask layerMask)
		{
			return (int)layerMask.m_bits;
		}

		public static implicit operator LayerMask(TsLayerMask layerMask)
		{
			return (int)layerMask.m_bits;
		}

		public static TsLayerMask operator +(TsLayerMask layerMask1, TsLayerMask layerMask2)
		{
			uint bit = layerMask1 | layerMask2;
			return new TsLayerMask(bit);
		}
	}
}
