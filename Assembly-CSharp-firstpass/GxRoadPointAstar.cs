using System;
using System.Collections.Generic;

public class GxRoadPointAstar
{
	public enum TMPRPINDEX
	{
		RAS_TMPSP_INDEX = -100,
		RAS_TMPDP_INDEX = -200
	}

	protected const int MAX_LINK = 4;

	protected GxRP m_sTmpRP;

	protected GxRP m_dTmpRP;

	protected short m_sRPIdx;

	protected short m_dRPIdx;

	protected int m_iSX;

	protected int m_iSY;

	protected int m_iDX;

	protected int m_iDY;

	public GxRoadPointManager m_pkRPSys;

	protected GxRpAsNode m_pOpen;

	protected GxRpAsNode m_pClosed;

	protected GxRpAsNode m_pBest;

	protected GxRasStack m_pStack;

	private short GetTmpRP(GxRoadPointAstar.TMPRPINDEX tempRPIndex, short p_MapIdx, short x, short y)
	{
		List<short> list = new List<short>();
		this.m_pkRPSys.GetValidRangeRPIndexes(p_MapIdx, x, y, ref list);
		GxRP gxRP = (tempRPIndex != GxRoadPointAstar.TMPRPINDEX.RAS_TMPSP_INDEX) ? this.m_dTmpRP : this.m_sTmpRP;
		this.m_sRPIdx = ((tempRPIndex != GxRoadPointAstar.TMPRPINDEX.RAS_TMPSP_INDEX) ? this.m_sRPIdx : -100);
		this.m_dRPIdx = ((tempRPIndex != GxRoadPointAstar.TMPRPINDEX.RAS_TMPDP_INDEX) ? this.m_dRPIdx : -200);
		gxRP.ClearData();
		gxRP.SetData((short)tempRPIndex, p_MapIdx, x, y);
		if (list.Count != 0)
		{
			foreach (short current in list)
			{
				if (!gxRP.AddLinkedRP(current))
				{
					break;
				}
			}
		}
		else
		{
			short nerestRP = this.m_pkRPSys.GetNerestRP(p_MapIdx, x, y, false, false);
			gxRP.AddLinkedRP(nerestRP);
		}
		return (short)tempRPIndex;
	}

	private bool StepInitialize(short MapIdx, short sRPIndex, short dRPIndex)
	{
		this.ClearNodes();
		this.m_sRPIdx = sRPIndex;
		this.m_dRPIdx = dRPIndex;
		GxRP gxRP = (this.m_sRPIdx <= 0) ? this.m_sTmpRP : this.m_pkRPSys.GetRP(MapIdx, (int)this.m_sRPIdx);
		GxRP gxRP2 = (this.m_dRPIdx <= 0) ? this.m_dTmpRP : this.m_pkRPSys.GetRP(MapIdx, (int)this.m_dRPIdx);
		this.m_iSX = (int)gxRP.GetX();
		this.m_iSY = (int)gxRP.GetY();
		this.m_iDX = (int)gxRP2.GetX();
		this.m_iDY = (int)gxRP2.GetY();
		GxRpAsNode gxRpAsNode = new GxRpAsNode(this.m_sRPIdx);
		gxRpAsNode.g = 0;
		gxRpAsNode.h = Math.Abs((int)(gxRP2.GetX() - gxRP.GetX())) + Math.Abs((int)(gxRP2.GetY() - gxRP.GetY()));
		gxRpAsNode.f = gxRpAsNode.g + gxRpAsNode.h;
		this.m_pOpen = gxRpAsNode;
		return true;
	}

	protected void ClearNodes()
	{
		if (this.m_pOpen != null)
		{
			while (this.m_pOpen != null)
			{
				GxRpAsNode next = this.m_pOpen.next;
				this.m_pOpen = next;
			}
		}
		if (this.m_pClosed != null)
		{
			while (this.m_pClosed != null)
			{
				GxRpAsNode next = this.m_pClosed.next;
				this.m_pClosed = next;
			}
		}
	}

	private void CreateChild(ref GxRpAsNode parentNode, short sMapIndex)
	{
		GxRpAsNode gxRpAsNode = null;
		GxRP gxRP = (parentNode.RPIdx <= 0) ? ((parentNode.RPIdx != -100) ? this.m_dTmpRP : this.m_sTmpRP) : this.m_pkRPSys.GetRP(parentNode.sMapIdx, (int)parentNode.RPIdx);
		for (int i = 0; i < 4; i++)
		{
			if (gxRP.GetLinkedRP(i) != 0)
			{
				gxRpAsNode = new GxRpAsNode(gxRP.GetLinkedRP(i));
				this.LinkedChild(sMapIndex, ref parentNode, ref gxRpAsNode);
			}
		}
	}

	private void LinkedChild(short sMapIndex, ref GxRpAsNode parentNode, ref GxRpAsNode childNode)
	{
		short rPIdx = childNode.RPIdx;
		GxRP gxRP = (rPIdx <= 0) ? ((rPIdx != -100) ? this.m_dTmpRP : this.m_sTmpRP) : this.m_pkRPSys.GetRP(sMapIndex, (int)rPIdx);
		int num = parentNode.g + 1;
		GxRpAsNode gxRpAsNode = null;
		gxRpAsNode = this.CheckList(this.m_pOpen, rPIdx);
		if (gxRpAsNode != null)
		{
			parentNode.child.Add(gxRpAsNode);
			if (num < gxRpAsNode.g)
			{
				gxRpAsNode.parent = parentNode;
				gxRpAsNode.g = num;
				gxRpAsNode.f = num + gxRpAsNode.h;
			}
		}
		else
		{
			gxRpAsNode = this.CheckList(this.m_pClosed, rPIdx);
			if (gxRpAsNode != null)
			{
				parentNode.child.Add(gxRpAsNode);
				if (num < gxRpAsNode.g)
				{
					gxRpAsNode.parent = parentNode;
					gxRpAsNode.g = num;
					gxRpAsNode.f = num + gxRpAsNode.h;
					this.UpdateParents(ref gxRpAsNode);
				}
			}
			else
			{
				GxRpAsNode gxRpAsNode2 = new GxRpAsNode(rPIdx);
				gxRpAsNode2.parent = parentNode;
				gxRpAsNode2.g = num;
				gxRpAsNode2.h = Math.Abs((int)gxRP.GetX() - this.m_iDX) + Math.Abs((int)gxRP.GetY() - this.m_iDY);
				gxRpAsNode2.f = gxRpAsNode2.g + gxRpAsNode2.h;
				gxRpAsNode2.sMapIdx = sMapIndex;
				this.AddToOpen(ref gxRpAsNode2);
				parentNode.child.Add(gxRpAsNode2);
			}
		}
	}

	private GxRpAsNode CheckList(GxRpAsNode node, short RPIdx)
	{
		while (node != null)
		{
			if (node.RPIdx == RPIdx)
			{
				return node;
			}
			node = node.next;
		}
		return null;
	}

	private void UpdateParents(ref GxRpAsNode node)
	{
		int g = node.g;
		int count = node.child.Count;
		GxRpAsNode gxRpAsNode = null;
		for (int i = 0; i < count; i++)
		{
			gxRpAsNode = node.child[i];
			if (g + 1 < gxRpAsNode.g)
			{
				gxRpAsNode.g = g + 1;
				gxRpAsNode.f = gxRpAsNode.g + gxRpAsNode.h;
				gxRpAsNode.parent = node;
				this.Push(ref gxRpAsNode);
			}
		}
		while (this.m_pStack != null)
		{
			GxRpAsNode gxRpAsNode2 = this.Pop();
			count = gxRpAsNode2.child.Count;
			for (int j = 0; j < count; j++)
			{
				gxRpAsNode = gxRpAsNode2.child[j];
				if (gxRpAsNode2.g + 1 < gxRpAsNode.g)
				{
					gxRpAsNode.g = gxRpAsNode2.g + 1;
					gxRpAsNode.f = gxRpAsNode.g + gxRpAsNode.h;
					gxRpAsNode.parent = gxRpAsNode2;
					this.Push(ref gxRpAsNode);
				}
			}
		}
	}

	private GxRpAsNode Pop()
	{
		GxRpAsNode data = this.m_pStack.data;
		GxRasStack pStack = this.m_pStack;
		this.m_pStack = pStack.next;
		return data;
	}

	private void Push(ref GxRpAsNode node)
	{
		if (this.m_pStack == null)
		{
			this.m_pStack = new GxRasStack();
			this.m_pStack.data = node;
			this.m_pStack.next = null;
		}
		else
		{
			this.m_pStack = new GxRasStack
			{
				data = node,
				next = this.m_pStack
			};
		}
	}

	private void AddToOpen(ref GxRpAsNode addnode)
	{
		GxRpAsNode gxRpAsNode = this.m_pOpen;
		GxRpAsNode gxRpAsNode2 = null;
		if (this.m_pOpen == null)
		{
			this.m_pOpen = addnode;
			this.m_pOpen.next = null;
			return;
		}
		while (gxRpAsNode != null)
		{
			if (addnode.f <= gxRpAsNode.f)
			{
				if (gxRpAsNode2 != null)
				{
					gxRpAsNode2.next = addnode;
					addnode.next = gxRpAsNode;
				}
				else
				{
					GxRpAsNode pOpen = this.m_pOpen;
					this.m_pOpen = addnode;
					this.m_pOpen.next = pOpen;
				}
				return;
			}
			gxRpAsNode2 = gxRpAsNode;
			gxRpAsNode = gxRpAsNode.next;
		}
		gxRpAsNode2.next = addnode;
	}

	private int Step(short sMapIndex)
	{
		this.m_pBest = this.GetBest();
		if (this.m_pBest == null)
		{
			return -1;
		}
		if (this.m_pBest.RPIdx == this.m_dRPIdx)
		{
			return 1;
		}
		this.CreateChild(ref this.m_pBest, sMapIndex);
		if (this.m_dRPIdx == -200 && this.m_dTmpRP.IsLinked(this.m_pBest.RPIdx))
		{
			return 1;
		}
		return 0;
	}

	private GxRpAsNode GetBest()
	{
		if (this.m_pOpen == null)
		{
			return null;
		}
		GxRpAsNode pOpen = this.m_pOpen;
		GxRpAsNode pClosed = this.m_pClosed;
		this.m_pOpen = pOpen.next;
		this.m_pClosed = pOpen;
		this.m_pClosed.next = pClosed;
		return pOpen;
	}

	public int GeneratePath(short sMapIndex, short p_start, short dest, ref LinkedList<GxRpAsNode> rRPPath)
	{
		rRPPath.Clear();
		if (!this.StepInitialize(sMapIndex, p_start, dest))
		{
			this.ClearNodes();
			return -1;
		}
		return this.RunGenerateRPPath(ref rRPPath, sMapIndex);
	}

	private int RunGenerateRPPath(ref LinkedList<GxRpAsNode> rRPPath, short sMapIndex)
	{
		int num = 0;
		int num2;
		for (num2 = 0; num2 == 0; num2 = this.Step(sMapIndex))
		{
		}
		if (num2 == -1 || this.m_pBest == null)
		{
			this.m_pBest = null;
			this.ClearNodes();
			return 0;
		}
		for (GxRpAsNode gxRpAsNode = this.m_pBest; gxRpAsNode != null; gxRpAsNode = gxRpAsNode.parent)
		{
			if (gxRpAsNode.RPIdx > 0)
			{
				rRPPath.AddFirst(gxRpAsNode);
				num++;
			}
		}
		this.ClearNodes();
		return num;
	}
}
