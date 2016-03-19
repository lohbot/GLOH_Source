using System;
using UnityForms;

public class GMCommand_Dlg : Form
{
	private DropDownList m_lbCommand;

	private TextField m_lbInputText;

	private Button m_lbSearch;

	private ListBox m_lbCommandList;

	private CheckBox m_lcShowNavPath;

	public static bool m_bShowNavPath;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "GMCommand/dlg_gmcommand", G_ID.GMCOMMAND_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_lbCommandList = (base.GetControl("ListBox_ListBox0") as ListBox);
		this.m_lbCommandList.AddValueChangedDelegate(new EZValueChangedDelegate(this.SelectCommandList));
		this.m_lbCommandList.SelectStyle = "Win_T_DropDwBtn";
		this.m_lbCommandList.ColumnNum = 1;
		this.m_lbCommandList.LineHeight = 28f;
		this.m_lbCommandList.FontEffect = SpriteText.Font_Effect.White_Shadow_Small;
		this.m_lbCommand = (base.GetControl("DropDownList_DropDownList5") as DropDownList);
		this.m_lbInputText = (base.GetControl("TextField_TextField3") as TextField);
		this.m_lbSearch = (base.GetControl("Button_Button4") as Button);
		this.m_lcShowNavPath = (base.GetControl("CheckBox_ShowNav") as CheckBox);
		this.m_lcShowNavPath.SetCheckState((!GMCommand_Dlg.m_bShowNavPath) ? 0 : 1);
		CheckBox expr_DF = this.m_lcShowNavPath;
		expr_DF.CheckedChanged = (EZValueChangedDelegate)Delegate.Combine(expr_DF.CheckedChanged, new EZValueChangedDelegate(this.CheckChange));
		this.m_lbCommand.Visible = false;
		this.m_lbInputText.Visible = false;
		this.m_lbSearch.Visible = false;
		base.SetScreenCenter();
		this.SetCommandList();
	}

	public override void InitData()
	{
		base.InitData();
	}

	private void SetCommandList()
	{
	}

	private void SelectCommandList(IUIObject a_cUIObject)
	{
		IUIListObject selectItem = this.m_lbCommandList.GetSelectItem();
		if (selectItem.Data != null)
		{
		}
	}

	public override void OnClose()
	{
	}

	private void CheckChange(IUIObject a_cUIObject)
	{
		GMCommand_Dlg.m_bShowNavPath = this.m_lcShowNavPath.IsChecked();
	}
}
