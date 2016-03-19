using GameMessage;
using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using Ts;
using UnityEngine;
using UnityForms;

public class UIBaseFileManager : NrTSingleton<UIBaseFileManager>
{
	public enum SCROLLBAR_POSITION
	{
		INNER_RIGHT_DOWN,
		OUTER_RIGHT_DOWN,
		INNER_LEFT_UP,
		OUTER_LEFT_UP
	}

	private static int MAX_CONTROL_NUM = 300;

	private string[] m_szDataFile = new string[UIBaseFileManager.MAX_CONTROL_NUM];

	private string[] m_szTok = new string[40];

	private Dictionary<string, string[]> m_TokenInfo = new Dictionary<string, string[]>();

	private Form pDlg;

	private float fBuffer;

	private float fBufferTopControl;

	private Vector3 vectorValue = Vector3.zero;

	private string m_szfilePath = string.Empty;

	private float defaultPosX = 6f;

	private UIBaseFileManager()
	{
	}

	private void InitStringData()
	{
		this.fBuffer = 0f;
		this.fBufferTopControl = 0f;
		for (int i = 0; i < this.m_szDataFile.Length; i++)
		{
			this.m_szDataFile[i] = string.Empty;
		}
		for (int j = 0; j < this.m_szTok.Length; j++)
		{
			this.m_szTok[j] = string.Empty;
		}
		this.m_TokenInfo.Clear();
	}

	private bool MakePanel(ref Form dlg, string name, G_ID nWindowID, bool bMove, bool bBoxCollider)
	{
		if (name == string.Empty)
		{
			return false;
		}
		this.InitStringData();
		this.m_szfilePath = name;
		string text = NrTSingleton<UIDataManager>.Instance.FilePath + "DLG/" + name + NrTSingleton<UIDataManager>.Instance.AddFilePath;
		TextAsset textAsset = (TextAsset)CResources.Load(text);
		if (textAsset == null)
		{
			string text2 = string.Format("{1}\tDLG FILE NOT FOUND : {0}", text, Scene.CurScene.ToString());
			TsLog.LogError(text2, new object[0]);
			if (TsPlatform.IsMobile)
			{
				TsPlatform.FileLog(text2);
			}
			return false;
		}
		char[] separator = new char[]
		{
			'\n'
		};
		char[] separator2 = new char[]
		{
			','
		};
		this.m_szDataFile = textAsset.text.Split(separator, UIBaseFileManager.MAX_CONTROL_NUM);
		for (int i = 1; i < this.m_szDataFile.Length; i++)
		{
			if (0 < this.m_szDataFile[i].Length)
			{
				if (!(string.Empty == this.m_szDataFile[i]))
				{
					string[] array = this.m_szDataFile[i].Split(separator2, 40);
					if (!this.m_TokenInfo.ContainsKey(array[2]))
					{
						this.m_TokenInfo.Add(array[2], array);
					}
				}
			}
		}
		this.GetSplitStringWindow(ref this.m_szTok);
		this.pDlg = dlg;
		if (!dlg.TopMost)
		{
			this.pDlg.CreateInteractivePanel(nWindowID, new Vector3(float.Parse(this.m_szTok[2]), float.Parse(this.m_szTok[3]), NrTSingleton<FormsManager>.Instance.GetZ()), bMove, dlg.TopMost);
		}
		else
		{
			this.pDlg.CreateInteractivePanel(nWindowID, new Vector3(float.Parse(this.m_szTok[2]), float.Parse(this.m_szTok[3]), NrTSingleton<FormsManager>.Instance.GetTopMostZ()), bMove, dlg.TopMost);
		}
		this.pDlg.FileName = name;
		this.pDlg.InteractivePanel.resourceFileName = name;
		float num = float.Parse(this.m_szTok[4]);
		float num2 = float.Parse(this.m_szTok[5]);
		if (this.m_szTok[7] != string.Empty && this.m_szTok[7] != "0")
		{
			this.pDlg.SetupBG(this.m_szTok[7], float.Parse(this.m_szTok[2]), float.Parse(this.m_szTok[3]), num, num2);
		}
		if (bMove || bBoxCollider)
		{
			BoxCollider boxCollider = (BoxCollider)this.pDlg.InteractivePanel.gameObject.AddComponent(typeof(BoxCollider));
			boxCollider.size = new Vector3(num, num2, 0f);
			boxCollider.center = new Vector3(num / 2f, num2 / 2f, 0f);
		}
		this.pDlg.SetSize(num, num2);
		if (this.m_szTok[6] == "1")
		{
			this.CreateCloseButton(ref dlg.closeButton, UIDataManager.closeButtonName, dlg.Scale);
		}
		if (this.m_szTok.Length > 9 && this.m_szTok[8] != string.Empty)
		{
			this.CreateHelpButton();
		}
		Resources.UnloadAsset(textAsset);
		CResources.Delete(text);
		return true;
	}

	public bool LoadFile(ref Form dlg, string name, G_ID nWindowID, bool bMove, bool bBoxCollider)
	{
		return this.MakePanel(ref dlg, name, nWindowID, bMove, bBoxCollider);
	}

	public bool LoadFile(ref Form dlg, string name, G_ID nWindowID, bool bMove)
	{
		return this.MakePanel(ref dlg, name, nWindowID, bMove, false);
	}

	public void ParsingControl()
	{
		foreach (KeyValuePair<string, string[]> current in this.m_TokenInfo)
		{
			if (3 <= current.Value.Length)
			{
				if (current.Value[1] == "Button")
				{
					Button button = null;
					this.CreateControl(ref button, current.Key);
				}
				else if (current.Value[1] == "Label")
				{
					Label label = null;
					this.CreateControl(ref label, current.Key);
				}
				else if (current.Value[1] == "ScrollLabel")
				{
					ScrollLabel scrollLabel = null;
					this.CreateControl(ref scrollLabel, current.Key);
				}
				else if (current.Value[1] == "FlashLabel")
				{
					FlashLabel flashLabel = null;
					this.CreateControl(ref flashLabel, current.Key);
				}
				else if (current.Value[1] == "ChatLabel")
				{
					ChatLabel chatLabel = null;
					this.CreateControl(ref chatLabel, current.Key);
				}
				else if (current.Value[1] == "ToolBar")
				{
					Toolbar toolbar = null;
					this.CreateControl(ref toolbar, current.Key);
				}
				else if (current.Value[1] == "Box")
				{
					Box box = null;
					this.CreateControl(ref box, current.Key);
				}
				else if (current.Value[1] == "ListBox")
				{
					ListBox listBox = null;
					this.CreateControl(ref listBox, current.Key);
				}
				else if (current.Value[1] == "NewListBox")
				{
					NewListBox newListBox = null;
					this.CreateControl(ref newListBox, current.Key);
				}
				else if (current.Value[1] == "DropDownList")
				{
					DropDownList dropDownList = null;
					this.CreateControl(ref dropDownList, current.Key);
				}
				else if (current.Value[1] == "HSlider")
				{
					HorizontalSlider horizontalSlider = null;
					this.CreateControl(ref horizontalSlider, current.Key);
				}
				else if (current.Value[1] == "Toggle")
				{
					Toggle toggle = null;
					this.CreateControl(ref toggle, current.Key);
				}
				else if (current.Value[1] == "CheckBox")
				{
					CheckBox checkBox = null;
					this.CreateControl(ref checkBox, current.Key);
				}
				else if (current.Value[1] == "TextField")
				{
					using (new ScopeProfile("CreateControl - TextField"))
					{
						TextField textField = null;
						this.CreateControl(ref textField, current.Key);
					}
				}
				else if (current.Value[1] == "TextArea")
				{
					TextArea textArea = null;
					this.CreateControl(ref textArea, current.Key);
				}
				else if (current.Value[1] == "DrawTexture")
				{
					DrawTexture drawTexture = null;
					this.CreateControl(ref drawTexture, current.Key);
				}
				else if (current.Value[1] == "ItemTexture")
				{
					ItemTexture itemTexture = null;
					this.CreateControl(ref itemTexture, current.Key);
				}
				else if (current.Value[1] == "TreeView")
				{
					TreeView treeView = null;
					this.CreateControl(ref treeView, current.Key);
				}
				else if (current.Value[1] == "ImageView")
				{
					ImageView imageView = null;
					this.CreateControl(ref imageView, current.Key);
				}
				else if (current.Value[1] == "ProgressBar")
				{
					ProgressBar progressBar = null;
					this.CreateControl(ref progressBar, current.Key);
				}
			}
		}
	}

	public bool LoadFileAll(ref Form dlg, string name, G_ID nWindowID, bool bMove, bool bBoxCollider)
	{
		if (!this.LoadFile(ref dlg, name, nWindowID, bMove, bBoxCollider))
		{
			return false;
		}
		this.ParsingControl();
		return true;
	}

	public bool LoadFileAll(ref Form dlg, string name, G_ID nWindowID, bool bMove)
	{
		if (!this.LoadFile(ref dlg, name, nWindowID, bMove))
		{
			return false;
		}
		this.ParsingControl();
		return true;
	}

	public void LoadFileSize(string name, out int siWidth, out int siHeight)
	{
		if (name == string.Empty)
		{
			TsLog.LogWarning("KYT : No Search File", new object[0]);
			siWidth = 0;
			siHeight = 0;
		}
		else
		{
			string path = NrTSingleton<UIDataManager>.Instance.FilePath + "DLG/" + name + NrTSingleton<UIDataManager>.Instance.AddFilePath;
			TextAsset textAsset = (TextAsset)CResources.Load(path);
			char[] separator = new char[]
			{
				'\n'
			};
			this.m_szDataFile = textAsset.text.Split(separator, UIBaseFileManager.MAX_CONTROL_NUM);
			this.GetSplitStringWindow(ref this.m_szTok);
			siWidth = int.Parse(this.m_szTok[4]);
			siHeight = int.Parse(this.m_szTok[5]);
		}
	}

	public void GetSplitStringWindow(ref string[] data)
	{
		char[] separator = new char[]
		{
			','
		};
		data = this.m_szDataFile[0].Split(separator);
	}

	public void GetSplitString(ref string[] data, string szID)
	{
		if (this.m_TokenInfo.ContainsKey(szID))
		{
			data = this.m_TokenInfo[szID];
		}
	}

	public float GetZBuffer()
	{
		this.fBuffer -= 0.001f;
		return this.fBuffer;
	}

	private float GetZBufferTopControl()
	{
		this.fBufferTopControl += 0.2f;
		return this.fBuffer - this.fBufferTopControl;
	}

	public static float GetPixelToUVsWidth(Material mat, float width)
	{
		float num = 1f / (float)mat.mainTexture.width;
		return width * num;
	}

	public static float GetPixelToUVsHeight(Material mat, float height)
	{
		float num = 1f / (float)mat.mainTexture.height;
		return height * num;
	}

	public static UIBaseInfoLoader FindUIImageDictionary(string szID, string key)
	{
		UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(key);
		if (uIBaseInfoLoader == null)
		{
			uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Com_I_Transparent");
		}
		return uIBaseInfoLoader;
	}

	private void CreateText(AutoSpriteControlBase ctrl, string[] token)
	{
		ctrl.CreateSpriteText();
		if (token.Length > 17)
		{
			ctrl.SetCharacterSize(float.Parse(token[16]));
		}
		if (token[17] != string.Empty)
		{
			string colorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
			{
				token[17]
			});
			ctrl.ColorText = colorText;
		}
		else
		{
			string colorText2 = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
			{
				"1002"
			});
			ctrl.ColorText = colorText2;
		}
		if (token.Length > 26)
		{
			ctrl.SetFontEffect(UIBaseFileManager.GetFontEffect(token[25]));
		}
		ctrl.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		if ("UpperLeft" == token[14])
		{
			this.vectorValue.x = this.defaultPosX;
			this.vectorValue.y = 0f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Upper_Left);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Left);
		}
		else if ("UpperCenter" == token[14])
		{
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Upper_Center);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Center);
		}
		else if ("UpperRight" == token[14])
		{
			this.vectorValue.x = -this.defaultPosX;
			this.vectorValue.y = 0f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Upper_Right);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Right);
		}
		else if ("MiddleLeft" == token[14])
		{
			this.vectorValue.x = this.defaultPosX;
			this.vectorValue.y = 0f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Middle_Left);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Left);
		}
		else if ("MiddleCenter" == token[14])
		{
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Center);
		}
		else if ("MiddleRight" == token[14])
		{
			this.vectorValue.x = -this.defaultPosX;
			this.vectorValue.y = 0f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Middle_Right);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Right);
		}
		else if ("LowerLeft" == token[14])
		{
			this.vectorValue.x = this.defaultPosX;
			this.vectorValue.y = 0f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Lower_Left);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Left);
		}
		else if ("LowerCenter" == token[14])
		{
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Lower_Center);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Center);
		}
		else if ("LowerRight" == token[14])
		{
			this.vectorValue.x = -this.defaultPosX;
			this.vectorValue.y = 0f;
			this.vectorValue.z = 0f;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Lower_Right);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Right);
		}
		ctrl.MaxWidth = float.Parse(token[5]);
		ctrl.MultiLine = (token[12] == "1");
		ctrl.PassWord = (token[13] == "1");
		string text = MsgHandler.HandleReturn<string>("GetTextFrom", new object[]
		{
			token[10],
			token[11]
		});
		if (!string.IsNullOrEmpty(text))
		{
			ctrl.Text = text;
		}
		else if (token[11] != string.Empty)
		{
			ctrl.Text = token[11];
		}
		else
		{
			ctrl.Text = " ";
		}
		if (token.Length > 32)
		{
			float num = float.Parse(token[31]);
			if (num != 100f)
			{
				ctrl.SetAlpha(float.Parse(token[31]) / 100f);
			}
		}
	}

	public virtual bool CreateControl(ref Box ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = Box.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		ctrl.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
		ctrl.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		ctrl.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
		Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		ctrl.States = new TextureAnim[1];
		ctrl.States[0] = new TextureAnim();
		ctrl.States[0].spriteFrames = new CSpriteFrame[1];
		ctrl.States[0].spriteFrames[0] = new CSpriteFrame();
		ctrl.States[0].spriteFrames[0].uvs = uvs;
		ctrl.animations[0].SetAnim(ctrl.States[0], 0);
		this.CreateText(ctrl, this.m_szTok);
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			ctrl.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		return true;
	}

	public virtual bool CreateControl(ref Button ctrl, string szID)
	{
		UIBaseInfoLoader uIBaseInfoLoader = null;
		using (new ScopeProfile("GetSplitString"))
		{
			this.GetSplitString(ref this.m_szTok, szID);
			uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		}
		ctrl = Button.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		ctrl.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
		ctrl.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		ctrl.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
		float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
		Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		Rect uvs = new Rect(rect);
		uvs.x += pixelToUVsWidth;
		if (TsPlatform.IsMobile)
		{
			if (uIBaseInfoLoader.ButtonCount == 4)
			{
				for (int i = 0; i < 4; i++)
				{
					ctrl.States[i].spriteFrames = new CSpriteFrame[1];
					ctrl.States[i].spriteFrames[0] = new CSpriteFrame();
					rect.x += pixelToUVsWidth;
					ctrl.States[i].spriteFrames[0].uvs = rect;
					ctrl.animations[i].SetAnim(ctrl.States[i], i);
				}
			}
			else if (uIBaseInfoLoader.ButtonCount == 3)
			{
				for (int j = 0; j < 4; j++)
				{
					ctrl.States[j].spriteFrames = new CSpriteFrame[1];
					ctrl.States[j].spriteFrames[0] = new CSpriteFrame();
					if (j != 1)
					{
						rect.x += pixelToUVsWidth;
					}
					else
					{
						rect.x = uvs.x;
					}
					ctrl.States[j].spriteFrames[0].uvs = rect;
					ctrl.animations[j].SetAnim(ctrl.States[j], j);
				}
			}
			else if (uIBaseInfoLoader.ButtonCount == 2)
			{
				for (int k = 0; k < 4; k++)
				{
					ctrl.States[k].spriteFrames = new CSpriteFrame[1];
					ctrl.States[k].spriteFrames[0] = new CSpriteFrame();
					if (k != 1 && k != 3)
					{
						rect.x += pixelToUVsWidth;
					}
					ctrl.States[k].spriteFrames[0].uvs = rect;
					ctrl.animations[k].SetAnim(ctrl.States[k], k);
				}
			}
			else
			{
				for (int l = 0; l < 4; l++)
				{
					ctrl.States[l].spriteFrames = new CSpriteFrame[1];
					ctrl.States[l].spriteFrames[0] = new CSpriteFrame();
					rect.x += pixelToUVsWidth;
					if ((int)uIBaseInfoLoader.ButtonCount <= l)
					{
						ctrl.States[l].spriteFrames[0].uvs = uvs;
					}
					else
					{
						ctrl.States[l].spriteFrames[0].uvs = rect;
					}
					ctrl.animations[l].SetAnim(ctrl.States[l], l);
				}
			}
		}
		else if (TsPlatform.IsWeb)
		{
			for (int m = 0; m < 4; m++)
			{
				ctrl.States[m].spriteFrames = new CSpriteFrame[1];
				ctrl.States[m].spriteFrames[0] = new CSpriteFrame();
				rect.x += pixelToUVsWidth;
				if ((int)uIBaseInfoLoader.ButtonCount <= m)
				{
					ctrl.States[m].spriteFrames[0].uvs = uvs;
				}
				else
				{
					ctrl.States[m].spriteFrames[0].uvs = rect;
				}
				ctrl.animations[m].SetAnim(ctrl.States[m], m);
			}
		}
		ctrl.autoResize = false;
		this.CreateText(ctrl, this.m_szTok);
		if (!ctrl.MultiLine)
		{
			ctrl.MaxWidth = 0f;
		}
		if (this.m_szTok.Length > 22)
		{
			ctrl.Data = this.m_szTok[22];
		}
		if (ctrl.Text == " ")
		{
			ctrl.DeleteSpriteText();
		}
		ctrl.repeat = false;
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			ctrl.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		return true;
	}

	public virtual bool CreateControl(ref DrawTexture ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		ctrl = DrawTexture.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		ctrl.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
		ctrl.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		ctrl.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
		ctrl.SetTextureUVs(new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height));
		ctrl.autoResize = false;
		ctrl.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		if (this.m_szTok.Length > 32)
		{
			float num = float.Parse(this.m_szTok[31]);
			if (num != 100f)
			{
				ctrl.SetAlpha(float.Parse(this.m_szTok[31]) / 100f);
			}
		}
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			ctrl.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		bool flag = false;
		if (this.m_szTok.Length > 21)
		{
			flag = (this.m_szTok[20] == "1");
		}
		if (flag)
		{
			BoxCollider boxCollider = (BoxCollider)ctrl.gameObject.AddComponent(typeof(BoxCollider));
			this.vectorValue.x = float.Parse(this.m_szTok[5]);
			this.vectorValue.y = float.Parse(this.m_szTok[6]);
			this.vectorValue.z = 0f;
			boxCollider.size = this.vectorValue;
			this.vectorValue.x = float.Parse(this.m_szTok[5]) / 2f;
			this.vectorValue.y = -float.Parse(this.m_szTok[6]) / 2f;
			this.vectorValue.z = 0f;
			boxCollider.center = this.vectorValue;
		}
		ctrl.Start();
		return true;
	}

	public virtual bool CreateControl(ref FlashLabel ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = FlashLabel.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		ctrl.width = float.Parse(this.m_szTok[5]);
		ctrl.height = float.Parse(this.m_szTok[6]);
		if (this.m_szTok.Length > 22)
		{
		}
		ctrl.FontSize = (float)int.Parse(this.m_szTok[16]);
		ctrl.LineSpacing = float.Parse(this.m_szTok[21]);
		ctrl.FontEffect = UIBaseFileManager.GetFontEffect(this.m_szTok[25]);
		if ("UpperLeft" == this.m_szTok[14])
		{
			ctrl.Anchor = SpriteText.Anchor_Pos.Upper_Left;
			ctrl.Alignment = SpriteText.Alignment_Type.Left;
		}
		else if ("UpperCenter" == this.m_szTok[14])
		{
			ctrl.Anchor = SpriteText.Anchor_Pos.Upper_Center;
			ctrl.Alignment = SpriteText.Alignment_Type.Center;
		}
		else if ("UpperRight" == this.m_szTok[14])
		{
			ctrl.Anchor = SpriteText.Anchor_Pos.Upper_Right;
			ctrl.Alignment = SpriteText.Alignment_Type.Right;
		}
		else if ("MiddleLeft" == this.m_szTok[14])
		{
			ctrl.Anchor = SpriteText.Anchor_Pos.Middle_Left;
			ctrl.Alignment = SpriteText.Alignment_Type.Left;
		}
		else if ("MiddleCenter" == this.m_szTok[14])
		{
			ctrl.Anchor = SpriteText.Anchor_Pos.Middle_Center;
			ctrl.Alignment = SpriteText.Alignment_Type.Center;
		}
		else if ("MiddleRight" == this.m_szTok[14])
		{
			ctrl.Anchor = SpriteText.Anchor_Pos.Middle_Right;
			ctrl.Alignment = SpriteText.Alignment_Type.Right;
		}
		else if ("LowerLeft" == this.m_szTok[14])
		{
			ctrl.Anchor = SpriteText.Anchor_Pos.Lower_Left;
			ctrl.Alignment = SpriteText.Alignment_Type.Left;
		}
		else if ("LowerCenter" == this.m_szTok[14])
		{
			ctrl.Anchor = SpriteText.Anchor_Pos.Lower_Center;
			ctrl.Alignment = SpriteText.Alignment_Type.Center;
		}
		else if ("LowerRight" == this.m_szTok[14])
		{
			ctrl.Anchor = SpriteText.Anchor_Pos.Lower_Right;
			ctrl.Alignment = SpriteText.Alignment_Type.Right;
		}
		ctrl.FontColor = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
		{
			this.m_szTok[17]
		});
		string text = MsgHandler.HandleReturn<string>("GetTextFrom", new object[]
		{
			this.m_szTok[10],
			this.m_szTok[11]
		});
		if (!string.IsNullOrEmpty(text))
		{
			ctrl.SetFlashLabel(text);
		}
		else if (this.m_szTok[11] != string.Empty)
		{
			ctrl.SetFlashLabel(this.m_szTok[11]);
		}
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		return true;
	}

	public virtual bool CreateControl(ref Label ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = Label.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		if (uIBaseInfoLoader.StyleName != "Com_I_Transparent")
		{
			ctrl.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
			ctrl.m_bPattern = uIBaseInfoLoader.Pattern;
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			ctrl.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
			Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
			ctrl.States = new TextureAnim[1];
			ctrl.States[0] = new TextureAnim();
			ctrl.States[0].spriteFrames = new CSpriteFrame[1];
			ctrl.States[0].spriteFrames[0] = new CSpriteFrame();
			ctrl.States[0].spriteFrames[0].uvs = uvs;
			ctrl.animations[0].SetAnim(ctrl.States[0], 0);
		}
		else
		{
			ctrl.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]));
			ctrl.BackGroundHide(true);
		}
		this.CreateText(ctrl, this.m_szTok);
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			ctrl.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		return true;
	}

	public virtual bool CreateControl(ref ChatLabel ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = ChatLabel.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		ctrl.orientation = UIScrollList.ORIENTATION.VERTICAL;
		ctrl.viewableArea = new Vector2(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]));
		ctrl.alignment = UIScrollList.ALIGNMENT.LEFT_TOP;
		ctrl.ScrollListTo(0f);
		ctrl.clipContents = true;
		ctrl.UseScrollLine = false;
		ctrl.chatLabelScroll = true;
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBufferTopControl());
		if (TsPlatform.IsMobile)
		{
			ctrl.touchScroll = true;
		}
		else
		{
			int num = int.Parse(this.m_szTok[24]);
			if (0 < num)
			{
				UIBaseFileManager.SCROLLBAR_POSITION scrollbarPos = UIBaseFileManager.SCROLLBAR_POSITION.INNER_RIGHT_DOWN;
				if (this.m_szTok.Length > 39)
				{
					scrollbarPos = (UIBaseFileManager.SCROLLBAR_POSITION)int.Parse(this.m_szTok[38]);
				}
				ctrl.slider = this.AddScrollbar(szID, ctrl.orientation, scrollbarPos, ctrl.GetLocation().x, ctrl.GetLocationY(), ctrl.viewableArea);
			}
		}
		ctrl.FontEffect = UIBaseFileManager.GetFontEffect(this.m_szTok[25]);
		ctrl.FontSize = float.Parse(this.m_szTok[16]);
		ctrl.Start();
		return true;
	}

	public virtual bool CreateControl(ref Toggle ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = Toggle.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		ctrl.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
		ctrl.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		ctrl.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
		float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
		Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		for (int i = 0; i <= (int)(uIBaseInfoLoader.ButtonCount - 1); i++)
		{
			if (ctrl.states.Length > i)
			{
				ctrl.States[i].spriteFrames = new CSpriteFrame[1];
				ctrl.States[i].spriteFrames[0] = new CSpriteFrame();
				uvs.x += pixelToUVsWidth;
				ctrl.States[i].spriteFrames[0].uvs = uvs;
				ctrl.animations[i].SetAnim(ctrl.States[i], i);
			}
		}
		ctrl.autoResize = false;
		ctrl.useParentForGrouping = false;
		if (this.m_szTok.Length > 30)
		{
			ctrl.SetGroup(100 * this.pDlg.WindowID + int.Parse(this.m_szTok[29]));
		}
		else
		{
			ctrl.SetGroup(this.pDlg.WindowID);
		}
		ctrl.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			ctrl.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		ctrl.Start();
		return true;
	}

	public virtual bool CreateControl(ref CheckBox ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = CheckBox.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		ctrl.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
		ctrl.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		ctrl.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
		ctrl.States = new TextureAnim[]
		{
			new TextureAnim("Checked"),
			new TextureAnim("Unchecked"),
			new TextureAnim("Disabled")
		};
		ctrl.transitions = new EZTransitionList[]
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
			ctrl.States[i].spriteFrames = new CSpriteFrame[1];
			ctrl.States[i].spriteFrames[0] = new CSpriteFrame();
			rect.x += pixelToUVsWidth;
			if ((int)uIBaseInfoLoader.ButtonCount <= i)
			{
				ctrl.States[i].spriteFrames[0].uvs = uvs;
			}
			else
			{
				ctrl.States[i].spriteFrames[0].uvs = rect;
			}
			ctrl.animations[i].SetAnim(ctrl.States[i], i);
		}
		ctrl.autoResize = false;
		ctrl.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			ctrl.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		ctrl.DTDesable0 = DrawTexture.Create(szID + "_Disable0", Vector3.zero);
		ctrl.DTDesable1 = DrawTexture.Create(szID + "_Disable1", Vector3.zero);
		ctrl.DTDesable0.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
		ctrl.DTDesable1.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
		ctrl.DTDesable0.m_bPattern = uIBaseInfoLoader.Pattern;
		ctrl.DTDesable1.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material2 = (Material)CResources.Load(uIBaseInfoLoader.Material);
		ctrl.DTDesable0.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material2);
		ctrl.DTDesable1.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material2);
		float num = uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount;
		float num2 = uIBaseInfoLoader.UVs.x + num * 2f;
		ctrl.DTDesable0.SetTextureUVs(new Vector2(num2, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(num, uIBaseInfoLoader.UVs.height));
		num2 += num;
		ctrl.DTDesable1.SetTextureUVs(new Vector2(num2, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(num, uIBaseInfoLoader.UVs.height));
		ctrl.DTDesable0.autoResize = false;
		ctrl.DTDesable1.autoResize = false;
		ctrl.DTDesable0.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		ctrl.DTDesable1.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		ctrl.DTDesable0.gameObject.layer = GUICamera.UILayer;
		ctrl.DTDesable1.gameObject.layer = GUICamera.UILayer;
		this.pDlg.InteractivePanel.MakeChild(ctrl.DTDesable0.gameObject);
		this.pDlg.InteractivePanel.MakeChild(ctrl.DTDesable1.gameObject);
		ctrl.DTDesable0.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		ctrl.DTDesable1.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			ctrl.DTDesable0.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
			ctrl.DTDesable1.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		BoxCollider boxCollider = (BoxCollider)ctrl.DTDesable0.gameObject.AddComponent(typeof(BoxCollider));
		boxCollider.size = new Vector3(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), 0f);
		boxCollider.center = new Vector3(float.Parse(this.m_szTok[5]) / 2f, -float.Parse(this.m_szTok[6]) / 2f, 0f);
		boxCollider = (BoxCollider)ctrl.DTDesable1.gameObject.AddComponent(typeof(BoxCollider));
		boxCollider.size = new Vector3(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), 0f);
		boxCollider.center = new Vector3(float.Parse(this.m_szTok[5]) / 2f, -float.Parse(this.m_szTok[6]) / 2f, 0f);
		ctrl.DTDesable0.Visible = false;
		ctrl.DTDesable1.Visible = false;
		ctrl.DTDesable0.gameObject.transform.parent = ctrl.gameObject.transform;
		ctrl.DTDesable1.gameObject.transform.parent = ctrl.gameObject.transform;
		return true;
	}

	public virtual bool CreateControl(ref NewListBox ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = NewListBox.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[8]);
		if (uIBaseInfoLoader.StyleName != "Com_I_Transparent")
		{
			ctrl.BG = DrawTexture.Create("BG", Vector3.zero);
			ctrl.BG.Layer = int.Parse(this.m_szTok[0]);
			ctrl.BG.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
			ctrl.BG.m_bPattern = uIBaseInfoLoader.Pattern;
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			ctrl.BG.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
			ctrl.BG.SetTextureUVs(new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height));
			ctrl.BG.autoResize = false;
			ctrl.BG.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			ctrl.BG.gameObject.layer = GUICamera.UILayer;
			this.pDlg.InteractivePanel.MakeChild(ctrl.BG.gameObject);
			ctrl.BG.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
			if (this.m_szTok.Length > 24)
			{
				ctrl.BG.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
			}
		}
		if (this.m_szTok.Length > 38)
		{
			ctrl.orientation = (UIScrollList.ORIENTATION)int.Parse(this.m_szTok[37]);
		}
		else
		{
			ctrl.orientation = UIScrollList.ORIENTATION.VERTICAL;
		}
		ctrl.ColumnNum = int.Parse(this.m_szTok[10]);
		ctrl.itemSpacing = float.Parse(this.m_szTok[11]);
		ctrl.viewableArea = new Vector2(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]));
		ctrl.alignment = UIScrollList.ALIGNMENT.LEFT_TOP;
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok[7] == string.Empty)
		{
			this.m_szTok[7] = "Win_T_DropDwBtn";
		}
		ctrl.SelectStyle = this.m_szTok[7];
		if (TsPlatform.IsMobile)
		{
			ctrl.touchScroll = true;
		}
		else
		{
			int num = int.Parse(this.m_szTok[24]);
			if (0 < num)
			{
				UIBaseFileManager.SCROLLBAR_POSITION scrollbarPos = UIBaseFileManager.SCROLLBAR_POSITION.INNER_RIGHT_DOWN;
				if (this.m_szTok.Length > 39)
				{
					scrollbarPos = (UIBaseFileManager.SCROLLBAR_POSITION)int.Parse(this.m_szTok[38]);
				}
				ctrl.slider = this.AddScrollbar(szID, ctrl.orientation, scrollbarPos, ctrl.GetLocation().x, ctrl.GetLocationY(), ctrl.viewableArea);
			}
			if (ctrl.slider)
			{
				ctrl.slider.Visible = false;
			}
			ctrl.touchScroll = true;
		}
		if (this.m_szTok.Length > 28)
		{
			ctrl.LineHeight = float.Parse(this.m_szTok[27]);
		}
		if (this.m_szTok.Length > 33 && this.m_szTok[33] != eOverView.eNULL.ToString())
		{
			ctrl.OverViewMode = true;
		}
		char[] separator = new char[]
		{
			'/'
		};
		string[] array = this.m_szfilePath.Split(separator);
		string text = string.Empty;
		for (int i = 0; i < array.Length - 1; i++)
		{
			text += array[i];
			text += "/";
		}
		text = text + szID + "_ColumnData";
		string path = NrTSingleton<UIDataManager>.Instance.FilePath + "DLG/" + text + NrTSingleton<UIDataManager>.Instance.AddFilePath;
		TextAsset textAsset = (TextAsset)CResources.Load(path);
		if (null != textAsset)
		{
			char[] separator2 = new char[]
			{
				'\n'
			};
			char[] separator3 = new char[]
			{
				','
			};
			string[] array2 = textAsset.text.Split(separator2);
			int num2 = 0;
			for (int j = 1; j < array2.Length; j++)
			{
				if (0 < array2[j].Length)
				{
					if (!(string.Empty == array2[j]))
					{
						string[] array3 = array2[j].Split(separator3, 40);
						ctrl.SetColumnData(array3[1], array3[7], array3[8], array3[9], array3[3], array3[4], array3[5], array3[6], array3[14], array3[16], array3[17], array3[12], array3[23], array3[31], array3[20], UIBaseFileManager.GetFontEffect(array3[25]));
						num2++;
					}
				}
			}
			ctrl.ColumnNum = num2;
			Resources.UnloadAsset(textAsset);
			CResources.Delete(path);
		}
		return true;
	}

	public virtual bool CreateControl(ref ListBox ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = ListBox.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[8]);
		if (uIBaseInfoLoader.StyleName != "Com_I_Transparent")
		{
			ctrl.BG = DrawTexture.Create("BG", Vector3.zero);
			ctrl.BG.Layer = int.Parse(this.m_szTok[0]);
			ctrl.BG.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
			ctrl.BG.m_bPattern = uIBaseInfoLoader.Pattern;
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			ctrl.BG.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
			ctrl.BG.SetTextureUVs(new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height));
			ctrl.BG.autoResize = false;
			ctrl.BG.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			ctrl.BG.gameObject.layer = GUICamera.UILayer;
			this.pDlg.InteractivePanel.MakeChild(ctrl.BG.gameObject);
			ctrl.BG.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
			if (this.m_szTok.Length > 24)
			{
				ctrl.BG.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
			}
		}
		if (this.m_szTok.Length > 38)
		{
			ctrl.orientation = (UIScrollList.ORIENTATION)int.Parse(this.m_szTok[37]);
		}
		else
		{
			ctrl.orientation = UIScrollList.ORIENTATION.VERTICAL;
		}
		ctrl.itemSpacing = 0f;
		ctrl.viewableArea = new Vector2(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]));
		ctrl.alignment = UIScrollList.ALIGNMENT.LEFT_TOP;
		ctrl.ScrollListTo(0f);
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok[7] == string.Empty)
		{
			this.m_szTok[7] = "Win_T_DropDwBtn";
		}
		ctrl.SelectStyle = this.m_szTok[7];
		if (TsPlatform.IsMobile)
		{
			ctrl.touchScroll = true;
		}
		else
		{
			int num = int.Parse(this.m_szTok[24]);
			if (0 < num)
			{
				UIBaseFileManager.SCROLLBAR_POSITION scrollbarPos = UIBaseFileManager.SCROLLBAR_POSITION.INNER_RIGHT_DOWN;
				if (this.m_szTok.Length > 39)
				{
					scrollbarPos = (UIBaseFileManager.SCROLLBAR_POSITION)int.Parse(this.m_szTok[38]);
				}
				ctrl.slider = this.AddScrollbar(szID, ctrl.orientation, scrollbarPos, ctrl.GetLocation().x, ctrl.GetLocationY(), ctrl.viewableArea);
			}
			if (ctrl.slider)
			{
				ctrl.slider.Visible = false;
			}
			ctrl.touchScroll = true;
		}
		if (this.m_szTok.Length > 26)
		{
			ctrl.FontEffect = UIBaseFileManager.GetFontEffect(this.m_szTok[25]);
		}
		if (this.m_szTok.Length > 28)
		{
			ctrl.LineHeight = float.Parse(this.m_szTok[27]);
		}
		if (this.m_szTok.Length > 33 && this.m_szTok[33] != eOverView.eNULL.ToString())
		{
			ctrl.OverViewMode = true;
		}
		return true;
	}

	public virtual bool CreateControl(ref TextField ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = TextField.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		ctrl.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
		ctrl.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		ctrl.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
		ctrl.States[0].spriteFrames = new CSpriteFrame[1];
		ctrl.States[0].spriteFrames[0] = new CSpriteFrame();
		Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		ctrl.States[0].spriteFrames[0].uvs = rect;
		ctrl.SetUVs(rect);
		ctrl.autoResize = false;
		ctrl.anchor = SpriteRoot.ANCHOR_METHOD.UPPER_LEFT;
		ctrl.States[1].spriteFrames = new CSpriteFrame[1];
		ctrl.States[1].spriteFrames[0] = new CSpriteFrame();
		Rect uvs = new Rect(0f, 0f, 1f, 1f);
		ctrl.States[1].spriteFrames[0].uvs = uvs;
		ctrl.CreateSpriteText();
		ctrl.MaxWidth = ctrl.spriteText.m_fParentWidth;
		ctrl.PassWord = (this.m_szTok[13] == "1");
		if ("UpperLeft" == this.m_szTok[14])
		{
			this.vectorValue.x = this.defaultPosX;
			this.vectorValue.y = -3f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Upper_Left);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Left);
		}
		else if ("UpperCenter" == this.m_szTok[14])
		{
			this.vectorValue.x = 0f;
			this.vectorValue.y = -3f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Upper_Center);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Center);
		}
		else if ("UpperRight" == this.m_szTok[14])
		{
			this.vectorValue.x = -this.defaultPosX;
			this.vectorValue.y = -3f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Upper_Right);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Right);
		}
		else if ("MiddleLeft" == this.m_szTok[14])
		{
			this.vectorValue.x = this.defaultPosX;
			this.vectorValue.y = 0f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Middle_Left);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Left);
		}
		else if ("MiddleCenter" == this.m_szTok[14])
		{
			this.vectorValue.x = 0f;
			this.vectorValue.y = 0f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Center);
		}
		else if ("MiddleRight" == this.m_szTok[14])
		{
			this.vectorValue.x = -this.defaultPosX;
			this.vectorValue.y = 0f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Middle_Right);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Right);
		}
		else if ("LowerLeft" == this.m_szTok[14])
		{
			this.vectorValue.x = this.defaultPosX;
			this.vectorValue.y = 3f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Lower_Left);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Left);
		}
		else if ("LowerCenter" == this.m_szTok[14])
		{
			this.vectorValue.x = 0f;
			this.vectorValue.y = 3f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Lower_Center);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Center);
		}
		else if ("LowerRight" == this.m_szTok[14])
		{
			this.vectorValue.x = -this.defaultPosX;
			this.vectorValue.y = 3f;
			this.vectorValue.z = 0f;
			ctrl.spriteText.transform.localPosition = this.vectorValue;
			ctrl.SetAnchor(SpriteText.Anchor_Pos.Lower_Right);
			ctrl.SetAlignment(SpriteText.Alignment_Type.Right);
		}
		if (this.m_szTok.Length > 17)
		{
			ctrl.SetCharacterSize(float.Parse(this.m_szTok[16]));
		}
		if (this.m_szTok.Length > 26)
		{
			ctrl.SetFontEffect(UIBaseFileManager.GetFontEffect(this.m_szTok[25]));
		}
		string text = MsgHandler.HandleReturn<string>("GetTextFrom", new object[]
		{
			this.m_szTok[10],
			this.m_szTok[11]
		});
		if (!string.IsNullOrEmpty(text))
		{
			ctrl.Text = text;
		}
		else if (this.m_szTok[11] != string.Empty)
		{
			ctrl.Text = this.m_szTok[11];
		}
		else
		{
			ctrl.Text = " ";
		}
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			ctrl.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		return true;
	}

	public virtual bool CreateControl(ref TextArea ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = TextArea.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		ctrl.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
		ctrl.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		ctrl.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
		ctrl.States[0].spriteFrames = new CSpriteFrame[1];
		ctrl.States[0].spriteFrames[0] = new CSpriteFrame();
		Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		ctrl.States[0].spriteFrames[0].uvs = rect;
		ctrl.SetUVs(rect);
		ctrl.autoResize = false;
		ctrl.anchor = SpriteRoot.ANCHOR_METHOD.UPPER_LEFT;
		Vector2 margins = new Vector2(4f, -3f);
		ctrl.margins = margins;
		string text = MsgHandler.HandleReturn<string>("GetTextFrom", new object[]
		{
			this.m_szTok[10],
			this.m_szTok[11]
		});
		if (!string.IsNullOrEmpty(text))
		{
			ctrl.Text = text;
		}
		else if (this.m_szTok[11] != string.Empty)
		{
			ctrl.Text = this.m_szTok[11];
		}
		else
		{
			ctrl.Text = " ";
		}
		ctrl.MultiLine = true;
		if (this.m_szTok.Length > 17)
		{
			ctrl.SetCharacterSize(float.Parse(this.m_szTok[16]));
		}
		if (this.m_szTok.Length > 26)
		{
			ctrl.SetFontEffect(UIBaseFileManager.GetFontEffect(this.m_szTok[25]));
		}
		ctrl.States[1].spriteFrames = new CSpriteFrame[1];
		ctrl.States[1].spriteFrames[0] = new CSpriteFrame();
		Rect uvs = new Rect(0f, 0f, 1f, 1f);
		ctrl.States[1].spriteFrames[0].uvs = uvs;
		ctrl.MaxWidth = ctrl.spriteText.m_fParentWidth;
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			ctrl.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		return true;
	}

	public virtual bool CreateControl(ref Toolbar Tab, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		Tab = Toolbar.Create(szID, Vector3.zero);
		Tab.Layer = int.Parse(this.m_szTok[0]);
		this.pDlg.InteractivePanel.MakeChild(Tab.gameObject);
		Tab.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBufferTopControl());
		float num = float.Parse(this.m_szTok[5]);
		float num2 = float.Parse(this.m_szTok[6]);
		int num3 = 0;
		if (this.m_szTok.Length > 39)
		{
			num3 = int.Parse(this.m_szTok[38]);
		}
		if (int.Parse(this.m_szTok[15]) <= 0)
		{
			Tab.Count = 1;
		}
		else
		{
			Tab.Count = int.Parse(this.m_szTok[15]);
		}
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		if (null == material)
		{
			Debug.LogError("Resources.Load Fail : " + uIBaseInfoLoader.Material);
		}
		float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
		Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		Tab.Control_Panel = new UIPanel[Tab.Count];
		Tab.Control_Tab = new UIPanelTab[Tab.Count];
		for (int i = 0; i < Tab.Count; i++)
		{
			Tab.Control_Panel[i] = UIPanel.Create("Panel" + i.ToString(), Vector3.zero);
			Tab.Control_Panel[i].index = i;
			Tab.Control_Panel[i].gameObject.layer = GUICamera.UILayer;
			this.pDlg.InteractivePanel.MakeChild(Tab.Control_Panel[i].gameObject);
			Tab.Control_Panel[i].transform.localPosition = Tab.transform.localPosition;
			Tab.Control_Tab[i] = UIPanelTab.Create("Tab" + i.ToString(), Vector3.zero);
			Tab.Control_Tab[i].SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
			if (num3 == 0)
			{
				Tab.Control_Tab[i].Setup(num / (float)Tab.Count, num2, material);
			}
			else
			{
				Tab.Control_Tab[i].Setup(num, num2 / (float)Tab.Count, material);
			}
			uvs.x = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth;
			for (int j = 0; j < (int)(uIBaseInfoLoader.ButtonCount - 1); j++)
			{
				Tab.Control_Tab[i].States[j].spriteFrames = new CSpriteFrame[1];
				Tab.Control_Tab[i].States[j].spriteFrames[0] = new CSpriteFrame();
				uvs.x += pixelToUVsWidth;
				Tab.Control_Tab[i].States[j].spriteFrames[0].uvs = uvs;
				Tab.Control_Tab[i].animations[j].SetAnim(Tab.Control_Tab[i].States[j], i);
			}
			Tab.Control_Tab[i].CreateSpriteText();
			Tab.Control_Tab[i].autoResize = false;
			Tab.Control_Tab[i].panel = Tab.Control_Panel[i];
			Tab.Control_Tab[i].panelManager = Tab;
			Tab.Control_Tab[i].panelShowingAtStart = true;
			Tab.MakeChild(Tab.Control_Tab[i].gameObject);
			if (this.m_szTok[17] != string.Empty)
			{
				string colorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
				{
					this.m_szTok[17]
				});
				Tab.Control_Tab[i].ColorText = colorText;
			}
			else
			{
				string colorText2 = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
				{
					"1002"
				});
				Tab.Control_Tab[i].ColorText = colorText2;
			}
			if (this.m_szTok.Length > 17)
			{
				Tab.Control_Tab[i].SetCharacterSize(float.Parse(this.m_szTok[16]));
			}
			if (this.m_szTok.Length > 26)
			{
				Tab.Control_Tab[i].SetFontEffect(UIBaseFileManager.GetFontEffect(this.m_szTok[25]));
			}
			Tab.Control_Tab[i].SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			if ("UpperLeft" == this.m_szTok[14])
			{
				Tab.Control_Tab[i].SetAnchor(SpriteText.Anchor_Pos.Upper_Left);
				Tab.Control_Tab[i].SetAlignment(SpriteText.Alignment_Type.Left);
			}
			else if ("UpperCenter" == this.m_szTok[14])
			{
				Tab.Control_Tab[i].SetAnchor(SpriteText.Anchor_Pos.Upper_Center);
				Tab.Control_Tab[i].SetAlignment(SpriteText.Alignment_Type.Center);
			}
			else if ("UpperRight" == this.m_szTok[14])
			{
				Tab.Control_Tab[i].SetAnchor(SpriteText.Anchor_Pos.Upper_Right);
				Tab.Control_Tab[i].SetAlignment(SpriteText.Alignment_Type.Right);
			}
			else if ("MiddleLeft" == this.m_szTok[14])
			{
				Tab.Control_Tab[i].SetAnchor(SpriteText.Anchor_Pos.Middle_Left);
				Tab.Control_Tab[i].SetAlignment(SpriteText.Alignment_Type.Left);
			}
			else if ("MiddleCenter" == this.m_szTok[14])
			{
				Tab.Control_Tab[i].SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
				Tab.Control_Tab[i].SetAlignment(SpriteText.Alignment_Type.Center);
			}
			else if ("MiddleRight" == this.m_szTok[14])
			{
				Tab.Control_Tab[i].SetAnchor(SpriteText.Anchor_Pos.Middle_Right);
				Tab.Control_Tab[i].SetAlignment(SpriteText.Alignment_Type.Right);
			}
			else if ("LowerLeft" == this.m_szTok[14])
			{
				Tab.Control_Tab[i].SetAnchor(SpriteText.Anchor_Pos.Lower_Left);
				Tab.Control_Tab[i].SetAlignment(SpriteText.Alignment_Type.Left);
			}
			else if ("LowerCenter" == this.m_szTok[14])
			{
				Tab.Control_Tab[i].SetAnchor(SpriteText.Anchor_Pos.Lower_Center);
				Tab.Control_Tab[i].SetAlignment(SpriteText.Alignment_Type.Center);
			}
			else if ("LowerRight" == this.m_szTok[14])
			{
				Tab.Control_Tab[i].SetAnchor(SpriteText.Anchor_Pos.Lower_Right);
				Tab.Control_Tab[i].SetAlignment(SpriteText.Alignment_Type.Right);
			}
			Tab.Control_Tab[i].MaxWidth = float.Parse(this.m_szTok[5]) / (float)Tab.Count;
			Tab.Control_Tab[i].MultiLine = (this.m_szTok[12] == "1");
			Tab.Control_Tab[i].gameObject.layer = GUICamera.UILayer;
			Tab.Control_Tab[i].parentPanel = this.pDlg.InteractivePanel;
			Tab.Control_Tab[i].SetGroup(Tab.gameObject);
			if (num3 == 0)
			{
				Tab.Control_Tab[i].SetLocation(num / (float)Tab.Count * (float)i, 0f, 0f);
			}
			else
			{
				Tab.Control_Tab[i].SetLocation(0f, num2 / (float)Tab.Count * (float)i, 0f);
			}
			if (this.m_szTok.Length > 24)
			{
				Tab.Control_Tab[i].Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
			}
		}
		if (this.m_szTok.Length > 27)
		{
		}
		Tab.AddPaelTapDelegate();
		this.pDlg.AddDictionaryControl(szID, Tab);
		Tab.Control_Tab[0].Value = true;
		Tab.CurrentPanel = Tab.Control_Panel[0];
		return true;
	}

	public virtual bool CreateControl(ref HorizontalSlider slider, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		slider = HorizontalSlider.Create(szID, Vector3.zero);
		slider.Layer = int.Parse(this.m_szTok[0]);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		slider.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
		slider.m_bPattern = uIBaseInfoLoader.Pattern;
		slider.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
		Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		for (int i = 0; i < 1; i++)
		{
			slider.States[i].spriteFrames = new CSpriteFrame[1];
			slider.States[i].spriteFrames[0] = new CSpriteFrame();
			slider.States[i].spriteFrames[0].uvs = rect;
			slider.animations[i].SetAnim(slider.States[i], i);
		}
		UIBaseInfoLoader uIBaseInfoLoader2 = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[8]);
		Material material2 = (Material)CResources.Load(uIBaseInfoLoader.Material);
		slider.emptySprite.SetSpriteTile(uIBaseInfoLoader2.Tile, uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount, uIBaseInfoLoader2.UVs.height);
		float num = uIBaseInfoLoader.UVs.height - uIBaseInfoLoader2.UVs.height;
		slider.emptySprite.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]) - num, material2);
		slider.emptySprite.SetTextureUVs(new Vector2(uIBaseInfoLoader2.UVs.x, uIBaseInfoLoader2.UVs.y + uIBaseInfoLoader2.UVs.height), new Vector2(uIBaseInfoLoader2.UVs.width, uIBaseInfoLoader2.UVs.height));
		slider.emptySprite.transform.localPosition = new Vector3(0f, -num / 2f, -0.5f);
		UIBaseInfoLoader uIBaseInfoLoader3 = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[9]);
		slider.m_sprKnobTile.SetSpriteTile(uIBaseInfoLoader3.Tile, uIBaseInfoLoader3.UVs.width, uIBaseInfoLoader3.UVs.height);
		slider.knobSize = new Vector2(uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount, float.Parse(this.m_szTok[6]));
		float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount);
		rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader3.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader3.UVs.y + uIBaseInfoLoader3.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader3.UVs.height));
		Rect uvs = new Rect(rect);
		uvs.x += pixelToUVsWidth;
		for (int j = 1; j < 4; j++)
		{
			slider.States[j].spriteFrames = new CSpriteFrame[1];
			slider.States[j].spriteFrames[0] = new CSpriteFrame();
			if ((int)uIBaseInfoLoader.ButtonCount > j - 1)
			{
				rect.x += pixelToUVsWidth;
				slider.States[j].spriteFrames[0].uvs = rect;
			}
			else
			{
				slider.States[j].spriteFrames[0].uvs = uvs;
			}
			slider.animations[j].SetAnim(slider.States[j], j);
		}
		slider.anchor = SpriteRoot.ANCHOR_METHOD.UPPER_LEFT;
		slider.autoResize = false;
		slider.gameObject.layer = GUICamera.UILayer;
		slider.stopKnobFromEdge = uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount / 2f - 3f;
		this.pDlg.AddDictionaryControl(szID, slider);
		this.pDlg.InteractivePanel.MakeChild(slider.gameObject);
		slider.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			slider.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		return true;
	}

	public virtual bool CreateControl(ref DropDownList ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		ctrl = DropDownList.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		if (this.m_szTok[7] == string.Empty)
		{
			this.m_szTok[7] = "Win_T_DropDwBtn";
		}
		ctrl.SelectStyle = this.m_szTok[7];
		UIBaseInfoLoader uIBaseInfoLoader2 = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[8]);
		if (uIBaseInfoLoader2.StyleName != "Com_I_Transparent")
		{
			ctrl.BG = DrawTexture.Create("BG", Vector3.zero);
			ctrl.BG.SetSpriteTile(uIBaseInfoLoader2.Tile, uIBaseInfoLoader2.UVs.width, uIBaseInfoLoader2.UVs.height);
			ctrl.BG.m_bPattern = uIBaseInfoLoader2.Pattern;
			material = (Material)CResources.Load(uIBaseInfoLoader2.Material);
			ctrl.BG.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
			ctrl.BG.SetTextureUVs(new Vector2(uIBaseInfoLoader2.UVs.x, uIBaseInfoLoader2.UVs.y + uIBaseInfoLoader2.UVs.height), new Vector2(uIBaseInfoLoader2.UVs.width, uIBaseInfoLoader2.UVs.height));
			ctrl.BG.autoResize = false;
			ctrl.BG.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
			ctrl.BG.gameObject.layer = GUICamera.UILayer;
			this.pDlg.InteractivePanel.MakeChild(ctrl.BG.gameObject);
			ctrl.BG.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
			if (this.m_szTok.Length > 24)
			{
				ctrl.BG.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
			}
		}
		ctrl.orientation = UIScrollList.ORIENTATION.VERTICAL;
		ctrl.itemSpacing = 0f;
		ctrl.viewableArea = new Vector2(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]));
		ctrl.alignment = UIScrollList.ALIGNMENT.LEFT_TOP;
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.transform.localPosition = Vector3.zero;
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBufferTopControl());
		if (this.m_szTok.Length > 28)
		{
			ctrl.DefaultHeight = float.Parse(this.m_szTok[27]);
		}
		else
		{
			ctrl.DefaultHeight = float.Parse(this.m_szTok[6]);
		}
		this.CreateControl(ref ctrl._ListBox, szID);
		ctrl._ListBox.LineHeight = float.Parse(this.m_szTok[6]);
		if (this.m_szTok[9] == string.Empty)
		{
			ctrl.ListBox.SelectStyle = "Win_T_DropDwBtn0";
		}
		else
		{
			ctrl.ListBox.SelectStyle = this.m_szTok[9];
		}
		ctrl.SetFunction();
		ctrl.SetViewArea(1);
		ctrl.ColumnNum = 1;
		ctrl.OffsetX = 12f;
		if (0f < float.Parse(this.m_szTok[16]))
		{
			ctrl.SetColumnRect(0, 12, 0, int.Parse(this.m_szTok[5]), 0, SpriteText.Anchor_Pos.Middle_Left, float.Parse(this.m_szTok[16]));
		}
		else
		{
			ctrl.SetColumnWidth(int.Parse(this.m_szTok[5]), 0, 0, 0, 0);
			ctrl.SetColumnAlignment(0, SpriteText.Anchor_Pos.Middle_Left);
		}
		ctrl.Clear();
		ctrl.SetHideList();
		return true;
	}

	public virtual bool CreateControl(ref TreeView ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = TreeView.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		ctrl.orientation = UIScrollList.ORIENTATION.VERTICAL;
		ctrl.itemSpacing = 0f;
		ctrl.viewableArea = new Vector2(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]));
		ctrl.alignment = UIScrollList.ALIGNMENT.LEFT_TOP;
		ctrl.FontEffect = UIBaseFileManager.GetFontEffect(this.m_szTok[25]);
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (TsPlatform.IsMobile)
		{
			int num = int.Parse(this.m_szTok[24]);
			if (num != 0)
			{
				ctrl.touchScroll = true;
			}
		}
		else
		{
			int num2 = int.Parse(this.m_szTok[24]);
			if (0 < num2)
			{
				UIBaseFileManager.SCROLLBAR_POSITION scrollbarPos = UIBaseFileManager.SCROLLBAR_POSITION.INNER_RIGHT_DOWN;
				if (this.m_szTok.Length > 39)
				{
					scrollbarPos = (UIBaseFileManager.SCROLLBAR_POSITION)int.Parse(this.m_szTok[38]);
				}
				ctrl.slider = this.AddScrollbar(szID, ctrl.orientation, scrollbarPos, ctrl.GetLocation().x, ctrl.GetLocationY(), ctrl.viewableArea);
			}
			if (ctrl.slider)
			{
				ctrl.slider.Visible = false;
			}
			ctrl.touchScroll = true;
		}
		if (this.m_szTok.Length > 28)
		{
			ctrl.LineHeight = float.Parse(this.m_szTok[27]);
		}
		if (this.m_szTok.Length > 33 && this.m_szTok[33] != eOverView.eNULL.ToString())
		{
			ctrl.OverViewMode = true;
		}
		return true;
	}

	public virtual bool CreateControl(ref ImageView ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = ImageView.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		ctrl.orientation = UIScrollList.ORIENTATION.VERTICAL;
		ctrl.itemSpacing = 0f;
		ctrl.viewableArea = new Vector2(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]));
		ctrl.alignment = UIScrollList.ALIGNMENT.LEFT_TOP;
		ctrl.ScrollListTo(1f);
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		return true;
	}

	public virtual bool CreateControl(ref ProgressBar bar, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[8]);
		bar = ProgressBar.Create(szID, Vector3.zero);
		bar.Layer = int.Parse(this.m_szTok[0]);
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		bar.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
		bar.m_bPattern = uIBaseInfoLoader.Pattern;
		bar.Setup(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]), material);
		Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		bar.States[0].spriteFrames = new CSpriteFrame[1];
		bar.States[0].spriteFrames[0] = new CSpriteFrame();
		bar.States[0].spriteFrames[0].uvs = uvs;
		bar.animations[0].SetAnim(bar.States[0], 0);
		UIBaseInfoLoader uIBaseInfoLoader2 = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		bar.m_bPattern = uIBaseInfoLoader2.Pattern;
		bar.SetSpriteTile(uIBaseInfoLoader2.Tile, uIBaseInfoLoader2.UVs.width, uIBaseInfoLoader2.UVs.height);
		material = (Material)CResources.Load(uIBaseInfoLoader2.Material);
		uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader2.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader2.UVs.y + uIBaseInfoLoader2.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader2.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader2.UVs.height));
		bar.States[1].spriteFrames = new CSpriteFrame[1];
		bar.States[1].spriteFrames[0] = new CSpriteFrame();
		bar.States[1].spriteFrames[0].uvs = uvs;
		bar.animations[1].SetAnim(bar.States[1], 1);
		bar.anchor = SpriteRoot.ANCHOR_METHOD.UPPER_LEFT;
		bar.autoResize = false;
		bar.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, bar);
		this.pDlg.InteractivePanel.MakeChild(bar.gameObject);
		bar.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			bar.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		return true;
	}

	public virtual bool CreateControl(ref ProgressBarUp bar, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[8]);
		bar = ProgressBarUp.Create(szID, Vector3.zero);
		bar.Layer = int.Parse(this.m_szTok[0]);
		Material material = CResources.Load(uIBaseInfoLoader.Material) as Material;
		bar.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
		bar.m_bPattern = uIBaseInfoLoader.Pattern;
		bar.Setup(float.Parse(this.m_szTok[6]), float.Parse(this.m_szTok[5]), material);
		Rect uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		bar.States[0].spriteFrames = new CSpriteFrame[1];
		bar.States[0].spriteFrames[0] = new CSpriteFrame();
		bar.States[0].spriteFrames[0].uvs = uvs;
		bar.animations[0].SetAnim(bar.States[0], 0);
		UIBaseInfoLoader uIBaseInfoLoader2 = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		uvs = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader2.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader2.UVs.y + uIBaseInfoLoader2.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader2.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader2.UVs.height));
		bar.States[1].spriteFrames = new CSpriteFrame[1];
		bar.States[1].spriteFrames[0] = new CSpriteFrame();
		bar.States[1].spriteFrames[0].uvs = uvs;
		bar.animations[1].SetAnim(bar.States[1], 1);
		bar.anchor = SpriteRoot.ANCHOR_METHOD.UPPER_LEFT;
		bar.autoResize = false;
		bar.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, bar);
		this.pDlg.InteractivePanel.MakeChild(bar.gameObject);
		bar.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]) + float.Parse(this.m_szTok[6]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			bar.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		bar.transform.Rotate(0f, 0f, 90f);
		return true;
	}

	public static INVERSE_MODE GetInverse(string _name)
	{
		switch (_name)
		{

			return INVERSE_MODE.NULL;
		case "LEFT_TO_RIGHT":
			return INVERSE_MODE.LEFT_TO_RIGHT;
		case "TOP_TO_BOTTOM":
			return INVERSE_MODE.TOP_TO_BOTTOM;
		case "TOPLEFT_TO_BOTTOMRIGHT":
			return INVERSE_MODE.TOPLEFT_TO_BOTTOMRIGHT;
		}
		return INVERSE_MODE.NULL;
	}

	public virtual bool CreateCloseButton(ref Button ctrl, string name, bool scale)
	{
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, "Win_B_Close");
		ctrl = Button.Create(name, Vector3.zero);
		float num = uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount;
		ctrl.SetSpriteTile(uIBaseInfoLoader.Tile, num, uIBaseInfoLoader.UVs.height);
		ctrl.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		ctrl.Setup(num, uIBaseInfoLoader.UVs.height, material);
		float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, num);
		Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		Rect uvs = new Rect(rect);
		uvs.x += pixelToUVsWidth;
		for (int i = 0; i < 4; i++)
		{
			ctrl.States[i].spriteFrames = new CSpriteFrame[1];
			ctrl.States[i].spriteFrames[0] = new CSpriteFrame();
			rect.x += pixelToUVsWidth;
			if ((int)uIBaseInfoLoader.ButtonCount <= i)
			{
				ctrl.States[i].spriteFrames[0].uvs = uvs;
			}
			else
			{
				ctrl.States[i].spriteFrames[0].uvs = rect;
			}
			ctrl.animations[i].SetAnim(ctrl.States[i], i);
		}
		ctrl.autoResize = false;
		ctrl.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		ctrl.gameObject.layer = GUICamera.UILayer;
		ctrl.EffectAni = false;
		this.pDlg.AddDictionaryControl(name, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		this.pDlg.SetCloseButton(ctrl);
		ctrl.SetLocation(this.pDlg.GetSizeX() - num, 0f, -0.1f);
		return true;
	}

	public virtual bool CreateControl(ref ScrollLabel ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = ScrollLabel.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		ctrl.orientation = UIScrollList.ORIENTATION.VERTICAL;
		ctrl.itemSpacing = 0f;
		ctrl.viewableArea = new Vector2(float.Parse(this.m_szTok[5]), float.Parse(this.m_szTok[6]));
		ctrl.alignment = UIScrollList.ALIGNMENT.LEFT_TOP;
		ctrl.ScrollListTo(0f);
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (TsPlatform.IsMobile)
		{
			ctrl.touchScroll = true;
		}
		ctrl.CreateBoxCollider();
		if (this.m_szTok.Length > 26)
		{
			ctrl.FontEffect = UIBaseFileManager.GetFontEffect(this.m_szTok[25]);
		}
		if (string.Empty != this.m_szTok[17])
		{
			string colorText = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
			{
				this.m_szTok[17]
			});
			ctrl.ColorText = colorText;
		}
		else
		{
			string colorText2 = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
			{
				"1002"
			});
			ctrl.ColorText = colorText2;
		}
		if (this.m_szTok.Length > 26)
		{
			ctrl.FontEffect = UIBaseFileManager.GetFontEffect(this.m_szTok[25]);
		}
		if (this.m_szTok.Length > 17)
		{
			ctrl.FontSize = int.Parse(this.m_szTok[16]);
		}
		ctrl.LineSpasing = float.Parse(this.m_szTok[21]);
		if ("UpperLeft" == this.m_szTok[14])
		{
			ctrl.AnchorPos = SpriteText.Anchor_Pos.Upper_Left;
			ctrl.AlignmentType = SpriteText.Alignment_Type.Left;
		}
		else if ("UpperCenter" == this.m_szTok[14])
		{
			ctrl.AnchorPos = SpriteText.Anchor_Pos.Upper_Center;
			ctrl.AlignmentType = SpriteText.Alignment_Type.Center;
		}
		else if ("UpperRight" == this.m_szTok[14])
		{
			ctrl.AnchorPos = SpriteText.Anchor_Pos.Upper_Right;
			ctrl.AlignmentType = SpriteText.Alignment_Type.Right;
		}
		else if ("MiddleLeft" == this.m_szTok[14])
		{
			ctrl.AnchorPos = SpriteText.Anchor_Pos.Middle_Left;
			ctrl.AlignmentType = SpriteText.Alignment_Type.Left;
		}
		else if ("MiddleCenter" == this.m_szTok[14])
		{
			ctrl.AnchorPos = SpriteText.Anchor_Pos.Middle_Center;
			ctrl.AlignmentType = SpriteText.Alignment_Type.Center;
		}
		else if ("MiddleRight" == this.m_szTok[14])
		{
			ctrl.AnchorPos = SpriteText.Anchor_Pos.Middle_Right;
			ctrl.AlignmentType = SpriteText.Alignment_Type.Right;
		}
		else if ("LowerLeft" == this.m_szTok[14])
		{
			ctrl.AnchorPos = SpriteText.Anchor_Pos.Lower_Left;
			ctrl.AlignmentType = SpriteText.Alignment_Type.Left;
		}
		else if ("LowerCenter" == this.m_szTok[14])
		{
			ctrl.AnchorPos = SpriteText.Anchor_Pos.Lower_Center;
			ctrl.AlignmentType = SpriteText.Alignment_Type.Center;
		}
		else if ("LowerRight" == this.m_szTok[14])
		{
			ctrl.AnchorPos = SpriteText.Anchor_Pos.Lower_Right;
			ctrl.AlignmentType = SpriteText.Alignment_Type.Right;
		}
		string text = MsgHandler.HandleReturn<string>("GetTextFrom", new object[]
		{
			this.m_szTok[10],
			this.m_szTok[11]
		});
		if (!string.IsNullOrEmpty(text))
		{
			ctrl.SetScrollLabel(text);
		}
		else if (this.m_szTok[11] != string.Empty)
		{
			ctrl.SetScrollLabel(this.m_szTok[11]);
		}
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		return true;
	}

	public virtual bool CreateControl(ref ItemTexture ctrl, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		ctrl = ItemTexture.Create(szID, Vector3.zero);
		ctrl.Layer = int.Parse(this.m_szTok[0]);
		float num = float.Parse(this.m_szTok[5]);
		float num2 = float.Parse(this.m_szTok[6]);
		if (this.m_szTok[7].Length > 0)
		{
			UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
			ctrl.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
			ctrl.m_bPattern = uIBaseInfoLoader.Pattern;
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			ctrl.Setup(num, num2, material);
			ctrl.SetTextureUVs(new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height));
		}
		else
		{
			ctrl.width = float.Parse(this.m_szTok[5]);
			ctrl.height = float.Parse(this.m_szTok[6]);
		}
		ctrl.autoResize = false;
		ctrl.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		ctrl.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl(szID, ctrl);
		this.pDlg.InteractivePanel.MakeChild(ctrl.gameObject);
		ctrl.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		if (this.m_szTok.Length > 24)
		{
			ctrl.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		bool flag = false;
		if (this.m_szTok.Length > 21)
		{
			flag = (this.m_szTok[20] == "1");
		}
		if (flag)
		{
			BoxCollider boxCollider = (BoxCollider)ctrl.gameObject.AddComponent(typeof(BoxCollider));
			this.vectorValue.x = num;
			this.vectorValue.y = num2;
			this.vectorValue.z = 0f;
			boxCollider.size = this.vectorValue;
			this.vectorValue.x = num / 2f;
			this.vectorValue.y = -num2 / 2f;
			this.vectorValue.z = 0f;
			boxCollider.center = this.vectorValue;
		}
		if (this.m_szTok.Length > 17)
		{
			ctrl.fontSize = float.Parse(this.m_szTok[16]);
		}
		if (this.m_szTok.Length > 26)
		{
			ctrl.DefaultFontEffect = UIBaseFileManager.GetFontEffect(this.m_szTok[25]);
		}
		ctrl.Start();
		return true;
	}

	public virtual bool CreateControl(ref VerticalSlider slider, string szID)
	{
		this.GetSplitString(ref this.m_szTok, szID);
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[7]);
		slider = VerticalSlider.Create(szID, Vector3.zero);
		slider.Layer = int.Parse(this.m_szTok[0]);
		Material material = CResources.Load(uIBaseInfoLoader.Material) as Material;
		slider.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
		slider.m_bPattern = uIBaseInfoLoader.Pattern;
		slider.Setup(float.Parse(this.m_szTok[6]), float.Parse(this.m_szTok[5]), material);
		Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		for (int i = 1; i < 2; i++)
		{
			slider.States[i].spriteFrames = new CSpriteFrame[1];
			slider.States[i].spriteFrames[0] = new CSpriteFrame();
			slider.States[i].spriteFrames[0].uvs = rect;
			slider.animations[i].SetAnim(slider.States[i], i);
		}
		UIBaseInfoLoader uIBaseInfoLoader2 = UIBaseFileManager.FindUIImageDictionary(szID, this.m_szTok[8]);
		slider.m_sprKnobTile.SetSpriteTile(uIBaseInfoLoader2.Tile, uIBaseInfoLoader2.UVs.width, uIBaseInfoLoader2.UVs.height);
		slider.knobSize = new Vector2(uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount, float.Parse(this.m_szTok[5]));
		float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount);
		rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader2.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader2.UVs.y + uIBaseInfoLoader2.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader2.UVs.height));
		Rect uvs = new Rect(rect);
		uvs.x += pixelToUVsWidth;
		for (int j = 2; j < 5; j++)
		{
			slider.States[j].spriteFrames = new CSpriteFrame[1];
			slider.States[j].spriteFrames[0] = new CSpriteFrame();
			if ((int)uIBaseInfoLoader.ButtonCount > j - 2)
			{
				rect.x += pixelToUVsWidth;
				slider.States[j].spriteFrames[0].uvs = rect;
			}
			else
			{
				slider.States[j].spriteFrames[0].uvs = uvs;
			}
			slider.animations[j].SetAnim(slider.States[j], j);
		}
		slider.anchor = SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT;
		slider.autoResize = false;
		slider.gameObject.layer = GUICamera.UILayer;
		slider.stopKnobFromEdge = uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount / 2f - 3f;
		this.pDlg.AddDictionaryControl(szID, slider);
		this.pDlg.InteractivePanel.MakeChild(slider.gameObject);
		slider.SetLocation(float.Parse(this.m_szTok[3]), float.Parse(this.m_szTok[4]), this.GetZBuffer());
		slider.transform.Rotate(0f, 0f, -90f);
		if (this.m_szTok.Length > 24)
		{
			slider.Inverse(UIBaseFileManager.GetInverse(this.m_szTok[23]));
		}
		return true;
	}

	public virtual bool CreateHelpButton()
	{
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary("HelpButton", "Win_B_Question");
		UIBtnWWW uIBtnWWW = UIBtnWWW.Create("Help", Vector3.zero);
		uIBtnWWW.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
		uIBtnWWW.m_bPattern = uIBaseInfoLoader.Pattern;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		uIBtnWWW.Setup(24f, 24f, material);
		float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
		Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		Rect uvs = new Rect(rect);
		uvs.x += pixelToUVsWidth;
		if (TsPlatform.IsMobile)
		{
			if (uIBaseInfoLoader.ButtonCount == 4)
			{
				for (int i = 0; i < 4; i++)
				{
					uIBtnWWW.States[i].spriteFrames = new CSpriteFrame[1];
					uIBtnWWW.States[i].spriteFrames[0] = new CSpriteFrame();
					rect.x += pixelToUVsWidth;
					uIBtnWWW.States[i].spriteFrames[0].uvs = rect;
					uIBtnWWW.animations[i].SetAnim(uIBtnWWW.States[i], i);
				}
			}
			else if (uIBaseInfoLoader.ButtonCount == 3)
			{
				for (int j = 0; j < 4; j++)
				{
					uIBtnWWW.States[j].spriteFrames = new CSpriteFrame[1];
					uIBtnWWW.States[j].spriteFrames[0] = new CSpriteFrame();
					if (j != 2)
					{
						rect.x += pixelToUVsWidth;
					}
					uIBtnWWW.States[j].spriteFrames[0].uvs = rect;
					uIBtnWWW.animations[j].SetAnim(uIBtnWWW.States[j], j);
				}
			}
			else if (uIBaseInfoLoader.ButtonCount == 2)
			{
				for (int k = 0; k < 4; k++)
				{
					uIBtnWWW.States[k].spriteFrames = new CSpriteFrame[1];
					uIBtnWWW.States[k].spriteFrames[0] = new CSpriteFrame();
					if ((int)uIBaseInfoLoader.ButtonCount > k)
					{
						rect.x += pixelToUVsWidth;
					}
					uIBtnWWW.States[k].spriteFrames[0].uvs = rect;
					uIBtnWWW.animations[k].SetAnim(uIBtnWWW.States[k], k);
				}
			}
			else
			{
				for (int l = 0; l < 4; l++)
				{
					uIBtnWWW.States[l].spriteFrames = new CSpriteFrame[1];
					uIBtnWWW.States[l].spriteFrames[0] = new CSpriteFrame();
					rect.x += pixelToUVsWidth;
					if ((int)uIBaseInfoLoader.ButtonCount <= l)
					{
						uIBtnWWW.States[l].spriteFrames[0].uvs = uvs;
					}
					else
					{
						uIBtnWWW.States[l].spriteFrames[0].uvs = rect;
					}
					uIBtnWWW.animations[l].SetAnim(uIBtnWWW.States[l], l);
				}
			}
		}
		else
		{
			for (int m = 0; m < 4; m++)
			{
				uIBtnWWW.States[m].spriteFrames = new CSpriteFrame[1];
				uIBtnWWW.States[m].spriteFrames[0] = new CSpriteFrame();
				rect.x += pixelToUVsWidth;
				if ((int)uIBaseInfoLoader.ButtonCount <= m)
				{
					uIBtnWWW.States[m].spriteFrames[0].uvs = uvs;
				}
				else
				{
					uIBtnWWW.States[m].spriteFrames[0].uvs = rect;
				}
				uIBtnWWW.animations[m].SetAnim(uIBtnWWW.States[m], m);
			}
		}
		uIBtnWWW.URL = this.m_szTok[8];
		uIBtnWWW.autoResize = false;
		uIBtnWWW.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		uIBtnWWW.gameObject.layer = GUICamera.UILayer;
		this.pDlg.AddDictionaryControl("HelpButton", uIBtnWWW);
		this.pDlg.InteractivePanel.MakeChild(uIBtnWWW.gameObject);
		uIBtnWWW.SetLocation(8f, 11f, this.GetZBuffer());
		return true;
	}

	public UISlider AddScrollbar(string name, UIScrollList.ORIENTATION orientation, UIBaseFileManager.SCROLLBAR_POSITION scrollbarPos, float x, float y, Vector2 size)
	{
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, "Com_I_Transparent1");
		VerticalSlider verticalSlider = VerticalSlider.Create(name + "ScrollBar", Vector3.zero);
		verticalSlider.gameObject.layer = GUICamera.UILayer;
		verticalSlider.darkStyle = false;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		verticalSlider.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
		verticalSlider.m_bPattern = uIBaseInfoLoader.Pattern;
		float num = 18f;
		float num2 = 0f;
		float num3;
		if (orientation == UIScrollList.ORIENTATION.VERTICAL)
		{
			num3 = size.y;
		}
		else
		{
			num3 = size.x;
		}
		verticalSlider.Setup(num3, 18f, material);
		Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		for (int i = 0; i < 2; i++)
		{
			verticalSlider.States[i].spriteFrames = new CSpriteFrame[1];
			verticalSlider.States[i].spriteFrames[0] = new CSpriteFrame();
			verticalSlider.States[i].spriteFrames[0].uvs = rect;
			verticalSlider.animations[i].SetAnim(verticalSlider.States[i], i);
		}
		UIBaseInfoLoader uIBaseInfoLoader2 = UIBaseFileManager.FindUIImageDictionary(name, "Main_T_ChatScBtn");
		verticalSlider.m_sprKnobTile.SetSpriteTile(uIBaseInfoLoader2.Tile, uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount, uIBaseInfoLoader2.UVs.height);
		verticalSlider.knobSize = new Vector2(uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount, uIBaseInfoLoader2.UVs.height);
		material = (Material)CResources.Load(uIBaseInfoLoader2.Material);
		float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount);
		rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader2.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader2.UVs.y + uIBaseInfoLoader2.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader2.UVs.height));
		Rect uvs = new Rect(rect);
		uvs.x += pixelToUVsWidth;
		for (int j = 2; j < 5; j++)
		{
			verticalSlider.States[j].spriteFrames = new CSpriteFrame[1];
			verticalSlider.States[j].spriteFrames[0] = new CSpriteFrame();
			if ((int)uIBaseInfoLoader.ButtonCount > j - 2)
			{
				rect.x += pixelToUVsWidth;
				verticalSlider.States[j].spriteFrames[0].uvs = rect;
			}
			else
			{
				verticalSlider.States[j].spriteFrames[0].uvs = uvs;
			}
			verticalSlider.animations[j].SetAnim(verticalSlider.States[j], j);
		}
		verticalSlider.anchor = SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT;
		verticalSlider.autoResize = false;
		verticalSlider.gameObject.layer = GUICamera.UILayer;
		UIBaseInfoLoader uIBaseInfoLoader3 = UIBaseFileManager.FindUIImageDictionary(name, "Win_B_ScrollArr");
		verticalSlider.upButton.SetSpriteTile(uIBaseInfoLoader3.Tile, uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount, uIBaseInfoLoader3.UVs.height);
		material = (Material)CResources.Load(uIBaseInfoLoader3.Material);
		verticalSlider.upButton.Setup(uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount, uIBaseInfoLoader3.UVs.height, material);
		pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount);
		rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader3.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader3.UVs.y + uIBaseInfoLoader3.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader3.UVs.height));
		Rect uvs2 = new Rect(rect);
		uvs2.x += pixelToUVsWidth;
		for (int k = 0; k < 4; k++)
		{
			verticalSlider.upButton.States[k].spriteFrames = new CSpriteFrame[1];
			verticalSlider.upButton.States[k].spriteFrames[0] = new CSpriteFrame();
			rect.x += pixelToUVsWidth;
			if ((int)uIBaseInfoLoader.ButtonCount <= k)
			{
				verticalSlider.upButton.States[k].spriteFrames[0].uvs = uvs2;
			}
			else
			{
				verticalSlider.upButton.States[k].spriteFrames[0].uvs = rect;
			}
			verticalSlider.upButton.animations[k].SetAnim(verticalSlider.upButton.States[k], k);
		}
		verticalSlider.upButton.transform.parent = verticalSlider.transform;
		verticalSlider.upButton.transform.localScale = Vector3.one;
		verticalSlider.upButton.transform.Rotate(0f, 0f, -90f);
		verticalSlider.upButton.gameObject.layer = GUICamera.UILayer;
		verticalSlider.upButton.autoResize = false;
		verticalSlider.upButton.Inverse(INVERSE_MODE.TOP_TO_BOTTOM);
		verticalSlider.upButton.SetControlState(UIButton.CONTROL_STATE.NORMAL);
		verticalSlider.upButton.transform.localPosition = new Vector3(UISlider.buttonX, UISlider.buttonY, -0.5f);
		verticalSlider.downButton.SetSpriteTile(uIBaseInfoLoader3.Tile, uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount, uIBaseInfoLoader3.UVs.height);
		material = (Material)CResources.Load(uIBaseInfoLoader3.Material);
		verticalSlider.downButton.Setup(uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount, uIBaseInfoLoader3.UVs.height, material);
		pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount);
		rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader3.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader3.UVs.y + uIBaseInfoLoader3.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader3.UVs.height));
		uvs2 = new Rect(rect);
		uvs2.x += pixelToUVsWidth;
		for (int l = 0; l < 4; l++)
		{
			verticalSlider.downButton.States[l].spriteFrames = new CSpriteFrame[1];
			verticalSlider.downButton.States[l].spriteFrames[0] = new CSpriteFrame();
			rect.x += pixelToUVsWidth;
			if ((int)uIBaseInfoLoader.ButtonCount <= l)
			{
				verticalSlider.downButton.States[l].spriteFrames[0].uvs = uvs2;
			}
			else
			{
				verticalSlider.downButton.States[l].spriteFrames[0].uvs = rect;
			}
			verticalSlider.downButton.animations[l].SetAnim(verticalSlider.downButton.States[l], l);
		}
		verticalSlider.downButton.transform.parent = verticalSlider.transform;
		verticalSlider.downButton.transform.localScale = Vector3.one;
		verticalSlider.downButton.transform.Rotate(0f, 0f, -90f);
		verticalSlider.downButton.gameObject.layer = GUICamera.UILayer;
		verticalSlider.downButton.autoResize = false;
		verticalSlider.downButton.SetControlState(UIButton.CONTROL_STATE.NORMAL);
		verticalSlider.downButton.transform.localPosition = new Vector3(num3 - UISlider.buttonX, UISlider.buttonY, -0.5f);
		if (orientation == UIScrollList.ORIENTATION.VERTICAL)
		{
			verticalSlider.transform.Rotate(0f, 0f, -90f);
		}
		this.pDlg.InteractivePanel.MakeChild(verticalSlider.gameObject);
		if (orientation == UIScrollList.ORIENTATION.VERTICAL)
		{
			if (scrollbarPos == UIBaseFileManager.SCROLLBAR_POSITION.INNER_RIGHT_DOWN)
			{
				verticalSlider.SetLocation(x + size.x / 2f - num - num2, y - size.y / 2f, -3f);
			}
			else if (scrollbarPos == UIBaseFileManager.SCROLLBAR_POSITION.OUTER_RIGHT_DOWN)
			{
				verticalSlider.SetLocation(x + size.x / 2f + num - num2, y - size.y / 2f, -3f);
			}
			else if (scrollbarPos == UIBaseFileManager.SCROLLBAR_POSITION.INNER_LEFT_UP)
			{
				verticalSlider.SetLocation(x - size.x / 2f + num - num2, y - size.y / 2f, -3f);
			}
			else if (scrollbarPos == UIBaseFileManager.SCROLLBAR_POSITION.OUTER_LEFT_UP)
			{
				verticalSlider.SetLocation(x - size.x / 2f - num - num2, y - size.y / 2f, -3f);
			}
		}
		else if (orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			if (scrollbarPos == UIBaseFileManager.SCROLLBAR_POSITION.INNER_RIGHT_DOWN)
			{
				verticalSlider.SetLocation(x - size.x / 2f, y + size.y / 2f - num + num2, -3f);
			}
			else if (scrollbarPos == UIBaseFileManager.SCROLLBAR_POSITION.OUTER_RIGHT_DOWN)
			{
				verticalSlider.SetLocation(x - size.x / 2f, y + size.y / 2f + num + num2, -3f);
			}
			else if (scrollbarPos == UIBaseFileManager.SCROLLBAR_POSITION.INNER_LEFT_UP)
			{
				verticalSlider.SetLocation(x - size.x / 2f, y - size.y / 2f + num + num2, -3f);
			}
			else if (scrollbarPos == UIBaseFileManager.SCROLLBAR_POSITION.OUTER_LEFT_UP)
			{
				verticalSlider.SetLocation(x - size.x / 2f, y - size.y / 2f - num + num2, -3f);
			}
		}
		verticalSlider.stopKnobFromEdge = 37f;
		verticalSlider.Start();
		if (orientation == UIScrollList.ORIENTATION.VERTICAL)
		{
			verticalSlider.GetKnob().transform.Rotate(0f, 0f, -90f);
		}
		return verticalSlider;
	}

	public UISlider AddScrollbar(UIInteractivePanel panel, string name, bool darkStyle, float x, float y, float scrollWidth, float scrollHeight, Vector3 knobPos)
	{
		string key = string.Empty;
		string key2 = string.Empty;
		string key3 = string.Empty;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float x2;
		float y2;
		float num5;
		float y3;
		float stopKnobFromEdge;
		float num6;
		if (darkStyle)
		{
			key = "Main_T_ChatScBg";
			key2 = "Main_T_ChatScBtn";
			key3 = "Main_B_Arrow";
			x2 = -num3;
			y2 = num3;
			num5 = num3;
			y3 = num3;
			stopKnobFromEdge = num2 / 2f;
			num6 = num4 + num;
		}
		else
		{
			key = "Main_T_ChatScBg";
			key2 = "Win_T_ScrollCon";
			key3 = "Main_B_Arrow";
			num2 = 24f;
			num3 = 13f;
			num4 = 10f;
			x2 = -UISlider.buttonX;
			y2 = UISlider.buttonY;
			num5 = UISlider.buttonX;
			y3 = UISlider.buttonY;
			stopKnobFromEdge = num2 / 2f;
			num6 = num4 + num;
		}
		UIBaseInfoLoader uIBaseInfoLoader = UIBaseFileManager.FindUIImageDictionary(name, key);
		VerticalSlider verticalSlider = VerticalSlider.Create(name + "ScrollBar", Vector3.zero);
		verticalSlider.knobOffset = knobPos;
		verticalSlider.gameObject.layer = GUICamera.UILayer;
		verticalSlider.darkStyle = darkStyle;
		Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		verticalSlider.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
		verticalSlider.m_bPattern = uIBaseInfoLoader.Pattern;
		verticalSlider.Setup(scrollHeight - num6, scrollWidth, material);
		Rect rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x), 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount), UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		for (int i = 0; i < 2; i++)
		{
			verticalSlider.States[i].spriteFrames = new CSpriteFrame[1];
			verticalSlider.States[i].spriteFrames[0] = new CSpriteFrame();
			verticalSlider.States[i].spriteFrames[0].uvs = rect;
			verticalSlider.animations[i].SetAnim(verticalSlider.States[i], i);
		}
		UIBaseInfoLoader uIBaseInfoLoader2 = UIBaseFileManager.FindUIImageDictionary(name, key2);
		verticalSlider.m_sprKnobTile.SetSpriteTile(uIBaseInfoLoader2.Tile, uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount, uIBaseInfoLoader2.UVs.height);
		verticalSlider.knobSize = new Vector2(uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount, uIBaseInfoLoader2.UVs.height);
		float pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader2.UVs.width / (float)uIBaseInfoLoader2.ButtonCount);
		rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader2.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader2.UVs.y + uIBaseInfoLoader2.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader2.UVs.height));
		Rect uvs = new Rect(rect);
		uvs.x += pixelToUVsWidth;
		for (int j = 2; j < 5; j++)
		{
			verticalSlider.States[j].spriteFrames = new CSpriteFrame[1];
			verticalSlider.States[j].spriteFrames[0] = new CSpriteFrame();
			if ((int)uIBaseInfoLoader.ButtonCount > j - 2)
			{
				rect.x += pixelToUVsWidth;
				verticalSlider.States[j].spriteFrames[0].uvs = rect;
			}
			else
			{
				verticalSlider.States[j].spriteFrames[0].uvs = uvs;
			}
			verticalSlider.animations[j].SetAnim(verticalSlider.States[j], j);
		}
		verticalSlider.anchor = SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT;
		verticalSlider.autoResize = false;
		verticalSlider.gameObject.layer = GUICamera.UILayer;
		UIBaseInfoLoader uIBaseInfoLoader3 = UIBaseFileManager.FindUIImageDictionary(name, key3);
		verticalSlider.upButton.SetSpriteTile(uIBaseInfoLoader3.Tile, uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount, uIBaseInfoLoader3.UVs.height);
		material = (Material)CResources.Load(uIBaseInfoLoader3.Material);
		verticalSlider.upButton.Setup(uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount, uIBaseInfoLoader3.UVs.height, material);
		pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader3.UVs.width / (float)uIBaseInfoLoader3.ButtonCount);
		rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader3.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader3.UVs.y + uIBaseInfoLoader3.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader3.UVs.height));
		Rect uvs2 = new Rect(rect);
		uvs2.x += pixelToUVsWidth;
		for (int k = 0; k < 4; k++)
		{
			verticalSlider.upButton.States[k].spriteFrames = new CSpriteFrame[1];
			verticalSlider.upButton.States[k].spriteFrames[0] = new CSpriteFrame();
			rect.x += pixelToUVsWidth;
			if ((int)uIBaseInfoLoader.ButtonCount <= k)
			{
				verticalSlider.upButton.States[k].spriteFrames[0].uvs = uvs2;
			}
			else
			{
				verticalSlider.upButton.States[k].spriteFrames[0].uvs = rect;
			}
			verticalSlider.upButton.animations[k].SetAnim(verticalSlider.upButton.States[k], k);
		}
		verticalSlider.upButton.transform.parent = verticalSlider.transform;
		verticalSlider.upButton.transform.localPosition = new Vector3(x2, y2, -0.5f);
		verticalSlider.upButton.transform.localScale = Vector3.one;
		verticalSlider.upButton.transform.Rotate(0f, 0f, -90f);
		verticalSlider.upButton.gameObject.layer = GUICamera.UILayer;
		verticalSlider.upButton.autoResize = false;
		verticalSlider.upButton.SetControlState(UIButton.CONTROL_STATE.NORMAL);
		verticalSlider.downButton.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount, uIBaseInfoLoader.UVs.height);
		material = (Material)CResources.Load(uIBaseInfoLoader.Material);
		verticalSlider.downButton.Setup(num3, num4, material);
		pixelToUVsWidth = UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.width / (float)uIBaseInfoLoader.ButtonCount);
		rect = new Rect(UIBaseFileManager.GetPixelToUVsWidth(material, uIBaseInfoLoader.UVs.x) - pixelToUVsWidth, 1f - UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), pixelToUVsWidth, UIBaseFileManager.GetPixelToUVsHeight(material, uIBaseInfoLoader.UVs.height));
		uvs2 = new Rect(rect);
		uvs2.x += pixelToUVsWidth;
		for (int l = 0; l < 4; l++)
		{
			verticalSlider.downButton.States[l].spriteFrames = new CSpriteFrame[1];
			verticalSlider.downButton.States[l].spriteFrames[0] = new CSpriteFrame();
			rect.x += pixelToUVsWidth;
			if ((int)uIBaseInfoLoader.ButtonCount <= l)
			{
				verticalSlider.downButton.States[l].spriteFrames[0].uvs = uvs2;
			}
			else
			{
				verticalSlider.downButton.States[l].spriteFrames[0].uvs = rect;
			}
			verticalSlider.downButton.animations[l].SetAnim(verticalSlider.downButton.States[l], l);
		}
		verticalSlider.downButton.transform.parent = verticalSlider.transform;
		verticalSlider.downButton.transform.localScale = Vector3.one;
		verticalSlider.downButton.transform.Rotate(0f, 0f, -90f);
		verticalSlider.downButton.gameObject.layer = GUICamera.UILayer;
		verticalSlider.downButton.autoResize = false;
		verticalSlider.downButton.Inverse(INVERSE_MODE.TOP_TO_BOTTOM);
		verticalSlider.downButton.SetControlState(UIButton.CONTROL_STATE.NORMAL);
		verticalSlider.downButton.transform.localPosition = new Vector3(scrollHeight + num5, y3, -0.5f);
		panel.MakeChild(verticalSlider.gameObject);
		verticalSlider.SetLocation(x, y + num6, -3f);
		verticalSlider.transform.Rotate(0f, 0f, -90f);
		verticalSlider.stopKnobFromEdge = stopKnobFromEdge;
		verticalSlider.Start();
		return verticalSlider;
	}

	public static SpriteText.Font_Effect GetFontEffect(string _name)
	{
		switch (_name)
		{
		case "Default":
			return SpriteText.Font_Effect.Black_Shadow_Small;
		case "Black_Shadow_Small":
			return SpriteText.Font_Effect.Black_Shadow_Small;
		case "White_Shadow_Small":
			return SpriteText.Font_Effect.White_Shadow_Small;
		case "Black_Shadow_Big":
			return SpriteText.Font_Effect.Black_Shadow_Big;
		case "White_Shadow_Big":
			return SpriteText.Font_Effect.White_Shadow_Big;
		case "Color_Shadow_Green":
			return SpriteText.Font_Effect.Color_Shadow_Green;
		case "Color_Shadow_Red":
			return SpriteText.Font_Effect.Color_Shadow_Red;
		}
		return SpriteText.Font_Effect.Black_Shadow_Small;
	}

	public Toolbar.TEXTPOS GetToolbarTextPos(string _name)
	{
		switch (_name)
		{
		case "eNULL":
			return Toolbar.TEXTPOS.CENTER;
		case "eUP":
			return Toolbar.TEXTPOS.UP;
		case "eDOWN":
			return Toolbar.TEXTPOS.DOWN;
		}
		return Toolbar.TEXTPOS.CENTER;
	}

	public eOverView GetOverViewType(string _name)
	{
		switch (_name)
		{
		case "eDrawTexture":
			return eOverView.eDrawTexture;
		case "eText":
			return eOverView.eText;
		case "eBox":
			return eOverView.eBox;
		}
		return eOverView.eNULL;
	}
}
