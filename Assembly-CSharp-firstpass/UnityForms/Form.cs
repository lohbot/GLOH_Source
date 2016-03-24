using GameMessage;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TsBundle;
using UnityEngine;

namespace UnityForms
{
	public abstract class Form
	{
		public enum ChildLocation
		{
			TOP,
			BOTTOM,
			LEFT,
			RIGHT,
			LEFTTOP,
			CENTER
		}

		private UIInteractivePanel interactivePanel;

		public float oldYPos;

		private float visibleYPos = -2000f;

		private int windowID;

		private G_ID orignalID;

		protected Dictionary<string, IUIObject> EZControlDictionary = new Dictionary<string, IUIObject>();

		private Vector3 position = Vector3.zero;

		private Vector2 size = Vector2.zero;

		public bool visible = true;

		private bool alwaysUpdate;

		private bool checkMouseEvent = true;

		private bool changeSceneDestory = true;

		private string filename = string.Empty;

		private bool autoAni;

		private bool closeAni = true;

		private int showSceneType = FormsManager.FORM_TYPE_POPUP;

		private bool bUseUpdateFrame;

		public Button closeButton;

		public bool bDestroy;

		private AnimatePosition pkAnimate;

		private bool topMost;

		private SimpleSprite _backGround;

		private Box _black_backGround;

		private Form.ChildLocation childFormLocation;

		private int myMainWindowID;

		private byte IconPosType;

		private bool icon;

		private int myIconWindowID;

		private bool haveIcon;

		private bool bShowHide;

		private bool bScale = true;

		private Vector3 oldScale = Vector3.zero;

		private Vector3 oldPos = Vector3.zero;

		private bool bStartAni;

		private bool setAni;

		public static event EventHandler OpenCallback
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				Form.OpenCallback = (EventHandler)Delegate.Combine(Form.OpenCallback, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				Form.OpenCallback = (EventHandler)Delegate.Remove(Form.OpenCallback, value);
			}
		}

		public bool AutoAni
		{
			set
			{
				this.autoAni = value;
			}
		}

		public bool bCloseAni
		{
			set
			{
				this.closeAni = value;
			}
		}

		public int ShowSceneType
		{
			get
			{
				return this.showSceneType;
			}
			set
			{
				this.showSceneType = value;
			}
		}

		public bool UseUpdateFrame
		{
			get
			{
				return this.bUseUpdateFrame;
			}
			set
			{
				this.bUseUpdateFrame = value;
			}
		}

		public int p_nCharKind
		{
			get;
			set;
		}

		public int p_nSelectIndex
		{
			get;
			set;
		}

		public bool ChangeSceneDestory
		{
			get
			{
				return this.changeSceneDestory;
			}
			set
			{
				this.changeSceneDestory = value;
			}
		}

		public bool IsMove
		{
			get
			{
				return this.pkAnimate != null && this.pkAnimate.running;
			}
		}

		public bool TopMost
		{
			get
			{
				return this.topMost;
			}
			set
			{
				this.topMost = value;
				if (null != this.InteractivePanel)
				{
					this.InteractivePanel.topMost = value;
				}
				if (this.topMost)
				{
					NrTSingleton<FormsManager>.Instance.TopMostFormID = this.windowID;
				}
			}
		}

		public string FileName
		{
			get
			{
				return this.filename;
			}
			set
			{
				this.filename = value;
			}
		}

		public UIInteractivePanel InteractivePanel
		{
			get
			{
				return this.interactivePanel;
			}
			set
			{
				this.interactivePanel = value;
			}
		}

		public Form.ChildLocation ChildFormLocation
		{
			get;
			private set;
		}

		public bool CheckMouseEvent
		{
			get
			{
				return this.checkMouseEvent;
			}
			set
			{
				this.checkMouseEvent = value;
			}
		}

		public bool AlwaysUpdate
		{
			get
			{
				return this.alwaysUpdate;
			}
			set
			{
				this.alwaysUpdate = value;
			}
		}

		public int MyMainWindowID
		{
			get
			{
				return this.myMainWindowID;
			}
			set
			{
				this.myMainWindowID = value;
				this.icon = true;
			}
		}

		public byte ICON_POSTYPE
		{
			get
			{
				return this.IconPosType;
			}
		}

		public bool IsIcon
		{
			get
			{
				return this.icon;
			}
		}

		public int MyIconWindowID
		{
			get
			{
				return this.myIconWindowID;
			}
			set
			{
				this.myIconWindowID = value;
				this.haveIcon = true;
			}
		}

		public bool HaveIcon
		{
			get
			{
				return this.haveIcon;
			}
		}

		public bool IconEffectSwitchOff
		{
			get;
			set;
		}

		public bool ShowHide
		{
			get
			{
				return this.bShowHide;
			}
			set
			{
				this.bShowHide = value;
			}
		}

		public bool Scale
		{
			get
			{
				return this.bScale;
			}
			set
			{
				this.bScale = value;
			}
		}

		public bool Draggable
		{
			get
			{
				return this.InteractivePanel.draggable;
			}
			set
			{
				this.InteractivePanel.draggable = value;
			}
		}

		public int WindowID
		{
			get
			{
				return this.windowID;
			}
			set
			{
				this.windowID = value;
				if (this.InteractivePanel)
				{
					this.InteractivePanel.index = value;
				}
			}
		}

		public G_ID Orignal_ID
		{
			get
			{
				return this.orignalID;
			}
			set
			{
				this.orignalID = value;
			}
		}

		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				if (value)
				{
					this.Show();
				}
				else
				{
					this.Hide();
				}
			}
		}

		public SimpleSprite BG
		{
			get
			{
				return this._backGround;
			}
			set
			{
				this._backGround = value;
			}
		}

		public Box BLACK_BG
		{
			get
			{
				return this._black_backGround;
			}
			set
			{
				this._black_backGround = value;
			}
		}

		public Texture BGImage
		{
			set
			{
				this.SetBGImage(value);
			}
		}

		public Form()
		{
		}

		public bool IsDestroy()
		{
			return this.bDestroy;
		}

		public void RegisterLeftIcon(G_ID _myMainWindowID)
		{
			this.icon = true;
			this.myMainWindowID = (int)_myMainWindowID;
			this.ChangeSceneDestory = false;
		}

		public void RegisterRightIcon(G_ID _myMainWindowID)
		{
			this.icon = true;
			this.myMainWindowID = (int)_myMainWindowID;
			this.ChangeSceneDestory = false;
		}

		public virtual bool CanMakeIcon()
		{
			return true;
		}

		public bool IsFocus()
		{
			return this.interactivePanel && this.interactivePanel.IsFocus();
		}

		public void SetFocus()
		{
			NrTSingleton<FormsManager>.Instance.PanelManager.DepthChange(this.InteractivePanel);
		}

		public void CreateInteractivePanel(G_ID ID, Vector3 pos, bool move, bool topMost)
		{
			this.windowID = (int)ID;
			this.interactivePanel = UIInteractivePanel.Create(ID.ToString(), new Vector3(pos.x, -pos.y, pos.z));
			if (null == this.interactivePanel)
			{
				return;
			}
			this.SetLocation(pos.x, pos.y, pos.z);
			this.interactivePanel.topMost = topMost;
			if (TsPlatform.IsWeb)
			{
				this.interactivePanel.draggable = move;
			}
			else if (TsPlatform.IsMobile)
			{
				this.interactivePanel.draggable = false;
			}
			this.interactivePanel.index = this.windowID;
			this.interactivePanel.gameObject.layer = GUICamera.UILayer;
			NrTSingleton<FormsManager>.Instance.PanelManager.MakeChild(this.interactivePanel.gameObject);
			this.interactivePanel.panelManager = NrTSingleton<FormsManager>.Instance.PanelManager;
		}

		public void DonotDepthChange()
		{
			if (this.interactivePanel)
			{
				this.interactivePanel.depthChangeable = false;
				this.SetLocation(this.interactivePanel.transform.position.x, this.interactivePanel.transform.position.y, UIPanelManager.SCENE_DEPTH);
			}
		}

		public void DonotDepthChange(float z)
		{
			if (this.interactivePanel)
			{
				this.interactivePanel.depthChangeable = false;
				this.SetLocation(this.interactivePanel.transform.position.x, this.interactivePanel.transform.position.y, z);
			}
		}

		public void StartInteractivePanel()
		{
			if (null != this.interactivePanel)
			{
				this.interactivePanel.Start();
				if (TsPlatform.IsWeb && this.bScale)
				{
					this.interactivePanel.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
				}
				else if (NrTSingleton<UIDataManager>.Instance.ScaleMode && this.bScale)
				{
					this.interactivePanel.transform.localScale = new Vector3(1f, 0.75f, 1f);
				}
			}
		}

		public abstract void InitializeComponent();

		public virtual void ChangedResolution()
		{
		}

		public void SetLocationX(float x)
		{
			if (this.interactivePanel)
			{
				this.SetLocation(x, -this.interactivePanel.transform.position.y, this.interactivePanel.transform.position.z);
			}
		}

		public void SetLocation(float x, float y, float z)
		{
			if (this.interactivePanel)
			{
				this.position.x = x;
				this.position.y = -y;
				this.position.z = z;
				this.interactivePanel.transform.position = this.position;
				if (null != this.BLACK_BG)
				{
					if (NrTSingleton<UIDataManager>.Instance.ScaleMode)
					{
						this.BLACK_BG.transform.localPosition = new Vector3(-this.GetLocation().x, -this.GetLocation().y * 1.8f, 0.1f);
					}
					else
					{
						this.BLACK_BG.transform.localPosition = new Vector3(-this.GetLocation().x, -this.GetLocation().y, 0.1f);
					}
				}
			}
			this.AdjustChildLocation();
		}

		public void SetLocation(float x, float y)
		{
			if (this.interactivePanel)
			{
				this.SetLocation(x, y, this.interactivePanel.transform.position.z);
			}
		}

		public void SetLocation(Vector2 pos)
		{
			if (this.interactivePanel)
			{
				this.SetLocation(pos.x, pos.y, this.interactivePanel.transform.position.z);
			}
		}

		private void AdjustChildLocation()
		{
		}

		public Vector3 GetLocation()
		{
			return this.interactivePanel.transform.position;
		}

		public float GetLocationX()
		{
			return this.interactivePanel.transform.position.x;
		}

		public float GetLocationY()
		{
			return -this.interactivePanel.transform.position.y;
		}

		public void SetSize(float width, float height)
		{
			this.size.x = width;
			this.size.y = height;
			if (null != this.InteractivePanel)
			{
				BoxCollider boxCollider = (BoxCollider)this.InteractivePanel.gameObject.GetComponent(typeof(BoxCollider));
				if (null != boxCollider)
				{
					boxCollider.size = new Vector3(this.size.x, this.size.y, 0f);
					boxCollider.center = new Vector3(this.size.x / 2f, -this.size.y / 2f, 0f);
				}
				if (null != this.BG)
				{
					this.BG.SetSize(this.size.x, this.size.y);
				}
				this.interactivePanel.width = this.size.x;
				this.interactivePanel.height = this.size.y;
			}
		}

		public void SetSize(int width, int height)
		{
			this.SetSize((float)width, (float)height);
		}

		public Vector2 GetSize()
		{
			return this.size;
		}

		public float GetSizeX()
		{
			return this.size.x * this.InteractivePanel.transform.localScale.x;
		}

		public float GetSizeY()
		{
			return this.size.y * this.InteractivePanel.transform.localScale.y;
		}

		public virtual void OnLoad()
		{
		}

		public virtual void OnClose()
		{
		}

		public virtual void InitData()
		{
		}

		public virtual void Close()
		{
			if (this.interactivePanel != null && this.interactivePanel.parentFormID != G_ID.NONE)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm((G_ID)this.WindowID);
				return;
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(this.WindowID);
		}

		public void CloseNow()
		{
			NrTSingleton<FormsManager>.Instance.CloseForm((G_ID)this.WindowID);
		}

		public virtual void CloseForm(IUIObject obj)
		{
			if (this.bStartAni || !this.visible)
			{
				return;
			}
			if (null == this.interactivePanel)
			{
				return;
			}
			if (TsPlatform.IsMobile && null != this.BLACK_BG)
			{
				this.BLACK_BG.Visible = false;
			}
			this.oldPos = this.InteractivePanel.transform.localPosition;
			if (this.Scale && TsPlatform.IsWeb)
			{
				Vector3 localPosition = this.InteractivePanel.transform.localPosition;
				localPosition.x -= this.GetSize().x * 0.3f / 2f;
				localPosition.y += this.GetSize().y * 0.3f / 2f;
				this.InteractivePanel.transform.localPosition = localPosition;
			}
			if (this.closeAni)
			{
				this.oldScale = this.InteractivePanel.transform.localScale;
				AnimateScale.Do(this.InteractivePanel.gameObject, EZAnimation.ANIM_MODE.FromTo, this.GetSize().x, this.GetSize().y, this.InteractivePanel.transform.localScale, Vector3.zero, EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.BackIn), 0.3f, 0f, null, new EZAnimation.CompletionDelegate(this.CloseAni));
			}
			else
			{
				if (null == this.interactivePanel)
				{
					return;
				}
				this.bStartAni = false;
				this.interactivePanel.transform.localPosition = this.oldPos;
				this.interactivePanel.transform.localScale = this.oldScale;
				if (this.interactivePanel.parentFormID != G_ID.NONE)
				{
					NrTSingleton<FormsManager>.Instance.CloseForm((G_ID)this.WindowID);
					return;
				}
				NrTSingleton<FormsManager>.Instance.CloseForm(this.WindowID);
			}
			this.bStartAni = true;
		}

		protected void AlphaAni(float startA, float destA, float time)
		{
			SpriteRoot[] componentsInChildren = this.InteractivePanel.transform.GetComponentsInChildren<SpriteRoot>(true);
			if (componentsInChildren != null)
			{
				SpriteRoot[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					SpriteRoot spriteRoot = array[i];
					if (null != spriteRoot)
					{
						FadeSprite.Do(spriteRoot, EZAnimation.ANIM_MODE.FromTo, new Color(spriteRoot.color.r, spriteRoot.color.g, spriteRoot.color.b, startA), new Color(spriteRoot.color.r, spriteRoot.color.g, spriteRoot.color.b, destA), new EZAnimation.Interpolator(EZAnimation.linear), time, 0f, null, null);
					}
				}
			}
			SpriteText[] componentsInChildren2 = this.InteractivePanel.transform.GetComponentsInChildren<SpriteText>(true);
			if (componentsInChildren2 != null)
			{
				SpriteText[] array2 = componentsInChildren2;
				for (int j = 0; j < array2.Length; j++)
				{
					SpriteText spriteText = array2[j];
					if (null != spriteText)
					{
						FadeText.Do(spriteText, EZAnimation.ANIM_MODE.FromTo, new Color(spriteText.color.r, spriteText.color.g, spriteText.color.b, startA), new Color(spriteText.color.r, spriteText.color.g, spriteText.color.b, destA), new EZAnimation.Interpolator(EZAnimation.linear), time, 0f, null, null);
					}
				}
			}
		}

		protected void StopAni()
		{
			SpriteRoot[] componentsInChildren = this.InteractivePanel.transform.GetComponentsInChildren<SpriteRoot>(true);
			if (componentsInChildren != null)
			{
				SpriteRoot[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					SpriteRoot spriteRoot = array[i];
					if (null != spriteRoot)
					{
						EZAnimator.instance.Stop(spriteRoot.gameObject);
						EZAnimator.instance.Stop(spriteRoot);
					}
				}
			}
			SpriteText[] componentsInChildren2 = this.InteractivePanel.transform.GetComponentsInChildren<SpriteText>(true);
			if (componentsInChildren2 != null)
			{
				SpriteText[] array2 = componentsInChildren2;
				for (int j = 0; j < array2.Length; j++)
				{
					SpriteText spriteText = array2[j];
					if (null != spriteText)
					{
						EZAnimator.instance.Stop(spriteText.gameObject);
						EZAnimator.instance.Stop(spriteText);
					}
				}
			}
		}

		private void endAlpha(EZAnimation ani)
		{
			if (null == this.interactivePanel)
			{
				return;
			}
			SpriteRoot[] componentsInChildren = this.InteractivePanel.transform.GetComponentsInChildren<SpriteRoot>(true);
			if (componentsInChildren != null)
			{
				SpriteRoot[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					SpriteRoot spriteRoot = array[i];
					if (null != spriteRoot)
					{
						spriteRoot.SetColor(Color.white);
					}
				}
			}
		}

		private void CloseAni(EZAnimation ani)
		{
			if (null == this.interactivePanel)
			{
				return;
			}
			this.bStartAni = false;
			this.interactivePanel.transform.localPosition = this.oldPos;
			this.interactivePanel.transform.localScale = this.oldScale;
			if (this.interactivePanel.parentFormID != G_ID.NONE)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm((G_ID)this.WindowID);
				return;
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(this.WindowID);
		}

		public virtual void Hide_End()
		{
		}

		public virtual void AfterShow()
		{
		}

		private void Show(G_ID formID)
		{
			Form form = NrTSingleton<FormsManager>.Instance.GetForm(formID);
			if (form != null)
			{
				form.InitData();
				if (!form.visible)
				{
					form.InteractivePanel.transform.position = new Vector3(form.InteractivePanel.transform.position.x, form.oldYPos, form.InteractivePanel.transform.position.z);
				}
				else
				{
					form.InteractivePanel.transform.position = new Vector3(form.InteractivePanel.transform.position.x, form.InteractivePanel.transform.position.y, form.InteractivePanel.transform.position.z);
				}
				form.visible = true;
			}
			else
			{
				form = NrTSingleton<FormsManager>.Instance.LoadForm(formID);
				form.InitData();
				if (!form.visible)
				{
					form.InteractivePanel.transform.position = new Vector3(form.InteractivePanel.transform.position.x, form.oldYPos, form.InteractivePanel.transform.position.z);
				}
				else
				{
					form.InteractivePanel.transform.position = new Vector3(form.InteractivePanel.transform.position.x, form.InteractivePanel.transform.position.y, form.InteractivePanel.transform.position.z);
				}
				form.visible = true;
			}
		}

		public virtual void Show()
		{
			this.InitData();
			if (this.InteractivePanel == null)
			{
				return;
			}
			if (!this.visible)
			{
				this.InteractivePanel.transform.position = new Vector3(this.InteractivePanel.transform.position.x, this.oldYPos, this.InteractivePanel.transform.position.z);
			}
			else
			{
				this.InteractivePanel.transform.position = new Vector3(this.InteractivePanel.transform.position.x, this.InteractivePanel.transform.position.y, this.InteractivePanel.transform.position.z);
			}
			this.visible = true;
			NrTSingleton<FormsManager>.Instance.PanelManager.DepthChange(this.InteractivePanel);
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DLG_LOADINGPAGE) || !this.IsNonePlayOpenCloseSoundForm())
			{
			}
			if (this.InteractivePanel.twinFormID != G_ID.NONE)
			{
				this.Show(this.InteractivePanel.twinFormID);
			}
			if (this.InteractivePanel.childFormID_0 != G_ID.NONE)
			{
				this.Show(this.InteractivePanel.childFormID_0);
			}
			if (this.InteractivePanel.childFormID_1 != G_ID.NONE)
			{
				this.Show(this.InteractivePanel.childFormID_1);
			}
			this.AfterShow();
			this.OnOpenCallback();
		}

		private void Hide(G_ID formID)
		{
			Form form = NrTSingleton<FormsManager>.Instance.GetForm(formID);
			if (form != null)
			{
				form.Hide_End();
				form.visible = false;
				if (this.visibleYPos != form.InteractivePanel.transform.position.y)
				{
					form.oldYPos = form.InteractivePanel.transform.position.y;
				}
				form.InteractivePanel.transform.position = new Vector3(form.InteractivePanel.transform.position.x, this.visibleYPos, form.InteractivePanel.transform.position.z);
			}
		}

		public virtual void Hide()
		{
			if (this.bStartAni)
			{
				return;
			}
			this.Hide_End();
			this.visible = false;
			if (this.InteractivePanel == null)
			{
				return;
			}
			if (this.visibleYPos != this.InteractivePanel.transform.position.y)
			{
				this.oldYPos = this.InteractivePanel.transform.position.y;
			}
			this.InteractivePanel.transform.position = new Vector3(this.InteractivePanel.transform.position.x, this.visibleYPos, this.InteractivePanel.transform.position.z);
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DLG_LOADINGPAGE) || !this.IsNonePlayOpenCloseSoundForm())
			{
			}
			if (this.InteractivePanel.twinFormID != G_ID.NONE)
			{
				this.Hide(this.InteractivePanel.twinFormID);
			}
			if (this.InteractivePanel.childFormID_0 != G_ID.NONE)
			{
				this.Hide(this.InteractivePanel.childFormID_0);
			}
			if (this.InteractivePanel.childFormID_1 != G_ID.NONE)
			{
				this.Hide(this.InteractivePanel.childFormID_1);
			}
		}

		public void InitializeForm()
		{
			this.InitializeComponent();
			if (this.InteractivePanel != null)
			{
				this.StartInteractivePanel();
				this.SetComponent();
			}
			else
			{
				this.CloseNow();
			}
		}

		public virtual void SetComponent()
		{
		}

		public void SetCloseButton(Button btn)
		{
			btn.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}

		public void SetDeleagteCloseButton(EZValueChangedDelegate del)
		{
			this.closeButton.SetValueChangedDelegate(del);
		}

		public void SetNonClickALL()
		{
			if (null != this.interactivePanel.gameObject)
			{
				BoxCollider component = this.interactivePanel.gameObject.GetComponent<BoxCollider>();
				if (component != null)
				{
					component.size = new Vector3(0f, 0f, 0f);
				}
			}
			foreach (IUIObject current in this.EZControlDictionary.Values)
			{
				BoxCollider component2 = current.gameObject.GetComponent<BoxCollider>();
				if (component2 != null)
				{
					component2.size = new Vector3(0f, 0f, 0f);
				}
			}
		}

		public void SetNonClick(bool Value)
		{
			foreach (IUIObject current in this.EZControlDictionary.Values)
			{
				if (Value)
				{
					BoxCollider component = current.gameObject.GetComponent<BoxCollider>();
					if (component != null && current as Button)
					{
						component.size = new Vector3(0f, 0f, 0f);
					}
				}
				else
				{
					BoxCollider component2 = current.gameObject.GetComponent<BoxCollider>();
					if (component2 != null && current as Button)
					{
						Button button = current as Button;
						component2.size = new Vector3(button.width, button.height, 0f);
					}
				}
			}
		}

		public IUIObject GetControl(string name)
		{
			if (this.EZControlDictionary.ContainsKey(name))
			{
				return this.EZControlDictionary[name];
			}
			return null;
		}

		public void AddDictionaryControl(string name, IUIObject source)
		{
			if (this.EZControlDictionary.ContainsKey(name))
			{
				return;
			}
			this.EZControlDictionary.Add(name, source);
		}

		protected virtual void BeforeOnGUI()
		{
		}

		protected virtual void AfterOnGUI()
		{
		}

		public virtual void FrameUpdate()
		{
		}

		public virtual void Update()
		{
			if (this.autoAni)
			{
				if (!this.setAni)
				{
					this.AlphaAni(0f, 1f, -0.5f);
					this.setAni = true;
				}
			}
			else if (this.setAni)
			{
				this.StopAni();
				this.AlphaAni(1f, 1f, 0f);
				this.setAni = false;
			}
		}

		public virtual void LateUpdate()
		{
		}

		public void InvisibleLayer()
		{
			foreach (IUIObject current in this.EZControlDictionary.Values)
			{
				if (!(current as Toolbar))
				{
					if (current.Layer != 0)
					{
						current.Visible = false;
						MsgHandler.Handle("VisibleDropDownList", new object[]
						{
							current
						});
					}
				}
			}
		}

		public void ShowLayer(int layer, string[] a_straName, bool a_bShow)
		{
			if (layer == 0)
			{
				return;
			}
			foreach (IUIObject current in this.EZControlDictionary.Values)
			{
				if (!(current as Toolbar))
				{
					if (current.Layer == layer)
					{
						bool flag = false;
						for (int i = 0; i < a_straName.Length; i++)
						{
							if (a_straName[i] == current.name)
							{
								flag = true;
								break;
							}
						}
						bool flag2 = (!flag) ? (!a_bShow) : a_bShow;
						current.Visible = flag2;
						Label label = current as Label;
						if (null != label && flag2)
						{
							label.gameObject.renderer.enabled = false;
						}
						MsgHandler.Handle("VisibleDropDownList", new object[]
						{
							current
						});
					}
					else if (current.Layer != 0)
					{
						current.Visible = false;
					}
				}
			}
		}

		public void ShowLayer(int layer)
		{
			if (layer == 0)
			{
				return;
			}
			foreach (IUIObject current in this.EZControlDictionary.Values)
			{
				if (!(current as Toolbar))
				{
					if (current.Layer == layer)
					{
						current.Visible = true;
						Label label = current as Label;
						if (null != label)
						{
							label.gameObject.renderer.enabled = false;
						}
						MsgHandler.Handle("VisibleDropDownList", new object[]
						{
							current
						});
					}
					else if (current.Layer != 0)
					{
						current.Visible = false;
					}
				}
			}
		}

		public void ShowLayer(int layer1, int layer2)
		{
			if (layer1 == 0 || layer2 == 0)
			{
				return;
			}
			foreach (IUIObject current in this.EZControlDictionary.Values)
			{
				if (!(current as Toolbar))
				{
					if (current.Layer == layer1 || current.Layer == layer2)
					{
						current.Visible = true;
						Label label = current as Label;
						if (null != label)
						{
							label.gameObject.renderer.enabled = false;
						}
						MsgHandler.Handle("VisibleDropDownList", new object[]
						{
							current
						});
					}
					else if (current.Layer != 0)
					{
						current.Visible = false;
					}
				}
			}
		}

		public void SetShowLayer(int si32Index, bool bShow)
		{
			foreach (IUIObject current in this.EZControlDictionary.Values)
			{
				if (!(current as Toolbar))
				{
					if (current.Layer == si32Index)
					{
						current.Visible = bShow;
						Label label = current as Label;
						if (null != label)
						{
							label.gameObject.renderer.enabled = false;
						}
						MsgHandler.Handle("VisibleDropDownList", new object[]
						{
							current
						});
					}
				}
			}
		}

		public void SetLayerZ(int si32Index, float value)
		{
			foreach (IUIObject current in this.EZControlDictionary.Values)
			{
				if (!(current as Toolbar))
				{
					if (current.Layer == si32Index)
					{
						AutoSpriteControlBase autoSpriteControlBase = current as AutoSpriteControlBase;
						if (null != autoSpriteControlBase)
						{
							autoSpriteControlBase.SetLocationZ(autoSpriteControlBase.GetLocation().z + value);
						}
						else
						{
							MsgHandler.Handle("ChangeZ", new object[]
							{
								value
							});
						}
					}
				}
			}
		}

		public void AllHideLayer()
		{
			foreach (IUIObject current in this.EZControlDictionary.Values)
			{
				if (current.Layer != 0)
				{
					current.Visible = false;
					Label label = current as Label;
					if (null != label)
					{
						label.gameObject.renderer.enabled = false;
					}
				}
			}
		}

		public Button GetCloseButton()
		{
			return this.GetControl(UIDataManager.closeButtonName) as Button;
		}

		public void SetupBG(string imagekey, float x, float y, float w, float h)
		{
			UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(imagekey);
			if (uIBaseInfoLoader == null)
			{
				return;
			}
			this.CreateBG();
			this.BG.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
			this.BG.m_bPattern = uIBaseInfoLoader.Pattern;
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			this.BG.Setup(w, h, new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height), material);
		}

		public void ChangeBG(string imagekey)
		{
			UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(imagekey);
			if (uIBaseInfoLoader == null)
			{
				return;
			}
			this.BG.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
			this.BG.m_bPattern = uIBaseInfoLoader.Pattern;
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			this.BG.Setup(this.GetSize().x, this.GetSize().y, new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height), material);
		}

		public void SetupBalckBG(string imagekey, float x, float y, float w, float h, float value, bool bMainBG, SpriteRoot.ANCHOR_METHOD anchor = SpriteRoot.ANCHOR_METHOD.UPPER_LEFT, float depth = 0.1f)
		{
			UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(imagekey);
			if (uIBaseInfoLoader == null)
			{
				return;
			}
			Box box = this.CreateBLACK_BG(x, y, anchor, depth);
			box.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
			box.SetSpriteTile(uIBaseInfoLoader.Tile, uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height);
			box.m_bPattern = uIBaseInfoLoader.Pattern;
			Material material = (Material)CResources.Load(uIBaseInfoLoader.Material);
			box.Setup(w, h, material);
			box.SetTextureUVs(new Vector2(uIBaseInfoLoader.UVs.x, uIBaseInfoLoader.UVs.y + uIBaseInfoLoader.UVs.height), new Vector2(uIBaseInfoLoader.UVs.width, uIBaseInfoLoader.UVs.height));
			box.SetColor(new Color(1f, 1f, 1f, value));
			if (bMainBG)
			{
				this.BLACK_BG = box;
			}
		}

		public void SetBGImage(Texture pkImage)
		{
			if (this.BG == null)
			{
				this.CreateBG();
			}
			Material material;
			if (TsPlatform.IsMobile)
			{
				material = new Material(Shader.Find("Transparent/Vertex Colored_mobile"));
			}
			else
			{
				material = new Material(Shader.Find("Transparent/Vertex Colored"));
			}
			if (null == material)
			{
				return;
			}
			material.mainTexture = pkImage;
			if (pkImage == null)
			{
				this.BG.Setup(this.size.x, this.size.y, new Vector2(0f, 0f), new Vector2(0f, 0f), material);
			}
			else
			{
				this.BG.Setup(this.size.x, this.size.y, new Vector2(0f, (float)pkImage.height), new Vector2((float)pkImage.width, (float)pkImage.height), material);
			}
		}

		private void CreateBG()
		{
			if (null == this.BG)
			{
				this.BG = SimpleSprite.Create("DEFAULT_BG", new Vector3(0f, 0f, 0f));
				this.BG.autoResize = true;
				this.BG.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
				this.BG.gameObject.layer = GUICamera.UILayer;
				this.interactivePanel.MakeChild(this.BG.gameObject);
				this.BG.transform.localPosition = Vector3.zero;
			}
		}

		private Box CreateBLACK_BG(float x, float y, SpriteRoot.ANCHOR_METHOD anchor, float depth)
		{
			Box box = Box.Create("Win_T_BK", new Vector3(0f, 0f, 0.1f));
			box.autoResize = true;
			box.SetAnchor(anchor);
			box.gameObject.layer = GUICamera.UILayer;
			BoxCollider boxCollider = (BoxCollider)box.gameObject.AddComponent(typeof(BoxCollider));
			if (null != boxCollider)
			{
				boxCollider.size = new Vector3(GUICamera.width, GUICamera.height, 0f);
				boxCollider.center = new Vector3(GUICamera.width / 2f, -GUICamera.height / 2f, 0f);
			}
			this.InteractivePanel.MakeChild(box.gameObject);
			if (TsPlatform.IsWeb)
			{
				box.transform.localPosition = new Vector3(-this.GetLocation().x * 1.43f, -this.GetLocation().y * 1.43f, 0.1f);
			}
			else if (NrTSingleton<UIDataManager>.Instance.ScaleMode)
			{
				box.transform.localScale = new Vector3(1f, 1.4f, 1f);
				box.transform.localPosition = new Vector3(-this.GetLocation().x, -this.GetLocation().y * 1.8f, 0.1f);
			}
			else
			{
				box.transform.localPosition = new Vector3(-this.GetLocation().x + x, -this.GetLocation().y + y, depth);
			}
			return box;
		}

		public void ShowBlackBG(float value = 0.5f)
		{
			if (TsPlatform.IsMobile)
			{
				this.SetupBalckBG("Win_T_BK", 0f, 0f, GUICamera.width, GUICamera.height, value, true, SpriteRoot.ANCHOR_METHOD.UPPER_LEFT, 0.1f);
			}
		}

		public void ShowUpperBG(float fDeath)
		{
			if (TsPlatform.IsMobile)
			{
				this.SetupBalckBG("Win_T_BK", 0f, 0f, this.GetSizeX(), (GUICamera.height - this.GetSizeY()) / 2f, 1f, false, SpriteRoot.ANCHOR_METHOD.BOTTOM_LEFT, fDeath);
			}
		}

		public void ShowDownBG(float fDeath)
		{
			if (TsPlatform.IsMobile)
			{
				this.SetupBalckBG("Win_T_BK", 0f, -this.GetSizeY(), this.GetSizeX(), (GUICamera.height - this.GetSizeY()) / 2f, 1f, false, SpriteRoot.ANCHOR_METHOD.UPPER_LEFT, fDeath);
			}
		}

		public void ShowLeftBG(float fDeath)
		{
			if (TsPlatform.IsMobile)
			{
				this.SetupBalckBG("Win_T_BK", 0f, (GUICamera.height - this.GetSizeY()) / 2f, (GUICamera.width - this.GetSizeX()) / 2f, GUICamera.height, 1f, false, SpriteRoot.ANCHOR_METHOD.UPPER_RIGHT, fDeath);
			}
		}

		public void ShowRightBG(float fDeath)
		{
			if (TsPlatform.IsMobile)
			{
				this.SetupBalckBG("Win_T_BK", this.GetSizeX(), (GUICamera.height - this.GetSizeY()) / 2f, (GUICamera.width - this.GetSizeX()) / 2f, GUICamera.height, 1f, false, SpriteRoot.ANCHOR_METHOD.UPPER_LEFT, fDeath);
			}
		}

		private bool IsNonePlayOpenCloseSoundForm()
		{
			G_ID g_ID = (G_ID)this.WindowID;
			switch (g_ID)
			{
			case G_ID.TREASUREBOX_DLG:
			case G_ID.MYTH_EVOLUTION_MAIN_DLG:
			case G_ID.MYTH_LEGEND_INFO_DLG:
			case G_ID.MYTH_EVOLUTION_SKILLDETAIL_DLG:
			case G_ID.MYTH_EVOLUTION_TIME_DLG:
			case G_ID.MYTH_EVOLUTION_CHECK_DLG:
			case G_ID.MYTH_EVOLUTION_SUCCESS_DLG:
			case G_ID.INVENTORY_DLG:
			case G_ID.TOOLTIP_DLG:
			case G_ID.TOOLTIP_SECOND_DLG:
			case G_ID.CHAT_MAIN_DLG:
			case G_ID.CHAT_OPTION_DLG:
			case G_ID.CHAT_AD_DLG:
			case G_ID.EMOTICON_DLG:
			case G_ID.CHAT_TABOPTION_DLG:
			case G_ID.WHISPER_DLG:
			case G_ID.WHISPER_MINIMIZE_DLG:
			case G_ID.DLG_SYSTEMMESSAGE:
			case G_ID.DLG_LOADINGPAGE:
			case G_ID.MINIDIRECTONTALK_DLG:
			case G_ID.MiniDRAMAEMOTICON_DLG:
			case G_ID.MINIDRAMACAPTION_DLG:
			case G_ID.UIGUIDE_DLG:
				return true;
			case G_ID.MYTH_EVOLUTION_MAIN_CHALLENGEQUEST_DLG:
			case G_ID.MYTH_LEGEND_INFO_CHALLENGEQUEST_DLG:
			case G_ID.MSGBOX_DLG:
			case G_ID.INTROMSGBOX_DLG:
			case G_ID.MESSAGE_DLG:
			case G_ID.MESSAGE_NOTIFY_DLG:
			case G_ID.MSGBOX_TWOCHECK_DLG:
			case G_ID.MSGBOX_AUTOSELL_DLG:
			case G_ID.BONUS_ITEM_INFO_DLG:
			case G_ID.ITEMTOOLTIP_DLG:
			case G_ID.ITEMTOOLTIP_SECOND_DLG:
			case G_ID.ITEMTOOLTIP_BUTTON:
			case G_ID.SETITEMTOOLTIP_DLG:
			case G_ID.CHAT_MOBILE_SUB_DLG:
			case G_ID.WHISPER_USERLIST_DLG:
			case G_ID.WHISPER_ROOMLIST_DLG:
			case G_ID.WHISPER_COLOR_DLG:
			case G_ID.WHISPER_INVITE_DLG:
			case G_ID.WHISPER_WHISPERPOPUPMENU_DLG:
			case G_ID.DLG_MONSTER_DETAILINFO:
				IL_BF:
				switch (g_ID)
				{
				case G_ID.MAINMENU_DLG:
				case G_ID.MAIN_UI_AUTO_MOVE:
				case G_ID.MAIN_UI_ICON:
				case G_ID.DLG_CHARINFO:
					return true;
				case G_ID.MAIN_MAP:
				case G_ID.MAIN_UI_MENU_SUB_ADVANCED:
				case G_ID.MAIN_UI_LEVELUP_ALARM_MONARCH:
				case G_ID.MAIN_UI_LEVELUP_ALARM_SOLDIER:
					IL_E7:
					switch (g_ID)
					{
					case G_ID.SOLGUIDE_DLG:
					case G_ID.SOLDETAIL_DLG:
					case G_ID.SOLELEMENTSUCCESS_DLG:
					case G_ID.SOLDETAIL_SKILLICON_DLG:
					case G_ID.SOLCOMBINATION_DLG:
					case G_ID.SOLCOMBINATION_DIRECTION_DLG:
						return true;
					default:
						switch (g_ID)
						{
						case G_ID.EVENT_MAIN:
						case G_ID.EVENT_MAIN_EXPLAIN:
						case G_ID.EVENT_REWARD_CHANGE_DLG:
							return true;
						case G_ID.EVENT_DAILY_GIFT_DLG:
						case G_ID.EVENT_NORMAL_ATTEND:
						case G_ID.EVENT_NEW_ATTEND:
							IL_12F:
							switch (g_ID)
							{
							case G_ID.MAIN_QUEST:
							case G_ID.QUEST_REWARD:
							case G_ID.QUESTLIST_DLG:
								return true;
							case G_ID.QUEST_GROUP_REWARD:
								IL_14B:
								switch (g_ID)
								{
								case G_ID.COSTUMEGUIDE_DLG:
								case G_ID.COSTUMEROOM_DLG:
								case G_ID.COSTUME_SKILLINFO_DLG:
								case G_ID.COSTUME_BUY_MSG_BOX:
									return true;
								default:
									switch (g_ID)
									{
									case G_ID.EXPEDITION_SEARCH_DLG:
									case G_ID.EXPEDITION_SEARCHDETAILINFO_DLG:
									case G_ID.MINE_MAINSELECT_DLG:
										return true;
									default:
										if (g_ID != G_ID.BATTLE_RESULT_DLG && g_ID != G_ID.BATTLE_RESULT_CONTENT_DLG && g_ID != G_ID.CHAT_NOTICE_DLG && g_ID != G_ID.REGION_NAME_DLG && g_ID != G_ID.WORLD_MAP && g_ID != G_ID.NPCTALK_DLG && g_ID != G_ID.NEARNPCSELECTUI_DLG && g_ID != G_ID.QUEST_CHAPTERSTART && g_ID != G_ID.DLG_RIGHTCLICK_MENU)
										{
											return false;
										}
										return true;
									}
									break;
								}
								break;
							}
							goto IL_14B;
						}
						goto IL_12F;
					}
					break;
				}
				goto IL_E7;
			}
			goto IL_BF;
		}

		public void SetScreenCenter()
		{
			float num = 0f;
			float num2 = 0f;
			if (this.InteractivePanel.childFormID_0 != G_ID.NONE)
			{
				Form form = NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_0);
				if (form != null)
				{
					Vector2 vector = this.GetSize();
					vector.x = this.GetSizeX();
					vector.y = this.GetSizeY();
					Vector2 vector2 = form.GetSize();
					vector2.x = form.GetSizeX();
					vector2.y = form.GetSizeY();
					if (form.ChildFormLocation == Form.ChildLocation.LEFT)
					{
						num += vector.x - vector2.x;
					}
					else
					{
						num += vector.x + vector2.x;
					}
					num2 += Math.Max(vector.y, vector2.y);
				}
			}
			if (this.InteractivePanel.childFormID_1 != G_ID.NONE)
			{
				Form form2 = NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_0);
				if (form2 != null)
				{
					Vector2 vector3 = this.GetSize();
					vector3.x = this.GetSizeX();
					vector3.y = this.GetSizeY();
					Vector2 vector4 = form2.GetSize();
					vector4.x = form2.GetSizeX();
					vector4.y = form2.GetSizeY();
					num += vector3.x + vector4.x;
					num2 += Math.Max(vector3.y, vector4.y);
				}
			}
			float x;
			float y;
			if (this.InteractivePanel.childFormID_0 != G_ID.NONE || this.InteractivePanel.childFormID_1 != G_ID.NONE)
			{
				x = (GUICamera.width - num) / 2f;
				y = (GUICamera.height - num2) / 2f;
			}
			else
			{
				x = (GUICamera.width - this.GetSizeX()) / 2f;
				y = (GUICamera.height - this.GetSizeY()) / 2f;
			}
			this.SetLocation(x, y);
			this.InteractivePanel.MoveChild();
			if (null != this.BLACK_BG)
			{
				if (TsPlatform.IsWeb)
				{
					this.BLACK_BG.transform.localPosition = new Vector3(-this.GetLocation().x * 1.43f, -this.GetLocation().y * 1.43f, 0.1f);
				}
				else if (NrTSingleton<UIDataManager>.Instance.ScaleMode)
				{
					this.BLACK_BG.transform.localPosition = new Vector3(-this.GetLocation().x, -this.GetLocation().y * 1.8f, 0.1f);
				}
				else
				{
					this.BLACK_BG.transform.localPosition = new Vector3(-this.GetLocation().x, -this.GetLocation().y, 0.1f);
				}
			}
		}

		public void CloseAllChildForm()
		{
			if (this.InteractivePanel.childFormID_0 != G_ID.NONE)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(this.InteractivePanel.childFormID_0);
			}
			if (this.InteractivePanel.childFormID_1 != G_ID.NONE)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(this.InteractivePanel.childFormID_1);
			}
		}

		public void DeleteChildForm(G_ID gid)
		{
			if (this.InteractivePanel.childFormID_0 == gid)
			{
				this.InteractivePanel.childFormID_0 = G_ID.NONE;
				if (this.InteractivePanel.childFormID_1 != G_ID.NONE)
				{
					if (NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_1).ChildFormLocation == Form.ChildLocation.LEFT)
					{
						NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_1).SetLocationX(this.GetLocation().x - NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_1).GetSizeX());
					}
					else if (NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_1).ChildFormLocation == Form.ChildLocation.RIGHT)
					{
						NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_1).SetLocationX(this.GetLocation().x + this.GetSizeX());
					}
				}
			}
			if (this.InteractivePanel.childFormID_1 == gid)
			{
				this.InteractivePanel.childFormID_1 = G_ID.NONE;
			}
		}

		public Form SetChildForm(G_ID formID)
		{
			return this.SetChildForm(formID, Form.ChildLocation.RIGHT);
		}

		public Form SetChildForm1(G_ID formID)
		{
			return this.SetChildForm1(formID, Form.ChildLocation.RIGHT);
		}

		public Form SetChildForm(G_ID formID, Form.ChildLocation location)
		{
			Form form;
			if (NrTSingleton<FormsManager>.Instance.IsForm(formID))
			{
				form = NrTSingleton<FormsManager>.Instance.GetForm(formID);
			}
			else
			{
				form = NrTSingleton<FormsManager>.Instance.LoadForm(formID);
			}
			if (form == null)
			{
				return null;
			}
			float x = 0f;
			float y = 0f;
			if (this.InteractivePanel.childFormID_1 != G_ID.NONE && location == NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_1).ChildFormLocation)
			{
				if (location == Form.ChildLocation.LEFT)
				{
					NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_1).SetLocationX(this.GetLocation().x - form.GetSizeX());
				}
				else if (location == Form.ChildLocation.RIGHT)
				{
					NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_1).SetLocationX(this.GetLocation().x + this.GetSizeX() + form.GetSizeX());
				}
			}
			switch (location)
			{
			case Form.ChildLocation.TOP:
				x = this.GetLocation().x;
				y = this.GetLocationY() - form.GetSizeY();
				break;
			case Form.ChildLocation.BOTTOM:
				x = this.GetLocation().x;
				y = this.GetLocationY() + this.GetSizeY();
				break;
			case Form.ChildLocation.LEFT:
				x = this.GetLocation().x - form.GetSizeX();
				y = this.GetLocationY();
				break;
			case Form.ChildLocation.RIGHT:
				x = this.GetLocation().x + this.GetSizeX();
				y = this.GetLocationY();
				break;
			case Form.ChildLocation.LEFTTOP:
				x = this.GetLocation().x - form.GetSizeX();
				y = this.GetLocationY();
				break;
			case Form.ChildLocation.CENTER:
				x = this.GetLocationX() + (this.GetSizeX() - form.GetSizeX()) / 2f;
				y = this.GetLocationY() + (this.GetSizeY() - form.GetSizeY()) / 2f;
				break;
			}
			form.interactivePanel.parentFormID = (G_ID)this.windowID;
			form.ChildFormLocation = location;
			form.SetLocation(x, y, this.GetLocation().z);
			form.InteractivePanel.draggable = false;
			this.InteractivePanel.childFormID_0 = formID;
			form.Show();
			return form;
		}

		public Form SetChildForm1(G_ID formID, Form.ChildLocation location)
		{
			Form form;
			if (NrTSingleton<FormsManager>.Instance.IsForm(formID))
			{
				form = NrTSingleton<FormsManager>.Instance.GetForm(formID);
			}
			else
			{
				form = NrTSingleton<FormsManager>.Instance.LoadForm(formID);
			}
			if (form == null)
			{
				return null;
			}
			float x = 0f;
			float y = 0f;
			float num = 0f;
			if (this.InteractivePanel.childFormID_0 != G_ID.NONE && location == NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_0).ChildFormLocation)
			{
				num += NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_0).interactivePanel.width * NrTSingleton<FormsManager>.Instance.GetForm(this.InteractivePanel.childFormID_0).interactivePanel.transform.localScale.x;
			}
			switch (location)
			{
			case Form.ChildLocation.TOP:
				x = this.GetLocation().x;
				y = this.GetLocationY() - form.GetSizeY();
				break;
			case Form.ChildLocation.BOTTOM:
				x = this.GetLocation().x;
				y = this.GetLocationY() + this.GetSizeY();
				break;
			case Form.ChildLocation.LEFT:
				x = this.GetLocation().x - form.GetSizeX() - num;
				y = this.GetLocationY();
				break;
			case Form.ChildLocation.RIGHT:
				x = this.GetLocation().x + this.GetSizeX() + num;
				y = this.GetLocationY();
				break;
			case Form.ChildLocation.LEFTTOP:
				x = this.GetLocation().x - form.GetSizeX();
				y = this.GetLocationY();
				break;
			case Form.ChildLocation.CENTER:
				x = this.GetLocationX() + (this.GetSizeX() - form.GetSizeX()) / 2f;
				y = this.GetLocationY() + (this.GetSizeY() - form.GetSizeY()) / 2f;
				break;
			}
			form.interactivePanel.parentFormID = (G_ID)this.windowID;
			form.ChildFormLocation = location;
			form.SetLocation(x, y, this.GetLocation().z);
			form.InteractivePanel.draggable = false;
			this.InteractivePanel.childFormID_1 = formID;
			form.Show();
			return form;
		}

		public void SetAlpha(float value)
		{
			if (null == this.interactivePanel)
			{
				return;
			}
			this.interactivePanel.SetAlpha(value);
		}

		public virtual void Set_Value(object a_oObject)
		{
		}

		public void RemoveChildControl()
		{
			if (null == this.InteractivePanel)
			{
				return;
			}
			foreach (IUIObject current in this.EZControlDictionary.Values)
			{
				this.EZControlDictionary.Remove(current.name);
				this.InteractivePanel.RemoveChild(current.gameObject);
				UnityEngine.Object.Destroy(current.gameObject);
			}
		}

		public virtual void FinishDownloadBundle(ref IDownloadedItem wItem)
		{
		}

		public virtual void SetLocationFromResolution(float _width, float _height)
		{
			float x = _width / 2f - this.GetSizeX() / 2f;
			float y = _height / 2f - this.GetSizeY() / 2f;
			this.SetLocation(x, y);
		}

		public Vector3 GetEffectUIPos(Vector2 ScreenPos)
		{
			Vector3 result = GUICamera.ScreenToGUIPoint(ScreenPos);
			result.y = -result.y;
			result.z = UIPanelManager.EFFECT_UI_DEPTH;
			return result;
		}

		public void ClearDictionary()
		{
			this.EZControlDictionary.Clear();
		}

		public void Move(float value)
		{
			Vector3 localPosition = this.interactivePanel.transform.localPosition;
			localPosition.x += value;
			this.pkAnimate = AnimatePosition.Do(this.interactivePanel.gameObject, EZAnimation.ANIM_MODE.To, localPosition, new EZAnimation.Interpolator(EZAnimation.linear), 0.5f, 0f, null, null);
		}

		public void OnOpenCallback()
		{
			if (Form.OpenCallback == null)
			{
				return;
			}
			Form.OpenCallback(this, null);
		}
	}
}
