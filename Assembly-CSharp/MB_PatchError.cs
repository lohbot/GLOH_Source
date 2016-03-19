using System;

internal class MB_PatchError : MessageBox
{
	public MB_PatchError()
	{
		base.Title = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("2");
		base.Message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("39");
		base.OK = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8");
		base.CANCEL = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("9");
	}

	public override void OnOK()
	{
		TsLog.LogError("StageNPatch - CHANGE", new object[0]);
		NrTSingleton<NrMainSystem>.Instance.m_ReLogin = false;
		NrTSingleton<NrMainSystem>.Instance.m_Login_BG = true;
		NrTSingleton<NrGlobalReference>.Instance.localWWW = true;
		NrTSingleton<NrMainSystem>.Instance.ReLogin(false);
	}
}
