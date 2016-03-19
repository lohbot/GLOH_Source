using System;
using System.Collections.Generic;

public class GxRpAsNode
{
	public short RPIdx;

	public short sMapIdx;

	public int f;

	public int g;

	public int h;

	public GxRpAsNode parent;

	public GxRpAsNode next;

	public List<GxRpAsNode> child = new List<GxRpAsNode>(4);

	public GxRpAsNode(short idx)
	{
		this.RPIdx = idx;
	}
}
