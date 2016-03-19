using System;
using System.Collections;
using System.Diagnostics;
using TsBundle;

namespace StageHelper
{
	public static class SceneSwitch
	{
		public static string MapPathBattle
		{
			get;
			set;
		}

		public static string MapPathDungeon
		{
			get;
			set;
		}

		public static bool IsDifferentDungeonBattleMap
		{
			get
			{
				return SceneSwitch.MapPathBattle != SceneSwitch.MapPathDungeon;
			}
		}

		public static bool IsAnotherBattleMap
		{
			get;
			set;
		}

		public static bool IsShowDungeonName
		{
			get;
			set;
		}

		public static string LastLoadedField
		{
			get;
			private set;
		}

		public static void DeleteSceneExceptTerritory()
		{
			TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.WorldScene);
			TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.BattleScene);
			Holder.ClearStackItem(Option.IndependentFromStageStackName, true);
		}

		public static void DeleteFieldScene()
		{
			TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.WorldScene);
		}

		public static void DeleteBattleScene()
		{
			TsSceneSwitcher.Instance.DeleteScene(TsSceneSwitcher.ESceneType.BattleScene);
		}

		[DebuggerHidden]
		public static IEnumerator CollectAndSwitch(TsSceneSwitcher.ESceneType eSceneType, bool bCollect, bool bCollectActivate, bool bSwitch)
		{
			SceneSwitch.<CollectAndSwitch>c__Iterator23 <CollectAndSwitch>c__Iterator = new SceneSwitch.<CollectAndSwitch>c__Iterator23();
			<CollectAndSwitch>c__Iterator.eSceneType = eSceneType;
			<CollectAndSwitch>c__Iterator.bCollect = bCollect;
			<CollectAndSwitch>c__Iterator.bCollectActivate = bCollectActivate;
			<CollectAndSwitch>c__Iterator.bSwitch = bSwitch;
			<CollectAndSwitch>c__Iterator.<$>eSceneType = eSceneType;
			<CollectAndSwitch>c__Iterator.<$>bCollect = bCollect;
			<CollectAndSwitch>c__Iterator.<$>bCollectActivate = bCollectActivate;
			<CollectAndSwitch>c__Iterator.<$>bSwitch = bSwitch;
			return <CollectAndSwitch>c__Iterator;
		}

		public static void SetLastLoadedField(string map)
		{
			SceneSwitch.LastLoadedField = map;
		}
	}
}
