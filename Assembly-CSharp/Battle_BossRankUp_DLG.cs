using System;
using UnityEngine;
using UnityForms;

public class Battle_BossRankUp_DLG : Form
{
	private DrawTexture m_dtRankUp;

	private Label m_lbRank;

	private float m_fTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/dlg_battle_boss_rankup", G_ID.BATTLE_BOSS_RANKUP_DLG, false);
		form.AlwaysUpdate = true;
		form.TopMost = true;
	}

	public override void SetComponent()
	{
		this.m_dtRankUp = (base.GetControl("DrawTexture_Rankup") as DrawTexture);
		this.m_lbRank = (base.GetControl("Label_Rankup") as Label);
		NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_RANKUP_BOSS", this.m_dtRankUp, this.m_dtRankUp.GetSize());
		this.m_dtRankUp.AddGameObjectDelegate(new EZGameObjectDelegate(this.RankUpEffectDelegate));
		this._SetDialogPos();
		this.Hide();
	}

	public override void Update()
	{
		base.Update();
		if (this.m_fTime != 0f && this.m_fTime < Time.realtimeSinceStartup)
		{
			this.Close();
		}
	}

	public void _SetDialogPos()
	{
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_COUNT_DLG);
		if (form != null)
		{
			float y = form.GetSizeY() + (base.GetSize().y + 3f);
			base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, y);
		}
		else
		{
			float y2 = base.GetSize().y + 3f;
			base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, y2);
		}
	}

	public void SetData(int nBeforeRank, int nCurrentRank)
	{
		if ((nBeforeRank != nCurrentRank && nCurrentRank < nBeforeRank) || nBeforeRank == 9999)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2126"),
				"count",
				(nCurrentRank + 1).ToString()
			});
			this.m_lbRank.SetText(empty);
			this.m_fTime = Time.realtimeSinceStartup + 2f;
			this.Show();
			this._SetDialogPos();
		}
		else
		{
			this.Close();
		}
	}

	public void RankUpEffectDelegate(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		if (this.m_lbRank != null)
		{
			this.m_lbRank.SetLocationZ(obj.transform.localPosition.z - 0.1f);
		}
	}
}
