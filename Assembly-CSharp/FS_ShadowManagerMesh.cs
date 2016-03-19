using System;
using System.Collections.Generic;
using UnityEngine;

public class FS_ShadowManagerMesh : MonoBehaviour
{
	public Material shadowMaterial;

	public bool isStatic;

	private int numShadows;

	private List<FS_ShadowSimple> shadows = new List<FS_ShadowSimple>();

	private Mesh _mesh;

	private Mesh _mesh1;

	private Mesh _mesh2;

	private bool pingPong;

	private MeshFilter _filter;

	private Renderer _ren;

	private Vector3[] _verts;

	private Vector2[] _uvs;

	private Vector3[] _norms;

	private Color[] _colors;

	private int[] _indices;

	private int blockGrowSize = 64;

	public int getNumShadows()
	{
		return this.numShadows;
	}

	public void Start()
	{
		if (this.isStatic)
		{
			this._CreateGeometry();
		}
	}

	public void registerGeometry(FS_ShadowSimple s)
	{
		if (s.shadowMaterial != this.shadowMaterial)
		{
			Debug.LogError("Shadow did not have the same material");
		}
		this.shadows.Add(s);
	}

	public void removeShadow(FS_ShadowSimple ss)
	{
		this.shadows.Remove(ss);
	}

	public void recreateStaticGeometry()
	{
		this._CreateGeometry();
	}

	private void LateUpdate()
	{
		if (!this.isStatic)
		{
			this._CreateGeometry();
		}
	}

	private Mesh _GetMesh()
	{
		this.pingPong = !this.pingPong;
		if (this.pingPong)
		{
			if (this._mesh1 == null)
			{
				this._mesh1 = new Mesh();
				this._mesh1.MarkDynamic();
				this._mesh1.hideFlags = HideFlags.DontSave;
			}
			else
			{
				this._mesh1.Clear();
			}
			return this._mesh1;
		}
		if (this._mesh2 == null)
		{
			this._mesh2 = new Mesh();
			this._mesh2.MarkDynamic();
			this._mesh2.hideFlags = HideFlags.DontSave;
		}
		else
		{
			this._mesh2.Clear();
		}
		return this._mesh2;
	}

	private void _removeDestroyedShadows()
	{
		List<FS_ShadowSimple> list = new List<FS_ShadowSimple>();
		for (int i = 0; i < this.shadows.Count; i++)
		{
			if (this.shadows[i] != null)
			{
				list.Add(this.shadows[i]);
			}
		}
		this.shadows = list;
	}

	private void _CreateGeometry()
	{
		for (int i = 0; i < this.shadows.Count; i++)
		{
			if (this.shadows[i] == null)
			{
				this._removeDestroyedShadows();
				break;
			}
		}
		this.numShadows = this.shadows.Count;
		if (this.numShadows > 0)
		{
			base.gameObject.layer = this.shadows[0].gameObject.layer;
		}
		int num = this.shadows.Count * 4;
		this._mesh = this._GetMesh();
		if (this._filter == null)
		{
			this._filter = base.GetComponent<MeshFilter>();
		}
		if (this._filter == null)
		{
			this._filter = base.gameObject.AddComponent<MeshFilter>();
		}
		if (this._ren == null)
		{
			this._ren = base.gameObject.GetComponent<MeshRenderer>();
		}
		if (this._ren == null)
		{
			this._ren = base.gameObject.AddComponent<MeshRenderer>();
			this._ren.material = this.shadowMaterial;
		}
		if (num < 65000)
		{
			int num2 = (num >> 1) * 3;
			if (this._indices == null || this._indices.Length != num2)
			{
				this._indices = new int[num2];
			}
			bool flag = false;
			int num3 = 0;
			if (this._verts != null)
			{
				num3 = this._verts.Length;
			}
			if (num > num3 || num < num3 - this.blockGrowSize)
			{
				flag = true;
				num = (Mathf.FloorToInt((float)(num / this.blockGrowSize)) + 1) * this.blockGrowSize;
			}
			if (flag)
			{
				this._verts = new Vector3[num];
				this._uvs = new Vector2[num];
				this._norms = new Vector3[num];
				this._colors = new Color[num];
			}
			int num5;
			int num4 = num5 = 0;
			for (int j = 0; j < this.shadows.Count; j++)
			{
				FS_ShadowSimple fS_ShadowSimple = this.shadows[j];
				this._verts[num5] = fS_ShadowSimple.corners[0];
				this._verts[num5 + 1] = fS_ShadowSimple.corners[1];
				this._verts[num5 + 2] = fS_ShadowSimple.corners[2];
				this._verts[num5 + 3] = fS_ShadowSimple.corners[3];
				this._indices[num4] = num5;
				this._indices[num4 + 1] = num5 + 1;
				this._indices[num4 + 2] = num5 + 2;
				this._indices[num4 + 3] = num5 + 2;
				this._indices[num4 + 4] = num5 + 3;
				this._indices[num4 + 5] = num5;
				this._uvs[num5].x = fS_ShadowSimple.uvs.x;
				this._uvs[num5].y = fS_ShadowSimple.uvs.y;
				this._uvs[num5 + 1].x = fS_ShadowSimple.uvs.x + fS_ShadowSimple.uvs.width;
				this._uvs[num5 + 1].y = fS_ShadowSimple.uvs.y;
				this._uvs[num5 + 2].x = fS_ShadowSimple.uvs.x + fS_ShadowSimple.uvs.width;
				this._uvs[num5 + 2].y = fS_ShadowSimple.uvs.y + fS_ShadowSimple.uvs.height;
				this._uvs[num5 + 3].x = fS_ShadowSimple.uvs.x;
				this._uvs[num5 + 3].y = fS_ShadowSimple.uvs.y + fS_ShadowSimple.uvs.height;
				this._norms[num5] = fS_ShadowSimple.normal;
				this._norms[num5 + 1] = fS_ShadowSimple.normal;
				this._norms[num5 + 2] = fS_ShadowSimple.normal;
				this._norms[num5 + 3] = fS_ShadowSimple.normal;
				this._colors[num5] = fS_ShadowSimple.color;
				this._colors[num5 + 1] = fS_ShadowSimple.color;
				this._colors[num5 + 2] = fS_ShadowSimple.color;
				this._colors[num5 + 3] = fS_ShadowSimple.color;
				num4 += 6;
				num5 += 4;
			}
			if (flag)
			{
				this._mesh.Clear(false);
			}
			else
			{
				this._mesh.Clear(true);
			}
			this._mesh.Clear();
			this._mesh.name = "shadow mesh";
			this._mesh.vertices = this._verts;
			this._mesh.uv = this._uvs;
			this._mesh.normals = this._norms;
			this._mesh.colors = this._colors;
			this._mesh.triangles = this._indices;
			this._mesh.RecalculateBounds();
			this._filter.mesh = this._mesh;
			if (!this.isStatic)
			{
				this.shadows.Clear();
			}
		}
		else
		{
			if (this._filter.mesh != null)
			{
				this._filter.mesh.Clear();
			}
			Debug.LogError("Too many shadows. limit is " + 16250);
		}
	}
}
