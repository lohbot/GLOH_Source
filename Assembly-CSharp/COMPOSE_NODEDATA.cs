using System;

public class COMPOSE_NODEDATA
{
	public byte i08TreeLevel;

	public int i32ComposeItemUnique;

	public int i64ComposeProductionID;

	public COMPOSE_NODEDATA(byte level, int ItemUnique, int ProductionID)
	{
		this.SetItemData(level, ItemUnique, ProductionID);
	}

	public void SetItemData(byte level, int skillunique, int productidx)
	{
		this.i08TreeLevel = level;
		this.i32ComposeItemUnique = skillunique;
		this.i64ComposeProductionID = productidx;
	}
}
