using System;

namespace WellFired
{
	public class Modification
	{
		public USInternalCurve curve;

		public float newTime;

		public float newValue;

		public Modification(USInternalCurve curve, float newTime, float newValue)
		{
			this.curve = curve;
			this.newTime = newTime;
			this.newValue = newValue;
		}
	}
}
