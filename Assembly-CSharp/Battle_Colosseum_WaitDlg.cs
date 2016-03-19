using System;
using UnityEngine;
using UnityForms;

public class Battle_Colosseum_WaitDlg : Form
{
	private Label m_lbRemainTime;

	private DrawTexture m_dtLoading;

	private float m_fEndtime;

	private int m_nBeforeTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		Form form = this;
		instance.LoadFileAll(ref form, "Colosseum/dlg_wating", G_ID.BATTLE_COLOSSEUM_WAIT_DLG, false);
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	public override void SetComponent()
	{
		this.m_lbRemainTime = (base.GetControl("Label_Time") as Label);
		this.m_dtLoading = (base.GetControl("DT_Loading") as DrawTexture);
		this._SetDialogPos();
		this.Hide();
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetScreenCenter();
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void SetRemainTime(float fRemainTime)
	{
		if (fRemainTime > 0f)
		{
			this.m_fEndtime = Time.realtimeSinceStartup + fRemainTime;
			this.Show();
			this._SetDialogPos();
		}
	}

	public override void Update()
	{
		base.Update();
		if (this.m_fEndtime != 0f)
		{
			this.m_dtLoading.Rotate(5f);
			int num = (int)(this.m_fEndtime - Time.realtimeSinceStartup);
			if (this.m_nBeforeTime == num)
			{
				return;
			}
			if (num < 0)
			{
				this.Close();
				return;
			}
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2378"),
				"count",
				num.ToString()
			});
			this.m_nBeforeTime = num;
			this.m_lbRemainTime.SetText(empty);
		}
	}
}
