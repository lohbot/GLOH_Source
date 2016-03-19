using Ndoors.Framework.Stage;
using StageHelper;
using System;
using System.IO;
using UnityEngine;
using UnityForms;

public class Battle_ResultTutorialDlg : Form
{
	private DrawTexture m_dtTotalBG;

	private DrawTexture m_dtWinLose;

	private DrawTexture m_dtTopBG;

	private float m_OpenTime;

	private float m_ScreenWidth;

	private float m_ScreenHeight;

	private bool m_CloseDlg = true;

	private bool m_bClearMiddleStage;

	private bool m_bUpdate;

	private bool m_bPlayMovie;

	private int BattleScenarioUnique;

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

	public bool PlayMovie
	{
		get
		{
			return this.m_bPlayMovie;
		}
		set
		{
			this.m_bPlayMovie = value;
		}
	}

	public override void InitializeComponent()
	{
		Main_UI_SystemMessage.CloseUI();
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		form.Scale = false;
		instance.LoadFileAll(ref form, "Battle/RESULT/DLG_Battle_Result_Tutorial", G_ID.BATTLE_RESULT_TUTORIAL_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.ChangeSceneDestory = false;
		base.Draggable = false;
		this.Show();
		base.DonotDepthChange(NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 4f);
	}

	public override void OnClose()
	{
		NrTSingleton<ChallengeManager>.Instance.ShowNotice();
		if (NrTSingleton<ContentsLimitManager>.Instance.IsTutorialBattleStart() && this.BattleScenarioUnique == 99999999)
		{
			NrTSingleton<EventConditionHandler>.Instance.MapIn.OnTrigger();
			NrTSingleton<EventConditionHandler>.Instance.SceneChange.Value.Set(Scene.Type.PREPAREGAME, Scene.Type.WORLD);
			NrTSingleton<EventConditionHandler>.Instance.SceneChange.OnTrigger();
		}
	}

	public override void SetComponent()
	{
		this._SetComponetBasic();
		this.ResizeDlg();
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
		}
		if (this.m_bUpdate && !this.m_bPlayMovie)
		{
			if (this.m_OpenTime == 0f)
			{
				if (!this.m_bClearMiddleStage)
				{
					NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
					this.m_bClearMiddleStage = true;
				}
				if (CommonTasks.IsEndOfPrework && NrTSingleton<NkCharManager>.Instance.IsSafeToWorld(true))
				{
					this.m_OpenTime = Time.realtimeSinceStartup;
				}
			}
			else if (Time.realtimeSinceStartup >= this.m_OpenTime + 1f && this.m_CloseDlg)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
				this.Close();
			}
			else
			{
				float alpha = 1f - (Time.realtimeSinceStartup - this.m_OpenTime);
				base.SetAlpha(alpha);
			}
		}
	}

	public void _SetComponetBasic()
	{
		this.m_dtTotalBG = (base.GetControl("DrawTexture_TotalBG") as DrawTexture);
		this.m_dtTopBG = (base.GetControl("DrawTexture_DrawTexture3") as DrawTexture);
		this.m_dtWinLose = (base.GetControl("DrawTexture_DrawTexture1_C") as DrawTexture);
		this.m_dtWinLose.Visible = false;
		this.m_dtTotalBG.Visible = false;
		this.m_dtTopBG.Visible = false;
		string path = string.Format("{0}Texture/Loading/0{1}", NrTSingleton<UIDataManager>.Instance.FilePath, NrTSingleton<UIDataManager>.Instance.AddFilePath);
		Texture2D texture = (Texture2D)CResources.Load(path);
		this.m_dtTotalBG.SetTexture(texture);
		this.BattleScenarioUnique = Battle.BATTLE.GetScenarioUnique();
		base.ShowBlackBG(1f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
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

	public void ShowResultFx()
	{
		this.m_dtWinLose.Visible = false;
		this.m_OpenTime = 0f;
		if (!this.m_bPlayMovie)
		{
			string str = string.Format("{0}/", NrTSingleton<NrGlobalReference>.Instance.basePath);
			if (File.Exists(str + "LOHBI.mp4"))
			{
				this.m_bPlayMovie = false;
				Debug.LogError("TUTORIAL MOVIE NOT EXIST");
				return;
			}
			TsPlatform.Operator.PlayMovieURL(str + "LOHBI.mp4", Color.black, false, 1f);
			this.m_bPlayMovie = true;
			this.Hide();
		}
	}
}
