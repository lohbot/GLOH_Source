using System;
using UnityEngine;
using UnityForms;

public class DLG_AppMemory : Form
{
	private const float MEM_CHECK_TIME = 10f;

	private Label m_lbFPS;

	private Label m_lbAppMemory;

	public float updateInterval = 0.5f;

	private double lastInterval;

	private float frames;

	private float fps;

	private float fMemoryCheckTime;

	private long m_nAppMemory;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "dlg_CheckMemory", G_ID.DLG_MEMORYCHECK, true);
		base.TopMost = true;
		base.ChangeSceneDestory = false;
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), 90f);
		base.InteractivePanel.draggable = true;
	}

	public override void SetComponent()
	{
		this.m_lbFPS = (base.GetControl("Label_FPS") as Label);
		this.m_lbAppMemory = (base.GetControl("Label_Memory") as Label);
		this.m_nAppMemory = NrTSingleton<NrMainSystem>.Instance.CurAppMemory;
		this.m_lbAppMemory.SetText(string.Format("AppMemory : {0}MB", this.m_nAppMemory));
	}

	public override void Update()
	{
		base.Update();
		this.frames += 1f;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if ((double)realtimeSinceStartup > this.lastInterval + (double)this.updateInterval)
		{
			this.fps = (float)((double)this.frames / ((double)realtimeSinceStartup - this.lastInterval));
			this.frames = 0f;
			this.lastInterval = (double)realtimeSinceStartup;
		}
		this.m_lbFPS.Text = "FPS: " + this.fps.ToString("f2");
		if (realtimeSinceStartup > this.fMemoryCheckTime)
		{
			this.fMemoryCheckTime = realtimeSinceStartup + 10f;
			this.m_nAppMemory = NrTSingleton<NrMainSystem>.Instance.AppMemory;
		}
		this.m_lbAppMemory.SetText(string.Format("AppMemory : {0}MB", this.m_nAppMemory));
		if (base.GetLocation().z != 90f)
		{
			base.SetLocation(base.GetLocationX(), base.GetLocationY(), 90f);
		}
	}
}
