using System;
using System.Collections;
using System.Diagnostics;
using TsBundle;
using UnityEngine;

namespace StageHelper
{
	public static class CommonTasks
	{
		private static bool Muted;

		private static bool gotoBattle;

		private static float fBattleStartTime;

		public static bool IsEndOfPrework
		{
			get;
			private set;
		}

		[DebuggerHidden]
		public static IEnumerator CommonUpdate()
		{
			return new CommonTasks.<CommonUpdate>c__Iterator10();
		}

		[DebuggerHidden]
		public static IEnumerator InitializeChangeScene()
		{
			return new CommonTasks.<InitializeChangeScene>c__Iterator11();
		}

		[DebuggerHidden]
		public static IEnumerator LoadEmptyMainScene()
		{
			return new CommonTasks.<LoadEmptyMainScene>c__Iterator12();
		}

		[DebuggerHidden]
		public static IEnumerator MuteAudio(bool bMute)
		{
			CommonTasks.<MuteAudio>c__Iterator13 <MuteAudio>c__Iterator = new CommonTasks.<MuteAudio>c__Iterator13();
			<MuteAudio>c__Iterator.bMute = bMute;
			<MuteAudio>c__Iterator.<$>bMute = bMute;
			return <MuteAudio>c__Iterator;
		}

		public static void MuteAudioOnOff(bool bMute)
		{
			if (bMute)
			{
				if (!CommonTasks.Muted)
				{
					TsAudio.StoreMuteAllAudio();
					TsAudio.SetMuteAllAudio(true);
					TsAudio.RefreshAllMuteAudio();
					TsAudio.UseReservePlay = true;
					CommonTasks.Muted = true;
					TsLog.Log("------ Audio Mute On", new object[0]);
				}
			}
			else if (CommonTasks.Muted)
			{
				TsAudio.RestoreMuteAllAudio();
				TsAudio.FlushReservedPlayList();
				TsAudio.UseReservePlay = false;
				TsAudio.RefreshAllMuteAudio();
				CommonTasks.Muted = false;
				TsLog.Log("------ Audio Mute Off", new object[0]);
			}
		}

		[DebuggerHidden]
		public static IEnumerator BGMExceptMuteAudio(bool bMute)
		{
			CommonTasks.<BGMExceptMuteAudio>c__Iterator14 <BGMExceptMuteAudio>c__Iterator = new CommonTasks.<BGMExceptMuteAudio>c__Iterator14();
			<BGMExceptMuteAudio>c__Iterator.bMute = bMute;
			<BGMExceptMuteAudio>c__Iterator.<$>bMute = bMute;
			return <BGMExceptMuteAudio>c__Iterator;
		}

		public static void BGMExceptMuteAudioOnOff(bool bMute)
		{
			if (bMute)
			{
				if (!CommonTasks.Muted)
				{
					TsAudio.StoreMuteAllAudio();
					TsAudio.SetExceptMuteAllAudio(EAudioType.BGM, true);
					TsAudio.RefreshAllMuteAudio();
					TsAudio.UseReservePlay = true;
					CommonTasks.Muted = true;
					TsLog.Log("------ Audio Mute On", new object[0]);
				}
			}
			else if (CommonTasks.Muted)
			{
				TsAudio.RestoreMuteAllAudio();
				TsAudio.FlushReservedPlayList();
				TsAudio.UseReservePlay = false;
				TsAudio.RefreshAllMuteAudio();
				CommonTasks.Muted = false;
				TsLog.Log("------ Audio Mute Off", new object[0]);
			}
		}

		[DebuggerHidden]
		public static IEnumerator ExceptMuteAudioOnOff(EAudioType type, bool bMute)
		{
			CommonTasks.<ExceptMuteAudioOnOff>c__Iterator15 <ExceptMuteAudioOnOff>c__Iterator = new CommonTasks.<ExceptMuteAudioOnOff>c__Iterator15();
			<ExceptMuteAudioOnOff>c__Iterator.bMute = bMute;
			<ExceptMuteAudioOnOff>c__Iterator.type = type;
			<ExceptMuteAudioOnOff>c__Iterator.<$>bMute = bMute;
			<ExceptMuteAudioOnOff>c__Iterator.<$>type = type;
			return <ExceptMuteAudioOnOff>c__Iterator;
		}

		[DebuggerHidden]
		public static IEnumerator ClearAudioStack()
		{
			return new CommonTasks.<ClearAudioStack>c__Iterator16();
		}

		[DebuggerHidden]
		public static IEnumerator MemoryCleaning(bool unloadUnusedAsset, int countGC)
		{
			CommonTasks.<MemoryCleaning>c__Iterator17 <MemoryCleaning>c__Iterator = new CommonTasks.<MemoryCleaning>c__Iterator17();
			<MemoryCleaning>c__Iterator.countGC = countGC;
			<MemoryCleaning>c__Iterator.unloadUnusedAsset = unloadUnusedAsset;
			<MemoryCleaning>c__Iterator.<$>countGC = countGC;
			<MemoryCleaning>c__Iterator.<$>unloadUnusedAsset = unloadUnusedAsset;
			return <MemoryCleaning>c__Iterator;
		}

		public static void ClearAssetBundleResources(bool unloadTrue)
		{
		}

		[DebuggerHidden]
		public static IEnumerator LoadLevelSubScene(string path, string stackName)
		{
			CommonTasks.<LoadLevelSubScene>c__Iterator18 <LoadLevelSubScene>c__Iterator = new CommonTasks.<LoadLevelSubScene>c__Iterator18();
			<LoadLevelSubScene>c__Iterator.path = path;
			<LoadLevelSubScene>c__Iterator.stackName = stackName;
			<LoadLevelSubScene>c__Iterator.<$>path = path;
			<LoadLevelSubScene>c__Iterator.<$>stackName = stackName;
			return <LoadLevelSubScene>c__Iterator;
		}

		[DebuggerHidden]
		public static IEnumerator EnableCharacterLoad()
		{
			return new CommonTasks.<EnableCharacterLoad>c__Iterator19();
		}

		[DebuggerHidden]
		public static IEnumerator LoadEnvironment(bool useEnvironmentSetting)
		{
			return new CommonTasks.<LoadEnvironment>c__Iterator1A();
		}

		[DebuggerHidden]
		public static IEnumerator WaitMyCharacterReadyToAction()
		{
			return new CommonTasks.<WaitMyCharacterReadyToAction>c__Iterator1B();
		}

		[DebuggerHidden]
		public static IEnumerator SetGUIBehaviourScene()
		{
			return new CommonTasks.<SetGUIBehaviourScene>c__Iterator1C();
		}

		[DebuggerHidden]
		public static IEnumerator FinalizeChangeScene(bool bHideLoadingScreen)
		{
			CommonTasks.<FinalizeChangeScene>c__Iterator1D <FinalizeChangeScene>c__Iterator1D = new CommonTasks.<FinalizeChangeScene>c__Iterator1D();
			<FinalizeChangeScene>c__Iterator1D.bHideLoadingScreen = bHideLoadingScreen;
			<FinalizeChangeScene>c__Iterator1D.<$>bHideLoadingScreen = bHideLoadingScreen;
			return <FinalizeChangeScene>c__Iterator1D;
		}

		[DebuggerHidden]
		public static IEnumerator DownloadAsset(string path, PostProcPerItem callback, object obj, bool unloadAfterDownload)
		{
			CommonTasks.<DownloadAsset>c__Iterator1E <DownloadAsset>c__Iterator1E = new CommonTasks.<DownloadAsset>c__Iterator1E();
			<DownloadAsset>c__Iterator1E.path = path;
			<DownloadAsset>c__Iterator1E.callback = callback;
			<DownloadAsset>c__Iterator1E.obj = obj;
			<DownloadAsset>c__Iterator1E.unloadAfterDownload = unloadAfterDownload;
			<DownloadAsset>c__Iterator1E.<$>path = path;
			<DownloadAsset>c__Iterator1E.<$>callback = callback;
			<DownloadAsset>c__Iterator1E.<$>obj = obj;
			<DownloadAsset>c__Iterator1E.<$>unloadAfterDownload = unloadAfterDownload;
			return <DownloadAsset>c__Iterator1E;
		}

		[DebuggerHidden]
		private static IEnumerator _LoadAsset(string path, ItemType type, PostProcPerItem callback, object obj, bool unloadAfterDownload, string stackName)
		{
			CommonTasks.<_LoadAsset>c__Iterator1F <_LoadAsset>c__Iterator1F = new CommonTasks.<_LoadAsset>c__Iterator1F();
			<_LoadAsset>c__Iterator1F.path = path;
			<_LoadAsset>c__Iterator1F.stackName = stackName;
			<_LoadAsset>c__Iterator1F.type = type;
			<_LoadAsset>c__Iterator1F.callback = callback;
			<_LoadAsset>c__Iterator1F.obj = obj;
			<_LoadAsset>c__Iterator1F.unloadAfterDownload = unloadAfterDownload;
			<_LoadAsset>c__Iterator1F.<$>path = path;
			<_LoadAsset>c__Iterator1F.<$>stackName = stackName;
			<_LoadAsset>c__Iterator1F.<$>type = type;
			<_LoadAsset>c__Iterator1F.<$>callback = callback;
			<_LoadAsset>c__Iterator1F.<$>obj = obj;
			<_LoadAsset>c__Iterator1F.<$>unloadAfterDownload = unloadAfterDownload;
			return <_LoadAsset>c__Iterator1F;
		}

		[DebuggerHidden]
		public static IEnumerator DownloadStringXML(string resPath, PostProcPerItem callback)
		{
			CommonTasks.<DownloadStringXML>c__Iterator20 <DownloadStringXML>c__Iterator = new CommonTasks.<DownloadStringXML>c__Iterator20();
			<DownloadStringXML>c__Iterator.resPath = resPath;
			<DownloadStringXML>c__Iterator.callback = callback;
			<DownloadStringXML>c__Iterator.<$>resPath = resPath;
			<DownloadStringXML>c__Iterator.<$>callback = callback;
			return <DownloadStringXML>c__Iterator;
		}

		[DebuggerHidden]
		public static IEnumerator WaitGoToBattleWorld()
		{
			return new CommonTasks.<WaitGoToBattleWorld>c__Iterator21();
		}

		[DebuggerHidden]
		public static IEnumerator WaitGoToBattleSoldierBatch()
		{
			return new CommonTasks.<WaitGoToBattleSoldierBatch>c__Iterator22();
		}

		public static void GotoBattleReserve()
		{
			CommonTasks.gotoBattle = true;
			CommonTasks.fBattleStartTime = Time.realtimeSinceStartup + 0.5f;
		}

		public static bool IsAlertZone()
		{
			return true;
		}

		public static void LinkShadersInTheScene()
		{
			Renderer[] array = UnityEngine.Object.FindObjectsOfType(typeof(Renderer)) as Renderer[];
			int num = 0;
			Renderer[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Renderer renderer = array2[i];
				if (null != renderer)
				{
					if (renderer.transform.gameObject.layer != GUICamera.UILayer)
					{
						Material[] sharedMaterials = renderer.sharedMaterials;
						for (int j = 0; j < sharedMaterials.Length; j++)
						{
							Material material = sharedMaterials[j];
							if (material != null)
							{
								Shader shader = Shader.Find(material.shader.name);
								if (null != shader)
								{
									num++;
									material.shader = shader;
								}
							}
						}
					}
				}
			}
		}
	}
}
