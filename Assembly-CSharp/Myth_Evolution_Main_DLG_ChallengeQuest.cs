using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Myth_Evolution_Main_DLG_ChallengeQuest : Myth_Evolution_Main_DLG
{
	private int m_iChallengeQuestUnique = -1;

	private bool m_bLegendListboxSetComplete;

	private int m_iDummyCharKind = -1;

	private List<int> guideWinIDList = new List<int>();

	private UIButton _Touch;

	public int ChallengeQuestUnique
	{
		get
		{
			return this.m_iChallengeQuestUnique;
		}
		set
		{
			this.m_iChallengeQuestUnique = value;
		}
	}

	public bool IsLegendListBoxSetComplete
	{
		get
		{
			return this.m_bLegendListboxSetComplete;
		}
		private set
		{
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Soldier/Evolution/DLG_SolEvolution_Main", G_ID.MYTH_EVOLUTION_MAIN_CHALLENGEQUEST_DLG, false);
		this.m_iDummyCharKind = 1053;
	}

	public override void Update()
	{
		base.Update();
		if (!this.m_bLegendListboxSetComplete && this.m_NewListBox.GetReserverItemCount() == 0)
		{
			this.m_bLegendListboxSetComplete = this.ScrollToTargetDummy();
			NrTSingleton<EventConditionHandler>.Instance.MythEvolutionSet.OnTrigger();
		}
	}

	public override void OnSoldierInfo(IUIObject obj)
	{
		NrCharKindInfo nrCharKindInfo = obj.Data as NrCharKindInfo;
		if (nrCharKindInfo == null || nrCharKindInfo.GetCharKind() != this.m_iDummyCharKind)
		{
			return;
		}
		this.HideTouch(true);
		Myth_Legend_Info_DLG_ChallengeQuest myth_Legend_Info_DLG_ChallengeQuest = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_LEGEND_INFO_CHALLENGEQUEST_DLG) as Myth_Legend_Info_DLG_ChallengeQuest;
		if (myth_Legend_Info_DLG_ChallengeQuest != null)
		{
			myth_Legend_Info_DLG_ChallengeQuest.InitSetCharKind(this.m_iDummyCharKind);
		}
	}

	protected override void Change_Season(IUIObject obj)
	{
	}

	public void InitDummyUI()
	{
		this.SetLegendChallengeQuest();
		this.m_DropDownList_Season.Clear();
		this.m_DropDownList_Season.SetIndex(-1);
		this.m_DropDownList_Season.controlIsEnabled = false;
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (string.IsNullOrEmpty(param1))
		{
			return;
		}
		if (this.guideWinIDList != null && !this.guideWinIDList.Contains(winID))
		{
			this.guideWinIDList.Add(winID);
		}
		string[] array = param1.Split(new char[]
		{
			','
		});
		if (array == null || array.Length != 4)
		{
			return;
		}
		UIListItemContainer listItemContainer = this.GetListItemContainer(this.m_iDummyCharKind);
		AutoSpriteControlBase charKindContainer = this.GetCharKindContainer(listItemContainer, this.m_iDummyCharKind);
		if (charKindContainer == null)
		{
			return;
		}
		if (this._Touch == null)
		{
			this._Touch = UICreateControl.Button("touch", "Main_I_Touch01", 196f, 154f);
		}
		if (this._Touch == null)
		{
			return;
		}
		int anchor = int.Parse(array[1]);
		this._Touch.SetAnchor((SpriteRoot.ANCHOR_METHOD)anchor);
		this._Touch.PlayAni(true);
		this._Touch.gameObject.SetActive(true);
		this._Touch.gameObject.transform.parent = charKindContainer.gameObject.transform;
		this._Touch.transform.position = new Vector3(charKindContainer.transform.position.x, charKindContainer.transform.position.y, charKindContainer.transform.position.z - 3f);
		float x = float.Parse(array[2]);
		float y = float.Parse(array[3]);
		this._Touch.transform.eulerAngles = new Vector3(x, y, this._Touch.transform.eulerAngles.z);
		BoxCollider component = this._Touch.gameObject.GetComponent<BoxCollider>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	public void OnLegendEvolutionDirectionEnd()
	{
		this.ShowGameGuideDlg(GameGuideType.CHALLENGE_LEGEND_EVOLUTION);
	}

	public static void OnGuideEnd()
	{
		Myth_Evolution_Main_DLG_ChallengeQuest.OnShowHelpDlg(eHELP_LIST.Soldier_Descent);
		GS_RECOMMEND_CHALLENGE_CLEAR_REQ gS_RECOMMEND_CHALLENGE_CLEAR_REQ = new GS_RECOMMEND_CHALLENGE_CLEAR_REQ();
		gS_RECOMMEND_CHALLENGE_CLEAR_REQ.i32RecommendChallengeUnique = 1504;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_RECOMMEND_CHALLENGE_CLEAR_REQ, gS_RECOMMEND_CHALLENGE_CLEAR_REQ);
	}

	private bool ScrollToTargetDummy()
	{
		UIListItemContainer listItemContainer = this.GetListItemContainer(this.m_iDummyCharKind);
		if (listItemContainer == null)
		{
			return false;
		}
		this.m_NewListBox.ScrollListTo_Internal(this.GetNewListBoxTargetPos(this.m_NewListBox.Count, listItemContainer));
		return true;
	}

	private float GetNewListBoxTargetPos(int maxCount, UIListItemContainer moveTargetItem)
	{
		if (moveTargetItem == null)
		{
			return 0f;
		}
		int index = moveTargetItem.index;
		return (float)index / (float)maxCount;
	}

	private UIListItemContainer GetListItemContainer(int charkind)
	{
		List<UIListItemContainer> items = this.m_NewListBox.GetItems();
		foreach (UIListItemContainer current in items)
		{
			if (!(current == null))
			{
				if (this.IsExistCharKind(current, charkind))
				{
					return current;
				}
			}
		}
		return null;
	}

	private bool IsExistCharKind(UIListItemContainer item, int pTargetCharKind)
	{
		AutoSpriteControlBase charKindContainer = this.GetCharKindContainer(item, pTargetCharKind);
		return !(charKindContainer == null);
	}

	private AutoSpriteControlBase GetCharKindContainer(UIListItemContainer item, int pTargetCharKind)
	{
		if (item == null)
		{
			return null;
		}
		int objCount = item.GetObjCount();
		for (int i = 0; i < objCount; i++)
		{
			object elementObject = item.GetElementObject(i);
			if (elementObject != null)
			{
				if (elementObject is NrCharKindInfo)
				{
					NrCharKindInfo nrCharKindInfo = (NrCharKindInfo)elementObject;
					if (nrCharKindInfo.GetCharKind() == pTargetCharKind)
					{
						return item.GetElement(i);
					}
				}
			}
		}
		return null;
	}

	private void ShowGameGuideDlg(GameGuideType guideType)
	{
		NrTSingleton<GameGuideManager>.Instance.Update(GameGuideCheck.NONE, guideType);
		NrTSingleton<GameGuideManager>.Instance.Update();
		OnCloseCallback callback = null;
		if (this.ChallengeQuestUnique == 1504)
		{
			callback = new OnCloseCallback(Myth_Evolution_Main_DLG_ChallengeQuest.OnGuideEnd);
		}
		GameGuideDlg gameGuideDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAMEGUIDE_DLG) as GameGuideDlg;
		gameGuideDlg.RegistCloseCallback(callback);
	}

	public static void OnShowHelpDlg(eHELP_LIST helpInfo)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(helpInfo.ToString());
		}
	}

	private void HideTouch(bool closeUI)
	{
		if (this._Touch != null && this._Touch.gameObject != null)
		{
			this._Touch.gameObject.SetActive(false);
		}
		if (!closeUI)
		{
			return;
		}
		if (this.guideWinIDList == null)
		{
			return;
		}
		foreach (int current in this.guideWinIDList)
		{
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)current) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				uI_UIGuide.CloseUI = true;
			}
		}
		this._Touch = null;
	}

	private void SetLegendChallengeQuest()
	{
		base.SetLegend();
		if (this.m_ToolBar != null)
		{
			this.m_ToolBar.Control_Tab[1].controlIsEnabled = false;
		}
	}
}
