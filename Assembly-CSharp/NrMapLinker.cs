using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NrMapLinker
{
	public enum LinkType
	{
		DUPLEX,
		SIMPLEX
	}

	protected LinkedList<Map> m_kMapList = new LinkedList<Map>();

	protected MapLinkAStar m_kAstar;

	protected NrGateManager m_kGateMgr;

	public void SetGateManager(ref NrGateManager GateMgr)
	{
		this.m_kGateMgr = GateMgr;
		this.MakeMapLink();
	}

	public void MakeMapLink()
	{
		ICollection mapInfo_Col = NrTSingleton<NrBaseTableManager>.Instance.GetMapInfo_Col();
		foreach (MAP_INFO mAP_INFO in mapInfo_Col)
		{
			Map map = new Map();
			Map.Record record = new Map.Record(mAP_INFO.MAP_INDEX);
			map.SetRecord(record);
			GATE_INFO[] gateInfo = mAP_INFO.GetGateInfo();
			GATE_INFO[] array = gateInfo;
			for (int i = 0; i < array.Length; i++)
			{
				GATE_INFO gATE_INFO = array[i];
				map.InsertLinkMapUnique(gATE_INFO.DST_MAP_IDX);
				GateData gate = new GateData(gATE_INFO.DST_MAP_IDX, new Vector2(gATE_INFO.SRC_POSX, gATE_INFO.SRC_POSZ));
				this.m_kGateMgr.Add(mAP_INFO.MAP_INDEX, gate);
			}
			this.AddMap(map);
		}
	}

	public LinkedListNode<Map> FindMap(int iMapUnique)
	{
		for (LinkedListNode<Map> linkedListNode = this.m_kMapList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			if (linkedListNode.Value.GetRecord().m_iUnique == iMapUnique)
			{
				return linkedListNode;
			}
		}
		return null;
	}

	public LinkedListNode<Map> AddMap(Map.Record kMapRecord)
	{
		if (this.FindMap(kMapRecord.m_iUnique) != null)
		{
			return null;
		}
		Map map = new Map();
		map.SetRecord(kMapRecord);
		this.m_kMapList.AddLast(map);
		return this.m_kMapList.Last;
	}

	public void AddMap(Map kMap)
	{
		if (this.FindMap(kMap.GetIndex()) != null)
		{
			return;
		}
		this.m_kMapList.AddLast(kMap);
	}

	public void DeleteMap(int iMapUnique)
	{
		for (LinkedListNode<Map> linkedListNode = this.m_kMapList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			if (linkedListNode.Value.GetRecord().m_iUnique == iMapUnique)
			{
				this.UnLinkMapAll(linkedListNode);
				this.m_kMapList.Remove(linkedListNode);
				return;
			}
		}
	}

	public void DeleteMapAll()
	{
		this.m_kMapList.Clear();
	}

	private bool LinkMap(int iMapUnique, int iLinkMapUnique)
	{
		NrMapLinker.LinkType eLinkType = NrMapLinker.LinkType.DUPLEX;
		LinkedListNode<Map> pMapNode = this.FindMap(iMapUnique);
		return this.LinkMap(pMapNode, iLinkMapUnique, eLinkType);
	}

	private bool LinkMap(int iMapUnique, int iLinkMapUnique, NrMapLinker.LinkType eLinkType)
	{
		LinkedListNode<Map> pMapNode = this.FindMap(iMapUnique);
		return this.LinkMap(pMapNode, iLinkMapUnique, eLinkType);
	}

	private bool LinkMap(LinkedListNode<Map> pMapNode, int iLinkMapUnique, NrMapLinker.LinkType eLinkType)
	{
		LinkedListNode<Map> linkedListNode = this.FindMap(iLinkMapUnique);
		if (linkedListNode == null)
		{
			return false;
		}
		int iUnique = this.m_kMapList.Find(pMapNode.Value).Value.GetRecord().m_iUnique;
		if (iUnique == iLinkMapUnique)
		{
			return false;
		}
		if (eLinkType != NrMapLinker.LinkType.DUPLEX)
		{
			if (eLinkType == NrMapLinker.LinkType.SIMPLEX)
			{
				this.m_kMapList.Find(pMapNode.Value).Value.InsertLinkMapUnique(iLinkMapUnique);
			}
		}
		else
		{
			this.m_kMapList.Find(pMapNode.Value).Value.InsertLinkMapUnique(iLinkMapUnique);
			this.m_kMapList.Find(linkedListNode.Value).Value.InsertLinkMapUnique(iUnique);
		}
		return true;
	}

	public bool UnLinkMap(int iMapUnique, int iLinkMapUnique)
	{
		NrMapLinker.LinkType eLinkType = NrMapLinker.LinkType.DUPLEX;
		LinkedListNode<Map> pMapNode = this.FindMap(iMapUnique);
		return this.LinkMap(pMapNode, iLinkMapUnique, eLinkType);
	}

	public bool UnLinkMap(int iMapUnique, int iLinkMapUnique, NrMapLinker.LinkType eLinkType)
	{
		LinkedListNode<Map> pMapNode = this.FindMap(iMapUnique);
		return this.LinkMap(pMapNode, iLinkMapUnique, eLinkType);
	}

	private bool UnLinkMap(LinkedListNode<Map> pMapNode, int iLinkMapUnique, NrMapLinker.LinkType eLinkType)
	{
		LinkedListNode<Map> linkedListNode = this.FindMap(iLinkMapUnique);
		if (linkedListNode == null)
		{
			return false;
		}
		int iUnique = this.m_kMapList.Find(pMapNode.Value).Value.GetRecord().m_iUnique;
		if (iUnique == iLinkMapUnique)
		{
			return false;
		}
		if (eLinkType != NrMapLinker.LinkType.DUPLEX)
		{
			if (eLinkType == NrMapLinker.LinkType.SIMPLEX)
			{
				if (pMapNode != null)
				{
					this.m_kMapList.Find(pMapNode.Value).Value.RemoveLinkMapUnique(iLinkMapUnique);
				}
			}
		}
		else
		{
			if (pMapNode != null)
			{
				this.m_kMapList.Find(pMapNode.Value).Value.RemoveLinkMapUnique(iLinkMapUnique);
			}
			if (linkedListNode != null)
			{
				this.m_kMapList.Find(linkedListNode.Value).Value.RemoveLinkMapUnique(iUnique);
			}
		}
		return true;
	}

	public void UnLinkMapAll(int iMapUnique)
	{
		LinkedListNode<Map> linkedListNode = this.FindMap(iMapUnique);
		if (linkedListNode == null)
		{
			return;
		}
		this.UnLinkMapAll(linkedListNode);
	}

	public void UnLinkMapAll(LinkedListNode<Map> pMapIter)
	{
		LinkedListNode<Map> linkedListNode = this.m_kMapList.Find(pMapIter.Value);
		linkedListNode.Value.RemoveLinkUniqueAll();
		for (LinkedListNode<Map> linkedListNode2 = this.m_kMapList.First; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
		{
			linkedListNode2.Value.RemoveLinkMapUnique(linkedListNode.Value.GetRecord().m_iUnique);
		}
	}
}
