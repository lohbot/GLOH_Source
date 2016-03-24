using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class SolMilitarySelectDlg_challengequest : SolMilitarySelectDlg
{
	private List<int> guideWinIDList = new List<int>();

	private UIButton _Touch;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolMilitarySelect", G_ID.SOLMILITARYSELECT_CHALLENGEQUEST_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public void SetDummySoldierList(int charkind)
	{
		this.SoldierList.Clear();
		base.SetSolListInfo(this.CreateDummySoldier(charkind));
		this.SoldierList.RepositionItems();
		this.SoldierList.SetSelectedItem(0);
		NrTSingleton<EventConditionHandler>.Instance.MythElementSelectSet.OnTrigger();
		this.SolSortTypeList.Clear();
		this.SolSortTypeList.SetIndex(-1);
		this.SolSortTypeList.controlIsEnabled = false;
		this.SolSortOrderList.Clear();
		this.SolSortOrderList.SetIndex(-1);
		this.SolSortOrderList.controlIsEnabled = false;
	}

	protected override void OnClickSoldierSelectConfirmOK(IUIObject obj)
	{
		if (this.SoldierList.SelectedItem == null)
		{
			return;
		}
		Myth_Legend_Info_DLG_ChallengeQuest myth_Legend_Info_DLG_ChallengeQuest = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_LEGEND_INFO_CHALLENGEQUEST_DLG) as Myth_Legend_Info_DLG_ChallengeQuest;
		if (myth_Legend_Info_DLG_ChallengeQuest != null)
		{
			myth_Legend_Info_DLG_ChallengeQuest.ReadyLegendEvolution();
		}
		base.CloseNow();
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
		string text = array[0];
		UIListItemContainer item = this.SoldierList.GetItem(0);
		if (item == null)
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
		this._Touch.gameObject.transform.parent = item.gameObject.transform;
		this._Touch.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z - 3f);
		float x = float.Parse(array[2]);
		float y = float.Parse(array[3]);
		this._Touch.transform.eulerAngles = new Vector3(x, y, this._Touch.transform.eulerAngles.z);
		BoxCollider component = this._Touch.gameObject.GetComponent<BoxCollider>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	protected override void OnChangeSortType(IUIObject obj)
	{
	}

	private NkSoldierInfo CreateDummySoldier(int charkind)
	{
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.SetCharKind(charkind);
		nkSoldierInfo.SetGrade(5);
		nkSoldierInfo.SetLevel(50);
		nkSoldierInfo.SetSolID(123456L);
		nkSoldierInfo.SetSolSubData(3, 0L);
		nkSoldierInfo.SetExp(nkSoldierInfo.GetCurBaseExp());
		return nkSoldierInfo;
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
