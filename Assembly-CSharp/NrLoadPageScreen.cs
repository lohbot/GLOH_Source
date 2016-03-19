using Ndoors.Framework.Stage;
using System;
using UnityEngine;
using UnityForms;

public static class NrLoadPageScreen
{
	public enum eLoadingPageType
	{
		NONE,
		BASIC,
		ENTER_DRAMA,
		QUIT_DRAMA,
		ENTER_BATTLE,
		QUIT_BATTLE,
		FIRST_LOADING
	}

	public static bool CustomLoadingProgress;

	public static bool LoginLatestChar;

	private static int _mainStepCnt;

	private static int _mainStepMax;

	private static float _mainStepRatio;

	private static float _mainStepAccum;

	private static float _curLoadCnt;

	private static float _maxLoadCnt;

	private static bool _IsCaptureLock;

	public static float LogicProgress
	{
		get;
		private set;
	}

	static NrLoadPageScreen()
	{
		NrLoadPageScreen.ResetMainStep_I();
	}

	public static void SetSkipMainStep(int skipCntMainStep)
	{
		TsLog.Assert(0 <= skipCntMainStep && skipCntMainStep < 5, "Invalid Progress Skip Count", new object[0]);
		NrLoadPageScreen._mainStepMax = skipCntMainStep;
		NrLoadPageScreen._mainStepCnt = 0;
		if (skipCntMainStep == 0)
		{
			NrLoadPageScreen._mainStepRatio = 1f;
		}
		else
		{
			NrLoadPageScreen._mainStepRatio = 1f / (float)NrLoadPageScreen._mainStepMax;
		}
		NrLoadPageScreen.ResetProgress(1f);
	}

	private static void ResetCurCounter_I()
	{
		NrLoadPageScreen._curLoadCnt = 0f;
		NrLoadPageScreen._maxLoadCnt = 0f;
	}

	private static void ResetMainStep_I()
	{
		NrLoadPageScreen._mainStepCnt = 0;
		NrLoadPageScreen._mainStepMax = 1;
		NrLoadPageScreen._mainStepRatio = 1f;
		NrLoadPageScreen._mainStepAccum = 0f;
	}

	public static void StepUpMain(int skipStageCnt)
	{
		if (NrLoadPageScreen._mainStepMax < skipStageCnt)
		{
			NrLoadPageScreen.SetSkipMainStep(skipStageCnt);
		}
		NrLoadPageScreen._mainStepCnt++;
		if (NrLoadPageScreen._mainStepCnt > NrLoadPageScreen._mainStepMax)
		{
			NrLoadPageScreen.SetSkipMainStep(skipStageCnt);
			NrLoadPageScreen._mainStepCnt++;
		}
		NrLoadPageScreen._mainStepAccum = (float)(NrLoadPageScreen._mainStepCnt - 1) * NrLoadPageScreen._mainStepRatio;
		NrLoadPageScreen.ResetCurCounter_I();
	}

	public static void AddLoadingItemCnt(int cnt)
	{
		NrLoadPageScreen._maxLoadCnt += (float)cnt;
	}

	public static void IncreaseProgress(float cnt)
	{
		if (cnt <= 0f || NrLoadPageScreen._maxLoadCnt <= 0f)
		{
			return;
		}
		NrLoadPageScreen._curLoadCnt += cnt;
		if (NrLoadPageScreen._curLoadCnt > NrLoadPageScreen._maxLoadCnt)
		{
			NrLoadPageScreen._maxLoadCnt = NrLoadPageScreen._curLoadCnt;
		}
		float progressValue = NrLoadPageScreen._mainStepAccum + NrLoadPageScreen._curLoadCnt / NrLoadPageScreen._maxLoadCnt * NrLoadPageScreen._mainStepRatio;
		if (!NrLoadPageScreen.CustomLoadingProgress)
		{
			NrLoadPageScreen.SetProgressValue(progressValue);
		}
	}

	public static void SetLogicProgress(float lprog)
	{
		NrLoadPageScreen.LogicProgress = lprog;
	}

	public static void DecideLoadingType(Scene.Type preScene, Scene.Type curScene)
	{
		if (NrLoadPageScreen._IsCaptureLock)
		{
			return;
		}
		if (preScene == Scene.Type.CUTSCENE)
		{
			NrLoadPageScreen.SetLoadingType(NrLoadPageScreen.eLoadingPageType.QUIT_DRAMA);
		}
		else if (curScene == Scene.Type.CUTSCENE)
		{
			NrLoadPageScreen.SetLoadingType(NrLoadPageScreen.eLoadingPageType.ENTER_DRAMA);
		}
		else if (curScene == Scene.Type.LOGIN)
		{
			NrLoadPageScreen.SetLoadingType(NrLoadPageScreen.eLoadingPageType.FIRST_LOADING);
		}
		else if (curScene == Scene.Type.BATTLE)
		{
			NrLoadPageScreen.SetLoadingType(NrLoadPageScreen.eLoadingPageType.ENTER_BATTLE);
		}
		else
		{
			NrLoadPageScreen.SetLoadingType(NrLoadPageScreen.eLoadingPageType.BASIC);
		}
	}

	public static void Init()
	{
	}

	public static void ShowHideLoadingImg(bool bShow)
	{
		if (bShow == NrLoadPageScreen.IsShow())
		{
			return;
		}
		TsLog.Log("LSC === ShowHideLodingImg({0})", new object[]
		{
			bShow
		});
		if (bShow)
		{
			NrLoadPageScreen.SetLogicProgress(0f);
			NrLoadPageScreen.SetProgressValue(0f);
		}
		else
		{
			NrLoadPageScreen._IsCaptureLock = false;
		}
		NewLoaingDlg newLoaingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE) as NewLoaingDlg;
		if (newLoaingDlg != null)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log(string.Concat(new object[]
				{
					"Profile Frame[",
					Time.frameCount,
					":",
					Time.fixedTime,
					"] ShowHideLodingImg :",
					bShow
				}), new object[0]);
			}
			newLoaingDlg.SetShowHide(bShow);
		}
		StageWorld.s_bIsShowRegionName = !bShow;
	}

	public static void SetLoadingType(NrLoadPageScreen.eLoadingPageType type)
	{
	}

	private static void ResetProgress(float fMax)
	{
		TsLog.LogWarning("--- NrLoadPageScreen.ResetProgress(Max={0})", new object[]
		{
			fMax
		});
		NewLoaingDlg newLoaingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE) as NewLoaingDlg;
		if (newLoaingDlg != null)
		{
			newLoaingDlg.ResetProgress(fMax);
		}
	}

	public static void AddProgressValue(float fValue)
	{
		NewLoaingDlg newLoaingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE) as NewLoaingDlg;
		if (newLoaingDlg != null)
		{
			newLoaingDlg.AddProgressValue(fValue);
		}
	}

	public static void SetProgressValue(float fValue)
	{
		NewLoaingDlg newLoaingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE) as NewLoaingDlg;
		if (newLoaingDlg != null)
		{
			newLoaingDlg.SetProgressValue(fValue);
		}
	}

	public static bool IsShow()
	{
		NewLoaingDlg newLoaingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE) as NewLoaingDlg;
		return newLoaingDlg != null && newLoaingDlg.Visible;
	}
}
