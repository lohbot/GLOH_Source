using System;
using UnityForms;

public class BabelTower_ModeSelect : Form
{
	private DrawTexture m_txBG;

	private DrawTexture m_txNormalBG;

	private DrawTexture m_txHardBG;

	private Button m_btNormalBig;

	private Button m_btNormalSmall;

	private Button m_btHardBig;

	private Button m_btHardSmall;

	private string m_normalmode_img;

	private string m_hardmode_img;

	private string m_babelBG_img;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		form.AlwaysUpdate = true;
		instance.LoadFileAll(ref form, "BabelTower/DLG_Babel_ModeSelect", G_ID.BABELTOWER_MODESELECT_DLG, false, true);
		base.TopMost = true;
		base.ShowBlackBG(1f);
		this.m_normalmode_img = "UI/babeltower/babel_mode_normal";
		this.m_hardmode_img = "UI/babeltower/babel_mode_hard";
		this.m_babelBG_img = "UI/babeltower/babel_bg";
	}

	public override void SetComponent()
	{
		this.m_txBG = (base.GetControl("DT_SubBG") as DrawTexture);
		this.m_txBG.SetTextureFromBundle(this.m_babelBG_img);
		this.m_btNormalBig = (base.GetControl("BT_normal1") as Button);
		this.m_btNormalBig.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnBtnNormalMode));
		this.m_btNormalSmall = (base.GetControl("BT_normal2") as Button);
		this.m_btNormalSmall.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnBtnNormalMode));
		this.m_btHardBig = (base.GetControl("BT_hard1") as Button);
		this.m_btHardBig.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnBtnHardMode));
		this.m_btHardSmall = (base.GetControl("BT_hard2") as Button);
		this.m_btHardSmall.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnBtnHardMode));
		this.m_txNormalBG = (base.GetControl("DT_mode_normal") as DrawTexture);
		this.m_txNormalBG.SetTextureFromBundle(this.m_normalmode_img);
		this.m_txHardBG = (base.GetControl("DT_mode_hard") as DrawTexture);
		this.m_txHardBG.SetTextureFromBundle(this.m_hardmode_img);
		base.SetScreenCenter();
	}

	public override void OnClose()
	{
		if (NrTSingleton<FormsManager>.Instance.IsPopUPDlgNotExist(base.WindowID))
		{
			NrTSingleton<UIImageBundleManager>.Instance.DeleteTexture(this.m_babelBG_img);
			NrTSingleton<UIImageBundleManager>.Instance.DeleteTexture(this.m_normalmode_img);
			NrTSingleton<UIImageBundleManager>.Instance.DeleteTexture(this.m_hardmode_img);
		}
		base.OnClose();
	}

	public void OnBtnHardMode(IUIObject obj)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo.GetBabelSubFloorRankInfo(100, 4, 1) > 0)
		{
			this.DirctionUI(2);
		}
		else
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("777"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	public void OnBtnNormalMode(IUIObject obj)
	{
		this.DirctionUI(1);
	}

	private void DirctionUI(short nFloorType)
	{
		DirectionDLG directionDLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_DIRECTION) as DirectionDLG;
		if (directionDLG != null)
		{
			directionDLG.ShowDirection(DirectionDLG.eDIRECTIONTYPE.eDIRECTION_BABEL, (int)nFloorType);
		}
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.ADVENTURECOLLECT_DLG))
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ADVENTURECOLLECT_DLG);
		}
		this.Close();
	}
}
