using System;
using UnityEngine;

public interface ISpriteMesh
{
	SpriteRoot sprite
	{
		get;
		set;
	}

	Texture texture
	{
		get;
	}

	Material material
	{
		get;
	}

	Vector3[] vertices
	{
		get;
		set;
	}

	Vector2[] uvs
	{
		get;
		set;
	}

	int[] faces
	{
		get;
		set;
	}

	Color[] colors
	{
		get;
		set;
	}

	void Init();

	void UpdateVerts();

	void UpdateUVs();

	void UpdateColors(Color color);

	void UpdateFaces();

	void Hide(bool tf);

	bool IsHidden();
}
