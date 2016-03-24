using System;
using UnityEngine;
using UnityForms;

public class UnityExceptionHandler : MonoBehaviour
{
	private float _exceptionDelayTime;

	private float _exceptionInvokeTime;

	private int _maxSendMailCount;

	private int _sendMailCount;

	private void Awake()
	{
		Application.RegisterLogCallback(new Application.LogCallback(this.HandleException));
		this._exceptionDelayTime = 20f;
		this._sendMailCount = 0;
		this._maxSendMailCount = 5;
	}

	private void HandleException(string condition, string stackTrace, LogType type)
	{
		if (type != LogType.Exception)
		{
			return;
		}
		if (Time.realtimeSinceStartup < this._exceptionInvokeTime + this._exceptionDelayTime)
		{
			return;
		}
		if (this._maxSendMailCount < this._sendMailCount)
		{
			return;
		}
		this._sendMailCount++;
		this._exceptionInvokeTime = Time.realtimeSinceStartup;
		string text = this.GetDumpInfo();
		text = text + "★ Exception Cause ★ \n" + condition + "\n\n";
		text = text + "★ StackInfo ★ \n" + stackTrace + "\n\n";
		this.SendMailShowMessageBox(text);
	}

	private string GetDumpInfo()
	{
		string text = string.Empty;
		text += "★ App Info ★ \n\n";
		text += "☆ Unity Info ☆ \n";
		text = text + "DeviceModel : " + SystemInfo.deviceModel + " \n";
		text = text + "DeviceName : " + SystemInfo.deviceName + " \n";
		string text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"DeviceType : ",
			SystemInfo.deviceType,
			" \n"
		});
		text = text + "DeviceUniqueID : " + SystemInfo.deviceUniqueIdentifier + " \n";
		text = text + "OS : " + SystemInfo.operatingSystem + " \n";
		text2 = text;
		text = string.Concat(new object[]
		{
			text2,
			"System.DateTime.Now : ",
			DateTime.Now,
			" \n"
		});
		text += "\n\n";
		text += "☆ Hero Info ☆ \n";
		if (NrTSingleton<NrUserDeviceInfo>.Instance != null)
		{
			text = text + "PackageName : " + NrTSingleton<NrUserDeviceInfo>.Instance.GetPackageName() + " \n";
			text = text + "App Version : " + TsPlatform.APP_VERSION_AND + " \n";
		}
		text = text + "CharName : " + this.GetCharName() + " \n";
		text += "\n\n";
		return text + "\n\n\n";
	}

	private string GetCharName()
	{
		if (NrTSingleton<NkCharManager>.Instance == null)
		{
			return string.Empty;
		}
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char == null)
		{
			return string.Empty;
		}
		return @char.GetCharName();
	}

	private void SendMailShowMessageBox(string dumpMsg)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		string title = "Error";
		string text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("437");
		if (string.IsNullOrEmpty(text))
		{
			text = "Send Error Message?";
		}
		msgBoxUI.SetMsg(new YesDelegate(this.Yes), dumpMsg, title, text, eMsgType.MB_OK_CANCEL, 2);
		msgBoxUI.Show();
	}

	public void Yes(object obj)
	{
		if (obj == null)
		{
			return;
		}
		string body = obj as string;
		DumpManager.GetInstance().SendMail("영웅의군단 - UnityException", body);
	}
}
