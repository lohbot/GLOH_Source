using System;

public interface IWebViewListener
{
	bool OnWebCall(string loadurl);

	void OnAlertViewConfirmed(string message);

	void OnFinishCall(string message);
}
