using System;
using UnityEngine;

public class SoldierBatchGridCell : MonoBehaviour
{
	public const float CELL_SIZE = 2f;

	public const float MESH_SIZE = 8f;

	public float PlaneSizeX = 0.25f;

	public float PlaneSizeZ = 0.25f;

	public float POSITION_SPACING = 0.15f;

	public float VERTEX_SPACING = 0.1f;

	public Vector3 mCenterPos = Vector3.zero;

	private static TsLayerMask mc_kPickLayer = TsLayer.NOTHING + TsLayer.TERRAIN;

	public string TEST_coll = string.Empty;

	public Vector3 GetCenterPos()
	{
		return this.mCenterPos;
	}

	private void Awake()
	{
		Vector3 localScale = new Vector3(this.PlaneSizeX, 1f, this.PlaneSizeZ);
		base.transform.localScale = localScale;
		Vector3 position = base.transform.position;
		position.y = NrTSingleton<NrTerrain>.Instance.SampleHeight(position) + this.POSITION_SPACING;
		base.transform.position = position;
		this.InitCell();
	}

	private void InitCell()
	{
		if (NrTSingleton<NrTerrain>.Instance.IsActive() && 20f >= NrTSingleton<NrTerrain>.Instance.heightmapPixelError)
		{
			this.POSITION_SPACING = this.VERTEX_SPACING;
		}
		this.mCenterPos = base.transform.position;
		if (this.UpdatePlane())
		{
			this.mCenterPos.y = this.mCenterPos.y - this.POSITION_SPACING;
		}
	}

	private void Update()
	{
	}

	public static bool PickTerrain(Vector3 _RayPos, ref Vector3 _CenterPos)
	{
		GameObject gameObject = GameObject.Find("battle_height");
		if (gameObject != null)
		{
			_CenterPos = _RayPos;
			_CenterPos.y = gameObject.transform.position.y;
			return false;
		}
		Vector3 down = Vector3.down;
		_RayPos.y = 600f;
		if (NkRaycast.Raycast(new Ray(_RayPos, down), 600f, SoldierBatchGridCell.mc_kPickLayer))
		{
			RaycastHit hIT = NkRaycast.HIT;
			_CenterPos = hIT.point;
			if (hIT.collider && typeof(MeshCollider) == hIT.collider.GetType())
			{
				return false;
			}
		}
		return true;
	}

	public bool UpdatePlane()
	{
		if (SoldierBatchGridCell.PickTerrain(base.transform.position, ref this.mCenterPos))
		{
			this.UpdatePlaneByTerrain();
			return true;
		}
		this.UpdatePlaneByRay(null);
		return false;
	}

	public void UpdatePlaneByTerrain()
	{
		NrTerrain instance = NrTSingleton<NrTerrain>.Instance;
		if (instance.IsActive())
		{
			return;
		}
		if (null == Terrain.activeTerrain)
		{
			Debug.Log(":activeTerrain fail:" + (null == Terrain.activeTerrain));
			return;
		}
		if (null == Terrain.activeTerrain.terrainData)
		{
			Debug.Log(":terrainData fail:" + (null == Terrain.activeTerrain.terrainData));
		}
	}

	private void UpdatePlaneByRay(Mesh _DesMesh)
	{
		this.mCenterPos.y = this.mCenterPos.y + this.POSITION_SPACING;
		base.transform.position = this.mCenterPos;
	}

	private bool Raycast(Vector3 _Pos, ref Vector3 _Vertex)
	{
		GameObject gameObject = GameObject.Find("battle_height");
		if (gameObject != null)
		{
			_Vertex = _Pos;
			_Vertex.y = gameObject.transform.position.y;
			return true;
		}
		Vector3 down = Vector3.down;
		float fDistance = 1000f;
		if (NkRaycast.Raycast(new Ray(_Pos, down), fDistance, SoldierBatchGridCell.mc_kPickLayer))
		{
			_Vertex = NkRaycast.POINT;
			return true;
		}
		return false;
	}

	private Vector3 Vertex2World(Vector3 _Vertex)
	{
		return base.transform.TransformPoint(_Vertex);
	}

	private Vector3 World2Vertex(Vector3 _Vertex)
	{
		return base.transform.InverseTransformPoint(_Vertex);
	}

	private Vector3 Vertex2Transform(Transform _WorldTFM, Vector3 _Vertex)
	{
		Vector3 position = _WorldTFM.TransformPoint(_Vertex);
		return base.transform.InverseTransformPoint(position);
	}

	private void OnTriggerEnter(Collider other)
	{
	}
}
