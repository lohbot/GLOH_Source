using System;
using UnityEngine;
using UnityForms;

public class IndunTime_DLG : Form
{
	public enum eSHOW_MODE
	{
		eSHOW_MODE_NORMAL,
		eSHOW_MODE_SMALL,
		eSHOW_MODE_MAX
	}

	private Label m_lbTitle;

	private Label m_lbTime;

	private Box m_bxStayTime;

	private IndunTime_DLG.eSHOW_MODE m_eShowMode;

	private float m_fStartTime;

	private float m_fEndTime;

	private float m_fTopY = 28f;

	private float m_fChangeTime = 5f;

	private float m_fUpdateTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Indun/dlg_stayindun_time", G_ID.INDUNTIME_DLG, false);
		base.Draggable = false;
		base.AlwaysUpdate = true;
		base.SetNonClickALL();
		base.ChangeSceneDestory = false;
		this.Hide();
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("Label_Title") as Label);
		this.m_lbTime = (base.GetControl("Label_Time") as Label);
		this.m_bxStayTime = (base.GetControl("Box_Dun_StayTime") as Box);
		if (this.m_lbTitle != null)
		{
			this.m_lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1529"));
		}
		this.m_eShowMode = IndunTime_DLG.eSHOW_MODE.eSHOW_MODE_NORMAL;
		if (TsPlatform.IsMobile)
		{
			base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, 0f);
		}
		else
		{
			base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, GUICamera.height * 0.25f - base.GetSize().y / 2f);
		}
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		if (this.m_eShowMode == IndunTime_DLG.eSHOW_MODE.eSHOW_MODE_NORMAL)
		{
			base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, GUICamera.height * 0.25f - base.GetSize().y / 2f);
		}
		else if (this.m_eShowMode == IndunTime_DLG.eSHOW_MODE.eSHOW_MODE_SMALL)
		{
			base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, this.m_fTopY);
		}
	}

	public void SetStayTime(float fStayTime)
	{
		this.m_fStartTime = Time.time;
		this.m_fEndTime = this.m_fStartTime + fStayTime;
		base.AllHideLayer();
		base.ShowLayer(1);
	}

	public override void OnClose()
	{
	}

	public override void Update()
	{
		if (Mathf.Abs(Time.time - this.m_fUpdateTime) < 1f)
		{
			return;
		}
		if (NrLoadPageScreen.IsShow())
		{
			base.AllHideLayer();
			this.m_fStartTime = Time.time;
			return;
		}
		float num = this.m_fEndTime - Time.time;
		int num2 = (int)(num / 3600f);
		int num3 = (int)((num - (float)num2 * 3600f) / 60f);
		int num4 = (int)((num - (float)num2 * 3600f - (float)num3 * 60f) % 60f);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1527"),
			"hour",
			num2.ToString(),
			"min",
			num3.ToString(),
			"sec",
			num4.ToString()
		});
		if (this.m_eShowMode == IndunTime_DLG.eSHOW_MODE.eSHOW_MODE_NORMAL)
		{
			if (Mathf.Abs(Time.time - this.m_fStartTime) < this.m_fChangeTime)
			{
				base.ShowLayer(1);
				this.m_lbTime.SetText(empty);
			}
			else
			{
				base.ShowLayer(2);
				base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, this.m_fTopY);
				this.m_eShowMode = IndunTime_DLG.eSHOW_MODE.eSHOW_MODE_SMALL;
				this.m_bxStayTime.SetText(empty);
			}
		}
		else if (this.m_eShowMode == IndunTime_DLG.eSHOW_MODE.eSHOW_MODE_SMALL)
		{
			base.ShowLayer(2);
			this.m_bxStayTime.SetText(empty);
		}
		this.m_fUpdateTime = Time.time;
	}
}
