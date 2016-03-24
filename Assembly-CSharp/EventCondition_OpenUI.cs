using System;
using UnityForms;

public class EventCondition_OpenUI : EventTriggerItem_EventCondition
{
	public string _dlgName = string.Empty;

	public override void RegisterEvent()
	{
		Form.OpenCallback += new EventHandler(this.IsVerify);
	}

	public override void CleanEvent()
	{
		Form.OpenCallback -= new EventHandler(this.IsVerify);
	}

	public override bool IsVaildValue()
	{
		return true;
	}

	public override void IsVerify(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(this._dlgName))
		{
			base.Verify = true;
			return;
		}
		if (sender == null)
		{
			return;
		}
		Form form = sender as Form;
		if (form == null || form.InteractivePanel == null || form.InteractivePanel.gameObject == null)
		{
			return;
		}
		if (this._dlgName != form.InteractivePanel.gameObject.name)
		{
			return;
		}
		base.Verify = true;
	}

	public override string GetComment()
	{
		return this._dlgName + " UI 열었을 때";
	}
}
