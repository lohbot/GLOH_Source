using GAME;
using GameMessage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace UnityForms
{
	public class EmoticonInfo
	{
		public class ChatTextInfo
		{
			public enum ChatTextInfoType
			{
				TEXT,
				EMOTICON,
				LINKTEXT,
				IMAGE,
				CHANGEFONTSIZE
			}

			public EmoticonInfo.ChatTextInfo.ChatTextInfoType type;

			public LinkText.TYPE linkTextType;

			public string normalText = string.Empty;

			public string textKey = string.Empty;

			public float fontSize;

			public float imageWidth;

			public float imageHeight;
		}

		private const string fontSizeTag = "{&";

		private const string emoticonTag = "^";

		private const string imageTag = "{$";

		private const string linkTextTag = "{@";

		private static string[] linkTextTags = new string[]
		{
			"{@I",
			"{@G",
			"{@M",
			"{@P",
			"{@N",
			"{@X",
			"{@H",
			"{@W",
			"{@C",
			"{@E",
			"{@U",
			"{@B",
			"{@F"
		};

		private static string[] jpnTokens = new string[]
		{
			"\r\n",
			"。",
			"！",
			"？",
			"が",
			"は",
			"を",
			"の",
			"か",
			"と",
			"も",
			"や",
			"に",
			"で",
			"へ",
			"から",
			"まで",
			"けど"
		};

		private static string[] globalTokens = new string[]
		{
			",",
			".",
			"!",
			"?",
			"。",
			"！",
			"？",
			"、"
		};

		private static Dictionary<int, string> emoticonTags = new Dictionary<int, string>();

		private static List<EmoticonInfo.ChatTextInfo> emoticonImageInfo = new List<EmoticonInfo.ChatTextInfo>();

		private static float EMOTICON_WIDTH = 30f;

		private static float EMOTICON_HEIGHT = 30f;

		private static float IMAGE_SIZE = 30f;

		private static string defaultColor = string.Empty;

		private static Color color = Color.white;

		private static List<IUIObject> AList = new List<IUIObject>();

		private static List<float> BList = new List<float>();

		private static List<bool> CList = new List<bool>();

		private static int newLineIndex = 0;

		private static float tallHeight = EmoticonInfo.EMOTICON_HEIGHT;

		private static Vector3 vectorValue = Vector3.zero;

		private static StringBuilder stringBuilder = new StringBuilder(256);

		private static bool bNoUserUsing = false;

		private static void InitEmotion()
		{
			EmoticonInfo.emoticonTags.Clear();
			EmoticonInfo.emoticonImageInfo.Clear();
		}

		private static void FirstParse(string str)
		{
			int num = str.IndexOf("[#");
			if (num >= 0 && str.Length > num + 10 && string.Compare(str, num, "[#", 0, 2) == 0 && str[num + 10] == ']')
			{
				int num2 = int.Parse(str.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier);
				int num3 = int.Parse(str.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier);
				int num4 = int.Parse(str.Substring(num + 6, 2), NumberStyles.AllowHexSpecifier);
				int num5 = int.Parse(str.Substring(num + 8, 2), NumberStyles.AllowHexSpecifier);
				EmoticonInfo.defaultColor = str.Substring(num, 11);
				EmoticonInfo.color = new Color((float)num2 / 255f, (float)num3 / 255f, (float)num4 / 255f, (float)num5 / 255f);
			}
			else
			{
				EmoticonInfo.defaultColor = string.Empty;
				EmoticonInfo.color = Color.white;
			}
			int num6 = 0;
			do
			{
				num6 = str.IndexOf("^", num6);
				if (num6 != -1)
				{
					EmoticonInfo.emoticonTags.Add(num6, "^");
					num6++;
				}
			}
			while (num6 != -1);
			num6 = 0;
			do
			{
				num6 = str.IndexOf("{@", num6);
				if (num6 != -1)
				{
					int num7 = str.IndexOf("}", num6);
					if (num7 == -1)
					{
						num6++;
					}
					else
					{
						EmoticonInfo.emoticonTags.Add(num6, "{@");
						num6++;
					}
				}
			}
			while (num6 != -1);
			num6 = 0;
			do
			{
				num6 = str.IndexOf("{$", num6);
				if (num6 != -1)
				{
					EmoticonInfo.emoticonTags.Add(num6, "{$");
					num6++;
				}
			}
			while (num6 != -1);
			if (EmoticonInfo.bNoUserUsing)
			{
				return;
			}
			num6 = 0;
			do
			{
				num6 = str.IndexOf("{&", num6);
				if (num6 != -1)
				{
					EmoticonInfo.emoticonTags.Add(num6, "{&");
					num6++;
				}
			}
			while (num6 != -1);
		}

		private static int AddNormalTextToEmoticonInfo(string str, int index, int strOffset, float labelFontSize)
		{
			EmoticonInfo.ChatTextInfo chatTextInfo = new EmoticonInfo.ChatTextInfo();
			chatTextInfo.type = EmoticonInfo.ChatTextInfo.ChatTextInfoType.TEXT;
			chatTextInfo.normalText = str.Substring(strOffset, index - strOffset);
			chatTextInfo.fontSize = labelFontSize;
			if (string.Empty != chatTextInfo.normalText)
			{
				EmoticonInfo.emoticonImageInfo.Add(chatTextInfo);
			}
			return chatTextInfo.normalText.Length;
		}

		private static bool EmoticonParse(string str, float fontSize, ref float tallFontSize)
		{
			bool result = false;
			int num = 0;
			float num2 = fontSize;
			List<int> list = new List<int>(EmoticonInfo.emoticonTags.Keys);
			list.Sort();
			Dictionary<string, UIEmoticonInfo> uIEmoticonDictionary = NrTSingleton<UIEmoticonManager>.Instance.UIEmoticonDictionary;
			foreach (int current in list)
			{
				if ("^" == EmoticonInfo.emoticonTags[current])
				{
					foreach (string current2 in uIEmoticonDictionary.Keys)
					{
						if (current <= str.Length - current2.Length)
						{
							if (string.Compare(str, current, current2, 0, current2.Length) == 0)
							{
								EmoticonInfo.AddNormalTextToEmoticonInfo(str, current, num, num2);
								EmoticonInfo.ChatTextInfo chatTextInfo = new EmoticonInfo.ChatTextInfo();
								chatTextInfo.type = EmoticonInfo.ChatTextInfo.ChatTextInfoType.EMOTICON;
								chatTextInfo.normalText = current2;
								EmoticonInfo.emoticonImageInfo.Add(chatTextInfo);
								if (str.Length > current + current2.Length + 8 && str[current + current2.Length] == '(' && str[current + current2.Length + 8] == ')')
								{
									string[] array = str.Substring(current + current2.Length + 1, 7).Split(new char[]
									{
										','
									});
									chatTextInfo.imageWidth = float.Parse(array[0]);
									chatTextInfo.imageHeight = float.Parse(array[1]);
									num = current + current2.Length + 9;
									break;
								}
								result = true;
								num = current + current2.Length;
								break;
							}
						}
					}
				}
				else if ("{@" == EmoticonInfo.emoticonTags[current])
				{
					for (int i = 0; i < EmoticonInfo.linkTextTags.Length; i++)
					{
						if (string.Compare(str, current, EmoticonInfo.linkTextTags[i], 0, EmoticonInfo.linkTextTags[i].Length) == 0)
						{
							if (current - num >= 0)
							{
								int num3 = EmoticonInfo.AddNormalTextToEmoticonInfo(str, current, num, num2);
								num += num3 + EmoticonInfo.linkTextTags[i].Length;
								int num4 = str.IndexOf('}', current) - num;
								if (num4 < 0)
								{
									num4 = str.Length - num;
								}
								string text = str.Substring(num, num4);
								string[] array2 = text.Split(new char[]
								{
									','
								});
								EmoticonInfo.ChatTextInfo chatTextInfo2 = new EmoticonInfo.ChatTextInfo();
								chatTextInfo2.type = EmoticonInfo.ChatTextInfo.ChatTextInfoType.LINKTEXT;
								chatTextInfo2.normalText = array2[0];
								if (array2.Length > 1)
								{
									chatTextInfo2.textKey = array2[1];
								}
								chatTextInfo2.linkTextType = i + LinkText.TYPE.ITEM;
								chatTextInfo2.fontSize = num2;
								if (string.Empty != chatTextInfo2.normalText)
								{
									EmoticonInfo.emoticonImageInfo.Add(chatTextInfo2);
								}
								num += num4 + 1;
								break;
							}
						}
					}
				}
				else if ("{$" == EmoticonInfo.emoticonTags[current])
				{
					int num5 = EmoticonInfo.AddNormalTextToEmoticonInfo(str, current, num, num2);
					num += num5 + "{$".Length;
					int num4 = str.IndexOf('}', current) - num;
					if (num4 < 0)
					{
						num4 = str.Length - num;
					}
					string normalText = str.Substring(num, num4);
					EmoticonInfo.ChatTextInfo chatTextInfo3 = new EmoticonInfo.ChatTextInfo();
					chatTextInfo3.type = EmoticonInfo.ChatTextInfo.ChatTextInfoType.IMAGE;
					chatTextInfo3.normalText = normalText;
					EmoticonInfo.emoticonImageInfo.Add(chatTextInfo3);
					if (str.Length > num4 + num + 9 && str[num4 + num + 1] == '(' && str[num4 + num + 9] == ')')
					{
						string[] array3 = str.Substring(num4 + num + 2, 7).Split(new char[]
						{
							','
						});
						chatTextInfo3.imageWidth = float.Parse(array3[0]);
						chatTextInfo3.imageHeight = float.Parse(array3[1]);
						num += num4 + 10;
					}
					else
					{
						num += num4 + 1;
					}
				}
				else if ("{&" == EmoticonInfo.emoticonTags[current])
				{
					int num6 = EmoticonInfo.AddNormalTextToEmoticonInfo(str, current, num, num2);
					num += num6 + "{&".Length;
					int num4 = str.IndexOf('}', current) - num;
					if (num4 < 0)
					{
						num4 = str.Length - num;
					}
					string s = str.Substring(num, num4);
					num2 = float.Parse(s);
					if (num2 > tallFontSize)
					{
						tallFontSize = num2;
					}
					num += num4 + 1;
				}
			}
			if (num < str.Length && str[num] != '\0')
			{
				EmoticonInfo.ChatTextInfo chatTextInfo4 = new EmoticonInfo.ChatTextInfo();
				chatTextInfo4.type = EmoticonInfo.ChatTextInfo.ChatTextInfoType.TEXT;
				chatTextInfo4.normalText = str.Substring(num, str.Length - num);
				chatTextInfo4.fontSize = num2;
				EmoticonInfo.emoticonImageInfo.Add(chatTextInfo4);
			}
			return result;
		}

		private static void AddChild(ref UIListItemContainer container, string str, bool haveEmoticon, float lineWidth, float fontSize, SpriteText.Font_Effect fontEffect, ITEM linkItem)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < EmoticonInfo.emoticonImageInfo.Count; i++)
			{
				if (EmoticonInfo.emoticonImageInfo[i] != null)
				{
					EmoticonInfo.ChatTextInfo chatTextInfo = EmoticonInfo.emoticonImageInfo[i];
					if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.TEXT)
					{
						if (EmoticonInfo.emoticonImageInfo.Count == 1)
						{
							Label label = UICreateControl.Label("Text", chatTextInfo.normalText, true, lineWidth, chatTextInfo.fontSize, fontEffect, SpriteText.Anchor_Pos.Upper_Left, SpriteText.Alignment_Type.Left, EmoticonInfo.color);
							EmoticonInfo.vectorValue.x = num;
							EmoticonInfo.vectorValue.y = 0f;
							EmoticonInfo.vectorValue.z = 0f;
							container.MakeChild(label.gameObject);
							label.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
						}
						else
						{
							string empty = string.Empty;
							int num4 = 0;
							do
							{
								Label label2 = UICreateControl.Label("Text", chatTextInfo.normalText, false, 0f, chatTextInfo.fontSize, fontEffect, SpriteText.Anchor_Pos.Upper_Left, SpriteText.Alignment_Type.Left, EmoticonInfo.color);
								EmoticonInfo.vectorValue.y = ((!haveEmoticon) ? num3 : (num3 + num2));
								EmoticonInfo.vectorValue.x = num;
								EmoticonInfo.vectorValue.z = 0f;
								label2.Text = EmoticonInfo.GetOneLineText(chatTextInfo.normalText, ref num4, label2, ref num, lineWidth - fontSize, ref empty);
								container.MakeChild(label2.gameObject);
								label2.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
								if (chatTextInfo.normalText.Length > num4 || num >= lineWidth - fontSize)
								{
									num = 0f;
									num3 -= ((!haveEmoticon) ? fontSize : fontSize);
								}
							}
							while (chatTextInfo.normalText.Length > num4);
						}
					}
					else if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.EMOTICON)
					{
						Emoticon emoticon = UICreateControl.Emoticon("Emoticon", fontSize, fontSize, chatTextInfo.normalText);
						emoticon.AutoAnimatorStop = false;
						container.MakeChild(emoticon.gameObject);
						if (num + fontSize > lineWidth)
						{
							num = 0f;
							num3 -= fontSize;
						}
						EmoticonInfo.vectorValue.x = num;
						EmoticonInfo.vectorValue.y = num3;
						EmoticonInfo.vectorValue.z = 0f;
						emoticon.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
						num += fontSize;
					}
					else if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.LINKTEXT)
					{
						LinkText linkText = UICreateControl.LinkText("LinkText", chatTextInfo, linkItem, EmoticonInfo.color, EmoticonInfo.defaultColor, fontEffect);
						if (null == linkText)
						{
							return;
						}
						container.MakeChild(linkText.gameObject);
						if (num + linkText.spriteText.TotalWidth > lineWidth)
						{
							num = 0f;
							if (haveEmoticon)
							{
								num3 -= fontSize;
							}
							else
							{
								num3 -= fontSize;
							}
						}
						EmoticonInfo.vectorValue.x = num;
						EmoticonInfo.vectorValue.y = ((!haveEmoticon) ? num3 : (num3 + num2));
						EmoticonInfo.vectorValue.z = 0f;
						linkText.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
						num += linkText.spriteText.TotalWidth;
					}
					else if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.IMAGE)
					{
						DrawTexture drawTexture = UICreateControl.DrawTexture("IMAGE", chatTextInfo.normalText, EmoticonInfo.IMAGE_SIZE, EmoticonInfo.IMAGE_SIZE);
						if (null == drawTexture)
						{
							return;
						}
						container.MakeChild(drawTexture.gameObject);
						if (num + drawTexture.width > lineWidth)
						{
							num = 0f;
							if (haveEmoticon)
							{
								num3 -= fontSize;
							}
							else
							{
								num3 -= fontSize;
							}
						}
						EmoticonInfo.vectorValue.x = num;
						EmoticonInfo.vectorValue.y = ((!haveEmoticon) ? num3 : (num3 + num2));
						EmoticonInfo.vectorValue.z = 0f;
						drawTexture.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
						num += drawTexture.width;
					}
				}
			}
		}

		private static string GetOneLineText(string strOrigin, ref int strOffset, Label label, ref float curX, float maxWidth, ref string colorString)
		{
			int num = strOffset;
			int i = strOffset;
			EmoticonInfo.stringBuilder.Length = 0;
			while (i < strOrigin.Length)
			{
				if (strOrigin[i] == '\n')
				{
					break;
				}
				i++;
			}
			label.spriteText.Text = strOrigin.Substring(num, i - num);
			if (curX + label.spriteText.GetTextWidth() <= maxWidth)
			{
				strOffset = i;
			}
			else
			{
				while (strOffset < i)
				{
					if (!UIDataManager.GetColorString(ref colorString, strOrigin, ref strOffset))
					{
						EmoticonInfo.stringBuilder.Append(strOrigin[strOffset]);
						label.Text = EmoticonInfo.stringBuilder.ToString();
						if (curX + label.spriteText.GetTextWidth() > maxWidth)
						{
							break;
						}
					}
					strOffset++;
				}
			}
			curX += label.spriteText.TotalWidth;
			string stringToTextColor = EmoticonInfo.GetStringToTextColor(ref colorString, strOrigin, num, strOffset);
			if (strOrigin.Length > strOffset && strOrigin[strOffset] == '\n')
			{
				strOffset++;
			}
			return stringToTextColor;
		}

		private static string GetStringToTextColor(ref string colorString, string str, int startOffset, int endOffset)
		{
			if (startOffset == endOffset)
			{
				return string.Empty;
			}
			if (colorString == string.Empty || colorString == string.Empty)
			{
				return str.Substring(startOffset, endOffset - startOffset);
			}
			string colorString2 = UIDataManager.GetColorString(str, startOffset);
			if (colorString2 != string.Empty || colorString2 == string.Empty)
			{
				colorString = colorString2;
				return str.Substring(startOffset, endOffset - startOffset);
			}
			EmoticonInfo.stringBuilder.Length = 0;
			EmoticonInfo.stringBuilder.Append(colorString);
			EmoticonInfo.stringBuilder.Append(str, startOffset, endOffset - startOffset);
			return EmoticonInfo.stringBuilder.ToString();
		}

		public static UIListItemContainer ParseEmoticon(string str, float lineWidth, float fontSize, SpriteText.Font_Effect fontEffect, ITEM linkItem, bool bNo)
		{
			EmoticonInfo.bNoUserUsing = bNo;
			return EmoticonInfo.ParseEmoticon(str, lineWidth, fontSize, fontEffect, linkItem);
		}

		public static UIListItemContainer ParseEmoticon(string str, float lineWidth, float fontSize, SpriteText.Font_Effect fontEffect, ITEM linkItem)
		{
			GameObject gameObject = new GameObject("EmoticonLine");
			UIListItemContainer uIListItemContainer = gameObject.AddComponent<UIListItemContainer>();
			EmoticonInfo.InitEmotion();
			EmoticonInfo.FirstParse(str);
			if (EmoticonInfo.emoticonTags.Count > 0)
			{
				bool haveEmoticon = EmoticonInfo.EmoticonParse(str, fontSize, ref fontSize);
				EmoticonInfo.AddChild(ref uIListItemContainer, str, haveEmoticon, lineWidth, fontSize, fontEffect, linkItem);
			}
			else
			{
				Label label;
				if (str.Contains("\r\n"))
				{
					string str2 = str.Replace("\r\n", " ");
					label = UICreateControl.Label("Text", str2, true, lineWidth, fontSize, fontEffect, SpriteText.Anchor_Pos.Upper_Left, SpriteText.Alignment_Type.Left, new Color(255f, 255f, 255f));
				}
				else
				{
					label = UICreateControl.Label("Text", str, true, lineWidth, fontSize, fontEffect, SpriteText.Anchor_Pos.Upper_Left, SpriteText.Alignment_Type.Left, new Color(255f, 255f, 255f));
				}
				uIListItemContainer.MakeChild(label.gameObject);
				EmoticonInfo.vectorValue.x = 0f;
				EmoticonInfo.vectorValue.y = 0f;
				EmoticonInfo.vectorValue.z = 0f;
				label.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
			}
			EmoticonInfo.bNoUserUsing = false;
			return uIListItemContainer;
		}

		public static bool ParseEmoticonFlashLabel(ref UIListItemContainer container, string str, float lineWidth, float height, int fontSize, float lineSpacing, SpriteText.Anchor_Pos anchor, string mainColor)
		{
			EmoticonInfo.InitEmotion();
			EmoticonInfo.FirstParse(str);
			EmoticonInfo.defaultColor = mainColor;
			bool result = false;
			if (EmoticonInfo.emoticonTags.Count >= 0)
			{
				float num = (float)fontSize;
				EmoticonInfo.EmoticonParse(str, (float)fontSize, ref num);
				int num2 = 0;
				float num3 = 0f;
				float num4 = 0f;
				float num5 = 0f;
				float num6 = lineSpacing * (float)fontSize;
				EmoticonInfo.newLineIndex = 0;
				EmoticonInfo.AList.Clear();
				EmoticonInfo.BList.Clear();
				EmoticonInfo.CList.Clear();
				for (int i = 0; i < EmoticonInfo.emoticonImageInfo.Count; i++)
				{
					EmoticonInfo.ChatTextInfo chatTextInfo = EmoticonInfo.emoticonImageInfo[i];
					if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.TEXT)
					{
						EmoticonInfo.ReLabel(ref num2, chatTextInfo.fontSize, chatTextInfo.normalText, ref container, ref num3, ref num4, ref num5, lineWidth, ref num6, ref EmoticonInfo.AList, ref EmoticonInfo.BList, ref EmoticonInfo.CList, EmoticonInfo.defaultColor, lineSpacing);
					}
					else if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.EMOTICON)
					{
						result = true;
						float num7 = EmoticonInfo.EMOTICON_WIDTH;
						if (chatTextInfo.imageWidth > 0f)
						{
							num7 = chatTextInfo.imageWidth;
						}
						float num8 = EmoticonInfo.EMOTICON_HEIGHT;
						if (chatTextInfo.imageHeight > 0f)
						{
							num8 = chatTextInfo.imageHeight;
						}
						Emoticon emoticon = UICreateControl.Emoticon("Emoticon", num7, num8, chatTextInfo.normalText);
						container.MakeChild(emoticon.gameObject);
						if (num8 > num6)
						{
							num6 = num8;
						}
						if (EmoticonInfo.AList.Count > 0)
						{
							for (int j = EmoticonInfo.AList.Count - 1; j >= 0; j--)
							{
								if (j == EmoticonInfo.newLineIndex - 1)
								{
									break;
								}
								if (!EmoticonInfo.CList[j])
								{
									if (EmoticonInfo.AList[j] is Label)
									{
										Vector3 position = EmoticonInfo.AList[j].gameObject.transform.position;
										float num9 = num6 - (float)fontSize;
										position.y = num4 + num5 - num9;
										EmoticonInfo.AList[j].gameObject.transform.position = position;
									}
									else if (EmoticonInfo.AList[j] is LinkText)
									{
										Vector3 localPosition = EmoticonInfo.AList[j].gameObject.transform.localPosition;
										float num10 = num6 - (float)fontSize;
										localPosition.y = num4 + num5 - num10;
										EmoticonInfo.AList[j].gameObject.transform.localPosition = localPosition;
									}
									else if (EmoticonInfo.AList[j] is DrawTexture && num8 > ((DrawTexture)EmoticonInfo.AList[j]).height)
									{
										Vector3 localPosition2 = EmoticonInfo.AList[j].gameObject.transform.localPosition;
										float num11 = num6 - ((DrawTexture)EmoticonInfo.AList[j]).height;
										localPosition2.y = num4 + num5 - num11;
										EmoticonInfo.AList[j].gameObject.transform.localPosition = localPosition2;
									}
								}
							}
						}
						EmoticonInfo.AList.Add(emoticon);
						EmoticonInfo.BList.Add(num7);
						EmoticonInfo.CList.Add(false);
						if (num3 + num7 > lineWidth)
						{
							num3 = 0f;
							num5 -= num6;
							num6 = num8;
							EmoticonInfo.vectorValue.x = num3;
							EmoticonInfo.vectorValue.y = num5;
							EmoticonInfo.vectorValue.z = 0f;
							emoticon.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
						}
						else
						{
							EmoticonInfo.vectorValue.x = num3;
							EmoticonInfo.vectorValue.y = num5 - (num6 - num8);
							EmoticonInfo.vectorValue.z = 0f;
							emoticon.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
						}
						num3 += num7;
						container.Data = new Vector2(num3, num5);
					}
					else if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.LINKTEXT)
					{
						LinkText linkText = LinkText.Create("LINKTEXT" + num2++, Vector3.zero);
						linkText.UpdateText = true;
						linkText.gameObject.layer = GUICamera.UILayer;
						linkText.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
						linkText.linkTextType = chatTextInfo.linkTextType;
						linkText.CreateSpriteText();
						linkText.MultiLine = false;
						UIBaseInfoLoader uIBaseInfoLoader;
						if (linkText.linkTextType == LinkText.TYPE.PLAYER)
						{
							linkText.ColorText = EmoticonInfo.defaultColor;
							uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary("LINKTEXT", "Com_B_LinkText");
						}
						else
						{
							if (chatTextInfo.linkTextType == LinkText.TYPE.COUPON)
							{
								linkText.ColorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1204"
								});
								linkText.NormalColorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1204"
								});
								linkText.OverColorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1204"
								});
							}
							else if (container.FontEffect == SpriteText.Font_Effect.White_Shadow_Small)
							{
								linkText.ColorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1206"
								});
								linkText.NormalColorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1206"
								});
								linkText.OverColorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1207"
								});
							}
							else
							{
								linkText.ColorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1107"
								});
								linkText.NormalColorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1107"
								});
								linkText.OverColorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1107"
								});
							}
							uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary("LINKTEXT", "Com_B_LinkText");
						}
						linkText.BaseString = chatTextInfo.normalText;
						linkText.Text = linkText.ColorText + chatTextInfo.normalText;
						linkText.textKey = chatTextInfo.textKey;
						linkText.SetFontEffect(container.FontEffect);
						linkText.SetCharacterSize(chatTextInfo.fontSize);
						linkText.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
						Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
						linkText.Setup(linkText.spriteText.TotalWidth, linkText.spriteText.TotalHeight, material);
						float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
						Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
						for (int k = 0; k < 4; k++)
						{
							linkText.States[k].spriteFrames = new CSpriteFrame[1];
							linkText.States[k].spriteFrames[0] = new CSpriteFrame();
							if ((int)uIBaseInfoLoader.ButtonCount <= k)
							{
								linkText.States[k].spriteFrames[0].uvs = uvs;
							}
							else
							{
								uvs.x += pixelToUVsWidth;
								linkText.States[k].spriteFrames[0].uvs = uvs;
							}
							linkText.animations[k].SetAnim(linkText.States[k], k);
						}
						linkText.autoResize = false;
						linkText.PlayAnim(0, 0);
						container.MakeChild(linkText.gameObject);
						EmoticonInfo.AList.Add(linkText);
						EmoticonInfo.BList.Add(linkText.spriteText.TotalWidth);
						EmoticonInfo.CList.Add(false);
						float num12 = num6 - (float)fontSize;
						if (num3 + linkText.spriteText.TotalWidth > lineWidth)
						{
							num5 -= num6;
							num6 = (float)fontSize * lineSpacing;
							num3 = 0f;
						}
						EmoticonInfo.vectorValue.x = num3;
						EmoticonInfo.vectorValue.y = num4 + num5 - num12;
						EmoticonInfo.vectorValue.z = 0f;
						linkText.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
						num3 += linkText.spriteText.TotalWidth;
						container.Data = new Vector2(num3, num5);
					}
					else if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.IMAGE)
					{
						UIBaseInfoLoader uIBaseInfoLoader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(chatTextInfo.normalText);
						if (uIBaseInfoLoader2 != null)
						{
							float num13 = uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount;
							if (chatTextInfo.imageWidth > 0f)
							{
								num13 = chatTextInfo.imageWidth;
							}
							float num14 = uIBaseInfoLoader2.UVs.height;
							if (chatTextInfo.imageHeight > 0f)
							{
								num14 = chatTextInfo.imageHeight;
							}
							if ((float)fontSize > num14)
							{
								num14 = (float)fontSize;
							}
							DrawTexture drawTexture = UICreateControl.DrawTexture("IMAGE", chatTextInfo.normalText, num13, num14);
							if (null != drawTexture)
							{
								container.MakeChild(drawTexture.gameObject);
								if (num14 > num6)
								{
									num6 = num14;
								}
								if (EmoticonInfo.AList.Count > 0 && num14 > (float)fontSize)
								{
									for (int l = EmoticonInfo.AList.Count - 1; l >= 0; l--)
									{
										if (l == EmoticonInfo.newLineIndex - 1)
										{
											break;
										}
										if (EmoticonInfo.CList[l])
										{
											break;
										}
										if (EmoticonInfo.AList[l] is Label)
										{
											Vector3 position2 = EmoticonInfo.AList[l].gameObject.transform.position;
											float num15 = num6 - (float)fontSize;
											position2.y = num4 + num5 - num15;
											EmoticonInfo.AList[l].gameObject.transform.position = position2;
										}
										else if (EmoticonInfo.AList[l] is LinkText)
										{
											Vector3 localPosition3 = EmoticonInfo.AList[l].gameObject.transform.localPosition;
											float num16 = num6 - (float)fontSize;
											localPosition3.y = num4 + num5 - num16;
											EmoticonInfo.AList[l].gameObject.transform.localPosition = localPosition3;
										}
										else if (EmoticonInfo.AList[l] is Emoticon)
										{
											if (num14 > ((Emoticon)EmoticonInfo.AList[l]).height)
											{
												Vector3 localPosition4 = EmoticonInfo.AList[l].gameObject.transform.localPosition;
												float num17 = num6 - ((Emoticon)EmoticonInfo.AList[l]).height;
												localPosition4.y = num4 + num5 - num17;
												EmoticonInfo.AList[l].gameObject.transform.localPosition = localPosition4;
											}
										}
										else if (EmoticonInfo.AList[l] is DrawTexture && num14 > ((DrawTexture)EmoticonInfo.AList[l]).height)
										{
											Vector3 localPosition5 = EmoticonInfo.AList[l].gameObject.transform.localPosition;
											float num18 = num6 - ((DrawTexture)EmoticonInfo.AList[l]).height;
											localPosition5.y = num4 + num5 - num18;
											EmoticonInfo.AList[l].gameObject.transform.localPosition = localPosition5;
										}
									}
								}
								EmoticonInfo.AList.Add(drawTexture);
								EmoticonInfo.BList.Add(drawTexture.width);
								EmoticonInfo.CList.Add(false);
								if (num3 + num13 > lineWidth)
								{
									num3 = 0f;
									num5 -= num6;
									num6 = num14;
									EmoticonInfo.vectorValue.y = num5;
								}
								else
								{
									EmoticonInfo.vectorValue.y = num5 - (num6 - num14);
								}
								EmoticonInfo.vectorValue.x = num3;
								EmoticonInfo.vectorValue.z = 0f;
								drawTexture.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
								num3 += drawTexture.width;
								container.Data = new Vector2(num3, num5);
							}
						}
					}
				}
				int num19 = 0;
				float num20 = 0f;
				float num21;
				for (int m = 0; m < EmoticonInfo.BList.Count; m++)
				{
					if (num20 + EmoticonInfo.BList[m] > lineWidth || EmoticonInfo.CList[m])
					{
						if (anchor == SpriteText.Anchor_Pos.Upper_Left || anchor == SpriteText.Anchor_Pos.Middle_Left || anchor == SpriteText.Anchor_Pos.Lower_Left)
						{
							num21 = 0f;
						}
						else if (anchor == SpriteText.Anchor_Pos.Upper_Center || anchor == SpriteText.Anchor_Pos.Lower_Center || anchor == SpriteText.Anchor_Pos.Middle_Center)
						{
							num21 = (lineWidth - num20) / 2f;
							if (num21 < 0f)
							{
								num21 = 0f;
							}
						}
						else
						{
							num21 = lineWidth - num20;
							if (num21 < 0f)
							{
								num21 = 0f;
							}
						}
						Vector3 localPosition6 = Vector3.zero;
						for (int n = num19; n < m; n++)
						{
							if (EmoticonInfo.AList[n] is Label)
							{
								localPosition6 = EmoticonInfo.AList[n].transform.position;
							}
							else
							{
								localPosition6 = EmoticonInfo.AList[n].transform.localPosition;
							}
							localPosition6.x += num21;
							EmoticonInfo.AList[n].transform.localPosition = localPosition6;
						}
						num20 = 0f;
						num19 = m;
					}
					num20 += EmoticonInfo.BList[m];
				}
				if (anchor == SpriteText.Anchor_Pos.Upper_Left || anchor == SpriteText.Anchor_Pos.Middle_Left || anchor == SpriteText.Anchor_Pos.Lower_Left)
				{
					num21 = 0f;
				}
				else if (anchor == SpriteText.Anchor_Pos.Upper_Center || anchor == SpriteText.Anchor_Pos.Lower_Center || anchor == SpriteText.Anchor_Pos.Middle_Center)
				{
					num21 = (lineWidth - num20) / 2f;
					if (num21 < 0f)
					{
						num21 = 0f;
					}
				}
				else
				{
					num21 = lineWidth - num20;
					if (num21 < 0f)
					{
						num21 = 0f;
					}
				}
				Vector3 localPosition7 = Vector3.zero;
				for (int num22 = num19; num22 < EmoticonInfo.BList.Count; num22++)
				{
					if (EmoticonInfo.AList[num22] is Label)
					{
						localPosition7 = EmoticonInfo.AList[num22].transform.position;
					}
					else
					{
						localPosition7 = EmoticonInfo.AList[num22].transform.localPosition;
					}
					localPosition7.x += num21;
					EmoticonInfo.AList[num22].transform.localPosition = localPosition7;
				}
				if (anchor == SpriteText.Anchor_Pos.Lower_Left || anchor == SpriteText.Anchor_Pos.Lower_Center || anchor == SpriteText.Anchor_Pos.Lower_Right)
				{
					float num23 = -num5 + EmoticonInfo.tallHeight * lineSpacing;
					num23 -= height;
					Vector3 localPosition8 = Vector3.zero;
					for (int num24 = 0; num24 < EmoticonInfo.AList.Count; num24++)
					{
						localPosition8 = EmoticonInfo.AList[num24].transform.localPosition;
						localPosition8.y += num23;
						EmoticonInfo.AList[num24].transform.localPosition = localPosition8;
					}
				}
				else if (anchor == SpriteText.Anchor_Pos.Middle_Left || anchor == SpriteText.Anchor_Pos.Middle_Center || anchor == SpriteText.Anchor_Pos.Middle_Right)
				{
					float num23 = -num5 + EmoticonInfo.tallHeight * lineSpacing;
					num23 -= height;
					num23 /= 2f;
					Vector3 localPosition9 = Vector3.zero;
					for (int num25 = 0; num25 < EmoticonInfo.AList.Count; num25++)
					{
						localPosition9 = EmoticonInfo.AList[num25].transform.localPosition;
						localPosition9.y += num23;
						EmoticonInfo.AList[num25].transform.localPosition = localPosition9;
					}
				}
			}
			else
			{
				GameObject gameObject = new GameObject("Text");
				Label label = gameObject.AddComponent<Label>();
				label.gameObject.layer = GUICamera.UILayer;
				label.CreateSpriteText();
				label.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
				label.SetAlignment(SpriteText.Alignment_Type.Center);
				label.MultiLine = true;
				label.MaxWidth = lineWidth;
				label.SetCharacterSize((float)fontSize);
				label.SetFontEffect(container.FontEffect);
				label.Text = str;
				container.MakeChild(label.gameObject);
				label.transform.rotation = container.transform.rotation;
				EmoticonInfo.vectorValue.x = 0f;
				EmoticonInfo.vectorValue.y = 0f;
				EmoticonInfo.vectorValue.z = 0f;
				label.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
				container.Data = new Vector2(label.spriteText.lastLineWidth, label.spriteText.lastLinePosY);
			}
			return result;
		}

		public static void EngReLabel(ref int count, float fontSize, string text, ref UIListItemContainer container, ref float totallineWidth, ref float textPosY, ref float emoticonPosY, float lineWidth, ref float flinespacing, ref List<IUIObject> AList, ref List<float> BList, ref List<bool> CList, string mainColor, float lineSpacing)
		{
			if (text == "\r\n")
			{
				GameObject gameObject = new GameObject("Text");
				Label label = gameObject.AddComponent<Label>();
				label.Visible = false;
				container.MakeChild(label.gameObject);
				AList.Add(label);
				BList.Add(0f);
				CList.Add(true);
				emoticonPosY -= flinespacing;
				flinespacing = lineSpacing * fontSize;
				totallineWidth = 0f;
				EmoticonInfo.newLineIndex = AList.Count;
				return;
			}
			GameObject gameObject2 = new GameObject("Text");
			Label label2 = gameObject2.AddComponent<Label>();
			label2.gameObject.layer = GUICamera.UILayer;
			label2.CreateSpriteText();
			label2.ColorText = mainColor;
			label2.SetFontEffect(container.FontEffect);
			label2.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			label2.SetAlignment(SpriteText.Alignment_Type.Left);
			label2.SetCharacterSize(fontSize);
			label2.MultiLine = true;
			label2.Text = text;
			float num = flinespacing - fontSize;
			EmoticonInfo.vectorValue.x = totallineWidth;
			EmoticonInfo.vectorValue.y = textPosY + emoticonPosY - num;
			EmoticonInfo.vectorValue.z = 0f;
			label2.gameObject.transform.position = EmoticonInfo.vectorValue;
			float num2 = label2.spriteText.TotalWidth;
			if (totallineWidth + num2 > lineWidth)
			{
				emoticonPosY -= flinespacing;
				flinespacing = lineSpacing * fontSize;
				char[] separator = new char[]
				{
					' '
				};
				string[] array = text.Split(separator);
				label2.Text = string.Empty;
				int num3 = 0;
				for (int i = 0; i < array.Length; i++)
				{
					Label expr_182 = label2;
					expr_182.Text = expr_182.Text + array[i] + " ";
					num3 += array[i].Length + 1;
					if (array[i] == "\r\n")
					{
						label2.Text = text.Substring(0, num3 - array[i].Length - 1);
						num2 = label2.spriteText.TotalWidth;
						container.MakeChild(label2.gameObject);
						totallineWidth = 0f;
						AList.Add(label2);
						BList.Add(num2);
						CList.Add(false);
						for (int j = 0; j < label2.Text.Length; j++)
						{
							UIDataManager.GetColorString(ref mainColor, text, ref j);
						}
						string text2 = string.Empty;
						for (int k = i + 1; k < array.Length; k++)
						{
							text2 = text2 + array[k] + " ";
						}
						if (text2[0] == ' ')
						{
							text2 = text2.Remove(0, 1);
						}
						EmoticonInfo.EngReLabel(ref count, fontSize, text2, ref container, ref totallineWidth, ref textPosY, ref emoticonPosY, lineWidth, ref flinespacing, ref AList, ref BList, ref CList, mainColor, lineSpacing);
						return;
					}
					num2 = label2.spriteText.TotalWidth;
					if (totallineWidth + num2 > lineWidth)
					{
						label2.Text = text.Substring(0, num3 - array[i].Length - 1);
						num2 = label2.spriteText.TotalWidth;
						container.MakeChild(label2.gameObject);
						totallineWidth = 0f;
						AList.Add(label2);
						BList.Add(num2);
						CList.Add(false);
						for (int l = 0; l < label2.Text.Length; l++)
						{
							UIDataManager.GetColorString(ref mainColor, text, ref l);
						}
						string text3 = string.Empty;
						for (int m = i; m < array.Length; m++)
						{
							text3 = text3 + array[m] + " ";
						}
						if (text3[0] == ' ')
						{
							text3 = text3.Remove(0, 1);
						}
						EmoticonInfo.EngReLabel(ref count, fontSize, text3, ref container, ref totallineWidth, ref textPosY, ref emoticonPosY, lineWidth, ref flinespacing, ref AList, ref BList, ref CList, mainColor, lineSpacing);
						return;
					}
				}
			}
			else
			{
				if (text.Contains("\r\n"))
				{
					string a = string.Empty;
					for (int n = 0; n < text.Length; n++)
					{
						if (n + 1 < text.Length)
						{
							a = text.Substring(n, 2);
							if (a == "\r\n")
							{
								if (n == 0)
								{
									emoticonPosY -= flinespacing;
									flinespacing = lineSpacing * fontSize;
									label2.Text = " ";
									num2 = 0f;
								}
								else
								{
									emoticonPosY -= flinespacing;
									flinespacing = lineSpacing * fontSize;
									label2.Text = text.Substring(0, n);
									num2 = label2.spriteText.TotalWidth;
								}
								container.MakeChild(label2.gameObject);
								totallineWidth = 0f;
								AList.Add(label2);
								BList.Add(num2);
								CList.Add(false);
								GameObject gameObject3 = new GameObject("Text");
								Label label3 = gameObject3.AddComponent<Label>();
								label3.Visible = false;
								container.MakeChild(label3.gameObject);
								AList.Add(label3);
								BList.Add(0f);
								CList.Add(true);
								EmoticonInfo.newLineIndex = AList.Count;
								if (n + 2 < text.Length)
								{
									string text4 = text.Substring(n + 2);
									if (text4[0] == ' ')
									{
										text4 = text4.Remove(0, 1);
									}
									EmoticonInfo.EngReLabel(ref count, fontSize, text4, ref container, ref totallineWidth, ref textPosY, ref emoticonPosY, lineWidth, ref flinespacing, ref AList, ref BList, ref CList, mainColor, lineSpacing);
								}
								return;
							}
						}
					}
				}
				container.MakeChild(label2.gameObject);
				totallineWidth += num2;
				container.Data = new Vector2(totallineWidth, emoticonPosY);
				AList.Add(label2);
				BList.Add(num2);
				CList.Add(false);
			}
		}

		public static void JpnReLabel(ref int count, float fontSize, string text, ref UIListItemContainer container, ref float totallineWidth, ref float textPosY, ref float emoticonPosY, float lineWidth, ref float flinespacing, ref List<IUIObject> AList, ref List<float> BList, ref List<bool> CList, string mainColor, float lineSpacing)
		{
			if (text == "\r\n")
			{
				GameObject gameObject = new GameObject("Text");
				Label label = gameObject.AddComponent<Label>();
				label.Visible = false;
				container.MakeChild(label.gameObject);
				AList.Add(label);
				BList.Add(0f);
				CList.Add(true);
				emoticonPosY -= flinespacing;
				flinespacing = lineSpacing * fontSize;
				totallineWidth = 0f;
				EmoticonInfo.newLineIndex = AList.Count;
				return;
			}
			GameObject gameObject2 = new GameObject("Text");
			Label label2 = gameObject2.AddComponent<Label>();
			label2.gameObject.layer = GUICamera.UILayer;
			label2.CreateSpriteText();
			label2.ColorText = mainColor;
			label2.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			label2.SetAlignment(SpriteText.Alignment_Type.Left);
			label2.MultiLine = true;
			label2.Text = text;
			label2.SetFontEffect(container.FontEffect);
			label2.SetCharacterSize(fontSize);
			float num = flinespacing - fontSize;
			EmoticonInfo.vectorValue.x = totallineWidth;
			EmoticonInfo.vectorValue.y = textPosY + emoticonPosY - num;
			EmoticonInfo.vectorValue.z = 0f;
			label2.gameObject.transform.position = EmoticonInfo.vectorValue;
			float num2 = label2.spriteText.TotalWidth;
			if (totallineWidth + num2 > lineWidth)
			{
				emoticonPosY -= flinespacing;
				flinespacing = lineSpacing * fontSize;
				char[] separator = new char[]
				{
					'|'
				};
				string text2 = text;
				for (int i = 0; i < EmoticonInfo.jpnTokens.Length; i++)
				{
					text2 = text2.Replace(EmoticonInfo.jpnTokens[i], EmoticonInfo.jpnTokens[i] + "|");
				}
				string[] array = text2.Split(separator);
				label2.Text = string.Empty;
				int num3 = 0;
				for (int j = 0; j < array.Length; j++)
				{
					Label expr_1C5 = label2;
					expr_1C5.Text += array[j];
					num3 += array[j].Length;
					if (array[j] == "\r\n")
					{
						label2.Text = text.Substring(0, num3 - array[j].Length);
						num2 = label2.spriteText.TotalWidth;
						container.MakeChild(label2.gameObject);
						totallineWidth = 0f;
						AList.Add(label2);
						BList.Add(num2);
						CList.Add(false);
						for (int k = 0; k < label2.Text.Length; k++)
						{
							UIDataManager.GetColorString(ref mainColor, text, ref k);
						}
						return;
					}
					num2 = label2.spriteText.TotalWidth;
					if (totallineWidth + num2 > lineWidth)
					{
						label2.Text = text.Substring(0, num3 - array[j].Length);
						num2 = label2.spriteText.TotalWidth;
						container.MakeChild(label2.gameObject);
						totallineWidth = 0f;
						AList.Add(label2);
						BList.Add(num2);
						CList.Add(false);
						for (int l = 0; l < label2.Text.Length; l++)
						{
							UIDataManager.GetColorString(ref mainColor, text, ref l);
						}
						string text3 = string.Empty;
						for (int m = j; m < array.Length; m++)
						{
							text3 += array[m];
						}
						if (text3[0] == ' ')
						{
							text3 = text3.Remove(0, 1);
						}
						EmoticonInfo.JpnReLabel(ref count, fontSize, text3, ref container, ref totallineWidth, ref textPosY, ref emoticonPosY, lineWidth, ref flinespacing, ref AList, ref BList, ref CList, mainColor, lineSpacing);
						return;
					}
				}
			}
			else
			{
				if (text.Contains("\r\n"))
				{
					string a = string.Empty;
					for (int n = 0; n < text.Length; n++)
					{
						if (n + 1 < text.Length)
						{
							a = text.Substring(n, 2);
							if (a == "\r\n")
							{
								if (n == 0)
								{
									emoticonPosY -= flinespacing;
									flinespacing = lineSpacing * fontSize;
									label2.Text = " ";
									num2 = 0f;
								}
								else
								{
									emoticonPosY -= flinespacing;
									flinespacing = lineSpacing * fontSize;
									label2.Text = text.Substring(0, n);
									num2 = label2.spriteText.TotalWidth;
								}
								container.MakeChild(label2.gameObject);
								totallineWidth = 0f;
								AList.Add(label2);
								BList.Add(num2);
								CList.Add(false);
								GameObject gameObject3 = new GameObject("Text");
								Label label3 = gameObject3.AddComponent<Label>();
								label3.Visible = false;
								container.MakeChild(label3.gameObject);
								AList.Add(label3);
								BList.Add(0f);
								CList.Add(true);
								EmoticonInfo.newLineIndex = AList.Count;
								if (n + 2 < text.Length)
								{
									string text4 = text.Substring(n + 2);
									if (text4[0] == ' ')
									{
										text4 = text4.Remove(0, 1);
									}
									EmoticonInfo.JpnReLabel(ref count, fontSize, text4, ref container, ref totallineWidth, ref textPosY, ref emoticonPosY, lineWidth, ref flinespacing, ref AList, ref BList, ref CList, mainColor, lineSpacing);
								}
								return;
							}
						}
					}
				}
				container.MakeChild(label2.gameObject);
				totallineWidth += num2;
				container.Data = new Vector2(totallineWidth, emoticonPosY);
				AList.Add(label2);
				BList.Add(num2);
				CList.Add(false);
			}
		}

		public static void ReLabel(ref int count, float fontSize, string text, ref UIListItemContainer container, ref float totallineWidth, ref float textPosY, ref float emoticonPosY, float lineWidth, ref float flinespacing, ref List<IUIObject> AList, ref List<float> BList, ref List<bool> CList, string mainColor, float lineSpacing)
		{
			if (text == "\r\n")
			{
				GameObject gameObject = new GameObject("Text");
				Label label = gameObject.AddComponent<Label>();
				label.Visible = false;
				container.MakeChild(label.gameObject);
				AList.Add(label);
				BList.Add(0f);
				CList.Add(true);
				emoticonPosY -= flinespacing;
				flinespacing = lineSpacing * fontSize;
				totallineWidth = 0f;
				EmoticonInfo.newLineIndex = AList.Count;
				return;
			}
			GameObject gameObject2 = new GameObject("Text");
			Label label2 = gameObject2.AddComponent<Label>();
			label2.gameObject.layer = GUICamera.UILayer;
			label2.BackGroundHide(true);
			label2.CreateSpriteText();
			label2.ColorText = mainColor;
			label2.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			label2.SetAlignment(SpriteText.Alignment_Type.Left);
			label2.MultiLine = true;
			label2.RenderEnabled = true;
			label2.Text = text;
			label2.SetFontEffect(container.FontEffect);
			label2.SetCharacterSize(fontSize);
			float num = flinespacing - fontSize;
			EmoticonInfo.vectorValue.x = totallineWidth;
			EmoticonInfo.vectorValue.y = textPosY + emoticonPosY - num;
			EmoticonInfo.vectorValue.z = 0f;
			label2.gameObject.transform.position = EmoticonInfo.vectorValue;
			float num2 = label2.spriteText.TotalWidth;
			if (totallineWidth + num2 > lineWidth)
			{
				emoticonPosY -= flinespacing;
				flinespacing = lineSpacing * fontSize;
				string a = string.Empty;
				for (int i = 0; i < text.Length; i++)
				{
					label2.Text = text.Substring(0, i + 1);
					if (i + 1 < text.Length)
					{
						a = text.Substring(i, 2);
						if (a == "\r\n")
						{
							label2.Text = text.Substring(0, i);
							container.MakeChild(label2.gameObject);
							totallineWidth = 0f;
							AList.Add(label2);
							BList.Add(num2);
							CList.Add(false);
							GameObject gameObject3 = new GameObject("Text");
							Label label3 = gameObject3.AddComponent<Label>();
							label3.Visible = false;
							container.MakeChild(label3.gameObject);
							AList.Add(label3);
							BList.Add(0f);
							CList.Add(true);
							EmoticonInfo.newLineIndex = AList.Count;
							string text2 = text.Substring(i + 2);
							if (text2[0] == ' ')
							{
								text2 = text2.Remove(0, 1);
							}
							EmoticonInfo.ReLabel(ref count, fontSize, text2, ref container, ref totallineWidth, ref textPosY, ref emoticonPosY, lineWidth, ref flinespacing, ref AList, ref BList, ref CList, mainColor, lineSpacing);
							return;
						}
					}
					bool flag = false;
					num2 = label2.spriteText.TotalWidth;
					if (0 < label2.Text.Length)
					{
						string a2 = string.Empty;
						a2 = label2.Text[label2.Text.Length - 1].ToString();
						for (int j = 0; j < EmoticonInfo.globalTokens.Length; j++)
						{
							if (a2 == EmoticonInfo.globalTokens[j])
							{
								flag = true;
								num2 -= fontSize;
								break;
							}
						}
					}
					if (totallineWidth + num2 > lineWidth)
					{
						label2.Text = text.Substring(0, i);
						num2 = label2.spriteText.TotalWidth;
						container.MakeChild(label2.gameObject);
						totallineWidth = 0f;
						AList.Add(label2);
						BList.Add(num2);
						CList.Add(false);
						for (int k = 0; k < label2.Text.Length; k++)
						{
							UIDataManager.GetColorString(ref mainColor, text, ref k);
						}
						string text3 = text.Substring(i);
						if (text3[0] == ' ')
						{
							text3 = text3.Remove(0, 1);
						}
						EmoticonInfo.ReLabel(ref count, fontSize, text3, ref container, ref totallineWidth, ref textPosY, ref emoticonPosY, lineWidth, ref flinespacing, ref AList, ref BList, ref CList, mainColor, lineSpacing);
						return;
					}
					if (flag && i == text.Length - 1)
					{
						label2.Text = text.Substring(0, i);
						num2 = label2.spriteText.TotalWidth;
						container.MakeChild(label2.gameObject);
						totallineWidth = 0f;
						AList.Add(label2);
						BList.Add(num2);
						CList.Add(false);
						for (int l = 0; l < label2.Text.Length; l++)
						{
							UIDataManager.GetColorString(ref mainColor, text, ref l);
						}
						string text4 = text.Substring(i);
						if (text4[0] == ' ')
						{
							text4 = text4.Remove(0, 1);
						}
						EmoticonInfo.ReLabel(ref count, fontSize, text4, ref container, ref totallineWidth, ref textPosY, ref emoticonPosY, lineWidth, ref flinespacing, ref AList, ref BList, ref CList, mainColor, lineSpacing);
						return;
					}
				}
			}
			else
			{
				string a3 = string.Empty;
				for (int m = 0; m < text.Length; m++)
				{
					if (m + 1 < text.Length)
					{
						a3 = text.Substring(m, 2);
						if (a3 == "\r\n")
						{
							if (m == 0)
							{
								emoticonPosY -= flinespacing;
								flinespacing = lineSpacing * fontSize;
								label2.Text = " ";
								num2 = 0f;
							}
							else
							{
								emoticonPosY -= flinespacing;
								flinespacing = lineSpacing * fontSize;
								label2.Text = text.Substring(0, m);
								num2 = label2.spriteText.TotalWidth;
							}
							container.MakeChild(label2.gameObject);
							totallineWidth = 0f;
							AList.Add(label2);
							BList.Add(num2);
							CList.Add(false);
							GameObject gameObject4 = new GameObject("Text");
							Label label4 = gameObject4.AddComponent<Label>();
							label4.Visible = false;
							container.MakeChild(label4.gameObject);
							AList.Add(label4);
							BList.Add(0f);
							CList.Add(true);
							EmoticonInfo.newLineIndex = AList.Count;
							if (m + 2 < text.Length)
							{
								string text5 = text.Substring(m + 2);
								if (text5[0] == ' ')
								{
									text5 = text5.Remove(0, 1);
								}
								EmoticonInfo.ReLabel(ref count, fontSize, text5, ref container, ref totallineWidth, ref textPosY, ref emoticonPosY, lineWidth, ref flinespacing, ref AList, ref BList, ref CList, mainColor, lineSpacing);
							}
							return;
						}
					}
				}
				container.MakeChild(label2.gameObject);
				totallineWidth += num2;
				container.Data = new Vector2(totallineWidth, emoticonPosY);
				AList.Add(label2);
				BList.Add(num2);
				CList.Add(false);
			}
		}

		public static void ParseAdEmoticon(ref UIListItemContainer container, string str, float emoticonScale, ref float yPos)
		{
			if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
			{
				str = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
				{
					str
				});
			}
			container.AutoAnimatorStop = false;
			string[] array = str.Split(new char[]
			{
				'\n'
			});
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			int num7 = 0;
			float num8 = 0f;
			string empty = string.Empty;
			float fontSize = container.FontSize;
			float maxWidth = 300f;
			int num9 = 0;
			UIDataManager.GetColorString(ref empty, str, ref num9);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				num7++;
				EmoticonInfo.InitEmotion();
				EmoticonInfo.FirstParse(text);
				num4 = 0f;
				if (EmoticonInfo.emoticonTags.Count > 0)
				{
					float fontSize2 = container.FontSize;
					bool flag = EmoticonInfo.EmoticonParse(text, container.FontSize, ref fontSize2);
					float num10 = 0f;
					foreach (EmoticonInfo.ChatTextInfo current in EmoticonInfo.emoticonImageInfo)
					{
						if (current.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.TEXT || current.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.LINKTEXT)
						{
							GameObject gameObject = new GameObject("Text");
							Label label = gameObject.AddComponent<Label>();
							label.CreateSpriteText();
							label.gameObject.layer = TsLayer.PC;
							label.ColorText = empty;
							label.spriteText.useWhiteSpace = false;
							label.spriteText.characterSize = container.FontSize;
							label.Text = current.normalText;
							label.SetFontEffect(container.FontEffect);
							label.spriteText.gameObject.layer = TsLayer.PC;
							label.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
							label.spriteText.SetAlignment(SpriteText.Alignment_Type.Left);
							container.MakeChild(label.gameObject);
							label.transform.rotation = container.transform.rotation;
							EmoticonInfo.vectorValue.x = 1f;
							EmoticonInfo.vectorValue.y = 1f;
							EmoticonInfo.vectorValue.z = 1f;
							label.transform.localScale = EmoticonInfo.vectorValue;
							EmoticonInfo.vectorValue.x = emoticonScale;
							EmoticonInfo.vectorValue.y = emoticonScale;
							EmoticonInfo.vectorValue.z = 1f;
							label.spriteText.transform.localScale = EmoticonInfo.vectorValue;
							if (flag)
							{
								EmoticonInfo.vectorValue.x = num10;
								EmoticonInfo.vectorValue.y = num2;
								EmoticonInfo.vectorValue.z = 0f;
								label.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
							}
							else
							{
								EmoticonInfo.vectorValue.x = num10;
								EmoticonInfo.vectorValue.y = num2;
								EmoticonInfo.vectorValue.z = 0f;
								label.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
							}
							num10 += label.spriteText.TotalWidth * emoticonScale;
							num4 += label.spriteText.TotalWidth;
							if (!flag)
							{
								num3 = label.spriteText.TotalHeight * emoticonScale;
								num8 = label.spriteText.TotalHeight * emoticonScale;
							}
						}
						else if (current.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.EMOTICON)
						{
							Emoticon emoticon = UICreateControl.EmoticonCharChat("Emoticon", fontSize, fontSize, current.normalText);
							emoticon.gameObject.layer = TsLayer.PC;
							emoticon.autoResize = false;
							container.MakeChild(emoticon.gameObject);
							emoticon.transform.rotation = container.transform.rotation;
							EmoticonInfo.vectorValue.x = emoticonScale;
							EmoticonInfo.vectorValue.y = emoticonScale;
							EmoticonInfo.vectorValue.z = 1f;
							emoticon.gameObject.transform.localScale = EmoticonInfo.vectorValue;
							EmoticonInfo.vectorValue.x = num10;
							EmoticonInfo.vectorValue.y = num2;
							EmoticonInfo.vectorValue.z = -0.2f;
							emoticon.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
							num10 += fontSize * emoticonScale;
							num4 += fontSize;
							num3 = fontSize * emoticonScale;
							num5 = fontSize * emoticonScale - container.FontSize * emoticonScale;
							num8 = fontSize * emoticonScale;
						}
						else if (current.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.IMAGE)
						{
							DrawTexture drawTexture = UICreateControl.DrawTexture("IMAGE", current.normalText, EmoticonInfo.IMAGE_SIZE, EmoticonInfo.IMAGE_SIZE);
							drawTexture.gameObject.layer = TsLayer.PC;
							if (null != drawTexture)
							{
								container.MakeChild(drawTexture.gameObject);
								drawTexture.transform.rotation = container.transform.rotation;
								EmoticonInfo.vectorValue.x = emoticonScale;
								EmoticonInfo.vectorValue.y = emoticonScale;
								EmoticonInfo.vectorValue.z = 1f;
								drawTexture.gameObject.transform.localScale = EmoticonInfo.vectorValue;
								if (flag)
								{
									EmoticonInfo.vectorValue.x = num10;
									EmoticonInfo.vectorValue.y = num2;
									EmoticonInfo.vectorValue.z = 0f;
									drawTexture.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
								}
								else
								{
									EmoticonInfo.vectorValue.x = num10;
									EmoticonInfo.vectorValue.y = num2;
									EmoticonInfo.vectorValue.z = 0f;
									drawTexture.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
								}
								num10 += drawTexture.width * emoticonScale;
								num4 += drawTexture.width;
								if (!flag)
								{
									num3 = drawTexture.height * emoticonScale;
									num8 = drawTexture.height * emoticonScale;
								}
							}
						}
					}
				}
				else
				{
					GameObject gameObject2 = new GameObject("Text");
					Label label2 = gameObject2.AddComponent<Label>();
					label2.CreateSpriteText();
					label2.spriteText.useWhiteSpace = false;
					label2.spriteText.multiline = true;
					label2.spriteText.maxWidth = maxWidth;
					label2.gameObject.layer = TsLayer.PC;
					label2.ColorText = empty;
					label2.spriteText.characterSize = container.FontSize;
					label2.Text = text;
					label2.SetFontEffect(container.FontEffect);
					label2.spriteText.gameObject.layer = TsLayer.PC;
					label2.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					label2.spriteText.SetAlignment(SpriteText.Alignment_Type.Left);
					container.MakeChild(label2.gameObject);
					label2.transform.rotation = container.transform.rotation;
					EmoticonInfo.vectorValue.x = 1f;
					EmoticonInfo.vectorValue.y = 1f;
					EmoticonInfo.vectorValue.z = 1f;
					label2.transform.localScale = EmoticonInfo.vectorValue;
					EmoticonInfo.vectorValue.x = emoticonScale;
					EmoticonInfo.vectorValue.y = emoticonScale;
					EmoticonInfo.vectorValue.z = 1f;
					label2.spriteText.transform.localScale = EmoticonInfo.vectorValue;
					EmoticonInfo.vectorValue.x = 0f;
					EmoticonInfo.vectorValue.y = num2;
					EmoticonInfo.vectorValue.z = 0f;
					label2.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
					num4 = label2.spriteText.TotalWidth;
					num3 = label2.spriteText.TotalHeight * emoticonScale;
					num8 = label2.spriteText.TotalHeight * emoticonScale;
				}
				if (num < num4)
				{
					num = num4;
				}
				num6 += num5;
				num2 -= num3;
				yPos += num8;
			}
			num2 *= -1f;
			container.Data = new Vector2(num, num2 * 100f / (emoticonScale * 100f));
			EmoticonInfo.vectorValue.x = 0f;
			EmoticonInfo.vectorValue.y = 0f;
			EmoticonInfo.vectorValue.z = -1f;
			container.transform.localPosition = EmoticonInfo.vectorValue;
			yPos = (yPos + 20f * emoticonScale) / 2f;
		}

		public static void OtherFlashLabel(ref UIListItemContainer container, string str, ref float fwidth, ref float fheight, int fontSize, float lineSpacing, SpriteText.Anchor_Pos anchor, string mainColor)
		{
			float num = fwidth;
			EmoticonInfo.InitEmotion();
			EmoticonInfo.FirstParse(str);
			if (string.Empty != mainColor)
			{
				EmoticonInfo.defaultColor = mainColor;
			}
			if (EmoticonInfo.emoticonTags.Count >= 0)
			{
				float num2 = (float)fontSize;
				EmoticonInfo.EmoticonParse(str, (float)fontSize, ref num2);
				int num3 = 0;
				float num4 = 0f;
				float num5 = 0f;
				float num6 = 0f;
				float num7 = lineSpacing * (float)fontSize;
				List<IUIObject> list = new List<IUIObject>();
				List<float> list2 = new List<float>();
				List<bool> list3 = new List<bool>();
				for (int i = 0; i < EmoticonInfo.emoticonImageInfo.Count; i++)
				{
					EmoticonInfo.ChatTextInfo chatTextInfo = EmoticonInfo.emoticonImageInfo[i];
					if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.TEXT)
					{
						EmoticonInfo.ReLabel(ref num3, chatTextInfo.fontSize, chatTextInfo.normalText, ref container, ref num4, ref num5, ref num6, num, ref num7, ref list, ref list2, ref list3, EmoticonInfo.defaultColor, lineSpacing);
					}
					else if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.EMOTICON)
					{
						Emoticon emoticon = UICreateControl.Emoticon("Emoticon", EmoticonInfo.EMOTICON_WIDTH, EmoticonInfo.EMOTICON_HEIGHT, chatTextInfo.normalText);
						container.MakeChild(emoticon.gameObject);
						if (list.Count > 0)
						{
							for (int j = list.Count - 1; j >= 0; j--)
							{
								if (list3[j])
								{
									break;
								}
								if (list[j] is Label)
								{
									Vector3 position = list[j].gameObject.transform.position;
									float num8 = EmoticonInfo.EMOTICON_HEIGHT - (float)fontSize;
									position.y = num5 + num6 - num8;
									list[j].gameObject.transform.position = position;
								}
								else
								{
									if (!(list[j] is LinkText))
									{
										break;
									}
									Vector3 localPosition = list[j].gameObject.transform.localPosition;
									float num9 = EmoticonInfo.EMOTICON_HEIGHT - (float)fontSize;
									localPosition.y = num5 + num6 - num9;
									list[j].gameObject.transform.localPosition = localPosition;
								}
							}
						}
						list.Add(emoticon);
						list2.Add(EmoticonInfo.EMOTICON_WIDTH);
						list3.Add(false);
						emoticon.transform.rotation = container.transform.rotation;
						if (num4 + EmoticonInfo.EMOTICON_WIDTH > num)
						{
							num4 = 0f;
							if (num7 > EmoticonInfo.EMOTICON_HEIGHT)
							{
								num6 -= num7;
							}
							else
							{
								num6 -= EmoticonInfo.EMOTICON_HEIGHT;
							}
							num7 = (float)fontSize * lineSpacing;
							EmoticonInfo.vectorValue.x = num4;
							EmoticonInfo.vectorValue.y = num6;
							EmoticonInfo.vectorValue.z = 0f;
							emoticon.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
						}
						else
						{
							if (num7 > EmoticonInfo.EMOTICON_HEIGHT)
							{
								num7 = (float)fontSize * lineSpacing;
							}
							else
							{
								num7 = EmoticonInfo.EMOTICON_HEIGHT;
							}
							EmoticonInfo.vectorValue.x = num4;
							EmoticonInfo.vectorValue.y = num6;
							EmoticonInfo.vectorValue.z = 0f;
							emoticon.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
						}
						num4 += EmoticonInfo.EMOTICON_WIDTH;
					}
					else if (chatTextInfo.type == EmoticonInfo.ChatTextInfo.ChatTextInfoType.LINKTEXT)
					{
						LinkText linkText = LinkText.Create("LINKTEXT" + num3++, Vector3.zero);
						linkText.UpdateText = true;
						linkText.gameObject.layer = GUICamera.UILayer;
						linkText.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
						linkText.linkTextType = chatTextInfo.linkTextType;
						linkText.CreateSpriteText();
						linkText.spriteText.multiline = false;
						UIBaseInfoLoader uIBaseInfoLoader;
						if (linkText.linkTextType == LinkText.TYPE.PLAYER)
						{
							linkText.ColorText = EmoticonInfo.defaultColor;
							uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary("LINKTEXT", "Com_B_LinkText");
						}
						else
						{
							if (chatTextInfo.linkTextType == LinkText.TYPE.COUPON)
							{
								string text = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1306"
								});
								linkText.ColorText = text;
								linkText.NormalColorText = text;
								linkText.OverColorText = text;
							}
							else if (container.FontEffect == SpriteText.Font_Effect.White_Shadow_Small)
							{
								string text2 = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1206"
								});
								linkText.ColorText = text2;
								linkText.NormalColorText = text2;
								linkText.OverColorText = text2;
							}
							else
							{
								string text3 = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
								{
									"1107"
								});
								linkText.ColorText = text3;
								linkText.NormalColorText = text3;
								linkText.OverColorText = text3;
							}
							uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary("LINKTEXT", "Com_B_LinkText");
						}
						if (uIBaseInfoLoader != null)
						{
							linkText.BaseString = chatTextInfo.normalText;
							linkText.Text = linkText.ColorText + chatTextInfo.normalText;
							linkText.textKey = chatTextInfo.textKey;
							linkText.SetFontEffect(container.FontEffect);
							linkText.SetCharacterSize(chatTextInfo.fontSize);
							linkText.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
							Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
							linkText.Setup(linkText.spriteText.TotalWidth, linkText.spriteText.TotalHeight, material);
							float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
							Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
							Rect uvs = new Rect(rect);
							uvs.x += pixelToUVsWidth;
							for (int k = 0; k < 4; k++)
							{
								linkText.States[k].spriteFrames = new CSpriteFrame[1];
								linkText.States[k].spriteFrames[0] = new CSpriteFrame();
								rect.x += pixelToUVsWidth;
								if ((int)uIBaseInfoLoader.ButtonCount <= k)
								{
									linkText.States[k].spriteFrames[0].uvs = uvs;
								}
								else
								{
									linkText.States[k].spriteFrames[0].uvs = rect;
								}
								linkText.animations[k].SetAnim(linkText.States[k], k);
							}
							linkText.autoResize = false;
							linkText.PlayAnim(0, 0);
							container.MakeChild(linkText.gameObject);
							list.Add(linkText);
							list2.Add(linkText.spriteText.TotalWidth);
							list3.Add(false);
							float num10 = num7 - (float)fontSize;
							if (num4 + linkText.spriteText.TotalWidth > num)
							{
								num6 -= num7;
								num7 = (float)fontSize * lineSpacing;
								num4 = 0f;
								EmoticonInfo.vectorValue.x = num4;
								EmoticonInfo.vectorValue.y = num5 + num6 - num10;
								EmoticonInfo.vectorValue.z = 0f;
								linkText.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
							}
							else
							{
								EmoticonInfo.vectorValue.x = num4;
								EmoticonInfo.vectorValue.y = num5 + num6 - num10;
								EmoticonInfo.vectorValue.z = 0f;
								linkText.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
							}
							num4 += linkText.spriteText.TotalWidth;
						}
					}
				}
				int num11 = 0;
				float num12 = 0f;
				for (int l = 0; l < list2.Count; l++)
				{
					if (list3[l] && num12 > fwidth)
					{
						fwidth = num12;
						num12 = 0f;
					}
					num12 += list2[l];
				}
				if (num12 > fwidth)
				{
					fwidth = num12;
				}
				fheight = -num6 + EmoticonInfo.EMOTICON_HEIGHT * lineSpacing;
				float num13;
				for (int m = 0; m < list2.Count; m++)
				{
					if (list3[m])
					{
						if (anchor == SpriteText.Anchor_Pos.Upper_Left || anchor == SpriteText.Anchor_Pos.Middle_Left || anchor == SpriteText.Anchor_Pos.Lower_Left)
						{
							num13 = 0f;
						}
						else if (anchor == SpriteText.Anchor_Pos.Upper_Center || anchor == SpriteText.Anchor_Pos.Lower_Center || anchor == SpriteText.Anchor_Pos.Middle_Center)
						{
							num13 = (fwidth - num12) / 2f;
							if (num13 < 0f)
							{
								num13 = 0f;
							}
						}
						else
						{
							num13 = fwidth - num12;
							if (num13 < 0f)
							{
								num13 = 0f;
							}
						}
						Vector3 localPosition2 = Vector3.zero;
						for (int n = num11; n < m; n++)
						{
							if (list[n] is Label)
							{
								localPosition2 = list[n].transform.position;
							}
							else
							{
								localPosition2 = list[n].transform.localPosition;
							}
							localPosition2.x += num13;
							list[n].transform.localPosition = localPosition2;
						}
						num12 = 0f;
						num11 = m;
					}
					num12 += list2[m];
				}
				if (anchor == SpriteText.Anchor_Pos.Upper_Left || anchor == SpriteText.Anchor_Pos.Middle_Left || anchor == SpriteText.Anchor_Pos.Lower_Left)
				{
					num13 = 0f;
				}
				else if (anchor == SpriteText.Anchor_Pos.Upper_Center || anchor == SpriteText.Anchor_Pos.Lower_Center || anchor == SpriteText.Anchor_Pos.Middle_Center)
				{
					num13 = (fwidth - num12) / 2f;
					if (num13 < 0f)
					{
						num13 = 0f;
					}
				}
				else
				{
					num13 = fwidth - num12;
					if (num13 < 0f)
					{
						num13 = 0f;
					}
				}
				Vector3 localPosition3 = Vector3.zero;
				for (int num14 = num11; num14 < list2.Count; num14++)
				{
					if (list[num14] is Label)
					{
						localPosition3 = list[num14].transform.position;
					}
					else
					{
						localPosition3 = list[num14].transform.localPosition;
					}
					localPosition3.x += num13;
					list[num14].transform.localPosition = localPosition3;
				}
				if (anchor == SpriteText.Anchor_Pos.Lower_Left || anchor == SpriteText.Anchor_Pos.Lower_Center || anchor == SpriteText.Anchor_Pos.Lower_Right)
				{
					float num15 = -num6 + EmoticonInfo.EMOTICON_HEIGHT * lineSpacing;
					num15 -= fheight;
					Vector3 localPosition4 = Vector3.zero;
					for (int num16 = 0; num16 < list.Count; num16++)
					{
						localPosition4 = list[num16].transform.localPosition;
						localPosition4.y += num15;
						list[num16].transform.localPosition = localPosition4;
					}
				}
				else if (anchor == SpriteText.Anchor_Pos.Middle_Left || anchor == SpriteText.Anchor_Pos.Middle_Center || anchor == SpriteText.Anchor_Pos.Middle_Right)
				{
					float num15 = -num6 + EmoticonInfo.EMOTICON_HEIGHT * lineSpacing;
					num15 -= fheight;
					num15 /= 2f;
					Vector3 localPosition5 = Vector3.zero;
					for (int num17 = 0; num17 < list.Count; num17++)
					{
						localPosition5 = list[num17].transform.localPosition;
						localPosition5.y += num15;
						list[num17].transform.localPosition = localPosition5;
					}
				}
			}
			else
			{
				GameObject gameObject = new GameObject("Text");
				Label label = gameObject.AddComponent<Label>();
				label.gameObject.layer = GUICamera.UILayer;
				label.CreateSpriteText();
				label.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
				if (anchor == SpriteText.Anchor_Pos.Upper_Left || anchor == SpriteText.Anchor_Pos.Middle_Left || anchor == SpriteText.Anchor_Pos.Lower_Left)
				{
					label.SetAnchor(anchor);
					label.SetAlignment(SpriteText.Alignment_Type.Left);
				}
				else if (anchor == SpriteText.Anchor_Pos.Upper_Center || anchor == SpriteText.Anchor_Pos.Middle_Center || anchor == SpriteText.Anchor_Pos.Lower_Center)
				{
					label.SetAnchor(anchor);
					label.SetAlignment(SpriteText.Alignment_Type.Center);
				}
				else if (anchor == SpriteText.Anchor_Pos.Upper_Right || anchor == SpriteText.Anchor_Pos.Middle_Right || anchor == SpriteText.Anchor_Pos.Lower_Right)
				{
					label.SetAnchor(anchor);
					label.SetAlignment(SpriteText.Alignment_Type.Right);
				}
				label.MultiLine = true;
				label.MaxWidth = num;
				label.SetFontEffect(container.FontEffect);
				label.Text = str;
				container.MakeChild(label.gameObject);
				label.transform.rotation = container.transform.rotation;
				EmoticonInfo.vectorValue.x = 0f;
				EmoticonInfo.vectorValue.y = 0f;
				EmoticonInfo.vectorValue.z = 0f;
				label.gameObject.transform.localPosition = EmoticonInfo.vectorValue;
			}
		}
	}
}
