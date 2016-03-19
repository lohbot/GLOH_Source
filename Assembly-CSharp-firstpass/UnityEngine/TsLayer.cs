using System;

namespace UnityEngine
{
	public struct TsLayer
	{
		private int m_value;

		public static TsLayer DEFAULT = new TsLayer(0);

		public static TsLayer TRANSPARENT_FX = new TsLayer(1);

		public static TsLayer IGNORE_RAYCAST = new TsLayer(2);

		public static TsLayer WATER = new TsLayer(4);

		public static TsLayer WATER_COLLISION = new TsLayer(9);

		public static TsLayer TERRAIN = new TsLayer(8);

		public static TsLayer PC = new TsLayer(10);

		public static TsLayer PC_DECORATION = new TsLayer(11);

		public static TsLayer NPC = new TsLayer(12);

		public static TsLayer BULLET = new TsLayer(13);

		public static TsLayer BLOCK = new TsLayer(14);

		public static TsLayer SMALL_OBJECT = new TsLayer(15);

		public static TsLayer MEDIUM_OBJECT = new TsLayer(16);

		public static TsLayer LANDSCAPE = new TsLayer(17);

		public static TsLayer PC_OTHER = new TsLayer(18);

		public static TsLayer DETAIL = new TsLayer(19);

		public static TsLayer FADE_OBJECT = new TsLayer(20);

		public static TsLayer LARGE_OBJECT = new TsLayer(21);

		public static TsLayer EFFECT_LOW = new TsLayer(25);

		public static TsLayer EFFECT_MIDDLE = new TsLayer(26);

		public static TsLayer EFFECT_HIGH = new TsLayer(27);

		public static TsLayer IGNORE_PICK = new TsLayer(28);

		public static TsLayer PICKING_ONLY = new TsLayer(29);

		public static TsLayer IGNORE_LIGHT = new TsLayer(30);

		public static TsLayer GUI = new TsLayer(31);

		public static TsLayerMask EVERYTHING = new TsLayerMask(4294967295u);

		public static TsLayerMask NOTHING = new TsLayerMask(0u);

		public uint Bit
		{
			get
			{
				return 1u << this.m_value;
			}
		}

		private TsLayer(int val)
		{
			this.m_value = val;
		}

		public override string ToString()
		{
			return LayerMask.LayerToName(this.m_value);
		}

		public static bool UnitTest()
		{
			if (TsLayer.GUI != -2147483648)
			{
				return false;
			}
			if (TsLayer.DEFAULT != 0)
			{
				return false;
			}
			TsLayerMask tsLayerMask = new TsLayerMask(TsLayer.SMALL_OBJECT);
			if (tsLayerMask != 32768)
			{
				return false;
			}
			TsLayerMask layerMask = TsLayer.SMALL_OBJECT;
			if (layerMask != 32768)
			{
				return false;
			}
			TsLayerMask layerMask2 = TsLayer.PC + TsLayer.PC_DECORATION + TsLayer.NPC + TsLayer.PC_OTHER;
			if (layerMask2 != 7168)
			{
				return false;
			}
			TsLayerMask layerMask3 = layerMask2 - TsLayer.PC_DECORATION;
			if (layerMask3 != 5120)
			{
				return false;
			}
			TsLayerMask layerMask4 = tsLayerMask;
			if (layerMask4 != 32768)
			{
				return false;
			}
			TsLayerMask layerMask5 = TsLayer.TERRAIN;
			if (layerMask5 != 256)
			{
				return false;
			}
			int num = layerMask5;
			if (num != 256)
			{
				return false;
			}
			string a = TsLayer.TERRAIN;
			return !(a != "TERRAIN") && !(a != LayerMask.LayerToName(8));
		}

		public static TsLayerMask operator +(TsLayer layer1, TsLayer layer2)
		{
			uint bit = layer1.Bit | layer2.Bit;
			return new TsLayerMask(bit);
		}

		public static TsLayerMask operator +(TsLayerMask layerMask, TsLayer layer2)
		{
			uint bit = layerMask | layer2.Bit;
			return new TsLayerMask(bit);
		}

		public static TsLayerMask operator -(TsLayer layer1, TsLayer layer2)
		{
			uint bit = layer1.Bit & ~layer2.Bit;
			return new TsLayerMask(bit);
		}

		public static TsLayerMask operator -(TsLayerMask layerMask, TsLayer layer2)
		{
			uint bit = layerMask & ~layer2.Bit;
			return new TsLayerMask(bit);
		}

		public static implicit operator int(TsLayer layer)
		{
			return layer.m_value;
		}

		public static implicit operator TsLayerMask(TsLayer layer)
		{
			return new TsLayerMask(layer.Bit);
		}

		public static implicit operator string(TsLayer layer)
		{
			return layer.ToString();
		}
	}
}
