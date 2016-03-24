using System;
using TsLibs;

public class NkTableCameraSettings : NrTableBase
{
	public NkTableCameraSettings() : base(CDefinePath.CAMERA_SETTINGS_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			CAMERASETTING_DATA cAMERASETTING_DATA = new CAMERASETTING_DATA();
			cAMERASETTING_DATA.SetData(data);
			cAMERASETTING_DATA.m_eType = NrTSingleton<NkCameraSettingsManager>.Instance.GetSceneEnum(cAMERASETTING_DATA.szSceneKind);
			NrTSingleton<NkCameraSettingsManager>.Instance.AddCameraSettingData(cAMERASETTING_DATA.m_eType, cAMERASETTING_DATA);
		}
		return true;
	}
}
