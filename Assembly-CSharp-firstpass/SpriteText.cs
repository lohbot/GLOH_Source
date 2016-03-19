using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Label"), ExecuteInEditMode, RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
[Serializable]
public class SpriteText : MonoBehaviour, IUseCamera
{
	public enum Anchor_Pos
	{
		Upper_Left,
		Upper_Center,
		Upper_Right,
		Middle_Left,
		Middle_Center,
		Middle_Right,
		Lower_Left,
		Lower_Center,
		Lower_Right
	}

	public enum Alignment_Type
	{
		Left,
		Center,
		Right
	}

	public enum Font_Effect
	{
		Default,
		Black_Shadow_Small,
		White_Shadow_Small,
		Black_Shadow_Big,
		White_Shadow_Big,
		Color_Shadow_Green,
		Color_Shadow_Red,
		HeadUp
	}

	protected struct NewlineInsertInfo
	{
		public int index;

		public int charDelta;

		public NewlineInsertInfo(int idx, int delta)
		{
			this.index = idx;
			this.charDelta = delta;
		}
	}

	[HideInInspector]
	public const string compositionStr = "RGBA{0,0,255,255}";

	public const string compositionTag = "RGBA{";

	public const string colorTag = "[#";

	public string text = string.Empty;

	public float offsetZ;

	public float characterSize = 1f;

	public float characterSpacing = 1f;

	public float lineSpacing = 1.1f;

	protected float lineSpaceSize;

	public SpriteText.Anchor_Pos anchor;

	public SpriteText.Alignment_Type alignment;

	public int tabSize = 4;

	protected string tabSpaces = "    ";

	public TextAsset font;

	public Color color = Color.white;

	public bool pixelPerfect;

	public float maxWidth;

	public float lastLineWidth;

	public float lastLinePosY;

	public bool multiline = true;

	public bool useWhiteSpace = true;

	public bool dynamicLength;

	public bool removeUnsupportedCharacters = true;

	public bool parseColorTags = true;

	public bool shadowText;

	public bool password;

	public string maskingCharacter = "*";

	protected EZScreenPlacement screenPlacer;

	private IControl parentControl;

	protected bool clipped;

	protected bool updateClipping;

	protected Rect3D clippingRect;

	protected Rect localClipRect;

	protected Vector3 topLeft;

	protected Vector3 bottomRight;

	protected Vector3 unclippedTL;

	protected Vector3 unclippedBR;

	protected Color[] colors = new Color[0];

	protected bool updateColors;

	private bool numberMode;

	protected string[] colDel = new string[]
	{
		"RGBA(",
		"[#",
		"RGBA{",
		")",
		"]",
		"}"
	};

	private string colorText = string.Empty;

	[HideInInspector]
	public bool isClone;

	protected bool m_awake;

	protected bool m_started;

	protected bool stringContentChanged = true;

	protected Vector2 screenSize;

	public Camera renderCamera;

	[HideInInspector]
	public Vector2 pixelsPerUV;

	protected float worldUnitsPerScreenPixel;

	protected float worldUnitsPerTexel;

	protected Vector2 worldUnitsPerUV;

	public bool hideAtStart;

	protected bool m_hidden;

	public bool persistent;

	public bool ignoreClipping;

	protected int capacity;

	protected string meshString = string.Empty;

	protected string plainText = string.Empty;

	protected string displayString = string.Empty;

	protected List<SpriteText.NewlineInsertInfo> newLineInserts = new List<SpriteText.NewlineInsertInfo>();

	protected float totalWidth;

	protected float totalHeight;

	protected SpriteFont spriteFont;

	protected SpriteTextMirror mirror;

	protected Mesh oldMesh;

	protected Mesh mesh;

	protected MeshRenderer meshRenderer;

	protected MeshFilter meshFilter;

	protected Texture texture;

	protected Vector3[] vertices;

	protected int[] faces;

	protected Vector2[] UVs;

	protected Color[] meshColors;

	private StringBuilder displaySB = new StringBuilder();

	private StringBuilder plainSB = new StringBuilder();

	private List<int> colorInserts = new List<int>();

	private List<int> colorTags = new List<int>();

	private List<Color> cols = new List<Color>();

	private string[] lines;

	protected float fAddPixelHeight;

	public bool m_bSpot;

	public float m_fParentWidth;

	public float m_fParentHeight;

	public bool NumberMode
	{
		set
		{
			this.numberMode = value;
		}
	}

	public string ColorText
	{
		get
		{
			return this.colorText;
		}
		set
		{
			this.colorText = value;
		}
	}

	public float TotalWidth
	{
		get
		{
			return this.totalWidth;
		}
	}

	public float TotalHeight
	{
		get
		{
			return this.totalHeight;
		}
	}

	public bool SPOT
	{
		get
		{
			return this.m_bSpot;
		}
		set
		{
			this.m_bSpot = value;
		}
	}

	public Camera RenderCamera
	{
		get
		{
			return this.renderCamera;
		}
		set
		{
			this.SetCamera(value);
		}
	}

	public bool Persistent
	{
		get
		{
			return this.persistent;
		}
		set
		{
			if (value)
			{
				UnityEngine.Object.DontDestroyOnLoad(this);
				UnityEngine.Object.DontDestroyOnLoad(this.mesh);
				this.persistent = value;
			}
		}
	}

	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			if (!this.m_awake)
			{
				this.Awake();
			}
			if (this.spriteFont == null)
			{
				return;
			}
			if (value == null)
			{
				return;
			}
			if (!this.m_started)
			{
				this.Start();
			}
			this.stringContentChanged = true;
			string str = string.Empty;
			if (this.numberMode)
			{
				str = this.ChangeNumberString(value);
			}
			else
			{
				str = value;
			}
			if (this.removeUnsupportedCharacters)
			{
				this.ProcessString(this.spriteFont.RemoveUnsupportedCharacters(str));
			}
			else
			{
				this.ProcessString(str);
			}
			this.UpdateMesh();
		}
	}

	public string PlainText
	{
		get
		{
			return this.plainText;
		}
	}

	public string DisplayString
	{
		get
		{
			return this.displayString;
		}
	}

	public float BaseHeight
	{
		get
		{
			if (this.spriteFont != null)
			{
				return (float)this.spriteFont.BaseHeight * this.worldUnitsPerTexel;
			}
			return 0f;
		}
	}

	public float LineSpan
	{
		get
		{
			return this.lineSpaceSize;
		}
	}

	public Vector3 TopLeft
	{
		get
		{
			return this.topLeft;
		}
	}

	public Vector3 BottomRight
	{
		get
		{
			return this.bottomRight;
		}
	}

	public Vector3 UnclippedTopLeft
	{
		get
		{
			if (!this.m_started)
			{
				this.Start();
			}
			return this.unclippedTL;
		}
	}

	public Vector3 UnclippedBottomRight
	{
		get
		{
			if (!this.m_started)
			{
				this.Start();
			}
			return this.unclippedBR;
		}
	}

	public float CharacterSpacing
	{
		get
		{
			return this.characterSpacing;
		}
		set
		{
			this.characterSpacing = value;
			this.LayoutText();
		}
	}

	public IControl Parent
	{
		get
		{
			return this.parentControl;
		}
		set
		{
			this.parentControl = value;
		}
	}

	protected virtual void Awake()
	{
		if (this.m_awake)
		{
			return;
		}
		this.m_awake = true;
		if (base.name.EndsWith("(Clone)"))
		{
			this.isClone = true;
		}
		this.meshFilter = (MeshFilter)base.GetComponent(typeof(MeshFilter));
		this.meshRenderer = (MeshRenderer)base.GetComponent(typeof(MeshRenderer));
		this.oldMesh = this.meshFilter.sharedMesh;
		this.meshFilter.sharedMesh = null;
		if (this.meshRenderer.sharedMaterial != null)
		{
			this.texture = this.meshRenderer.sharedMaterial.GetTexture("_MainTex");
		}
		else if (Application.isPlaying)
		{
			TsLog.LogWarning("Text on GameObject \"" + base.name + "\" has not been assigned a material.", new object[0]);
		}
		this.Init();
	}

	public virtual void Start()
	{
		if (this.m_started)
		{
			return;
		}
		this.m_started = true;
		if (!this.isClone && Application.isPlaying)
		{
			UnityEngine.Object.Destroy(this.oldMesh);
			this.oldMesh = null;
		}
		if (this.renderCamera == null)
		{
			if (UIManager.Exists() && NrTSingleton<UIManager>.Instance.uiCameras.Length > 0)
			{
				this.renderCamera = NrTSingleton<UIManager>.Instance.uiCameras[0].camera;
			}
			else
			{
				this.renderCamera = Camera.main;
			}
		}
		this.SetCamera(this.renderCamera);
		this.updateColors = true;
		this.UpdateMesh();
	}

	protected virtual void Init()
	{
		this.screenPlacer = (EZScreenPlacement)base.GetComponent(typeof(EZScreenPlacement));
		if (!Application.isPlaying && this.screenPlacer != null)
		{
			this.screenPlacer.SetCamera(this.renderCamera);
		}
		if (NrTSingleton<UIManager>.Instance.defaultSpriteFont != null)
		{
			this.spriteFont = NrTSingleton<UIManager>.Instance.defaultSpriteFont;
			if (this.spriteFont == null)
			{
				TsLog.LogWarning(string.Concat(new string[]
				{
					"Warning: ",
					base.name,
					" was unable to load font \"",
					this.font.name,
					"\"!"
				}), new object[0]);
			}
		}
		else if (Application.isPlaying)
		{
			TsLog.LogWarning("Warning: " + base.name + " currently has no font assigned.", new object[0]);
		}
		if (this.mesh == null)
		{
			this.CreateMesh();
		}
		if (this.persistent)
		{
			this.Persistent = true;
		}
		if (this.texture != null)
		{
			this.SetPixelToUV(this.texture);
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < this.tabSize; i++)
		{
			stringBuilder.Append(' ');
		}
		this.tabSpaces = stringBuilder.ToString();
		this.ProcessString(this.text);
	}

	protected void CreateMesh()
	{
		if (this.meshFilter == null)
		{
			return;
		}
		this.meshFilter.sharedMesh = new Mesh();
		this.mesh = this.meshFilter.sharedMesh;
		if (this.persistent)
		{
			UnityEngine.Object.DontDestroyOnLoad(this.mesh);
		}
	}

	protected void ProcessString(string str)
	{
		this.text = str;
		this.colorInserts.Clear();
		this.colorTags.Clear();
		this.cols.Clear();
		this.newLineInserts.Clear();
		int i = 0;
		int num = -1;
		int num2 = -1;
		float num3 = this.maxWidth;
		this.text = str;
		if (string.IsNullOrEmpty(str) || this.spriteFont == null)
		{
			this.plainText = string.Empty;
			this.displayString = string.Empty;
			return;
		}
		if (str.IndexOf('\t') != -1)
		{
			str = str.Replace("\t", this.tabSpaces);
		}
		int num4;
		if (this.parseColorTags)
		{
			for (int j = 0; j < 3; j++)
			{
				num4 = 0;
				do
				{
					num4 = str.IndexOf(this.colDel[j], num4);
					if (num4 != -1)
					{
						this.colorTags.Add(num4);
						num4++;
					}
				}
				while (num4 != -1);
			}
		}
		if (this.maskingCharacter.Length < 1)
		{
			this.maskingCharacter = "*";
		}
		if (this.colorTags.Count < 1)
		{
			this.plainText = str;
			this.displaySB.Remove(0, this.displaySB.Length);
			if (this.maxWidth > 0f && this.multiline)
			{
				num3 = this.maxWidth;
				for (int k = 0; k < str.Length; k++)
				{
					if (char.IsWhiteSpace(str[k]) && this.useWhiteSpace)
					{
						if (str[k] == '\n')
						{
							num3 = this.maxWidth;
						}
						num = this.displaySB.Length;
						num2 = k;
					}
					if (this.password && str[k] != '\n')
					{
						this.displaySB.Append(this.maskingCharacter[0]);
					}
					else
					{
						this.displaySB.Append(str[k]);
					}
					if (k != num2 + 1 && k > 0)
					{
						num3 -= this.spriteFont.GetWidth(str[k - 1], str[k]) * this.worldUnitsPerTexel * this.characterSpacing;
					}
					else
					{
						num3 -= this.spriteFont.GetSpriteChar(str[k]).xAdvance * this.worldUnitsPerTexel * this.characterSpacing;
					}
					if (num3 < 0f)
					{
						if (k == num)
						{
							num3 = this.maxWidth;
						}
						else
						{
							num3 = this.maxWidth - this.spriteFont.GetWidth(this.displaySB, num + 1, k) * this.worldUnitsPerTexel * this.characterSpacing;
						}
						if (num3 < 0f)
						{
							if (this.displaySB.Length > 0)
							{
								this.displaySB.Insert(this.displaySB.Length - 1, '\n');
								num3 = this.maxWidth - this.spriteFont.GetSpriteChar(str[k]).xAdvance * this.worldUnitsPerTexel * this.characterSpacing;
								this.newLineInserts.Add(new SpriteText.NewlineInsertInfo(k, 1));
							}
						}
						else if (num >= 0 && this.useWhiteSpace)
						{
							this.displaySB[num] = '\n';
							this.newLineInserts.Add(new SpriteText.NewlineInsertInfo(num2, 0));
						}
					}
				}
				this.displayString = this.displaySB.ToString();
			}
			else if (this.password)
			{
				this.displaySB.Remove(0, this.displaySB.Length);
				for (int l = 0; l < str.Length; l++)
				{
					this.displaySB.Append(this.maskingCharacter[0]);
				}
				this.displayString = this.displaySB.ToString();
			}
			else
			{
				this.displaySB.Append(str);
				this.displayString = str;
			}
			if (!this.multiline)
			{
				this.DoSingleLineTruncation();
				this.displayString = this.displaySB.ToString();
				this.totalWidth = this.spriteFont.GetWidth(this.displayString) * this.worldUnitsPerTexel * this.characterSpacing;
			}
			if (this.colors.Length < this.displayString.Length)
			{
				this.colors = new Color[this.displayString.Length];
			}
			for (int m = 0; m < this.colors.Length; m++)
			{
				this.colors[m] = this.color;
			}
			this.updateColors = true;
			return;
		}
		this.colorTags.Sort();
		this.colorTags.Add(-1);
		this.plainSB.Remove(0, this.plainSB.Length);
		this.displaySB.Remove(0, this.displaySB.Length);
		num4 = 0;
		while (i < str.Length)
		{
			if (i == this.colorTags[num4])
			{
				if (string.Compare(str, i, this.colDel[0], 0, this.colDel[0].Length) == 0)
				{
					i += this.colDel[0].Length;
					int num5 = str.IndexOf(')', this.colorTags[num4]) - i;
					if (num5 < 0)
					{
						num5 = str.Length - i;
					}
					Color color = this.ParseHexColor(str.Substring(i, num5));
					if (this.cols.Count == 0)
					{
						if (!this.shadowText)
						{
							this.colorInserts.Add(this.displaySB.Length);
							this.cols.Add(color);
						}
					}
					else if (this.cols[this.cols.Count - 1] != color && !this.shadowText)
					{
						this.colorInserts.Add(this.displaySB.Length);
						this.cols.Add(color);
					}
					i += num5 + 1;
				}
				else if (string.Compare(str, i, this.colDel[1], 0, this.colDel[1].Length) == 0)
				{
					i += this.colDel[1].Length;
					int num5 = str.IndexOf(']', this.colorTags[num4]) - i;
					if (num5 < 0)
					{
						num5 = str.Length - i;
					}
					Color color2 = this.ParseHexColor(str.Substring(i, num5));
					if (this.cols.Count == 0)
					{
						if (!this.shadowText)
						{
							this.colorInserts.Add(this.displaySB.Length);
							this.cols.Add(color2);
						}
					}
					else if (this.cols[this.cols.Count - 1] != color2 && !this.shadowText)
					{
						this.colorInserts.Add(this.displaySB.Length);
						this.cols.Add(color2);
					}
					i += num5 + 1;
				}
				else if (string.Compare(str, i, this.colDel[2], 0, this.colDel[2].Length) == 0)
				{
					i += this.colDel[2].Length;
					int num5 = str.IndexOf('}', this.colorTags[num4]) - i;
					if (num5 < 0)
					{
						num5 = str.Length - i;
					}
					if (!this.shadowText)
					{
						this.colorInserts.Add(this.displaySB.Length);
						this.cols.Add(this.ParseColor(str.Substring(i, num5)));
					}
					i += num5 + 1;
				}
				num4++;
			}
			else if (this.maxWidth > 0f && this.multiline)
			{
				if (char.IsWhiteSpace(str[i]) && this.useWhiteSpace)
				{
					if (str[i] == '\n')
					{
						num3 = this.maxWidth;
					}
					num = this.displaySB.Length;
					num2 = i;
				}
				if (this.password && str[i] != '\n')
				{
					this.displaySB.Append(this.maskingCharacter[0]);
				}
				else
				{
					this.displaySB.Append(str[i]);
				}
				this.plainSB.Append(str[i]);
				if (i != num2 + 1 && i > 0)
				{
					num3 -= this.spriteFont.GetWidth(str[i - 1], str[i]) * this.worldUnitsPerTexel * this.characterSpacing;
				}
				else
				{
					num3 -= this.spriteFont.GetSpriteChar(str[i]).xAdvance * this.worldUnitsPerTexel * this.characterSpacing;
				}
				if (num3 < 0f)
				{
					if (i == num2)
					{
						num3 = this.maxWidth;
					}
					else
					{
						num3 = this.maxWidth - this.spriteFont.GetWidth(this.displaySB, num + 1, i) * this.worldUnitsPerTexel * this.characterSpacing;
					}
					if (num3 < 0f)
					{
						if (this.displaySB.Length > 0)
						{
							int num6 = this.displaySB.Length - 1;
							this.displaySB.Insert(num6, '\n');
							num3 = this.maxWidth - this.spriteFont.GetSpriteChar(str[i]).xAdvance * this.worldUnitsPerTexel * this.characterSpacing;
							this.newLineInserts.Add(new SpriteText.NewlineInsertInfo(this.plainSB.Length - 1, 1));
							int num7 = this.colorInserts.Count - 1;
							while (num7 >= 0 && this.colorInserts[num7] > num6)
							{
								List<int> list;
								List<int> expr_9F6 = list = this.colorInserts;
								int num8;
								int expr_9FB = num8 = num7;
								num8 = list[num8];
								expr_9F6[expr_9FB] = num8 + 1;
								num7--;
							}
						}
					}
					else if (num >= 0 && this.useWhiteSpace)
					{
						this.displaySB[num] = '\n';
						this.newLineInserts.Add(new SpriteText.NewlineInsertInfo(num2, 0));
					}
				}
				i++;
			}
			else
			{
				int num5;
				if (this.colorTags[num4] == -1)
				{
					num5 = str.Length - i;
				}
				else
				{
					num5 = this.colorTags[num4] - i;
				}
				this.plainSB.Append(str, i, num5);
				if (this.password)
				{
					for (int n = i; n < i + num5; n++)
					{
						if (this.spriteFont.ContainsCharacter(str[n]))
						{
							this.displaySB.Append(this.maskingCharacter[0]);
						}
					}
				}
				else
				{
					this.displaySB.Append(str, i, num5);
				}
				i += num5;
			}
		}
		if (this.colorInserts.Count == 0)
		{
			this.colorInserts.Add(0);
			this.cols.Add(this.color);
		}
		if (!this.multiline)
		{
			this.DoSingleLineTruncation();
		}
		this.plainText = this.plainSB.ToString();
		this.displayString = this.displaySB.ToString();
		if (this.colors.Length < this.displayString.Length)
		{
			this.colors = new Color[this.displayString.Length];
		}
		Color color3 = this.color;
		int num9 = 0;
		int num10 = 0;
		while (num9 < this.displayString.Length)
		{
			if (num9 == this.colorInserts[num10])
			{
				color3 = this.cols[num10];
				num10 = (num10 + 1) % this.colorInserts.Count;
			}
			this.colors[num9] = color3;
			num9++;
		}
		this.updateColors = true;
	}

	protected void ProcessStringTextField(string str)
	{
		this.text = str;
		this.colorInserts.Clear();
		this.colorTags.Clear();
		this.cols.Clear();
		this.newLineInserts.Clear();
		int i = 0;
		int num = -1;
		int num2 = -1;
		float num3 = this.maxWidth;
		this.text = str;
		if (string.IsNullOrEmpty(str) || this.spriteFont == null)
		{
			this.plainText = string.Empty;
			this.displayString = string.Empty;
			return;
		}
		if (str.IndexOf('\t') != -1)
		{
			str = str.Replace("\t", this.tabSpaces);
		}
		if (this.maskingCharacter.Length < 1)
		{
			this.maskingCharacter = "*";
		}
		if (this.colorTags.Count < 1)
		{
			this.plainText = str;
			this.displaySB.Remove(0, this.displaySB.Length);
			if (this.maxWidth > 0f && this.multiline)
			{
				num3 = this.maxWidth;
				for (int j = 0; j < str.Length; j++)
				{
					if (char.IsWhiteSpace(str[j]) && this.useWhiteSpace)
					{
						if (str[j] == '\n')
						{
							num3 = this.maxWidth;
						}
						num = this.displaySB.Length;
						num2 = j;
					}
					if (this.password && str[j] != '\n')
					{
						this.displaySB.Append(this.maskingCharacter[0]);
					}
					else
					{
						this.displaySB.Append(str[j]);
					}
					if (j != num2 + 1 && j > 0)
					{
						num3 -= this.spriteFont.GetWidth(str[j - 1], str[j]) * this.worldUnitsPerTexel * this.characterSpacing;
					}
					else
					{
						num3 -= this.spriteFont.GetSpriteChar(str[j]).xAdvance * this.worldUnitsPerTexel * this.characterSpacing;
					}
					if (num3 < 0f)
					{
						if (j == num)
						{
							num3 = this.maxWidth;
						}
						else
						{
							num3 = this.maxWidth - this.spriteFont.GetWidth(this.displaySB, num + 1, j) * this.worldUnitsPerTexel * this.characterSpacing;
						}
						if (num3 < 0f)
						{
							if (this.displaySB.Length > 0)
							{
								this.displaySB.Insert(this.displaySB.Length - 1, '\n');
								num3 = this.maxWidth - this.spriteFont.GetSpriteChar(str[j]).xAdvance * this.worldUnitsPerTexel * this.characterSpacing;
								this.newLineInserts.Add(new SpriteText.NewlineInsertInfo(j, 1));
							}
						}
						else if (num >= 0 && this.useWhiteSpace)
						{
							this.displaySB[num] = '\n';
							this.newLineInserts.Add(new SpriteText.NewlineInsertInfo(num2, 0));
						}
					}
				}
				this.displayString = this.displaySB.ToString();
			}
			else if (this.password)
			{
				this.displaySB.Remove(0, this.displaySB.Length);
				for (int k = 0; k < str.Length; k++)
				{
					this.displaySB.Append(this.maskingCharacter[0]);
				}
				this.displayString = this.displaySB.ToString();
			}
			else
			{
				this.displaySB.Append(str);
				this.displayString = str;
			}
			if (!this.multiline)
			{
				this.DoSingleLineTruncation();
				this.displayString = this.displaySB.ToString();
			}
			if (this.colors.Length < this.displayString.Length)
			{
				this.colors = new Color[this.displayString.Length];
			}
			for (int l = 0; l < this.colors.Length; l++)
			{
				this.colors[l] = this.color;
			}
			this.updateColors = true;
			return;
		}
		this.colorTags.Sort();
		this.colorTags.Add(-1);
		this.plainSB.Remove(0, this.plainSB.Length);
		this.displaySB.Remove(0, this.displaySB.Length);
		int num4 = 0;
		while (i < str.Length)
		{
			if (i == this.colorTags[num4])
			{
				if (string.Compare(str, i, this.colDel[0], 0, this.colDel[0].Length) == 0)
				{
					i += this.colDel[0].Length;
					int num5 = str.IndexOf(')', this.colorTags[num4]) - i;
					if (num5 < 0)
					{
						num5 = str.Length - i;
					}
					Color color = this.ParseHexColor(str.Substring(i, num5));
					if (this.cols.Count == 0)
					{
						this.colorInserts.Add(this.displaySB.Length);
						this.cols.Add(color);
					}
					else if (this.cols[this.cols.Count - 1] != color)
					{
						this.colorInserts.Add(this.displaySB.Length);
						this.cols.Add(color);
					}
					i += num5 + 1;
				}
				else if (string.Compare(str, i, this.colDel[1], 0, this.colDel[1].Length) == 0)
				{
					i += this.colDel[1].Length;
					int num5 = str.IndexOf(']', this.colorTags[num4]) - i;
					if (num5 < 0)
					{
						num5 = str.Length - i;
					}
					Color color2 = this.ParseHexColor(str.Substring(i, num5));
					if (this.cols.Count == 0)
					{
						this.colorInserts.Add(this.displaySB.Length);
						this.cols.Add(color2);
					}
					else if (this.cols[this.cols.Count - 1] != color2)
					{
						this.colorInserts.Add(this.displaySB.Length);
						this.cols.Add(color2);
					}
					i += num5 + 1;
				}
				else if (string.Compare(str, i, this.colDel[2], 0, this.colDel[2].Length) == 0)
				{
					i += this.colDel[2].Length;
					int num5 = str.IndexOf('}', this.colorTags[num4]) - i;
					if (num5 < 0)
					{
						num5 = str.Length - i;
					}
					this.colorInserts.Add(this.displaySB.Length);
					this.cols.Add(this.ParseColor(str.Substring(i, num5)));
					i += num5 + 1;
				}
				num4++;
			}
			else if (this.maxWidth > 0f && this.multiline)
			{
				if (char.IsWhiteSpace(str[i]) && this.useWhiteSpace)
				{
					if (str[i] == '\n')
					{
						num3 = this.maxWidth;
					}
					num = this.displaySB.Length;
					num2 = i;
				}
				if (this.password && str[i] != '\n')
				{
					this.displaySB.Append(this.maskingCharacter[0]);
				}
				else
				{
					this.displaySB.Append(str[i]);
				}
				this.plainSB.Append(str[i]);
				if (i != num2 + 1 && i > 0)
				{
					num3 -= this.spriteFont.GetWidth(str[i - 1], str[i]) * this.worldUnitsPerTexel * this.characterSpacing;
				}
				else
				{
					num3 -= this.spriteFont.GetSpriteChar(str[i]).xAdvance * this.worldUnitsPerTexel * this.characterSpacing;
				}
				if (num3 < 0f)
				{
					if (i == num2)
					{
						num3 = this.maxWidth;
					}
					else
					{
						num3 = this.maxWidth - this.spriteFont.GetWidth(this.displaySB, num + 1, i) * this.worldUnitsPerTexel * this.characterSpacing;
					}
					if (num3 < 0f)
					{
						if (this.displaySB.Length > 0)
						{
							int num6 = this.displaySB.Length - 1;
							this.displaySB.Insert(num6, '\n');
							num3 = this.maxWidth - this.spriteFont.GetSpriteChar(str[i]).xAdvance * this.worldUnitsPerTexel * this.characterSpacing;
							this.newLineInserts.Add(new SpriteText.NewlineInsertInfo(this.plainSB.Length - 1, 1));
							int num7 = this.colorInserts.Count - 1;
							while (num7 >= 0 && this.colorInserts[num7] > num6)
							{
								List<int> list;
								List<int> expr_948 = list = this.colorInserts;
								int num8;
								int expr_94D = num8 = num7;
								num8 = list[num8];
								expr_948[expr_94D] = num8 + 1;
								num7--;
							}
						}
					}
					else if (num >= 0 && this.useWhiteSpace)
					{
						this.displaySB[num] = '\n';
						this.newLineInserts.Add(new SpriteText.NewlineInsertInfo(num2, 0));
					}
				}
				i++;
			}
			else
			{
				int num5;
				if (this.colorTags[num4] == -1)
				{
					num5 = str.Length - i;
				}
				else
				{
					num5 = this.colorTags[num4] - i;
				}
				this.plainSB.Append(str, i, num5);
				if (this.password)
				{
					for (int m = i; m < i + num5; m++)
					{
						if (this.spriteFont.ContainsCharacter(str[m]))
						{
							this.displaySB.Append(this.maskingCharacter[0]);
						}
					}
				}
				else
				{
					this.displaySB.Append(str, i, num5);
				}
				i += num5;
			}
		}
		if (this.colorInserts.Count == 0)
		{
			this.colorInserts.Add(0);
			this.cols.Add(this.color);
		}
		if (!this.multiline)
		{
			this.DoSingleLineTruncation();
		}
		this.plainText = this.plainSB.ToString();
		this.displayString = this.displaySB.ToString();
		if (this.colors.Length < this.displayString.Length)
		{
			this.colors = new Color[this.displayString.Length];
		}
		Color color3 = this.color;
		int n = 0;
		int num9 = 0;
		while (n < this.displayString.Length)
		{
			if (n == this.colorInserts[num9])
			{
				color3 = this.cols[num9];
				num9 = (num9 + 1) % this.colorInserts.Count;
			}
			this.colors[n] = color3;
			n++;
		}
		this.updateColors = true;
	}

	protected void DoSingleLineTruncation()
	{
		int num = this.displayString.IndexOf('\n');
		if (num >= 0 && this.displaySB.Length > num)
		{
			this.displaySB.Remove(num, this.displaySB.Length - num);
			this.displaySB.Append((!this.m_bSpot) ? string.Empty : "...");
		}
		if (this.maxWidth > 0f)
		{
			float num2 = this.spriteFont.GetWidth(this.displaySB, 0, this.displaySB.Length - 1) * this.worldUnitsPerTexel * this.characterSpacing;
			if (num2 > this.maxWidth)
			{
				int num3 = 0;
				float num4 = this.spriteFont.GetWidth((!this.m_bSpot) ? string.Empty : "...") * this.worldUnitsPerTexel * this.characterSpacing;
				do
				{
					num3 += 2;
					num2 = this.spriteFont.GetWidth(this.displaySB, 0, this.displaySB.Length - 1 - num3) * this.worldUnitsPerTexel * this.characterSpacing;
				}
				while (num2 + num4 > this.maxWidth && num2 != 0f);
				num3 = Mathf.Clamp(num3, 0, this.displaySB.Length);
				this.displaySB.Remove(this.displaySB.Length - num3, num3);
				this.displaySB.Append((!this.m_bSpot) ? string.Empty : "...");
			}
		}
		if (this.password)
		{
			for (int i = 0; i < this.displaySB.Length; i++)
			{
				this.displaySB[i] = this.maskingCharacter[0];
			}
		}
	}

	protected Color ParseColor(string str)
	{
		string[] array = str.Split(new char[]
		{
			','
		});
		if (array.Length != 4)
		{
			return this.color;
		}
		return new Color(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
	}

	protected Color ParseHexColor(string str)
	{
		if (str.Length < 6)
		{
			return this.color;
		}
		Color result;
		try
		{
			int num = int.Parse(str.Substring(0, 2), NumberStyles.AllowHexSpecifier);
			int num2 = int.Parse(str.Substring(2, 2), NumberStyles.AllowHexSpecifier);
			int num3 = int.Parse(str.Substring(4, 2), NumberStyles.AllowHexSpecifier);
			int num4 = 255;
			if (str.Length == 8)
			{
				num4 = int.Parse(str.Substring(6, 2), NumberStyles.AllowHexSpecifier);
			}
			result = new Color((float)num / 255f, (float)num2 / 255f, (float)num3 / 255f, (float)num4 / 255f);
		}
		catch
		{
			result = this.color;
		}
		return result;
	}

	protected void EnlargeMesh()
	{
		this.vertices = new Vector3[this.displayString.Length * 4];
		this.UVs = new Vector2[this.displayString.Length * 4];
		this.meshColors = new Color[this.displayString.Length * 4];
		this.faces = new int[this.displayString.Length * 6];
		for (int i = 0; i < this.displayString.Length; i++)
		{
			this.faces[i * 6] = i * 4;
			this.faces[i * 6 + 1] = i * 4 + 3;
			this.faces[i * 6 + 2] = i * 4 + 1;
			this.faces[i * 6 + 3] = i * 4 + 3;
			this.faces[i * 6 + 4] = i * 4 + 2;
			this.faces[i * 6 + 5] = i * 4 + 1;
		}
		this.capacity = this.displayString.Length;
	}

	public void UpdateMesh()
	{
		if (this.mesh == null)
		{
			this.CreateMesh();
		}
		if (this.spriteFont == null)
		{
			return;
		}
		bool flag = false;
		if (this.meshString.Length < 15 && !this.updateClipping && !this.updateColors && this.stringContentChanged && this.meshString == this.displayString)
		{
			return;
		}
		if (this.displayString.Length < 1)
		{
			this.ClearMesh();
		}
		else
		{
			if (this.displayString.Length > this.capacity)
			{
				this.EnlargeMesh();
				flag = true;
			}
			if (this.clipped)
			{
				this.updateClipping = false;
				this.localClipRect = Rect3D.MultFast(this.clippingRect, base.transform.worldToLocalMatrix).GetRect();
			}
			if (this.stringContentChanged)
			{
				this.lines = this.displayString.Split(new char[]
				{
					'\n'
				});
			}
			if (this.lines.Length == 1)
			{
				this.Layout_Single_Line();
			}
			else
			{
				this.Layout_Multiline(this.lines);
			}
			this.unclippedTL = this.topLeft;
			this.unclippedBR = this.bottomRight;
			if (this.clipped)
			{
				this.topLeft.x = Mathf.Max(this.localClipRect.x, this.topLeft.x);
				this.topLeft.y = Mathf.Min(this.localClipRect.yMax, this.topLeft.y);
				this.bottomRight.x = Mathf.Min(this.localClipRect.xMax, this.bottomRight.x);
				this.bottomRight.y = Mathf.Max(this.localClipRect.y, this.bottomRight.y);
			}
		}
		this.stringContentChanged = false;
		this.meshString = this.displayString;
		if (flag)
		{
			this.mesh.Clear();
		}
		this.mesh.vertices = this.vertices;
		this.mesh.uv = this.UVs;
		this.mesh.colors = this.meshColors;
		this.mesh.triangles = this.faces;
		if (flag)
		{
			this.mesh.RecalculateNormals();
		}
		this.mesh.RecalculateBounds();
		if (this.parentControl != null)
		{
			if (this.parentControl is AutoSpriteControlBase)
			{
				if (((AutoSpriteControlBase)this.parentControl).includeTextInAutoCollider)
				{
					((AutoSpriteControlBase)this.parentControl).UpdateCollider();
				}
				((AutoSpriteControlBase)this.parentControl).FindOuterEdges();
			}
			else if (this.parentControl is ControlBase && ((ControlBase)this.parentControl).includeTextInAutoCollider)
			{
				((ControlBase)this.parentControl).UpdateCollider();
			}
		}
	}

	protected Vector3 GetStartPos_SingleLine(float baseHeight, float width)
	{
		switch (this.anchor)
		{
		case SpriteText.Anchor_Pos.Upper_Left:
			return new Vector3(0f, 0f, this.offsetZ);
		case SpriteText.Anchor_Pos.Upper_Center:
			return new Vector3(width * -0.5f + this.m_fParentWidth * 0.5f, 0f, this.offsetZ);
		case SpriteText.Anchor_Pos.Upper_Right:
			return new Vector3(-width + this.m_fParentWidth, 0f, this.offsetZ);
		case SpriteText.Anchor_Pos.Middle_Left:
			return new Vector3(0f, baseHeight * 0.5f + this.m_fParentHeight * 0.5f, this.offsetZ);
		case SpriteText.Anchor_Pos.Middle_Center:
			return new Vector3(width * -0.5f + this.m_fParentWidth * 0.5f, baseHeight * 0.5f + this.m_fParentHeight * 0.5f, this.offsetZ);
		case SpriteText.Anchor_Pos.Middle_Right:
			return new Vector3(-width + this.m_fParentWidth, baseHeight * 0.5f + this.m_fParentHeight * 0.5f, this.offsetZ);
		case SpriteText.Anchor_Pos.Lower_Left:
			return new Vector3(0f, baseHeight + this.m_fParentHeight, this.offsetZ);
		case SpriteText.Anchor_Pos.Lower_Center:
			return new Vector3(width * -0.5f + this.m_fParentWidth * 0.5f, baseHeight + this.m_fParentHeight, this.offsetZ);
		case SpriteText.Anchor_Pos.Lower_Right:
			return new Vector3(-width + this.m_fParentWidth, baseHeight + this.m_fParentHeight, this.offsetZ);
		default:
			return new Vector3(0f, 0f, this.offsetZ);
		}
	}

	public int GetDisplayLineCount(int charIndex, out int charLine, out int lineStart, out int lineEnd)
	{
		int num = 1;
		int num2 = 0;
		charLine = -1;
		int num3 = -1;
		lineStart = 0;
		lineEnd = -1;
		for (int i = 0; i < this.displayString.Length; i++)
		{
			if (this.displayString[i] == '\n')
			{
				if (num == charLine)
				{
					lineEnd = Mathf.Max(0, i - 1);
				}
				num3 = i;
				num++;
			}
			if (num2 == charIndex)
			{
				charLine = num;
				lineStart = num3 + 1;
			}
			num2++;
		}
		if (lineEnd < 0)
		{
			lineEnd = this.displayString.Length - 1;
		}
		if (charLine < 0)
		{
			charLine = num;
			lineStart = Mathf.Min(this.displayString.Length - 1, num3 + 1);
		}
		return num;
	}

	public int GetDisplayLineCount()
	{
		int num = 1;
		for (int i = 0; i < this.displayString.Length; i++)
		{
			if (this.displayString[i] == '\n')
			{
				num++;
			}
		}
		return num;
	}

	public int PlainIndexToDisplayIndex(int plainCharIndex)
	{
		int num = plainCharIndex;
		for (int i = 0; i < this.newLineInserts.Count; i++)
		{
			if (this.newLineInserts[i].index > plainCharIndex)
			{
				break;
			}
			num += this.newLineInserts[i].charDelta;
		}
		return num;
	}

	public int DisplayIndexToPlainIndex(int dispCharIndex)
	{
		int num = dispCharIndex;
		for (int i = 0; i < this.newLineInserts.Count; i++)
		{
			if (this.newLineInserts[i].index > dispCharIndex)
			{
				break;
			}
			num -= this.newLineInserts[i].charDelta;
		}
		return num;
	}

	public int GetNearestInsertionPoint(Vector3 point)
	{
		point = base.transform.InverseTransformPoint(point);
		int displayLineCount = this.GetDisplayLineCount();
		int num = 0;
		if (displayLineCount > 1)
		{
			float num2 = float.PositiveInfinity;
			int num3 = 1;
			for (int i = 1; i <= displayLineCount; i++)
			{
				float f = point.y - (this.GetLineBaseline(displayLineCount, i) + this.BaseHeight * 0.5f);
				if (Mathf.Abs(f) < num2)
				{
					num2 = Mathf.Abs(f);
					num3 = i;
				}
			}
			int num4 = 0;
			int num5 = 1;
			while (num4 < this.displayString.Length && num5 < num3)
			{
				if (this.displayString[num4] == '\n')
				{
					num5++;
					num = num4 + 1;
				}
				num4++;
			}
		}
		int result = num;
		int num6 = num;
		while (num6 < this.displayString.Length && this.displayString[num6] != '\n')
		{
			if (!char.IsWhiteSpace(this.displayString[num6]))
			{
				result = num6 + 1;
				int num7 = num6 * 4;
				float num8 = this.vertices[num7].x + 0.5f * (this.vertices[num7 + 2].x - this.vertices[num7].x);
				if (num8 >= point.x)
				{
					result = num6;
					break;
				}
			}
			num6++;
		}
		return result;
	}

	public Vector3 GetNearestInsertionPointPos(Vector3 point, ref int insertionPt)
	{
		insertionPt = this.GetNearestInsertionPoint(point);
		return this.GetInsertionPointPos(insertionPt);
	}

	protected float GetLineBaseline(int numLines, int lineNum)
	{
		float num = (float)this.spriteFont.BaseHeight * this.worldUnitsPerTexel;
		float num2 = this.lineSpaceSize - num;
		float num3 = this.lineSpaceSize - this.characterSize;
		float num4 = this.characterSize * (float)numLines + num3 * ((float)numLines - 1f);
		switch (this.anchor)
		{
		case SpriteText.Anchor_Pos.Upper_Left:
		case SpriteText.Anchor_Pos.Upper_Center:
		case SpriteText.Anchor_Pos.Upper_Right:
			return (float)lineNum * -this.lineSpaceSize + num2;
		case SpriteText.Anchor_Pos.Middle_Left:
		case SpriteText.Anchor_Pos.Middle_Center:
		case SpriteText.Anchor_Pos.Middle_Right:
			return num4 * 0.5f + (float)lineNum * -this.lineSpaceSize + num2 * 0.5f + this.m_fParentHeight * 0.5f;
		case SpriteText.Anchor_Pos.Lower_Left:
		case SpriteText.Anchor_Pos.Lower_Center:
		case SpriteText.Anchor_Pos.Lower_Right:
			return num4 + (float)lineNum * -this.lineSpaceSize + this.m_fParentHeight;
		default:
			return 0f;
		}
	}

	protected void Layout_Single_Line()
	{
		if (this.spriteFont == null)
		{
			return;
		}
		Vector3 startPos = Vector3.zero;
		float num = (float)this.spriteFont.PixelSize * this.worldUnitsPerTexel;
		float num2 = this.spriteFont.GetWidth(this.displayString) * this.worldUnitsPerTexel * this.characterSpacing;
		this.totalWidth = num2;
		this.totalHeight = num;
		this.lastLineWidth = this.totalWidth;
		this.lastLinePosY = 0f;
		startPos = this.GetStartPos_SingleLine(num, num2);
		this.topLeft = startPos;
		this.bottomRight = new Vector3(this.topLeft.x + num2, this.topLeft.y - num);
		this.Layout_Line(startPos, this.displayString, 0);
		if (this.displayString.Length < this.capacity)
		{
			for (int i = this.displayString.Length; i < this.capacity; i++)
			{
				this.vertices[i * 4] = Vector3.zero;
				this.vertices[i * 4 + 1] = Vector3.zero;
				this.vertices[i * 4 + 2] = Vector3.zero;
				this.vertices[i * 4 + 3] = Vector3.zero;
			}
		}
	}

	protected void Layout_Multiline(string[] lines)
	{
		float[] array = new float[lines.Length];
		float num = 0f;
		Vector3 zero = Vector3.zero;
		int num2 = 0;
		float num3 = this.lineSpaceSize - this.characterSize;
		this.totalHeight = this.characterSize * (float)lines.Length + num3 * ((float)lines.Length - 1f);
		for (int i = 0; i < lines.Length; i++)
		{
			array[i] = this.spriteFont.GetWidth(lines[i]) * this.worldUnitsPerTexel * this.characterSpacing;
			if (num < array[i])
			{
				num = array[i];
			}
		}
		this.totalWidth = num;
		this.lastLineWidth = array[lines.Length - 1];
		this.lastLinePosY = this.totalHeight - (this.characterSize + num3);
		switch (this.anchor)
		{
		case SpriteText.Anchor_Pos.Upper_Left:
			zero = new Vector3(0f, 0f, this.offsetZ);
			this.topLeft = zero;
			this.bottomRight = new Vector3(num, -this.totalHeight, this.offsetZ);
			break;
		case SpriteText.Anchor_Pos.Upper_Center:
			zero = new Vector3(num * -0.5f + this.m_fParentWidth * 0.5f, 0f, this.offsetZ);
			this.topLeft = zero;
			this.bottomRight = new Vector3(num * 0.5f + this.m_fParentWidth * 0.5f, -this.totalHeight, this.offsetZ);
			break;
		case SpriteText.Anchor_Pos.Upper_Right:
			zero = new Vector3(-num + this.m_fParentWidth, 0f, this.offsetZ);
			this.topLeft = zero;
			this.bottomRight = new Vector3(0f + this.m_fParentWidth, -this.totalHeight, this.offsetZ);
			break;
		case SpriteText.Anchor_Pos.Middle_Left:
			zero = new Vector3(0f, this.totalHeight * 0.5f + this.m_fParentHeight * 0.5f, this.offsetZ);
			this.topLeft = zero;
			this.bottomRight = new Vector3(num * 0.5f, -this.totalHeight + this.m_fParentHeight * 0.5f, this.offsetZ);
			break;
		case SpriteText.Anchor_Pos.Middle_Center:
			zero = new Vector3(num * -0.5f + this.m_fParentWidth * 0.5f, this.totalHeight * 0.5f + this.m_fParentHeight * 0.5f, this.offsetZ);
			this.topLeft = zero;
			this.bottomRight = new Vector3(num * 0.5f + this.m_fParentWidth * 0.5f, this.totalHeight * -0.5f + this.m_fParentHeight * 0.5f, this.offsetZ);
			break;
		case SpriteText.Anchor_Pos.Middle_Right:
			zero = new Vector3(-num + this.m_fParentWidth, this.totalHeight * 0.5f + this.m_fParentHeight * 0.5f, this.offsetZ);
			this.topLeft = zero;
			this.bottomRight = new Vector3(0f + this.m_fParentWidth, this.totalHeight * -0.5f + this.m_fParentHeight * 0.5f, this.offsetZ);
			break;
		case SpriteText.Anchor_Pos.Lower_Left:
			zero = new Vector3(0f, this.totalHeight + this.m_fParentHeight, this.offsetZ);
			this.topLeft = zero;
			this.bottomRight = new Vector3(num, 0f + this.m_fParentHeight, this.offsetZ);
			break;
		case SpriteText.Anchor_Pos.Lower_Center:
			zero = new Vector3(num * -0.5f + this.m_fParentWidth * 0.5f, this.totalHeight + this.m_fParentHeight, this.offsetZ);
			this.topLeft = zero;
			this.bottomRight = new Vector3(num * 0.5f + this.m_fParentWidth * 0.5f, 0f + this.m_fParentHeight, this.offsetZ);
			break;
		case SpriteText.Anchor_Pos.Lower_Right:
			zero = new Vector3(-num + this.m_fParentWidth, this.totalHeight + this.m_fParentHeight, this.offsetZ);
			this.topLeft = zero;
			this.bottomRight = new Vector3(0f + this.m_fParentWidth, 0f + this.m_fParentHeight, this.offsetZ);
			break;
		}
		switch (this.alignment)
		{
		case SpriteText.Alignment_Type.Left:
			for (int j = 0; j < lines.Length; j++)
			{
				this.Layout_Line(zero, lines[j], num2);
				num2 += lines[j].Length + 1;
				this.ZeroQuad(num2 - 1);
				zero.y -= this.lineSpaceSize;
			}
			break;
		case SpriteText.Alignment_Type.Center:
			for (int k = 0; k < lines.Length; k++)
			{
				this.Layout_Line(zero + Vector3.right * 0.5f * (num - array[k]), lines[k], num2);
				num2 += lines[k].Length + 1;
				this.ZeroQuad(num2 - 1);
				zero.y -= this.lineSpaceSize;
			}
			break;
		case SpriteText.Alignment_Type.Right:
			for (int l = 0; l < lines.Length; l++)
			{
				this.Layout_Line(zero + Vector3.right * (num - array[l]), lines[l], num2);
				num2 += lines[l].Length + 1;
				this.ZeroQuad(num2 - 1);
				zero.y -= this.lineSpaceSize;
			}
			break;
		}
		if (num2 < this.capacity)
		{
			for (int m = num2; m < this.capacity; m++)
			{
				this.vertices[m * 4] = Vector3.zero;
				this.vertices[m * 4 + 1] = Vector3.zero;
				this.vertices[m * 4 + 2] = Vector3.zero;
				this.vertices[m * 4 + 3] = Vector3.zero;
			}
		}
	}

	protected void ZeroQuad(int i)
	{
		i *= 4;
		if (i >= this.vertices.Length)
		{
			return;
		}
		this.vertices[i] = (this.vertices[i + 1] = (this.vertices[i + 2] = (this.vertices[i + 3] = Vector3.zero)));
	}

	protected void BuildCharacter(int vertNum, int charNum, Vector3 upperLeft, ref SpriteChar ch)
	{
		this.vertices[vertNum] = upperLeft;
		this.vertices[vertNum + 1].x = upperLeft.x;
		this.vertices[vertNum + 1].y = upperLeft.y - ch.UVs.height * this.worldUnitsPerUV.y - this.fAddPixelHeight;
		this.vertices[vertNum + 1].z = upperLeft.z;
		this.vertices[vertNum + 2] = this.vertices[vertNum + 1];
		Vector3[] expr_B4_cp_0 = this.vertices;
		int expr_B4_cp_1 = vertNum + 2;
		expr_B4_cp_0[expr_B4_cp_1].x = expr_B4_cp_0[expr_B4_cp_1].x + ch.UVs.width * this.worldUnitsPerUV.x;
		this.vertices[vertNum + 3] = this.vertices[vertNum + 2];
		this.vertices[vertNum + 3].y = this.vertices[vertNum].y;
		this.UVs[vertNum].x = ch.UVs.x;
		this.UVs[vertNum].y = ch.UVs.yMax;
		this.UVs[vertNum + 1].x = ch.UVs.x;
		this.UVs[vertNum + 1].y = ch.UVs.y - this.fAddPixelHeight;
		this.UVs[vertNum + 2].x = ch.UVs.xMax;
		this.UVs[vertNum + 2].y = ch.UVs.y - this.fAddPixelHeight;
		this.UVs[vertNum + 3].x = ch.UVs.xMax;
		this.UVs[vertNum + 3].y = ch.UVs.yMax;
		this.meshColors[vertNum] = this.colors[charNum];
		this.meshColors[vertNum + 1] = this.colors[charNum];
		this.meshColors[vertNum + 2] = this.colors[charNum];
		this.meshColors[vertNum + 3] = this.colors[charNum];
		float a = ch.layer * 0.1f;
		if (ch.chnl == 1)
		{
			a = 0.2f;
		}
		else if (ch.chnl == 2)
		{
			a = 0.1f;
		}
		else if (ch.chnl == 4)
		{
			a = 0f;
		}
		else if (ch.chnl == 8)
		{
			a = 0.3f;
		}
		this.meshColors[vertNum].a = a;
		this.meshColors[vertNum + 1].a = a;
		this.meshColors[vertNum + 2].a = a;
		this.meshColors[vertNum + 3].a = a;
		if (this.clipped)
		{
			if (this.vertices[vertNum].x < this.localClipRect.x)
			{
				if (this.vertices[vertNum + 2].x < this.localClipRect.x)
				{
					this.vertices[vertNum].x = (this.vertices[vertNum + 1].x = this.vertices[vertNum + 2].x);
					return;
				}
				float t = (this.localClipRect.x - this.vertices[vertNum].x) / (this.vertices[vertNum + 2].x - this.vertices[vertNum].x);
				this.vertices[vertNum].x = (this.vertices[vertNum + 1].x = this.localClipRect.x);
				this.UVs[vertNum].x = (this.UVs[vertNum + 1].x = Mathf.Lerp(this.UVs[vertNum].x, this.UVs[vertNum + 2].x, t));
			}
			else if (this.vertices[vertNum + 2].x > this.localClipRect.xMax)
			{
				if (this.vertices[vertNum].x > this.localClipRect.xMax)
				{
					this.vertices[vertNum + 2].x = (this.vertices[vertNum + 3].x = this.vertices[vertNum].x);
					return;
				}
				float t2 = (this.localClipRect.xMax - this.vertices[vertNum].x) / (this.vertices[vertNum + 2].x - this.vertices[vertNum].x);
				this.vertices[vertNum + 2].x = (this.vertices[vertNum + 3].x = this.localClipRect.xMax);
				this.UVs[vertNum + 2].x = (this.UVs[vertNum + 3].x = Mathf.Lerp(this.UVs[vertNum].x, this.UVs[vertNum + 2].x, t2));
			}
			if (this.vertices[vertNum].y > this.localClipRect.yMax)
			{
				if (this.vertices[vertNum + 2].y > this.localClipRect.yMax)
				{
					this.vertices[vertNum].y = (this.vertices[vertNum + 3].y = this.vertices[vertNum + 2].y);
					return;
				}
				float t3 = (this.vertices[vertNum].y - this.localClipRect.yMax) / (this.vertices[vertNum].y - this.vertices[vertNum + 1].y);
				this.vertices[vertNum].y = (this.vertices[vertNum + 3].y = this.localClipRect.yMax);
				this.UVs[vertNum].y = (this.UVs[vertNum + 3].y = Mathf.Lerp(this.UVs[vertNum].y, this.UVs[vertNum + 1].y, t3));
			}
			else if (this.vertices[vertNum + 2].y < this.localClipRect.y)
			{
				if (this.vertices[vertNum].y < this.localClipRect.y)
				{
					this.vertices[vertNum + 1].y = (this.vertices[vertNum + 2].y = this.vertices[vertNum].y);
					return;
				}
				float t4 = (this.vertices[vertNum].y - this.localClipRect.y) / (this.vertices[vertNum].y - this.vertices[vertNum + 1].y);
				this.vertices[vertNum + 1].y = (this.vertices[vertNum + 2].y = this.localClipRect.y);
				this.UVs[vertNum + 1].y = (this.UVs[vertNum + 2].y = Mathf.Lerp(this.UVs[vertNum].y, this.UVs[vertNum + 1].y, t4));
			}
		}
	}

	protected void Layout_Line(Vector3 startPos, string txt, int charIdx)
	{
		if (txt.Length == 0)
		{
			return;
		}
		SpriteChar spriteChar = this.spriteFont.GetSpriteChar(txt[0]);
		Vector3 upperLeft = startPos + new Vector3(spriteChar.xOffset * this.worldUnitsPerTexel, spriteChar.yOffset * this.worldUnitsPerTexel);
		this.BuildCharacter(charIdx * 4, charIdx, upperLeft, ref spriteChar);
		for (int i = 1; i < txt.Length; i++)
		{
			startPos.x += spriteChar.xAdvance * this.worldUnitsPerTexel * this.characterSpacing;
			spriteChar = this.spriteFont.GetSpriteChar(txt[i]);
			startPos.x += spriteChar.GetKerning(txt[i - 1]) * this.worldUnitsPerTexel * this.characterSize;
			upperLeft = startPos + new Vector3(spriteChar.xOffset * this.worldUnitsPerTexel, spriteChar.yOffset * this.worldUnitsPerTexel);
			this.BuildCharacter((charIdx + i) * 4, charIdx + i, upperLeft, ref spriteChar);
		}
	}

	protected void ClearMesh()
	{
		if (this.vertices == null)
		{
			this.EnlargeMesh();
		}
		for (int i = 0; i < this.vertices.Length; i++)
		{
			this.vertices[i] = Vector3.zero;
			this.meshColors[i] = this.color;
		}
		this.topLeft = Vector3.zero;
		this.bottomRight = Vector3.zero;
		this.unclippedTL = Vector3.zero;
		this.unclippedBR = Vector3.zero;
	}

	public void Unclip()
	{
		if (this.ignoreClipping)
		{
			return;
		}
		this.clipped = false;
		this.updateClipping = true;
		this.UpdateMesh();
	}

	public void Delete()
	{
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(this.mesh);
			this.mesh = null;
		}
	}

	private void OnEnable()
	{
		if (this.parentControl == null)
		{
			return;
		}
	}

	protected virtual void OnDisable()
	{
		if (Application.isPlaying && EZAnimator.Exists())
		{
			EZAnimator.instance.Stop(base.gameObject);
			EZAnimator.instance.Stop(this);
		}
	}

	public virtual void OnDestroy()
	{
		this.Delete();
	}

	public virtual void Copy(SpriteText s)
	{
		this.offsetZ = s.offsetZ;
		this.characterSize = s.characterSize;
		this.lineSpacing = s.lineSpacing;
		this.lineSpaceSize = s.lineSpaceSize;
		this.anchor = s.anchor;
		this.alignment = s.alignment;
		this.tabSize = s.tabSize;
		this.texture = s.texture;
		this.SetPixelToUV(this.texture);
		this.font = s.font;
		this.spriteFont = FontStore.GetFont(this.font);
		this.lineSpaceSize = this.lineSpacing * (float)this.spriteFont.LineHeight * this.worldUnitsPerTexel;
		this.color = s.color;
		this.text = s.text;
		this.ProcessString(this.text);
		this.pixelPerfect = s.pixelPerfect;
		this.dynamicLength = s.dynamicLength;
		this.SetCamera(s.renderCamera);
		this.hideAtStart = s.hideAtStart;
		this.m_hidden = s.m_hidden;
		this.Hide(this.m_hidden);
	}

	public void CalcSize()
	{
		if (this.spriteFont == null)
		{
			return;
		}
		if (this.pixelPerfect)
		{
			this.characterSize = (float)this.spriteFont.PixelSize * this.worldUnitsPerScreenPixel;
			this.worldUnitsPerTexel = this.worldUnitsPerScreenPixel;
			this.worldUnitsPerUV.x = this.worldUnitsPerTexel * this.pixelsPerUV.x;
			this.worldUnitsPerUV.y = this.worldUnitsPerTexel * this.pixelsPerUV.y;
			this.fAddPixelHeight = 0.2f / this.worldUnitsPerUV.y;
		}
		this.lineSpaceSize = this.lineSpacing * (float)this.spriteFont.LineHeight * this.worldUnitsPerTexel;
		this.UpdateMesh();
	}

	protected void LayoutText()
	{
		this.stringContentChanged = true;
		this.ProcessString(this.text);
		this.UpdateMesh();
	}

	public void SetColor(Color c)
	{
		if (this.color != c)
		{
			this.color = c;
			if (TsPlatform.IsMobile && base.renderer.material.HasProperty("_Alpha"))
			{
				base.renderer.material.SetFloat("_Alpha", c.a);
			}
			this.updateColors = true;
			this.Text = this.text;
		}
	}

	public void SetCharacterSize(float size)
	{
		if (this.spriteFont == null)
		{
			return;
		}
		this.pixelPerfect = false;
		this.characterSize = size;
		this.SetPixelToUV(this.texture);
		this.lineSpaceSize = this.lineSpacing * (float)this.spriteFont.LineHeight * this.worldUnitsPerTexel;
		this.LayoutText();
	}

	public void SetLineSpacing(float spacing)
	{
		this.lineSpacing = spacing;
		this.lineSpaceSize = this.lineSpacing * (float)this.spriteFont.LineHeight * this.worldUnitsPerTexel;
		this.LayoutText();
	}

	public void SetFont(TextAsset newFont, Material fontMaterial)
	{
		this.SetFont(FontStore.GetFont(newFont), fontMaterial);
	}

	public void SetFont(Material fontMaterial)
	{
		this.SetFont(NrTSingleton<UIManager>.Instance.defaultSpriteFont, fontMaterial);
	}

	public void SetFont(SpriteFont newFont, Material fontMaterial)
	{
		this.spriteFont = newFont;
		this.SetTextMaterial(fontMaterial);
		this.lineSpaceSize = this.lineSpacing * (float)this.spriteFont.LineHeight * this.worldUnitsPerTexel;
		this.CalcSize();
		this.LayoutText();
	}

	public void SetTextMaterial(Material fontMaterial)
	{
		base.renderer.sharedMaterial = fontMaterial;
	}

	public void SetPixelToUV(int texWidth, int texHeight)
	{
		if (this.spriteFont == null)
		{
			return;
		}
		this.pixelsPerUV.x = (float)texWidth;
		this.pixelsPerUV.y = (float)texHeight;
		this.worldUnitsPerTexel = this.characterSize / (float)this.spriteFont.PixelSize;
		this.worldUnitsPerUV.x = this.worldUnitsPerTexel * (float)texWidth;
		this.worldUnitsPerUV.y = this.worldUnitsPerTexel * (float)texHeight;
		this.fAddPixelHeight = 0.2f / this.worldUnitsPerUV.y;
	}

	public void SetPixelToUV(Texture tex)
	{
		this.SetPixelToUV(tex.width, tex.height);
	}

	public void SetCamera()
	{
		this.SetCamera(this.renderCamera);
	}

	public void SetCamera(Camera c)
	{
		if (c == null || !this.m_started)
		{
			return;
		}
		Plane plane = new Plane(c.transform.forward, c.transform.position);
		float distanceToPoint;
		if (Application.isPlaying)
		{
			this.screenSize.x = c.pixelWidth;
			this.screenSize.y = c.pixelHeight;
			this.renderCamera = c;
			if (this.screenPlacer != null)
			{
				this.screenPlacer.SetCamera(this.renderCamera);
			}
			distanceToPoint = plane.GetDistanceToPoint(base.transform.position);
			this.worldUnitsPerScreenPixel = Vector3.Distance(c.ScreenToWorldPoint(new Vector3(0f, 1f, distanceToPoint)), c.ScreenToWorldPoint(new Vector3(0f, 0f, distanceToPoint)));
			this.CalcSize();
			return;
		}
		this.screenSize.x = c.pixelWidth;
		this.screenSize.y = c.pixelHeight;
		if (this.screenSize.x == 0f)
		{
			return;
		}
		this.renderCamera = c;
		if (this.screenPlacer != null)
		{
			this.screenPlacer.SetCamera(this.renderCamera);
		}
		distanceToPoint = plane.GetDistanceToPoint(base.transform.position);
		this.worldUnitsPerScreenPixel = Vector3.Distance(c.ScreenToWorldPoint(new Vector3(0f, 1f, distanceToPoint)), c.ScreenToWorldPoint(new Vector3(0f, 0f, distanceToPoint)));
		if (!this.hideAtStart)
		{
			this.CalcSize();
		}
	}

	public virtual void Hide(bool tf)
	{
		this.m_hidden = tf;
		this.meshRenderer.enabled = !tf;
	}

	public bool IsHidden()
	{
		return this.m_hidden;
	}

	private string ChangeNumberString(string value)
	{
		if (3 < value.Length)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int num2 = 0;
			for (int i = value.Length - 1; i >= 0; i--)
			{
				if (0 < num && num % 3 == 0)
				{
					stringBuilder.Insert(num2++, ",");
				}
				stringBuilder.Insert(num2++, value[i]);
				num++;
			}
			string text = stringBuilder.ToString();
			stringBuilder.Remove(0, text.Length);
			num2 = 0;
			for (int j = text.Length - 1; j >= 0; j--)
			{
				stringBuilder.Insert(num2++, text[j]);
			}
			return stringBuilder.ToString();
		}
		return value;
	}

	public Rect3D GetClippingRect()
	{
		return this.clippingRect;
	}

	public void SetClippingRect(Rect3D value)
	{
		if (this.ignoreClipping)
		{
			return;
		}
		this.clippingRect = value;
		this.clipped = true;
		this.updateClipping = true;
		this.UpdateMesh();
	}

	public bool IsClipped()
	{
		return this.clipped;
	}

	public void SetClipped(bool value)
	{
		if (this.ignoreClipping)
		{
			return;
		}
		if (value && !this.clipped)
		{
			this.clipped = true;
			this.updateClipping = true;
			this.UpdateMesh();
		}
		else if (this.clipped)
		{
			this.Unclip();
		}
	}

	public Vector3 GetInsertionPointPos(int charIndex)
	{
		if (this.spriteFont == null)
		{
			return Vector3.zero;
		}
		if (this.meshString.Length < 1 || (charIndex <= 0 && NkInputManager.compositionString == string.Empty))
		{
			float baseHeight = (float)this.spriteFont.BaseHeight * this.worldUnitsPerTexel;
			return base.transform.TransformPoint(this.GetStartPos_SingleLine(baseHeight, this.spriteFont.GetWidth(this.displayString) * this.worldUnitsPerTexel * this.characterSpacing).x, this.GetLineBaseline(1, 1), this.offsetZ);
		}
		int num = 1;
		float num2 = 0f;
		if (charIndex >= this.meshString.Length)
		{
			num = 0;
		}
		int lineNum;
		int num3;
		int end;
		int displayLineCount = this.GetDisplayLineCount(charIndex, out lineNum, out num3, out end);
		charIndex = Mathf.Min(charIndex, this.meshString.Length - 1);
		if (charIndex < num3)
		{
			this.GetDisplayLineCount(charIndex - 1, out lineNum, out num3, out end);
		}
		float num4 = this.spriteFont.GetWidth(this.displayString, num3, charIndex - num) * this.worldUnitsPerTexel * this.characterSpacing;
		if (NkInputManager.imeCompositionMode == IMECompositionMode.On && NkInputManager.compositionString != string.Empty && NrTSingleton<UIManager>.Instance.MaxLengthBool)
		{
			if (this.spriteFont.ContainsCharacter(NkInputManager.compositionString[0]))
			{
				num2 = this.spriteFont.GetWidth(NkInputManager.compositionString) * this.worldUnitsPerTexel * this.characterSpacing;
			}
			else
			{
				num2 = this.spriteFont.GetWidth("@") * this.worldUnitsPerTexel * this.characterSpacing;
			}
		}
		switch (this.anchor)
		{
		case SpriteText.Anchor_Pos.Upper_Left:
		case SpriteText.Anchor_Pos.Middle_Left:
		case SpriteText.Anchor_Pos.Lower_Left:
			if (this.alignment == SpriteText.Alignment_Type.Center)
			{
				num4 += this.totalWidth * 0.5f - this.spriteFont.GetWidth(this.displayString, num3, end) * this.worldUnitsPerTexel * this.characterSpacing * 0.5f;
			}
			else if (this.alignment == SpriteText.Alignment_Type.Right)
			{
				num4 += this.totalWidth - this.spriteFont.GetWidth(this.displayString, num3, end) * this.worldUnitsPerTexel * this.characterSpacing;
			}
			break;
		case SpriteText.Anchor_Pos.Upper_Center:
		case SpriteText.Anchor_Pos.Middle_Center:
		case SpriteText.Anchor_Pos.Lower_Center:
			if (this.alignment == SpriteText.Alignment_Type.Left)
			{
				num4 -= this.totalWidth * 0.5f;
			}
			else if (this.alignment != SpriteText.Alignment_Type.Right)
			{
				num4 += this.m_fParentWidth / 2f - this.maxWidth / 2f - this.totalWidth * 0.5f;
			}
			break;
		case SpriteText.Anchor_Pos.Upper_Right:
		case SpriteText.Anchor_Pos.Middle_Right:
		case SpriteText.Anchor_Pos.Lower_Right:
			if (this.alignment == SpriteText.Alignment_Type.Center)
			{
				num4 += this.totalWidth * -0.5f - this.spriteFont.GetWidth(this.displayString, num3, end) * this.worldUnitsPerTexel * this.characterSpacing * 0.5f;
			}
			else if (this.alignment == SpriteText.Alignment_Type.Left)
			{
				num4 -= this.totalWidth;
			}
			else
			{
				num4 += this.m_fParentWidth - this.totalWidth;
			}
			break;
		}
		return base.transform.TransformPoint(num4 + num2, this.GetLineBaseline(displayLineCount, lineNum), this.offsetZ);
	}

	public Vector3 GetNearestInsertionPoint(Vector3 point, ref int insertionPt)
	{
		Vector3 vector = base.transform.InverseTransformPoint(point);
		if (vector.x <= 0f)
		{
			insertionPt = 0;
			return base.transform.TransformPoint(Vector3.zero);
		}
		for (int i = 0; i <= this.meshString.Length; i++)
		{
			float num = this.spriteFont.GetWidth(this.meshString, 0, i) * this.worldUnitsPerTexel * this.characterSpacing;
			if (vector.x < num)
			{
				insertionPt = i;
				return this.GetInsertionPointPos(i);
			}
		}
		insertionPt = this.meshString.Length;
		return this.GetInsertionPointPos(this.meshString.Length);
	}

	public Vector3[] GetVertices()
	{
		return this.mesh.vertices;
	}

	public Vector3 GetCenterPoint()
	{
		return new Vector3(this.topLeft.x + 0.5f * (this.bottomRight.x - this.topLeft.x), this.topLeft.y - 0.5f * (this.topLeft.y - this.bottomRight.y), this.offsetZ);
	}

	public void SetAnchor(SpriteText.Anchor_Pos a)
	{
		this.anchor = a;
		this.LayoutText();
	}

	public void SetAlignment(SpriteText.Alignment_Type a)
	{
		this.alignment = a;
		this.LayoutText();
	}

	public Vector2 PixelSpaceToUVSpace(Vector2 xy)
	{
		if (this.pixelsPerUV.x == 0f || this.pixelsPerUV.y == 0f)
		{
			return Vector2.zero;
		}
		return new Vector2(xy.x / this.pixelsPerUV.x, xy.y / this.pixelsPerUV.y);
	}

	public Vector2 PixelSpaceToUVSpace(int x, int y)
	{
		return this.PixelSpaceToUVSpace(new Vector2((float)x, (float)y));
	}

	public Vector2 PixelCoordToUVCoord(Vector2 xy)
	{
		if (this.pixelsPerUV.x == 0f || this.pixelsPerUV.y == 0f)
		{
			return Vector2.zero;
		}
		return new Vector2(xy.x / (this.pixelsPerUV.x - 1f), 1f - xy.y / (this.pixelsPerUV.y - 1f));
	}

	public Vector2 PixelCoordToUVCoord(int x, int y)
	{
		return this.PixelCoordToUVCoord(new Vector2((float)x, (float)y));
	}

	public void SetFontEffect(SpriteText.Font_Effect eFontEffect)
	{
		switch (eFontEffect)
		{
		case SpriteText.Font_Effect.Black_Shadow_Small:
			if (this.shadowText)
			{
				this.color = Color.black;
			}
			break;
		case SpriteText.Font_Effect.White_Shadow_Small:
			if (this.shadowText)
			{
				this.color = Color.white;
			}
			break;
		case SpriteText.Font_Effect.Black_Shadow_Big:
			if (this.shadowText)
			{
				this.color = Color.black;
			}
			break;
		case SpriteText.Font_Effect.White_Shadow_Big:
			if (this.shadowText)
			{
				this.color = Color.white;
			}
			break;
		case SpriteText.Font_Effect.Color_Shadow_Green:
			if (this.shadowText)
			{
				this.color = Color.green;
			}
			break;
		case SpriteText.Font_Effect.Color_Shadow_Red:
			if (this.shadowText)
			{
				this.color = Color.red;
			}
			break;
		case SpriteText.Font_Effect.HeadUp:
			break;
		default:
			if (this.shadowText)
			{
				this.color = Color.black;
			}
			break;
		}
		this.SetTextMaterial(NrTSingleton<UIManager>.Instance.defaultFontMaterial);
	}

	public void SetFontEffect(int nFontEffect)
	{
		this.SetFontEffect((SpriteText.Font_Effect)nFontEffect);
	}

	public virtual void DoMirror()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.screenSize.x == 0f || this.screenSize.y == 0f)
		{
			this.Start();
		}
		if (this.mirror == null)
		{
			this.mirror = new SpriteTextMirror();
			this.mirror.Mirror(this);
		}
		this.mirror.Validate(this);
		if (this.mirror.DidChange(this))
		{
			this.stringContentChanged = true;
			this.Init();
			this.meshString = string.Empty;
			this.CalcSize();
			this.mirror.Mirror(this);
		}
	}

	public virtual void OnDrawGizmosSelected()
	{
		this.DoMirror();
	}

	public virtual void OnDrawGizmos()
	{
		this.DoMirror();
	}

	public float GetTextWidth()
	{
		return this.spriteFont.GetWidth(this.displayString) * this.worldUnitsPerTexel * this.characterSpacing;
	}

	public float GetTextWidthStringLength(int index)
	{
		if (this.displayString.Length < 1 || index > this.displayString.Length)
		{
			return 0f;
		}
		float num = this.spriteFont.GetSpriteChar(this.displayString[0]).xAdvance;
		for (int i = 1; i < index; i++)
		{
			SpriteChar spriteChar = this.spriteFont.GetSpriteChar(this.displayString[i]);
			num += spriteChar.xAdvance + spriteChar.GetKerning(this.displayString[i - 1]);
		}
		return num * this.worldUnitsPerTexel * this.characterSpacing;
	}
}
