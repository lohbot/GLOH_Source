using ConsoleCommand;
using ConsoleCommand.Statistics;
using Ndoors.Framework.Stage;
using Statistics;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class NrCommandInterpreter
{
	public class NrCommand
	{
		public string m_strCommand;

		public List<string> m_arArg;

		public NrCommand()
		{
			this.m_strCommand = string.Empty;
			this.m_arArg = new List<string>();
		}
	}

	public void Interpret(string strCommand)
	{
	}

	private void ErrorArgCount()
	{
		NrTSingleton<NrDebugConsole>.Instance.Print("argument count not match.");
	}

	public void ParseCommand(string str)
	{
		string[] array = str.Split(new char[]
		{
			' '
		});
		NrCommandInterpreter.NrCommand nrCommand = new NrCommandInterpreter.NrCommand();
		nrCommand.m_strCommand = array[0].Trim().ToLower();
		for (int i = 1; i < array.Length; i++)
		{
			nrCommand.m_arArg.Add(array[i].Trim().ToLower());
		}
		this.ProcessCommand(nrCommand);
	}

	public void ProcessCommand(NrCommandInterpreter.NrCommand command)
	{
		string strCommand = command.m_strCommand;
		switch (strCommand)
		{
		case "help":
			this.CommandHelp();
			return;
		case "cls":
			NrTSingleton<NrDebugConsole>.Instance.ClearScreen();
			return;
		case "list":
		{
			string text = command.m_arArg[0];
			if (text != null)
			{
				if (NrCommandInterpreter.<>f__switch$map6 == null)
				{
					NrCommandInterpreter.<>f__switch$map6 = new Dictionary<string, int>(2)
					{
						{
							"char",
							0
						},
						{
							"packet",
							1
						}
					};
				}
				int num2;
				if (NrCommandInterpreter.<>f__switch$map6.TryGetValue(text, out num2))
				{
					if (num2 != 0)
					{
						if (num2 == 1)
						{
							this.ShowPacketHistory(command);
						}
					}
					else
					{
						this.CommandCharList();
					}
				}
			}
			return;
		}
		case "set":
		{
			List<string> arArg = command.m_arArg;
			string text = command.m_arArg[0];
			switch (text)
			{
			case "mem.monitor":
				if (arArg.Count >= 2)
				{
					bool flag = Utility.ParseBoolean(arArg[1], false);
					if (flag)
					{
						if (MemoryMonitor.Start())
						{
							NrTSingleton<NrDebugConsole>.Instance.Print("메모리 모니터링이 시작되었습니다. ({0}분 마다 메모리 검사 진행)", new object[]
							{
								MemoryMonitor.cycleTime
							});
						}
					}
					else if (MemoryMonitor.Stop())
					{
						NrTSingleton<NrDebugConsole>.Instance.Print("메모리 모니터링이 종료되었습니다.");
					}
				}
				break;
			case "mem.monitor.cycle":
			{
				int num3;
				if (arArg.Count >= 2 && int.TryParse(arArg[1], out num3))
				{
					MemoryMonitor.cycleTime = num3;
					NrTSingleton<NrDebugConsole>.Instance.Print(string.Format(" => 모니터링 주기가 {0}분으로 설정되었습니다. 해당 설정은 게임 재시작시 적용됩니다.", num3));
				}
				break;
			}
			case "mem.monitor.growup":
			{
				int num4;
				if (arArg.Count >= 2 && int.TryParse(arArg[1], out num4))
				{
					MemoryMonitor.growUpAllowedSize = num4;
					NrTSingleton<NrDebugConsole>.Instance.Print(string.Format("평균 증가량이 {0}MB 보다 크면, 경고 화면을 보여줍니다.", num4));
				}
				break;
			}
			case "showhide":
			{
				short charunique = Convert.ToInt16(command.m_arArg[1]);
				NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(charunique);
				if (command.m_arArg[2] == "true")
				{
					charByCharUnique.SetShowHide3DModel(true, true, true);
				}
				else
				{
					charByCharUnique.SetShowHide3DModel(false, false, false);
				}
				break;
			}
			case "charpos":
			{
				short charunique2 = Convert.ToInt16(command.m_arArg[1]);
				NrCharBase charByCharUnique2 = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(charunique2);
				Vector3 localPosition = charByCharUnique2.Get3DChar().GetRootGameObject().transform.localPosition;
				localPosition.y = 150f;
				charByCharUnique2.Get3DChar().GetRootGameObject().transform.localPosition = localPosition;
				NrTSingleton<NrDebugConsole>.Instance.Print(charByCharUnique2.Get3DChar().GetRootGameObject().name);
				break;
			}
			case "log":
				if (command.m_arArg[1].Contains("true"))
				{
					NrTSingleton<NrGlobalReference>.Instance.IsEnableLog = true;
					PlayerPrefs.SetInt("EnableLog", 1);
					NrTSingleton<NrDebugConsole>.Instance.Print("log Enable.");
				}
				else
				{
					NrTSingleton<NrGlobalReference>.Instance.IsEnableLog = false;
					PlayerPrefs.SetInt("EnableLog", 0);
					NrTSingleton<NrDebugConsole>.Instance.Print("log Disable.");
				}
				break;
			case "localwww":
			{
				string text2 = command.m_arArg[1];
				if (text2 != null)
				{
					if (NrCommandInterpreter.<>f__switch$map7 == null)
					{
						NrCommandInterpreter.<>f__switch$map7 = new Dictionary<string, int>(2)
						{
							{
								"on",
								0
							},
							{
								"off",
								1
							}
						};
					}
					int num5;
					if (NrCommandInterpreter.<>f__switch$map7.TryGetValue(text2, out num5))
					{
						if (num5 != 0)
						{
							if (num5 == 1)
							{
								NrTSingleton<NrGlobalReference>.Instance.localWWW = false;
								PlayerPrefs.SetInt("LocalWWW", 0);
								NrTSingleton<NrDebugConsole>.Instance.Print("set local www off");
							}
						}
						else
						{
							NrTSingleton<NrGlobalReference>.Instance.localWWW = true;
							PlayerPrefs.SetInt("LocalWWW", 1);
							NrTSingleton<NrDebugConsole>.Instance.Print("set local www on");
						}
					}
				}
				break;
			}
			case "basepath":
				NrTSingleton<NrGlobalReference>.Instance.basePath = command.m_arArg[1];
				NrTSingleton<NrDebugConsole>.Instance.Print("basepath set to " + NrTSingleton<NrGlobalReference>.Instance.basePath);
				break;
			case "ndt":
			{
				string a = command.m_arArg[1];
				if (a == "off")
				{
					NrTSingleton<NrGlobalReference>.Instance.useCache = true;
					NrTSingleton<NrDebugConsole>.Instance.Print("set to using Assetbundle Table");
				}
				else
				{
					NrTSingleton<NrGlobalReference>.Instance.useCache = false;
					NrTSingleton<NrDebugConsole>.Instance.Print("set to using NDT Table");
				}
				PlayerPrefs.SetInt("UseNDT", (!NrTSingleton<NrGlobalReference>.Instance.useCache) ? 0 : 1);
				break;
			}
			case "cache":
			{
				string text2 = command.m_arArg[1];
				switch (text2)
				{
				case "on":
					NrTSingleton<NrGlobalReference>.Instance.useCache = true;
					PlayerPrefs.SetInt("UseCache", 1);
					NrTSingleton<NrDebugConsole>.Instance.Print("set cache On");
					break;
				case "off":
					NrTSingleton<NrGlobalReference>.Instance.useCache = false;
					PlayerPrefs.SetInt("UseCache", 0);
					NrTSingleton<NrDebugConsole>.Instance.Print("set cache Off");
					break;
				case "clean":
					Caching.CleanCache();
					NrTSingleton<NrDebugConsole>.Instance.Print("cleaning cache space...");
					break;
				}
				break;
			}
			case "clear":
				PlayerPrefs.DeleteAll();
				NrTSingleton<NrDebugConsole>.Instance.Print("All PlayerPrefs are cleared.");
				break;
			case "sound":
			{
				string text2 = command.m_arArg[1];
				if (text2 != null)
				{
					if (NrCommandInterpreter.<>f__switch$map9 == null)
					{
						NrCommandInterpreter.<>f__switch$map9 = new Dictionary<string, int>(2)
						{
							{
								"on",
								0
							},
							{
								"off",
								1
							}
						};
					}
					int num5;
					if (NrCommandInterpreter.<>f__switch$map9.TryGetValue(text2, out num5))
					{
						if (num5 != 0)
						{
							if (num5 == 1)
							{
								TsAudio.SetDisableDownloadAllAudio(true);
								NrTSingleton<NrDebugConsole>.Instance.Print("set sound off");
							}
						}
						else
						{
							TsAudio.SetDisableDownloadAllAudio(false);
							NrTSingleton<NrDebugConsole>.Instance.Print("set sound on");
						}
					}
				}
				break;
			}
			case "framerate":
			{
				int targetFrameRate = int.Parse(command.m_arArg[1]);
				Application.targetFrameRate = targetFrameRate;
				NrTSingleton<NrDebugConsole>.Instance.Print("Set FrameRate {0}", new object[]
				{
					targetFrameRate.ToString()
				});
				break;
			}
			}
			return;
		}
		case "clear":
		{
			string text = command.m_arArg[0];
			if (text != null)
			{
				if (NrCommandInterpreter.<>f__switch$mapB == null)
				{
					NrCommandInterpreter.<>f__switch$mapB = new Dictionary<string, int>(1)
					{
						{
							"bundle",
							0
						}
					};
				}
				int num2;
				if (NrCommandInterpreter.<>f__switch$mapB.TryGetValue(text, out num2))
				{
					if (num2 == 0)
					{
						if (command.m_arArg.Count >= 2)
						{
							string name = command.m_arArg[1];
							Holder.ClearStackItem(name, false);
						}
					}
				}
			}
			return;
		}
		case "call":
		{
			string text = command.m_arArg[0];
			switch (text)
			{
			case "bundle.clear":
				Resources.UnloadUnusedAssets();
				NrTSingleton<NrDebugConsole>.Instance.Print("called Resources.UnloadUnusedAssets()");
				break;
			case "gc":
				GC.Collect();
				NrTSingleton<NrDebugConsole>.Instance.Print("called garbage collection.");
				break;
			case "bundle.unload":
				if (command.m_arArg.Count >= 2)
				{
					string text3 = command.m_arArg[1];
					Holder.RemoveWWWItem(text3, false);
					Resources.UnloadUnusedAssets();
					NrTSingleton<NrDebugConsole>.Instance.Print("unload => \"{0}\"", new object[]
					{
						text3
					});
				}
				else
				{
					Resources.UnloadUnusedAssets();
					NrTSingleton<NrDebugConsole>.Instance.Print("called Resources.UnloadUnusedAssets()");
				}
				break;
			}
			return;
		}
		case "quit":
			NrTSingleton<NrMainSystem>.Instance.QuitGame(false);
			return;
		case "show":
		{
			string text = command.m_arArg[0];
			switch (text)
			{
			case "mem.growup":
				MemoryCollection.Print(MemoryCollection.Mode.NewlyLeakOnly);
				goto IL_1E8D;
			case "mem.leak":
				MemoryCollection.Print(MemoryCollection.Mode.AllLeaks);
				goto IL_1E8D;
			case "mem.all":
				MemoryCollection.Print(MemoryCollection.Mode.LoadedAll);
				goto IL_1E8D;
			case "mem.newl":
				MemoryCollection.Print(MemoryCollection.Mode.NewObjectOnly);
				goto IL_1E8D;
			case "mem.monitor":
				MemoryMonitor.Show(180f);
				goto IL_1E8D;
			case "mem.monitor.cycle":
				NrTSingleton<NrDebugConsole>.Instance.Print(string.Format(" => {0}분 마다 메모리 모니터링을 합니다. (현재 로딩되어 있는 오브젝트 수집 및 경고)", MemoryMonitor.cycleTime));
				goto IL_1E8D;
			case "mem.monitor.growup":
				NrTSingleton<NrDebugConsole>.Instance.Print(string.Format("평균 증가량이 {0}MB 보다 크면, 경고 화면을 보여줍니다.", MemoryMonitor.growUpAllowedSize));
				goto IL_1E8D;
			case "packet":
				this.ShowPacketInfo(command);
				goto IL_1E8D;
			case "bundle":
			{
				string text4;
				if (command.m_arArg.Count < 2)
				{
					text4 = Holder.DbgPrint_BundleList();
				}
				else
				{
					text4 = Holder.DbgPrint_BundleList(command.m_arArg[1]);
				}
				if (!string.IsNullOrEmpty(text4))
				{
					NrTSingleton<NrDebugConsole>.Instance.Print(text4);
					TsLog.Log("[TsBundle] {0}", new object[]
					{
						text4
					});
				}
				goto IL_1E8D;
			}
			case "bundlecnt":
			{
				string text5 = Holder.DbgPrint_BundleCount();
				NrTSingleton<NrDebugConsole>.Instance.Print(text5);
				TsLog.Log("[TsBundle] {0}", new object[]
				{
					text5
				});
				goto IL_1E8D;
			}
			case "downloaded":
				NrTSingleton<NrDebugConsole>.Instance.Print(Holder.DbgPrint_Downloaded());
				goto IL_1E8D;
			case "bundleinfo":
				TsImmortal.bundleDbgPrint.enabled = !TsImmortal.bundleDbgPrint.enabled;
				goto IL_1E8D;
			case "char":
				this.CommandCharInfo(command);
				goto IL_1E8D;
			case "basepath":
				NrTSingleton<NrDebugConsole>.Instance.Print(NrTSingleton<NrGlobalReference>.Instance.basePath);
				goto IL_1E8D;
			case "cache":
			{
				NrTSingleton<NrDebugConsole>.Instance.Print("Cache Active : " + NrTSingleton<NrGlobalReference>.Instance.useCache.ToString());
				long num6 = Caching.spaceFree / 1048576L;
				NrTSingleton<NrDebugConsole>.Instance.Print(" Free space : " + num6.ToString() + "MB");
				goto IL_1E8D;
			}
			case "autopath":
				NrTSingleton<NrDebugConsole>.Instance.Print(NrTSingleton<NrAutoPath>.Instance.GetDebug());
				goto IL_1E8D;
			case "quest":
				NrTSingleton<NrDebugConsole>.Instance.Print(NrTSingleton<NkQuestManager>.Instance.GetDebugMsg());
				goto IL_1E8D;
			case "config":
				NrTSingleton<NrDebugConsole>.Instance.Print("Local WWW : " + NrTSingleton<NrGlobalReference>.Instance.localWWW.ToString());
				NrTSingleton<NrDebugConsole>.Instance.Print("BasePath : " + NrTSingleton<NrGlobalReference>.Instance.basePath);
				NrTSingleton<NrDebugConsole>.Instance.Print("LocalPath : " + Option.GetProtocolRootPath(Protocol.FILE));
				NrTSingleton<NrDebugConsole>.Instance.Print("WebPath : " + Option.GetProtocolRootPath(Protocol.HTTP));
				NrTSingleton<NrDebugConsole>.Instance.Print("Cache Active : " + NrTSingleton<NrGlobalReference>.Instance.useCache);
				NrTSingleton<NrDebugConsole>.Instance.Print("Loging Active : " + NrTSingleton<NrGlobalReference>.Instance.IsEnableLog);
				NrTSingleton<NrDebugConsole>.Instance.Print("Audio Options : ");
				for (EAudioType eAudioType = EAudioType.SFX; eAudioType < EAudioType.TOTAL; eAudioType++)
				{
					NrTSingleton<NrDebugConsole>.Instance.Print(" - Audio " + eAudioType.ToString() + " : " + (!TsAudio.IsDisableDownloadAudio(eAudioType)).ToString());
				}
				goto IL_1E8D;
			case "usetexture":
				TsMemLog.TextureByFormat();
				goto IL_1E8D;
			case "camera":
				NrTSingleton<NrDebugConsole>.Instance.Print(string.Concat(new object[]
				{
					"UICamera Height: ",
					GUICamera.height,
					" Width: ",
					GUICamera.width
				}));
				NrTSingleton<NrDebugConsole>.Instance.Print(string.Concat(new object[]
				{
					"MainCamera Height: ",
					Screen.height,
					"Width: ",
					Screen.width
				}));
				goto IL_1E8D;
			case "fps":
				NrTSingleton<NrDebugConsole>.Instance.Print("TargetFrameRate (FPS) = {0}", new object[]
				{
					Application.targetFrameRate
				});
				goto IL_1E8D;
			case "version":
				NrTSingleton<NrDebugConsole>.Instance.Print("UnityVersion = {0}", new object[]
				{
					Application.unityVersion
				});
				goto IL_1E8D;
			case "log":
				NrTSingleton<NrDebugConsole>.Instance.Print(string.Format("debugLog => {0}", (!NrTSingleton<NrGlobalReference>.Instance.IsEnableLog) ? "Disabled" : "Enabled"));
				goto IL_1E8D;
			case "stage":
			{
				string str = StageSystem.ToStringStatus();
				NrTSingleton<NrDebugConsole>.Instance.Print(str);
				goto IL_1E8D;
			}
			case "dlginfo":
				NrTSingleton<NrDebugConsole>.Instance.Print(NrTSingleton<FormsManager>.Instance.GetDlgStatus());
				goto IL_1E8D;
			case "monhp":
				if (Scene.IsCurScene(Scene.Type.BATTLE))
				{
					string text6 = string.Empty;
					NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
					if (charArray != null)
					{
						for (int i = 0; i < charArray.Length; i++)
						{
							NkBattleChar nkBattleChar = charArray[i];
							if (nkBattleChar != null)
							{
								text6 += string.Format("{0} : {1} / {2}\r\n", nkBattleChar.Get3DName(), nkBattleChar.GetSoldierInfo().GetHP(), nkBattleChar.GetMaxHP(false));
							}
						}
						NrTSingleton<NrDebugConsole>.Instance.Print(text6);
					}
				}
				goto IL_1E8D;
			case "path":
			{
				string text2 = command.m_arArg[1];
				if (text2 != null)
				{
					if (NrCommandInterpreter.<>f__switch$mapD == null)
					{
						NrCommandInterpreter.<>f__switch$mapD = new Dictionary<string, int>(2)
						{
							{
								"on",
								0
							},
							{
								"off",
								1
							}
						};
					}
					int num5;
					if (NrCommandInterpreter.<>f__switch$mapD.TryGetValue(text2, out num5))
					{
						if (num5 != 0)
						{
							if (num5 == 1)
							{
								GMCommand_Dlg.m_bShowNavPath = false;
								NrTSingleton<NrDebugConsole>.Instance.Print("Show Move Path : false");
							}
						}
						else
						{
							GMCommand_Dlg.m_bShowNavPath = true;
							NrTSingleton<NrDebugConsole>.Instance.Print("Show Move Path : true");
						}
					}
				}
				goto IL_1E8D;
			}
			case "qu":
			{
				TsQualityManager.Level qualitySettings = TsQualityManager.Level.LOWEST;
				string text2 = command.m_arArg[1];
				switch (text2)
				{
				case "0":
					qualitySettings = TsQualityManager.Level.LOWEST;
					break;
				case "1":
					qualitySettings = TsQualityManager.Level.MEDIUM;
					break;
				case "2":
					qualitySettings = TsQualityManager.Level.HIGHEST;
					break;
				}
				CustomQuality.GetInstance().SetQualitySettings(qualitySettings);
				goto IL_1E8D;
			}
			case "tx":
			{
				string text2 = command.m_arArg[1];
				switch (text2)
				{
				case "0":
					QualitySettings.masterTextureLimit = 0;
					break;
				case "1":
					QualitySettings.masterTextureLimit = 1;
					break;
				case "2":
					QualitySettings.masterTextureLimit = 2;
					break;
				case "4":
					QualitySettings.masterTextureLimit = 4;
					break;
				}
				goto IL_1E8D;
			}
			case "charinfo":
			{
				GameObject gameObject = GameObject.Find(NrTSingleton<NkCharManager>.Instance.GetCharName());
				if (gameObject != null)
				{
					Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
					Renderer[] array = componentsInChildren;
					for (int j = 0; j < array.Length; j++)
					{
						Renderer renderer = array[j];
						if (renderer != null)
						{
							if (renderer as SkinnedMeshRenderer)
							{
								NrTSingleton<NrDebugConsole>.Instance.Print("======================");
								SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
								skinnedMeshRenderer.castShadows = false;
								skinnedMeshRenderer.receiveShadows = false;
								NrTSingleton<NrDebugConsole>.Instance.Print("SkinQuality : " + skinnedMeshRenderer.quality);
								NrTSingleton<NrDebugConsole>.Instance.Print("R Shadow : " + skinnedMeshRenderer.receiveShadows);
								NrTSingleton<NrDebugConsole>.Instance.Print("Update : " + skinnedMeshRenderer.updateWhenOffscreen);
								if (renderer.material != null)
								{
									NrTSingleton<NrDebugConsole>.Instance.Print("Material : " + renderer.material.name);
									NrTSingleton<NrDebugConsole>.Instance.Print("Shader Name : " + renderer.material.shader.name);
									if (renderer.material.mainTexture != null)
									{
										Texture2D texture2D = renderer.material.mainTexture as Texture2D;
										NrTSingleton<NrDebugConsole>.Instance.Print("Texture : " + renderer.material.mainTexture.name);
										NrTSingleton<NrDebugConsole>.Instance.Print("anisoLevel : " + renderer.material.mainTexture.anisoLevel);
										NrTSingleton<NrDebugConsole>.Instance.Print("filterMode : " + renderer.material.mainTexture.filterMode);
										NrTSingleton<NrDebugConsole>.Instance.Print("masterTextureLimit : " + Texture.masterTextureLimit);
										NrTSingleton<NrDebugConsole>.Instance.Print("texelSize : " + renderer.material.mainTexture.texelSize.ToString());
										NrTSingleton<NrDebugConsole>.Instance.Print("width : " + renderer.material.mainTexture.width);
										NrTSingleton<NrDebugConsole>.Instance.Print("height : " + renderer.material.mainTexture.height);
										NrTSingleton<NrDebugConsole>.Instance.Print("mipMapBias : " + renderer.material.mainTexture.mipMapBias);
										if (texture2D != null)
										{
											NrTSingleton<NrDebugConsole>.Instance.Print("mipmapcount : " + texture2D.mipmapCount);
											NrTSingleton<NrDebugConsole>.Instance.Print("format : " + texture2D.format);
											NrTSingleton<NrDebugConsole>.Instance.Print("Wrap : " + texture2D.wrapMode);
										}
									}
								}
								Color color = renderer.material.color;
								NrTSingleton<NrDebugConsole>.Instance.Print("R : " + color.r);
								NrTSingleton<NrDebugConsole>.Instance.Print("G : " + color.g);
								NrTSingleton<NrDebugConsole>.Instance.Print("b : " + color.b);
								NrTSingleton<NrDebugConsole>.Instance.Print("a : " + color.a);
								renderer.material.color = Color.white;
								color = renderer.material.color;
								NrTSingleton<NrDebugConsole>.Instance.Print("CR : " + color.r);
								NrTSingleton<NrDebugConsole>.Instance.Print("CG : " + color.g);
								NrTSingleton<NrDebugConsole>.Instance.Print("Cb : " + color.b);
								NrTSingleton<NrDebugConsole>.Instance.Print("CA : " + color.a);
							}
							else if (renderer.material != null && renderer.material.name.Contains("Axe"))
							{
								Color color2 = renderer.material.color;
								NrTSingleton<NrDebugConsole>.Instance.Print("CR : " + color2.r);
								NrTSingleton<NrDebugConsole>.Instance.Print("CG : " + color2.g);
								NrTSingleton<NrDebugConsole>.Instance.Print("Cb : " + color2.b);
								NrTSingleton<NrDebugConsole>.Instance.Print("CA : " + color2.a);
								NrTSingleton<NrDebugConsole>.Instance.Print("Material : " + renderer.material.name);
								NrTSingleton<NrDebugConsole>.Instance.Print("Shader Name : " + renderer.material.shader.name);
								if (renderer.material.mainTexture != null)
								{
									Texture2D texture2D2 = renderer.material.mainTexture as Texture2D;
									NrTSingleton<NrDebugConsole>.Instance.Print("Texture : " + renderer.material.mainTexture.name);
									NrTSingleton<NrDebugConsole>.Instance.Print("anisoLevel : " + renderer.material.mainTexture.anisoLevel);
									NrTSingleton<NrDebugConsole>.Instance.Print("filterMode : " + renderer.material.mainTexture.filterMode);
									NrTSingleton<NrDebugConsole>.Instance.Print("masterTextureLimit : " + Texture.masterTextureLimit);
									NrTSingleton<NrDebugConsole>.Instance.Print("texelSize : " + renderer.material.mainTexture.texelSize.ToString());
									NrTSingleton<NrDebugConsole>.Instance.Print("width : " + renderer.material.mainTexture.width);
									NrTSingleton<NrDebugConsole>.Instance.Print("height : " + renderer.material.mainTexture.height);
									NrTSingleton<NrDebugConsole>.Instance.Print("mipMapBias : " + renderer.material.mainTexture.mipMapBias);
									if (texture2D2 != null)
									{
										NrTSingleton<NrDebugConsole>.Instance.Print("mipmapcount : " + texture2D2.mipmapCount);
										NrTSingleton<NrDebugConsole>.Instance.Print("format : " + texture2D2.format);
										NrTSingleton<NrDebugConsole>.Instance.Print("Wrap : " + texture2D2.wrapMode);
									}
								}
							}
						}
					}
				}
				UnityEngine.Object[] array2 = UnityEngine.Object.FindObjectsOfType(typeof(Light));
				UnityEngine.Object[] array3 = array2;
				for (int k = 0; k < array3.Length; k++)
				{
					UnityEngine.Object @object = array3[k];
					if (@object is Light)
					{
						Light light = @object as Light;
						light.enabled = false;
						NrTSingleton<NrDebugConsole>.Instance.Print("======================");
						NrTSingleton<NrDebugConsole>.Instance.Print("Lighht: " + light.name);
						NrTSingleton<NrDebugConsole>.Instance.Print("Lighht: " + light.enabled);
						NrTSingleton<NrDebugConsole>.Instance.Print("Shadow: " + light.shadows.ToString());
					}
				}
				goto IL_1E8D;
			}
			case "ev":
			{
				int nMapIndex = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().m_kCharMapInfo.m_nMapIndex;
				MapTriggerInfo mapTriggerInfo = EventTriggerMapManager.Instance.GetMapTriggerInfo(nMapIndex);
				if (mapTriggerInfo == null)
				{
					return;
				}
				NrTSingleton<NrDebugConsole>.Instance.Print("======================");
				NrTSingleton<NrDebugConsole>.Instance.Print("EventTrigger Name");
				EventTrigger_Game[] trigger_Game = mapTriggerInfo.GetTrigger_Game();
				for (int l = 0; l < trigger_Game.Length; l++)
				{
					EventTrigger_Game eventTrigger_Game = trigger_Game[l];
					NrTSingleton<NrDebugConsole>.Instance.Print("{0}", new object[]
					{
						eventTrigger_Game.name
					});
				}
				NrTSingleton<NrDebugConsole>.Instance.Print("======================");
				goto IL_1E8D;
			}
			case "evst":
			{
				int nMapIndex2 = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().m_kCharMapInfo.m_nMapIndex;
				EventTrigger_Game eventTrigger_Game2 = EventTriggerMapManager.Instance.GetEventTrigger(nMapIndex2, command.m_arArg[1]) as EventTrigger_Game;
				List<GameObject> eventConditionList = eventTrigger_Game2.EventConditionList;
				List<GameObject> stateConditionList = eventTrigger_Game2.StateConditionList;
				NrTSingleton<NrDebugConsole>.Instance.Print("EventTrigger Name - {0} (Unique:{1})", new object[]
				{
					eventTrigger_Game2.gameObject.name,
					eventTrigger_Game2.EventTriggerUnique
				});
				NrTSingleton<NrDebugConsole>.Instance.Print("\nEventCondition");
				NrTSingleton<NrDebugConsole>.Instance.Print("======================================");
				foreach (GameObject current in eventConditionList)
				{
					EventTriggerItem_EventCondition component = current.GetComponent<EventTriggerItem_EventCondition>();
					if (!(component == null))
					{
						NrTSingleton<NrDebugConsole>.Instance.Print("{0} - State : {1}", new object[]
						{
							component.name,
							component.Verify
						});
					}
				}
				NrTSingleton<NrDebugConsole>.Instance.Print("Event : {0}", new object[]
				{
					eventTrigger_Game2.IsVerifyEvent()
				});
				NrTSingleton<NrDebugConsole>.Instance.Print("======================================");
				NrTSingleton<NrDebugConsole>.Instance.Print("\nStateCondition");
				NrTSingleton<NrDebugConsole>.Instance.Print("======================================");
				foreach (GameObject current2 in stateConditionList)
				{
					EventTriggerItem_StateCondition[] components = current2.GetComponents<EventTriggerItem_StateCondition>();
					if (components != null)
					{
						EventTriggerItem_StateCondition[] array4 = components;
						for (int m = 0; m < array4.Length; m++)
						{
							EventTriggerItem_StateCondition eventTriggerItem_StateCondition = array4[m];
							NrTSingleton<NrDebugConsole>.Instance.Print("{0} - State : {1}", new object[]
							{
								eventTriggerItem_StateCondition.name,
								eventTriggerItem_StateCondition.Verify()
							});
						}
					}
				}
				NrTSingleton<NrDebugConsole>.Instance.Print("State : {0}", new object[]
				{
					eventTrigger_Game2.IsVerifyState()
				});
				NrTSingleton<NrDebugConsole>.Instance.Print("======================================");
				goto IL_1E8D;
			}
			}
			NrTSingleton<NrDebugConsole>.Instance.Print("don't show anything.");
			IL_1E8D:
			return;
		}
		}
		NrTSingleton<NrDebugConsole>.Instance.Print("sorry, can not interpret command.");
	}

	private void CommandHelp()
	{
		NrTSingleton<NrDebugConsole>.Instance.Print(" help : command list");
		NrTSingleton<NrDebugConsole>.Instance.Print(" cls : clear screen");
		NrTSingleton<NrDebugConsole>.Instance.Print(" charlist : list of characters");
		NrTSingleton<NrDebugConsole>.Instance.Print(" charinfo [personid] : describe character infomations");
		NrTSingleton<NrDebugConsole>.Instance.Print(" show packetlist [count] : list of net packets");
		NrTSingleton<NrDebugConsole>.Instance.Print(" show packet [id] : detail contents of packet");
	}

	private void PrintObjectRenderStateReculsively(Transform kTrans)
	{
		for (int i = 0; i < kTrans.childCount; i++)
		{
			NrTSingleton<NrDebugConsole>.Instance.Print("GameObject is in " + kTrans.gameObject.activeInHierarchy);
			this.PrintObjectRenderStateReculsively(kTrans.GetChild(i));
		}
		Renderer[] components = kTrans.gameObject.GetComponents<Renderer>();
		Renderer[] array = components;
		for (int j = 0; j < array.Length; j++)
		{
			Renderer renderer = array[j];
			NrTSingleton<NrDebugConsole>.Instance.Print(string.Concat(new object[]
			{
				renderer.gameObject.name,
				" : Renderer(",
				renderer.enabled,
				"), Active(",
				renderer.gameObject.activeInHierarchy,
				")"
			}));
		}
	}

	private void CommandCharInfo(NrCommandInterpreter.NrCommand kCommand)
	{
		if (kCommand.m_arArg.Count < 1)
		{
			this.ErrorArgCount();
			return;
		}
		short charunique = 0;
		try
		{
			charunique = Convert.ToInt16(kCommand.m_arArg[1]);
		}
		catch (Exception)
		{
			NrTSingleton<NrDebugConsole>.Instance.Print("convert error.");
			return;
		}
		NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(charunique);
		if (charByCharUnique == null)
		{
			NrTSingleton<NrDebugConsole>.Instance.Print("not found character.");
			return;
		}
		NrTSingleton<NrDebugConsole>.Instance.Print("========= " + charByCharUnique.GetCharName() + " ======================");
		NrTSingleton<NrDebugConsole>.Instance.Print(charByCharUnique.Get3DChar().ToString());
		NrTSingleton<NrDebugConsole>.Instance.Print("Pos : " + charByCharUnique.Get3DChar().GetRootGameObject().transform.position.ToString());
		NrTSingleton<NrDebugConsole>.Instance.Print("Scale : " + charByCharUnique.Get3DChar().GetRootGameObject().transform.localScale.ToString());
		NrTSingleton<NrDebugConsole>.Instance.Print("Fade state : " + charByCharUnique.m_fCurrentAlphaFadeLerp);
		NrTSingleton<NrDebugConsole>.Instance.Print("Build step : " + charByCharUnique.m_e3DCharStep);
		this.PrintObjectRenderStateReculsively(charByCharUnique.Get3DChar().GetRootGameObject().transform);
		CharacterController component = charByCharUnique.Get3DChar().GetRootGameObject().GetComponent<CharacterController>();
		if (null != component)
		{
			NrTSingleton<NrDebugConsole>.Instance.Print("there's character controller.");
		}
		charByCharUnique.debugLog.PrintLogs();
		charByCharUnique.m_k3DChar.debugLog.PrintLogs();
	}

	private void CommandCharList()
	{
		NrCharBase[] @char = NrTSingleton<NkCharManager>.Instance.Get_Char();
		int num = 0;
		NrCharBase[] array = @char;
		for (int i = 0; i < array.Length; i++)
		{
			NrCharBase nrCharBase = array[i];
			if (nrCharBase != null)
			{
				NrTSingleton<NrDebugConsole>.Instance.Print(string.Concat(new object[]
				{
					num.ToString(),
					": ",
					nrCharBase.GetCharName(),
					"(",
					nrCharBase.GetCharUnique(),
					")"
				}));
			}
			num++;
		}
	}

	private void ShowPacketHistory(NrCommandInterpreter.NrCommand command)
	{
		ushort num = 100;
		if (command.m_arArg.Count >= 2)
		{
			try
			{
				num = Convert.ToUInt16(command.m_arArg[1]);
			}
			catch (Exception ex)
			{
				NrTSingleton<NrDebugConsole>.Instance.Print("don't convert to decimal. " + ex.Message + "\n" + ex.StackTrace);
				num = 100;
			}
		}
		List<NrPacketInfo> packetInfoList = NrTSingleton<NrPacketHistory>.Instance.GetPacketInfoList();
		if (packetInfoList.Count < (int)num)
		{
			num = (ushort)packetInfoList.Count;
		}
		for (int i = packetInfoList.Count - (int)num; i < packetInfoList.Count; i++)
		{
			NrPacketInfo nrPacketInfo = packetInfoList[i];
			NrTSingleton<NrDebugConsole>.Instance.Print(string.Concat(new string[]
			{
				nrPacketInfo.m_strTime,
				" ",
				nrPacketInfo.m_strPacketName,
				"(",
				nrPacketInfo.m_ui16UniqID.ToString(),
				")"
			}));
		}
	}

	private void ShowPacketInfo(NrCommandInterpreter.NrCommand command)
	{
		ushort ui16ID = 0;
		try
		{
			ui16ID = Convert.ToUInt16(command.m_arArg[1]);
		}
		catch
		{
			NrTSingleton<NrDebugConsole>.Instance.Print("convert error.");
		}
		NrPacketInfo packetInfo = NrTSingleton<NrPacketHistory>.Instance.GetPacketInfo(ui16ID);
		if (packetInfo == null)
		{
			NrTSingleton<NrDebugConsole>.Instance.Print("not found packet data.");
			return;
		}
		NrTSingleton<NrDebugConsole>.Instance.Print("Packet Infomation =====================================");
		NrTSingleton<NrDebugConsole>.Instance.Print("Uniq ID : " + packetInfo.m_ui16UniqID.ToString());
		NrTSingleton<NrDebugConsole>.Instance.Print("Net Time : " + packetInfo.m_strTime);
		NrTSingleton<NrDebugConsole>.Instance.Print("=======================================================");
		foreach (string current in packetInfo.m_arContents)
		{
			NrTSingleton<NrDebugConsole>.Instance.Print(current);
		}
		NrTSingleton<NrDebugConsole>.Instance.Print("=======================================================");
	}
}
