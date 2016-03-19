using System;

[Serializable]
public class BlendData
{
	public string _strSourceName = string.Empty;

	public string _strTagetName = string.Empty;

	public float _fBlendingTime = NmAnimationBlendingHelper.DEFAULT_BLENDING_TIME;
}
