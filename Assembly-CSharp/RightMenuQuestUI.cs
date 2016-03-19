using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class RightMenuQuestUI : Form
{
	private const int MAX_RIGHT_MENU_QUEST_COUNT = 15;

	private NewListBox m_kQuestList;

	public CSelQuestInfo m_SelectedCondition;

	private string m_ColorTitle = NrTSingleton<CTextParser>.Instance.GetBaseColor();

	private string m_ColorNormal = NrTSingleton<CTextParser>.Instance.GetBaseColor();

	private string m_ColorComplte = NrTSingleton<CTextParser>.Instance.GetBaseColor();

	private UIButton m_Touch;

	private CQuest m_kQuest;

	public string m_szSelectQuestUnique = string.Empty;

	public bool bClickTouch;

	private float _Depth;

	private int m_nWinID;

	private TsAudio m_TouchSound;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Main/Dlg_MainQuest", G_ID.MAIN_QUEST, false);
		this.SetTextColor();
		base.ChangeSceneDestory = false;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_kQuestList = (base.GetControl("NLB_QuestList") as NewListBox);
		this.m_kQuestList.Reserve = false;
		this.m_kQuestList.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickQuestList));
		this.m_kQuestList.SetDoubleClickDelegate(new EZValueChangedDelegate(this.BtnDoubleClickQuestList));
		this.m_kQuestList.RightMouseSelect = true;
		this.m_kQuestList.SetRightMouseDelegate(new EZValueChangedDelegate(this.BtnRightMouseClick));
		this.m_kQuestList.SetMouseOverDelegate(new EZValueChangedDelegate(this.OnMouseOver));
		BoxCollider component = this.m_kQuestList.GetComponent<BoxCollider>();
		if (component != null)
		{
			component.size = new Vector3(0f, 0f, 0f);
		}
	}

	public void ShowTouchButton(CQuest quest)
	{
		if (quest == null)
		{
			return;
		}
		this.m_kQuest = quest;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		int level = charPersonInfo.GetLevel(0L);
		if (3 >= level)
		{
			if ("10101_005" == quest.GetQuestUnique())
			{
				return;
			}
			QUEST_CONST.eQUESTSTATE questState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(quest.GetQuestUnique());
			if ((null == this.m_Touch && quest.IsAutoMoveQuest()) || questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
			{
				if (null != this.m_Touch)
				{
					return;
				}
				this.m_Touch = UICreateControl.Button("touch", "Main_I_Touch01", 196f, 154f);
				this.m_Touch.PlayAni(true);
				base.InteractivePanel.MakeChild(this.m_Touch.gameObject);
				this.m_Touch.SetLocation(-this.m_Touch.GetSize().x + 85f, 0f, this.m_kQuestList.GetLocation().z - 1f);
				BoxCollider component = this.m_Touch.gameObject.GetComponent<BoxCollider>();
				if (null != component)
				{
					UnityEngine.Object.Destroy(component);
				}
			}
		}
		else
		{
			if (null != this.m_Touch)
			{
				base.InteractivePanel.RemoveChild(this.m_Touch.gameObject);
				UnityEngine.Object.Destroy(this.m_Touch.gameObject);
			}
			this.m_kQuest = null;
		}
	}

	public override void ChangedResolution()
	{
		if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
		{
			return;
		}
		this.RePosition();
	}

	public void RePosition()
	{
		BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
		if (bookmarkDlg != null)
		{
			float num;
			if (bookmarkDlg.IsHide())
			{
				num = 0f;
			}
			else
			{
				num = bookmarkDlg.GetSizeX();
			}
			base.SetLocation(GUICamera.width - num - base.GetSizeX(), 0f);
		}
	}

	public void InitClickTouch()
	{
		this.bClickTouch = false;
	}

	public override void Update()
	{
		if (null != this.m_Touch)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null && this.m_kQuest != null)
			{
				if (!@char.m_kCharMove.IsMoving() && !this.bClickTouch && 0 < this.m_kQuestList.Count)
				{
					QUEST_CONST.eQUESTSTATE questState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(this.m_kQuest.GetQuestUnique());
					if (this.m_kQuest.IsAutoMoveQuest() || questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
					{
						this.m_Touch.RenderEnabled = true;
					}
					else
					{
						this.m_Touch.RenderEnabled = false;
					}
				}
				else if (@char.m_kCharMove.IsMoving())
				{
					this.bClickTouch = false;
				}
			}
			else
			{
				this.m_Touch.RenderEnabled = false;
			}
			if (this.m_kQuestList.Count == 0)
			{
				this.m_Touch.RenderEnabled = false;
			}
		}
		base.Update();
	}

	private void SetTextColor()
	{
		this.m_ColorTitle = NrTSingleton<CTextParser>.Instance.GetTextColor("303");
		this.m_ColorNormal = NrTSingleton<CTextParser>.Instance.GetTextColor("101");
		this.m_ColorComplte = NrTSingleton<CTextParser>.Instance.GetTextColor("307");
	}

	public void QuestUpdate()
	{
		this.m_kQuestList.Clear();
		List<USER_CURRENT_QUEST_INFO> list = new List<USER_CURRENT_QUEST_INFO>();
		foreach (USER_CURRENT_QUEST_INFO current in NrTSingleton<NkQuestManager>.Instance.GetMainlist())
		{
			list.Add(current);
		}
		foreach (USER_CURRENT_QUEST_INFO current2 in NrTSingleton<NkQuestManager>.Instance.GetSublist())
		{
			list.Add(current2);
		}
		float num = 0f;
		int num2 = 0;
		foreach (USER_CURRENT_QUEST_INFO current3 in list)
		{
			if (!(current3.strQuestUnique == string.Empty))
			{
				string strQuestUnique = current3.strQuestUnique;
				if (strQuestUnique != string.Empty)
				{
					if (NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(strQuestUnique) != null)
					{
						CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
						if (questByQuestUnique != null)
						{
							float num3 = 80f;
							int num4 = 0;
							for (int i = 0; i < 3; i++)
							{
								if (0 < questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode)
								{
									num4++;
								}
							}
							if (num4 == 1)
							{
								this.m_kQuestList.SetColumnData("Mobile/DLG/Main/nlb_questlist_columndata" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
							}
							else if (num4 == 2)
							{
								this.m_kQuestList.SetColumnData("Mobile/DLG/Main/nlb_questlist2_columndata" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
								num3 = 120f;
							}
							else if (num4 == 3)
							{
								this.m_kQuestList.SetColumnData("Mobile/DLG/Main/nlb_questlist3_columndata" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
								num3 = 160f;
							}
							num += num3;
							this.m_kQuestList.LineHeight = num3;
							NewListItem newListItem = new NewListItem(this.m_kQuestList.ColumnNum, true);
							this.ShowTouchButton(questByQuestUnique);
							string text = string.Empty;
							string[] array = new string[3];
							text = questByQuestUnique.GetGiveQuestNpcName();
							if (!(text != string.Empty))
							{
								string msg = string.Format("Quest Give Npc Name == Blank, unique = {0}, charcode = {1}", questByQuestUnique.GetQuestUnique(), questByQuestUnique.GetQuestCommon().i32QuestCharKind);
								NrTSingleton<NrMainSystem>.Instance.Alert(msg);
								return;
							}
							CSelQuestInfo cSelQuestInfo = new CSelQuestInfo();
							cSelQuestInfo.m_bComplete = true;
							cSelQuestInfo.m_SeldQuestCondition = questByQuestUnique.GetQuestCommon().cQuestCondition[0];
							cSelQuestInfo.m_SelQuest = questByQuestUnique;
							cSelQuestInfo.bType = 0;
							CQuestGroup questGroupByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByQuestUnique(questByQuestUnique.GetQuestUnique());
							if (questGroupByQuestUnique != null)
							{
								if (NrTSingleton<NkQuestManager>.Instance.GetToggleQuestUnique())
								{
									text = NrTSingleton<UIDataManager>.Instance.GetString(this.m_ColorTitle, questByQuestUnique.GetQuestTitle(), "(", questByQuestUnique.GetQuestUnique().ToString(), ")");
								}
								else
								{
									text = NrTSingleton<UIDataManager>.Instance.GetString(this.m_ColorTitle, questByQuestUnique.GetQuestTitle());
								}
								newListItem.SetListItemData(3, text, cSelQuestInfo, null, null);
							}
							for (int j = 0; j < 3; j++)
							{
								if (0 < questByQuestUnique.GetQuestCommon().cQuestCondition[j].i32QuestCode)
								{
									array[j] = questByQuestUnique.GetConditionText(current3.i64ParamVal[j], j);
									if (string.Empty != array[j])
									{
										string conditionText = questByQuestUnique.GetConditionText(current3.i64ParamVal[j], j);
										string empty = string.Empty;
										bool bTitle = NrTSingleton<CTextParser>.Instance.ChangeQuestConditionText(conditionText, out empty);
										if (NrTSingleton<NkQuestManager>.Instance.GetToggleQuestUnique())
										{
											array[j] = string.Concat(new object[]
											{
												empty,
												"(",
												questByQuestUnique.GetQuestCommon().cQuestCondition[j].i32QuestCode,
												")"
											});
										}
										else
										{
											array[j] = empty;
										}
										CSelQuestInfo cSelQuestInfo2 = new CSelQuestInfo();
										int index = 5;
										if (j == 0)
										{
											index = 5;
										}
										else if (j == 1)
										{
											index = 10;
										}
										else if (j == 2)
										{
											index = 13;
										}
										if (questByQuestUnique.CheckCondition(questByQuestUnique.GetQuestCommon().cQuestCondition[j].i64Param, ref current3.i64ParamVal[j], j) && current3.bFailed == 0)
										{
											cSelQuestInfo2.m_bComplete = true;
											cSelQuestInfo2.m_SeldQuestCondition = questByQuestUnique.GetQuestCommon().cQuestCondition[j];
											cSelQuestInfo2.m_SelQuest = questByQuestUnique;
											cSelQuestInfo2.bType = 1;
											cSelQuestInfo2.bTitle = bTitle;
											cSelQuestInfo2.strCon = conditionText;
											newListItem.SetListItemData(index, this.m_ColorComplte + array[j], cSelQuestInfo2, null, null);
										}
										else
										{
											cSelQuestInfo2.m_bComplete = false;
											cSelQuestInfo2.m_SeldQuestCondition = questByQuestUnique.GetQuestCommon().cQuestCondition[j];
											cSelQuestInfo2.m_SelQuest = questByQuestUnique;
											cSelQuestInfo2.bType = 1;
											cSelQuestInfo2.bTitle = bTitle;
											cSelQuestInfo2.strCon = conditionText;
											newListItem.SetListItemData(index, this.m_ColorNormal + array[j], cSelQuestInfo2, null, null);
										}
									}
								}
							}
							newListItem.SetListItemData(6, false);
							newListItem.SetListItemData(7, false);
							newListItem.Data = strQuestUnique;
							this.m_kQuestList.Add(newListItem);
							if (0 < num2)
							{
								num += this.m_kQuestList.itemSpacing;
							}
							num2++;
						}
					}
				}
			}
		}
		base.SetSize(base.GetSizeX(), num);
		this.m_kQuestList.ResizeViewableArea(this.m_kQuestList.GetSize().x, num);
		NrTSingleton<NkQuestManager>.Instance.UpdateClientNpc(0);
	}

	public void ClickCancelQuest(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		CQuest cQuest = (CQuest)obj.Data;
		if (cQuest == null)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			msgBoxUI.SetMsg(new YesDelegate(this.CancelQuest), cQuest, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("799"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("126"), eMsgType.MB_OK_CANCEL);
			msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("320"));
			msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("321"));
		}
	}

	public void CancelQuest(object a_oObject)
	{
		CQuest cQuest = (CQuest)a_oObject;
		if (cQuest == null)
		{
			return;
		}
		GS_QUEST_CANCLE_REQ gS_QUEST_CANCLE_REQ = new GS_QUEST_CANCLE_REQ();
		TKString.StringChar(cQuest.GetQuestUnique(), ref gS_QUEST_CANCLE_REQ.strQuestUnique);
		SendPacket.GetInstance().SendObject(1013, gS_QUEST_CANCLE_REQ);
		this.Close();
	}

	public void QuestAccept(CQuest kQuest)
	{
	}

	public UIListItemContainer FindQeustListItem(string questunique)
	{
		return null;
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		this.m_nWinID = winID;
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide == null)
		{
			return;
		}
		uI_UIGuide.m_nOtherWinID = G_ID.MAIN_QUEST;
		this._Depth = base.GetLocation().z;
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 1f);
	}

	public void HideUIGuide()
	{
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), this._Depth);
	}

	private void BtnClickQuestList(IUIObject obj)
	{
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		this.m_szSelectQuestUnique = string.Empty;
		UIListItemContainer selectedItem = this.m_kQuestList.SelectedItem;
		if (null == selectedItem)
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.UIGUIDE_DLG) != null)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.UIGUIDE_DLG);
		}
		this.m_szSelectQuestUnique = (string)selectedItem.Data;
		this.BtnDoubleClickQuestList(null);
	}

	private void OnMouseOver(object sender)
	{
	}

	private void BtnRightMouseClick(object sender)
	{
	}

	private void BtnDoubleClickQuestList(object sender)
	{
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.UIGUIDE_DLG) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		if (string.Empty != this.m_szSelectQuestUnique)
		{
			CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(this.m_szSelectQuestUnique);
			if (questByQuestUnique == null)
			{
				return;
			}
			QUEST_CONST.eQUESTSTATE questState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(questByQuestUnique.GetQuestUnique());
			if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING)
			{
				for (int i = 0; i < 3; i++)
				{
					if (questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode == 155)
					{
						MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
						if (msgBoxUI != null)
						{
							string empty = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1533"),
								"targetname",
								NrTSingleton<NrCharKindInfoManager>.Instance.GetName((int)questByQuestUnique.GetQuestCommon().cQuestCondition[1].i64Param)
							});
							msgBoxUI.SetMsg(new YesDelegate(NrTSingleton<NkQuestManager>.Instance.OpenQuestBattle), questByQuestUnique, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1532"), empty, eMsgType.MB_OK_CANCEL);
							msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("320"));
							msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("321"));
							return;
						}
					}
				}
			}
			NrTSingleton<NkQuestManager>.Instance.QuestAutoMove(this.m_szSelectQuestUnique);
			if (null != this.m_Touch)
			{
				this.m_Touch.RenderEnabled = false;
				this.bClickTouch = true;
			}
		}
	}

	private void SlideChange(IUIObject obj)
	{
	}

	private void OnTouchSoundDownload(IDownloadedItem wItem, object obj)
	{
		if (wItem.isCanceled)
		{
			return;
		}
		if (wItem.canAccessAssetBundle)
		{
			TsAudio.RequestData requestData = obj as TsAudio.RequestData;
			TsAudio tsAudio = TsAudioCreator.Create(requestData.baseData);
			if (tsAudio != null)
			{
				tsAudio.RefAudioClip = (wItem.mainAsset as AudioClip);
				tsAudio.PlayClipAtPoint(Vector3.zero);
				this.m_TouchSound = tsAudio;
			}
		}
	}

	public void OnTouchSoundStop()
	{
		if (this.m_TouchSound != null)
		{
			this.m_TouchSound.Stop();
			string name = string.Empty;
			name = string.Format("_PlayClipAtPoint= {0}", this.m_TouchSound.RefAudioClip.name);
			GameObject gameObject = GameObject.Find(name);
			if (null != gameObject)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}

	public override void Hide()
	{
		base.Hide();
	}

	public override void Show()
	{
		base.Show();
		this.RePosition();
	}
}
