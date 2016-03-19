using System;
using UnityForms;

public class Region_Name_Dlg : Form
{
	public static int REGION_SHOW_TICK = 3000;

	private int m_nShowTick;

	private int m_nStartTick;

	private Label m_laRegionName;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Main/DLG_regionName", G_ID.REGION_NAME_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_laRegionName = (base.GetControl("Label_regionName") as Label);
	}

	public override void Update()
	{
		if (0 < this.m_nShowTick)
		{
			int num = this.m_nShowTick - Math.Abs(Environment.TickCount - this.m_nStartTick);
			if (0 >= num)
			{
				this.Close();
			}
			else
			{
				base.SetAlpha((float)num / (float)this.m_nShowTick);
			}
		}
	}

	public void ShowRegionName()
	{
		string mapName = NrTSingleton<MapManager>.Instance.GetMapName();
		this.ShowRegionName(mapName, Region_Name_Dlg.REGION_SHOW_TICK);
	}

	public void ShowRegionName(string strRegionName)
	{
		this.ShowRegionName(strRegionName, Region_Name_Dlg.REGION_SHOW_TICK);
	}

	public void ShowRegionName(string strRegionName, int nRegionShowTick)
	{
		if (string.IsNullOrEmpty(strRegionName) || 0 >= nRegionShowTick)
		{
			this.Close();
			return;
		}
		float x = GUICamera.width / 2f - base.GetSize().x / 2f;
		float y = GUICamera.height * 0.24f - base.GetSize().y / 2f;
		base.SetLocation(x, y);
		this.m_laRegionName.Text = strRegionName;
		this.m_nStartTick = Environment.TickCount;
		this.m_nShowTick = nRegionShowTick;
		this.Show();
	}
}
