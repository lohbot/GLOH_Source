using GAME;
using GameMessage.Private;
using Ndoors.Framework.Stage;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class KeyboardShotCutCommandLayer : InputCommandLayer
{
	public KeyboardShotCutCommandLayer()
	{
		base.AddKeyInputDelegate(new KeyInputDelegate(this.CloseForm));
		base.AddKeyInputDelegate(new KeyInputDelegate(KeyboardShotCutCommandLayer.ShowSolMilityDlg));
		base.AddKeyInputDelegate(new KeyInputDelegate(this.ShowPostDlg));
		base.AddKeyInputDelegate(new KeyInputDelegate(this.ShowGuildDlg));
		base.AddKeyInputDelegate(new KeyInputDelegate(this.ShowNearNpcSelectUIDlg));
		base.AddKeyInputDelegate(new KeyInputDelegate(this.ShowQuestListDlg));
		base.AddKeyInputDelegate(new KeyInputDelegate(this.ShowInvenDlg));
		base.AddKeyInputDelegate(new KeyInputDelegate(this.ShowCommunityDlg));
		base.AddKeyInputDelegate(new KeyInputDelegate(this.ShowSystemOptionDlg));
		base.AddKeyInputDelegate(new KeyInputDelegate(this.ShowWorldMapDlg));
		base.AddKeyInputDelegate(new KeyInputDelegate(this.HideAllUI));
		base.AddKeyInputDelegate(new KeyInputDelegate(this.Other));
	}

	public override bool PreCheckUpdate()
	{
		return Scene.CurScene != Scene.Type.SELECTCHAR && Scene.CurScene != Scene.Type.LOGIN && Scene.CurScene != Scene.Type.CUTSCENE && Scene.CurScene != Scene.Type.EMPTY && !NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState();
	}

	public void CloseForm()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			NrTSingleton<FormsManager>.Instance.CloseFormESC();
		}
	}

	public static void ShowSolMilityDlg()
	{
		if (NkInputManager.GetKeyUp(KeyCode.Space))
		{
			Scene.Type curScene = Scene.CurScene;
			if (curScene == Scene.Type.EMPTY || curScene == Scene.Type.SELECTCHAR || curScene == Scene.Type.CUTSCENE)
			{
				return;
			}
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg == null || !solMilitaryGroupDlg.Visible)
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.SOLMILITARYGROUP_DLG);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.ClearShowHideForms();
			}
			SolMilitarySelectDlg solMilitarySelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYSELECT_DLG) as SolMilitarySelectDlg;
			if (solMilitarySelectDlg != null)
			{
				solMilitarySelectDlg.CloseByParent(79);
			}
		}
	}

	public void ShowPostDlg()
	{
		if (NkInputManager.GetKeyUp(KeyCode.P))
		{
			if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) != null)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.POST_DLG);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POST_DLG);
			}
		}
	}

	public void ShowGuildDlg()
	{
		if (NkInputManager.GetKeyUp(KeyCode.G) && 0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID())
		{
			if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_MAIN_DLG) != null)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_MAIN_DLG);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_MAIN_DLG);
			}
		}
	}

	public void ShowNearNpcSelectUIDlg()
	{
		if (NkInputManager.GetKeyUp(KeyCode.F))
		{
			NearNpcSelectUI_DLG nearNpcSelectUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEARNPCSELECTUI_DLG) as NearNpcSelectUI_DLG;
			if (nearNpcSelectUI_DLG != null)
			{
				nearNpcSelectUI_DLG.NpcClick(null);
			}
		}
	}

	public void ShowQuestListDlg()
	{
		if (!TsPlatform.IsEditor)
		{
			return;
		}
		if (NkInputManager.GetKeyUp(KeyCode.L))
		{
			QuestList_DLG questList_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.QUESTLIST_DLG) as QuestList_DLG;
			if (questList_DLG != null)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.QUESTLIST_DLG);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.QUESTLIST_DLG);
			}
		}
	}

	public void ShowInvenDlg()
	{
		if (NkInputManager.GetKeyUp(KeyCode.I) && !Scene.IsCurScene(Scene.Type.BATTLE))
		{
			NrTSingleton<FormsManager>.Instance.ShowHide(G_ID.INVENTORY_DLG);
		}
	}

	public void ShowCommunityDlg()
	{
		if (NkInputManager.GetKeyUp(KeyCode.U))
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COMMUNITY_DLG))
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COMMUNITY_DLG);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COMMUNITY_DLG);
			}
		}
	}

	public void ShowSystemOptionDlg()
	{
		if (NkInputManager.GetKeyUp(KeyCode.O))
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SYSTEM_OPTION))
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.SYSTEM_OPTION);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SYSTEM_OPTION);
			}
		}
	}

	public void ShowWorldMapDlg()
	{
		if (NkInputManager.GetKeyUp(KeyCode.M))
		{
			if (Scene.CurScene == Scene.Type.BATTLE)
			{
				return;
			}
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.WORLD_MAP))
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WORLD_MAP);
				NrTSingleton<FormsManager>.Instance.Show(G_ID.WORLD_MAP);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MAP", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WORLD_MAP);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MAP", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
		}
	}

	public void HideAllUI()
	{
		if (NkInputManager.GetKeyUp(KeyCode.F10))
		{
			NrTSingleton<FormsManager>.Instance.ShowHideAll();
		}
	}

	public void Other()
	{
		if (NkInputManager.GetKeyUp(KeyCode.F7))
		{
			Agit_GoldenEggDramaDlg agit_GoldenEggDramaDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AGIT_GOLDENEGGDRAMA_DLG) as Agit_GoldenEggDramaDlg;
			ITEM item = NkUserInventory.GetInstance().GetItem(50305);
			agit_GoldenEggDramaDlg.SetItem(item);
			agit_GoldenEggDramaDlg.ShowWhiteEgg();
		}
		if (NkInputManager.GetKeyUp(KeyCode.Z) && NkInputManager.GetKey(KeyCode.LeftShift))
		{
			GameObject gameObject = GameObject.Find("back light");
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		if (NkInputManager.GetKeyUp(KeyCode.Q) && NkInputManager.GetKey(KeyCode.LeftShift))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.QUEST_GM_DLG);
		}
		if (NkInputManager.GetKeyUp(KeyCode.C) && NkInputManager.GetKey(KeyCode.LeftShift))
		{
			Screen.fullScreen = !Screen.fullScreen;
		}
		if (NkInputManager.GetKeyUp(KeyCode.C) && NkInputManager.GetKey(KeyCode.RightShift))
		{
			NkWorldMapATB worldMapATB = NrTSingleton<MapManager>.Instance.GetWorldMapATB();
			if (worldMapATB != null)
			{
				worldMapATB.ShowCellGrid();
			}
		}
		if (NkInputManager.GetKeyUp(KeyCode.U) && NkInputManager.GetKey(KeyCode.RightShift))
		{
			GUICamera.ShowUI_Toggle();
		}
		if (NkInputManager.GetKeyUp(KeyCode.D) && NkInputManager.GetKey(KeyCode.LeftShift))
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MESSAGE_DLG))
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.MESSAGE_DLG);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.Hide(G_ID.MESSAGE_DLG);
			}
		}
		if (NkInputManager.GetKey(KeyCode.LeftShift) && NkInputManager.GetKey(KeyCode.LeftControl) && NkInputManager.GetKeyUp(KeyCode.D))
		{
			NrTSingleton<MapManager>.Instance.ShowDestPosition(true);
		}
		if (NkInputManager.GetKey(KeyCode.LeftShift) && NkInputManager.GetKey(KeyCode.LeftAlt) && NkInputManager.GetKeyUp(KeyCode.D))
		{
			NrTSingleton<MapManager>.Instance.ShowDestPosition(false);
		}
		if (NkInputManager.GetKeyDown(KeyCode.Alpha1) && NkInputManager.GetKey(KeyCode.LeftShift) && Scene.CurScene != Scene.Type.BATTLE)
		{
			NrTSingleton<NkCharManager>.Instance.ToggleShowCharUnique();
			NrTSingleton<NkQuestManager>.Instance.ToggleQeustUnique();
		}
		if (NkInputManager.GetKeyDown(KeyCode.P) && NkInputManager.GetKey(KeyCode.LeftShift))
		{
			NrTSingleton<NrAutoPath>.Instance.ShowRPPoint();
		}
		if (NkInputManager.GetKeyDown(KeyCode.P) && NkInputManager.GetKey(KeyCode.RightShift))
		{
			NrTSingleton<NrAutoPath>.Instance.ClearRPPoint();
		}
		if (!NkInputManager.GetKey(KeyCode.LeftAlt) || NkInputManager.GetKeyUp(KeyCode.K))
		{
		}
		if (NkInputManager.GetKeyUp(KeyCode.S) && NkInputManager.GetKey(KeyCode.LeftShift))
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DLG_AUDIO))
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_AUDIO);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.Hide(G_ID.DLG_AUDIO);
			}
		}
		if (!NkInputManager.GetKeyUp(KeyCode.Alpha2) || NkInputManager.GetKey(KeyCode.LeftShift))
		{
		}
		if (NkInputManager.GetKeyUp(KeyCode.A) && NkInputManager.GetKey(KeyCode.LeftShift) && !Scene.IsCurScene(Scene.Type.SOLDIER_BATCH))
		{
			SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP;
			SoldierBatch.GUILDBOSS_INFO.m_i16Floor = 1;
			FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
		}
	}
}
