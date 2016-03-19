using System;
using UnityEngine;

public interface ISpriteAggregator
{
	Texture2D[] SourceTextures
	{
		get;
	}

	CSpriteFrame[] SpriteFrames
	{
		get;
	}

	void Aggregate(PathFromGUIDDelegate guid2Path, LoadAssetDelegate load, GUIDFromPathDelegate path2Guid);
}
