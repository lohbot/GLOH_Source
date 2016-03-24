using GAME;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolComposeSuccessDlg : Form
{
	private const int VALID_PROPERTY_MAX = 4;

	private Label lbName;

	private DrawTexture SolRank;

	private DrawTexture dtImg;

	private DrawTexture dtGradeUpImg;

	private Label lbGradeUpText;

	private Label lbExp;

	private Label lbExpRate;

	private ProgressBar pbGage;

	private Label[] lbLevel;

	private Label[] m_lbLevelUpStat;

	private Label[] m_lbRankUpStat;

	private DrawTexture GradeExpBG;

	private DrawTexture GradeExpGage;

	private DrawTexture m_txPreRank;

	private DrawTexture m_txCurRank;

	private DrawTexture m_txUpGrade;

	private Label GradeExpText;

	private Label m_lbGetGradeExpText;

	private Button btnOk;

	private List<int> guideWinIDList = new List<int>();

	private UIButton _Touch;

	private string[] arPerpertyKeys = new string[]
	{
		"1209",
		"1210",
		"1211",
		"1212",
		"1746",
		"1747",
		"1236",
		"1237",
		"1216",
		"1238",
		"1239",
		"1240"
	};

	private GameObject rootGameObject;

	private NkSoldierInfo m_TargetSolInfo;

	private bool m_bLevelUP;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/dlg_SolComposesuccess", G_ID.SOLCOMPOSE_SUCCESS_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public void ControlSetting(ref Label[] lbControl, string key, string ArrowKey)
	{
		lbControl = new Label[3];
		lbControl[0] = (base.GetControl(string.Format("Label_stats_{0}", key)) as Label);
		lbControl[1] = (base.GetControl(string.Format("Label_stats_{0}2", key)) as Label);
		lbControl[1] = (base.GetControl(string.Format("Label_Arrow_{0}", ArrowKey)) as Label);
		lbControl[2] = (base.GetControl(string.Format("Label_stats_{0}3", key)) as Label);
	}

	public void SetTextShowOrNot(ref Label[] lbControl, int[] arCurrentValue)
	{
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1319");
		for (int i = 0; i < 4; i++)
		{
			lbControl[i].Visible = (arCurrentValue[i] != 0);
			if (arCurrentValue[i] > 0)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromInterface,
					"count",
					arCurrentValue[i]
				});
				lbControl[i].SetText(empty);
			}
			TsLog.Log("arCurrentValue[{0}] = {1}", new object[]
			{
				i,
				arCurrentValue[i]
			});
		}
	}

	public void SetTextShowOrNot(ref Label[] lbControl, int[] arCurrentValue, short Index)
	{
		if (0 <= Index && (int)Index < this.arPerpertyKeys.Length)
		{
			lbControl[0].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.arPerpertyKeys[(int)Index]));
		}
	}

	private int[] GetStat(NkSoldierInfo pSoldierinfo, short nLevel, int nGrade = -1)
	{
		NrCharKindInfo charKindInfo = pSoldierinfo.GetCharKindInfo();
		int[] array = new int[4];
		int solgrade = (nGrade != -1) ? nGrade : ((int)pSoldierinfo.GetGrade());
		array[0] = charKindInfo.GetGradePlusSTR(solgrade);
		array[1] = charKindInfo.GetGradePlusVIT(solgrade);
		array[2] = charKindInfo.GetGradePlusDEX(solgrade);
		array[3] = charKindInfo.GetGradePlusINT(solgrade);
		array[0] += charKindInfo.GetIncSTR(solgrade, (int)nLevel);
		array[1] += charKindInfo.GetIncVIT(solgrade, (int)nLevel);
		array[2] += charKindInfo.GetIncDEX(solgrade, (int)nLevel);
		array[3] += charKindInfo.GetIncINT(solgrade, (int)nLevel);
		TsLog.Log("Grade : {4} Level : {5} ,STR : {0} VIT : {1} DEX: {2} INT:{3}", new object[]
		{
			array[0],
			array[1],
			array[2],
			array[3],
			nGrade,
			nLevel
		});
		return array;
	}

	private int[] GetLevelChangeValue(NkSoldierInfo pSoldierinfo, short PreLevel, short nLevel, int nGrade = -1)
	{
		int nGrade2 = (nGrade != -1) ? nGrade : ((int)pSoldierinfo.GetGrade());
		int[] array = new int[4];
		int[] array2 = new int[4];
		array = this.GetStat(pSoldierinfo, PreLevel, nGrade2);
		array2 = this.GetStat(pSoldierinfo, nLevel, nGrade2);
		return new int[]
		{
			array2[0] - array[0],
			array2[1] - array[1],
			array2[2] - array[2],
			array2[3] - array[3]
		};
	}

	private int[] GetGradeChangeValue(NkSoldierInfo pSoldierinfo, short nLevel, int PreGrade, int CurGrade)
	{
		int[] array = new int[4];
		int[] array2 = new int[4];
		array = this.GetStat(pSoldierinfo, nLevel, PreGrade);
		array2 = this.GetStat(pSoldierinfo, nLevel, CurGrade);
		return new int[]
		{
			array2[0] - array[0],
			array2[1] - array[1],
			array2[2] - array[2],
			array2[3] - array[3]
		};
	}

	private int[] GetPropertys(CHARKIND_SOLSTATINFO PreStateInfo, CHARKIND_SOLSTATINFO StateInfo)
	{
		return new int[]
		{
			StateInfo.STR - PreStateInfo.STR,
			StateInfo.DEX - PreStateInfo.DEX,
			StateInfo.INT - PreStateInfo.INT,
			StateInfo.VIT - PreStateInfo.VIT,
			StateInfo.MIN_DAMAGE - PreStateInfo.MIN_DAMAGE,
			StateInfo.MAX_DAMAGE - PreStateInfo.MAX_DAMAGE,
			StateInfo.DEFENSE - PreStateInfo.DEFENSE,
			StateInfo.MAGICDEFENSE - PreStateInfo.MAGICDEFENSE,
			StateInfo.HP - PreStateInfo.HP,
			StateInfo.CRITICAL - PreStateInfo.CRITICAL,
			StateInfo.HITRATE - PreStateInfo.HITRATE,
			StateInfo.EVASION - PreStateInfo.EVASION
		};
	}

	private int[] GetPropertys(CHARKIND_SOLSTATINFO StateInfo)
	{
		return new int[]
		{
			StateInfo.STR,
			StateInfo.DEX,
			StateInfo.INT,
			StateInfo.VIT,
			StateInfo.MIN_DAMAGE,
			StateInfo.MAX_DAMAGE,
			StateInfo.DEFENSE,
			StateInfo.MAGICDEFENSE,
			StateInfo.HP,
			StateInfo.CRITICAL,
			StateInfo.HITRATE,
			StateInfo.EVASION
		};
	}

	public override void SetComponent()
	{
		this.lbName = (base.GetControl("Label_character_name") as Label);
		this.SolRank = (base.GetControl("DrawTexture_SolRank") as DrawTexture);
		this.dtImg = (base.GetControl("DrawTexture_character") as DrawTexture);
		this.dtGradeUpImg = (base.GetControl("DrawTexture_StrengthMark") as DrawTexture);
		this.lbGradeUpText = (base.GetControl("Label_StrengthLabel") as Label);
		this.lbGradeUpText.SetText(string.Empty);
		this.lbExp = (base.GetControl("Label_stats_exp2") as Label);
		this.lbExpRate = (base.GetControl("Label_EXPRATE") as Label);
		this.pbGage = (base.GetControl("ProgressBar_EXPBAR") as ProgressBar);
		this.lbLevel = new Label[4];
		this.lbLevel[0] = (base.GetControl("Label_stats_lv1") as Label);
		this.lbLevel[1] = (base.GetControl("Label_stats_lv2") as Label);
		this.lbLevel[2] = (base.GetControl("Label_Arrow_Lv") as Label);
		this.lbLevel[3] = (base.GetControl("Label_stats_lv3") as Label);
		this.GradeExpBG = (base.GetControl("DrawTexture_GradePRGBG") as DrawTexture);
		this.GradeExpGage = (base.GetControl("DrawTexture_GradePRG") as DrawTexture);
		this.GradeExpText = (base.GetControl("Label_GradeText") as Label);
		this.m_txPreRank = (base.GetControl("DrawTexture_EvoRank1") as DrawTexture);
		this.m_txCurRank = (base.GetControl("DrawTexture_EvoRank2") as DrawTexture);
		this.m_lbGetGradeExpText = (base.GetControl("Label_evo_exp2") as Label);
		this.m_lbLevelUpStat = new Label[4];
		this.m_lbLevelUpStat[0] = (base.GetControl("LvUp_STR_02") as Label);
		this.m_lbLevelUpStat[1] = (base.GetControl("LvUp_VIT_02") as Label);
		this.m_lbLevelUpStat[2] = (base.GetControl("LvUp_DEX_02") as Label);
		this.m_lbLevelUpStat[3] = (base.GetControl("LvUp_INT_02") as Label);
		this.m_lbRankUpStat = new Label[4];
		this.m_lbRankUpStat[0] = (base.GetControl("RankUp_STR_02") as Label);
		this.m_lbRankUpStat[1] = (base.GetControl("RankUp_VIT_02") as Label);
		this.m_lbRankUpStat[2] = (base.GetControl("RankUp_DEX_02") as Label);
		this.m_lbRankUpStat[3] = (base.GetControl("RankUp_INT_02") as Label);
		this.m_txUpGrade = (base.GetControl("Dummy_EvoEffect") as DrawTexture);
		this.btnOk = (base.GetControl("BT_Feed") as Button);
		Button expr_2BA = this.btnOk;
		expr_2BA.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2BA.Click, new EZValueChangedDelegate(this.BtnClickOk));
		this.SetData(0, 0, 10L, null, 0L, 0L);
		base.SetScreenCenter();
		this.Hide();
	}

	public void LoadSolComposeSuccessBundle()
	{
		string path = string.Format("{0}", "UI/Soldier/fx_success" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		path = string.Format("{0}", "UI/Soldier/fx_evolution_result" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_txUpGrade, this.m_txUpGrade.GetSize());
	}

	public void LoadSolLevelSuccessBundle()
	{
		string path = string.Format("{0}", "UI/Soldier/fx_levelup_result" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect(path, this.m_txUpGrade, this.m_txUpGrade.GetSize());
	}

	private void SolComposeSuccess(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				if (null == this.rootGameObject)
				{
					return;
				}
				NkUtil.SetAllChildLayer(this.rootGameObject, GUICamera.UILayer);
				base.InteractivePanel.MakeChild(this.rootGameObject);
				DrawTexture drawTexture = _param as DrawTexture;
				if (_param == null)
				{
					this.rootGameObject.transform.localPosition = new Vector3(430f, -170f, -1f);
				}
				else
				{
					this.rootGameObject.transform.localPosition = drawTexture.GetLocation();
				}
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.rootGameObject);
				}
				this.Show();
				if (this.m_TargetSolInfo.IsLeader() && this.m_bLevelUP)
				{
					AlarmManager.GetInstance().ShowLevelUpAlarm1();
					AlarmManager.GetInstance().ShowLevelUpAlarm2();
				}
			}
		}
	}

	public void SetData(byte bPreGrade, int lPreLevel, long lExp, NkSoldierInfo kSolInfo, long nAddEvolutionExp, long nMaxLvEvolution)
	{
		this.m_TargetSolInfo = kSolInfo;
		if (kSolInfo != null)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1731");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
			{
				textFromInterface,
				"count",
				lExp
			});
			this.lbExp.SetText(textFromInterface);
			this.lbName.SetText(kSolInfo.GetName());
			if (!NrTSingleton<ContentsLimitManager>.Instance.IsReincarnation())
			{
				if (kSolInfo.IsLeader())
				{
					this.SolRank.Visible = false;
				}
				else
				{
					UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(kSolInfo.GetCharKind(), (int)kSolInfo.GetGrade());
					if (solLargeGradeImg != null)
					{
						if (0 < NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(kSolInfo.GetCharKind(), (int)kSolInfo.GetGrade()))
						{
							this.SolRank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
						}
						this.SolRank.SetTexture(solLargeGradeImg);
					}
				}
			}
			else
			{
				UIBaseInfoLoader solLargeGradeImg2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(kSolInfo.GetCharKind(), (int)kSolInfo.GetGrade());
				if (solLargeGradeImg2 != null)
				{
					if (0 < NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(kSolInfo.GetCharKind(), (int)kSolInfo.GetGrade()))
					{
						this.SolRank.SetSize(solLargeGradeImg2.UVs.width, solLargeGradeImg2.UVs.height);
					}
					this.SolRank.SetTexture(solLargeGradeImg2);
				}
			}
			int costumeUnique = (int)kSolInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
			this.dtImg.SetTexture(eCharImageType.LARGE, kSolInfo.GetCharKind(), (int)kSolInfo.GetGrade(), NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(costumeUnique));
			long num = kSolInfo.GetNextExp() - kSolInfo.GetCurBaseExp();
			float num2 = ((float)num - (float)kSolInfo.GetRemainExp()) / (float)num;
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			if (kSolInfo.IsMaxLevel())
			{
				string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
				this.lbExpRate.SetText(textFromInterface2);
				num2 = 1f;
			}
			else
			{
				this.lbExpRate.SetText(string.Format("{0:F2}%", num2 * 100f));
			}
			this.pbGage.Value = num2;
			BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = kSolInfo.GetCharKindInfo().GetCHARKIND_SOLGRADEINFO((int)kSolInfo.GetGrade());
			BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO2 = kSolInfo.GetCharKindInfo().GetCHARKIND_SOLGRADEINFO((int)bPreGrade);
			if (cHARKIND_SOLGRADEINFO != null && cHARKIND_SOLGRADEINFO2 != null)
			{
				int level = (int)kSolInfo.GetLevel();
				bool flag = lPreLevel != level;
				this.lbLevel[1].SetText(lPreLevel.ToString());
				this.lbLevel[2].Visible = flag;
				this.lbLevel[3].Visible = flag;
				this.lbLevel[3].SetText(level.ToString());
				this.m_bLevelUP = (lPreLevel != level);
				if (flag)
				{
					base.SetShowLayer(2, true);
				}
				else
				{
					base.SetShowLayer(2, false);
				}
				if (flag)
				{
					int[] levelChangeValue = this.GetLevelChangeValue(kSolInfo, (short)lPreLevel, (short)level, (int)bPreGrade);
					this.SetTextShowOrNot(ref this.m_lbLevelUpStat, levelChangeValue);
				}
			}
			if (nAddEvolutionExp > 0L)
			{
				textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1731");
				if (nMaxLvEvolution > 0L)
				{
					textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2593");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
					{
						textFromInterface,
						"count",
						nAddEvolutionExp,
						"count1",
						nAddEvolutionExp - nMaxLvEvolution,
						"count2",
						nMaxLvEvolution
					});
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
					{
						textFromInterface,
						"count",
						nAddEvolutionExp
					});
				}
				if (!kSolInfo.IsMaxGrade())
				{
					this.m_lbGetGradeExpText.SetText(textFromInterface);
				}
				else
				{
					this.m_lbGetGradeExpText.SetText(string.Empty);
				}
				base.SetShowLayer(3, true);
				bool flag2 = kSolInfo.GetGrade() != bPreGrade;
				this.dtGradeUpImg.Visible = flag2;
				bool flag3 = false;
				if (COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TRADECOUNT_USE) == 1 && flag2 && kSolInfo.IsMaxGrade())
				{
					int num3 = (int)kSolInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_TRADE_COUNT);
					this.lbGradeUpText.Visible = true;
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2127"),
						"count",
						num3
					});
					this.lbGradeUpText.SetText(empty);
					flag3 = true;
				}
				if (!flag3)
				{
					this.lbGradeUpText.Visible = false;
				}
				if (flag2)
				{
					short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(kSolInfo.GetCharKind(), (int)bPreGrade);
					UIBaseInfoLoader solLargeGradeImg3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(kSolInfo.GetCharKind(), (int)bPreGrade);
					if (0 < legendType)
					{
						this.m_txPreRank.SetSize(solLargeGradeImg3.UVs.width, solLargeGradeImg3.UVs.height);
					}
					this.m_txPreRank.SetTexture(solLargeGradeImg3);
					legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(kSolInfo.GetCharKind(), (int)(bPreGrade + 1));
					solLargeGradeImg3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(kSolInfo.GetCharKind(), (int)(bPreGrade + 1));
					if (0 < legendType)
					{
						this.m_txCurRank.SetSize(solLargeGradeImg3.UVs.width, solLargeGradeImg3.UVs.height);
					}
					this.m_txCurRank.SetTexture(solLargeGradeImg3);
					base.SetShowLayer(4, true);
					int[] gradeChangeValue = this.GetGradeChangeValue(kSolInfo, kSolInfo.GetLevel(), (int)bPreGrade, (int)kSolInfo.GetGrade());
					this.SetTextShowOrNot(ref this.m_lbRankUpStat, gradeChangeValue);
				}
				else
				{
					base.SetShowLayer(4, false);
				}
				this.GradeExpBG.Visible = true;
				this.GradeExpGage.Visible = true;
				this.GradeExpText.Visible = true;
				long num4 = kSolInfo.GetEvolutionExp() - kSolInfo.GetCurBaseEvolutionExp();
				long num5 = kSolInfo.GetNextEvolutionExp() - kSolInfo.GetCurBaseEvolutionExp();
				float evolutionExpPercent = kSolInfo.GetEvolutionExpPercent();
				string text = string.Empty;
				this.GradeExpGage.SetSize(228f * evolutionExpPercent, this.GradeExpGage.height);
				if (!kSolInfo.IsMaxGrade())
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1871"),
						"exp",
						num4.ToString(),
						"maxexp",
						num5.ToString()
					});
				}
				else
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("484");
				}
				this.GradeExpText.SetText(text);
			}
			else
			{
				base.SetShowLayer(3, false);
				base.SetShowLayer(4, false);
			}
		}
	}

	private void BtnClickOk(IUIObject obj)
	{
		this.HideTouch(false);
		SolComposeMainDlg_challengequest solComposeMainDlg_challengequest = (SolComposeMainDlg_challengequest)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG);
		if (solComposeMainDlg_challengequest != null)
		{
			solComposeMainDlg_challengequest.ClearRecommendChallenge();
		}
		this.Close();
	}

	private void ClickTimeLine(IUIObject obj)
	{
	}

	public override void OnClose()
	{
		this.HideTouch(true);
		base.OnClose();
		Resources.UnloadUnusedAssets();
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
		IUIObject control = base.GetControl(array[0]);
		if (control == null)
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
		this._Touch.gameObject.transform.parent = control.gameObject.transform;
		this._Touch.transform.position = new Vector3(control.transform.position.x, control.transform.position.y, control.transform.position.z - 3f);
		float x = float.Parse(array[2]);
		float y = float.Parse(array[3]);
		this._Touch.transform.eulerAngles = new Vector3(x, y, this._Touch.transform.eulerAngles.z);
		BoxCollider component = this._Touch.gameObject.GetComponent<BoxCollider>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
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
}
