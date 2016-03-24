using GAME;
using GameMessage;
using System;
using TsBundle;
using UnityEngine;

namespace UnityForms
{
	public class ItemTexture : AutoSpriteControlBase
	{
		private Label upText;

		private Label downText;

		private DrawTexture EventMark;

		[HideInInspector]
		public TextureAnim[] states = new TextureAnim[]
		{
			new TextureAnim("Normal")
		};

		[HideInInspector]
		public EZTransitionList[] transitions = new EZTransitionList[1];

		private bool reserve;

		private string imageKey = string.Empty;

		private bool bInjury;

		private bool bHaveEffect;

		private eCharImageType m_etype;

		public override bool Visible
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		public override TextureAnim[] States
		{
			get
			{
				return this.states;
			}
			set
			{
				this.states = value;
			}
		}

		public bool DownText_Visible
		{
			get
			{
				return this.downText.Visible;
			}
			set
			{
				this.downText.Visible = value;
			}
		}

		public override EZTransitionList[] Transitions
		{
			get
			{
				return this.transitions;
			}
			set
			{
				this.transitions = value;
			}
		}

		public override IUIContainer Container
		{
			get
			{
				return base.Container;
			}
			set
			{
				if (value != this.container)
				{
					if (this.container != null)
					{
						this.container.RemoveChild(this.upText.gameObject);
						this.container.RemoveChild(this.downText.gameObject);
						this.container.RemoveChild(this.EventMark.gameObject);
					}
					if (value != null)
					{
						if (this.upText != null)
						{
							value.AddChild(this.upText.gameObject);
						}
						if (this.downText != null)
						{
							value.AddChild(this.downText.gameObject);
						}
						if (this.EventMark != null)
						{
							value.AddChild(this.EventMark.gameObject);
						}
					}
				}
				base.Container = value;
			}
		}

		public override Rect3D GetClippingRect()
		{
			return base.GetClippingRect();
		}

		public override void SetClippingRect(Rect3D value)
		{
			if (this.ignoreClipping)
			{
				return;
			}
			base.SetClippingRect(value);
			Transform transform = base.transform.FindChild(NrTSingleton<UIDataManager>.Instance.AttachEffectKeyName);
			if (null != transform)
			{
				Vector3[] v3TotalVertices = this.m_v3TotalVertices;
				Vector3 vector = v3TotalVertices[1];
				Vector3 vector2 = v3TotalVertices[3];
				float num = vector2.y - vector.y;
				if (this.height > num)
				{
					transform.gameObject.SetActive(false);
				}
				else
				{
					transform.gameObject.SetActive(true);
				}
			}
		}

		public override EZTransitionList GetTransitions(int index)
		{
			if (index >= this.transitions.Length)
			{
				return null;
			}
			return this.transitions[index];
		}

		public static ItemTexture Create(string name, Vector3 pos)
		{
			return (ItemTexture)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(ItemTexture));
		}

		protected override void Awake()
		{
			this.multiLine = false;
			base.Awake();
		}

		public override void Start()
		{
			if (this.m_started)
			{
				return;
			}
			base.Start();
			if (Application.isPlaying)
			{
				base.gameObject.layer = GUICamera.UILayer;
				this.anchor = SpriteRoot.ANCHOR_METHOD.UPPER_LEFT;
				base.SetTexture("Com_I_Transparent");
				this.upText = UICreateControl.Label("upText", " ", false, this.width, this.fontSize, this.defaultFontEffect, SpriteText.Anchor_Pos.Upper_Left, SpriteText.Alignment_Type.Left, Color.white);
				this.upText.UpdateText = true;
				this.upText.gameObject.transform.parent = base.transform;
				this.upText.gameObject.transform.localPosition = new Vector3(2f, -2f, -0.02f);
				this.downText = UICreateControl.Label("downText", " ", false, this.width - 2f, this.fontSize, this.defaultFontEffect, SpriteText.Anchor_Pos.Upper_Right, SpriteText.Alignment_Type.Right, Color.white);
				this.downText.UpdateText = true;
				this.downText.gameObject.transform.parent = base.transform;
				this.downText.gameObject.transform.localPosition = new Vector3(0f, -(this.height - this.fontSize - 2f), -0.02f);
				this.EventMark = UICreateControl.DrawTexture("EventMark", "Com_I_Transparent");
				this.EventMark.gameObject.transform.parent = base.transform;
				this.EventMark.gameObject.transform.localPosition = new Vector3(0f, 0f, -0.02f);
				this.EventMark.Visible = false;
				if (this.container != null)
				{
					this.container.AddChild(this.upText.gameObject);
					this.container.AddChild(this.downText.gameObject);
					this.container.AddChild(this.EventMark.gameObject);
				}
				this.SetState(0);
			}
		}

		public void SetItemTexture(int unique)
		{
			base.SetTexture(MsgHandler.HandleReturn<UIBaseInfoLoader>("GetItemTexture", new object[]
			{
				unique
			}));
		}

		public void SetItemTexture(ITEM item, bool showItemNum, bool showItemLevel = true, float ResizeRate = 1f)
		{
			if (item != null)
			{
				base.SetTexture(MsgHandler.HandleReturn<UIBaseInfoLoader>("GetItemTexture", new object[]
				{
					item.m_nItemUnique
				}));
				this.Data = item;
				int num = item.m_nOption[2];
				this.downText.Text = string.Empty;
				this.upText.Text = string.Empty;
				UIBaseInfoLoader uIBaseInfoLoader = MsgHandler.HandleReturn<UIBaseInfoLoader>("GetLegendItemGrade", new object[]
				{
					item.m_nItemUnique
				});
				if (uIBaseInfoLoader != null)
				{
					this.upText.SetSize(uIBaseInfoLoader.UVs.width * ResizeRate, uIBaseInfoLoader.UVs.height * ResizeRate);
					this.upText.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
					this.upText.SetTexture(uIBaseInfoLoader);
					this.upText.DeleteSpriteText();
					this.downText.Visible = true;
					this.downText.SetCharacterSize(20f);
					if (showItemLevel)
					{
						this.downText.Text = NrTSingleton<UIDataManager>.Instance.GetString("Lv.", MsgHandler.HandleReturn<string>("GetUseMinLevel", new object[]
						{
							item
						}));
					}
				}
				else if ("true" == MsgHandler.HandleReturn<string>("IsRank", new object[]
				{
					item.m_nItemUnique
				}) && num >= 1)
				{
					this.upText.Visible = true;
					string @string = NrTSingleton<UIDataManager>.Instance.GetString("Win_I_WorrGrade", MsgHandler.HandleReturn<string>("ChangeRankToString", new object[]
					{
						num
					}));
					UIBaseInfoLoader uIBaseInfoLoader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(@string);
					if (uIBaseInfoLoader2 != null)
					{
						this.upText.SetSize(20f, 20f);
						this.upText.SetTexture(uIBaseInfoLoader2);
						this.upText.DeleteSpriteText();
					}
					this.downText.Visible = true;
					this.downText.SetCharacterSize(20f);
					this.downText.Text = NrTSingleton<UIDataManager>.Instance.GetString("Lv.", MsgHandler.HandleReturn<string>("GetUseMinLevel", new object[]
					{
						item
					}));
				}
				else
				{
					this.upText.Visible = false;
					if (showItemNum)
					{
						this.downText.Visible = true;
						this.downText.SetCharacterSize(20f);
						this.downText.Text = item.m_nItemNum.ToString();
					}
				}
				this.downText.gameObject.transform.localPosition = new Vector3(0f, -(this.height - this.fontSize - 2f), -0.02f);
			}
		}

		public void SetItemTexture(int ItemUnique, int MInLevel, bool isMinLevelShow = true, float ResizeRate = 1f)
		{
			base.SetTexture(MsgHandler.HandleReturn<UIBaseInfoLoader>("GetItemTexture", new object[]
			{
				ItemUnique
			}));
			this.downText.Text = string.Empty;
			this.upText.Text = string.Empty;
			UIBaseInfoLoader uIBaseInfoLoader = MsgHandler.HandleReturn<UIBaseInfoLoader>("GetLegendItemGrade", new object[]
			{
				ItemUnique
			});
			if (uIBaseInfoLoader != null)
			{
				this.upText.SetSize(uIBaseInfoLoader.UVs.width * ResizeRate, uIBaseInfoLoader.UVs.height * ResizeRate);
				this.upText.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
				this.upText.SetTexture(uIBaseInfoLoader);
				this.upText.DeleteSpriteText();
				if (isMinLevelShow)
				{
					this.downText.Visible = true;
					this.downText.SetCharacterSize(20f);
					this.downText.Text = NrTSingleton<UIDataManager>.Instance.GetString("Lv.", MInLevel.ToString());
					this.downText.gameObject.transform.localPosition = new Vector3(0f, -(this.height - this.fontSize - 2f), -0.02f);
				}
			}
		}

		public void SetTextureFromBundle(string bundlepath)
		{
			this.imageKey = bundlepath;
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(bundlepath);
			if (null != texture)
			{
				base.SetTexture(texture);
			}
			else
			{
				NrTSingleton<UIImageBundleManager>.Instance.RequestBundleImage(this.imageKey, new PostProcPerItem(this.SetImage));
			}
		}

		private void SetImage(WWWItem _item, object _param)
		{
			if (null == this)
			{
				return;
			}
			if (_item.isCanceled)
			{
				return;
			}
			if (_item.GetSafeBundle() != null)
			{
				Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
				if (null != texture2D)
				{
					base.SetTexture(texture2D);
					string text = string.Empty;
					if (_param is string)
					{
						text = (string)_param;
						NrTSingleton<UIImageBundleManager>.Instance.AddTexture(text, texture2D);
					}
				}
			}
		}

		public void SetItemTexture(ITEM item)
		{
			this.SetItemTexture(item, true, true, 1f);
		}

		public void SetItemTexture(string imageKey1, float width, float height)
		{
			UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(imageKey1);
			if (uIBaseInfoLoader != null)
			{
				this.upText.SetSize(width, height);
				this.upText.SetTexture(uIBaseInfoLoader);
				this.upText.DeleteSpriteText();
			}
		}

		public void SetItemTexture(string imageKey1, string imageKey2)
		{
			UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(imageKey1);
			if (uIBaseInfoLoader != null)
			{
				base.SetTexture(uIBaseInfoLoader);
			}
			uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(imageKey2);
			if (uIBaseInfoLoader != null)
			{
				this.upText.SetSize(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
				this.upText.SetTexture(uIBaseInfoLoader);
				this.upText.DeleteSpriteText();
			}
		}

		private void DrawHero(Texture2D tex)
		{
			base.SetTexture(tex);
			if (this.bInjury)
			{
				base.SetMatColor(new Color(1f, 0.235294119f, 0.235294119f, 0.7f));
			}
			else
			{
				base.SetMatColor(new Color(0.5f, 0.5f, 0.5f, 1f));
			}
		}

		private void Update()
		{
			if (string.Empty != this.imageKey && this.reserve)
			{
				Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.imageKey);
				if (null == texture)
				{
					this.reserve = false;
					NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.imageKey, this.m_etype, new PostProcPerItem(this.SetCharImage));
				}
				else
				{
					if (null == this.texMat)
					{
						return;
					}
					if (null == this.texMat.mainTexture)
					{
						this.DrawHero(texture);
					}
					else if (this.imageKey != this.texMat.mainTexture.name)
					{
						this.DrawHero(texture);
					}
				}
			}
		}

		private void SolInjuryStatus(bool Injury)
		{
			this.bInjury = Injury;
		}

		private void SolOtherInfo(NkListSolInfo solInfo)
		{
			if (solInfo.ShowCombat)
			{
				UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_BattlePower");
				if (uIBaseInfoLoader != null)
				{
					this.downText.Visible = true;
					this.downText.SetTexture(uIBaseInfoLoader);
					this.downText.SetSize(this.width, uIBaseInfoLoader.UVs.height);
					this.downText.gameObject.transform.localPosition = new Vector3(0f, 0f, -0.02f);
					this.downText.SetAnchor(SpriteText.Anchor_Pos.Upper_Left);
					this.downText.SetAlignment(SpriteText.Alignment_Type.Left);
					this.downText.spriteText.gameObject.transform.localPosition = new Vector3(24f, -2f, -0.02f);
					this.downText.SetCharacterSize(20f);
					this.downText.Text = solInfo.FightPower.ToString();
				}
			}
			else if (solInfo.ShowLevel)
			{
				this.downText.Visible = true;
				this.downText.SetAnchor(SpriteText.Anchor_Pos.Upper_Left);
				this.downText.SetAlignment(SpriteText.Alignment_Type.Left);
				this.downText.SetSize(this.width, this.fontSize);
				this.downText.gameObject.transform.localPosition = new Vector3(0f, 0f, -0.02f);
				this.downText.spriteText.gameObject.transform.localPosition = new Vector3(2f, -2f, -0.02f);
				this.downText.Text = NrTSingleton<UIDataManager>.Instance.GetString("Lv ", solInfo.SolLevel.ToString());
			}
		}

		private void ShowLegendMark(short type)
		{
			this.EventMark.Visible = true;
			UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_LegendFrame");
			if (type == 1)
			{
				if (uIBaseInfoLoader != null)
				{
					this.EventMark.SetSize(this.width, this.height);
					this.EventMark.SetTexture(uIBaseInfoLoader);
				}
			}
			else if (type == 2 && uIBaseInfoLoader != null)
			{
				this.EventMark.SetSize(this.width, this.height);
				this.EventMark.SetTexture(uIBaseInfoLoader);
			}
		}

		public void SetSolImageTexure(eCharImageType type, NkListSolInfo solInfo)
		{
			this.SetSolImageTexure(type, solInfo, false);
		}

		public void SetSolImageTexure(eCharImageType type, NkListSolInfo solInfo, bool bShowEventMark)
		{
			if (solInfo == null)
			{
				return;
			}
			this.SetSolImageTexure(type, solInfo.SolCharKind, solInfo.SolGrade, solInfo.SolInjuryStatus, solInfo);
			this.SolOtherInfo(solInfo);
			this.SolEventMark(bShowEventMark);
		}

		private void SolEventMark(bool eventMark)
		{
			if (eventMark)
			{
				this.EventMark.Visible = true;
				this.EventMark.SetSize(this.width * 0.45f, this.height * 0.45f);
				this.EventMark.SetTexture("Win_I_Notice05");
				this.EventMark.gameObject.transform.localPosition = new Vector3(this.width * 0.8f, this.height * 0.1f);
			}
		}

		public void SetSolImageTexure(eCharImageType type, int charkind, int solgrade, bool injury, NkListSolInfo solInfo)
		{
			this.reserve = false;
			this.imageKey = string.Empty;
			string text = MsgHandler.HandleReturn<string>("PortraitFileName", new object[]
			{
				charkind,
				solgrade
			});
			if (string.Empty == text)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"fail find SolImage = ",
					charkind.ToString(),
					" SolGrade = ",
					solgrade,
					" ",
					Time.time
				}));
				this.ClearData();
				return;
			}
			if (solInfo != null && !string.IsNullOrEmpty(solInfo.SolCostumePortraitPath))
			{
				string text2 = MsgHandler.HandleReturn<string>("PortraitCostumeFileName", new object[]
				{
					charkind,
					solgrade,
					solInfo.SolCostumePortraitPath
				});
				if (!string.IsNullOrEmpty(text2))
				{
					text = text2;
				}
			}
			this.SolInjuryStatus(injury);
			this.m_etype = type;
			if (type == eCharImageType.SMALL)
			{
				this.imageKey = text + "_64";
			}
			else if (type == eCharImageType.MIDDLE)
			{
				this.imageKey = text + "_256";
			}
			else if (type == eCharImageType.LARGE)
			{
				if (UIDataManager.IsUse256Texture())
				{
					this.imageKey = text + "_256";
				}
				else
				{
					this.imageKey = text + "_512";
				}
			}
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.imageKey);
			if (null != texture)
			{
				this.DrawHero(texture);
				this.reserve = true;
			}
			else
			{
				NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.imageKey, type, new PostProcPerItem(this.SetCharImage));
			}
			string s = MsgHandler.HandleReturn<string>("GetLegendTypeToString", new object[]
			{
				charkind,
				solgrade
			});
			short num = short.Parse(s);
			if (0 < num)
			{
				Transform y = base.transform.FindChild(NrTSingleton<UIDataManager>.Instance.AttachEffectKeyName);
				if (null == y && !this.bHaveEffect)
				{
					if (type == eCharImageType.SMALL)
					{
						NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("Effect/Instant/fx_legendhero_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath, this, base.GetSize());
					}
					else
					{
						NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("Effect/Instant/fx_herolegendblg_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath, this, base.GetSize());
					}
					this.AddGameObjectDelegate(new EZGameObjectDelegate(NrTSingleton<UIDataManager>.Instance.ResizeEffect));
					this.bHaveEffect = true;
				}
			}
			else
			{
				Transform transform = base.transform.FindChild(NrTSingleton<UIDataManager>.Instance.AttachEffectKeyName);
				if (null != transform)
				{
					UnityEngine.Object.Destroy(transform.gameObject);
					this.bHaveEffect = false;
				}
			}
			if ("true" == MsgHandler.HandleReturn<string>("CharKindIsATB", new object[]
			{
				charkind,
				128L
			}))
			{
				this.upText.Visible = false;
			}
			else
			{
				UIBaseInfoLoader uIBaseInfoLoader = MsgHandler.HandleReturn<UIBaseInfoLoader>("GetSolGradeImg", new object[]
				{
					charkind,
					solgrade
				});
				if (null != this.downText && null != this.upText)
				{
					if (uIBaseInfoLoader != null)
					{
						this.upText.Visible = true;
						this.downText.Visible = false;
						this.upText.SetSize(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
						this.upText.SetTexture(uIBaseInfoLoader);
						if (0 < num)
						{
							this.upText.gameObject.transform.localPosition = new Vector3(-6f, -(this.height - uIBaseInfoLoader.UVs.height + 6f), -0.02f);
						}
						else
						{
							this.upText.gameObject.transform.localPosition = new Vector3(2f, -(this.height - uIBaseInfoLoader.UVs.height - 2f), -0.02f);
						}
						this.upText.DeleteSpriteText();
					}
					else
					{
						this.upText.Visible = false;
						this.downText.Visible = false;
					}
				}
			}
		}

		public void SetSolImageTexure(eCharImageType type, int charkind, int solgrade)
		{
			this.SetSolImageTexure(type, charkind, solgrade, false, null);
		}

		public void SetSolImageTexure(eCharImageType type, int charkind, int solgrade, int level)
		{
			this.SetSolImageTexure(type, charkind, solgrade);
			this.downText.Visible = true;
			this.downText.SetAnchor(SpriteText.Anchor_Pos.Upper_Left);
			this.downText.SetAlignment(SpriteText.Alignment_Type.Left);
			this.downText.spriteText.gameObject.transform.localPosition = new Vector3(2f, this.height - this.fontSize - 2f, -0.02f);
			this.downText.Text = NrTSingleton<UIDataManager>.Instance.GetString("Lv ", level.ToString());
		}

		public void SetItemTexture(UIBaseInfoLoader imageLoader1, UIBaseInfoLoader imageLoader2)
		{
			if (imageLoader1 != null)
			{
				base.SetTexture(imageLoader1);
			}
			if (imageLoader2 != null)
			{
				this.upText.SetSize(imageLoader2.UVs.width, imageLoader2.UVs.height);
				this.upText.SetTexture(imageLoader2);
				this.upText.DeleteSpriteText();
			}
		}

		public void SetItemTexture(Texture2D Tex2d1, UIBaseInfoLoader imageLoader2)
		{
			if (null != Tex2d1)
			{
				base.SetTexture(Tex2d1);
			}
			if (imageLoader2 != null)
			{
				this.upText.SetSize(imageLoader2.UVs.width, imageLoader2.UVs.height);
				this.upText.SetTexture(imageLoader2);
				this.upText.DeleteSpriteText();
			}
		}

		public void SetEventImageTexure(Texture2D Tex2d, short nLevel, bool showEventMark)
		{
			if (null != Tex2d)
			{
				base.SetTexture(Tex2d);
			}
			if (showEventMark)
			{
			}
			if (nLevel != 0)
			{
				this.downText.Visible = true;
				this.downText.SetAnchor(SpriteText.Anchor_Pos.Upper_Left);
				this.downText.SetAlignment(SpriteText.Alignment_Type.Left);
				this.downText.spriteText.gameObject.transform.localPosition = new Vector3(2f, this.height - this.fontSize - 2f, -0.02f);
				this.downText.Text = NrTSingleton<UIDataManager>.Instance.GetString("Lv ", nLevel.ToString());
			}
		}

		public void ClearData()
		{
			this.reserve = false;
			this.imageKey = string.Empty;
			this.bInjury = false;
			base.SetTexture("Com_I_Transparent");
			if (this.downText.spriteText)
			{
				this.downText.Text = string.Empty;
			}
			this.downText.SetTexture("Com_I_Transparent");
			if (this.upText.spriteText)
			{
				this.upText.Text = string.Empty;
			}
			this.upText.SetTexture("Com_I_Transparent");
			this.EventMark.SetTexture("Com_I_Transparent");
			this.data = null;
			Transform transform = base.transform.FindChild(NrTSingleton<UIDataManager>.Instance.AttachEffectKeyName);
			if (null != transform)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
				this.bHaveEffect = false;
			}
		}

		public override void OnInput(ref POINTER_INFO ptr)
		{
			if (this.deleted)
			{
				return;
			}
			if (!this.m_controlIsEnabled || base.IsHidden())
			{
				base.OnInput(ref ptr);
				return;
			}
			if (this.inputDelegate != null)
			{
				this.inputDelegate(ref ptr);
			}
			if (!this.m_controlIsEnabled || base.IsHidden())
			{
				base.OnInput(ref ptr);
				return;
			}
			POINTER_INFO.INPUT_EVENT evt = ptr.evt;
			switch (evt)
			{
			case POINTER_INFO.INPUT_EVENT.PRESS:
				if (this.mouseDownDelegate != null)
				{
					this.mouseDownDelegate(this);
				}
				goto IL_108;
			case POINTER_INFO.INPUT_EVENT.DOUBLE_PRESS:
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.RIGHT_RELEASE:
				IL_86:
				if (evt != POINTER_INFO.INPUT_EVENT.MOVE)
				{
					goto IL_108;
				}
				if (this.mouseOverDelegate != null)
				{
					this.mouseOverDelegate(this);
				}
				goto IL_108;
			case POINTER_INFO.INPUT_EVENT.RIGHT_PRESS:
				if (this.rightMouseDelegate != null)
				{
					this.rightMouseDelegate(this);
				}
				goto IL_108;
			case POINTER_INFO.INPUT_EVENT.TAP:
				if (this.changeDelegate != null)
				{
					this.changeDelegate(this);
				}
				goto IL_108;
			}
			goto IL_86;
			IL_108:
			base.OnInput(ref ptr);
		}

		private void SetCharImage(WWWItem _item, object _param)
		{
			if (null == this)
			{
				return;
			}
			if (_item.isCanceled)
			{
				this.reserve = true;
				return;
			}
			if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
			{
				Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
				if (null != texture2D)
				{
					this.DrawHero(texture2D);
					string text = string.Empty;
					if (_param is string)
					{
						text = (string)_param;
						NrTSingleton<UIImageBundleManager>.Instance.AddTexture(text, texture2D);
					}
				}
			}
			this.reserve = true;
		}

		public void UsedCollider(bool use)
		{
			if (use)
			{
				base.SetUseBoxCollider(true);
				this.AddCollider();
			}
			else
			{
				base.SetUseBoxCollider(false);
				BoxCollider obj = (BoxCollider)base.gameObject.GetComponent(typeof(BoxCollider));
				UnityEngine.Object.Destroy(obj);
			}
		}

		protected override void AddCollider()
		{
			base.AddCollider();
		}

		public void ResizeEffect(IUIObject control, GameObject obj)
		{
			if (obj == null)
			{
				return;
			}
			ItemTexture itemTexture = (ItemTexture)control;
			if (null == itemTexture)
			{
				return;
			}
			if (itemTexture.GetSize().x == 115f)
			{
				obj.transform.localScale = new Vector3(1.6f, 1.6f, 1f);
				obj.transform.localPosition = new Vector3(58f, -58f, obj.transform.localPosition.z);
				Transform transform = obj.transform.FindChild("fx_aura_01");
				if (null != transform)
				{
					transform.transform.localPosition = new Vector3(58f, -58f, transform.transform.localPosition.z);
				}
			}
			else if (itemTexture.GetSize().x == 504f && itemTexture.GetSize().y == 448f)
			{
				obj.transform.localScale = new Vector3(0.98f, 0.98f, 1f);
				obj.transform.localPosition = new Vector3(251f, -210f, obj.transform.localPosition.z);
			}
			else if (itemTexture.GetSize().x == 512f && itemTexture.GetSize().y == 512f)
			{
				obj.transform.localScale = new Vector3(1f, 1.12f, 1f);
				obj.transform.localPosition = new Vector3(256f, -242f, obj.transform.localPosition.z);
			}
			else if (itemTexture.GetSize().x == 424f && itemTexture.GetSize().y == 432f)
			{
				obj.transform.localScale = new Vector3(0.82f, 0.95f, 1f);
				obj.transform.localPosition = new Vector3(211f, -204f, obj.transform.localPosition.z);
			}
			else if (itemTexture.GetSize().x == 315f && itemTexture.GetSize().y == 315f)
			{
				obj.transform.localScale = new Vector3(0.6f, 0.68f, 1f);
				obj.transform.localPosition = new Vector3(157.5f, -151f, obj.transform.localPosition.z);
			}
			else
			{
				obj.transform.localScale = new Vector3(itemTexture.GetSize().x / 64f - 0.1f, itemTexture.GetSize().x / 64f - 0.1f, 1f);
			}
		}
	}
}
