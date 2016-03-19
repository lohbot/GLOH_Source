using Ndoors.Framework.Stage;
using System;
using TsLibs;

public class CAMERASETTING_DATA : NrTableData
{
	public Scene.Type m_eScene;

	public int m_Level;

	public float m_Zoom;

	public float m_YRotate;

	public float m_Fov;

	public float m_RunFov;

	public float m_RunTime;

	public float m_LerpTime;

	public float m_fHumanHeight;

	public float m_fFurryHeight;

	public float m_fElfHeight;

	public float m_fAperture;

	public float m_fHumanZoom;

	public float m_fFurryZoom;

	public float m_fElfZoom;

	public float m_fsunShaftIntensity;

	public string szSceneKind = string.Empty;

	public CAMERASETTING_DATA()
	{
		this.Init();
	}

	public void Init()
	{
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.szSceneKind);
		row.GetColumn(num++, out this.m_Level);
		row.GetColumn(num++, out this.m_Zoom);
		row.GetColumn(num++, out this.m_YRotate);
		row.GetColumn(num++, out this.m_Fov);
		row.GetColumn(num++, out this.m_RunFov);
		row.GetColumn(num++, out this.m_RunTime);
		row.GetColumn(num++, out this.m_LerpTime);
		row.GetColumn(num++, out this.m_fHumanHeight);
		row.GetColumn(num++, out this.m_fFurryHeight);
		row.GetColumn(num++, out this.m_fElfHeight);
		row.GetColumn(num++, out this.m_fHumanZoom);
		row.GetColumn(num++, out this.m_fFurryZoom);
		row.GetColumn(num++, out this.m_fElfZoom);
		row.GetColumn(num++, out this.m_fAperture);
		row.GetColumn(num++, out this.m_fsunShaftIntensity);
	}

	public float GetTribeHeight(byte tribe)
	{
		if ((1 & tribe) > 0)
		{
			return this.m_fHumanHeight;
		}
		if ((2 & tribe) > 0)
		{
			return this.m_fFurryHeight;
		}
		if ((4 & tribe) > 0)
		{
			return this.m_fElfHeight;
		}
		return 0f;
	}

	public float GetTribeZoom(byte tribe)
	{
		if ((1 & tribe) > 0)
		{
			return this.m_fHumanZoom;
		}
		if ((2 & tribe) > 0)
		{
			return this.m_fFurryZoom;
		}
		if ((4 & tribe) > 0)
		{
			return this.m_fElfZoom;
		}
		return 0f;
	}
}
