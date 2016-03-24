using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class HeroCollect_DLG_challengequest : HeroCollect_DLG
{
	private int _challengeQuestUnique = -1;

	private List<int> guideWinIDList = new List<int>();

	private UIButton _Touch;

	private Label m_lbEvolution;

	private Label m_lbRecruit;

	private Label m_lbHeroInfo;

	private Label m_lbHeroCompose;

	public int _ChallengeQuestUnique
	{
		get
		{
			return this._challengeQuestUnique;
		}
		set
		{
			this._challengeQuestUnique = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "uibutton/DLG_Hero_Main", G_ID.HEROCOLLECT_CHALLENGEQUEST_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		base.SetComponent();
		this.m_lbEvolution = (base.GetControl("LB_Evolution") as Label);
		this.m_lbRecruit = (base.GetControl("LB_Recruit") as Label);
		this.m_lbHeroInfo = (base.GetControl("LB_HeroInfo") as Label);
		this.m_lbHeroCompose = (base.GetControl("LB_HeroCompose") as Label);
	}

	public override void OnClose()
	{
		this.HideTouch(true);
		base.OnClose();
	}

	public void SetDummyUI()
	{
		this.m_lbEvolution.controlIsEnabled = false;
		this.m_lbRecruit.controlIsEnabled = false;
		this.m_lbHeroInfo.controlIsEnabled = false;
		if (this._challengeQuestUnique != 1499 && this._challengeQuestUnique != 1502 && this._challengeQuestUnique != 1505)
		{
			this.m_lbHeroCompose.controlIsEnabled = false;
		}
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

	protected override void Click_HeroInfo(IUIObject Obj)
	{
	}

	protected override void Click_HeroCompose(IUIObject Obj)
	{
		if (this._challengeQuestUnique != 1499 && this._challengeQuestUnique != 1502 && this._challengeQuestUnique != 1505)
		{
			return;
		}
		SolComposeMainDlg_challengequest solComposeMainDlg_challengequest = (SolComposeMainDlg_challengequest)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_MAIN_CHALLENGEQUEST_DLG);
		if (solComposeMainDlg_challengequest != null)
		{
			this.Close();
			solComposeMainDlg_challengequest._ChallengeQuestUnique = this._challengeQuestUnique;
			if (this._challengeQuestUnique == 1499)
			{
				solComposeMainDlg_challengequest.SetComposeType(SOLCOMPOSE_TYPE.COMPOSE);
			}
			if (this._challengeQuestUnique == 1502)
			{
				solComposeMainDlg_challengequest.SetComposeType(SOLCOMPOSE_TYPE.EXTRACT);
			}
			if (this._challengeQuestUnique == 1505)
			{
				solComposeMainDlg_challengequest.SetComposeType(SOLCOMPOSE_TYPE.TRANSCENDENCE);
			}
			solComposeMainDlg_challengequest.Show();
		}
	}

	protected override void Click_Evolution(IUIObject Obj)
	{
	}

	protected override void Click_Recruit(IUIObject Obj)
	{
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
