using System;
using UnityEngine;
using UnityForms;

public class GooglePlayDlg : Form
{
	private DrawTexture m_dwBG;

	private Button m_bGoogleGame;

	private Button m_bAchievement;

	private Button m_bLeaderboard;

	private DrawTexture m_dwIOSBG;

	private Button m_bIOSGoogleGame;

	private Button m_bIOSAchievement;

	private Button m_bIOSLeaderboard;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "GooglePlay/dlg_googleplaygames", G_ID.GOOGLEPLAY_DLG, true);
		base.ChangeSceneDestory = false;
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.draggable = false;
		}
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.DonotDepthChange(UIPanelManager.UI_DEPTH);
	}

	public override void SetComponent()
	{
		base.SetLocation(0f, 80f);
		this.m_dwBG = (base.GetControl("DrawTexture_bg") as DrawTexture);
		this.m_dwBG.Visible = false;
		this.m_bGoogleGame = (base.GetControl("Button_GoogleGames") as Button);
		Button expr_4E = this.m_bGoogleGame;
		expr_4E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_4E.Click, new EZValueChangedDelegate(this.OnGameMenu));
		this.m_bGoogleGame.Visible = false;
		this.m_bAchievement = (base.GetControl("Button_Achievements") as Button);
		Button expr_97 = this.m_bAchievement;
		expr_97.Click = (EZValueChangedDelegate)Delegate.Combine(expr_97.Click, new EZValueChangedDelegate(this.OnAchievement));
		this.m_bAchievement.Visible = false;
		this.m_bLeaderboard = (base.GetControl("Button_Leaderboard") as Button);
		Button expr_E0 = this.m_bLeaderboard;
		expr_E0.Click = (EZValueChangedDelegate)Delegate.Combine(expr_E0.Click, new EZValueChangedDelegate(this.OnLeaderboard));
		this.m_bLeaderboard.Visible = false;
		this.m_dwIOSBG = (base.GetControl("DrawTexture_IOSbg") as DrawTexture);
		this.m_dwIOSBG.Visible = false;
		this.m_bIOSGoogleGame = (base.GetControl("Button_IOSGames") as Button);
		Button expr_14B = this.m_bIOSGoogleGame;
		expr_14B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_14B.Click, new EZValueChangedDelegate(this.OnIOSGameMenu));
		this.m_bIOSGoogleGame.Visible = false;
		this.m_bIOSAchievement = (base.GetControl("Button_IOSAchievements") as Button);
		Button expr_194 = this.m_bIOSAchievement;
		expr_194.Click = (EZValueChangedDelegate)Delegate.Combine(expr_194.Click, new EZValueChangedDelegate(this.OnIOSAchievement));
		this.m_bIOSAchievement.Visible = false;
		this.m_bIOSLeaderboard = (base.GetControl("Button_IOSLeaderboard") as Button);
		Button expr_1DD = this.m_bIOSLeaderboard;
		expr_1DD.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1DD.Click, new EZValueChangedDelegate(this.OnIOSLeaderboard));
		this.m_bIOSLeaderboard.Visible = false;
		if (TsPlatform.IsAndroid)
		{
			this.m_bGoogleGame.Visible = true;
		}
		else
		{
			this.m_bIOSGoogleGame.Visible = true;
		}
	}

	public override void Update()
	{
		if (TsPlatform.IsAndroid)
		{
			this.InputMouseEvent();
		}
	}

	public void InputMouseEvent()
	{
		bool flag = false;
		if (TsPlatform.IsEditor)
		{
			if (NkInputManager.GetMouseButtonUp(0) || NkInputManager.GetMouseButtonUp(1))
			{
				flag = true;
				G_ID g_ID = (G_ID)NrTSingleton<FormsManager>.Instance.MouseOverFormID();
				if (NrTSingleton<FormsManager>.Instance.IsMouseOverForm() && g_ID == G_ID.GOOGLEPLAY_DLG)
				{
					flag = false;
				}
			}
		}
		else if (TsPlatform.IsMobile && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			flag = true;
			G_ID g_ID2 = (G_ID)NrTSingleton<FormsManager>.Instance.MouseOverFormID();
			if (NrTSingleton<FormsManager>.Instance.IsMouseOverForm() && g_ID2 == G_ID.GOOGLEPLAY_DLG)
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.HideMenu(flag);
		}
	}

	private void HideMenu(bool bShow)
	{
		if (bShow)
		{
			this.m_bAchievement.Visible = false;
			this.m_bLeaderboard.Visible = false;
			this.m_dwBG.Visible = false;
		}
		else
		{
			this.m_bAchievement.Visible = true;
			this.m_bLeaderboard.Visible = true;
			this.m_dwBG.Visible = true;
		}
	}

	private void OnGameMenu(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		this.HideMenu(this.m_bAchievement.Visible);
	}

	private void OnAchievement(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Social.ShowAchievementsUI();
	}

	private void OnLeaderboard(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Social.ShowLeaderboardUI();
	}

	private void OnIOSGameMenu(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Social.ShowAchievementsUI();
	}

	private void OnIOSAchievement(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Social.ShowAchievementsUI();
	}

	private void OnIOSLeaderboard(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Social.ShowLeaderboardUI();
	}
}
