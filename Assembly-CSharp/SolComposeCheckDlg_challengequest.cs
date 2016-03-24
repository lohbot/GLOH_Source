using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class SolComposeCheckDlg_challengequest : SolComposeCheckDlg
{
	private NkSoldierInfo _dummyComposeBaseSol;

	private NkSoldierInfo _dummyComposeMaterialSol;

	private List<int> guideWinIDList = new List<int>();

	private UIButton _Touch;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/dlg_solcomposecheck", G_ID.SOLCOMPOSE_CHECK_CHALLENGEQUEST_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void OnClose()
	{
		this.HideTouch(true);
		base.OnClose();
	}

	public void SetData(NkSoldierInfo composeBaseSol, NkSoldierInfo composeMaterialSol, SOLCOMPOSE_TYPE _Type = SOLCOMPOSE_TYPE.COMPOSE)
	{
		if (composeMaterialSol == null)
		{
			this.Close();
			return;
		}
		this._dummyComposeBaseSol = composeBaseSol;
		this._dummyComposeMaterialSol = composeMaterialSol;
		this.lbName.SetText(composeMaterialSol.GetName());
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1031");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromInterface, new object[]
		{
			textFromInterface,
			"count",
			composeMaterialSol.GetLevel()
		});
		this.lbLevel.SetText(textFromInterface);
		this.dtSoldier.SetTexture(eCharImageType.SMALL, composeMaterialSol.GetCharKind(), (int)composeMaterialSol.GetGrade(), string.Empty);
		this.lbSubNum.Visible = false;
		this.lbGold.SetText(string.Format("{0:###,###,###,##0}", SolComposeMainDlg.Instance.COST));
		this.lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2033"));
		this.lbExplain.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1717"));
		this.lbMoneyName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1728"));
		this.btnOk.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1729"));
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

	protected override void BtnClickOk(IUIObject obj)
	{
		if (this._dummyComposeMaterialSol == null)
		{
			return;
		}
		this.HideTouch(false);
		short level = this._dummyComposeBaseSol.GetLevel();
		byte grade = this._dummyComposeBaseSol.GetGrade();
		this._dummyComposeBaseSol.SetGrade(5);
		SolComposeDirection solComposeDirection = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_DIRECTION_DLG) as SolComposeDirection;
		if (solComposeDirection == null)
		{
			return;
		}
		solComposeDirection.SetImage(this._dummyComposeBaseSol, 1);
		SolComposeSuccessDlg solComposeSuccessDlg = (SolComposeSuccessDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_SUCCESS_DLG);
		if (solComposeSuccessDlg != null)
		{
			solComposeSuccessDlg.SetData(grade, (int)level, 0L, this._dummyComposeBaseSol, 2000L, 0L);
			solComposeSuccessDlg.Hide();
		}
		this.Close();
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
