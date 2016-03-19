using System;

namespace UnityForms
{
	public class UIEmoticonInfo
	{
		public UIBaseInfoLoader infoLoader;

		public float[] delays = new float[3];

		public UIEmoticonInfo()
		{
			this.infoLoader = new UIBaseInfoLoader();
			this.delays[0] = 0f;
			this.delays[1] = 0f;
			this.delays[2] = 0f;
		}
	}
}
