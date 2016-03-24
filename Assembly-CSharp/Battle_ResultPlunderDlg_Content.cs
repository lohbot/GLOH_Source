using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using StageHelper;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle_ResultPlunderDlg_Content : Form
{
	public enum eMODE
	{
		eMODE_PLUNDER,
		eMODE_INFIBATTLE,
		eMODE_MAX
	}

	private Button m_btClose;

	private Label m_lbLoading;

	private NewListBox m_lbSolList;

	private Label m_lbWin;

	private Label m_lbLose;

	private Label m_lbGold;

	private Label m_lbScore;

	private Label m_lbAttacker;

	private Label m_lbDefencer;

	private Label[] m_lbAttackerDeadStartPos;

	private Label[] m_lbDefencerDeadStartPos;

	private Label m_lbInfiBattleReward_1;

	private Label m_lbInfiBattleRank_1;

	private Label m_lbInfiBattleReward_2;

	private Label m_lbInfiBattleRank_2;

	private Label m_lbInfiBattleWin;

	private ItemTexture m_itInfiBattleRewardItem;

	public string m_strBattleTime;

	public string m_strWin;

	public bool m_bWin;

	public int m_BattleSRewardUnique;

	private bool m_bShowPlunderDlg;

	private float m_fCloseEnableTime;

	private float m_fBattleTime;

	private int m_nInjurySolCount;

	private List<GS_BATTLE_RESULT_SOLDIER> m_SolInfoList = new List<GS_BATTLE_RESULT_SOLDIER>();

	private GS_BATTLE_RESULT_PLUNDER_NFY m_BasicInfo = new GS_BATTLE_RESULT_PLUNDER_NFY();

	private GS_INFIBATTLE_RESULT_ACK m_InfiBattleInfo;

	private GameObject m_goRankEffectObject;

	private bool m_bRankEffect;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Battle/RESULT/DLG_Battle_Result_Plunder_Content", G_ID.BATTLE_RESULT_PLUNDER_CONTENT_DLG, true);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.ChangeSceneDestory = false;
		base.Draggable = false;
		base.DonotDepthChange(NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 8f);
		if (!NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
		{
			this.m_bShowPlunderDlg = true;
		}
		else
		{
			this.m_bShowPlunderDlg = false;
		}
		this.Show();
	}

	public override void SetComponent()
	{
		this.m_lbLoading = (base.GetControl("Label_Loading") as Label);
		this.m_btClose = (base.GetControl("Button_ok") as Button);
		Button expr_32 = this.m_btClose;
		expr_32.Click = (EZValueChangedDelegate)Delegate.Combine(expr_32.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_lbSolList = (base.GetControl("plunder_result_sollist") as NewListBox);
		this.m_lbSolList.Clear();
		this.m_lbWin = (base.GetControl("LB_Win") as Label);
		this.m_lbLose = (base.GetControl("LB_Lose") as Label);
		this.m_lbGold = (base.GetControl("LB_Gold") as Label);
		this.m_lbScore = (base.GetControl("LB_Score") as Label);
		this.m_lbScore.SetText(string.Empty);
		this.m_lbAttacker = (base.GetControl("LB_WinText") as Label);
		this.m_lbDefencer = (base.GetControl("LB_LoseText") as Label);
		this.m_lbAttackerDeadStartPos = new Label[3];
		this.m_lbDefencerDeadStartPos = new Label[3];
		for (int i = 0; i < 3; i++)
		{
			string name = string.Format("Label_A_result{0}", i.ToString());
			string name2 = string.Format("Label_D_result{0}", i.ToString());
			this.m_lbAttackerDeadStartPos[i] = (base.GetControl(name) as Label);
			this.m_lbDefencerDeadStartPos[i] = (base.GetControl(name2) as Label);
		}
		this.m_lbInfiBattleReward_1 = (base.GetControl("LB_reward1") as Label);
		this.m_lbInfiBattleRank_1 = (base.GetControl("LB_rank1") as Label);
		this.m_lbInfiBattleReward_2 = (base.GetControl("LB_reward2") as Label);
		this.m_lbInfiBattleRank_2 = (base.GetControl("LB_rank2") as Label);
		this.m_lbInfiBattleWin = (base.GetControl("LB_winningpoint") as Label);
		this.m_itInfiBattleRewardItem = (base.GetControl("ItemTexture_ItemTexture36") as ItemTexture);
		this.m_itInfiBattleRewardItem.Hide(false);
		this.m_btClose.Visible = false;
		this.m_fCloseEnableTime = 0f;
	}

	public override void InitData()
	{
		base.InitData();
		this.ResizeDlg();
	}

	public override void Update()
	{
		base.Update();
		if (!this.m_btClose.Visible && Scene.CurScene != Scene.Type.BATTLE && CommonTasks.IsEndOfPrework)
		{
			if (this.m_fCloseEnableTime == 0f)
			{
				this.m_fCloseEnableTime = Time.realtimeSinceStartup + 0.3f;
			}
			else if (this.m_fCloseEnableTime < Time.realtimeSinceStartup)
			{
				this.m_btClose.Visible = true;
				this.m_lbLoading.Visible = false;
				if (this.m_bRankEffect && this.m_goRankEffectObject != null)
				{
					NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
					if (myCharInfo.InfiBattleStraightWin <= 0)
					{
						return;
					}
					this.m_goRankEffectObject.SetActive(true);
					Animation componentInChildren = this.m_goRankEffectObject.GetComponentInChildren<Animation>();
					if (componentInChildren != null)
					{
						Transform transform = this.m_goRankEffectObject.transform.FindChild("fx_victory_ui");
						if (transform != null)
						{
							GameObject gameObject;
							if (myCharInfo.InfiBattleStraightWin / 100 > 0)
							{
								gameObject = NkUtil.GetChild(transform, "fx_numer3").gameObject;
								if (gameObject != null)
								{
									MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
									if (component != null)
									{
										Material material = new Material(component.sharedMaterial);
										if (null != material)
										{
											material.mainTextureOffset = new Vector2(this.CreateNum(myCharInfo.InfiBattleStraightWin / 100), 0f);
											component.material = material;
										}
									}
								}
							}
							if (myCharInfo.InfiBattleStraightWin / 10 > 0)
							{
								gameObject = NkUtil.GetChild(transform, "fx_numer2").gameObject;
								if (gameObject != null)
								{
									MeshRenderer component2 = gameObject.GetComponent<MeshRenderer>();
									if (component2 != null)
									{
										Material material = new Material(component2.sharedMaterial);
										if (null != material)
										{
											material.mainTextureOffset = new Vector2(this.CreateNum(myCharInfo.InfiBattleStraightWin / 10), 0f);
											component2.material = material;
										}
									}
								}
							}
							gameObject = NkUtil.GetChild(transform, "fx_numer1").gameObject;
							if (gameObject != null)
							{
								MeshRenderer component3 = gameObject.GetComponent<MeshRenderer>();
								if (component3 != null)
								{
									Material material = new Material(component3.sharedMaterial);
									if (null != material)
									{
										material.mainTextureOffset = new Vector2(this.CreateNum(myCharInfo.InfiBattleStraightWin % 10), 0f);
										component3.material = material;
									}
								}
							}
						}
						if (myCharInfo.InfiBattleStraightWin / 100 > 0)
						{
							componentInChildren.Play("fx_victory03");
						}
						else if (myCharInfo.InfiBattleStraightWin / 10 > 0)
						{
							componentInChildren.Play("fx_victory02");
						}
						else
						{
							componentInChildren.Play("fx_victory01");
						}
						TsAudioManager.Container.RequestAudioClip("UI_SFX", "ETC", "INFINITE_MATCH_WIN", new PostProcPerItem(this.OnDownloaded_Sound));
						this.m_bRankEffect = false;
					}
				}
			}
		}
	}

	public void OnDownloaded_Sound(IDownloadedItem wItem, object obj)
	{
		if (base.IsDestroy())
		{
			return;
		}
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
				if (wItem.mainAsset == null)
				{
					TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
					{
						wItem.assetPath
					});
				}
				else
				{
					tsAudio.RefAudioClip = (wItem.mainAsset as AudioClip);
					wItem.unloadImmediate = true;
				}
				tsAudio.PlayClipAtPoint(Vector3.zero);
			}
		}
	}

	public override void OnClose()
	{
		if (this.m_bShowPlunderDlg && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERMAIN_DLG))
		{
			PlunderMainDlg plunderMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERMAIN_DLG) as PlunderMainDlg;
			if (plunderMainDlg != null)
			{
				plunderMainDlg.ShowInfiBattle();
			}
		}
		NrTSingleton<FiveRocksEventManager>.Instance.BattleResult(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER, this.m_fBattleTime, this.m_nInjurySolCount);
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bNoMove)
		{
			GS_WARP_REQ gS_WARP_REQ = new GS_WARP_REQ();
			gS_WARP_REQ.nMode = 1;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WARP_REQ, gS_WARP_REQ);
		}
		if (this.m_goRankEffectObject != null)
		{
			UnityEngine.Object.DestroyObject(this.m_goRankEffectObject);
			this.m_goRankEffectObject = null;
		}
	}

	public void ResizeDlg()
	{
		float x = (GUICamera.width - base.GetSizeX()) / 2f;
		float num = (GUICamera.height - base.GetSizeY()) / 2f;
		if (num < 76f)
		{
			num = 76f;
		}
		if (num + base.GetSizeY() >= GUICamera.height)
		{
			num -= num + base.GetSizeY() - GUICamera.height;
		}
		base.SetLocation(x, num);
	}

	public void OnClickClose(IUIObject obj)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		if (!CommonTasks.IsEndOfPrework)
		{
			return;
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_RESULT_PLUNDER_DLG);
	}

	public void AddSolData(GS_BATTLE_RESULT_SOLDIER solinfo)
	{
		this.m_SolInfoList.Add(solinfo);
	}

	public void SetBasicData(GS_BATTLE_RESULT_PLUNDER_NFY info)
	{
		this.m_BasicInfo = info;
	}

	public void ClearSolData()
	{
		this.m_SolInfoList.Clear();
	}

	public void LinkData()
	{
		this._LinkSolData();
	}

	public void _LinkBasicData()
	{
		ushort num = (ushort)(this.m_BasicInfo.fBattleTime / 3600f);
		ushort num2 = (ushort)((this.m_BasicInfo.fBattleTime - (float)(num * 3600)) / 60f);
		ushort num3 = (ushort)(this.m_BasicInfo.fBattleTime - (float)(num * 3600) - (float)(num2 * 60));
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strBattleTime, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("976"),
			"hour",
			string.Format("{0:D2}", num),
			"min",
			string.Format("{0:D2}", num2),
			"sec",
			string.Format("{0:D2}", num3)
		});
		this.m_fBattleTime = this.m_BasicInfo.fBattleTime;
		if (Battle.BATTLE == null)
		{
			Debug.LogError("Battle Result dialog : Battle.BATTLE Is NULL");
			return;
		}
		int num4 = 3;
		int num5 = 3;
		for (int i = 0; i < 3; i++)
		{
			if (this.m_BasicInfo.bAttackDeadStartPos[i])
			{
				this.m_lbAttackerDeadStartPos[i].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("235"));
				this.m_lbDefencerDeadStartPos[i].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("234"));
				num4--;
			}
			else
			{
				this.m_lbAttackerDeadStartPos[i].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("234"));
				this.m_lbDefencerDeadStartPos[i].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("235"));
				num5--;
			}
		}
		bool flag = (eBATTLE_ALLY)this.m_BasicInfo.i8WinAlly == Battle.BATTLE.MyAlly;
		long num6 = 0L;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			num6 = kMyCharInfo.PlunderMoney;
			kMyCharInfo.PlunderMoney = 0L;
			kMyCharInfo.PlunderCharName = string.Empty;
			kMyCharInfo.PlunderCharLevel = 0;
		}
		if (flag)
		{
			if (num4 >= 3)
			{
				this.m_strWin = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("307");
			}
			else
			{
				this.m_strWin = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("308");
			}
			this.m_bWin = true;
		}
		else
		{
			if (num6 != 0L && this.m_BasicInfo.nRewardMoney != 0L)
			{
				float num7 = (float)this.m_BasicInfo.nRewardMoney / (float)num6;
				if (num7 >= 0.5f)
				{
					this.m_strWin = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("309");
				}
				else
				{
					this.m_strWin = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("310");
				}
			}
			else
			{
				this.m_strWin = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("311");
			}
			this.m_bWin = false;
		}
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("133"),
			"point",
			(1000L + (long)this.m_BasicInfo.nCurrentMatchPoint + (long)this.m_BasicInfo.nAddMatchPoint).ToString(),
			"addpoint",
			this.m_BasicInfo.nAddMatchPoint.ToString()
		});
		this.m_lbScore.SetText(empty);
		this.m_lbGold.SetText(this.m_BasicInfo.nRewardMoney.ToString());
		this.m_lbAttacker.SetText(TKString.NEWString(this.m_BasicInfo.szAttackerName));
		this.m_lbDefencer.SetText(TKString.NEWString(this.m_BasicInfo.szDefencerName));
		this.m_lbWin.SetText(num4.ToString());
		this.m_lbLose.SetText(num5.ToString());
	}

	private Texture2D GetPortraitLeaderSol(int iCharKind)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null && kMyCharInfo.UserPortrait)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser.GetCharKind() == iCharKind)
			{
				return kMyCharInfo.UserPortraitTexture;
			}
		}
		return null;
	}

	private void _LinkSolData()
	{
		int num = 0;
		foreach (GS_BATTLE_RESULT_SOLDIER current in this.m_SolInfoList)
		{
			NewListItem newListItem = new NewListItem(this.m_lbSolList.ColumnNum, true, string.Empty);
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.CharKind);
			if (charKindInfo != null)
			{
				NkListSolInfo nkListSolInfo = new NkListSolInfo();
				nkListSolInfo.SolCharKind = current.CharKind;
				nkListSolInfo.SolGrade = current.SolGrade;
				nkListSolInfo.SolInjuryStatus = current.bInjury;
				nkListSolInfo.SolLevel = current.i16Level;
				nkListSolInfo.ShowLevel = true;
				if (NrTSingleton<NrCharCostumeTableManager>.Instance.IsCostumeUniqueEqualSolKind(current.i32CostumeUnique, current.CharKind))
				{
					nkListSolInfo.SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(current.i32CostumeUnique);
				}
				EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(current.CharKind, (byte)current.SolGrade);
				Texture2D portraitLeaderSol = this.GetPortraitLeaderSol(current.CharKind);
				if (portraitLeaderSol != null)
				{
					newListItem.SetListItemData(1, portraitLeaderSol, null, null, null, null);
				}
				else
				{
					if (eventHeroCharCode != null)
					{
						newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
						newListItem.EventMark = true;
					}
					else
					{
						UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(current.CharKind, current.SolGrade);
						if (legendFrame != null)
						{
							newListItem.SetListItemData(0, legendFrame, null, null, null);
						}
					}
					newListItem.SetListItemData(1, nkListSolInfo, null, null, null);
				}
				string text = string.Empty;
				if (NrTSingleton<NrCharKindInfoManager>.Instance.IsUserCharKind(current.CharKind))
				{
					text = TKString.NEWString(this.m_BasicInfo.szAttackerName);
				}
				else
				{
					text = charKindInfo.GetName();
				}
				string text2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("471"),
					"targetname",
					text,
					"count",
					current.i16Level
				});
				short gradeMaxLevel = charKindInfo.GetGradeMaxLevel((short)((byte)current.SolGrade));
				string str = string.Empty;
				if (gradeMaxLevel <= current.i16Level)
				{
					str = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1802"),
						"exp",
						ANNUALIZED.Convert(current.i32AddExp)
					});
				}
				text2 += "\r\n";
				text2 += str;
				newListItem.SetListItemData(2, text2, null, null, null);
				this.m_lbSolList.Add(newListItem);
				this.m_lbSolList.RepositionItems();
				num++;
				if (nkListSolInfo.SolInjuryStatus)
				{
					this.m_nInjurySolCount++;
				}
			}
		}
	}

	public void SetInfiBattleInfo(GS_INFIBATTLE_RESULT_ACK ACK)
	{
		if (this.m_InfiBattleInfo == null)
		{
			this.m_InfiBattleInfo = new GS_INFIBATTLE_RESULT_ACK();
		}
		this.m_InfiBattleInfo = ACK;
	}

	public void _LinkBasicDataInfiBattle()
	{
		ushort num = (ushort)(this.m_InfiBattleInfo.fBattleTime / 3600f);
		ushort num2 = (ushort)((this.m_InfiBattleInfo.fBattleTime - (float)(num * 3600)) / 60f);
		ushort num3 = (ushort)(this.m_InfiBattleInfo.fBattleTime - (float)(num * 3600) - (float)(num2 * 60));
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strBattleTime, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("976"),
			"hour",
			string.Format("{0:D2}", num),
			"min",
			string.Format("{0:D2}", num2),
			"sec",
			string.Format("{0:D2}", num3)
		});
		this.m_fBattleTime = this.m_InfiBattleInfo.fBattleTime;
		if (Battle.BATTLE == null)
		{
			Debug.LogError("Battle Result dialog : Battle.BATTLE Is NULL");
			return;
		}
		int num4 = 3;
		int num5 = 3;
		for (int i = 0; i < 3; i++)
		{
			if (this.m_InfiBattleInfo.bAttackDeadStartPos[i])
			{
				this.m_lbAttackerDeadStartPos[i].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("235"));
				this.m_lbDefencerDeadStartPos[i].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("234"));
				num4--;
			}
			else
			{
				this.m_lbAttackerDeadStartPos[i].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("234"));
				this.m_lbDefencerDeadStartPos[i].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("235"));
				num5--;
			}
		}
		this.m_bWin = false;
		if (this.m_InfiBattleInfo.i8WinAlly == 0)
		{
			this.m_bWin = true;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			kMyCharInfo.PlunderMoney = 0L;
			kMyCharInfo.PlunderCharName = string.Empty;
			kMyCharInfo.PlunderCharLevel = 0;
		}
		this.m_lbAttacker.SetText(NrTSingleton<NkCharManager>.Instance.GetCharName());
		this.m_lbDefencer.SetText(TKString.NEWString(this.m_InfiBattleInfo.strDefencerName));
		string text = string.Empty;
		string empty = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2937");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"count1",
			kMyCharInfo.InfiBattleStraightWin,
			"count2",
			kMyCharInfo.InfinityBattle_OldRank
		});
		this.m_lbWin.SetText(num4.ToString());
		this.m_lbLose.SetText(num5.ToString());
		string text2 = string.Empty;
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		int num6 = -1;
		if (instance != null)
		{
			num6 = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_RANKLIMIT);
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (this.m_bWin)
		{
			if (num4 >= 3)
			{
				this.m_strWin = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("307");
			}
			else
			{
				this.m_strWin = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("308");
			}
			if (this.m_InfiBattleInfo.i32RewardItemUnique > 0)
			{
				UIBaseInfoLoader itemTexture = NrTSingleton<ItemManager>.Instance.GetItemTexture(this.m_InfiBattleInfo.i32RewardItemUnique);
				this.m_itInfiBattleRewardItem.SetTexture(itemTexture);
				text2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_InfiBattleInfo.i32RewardItemUnique);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1827"),
					"itemname",
					text2,
					"num",
					this.m_InfiBattleInfo.i32RewardItemNum.ToString()
				});
				int num7 = this.m_InfiBattleInfo.i32StraightLoseItemNum + this.m_InfiBattleInfo.i32StraightWinItemNum;
				if (num7 > 0)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2233"),
						"itemname",
						text2,
						"num",
						this.m_InfiBattleInfo.i32RewardItemNum.ToString(),
						"num2",
						num7.ToString()
					});
				}
				this.m_lbInfiBattleReward_2.SetText(text);
			}
			else
			{
				this.m_lbInfiBattleReward_2.SetText(string.Empty);
			}
			int num8 = (this.m_InfiBattleInfo.i32AttackRank <= this.m_InfiBattleInfo.i32DefenseRank) ? this.m_InfiBattleInfo.i32AttackRank : this.m_InfiBattleInfo.i32DefenseRank;
			if (num6 < num8 || 0 >= num8)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("447"),
					"rank",
					num8.ToString()
				});
			}
			this.m_lbInfiBattleRank_2.SetText(text);
			myCharInfo.InfiBattleStraightWin = this.m_InfiBattleInfo.i32AttackWinCount;
			string str = "Effect/Instant/fx_victory_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.BattleRankEffect), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		else
		{
			myCharInfo.InfiBattleStraightWin = this.m_InfiBattleInfo.i32AttackWinCount;
			this.m_strWin = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("311");
			this.m_lbInfiBattleReward_1.SetText(string.Empty);
			this.m_lbInfiBattleRank_1.SetText(string.Empty);
			this.m_lbInfiBattleReward_2.SetText(string.Empty);
			int i32AttackRank = this.m_InfiBattleInfo.i32AttackRank;
			if (num6 < i32AttackRank || 0 >= i32AttackRank)
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("447"),
					"rank",
					i32AttackRank.ToString()
				});
			}
			this.m_lbInfiBattleRank_2.SetText(text);
		}
		if (instance != null)
		{
			if (kMyCharInfo.InfinityBattle_Rank < instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_RANK))
			{
				this.m_lbInfiBattleWin.SetText("0");
			}
			else
			{
				this.m_lbInfiBattleWin.SetText(string.Empty);
			}
		}
	}

	public void SetMode()
	{
		this.ShowInfiBattle();
	}

	public void ShowPlunder()
	{
		base.ShowLayer(1);
	}

	public void ShowInfiBattle()
	{
		base.ShowLayer(2);
	}

	public void _LinkSolDataInfiBattle()
	{
		int num = 0;
		this.m_lbSolList.Clear();
		foreach (GS_BATTLE_RESULT_SOLDIER current in this.m_SolInfoList)
		{
			NewListItem newListItem = new NewListItem(this.m_lbSolList.ColumnNum, true, string.Empty);
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.CharKind);
			if (charKindInfo != null)
			{
				NkListSolInfo nkListSolInfo = new NkListSolInfo();
				nkListSolInfo.SolCharKind = current.CharKind;
				nkListSolInfo.SolGrade = current.SolGrade;
				nkListSolInfo.SolInjuryStatus = current.bInjury;
				nkListSolInfo.SolLevel = current.i16Level;
				nkListSolInfo.ShowLevel = true;
				if (NrTSingleton<NrCharCostumeTableManager>.Instance.IsCostumeUniqueEqualSolKind(current.i32CostumeUnique, current.CharKind))
				{
					nkListSolInfo.SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(current.i32CostumeUnique);
				}
				EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(current.CharKind, (byte)current.SolGrade);
				Texture2D portraitLeaderSol = this.GetPortraitLeaderSol(current.CharKind);
				if (portraitLeaderSol != null)
				{
					newListItem.SetListItemData(1, portraitLeaderSol, null, null, null, null);
				}
				else
				{
					if (eventHeroCharCode != null)
					{
						newListItem.SetListItemData(0, "Win_I_EventSol", null, null, null);
						newListItem.EventMark = true;
					}
					else
					{
						UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(current.CharKind, current.SolGrade);
						if (legendFrame != null)
						{
							newListItem.SetListItemData(0, legendFrame, null, null, null);
						}
					}
					newListItem.SetListItemData(1, nkListSolInfo, null, null, null);
				}
				string text = string.Empty;
				if (NrTSingleton<NrCharKindInfoManager>.Instance.IsUserCharKind(current.CharKind))
				{
					text = TKString.NEWString(this.m_InfiBattleInfo.strAttackName);
				}
				else
				{
					text = charKindInfo.GetName();
				}
				string text2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("471"),
					"targetname",
					text,
					"count",
					current.i16Level
				});
				short gradeMaxLevel = charKindInfo.GetGradeMaxLevel((short)((byte)current.SolGrade));
				string str = string.Empty;
				if (gradeMaxLevel <= current.i16Level)
				{
					str = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1802"),
						"exp",
						ANNUALIZED.Convert(current.i32AddExp)
					});
				}
				text2 += "\r\n";
				text2 += str;
				newListItem.SetListItemData(2, text2, null, null, null);
				this.m_lbSolList.Add(newListItem);
				this.m_lbSolList.RepositionItems();
				num++;
				if (nkListSolInfo.SolInjuryStatus)
				{
					this.m_nInjurySolCount++;
				}
			}
		}
	}

	public float CreateNum(int iRank)
	{
		if (iRank == 0 || iRank > 9)
		{
			return 1.9f;
		}
		if (iRank < 0 || iRank == 1)
		{
			return 1f;
		}
		return ((float)iRank - 1f) * 0.1f + 1f;
	}

	private void BattleRankEffect(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_goRankEffectObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				UnityEngine.Object.DontDestroyOnLoad(this.m_goRankEffectObject);
				if (this == null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_goRankEffectObject);
					return;
				}
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				this.m_goRankEffectObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_goRankEffectObject, GUICamera.UILayer);
				this.m_goRankEffectObject.SetActive(false);
				this.m_bRankEffect = true;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goRankEffectObject);
				}
			}
		}
	}

	private void LoadCompleteRankTexture(WWWItem _item, object _param)
	{
		if (_item != null && _item.canAccessAssetBundle)
		{
			Texture2D texture2D = _item.mainAsset as Texture2D;
			if (texture2D != null)
			{
				GameObject gameObject = NkUtil.GetChild(this.m_goRankEffectObject.transform, "fx_victory01").gameObject;
				if (gameObject != null)
				{
					gameObject.renderer.sharedMaterial.mainTexture = texture2D;
				}
			}
		}
	}
}
