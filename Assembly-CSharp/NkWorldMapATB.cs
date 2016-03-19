using System;
using System.Collections;
using TsLibs;
using UnityEngine;

public class NkWorldMapATB
{
	private MAP_TILEINFO m_pkTileInfo;

	private bool m_CompleteLoad;

	public void Init()
	{
		this.m_CompleteLoad = true;
	}

	public bool IsLoadCompleted()
	{
		return this.m_CompleteLoad;
	}

	public bool ParseDataFromNDT(TsDataReader dr)
	{
		IEnumerator enumerator = dr.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				TsDataReader.Row row = (TsDataReader.Row)enumerator.Current;
				if (this.m_pkTileInfo == null)
				{
					this.m_pkTileInfo = new MAP_TILEINFO();
				}
				this.m_pkTileInfo.SetData(row, base.GetType());
				this.Init();
				return true;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return false;
	}

	public short GetSizeX()
	{
		return this.m_pkTileInfo.WIDTH;
	}

	public short GetSizeY()
	{
		return this.m_pkTileInfo.HEIGHT;
	}

	public int GetCellSize()
	{
		return (int)this.m_pkTileInfo.TILESIZE;
	}

	public void ShowCellGrid()
	{
		GameObject gameObject = GameObject.Find("CellGrid");
		if (null == gameObject)
		{
			gameObject = new GameObject("CellGrid");
			int num = 96;
			int num2 = (int)(this.m_pkTileInfo.WIDTH * (short)this.m_pkTileInfo.TILESIZE) / num + 1;
			int num3 = (int)(this.m_pkTileInfo.HEIGHT * (short)this.m_pkTileInfo.TILESIZE) / num + 1;
			int num4 = 0;
			Transform parent = Terrain.activeTerrain.transform.parent;
			if (parent != null)
			{
				GameObject gameObject2 = parent.gameObject;
				if (gameObject2 != null)
				{
					num4 = (int)gameObject2.transform.position.y;
				}
			}
			Vector3 vector = default(Vector3);
			for (int i = 0; i < num2; i++)
			{
				int num5 = 0;
				GameObject gameObject3 = new GameObject("X_" + i.ToString());
				LineRenderer lineRenderer = gameObject3.AddComponent<LineRenderer>();
				gameObject3.transform.parent = gameObject.transform;
				lineRenderer.SetColors(Color.yellow, Color.yellow);
				lineRenderer.SetVertexCount(num2);
				lineRenderer.SetWidth(0.5f, 0.5f);
				int num6 = i * num;
				for (int j = 0; j < num3; j++)
				{
					vector.x = (float)num6;
					vector.y = 0f;
					vector.z = (float)(j * num);
					vector.y = Terrain.activeTerrain.SampleHeight(vector) + (float)num4;
					lineRenderer.SetPosition(num5++, vector);
				}
			}
			for (int k = 0; k < num3; k++)
			{
				int num5 = 0;
				int num6 = k * num;
				GameObject gameObject4 = new GameObject("Y_" + k.ToString());
				LineRenderer lineRenderer2 = gameObject4.AddComponent<LineRenderer>();
				gameObject4.transform.parent = gameObject.transform;
				lineRenderer2.SetColors(Color.yellow, Color.yellow);
				lineRenderer2.SetVertexCount(num3);
				lineRenderer2.SetWidth(0.5f, 0.5f);
				for (int l = 0; l < num3; l++)
				{
					vector.x = (float)(l * num);
					vector.y = 0f;
					vector.z = (float)num6;
					vector.y = Terrain.activeTerrain.SampleHeight(vector) + (float)num4;
					lineRenderer2.SetPosition(num5++, vector);
				}
			}
			return;
		}
		LineRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<LineRenderer>();
		LineRenderer[] array = componentsInChildren;
		for (int m = 0; m < array.Length; m++)
		{
			LineRenderer lineRenderer3 = array[m];
			UnityEngine.Object.Destroy(lineRenderer3.gameObject);
		}
		gameObject.SetActive(false);
		UnityEngine.Object.Destroy(gameObject);
	}
}
