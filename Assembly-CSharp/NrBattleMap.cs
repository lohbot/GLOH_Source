using Ndoors.Framework.Stage;
using System;
using UnityEngine;

public class NrBattleMap
{
	private BATTLE_MAP m_BattleMap;

	private GameObject m_BattleTerrain;

	private bool m_CompleteLoad;

	public void Init(BATTLE_MAP BattleMap)
	{
		this.m_BattleMap = BattleMap;
		if (!Scene.IsCurScene(Scene.Type.BATTLE))
		{
			Debug.LogError("Battle::LoadMapData is called from non-BattleStage!");
		}
		this.m_CompleteLoad = true;
	}

	public bool IsLoadCompleted()
	{
		return this.m_CompleteLoad;
	}

	public void SetTerrainGameObject(GameObject goTerrain)
	{
		this.m_BattleTerrain = goTerrain;
	}

	public float GetBattleMapHeight(Vector3 pos)
	{
		Terrain component = this.m_BattleTerrain.GetComponent<Terrain>();
		float num = 0f;
		if (component != null)
		{
			num = component.SampleHeight(pos) + this.m_BattleTerrain.transform.position.y;
		}
		TsLayerMask layerMask = TsLayer.NOTHING + TsLayer.TERRAIN;
		pos.y += 1000f;
		if (!NkRaycast.Raycast(new Ray(pos, Vector3.down), 1500f, layerMask))
		{
			return num;
		}
		RaycastHit hIT = NkRaycast.HIT;
		if (hIT.point != Vector3.zero && num < hIT.point.y)
		{
			num = hIT.point.y;
		}
		return num;
	}

	public Vector3 GetBattleMapCenterPos()
	{
		float x = (float)this.GetSizeX() / 2f;
		float z = (float)this.GetSizeY() / 2f;
		Vector3 vector = new Vector3(x, 0f, z);
		vector.y = this.GetBattleMapHeight(vector);
		return vector;
	}

	public short GetSizeX()
	{
		return this.m_BattleMap.SIZE_X;
	}

	public short GetSizeY()
	{
		return this.m_BattleMap.SIZE_Y;
	}

	public int GetCellSize()
	{
		return (int)this.m_BattleMap.CELL_SIZE;
	}
}
