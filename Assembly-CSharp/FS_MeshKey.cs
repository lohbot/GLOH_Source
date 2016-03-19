using System;
using UnityEngine;

public class FS_MeshKey
{
	public bool isStatic;

	public Material mat;

	public FS_MeshKey(Material m, bool s)
	{
		this.isStatic = s;
		this.mat = m;
	}
}
