using System;
using UnityEngine;
using UnityForms;

public class SolComposeGradeUpSuccessDlg : Form
{
	private const int VALID_PROPERTY_MAX = 4;

	private Label lbAddExp;

	private Label lbAddLevel;

	private Label lbName;

	private Label[] lbPropertyText;

	private Label[] lbPropertyValue;

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

	private Button btnOk;

	private ItemTexture itSoldier;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Soldier/dlg_SolGradeupsuccess", G_ID.SOLCOMPOSE_GRADE_UP_SUCCESS_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.lbAddExp = (base.GetControl("Label_text1") as Label);
		this.lbAddLevel = (base.GetControl("Label_text2") as Label);
		this.lbName = (base.GetControl("Label_Label9") as Label);
		this.lbPropertyText = new Label[4];
		this.lbPropertyText[0] = (base.GetControl("Label_text4") as Label);
		this.lbPropertyText[1] = (base.GetControl("Label_text6") as Label);
		this.lbPropertyText[2] = (base.GetControl("Label_text8") as Label);
		this.lbPropertyText[3] = (base.GetControl("Label_text10") as Label);
		this.lbPropertyValue = new Label[4];
		this.lbPropertyValue[0] = (base.GetControl("Label_text5") as Label);
		this.lbPropertyValue[1] = (base.GetControl("Label_text7") as Label);
		this.lbPropertyValue[2] = (base.GetControl("Label_text9") as Label);
		this.lbPropertyValue[3] = (base.GetControl("Label_text11") as Label);
		this.btnOk = (base.GetControl("Button_ok") as Button);
		Button expr_136 = this.btnOk;
		expr_136.Click = (EZValueChangedDelegate)Delegate.Combine(expr_136.Click, new EZValueChangedDelegate(this.BtnClickOk));
		this.itSoldier = (base.GetControl("ItemTexture_Soldier") as ItemTexture);
		base.SetScreenCenter();
		this.SetData(0, 0L, 0L, null);
	}

	public void SetData(byte bPreGrade, long lPreLevel, long lGetExp, NkSoldierInfo kSolInfo)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		int charkind = 0;
		int solgrade = 0;
		if (kSolInfo != null)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1731");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text,
				"count",
				lGetExp
			});
			if (lPreLevel != (long)kSolInfo.GetLevel())
			{
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1732");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text2,
					"count1",
					lPreLevel,
					"count2",
					kSolInfo.GetLevel()
				});
			}
			BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO = kSolInfo.GetCharKindInfo().GetCHARKIND_SOLGRADEINFO((int)bPreGrade);
			CHARKIND_SOLSTATINFO kSolStatInfo = cHARKIND_SOLGRADEINFO.kSolStatInfo;
			BASE_SOLGRADEINFO cHARKIND_SOLGRADEINFO2 = kSolInfo.GetCharKindInfo().GetCHARKIND_SOLGRADEINFO((int)kSolInfo.GetGrade());
			CHARKIND_SOLSTATINFO kSolStatInfo2 = cHARKIND_SOLGRADEINFO2.kSolStatInfo;
			text3 = kSolInfo.GetName();
			charkind = kSolInfo.GetCharKind();
			solgrade = (int)kSolInfo.GetGrade();
			NrSound.ImmedatePlay("UI_SFX", "QUEST", "SOLDIERRECRUIT");
			short[] propertys = this.GetPropertys(kSolStatInfo, kSolStatInfo2);
			if (propertys.Length != this.arPerpertyKeys.Length)
			{
				Debug.LogError(string.Format("Property size V:{0} != S:{1}", propertys.Length, this.arPerpertyKeys.Length));
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				this.lbPropertyText[i].Visible = false;
				this.lbPropertyValue[i].Visible = false;
			}
			int num = 0;
			for (int j = 0; j < propertys.Length; j++)
			{
				string strTextKey = this.arPerpertyKeys[j];
				short num2 = propertys[j];
				if (0 < num2)
				{
					this.lbPropertyText[num].Visible = true;
					this.lbPropertyText[num].SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey));
					this.lbPropertyValue[num].Visible = true;
					this.lbPropertyValue[num].SetText(num2.ToString());
					num++;
					if (num > 4)
					{
						Debug.LogError(string.Format("VALID_PROPERTY_MAX {0}", j));
						break;
					}
				}
			}
		}
		this.itSoldier.SetSolImageTexure(eCharImageType.LARGE, charkind, solgrade);
		this.lbAddExp.SetText(text);
		this.lbAddLevel.SetText(text2);
		this.lbName.SetText(text3);
	}

	private short[] GetPropertys(CHARKIND_SOLSTATINFO PreStateInfo, CHARKIND_SOLSTATINFO StateInfo)
	{
		return new short[]
		{
			(short)(StateInfo.STR - PreStateInfo.STR),
			(short)(StateInfo.DEX - PreStateInfo.DEX),
			(short)(StateInfo.VIT - PreStateInfo.VIT),
			(short)(StateInfo.INT - PreStateInfo.INT),
			(short)(StateInfo.MIN_DAMAGE - PreStateInfo.MIN_DAMAGE),
			(short)(StateInfo.MAX_DAMAGE - PreStateInfo.MAX_DAMAGE),
			(short)(StateInfo.DEFENSE - PreStateInfo.DEFENSE),
			(short)(StateInfo.MAGICDEFENSE - PreStateInfo.MAGICDEFENSE),
			(short)(StateInfo.HP - PreStateInfo.HP),
			(short)(StateInfo.CRITICAL - PreStateInfo.CRITICAL),
			(short)(StateInfo.HITRATE - PreStateInfo.HITRATE),
			(short)(StateInfo.EVASION - PreStateInfo.EVASION)
		};
	}

	private void BtnClickOk(IUIObject obj)
	{
		this.Close();
	}
}
