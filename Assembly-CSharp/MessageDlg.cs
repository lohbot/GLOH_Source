using System;
using UnityForms;

public class MessageDlg : Form
{
	public class MESSAGE_INFO
	{
		public string title = string.Empty;

		public string message = string.Empty;

		public bool bUnread;
	}

	private int i32TotalMessageNum;

	private int i32UnreadMessageNum;

	private int m_iCurrentPage;

	private int m_iTotalPage;

	private Label m_WindwTitle;

	private ListBox m_lbTitle;

	private TextArea m_Message;

	private Label m_Page;

	private Button m_leftButton;

	private Button m_rightButton;

	private MessageDlg.MESSAGE_INFO[] message_info;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Community/DLG_Message", G_ID.MESSAGE_DLG, true);
		this.message_info = new MessageDlg.MESSAGE_INFO[80];
		for (int i = 0; i < 80; i++)
		{
			this.message_info[i] = new MessageDlg.MESSAGE_INFO();
		}
		this.i32TotalMessageNum = 0;
		this.i32UnreadMessageNum = 0;
		this.m_iCurrentPage = 1;
		this.m_iTotalPage = 1;
	}

	public override void InitData()
	{
		this.Update();
	}

	public override void SetComponent()
	{
		this.m_WindwTitle = (base.GetControl("Label_Label32") as Label);
		this.m_lbTitle = (base.GetControl("ListBox_ListBox22") as ListBox);
		this.m_Message = (base.GetControl("TextArea_TextArea69_C") as TextArea);
		this.m_Page = (base.GetControl("Label_Label62") as Label);
		this.m_leftButton = (base.GetControl("Button_Button59") as Button);
		this.m_rightButton = (base.GetControl("Button_Button59_C") as Button);
		this.m_leftButton.SetValueChangedDelegate(new EZValueChangedDelegate(this.OnLeftPage));
		this.m_rightButton.SetValueChangedDelegate(new EZValueChangedDelegate(this.OnRightPage));
		this.m_lbTitle.SetValueChangedDelegate(new EZValueChangedDelegate(this.OnSelectMessage));
		this.m_lbTitle.ColumnNum = 1;
		this.m_lbTitle.LineHeight = 31f;
		this.m_lbTitle.UseColumnRect = true;
		this.m_lbTitle.SetColumnRect(0, 17, 0, 160, 31);
		this.m_Message.spriteText.multiline = true;
	}

	public override void CloseForm(IUIObject obj)
	{
		this.Hide();
	}

	public void AddMessage(string title, string message)
	{
		for (int i = 78; i >= 0; i--)
		{
			this.message_info[i + 1].message = this.message_info[i].message;
			this.message_info[i + 1].title = this.message_info[i].title;
			this.message_info[i + 1].bUnread = this.message_info[i].bUnread;
		}
		this.message_info[0].title = title;
		this.message_info[0].message = message;
		this.message_info[0].bUnread = true;
		this.i32TotalMessageNum = Math.Min(this.i32TotalMessageNum + 1, 80);
		this.i32UnreadMessageNum++;
		this.Update();
	}

	public void Update_WindowTitle()
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("456");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"Count",
			this.i32UnreadMessageNum.ToString()
		});
		this.m_WindwTitle.Text = empty;
	}

	public override void Update()
	{
		string color = NrTSingleton<CTextParser>.Instance.GetBaseColor();
		string color2 = NrTSingleton<CTextParser>.Instance.GetBaseColor();
		color = NrTSingleton<CTextParser>.Instance.GetTextColor("235");
		color2 = NrTSingleton<CTextParser>.Instance.GetTextColor("216");
		this.m_lbTitle.Clear();
		int num = (this.m_iCurrentPage - 1) * 8;
		for (int i = 0; i < 8; i++)
		{
			ListItem listItem = new ListItem();
			if (this.message_info[num + i].bUnread)
			{
				listItem.SetColumnStr(0, this.message_info[num + i].title, color2);
			}
			else
			{
				listItem.SetColumnStr(0, this.message_info[num + i].title, color);
			}
			this.m_lbTitle.Add(listItem);
		}
		this.m_lbTitle.RepositionItems();
		if (this.i32TotalMessageNum == 0)
		{
			this.m_iTotalPage = 1;
		}
		else
		{
			this.m_iTotalPage = this.i32TotalMessageNum / 8;
			if (this.i32TotalMessageNum % 8 > 0)
			{
				this.m_iTotalPage++;
			}
		}
		this.m_Page.Text = this.m_iCurrentPage + "/" + this.m_iTotalPage;
		this.Update_WindowTitle();
	}

	private void OnLeftPage(IUIObject obj)
	{
		this.m_iCurrentPage = Math.Max(this.m_iCurrentPage - 1, 1);
		this.Update();
	}

	private void OnRightPage(IUIObject obj)
	{
		this.m_iCurrentPage = Math.Min(this.m_iCurrentPage + 1, this.m_iTotalPage);
		this.Update();
	}

	private void OnSelectMessage(IUIObject obj)
	{
	}
}
