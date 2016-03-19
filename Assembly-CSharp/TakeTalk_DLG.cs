using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityForms;

public class TakeTalk_DLG : Form
{
	private Box m_NPCTalk_BG;

	private Button m_NPCTalk_Transbutton;

	private DrawTexture m_DrawTexture_DrawTexture27;

	private Button m_NPCTalk_close;

	private Label m_NPCTalk_npcname;

	private DrawTexture m_Face;

	private FlashLabel m_NPCTalk_talklabel;

	private Button m_Button_Button36;

	private maxCamera m_WorldCamera;

	private int m_i32CurCharKind;

	private short m_i16CharUnique;

	private CQuest m_Quest;

	private bool m_bOK;

	private int m_bCount;

	private int m_nTalkMode;

	private ITEM m_cItem;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SOLRECRUITSUCCESS_DLG);
		instance.LoadFileAll(ref form, "NpcTalk/DLG_taketalk", G_ID.TAKETALK_DLG, false);
		NrTSingleton<FormsManager>.Instance.HideExcept(G_ID.TAKETALK_DLG);
		NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(true);
	}

	public override void SetComponent()
	{
		this.m_NPCTalk_BG = (base.GetControl("NPCTalk_BG") as Box);
		this.m_NPCTalk_Transbutton = (base.GetControl("NPCTalk_Transbutton") as Button);
		this.m_DrawTexture_DrawTexture27 = (base.GetControl("DrawTexture_DrawTexture27") as DrawTexture);
		this.m_NPCTalk_close = (base.GetControl("NPCTalk_close") as Button);
		this.m_NPCTalk_npcname = (base.GetControl("NPCTalk_npcname") as Label);
		this.m_NPCTalk_talklabel = (base.GetControl("NPCTalk_talklabel") as FlashLabel);
		this.m_Button_Button36 = (base.GetControl("Button_Button36") as Button);
		base.SetSize(GUICamera.width, base.GetSize().y);
		base.SetLocation(base.GetLocation().x, GUICamera.height - base.GetSize().y);
		this.m_NPCTalk_BG.SetSize(GUICamera.width, this.m_NPCTalk_BG.GetSize().y);
		this.m_NPCTalk_Transbutton.SetLocation((float)this.PointX(this.m_NPCTalk_Transbutton.GetLocation().x), this.m_NPCTalk_Transbutton.GetLocationY());
		this.m_NPCTalk_Transbutton.SetSize(GUICamera.width, this.m_NPCTalk_Transbutton.GetSize().y);
		float x = (GUICamera.width - this.m_NPCTalk_npcname.GetSize().x + 512f) / 2f;
		this.m_NPCTalk_npcname.SetLocation(x, this.m_NPCTalk_npcname.GetLocationY());
		x = (GUICamera.width - this.m_DrawTexture_DrawTexture27.GetSize().x + 512f) / 2f;
		this.m_DrawTexture_DrawTexture27.SetLocation(x, this.m_DrawTexture_DrawTexture27.GetLocationY());
		x = (GUICamera.width - this.m_NPCTalk_talklabel.width + 512f) / 2f;
		this.m_NPCTalk_talklabel.SetLocation(x, this.m_NPCTalk_talklabel.GetLocationY());
		this.m_NPCTalk_talklabel.SetFlashLabel(string.Empty);
		this.m_NPCTalk_close.SetLocation(GUICamera.width - this.m_NPCTalk_close.GetSize().x - 10f, this.m_NPCTalk_close.GetLocationY());
		this.m_Button_Button36.SetLocation(GUICamera.width - this.m_Button_Button36.GetSize().x - 10f, this.m_Button_Button36.GetLocationY());
		this.m_Face = (base.GetControl("DrawTexture_NPCFace01") as DrawTexture);
		this.m_Face.SetLocation(this.m_Face.GetLocation().x, -232f);
		Button expr_2C2 = this.m_NPCTalk_close;
		expr_2C2.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2C2.Click, new EZValueChangedDelegate(this.CloseTakeTalk));
		Button expr_2E9 = this.m_Button_Button36;
		expr_2E9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2E9.Click, new EZValueChangedDelegate(this.OnTake));
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		this.m_NPCTalk_BG.SetSize(GUICamera.width, this.m_NPCTalk_BG.GetSize().y);
		float x = (GUICamera.width - this.m_NPCTalk_npcname.GetSize().x + 512f) / 2f;
		this.m_NPCTalk_npcname.SetLocation(x, this.m_NPCTalk_npcname.GetLocationY());
		x = (GUICamera.width - this.m_DrawTexture_DrawTexture27.GetSize().x + 512f) / 2f;
		this.m_DrawTexture_DrawTexture27.SetLocation(x, this.m_DrawTexture_DrawTexture27.GetLocationY());
		x = (GUICamera.width - this.m_NPCTalk_talklabel.width + 512f) / 2f;
		this.m_NPCTalk_talklabel.SetLocation(x, this.m_NPCTalk_talklabel.GetLocationY());
		this.m_NPCTalk_Transbutton.SetLocation((float)this.PointX(this.m_NPCTalk_Transbutton.GetLocation().x), this.m_NPCTalk_Transbutton.GetLocationY());
		this.m_NPCTalk_Transbutton.SetSize(GUICamera.width, this.m_NPCTalk_Transbutton.GetSize().y);
		this.m_NPCTalk_close.SetLocation(GUICamera.width - this.m_NPCTalk_close.GetSize().x - 10f, this.m_NPCTalk_close.GetLocationY());
		this.m_Button_Button36.SetLocation(GUICamera.width - this.m_Button_Button36.GetSize().x - 10f, this.m_Button_Button36.GetLocationY());
	}

	public int PointX(float i32X)
	{
		float num = GUICamera.width * (i32X / base.GetSize().x);
		return (int)num;
	}

	public void SetInventorySellItem(ITEM cItem, long nMoney)
	{
		this.m_nTalkMode = 1;
		this.m_cItem = cItem;
		this.m_Button_Button36.Visible = true;
		this.m_Button_Button36.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("52");
		this.m_NPCTalk_Transbutton.Visible = true;
		this.m_Face.SetTexture(eCharImageType.LARGE, 242, -1);
		this.m_NPCTalk_npcname.Text = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(242);
		string empty = string.Empty;
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_cItem);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip("2031"),
			"targetname",
			itemNameByItemUnique,
			"money",
			nMoney
		});
		this.m_NPCTalk_talklabel.Visible = true;
		this.m_NPCTalk_talklabel.SetFlashLabel(empty);
	}

	public void SetNpc(int i32CharKind, short i16CharUnique, string strQuestUnique)
	{
		this.m_nTalkMode = 0;
		this.m_Button_Button36.Visible = true;
		this.m_NPCTalk_Transbutton.Visible = true;
		this.m_i32CurCharKind = i32CharKind;
		this.m_i16CharUnique = i16CharUnique;
		this.m_Quest = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
		if (this.m_Quest == null)
		{
			return;
		}
		this.m_bCount = 0;
		while (this.m_bCount < 3)
		{
			if ((int)this.m_Quest.GetQuestCommon().cQuestCondition[this.m_bCount].i64Param == i32CharKind)
			{
				break;
			}
			this.m_bCount++;
		}
		NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(i16CharUnique);
		if (charByCharUnique == null)
		{
			return;
		}
		this.m_Face.SetTexture(eCharImageType.LARGE, i32CharKind, -1);
		this.m_NPCTalk_npcname.Text = charByCharUnique.GetCharKindInfo().GetName();
		string textFromCharInfo = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo(charByCharUnique.GetCharKindInfo().GetCHARKIND_NPCINFO().GetTextGreeting());
		this.m_NPCTalk_talklabel.Visible = true;
		this.m_NPCTalk_talklabel.SetFlashLabel(textFromCharInfo);
		this.SetCameraSet(this.m_i16CharUnique);
	}

	private void SetCameraSet(short i16CharUnique)
	{
		NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(i16CharUnique);
		if (charByCharUnique == null)
		{
			return;
		}
		charByCharUnique.ProcessMouseEvent = false;
		NrTSingleton<NkClientLogic>.Instance.BackMainCameraInfo();
	}

	private void CloseTakeTalk(IUIObject obj)
	{
		NrCharNPC nrCharNPC = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(this.m_i16CharUnique) as NrCharNPC;
		if (nrCharNPC != null)
		{
			nrCharNPC.ProcessMouseEvent = true;
		}
		this.Close();
	}

	private void OnTake(IUIObject obj)
	{
		int nTalkMode = this.m_nTalkMode;
		if (nTalkMode != 0)
		{
			if (nTalkMode == 1)
			{
				Protocol_Item.Send_AutoItemSell(this.m_cItem.m_nItemID);
			}
		}
		else
		{
			GS_QUEST_GET_CHAR_REQ gS_QUEST_GET_CHAR_REQ = new GS_QUEST_GET_CHAR_REQ();
			TKString.StringChar(this.m_Quest.GetQuestUnique(), ref gS_QUEST_GET_CHAR_REQ.strQuestUnique);
			gS_QUEST_GET_CHAR_REQ.i32CharKind = this.m_i32CurCharKind;
			gS_QUEST_GET_CHAR_REQ.bItemType = 0;
			gS_QUEST_GET_CHAR_REQ.nItemPos = 0;
			gS_QUEST_GET_CHAR_REQ.i32ItemNum = 0;
			SendPacket.GetInstance().SendObject(1023, gS_QUEST_GET_CHAR_REQ);
		}
		this.m_bOK = true;
		this.Close();
	}

	public void SetOkMessge(bool bOk)
	{
		this.m_bOK = bOk;
		if (bOk)
		{
			string strTextKey = string.Format("{0}+{1}+p", this.m_Quest.GetQuestUnique(), this.m_bCount);
			string textFromQuest_Dialog = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Dialog(strTextKey);
			this.m_NPCTalk_talklabel.SetFlashLabel(textFromQuest_Dialog);
			this.m_Button_Button36.Visible = false;
		}
	}

	public void SetFailMessage()
	{
		string strTextKey = string.Format("{0}+{1}+g", this.m_Quest.GetQuestUnique(), this.m_bCount);
		string textFromQuest_Dialog = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Dialog(strTextKey);
		this.m_NPCTalk_talklabel.SetFlashLabel(textFromQuest_Dialog);
	}

	public override void Close()
	{
		if (this.m_bOK)
		{
			NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(this.m_i16CharUnique);
			if (charByCharUnique != null)
			{
				NrTSingleton<NkCharManager>.Instance.DeleteChar(charByCharUnique.GetID());
			}
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TAKE_DLG);
		NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
		NrTSingleton<FormsManager>.Instance.Main_UI_Show(FormsManager.eMAIN_UI_VISIBLE_MODE.COMMON);
		if (this.m_nTalkMode == 1)
		{
			Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
			inventory_Dlg.Show();
		}
		NrTSingleton<NkQuestManager>.Instance.ReleaseQuestCamera();
		if (null != this.m_WorldCamera)
		{
			this.m_WorldCamera.RestoreCameraInfo();
		}
		base.Close();
	}

	public override void OnClose()
	{
		NrTSingleton<NrMainSystem>.Instance.MemoryCleanUP();
	}
}
