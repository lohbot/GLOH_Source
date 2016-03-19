using System;
using UnityForms;

public class NewGuildMainSelectDlg : Form
{
	private DrawTexture m_dtBG;

	private DrawTexture m_dtJoin;

	private DrawTexture m_dtCreate;

	private Button m_btJoin;

	private Button m_btCrate;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/DLG_Newguild_MainSelect", G_ID.NEWGUILD_MAINSELECT_DLG, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_dtBG = (base.GetControl("DT_SubBG") as DrawTexture);
		this.m_dtBG.SetTextureFromBundle("UI/Etc/GuildBG");
		this.m_dtJoin = (base.GetControl("DT_applicationBG1") as DrawTexture);
		this.m_dtJoin.SetTextureFromBundle("UI/Etc/GuildImg01");
		this.m_dtCreate = (base.GetControl("DT_createBG") as DrawTexture);
		this.m_dtCreate.SetTextureFromBundle("UI/Etc/GuildImg02");
		this.m_btJoin = (base.GetControl("BT_application") as Button);
		this.m_btJoin.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickJoin));
		this.m_btCrate = (base.GetControl("BT_create") as Button);
		this.m_btCrate.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCreate));
		base.SetScreenCenter();
	}

	public void ClickJoin(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_LIST_DLG);
		base.CloseNow();
	}

	public void ClickCreate(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_CREATE_DLG);
		base.CloseNow();
	}
}
