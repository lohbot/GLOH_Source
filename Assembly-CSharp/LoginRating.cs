using SERVICE;
using System;
using UnityForms;

public class LoginRating : Form
{
	private Label m_pkLableAPKVersion;

	private DrawTexture m_txRating1;

	private DrawTexture m_txRating2;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = false;
		instance.LoadFileAll(ref form, "Login/DLG_Login_Rating", G_ID.LOGINRATING_DLG, false);
		base.DonotDepthChange(96f);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_pkLableAPKVersion = (base.GetControl("Label_Version") as Label);
		string empty = string.Empty;
		if (TsPlatform.IsAndroid)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("27"),
				"version",
				TsPlatform.APP_VERSION_AND
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("27"),
				"version",
				TsPlatform.APP_VERSION_IOS
			});
		}
		this.m_pkLableAPKVersion.SetText("[#66cbff]" + empty);
		this.m_txRating1 = (base.GetControl("DT_Rating1") as DrawTexture);
		this.m_txRating2 = (base.GetControl("DT_Rating2") as DrawTexture);
		eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
		if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORTSTORE || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_BANDNAVER || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORNAVER || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORKAKAO || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_KORKAKAO)
		{
			this.m_txRating1.SetTexture("Win_I_Deliberation04");
		}
		else
		{
			this.m_txRating1.SetTexture("Win_I_Deliberation01");
		}
		this.m_txRating1.Visible = false;
		this.m_txRating2.Visible = false;
	}
}
