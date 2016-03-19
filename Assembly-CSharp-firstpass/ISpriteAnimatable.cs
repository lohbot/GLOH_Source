using System;
using UnityEngine;

public interface ISpriteAnimatable
{
	GameObject gameObject
	{
		get;
	}

	TextureAnim[] States
	{
		get;
		set;
	}
}
