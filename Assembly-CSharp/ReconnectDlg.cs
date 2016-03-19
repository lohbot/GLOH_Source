using System;
using UnityEngine;
using UnityForms;

public class ReconnectDlg : Form
{
	private DrawTexture m_DT_BG;

	private DrawTexture m_DT_NpcImg;

	private Label m_LB_BubbleText;

	private Button m_BT_Ok;

	private Button m_BT_Exit;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Login/DLG_ReConnect", G_ID.RECONNECT_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_DT_BG = (base.GetControl("DrawTexture_ReConnectBG") as DrawTexture);
		this.m_DT_NpcImg = (base.GetControl("DrawTexture_NPCIMG") as DrawTexture);
		this.m_LB_BubbleText = (base.GetControl("Label_BubbleText") as Label);
		this.m_BT_Ok = (base.GetControl("Button_OK") as Button);
		this.m_BT_Exit = (base.GetControl("Button_exit") as Button);
		this.m_BT_Ok.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickBT_Ok));
		this.m_BT_Exit.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickBT_Exit));
		string str = NrTSingleton<UIDataManager>.Instance.FilePath + "Texture/Loading/loading_BG";
		Texture2D texture = (Texture2D)CResources.Load(str + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		this.m_DT_BG.SetTexture(texture);
		this.m_DT_NpcImg.SetTextureFromBundle(string.Format("UI/Soldier/256/mine_256", new object[0]));
		this.m_LB_BubbleText.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2563"));
		this.m_BT_Ok.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2565"));
		this.m_BT_Exit.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("216"));
		if (TsPlatform.IsIPhone)
		{
			this.m_BT_Exit.Visible = false;
		}
		base.Draggable = false;
		base.SetSize(GUICamera.width, GUICamera.height);
		this.m_DT_BG.SetSize(GUICamera.width, GUICamera.height);
		base.SetScreenCenter();
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DLG_LOADINGPAGE))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_LOADINGPAGE);
		}
	}

	private void On_ClickBT_Ok(IUIObject a_cObject)
	{
		Debug.LogWarning("ReLogin");
		NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
	}

	private void On_ClickBT_Exit(IUIObject a_cObject)
	{
		EventTriggerMapManager.Instance.Claer();
		NrMobileAuthSystem.Instance.RequestLogout = true;
		NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
		NrTSingleton<NrMainSystem>.Instance.QuitGame();
	}
}
