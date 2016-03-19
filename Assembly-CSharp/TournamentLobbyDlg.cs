using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class TournamentLobbyDlg : Form
{
	public class BANCONTROLLER
	{
		public DrawTexture m_dtFrame;

		public DrawTexture m_dtSol;
	}

	public class SOLPOS_DRAWTEXTURE
	{
		public eBATTLE_ALLY eAlly;

		public int nPos = -1;

		public eTMLOBBY_STEP m_eSetStep;
	}

	public class SOLDIER_BATCH_INFO
	{
		public int m_nSoldierKind;

		public int m_nSoldierPos = -1;

		public eTMLOBBY_STEP m_eSetStep;
	}

	private int m_nSolLineCount = 8;

	private int SOLFRAME;

	private int SOLFACE = 8;

	private int BANMARK = 16;

	private eTMLOBBY_STEP m_eLobbyStep;

	private eBATTLE_ALLY m_eMyAlly = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;

	private eBATTLE_ALLY m_eActiveAlly = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;

	private NewListBox m_lbSolList;

	private Button m_btChat;

	private Label m_lbPlayer1;

	private Label m_lbPlayer2;

	private Label m_lbTimeCount;

	private ChatLabel _clChat;

	private DrawTexture m_dwBG;

	private Label m_lbExplain;

	private Button[] m_btOk;

	private DrawTexture[][] m_dtSolPos;

	private DrawTexture[][] m_dtSolPosFrame;

	private TournamentLobbyDlg.BANCONTROLLER[][] m_dtBanTexture;

	private int m_nLobbyIndex;

	private int[] SELECTCOUNT = new int[]
	{
		1,
		2,
		2,
		1
	};

	private int[][] m_nBanSolKind;

	private TournamentLobbyDlg.SOLDIER_BATCH_INFO[][] m_SelectSoldier;

	private int m_nSelectStep;

	private int m_nStartIndex;

	private string m_szPlayer1 = string.Empty;

	private string m_szPlayer2 = string.Empty;

	private int m_nSelectSolKind;

	private float m_TurnSwapTime;

	private float m_TurnTime;

	private float m_TurnTimeEnd;

	private bool m_bStop;

	public bool m_bUpdate = true;

	private bool m_RandOK;

	private int nPos0;

	private int nPos1;

	private GameObject m_goCountDown;

	private TOURNAMENT_LOBBY_LISTBOX_DATA m_FirstSoldierKind;

	private TOURNAMENT_LOBBY_LISTBOX_DATA m_SecondSoldierKind;

	private TOURNAMENT_LOBBY_LISTBOX_DATA m_RandSolInfo = new TOURNAMENT_LOBBY_LISTBOX_DATA();

	private TournamentLobbyDlg.SOLPOS_DRAWTEXTURE m_RandSolPos = new TournamentLobbyDlg.SOLPOS_DRAWTEXTURE();

	public int LobbyIndex
	{
		get
		{
			return this.m_nLobbyIndex;
		}
		set
		{
			this.m_nLobbyIndex = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Lobby", G_ID.TOURNAMENT_LOBBY_DLG, false, true);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	public override void SetComponent()
	{
		this.m_dwBG = (base.GetControl("DT_Count") as DrawTexture);
		if (this.m_dwBG != null)
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_COUNTDOWN", this.m_dwBG, this.m_dwBG.GetSize());
			this.m_dwBG.AddGameObjectDelegate(new EZGameObjectDelegate(this.effectdownload));
			this.m_dwBG.Visible = false;
		}
		this.m_lbTimeCount = (base.GetControl("LB_Count") as Label);
		this.m_lbTimeCount.Visible = false;
		this.m_lbSolList = (base.GetControl("NewListBox_SolSelect") as NewListBox);
		this.m_btChat = (base.GetControl("Button_ChatBtn") as Button);
		Button expr_BE = this.m_btChat;
		expr_BE.Click = (EZValueChangedDelegate)Delegate.Combine(expr_BE.Click, new EZValueChangedDelegate(this.OnClickChat));
		this.m_lbPlayer1 = (base.GetControl("Label_1P_Name") as Label);
		this.m_lbPlayer2 = (base.GetControl("Label_2P_Name") as Label);
		this.m_lbExplain = (base.GetControl("Label_Narr") as Label);
		this.m_dtSolPos = new DrawTexture[2][];
		this.m_dtSolPosFrame = new DrawTexture[2][];
		this.m_dtBanTexture = new TournamentLobbyDlg.BANCONTROLLER[2][];
		this.m_nBanSolKind = new int[2][];
		this.m_SelectSoldier = new TournamentLobbyDlg.SOLDIER_BATCH_INFO[2][];
		this.m_btOk = new Button[2];
		this._clChat = ChatManager.MakeChatLabel(this, "ChatLabel", Vector2.zero);
		for (int i = 0; i < 2; i++)
		{
			string name = string.Format("Button_OK_{0}P", (i + 1).ToString());
			this.m_btOk[i] = (base.GetControl(name) as Button);
			Button expr_1B9 = this.m_btOk[i];
			expr_1B9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1B9.Click, new EZValueChangedDelegate(this.OnClickOK));
			this.m_btOk[i].Visible = false;
			this.m_dtBanTexture[i] = new TournamentLobbyDlg.BANCONTROLLER[3];
			this.m_dtSolPos[i] = new DrawTexture[9];
			this.m_dtSolPosFrame[i] = new DrawTexture[9];
			this.m_nBanSolKind[i] = new int[3];
			this.m_SelectSoldier[i] = new TournamentLobbyDlg.SOLDIER_BATCH_INFO[3];
			for (int j = 0; j < 3; j++)
			{
				this.m_dtBanTexture[i][j] = new TournamentLobbyDlg.BANCONTROLLER();
				this.m_nBanSolKind[i][j] = 0;
				this.m_SelectSoldier[i][j] = new TournamentLobbyDlg.SOLDIER_BATCH_INFO();
				this.m_SelectSoldier[i][j].m_nSoldierKind = 0;
				this.m_SelectSoldier[i][j].m_nSoldierPos = -1;
				string name2 = string.Format("DrawTexture_{0}P_BanFace{1}", (i + 1).ToString(), (j + 1).ToString("00"));
				this.m_dtBanTexture[i][j].m_dtSol = (base.GetControl(name2) as DrawTexture);
				name2 = string.Format("DrawTexture_{0}P_BanHeroFrame{1}", (i + 1).ToString(), (j + 1).ToString("00"));
				this.m_dtBanTexture[i][j].m_dtFrame = (base.GetControl(name2) as DrawTexture);
			}
			for (int j = 0; j < 9; j++)
			{
				string name3 = string.Format("DrawTexture_{0}P_SelectFace{1}", (i + 1).ToString(), (j + 1).ToString("00"));
				this.m_dtSolPos[i][j] = (base.GetControl(name3) as DrawTexture);
				this.m_dtSolPos[i][j].UsedCollider(true);
				this.m_dtSolPos[i][j].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSolPos));
				TournamentLobbyDlg.SOLPOS_DRAWTEXTURE sOLPOS_DRAWTEXTURE = new TournamentLobbyDlg.SOLPOS_DRAWTEXTURE();
				sOLPOS_DRAWTEXTURE.eAlly = (eBATTLE_ALLY)i;
				sOLPOS_DRAWTEXTURE.nPos = j;
				sOLPOS_DRAWTEXTURE.m_eSetStep = eTMLOBBY_STEP.eTMLOBBY_STEP_NONE;
				this.m_dtSolPos[i][j].data = sOLPOS_DRAWTEXTURE;
				name3 = string.Format("DrawTexture_{0}P_GridFaceFrame_{1}", (i + 1).ToString(), (j + 1).ToString("00"));
				this.m_dtSolPosFrame[i][j] = (base.GetControl(name3) as DrawTexture);
			}
		}
		this.m_lbExplain.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2406"));
		this.Hide();
	}

	public void OnClickChat(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.CHAT_MOBILE_SUB_DLG);
	}

	public void NPCTellChat(string name)
	{
		this._clChat.SystemChatPushText(name, string.Empty, string.Empty, null);
	}

	public ChatLabel GetChatLable(CHAT_TYPE msgtype)
	{
		return this._clChat;
	}

	public override void Show()
	{
		base.SetScreenCenter();
		this._clChat.Visible = true;
		base.Show();
	}

	public override void Update()
	{
		base.Update();
		this.ShowTurnCount();
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
		if (this.m_goCountDown != null)
		{
			UnityEngine.Object.Destroy(this.m_goCountDown);
			this.m_goCountDown = null;
		}
	}

	public void SetUserName(string szPlayer1, string szPlayer2)
	{
		if (szPlayer1 != null && this.m_lbPlayer1 != null)
		{
			this.m_lbPlayer1.SetText(szPlayer1);
			this.m_szPlayer1 = szPlayer1;
		}
		if (szPlayer2 != null && this.m_lbPlayer2 != null)
		{
			this.m_lbPlayer2.SetText(szPlayer2);
			this.m_szPlayer2 = szPlayer2;
		}
	}

	public void SetCharUnqiue(int nCharunique1, int nCharunique2)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser != null)
		{
			if ((int)nrCharUser.GetCharUnique() == nCharunique1)
			{
				this.m_eMyAlly = eBATTLE_ALLY.eBATTLE_ALLY_0;
				this.m_btOk[0].Visible = true;
			}
			else if ((int)nrCharUser.GetCharUnique() == nCharunique2)
			{
				this.m_eMyAlly = eBATTLE_ALLY.eBATTLE_ALLY_1;
				this.m_btOk[1].Visible = true;
			}
		}
	}

	public void SetSolList()
	{
		int colosseumEnableBatchSoldierKindCount = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetColosseumEnableBatchSoldierKindCount();
		if (colosseumEnableBatchSoldierKindCount <= 0)
		{
			return;
		}
		this.m_lbSolList.Clear();
		int num = colosseumEnableBatchSoldierKindCount / this.m_nSolLineCount + 1;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			NewListItem newListItem = new NewListItem(this.m_lbSolList.ColumnNum, true);
			for (int j = 0; j < this.m_nSolLineCount; j++)
			{
				int iIndex = i * this.m_nSolLineCount + j;
				int colosseumEnableBatchSoldierKind = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetColosseumEnableBatchSoldierKind(iIndex);
				if (colosseumEnableBatchSoldierKind > 0)
				{
					TOURNAMENT_LOBBY_LISTBOX_DATA tOURNAMENT_LOBBY_LISTBOX_DATA = new TOURNAMENT_LOBBY_LISTBOX_DATA();
					tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind = colosseumEnableBatchSoldierKind;
					tOURNAMENT_LOBBY_LISTBOX_DATA.nItemIndex = i;
					tOURNAMENT_LOBBY_LISTBOX_DATA.nDataIndex = j;
					newListItem.SetListItemData(this.SOLFACE + j, colosseumEnableBatchSoldierKind, tOURNAMENT_LOBBY_LISTBOX_DATA, new EZValueChangedDelegate(this.btClickSol), null);
					num2++;
				}
				newListItem.SetListItemData(this.BANMARK + j, false);
			}
			this.m_lbSolList.Add(newListItem);
		}
		this.m_lbSolList.RepositionItems();
		this.Show();
		GS_TOURNAMENT_PLAYER_READY_REQ gS_TOURNAMENT_PLAYER_READY_REQ = new GS_TOURNAMENT_PLAYER_READY_REQ();
		gS_TOURNAMENT_PLAYER_READY_REQ.m_SetMode = 2;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_PLAYER_READY_REQ, gS_TOURNAMENT_PLAYER_READY_REQ);
	}

	private void btClickSol(IUIObject obj)
	{
		TOURNAMENT_LOBBY_LISTBOX_DATA tOURNAMENT_LOBBY_LISTBOX_DATA = null;
		if (obj != null)
		{
			tOURNAMENT_LOBBY_LISTBOX_DATA = (obj.Data as TOURNAMENT_LOBBY_LISTBOX_DATA);
			this.m_RandOK = false;
		}
		else if (obj == null && this.m_RandSolInfo.nSoldierKind > 0)
		{
			tOURNAMENT_LOBBY_LISTBOX_DATA = this.m_RandSolInfo;
			this.m_RandOK = true;
		}
		if (this.m_FirstSoldierKind == null)
		{
			this.m_FirstSoldierKind = tOURNAMENT_LOBBY_LISTBOX_DATA;
		}
		else if (this.m_FirstSoldierKind != tOURNAMENT_LOBBY_LISTBOX_DATA)
		{
			this.m_SecondSoldierKind = tOURNAMENT_LOBBY_LISTBOX_DATA;
		}
		if (tOURNAMENT_LOBBY_LISTBOX_DATA == null || tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind <= 0)
		{
			return;
		}
		if (this.m_eActiveAlly != this.m_eMyAlly)
		{
			return;
		}
		if (this.m_eLobbyStep >= eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_1 && this.m_eLobbyStep <= eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_4)
		{
			bool flag = false;
			int num = this.SELECTCOUNT[this.m_nSelectStep];
			for (int i = 0; i < num; i++)
			{
				if (this.m_nBanSolKind[(int)this.m_eActiveAlly][i + this.m_nStartIndex] == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
				{
					flag = true;
				}
			}
			bool flag2 = true;
			if (!flag)
			{
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					if (this.m_nBanSolKind[(int)this.m_eActiveAlly][i + this.m_nStartIndex] > 0)
					{
						num2++;
					}
				}
				if (num2 == num)
				{
					flag2 = false;
				}
			}
			if (!flag && !flag2)
			{
				return;
			}
			UIListItemContainer uIListItemContainer = (UIListItemContainer)this.m_lbSolList.GetItem(tOURNAMENT_LOBBY_LISTBOX_DATA.nItemIndex);
			if (uIListItemContainer != null)
			{
				DrawTexture drawTexture = uIListItemContainer.GetElement(this.SOLFRAME + tOURNAMENT_LOBBY_LISTBOX_DATA.nDataIndex) as DrawTexture;
				if (drawTexture != null)
				{
					if (!flag && flag2)
					{
						for (int i = 0; i < 2; i++)
						{
							for (int j = 0; j < 3; j++)
							{
								if (this.m_nBanSolKind[i][j] > 0 && this.m_nBanSolKind[i][j] == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
								{
									return;
								}
							}
						}
						drawTexture.SetTexture("Win_I_FrameS");
						if (!this.m_RandOK)
						{
							Transform child = NkUtil.GetChild(drawTexture.transform, "child_effect");
							if (child != null)
							{
								UnityEngine.Object.Destroy(child.gameObject);
							}
							NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_ANCIENTTREASURE_UI", drawTexture, drawTexture.GetSize());
						}
						for (int i = 0; i < this.SELECTCOUNT[this.m_nSelectStep]; i++)
						{
							if (this.m_nBanSolKind[(int)this.m_eActiveAlly][i + this.m_nStartIndex] == 0)
							{
								this.m_nBanSolKind[(int)this.m_eActiveAlly][i + this.m_nStartIndex] = tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind;
								this.m_dtBanTexture[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_dtSol.Visible = true;
								this.m_dtBanTexture[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_dtSol.SetTexture(eCharImageType.SMALL, tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind, 0);
								break;
							}
						}
					}
					else
					{
						drawTexture.SetTexture("Win_T_ItemEmpty02");
						Transform child2 = NkUtil.GetChild(drawTexture.transform, "child_effect");
						if (child2 != null)
						{
							UnityEngine.Object.Destroy(child2.gameObject);
						}
						for (int i = 0; i < this.SELECTCOUNT[this.m_nSelectStep]; i++)
						{
							if (this.m_nBanSolKind[(int)this.m_eActiveAlly][i + this.m_nStartIndex] == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
							{
								this.m_nBanSolKind[(int)this.m_eActiveAlly][i + this.m_nStartIndex] = 0;
								this.m_dtBanTexture[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_dtSol.Visible = false;
							}
						}
					}
				}
			}
		}
		if (this.m_eLobbyStep >= eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_1 && this.m_eLobbyStep <= eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_4)
		{
			bool flag3 = false;
			for (int k = 0; k < 2; k++)
			{
				for (int l = 0; l < 3; l++)
				{
					if (this.m_nBanSolKind[k][l] > 0 && this.m_nBanSolKind[k][l] == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
					{
						return;
					}
				}
			}
			for (int k = 0; k < 3; k++)
			{
				if (this.m_SelectSoldier[(int)this.m_eMyAlly][k].m_eSetStep != eTMLOBBY_STEP.eTMLOBBY_STEP_NONE && this.m_SelectSoldier[(int)this.m_eMyAlly][k].m_eSetStep < this.m_eLobbyStep && this.m_SelectSoldier[(int)this.m_eMyAlly][k].m_nSoldierKind == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
				{
					return;
				}
			}
			int num3 = this.SELECTCOUNT[this.m_nSelectStep];
			for (int k = 0; k < num3; k++)
			{
				if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierKind == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
				{
					flag3 = true;
				}
			}
			bool flag4 = true;
			if (!flag3)
			{
				int num4 = 0;
				for (int k = 0; k < num3; k++)
				{
					if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierKind > 0)
					{
						if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierPos < 0)
						{
							return;
						}
						num4++;
					}
				}
				if (num4 == num3)
				{
					flag4 = false;
				}
			}
			if (!flag3 && !flag4)
			{
				return;
			}
			UIListItemContainer uIListItemContainer2 = (UIListItemContainer)this.m_lbSolList.GetItem(tOURNAMENT_LOBBY_LISTBOX_DATA.nItemIndex);
			if (uIListItemContainer2 != null)
			{
				DrawTexture drawTexture2 = uIListItemContainer2.GetElement(this.SOLFRAME + tOURNAMENT_LOBBY_LISTBOX_DATA.nDataIndex) as DrawTexture;
				if (drawTexture2 != null)
				{
					if (!flag3 && flag4)
					{
						for (int l = 0; l < 3; l++)
						{
							if (this.m_SelectSoldier[(int)this.m_eMyAlly][l].m_nSoldierKind > 0 && this.m_SelectSoldier[(int)this.m_eActiveAlly][l].m_nSoldierKind == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
							{
								return;
							}
						}
						drawTexture2.SetTexture("Win_I_FrameS");
						if (!this.m_RandOK)
						{
							Transform child3 = NkUtil.GetChild(drawTexture2.transform, "child_effect");
							if (child3 != null)
							{
								UnityEngine.Object.Destroy(child3.gameObject);
							}
							NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_ANCIENTTREASURE_UI", drawTexture2, drawTexture2.GetSize());
						}
						for (int k = 0; k < this.SELECTCOUNT[this.m_nSelectStep]; k++)
						{
							if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierKind == 0)
							{
								this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierKind = tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind;
								this.m_nSelectSolKind = tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind;
								break;
							}
						}
					}
					else
					{
						drawTexture2.SetTexture("Win_T_ItemEmpty02");
						Transform child4 = NkUtil.GetChild(drawTexture2.transform, "child_effect");
						if (child4 != null)
						{
							UnityEngine.Object.Destroy(child4.gameObject);
						}
						for (int k = 0; k < this.SELECTCOUNT[this.m_nSelectStep]; k++)
						{
							if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierKind == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
							{
								this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierKind = 0;
								if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierPos >= 0)
								{
									int nSoldierPos = this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierPos;
									this.m_dtSolPos[(int)this.m_eActiveAlly][nSoldierPos].SetTexture("Win_T_ItemEmpty02");
									this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierPos = -1;
								}
							}
						}
					}
				}
			}
		}
	}

	public void OnClickSolPos(IUIObject obj)
	{
		if (obj != null)
		{
			TournamentLobbyDlg.SOLPOS_DRAWTEXTURE sOLPOS_DRAWTEXTURE = obj.Data as TournamentLobbyDlg.SOLPOS_DRAWTEXTURE;
			if (this.m_eActiveAlly != this.m_eMyAlly)
			{
				return;
			}
			if (this.m_eLobbyStep < eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_1 || this.m_eLobbyStep > eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_4)
			{
				return;
			}
			if (this.m_nSelectSolKind <= 0)
			{
				return;
			}
			if (sOLPOS_DRAWTEXTURE.m_eSetStep != eTMLOBBY_STEP.eTMLOBBY_STEP_NONE && sOLPOS_DRAWTEXTURE.m_eSetStep < this.m_eLobbyStep)
			{
				return;
			}
			for (int i = 0; i < this.SELECTCOUNT[this.m_nSelectStep]; i++)
			{
				if (this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierKind > 0 && this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierKind != this.m_nSelectSolKind && this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierPos == sOLPOS_DRAWTEXTURE.nPos)
				{
					this.ClearListBoxSelectTextureFromKind(this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierKind);
					this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierKind = 0;
					this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierPos = -1;
				}
			}
			for (int i = 0; i < this.SELECTCOUNT[this.m_nSelectStep]; i++)
			{
				if (this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierKind == this.m_nSelectSolKind && this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierPos >= 0 && this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierPos != sOLPOS_DRAWTEXTURE.nPos && this.m_dtSolPos[(int)this.m_eActiveAlly][this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierPos] != null)
				{
					this.m_dtSolPos[(int)this.m_eActiveAlly][this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierPos].SetTexture("Win_T_ItemEmpty02");
					this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierPos = -1;
				}
			}
			DrawTexture drawTexture = obj as DrawTexture;
			if (drawTexture != null)
			{
				drawTexture.SetTexture(eCharImageType.SMALL, this.m_nSelectSolKind, 0);
				for (int i = 0; i < this.SELECTCOUNT[this.m_nSelectStep]; i++)
				{
					if (this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierKind == this.m_nSelectSolKind)
					{
						this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierPos = sOLPOS_DRAWTEXTURE.nPos;
					}
				}
			}
		}
		else
		{
			TournamentLobbyDlg.SOLPOS_DRAWTEXTURE sOLPOS_DRAWTEXTURE = this.m_RandSolPos;
			this.m_nSelectSolKind = this.m_RandSolInfo.nSoldierKind;
			if (this.m_eActiveAlly != this.m_eMyAlly)
			{
				return;
			}
			if (this.m_eLobbyStep < eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_1 || this.m_eLobbyStep > eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_4)
			{
				return;
			}
			if (this.m_nSelectSolKind <= 0)
			{
				return;
			}
			if (sOLPOS_DRAWTEXTURE.m_eSetStep != eTMLOBBY_STEP.eTMLOBBY_STEP_NONE && sOLPOS_DRAWTEXTURE.m_eSetStep < this.m_eLobbyStep)
			{
				return;
			}
			for (int j = 0; j < this.SELECTCOUNT[this.m_nSelectStep]; j++)
			{
				if (this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierKind > 0 && this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierKind != this.m_nSelectSolKind && this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierPos == sOLPOS_DRAWTEXTURE.nPos)
				{
					this.ClearListBoxSelectTextureFromKind(this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierKind);
					this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierKind = 0;
					this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierPos = -1;
				}
			}
			for (int j = 0; j < this.SELECTCOUNT[this.m_nSelectStep]; j++)
			{
				if (this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierKind == this.m_nSelectSolKind && this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierPos >= 0 && this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierPos != sOLPOS_DRAWTEXTURE.nPos && this.m_dtSolPos[(int)this.m_eActiveAlly][this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierPos] != null)
				{
					this.m_dtSolPos[(int)this.m_eActiveAlly][this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierPos].SetTexture("Win_T_ItemEmpty02");
					this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierPos = -1;
				}
			}
			DrawTexture drawTexture2 = this.m_dtSolPos[(int)this.m_eActiveAlly][this.m_RandSolPos.nPos];
			if (drawTexture2 != null)
			{
				drawTexture2.SetTexture(eCharImageType.SMALL, this.m_nSelectSolKind, 0);
				for (int j = 0; j < this.SELECTCOUNT[this.m_nSelectStep]; j++)
				{
					if (this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierKind == this.m_nSelectSolKind)
					{
						this.m_SelectSoldier[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_nSoldierPos = sOLPOS_DRAWTEXTURE.nPos;
					}
				}
			}
		}
	}

	public void SetStep(eTMLOBBY_STEP eStep, short nAtctveAlly)
	{
		this.TurnCountInfo();
		this.m_eLobbyStep = eStep;
		this.m_eActiveAlly = (eBATTLE_ALLY)nAtctveAlly;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				this.m_dtBanTexture[i][j].m_dtFrame.SetTexture("Win_T_ItemEmpty");
			}
		}
		if (eStep >= eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_1 && eStep <= eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_4)
		{
			this.m_nSelectStep = eStep - eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_1;
			this.m_nStartIndex = 0;
			if (eStep == eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_3)
			{
				this.m_nStartIndex = 1;
			}
			else if (eStep == eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_4)
			{
				this.m_nStartIndex = 2;
			}
			for (int i = 0; i < this.SELECTCOUNT[this.m_nSelectStep]; i++)
			{
				this.m_dtBanTexture[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_dtFrame.SetTexture("Win_I_FrameS");
			}
			string text = string.Empty;
			string empty = string.Empty;
			string text2 = this.m_szPlayer1;
			if (this.m_eActiveAlly == eBATTLE_ALLY.eBATTLE_ALLY_1)
			{
				text2 = this.m_szPlayer2;
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2714"),
				"count1",
				text2,
				"count2",
				this.SELECTCOUNT[this.m_nSelectStep].ToString()
			});
			text = this.ExplainColor(text2, empty);
			this.m_lbExplain.SetText(text);
		}
		else if (eStep >= eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_1 && eStep <= eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_4)
		{
			this.m_nSelectStep = eStep - eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_1;
			this.m_nStartIndex = 0;
			if (eStep == eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_3)
			{
				this.m_nStartIndex = 1;
			}
			else if (eStep == eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_4)
			{
				this.m_nStartIndex = 2;
			}
			string text3 = string.Empty;
			string empty2 = string.Empty;
			string text4 = this.m_szPlayer1;
			if (this.m_eActiveAlly == eBATTLE_ALLY.eBATTLE_ALLY_1)
			{
				text4 = this.m_szPlayer2;
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2713"),
				"count1",
				text4,
				"count2",
				this.SELECTCOUNT[this.m_nSelectStep].ToString()
			});
			text3 = this.ExplainColor(text4, empty2);
			this.m_lbExplain.SetText(text3);
		}
		else if (eStep == eTMLOBBY_STEP.eTMLOBBY_STEP_CHANGE_POS)
		{
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					this.m_dtSolPos[i][j].RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSolPos));
					this.m_dtSolPos[i][j].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickChangeSolPos));
				}
			}
			string text5 = this.ExplainColor(string.Empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2767"));
			this.m_lbExplain.SetText(text5);
		}
	}

	public void OnClickOK(IUIObject obj)
	{
		if (this.m_eLobbyStep != eTMLOBBY_STEP.eTMLOBBY_STEP_CHANGE_POS && this.m_eActiveAlly != this.m_eMyAlly)
		{
			return;
		}
		GS_TOURNAMENT_LOBBY_SET_REQ gS_TOURNAMENT_LOBBY_SET_REQ = new GS_TOURNAMENT_LOBBY_SET_REQ();
		gS_TOURNAMENT_LOBBY_SET_REQ.nLobbyIndex = this.m_nLobbyIndex;
		gS_TOURNAMENT_LOBBY_SET_REQ.eLobbyStep = (int)this.m_eLobbyStep;
		int num = 0;
		if (this.m_eLobbyStep != eTMLOBBY_STEP.eTMLOBBY_STEP_CHANGE_POS)
		{
			if (this.m_eLobbyStep >= eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_1 && this.m_eLobbyStep <= eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_4)
			{
				for (int i = 0; i < this.SELECTCOUNT[this.m_nSelectStep]; i++)
				{
					if (this.m_nBanSolKind[(int)this.m_eActiveAlly][i + this.m_nStartIndex] > 0)
					{
						gS_TOURNAMENT_LOBBY_SET_REQ.nSoldierKind[num] = this.m_nBanSolKind[(int)this.m_eActiveAlly][i + this.m_nStartIndex];
						num++;
					}
				}
			}
			else if (this.m_eLobbyStep >= eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_1 && this.m_eLobbyStep <= eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_4)
			{
				for (int i = 0; i < this.SELECTCOUNT[this.m_nSelectStep]; i++)
				{
					if (this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierKind > 0 && this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierPos >= 0)
					{
						gS_TOURNAMENT_LOBBY_SET_REQ.nSoldierKind[num] = this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierKind;
						gS_TOURNAMENT_LOBBY_SET_REQ.nSolPos[num] = (byte)this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierPos;
						num++;
					}
				}
			}
			if (this.SELECTCOUNT[this.m_nSelectStep] != num)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2768"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
		}
		else
		{
			for (int i = 0; i < 3; i++)
			{
				if (this.m_SelectSoldier[(int)this.m_eMyAlly][i].m_nSoldierKind > 0 && this.m_SelectSoldier[(int)this.m_eMyAlly][i].m_nSoldierPos >= 0)
				{
					gS_TOURNAMENT_LOBBY_SET_REQ.nSoldierKind[num] = this.m_SelectSoldier[(int)this.m_eMyAlly][i].m_nSoldierKind;
					gS_TOURNAMENT_LOBBY_SET_REQ.nSolPos[num] = (byte)this.m_SelectSoldier[(int)this.m_eMyAlly][i].m_nSoldierPos;
					num++;
				}
			}
			this.m_btOk[(int)this.m_eMyAlly].Visible = false;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					this.m_dtSolPos[i][j].RemoveValueChangedDelegate(new EZValueChangedDelegate(this.OnClickChangeSolPos));
				}
			}
		}
		if (this.m_FirstSoldierKind != null)
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)this.m_lbSolList.GetItem(this.m_FirstSoldierKind.nItemIndex);
			if (uIListItemContainer != null)
			{
				DrawTexture drawTexture = uIListItemContainer.GetElement(this.SOLFRAME + this.m_FirstSoldierKind.nDataIndex) as DrawTexture;
				if (drawTexture != null)
				{
					Transform child = NkUtil.GetChild(drawTexture.transform, "child_effect");
					if (child != null)
					{
						UnityEngine.Object.Destroy(child.gameObject);
					}
				}
			}
		}
		if (this.m_SecondSoldierKind != null)
		{
			UIListItemContainer uIListItemContainer2 = (UIListItemContainer)this.m_lbSolList.GetItem(this.m_SecondSoldierKind.nItemIndex);
			if (uIListItemContainer2 != null)
			{
				DrawTexture drawTexture2 = uIListItemContainer2.GetElement(this.SOLFRAME + this.m_SecondSoldierKind.nDataIndex) as DrawTexture;
				if (drawTexture2 != null)
				{
					Transform child2 = NkUtil.GetChild(drawTexture2.transform, "child_effect");
					if (child2 != null)
					{
						UnityEngine.Object.Destroy(child2.gameObject);
					}
				}
			}
		}
		this.m_FirstSoldierKind = null;
		this.m_SecondSoldierKind = null;
		this.m_bUpdate = true;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TOURNAMENT_LOBBY_SET_REQ, gS_TOURNAMENT_LOBBY_SET_REQ);
	}

	public void SetData(GS_TOURNAMENT_LOBBY_SET_ACK ACK)
	{
		if (ACK.eLobbyStep >= 2 && ACK.eLobbyStep <= 5)
		{
			int num = ACK.eLobbyStep - 2;
			int num2 = 0;
			if (ACK.eLobbyStep == 4)
			{
				num2 = 1;
			}
			else if (ACK.eLobbyStep == 5)
			{
				num2 = 2;
			}
			for (int i = 0; i < this.SELECTCOUNT[num]; i++)
			{
				this.m_nBanSolKind[(int)ACK.nActiveAlly][i + num2] = ACK.nSoldierKind[i];
				this.m_dtBanTexture[(int)ACK.nActiveAlly][i + num2].m_dtSol.Visible = true;
				this.m_dtBanTexture[(int)ACK.nActiveAlly][i + num2].m_dtSol.SetTexture(eCharImageType.SMALL, ACK.nSoldierKind[i], 0);
				this.ActiveBanMark(ACK.nSoldierKind[i]);
			}
		}
		if (ACK.eLobbyStep >= 6 && ACK.eLobbyStep <= 9)
		{
			int num3 = ACK.eLobbyStep - 6;
			int num4 = 0;
			if (ACK.eLobbyStep == 8)
			{
				num4 = 1;
			}
			else if (ACK.eLobbyStep == 9)
			{
				num4 = 2;
			}
			for (int i = 0; i < this.SELECTCOUNT[num3]; i++)
			{
				this.m_SelectSoldier[(int)ACK.nActiveAlly][i + num4].m_nSoldierKind = ACK.nSoldierKind[i];
				this.m_SelectSoldier[(int)ACK.nActiveAlly][i + num4].m_nSoldierPos = (int)ACK.nSolPos[i];
				this.m_SelectSoldier[(int)ACK.nActiveAlly][i + num4].m_eSetStep = (eTMLOBBY_STEP)ACK.eLobbyStep;
				this.m_dtSolPos[(int)ACK.nActiveAlly][(int)ACK.nSolPos[i]].Visible = true;
				this.m_dtSolPos[(int)ACK.nActiveAlly][(int)ACK.nSolPos[i]].SetTexture(eCharImageType.SMALL, ACK.nSoldierKind[i], 0);
				TournamentLobbyDlg.SOLPOS_DRAWTEXTURE sOLPOS_DRAWTEXTURE = this.m_dtSolPos[(int)ACK.nActiveAlly][(int)ACK.nSolPos[i]].Data as TournamentLobbyDlg.SOLPOS_DRAWTEXTURE;
				if (sOLPOS_DRAWTEXTURE != null)
				{
					sOLPOS_DRAWTEXTURE.m_eSetStep = (eTMLOBBY_STEP)ACK.eLobbyStep;
				}
			}
			this.ClearListBoxSelectTexture();
			this.m_nSelectSolKind = 0;
		}
		else if (ACK.eLobbyStep == 10)
		{
			int nActiveAlly = (int)ACK.nActiveAlly;
			for (int i = 0; i < 9; i++)
			{
				this.m_dtSolPos[nActiveAlly][i].SetTexture("Win_T_ItemEmpty02");
			}
			for (int i = 0; i < 3; i++)
			{
				this.m_SelectSoldier[(int)ACK.nActiveAlly][i].m_nSoldierKind = ACK.nSoldierKind[i];
				this.m_SelectSoldier[(int)ACK.nActiveAlly][i].m_nSoldierPos = (int)ACK.nSolPos[i];
				this.m_dtSolPos[(int)ACK.nActiveAlly][(int)ACK.nSolPos[i]].Visible = true;
				this.m_dtSolPos[(int)ACK.nActiveAlly][(int)ACK.nSolPos[i]].SetTexture(eCharImageType.SMALL, ACK.nSoldierKind[i], 0);
			}
		}
	}

	public void ActiveBanMark(int nBanKind)
	{
		if (this.m_lbSolList.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.m_lbSolList.Count; i++)
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)this.m_lbSolList.GetItem(i);
			if (uIListItemContainer != null)
			{
				for (int j = 0; j < this.m_nSolLineCount; j++)
				{
					ItemTexture itemTexture = uIListItemContainer.GetElement(this.SOLFACE + j) as ItemTexture;
					TOURNAMENT_LOBBY_LISTBOX_DATA tOURNAMENT_LOBBY_LISTBOX_DATA = itemTexture.Data as TOURNAMENT_LOBBY_LISTBOX_DATA;
					if (tOURNAMENT_LOBBY_LISTBOX_DATA != null && tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind == nBanKind)
					{
						DrawTexture drawTexture = uIListItemContainer.GetElement(this.BANMARK + j) as DrawTexture;
						if (drawTexture != null)
						{
							drawTexture.Visible = true;
						}
					}
				}
			}
		}
	}

	public void ClearListBoxSelectTexture()
	{
		if (this.m_lbSolList.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.m_lbSolList.Count; i++)
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)this.m_lbSolList.GetItem(i);
			if (uIListItemContainer != null)
			{
				for (int j = 0; j < this.m_nSolLineCount; j++)
				{
					DrawTexture drawTexture = uIListItemContainer.GetElement(this.SOLFRAME + j) as DrawTexture;
					if (drawTexture != null)
					{
						drawTexture.SetTexture("Win_T_ItemEmpty02");
					}
				}
			}
		}
	}

	public void ClearListBoxSelectTextureFromKind(int nSoldierKind)
	{
		if (this.m_lbSolList.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.m_lbSolList.Count; i++)
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)this.m_lbSolList.GetItem(i);
			if (uIListItemContainer != null)
			{
				for (int j = 0; j < this.m_nSolLineCount; j++)
				{
					ItemTexture itemTexture = uIListItemContainer.GetElement(this.SOLFACE + j) as ItemTexture;
					TOURNAMENT_LOBBY_LISTBOX_DATA tOURNAMENT_LOBBY_LISTBOX_DATA = itemTexture.Data as TOURNAMENT_LOBBY_LISTBOX_DATA;
					if (tOURNAMENT_LOBBY_LISTBOX_DATA != null && tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind == nSoldierKind)
					{
						DrawTexture drawTexture = uIListItemContainer.GetElement(this.SOLFRAME + j) as DrawTexture;
						if (drawTexture != null)
						{
							drawTexture.SetTexture("Win_T_ItemEmpty02");
						}
						return;
					}
				}
			}
		}
	}

	public void OnClickChangeSolPos(IUIObject obj)
	{
		TournamentLobbyDlg.SOLPOS_DRAWTEXTURE sOLPOS_DRAWTEXTURE = obj.Data as TournamentLobbyDlg.SOLPOS_DRAWTEXTURE;
		if (sOLPOS_DRAWTEXTURE == null)
		{
			return;
		}
		if (sOLPOS_DRAWTEXTURE.eAlly != this.m_eMyAlly)
		{
			return;
		}
		if (this.m_nSelectSolKind <= 0)
		{
			int nPos = sOLPOS_DRAWTEXTURE.nPos;
			for (int i = 0; i < 3; i++)
			{
				if (this.m_SelectSoldier[(int)this.m_eMyAlly][i].m_nSoldierPos == nPos)
				{
					this.m_nSelectSolKind = this.m_SelectSoldier[(int)this.m_eMyAlly][i].m_nSoldierKind;
					this.m_dtSolPosFrame[(int)this.m_eMyAlly][nPos].SetTexture("Win_I_FrameS");
					return;
				}
			}
		}
		else
		{
			int nPos2 = sOLPOS_DRAWTEXTURE.nPos;
			for (int j = 0; j < 3; j++)
			{
				if (this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierPos == nPos2 && this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierKind == this.m_nSelectSolKind)
				{
					this.m_nSelectSolKind = 0;
					this.m_dtSolPosFrame[(int)this.m_eMyAlly][nPos2].SetTexture("Win_T_ItemEmpty");
					return;
				}
			}
			int num = 0;
			for (int j = 0; j < 3; j++)
			{
				if (this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierKind != this.m_nSelectSolKind && this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierPos == nPos2)
				{
					num = this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierKind;
					break;
				}
			}
			if (num == 0)
			{
				for (int j = 0; j < 3; j++)
				{
					if (this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierKind == this.m_nSelectSolKind)
					{
						int nSoldierPos = this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierPos;
						this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierPos = nPos2;
						this.m_dtSolPos[(int)this.m_eMyAlly][nSoldierPos].SetTexture("Win_T_ItemEmpty");
						this.m_dtSolPos[(int)this.m_eMyAlly][nPos2].SetTexture(eCharImageType.SMALL, this.m_nSelectSolKind, 0);
						this.m_nSelectSolKind = 0;
						this.m_dtSolPosFrame[(int)this.m_eMyAlly][nSoldierPos].SetTexture("Win_T_ItemEmpty");
						return;
					}
				}
			}
			else
			{
				int num2 = -1;
				for (int j = 0; j < 3; j++)
				{
					if (this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierKind == this.m_nSelectSolKind)
					{
						num2 = this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierPos;
						this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierPos = nPos2;
						this.m_dtSolPos[(int)this.m_eMyAlly][nPos2].SetTexture(eCharImageType.SMALL, this.m_nSelectSolKind, 0);
						break;
					}
				}
				if (num2 < 0)
				{
					return;
				}
				for (int j = 0; j < 3; j++)
				{
					if (this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierKind == num)
					{
						this.m_SelectSoldier[(int)this.m_eMyAlly][j].m_nSoldierPos = num2;
						this.m_dtSolPos[(int)this.m_eMyAlly][num2].SetTexture(eCharImageType.SMALL, num, 0);
						break;
					}
				}
				this.m_nSelectSolKind = 0;
				this.m_dtSolPosFrame[(int)this.m_eMyAlly][num2].SetTexture("Win_T_ItemEmpty");
			}
		}
	}

	public string ExplainColor(string UserName, string ExplainText)
	{
		string result = string.Empty;
		if (this.m_lbPlayer1.Text == UserName)
		{
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1402") + ExplainText;
		}
		else if (this.m_lbPlayer2.Text == UserName)
		{
			result = NrTSingleton<CTextParser>.Instance.GetTextColor("1401") + ExplainText;
		}
		else
		{
			result = ExplainText;
		}
		return result;
	}

	public void TurnCountInfo()
	{
		this.m_TurnTime = (float)COLOSSEUM_CONSTANT_Manager.GetInstance().GetValue(eCOLOSSEUM_CONSTANT.eCOLOSSEUM_CONSTANT_LOBBY_TIMELIMIT);
		this.m_TurnTimeEnd = Time.realtimeSinceStartup + this.m_TurnTime;
		this.m_TurnSwapTime = Time.realtimeSinceStartup;
	}

	public void ShowTurnCount()
	{
		if (!this.m_bUpdate)
		{
			return;
		}
		this.SetVisibleFlag(true);
		long turnNumber = -1L;
		if (this.m_TurnTimeEnd >= Time.realtimeSinceStartup)
		{
			float num = Time.realtimeSinceStartup - this.m_TurnSwapTime;
			turnNumber = (long)(this.m_TurnTime - num);
			this.m_bStop = false;
		}
		else if (this.m_TurnTimeEnd < Time.realtimeSinceStartup)
		{
			this.m_bStop = true;
		}
		this.SetTurnNumber(turnNumber);
	}

	private void SetTurnNumber(long Count)
	{
		string text = NrTSingleton<CTextParser>.Instance.GetTextColor("2002") + Count.ToString();
		this.m_lbTimeCount.SetText(text);
		this.m_lbTimeCount.Visible = true;
		this.m_dwBG.Visible = true;
		if (Count <= 10L)
		{
			if (this.m_goCountDown != null && !this.m_goCountDown.activeInHierarchy)
			{
				this.m_goCountDown.SetActive(true);
			}
			if (this.m_bStop)
			{
				if (this.m_eLobbyStep != eTMLOBBY_STEP.eTMLOBBY_STEP_CHANGE_POS && this.m_eActiveAlly == this.m_eMyAlly)
				{
					string charName = NrTSingleton<NkCharManager>.Instance.GetCharName();
					if (charName == this.m_szPlayer1 || charName == this.m_szPlayer2)
					{
						int num = this.SELECTCOUNT[this.m_nSelectStep];
						for (int i = 0; i < num; i++)
						{
							this.rand();
						}
					}
				}
				else if (this.m_eLobbyStep == eTMLOBBY_STEP.eTMLOBBY_STEP_CHANGE_POS && this.m_eMyAlly != eBATTLE_ALLY.eBATTLE_ALLY_INVALID)
				{
					this.OnClickOK(null);
				}
				this.m_lbTimeCount.Visible = false;
				this.m_dwBG.Visible = false;
				this.m_bStop = false;
				this.m_bUpdate = false;
			}
		}
		else if (this.m_goCountDown != null && this.m_goCountDown.activeInHierarchy)
		{
			this.m_goCountDown.SetActive(false);
		}
	}

	public void effectdownload(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		this.m_goCountDown = obj;
		this.m_goCountDown.SetActive(false);
		Vector3 localPosition = this.m_goCountDown.transform.localPosition;
		localPosition.z = 1f;
		this.m_goCountDown.transform.localPosition = localPosition;
	}

	public void SetVisibleFlag(bool bShow)
	{
		if (base.Visible != bShow)
		{
			base.Visible = bShow;
		}
		if (bShow)
		{
			this.InitData();
		}
	}

	public void rand()
	{
		int colosseumEnableBatchSoldierKindCount = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetColosseumEnableBatchSoldierKindCount();
		int iIndex = UnityEngine.Random.Range(0, colosseumEnableBatchSoldierKindCount);
		int colosseumEnableBatchSoldierKind = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetColosseumEnableBatchSoldierKind(iIndex);
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (this.m_nBanSolKind[i][j] > 0 && this.m_nBanSolKind[i][j] == colosseumEnableBatchSoldierKind)
				{
					this.rand();
					return;
				}
			}
		}
		for (int i = 0; i < this.SELECTCOUNT[this.m_nSelectStep]; i++)
		{
			if (this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierKind > 0 && this.m_SelectSoldier[(int)this.m_eActiveAlly][i + this.m_nStartIndex].m_nSoldierKind == colosseumEnableBatchSoldierKind)
			{
				this.rand();
				return;
			}
		}
		if (this.m_lbSolList.Count <= 0)
		{
			return;
		}
		for (int k = 0; k < this.m_lbSolList.Count; k++)
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)this.m_lbSolList.GetItem(k);
			if (uIListItemContainer != null)
			{
				for (int l = 0; l < this.m_nSolLineCount; l++)
				{
					ItemTexture itemTexture = uIListItemContainer.GetElement(this.SOLFACE + l) as ItemTexture;
					TOURNAMENT_LOBBY_LISTBOX_DATA tOURNAMENT_LOBBY_LISTBOX_DATA = itemTexture.Data as TOURNAMENT_LOBBY_LISTBOX_DATA;
					if (tOURNAMENT_LOBBY_LISTBOX_DATA != null && tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind == colosseumEnableBatchSoldierKind)
					{
						this.m_RandSolInfo.nSoldierKind = tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind;
						this.m_RandSolInfo.nItemIndex = tOURNAMENT_LOBBY_LISTBOX_DATA.nItemIndex;
						this.m_RandSolInfo.nDataIndex = tOURNAMENT_LOBBY_LISTBOX_DATA.nDataIndex;
					}
				}
			}
		}
		if (this.m_eLobbyStep >= eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_1 && this.m_eLobbyStep <= eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_4)
		{
			this.btClickSol(null);
			this.OnClickOK(null);
		}
		else if (this.m_eLobbyStep >= eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_1 && this.m_eLobbyStep <= eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_4)
		{
			if (this.m_eLobbyStep == eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_1 || this.m_eLobbyStep == eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_3)
			{
				if (this.m_FirstSoldierKind != null)
				{
					this.ClearListBoxSelectTextureFromKind(this.m_FirstSoldierKind.nSoldierKind);
					this.ClickSolClear();
				}
				else if (this.m_SecondSoldierKind != null)
				{
					this.ClearListBoxSelectTextureFromKind(this.m_SecondSoldierKind.nSoldierKind);
					this.ClickSolClear();
				}
				for (int i = 0; i < this.m_SelectSoldier[(int)this.m_eActiveAlly].Length; i++)
				{
					if (this.m_SelectSoldier[(int)this.m_eActiveAlly][i].m_nSoldierKind > 0 && this.m_SelectSoldier[(int)this.m_eActiveAlly][i].m_nSoldierPos == this.nPos0)
					{
						this.nPos0++;
					}
				}
				this.m_RandSolPos.eAlly = eBATTLE_ALLY.eBATTLE_ALLY_0;
				this.m_RandSolPos.nPos = this.nPos0;
				this.m_RandSolPos.m_eSetStep = eTMLOBBY_STEP.eTMLOBBY_STEP_NONE;
			}
			else
			{
				if (this.m_FirstSoldierKind != null)
				{
					this.ClearListBoxSelectTextureFromKind(this.m_FirstSoldierKind.nSoldierKind);
					this.ClickSolClear();
				}
				else if (this.m_SecondSoldierKind != null)
				{
					this.ClearListBoxSelectTextureFromKind(this.m_SecondSoldierKind.nSoldierKind);
					this.ClickSolClear();
				}
				for (int i = 0; i < this.m_SelectSoldier[(int)this.m_eActiveAlly].Length; i++)
				{
					if (this.m_SelectSoldier[(int)this.m_eActiveAlly][i].m_nSoldierKind > 0 && this.m_SelectSoldier[(int)this.m_eActiveAlly][i].m_nSoldierPos == this.nPos1)
					{
						this.nPos1++;
					}
				}
				this.m_RandSolPos.eAlly = eBATTLE_ALLY.eBATTLE_ALLY_1;
				this.m_RandSolPos.nPos = this.nPos1;
				this.m_RandSolPos.m_eSetStep = eTMLOBBY_STEP.eTMLOBBY_STEP_NONE;
			}
			this.btClickSol(null);
			this.OnClickSolPos(null);
			this.OnClickOK(null);
		}
		this.m_RandSolInfo.nSoldierKind = 0;
		this.m_RandSolInfo.nItemIndex = 0;
		this.m_RandSolInfo.nDataIndex = 0;
	}

	public void ClickSolClear()
	{
		TOURNAMENT_LOBBY_LISTBOX_DATA tOURNAMENT_LOBBY_LISTBOX_DATA = null;
		int num = this.SELECTCOUNT[this.m_nSelectStep];
		for (int i = 0; i < num; i++)
		{
			if (this.m_FirstSoldierKind != null)
			{
				tOURNAMENT_LOBBY_LISTBOX_DATA = this.m_FirstSoldierKind;
			}
			else if (this.m_SecondSoldierKind != null)
			{
				tOURNAMENT_LOBBY_LISTBOX_DATA = this.m_SecondSoldierKind;
			}
			if (tOURNAMENT_LOBBY_LISTBOX_DATA == null || tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind <= 0)
			{
				return;
			}
			if (this.m_eActiveAlly != this.m_eMyAlly)
			{
				return;
			}
			if (this.m_eLobbyStep >= eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_1 && this.m_eLobbyStep <= eTMLOBBY_STEP.eTMLOBBY_STEP_BAN_4)
			{
				bool flag = false;
				for (int j = 0; j < num; j++)
				{
					if (this.m_nBanSolKind[(int)this.m_eActiveAlly][j + this.m_nStartIndex] == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
					{
						flag = true;
					}
				}
				bool flag2 = true;
				if (!flag)
				{
					int num2 = 0;
					for (int j = 0; j < num; j++)
					{
						if (this.m_nBanSolKind[(int)this.m_eActiveAlly][j + this.m_nStartIndex] > 0)
						{
							num2++;
						}
					}
					if (num2 == num)
					{
						flag2 = false;
					}
				}
				if (!flag && !flag2)
				{
					return;
				}
				UIListItemContainer uIListItemContainer = (UIListItemContainer)this.m_lbSolList.GetItem(tOURNAMENT_LOBBY_LISTBOX_DATA.nItemIndex);
				if (uIListItemContainer != null)
				{
					DrawTexture drawTexture = uIListItemContainer.GetElement(this.SOLFRAME + tOURNAMENT_LOBBY_LISTBOX_DATA.nDataIndex) as DrawTexture;
					if (drawTexture != null)
					{
						drawTexture.SetTexture("Win_T_ItemEmpty02");
						Transform child = NkUtil.GetChild(drawTexture.transform, "child_effect");
						if (child != null)
						{
							UnityEngine.Object.Destroy(child.gameObject);
						}
						for (int j = 0; j < this.SELECTCOUNT[this.m_nSelectStep]; j++)
						{
							if (this.m_nBanSolKind[(int)this.m_eActiveAlly][j + this.m_nStartIndex] == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
							{
								this.m_nBanSolKind[(int)this.m_eActiveAlly][j + this.m_nStartIndex] = 0;
								this.m_dtBanTexture[(int)this.m_eActiveAlly][j + this.m_nStartIndex].m_dtSol.Visible = false;
							}
						}
					}
				}
			}
			if (this.m_eLobbyStep >= eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_1 && this.m_eLobbyStep <= eTMLOBBY_STEP.eTMLOBBY_STEP_SELECT_4)
			{
				bool flag3 = false;
				for (int k = 0; k < 2; k++)
				{
					for (int l = 0; l < 3; l++)
					{
						if (this.m_nBanSolKind[k][l] > 0 && this.m_nBanSolKind[k][l] == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
						{
							return;
						}
					}
				}
				for (int k = 0; k < 3; k++)
				{
					if (this.m_SelectSoldier[(int)this.m_eMyAlly][k].m_eSetStep != eTMLOBBY_STEP.eTMLOBBY_STEP_NONE && this.m_SelectSoldier[(int)this.m_eMyAlly][k].m_eSetStep < this.m_eLobbyStep && this.m_SelectSoldier[(int)this.m_eMyAlly][k].m_nSoldierKind == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
					{
						return;
					}
				}
				for (int k = 0; k < num; k++)
				{
					if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierKind == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
					{
						flag3 = true;
					}
				}
				bool flag4 = true;
				if (!flag3)
				{
					int num3 = 0;
					for (int k = 0; k < num; k++)
					{
						if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierKind > 0)
						{
							if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierPos < 0)
							{
								return;
							}
							num3++;
						}
					}
					if (num3 == num)
					{
						flag4 = false;
					}
				}
				if (!flag3 && !flag4)
				{
					return;
				}
				UIListItemContainer uIListItemContainer2 = (UIListItemContainer)this.m_lbSolList.GetItem(tOURNAMENT_LOBBY_LISTBOX_DATA.nItemIndex);
				if (uIListItemContainer2 != null)
				{
					DrawTexture drawTexture2 = uIListItemContainer2.GetElement(this.SOLFRAME + tOURNAMENT_LOBBY_LISTBOX_DATA.nDataIndex) as DrawTexture;
					if (drawTexture2 != null)
					{
						drawTexture2.SetTexture("Win_T_ItemEmpty02");
						Transform child2 = NkUtil.GetChild(drawTexture2.transform, "child_effect");
						if (child2 != null)
						{
							UnityEngine.Object.Destroy(child2.gameObject);
						}
						for (int k = 0; k < this.SELECTCOUNT[this.m_nSelectStep]; k++)
						{
							if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierKind == tOURNAMENT_LOBBY_LISTBOX_DATA.nSoldierKind)
							{
								this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierKind = 0;
								if (this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierPos >= 0)
								{
									int nSoldierPos = this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierPos;
									this.m_dtSolPos[(int)this.m_eActiveAlly][nSoldierPos].SetTexture("Win_T_ItemEmpty02");
									this.m_SelectSoldier[(int)this.m_eActiveAlly][k + this.m_nStartIndex].m_nSoldierPos = -1;
								}
							}
						}
					}
				}
			}
			if (i == 0)
			{
				this.m_FirstSoldierKind = null;
			}
			else
			{
				this.m_SecondSoldierKind = null;
			}
		}
	}
}
