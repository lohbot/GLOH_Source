using PROTOCOL.GAME;
using StageHelper;
using System;
using UnityEngine;
using UnityForms;

public class Battle_ResultPlunderDlg : Form
{
	public enum eMODE
	{
		eMODE_PLUNDER,
		eMODE_INFIBATTLE,
		eMODE_MAX
	}

	private DrawTexture m_dtTotalBG;

	private Label m_lbResult;

	private Label m_lbBattleTime;

	private float m_OpenTime;

	private float m_ScreenWidth;

	private float m_ScreenHeight;

	private Battle_ResultPlunderDlg_Content m_ChildDlg;

	private bool m_CloseDlg = true;

	private float m_fResultFxTime;

	private GameObject m_goResultFX;

	private bool m_bClearMiddleStage;

	private bool m_bUpdate;

	private Battle_ResultPlunderDlg.eMODE m_eMode;

	public bool UpdateCheck
	{
		get
		{
			return this.m_bUpdate;
		}
		set
		{
			this.m_bUpdate = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		form.Scale = false;
		instance.LoadFileAll(ref form, "Battle/RESULT/DLG_Battle_Result_Plunder", G_ID.BATTLE_RESULT_PLUNDER_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.ChangeSceneDestory = false;
		base.Draggable = false;
		this.Show();
		base.DonotDepthChange(NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 4f);
		this.m_ChildDlg = (base.SetChildForm(G_ID.BATTLE_RESULT_PLUNDER_CONTENT_DLG, Form.ChildLocation.CENTER) as Battle_ResultPlunderDlg_Content);
		this.m_ChildDlg.Hide();
	}

	public override void SetComponent()
	{
		this._SetComponetBasic();
		this.ResizeDlg();
		base.SetShowLayer(5, false);
	}

	public override void InitData()
	{
		base.InitData();
	}

	public override void Update()
	{
		base.Update();
		if (this.m_ScreenWidth != GUICamera.width || this.m_ScreenHeight != GUICamera.height)
		{
			this.ResizeDlg();
			this.m_ChildDlg.ResizeDlg();
		}
		if (this.m_fResultFxTime != 0f)
		{
			if (Time.time > this.m_fResultFxTime)
			{
				this.m_ChildDlg.Show();
				this.m_lbResult.Visible = true;
				this.m_lbBattleTime.Visible = true;
				UnityEngine.Object.Destroy(this.m_goResultFX);
				if (this.m_bUpdate)
				{
					this.m_fResultFxTime = 0f;
					if (!this.m_bClearMiddleStage)
					{
						NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
						this.m_bClearMiddleStage = true;
					}
					this.m_OpenTime = Time.time;
				}
			}
		}
		else if (CommonTasks.IsEndOfPrework && Time.time >= this.m_OpenTime + 20f && this.m_CloseDlg)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
			this.Close();
		}
	}

	public void _SetComponetBasic()
	{
		this.m_dtTotalBG = (base.GetControl("DrawTexture_TotalBG") as DrawTexture);
		this.m_lbResult = (base.GetControl("Label_Result") as Label);
		this.m_lbBattleTime = (base.GetControl("Label_BattleTime") as Label);
		this.m_lbResult.SetCharacterSize(45f);
		this.m_lbResult.Visible = false;
		this.m_lbBattleTime.Visible = false;
		string path = string.Format("{0}Texture/Loading/0{1}", NrTSingleton<UIDataManager>.Instance.FilePath, NrTSingleton<UIDataManager>.Instance.AddFilePath);
		Texture2D texture = (Texture2D)CResources.Load(path);
		this.m_dtTotalBG.SetTexture(texture);
	}

	public void ResizeDlg()
	{
		base.SetLocation(0f, 0f);
		this.m_ScreenWidth = GUICamera.width;
		this.m_ScreenHeight = GUICamera.height;
		this.m_dtTotalBG.SetSize(this.m_ScreenWidth, this.m_ScreenHeight);
		this.m_lbResult.SetLocation(9, 13);
		this.m_lbResult.SetSize(774f, 76f);
		this.m_lbBattleTime.SetLocation(this.m_ScreenWidth - this.m_lbBattleTime.GetSize().x, 13f);
	}

	public void AddSolData(GS_BATTLE_RESULT_SOLDIER solinfo)
	{
		this.m_ChildDlg.AddSolData(solinfo);
	}

	public void SetBasicData(GS_BATTLE_RESULT_PLUNDER_NFY info)
	{
		this.m_ChildDlg.SetBasicData(info);
	}

	public void ClearSolData()
	{
		this.m_ChildDlg.ClearSolData();
	}

	public void LinkData()
	{
		if (this.m_eMode == Battle_ResultPlunderDlg.eMODE.eMODE_PLUNDER)
		{
			this.m_ChildDlg._LinkBasicData();
			this.m_ChildDlg.LinkData();
		}
		else
		{
			this.m_ChildDlg._LinkBasicDataInfiBattle();
			this.m_ChildDlg._LinkSolDataInfiBattle();
		}
		this.m_lbResult.SetText(this.m_ChildDlg.m_strWin);
		this.m_lbBattleTime.SetText(this.m_ChildDlg.m_strBattleTime);
		this.ShowResultFx();
	}

	private void ShowResultFx()
	{
		if (Battle.BATTLE.ResultEffect != null)
		{
			this.m_goResultFX = (GameObject)UnityEngine.Object.Instantiate(Battle.BATTLE.ResultEffect, Vector3.zero, Quaternion.identity);
			this.m_goResultFX.SetActive(true);
			NkUtil.SetAllChildLayer(this.m_goResultFX, GUICamera.UILayer);
			Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			this.m_goResultFX.transform.position = base.GetEffectUIPos(screenPos);
			this.m_goResultFX.SetActive(true);
			Transform child = NkUtil.GetChild(this.m_goResultFX.transform, "result");
			if (child != null)
			{
				Animation component = child.GetComponent<Animation>();
				if (component != null)
				{
					if (component.isPlaying)
					{
						component.Stop();
					}
					AnimationClip clip;
					if (this.m_ChildDlg.m_bWin)
					{
						clip = component.GetClip("victory");
						component.Play("victory");
					}
					else
					{
						clip = component.GetClip("defeat");
						component.Play("defeat");
					}
					if (clip != null)
					{
						this.m_fResultFxTime = Time.time + clip.length;
					}
					else
					{
						this.m_fResultFxTime = Time.time - 1.5f;
					}
				}
			}
			this.m_OpenTime = 0f;
		}
		else
		{
			this.m_ChildDlg.Show();
			this.m_lbResult.Visible = true;
			this.m_lbBattleTime.Visible = true;
			this.m_OpenTime = Time.time;
			this.m_fResultFxTime = 0f;
			UnityEngine.Object.Destroy(this.m_goResultFX);
			if (!this.m_bClearMiddleStage)
			{
				NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
				this.m_bClearMiddleStage = true;
			}
		}
	}

	public void SetMode(Battle_ResultPlunderDlg.eMODE eMode)
	{
		this.m_eMode = eMode;
		this.m_ChildDlg.SetMode((Battle_ResultPlunderDlg_Content.eMODE)eMode);
		this.ShowMode();
	}

	public void ShowMode()
	{
		Battle_ResultPlunderDlg.eMODE eMode = this.m_eMode;
		if (eMode != Battle_ResultPlunderDlg.eMODE.eMODE_PLUNDER)
		{
			if (eMode == Battle_ResultPlunderDlg.eMODE.eMODE_INFIBATTLE)
			{
				this.ShowInfiBattle();
			}
		}
		else
		{
			this.ShowPlunder();
		}
	}

	public void ShowPlunder()
	{
	}

	public void ShowInfiBattle()
	{
	}

	public void SetInfiBattleInfo(GS_INFIBATTLE_RESULT_ACK ACK)
	{
		this.m_ChildDlg.SetInfiBattleInfo(ACK);
	}
}
