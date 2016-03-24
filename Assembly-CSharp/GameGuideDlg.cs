using GAME;
using System;
using UnityEngine;
using UnityForms;

public class GameGuideDlg : Form
{
	private DrawTexture backImage;

	private FlashLabel talkText;

	private Button close;

	private Button excute;

	private DrawTexture m_CenterNpcImage;

	private Label m_CenterNpcName;

	private DrawTexture[] m_CenterNpcNameBack = new DrawTexture[4];

	private ItemTexture item1;

	private DrawTexture backItem1;

	private ItemTexture item2;

	private maxCamera m_WorldCamera;

	private Button m_bttransbutton;

	private GameGuideInfo currentGameGuideInfo;

	private OnCloseCallback _closeCallback;

	private bool escClose = true;

	public override void InitializeComponent()
	{
		NrTSingleton<FormsManager>.Instance.HideExcept(G_ID.BACK_DLG);
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = false;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "System/Dlg_GameGuide", G_ID.GAMEGUIDE_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_bttransbutton = (base.GetControl("Button_Transbutton") as Button);
		this.m_bttransbutton.Click = new EZValueChangedDelegate(this.ClickClose);
		this.m_bttransbutton.UseDefaultSound = false;
		this.backImage = (base.GetControl("NPCTalk_BG") as DrawTexture);
		this.m_CenterNpcImage = (base.GetControl("DrawTexture_NPCFace01") as DrawTexture);
		this.m_CenterNpcImage.SetTexture(eCharImageType.LARGE, 242, -1, string.Empty);
		this.m_CenterNpcName = (base.GetControl("NPCTalk_npcname") as Label);
		this.m_CenterNpcName.Text = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(242);
		this.m_CenterNpcNameBack[0] = (base.GetControl("DrawTexture_NPCTalk_npcnameBG_left_line") as DrawTexture);
		this.m_CenterNpcNameBack[1] = (base.GetControl("DrawTexture_NPCTalk_npcnameBG_right_line") as DrawTexture);
		this.m_CenterNpcNameBack[2] = (base.GetControl("DrawTexture_NPCTalk_npcnameBG_left") as DrawTexture);
		this.m_CenterNpcNameBack[3] = (base.GetControl("DrawTexture_NPCTalk_npcnameBG_right") as DrawTexture);
		this.close = (base.GetControl("NPCTalk_close") as Button);
		this.close.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickClose));
		this.excute = (base.GetControl("Button_Button9") as Button);
		this.excute.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickExcuteGameGuide));
		this.talkText = (base.GetControl("NPCTalk_talklabel") as FlashLabel);
		this.talkText.Visible = true;
		this.item1 = (base.GetControl("ItemTexture_Item1") as ItemTexture);
		this.backItem1 = (base.GetControl("DrawTexture_itemtexture1") as DrawTexture);
		this.item2 = (base.GetControl("ItemTexture_Item2") as ItemTexture);
		base.SetShowLayer(2, false);
		this.RepositionControl();
		NrTSingleton<NkCharManager>.Instance.ShowHideAll(false, false, false);
		this.SetTalkText();
		if (null != Camera.main)
		{
			this.m_WorldCamera = Camera.main.GetComponent<maxCamera>();
			if (this.m_WorldCamera != null)
			{
				NrTSingleton<NkClientLogic>.Instance.BackMainCameraInfo();
				this.m_WorldCamera.StopCameraControl();
			}
		}
	}

	private void RepositionControl()
	{
		base.SetSize(GUICamera.width, base.GetSizeY());
		base.SetLocation(0f, GUICamera.height - base.GetSizeY());
		this.backImage.SetSize(GUICamera.width, this.backImage.height);
		this.close.SetLocation(GUICamera.width - this.close.GetSize().x - 10f, this.close.GetLocationY());
		GameGuideInfo gameGuideInfo = NrTSingleton<GameGuideManager>.Instance.GetGameGuideInfo();
		if (gameGuideInfo != null)
		{
			if (gameGuideInfo.m_eType != GameGuideType.DEFAULT)
			{
				if (NrTSingleton<ContentsLimitManager>.Instance.IsHeroBattle())
				{
					if (gameGuideInfo.m_eType == GameGuideType.EQUIP_SELL || gameGuideInfo.m_eType == GameGuideType.SUPPORT_GOLD || gameGuideInfo.m_eType == GameGuideType.MINE_ITEMGET || gameGuideInfo.m_eType == GameGuideType.PURCHASE_RESTORE || gameGuideInfo.m_eType == GameGuideType.EXPEDITION_ITEMGET)
					{
						this.close.Visible = false;
					}
				}
				else if (gameGuideInfo.m_eType == GameGuideType.EQUIP_SELL || gameGuideInfo.m_eType == GameGuideType.MINE_ITEMGET || gameGuideInfo.m_eType == GameGuideType.PURCHASE_RESTORE || gameGuideInfo.m_eType == GameGuideType.EXPEDITION_ITEMGET)
				{
					this.close.Visible = false;
				}
				this.excute.Visible = true;
				this.excute.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1860");
				this.excute.SetLocation(GUICamera.width - this.excute.GetSize().x - 10f, this.excute.GetLocationY());
			}
			else
			{
				this.excute.Visible = false;
			}
		}
		else
		{
			this.excute.Visible = false;
		}
		float x = (GUICamera.width - this.m_CenterNpcImage.GetSize().x) / 2f;
		this.m_CenterNpcImage.SetLocation(x, this.m_CenterNpcImage.GetLocationY());
		x = (GUICamera.width - this.m_CenterNpcName.GetSize().x) / 2f;
		this.m_CenterNpcName.SetLocation(x, this.m_CenterNpcName.GetLocationY());
		x = (GUICamera.width - this.m_CenterNpcNameBack[0].GetSize().x) / 2f + this.m_CenterNpcNameBack[0].GetSize().x / 2f;
		this.m_CenterNpcNameBack[0].SetLocation(x, this.m_CenterNpcNameBack[0].GetLocationY());
		x = this.m_CenterNpcNameBack[0].GetLocationX();
		this.m_CenterNpcNameBack[1].SetLocation(x, this.m_CenterNpcNameBack[1].GetLocationY());
		x = (GUICamera.width - this.m_CenterNpcNameBack[2].GetSize().x) / 2f + this.m_CenterNpcNameBack[2].GetSize().x / 2f;
		this.m_CenterNpcNameBack[2].SetLocation(x, this.m_CenterNpcNameBack[2].GetLocationY());
		x = this.m_CenterNpcNameBack[2].GetLocationX();
		this.m_CenterNpcNameBack[3].SetLocation(x, this.m_CenterNpcNameBack[3].GetLocationY());
	}

	public override void OnClose()
	{
		if (this._closeCallback != null)
		{
			this._closeCallback();
		}
		if (this.escClose)
		{
			this.ClickClose(null);
		}
	}

	public void ClickClose(IUIObject obj)
	{
		this.escClose = false;
		this.ExecuteGuide();
		NrTSingleton<GameGuideManager>.Instance.InitGameGuide();
	}

	private void ClickExcuteGameGuide(IUIObject obj)
	{
		this.escClose = false;
		if (this.currentGameGuideInfo.m_eType == GameGuideType.EQUIP_ITEM || this.currentGameGuideInfo.m_eType == GameGuideType.PURCHASE_RESTORE)
		{
			this.currentGameGuideInfo.ExcuteGameGuide();
		}
		else
		{
			this.ExecuteGuide();
			NrTSingleton<GameGuideManager>.Instance.ExcuteGameGuide();
		}
	}

	private void ExecuteGuide()
	{
		NoticeIconDlg.SetIcon(ICON_TYPE.GAMEGUIDE, false);
		NrTSingleton<FormsManager>.Instance.Main_UI_Show(FormsManager.eMAIN_UI_VISIBLE_MODE.COMMON);
		NrTSingleton<NkCharManager>.Instance.ShowHideAll(true, true, true);
		if (null != this.m_WorldCamera)
		{
			this.m_WorldCamera.RestoreCameraInfo();
		}
		NrTSingleton<FormsManager>.Instance.AddReserveDeleteForm(base.WindowID);
	}

	public void SetTalkText()
	{
		this.currentGameGuideInfo = NrTSingleton<GameGuideManager>.Instance.GetGameGuideInfo();
		if (this.currentGameGuideInfo != null)
		{
			if (this.currentGameGuideInfo.m_eType == GameGuideType.DEFAULT)
			{
				this.talkText.SetFlashLabel(NrTSingleton<GameGuideManager>.Instance.GetDefaultGuideText());
			}
			else
			{
				this.talkText.SetFlashLabel(NrTSingleton<GameGuideManager>.Instance.GetGameGuideText());
				this.excute.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.currentGameGuideInfo.m_strButtonKey);
				if (this.currentGameGuideInfo.m_eType == GameGuideType.EQUIP_ITEM)
				{
					GameGuideEquip gameGuideEquip = this.currentGameGuideInfo as GameGuideEquip;
					if (gameGuideEquip != null)
					{
						base.SetShowLayer(2, true);
						if (gameGuideEquip.GetSrcItem() != null && gameGuideEquip.GetDestItem() == null)
						{
							this.item1.ClearData();
							this.item1.SetTexture("Win_T_ItemEmpty");
							this.item2.SetItemTexture(gameGuideEquip.GetSrcItem());
						}
						else if (gameGuideEquip.GetSrcItem() != null && gameGuideEquip.GetDestItem() != null)
						{
							this.item1.SetItemTexture(gameGuideEquip.GetDestItem());
							this.backItem1.SetTexture(gameGuideEquip.GetDestItem().GetRankImage());
							this.item2.SetItemTexture(gameGuideEquip.GetSrcItem());
						}
					}
				}
				else if (this.currentGameGuideInfo.m_eType == GameGuideType.EQUIP_SELL && NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetEquipSellMoney() == 0L && NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().EquipMoneyAttackPlunder)
				{
					NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().EquipMoneyAttackPlunder = false;
					this.excute.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10"));
				}
			}
		}
	}

	public override void ChangedResolution()
	{
		this.RepositionControl();
	}

	public void RegistCloseCallback(OnCloseCallback callback)
	{
		this._closeCallback = callback;
	}
}
