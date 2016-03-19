using System;

public interface NrMobileAuth
{
	void Init();

	bool Login(string id, string pw);

	bool IsLogin(Action ShowLoginFunc);

	void RequestOauthToken(string id, string pw, Action<string> ResultFunc);

	bool IsGuestLogin(Action ErrorGuestLoginFunc);

	bool IsGuest();

	void DeleteAuthInfo();

	void RequestLogin();

	void RequestPlatformLogin();
}
