using Ndoors.Framework.Stage;
using System;
using System.Collections;
using UnityEngine;
using UnityForms;

public class MapManager : NrTSingleton<MapManager>
{
	public int nCurrentMapIndex;

	private NkWorldMapATB kWorldMapATB;

	private int[] m_nAutoMoveDestMapIndex = new int[5];

	public int CurrentMapIndex
	{
		get
		{
			return this.nCurrentMapIndex;
		}
	}

	private MapManager()
	{
		this.kWorldMapATB = new NkWorldMapATB();
		this.m_nAutoMoveDestMapIndex[0] = 2;
		this.m_nAutoMoveDestMapIndex[1] = 62;
		this.m_nAutoMoveDestMapIndex[2] = 63;
		this.m_nAutoMoveDestMapIndex[3] = 64;
		this.m_nAutoMoveDestMapIndex[4] = 65;
	}

	public int GetMapIndexFromUnique(int nMapUnique)
	{
		MAP_UNIT mapUnit = NrTSingleton<NrBaseTableManager>.Instance.GetMapUnit(nMapUnique.ToString());
		if (mapUnit == null)
		{
			return 0;
		}
		return mapUnit.MAP_IDX;
	}

	public string GetMapNameFromUnique(int nMapUnique)
	{
		int mapIndexFromUnique = this.GetMapIndexFromUnique(nMapUnique);
		return this.GetMapName(mapIndexFromUnique);
	}

	public bool LoadCurrentMapResource(int nMapIndex, AStage kStage)
	{
		if (nMapIndex == this.nCurrentMapIndex)
		{
			return false;
		}
		if (NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(nMapIndex.ToString()) == null)
		{
			return false;
		}
		this.nCurrentMapIndex = nMapIndex;
		return true;
	}

	public string GetGateToolTip(int nGateIdx)
	{
		GATE_INFO gateInfo = NrTSingleton<NrBaseTableManager>.Instance.GetGateInfo(nGateIdx.ToString());
		if (gateInfo == null)
		{
			return string.Empty;
		}
		MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(gateInfo.DST_MAP_IDX.ToString());
		if (mapInfo == null)
		{
			return string.Empty;
		}
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromMap(mapInfo.TEXTKEY);
	}

	public string GetMapName()
	{
		return this.GetMapName(this.CurrentMapIndex);
	}

	public string GetMapName(int nMapIdx)
	{
		MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(nMapIdx.ToString());
		if (mapInfo == null)
		{
			return string.Empty;
		}
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromMap(mapInfo.TEXTKEY);
	}

	public string GetMapNameAndOST()
	{
		MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(this.CurrentMapIndex.ToString());
		if (mapInfo == null)
		{
			return string.Empty;
		}
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromMap(mapInfo.TEXTKEY) + " " + NrTSingleton<NrTextMgr>.Instance.GetTextFromOST(mapInfo.OST_NAME);
	}

	public bool IsMapATB(int nMapIdx, long nFlag)
	{
		MAP_INFO mapInfo = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo(nMapIdx.ToString());
		return mapInfo != null && mapInfo.IsMapATB(nFlag);
	}

	public bool IsCurrentMapATB(long nFlag)
	{
		return this.IsMapATB(this.nCurrentMapIndex, nFlag);
	}

	public NkWorldMapATB GetWorldMapATB()
	{
		return this.kWorldMapATB;
	}

	public void ShowDestPosition(bool bShow)
	{
		GameObject gameObject = GameObject.Find("ShowDestPosition");
		if (null == gameObject)
		{
			if (!bShow)
			{
				return;
			}
			gameObject = new GameObject("ShowDestPosition");
			ICollection gateInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetGateInfo_Col();
			foreach (GATE_INFO gATE_INFO in gateInfo_Col)
			{
				if (this.CurrentMapIndex == gATE_INFO.DST_MAP_IDX)
				{
					GameObject gameObject2 = (GameObject)CResources.LoadClone(NrTSingleton<UIDataManager>.Instance.FilePath + "Common/Prefabs/ToolArrow");
					if (!(null == gameObject2))
					{
						GameObject gameObject3 = new GameObject(string.Format("DestPos_{0}", gATE_INFO.GATE_IDX));
						gameObject2.transform.parent = gameObject3.transform;
						gameObject2.transform.localRotation = Quaternion.Euler(90f, 90f, 0f);
						gameObject3.transform.parent = gameObject.transform;
						Vector3 localPosition = gameObject3.transform.localPosition;
						localPosition.x = gATE_INFO.DST_POSX;
						localPosition.y = gATE_INFO.DST_POSY;
						localPosition.z = gATE_INFO.DST_POSZ;
						gameObject3.transform.localPosition = localPosition;
						gameObject3.transform.localRotation = Quaternion.Euler(0f, gATE_INFO.DST_ANGLE, 0f);
					}
				}
			}
		}
		gameObject.SetActive(bShow);
	}

	public void ClearExtraMapInfo()
	{
	}

	public int GetAutoMoveDestMapIndex(int mapindex)
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < 5; i++)
		{
			if (this.m_nAutoMoveDestMapIndex[i] == mapindex)
			{
				flag = true;
			}
			if (this.m_nAutoMoveDestMapIndex[i] == this.nCurrentMapIndex)
			{
				flag2 = true;
			}
		}
		if (flag && flag2)
		{
			return this.nCurrentMapIndex;
		}
		return mapindex;
	}
}
