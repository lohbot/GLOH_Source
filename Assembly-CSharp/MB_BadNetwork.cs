using System;

internal class MB_BadNetwork : MessageBox
{
	public MB_BadNetwork()
	{
		base.Title = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8");
		base.Message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("4");
		base.OK = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("7");
	}

	public override void OnOK()
	{
		NrTSingleton<NrMainSystem>.Instance.QuitGame();
	}
}
