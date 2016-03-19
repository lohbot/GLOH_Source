using System;
using UnityEngine;
using UnityForms;

[ExecuteInEditMode]
public class SimpleSprite : SpriteRoot
{
	public Vector2 lowerLeftPixel;

	public Vector2 pixelDimensions;

	private EZValueChangedDelegate changeDelegate;

	public bool Visible
	{
		get
		{
			return base.gameObject.activeInHierarchy;
		}
		set
		{
			base.gameObject.SetActive(value);
		}
	}

	public override Vector2 GetDefaultPixelSize(PathFromGUIDDelegate guid2Path, AssetLoaderDelegate loader)
	{
		return this.pixelDimensions;
	}

	protected override void Awake()
	{
		base.Awake();
		this.Init();
	}

	protected override void Init()
	{
		base.Init();
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Clear()
	{
		base.Clear();
	}

	public void Setup(float w, float h, Vector2 lowerleftPixel, Vector2 pixeldimensions)
	{
		this.Setup(w, h, lowerleftPixel, pixeldimensions, this.m_spriteMesh.material);
	}

	public void Setup(float w, float h, string strUIKey)
	{
		UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(strUIKey);
		if (uIBaseInfoLoader != null)
		{
			base.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			this.Setup(w, h, new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height), material);
		}
	}

	public void Setup(float w, float h, Vector2 lowerleftPixel, Vector2 pixeldimensions, Material material)
	{
		this.width = w;
		this.height = h;
		if (NrTSingleton<UIDataManager>.Instance.LowImage)
		{
			this.lowerLeftPixel = lowerleftPixel / 2f;
			this.pixelDimensions = pixeldimensions / 2f;
		}
		else
		{
			this.lowerLeftPixel = lowerleftPixel;
			this.pixelDimensions = pixeldimensions;
		}
		if (!this.managed)
		{
			((SpriteMesh)this.m_spriteMesh).material = material;
		}
		this.Init();
	}

	public override void Copy(SpriteRoot s)
	{
		base.Copy(s);
		if (!(s is SimpleSprite))
		{
			return;
		}
		this.lowerLeftPixel = ((SimpleSprite)s).lowerLeftPixel;
		this.pixelDimensions = ((SimpleSprite)s).pixelDimensions;
		this.InitUVs();
		base.SetBleedCompensation(s.bleedCompensation);
		if (this.autoResize || this.pixelPerfect)
		{
			base.CalcSize();
		}
		else
		{
			this.SetSize(s.width, s.height);
		}
	}

	public override void InitUVs()
	{
		this.tempUV = base.PixelCoordToUVCoord(this.lowerLeftPixel);
		this.uvRect.x = this.tempUV.x;
		this.uvRect.y = this.tempUV.y;
		this.tempUV = base.PixelSpaceToUVSpace(this.pixelDimensions);
		this.uvRect.xMax = this.uvRect.x + this.tempUV.x;
		this.uvRect.yMax = this.uvRect.y + this.tempUV.y;
		this.frameInfo.uvs = this.uvRect;
		base.InitUVs();
	}

	public void SetLowerLeftPixel(Vector2 lowerLeft)
	{
		this.lowerLeftPixel = lowerLeft;
		this.tempUV = base.PixelCoordToUVCoord(this.lowerLeftPixel);
		this.uvRect.x = this.tempUV.x;
		this.uvRect.y = this.tempUV.y;
		this.tempUV = base.PixelSpaceToUVSpace(this.pixelDimensions);
		this.uvRect.xMax = this.uvRect.x + this.tempUV.x;
		this.uvRect.yMax = this.uvRect.y + this.tempUV.y;
		base.SetBleedCompensation(this.bleedCompensation);
		if (this.autoResize || this.pixelPerfect)
		{
			base.CalcSize();
		}
	}

	public void SetLowerLeftPixel(int x, int y)
	{
		this.SetLowerLeftPixel(new Vector2((float)x, (float)y));
	}

	public void SetPixelDimensions(Vector2 size)
	{
		this.pixelDimensions = size;
		this.tempUV = base.PixelSpaceToUVSpace(this.pixelDimensions);
		this.uvRect.xMax = this.uvRect.x + this.tempUV.x;
		this.uvRect.yMax = this.uvRect.y + this.tempUV.y;
		if (this.autoResize || this.pixelPerfect)
		{
			base.CalcSize();
		}
	}

	public void SetPixelDimensions(int x, int y)
	{
		this.SetPixelDimensions(new Vector2((float)x, (float)y));
	}

	public override int GetStateIndex(string stateName)
	{
		return -1;
	}

	public override void SetColor(Color c)
	{
		base.SetColor(c);
	}

	public override void SetState(int index)
	{
	}

	public static SimpleSprite Create(string name, Vector3 pos)
	{
		return (SimpleSprite)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(SimpleSprite));
	}

	public static SimpleSprite Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (SimpleSprite)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(SimpleSprite));
	}

	public override void DoMirror()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.screenSize.x == 0f || this.screenSize.y == 0f)
		{
			base.Start();
		}
		if (this.mirror == null)
		{
			this.mirror = new SimpleSpriteMirror();
			this.mirror.Mirror(this);
		}
		this.mirror.Validate(this);
		if (this.mirror.DidChange(this))
		{
			this.Init();
			this.mirror.Mirror(this);
		}
	}

	public void Setup(float w, float h, Material material)
	{
		this.width = w;
		this.height = h;
		if (!this.managed)
		{
			((SpriteMesh)this.m_spriteMesh).material = material;
		}
		this.Init();
	}

	public void SetTextureUVs(Vector2 lowerLeftPixel, Vector2 pixelDimensions)
	{
		Rect uVs = new Rect(0f, 0f, 0f, 0f);
		Vector2 vector = base.PixelCoordToUVCoord(lowerLeftPixel);
		uVs.x = vector.x;
		uVs.y = vector.y;
		vector = base.PixelSpaceToUVSpace(pixelDimensions);
		uVs.xMax = uVs.x + vector.x;
		uVs.yMax = uVs.y + vector.y;
		base.SetUVs(uVs);
	}

	public virtual void SetValueChangedDelegate(EZValueChangedDelegate del)
	{
		this.changeDelegate = del;
	}

	public virtual void AddValueChangedDelegate(EZValueChangedDelegate del)
	{
		this.changeDelegate = (EZValueChangedDelegate)Delegate.Combine(this.changeDelegate, del);
	}

	public virtual void RemoveValueChangedDelegate(EZValueChangedDelegate del)
	{
		this.changeDelegate = (EZValueChangedDelegate)Delegate.Remove(this.changeDelegate, del);
	}
}
