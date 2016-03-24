using System;
using UnityEngine;

namespace WellFired.Shared
{
	public class Fractal
	{
		private Perlin noise;

		private float[] exponent;

		private int intOctaves;

		private float floatOctaves;

		private float lacunarity;

		public Fractal(int seed, float inH, float inLacunarity, float inOctaves) : this(seed, inH, inLacunarity, inOctaves, null)
		{
		}

		public Fractal(int seed, float inH, float inLacunarity, float inOctaves, Perlin passedNoise)
		{
			this.lacunarity = inLacunarity;
			this.floatOctaves = inOctaves;
			this.intOctaves = (int)inOctaves;
			this.exponent = new float[this.intOctaves + 1];
			float num = 1f;
			for (int i = 0; i < this.intOctaves + 1; i++)
			{
				this.exponent[i] = (float)Math.Pow((double)this.lacunarity, (double)(-(double)inH));
				num *= this.lacunarity;
			}
			if (passedNoise == null)
			{
				this.noise = new Perlin(seed);
			}
			else
			{
				this.noise = passedNoise;
			}
		}

		public float HybridMultifractal(float x, float y, float offset)
		{
			float num = (this.noise.Noise(x, y) + offset) * this.exponent[0];
			float num2 = num;
			x *= this.lacunarity;
			y *= this.lacunarity;
			int i;
			for (i = 1; i < this.intOctaves; i++)
			{
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				float num3 = (this.noise.Noise(x, y) + offset) * this.exponent[i];
				num += num2 * num3;
				num2 *= num3;
				x *= this.lacunarity;
				y *= this.lacunarity;
			}
			float num4 = this.floatOctaves - (float)this.intOctaves;
			return num + num4 * this.noise.Noise(x, y) * this.exponent[i];
		}

		public float RidgedMultifractal(float x, float y, float offset, float gain)
		{
			float num = Mathf.Abs(this.noise.Noise(x, y));
			num = offset - num;
			num *= num;
			float num2 = num;
			for (int i = 1; i < this.intOctaves; i++)
			{
				x *= this.lacunarity;
				y *= this.lacunarity;
				float num3 = num * gain;
				num3 = Mathf.Clamp01(num3);
				num = Mathf.Abs(this.noise.Noise(x, y));
				num = offset - num;
				num *= num;
				num *= num3;
				num2 += num * this.exponent[i];
			}
			return num2;
		}

		public float BrownianMotion(float x, float y)
		{
			float num = 0f;
			long num2;
			for (num2 = 0L; num2 < (long)this.intOctaves; num2 += 1L)
			{
				num = this.noise.Noise(x, y) * this.exponent[(int)(checked((IntPtr)num2))];
				x *= this.lacunarity;
				y *= this.lacunarity;
			}
			float num3 = this.floatOctaves - (float)this.intOctaves;
			return num + num3 * this.noise.Noise(x, y) * this.exponent[(int)(checked((IntPtr)num2))];
		}
	}
}
