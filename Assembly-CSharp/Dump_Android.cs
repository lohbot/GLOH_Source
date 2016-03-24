using System;
using UnityEngine;

public class Dump_Android : Dump_Base
{
	private readonly string _andoirdDumpHnalderName = string.Empty;

	private AndroidJavaObject _dumpAndroidObject;

	private readonly string _androidEmailSenderName = string.Empty;

	private AndroidJavaObject _emailSendManagerObject;

	private readonly string _emailAuthID = string.Empty;

	private readonly string _emailAuthPassword = string.Empty;

	private readonly string _emailSender = string.Empty;

	private readonly string _emailRecipient = string.Empty;

	public Dump_Android()
	{
		this._andoirdDumpHnalderName = "com.ndoors.dumphandler.DumpHandlerManager";
		this.RegistAndroidObject(this._andoirdDumpHnalderName, ref this._dumpAndroidObject);
		this._androidEmailSenderName = "com.ndoors.gmailsender.GMailSendManager";
		this.RegistAndroidObject(this._androidEmailSenderName, ref this._emailSendManagerObject);
		this._emailAuthID = "ndoors.dev@gmail.com";
		this._emailAuthPassword = "qwe123!@#";
		this._emailSender = "ndoors.dev@gmail.com";
		this._emailRecipient = "ndoors.dev@gmail.com";
	}

	public override void RegistDumpHandler()
	{
		if (this._dumpAndroidObject == null)
		{
			return;
		}
		this._dumpAndroidObject.Call("RegistDumpHandler", new object[]
		{
			this._emailAuthID,
			this._emailAuthPassword,
			this._emailSender,
			this._emailRecipient
		});
	}

	public override void ForceCrash()
	{
		if (this._dumpAndroidObject == null)
		{
			return;
		}
		this._dumpAndroidObject.Call("ForceCrash", new object[0]);
	}

	public override void SendMail(string subject, string body)
	{
		if (this._dumpAndroidObject == null)
		{
			return;
		}
		this._emailSendManagerObject.Call("SendMail", new object[]
		{
			this._emailAuthID,
			this._emailAuthPassword,
			subject,
			body,
			this._emailSender,
			this._emailRecipient
		});
	}

	private void RegistAndroidObject(string objectName, ref AndroidJavaObject androidObject)
	{
		try
		{
			androidObject = new AndroidJavaObject(objectName, new object[0]);
		}
		catch (Exception ex)
		{
			Debug.LogError("ERROR, Dump_Android.cs, RegistAndroidObject(), androidObject is Null : " + objectName);
			Debug.LogError("Exception : " + ex.ToString());
		}
	}
}
