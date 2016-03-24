using System;
using System.Collections.Generic;
using TsLibs;

public class NrEmulator_Table : NrTableBase
{
	private List<string> m_IncludeList = new List<string>();

	private List<string> m_ExceptionList = new List<string>();

	private List<string> m_ActiveList = new List<string>();

	public NrEmulator_Table() : base(CDefinePath.EMULATOR_INFO_URL)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			EmulatorData emulatorData = new EmulatorData();
			emulatorData.SetData(data);
			switch (emulatorData.type)
			{
			case 0:
				this.m_IncludeList.Add(emulatorData.package);
				break;
			case 1:
				this.m_ActiveList.Add(emulatorData.package);
				break;
			case 2:
				this.m_ExceptionList.Add(emulatorData.package);
				break;
			}
		}
		NrTSingleton<NrUserDeviceInfo>.Instance.SetIncludeEmulator(this.m_IncludeList);
		NrTSingleton<NrUserDeviceInfo>.Instance.SetActiveEmulator(this.m_ActiveList);
		NrTSingleton<NrUserDeviceInfo>.Instance.SetExceptionEmulator(this.m_ExceptionList);
		return true;
	}
}
