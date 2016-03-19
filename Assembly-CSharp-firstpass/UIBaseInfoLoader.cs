using System;
using UnityEngine;

[Serializable]
public class UIBaseInfoLoader
{
	private SpriteTile.SPRITE_TILE_MODE eTile;

	private string szMaterial = string.Empty;

	private Rect rUVs = new Rect(0f, 0f, 1f, 1f);

	private byte bytButtonCount;

	private bool m_bPattern;

	private string styleName = string.Empty;

	public SpriteTile.SPRITE_TILE_MODE Tile
	{
		get
		{
			return this.eTile;
		}
		set
		{
			this.eTile = value;
		}
	}

	public string Material
	{
		get
		{
			return this.szMaterial;
		}
		set
		{
			this.szMaterial = value;
		}
	}

	public Rect UVs
	{
		get
		{
			return this.rUVs;
		}
		set
		{
			this.rUVs = value;
		}
	}

	public byte ButtonCount
	{
		get
		{
			return this.bytButtonCount;
		}
		set
		{
			this.bytButtonCount = value;
		}
	}

	public bool Pattern
	{
		get
		{
			return this.m_bPattern;
		}
		set
		{
			this.m_bPattern = value;
		}
	}

	public string StyleName
	{
		get
		{
			return this.styleName;
		}
		set
		{
			this.styleName = value;
		}
	}

	public UIBaseInfoLoader()
	{
		this.Initialize();
	}

	public void Initialize()
	{
		this.eTile = SpriteTile.SPRITE_TILE_MODE.STM_MIN;
		this.szMaterial = string.Empty;
		this.rUVs = new Rect(0f, 0f, 1f, 1f);
		this.bytButtonCount = 0;
		this.m_bPattern = false;
		this.styleName = string.Empty;
	}

	public void Set(UIBaseInfoLoader baseInfoLoader)
	{
		this.eTile = baseInfoLoader.eTile;
		this.szMaterial = baseInfoLoader.szMaterial;
		this.rUVs = baseInfoLoader.rUVs;
		this.bytButtonCount = baseInfoLoader.bytButtonCount;
		this.m_bPattern = baseInfoLoader.m_bPattern;
		this.styleName = baseInfoLoader.styleName;
	}

	public Texture GetTexTure()
	{
		return new Texture();
	}
}
