using GameMessage;
using SERVICE;
using System;
using System.Text;
using UnityEngine;

namespace UnityForms
{
	public class UIDataManager : NrTSingleton<UIDataManager>
	{
		private StringBuilder strBuilder = new StringBuilder(512);

		private static string filePath = string.Empty;

		private static string addFilePath = string.Empty;

		public static float fTotalTime = 0f;

		public static float fStageInitTotalTime = 0f;

		private static bool bMuteBGM = false;

		private static bool bMuteEffect = false;

		public string AttachEffectKeyName = "child_effect";

		public string UIBundleTag = "UI_BUNDLE";

		public bool NoticeStoryChat;

		public bool InitChar;

		private int brightness;

		public static string closeButtonName = "CloseButton";

		private bool scaleMode;

		private bool lowImage;

		public static bool MuteBGM
		{
			get
			{
				return UIDataManager.bMuteBGM;
			}
			set
			{
				UIDataManager.bMuteBGM = value;
			}
		}

		public static bool MuteEffect
		{
			get
			{
				return UIDataManager.bMuteEffect;
			}
			set
			{
				UIDataManager.bMuteEffect = value;
			}
		}

		public string FilePath
		{
			get
			{
				return UIDataManager.filePath;
			}
		}

		public string AddFilePath
		{
			get
			{
				return UIDataManager.addFilePath;
			}
		}

		public int Brightness
		{
			get
			{
				return this.brightness;
			}
			set
			{
				this.brightness = value;
			}
		}

		public bool ScaleMode
		{
			get
			{
				return this.scaleMode;
			}
		}

		public bool LowImage
		{
			get
			{
				return this.lowImage;
			}
		}

		private UIDataManager()
		{
			if (TsPlatform.IsMobile)
			{
				UIDataManager.filePath = "Mobile/";
				UIDataManager.addFilePath = "_mobile";
			}
			else
			{
				UIDataManager.filePath = "WebPlayer/";
			}
			this.lowImage = false;
		}

		public void DeleteBundle()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(this.UIBundleTag);
			if (array != null)
			{
				GameObject[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					GameObject obj = array2[i];
					UnityEngine.Object.Destroy(obj);
				}
			}
		}

		public static void MuteMiniDramaSound(bool flag)
		{
			MsgHandler.Handle("MuteMiniDramaSound", new object[]
			{
				UIDataManager.bMuteEffect,
				flag
			});
		}

		public static void MuteSound(bool flag)
		{
			MsgHandler.Handle("MuteSound", new object[]
			{
				UIDataManager.bMuteBGM,
				UIDataManager.bMuteEffect,
				flag
			});
		}

		public void Init()
		{
		}

		public StringBuilder GetStringBuilder()
		{
			return this.strBuilder;
		}

		public void InitStringBuilder()
		{
			this.strBuilder.Length = 0;
		}

		public void AppendString(string str)
		{
			this.strBuilder.Append(str);
		}

		public void AppendString(char str)
		{
			this.strBuilder.Append(str);
		}

		public string GetString()
		{
			return this.strBuilder.ToString();
		}

		public string GetString(string first, string second)
		{
			this.strBuilder.Length = 0;
			this.strBuilder.AppendFormat("{0}{1}", first, second);
			return this.strBuilder.ToString();
		}

		public string GetString(string first, string second, string third)
		{
			this.strBuilder.Length = 0;
			this.strBuilder.AppendFormat("{0}{1}{2}", first, second, third);
			return this.strBuilder.ToString();
		}

		public string GetString(string first, string second, string third, string fourth)
		{
			this.strBuilder.Length = 0;
			this.strBuilder.AppendFormat("{0}{1}{2}{3}", new object[]
			{
				first,
				second,
				third,
				fourth
			});
			return this.strBuilder.ToString();
		}

		public string GetString(string first, string second, string third, string fourth, string fifth)
		{
			this.strBuilder.Length = 0;
			this.strBuilder.AppendFormat("{0}{1}{2}{3}{4}", new object[]
			{
				first,
				second,
				third,
				fourth,
				fifth
			});
			return this.strBuilder.ToString();
		}

		public string GetString(string first, string second, string third, string fourth, string fifth, string sixth)
		{
			this.strBuilder.Length = 0;
			this.strBuilder.AppendFormat("{0}{1}{2}{3}{4}{5}", new object[]
			{
				first,
				second,
				third,
				fourth,
				fifth,
				sixth
			});
			return this.strBuilder.ToString();
		}

		public static string GetColorString(string text, int index)
		{
			if (text.Length > index + 10 && text[index] == '[' && text[index + 1] == '#' && text[index + 10] == ']')
			{
				return text.Substring(index, 11);
			}
			return string.Empty;
		}

		public static bool GetColorString(ref string strColor, string text, ref int index)
		{
			string colorString = UIDataManager.GetColorString(text, index);
			if (colorString == string.Empty)
			{
				return false;
			}
			strColor = colorString;
			index += 10;
			return true;
		}

		public static bool IsUse256Texture()
		{
			return TsPlatform.IsLowSystemMemory || NrGlobalReference.IsLiteVersion() || TsPlatform.IsIPhone;
		}

		public Vector3 GetEffectUIPos(Vector2 ScreenPos)
		{
			Vector3 result = GUICamera.ScreenToGUIPoint(ScreenPos);
			result.y = -result.y;
			result.z = UIPanelManager.EFFECT_UI_DEPTH;
			return result;
		}

		public void EffectLocateUIPos(GameObject root, IUIObject UIObject)
		{
			if (UIObject != null && null != UIObject.transform)
			{
				Vector3 localPosition = Vector3.zero;
				localPosition.z = UIPanelManager.EFFECT_UI_DEPTH;
				root.transform.parent = UIObject.transform;
				localPosition = UIObject.transform.localPosition;
				localPosition.x = (localPosition.y = 0f);
				root.transform.localPosition = localPosition;
			}
		}

		public void ResizeEffect(IUIObject control, GameObject obj)
		{
			if (obj == null)
			{
				return;
			}
			AutoSpriteControlBase autoSpriteControlBase = (AutoSpriteControlBase)control;
			if (null == autoSpriteControlBase)
			{
				return;
			}
			if (autoSpriteControlBase.GetSize().x == 115f)
			{
				obj.transform.localScale = new Vector3(1.6f, 1.6f, 1f);
				obj.transform.localPosition = new Vector3(58f, -58f, obj.transform.localPosition.z);
				Transform transform = obj.transform.FindChild("fx_aura_01");
				if (null != transform)
				{
					transform.transform.localPosition = new Vector3(58f, -58f, transform.transform.localPosition.z);
				}
			}
			else if (autoSpriteControlBase.GetSize().x == 504f && autoSpriteControlBase.GetSize().y == 448f)
			{
				obj.transform.localScale = new Vector3(0.98f, 0.98f, 1f);
				obj.transform.localPosition = new Vector3(251f, -210f, obj.transform.localPosition.z);
			}
			else if (autoSpriteControlBase.GetSize().x == 512f && autoSpriteControlBase.GetSize().y == 512f)
			{
				obj.transform.localScale = new Vector3(1f, 1.12f, 1f);
				obj.transform.localPosition = new Vector3(256f, -242f, obj.transform.localPosition.z);
			}
			else if (autoSpriteControlBase.GetSize().x == 424f && autoSpriteControlBase.GetSize().y == 432f)
			{
				obj.transform.localScale = new Vector3(0.82f, 0.95f, 1f);
				obj.transform.localPosition = new Vector3(211f, -204f, obj.transform.localPosition.z);
			}
			else if (autoSpriteControlBase.GetSize().x == 315f && autoSpriteControlBase.GetSize().y == 315f)
			{
				obj.transform.localScale = new Vector3(0.6f, 0.68f, 1f);
				obj.transform.localPosition = new Vector3(157.5f, -151f, obj.transform.localPosition.z);
			}
			else
			{
				obj.transform.localScale = new Vector3(autoSpriteControlBase.GetSize().x / 64f - 0.1f, autoSpriteControlBase.GetSize().x / 64f - 0.1f, 1f);
			}
		}

		public static bool IsFilterSpecialCharacters(string strCheckString, eSERVICE_AREA eCurrentService)
		{
			for (int i = 0; i < strCheckString.Length; i++)
			{
				if (strCheckString[i] < '0' || strCheckString[i] > '9')
				{
					if (strCheckString[i] < 'A' || strCheckString[i] > 'Z')
					{
						if (strCheckString[i] < 'a' || strCheckString[i] > 'z')
						{
							if ((eCurrentService != eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL && eCurrentService != eSERVICE_AREA.SERVICE_ANDROID_KORQA && eCurrentService != eSERVICE_AREA.SERVICE_ANDROID_KORTSTORE && eCurrentService != eSERVICE_AREA.SERVICE_ANDROID_KORGOOGLE && eCurrentService != eSERVICE_AREA.SERVICE_ANDROID_KORNAVER && eCurrentService != eSERVICE_AREA.SERVICE_ANDROID_BANDNAVER && eCurrentService != eSERVICE_AREA.SERVICE_ANDROID_BANDGOOGLE && eCurrentService != eSERVICE_AREA.SERVICE_ANDROID_KORKAKAO && eCurrentService != eSERVICE_AREA.SERVICE_ANDROID_KAKAOTSTORE && eCurrentService != eSERVICE_AREA.SERVICE_IOS_KORLOCAL && eCurrentService != eSERVICE_AREA.SERVICE_IOS_KORQA && eCurrentService != eSERVICE_AREA.SERVICE_IOS_KORAPPSTORE && eCurrentService != eSERVICE_AREA.SERVICE_IOS_KORKAKAO) || strCheckString[i] < '가' || strCheckString[i] > '힣')
							{
								if (eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNQA || eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNLOCAL || eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_CNLOCAL || eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_CNQA || eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_CNTEST || eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_CNREVIEW || eCurrentService == eSERVICE_AREA.SERVICE_IOS_CNQA || eCurrentService == eSERVICE_AREA.SERVICE_IOS_CNTEST)
								{
									if (strCheckString[i] >= '⺀' && strCheckString[i] <= '⻿')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '\u3000' && strCheckString[i] <= '〿')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '㌀' && strCheckString[i] <= '㏿')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '㐀' && strCheckString[i] <= '䶵')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '一' && strCheckString[i] <= '鿿')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '豈' && strCheckString[i] <= '﫿')
									{
										goto IL_385;
									}
								}
								if (eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_JPQA || eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_JPQALINE || eCurrentService == eSERVICE_AREA.SERVICE_ANDROID_JPLINE || eCurrentService == eSERVICE_AREA.SERVICE_IOS_JPQA || eCurrentService == eSERVICE_AREA.SERVICE_IOS_JPQALINE || eCurrentService == eSERVICE_AREA.SERVICE_IOS_JPLINE)
								{
									if (strCheckString[i] >= '぀' && strCheckString[i] <= 'ゟ')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '゠' && strCheckString[i] <= 'ヿ')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= 'ㇰ' && strCheckString[i] <= 'ㇿ')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '！' && strCheckString[i] <= 'ﾟ')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '㌀' && strCheckString[i] <= '㏿')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '㐀' && strCheckString[i] <= '䶵')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '一' && strCheckString[i] <= '鿿')
									{
										goto IL_385;
									}
									if (strCheckString[i] >= '豈' && strCheckString[i] <= '﫿')
									{
										goto IL_385;
									}
								}
								return true;
							}
						}
					}
				}
				IL_385:;
			}
			return false;
		}

		public static void SaveLineImagFile(Texture2D texture, object obj)
		{
		}
	}
}
