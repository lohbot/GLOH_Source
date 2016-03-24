using System;

internal class MB_WifiWarning : MessageBox
{
	public MB_WifiWarning()
	{
		base.Title = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8");
		base.Message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("5");
		base.OK = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8");
		base.CANCEL = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("9");
	}

	public override void OnOK()
	{
		NPatchLauncherHandler_forInGame._isWifiOK = true;
		NPatchLauncherHandler_forInGame._isClosedMsgBox = true;
	}

	public override void OnCancel()
	{
		NPatchLauncherHandler_forInGame._isClosedMsgBox = true;
		NrTSingleton<NrMainSystem>.Instance.QuitGame(false);
	}
}
