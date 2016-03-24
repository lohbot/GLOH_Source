using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Main_UI_SystemMessage : Form
{
	private class cMessage
	{
		public object m_LinkControl;

		public float m_fStartTime;

		public float m_fStepTime;
	}

	private const float HEIGHTPECENT = 0.3f;

	private const int HEIGHT = 38;

	private const int MAX_TEXT = 3;

	private Box[] m_bxData;

	private List<Main_UI_SystemMessage.cMessage> m_TextList = new List<Main_UI_SystemMessage.cMessage>();

	private static bool m_bLiveMessage;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFile(ref form, "Message/DLG_SystemMessage", G_ID.DLG_SYSTEMMESSAGE, false);
		this.m_bxData = new Box[3];
		for (int i = 0; i < 3; i++)
		{
			instance.CreateControl(ref this.m_bxData[i], "bx_Message" + i);
			this.m_bxData[i].Visible = false;
			this.m_bxData[i].Text = string.Empty;
			this.m_bxData[i].UpdateText = true;
			BoxCollider component = this.m_bxData[i].GetComponent<BoxCollider>();
			UnityEngine.Object.DestroyImmediate(component);
		}
		base.SetLocation(0f, 0f);
		base.SetSize(GUICamera.width, GUICamera.height);
		base.Draggable = false;
		base.CheckMouseEvent = false;
		base.DonotDepthChange(4f);
	}

	public override void Update()
	{
		if (Main_UI_SystemMessage.m_bLiveMessage)
		{
			return;
		}
		for (int i = 0; i < this.m_TextList.Count; i++)
		{
			Box box = (Box)this.m_TextList[i].m_LinkControl;
			if (Time.time - this.m_TextList[i].m_fStepTime > 2f)
			{
				this.m_TextList[i].m_fStepTime = Time.time;
				box.SetLocation(box.GetLocation().x, box.GetLocationY() - 38f);
			}
			if (Time.time - this.m_TextList[i].m_fStartTime >= 5f)
			{
				box.Text = string.Empty;
				box.Visible = false;
				this.m_TextList.RemoveAt(i);
			}
		}
		if (this.m_TextList.Count == 0)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_SYSTEMMESSAGE);
		}
	}

	public void Add(string Message, SYSTEM_MESSAGE_TYPE MessageType)
	{
		Main_UI_SystemMessage.cMessage cMessage = new Main_UI_SystemMessage.cMessage();
		if (this.m_TextList.Count == 3)
		{
			Box box = (Box)this.m_TextList[0].m_LinkControl;
			box.Text = string.Empty;
			this.m_TextList.RemoveAt(0);
		}
		for (int i = 0; i < 3; i++)
		{
			if (this.m_bxData[i].Text == string.Empty)
			{
				string @string = NrTSingleton<UIDataManager>.Instance.GetString(this.GetColor(MessageType), Message);
				this.m_bxData[i].Text = @string;
				float num = this.m_bxData[i].spriteText.TotalWidth + 20f;
				this.m_bxData[i].SetSize(num, 38f);
				this.m_bxData[i].Text = @string;
				this.m_bxData[i].SetLocation(GUICamera.width / 2f - num / 2f, (float)((int)(GUICamera.height * 0.3f)));
				this.m_bxData[i].Visible = true;
				this.m_bxData[i].AlphaValue = 1f;
				this.m_bxData[i].Text = @string;
				cMessage.m_LinkControl = this.m_bxData[i];
				break;
			}
		}
		if (cMessage.m_LinkControl != null)
		{
			if (this.m_TextList.Count == 1)
			{
				Box box2 = (Box)this.m_TextList[0].m_LinkControl;
				if (Time.time - this.m_TextList[0].m_fStartTime < 2f)
				{
					box2.SetLocation(box2.GetLocation().x, box2.GetLocationY() - 38f);
					this.m_TextList[0].m_fStartTime = Time.time - 2f;
					this.m_TextList[0].m_fStepTime = Time.time - 0.2f;
					box2.AlphaValue = 0.6f;
				}
			}
			else if (this.m_TextList.Count == 2)
			{
				Box box3 = (Box)this.m_TextList[0].m_LinkControl;
				box3.SetLocation(box3.GetLocation().x, (float)((int)(GUICamera.height * 0.3f) - 76));
				this.m_TextList[0].m_fStartTime = Time.time - 4.1f;
				this.m_TextList[0].m_fStepTime = Time.time - 0.2f;
				box3.AlphaValue = 0.3f;
				Box box4 = (Box)this.m_TextList[1].m_LinkControl;
				box4.SetLocation(box4.GetLocation().x, (float)((int)(GUICamera.height * 0.3f) - 38));
				this.m_TextList[1].m_fStartTime = Time.time - 2.1f;
				this.m_TextList[1].m_fStepTime = Time.time - 0.2f;
				box4.AlphaValue = 0.6f;
			}
			cMessage.m_fStartTime = Time.time;
			cMessage.m_fStepTime = Time.time;
			this.m_TextList.Add(cMessage);
		}
	}

	private string GetColor(SYSTEM_MESSAGE_TYPE type)
	{
		CTextParser instance = NrTSingleton<CTextParser>.Instance;
		string result = instance.GetBaseColor();
		switch (type)
		{
		case SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE:
			result = instance.GetTextColor("1107");
			break;
		case SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE:
			result = instance.GetTextColor("1305");
			break;
		case SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE:
			result = instance.GetTextColor("1106");
			break;
		case SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE:
			result = instance.GetTextColor("1302");
			break;
		case SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE:
			result = instance.GetTextColor("1105");
			break;
		case SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE:
			result = instance.GetTextColor("1306");
			break;
		case SYSTEM_MESSAGE_TYPE.QUEST_COMLPETE:
			result = instance.GetTextColor("1105");
			break;
		case SYSTEM_MESSAGE_TYPE.BATTLE_SHOWTEXT_MODE4:
			result = instance.GetTextColor("1602");
			break;
		case SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN:
			result = instance.GetTextColor("2002");
			break;
		case SYSTEM_MESSAGE_TYPE.BATTLE_RADIO_ALARM:
			result = instance.GetTextColor("1002");
			break;
		default:
			result = instance.GetTextColor("1106");
			break;
		}
		return result;
	}

	public void Add(string Message, SYSTEM_MESSAGE_TYPE MessageType, string strColor)
	{
		Main_UI_SystemMessage.cMessage cMessage = new Main_UI_SystemMessage.cMessage();
		CTextParser instance = NrTSingleton<CTextParser>.Instance;
		if (this.m_TextList.Count == 3)
		{
			Box box = (Box)this.m_TextList[0].m_LinkControl;
			box.Text = string.Empty;
			this.m_TextList.RemoveAt(0);
		}
		for (int i = 0; i < 3; i++)
		{
			if (this.m_bxData[i].Text == string.Empty)
			{
				string @string = NrTSingleton<UIDataManager>.Instance.GetString(instance.GetTextColor(strColor), Message);
				this.m_bxData[i].Text = @string;
				float num = this.m_bxData[i].spriteText.TotalWidth + 20f;
				this.m_bxData[i].SetSize(num, 38f);
				this.m_bxData[i].Text = @string;
				this.m_bxData[i].SetLocation(GUICamera.width / 2f - num / 2f, (float)((int)(GUICamera.height * 0.3f)));
				this.m_bxData[i].Visible = true;
				this.m_bxData[i].AlphaValue = 1f;
				this.m_bxData[i].Text = @string;
				cMessage.m_LinkControl = this.m_bxData[i];
				break;
			}
		}
		if (cMessage.m_LinkControl != null)
		{
			if (this.m_TextList.Count == 1)
			{
				Box box2 = (Box)this.m_TextList[0].m_LinkControl;
				if (Time.time - this.m_TextList[0].m_fStartTime < 2f)
				{
					box2.SetLocation(box2.GetLocation().x, box2.GetLocationY() - 38f);
					this.m_TextList[0].m_fStartTime = Time.time - 2f;
					this.m_TextList[0].m_fStepTime = Time.time - 0.2f;
					box2.AlphaValue = 0.6f;
				}
			}
			else if (this.m_TextList.Count == 2)
			{
				Box box3 = (Box)this.m_TextList[0].m_LinkControl;
				box3.SetLocation(box3.GetLocation().x, (float)((int)(GUICamera.height * 0.3f) - 76));
				this.m_TextList[0].m_fStartTime = Time.time - 4.1f;
				this.m_TextList[0].m_fStepTime = Time.time - 0.2f;
				box3.AlphaValue = 0.3f;
				Box box4 = (Box)this.m_TextList[1].m_LinkControl;
				box4.SetLocation(box4.GetLocation().x, (float)((int)(GUICamera.height * 0.3f) - 38));
				this.m_TextList[1].m_fStartTime = Time.time - 2.1f;
				this.m_TextList[1].m_fStepTime = Time.time - 0.2f;
				box4.AlphaValue = 0.6f;
			}
			cMessage.m_fStartTime = Time.time;
			cMessage.m_fStepTime = Time.time;
			this.m_TextList.Add(cMessage);
		}
	}

	public static void ADDMessage(string Message)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DLG_LOADINGPAGE))
		{
			return;
		}
		if (Main_UI_SystemMessage.m_bLiveMessage)
		{
			return;
		}
		Main_UI_SystemMessage main_UI_SystemMessage = (Main_UI_SystemMessage)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_SYSTEMMESSAGE);
		main_UI_SystemMessage.Add(Message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		main_UI_SystemMessage.Show();
	}

	public static void ADDMessage(string Message, SYSTEM_MESSAGE_TYPE Type)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DLG_LOADINGPAGE))
		{
			return;
		}
		if (Main_UI_SystemMessage.m_bLiveMessage)
		{
			return;
		}
		Main_UI_SystemMessage main_UI_SystemMessage = (Main_UI_SystemMessage)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_SYSTEMMESSAGE);
		if (Main_UI_SystemMessage.m_bLiveMessage)
		{
			main_UI_SystemMessage.TopMost = false;
		}
		main_UI_SystemMessage.Add(Message, Type);
		main_UI_SystemMessage.Show();
		main_UI_SystemMessage.DonotDepthChange(main_UI_SystemMessage.GetLocation().z);
	}

	public static void ADDMessage(string Message, SYSTEM_MESSAGE_TYPE Type, string strColor)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DLG_LOADINGPAGE))
		{
			return;
		}
		if (Main_UI_SystemMessage.m_bLiveMessage)
		{
			return;
		}
		Main_UI_SystemMessage main_UI_SystemMessage = (Main_UI_SystemMessage)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_SYSTEMMESSAGE);
		if (Main_UI_SystemMessage.m_bLiveMessage)
		{
			main_UI_SystemMessage.TopMost = false;
		}
		main_UI_SystemMessage.Add(Message, Type, strColor);
		main_UI_SystemMessage.Show();
		main_UI_SystemMessage.DonotDepthChange(main_UI_SystemMessage.GetLocation().z);
	}

	public static void ClearText()
	{
		Main_UI_SystemMessage.m_bLiveMessage = false;
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_SYSTEMMESSAGE);
	}

	public static void CloseUI()
	{
		if (!Main_UI_SystemMessage.m_bLiveMessage)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_SYSTEMMESSAGE);
		}
	}
}
