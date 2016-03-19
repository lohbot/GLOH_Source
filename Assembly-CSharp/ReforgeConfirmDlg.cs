using GAME;
using System;
using UnityForms;

public class ReforgeConfirmDlg : Form
{
	private Button m_btnOK;

	private Button m_btnCancle;

	private Label m_lbContext;

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
		if (reforgeMainDlg != null)
		{
			reforgeMainDlg.ActionReforge();
		}
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
}
