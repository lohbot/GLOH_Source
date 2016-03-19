using System;
using UnityEngine;

internal class MB_NeedNewApp : MessageBox
{
	public MB_NeedNewApp()
	{
		base.Title = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("2");
		base.Message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("1");
	}

	public override void OnOK()
	{
		string url = string.Format("http://{0}/mobile/updateurl.aspx?code={1}", NrGlobalReference.strWebPageDomain, NrGlobalReference.MOBILEID);
		Application.OpenURL(url);
	}
}
