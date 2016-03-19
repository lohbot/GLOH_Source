using System;
using System.Collections.Generic;
using UnityEngine;

public class MapLinkAStar
{
	public class AsStack
	{
		public MapLinkAStar.AsNode data;

		public MapLinkAStar.AsStack next;
	}

	public class AsNode
	{
		public MapLinkAStar.AsNode parent;

		public MapLinkAStar.AsNode next;

		public MapLinkAStar.AsStack child;

		public int mapUnique;

		public int f;

		public int g;

		public int h;

		public AsNode()
		{
			this.f = 0;
			this.g = 0;
			this.h = 0;
		}

		public AsNode(int mu)
		{
			this.mapUnique = mu;
		}

		~AsNode()
		{
			this.DeleteChildAll();
		}

		public void AddChild(ref MapLinkAStar.AsNode node)
		{
			this.child = new MapLinkAStar.AsStack
			{
				data = node,
				next = this.child
			};
		}

		public void DeleteChildAll()
		{
			while (this.child != null)
			{
				MapLinkAStar.AsStack asStack = this.child;
				this.child = asStack.next;
			}
		}
	}

	protected int m_iStartMapUnique;

	protected int m_iDestMapUnique;

	private NrMapLinker m_pkMapLinker;

	private MapLinkAStar.AsNode m_rOpen;

	private MapLinkAStar.AsNode m_rClosed;

	private MapLinkAStar.AsNode m_rBest;

	private MapLinkAStar.AsStack m_rStack;

	public void SetMapData(ref NrMapLinker pMapLinker)
	{
		this.m_pkMapLinker = pMapLinker;
	}

	private MapLinkAStar.AsNode GetBest()
	{
		if (this.m_rOpen == null)
		{
			return null;
		}
		MapLinkAStar.AsNode rOpen = this.m_rOpen;
		MapLinkAStar.AsNode rClosed = this.m_rClosed;
		this.m_rOpen = rOpen.next;
		this.m_rClosed = rOpen;
		this.m_rClosed.next = rClosed;
		return rOpen;
	}

	private MapLinkAStar.AsNode CheckList(MapLinkAStar.AsNode node, int mapUnique)
	{
		while (node != null)
		{
			if (node.mapUnique == mapUnique)
			{
				return node;
			}
			node = node.next;
		}
		return null;
	}

	private int Run(ref LinkedList<int> rkMapPath)
	{
		int num;
		for (num = 0; num == 0; num = this.Step())
		{
		}
		if (num == -1 || this.m_rBest == null)
		{
			this.m_rBest = null;
			this.ClearNodes();
			return 0;
		}
		string text = "LINK MAP: ";
		int num2 = 0;
		for (MapLinkAStar.AsNode asNode = this.m_rBest; asNode != null; asNode = asNode.parent)
		{
			if (asNode.mapUnique > -1)
			{
				rkMapPath.AddFirst(asNode.mapUnique);
				text = text + asNode.mapUnique + " ==> Next: ";
				num2++;
			}
		}
		NrTSingleton<NrAutoPath>.Instance.AddDebug(text);
		return num2;
	}

	private int Step()
	{
		this.m_rBest = this.GetBest();
		if (this.m_rBest == null)
		{
			return -1;
		}
		if (this.m_rBest.mapUnique == this.m_iDestMapUnique)
		{
			return 1;
		}
		this.CreateChild(this.m_rBest);
		return 0;
	}

	private void ClearNodes()
	{
		if (this.m_rOpen != null)
		{
			while (this.m_rOpen != null)
			{
				MapLinkAStar.AsNode next = this.m_rOpen.next;
				this.m_rOpen = null;
				this.m_rOpen = next;
			}
		}
		if (this.m_rClosed != null)
		{
			while (this.m_rClosed != null)
			{
				MapLinkAStar.AsNode next = this.m_rClosed.next;
				this.m_rClosed = null;
				this.m_rClosed = next;
			}
		}
	}

	private void CreateChild(MapLinkAStar.AsNode parentNode)
	{
		MapLinkAStar.AsNode asNode = new MapLinkAStar.AsNode();
		LinkedListNode<Map> linkedListNode = this.m_pkMapLinker.FindMap(parentNode.mapUnique);
		if (linkedListNode == null)
		{
			return;
		}
		LinkedList<int> linkMapUniqueList = linkedListNode.Value.GetLinkMapUniqueList();
		foreach (int current in linkMapUniqueList)
		{
			asNode.mapUnique = current;
			this.LinkChild(parentNode, asNode);
		}
	}

	private void LinkChild(MapLinkAStar.AsNode parentNode, MapLinkAStar.AsNode childNode)
	{
		int mapUnique = childNode.mapUnique;
		if (this.m_pkMapLinker.FindMap(mapUnique) == null)
		{
			return;
		}
		int num = parentNode.g + 1;
		MapLinkAStar.AsNode asNode = null;
		if ((asNode = this.CheckList(this.m_rOpen, mapUnique)) != null)
		{
			parentNode.AddChild(ref asNode);
			if (num < asNode.g)
			{
				asNode.parent = parentNode;
				asNode.g = num;
				asNode.f = num + asNode.h;
			}
		}
		else if ((asNode = this.CheckList(this.m_rClosed, mapUnique)) != null)
		{
			parentNode.AddChild(ref asNode);
			if (num < asNode.g)
			{
				asNode.parent = parentNode;
				asNode.g = num;
				asNode.f = num + asNode.h;
				this.UpdateParents(asNode);
			}
		}
		else
		{
			MapLinkAStar.AsNode asNode2 = new MapLinkAStar.AsNode(mapUnique);
			asNode2.parent = parentNode;
			asNode2.g = num;
			asNode2.h = 0;
			asNode2.f = asNode2.g + asNode2.h;
			this.AddToOpen(asNode2);
			parentNode.AddChild(ref asNode2);
		}
	}

	private void AddToOpen(MapLinkAStar.AsNode addNode)
	{
		MapLinkAStar.AsNode asNode = this.m_rOpen;
		MapLinkAStar.AsNode asNode2 = null;
		if (this.m_rOpen == null)
		{
			this.m_rOpen = addNode;
			this.m_rOpen.next = null;
			return;
		}
		while (asNode != null)
		{
			if (addNode.f <= asNode.f)
			{
				if (asNode2 != null)
				{
					asNode2.next = addNode;
					addNode.next = asNode;
				}
				else
				{
					MapLinkAStar.AsNode rOpen = this.m_rOpen;
					this.m_rOpen = addNode;
					this.m_rOpen.next = rOpen;
				}
				return;
			}
			asNode2 = asNode;
			asNode = asNode.next;
		}
		asNode2.next = addNode;
	}

	private void UpdateParents(MapLinkAStar.AsNode node)
	{
		int g = node.g;
		for (MapLinkAStar.AsStack asStack = node.child; asStack != null; asStack = asStack.next)
		{
			MapLinkAStar.AsNode data = asStack.data;
			if (g + 1 < data.g)
			{
				data.g = g + 1;
				data.f = data.g + data.h;
				data.parent = node;
				this.Push(data);
			}
		}
		while (this.m_rStack != null)
		{
			MapLinkAStar.AsNode asNode = this.Pop();
			for (MapLinkAStar.AsStack asStack = asNode.child; asStack != null; asStack = asStack.next)
			{
				MapLinkAStar.AsNode data = asStack.data;
				if (asNode.g + 1 < data.g)
				{
					data.g = asNode.g + 1;
					data.f = data.g + data.h;
					data.parent = asNode;
					this.Push(data);
				}
			}
		}
	}

	private MapLinkAStar.AsNode Pop()
	{
		MapLinkAStar.AsNode data = this.m_rStack.data;
		MapLinkAStar.AsStack rStack = this.m_rStack;
		this.m_rStack = rStack.next;
		return data;
	}

	private void Push(MapLinkAStar.AsNode node)
	{
		if (this.m_rStack != null)
		{
			this.m_rStack = new MapLinkAStar.AsStack();
			this.m_rStack.data = node;
			this.m_rStack.next = null;
		}
		else
		{
			this.m_rStack = new MapLinkAStar.AsStack
			{
				data = node,
				next = this.m_rStack
			};
		}
	}

	public int Generate(int iStart, int iDest, ref LinkedList<int> rkMapPath)
	{
		LinkedListNode<Map> linkedListNode = this.m_pkMapLinker.FindMap(iStart);
		LinkedListNode<Map> linkedListNode2 = this.m_pkMapLinker.FindMap(iDest);
		if (linkedListNode == null || linkedListNode2 == null)
		{
			if (linkedListNode == null)
			{
				Debug.Log("startMap == null Start:" + iStart);
			}
			if (linkedListNode2 == null)
			{
				Debug.Log("destMap == null Dest:" + iDest);
			}
			Debug.Log("return -1;\t// 오류 : 맵정보에 없는 맵 인덱스로 맵경유를 찾으려 함");
			return -1;
		}
		this.ClearNodes();
		rkMapPath.Clear();
		this.m_iStartMapUnique = iStart;
		this.m_iDestMapUnique = iDest;
		MapLinkAStar.AsNode asNode = new MapLinkAStar.AsNode(this.m_iStartMapUnique);
		asNode.g = 0;
		asNode.h = 0;
		asNode.f = asNode.g + asNode.h;
		this.m_rOpen = asNode;
		return this.Run(ref rkMapPath);
	}
}
