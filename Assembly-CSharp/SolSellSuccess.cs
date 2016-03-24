using System;
using UnityForms;

public class SolSellSuccess : Form
{
	private DrawTexture m_dtBG;

	private Button m_btnOk;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/Compose/DLG_SolSellSuccess", G_ID.SOLCOMPOSE_SELL_SUCCESS, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_dtBG = (base.GetControl("DrawTexture_NPCImg") as DrawTexture);
		this.m_dtBG.SetTexture(eCharImageType.LARGE, 242, -1, string.Empty);
		this.m_btnOk = (base.GetControl("Button_OK") as Button);
		this.m_btnOk.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		base.SetScreenCenter();
	}
}
