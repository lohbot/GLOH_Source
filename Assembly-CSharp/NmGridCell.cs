using System;
using UnityEngine;

public class NmGridCell : MonoBehaviour
{
	public const float CELL_SIZE = 2f;

	public const float MESH_SIZE = 8f;

	public float PlaneSizeX = 0.25f;

	public float PlaneSizeZ = 0.25f;

	private int m_planeSegments = 2;

	public float POSITION_SPACING = 0.15f;

	public float VERTEX_SPACING = 0.1f;

	private Mesh mMesh;

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
		if (NkRaycast.Raycast(new Ray(_RayPos, down), 600f, NmGridCell.mc_kPickLayer))
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
		if (NmGridCell.PickTerrain(base.transform.position, ref this.mCenterPos))
		{
			this.UpdatePlaneByTerrain();
			return true;
		}
		this.UpdatePlaneByRay(this.mMesh);
		return false;
	}

	public void UpdatePlaneByTerrain()
	{
		NrTerrain instance = NrTSingleton<NrTerrain>.Instance;
		if (instance.IsActive())
		{
			if (this.mMesh != null)
			{
				Vector3 pos = new Vector3(base.transform.position.x + this.PlaneSizeX / 2f, base.transform.position.y, base.transform.position.z + this.PlaneSizeZ / 2f);
				float num = this.PlaneSizeX / (float)this.m_planeSegments;
				float num2 = this.PlaneSizeZ / (float)this.m_planeSegments;
				int num3 = this.m_planeSegments + 1;
				Vector3[] vertices = this.mMesh.vertices;
				for (int i = 0; i < num3; i++)
				{
					for (int j = 0; j < num3; j++)
					{
						vertices[i * num3 + j].y = instance.SampleHeight(pos) - pos.y + this.VERTEX_SPACING;
						pos.x -= num;
					}
					pos.x += (float)num3 * num;
					pos.z -= num2;
				}
				this.mMesh.vertices = vertices;
				this.mMesh.RecalculateBounds();
				int num4 = (int)((float)num3 / 2f * (float)num3);
				this.mCenterPos = this.Vertex2World(vertices[num4]);
			}
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
		if (NkRaycast.Raycast(new Ray(_Pos, down), fDistance, NmGridCell.mc_kPickLayer))
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
