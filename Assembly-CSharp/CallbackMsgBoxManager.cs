using System;
using UnityForms;

public class CallbackMsgBoxManager : NrTSingleton<CallbackMsgBoxManager>
{
	private CallbackMsgBoxManager()
	{
	}

	public void OnMsgBox(string msg)
	{
		MsgBoxUI msgBoxUI = (MsgBoxUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG);
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(null, null, "확인", msg, eMsgType.MB_OK, 2);
	}
}
