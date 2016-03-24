using System;
using UnityForms;

public class RaceSelectDlg : Form
{
	public Button _Back;

	private bool m_MouseOver;

	public bool MouseOver
	{
		get
		{
			return this.m_MouseOver;
		}
		set
		{
			this.m_MouseOver = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "SelectChar/dlg_raceselect", G_ID.RACE_SELECT_DLG, false);
	}

	public override void SetComponent()
	{
		this._Back = (base.GetControl("Button_BackButton01") as Button);
		this._Back.AddMouseOutDelegate(new EZValueChangedDelegate(this.OnMouseOut));
	}

	private void OnMouserOver(IUIObject obj)
	{
		this.m_MouseOver = true;
	}

	private void OnMouseOut(IUIObject obj)
	{
		this.m_MouseOver = false;
	}

	public void ReLocation()
	{
	}
}
