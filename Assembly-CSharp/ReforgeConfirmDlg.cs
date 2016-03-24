using GAME;
using System;
using UnityEngine;
using UnityForms;

public class ReforgeConfirmDlg : Form
{
	private Button m_btnOK;

	private Button m_btnCancle;

	private Label m_lbContext;

	private UIButton _GuideItem;

	private float _ButtonZ;

	private int m_nWinID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Reforge/DLG_ReforgeConfirm", G_ID.REFORGECONFIRM_DLG, true);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_btnOK = (base.GetControl("Button_confirm") as Button);
		this.m_btnOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickOK));
		this.m_btnCancle = (base.GetControl("Button_cancle") as Button);
		this.m_btnCancle.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickCancle));
		this.m_lbContext = (base.GetControl("Label_contents") as Label);
	}

	public void SetData(ITEM item, long money, int nMatItemUni, int nMatItemNum)
	{
		string name = NrTSingleton<ItemManager>.Instance.GetName(item);
		eITEM_RANK_TYPE rank = item.GetRank();
		string rankName = NrTSingleton<ItemManager>.Instance.GetRankName(item);
		string text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1992");
		string empty = string.Empty;
		if (money != 0L)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1992");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"money",
				money.ToString(),
				"itemname2",
				ItemManager.RankTextColor(rank) + name,
				"grade",
				rankName
			});
		}
		else
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("178");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"targetname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(nMatItemUni),
				"count",
				nMatItemNum.ToString(),
				"targetname1",
				ItemManager.RankTextColor(rank) + name
			});
		}
		this.m_lbContext.Text = empty;
	}

	public void OnClickOK(IUIObject a_oObject)
	{
		ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
		if (reforgeMainDlg != null && reforgeMainDlg.IsCheck())
		{
			reforgeMainDlg.ActionReforge();
		}
		this.HideUIGuide();
		this.Close();
	}

	public void OnClickCancle(IUIObject a_oObject)
	{
		this.Close();
		ReforgeSelectDlg reforgeSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGESELECT_DLG) as ReforgeSelectDlg;
		if (reforgeSelectDlg != null)
		{
			reforgeSelectDlg.closeButton.Visible = true;
		}
	}

	public override void CloseForm(IUIObject obj)
	{
		base.CloseForm(obj);
		ReforgeSelectDlg reforgeSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGESELECT_DLG) as ReforgeSelectDlg;
		if (reforgeSelectDlg != null)
		{
			reforgeSelectDlg.closeButton.Visible = true;
		}
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.depthChangeable = false;
		}
		this._GuideItem = (base.GetControl(param1) as UIButton);
		this.m_nWinID = winID;
		if (null != this._GuideItem)
		{
			this._ButtonZ = this._GuideItem.GetLocation().z;
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				if (uI_UIGuide.GetLocation().z == base.GetLocation().z)
				{
					uI_UIGuide.SetLocation(uI_UIGuide.GetLocationX(), uI_UIGuide.GetLocationY(), uI_UIGuide.GetLocation().z - 10f);
				}
				this._GuideItem.EffectAni = false;
				Vector2 x = new Vector2(base.GetLocationX() + this._GuideItem.GetLocationX() + 72f, base.GetLocationY() + this._GuideItem.GetLocationY() - 17f);
				uI_UIGuide.Move(x, UI_UIGuide.eTIPPOS.BUTTOM);
				this._ButtonZ = this._GuideItem.gameObject.transform.localPosition.z;
				this._GuideItem.SetLocationZ(uI_UIGuide.GetLocation().z - base.GetLocation().z - 1f);
				this._GuideItem.AlphaAni(1f, 0.5f, -0.5f);
			}
		}
	}

	public void HideUIGuide()
	{
		if (null != this._GuideItem)
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			this._GuideItem.SetLocationZ(this._ButtonZ);
			this._GuideItem.StopAni();
			this._GuideItem.AlphaAni(1f, 1f, 0f);
			UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
			if (uI_UIGuide != null)
			{
				uI_UIGuide.Close();
			}
		}
		this._GuideItem = null;
	}
}
