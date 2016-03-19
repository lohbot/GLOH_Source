using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class IndunEnterScenario_DLG : Form
{
	private DrawTexture m_dtBackImg;

	private Label m_lbTitle;

	private Button m_btEnter;

	private TsWeakReference<INDUN_INFO> m_pkIndunInfo;

	private Texture2D m_IndunUIBackTexture;

	private int m_nRequestIndunIDX = -1;

	private short m_nRequestNpcUnique;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Indun/dlg_indunenter_scenario", G_ID.INDUN_ENTER_SCENARIO_DLG, true);
		base.ChangeSceneDestory = false;
		this.Hide();
	}

	public override void SetComponent()
	{
		this.m_dtBackImg = (base.GetControl("DrawTexture_Img") as DrawTexture);
		this.m_btEnter = (base.GetControl("Button_Enter") as Button);
		this.m_lbTitle = (base.GetControl("Label_ScenarioTitle") as Label);
		Button expr_48 = this.m_btEnter;
		expr_48.Click = (EZValueChangedDelegate)Delegate.Combine(expr_48.Click, new EZValueChangedDelegate(this.RequestIndunScenarioOpen));
	}

	public void Set(int nIndunIDX, short nNpcCharUnique)
	{
		this.m_pkIndunInfo = NrTSingleton<NrBaseTableManager>.Instance.GetIndunInfo(nIndunIDX.ToString());
		this.m_nRequestIndunIDX = nIndunIDX;
		this.m_nRequestNpcUnique = nNpcCharUnique;
		this.m_lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromMap(this.m_pkIndunInfo.CastedTarget.szTextKey));
		this.RequestIndunScenarioOpen(null);
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, GUICamera.height / 2f - base.GetSize().y / 2f);
	}

	public override void OnClose()
	{
	}

	public override void Update()
	{
	}

	private void RequestIndunScenarioOpen(IUIObject obj)
	{
		if (this.m_nRequestIndunIDX <= -1)
		{
			return;
		}
		if (this.m_nRequestNpcUnique <= 0)
		{
			return;
		}
		GS_INDUN_OPEN_REQ gS_INDUN_OPEN_REQ = new GS_INDUN_OPEN_REQ();
		gS_INDUN_OPEN_REQ.nIndunIDX = this.m_nRequestIndunIDX;
		gS_INDUN_OPEN_REQ.nNpcCharUnique = this.m_nRequestNpcUnique;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INDUN_OPEN_REQ, gS_INDUN_OPEN_REQ);
		this.Close();
	}

	public bool LoadRequestIndunUIBackTexture()
	{
		if (this.m_pkIndunInfo == null)
		{
			Debug.Log("IndunInfo is NULL");
			return false;
		}
		this.m_IndunUIBackTexture = null;
		string str = "UI/Indun";
		string text = string.Empty;
		if (string.Empty != this.m_pkIndunInfo.CastedTarget.IndunImagePath && this.m_pkIndunInfo.CastedTarget.IndunImagePath.ToUpper() != "NULL")
		{
			text = this.m_pkIndunInfo.CastedTarget.IndunImagePath;
		}
		else
		{
			text = "burnpan";
			Debug.LogError(string.Format("Fail Load {0}", this.m_pkIndunInfo.CastedTarget.IndunImagePath));
		}
		if (string.Empty != text && 2 < text.Length)
		{
			string path = str + "/" + text;
			if (!NrTSingleton<FormsManager>.Instance.RequestUIBundleDownLoad(path, new PostProcPerItem(this.LoadCompleteIndunUIBackTexture), text))
			{
				return false;
			}
		}
		return true;
	}

	private void LoadCompleteIndunUIBackTexture(WWWItem _item, object _param)
	{
		if (_item != null && _item.canAccessAssetBundle)
		{
			if (null == this.m_IndunUIBackTexture)
			{
				this.m_IndunUIBackTexture = (_item.mainAsset as Texture2D);
			}
			this.m_dtBackImg.SetTexture(this.m_IndunUIBackTexture);
			this.Show();
			base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, GUICamera.height / 2f - base.GetSize().y / 2f);
		}
	}
}
