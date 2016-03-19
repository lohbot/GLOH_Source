using System;
using System.Collections.Generic;
using System.IO;
using TsLibs;
using UnityEngine;

public class GxRoadPointManager : NrTSingleton<GxRoadPointManager>
{
	private enum TABLE
	{
		DEFAULT_VALIDRADIUS_MAX = 20,
		DEFAULT_VALIDRADIUS_MIN = 0,
		RPTABLE_LIMIT = 20000,
		MAX_RODAPOINT_COUNT = 12000
	}

	private enum ERROR : short
	{
		ADDERR_ALREADY = -1,
		ADDERR_FULLTABLE = -2,
		ADDERR_INVALIDPARAM = -3
	}

	public const int MAX_LINK = 4;

	private Dictionary<short, List<GxRP>> m_RPList = new Dictionary<short, List<GxRP>>();

	private List<short> m_ValidList = new List<short>();

	private LinkedList<short> m_FinalList = new LinkedList<short>();

	private short m_iValidRadiusMax = 100;

	private short m_iValidRadiusMin = 5;

	private int m_nCurrentMapIdx;

	public int CurrentMapIdx
	{
		get
		{
			return this.m_nCurrentMapIdx;
		}
		set
		{
			this.m_nCurrentMapIdx = value;
		}
	}

	private GxRoadPointManager()
	{
	}

	public bool InitSystem(int iRPTableSize, short iValidRadiusMax, short iValidRadiusMin)
	{
		if (iRPTableSize >= 20000)
		{
			return false;
		}
		this.m_iValidRadiusMax = iValidRadiusMax;
		this.m_iValidRadiusMin = iValidRadiusMin;
		return true;
	}

	public short AddRP(short sMapIndex, Vector3 v3Pos)
	{
		if (sMapIndex < 0 || v3Pos.x < 0f || v3Pos.y < 0f || v3Pos.z < 0f)
		{
			return -3;
		}
		if (!this.m_RPList.ContainsKey(sMapIndex))
		{
			this.m_RPList.Add(sMapIndex, new List<GxRP>());
		}
		if (this.FindRP(sMapIndex, v3Pos))
		{
			return -1;
		}
		List<GxRP> list = this.m_RPList[sMapIndex];
		if (list.Count == 0)
		{
			GxRP gxRP = new GxRP();
			gxRP.SetData(0, sMapIndex, Vector3.zero);
			list.Add(gxRP);
		}
		GxRP gxRP2 = new GxRP();
		short emptyRPidx = this.GetEmptyRPidx(sMapIndex);
		short sRPIndex = (short)list.Count;
		if (emptyRPidx == 0)
		{
			gxRP2.SetData(sRPIndex, sMapIndex, v3Pos);
			list.Add(gxRP2);
		}
		else
		{
			gxRP2.SetData(emptyRPidx, sMapIndex, v3Pos);
			list[(int)emptyRPidx] = gxRP2;
			sRPIndex = emptyRPidx;
		}
		if (!this.AutoLinkRPs(sMapIndex, sRPIndex))
		{
			Debug.Log("AutoLinkRPs Faild\n");
		}
		if (emptyRPidx != 0)
		{
			return emptyRPidx;
		}
		return (short)(list.Count - 1);
	}

	private short GetEmptyRPidx(short sMapIdx)
	{
		short num = 1;
		while ((int)num < this.m_RPList[sMapIdx].Count)
		{
			if (this.m_RPList[sMapIdx][(int)num].GetPos() == Vector3.zero)
			{
				return num;
			}
			num += 1;
		}
		return 0;
	}

	private bool FindRP(short _sMapIndex, Vector3 _Pos)
	{
		List<GxRP> list = this.GetList(_sMapIndex);
		if (list == null)
		{
			return false;
		}
		foreach (GxRP current in list)
		{
			if ((float)current.GetX() == _Pos.x && (float)current.GetY() == _Pos.z)
			{
				return true;
			}
		}
		return false;
	}

	private bool AutoLinkRPs(short MapIndex, short sRPIndex)
	{
		if (!this.UnLinkAll(MapIndex, sRPIndex))
		{
			return false;
		}
		GxRP gxRP = this.m_RPList[MapIndex][(int)sRPIndex];
		int num = 0;
		this.m_ValidList.Clear();
		this.m_FinalList.Clear();
		this.GetValidRangeRPIndexes(gxRP.GetMapIndex(), gxRP.GetX(), gxRP.GetY(), false);
		if (this.m_ValidList.Count <= 4)
		{
			for (int i = 0; i < this.m_ValidList.Count; i++)
			{
				this.LinkRP(MapIndex, sRPIndex, this.m_ValidList[i]);
			}
			return true;
		}
		for (int j = 0; j < this.m_ValidList.Count; j++)
		{
			if (sRPIndex != this.m_ValidList[j])
			{
				GxRP rP = this.GetRP(MapIndex, (int)this.m_ValidList[j]);
				if (rP.GetLinkedCount() > 1)
				{
					short num2 = this.m_ValidList[j];
					int num3 = rP.CalcCost(rP.GetX(), rP.GetY());
					for (int k = 0; k < 4; k++)
					{
						if (rP.GetLinkedRP(k) != 0)
						{
							if (this.m_ValidList.Contains(rP.GetLinkedRP(k)))
							{
								GxRP rP2 = this.GetRP(MapIndex, (int)rP.GetLinkedRP(k));
								int num4 = rP2.CalcCost(gxRP.GetX(), gxRP.GetY());
								if (rP2 != null && num4 <= num3)
								{
									num2 = rP.GetLinkedRP(j);
									num3 = num4;
								}
							}
						}
					}
					if (!this.m_FinalList.Contains(num2))
					{
						this.m_FinalList.AddFirst(num2);
						num++;
					}
					if (num2 != this.m_ValidList[j] && !this.m_FinalList.Contains(this.m_ValidList[j]))
					{
						this.m_FinalList.AddLast(this.m_ValidList[j]);
					}
				}
				else
				{
					this.m_FinalList.AddFirst(this.m_ValidList[j]);
					num++;
				}
			}
		}
		if (num > 4)
		{
			Debug.Log("MAX_LINK Over :" + sRPIndex.ToString());
			return false;
		}
		int num5 = 0;
		foreach (short current in this.m_FinalList)
		{
			if (num5 == 4)
			{
				break;
			}
			this.LinkRP(MapIndex, sRPIndex, current);
			num5++;
		}
		return true;
	}

	private bool UnLinkAll(short MapIndex, short sRPIndex)
	{
		try
		{
			GxRP gxRP = this.m_RPList[MapIndex][(int)sRPIndex];
			if (gxRP == null)
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				this.UnLinkRP(MapIndex, sRPIndex, gxRP.GetLinkedRP(i));
			}
		}
		catch (Exception arg)
		{
			Debug.Log(arg + "sRPIndex :" + sRPIndex.ToString());
		}
		return true;
	}

	public bool LinkRP(short MapIndex, short sRPIdx1, short sRPIdx2)
	{
		GxRP rP = this.GetRP(MapIndex, (int)sRPIdx1);
		if (rP == null || rP.GetLinkedCount() <= 4)
		{
			return false;
		}
		GxRP rP2 = this.GetRP(MapIndex, (int)sRPIdx2);
		if (rP2 == null || rP2.GetLinkedCount() <= 4)
		{
			return false;
		}
		rP.AddLinkedRP(sRPIdx2);
		rP2.AddLinkedRP(sRPIdx1);
		return true;
	}

	public bool UnLinkRP(short MapIndex, short sRPIdx1, short sRPIdx2)
	{
		GxRP rP = this.GetRP(MapIndex, (int)sRPIdx1);
		if (rP == null)
		{
			return false;
		}
		GxRP rP2 = this.GetRP(MapIndex, (int)sRPIdx2);
		if (rP2 == null)
		{
			return false;
		}
		rP.DelLinkedRP(sRPIdx2);
		rP2.DelLinkedRP(sRPIdx1);
		return true;
	}

	private int GetValidRangeRPIndexes(short sMapIdx, short X, short Y, bool bMinRadius)
	{
		List<GxRP> list = this.GetList(sMapIdx);
		if (list == null)
		{
			return 0;
		}
		int num = 0;
		short num2 = 0;
		while ((int)num2 < list.Count)
		{
			int num3 = list[(int)num2].CalcDistance(X, Y);
			if (num3 <= (int)this.m_iValidRadiusMax && num3 >= (int)this.m_iValidRadiusMin)
			{
				this.m_ValidList.Add(num2);
				num++;
			}
			num2 += 1;
		}
		return num;
	}

	public int GetValidRangeRPIndexes(short sMapIdx, short X, short Y, ref List<short> _rkRPIndexList)
	{
		List<GxRP> list = this.GetList(sMapIdx);
		if (list == null)
		{
			return 0;
		}
		int num = 0;
		short num2 = 0;
		while ((int)num2 < list.Count)
		{
			int num3 = list[(int)num2].CalcDistance(X, Y);
			if (num3 <= (int)this.m_iValidRadiusMax && num3 >= (int)this.m_iValidRadiusMin)
			{
				_rkRPIndexList.Add(num2);
				num++;
			}
			num2 += 1;
		}
		return num;
	}

	public GxRP GetRP(short _MapIdx, int RPIdx)
	{
		if (!this.m_RPList.ContainsKey(_MapIdx))
		{
			return null;
		}
		if (0 > RPIdx || this.m_RPList[_MapIdx].Count <= RPIdx)
		{
			return null;
		}
		return this.m_RPList[_MapIdx][RPIdx];
	}

	public GxRP FindRP(short sMapIndex, short X, short Y)
	{
		List<GxRP> list = this.GetList(sMapIndex);
		if (list == null)
		{
			return null;
		}
		foreach (GxRP current in list)
		{
			if (current.GetX() == X && current.GetY() == Y)
			{
				return current;
			}
		}
		return null;
	}

	public short GetNerestRP(short sMapIndex, short x, short y, bool bValidRangeUse, bool bValidRangeMinUse)
	{
		short num = 0;
		int num2 = 100000;
		int num3 = (int)((!bValidRangeMinUse) ? 0 : this.m_iValidRadiusMin);
		short num4 = 1;
		while ((int)num4 < this.m_RPList[sMapIndex].Count)
		{
			int num5 = this.m_RPList[sMapIndex][(int)num4].CalcDistance(x, y);
			if (num5 < num2)
			{
				num = num4;
				num2 = num5;
			}
			num4 += 1;
		}
		if (bValidRangeUse)
		{
			GxRP rP = this.GetRP(sMapIndex, (int)num);
			int num6 = rP.CalcDistance(x, y);
			if (num6 > (int)this.m_iValidRadiusMin || num6 < num3)
			{
				num = 0;
			}
		}
		return num;
	}

	public List<GxRP> GetList(short sMapIndex)
	{
		if (!this.m_RPList.ContainsKey(sMapIndex))
		{
			return null;
		}
		return this.m_RPList[sMapIndex];
	}

	public void SAVE()
	{
		StreamWriter streamWriter = new StreamWriter("RPTest.txt");
		string value = string.Empty;
		foreach (short current in this.m_RPList.Keys)
		{
			for (int i = 0; i < this.m_RPList[current].Count; i++)
			{
				GxRP gxRP = this.m_RPList[current][i];
				string text = string.Empty;
				for (int j = 0; j < 4; j++)
				{
					text = text + gxRP.GetLinkedRP(j).ToString() + "/";
				}
				value = string.Concat(new string[]
				{
					gxRP.GetIndex().ToString(),
					"/",
					gxRP.GetMapIndex().ToString(),
					"/",
					gxRP.GetPos().x.ToString(),
					"/",
					gxRP.GetPos().y.ToString(),
					"/",
					gxRP.GetPos().z.ToString(),
					"/",
					text
				});
				streamWriter.WriteLine(value);
			}
		}
		streamWriter.WriteLine("<End>");
		streamWriter.Close();
	}

	public bool _EraseRP(short MapIndex, short nRoadPointIndex)
	{
		GxRP rP = this.GetRP(MapIndex, (int)nRoadPointIndex);
		if (rP == null)
		{
			return false;
		}
		for (int i = 0; i < rP.GetLinkedCount(); i++)
		{
			GxRP rP2 = this.GetRP(MapIndex, (int)rP.GetLinkedRP(i));
			if (rP2 != null)
			{
				rP2.DelLinkedRP(nRoadPointIndex);
			}
		}
		rP.ClearData();
		rP.SetData(nRoadPointIndex, MapIndex, Vector3.zero);
		return true;
	}

	public void AllReflush(short sMapIndex)
	{
		if (!this.m_RPList.ContainsKey(sMapIndex))
		{
			return;
		}
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < this.m_RPList[sMapIndex].Count; i++)
		{
			list.Add(this.m_RPList[sMapIndex][i].GetPos());
		}
		this.m_RPList[sMapIndex].Clear();
		for (int j = 0; j < list.Count; j++)
		{
			if (j != 0 && list[j] == Vector3.zero)
			{
				this.AddRP(sMapIndex, list[j]);
			}
			else
			{
				this.AddRP(sMapIndex, list[j]);
			}
		}
	}

	public bool ParseDataFromNDT(TsDataReader dr)
	{
		this.m_RPList.Clear();
		ROAD_POINT rOAD_POINT = new ROAD_POINT();
		foreach (TsDataReader.Row data in dr)
		{
			rOAD_POINT.SetData(data);
			rOAD_POINT.MAP_IDX = 0;
			GxRP gxRP = new GxRP();
			gxRP.SetData(rOAD_POINT);
			if (!this.m_RPList.ContainsKey(rOAD_POINT.MAP_IDX))
			{
				this.m_RPList.Add(rOAD_POINT.MAP_IDX, new List<GxRP>());
			}
			this.m_RPList[rOAD_POINT.MAP_IDX].Insert((int)rOAD_POINT.ROADPOINT_IDX, gxRP);
		}
		return true;
	}
}
