using System;

namespace Ndoors.Framework.Stage
{
	public static class Scene
	{
		public enum Type
		{
			EMPTY,
			ERROR,
			SYSCHECK,
			PREDOWNLOAD,
			NPATCH_DOWNLOAD,
			LOGIN,
			INITIALIZE,
			SELECTCHAR,
			PREPAREGAME,
			JUSTWAIT,
			WORLD,
			DUNGEON,
			BATTLE,
			CUTSCENE,
			SOLDIER_BATCH,
			MAX
		}

		public enum SubType
		{
			EMPTY,
			SUB_SELECT_CHAR,
			SUB_CREATE_CHAR
		}

		public static bool IsGamePlayScene
		{
			get
			{
				return Scene.Type.WORLD <= Scene.CurScene;
			}
		}

		public static bool IsSwitchingScene
		{
			get
			{
				return Scene.Type.DUNGEON <= Scene.CurScene && Scene.CurScene <= Scene.Type.BATTLE;
			}
		}

		public static Scene.Type CurScene
		{
			get;
			private set;
		}

		public static Scene.Type PreScene
		{
			get;
			private set;
		}

		public static Scene.SubType CurSubScene
		{
			get;
			private set;
		}

		static Scene()
		{
			Scene.CurScene = Scene.Type.EMPTY;
			Scene.PreScene = Scene.Type.EMPTY;
			Scene.CurSubScene = Scene.SubType.EMPTY;
		}

		public static bool IsCurScene(Scene.Type et)
		{
			return et == Scene.CurScene;
		}

		public static void ChangeSceneType(Scene.Type ename)
		{
			Scene.PreScene = Scene.CurScene;
			Scene.CurScene = ename;
			TsLog.LogWarning("ChangeSceneName {0} => {1} ~~~~~~~~~~~~~~~", new object[]
			{
				Scene.PreScene,
				Scene.CurScene
			});
		}

		public static void ChangeSubSceneType(Scene.SubType ename)
		{
			Scene.CurSubScene = ename;
		}

		public static bool NeedLoadNewScene()
		{
			if (!Scene.IsSwitchingScene)
			{
				return true;
			}
			bool flag = false;
			if (Scene.PreScene.Equals(Scene.Type.DUNGEON) && Scene.CurScene.Equals(Scene.Type.BATTLE))
			{
				flag = true;
			}
			else if (Scene.PreScene.Equals(Scene.Type.BATTLE) && Scene.CurScene.Equals(Scene.Type.DUNGEON))
			{
				flag = true;
			}
			return !flag;
		}
	}
}
