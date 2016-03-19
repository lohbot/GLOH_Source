using System;

public class EventCondition_ItemEquip : EventTriggerItem_EventCondition
{
	public string ItemUnique = string.Empty;

	public override void RegisterEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.ItemEquip.ItemEquip += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		NrTSingleton<EventConditionHandler>.Instance.ItemEquip.ItemEquip -= new EventHandler(this.IsVerify);
	}

	public override bool IsVaildValue()
	{
		return !string.IsNullOrEmpty(this.ItemUnique);
	}

	public override void IsVerify(object sender, EventArgs e)
	{
		EventArgs_ItemEquip eventArgs_ItemEquip = e as EventArgs_ItemEquip;
		if (eventArgs_ItemEquip == null)
		{
			return;
		}
		if (this.ItemUnique.Equals(eventArgs_ItemEquip.ItemUnique.ToString()))
		{
			base.Verify = true;
		}
	}

	public override string GetComment()
	{
		return this.ItemUnique.ToString() + " 아이템을 장착 했을 때";
	}
}
