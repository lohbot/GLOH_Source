using System;
using UnityEngine;
using UnityForms;

public class SupporterDlg : Form
{
	private DrawTexture m_DT_DrawTexture0;

	private DrawTexture m_DT_NpcImg;

	private Label m_LB_Label01;

	private DrawTexture m_DT_LabelBG01;

	private DrawTexture m_DT_LabelBG02;

	private Button m_BT_Select01;

	private Button m_BT_Select02;

	private Button m_BT_Select03;

	private string m_strCharName = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Community/dlg_supporterregister", G_ID.SUPPORTER_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_DT_DrawTexture0 = (base.GetControl("DrawTexture_DrawTexture0") as DrawTexture);
		this.m_DT_NpcImg = (base.GetControl("DT_NpcImg") as DrawTexture);
		this.m_LB_Label01 = (base.GetControl("LB_Label01") as Label);
		this.m_DT_LabelBG01 = (base.GetControl("DT_LabelBG01") as DrawTexture);
		this.m_DT_LabelBG02 = (base.GetControl("DT_LabelBG02") as DrawTexture);
		this.m_BT_Select01 = (base.GetControl("BT_Select01") as Button);
		this.m_BT_Select02 = (base.GetControl("BT_Select02") as Button);
		this.m_BT_Select03 = (base.GetControl("BT_Select03") as Button);
		this.m_BT_Select01.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickBT_Select1));
		this.m_BT_Select02.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickBT_Select2));
		this.m_BT_Select03.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickBT_Select3));
		base.Draggable = false;
		base.SetScreenCenter();
		this.SetupBalck_BG("Win_T_BK", 0f, 0f, GUICamera.width, GUICamera.height);
		string textureFromBundle = string.Empty;
		this.m_DT_NpcImg.SetTextureFromUISoldierBundle(eCharImageType.LARGE, "mine");
		textureFromBundle = string.Format("UI/CharSelect/ChaSelect_BG", new object[0]);
		this.m_DT_DrawTexture0.SetTextureFromBundle(textureFromBundle);
		this.ShowDrawHide(false);
		this.SupporterDlgGuiSet();
	}

	public void SetType(string strcharname, bool bShow)
	{
		this.m_strCharName = strcharname;
		this.SupporterDlgGuiSet();
	}

	private void SupporterDlgGuiSet()
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1818");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
			{
				textFromInterface,
				"targetname1",
				this.m_strCharName,
				"targetname2",
				this.m_strCharName
			});
			this.m_LB_Label01.SetText(textFromInterface);
		}
	}

	public void ShowDrawHide(bool bShow)
	{
		this.m_LB_Label01.Hide(bShow);
		this.m_DT_LabelBG01.Hide(bShow);
		this.m_DT_LabelBG02.Hide(bShow);
		this.m_BT_Select01.Hide(bShow);
		this.m_BT_Select02.Hide(bShow);
		this.m_BT_Select03.Hide(bShow);
		if (TsPlatform.IsIPad() || TsPlatform.IsIPhone)
		{
			this.m_BT_Select03.Visible = false;
		}
	}

	public void On_ClickBT_Select1(IUIObject a_cObject)
	{
		if (a_cObject == null)
		{
			return;
		}
		this.ShowDrawHide(true);
		if (!(NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SUPPORTERSUB_DLG) is SupporterSubDlg))
		{
			TsLog.LogWarning("!!!!!!!!!!! NOT SUPPORTERSUB_DLG", new object[0]);
		}
		this.OnClose();
	}

	public void On_ClickBT_Select2(IUIObject a_cObject)
	{
		if (a_cObject == null)
		{
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("550");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("172");
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnSupporterOK), null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
	}

	public void OnSupporterOK(object a_oObject)
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return;
		}
		this.SetSupport(string.Empty);
	}

	public void On_ClickBT_Select3(IUIObject a_cObject)
	{
		if (a_cObject == null)
		{
			return;
		}
		if (TsPlatform.IsIPad() || TsPlatform.IsIPhone)
		{
			return;
		}
		this.OnClose();
		NrTSingleton<NrMainSystem>.Instance.QuitGame();
	}

	public void SetSupport(string strSupporterName)
	{
		DLG_CreateChar dLG_CreateChar = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEW_CREATECHAR_DLG) as DLG_CreateChar;
		if (dLG_CreateChar != null)
		{
			dLG_CreateChar.SetCharacterCustomNext(strSupporterName);
		}
		this.OnClose();
	}

	public string GetCharName()
	{
		return this.m_strCharName;
	}

	public override void CloseForm(IUIObject obj)
	{
		base.CloseForm(obj);
	}

	public void SetupBalck_BG(string imagekey, float x, float y, float w, float h)
	{
		UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(imagekey);
		if (uIBaseInfoLoader == null)
		{
			return;
		}
		base.BLACK_BG = Box.Create("Win_T_BK", new Vector3(0f, 0f, 0.1f));
		base.BLACK_BG.autoResize = true;
		base.BLACK_BG.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		base.BLACK_BG.gameObject.layer = GUICamera.UILayer;
		BoxCollider boxCollider = (BoxCollider)base.BLACK_BG.gameObject.AddComponent(typeof(BoxCollider));
		if (null != boxCollider)
		{
			boxCollider.size = new Vector3(GUICamera.width, GUICamera.height, 0f);
			boxCollider.center = new Vector3(GUICamera.width / 2f, -GUICamera.height / 2f, 0f);
		}
		base.InteractivePanel.MakeChild(base.BLACK_BG.gameObject);
		base.BLACK_BG.transform.localPosition = new Vector3(-base.GetLocation().x, -base.GetLocation().y, 0.1f);
		base.BLACK_BG.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
		base.BLACK_BG.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		base.BLACK_BG.Setup(w, h, material);
		base.BLACK_BG.SetTextureUVs(new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height));
		base.BLACK_BG.SetColor(new Color(1f, 1f, 1f, 0.8f));
	}
}
