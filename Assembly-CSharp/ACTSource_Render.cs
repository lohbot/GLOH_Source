using System;
using UnityEngine;

public class ACTSource_Render : ACTSource
{
	private Renderer mSrcRender;

	public ACTSource_Render(Renderer Src)
	{
		this.mSrcRender = Src;
	}

	protected override bool IsValid()
	{
		return null != this.mSrcRender;
	}

	protected override void Disable()
	{
		this.mSrcRender.enabled = false;
	}

	protected override void Active()
	{
		this.mSrcRender.enabled = true;
	}
}
