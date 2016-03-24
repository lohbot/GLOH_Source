using System;
using UnityForms;

public class Batch_Chat_DLG : Form
{
	private ChatLabel cl_party;

	private ChatLabel cl_guild;

	private Button bt_ChatChange;

	private Button bt_ChatChange2;

	private Label lb_Chat;

	private Box box_chat;

	private CHAT_TYPE selectTab = CHAT_TYPE.MYTHRAID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "BabelTower/dlg_batch_chat", G_ID.BATCH_CAHT_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
		if (plunderSolListDlg != null)
		{
			base.SetLocation(plunderSolListDlg.GetLocationX() + plunderSolListDlg.GetSizeX(), base.GetLocationY(), base.GetLocation().z);
		}
		this.cl_party = (base.GetControl("CL_Party") as ChatLabel);
		this.cl_party.RemoveBoxCollider();
		this.cl_guild = (base.GetControl("CL_Guild") as ChatLabel);
		this.cl_guild.RemoveBoxCollider();
		this.bt_ChatChange = (base.GetControl("BT_ChatChange") as Button);
		this.bt_ChatChange2 = (base.GetControl("BT_ChatChange2") as Button);
		this.bt_ChatChange.Click = new EZValueChangedDelegate(this.OnClickChangeChatType);
		this.bt_ChatChange2.Click = new EZValueChangedDelegate(this.OnClickChangeChatType);
		this.lb_Chat = (base.GetControl("LB_Chat") as Label);
		this.lb_Chat.Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2138");
		this.box_chat = (base.GetControl("BOX_Chat") as Box);
		this.box_chat.Visible = false;
		base.SetShowLayer(2, false);
	}

	public CHAT_TYPE GetChatType()
	{
		return this.selectTab;
	}

	public void PushMsg(string name, string msg, string color)
	{
		this.cl_party.PushText(name, msg, color, null);
	}

	public void PushMsg(string name, string msg, CHAT_TYPE _chatType)
	{
		if (_chatType == CHAT_TYPE.MYTHRAID)
		{
			this.cl_party.PushText(name, msg, ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL), null);
		}
		else if (_chatType == CHAT_TYPE.GUILD)
		{
			this.cl_guild.PushText(name, msg, ChatManager.GetChatColorKey(CHAT_TYPE.GUILD), null);
		}
		if (this.selectTab != _chatType)
		{
			this.box_chat.Visible = true;
		}
	}

	public void OnClickChangeChatType(IUIObject obj)
	{
		if (this.selectTab == CHAT_TYPE.MYTHRAID)
		{
			this.selectTab = CHAT_TYPE.GUILD;
			this.lb_Chat.Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.GUILD)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2139");
			base.SetShowLayer(1, false);
			base.SetShowLayer(2, true);
			this.cl_party.ScrollPosition = 1f;
		}
		else
		{
			this.selectTab = CHAT_TYPE.MYTHRAID;
			this.lb_Chat.Text = NrTSingleton<CTextParser>.Instance.GetTextColor(ChatManager.GetChatColorKey(CHAT_TYPE.NORMAL)) + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2138");
			base.SetShowLayer(1, true);
			base.SetShowLayer(2, false);
			this.cl_party.ScrollPosition = 1f;
		}
		this.box_chat.Visible = false;
	}
}
