using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(SkinnedMeshRenderer))]
public class SpriteManager : MonoBehaviour
{
	public SpriteRoot.WINDING_ORDER winding = SpriteRoot.WINDING_ORDER.CW;

	public int allocBlockSize = 10;

	public bool autoUpdateBounds = true;

	public bool drawBoundingBox;

	public bool persistent;

	protected bool initialized;

	protected EZLinkedList<SpriteMesh_Managed> availableBlocks = new EZLinkedList<SpriteMesh_Managed>();

	protected bool vertsChanged;

	protected bool uvsChanged;

	protected bool colorsChanged;

	protected bool vertCountChanged;

	protected bool updateBounds;

	protected SpriteMesh_Managed[] sprites;

	protected EZLinkedList<SpriteMesh_Managed> activeBlocks = new EZLinkedList<SpriteMesh_Managed>();

	protected List<SpriteMesh_Managed> spriteDrawOrder = new List<SpriteMesh_Managed>();

	protected SpriteDrawLayerComparer drawOrderComparer = new SpriteDrawLayerComparer();

	protected float boundUpdateInterval;

	protected List<SpriteRoot> spriteAddQueue;

	protected SkinnedMeshRenderer meshRenderer;

	protected Mesh mesh;

	protected Texture texture;

	protected Transform[] bones;

	protected BoneWeight[] boneWeights;

	protected Matrix4x4[] bindPoses;

	protected Vector3[] vertices;

	protected int[] triIndices;

	protected Vector2[] UVs;

	protected Color[] colors;

	protected SpriteMesh_Managed tempSprite;

	public bool IsInitialized
	{
		get
		{
			return this.initialized;
		}
	}

	public Vector2 PixelSpaceToUVSpace(Vector2 xy)
	{
		if (this.texture == null)
		{
			return Vector2.zero;
		}
		return new Vector2(xy.x / (float)this.texture.width, xy.y / (float)this.texture.height);
	}

	public Vector2 PixelSpaceToUVSpace(int x, int y)
	{
		return this.PixelSpaceToUVSpace(new Vector2((float)x, (float)y));
	}

	public Vector2 PixelCoordToUVCoord(Vector2 xy)
	{
		if (this.texture == null)
		{
			return Vector2.zero;
		}
		return new Vector2(xy.x / ((float)this.texture.width - 1f), 1f - xy.y / ((float)this.texture.height - 1f));
	}

	public Vector2 PixelCoordToUVCoord(int x, int y)
	{
		return this.PixelCoordToUVCoord(new Vector2((float)x, (float)y));
	}

	protected void SetupBoneWeights(SpriteMesh_Managed s)
	{
		this.boneWeights[s.mv1].boneIndex0 = s.index;
		this.boneWeights[s.mv1].weight0 = 1f;
		this.boneWeights[s.mv2].boneIndex0 = s.index;
		this.boneWeights[s.mv2].weight0 = 1f;
		this.boneWeights[s.mv3].boneIndex0 = s.index;
		this.boneWeights[s.mv3].weight0 = 1f;
		this.boneWeights[s.mv4].boneIndex0 = s.index;
		this.boneWeights[s.mv4].weight0 = 1f;
	}

	private void Awake()
	{
		if (this.spriteAddQueue == null)
		{
			this.spriteAddQueue = new List<SpriteRoot>();
		}
		this.meshRenderer = (SkinnedMeshRenderer)base.GetComponent(typeof(SkinnedMeshRenderer));
		if (this.meshRenderer != null && this.meshRenderer.sharedMaterial != null)
		{
			this.texture = this.meshRenderer.sharedMaterial.GetTexture("_MainTex");
		}
		if (this.meshRenderer.sharedMesh == null)
		{
			this.meshRenderer.sharedMesh = new Mesh();
		}
		this.mesh = this.meshRenderer.sharedMesh;
		if (this.persistent)
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
			UnityEngine.Object.DontDestroyOnLoad(this.mesh);
		}
		this.EnlargeArrays(this.allocBlockSize);
		base.transform.rotation = Quaternion.identity;
		this.initialized = true;
		for (int i = 0; i < this.spriteAddQueue.Count; i++)
		{
			this.AddSprite(this.spriteAddQueue[i]);
		}
	}

	protected void InitArrays()
	{
		this.bones = new Transform[1];
		this.bones[0] = base.transform;
		this.bindPoses = new Matrix4x4[1];
		this.sprites = new SpriteMesh_Managed[1];
		this.sprites[0] = new SpriteMesh_Managed();
		this.vertices = new Vector3[4];
		this.UVs = new Vector2[4];
		this.colors = new Color[4];
		this.triIndices = new int[6];
		this.boneWeights = new BoneWeight[4];
		this.sprites[0].index = 0;
		this.sprites[0].mv1 = 0;
		this.sprites[0].mv2 = 1;
		this.sprites[0].mv3 = 2;
		this.sprites[0].mv4 = 3;
		this.SetupBoneWeights(this.sprites[0]);
	}

	protected int EnlargeArrays(int count)
	{
		int num;
		if (this.sprites == null)
		{
			this.InitArrays();
			num = 0;
			count--;
		}
		else
		{
			num = this.sprites.Length;
		}
		SpriteMesh_Managed[] array = this.sprites;
		this.sprites = new SpriteMesh_Managed[this.sprites.Length + count];
		array.CopyTo(this.sprites, 0);
		Transform[] array2 = this.bones;
		this.bones = new Transform[this.bones.Length + count];
		array2.CopyTo(this.bones, 0);
		Matrix4x4[] array3 = this.bindPoses;
		this.bindPoses = new Matrix4x4[this.bindPoses.Length + count];
		array3.CopyTo(this.bindPoses, 0);
		Vector3[] array4 = this.vertices;
		this.vertices = new Vector3[this.vertices.Length + count * 4];
		array4.CopyTo(this.vertices, 0);
		BoneWeight[] array5 = this.boneWeights;
		this.boneWeights = new BoneWeight[this.boneWeights.Length + count * 4];
		array5.CopyTo(this.boneWeights, 0);
		Vector2[] uVs = this.UVs;
		this.UVs = new Vector2[this.UVs.Length + count * 4];
		uVs.CopyTo(this.UVs, 0);
		Color[] array6 = this.colors;
		this.colors = new Color[this.colors.Length + count * 4];
		array6.CopyTo(this.colors, 0);
		int[] array7 = this.triIndices;
		this.triIndices = new int[this.triIndices.Length + count * 6];
		array7.CopyTo(this.triIndices, 0);
		for (int i = 0; i < num; i++)
		{
			this.sprites[i].SetBuffers(this.vertices, this.UVs, this.colors);
		}
		for (int j = num; j < this.sprites.Length; j++)
		{
			this.sprites[j] = new SpriteMesh_Managed();
			this.sprites[j].index = j;
			this.sprites[j].manager = this;
			this.sprites[j].SetBuffers(this.vertices, this.UVs, this.colors);
			this.sprites[j].mv1 = j * 4;
			this.sprites[j].mv2 = j * 4 + 1;
			this.sprites[j].mv3 = j * 4 + 2;
			this.sprites[j].mv4 = j * 4 + 3;
			this.sprites[j].uv1 = j * 4;
			this.sprites[j].uv2 = j * 4 + 1;
			this.sprites[j].uv3 = j * 4 + 2;
			this.sprites[j].uv4 = j * 4 + 3;
			this.sprites[j].cv1 = j * 4;
			this.sprites[j].cv2 = j * 4 + 1;
			this.sprites[j].cv3 = j * 4 + 2;
			this.sprites[j].cv4 = j * 4 + 3;
			this.availableBlocks.Add(this.sprites[j]);
			this.triIndices[j * 6] = j * 4;
			this.triIndices[j * 6 + 1] = j * 4 + 3;
			this.triIndices[j * 6 + 2] = j * 4 + 1;
			this.triIndices[j * 6 + 3] = j * 4 + 3;
			this.triIndices[j * 6 + 4] = j * 4 + 2;
			this.triIndices[j * 6 + 5] = j * 4 + 1;
			this.spriteDrawOrder.Add(this.sprites[j]);
			this.bones[j] = base.transform;
			this.bindPoses[j] = this.bones[j].worldToLocalMatrix * base.transform.localToWorldMatrix;
			this.SetupBoneWeights(this.sprites[j]);
		}
		this.vertsChanged = true;
		this.uvsChanged = true;
		this.colorsChanged = true;
		this.vertCountChanged = true;
		return num;
	}

	public bool AlreadyAdded(SpriteRoot sprite)
	{
		if (this.activeBlocks.Rewind())
		{
			while (!(this.activeBlocks.Current.sprite == sprite))
			{
				if (!this.activeBlocks.MoveNext())
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public SpriteMesh_Managed AddSprite(GameObject go)
	{
		SpriteRoot spriteRoot = (SpriteRoot)go.GetComponent(typeof(SpriteRoot));
		if (spriteRoot == null)
		{
			return null;
		}
		return this.AddSprite(spriteRoot);
	}

	public SpriteMesh_Managed AddSprite(SpriteRoot sprite)
	{
		if (sprite.manager == this && sprite.AddedToManager)
		{
			return (SpriteMesh_Managed)sprite.spriteMesh;
		}
		if (!this.initialized)
		{
			if (this.spriteAddQueue == null)
			{
				this.spriteAddQueue = new List<SpriteRoot>();
			}
			this.spriteAddQueue.Add(sprite);
			return null;
		}
		if (this.availableBlocks.Empty)
		{
			this.EnlargeArrays(this.allocBlockSize);
		}
		int index = this.availableBlocks.Head.index;
		this.availableBlocks.Remove(this.availableBlocks.Head);
		SpriteMesh_Managed spriteMesh_Managed = this.sprites[index];
		sprite.spriteMesh = spriteMesh_Managed;
		sprite.manager = this;
		sprite.AddedToManager = true;
		spriteMesh_Managed.drawLayer = sprite.drawLayer;
		this.bones[index] = sprite.gameObject.transform;
		this.bindPoses[index] = this.bones[index].worldToLocalMatrix * sprite.transform.localToWorldMatrix;
		this.activeBlocks.Add(spriteMesh_Managed);
		spriteMesh_Managed.Init();
		this.SortDrawingOrder();
		this.vertCountChanged = true;
		this.vertsChanged = true;
		this.uvsChanged = true;
		return spriteMesh_Managed;
	}

	public SpriteRoot CreateSprite(GameObject prefab)
	{
		return this.CreateSprite(prefab, Vector3.zero, Quaternion.identity);
	}

	public SpriteRoot CreateSprite(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		GameObject go = (GameObject)UnityEngine.Object.Instantiate(prefab, position, rotation);
		SpriteMesh_Managed spriteMesh_Managed = this.AddSprite(go);
		if (spriteMesh_Managed == null)
		{
			return null;
		}
		return spriteMesh_Managed.sprite;
	}

	public void RemoveSprite(SpriteRoot sprite)
	{
		if (sprite.spriteMesh is SpriteMesh_Managed && sprite.spriteMesh != null)
		{
			if (sprite.manager == this)
			{
				sprite.manager = null;
				sprite.AddedToManager = false;
			}
			this.RemoveSprite((SpriteMesh_Managed)sprite.spriteMesh);
		}
	}

	public void RemoveSprite(SpriteMesh_Managed sprite)
	{
		this.vertices[sprite.mv1] = Vector3.zero;
		this.vertices[sprite.mv2] = Vector3.zero;
		this.vertices[sprite.mv3] = Vector3.zero;
		this.vertices[sprite.mv4] = Vector3.zero;
		this.activeBlocks.Remove(sprite);
		if (base.gameObject != null)
		{
			this.bones[sprite.index] = base.transform;
		}
		sprite.Clear();
		sprite.sprite.spriteMesh = null;
		sprite.sprite = null;
		this.availableBlocks.Add(sprite);
		this.vertsChanged = true;
	}

	public void MoveToFront(SpriteMesh_Managed s)
	{
		int[] array = new int[6];
		int num = this.spriteDrawOrder.IndexOf(s) * 6;
		if (num < 0)
		{
			return;
		}
		array[0] = this.triIndices[num];
		array[1] = this.triIndices[num + 1];
		array[2] = this.triIndices[num + 2];
		array[3] = this.triIndices[num + 3];
		array[4] = this.triIndices[num + 4];
		array[5] = this.triIndices[num + 5];
		for (int i = num; i < this.triIndices.Length - 6; i += 6)
		{
			this.triIndices[i] = this.triIndices[i + 6];
			this.triIndices[i + 1] = this.triIndices[i + 7];
			this.triIndices[i + 2] = this.triIndices[i + 8];
			this.triIndices[i + 3] = this.triIndices[i + 9];
			this.triIndices[i + 4] = this.triIndices[i + 10];
			this.triIndices[i + 5] = this.triIndices[i + 11];
			this.spriteDrawOrder[i / 6] = this.spriteDrawOrder[i / 6 + 1];
		}
		this.triIndices[this.triIndices.Length - 6] = array[0];
		this.triIndices[this.triIndices.Length - 5] = array[1];
		this.triIndices[this.triIndices.Length - 4] = array[2];
		this.triIndices[this.triIndices.Length - 3] = array[3];
		this.triIndices[this.triIndices.Length - 2] = array[4];
		this.triIndices[this.triIndices.Length - 1] = array[5];
		this.spriteDrawOrder[this.spriteDrawOrder.Count - 1] = s;
		this.vertCountChanged = true;
	}

	public void MoveToBack(SpriteMesh_Managed s)
	{
		int[] array = new int[6];
		int num = this.spriteDrawOrder.IndexOf(s) * 6;
		if (num < 0)
		{
			return;
		}
		array[0] = this.triIndices[num];
		array[1] = this.triIndices[num + 1];
		array[2] = this.triIndices[num + 2];
		array[3] = this.triIndices[num + 3];
		array[4] = this.triIndices[num + 4];
		array[5] = this.triIndices[num + 5];
		for (int i = num; i > 5; i -= 6)
		{
			this.triIndices[i] = this.triIndices[i - 6];
			this.triIndices[i + 1] = this.triIndices[i - 5];
			this.triIndices[i + 2] = this.triIndices[i - 4];
			this.triIndices[i + 3] = this.triIndices[i - 3];
			this.triIndices[i + 4] = this.triIndices[i - 2];
			this.triIndices[i + 5] = this.triIndices[i - 1];
			this.spriteDrawOrder[i / 6] = this.spriteDrawOrder[i / 6 - 1];
		}
		this.triIndices[0] = array[0];
		this.triIndices[1] = array[1];
		this.triIndices[2] = array[2];
		this.triIndices[3] = array[3];
		this.triIndices[4] = array[4];
		this.triIndices[5] = array[5];
		this.spriteDrawOrder[0] = s;
		this.vertCountChanged = true;
	}

	public void MoveInfrontOf(SpriteMesh_Managed toMove, SpriteMesh_Managed reference)
	{
		int[] array = new int[6];
		int num = this.spriteDrawOrder.IndexOf(toMove) * 6;
		int num2 = this.spriteDrawOrder.IndexOf(reference) * 6;
		if (num < 0)
		{
			return;
		}
		if (num > num2)
		{
			return;
		}
		array[0] = this.triIndices[num];
		array[1] = this.triIndices[num + 1];
		array[2] = this.triIndices[num + 2];
		array[3] = this.triIndices[num + 3];
		array[4] = this.triIndices[num + 4];
		array[5] = this.triIndices[num + 5];
		for (int i = num; i < num2; i += 6)
		{
			this.triIndices[i] = this.triIndices[i + 6];
			this.triIndices[i + 1] = this.triIndices[i + 7];
			this.triIndices[i + 2] = this.triIndices[i + 8];
			this.triIndices[i + 3] = this.triIndices[i + 9];
			this.triIndices[i + 4] = this.triIndices[i + 10];
			this.triIndices[i + 5] = this.triIndices[i + 11];
			this.spriteDrawOrder[i / 6] = this.spriteDrawOrder[i / 6 + 1];
		}
		this.triIndices[num2] = array[0];
		this.triIndices[num2 + 1] = array[1];
		this.triIndices[num2 + 2] = array[2];
		this.triIndices[num2 + 3] = array[3];
		this.triIndices[num2 + 4] = array[4];
		this.triIndices[num2 + 5] = array[5];
		this.spriteDrawOrder[num2 / 6] = toMove;
		this.vertCountChanged = true;
	}

	public void MoveBehind(SpriteMesh_Managed toMove, SpriteMesh_Managed reference)
	{
		int[] array = new int[6];
		int num = this.spriteDrawOrder.IndexOf(toMove) * 6;
		int num2 = this.spriteDrawOrder.IndexOf(reference) * 6;
		if (num < 0)
		{
			return;
		}
		if (num < num2)
		{
			return;
		}
		array[0] = this.triIndices[num];
		array[1] = this.triIndices[num + 1];
		array[2] = this.triIndices[num + 2];
		array[3] = this.triIndices[num + 3];
		array[4] = this.triIndices[num + 4];
		array[5] = this.triIndices[num + 5];
		for (int i = num; i > num2; i -= 6)
		{
			this.triIndices[i] = this.triIndices[i - 6];
			this.triIndices[i + 1] = this.triIndices[i - 5];
			this.triIndices[i + 2] = this.triIndices[i - 4];
			this.triIndices[i + 3] = this.triIndices[i - 3];
			this.triIndices[i + 4] = this.triIndices[i - 2];
			this.triIndices[i + 5] = this.triIndices[i - 1];
			this.spriteDrawOrder[i / 6] = this.spriteDrawOrder[i / 6 - 1];
		}
		this.triIndices[num2] = array[0];
		this.triIndices[num2 + 1] = array[1];
		this.triIndices[num2 + 2] = array[2];
		this.triIndices[num2 + 3] = array[3];
		this.triIndices[num2 + 4] = array[4];
		this.triIndices[num2 + 5] = array[5];
		this.spriteDrawOrder[num2 / 6] = toMove;
		this.vertCountChanged = true;
	}

	public void SortDrawingOrder()
	{
		this.spriteDrawOrder.Sort(this.drawOrderComparer);
		if (this.winding == SpriteRoot.WINDING_ORDER.CCW)
		{
			for (int i = 0; i < this.spriteDrawOrder.Count; i++)
			{
				SpriteMesh_Managed spriteMesh_Managed = this.spriteDrawOrder[i];
				this.triIndices[i * 6] = spriteMesh_Managed.mv1;
				this.triIndices[i * 6 + 1] = spriteMesh_Managed.mv2;
				this.triIndices[i * 6 + 2] = spriteMesh_Managed.mv4;
				this.triIndices[i * 6 + 3] = spriteMesh_Managed.mv4;
				this.triIndices[i * 6 + 4] = spriteMesh_Managed.mv2;
				this.triIndices[i * 6 + 5] = spriteMesh_Managed.mv3;
			}
		}
		else
		{
			for (int j = 0; j < this.spriteDrawOrder.Count; j++)
			{
				SpriteMesh_Managed spriteMesh_Managed = this.spriteDrawOrder[j];
				this.triIndices[j * 6] = spriteMesh_Managed.mv1;
				this.triIndices[j * 6 + 1] = spriteMesh_Managed.mv4;
				this.triIndices[j * 6 + 2] = spriteMesh_Managed.mv2;
				this.triIndices[j * 6 + 3] = spriteMesh_Managed.mv4;
				this.triIndices[j * 6 + 4] = spriteMesh_Managed.mv3;
				this.triIndices[j * 6 + 5] = spriteMesh_Managed.mv2;
			}
		}
		this.vertCountChanged = true;
	}

	public SpriteMesh_Managed GetSprite(int i)
	{
		if (i < this.sprites.Length)
		{
			return this.sprites[i];
		}
		return null;
	}

	public void UpdatePositions()
	{
		this.vertsChanged = true;
	}

	public void UpdateUVs()
	{
		this.uvsChanged = true;
	}

	public void UpdateColors()
	{
		this.colorsChanged = true;
	}

	public void UpdateBounds()
	{
		this.updateBounds = true;
	}

	public void ScheduleBoundsUpdate(float seconds)
	{
		this.boundUpdateInterval = seconds;
		base.InvokeRepeating("UpdateBounds", seconds, seconds);
	}

	public void CancelBoundsUpdate()
	{
		base.CancelInvoke("UpdateBounds");
	}

	public virtual void LateUpdate()
	{
		if (this.vertCountChanged)
		{
			this.vertCountChanged = false;
			this.colorsChanged = false;
			this.vertsChanged = false;
			this.uvsChanged = false;
			this.updateBounds = false;
			this.meshRenderer.bones = this.bones;
			this.mesh.Clear();
			this.mesh.vertices = this.vertices;
			this.mesh.bindposes = this.bindPoses;
			this.mesh.boneWeights = this.boneWeights;
			this.mesh.uv = this.UVs;
			this.mesh.colors = this.colors;
			this.mesh.triangles = this.triIndices;
			this.mesh.RecalculateNormals();
			if (this.autoUpdateBounds)
			{
				this.mesh.RecalculateBounds();
			}
		}
		else
		{
			if (this.vertsChanged)
			{
				this.vertsChanged = false;
				if (this.autoUpdateBounds)
				{
					this.updateBounds = true;
				}
				this.mesh.vertices = this.vertices;
			}
			if (this.updateBounds)
			{
				this.mesh.RecalculateBounds();
				this.updateBounds = false;
			}
			if (this.colorsChanged)
			{
				this.colorsChanged = false;
				this.mesh.colors = this.colors;
			}
			if (this.uvsChanged)
			{
				this.uvsChanged = false;
				this.mesh.uv = this.UVs;
			}
		}
	}

	public virtual void DoMirror()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.vertCountChanged)
		{
			this.vertCountChanged = false;
			this.colorsChanged = false;
			this.vertsChanged = false;
			this.uvsChanged = false;
			this.updateBounds = false;
			this.meshRenderer.bones = this.bones;
			this.mesh.Clear();
			this.mesh.vertices = this.vertices;
			this.mesh.bindposes = this.bindPoses;
			this.mesh.boneWeights = this.boneWeights;
			this.mesh.uv = this.UVs;
			this.mesh.colors = this.colors;
			this.mesh.triangles = this.triIndices;
		}
		else
		{
			if (this.vertsChanged)
			{
				this.vertsChanged = false;
				this.updateBounds = true;
				this.mesh.vertices = this.vertices;
			}
			if (this.updateBounds)
			{
				this.mesh.RecalculateBounds();
				this.updateBounds = false;
			}
			if (this.colorsChanged)
			{
				this.colorsChanged = false;
				this.mesh.colors = this.colors;
			}
			if (this.uvsChanged)
			{
				this.uvsChanged = false;
				this.mesh.uv = this.UVs;
			}
		}
		if (this.drawBoundingBox)
		{
			Gizmos.color = Color.yellow;
			this.DrawCenter();
			Gizmos.DrawWireCube(this.meshRenderer.bounds.center, this.meshRenderer.bounds.size);
		}
	}

	public virtual void OnDrawGizmos()
	{
		if (this.drawBoundingBox)
		{
			Gizmos.color = Color.yellow;
			this.DrawCenter();
			Gizmos.DrawWireCube(this.meshRenderer.bounds.center, this.meshRenderer.bounds.size);
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		this.DrawCenter();
		Gizmos.DrawWireCube(this.meshRenderer.bounds.center, this.meshRenderer.bounds.size);
	}

	protected void DrawCenter()
	{
		float num = Mathf.Max(this.meshRenderer.bounds.size.x, this.meshRenderer.bounds.size.y);
		num = Mathf.Max(num, this.meshRenderer.bounds.size.z);
		float d = num * 0.015f;
		Gizmos.DrawLine(this.meshRenderer.bounds.center - Vector3.up * d, this.meshRenderer.bounds.center + Vector3.up * d);
		Gizmos.DrawLine(this.meshRenderer.bounds.center - Vector3.right * d, this.meshRenderer.bounds.center + Vector3.right * d);
		Gizmos.DrawLine(this.meshRenderer.bounds.center - Vector3.forward * d, this.meshRenderer.bounds.center + Vector3.forward * d);
	}
}
