using System;
using UnityEngine;
using UnityForms;

public class UI_RightClickMenu : Form
{
	public struct callbackData
	{
		public bool bCloseRightUI;

		public CRightClickMenu._OnClickMenu callback;

		public object data;
	}

	private DrawTexture m_DrawTexture_ListBG1;

	private DrawTexture m_DrawTexture_Name1;

	private Label m_Label_Label4;

	private DrawTexture[] m_DrawTexture_Line;

	private ListBox[] m_ListBox_ListBox;

	private int[] m_ItemsCount;

	private CRightClickMenu.TYPE m_type;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "RightClick/DLG_RightClick", G_ID.DLG_RIGHTCLICK_MENU, true);
		base.InteractivePanel.draggable = false;
	}

	public override void InitData()
	{
	}

	public override void SetComponent()
	{
		this.m_ListBox_ListBox = new ListBox[3];
		this.m_ItemsCount = new int[3];
		this.m_DrawTexture_Line = new DrawTexture[2];
		this.m_DrawTexture_ListBG1 = (base.GetControl("DrawTexture_ListBG1") as DrawTexture);
		this.m_DrawTexture_Name1 = (base.GetControl("DrawTexture_Name1") as DrawTexture);
		this.m_ListBox_ListBox[0] = (base.GetControl("ListBox_ListBox0") as ListBox);
		this.m_ListBox_ListBox[0].Reserve = false;
		this.m_ListBox_ListBox[1] = (base.GetControl("ListBox_ListBox1") as ListBox);
		this.m_ListBox_ListBox[1].Reserve = false;
		this.m_ListBox_ListBox[2] = (base.GetControl("ListBox_ListBox2") as ListBox);
		this.m_ListBox_ListBox[2].Reserve = false;
		this.m_Label_Label4 = (base.GetControl("Label_Label4") as Label);
		this.m_DrawTexture_Line[0] = (base.GetControl("DrawTexture_Line1") as DrawTexture);
		this.m_DrawTexture_Line[1] = (base.GetControl("DrawTexture_Line2") as DrawTexture);
		for (int i = 0; i < 3; i++)
		{
			if (this.m_ListBox_ListBox[i].slider != null)
			{
				UnityEngine.Object.Destroy(this.m_ListBox_ListBox[i].slider.gameObject);
			}
			this.m_ListBox_ListBox[i].OffsetX = 0f;
			if (TsPlatform.IsMobile)
			{
				this.m_ListBox_ListBox[i].LineHeight = 80f;
				this.m_ListBox_ListBox[i].ColumnNum = 1;
				this.m_ListBox_ListBox[i].SetColumnWidth((int)this.m_ListBox_ListBox[i].GetSize().x, 0, 0, 0, 0, 32f);
			}
			else
			{
				this.m_ListBox_ListBox[i].LineHeight = 20f;
				this.m_ListBox_ListBox[i].ColumnNum = 1;
				this.m_ListBox_ListBox[i].SetColumnWidth((int)this.m_ListBox_ListBox[i].GetSize().x, 0, 0, 0, 0);
			}
			this.m_ListBox_ListBox[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ListBoxSelect));
			this.m_ListBox_ListBox[i].AddLongTapDelegate(new EZValueChangedDelegate(this.ListBoxSelect));
		}
		if (TsPlatform.IsWeb || TsPlatform.IsEditor)
		{
			float x = NkInputManager.mousePosition.x;
			float num = GUICamera.height - NkInputManager.mousePosition.y;
			base.SetLocation((float)((int)x), (float)((int)num));
		}
		else
		{
			Vector3 vector = GUICamera.ScreenToGUIPoint(new Vector3(NkInputManager.GetTouch(0).position.x, GUICamera.height - NkInputManager.GetTouch(0).position.y, 1f));
			base.SetLocation((float)((int)vector.x), (float)((int)vector.y));
		}
	}

	public override void Update()
	{
		NrTSingleton<CRightClickMenu>.Instance.Update();
	}

	public override void OnClose()
	{
	}

	public void SetControl(CRightClickMenu.TYPE type)
	{
		this.m_type = type;
		switch (type)
		{
		case CRightClickMenu.TYPE.SIMPLE_SECTION_1:
			this.SetSimple_Section_1();
			break;
		case CRightClickMenu.TYPE.SIMPLE_SECTION_2:
			this.SetSimple_Section_2();
			break;
		case CRightClickMenu.TYPE.SIMPLE_SECTION_3:
			this.SetSimple_Section_3();
			break;
		case CRightClickMenu.TYPE.NAME_SECTION_1:
			this.SetName_Section_1();
			break;
		case CRightClickMenu.TYPE.NAME_SECTION_2:
			this.SetName_Section_2();
			break;
		case CRightClickMenu.TYPE.NAME_SECTION_3:
			this.SetName_Section_3();
			break;
		}
	}

	public Vector2 GetSelectList_RightUp_Position(int section)
	{
		Vector2 zero = Vector2.zero;
		if (section < 0 || section >= 3)
		{
			return zero;
		}
		zero.x = base.GetLocation().x + base.GetSize().x;
		zero.y = base.GetLocationY() + this.m_ListBox_ListBox[section].GetLocationY() + (float)(this.m_ListBox_ListBox[section].SelectIndex - 1) * this.m_ListBox_ListBox[section].LineHeight;
		return zero;
	}

	public void SetName(string charname)
	{
		if (TsPlatform.IsMobile && this.m_type == CRightClickMenu.TYPE.NAME_SECTION_1)
		{
			this.m_Label_Label4.MaxWidth = base.GetSizeX();
		}
		this.m_Label_Label4.SetText(charname);
		if (TsPlatform.IsMobile && this.m_type == CRightClickMenu.TYPE.NAME_SECTION_1)
		{
			this.m_Label_Label4.SetCharacterSize(26f);
		}
		Vector2 zero = Vector2.zero;
		zero.x = base.GetSizeX();
		zero.y = 10f + this.m_Label_Label4.spriteText.TotalHeight;
		this.m_DrawTexture_Name1.SetSize(zero.x, zero.y);
	}

	public string GetTileCharName()
	{
		return this.m_Label_Label4.Text;
	}

	public void AddListMenu(int section, string name, string color, bool bCloseUIAfterCallback, CRightClickMenu._OnClickMenu callback)
	{
		if (section < 0 || section >= 3)
		{
			return;
		}
		ListItem listItem = new ListItem();
		listItem.SetColumnStr(0, name, color);
		listItem.Key = new UI_RightClickMenu.callbackData
		{
			bCloseRightUI = bCloseUIAfterCallback,
			callback = callback
		};
		this.m_ListBox_ListBox[section].Add(listItem);
		this.m_ItemsCount[section]++;
	}

	public void AddListMenu(int section, string name, string color, bool bCloseUIAfterCallback, CRightClickMenu._OnClickMenu callback, object data)
	{
		if (section < 0 || section >= 3)
		{
			return;
		}
		ListItem listItem = new ListItem();
		listItem.SetColumnStr(0, name, color);
		listItem.Key = new UI_RightClickMenu.callbackData
		{
			bCloseRightUI = bCloseUIAfterCallback,
			callback = callback,
			data = data
		};
		this.m_ListBox_ListBox[section].Add(listItem);
		this.m_ItemsCount[section]++;
	}

	public void Rearrange()
	{
		for (int i = 0; i < 3; i++)
		{
			this.m_ListBox_ListBox[i].SetSize(this.m_ListBox_ListBox[i].GetSize().x, this.m_ListBox_ListBox[i].LineHeight * (float)this.m_ItemsCount[i]);
			this.m_ListBox_ListBox[i].RepositionItems();
		}
		switch (this.m_type)
		{
		case CRightClickMenu.TYPE.SIMPLE_SECTION_1:
		case CRightClickMenu.TYPE.NAME_SECTION_1:
			this.Rearrange_Section1();
			break;
		case CRightClickMenu.TYPE.SIMPLE_SECTION_2:
		case CRightClickMenu.TYPE.NAME_SECTION_2:
			this.Rearrange_Section2();
			break;
		case CRightClickMenu.TYPE.SIMPLE_SECTION_3:
		case CRightClickMenu.TYPE.NAME_SECTION_3:
			this.Rearrange_Section3();
			break;
		}
		BoxCollider boxCollider = (BoxCollider)base.InteractivePanel.gameObject.GetComponent(typeof(BoxCollider));
		boxCollider.size = new Vector3(base.GetSize().x, base.GetSize().y, 0f);
		boxCollider.center = new Vector3(base.GetSize().x / 2f, -base.GetSize().y / 2f, 0f);
		float val = GUICamera.width - base.GetSize().x - 1f;
		float val2 = GUICamera.height - base.GetSize().y - 1f;
		Vector2 location = new Vector2(Math.Min(base.GetLocation().x, val), Math.Min(base.GetLocationY(), val2));
		base.SetLocation(location);
	}

	public void SizeSet(float width)
	{
		base.SetSize(width, base.GetSize().y);
		this.m_DrawTexture_Name1.width = width;
		this.m_Label_Label4.width = width;
		for (int i = 0; i < 3; i++)
		{
			this.m_ListBox_ListBox[i].SetSize(width, this.m_ListBox_ListBox[i].GetSize().y);
			this.m_ListBox_ListBox[i].SetColumnWidth((int)this.m_ListBox_ListBox[i].GetSize().x, 0, 0, 0, 0);
		}
		this.m_DrawTexture_Line[1].width = width;
		this.m_DrawTexture_Line[0].width = width;
		this.m_DrawTexture_ListBG1.width = width;
	}

	private void Rearrange_Section1()
	{
		float num = (!this.m_DrawTexture_Name1.Visible) ? 0f : (this.m_DrawTexture_Name1.GetSize().y - 1f);
		float num2 = num;
		float num3 = 5f;
		this.m_DrawTexture_ListBG1.SetLocation(0f, num2);
		num2 += num3;
		this.m_ListBox_ListBox[0].SetLocation(1f, num2);
		num2 += this.m_ListBox_ListBox[0].GetSize().y + num3;
		base.SetSize(base.GetSize().x, num2);
		this.m_DrawTexture_ListBG1.SetSize(base.GetSize().x, num2 - num);
	}

	private void Rearrange_Section2()
	{
		float num = (!this.m_DrawTexture_Name1.Visible) ? 0f : (this.m_DrawTexture_Name1.GetSize().y - 1f);
		float num2 = num;
		float num3 = 5f;
		this.m_DrawTexture_ListBG1.SetLocation(0f, num2);
		num2 += num3;
		this.m_ListBox_ListBox[0].SetLocation(1f, num2);
		num2 += this.m_ListBox_ListBox[0].GetSize().y + num3;
		this.m_DrawTexture_Line[0].SetLocation(1f, num2);
		num2 += this.m_DrawTexture_Line[0].GetSize().y + num3;
		this.m_ListBox_ListBox[1].SetLocation(1f, num2);
		num2 += this.m_ListBox_ListBox[1].GetSize().y + num3;
		base.SetSize(base.GetSize().x, num2);
		this.m_DrawTexture_ListBG1.SetSize(base.GetSize().x, num2 - num);
	}

	private void Rearrange_Section3()
	{
		float num = (!this.m_DrawTexture_Name1.Visible) ? 0f : (this.m_DrawTexture_Name1.GetSize().y - 1f);
		float num2 = num;
		float num3 = 5f;
		this.m_DrawTexture_ListBG1.SetLocation(0f, num2);
		num2 += num3;
		this.m_ListBox_ListBox[0].SetLocation(1f, num2);
		num2 += this.m_ListBox_ListBox[0].GetSize().y + num3;
		this.m_DrawTexture_Line[0].SetLocation(1f, num2);
		num2 += this.m_DrawTexture_Line[0].GetSize().y + num3;
		this.m_ListBox_ListBox[1].SetLocation(1f, num2);
		num2 += this.m_ListBox_ListBox[1].GetSize().y + num3;
		this.m_DrawTexture_Line[1].SetLocation(1f, num2);
		num2 += this.m_DrawTexture_Line[1].GetSize().y + num3;
		this.m_ListBox_ListBox[2].SetLocation(1f, num2);
		num2 += this.m_ListBox_ListBox[2].GetSize().y + num3;
		base.SetSize(base.GetSize().x, num2);
		this.m_DrawTexture_ListBG1.SetSize(base.GetSize().x, num2 - num);
	}

	public void SetPosition(float _x, float _y)
	{
		base.SetLocation(_x, _y);
	}

	public void SetSimple_Section_1()
	{
		this.m_DrawTexture_Name1.Visible = false;
		this.m_Label_Label4.Visible = false;
		this.m_Label_Label4.spriteText.gameObject.SetActive(false);
		this.m_ListBox_ListBox[0].Visible = true;
		this.m_DrawTexture_Line[0].Visible = false;
		this.m_ListBox_ListBox[1].Visible = false;
		this.m_ListBox_ListBox[2].Visible = false;
		this.m_DrawTexture_Line[1].Visible = false;
	}

	public void SetSimple_Section_2()
	{
		this.m_DrawTexture_Name1.Visible = false;
		this.m_Label_Label4.Visible = false;
		this.m_Label_Label4.spriteText.gameObject.SetActive(false);
		this.m_ListBox_ListBox[0].Visible = true;
		this.m_DrawTexture_Line[0].Visible = true;
		this.m_ListBox_ListBox[1].Visible = true;
		this.m_ListBox_ListBox[2].Visible = false;
		this.m_DrawTexture_Line[1].Visible = false;
	}

	public void SetSimple_Section_3()
	{
		this.m_DrawTexture_Name1.Visible = false;
		this.m_Label_Label4.spriteText.gameObject.SetActive(false);
		this.m_ListBox_ListBox[0].Visible = true;
		this.m_DrawTexture_Line[1].Visible = true;
		this.m_ListBox_ListBox[1].Visible = true;
		this.m_ListBox_ListBox[2].Visible = true;
		this.m_DrawTexture_Line[0].Visible = true;
	}

	public void SetName_Section_1()
	{
		this.m_DrawTexture_Name1.Visible = true;
		this.m_Label_Label4.Visible = true;
		this.m_ListBox_ListBox[0].Visible = true;
		this.m_DrawTexture_Line[1].Visible = false;
		this.m_ListBox_ListBox[1].Visible = false;
		this.m_ListBox_ListBox[2].Visible = false;
		this.m_DrawTexture_Line[0].Visible = false;
	}

	public void SetName_Section_2()
	{
		this.m_DrawTexture_Name1.Visible = true;
		this.m_Label_Label4.Visible = true;
		this.m_ListBox_ListBox[0].Visible = true;
		this.m_DrawTexture_Line[0].Visible = true;
		this.m_ListBox_ListBox[1].Visible = true;
		this.m_ListBox_ListBox[2].Visible = false;
		this.m_DrawTexture_Line[1].Visible = false;
	}

	public void SetName_Section_3()
	{
		this.m_DrawTexture_Name1.Visible = true;
		this.m_Label_Label4.Visible = true;
		this.m_ListBox_ListBox[0].Visible = true;
		this.m_DrawTexture_Line[1].Visible = true;
		this.m_ListBox_ListBox[1].Visible = true;
		this.m_ListBox_ListBox[2].Visible = true;
		this.m_DrawTexture_Line[0].Visible = true;
	}

	private void ListBoxSelect(IUIObject obj)
	{
		ListBox listBox = (ListBox)obj;
		UIListItemContainer selectedItem = listBox.SelectedItem;
		if (selectedItem == null)
		{
			return;
		}
		if (selectedItem.Data != null)
		{
			UI_RightClickMenu.callbackData callbackData = (UI_RightClickMenu.callbackData)selectedItem.Data;
			callbackData.callback(callbackData.data);
			if (callbackData.bCloseRightUI)
			{
				this.Close();
			}
		}
	}
}
