using GAME;
using System;
using UnityForms;

public class ExpBoosterDlg : Form
{
	private Label m_lbCount;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Main/DLG_Main_expbooster", G_ID.MAIN_EXPBOOSTER_DLG, false);
		base.Draggable = false;
	}

	public override void SetComponent()
	{
		this.m_lbCount = (base.GetControl("Label_Count01") as Label);
		this.RefreshData();
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
		if (form == null)
		{
			base.SetLocation(0f, GUICamera.height - base.GetSizeY());
			return;
		}
		float x = form.GetLocationX() + form.GetSizeX();
		float y = GUICamera.height - base.GetSizeY();
		base.SetLocation(x, y);
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void RefreshData()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_EXPBOOSTER);
		if (charSubData <= 0L)
		{
			this.Hide();
			return;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2158"),
			"count",
			charSubData.ToString()
		});
		this.m_lbCount.SetText(empty);
		this._SetDialogPos();
	}

	public override void Show()
	{
		base.Show();
		this.RefreshData();
	}
}
