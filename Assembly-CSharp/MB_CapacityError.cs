using System;

internal class MB_CapacityError : MessageBox
{
	public MB_CapacityError()
	{
		base.Title = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8");
		base.Message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("6");
		base.OK = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("8");
	}

	public override void OnOK()
	{
		NrTSingleton<NrMainSystem>.Instance.QuitGame(false);
	}
}
