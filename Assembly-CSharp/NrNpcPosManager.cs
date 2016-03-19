using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NrNpcPosManager : NrTSingleton<NrNpcPosManager>
{
	private Dictionary<int, Dictionary<string, NrNpcPos>> m_mapNpcPos = new Dictionary<int, Dictionary<string, NrNpcPos>>();

	private Dictionary<int, Vector2> m_kWideCollArea = new Dictionary<int, Vector2>();

	private List<GS_INDUN_EXCEPT_AREA_INFO> m_listIndunExceptMovePos = new List<GS_INDUN_EXCEPT_AREA_INFO>();

	private NrNpcPosManager()
	{
		this.Init();
	}

	private void Init()
	{
		this.m_mapNpcPos.Clear();
		this.m_kWideCollArea.Clear();
		this.m_listIndunExceptMovePos.Clear();
	}

	public void ClearIndunExceptMovePos()
	{
		this.m_listIndunExceptMovePos.Clear();
	}

	public void AddNpcPos(ECO kEco)
	{
		if (0f < kEco.kMovePos[0].x || 0f < kEco.kMovePos[0].z)
		{
			return;
		}
		if (0f < kEco.kRanPos.x || 0f < kEco.kRanPos.z)
		{
			return;
		}
		NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(kEco.szCharCode[0]);
		if (charKindInfoFromCode == null)
		{
			return;
		}
		if (charKindInfoFromCode.IsATB(8L))
		{
			NrNpcPos nrNpcPos = new NrNpcPos();
			nrNpcPos.strKey = NrTSingleton<UIDataManager>.Instance.GetString(kEco.szCharCode[0], ((int)kEco.kFixPos.x).ToString(), ((int)kEco.kFixPos.z).ToString());
			charKindInfoFromCode.SetPosKey(nrNpcPos.strKey);
			nrNpcPos.strName = charKindInfoFromCode.GetName();
			nrNpcPos.nCharKind = charKindInfoFromCode.GetCharKind();
			nrNpcPos.nMapIndex = kEco.MapIndex;
			nrNpcPos.kPos.x = kEco.kFixPos.x;
			nrNpcPos.kPos.y = kEco.kFixPos.y;
			nrNpcPos.kPos.z = kEco.kFixPos.z;
			this.AddNpcPos(nrNpcPos);
		}
	}

	public void AddNpcPos(NrNpcPos kNPCPos)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsNPCLimit(kNPCPos.nCharKind))
		{
			return;
		}
		if (!this.m_mapNpcPos.ContainsKey(kNPCPos.nMapIndex))
		{
			Dictionary<string, NrNpcPos> dictionary = new Dictionary<string, NrNpcPos>();
			dictionary.Add(kNPCPos.strKey, kNPCPos);
			this.m_mapNpcPos.Add(kNPCPos.nMapIndex, dictionary);
		}
		else
		{
			Dictionary<string, NrNpcPos> dictionary2 = this.m_mapNpcPos[kNPCPos.nMapIndex];
			if (dictionary2 != null && !dictionary2.ContainsKey(kNPCPos.strKey))
			{
				dictionary2.Add(kNPCPos.strKey, kNPCPos);
			}
		}
	}

	public void RemoveNpcPos(int mapIndex, NrNpcPos kNPCPos)
	{
		if (this.m_mapNpcPos.ContainsKey(mapIndex))
		{
			Dictionary<string, NrNpcPos> dictionary = this.m_mapNpcPos[mapIndex];
			if (dictionary != null && dictionary.ContainsKey(kNPCPos.strKey))
			{
				dictionary.Remove(kNPCPos.strKey);
			}
		}
	}

	public void RemoveNpcPos(int mapIndex, string key)
	{
		if (this.m_mapNpcPos.ContainsKey(mapIndex))
		{
			Dictionary<string, NrNpcPos> dictionary = this.m_mapNpcPos[mapIndex];
			if (dictionary != null && dictionary.ContainsKey(key))
			{
				dictionary.Remove(key);
			}
		}
	}

	public void ClearAllNpcPos()
	{
		this.m_mapNpcPos.Clear();
	}

	public NrNpcPos GetNpcPos(string key, int charKind, int mapIndex)
	{
		if (this.m_mapNpcPos.ContainsKey(mapIndex))
		{
			Dictionary<string, NrNpcPos> dictionary = this.m_mapNpcPos[mapIndex];
			if (dictionary != null)
			{
				if (dictionary.ContainsKey(key))
				{
					return dictionary[key];
				}
				foreach (NrNpcPos current in dictionary.Values)
				{
					if (current.nCharKind == charKind)
					{
						return current;
					}
				}
			}
		}
		return null;
	}

	public Dictionary<string, NrNpcPos> GetNpcPosList(int mapIndex)
	{
		if (this.m_mapNpcPos.ContainsKey(mapIndex))
		{
			Dictionary<string, NrNpcPos> dictionary = this.m_mapNpcPos[mapIndex];
			if (dictionary != null)
			{
				return dictionary;
			}
		}
		return null;
	}

	public void AddWideCollArea(int charid, Vector3 charpos)
	{
		if (NrTSingleton<NkCharManager>.Instance.GetChar(charid) == null)
		{
			return;
		}
		if (this.m_kWideCollArea.ContainsKey(charid))
		{
			return;
		}
		Vector2 value = new Vector2(charpos.x, charpos.z);
		this.m_kWideCollArea.Add(charid, value);
	}

	public void DelWideCollArea(int charid)
	{
		if (!this.m_kWideCollArea.ContainsKey(charid))
		{
			return;
		}
		this.m_kWideCollArea.Remove(charid);
	}

	public void AddIndunExceptArea(GS_INDUN_EXCEPT_AREA_INFO pkInfo)
	{
		foreach (GS_INDUN_EXCEPT_AREA_INFO current in this.m_listIndunExceptMovePos)
		{
			if (current.m_nIndex == pkInfo.m_nIndex)
			{
				return;
			}
		}
		this.m_listIndunExceptMovePos.Add(pkInfo);
	}

	public void DelIndunExceptArea(GS_INDUN_EXCEPT_AREA_INFO pkInfo)
	{
		int num = -1;
		foreach (GS_INDUN_EXCEPT_AREA_INFO current in this.m_listIndunExceptMovePos)
		{
			num++;
			if (current.m_nIndex == pkInfo.m_nIndex)
			{
				break;
			}
		}
		this.m_listIndunExceptMovePos.RemoveAt(num);
	}

	public void CleanWideCollArea()
	{
		int[] array = new int[3];
		int num = 0;
		foreach (KeyValuePair<int, Vector2> current in this.m_kWideCollArea)
		{
			if (current.Key != 0)
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(current.Key);
				if (@char == null || !@char.IsReady3DModel())
				{
					array[num++] = current.Key;
				}
				if (num >= 3)
				{
					break;
				}
			}
		}
		for (int i = 0; i < num; i++)
		{
			this.DelWideCollArea(array[i]);
		}
	}

	public void ClearWideCollArea()
	{
		this.m_kWideCollArea.Clear();
	}

	public bool IsWideCollArea(Vector2 pos)
	{
		foreach (KeyValuePair<int, Vector2> current in this.m_kWideCollArea)
		{
			if (current.Key != 0)
			{
				if (Vector2.Distance(current.Value, pos) <= 2f)
				{
					bool result = true;
					return result;
				}
			}
		}
		foreach (GS_INDUN_EXCEPT_AREA_INFO current2 in this.m_listIndunExceptMovePos)
		{
			if (current2.posStart.x <= pos.x && pos.x <= current2.posEnd.x && current2.posStart.z <= pos.y && pos.y <= current2.posEnd.z)
			{
				bool result = true;
				return result;
			}
		}
		return false;
	}

	public bool FindWideCollArea(Vector2 pos, ref Vector2 widecollcenter)
	{
		foreach (KeyValuePair<int, Vector2> current in this.m_kWideCollArea)
		{
			if (current.Key != 0)
			{
				if (Vector2.Distance(current.Value, pos) <= 2f)
				{
					widecollcenter = current.Value;
					return true;
				}
			}
		}
		return false;
	}
}
