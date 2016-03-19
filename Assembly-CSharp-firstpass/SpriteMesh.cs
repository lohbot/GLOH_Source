using System;
using UnityEngine;

public class SpriteMesh : ISpriteMesh
{
	protected SpriteRoot m_sprite;

	protected MeshFilter meshFilter;

	protected MeshRenderer meshRenderer;

	protected Mesh m_mesh;

	protected Texture m_texture;

	protected Vector3[] m_vertices = new Vector3[4];

	protected Color[] m_colors = new Color[4];

	protected Vector2[] m_uvs = new Vector2[4];

	protected int[] m_faces = new int[6];

	public virtual SpriteRoot sprite
	{
		get
		{
			return this.m_sprite;
		}
		set
		{
			this.m_sprite = value;
			if (this.m_sprite != null)
			{
				if (this.m_sprite.spriteMesh != this)
				{
					this.m_sprite.spriteMesh = this;
				}
				this.meshFilter = (MeshFilter)this.m_sprite.gameObject.GetComponent(typeof(MeshFilter));
				if (this.meshFilter == null)
				{
					this.meshFilter = (MeshFilter)this.m_sprite.gameObject.AddComponent(typeof(MeshFilter));
				}
				this.meshRenderer = (MeshRenderer)this.m_sprite.gameObject.GetComponent(typeof(MeshRenderer));
				if (this.meshRenderer == null)
				{
					this.meshRenderer = (MeshRenderer)this.m_sprite.gameObject.AddComponent(typeof(MeshRenderer));
				}
				this.m_mesh = this.meshFilter.sharedMesh;
				if (this.meshRenderer.sharedMaterial != null)
				{
					this.m_texture = this.meshRenderer.sharedMaterial.GetTexture("_MainTex");
				}
				return;
			}
		}
	}

	public virtual Texture texture
	{
		get
		{
			return this.m_texture;
		}
	}

	public virtual Material material
	{
		get
		{
			return this.meshRenderer.sharedMaterial;
		}
		set
		{
			this.meshRenderer.sharedMaterial = value;
			if (this.meshRenderer.sharedMaterial != null)
			{
				this.m_texture = this.meshRenderer.sharedMaterial.mainTexture;
				if (this.m_sprite != null && this.m_texture != null)
				{
					this.m_sprite.SetPixelToUV(this.m_texture);
				}
			}
			else
			{
				this.m_texture = null;
			}
		}
	}

	public virtual Vector3[] vertices
	{
		get
		{
			return this.m_vertices;
		}
		set
		{
			this.m_vertices = value;
		}
	}

	public virtual Vector2[] uvs
	{
		get
		{
			return this.m_uvs;
		}
		set
		{
			this.m_uvs = value;
		}
	}

	public virtual int[] faces
	{
		get
		{
			return this.m_faces;
		}
		set
		{
			this.m_faces = value;
		}
	}

	public virtual Mesh mesh
	{
		get
		{
			if (this.m_mesh == null)
			{
				this.CreateMesh();
			}
			return this.m_mesh;
		}
		set
		{
			this.m_mesh = value;
		}
	}

	public virtual Color[] colors
	{
		get
		{
			return this.m_colors;
		}
		set
		{
			this.m_colors = value;
		}
	}

	public virtual void Init()
	{
		if (this.m_mesh == null)
		{
			this.CreateMesh();
		}
		this.m_mesh.Clear();
		this.m_mesh.vertices = this.m_vertices;
		this.m_mesh.uv = this.m_uvs;
		this.m_mesh.colors = this.m_colors;
		this.m_mesh.triangles = this.m_faces;
		this.SetWindingOrder(this.m_sprite.winding);
		this.m_sprite.InitUVs();
		this.m_sprite.SetBleedCompensation(this.m_sprite.bleedCompensation);
		if (this.m_sprite.pixelPerfect)
		{
			if (this.m_texture == null && this.meshRenderer.sharedMaterial != null)
			{
				this.m_texture = this.meshRenderer.sharedMaterial.GetTexture("_MainTex");
			}
			if (this.m_texture != null)
			{
				this.m_sprite.SetPixelToUV(this.m_texture);
			}
			if (this.m_sprite.renderCamera == null)
			{
				this.m_sprite.SetCamera(Camera.main);
			}
			else
			{
				this.m_sprite.SetCamera(this.m_sprite.renderCamera);
			}
		}
		else if (!this.m_sprite.hideAtStart)
		{
			this.m_sprite.SetSize(this.m_sprite.width, this.m_sprite.height);
		}
		this.m_mesh.RecalculateNormals();
		this.m_sprite.SetColor(this.m_sprite.color);
	}

	protected void CreateMesh()
	{
		this.meshFilter.sharedMesh = new Mesh();
		this.m_mesh = this.meshFilter.sharedMesh;
		if (this.m_sprite.persistent)
		{
			UnityEngine.Object.DontDestroyOnLoad(this.m_mesh);
		}
	}

	public virtual void UpdateVerts()
	{
		if (null != this.m_mesh)
		{
			this.m_mesh.vertices = this.m_vertices;
			this.m_mesh.RecalculateBounds();
		}
	}

	public virtual void UpdateUVs()
	{
		if (this.m_mesh != null)
		{
			this.m_mesh.uv = this.m_uvs;
		}
	}

	public virtual void UpdateColors(Color color)
	{
		this.m_colors[0] = color;
		this.m_colors[1] = color;
		this.m_colors[2] = color;
		this.m_colors[3] = color;
		if (this.m_mesh != null)
		{
			this.m_mesh.colors = this.m_colors;
		}
	}

	public void UpdateColors()
	{
		if (this.m_mesh != null)
		{
			this.m_mesh.colors = this.m_colors;
		}
	}

	public virtual void UpdateFaces()
	{
		if (null != this.m_mesh)
		{
			this.m_mesh.triangles = this.m_faces;
		}
	}

	public virtual void UpdateMesh()
	{
		if (null == this.m_mesh)
		{
			return;
		}
		this.m_mesh.Clear();
		this.m_mesh.vertices = this.m_vertices;
		this.m_mesh.uv = this.m_uvs;
		this.m_mesh.colors = this.m_colors;
		this.m_mesh.triangles = this.m_faces;
		this.m_mesh.RecalculateBounds();
	}

	public virtual void Hide(bool tf)
	{
		if (this.meshRenderer == null)
		{
			return;
		}
		this.meshRenderer.enabled = !tf;
	}

	public virtual bool IsHidden()
	{
		return this.meshRenderer == null || !this.meshRenderer.enabled;
	}

	public void SetPersistent()
	{
		if (this.m_mesh != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(this.m_mesh);
		}
	}

	public virtual void SetWindingOrder(SpriteRoot.WINDING_ORDER winding)
	{
		this.m_faces[0] = 0;
		this.m_faces[1] = 3;
		this.m_faces[2] = 1;
		this.m_faces[3] = 3;
		this.m_faces[4] = 2;
		this.m_faces[5] = 1;
		if (this.m_mesh != null)
		{
			this.m_mesh.triangles = this.m_faces;
		}
	}
}
