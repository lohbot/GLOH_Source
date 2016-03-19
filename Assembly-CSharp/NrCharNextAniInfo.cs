using GAME;
using System;

public class NrCharNextAniInfo
{
	public eCharAnimationType eAnimationType;

	public bool bBlend;

	public NrCharNextAniInfo(eCharAnimationType anitype, bool blend)
	{
		this.eAnimationType = anitype;
		this.bBlend = blend;
	}
}
