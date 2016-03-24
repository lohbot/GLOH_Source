using Ndoors.Framework.Stage;
using StageHelper;
using System;
using System.IO;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle_ResultTutorialDlg : Form
{
	private DrawTexture m_dtTotalBG;

	private DrawTexture m_dtTopBG;

	private DrawTexture m_waitImg;

	private float m_OpenTime;

	private float m_MovieTime;

	private float m_ScreenWidth;

	private float m_ScreenHeight;

	private bool m_bClearMiddleStage;

	private bool m_bUpdate;

	private bool m_bPlayMovie;

	private int BattleScenarioUnique;

	public bool m_bUpdateCheck;

	public float OpenTime
	{
		get
		{
			return this.m_OpenTime;
		}
		set
		{
			this.m_OpenTime = value;
		}
	}

	public float MovieTime
	{
		get
		{
			return this.m_MovieTime;
		}
		set
		{
			this.m_MovieTime = value;
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
			Debug.LogError("=========== CLOSE TUTORIAL BATTLE DLG ======================");
		}
	}

	public override void SetComponent()
	{
		this._SetComponetBasic();
		this.ResizeDlg();
		this.Show();
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
		if (this.m_MovieTime != 0f && Time.realtimeSinceStartup - this.m_MovieTime > 40f)
		{
			this.m_bUpdateCheck = true;
			this.m_OpenTime = 0f;
			this.m_bPlayMovie = false;
			this.m_MovieTime = 0f;
			Debug.LogError("Movie PlayTime Error");
		}
		if (!this.m_bUpdateCheck)
		{
			return;
		}
		if (this.m_bUpdate && !this.m_bPlayMovie)
		{
			if (!this.m_waitImg.Visible)
			{
				this.m_waitImg.Visible = true;
			}
			this.m_waitImg.Rotate(5f);
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
		}
	}

	public void _SetComponetBasic()
	{
		this.m_dtTotalBG = (base.GetControl("DrawTexture_TotalBG") as DrawTexture);
		this.m_dtTopBG = (base.GetControl("DrawTexture_DrawTexture3") as DrawTexture);
		this.m_waitImg = (base.GetControl("DrawTexture_Loading") as DrawTexture);
		this.m_dtTotalBG.Visible = false;
		this.m_dtTopBG.Visible = false;
		this.m_waitImg.Visible = false;
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
		this.m_waitImg.SetLocation((this.m_ScreenWidth - this.m_waitImg.GetSize().x) / 2f, (this.m_ScreenHeight - this.m_waitImg.GetSize().y) / 2f);
	}

	public void ShowResultFx()
	{
		this.m_OpenTime = 0f;
		if (!this.m_bPlayMovie)
		{
			this.m_waitImg.Visible = false;
			string text = "UI/Drama/TUTORIAL.mp4";
			text = text.ToLower();
			string path = string.Format("{0}/{1}", NrTSingleton<NrGlobalReference>.Instance.basePath, text);
			if (File.Exists(path))
			{
				this.m_bPlayMovie = false;
				this.m_bUpdateCheck = true;
				Debug.LogError("TUTORIAL MOVIE NOT EXIST");
				return;
			}
			NmMainFrameWork.PlayMovieURL(path, true, false, true);
			this.m_bPlayMovie = true;
			this.MovieTime = Time.realtimeSinceStartup;
			this.m_bUpdateCheck = false;
		}
	}
}
