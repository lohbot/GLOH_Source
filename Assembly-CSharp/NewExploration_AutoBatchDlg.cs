using System;
using UnityForms;

public class NewExploration_AutoBatchDlg : Form
{
	private Button m_btAutoBatch;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Battle/DLG_AutoBatch", G_ID.PLUNDER_AUTOBATCH_DLG, false, true);
	}

	public override void SetComponent()
	{
		this.m_btAutoBatch = (base.GetControl("BT_AutoBatch") as Button);
		Button expr_1C = this.m_btAutoBatch;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickAutoBatch));
		float num = GUICamera.width;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
		{
			BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
			if (babelLobbyUserListDlg != null)
			{
				num = babelLobbyUserListDlg.GetLocationX();
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID)
		{
			MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
			if (mythRaidLobbyUserListDlg != null)
			{
				num = mythRaidLobbyUserListDlg.GetLocationX();
			}
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP)
		{
			BabelTowerGuildBossLobbyDlg babelTowerGuildBossLobbyDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABEL_GUILDBOSS_LOBBY_DLG) as BabelTowerGuildBossLobbyDlg;
			if (babelTowerGuildBossLobbyDlg != null)
			{
				num = babelTowerGuildBossLobbyDlg.GetLocationX();
			}
		}
		base.SetLocation(num - base.GetSizeX(), GUICamera.height - base.GetSizeY(), base.GetLocation().z);
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
		if (plunderSolListDlg != null)
		{
			base.SetLocation(plunderSolListDlg.GetLocationX() + plunderSolListDlg.GetSizeX(), 0f, base.GetLocation().z);
		}
	}

	public void OnClickAutoBatch(IUIObject obj)
	{
		SoldierBatch_AutoBatchTool.AutoBatch();
	}
}
