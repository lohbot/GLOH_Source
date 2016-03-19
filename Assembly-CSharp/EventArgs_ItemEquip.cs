using System;

public class EventArgs_ItemEquip : EventArgs
{
	public long ItemUnique;

	public void Set(long itemunique)
	{
		this.ItemUnique = itemunique;
	}
}
