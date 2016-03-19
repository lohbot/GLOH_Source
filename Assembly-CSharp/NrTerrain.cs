using System;
using UnityEngine;

public class NrTerrain : NrTSingleton<NrTerrain>
{
	private Vector3 vec3 = new Vector3(0f, 0f, 0f);

	private string str = string.Empty;

	private bool pTerrain
	{
		get
		{
			return true;
		}
	}

	private bool pTerrainData
	{
		get
		{
			return true;
		}
	}

	public int alphamapResolution
	{
		get
		{
			return 0;
		}
	}

	public int heightmapResolution
	{
		get
		{
			return 0;
		}
	}

	public float heightmapHeight
	{
		get
		{
			return 0f;
		}
	}

	public float heightmapWidth
	{
		get
		{
			return 0f;
		}
	}

	public float detailHeight
	{
		get
		{
			return 0f;
		}
	}

	public float detailWidth
	{
		get
		{
			return 0f;
		}
	}

	public float heightmapPixelError
	{
		get
		{
			return 0f;
		}
	}

	private NrTerrain()
	{
	}

	public bool InitTerrainInfo()
	{
		return true;
	}

	public bool IsActive()
	{
		return this.pTerrain;
	}

	public Vector3 GetPosition()
	{
		return this.vec3;
	}

	public Vector3 GetSize()
	{
		return this.vec3;
	}

	public string GetName()
	{
		return this.str;
	}

	public Vector3 GetCenter()
	{
		return this.vec3;
	}

	public Vector3 GetWorldHeight(Vector3 Pos)
	{
		TsLayerMask layerMask = TsLayer.NOTHING + TsLayer.TERRAIN;
		Pos.y += 1000f;
		RaycastHit raycastHit;
		if (!Physics.Raycast(Pos, Vector3.down, out raycastHit, 1500f, layerMask))
		{
			return Vector3.zero;
		}
		if (raycastHit.point != Vector3.zero)
		{
			Debug.Log(string.Concat(new object[]
			{
				"HitPoint : ",
				raycastHit.point,
				" ",
				Pos
			}));
		}
		Vector3 result = new Vector3(Pos.x, raycastHit.point.y, Pos.z);
		return result;
	}

	public void Init()
	{
		if (!this.IsActive())
		{
		}
	}

	public void SetTerrainLayer(int Layer)
	{
		GameObject gameObject = GameObject.Find("Terrain");
		if (null != gameObject)
		{
			gameObject.layer = Layer;
		}
	}

	public float SampleHeight(Vector3 Pos)
	{
		Vector3 zero = Vector3.zero;
		Pos.y = 200f;
		Ray ray = new Ray(Pos, new Vector3(0f, -1f, 0f));
		RaycastHit raycastHit;
		Physics.Raycast(ray, out raycastHit);
		if (null == raycastHit.transform)
		{
			return 0f;
		}
		if (raycastHit.transform.position != Vector3.zero)
		{
			Pos.y = raycastHit.transform.collider.ClosestPointOnBounds(Pos).y;
		}
		return Pos.y;
	}

	public void SetAlphamaps(int x, int y, float[,,] map)
	{
	}

	public float[,,] GetAlphamaps(int x, int y, int width, int height)
	{
		return null;
	}

	public float[,] GetHeights(int xBase, int yBase, int width, int height)
	{
		return null;
	}

	public float BillboardDistance()
	{
		return 0f;
	}

	public float TreeDistance()
	{
		return 0f;
	}

	public void SetBillboardDistance(float fValue)
	{
	}

	public void SetTreeDistance(float fValue)
	{
	}
}
