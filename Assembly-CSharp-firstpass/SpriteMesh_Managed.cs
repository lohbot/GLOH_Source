using System;
using UnityEngine;

public class SpriteMesh_Managed : ISpriteMesh, IEZLinkedListItem<SpriteMesh_Managed>
{
	protected SpriteRoot m_sprite;

	protected bool hidden;

	public int index;

	public int drawLayer;

	public SpriteManager m_manager;

	public SpriteMesh_Managed m_next;

	public SpriteMesh_Managed m_prev;

	protected Vector3[] m_vertices = new Vector3[4];

	protected Vector2[] m_uvs = new Vector2[4];

	protected Material m_material;

	protected Texture m_texture;

	protected Vector3[] meshVerts;

	protected Vector2[] meshUVs;

	protected Color[] meshColors;

	public int mv1;

	public int mv2;

	public int mv3;

	public int mv4;

	public int uv1;

	public int uv2;

	public int uv3;

	public int uv4;

	public int cv1;

	public int cv2;

	public int cv3;

	public int cv4;

	public SpriteManager manager
	{
		get
		{
			return this.m_manager;
		}
		set
		{
			this.m_manager = value;
			this.m_material = this.m_manager.renderer.sharedMaterial;
			if (this.m_material != null)
			{
				this.m_texture = this.m_material.GetTexture("_MainTex");
			}
		}
	}

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
				this.UpdateColors(this.m_sprite.color);
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
			return this.m_material;
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
			return null;
		}
		set
		{
		}
	}

	public virtual Color[] colors
	{
		get
		{
			return this.meshColors;
		}
		set
		{
			this.meshColors = value;
		}
	}

	public SpriteMesh_Managed prev
	{
		get
		{
			return this.m_prev;
		}
		set
		{
			this.m_prev = value;
		}
	}

	public SpriteMesh_Managed next
	{
		get
		{
			return this.m_next;
		}
		set
		{
			this.m_next = value;
		}
	}

	public void SetBuffers(Vector3[] verts, Vector2[] uvs, Color[] cols)
	{
		this.meshVerts = verts;
		this.meshUVs = uvs;
		this.meshColors = cols;
	}

	public void Clear()
	{
		this.hidden = false;
	}

	public virtual void Init()
	{
		if (!this.m_sprite.Started)
		{
			return;
		}
		this.m_sprite.InitUVs();
		this.m_sprite.SetBleedCompensation(this.m_sprite.bleedCompensation);
		if (this.m_sprite.pixelPerfect)
		{
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
		this.m_sprite.SetColor(this.m_sprite.color);
	}

	public virtual void UpdateVerts()
	{
		if (this.hidden)
		{
			return;
		}
		this.meshVerts[this.mv1] = this.m_vertices[0];
		this.meshVerts[this.mv2] = this.m_vertices[1];
		this.meshVerts[this.mv3] = this.m_vertices[2];
		this.meshVerts[this.mv4] = this.m_vertices[3];
		this.m_manager.UpdatePositions();
	}

	public virtual void UpdateUVs()
	{
		this.meshUVs[this.uv1] = this.uvs[0];
		this.meshUVs[this.uv2] = this.uvs[1];
		this.meshUVs[this.uv3] = this.uvs[2];
		this.meshUVs[this.uv4] = this.uvs[3];
		this.m_manager.UpdateUVs();
	}

	public virtual void UpdateColors(Color color)
	{
		this.meshColors[this.cv1] = color;
		this.meshColors[this.cv2] = color;
		this.meshColors[this.cv3] = color;
		this.meshColors[this.cv4] = color;
		this.m_manager.UpdateColors();
	}

	public virtual void UpdateFaces()
	{
	}

	public virtual void Hide(bool tf)
	{
		if (tf)
		{
			this.m_vertices[0] = Vector3.zero;
			this.m_vertices[1] = Vector3.zero;
			this.m_vertices[2] = Vector3.zero;
			this.m_vertices[3] = Vector3.zero;
			this.UpdateVerts();
			this.hidden = tf;
		}
		else
		{
			this.hidden = tf;
			if (this.m_sprite.pixelPerfect)
			{
				this.m_sprite.CalcSize();
			}
			else
			{
				this.m_sprite.SetSize(this.m_sprite.width, this.m_sprite.height);
			}
		}
	}

	public virtual bool IsHidden()
	{
		return this.hidden;
	}
}
