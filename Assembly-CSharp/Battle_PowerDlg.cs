using System;
using UnityEngine;
using UnityForms;

public class Battle_PowerDlg : Form
{
	private Label m_lbPower;

	private TsWeakReference<GameObject> m_goTarget;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_Power", G_ID.BATTLE_POWER_GROUP_DLG, false);
		base.DonotDepthChange(1010f);
		this.Hide();
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_lbPower = (base.GetControl("Label_Power") as Label);
	}

	public override void Update()
	{
		base.Update();
		this.UpdatePosition();
	}

	public void Set(GameObject pkTarget, long nFightPower)
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP)
		{
			this.Close();
			return;
		}
		if (pkTarget == null)
		{
			this.Close();
			return;
		}
		if (nFightPower == 0L)
		{
			this.Close();
			return;
		}
		this.m_goTarget = pkTarget;
		this.m_lbPower.SetText(nFightPower.ToString());
		this.UpdatePosition();
		this.Show();
	}

	public void UpdatePosition()
	{
		if (this.m_goTarget == null)
		{
			this.Close();
			return;
		}
		if (this.m_goTarget.CastedTarget == null)
		{
			this.Close();
			return;
		}
		Vector3 pos = Vector3.zero;
		pos = this.m_goTarget.CastedTarget.transform.position;
		pos = GUICamera.WorldToEZ(pos);
		pos.x -= base.GetSizeX() / 2f;
		base.SetLocation(pos.x, pos.y);
	}
}
