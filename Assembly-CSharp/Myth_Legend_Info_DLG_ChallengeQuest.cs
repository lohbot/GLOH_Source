using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Myth_Legend_Info_DLG_ChallengeQuest : Myth_Legend_Info_DLG
{
	private int m_iChallengeQuestUnique = -1;

	private bool m_bLegendListboxSetComplete;

	private int m_iDummyCharKind = -1;

	private bool m_bElementListItemSetComplete;

	private List<int> guideWinIDList = new List<int>();

	private UIButton _Touch;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Soldier/Evolution/DLG_SolEvolution_Legend_Info", G_ID.MYTH_LEGEND_INFO_CHALLENGEQUEST_DLG, false);
	}

	public override void Update()
	{
		base.Update();
		if (this.m_NewListBox_Reincarnate != null && !this.m_bElementListItemSetComplete && this.m_NewListBox_Reincarnate.Count == 5)
		{
			NrTSingleton<EventConditionHandler>.Instance.MythEvolutionListSet.OnTrigger();
			this.m_bElementListItemSetComplete = true;
		}
	}

	protected override void OnClickReincarnate(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_LEGENDMAKETIME);
			long curTime = PublicMethod.GetCurTime();
			if (curTime < charSubData)
			{
				Myth_Evolution_Time_DLG myth_Evolution_Time_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_TIME_DLG) as Myth_Evolution_Time_DLG;
				if (myth_Evolution_Time_DLG != null)
				{
					myth_Evolution_Time_DLG.InitSet(MYTH_TYPE.MYTHTYPE_LEGEND, this.m_CharKind_Legendinfo.i32Element_LegendCharkind, 0L);
				}
				return;
			}
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1706");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("164");
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnReincarnateOK), null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL, 2);
		NrTSingleton<EventConditionHandler>.Instance.MythEvolutionInfoMsgBox.OnTrigger();
		this.HideTouch(false);
	}

	protected override void Click_PreViewHero(IUIObject obj)
	{
	}

	protected override void ClickHelp(IUIObject obj)
	{
	}

	public override void InitSetCharKind(int i32CharKind)
	{
		this.m_iDummyCharKind = i32CharKind;
		base.SetShowLayer(1, false);
		this.m_Label_LegendTime.Text = string.Empty;
		this.InitLegendDataSet(i32CharKind);
	}

	public void ReadyLegendEvolution()
	{
		this.m_btn_Legend.SetEnabled(true);
		int num = 4;
		NewListItem item = new NewListItem(this.m_NewListBox_Reincarnate.ColumnNum, true, string.Empty);
		this.SetLegendReincarnateListBox(ref item, num, this.m_CharKind_Legendinfo.i32Base_CharKind[num], this.m_CharKind_Legendinfo.ui8Base_LegendGrade[num], false);
		this.m_NewListBox_Reincarnate.RemoveAdd(num, item);
		this.m_NewListBox_Reincarnate.RepositionItems();
		NrTSingleton<EventConditionHandler>.Instance.MythEvolutionListReady.OnTrigger();
	}

	public override void OnReincarnateOK(object a_oObject)
	{
		SolElementSuccessDlg solElementSuccessDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLELEMENTSUCCESS_DLG) as SolElementSuccessDlg;
		if (solElementSuccessDlg != null)
		{
			solElementSuccessDlg.LoadLegendSolCompleteElement(new SOLDIER_INFO
			{
				CharKind = this.m_iDummyCharKind,
				Grade = 6
			});
		}
		this.Close();
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
		string a = array[0];
		IUIObject iUIObject = null;
		if (a == "MATERIAL_GUIDE")
		{
			int index = 4;
			UIListItemContainer item = this.m_NewListBox_Reincarnate.GetItem(index);
			if (item == null)
			{
				return;
			}
			iUIObject = item.GetElement(10);
		}
		else if (a == "EVOLUTION_GUIDE")
		{
			iUIObject = base.GetControl("btn_Legend");
		}
		if (iUIObject == null)
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
		this._Touch.gameObject.transform.parent = iUIObject.gameObject.transform;
		this._Touch.transform.position = new Vector3(iUIObject.transform.position.x, iUIObject.transform.position.y, iUIObject.transform.position.z - 3f);
		float x = float.Parse(array[2]);
		float y = float.Parse(array[3]);
		this._Touch.transform.eulerAngles = new Vector3(x, y, this._Touch.transform.eulerAngles.z);
		BoxCollider component = this._Touch.gameObject.GetComponent<BoxCollider>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	private void InitLegendDataSet(int i32CharKind)
	{
		this.m_btn_Legend.SetEnabled(false);
		this.m_CharKind_Legendinfo = NrTSingleton<NrBaseTableManager>.Instance.GetLegendGuide_Col(i32CharKind);
		if (this.m_CharKind_Legendinfo == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_CharKind_Legendinfo.i32Element_LegendCharkind);
		if (charKindInfo == null)
		{
			return;
		}
		this.m_Label_character_name.SetText(charKindInfo.GetName());
		byte b = this.m_CharKind_Legendinfo.ui8Element_LegendGrade - 1;
		short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(i32CharKind, (int)b);
		this.m_DrawTexture_character.SetTextureEffect(eCharImageType.LARGE, i32CharKind, (int)b, string.Empty);
		UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(i32CharKind, (int)b);
		if (0 < legendType)
		{
			this.m_DrawTexture_rank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
		}
		this.m_DrawTexture_rank.SetTexture(solLargeGradeImg);
		int weaponType = charKindInfo.GetWeaponType();
		this.m_DT_WEAPON.SetTexture(string.Format("Win_I_Weapon{0}", weaponType.ToString()));
		int season = charKindInfo.GetSeason(b);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1208"),
			"count",
			season + 1
		});
		this.m_Label_SeasonNum.SetText(empty);
		SOL_GUIDE solGuild = NrTSingleton<NrTableSolGuideManager>.Instance.GetSolGuild(this.m_CharKind_Legendinfo.i32Element_LegendCharkind);
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(solGuild.m_i32SkillUnique);
		if (battleSkillBase != null)
		{
			UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase.m_nSkillUnique);
			this.m_DT_SKILLICON.SetTexture(battleSkillIconTexture);
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1293"),
				"skillname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey)
			});
		}
		else
		{
			this.m_DT_SKILLICON.SetTexture(string.Empty);
		}
		this.m_Label_Gold2.SetText(ANNUALIZED.Convert(this.m_CharKind_Legendinfo.i64NeedMoney));
		this.m_Label_Essence2.SetText(ANNUALIZED.Convert(this.m_CharKind_Legendinfo.i32NeedEssence));
		this.m_NewListBox_Reincarnate.Clear();
		for (int i = 0; i < 5; i++)
		{
			this.m_eElement_Msg[i] = eElement_MsgType.eElement_NONE;
			if (i32CharKind != 0)
			{
				bool bLastElement;
				if (i == 4)
				{
					this.m_eElement_Msg[i] = eElement_MsgType.eElement_NONE;
					bLastElement = true;
				}
				else
				{
					this.m_eElement_Msg[i] = eElement_MsgType.eElement_OK;
					bLastElement = false;
				}
				NewListItem item = new NewListItem(this.m_NewListBox_Reincarnate.ColumnNum, true, string.Empty);
				this.SetLegendReincarnateListBox(ref item, i, this.m_CharKind_Legendinfo.i32Base_CharKind[i], this.m_CharKind_Legendinfo.ui8Base_LegendGrade[i], bLastElement);
				this.m_NewListBox_Reincarnate.Add(item);
			}
		}
		this.m_NewListBox_Reincarnate.RepositionItems();
	}

	private void SetLegendReincarnateListBox(ref NewListItem item, int i, int i32CharKind, byte bCharRank, bool bLastElement)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!!!!!!!! SolGuild - Element CharKind " + i32CharKind + " Error");
			return;
		}
		NkListSolInfo nkListSolInfo = new NkListSolInfo();
		nkListSolInfo.SolCharKind = i32CharKind;
		nkListSolInfo.SolGrade = (int)(bCharRank - 1);
		if (bLastElement)
		{
			item.SetListItemData(0, false);
			item.SetListItemData(1, true);
		}
		else
		{
			item.SetListItemData(0, true);
			item.SetListItemData(1, false);
		}
		item.SetListItemData(3, nkListSolInfo, null, null, null);
		item.SetListItemData(4, charKindInfo.GetName(), null, null, null);
		string elementSolMsg = base.GetElementSolMsg(this.m_eElement_Msg[i]);
		item.SetListItemData(5, elementSolMsg, null, null, null);
		item.SetListItemData(6, false);
		if (bLastElement)
		{
			item.SetListItemData(7, false);
		}
		else
		{
			item.SetListItemData(7, nkListSolInfo, null, null, null);
		}
		item.SetListItemData(8, false);
		item.SetListItemData(9, false);
		if (bLastElement)
		{
			item.SetListItemData(10, true);
		}
		else
		{
			item.SetListItemData(10, false);
		}
		item.SetListItemData(10, string.Empty, 0, new EZValueChangedDelegate(this.OnElementClick), null);
		item.SetListItemData(11, false);
	}

	private void OnElementClick(IUIObject obj)
	{
		SolMilitarySelectDlg_challengequest solMilitarySelectDlg_challengequest = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLMILITARYSELECT_CHALLENGEQUEST_DLG) as SolMilitarySelectDlg_challengequest;
		if (solMilitarySelectDlg_challengequest == null)
		{
			return;
		}
		this.HideTouch(false);
		solMilitarySelectDlg_challengequest.SetDummySoldierList(this.m_iDummyCharKind);
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
}
