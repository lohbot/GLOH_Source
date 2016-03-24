using PROTOCOL.GAME;
using StageHelper;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle_ResultDlg : Form
{
	private DrawTexture m_dtTotalBG;

	private DrawTexture m_dtWinLose;

	private DrawTexture m_dtTopBG;

	private float m_OpenTime;

	private float m_ScreenWidth;

	private float m_ScreenHeight;

	private Battle_ResultDlg_Content m_ChildDlg;

	private bool m_CloseDlg = true;

	private float m_fResultFxTime;

	private GameObject m_goResultFX;

	private bool m_bClearMiddleStage;

	private bool m_bUpdate;

	private eBATTLE_ROOMTYPE m_eRoomType;

	public float ResultFxTime
	{
		get
		{
			return this.m_fResultFxTime;
		}
		set
		{
			this.m_fResultFxTime = value;
		}
	}

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
		Main_UI_SystemMessage.CloseUI();
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		form.Scale = false;
		instance.LoadFileAll(ref form, "Battle/RESULT/DLG_Battle_Result", G_ID.BATTLE_RESULT_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.ChangeSceneDestory = false;
		base.Draggable = false;
		this.Show();
		base.DonotDepthChange(NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 4f);
		this.m_ChildDlg = (base.SetChildForm(G_ID.BATTLE_RESULT_CONTENT_DLG, Form.ChildLocation.CENTER) as Battle_ResultDlg_Content);
		this.m_ChildDlg.Hide();
		this.m_eRoomType = Battle.BATTLE.BattleRoomtype;
	}

	public override void OnClose()
	{
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
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
			if (Time.realtimeSinceStartup > this.m_fResultFxTime)
			{
				this.m_ChildDlg.Show();
				this.m_dtWinLose.Visible = true;
				UnityEngine.Object.Destroy(this.m_goResultFX);
				if (this.m_bUpdate)
				{
					this.m_fResultFxTime = 0f;
					if (!this.m_bClearMiddleStage)
					{
						NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
						this.m_bClearMiddleStage = true;
					}
					this.m_OpenTime = Time.realtimeSinceStartup;
				}
			}
		}
		else if (CommonTasks.IsEndOfPrework)
		{
			if (this.m_eRoomType == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER)
			{
				return;
			}
			if (Time.realtimeSinceStartup >= this.m_OpenTime + 20f && this.m_CloseDlg)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
				this.Close();
			}
		}
	}

	public void _SetComponetBasic()
	{
		this.m_dtTotalBG = (base.GetControl("DrawTexture_TotalBG") as DrawTexture);
		this.m_dtTopBG = (base.GetControl("DrawTexture_DrawTexture3") as DrawTexture);
		this.m_dtWinLose = (base.GetControl("DrawTexture_DrawTexture1_C") as DrawTexture);
		this.m_dtWinLose.Visible = false;
		string path = string.Format("{0}Texture/Loading/0{1}", NrTSingleton<UIDataManager>.Instance.FilePath, NrTSingleton<UIDataManager>.Instance.AddFilePath);
		Texture2D texture = (Texture2D)CResources.Load(path);
		this.m_dtTotalBG.SetTexture(texture);
	}

	public void SetBG(WWWItem _item, object _param)
	{
		if (this == null)
		{
			return;
		}
		if (_item.isCanceled)
		{
			return;
		}
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				this.m_dtTotalBG.SetTexture(texture2D);
			}
		}
	}

	public void ResizeDlg()
	{
		base.SetLocation(0f, 0f);
		this.m_ScreenWidth = GUICamera.width;
		this.m_ScreenHeight = GUICamera.height;
		this.m_dtTotalBG.SetSize(this.m_ScreenWidth, this.m_ScreenHeight);
		if (this.m_dtTopBG != null)
		{
			this.m_dtTopBG.SetSize(this.m_ScreenWidth, this.m_dtTopBG.height);
		}
		this.m_dtWinLose.SetLocation(GUICamera.width / 2f - 262f, 0f);
		this.m_dtWinLose.SetSize(525f, 144f);
	}

	public void AddSolData(GS_BATTLE_RESULT_SOLDIER solinfo)
	{
		this.m_ChildDlg.AddSolData(solinfo);
	}

	public void AddItemData(GS_BATTLE_RESULT_ITEM iteminfo)
	{
		this.m_ChildDlg.AddItemData(iteminfo);
	}

	public void SetBasicData(GS_BATTLE_RESULT_NFY info)
	{
		this.m_ChildDlg.SetBasicData(info);
	}

	public void ClearSolData()
	{
		this.m_ChildDlg.ClearSolData();
	}

	public void ClearItemData()
	{
		this.m_ChildDlg.ClearItemData();
	}

	public void LinkData(int BattleSRewardUnique)
	{
		if (BattleSRewardUnique > 0)
		{
			this.m_CloseDlg = false;
			this.m_dtWinLose.Visible = false;
			this.m_ChildDlg.ShowSRewardDlg(BattleSRewardUnique);
		}
		else if (BattleSRewardUnique < 0)
		{
			if (!this.m_CloseDlg)
			{
				this.m_OpenTime = Time.realtimeSinceStartup;
				this.m_dtWinLose.Visible = true;
			}
			this.m_ChildDlg._LinkBasicData();
			this.m_ChildDlg.LinkData();
			if (this.m_ChildDlg.m_bWin)
			{
				NrSound.ImmedatePlay("UI_SFX", "BATTLE", "WIN", true);
				this.m_dtWinLose.SetTexture("Bat_I_ResultWin");
			}
			else
			{
				NrSound.ImmedatePlay("UI_SFX", "BATTLE", "LOSE", true);
				this.m_dtWinLose.SetTexture("Bat_I_ResultLose");
			}
			if (!this.m_bClearMiddleStage && !this.m_ChildDlg.RankEffectSet)
			{
				NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
				this.m_bClearMiddleStage = true;
			}
		}
		else
		{
			this.m_CloseDlg = true;
			this.m_ChildDlg._LinkBasicData();
			this.m_ChildDlg.LinkData();
			if (this.m_ChildDlg.m_bWin)
			{
				this.m_dtWinLose.SetTexture("Bat_I_ResultWin");
			}
			else
			{
				this.m_dtWinLose.SetTexture("Bat_I_ResultLose");
			}
			this.ShowResultFx();
		}
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
						NrSound.ImmedatePlay("UI_SFX", "BATTLE", "WIN", true);
					}
					else
					{
						clip = component.GetClip("defeat");
						component.Play("defeat");
						NrSound.ImmedatePlay("UI_SFX", "BATTLE", "LOSE", true);
					}
					if (clip != null)
					{
						this.m_fResultFxTime = Time.realtimeSinceStartup + clip.length;
					}
					else
					{
						this.m_fResultFxTime = Time.realtimeSinceStartup - 1.5f;
					}
				}
			}
			this.m_OpenTime = 0f;
		}
		else
		{
			this.m_dtWinLose.Visible = true;
			this.m_OpenTime = Time.realtimeSinceStartup;
			this.m_fResultFxTime = 0f;
			UnityEngine.Object.Destroy(this.m_goResultFX);
			if (!this.m_bClearMiddleStage)
			{
				NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
				this.m_bClearMiddleStage = true;
			}
		}
	}
}
