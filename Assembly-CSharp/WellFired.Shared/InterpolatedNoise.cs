using System;
using UnityEngine;

namespace WellFired.Shared
{
	public class InterpolatedNoise
	{
		public Fractal Fractal
		{
			get;
			set;
		}

		public int Seed
		{
			get;
			set;
		}

		public InterpolatedNoise(int seed)
		{
			this.Seed = seed;
			this.Fractal = new Fractal(this.Seed, 1.27f, 2.04f, 8.36f);
		}

		public Vector3 GetVector3(float speed, float time)
		{
			float x = time * 0.01f * speed;
			return new Vector3(this.Fractal.HybridMultifractal(x, 15.73f, 0f), this.Fractal.HybridMultifractal(x, 63.94f, 0f), this.Fractal.HybridMultifractal(x, 0.2f, 0f));
		}

		public float GetFloat(float speed, float time)
		{
			float x = time * 0.01f * speed;
			return this.Fractal.HybridMultifractal(x, 15.7f, 0.65f);
		}
	}
}
