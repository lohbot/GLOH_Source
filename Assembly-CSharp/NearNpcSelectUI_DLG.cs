using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class NearNpcSelectUI_DLG : Form
{
	private const string BUTTON_TEXTURE_01 = "Win_B_NewTileBtnOrange";

	private const string BUTTON_TEXTURE_02 = "Win_B_BattleControl";

	private Button m_Button_Npc;

	private DrawTexture m_DrawTexture_NpcIcon;

	private short m_i16CharUnique;

	private short m_nOldCharUnique;

	private bool m_bMove;

	private int m_nWinID;

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 1f);
		this.m_nWinID = winID;
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide == null)
		{
			return;
		}
		uI_UIGuide.m_nOtherWinID = G_ID.NEARNPCSELECTUI_DLG;
		Vector3 v = new Vector2(base.GetLocationX() + this.m_Button_Npc.GetLocationX() + 50f, base.GetLocationY() + this.m_Button_Npc.GetLocationY());
		Vector2 x = new Vector2(base.GetLocationX() - 100f, base.GetLocationY() + 180f);
		uI_UIGuide.Move(v, x);
	}

	public override void Update()
	{
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "DLG_NpcSelect", G_ID.NEARNPCSELECTUI_DLG, false);
		float x = (GUICamera.width - base.GetSize().x) / 2f;
		float num = (GUICamera.height - base.GetSize().y) / 2f;
		num = GUICamera.height * 0.3f;
		base.SetLocation(x, -num);
		base.DonotDepthChange(UIPanelManager.UI_DEPTH);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_Button_Npc = (base.GetControl("Button_Npc") as Button);
		this.m_Button_Npc.SetMultiLine(false);
		Button expr_28 = this.m_Button_Npc;
		expr_28.Click = (EZValueChangedDelegate)Delegate.Combine(expr_28.Click, new EZValueChangedDelegate(this.NpcClick));
		this.m_DrawTexture_NpcIcon = (base.GetControl("DrawTexture_NpcIcon") as DrawTexture);
	}

	public void SetCharKind(short i16CharUnique, NrCharKindInfo charKindInfo, QUEST_CONST.eQUESTSTATE eState)
	{
		int charKind = charKindInfo.GetCharKind();
		if (this.m_nOldCharUnique == i16CharUnique && charKindInfo.IsATB(8L))
		{
			this.QuestSymbol(charKind, eState);
			this.m_Button_Npc.SetButtonTextureKey("Win_B_NewTileBtnOrange");
			return;
		}
		if (this.m_nOldCharUnique == i16CharUnique && charKindInfo.IsATB(16L))
		{
			this.QuestSymbol(charKind, eState);
			this.m_Button_Npc.SetButtonTextureKey("Win_B_NewTileBtnOrange");
			return;
		}
		if (this.m_nOldCharUnique == i16CharUnique && charKindInfo.IsATB(4L))
		{
			this.BattleSymbol();
			this.m_Button_Npc.SetButtonTextureKey("Win_B_BattleControl");
			return;
		}
		this.m_i16CharUnique = i16CharUnique;
		this.m_nOldCharUnique = i16CharUnique;
		string name = NrTSingleton<NrCharKindInfoManager>.Instance.GetName(charKind);
		if (name.Length > 5)
		{
			float num;
			if (NrGlobalReference.strLangType.Equals("eng"))
			{
				num = (float)name.Length * this.m_Button_Npc.fontSize / 2f;
			}
			else
			{
				num = (float)name.Length * this.m_Button_Npc.fontSize;
			}
			base.SetSize(num + 50f, base.GetSize().y);
			this.m_Button_Npc.SetSize(num, this.m_Button_Npc.GetSize().y);
		}
		this.m_Button_Npc.Text = name;
		if (this.m_nOldCharUnique == i16CharUnique && charKindInfo.IsATB(8L))
		{
			this.QuestSymbol(charKind, eState);
			this.m_Button_Npc.SetButtonTextureKey("Win_B_NewTileBtnOrange");
		}
		else if (this.m_nOldCharUnique == i16CharUnique && charKindInfo.IsATB(16L))
		{
			this.QuestSymbol(charKind, eState);
			this.m_Button_Npc.SetButtonTextureKey("Win_B_NewTileBtnOrange");
		}
		else if (this.m_nOldCharUnique == i16CharUnique && charKindInfo.IsATB(4L))
		{
			this.BattleSymbol();
			this.m_Button_Npc.SetButtonTextureKey("Win_B_BattleControl");
		}
	}

	public void NpcClick(IUIObject obj)
	{
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(this.m_i16CharUnique);
		if (charByCharUnique == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = charByCharUnique.GetCharKindInfo();
		if (charKindInfo == null)
		{
			return;
		}
		if (!charByCharUnique.IsCharKindATB(16L))
		{
			if (charByCharUnique.IsCharKindATB(8L))
			{
				if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
				{
					return;
				}
				if (charByCharUnique.IsCharKindATB(562949953421312L))
				{
					GS_TREASUREBOX_CLICK_REQ gS_TREASUREBOX_CLICK_REQ = new GS_TREASUREBOX_CLICK_REQ();
					gS_TREASUREBOX_CLICK_REQ.i32CharUnique = (int)charByCharUnique.GetCharUnique();
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TREASUREBOX_CLICK_REQ, gS_TREASUREBOX_CLICK_REQ);
					return;
				}
				if (charByCharUnique.IsCharKindATB(1125899906842624L))
				{
					if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsBountyHunt())
					{
						return;
					}
					GS_BABELTOWER_GOLOBBY_REQ gS_BABELTOWER_GOLOBBY_REQ = new GS_BABELTOWER_GOLOBBY_REQ();
					gS_BABELTOWER_GOLOBBY_REQ.mode = 0;
					gS_BABELTOWER_GOLOBBY_REQ.babel_floor = 0;
					gS_BABELTOWER_GOLOBBY_REQ.babel_subfloor = 0;
					gS_BABELTOWER_GOLOBBY_REQ.nPersonID = 0L;
					gS_BABELTOWER_GOLOBBY_REQ.i16BountyHuntUnique = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_GOLOBBY_REQ, gS_BABELTOWER_GOLOBBY_REQ);
					return;
				}
				else
				{
					NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(30, (long)charKindInfo.GetCharKind(), 1L);
					NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(8, (long)charKindInfo.GetCharKind(), 1L);
					NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(99, (long)charKindInfo.GetCharKind(), 1L);
					NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(48, (long)charKindInfo.GetCharKind(), 1L);
					if (charByCharUnique.GetCharUnique() >= 31300 && charByCharUnique.GetCharUnique() <= 31400)
					{
						string text = NrTSingleton<NkQuestManager>.Instance.IsCheckCodeANDParam(QUEST_CONST.eQUESTCODE.QUESTCODE_TAKECHAR, (long)charKindInfo.GetCharKind());
						if (text != string.Empty)
						{
							TakeTalk_DLG takeTalk_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TAKETALK_DLG) as TakeTalk_DLG;
							if (takeTalk_DLG != null)
							{
								takeTalk_DLG.SetNpc(charKindInfo.GetCharKind(), charByCharUnique.GetCharUnique(), text);
								takeTalk_DLG.Show();
							}
							return;
						}
					}
					if (charByCharUnique.GetCharUnique() >= 31005 && charByCharUnique.GetCharUnique() <= 31200)
					{
						return;
					}
					NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NPCTALK_DLG);
					npcTalkUI_DLG.SetNpcID(charKindInfo.GetCharKind(), charByCharUnique.GetCharUnique());
					npcTalkUI_DLG.Show();
				}
			}
			else if (charByCharUnique.IsCharKindATB(4L))
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char == null)
				{
					return;
				}
				@char.MoveTo(charByCharUnique.GetCharObject().transform.position);
				NrTSingleton<NkClientLogic>.Instance.SetPickChar(charByCharUnique);
				if (charKindInfo.GetCHARKIND_MONSTERINFO() != null)
				{
					NrTSingleton<GameGuideManager>.Instance.MonsterLevel = (int)charKindInfo.GetCHARKIND_MONSTERINFO().MINLEVEL;
				}
				else
				{
					NrTSingleton<GameGuideManager>.Instance.MonsterLevel = 0;
				}
				this.Close();
				return;
			}
			return;
		}
		if (charByCharUnique.IsCharKindATB(268435456L))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("39"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			return;
		}
		NrCharBase char2 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (char2 == null)
		{
			return;
		}
		char2.MoveTo(charByCharUnique.GetCharObject().transform.position);
		NrTSingleton<NkClientLogic>.Instance.SetPickChar(charByCharUnique);
		this.Close();
	}

	private void QuestSymbol(int i32CharKind, QUEST_CONST.eQUESTSTATE eState)
	{
		this.m_DrawTexture_NpcIcon.Visible = true;
		if (eState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_NOT_ACCEPTABLE_NOT_VIEW && eState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_NONE && eState != QUEST_CONST.eQUESTSTATE.QUESTSTATE_END)
		{
			if (eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
			{
				this.m_DrawTexture_NpcIcon.SetTexture("NPC_I_QuestI11");
			}
			else if (eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_DAYQUEST_COMPLETE)
			{
				this.m_DrawTexture_NpcIcon.SetTexture("NPC_I_QuestI13");
			}
			else if (eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING)
			{
				this.m_DrawTexture_NpcIcon.SetTexture("NPC_I_QuestI12");
			}
			else if (eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
			{
				this.m_DrawTexture_NpcIcon.SetTexture("NPC_I_QuestI21");
			}
			else if (eState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_DAYQUEST_ACCEPTABLE)
			{
				this.m_DrawTexture_NpcIcon.SetTexture("NPC_I_QuestI23");
			}
		}
		else
		{
			this.m_DrawTexture_NpcIcon.SetTexture("NPC_I_Nomal");
		}
	}

	private void BattleSymbol()
	{
		this.m_DrawTexture_NpcIcon.Visible = true;
		this.m_DrawTexture_NpcIcon.SetTexture("Main_I_ForceIcon");
		if (null != base.BG)
		{
			base.ChangeBG("Main_T_AreaBg5");
		}
	}

	public override void Show()
	{
		if (TsPlatform.IsMobile && base.Visible)
		{
			return;
		}
		if (!this.m_bMove)
		{
			return;
		}
		this.m_bMove = false;
		base.Show();
	}

	public override void Close()
	{
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.UIGUIDE_DLG) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		base.Close();
	}
}
