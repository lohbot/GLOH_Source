using GAME;
using GameMessage;
using System;
using Ts;
using UnityEngine;

namespace UnityForms
{
	public class UICreateControl
	{
		public static DrawTexture DrawTexture(string name, string imageKey)
		{
			DrawTexture result;
			using (new ScopeProfile("UICreateControl - DrawTexture"))
			{
				UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, imageKey);
				if (uIBaseInfoLoader == null)
				{
					result = null;
				}
				else
				{
					GameObject gameObject = new GameObject(name);
					DrawTexture drawTexture = gameObject.AddComponent<DrawTexture>();
					drawTexture.autoResize = false;
					drawTexture.gameObject.layer = GUICamera.UILayer;
					drawTexture.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					drawTexture.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
					drawTexture.m_bPattern = uIBaseInfoLoader.Pattern;
					Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
					drawTexture.Setup(uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height, material);
					drawTexture.SetTextureUVs(new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height));
					drawTexture.Start();
					drawTexture.UsedCollider(true);
					result = drawTexture;
				}
			}
			return result;
		}

		public static DrawTexture DrawTexture(string name, string imageKey, float width, float height)
		{
			DrawTexture result;
			using (new ScopeProfile("UICreateControl - DrawTexture"))
			{
				UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, imageKey);
				if (uIBaseInfoLoader == null)
				{
					result = null;
				}
				else
				{
					GameObject gameObject = new GameObject(name);
					DrawTexture drawTexture = gameObject.AddComponent<DrawTexture>();
					drawTexture.autoResize = false;
					drawTexture.gameObject.layer = GUICamera.UILayer;
					drawTexture.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					drawTexture.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
					drawTexture.m_bPattern = uIBaseInfoLoader.Pattern;
					Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
					drawTexture.Setup(width, height, material);
					drawTexture.SetTextureUVs(new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height));
					drawTexture.Start();
					drawTexture.UsedCollider(true);
					result = drawTexture;
				}
			}
			return result;
		}

		public static DrawTexture DrawTexture(string name, string imageKey, float width, float height, bool useBox)
		{
			DrawTexture result;
			using (new ScopeProfile("UICreateControl - DrawTexture"))
			{
				UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, imageKey);
				if (uIBaseInfoLoader == null)
				{
					result = null;
				}
				else
				{
					GameObject gameObject = new GameObject(name);
					DrawTexture drawTexture = gameObject.AddComponent<DrawTexture>();
					drawTexture.autoResize = false;
					drawTexture.gameObject.layer = GUICamera.UILayer;
					drawTexture.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					drawTexture.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
					drawTexture.m_bPattern = uIBaseInfoLoader.Pattern;
					Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
					drawTexture.Setup(width, height, material);
					drawTexture.SetTextureUVs(new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height));
					drawTexture.Start();
					drawTexture.UsedCollider(useBox);
					result = drawTexture;
				}
			}
			return result;
		}

		public static ItemTexture ItemTexture(string name, float width, float height, bool useBox)
		{
			ItemTexture result;
			using (new ScopeProfile("UICreateControl - ItemTexture"))
			{
				GameObject gameObject = new GameObject(name);
				ItemTexture itemTexture = gameObject.AddComponent<ItemTexture>();
				itemTexture.autoResize = false;
				itemTexture.gameObject.layer = GUICamera.UILayer;
				itemTexture.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
				itemTexture.width = width;
				itemTexture.height = height;
				itemTexture.Start();
				itemTexture.UsedCollider(useBox);
				result = itemTexture;
			}
			return result;
		}

		public static UIButton Button(string name, string imageKey, float width, float height)
		{
			UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, imageKey);
			if (uIBaseInfoLoader == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject(name);
			UIButton uIButton = gameObject.AddComponent<UIButton>();
			uIButton.gameObject.layer = GUICamera.UILayer;
			uIButton.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			uIButton.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			uIButton.Setup(width, height, material);
			float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
			Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
			Rect uvs = new Rect(rect);
			uvs.x += pixelToUVsWidth;
			for (int i = 0; i < (int)uIBaseInfoLoader.ButtonCount; i++)
			{
				uIButton.States[i].spriteFrames = new CSpriteFrame[1];
				uIButton.States[i].spriteFrames[0] = new CSpriteFrame();
				rect.x += pixelToUVsWidth;
				if ((int)uIBaseInfoLoader.ButtonCount <= i)
				{
					uIButton.States[i].spriteFrames[0].uvs = uvs;
				}
				else
				{
					uIButton.States[i].spriteFrames[0].uvs = rect;
				}
				uIButton.animations[i].SetAnim(uIButton.States[i], i);
			}
			uIButton.autoResize = false;
			uIButton.Start();
			return uIButton;
		}

		public static Label Label(string name, string str, bool multiLine, float maxWidth, float fontSize, SpriteText.Font_Effect fontEffect, SpriteText.Anchor_Pos anchor, SpriteText.Alignment_Type alignment, Color color)
		{
			Label result;
			using (new ScopeProfile("UICreateControl - Label"))
			{
				GameObject gameObject = new GameObject(name);
				Label label = gameObject.AddComponent<Label>();
				label.Setup(maxWidth, fontSize);
				label.gameObject.layer = GUICamera.UILayer;
				label.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
				label.includeTextInAutoCollider = false;
				label.DefaultTextAnchor = anchor;
				label.DefaultTextAlignment = alignment;
				label.fontSize = fontSize;
				label.multiLine = multiLine;
				label.maxWidth = maxWidth;
				label.color = color;
				label.CreateSpriteText();
				if (multiLine)
				{
					label.spriteText.useWhiteSpace = false;
					label.spriteTextShadow.useWhiteSpace = false;
				}
				label.SetCharacterSize(fontSize);
				label.SetFontEffect(fontEffect);
				label.Text = str;
				label.BackGroundHide(true);
				label.Start();
				BoxCollider boxCollider = (BoxCollider)label.GetComponent(typeof(BoxCollider));
				if (null != boxCollider)
				{
					UnityEngine.Object.Destroy(boxCollider);
				}
				result = label;
			}
			return result;
		}

		public static Label Label(string name, string str, bool multiLine, float maxWidth, float fontSize, SpriteText.Font_Effect fontEffect, SpriteText.Anchor_Pos anchor, string fontColor)
		{
			Label result;
			using (new ScopeProfile("UICreateControl - Label"))
			{
				GameObject gameObject = new GameObject(name);
				Label label = gameObject.AddComponent<Label>();
				label.width = maxWidth;
				label.height = fontSize;
				label.gameObject.layer = GUICamera.UILayer;
				label.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
				label.includeTextInAutoCollider = false;
				label.DefaultTextAnchor = anchor;
				if (anchor == SpriteText.Anchor_Pos.Upper_Left || (anchor == SpriteText.Anchor_Pos.Middle_Left | anchor == SpriteText.Anchor_Pos.Lower_Left))
				{
					label.DefaultTextAlignment = SpriteText.Alignment_Type.Left;
				}
				else if (anchor == SpriteText.Anchor_Pos.Upper_Center || (anchor == SpriteText.Anchor_Pos.Middle_Center | anchor == SpriteText.Anchor_Pos.Lower_Center))
				{
					label.DefaultTextAlignment = SpriteText.Alignment_Type.Center;
				}
				if (anchor == SpriteText.Anchor_Pos.Upper_Right || anchor == SpriteText.Anchor_Pos.Middle_Right || anchor == SpriteText.Anchor_Pos.Lower_Right)
				{
					label.DefaultTextAlignment = SpriteText.Alignment_Type.Right;
				}
				label.fontSize = fontSize;
				label.multiLine = multiLine;
				label.maxWidth = maxWidth;
				label.CreateSpriteText();
				label.ColorText = fontColor;
				if (multiLine)
				{
					label.spriteText.useWhiteSpace = false;
				}
				label.SetCharacterSize(fontSize);
				label.SetFontEffect(fontEffect);
				label.Text = str;
				label.BackGroundHide(true);
				result = label;
			}
			return result;
		}

		public static Label Label(string name, string str, bool multiLine, float maxWidth, float height, float fontSize, SpriteText.Font_Effect fontEffect, SpriteText.Anchor_Pos anchor, string fontColor)
		{
			GameObject gameObject = new GameObject(name);
			Label label = gameObject.AddComponent<Label>();
			label.customCollider = true;
			label.gameObject.layer = GUICamera.UILayer;
			label.width = maxWidth;
			label.height = height;
			label.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			label.includeTextInAutoCollider = false;
			label.DefaultTextAnchor = anchor;
			if (anchor == SpriteText.Anchor_Pos.Upper_Left || (anchor == SpriteText.Anchor_Pos.Middle_Left | anchor == SpriteText.Anchor_Pos.Lower_Left))
			{
				label.DefaultTextAlignment = SpriteText.Alignment_Type.Left;
			}
			else if (anchor == SpriteText.Anchor_Pos.Upper_Center || (anchor == SpriteText.Anchor_Pos.Middle_Center | anchor == SpriteText.Anchor_Pos.Lower_Center))
			{
				label.DefaultTextAlignment = SpriteText.Alignment_Type.Center;
			}
			if (anchor == SpriteText.Anchor_Pos.Upper_Right || anchor == SpriteText.Anchor_Pos.Middle_Right || anchor == SpriteText.Anchor_Pos.Lower_Right)
			{
				label.DefaultTextAlignment = SpriteText.Alignment_Type.Right;
			}
			label.fontSize = fontSize;
			label.multiLine = multiLine;
			label.maxWidth = maxWidth;
			label.CreateSpriteText();
			label.ColorText = fontColor;
			label.SetCharacterSize(fontSize);
			label.SetFontEffect(fontEffect);
			label.Text = str;
			label.BackGroundHide(true);
			return label;
		}

		public static Emoticon Emoticon(string name, float emoticonWidth, float emoticonHeight, UIBaseInfoLoader baseInfoLoder, float[] delays)
		{
			GameObject gameObject = new GameObject(name);
			Emoticon emoticon = gameObject.AddComponent<Emoticon>();
			emoticon.SetDelay(delays);
			emoticon.SetUseBoxCollider(false);
			emoticon.gameObject.layer = GUICamera.UILayer;
			emoticon.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			emoticon.AutoAnimatorStop = false;
			if (baseInfoLoder != null)
			{
				emoticon.SetSpriteTile(baseInfoLoder.Tile, baseInfoLoder.UVs.width / (float)baseInfoLoder.ButtonCount, baseInfoLoder.UVs.height);
				Material material = (Material)CResources.Load(baseInfoLoder.Material);
				emoticon.Setup(emoticonWidth, emoticonHeight, material);
				float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, baseInfoLoder.UVs.width / (float)baseInfoLoder.ButtonCount);
				Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, baseInfoLoder.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, baseInfoLoder.UVs.y + baseInfoLoder.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, baseInfoLoder.UVs.height));
				for (int i = 0; i < 3; i++)
				{
					emoticon.States[i].spriteFrames = new CSpriteFrame[1];
					emoticon.States[i].spriteFrames[0] = new CSpriteFrame();
					uvs.x += pixelToUVsWidth;
					emoticon.States[i].spriteFrames[0].uvs = uvs;
					emoticon.animations[i] = new UVAnimation();
					emoticon.animations[i].SetAnim(emoticon.States[i], i);
				}
				emoticon.autoResize = false;
			}
			return emoticon;
		}

		public static CheckBox CheckBox(string name, string imageKey, float width, float height)
		{
			UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, imageKey);
			if (uIBaseInfoLoader == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject(name);
			CheckBox checkBox = gameObject.AddComponent<CheckBox>();
			checkBox.autoResize = false;
			checkBox.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			checkBox.gameObject.layer = GUICamera.UILayer;
			checkBox.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			checkBox.Setup(width, height, material);
			checkBox.States = new TextureAnim[]
			{
				new TextureAnim("Checked"),
				new TextureAnim("Unchecked"),
				new TextureAnim("Disabled")
			};
			checkBox.transitions = new EZTransitionList[]
			{
				new EZTransitionList(new EZTransition[]
				{
					new EZTransition("Checked")
				}),
				new EZTransitionList(new EZTransition[]
				{
					new EZTransition("Unchecked")
				}),
				new EZTransitionList(new EZTransition[]
				{
					new EZTransition("Disabled")
				})
			};
			float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
			Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
			Rect uvs = new Rect(rect);
			uvs.x += pixelToUVsWidth;
			for (int i = 0; i < 2; i++)
			{
				checkBox.States[i].spriteFrames = new CSpriteFrame[1];
				checkBox.States[i].spriteFrames[0] = new CSpriteFrame();
				rect.x += pixelToUVsWidth;
				if ((int)uIBaseInfoLoader.ButtonCount <= i)
				{
					checkBox.States[i].spriteFrames[0].uvs = uvs;
				}
				else
				{
					checkBox.States[i].spriteFrames[0].uvs = rect;
				}
				checkBox.animations[i] = new UVAnimation();
				checkBox.animations[i].SetAnim(checkBox.States[i], i);
			}
			return checkBox;
		}

		public static UIRadioBtn RadioBtn(string name, string imageKey, float width, float height)
		{
			UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, imageKey);
			if (uIBaseInfoLoader == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject(name);
			UIRadioBtn uIRadioBtn = gameObject.AddComponent<UIRadioBtn>();
			uIRadioBtn.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			uIRadioBtn.gameObject.layer = GUICamera.UILayer;
			uIRadioBtn.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
			uIRadioBtn.m_bPattern = uIBaseInfoLoader.Pattern;
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			uIRadioBtn.Setup(width, height, material);
			float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
			Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
			for (int i = 0; i <= (int)(uIBaseInfoLoader.ButtonCount - 1); i++)
			{
				uIRadioBtn.States[i].spriteFrames = new CSpriteFrame[1];
				uIRadioBtn.States[i].spriteFrames[0] = new CSpriteFrame();
				uvs.x += pixelToUVsWidth;
				uIRadioBtn.States[i].spriteFrames[0].uvs = uvs;
				uIRadioBtn.animations[i].SetAnim(uIRadioBtn.States[i], i);
			}
			uIRadioBtn.autoResize = false;
			uIRadioBtn.useParentForGrouping = false;
			uIRadioBtn.SetGroup(0);
			uIRadioBtn.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			uIRadioBtn.gameObject.layer = GUICamera.UILayer;
			uIRadioBtn.Start();
			return uIRadioBtn;
		}

		public static UIRadioBtn RadioBtn(Form form, string name, string imageKey, float width, float height)
		{
			UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, imageKey);
			if (uIBaseInfoLoader == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject(name);
			UIRadioBtn uIRadioBtn = gameObject.AddComponent<UIRadioBtn>();
			uIRadioBtn.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			uIRadioBtn.gameObject.layer = GUICamera.UILayer;
			uIRadioBtn.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
			uIRadioBtn.m_bPattern = uIBaseInfoLoader.Pattern;
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			uIRadioBtn.Setup(width, height, material);
			float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
			Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
			for (int i = 0; i <= (int)(uIBaseInfoLoader.ButtonCount - 1); i++)
			{
				uIRadioBtn.States[i].spriteFrames = new CSpriteFrame[1];
				uIRadioBtn.States[i].spriteFrames[0] = new CSpriteFrame();
				uvs.x += pixelToUVsWidth;
				uIRadioBtn.States[i].spriteFrames[0].uvs = uvs;
				uIRadioBtn.animations[i].SetAnim(uIRadioBtn.States[i], i);
			}
			uIRadioBtn.autoResize = false;
			uIRadioBtn.useParentForGrouping = false;
			uIRadioBtn.SetGroup(100 * form.WindowID);
			form.AddDictionaryControl(name, uIRadioBtn);
			form.InteractivePanel.MakeChild(uIRadioBtn.gameObject);
			uIRadioBtn.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			uIRadioBtn.gameObject.layer = GUICamera.UILayer;
			UIButton uIButton = UICreateControl.Button(name + "OverButton", imageKey + "_over", uIRadioBtn.width, uIRadioBtn.height);
			if (null != uIButton)
			{
				uIButton.Start();
				BoxCollider component = uIButton.gameObject.GetComponent<BoxCollider>();
				if (null != component)
				{
					UnityEngine.Object.Destroy(component);
				}
				form.InteractivePanel.MakeChild(uIButton.gameObject);
				uIRadioBtn.layers = new SpriteRoot[1];
				for (int j = 0; j < 1; j++)
				{
					uIRadioBtn.layers[j] = uIButton;
					uIButton.transform.localPosition = uIRadioBtn.GetLocation();
				}
			}
			uIRadioBtn.Start();
			return uIRadioBtn;
		}

		public static Emoticon Emoticon(string name, float emoticonWidth, float emoticonHeight, string emoticonKey)
		{
			GameObject gameObject = new GameObject(name);
			Emoticon emoticon = gameObject.AddComponent<Emoticon>();
			emoticon.SetUseBoxCollider(false);
			emoticon.gameObject.layer = GUICamera.UILayer;
			emoticon.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			emoticon.AutoAnimatorStop = false;
			UIEmoticonInfo uIEmoticonInfo = NrTSingleton<UIEmoticonManager>.Instance.FindUIEmoticonDictionary(emoticonKey);
			if (uIEmoticonInfo != null)
			{
				emoticon.SetDelay(uIEmoticonInfo.delays);
				UIBaseInfoLoader infoLoader = uIEmoticonInfo.infoLoader;
				if (infoLoader == null)
				{
					return emoticon;
				}
				emoticon.SetSpriteTile(infoLoader.Tile, infoLoader.UVs.width / (float)infoLoader.ButtonCount, infoLoader.UVs.height);
				Material material = (Material)CResources.Load(infoLoader.Material);
				emoticon.Setup(emoticonWidth, emoticonHeight, material);
				float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, infoLoader.UVs.width / (float)infoLoader.ButtonCount);
				Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, infoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, infoLoader.UVs.y + infoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, infoLoader.UVs.height));
				for (int i = 0; i < 3; i++)
				{
					emoticon.States[i].spriteFrames = new CSpriteFrame[1];
					emoticon.States[i].spriteFrames[0] = new CSpriteFrame();
					uvs.x += pixelToUVsWidth;
					emoticon.States[i].spriteFrames[0].uvs = uvs;
					emoticon.animations[i].SetAnim(emoticon.States[i], i);
				}
				emoticon.autoResize = false;
			}
			return emoticon;
		}

		public static Emoticon EmoticonCharChat(string name, float emoticonWidth, float emoticonHeight, string emoticonKey)
		{
			GameObject gameObject = new GameObject(name);
			Emoticon emoticon = gameObject.AddComponent<Emoticon>();
			emoticon.SetUseBoxCollider(false);
			emoticon.gameObject.layer = GUICamera.UILayer;
			emoticon.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			emoticon.AutoAnimatorStop = false;
			UIEmoticonInfo uIEmoticonInfo = NrTSingleton<UIEmoticonManager>.Instance.FindUIEmoticonDictionary(emoticonKey);
			if (uIEmoticonInfo != null)
			{
				emoticon.SetDelay(uIEmoticonInfo.delays);
				UIBaseInfoLoader infoLoader = uIEmoticonInfo.infoLoader;
				if (infoLoader == null)
				{
					return emoticon;
				}
				emoticon.SetSpriteTile(infoLoader.Tile, infoLoader.UVs.width / (float)infoLoader.ButtonCount, infoLoader.UVs.height);
				Material material = (Material)CResources.Load(infoLoader.Material);
				emoticon.Setup(emoticonWidth, emoticonHeight, material);
				float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, infoLoader.UVs.width / (float)infoLoader.ButtonCount);
				Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, infoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, infoLoader.UVs.y + infoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, infoLoader.UVs.height));
				for (int i = 0; i < 3; i++)
				{
					emoticon.States[i].spriteFrames = new CSpriteFrame[1];
					emoticon.States[i].spriteFrames[0] = new CSpriteFrame();
					uvs.x += pixelToUVsWidth;
					emoticon.States[i].spriteFrames[0].uvs = uvs;
					emoticon.animations[i].SetAnim(emoticon.States[i], i);
				}
				emoticon.autoResize = false;
			}
			return emoticon;
		}

		public static LinkText LinkText(string name, EmoticonInfo.ChatTextInfo chatTextInfo, ITEM linkItem, Color color, string defaultColor, SpriteText.Font_Effect fontEffect)
		{
			UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, "Com_I_Transparent");
			if (uIBaseInfoLoader == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject(name);
			LinkText linkText = gameObject.AddComponent<LinkText>();
			linkText.UpdateText = true;
			linkText.gameObject.layer = GUICamera.UILayer;
			linkText.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			linkText.linkTextType = chatTextInfo.linkTextType;
			linkText.CreateSpriteText();
			if (chatTextInfo.linkTextType == UnityForms.LinkText.TYPE.PLAYER)
			{
				if (10 >= chatTextInfo.normalText.Length || string.Compare(chatTextInfo.normalText, 0, "[#", 0, 2) != 0 || chatTextInfo.normalText[10] != ']')
				{
					linkText.ColorText = defaultColor;
				}
				linkText.NormalColorText = defaultColor;
				linkText.OverColorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
				{
					"1302"
				});
			}
			else
			{
				string text = string.Empty;
				if (chatTextInfo.linkTextType == UnityForms.LinkText.TYPE.COUPON)
				{
					text = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
					{
						"1306"
					});
				}
				else if (fontEffect == SpriteText.Font_Effect.White_Shadow_Small)
				{
					text = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
					{
						"1206"
					});
				}
				else
				{
					text = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
					{
						"1107"
					});
				}
				linkText.ColorText = text;
				linkText.NormalColorText = text;
				linkText.OverColorText = text;
			}
			linkText.MultiLine = false;
			linkText.BaseString = chatTextInfo.normalText;
			linkText.textKey = chatTextInfo.textKey;
			linkText.SetCharacterSize(chatTextInfo.fontSize);
			linkText.SetFontEffect(fontEffect);
			linkText.Text = linkText.ColorText + chatTextInfo.normalText;
			if (chatTextInfo.linkTextType == UnityForms.LinkText.TYPE.ITEM)
			{
				linkText.data = linkItem;
			}
			linkText.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			linkText.Setup(linkText.spriteText.TotalWidth, linkText.spriteText.TotalHeight, material);
			float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
			Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
			Rect uvs = new Rect(rect);
			uvs.x += pixelToUVsWidth;
			for (int i = 0; i < (int)uIBaseInfoLoader.ButtonCount; i++)
			{
				linkText.States[i].spriteFrames = new CSpriteFrame[1];
				linkText.States[i].spriteFrames[0] = new CSpriteFrame();
				rect.x += pixelToUVsWidth;
				if ((int)uIBaseInfoLoader.ButtonCount <= i)
				{
					linkText.States[i].spriteFrames[0].uvs = uvs;
				}
				else
				{
					linkText.States[i].spriteFrames[0].uvs = rect;
				}
				linkText.animations[i].SetAnim(linkText.States[i], i);
			}
			linkText.autoResize = false;
			linkText.SetState(0);
			linkText.Start();
			return linkText;
		}

		public static TextField TextField(string name, string str, bool multiLine, float maxWidth, float height, float fontSize, SpriteText.Font_Effect fontEffect, SpriteText.Anchor_Pos anchor, string fontColor)
		{
			GameObject gameObject = new GameObject(name);
			TextField textField = gameObject.AddComponent<TextField>();
			if (null == textField)
			{
				return null;
			}
			textField.customCollider = false;
			textField.gameObject.layer = GUICamera.UILayer;
			textField.width = maxWidth;
			textField.height = height;
			textField.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			textField.includeTextInAutoCollider = false;
			textField.DefaultTextAnchor = anchor;
			if (anchor == SpriteText.Anchor_Pos.Upper_Left || (anchor == SpriteText.Anchor_Pos.Middle_Left | anchor == SpriteText.Anchor_Pos.Lower_Left))
			{
				textField.DefaultTextAlignment = SpriteText.Alignment_Type.Left;
			}
			else if (anchor == SpriteText.Anchor_Pos.Upper_Center || (anchor == SpriteText.Anchor_Pos.Middle_Center | anchor == SpriteText.Anchor_Pos.Lower_Center))
			{
				textField.DefaultTextAlignment = SpriteText.Alignment_Type.Center;
			}
			if (anchor == SpriteText.Anchor_Pos.Upper_Right || anchor == SpriteText.Anchor_Pos.Middle_Right || anchor == SpriteText.Anchor_Pos.Lower_Right)
			{
				textField.DefaultTextAlignment = SpriteText.Alignment_Type.Right;
			}
			textField.fontSize = fontSize;
			textField.multiLine = multiLine;
			textField.maxWidth = maxWidth;
			UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Com_I_Transparent");
			textField.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
			textField.m_bPattern = uIBaseInfoLoader.Pattern;
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			textField.Setup(maxWidth, height, material);
			textField.States[0].spriteFrames = new CSpriteFrame[1];
			textField.States[0].spriteFrames[0] = new CSpriteFrame();
			Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
			textField.States[0].spriteFrames[0].uvs = rect;
			textField.SetUVs(rect);
			textField.autoResize = false;
			textField.anchor = SpriteRoot.ANCHOR_METHOD.UPPER_LEFT;
			textField.States[1].spriteFrames = new CSpriteFrame[1];
			textField.States[1].spriteFrames[0] = new CSpriteFrame();
			Rect uvs = new Rect(0f, 0f, 1f, 1f);
			textField.States[1].spriteFrames[0].uvs = uvs;
			textField.CreateSpriteText();
			textField.ColorText = fontColor;
			textField.SetCharacterSize(fontSize);
			textField.SetFontEffect(fontEffect);
			textField.Text = str;
			textField.Start();
			return textField;
		}

		public static HorizontalSlider HorizontalSlider(string name, string styleName1, string styleName2, string styleName3, float width, float height)
		{
			GameObject gameObject = new GameObject(name);
			HorizontalSlider horizontalSlider = gameObject.AddComponent<HorizontalSlider>();
			if (null == horizontalSlider)
			{
				return null;
			}
			horizontalSlider.gameObject.layer = GUICamera.UILayer;
			UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, styleName1);
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			horizontalSlider.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
			horizontalSlider.m_bPattern = uIBaseInfoLoader.Pattern;
			horizontalSlider.Setup(width, height, material);
			Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
			for (int i = 0; i < 1; i++)
			{
				horizontalSlider.States[i].spriteFrames = new CSpriteFrame[1];
				horizontalSlider.States[i].spriteFrames[0] = new CSpriteFrame();
				horizontalSlider.States[i].spriteFrames[0].uvs = rect;
				horizontalSlider.animations[i].SetAnim(horizontalSlider.States[i], i);
			}
			UIBaseInfoLoader uIBaseInfoLoader2 = UIBaseFileManager.FindUIImageDictionary(name, styleName2);
			Material material2 = (Material)CResources.Load(uIBaseInfoLoader.Material);
			horizontalSlider.emptySprite.SetSpriteTile(uIBaseInfoLoader2.Tile, uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount, uIBaseInfoLoader2.UVs.height);
			float num = uIBaseInfoLoader.UVs.height - uIBaseInfoLoader2.UVs.height;
			horizontalSlider.emptySprite.Setup(width, height - num, material2);
			horizontalSlider.emptySprite.SetTextureUVs(new Vector2(uIBaseInfoLoader2.UVs.x, uIBaseInfoLoader2.UVs.y + uIBaseInfoLoader2.UVs.height), new Vector2(uIBaseInfoLoader2.UVs.width, uIBaseInfoLoader2.UVs.height));
			horizontalSlider.emptySprite.transform.localPosition = new Vector3(0f, -num / 2f, -0.5f);
			UIBaseInfoLoader uIBaseInfoLoader3 = UIBaseFileManager.FindUIImageDictionary(name, styleName3);
			horizontalSlider.m_sprKnobTile.SetSpriteTile(uIBaseInfoLoader3.Tile, uIBaseInfoLoader3.UVs.width, uIBaseInfoLoader3.UVs.height);
			horizontalSlider.knobSize = new Vector2(uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount, uIBaseInfoLoader3.UVs.height);
			float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount);
			rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader3.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader3.UVs.y + uIBaseInfoLoader3.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader3.UVs.height));
			Rect uvs = new Rect(rect);
			uvs.x += pixelToUVsWidth;
			for (int j = 1; j < 4; j++)
			{
				horizontalSlider.States[j].spriteFrames = new CSpriteFrame[1];
				horizontalSlider.States[j].spriteFrames[0] = new CSpriteFrame();
				if ((int)uIBaseInfoLoader.ButtonCount > j - 1)
				{
					rect.x += pixelToUVsWidth;
					horizontalSlider.States[j].spriteFrames[0].uvs = rect;
				}
				else
				{
					horizontalSlider.States[j].spriteFrames[0].uvs = uvs;
				}
				horizontalSlider.animations[j].SetAnim(horizontalSlider.States[j], j);
			}
			horizontalSlider.anchor = SpriteRoot.ANCHOR_METHOD.UPPER_LEFT;
			horizontalSlider.autoResize = false;
			horizontalSlider.gameObject.layer = GUICamera.UILayer;
			horizontalSlider.stopKnobFromEdge = uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount / 2f - 3f;
			horizontalSlider.Start();
			return horizontalSlider;
		}

		public static ScrollLabel ScrollLabel(string name, string str, bool multiLine, float maxWidth, float height, float fontSize, SpriteText.Font_Effect fontEffect, SpriteText.Anchor_Pos anchor, string fontColor)
		{
			GameObject gameObject = new GameObject(name);
			ScrollLabel scrollLabel = gameObject.AddComponent<ScrollLabel>();
			scrollLabel.gameObject.layer = GUICamera.UILayer;
			scrollLabel.viewableArea = new Vector2(maxWidth, height);
			scrollLabel.alignment = UIScrollList.ALIGNMENT.LEFT_TOP;
			scrollLabel.orientation = UIScrollList.ORIENTATION.VERTICAL;
			scrollLabel.AnchorPos = anchor;
			scrollLabel.ScrollListTo(0f);
			if (anchor == SpriteText.Anchor_Pos.Upper_Left || (anchor == SpriteText.Anchor_Pos.Middle_Left | anchor == SpriteText.Anchor_Pos.Lower_Left))
			{
				scrollLabel.AlignmentType = SpriteText.Alignment_Type.Left;
			}
			else if (anchor == SpriteText.Anchor_Pos.Upper_Center || (anchor == SpriteText.Anchor_Pos.Middle_Center | anchor == SpriteText.Anchor_Pos.Lower_Center))
			{
				scrollLabel.AlignmentType = SpriteText.Alignment_Type.Center;
			}
			if (anchor == SpriteText.Anchor_Pos.Upper_Right || anchor == SpriteText.Anchor_Pos.Middle_Right || anchor == SpriteText.Anchor_Pos.Lower_Right)
			{
				scrollLabel.AlignmentType = SpriteText.Alignment_Type.Right;
			}
			scrollLabel.FontSize = (int)fontSize;
			scrollLabel.touchScroll = true;
			scrollLabel.CreateBoxCollider();
			scrollLabel.ColorText = fontColor;
			scrollLabel.FontEffect = fontEffect;
			scrollLabel.SetScrollLabel(str);
			return scrollLabel;
		}
	}
}
