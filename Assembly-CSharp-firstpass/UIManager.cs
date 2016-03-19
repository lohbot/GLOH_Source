using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using System.Text;
using TsBundle;
using UnityEngine;
using UnityForms;

[AddComponentMenu("EZ GUI/Management/UI Manager")]
public class UIManager : NrTSingleton<UIManager>
{
	public enum POINTER_TYPE
	{
		MOUSE,
		TOUCHPAD,
		AUTO_TOUCHPAD,
		RAY,
		MOUSE_AND_RAY,
		TOUCHPAD_AND_RAY
	}

	public enum RAY_ACTIVE_STATE
	{
		Inactive,
		Momentary,
		Constant
	}

	public enum OUTSIDE_VIEWPORT
	{
		Process_All,
		Ignore,
		Move_Off
	}

	public struct NonUIHitInfo
	{
		public int ptrIndex;

		public int camIndex;

		public NonUIHitInfo(int pIndex, int cIndex)
		{
			this.ptrIndex = pIndex;
			this.camIndex = cIndex;
		}
	}

	public delegate void PointerPollerDelegate();

	public delegate void PointerInfoDelegate(POINTER_INFO ptr);

	public static readonly string UI_BundleStackName = "UI_BundleStack";

	private char[] cNumCode = new char[]
	{
		'!',
		'@',
		'#',
		'$',
		'%',
		'^',
		'&',
		'*',
		'(',
		')'
	};

	public UIManager.POINTER_TYPE pointerType;

	public float dragThreshold = 48f;

	public float rayDragThreshold = 2f;

	public float rayDepth = float.PositiveInfinity;

	public LayerMask rayMask = -1;

	public bool focusWithRay;

	public string actionAxis = "Fire1";

	public UIManager.OUTSIDE_VIEWPORT inputOutsideViewport = UIManager.OUTSIDE_VIEWPORT.Move_Off;

	public bool warnOnNonUiHits = true;

	protected Transform raycastingTransform;

	public EZCameraSettings[] uiCameras = new EZCameraSettings[1];

	public Camera rayCamera;

	public bool blockInput;

	public float defaultDragOffset = 1f;

	public EZAnimation.EASING_TYPE cancelDragEasing = EZAnimation.EASING_TYPE.ExponentialOut;

	public float cancelDragDuration;

	public TextAsset defaultFont;

	public SpriteFont defaultSpriteFont;

	public Material defaultFontMaterial;

	private bool loadFontTexture;

	public bool autoRotateKeyboardPortrait = true;

	public bool autoRotateKeyboardPortraitUpsideDown = true;

	public bool autoRotateKeyboardLandscapeLeft = true;

	public bool autoRotateKeyboardLandscapeRight = true;

	protected bool rayActive;

	protected UIManager.RAY_ACTIVE_STATE rayState;

	private Queue<POINTER_INFO> inputQueue = new Queue<POINTER_INFO>();

	protected POINTER_INFO[,] pointers;

	protected UIManager.NonUIHitInfo[] nonUIHits;

	protected bool[] usedPointers;

	protected bool[] usedNonUIHits;

	protected bool rayIsNonUIHit;

	protected int numPointers;

	protected int numTouchPointers;

	protected int[] activePointers;

	protected int numActivePointers;

	protected int numNonUIHits;

	protected POINTER_INFO rayPtr;

	protected UIManager.PointerPollerDelegate pointerPoller;

	protected UIManager.PointerInfoDelegate informNonUIHit;

	protected UIManager.PointerInfoDelegate mouseTouchListeners;

	protected UIManager.PointerInfoDelegate rayListeners;

	protected IUIObject focusObj;

	protected string controlText;

	public int insert;

	private KEYBOARD_INFO kbInfo = default(KEYBOARD_INFO);

	protected int compStatus;

	protected int oldCompStatus;

	protected string compositionText;

	private bool bLastWord;

	private bool bfocusOn;

	public bool bLinkText;

	private bool clickUI;

	private bool dragClick;

	private bool m_bPressUI;

	private bool m_bDragUpUI;

	private int siNewKey;

	private int siOldKey;

	private bool bKeyTimer;

	private int siKeyTimer;

	private bool bMaxLength;

	private bool bSystemMessageBox;

	protected int inputLockCount;

	private int linkItemIndex;

	private bool m_bActiveFocus;

	private int lineCount;

	private int lastUpdateFrame;

	private int curActionID;

	private int numTouches;

	protected RaycastHit hit;

	protected Vector3 tempVec;

	private bool down;

	private bool rightdown;

	private IUIObject tempObj;

	private POINTER_INFO tempPtr;

	private StringBuilder sb = new StringBuilder();

	private TsPlatform.TouchScreenKeyboard iKeyboard;

	private bool m_bMobileKeyboard;

	private bool bInitialize;

	private int langType;

	private List<RaycastHit> list = new List<RaycastHit>();

	private float firstKeyTime;

	public bool checkReturn;

	public bool pressUI
	{
		get
		{
			return this.m_bPressUI;
		}
		set
		{
			this.m_bPressUI = value;
		}
	}

	public bool DragUpUI
	{
		get
		{
			return this.m_bDragUpUI;
		}
		set
		{
			this.m_bDragUpUI = value;
		}
	}

	public bool DragClick
	{
		get
		{
			return this.dragClick;
		}
		set
		{
			this.dragClick = value;
		}
	}

	public bool MaxLengthBool
	{
		get
		{
			return this.bMaxLength;
		}
		set
		{
			this.bMaxLength = value;
		}
	}

	public bool SystemMessageBox
	{
		get
		{
			return this.bSystemMessageBox;
		}
		set
		{
			this.bSystemMessageBox = value;
		}
	}

	public bool ClickUI
	{
		get
		{
			return this.clickUI;
		}
	}

	public int LinkItemIndex
	{
		get
		{
			return this.linkItemIndex;
		}
		set
		{
			this.linkItemIndex = value;
		}
	}

	public UIManager.RAY_ACTIVE_STATE RayActive
	{
		get
		{
			return this.rayState;
		}
		set
		{
			this.rayState = value;
		}
	}

	public IUIObject FocusObject
	{
		get
		{
			return this.focusObj;
		}
		set
		{
			if (this.focusObj != null && this.focusObj is IKeyFocusable)
			{
				this.m_bActiveFocus = false;
				if (TsPlatform.IsWeb || TsPlatform.IsEditor)
				{
					NkInputManager.imeCompositionMode = IMECompositionMode.Off;
				}
				else if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
				{
					this.m_bMobileKeyboard = false;
				}
				IKeyFocusable expr_67 = (IKeyFocusable)this.focusObj;
				expr_67.OriginalContent += NkInputManager.inputString;
				this.compositionText = string.Empty;
				((IKeyFocusable)this.focusObj).LostFocus();
			}
			this.focusObj = value;
			if (this.sb.Length > 0)
			{
				int length = this.sb.Length;
				this.sb.Remove(0, length);
			}
			if (this.focusObj != null)
			{
				this.m_bActiveFocus = true;
				if (TsPlatform.IsWeb || TsPlatform.IsEditor)
				{
					if (!((IKeyFocusable)this.focusObj).NumberMode)
					{
						NkInputManager.imeCompositionMode = IMECompositionMode.On;
					}
					this.clickUI = true;
				}
				this.bfocusOn = true;
				this.firstKeyTime = Time.realtimeSinceStartup;
				this.controlText = ((IKeyFocusable)this.focusObj).GetInputText(ref this.kbInfo);
				((IKeyFocusable)this.focusObj).OriginalContent = this.controlText;
				if (this.controlText == null)
				{
					this.controlText = string.Empty;
				}
				if (this.compositionText == null)
				{
					this.compositionText = string.Empty;
				}
				if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
				{
					if (!Application.isEditor)
					{
						this.iKeyboard = TsPlatform.TouchScreenKeyboard.Open(this.controlText, this.kbInfo.type, this.kbInfo.autoCorrect, this.kbInfo.multiline, this.kbInfo.secure, this.kbInfo.alert, this.controlText);
						this.iKeyboard.text = this.controlText;
						this.m_bMobileKeyboard = true;
					}
				}
				else if (this.kbInfo.secure)
				{
					NkInputManager.imeCompositionMode = IMECompositionMode.On;
				}
				if (this.kbInfo.insert >= this.controlText.Length)
				{
					this.insert = this.kbInfo.insert;
				}
				else
				{
					this.insert = this.controlText.Length;
				}
				if (this.sb.Length > 0)
				{
					this.sb.Replace(this.sb.ToString(), this.controlText);
				}
				else
				{
					this.sb.Append(this.controlText);
				}
			}
			else
			{
				if (TsPlatform.IsMobile && TsPlatform.IsAndroid && this.iKeyboard != null)
				{
					this.iKeyboard.active = false;
					this.iKeyboard = null;
				}
				this.clickUI = false;
			}
		}
	}

	public int InsertionPoint
	{
		get
		{
			return this.insert;
		}
		set
		{
			this.insert = value;
		}
	}

	private UIManager()
	{
	}

	public static bool Exists()
	{
		return NrTSingleton<UIManager>.Instance != null;
	}

	public bool IsLoadFontTexture()
	{
		return this.loadFontTexture;
	}

	public void SetEnableInput()
	{
		if (TsPlatform.IsMobile)
		{
			this.pressUI = false;
		}
		this.clickUI = false;
	}

	public bool IsMobileKeyboard()
	{
		return TsPlatform.IsMobile && this.m_bMobileKeyboard;
	}

	public void CloseKeyboard()
	{
		this.FocusObject = null;
	}

	public bool Initialize()
	{
		if (this.bInitialize)
		{
			return false;
		}
		if (TsPlatform.IsMobile)
		{
			if (TsPlatform.IsEditor)
			{
				this.pointerType = UIManager.POINTER_TYPE.MOUSE;
			}
			else
			{
				this.pointerType = UIManager.POINTER_TYPE.TOUCHPAD;
			}
		}
		if (this.pointerType == UIManager.POINTER_TYPE.TOUCHPAD || this.pointerType == UIManager.POINTER_TYPE.TOUCHPAD_AND_RAY)
		{
			if (TsPlatform.IsMobile)
			{
				TsPlatform.TouchScreenKeyboard.autorotateToPortrait = this.autoRotateKeyboardPortrait;
				TsPlatform.TouchScreenKeyboard.autorotateToPortraitUpsideDown = this.autoRotateKeyboardPortraitUpsideDown;
				TsPlatform.TouchScreenKeyboard.autorotateToLandscapeLeft = this.autoRotateKeyboardLandscapeLeft;
				TsPlatform.TouchScreenKeyboard.autorotateToLandscapeRight = this.autoRotateKeyboardLandscapeRight;
			}
			this.numTouches = 5;
		}
		else if (this.pointerType == UIManager.POINTER_TYPE.AUTO_TOUCHPAD)
		{
			this.numTouches = 12;
		}
		else if (this.pointerType == UIManager.POINTER_TYPE.MOUSE_AND_RAY)
		{
			this.numTouches = 1;
		}
		else
		{
			this.numTouches = 1;
		}
		if (this.pointerType == UIManager.POINTER_TYPE.AUTO_TOUCHPAD || this.pointerType == UIManager.POINTER_TYPE.MOUSE || this.pointerType == UIManager.POINTER_TYPE.MOUSE_AND_RAY)
		{
			this.numTouchPointers = this.numTouches - 1;
		}
		else
		{
			this.numTouchPointers = this.numTouches;
		}
		this.uiCameras = new EZCameraSettings[1];
		this.uiCameras[0] = new EZCameraSettings();
		Camera component = GameObject.Find("UI Camera").GetComponent<Camera>();
		if (null != component)
		{
			this.uiCameras[0].camera = component;
		}
		if (this.rayCamera == null)
		{
			this.rayCamera = this.uiCameras[0].camera;
		}
		this.Start();
		this.SetEZGUIResources();
		this.bInitialize = true;
		return true;
	}

	public void SetEZGUIResources()
	{
		string path = string.Empty;
		if (this.langType == 0)
		{
			path = NrTSingleton<UIDataManager>.Instance.FilePath + "Font/yFont1024";
			this.defaultFont = (CResources.Load(path) as TextAsset);
			this.defaultSpriteFont = FontStore.GetFont(this.defaultFont);
			this.defaultFontMaterial = (CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Font/yFont01_Bshadow_small") as Material);
		}
		else if (this.langType == 1)
		{
			path = NrTSingleton<UIDataManager>.Instance.FilePath + "Font/Eng/yFont1024";
			this.defaultFont = (CResources.Load(path) as TextAsset);
			this.defaultSpriteFont = FontStore.GetFont(this.defaultFont);
			this.defaultFontMaterial = (CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Font/Eng/yFont01_Bshadow_small") as Material);
		}
		else if (this.langType == 2)
		{
			path = NrTSingleton<UIDataManager>.Instance.FilePath + "Font/Cha/yFont1024";
			this.defaultFont = (CResources.Load(path) as TextAsset);
			this.defaultSpriteFont = FontStore.GetFont(this.defaultFont);
			this.defaultFontMaterial = (CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Font/Cha/yFont01_Bshadow_small") as Material);
		}
		else if (this.langType == 3)
		{
			path = NrTSingleton<UIDataManager>.Instance.FilePath + "Font/Jpn/yFont1024";
			this.defaultFont = (CResources.Load(path) as TextAsset);
			this.defaultSpriteFont = FontStore.GetFont(this.defaultFont);
			this.defaultFontMaterial = (CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Font/Jpn/yFont01_Bshadow_small") as Material);
		}
		Resources.UnloadAsset(this.defaultFont);
		CResources.Delete(path);
	}

	private void Start()
	{
		this.numPointers = this.numTouches;
		this.activePointers = new int[this.numTouches];
		this.usedPointers = new bool[this.numPointers];
		this.nonUIHits = new UIManager.NonUIHitInfo[this.numTouches];
		this.usedNonUIHits = new bool[this.numPointers];
		this.numNonUIHits = 0;
		this.SetupPointers();
		if (TsPlatform.IsWeb || TsPlatform.IsEditor)
		{
			NkInputManager.imeCompositionMode = IMECompositionMode.Off;
		}
		this.kbInfo.multiline = true;
	}

	protected void SetupPointers()
	{
		this.pointers = new POINTER_INFO[this.uiCameras.Length, this.numTouches];
		this.raycastingTransform = this.rayCamera.gameObject.transform;
		switch (this.pointerType)
		{
		case UIManager.POINTER_TYPE.MOUSE:
			this.pointerPoller = new UIManager.PointerPollerDelegate(this.PollMouse);
			this.activePointers[0] = 0;
			this.numActivePointers = 1;
			for (int i = 0; i < this.uiCameras.Length; i++)
			{
				this.pointers[i, 0].id = 0;
				this.pointers[i, 0].fingerID = 0;
				this.pointers[i, 0].rayDepth = this.uiCameras[i].rayDepth;
				this.pointers[i, 0].layerMask = this.uiCameras[i].mask;
				this.pointers[i, 0].camera = this.uiCameras[i].camera;
				this.pointers[i, 0].type = POINTER_INFO.POINTER_TYPE.MOUSE;
			}
			break;
		case UIManager.POINTER_TYPE.TOUCHPAD:
			this.pointerPoller = new UIManager.PointerPollerDelegate(this.PollTouchpad);
			for (int j = 0; j < this.uiCameras.Length; j++)
			{
				for (int k = 0; k < this.numPointers; k++)
				{
					this.pointers[j, k].id = k;
					this.pointers[j, k].rayDepth = this.uiCameras[j].rayDepth;
					this.pointers[j, k].layerMask = this.uiCameras[j].mask;
					this.pointers[j, k].camera = this.uiCameras[j].camera;
					this.pointers[j, k].type = POINTER_INFO.POINTER_TYPE.TOUCHPAD;
				}
			}
			break;
		case UIManager.POINTER_TYPE.AUTO_TOUCHPAD:
			this.pointerPoller = new UIManager.PointerPollerDelegate(this.PollMouseAndTouchpad);
			for (int l = 0; l < this.uiCameras.Length; l++)
			{
				for (int m = 0; m < this.numPointers; m++)
				{
					this.pointers[l, m].id = m;
					this.pointers[l, m].rayDepth = this.uiCameras[l].rayDepth;
					this.pointers[l, m].layerMask = this.uiCameras[l].mask;
					this.pointers[l, m].camera = this.uiCameras[l].camera;
					this.pointers[l, m].type = POINTER_INFO.POINTER_TYPE.TOUCHPAD;
				}
				this.pointers[l, this.numPointers - 1].type = POINTER_INFO.POINTER_TYPE.MOUSE;
			}
			break;
		case UIManager.POINTER_TYPE.RAY:
			this.pointerPoller = new UIManager.PointerPollerDelegate(this.PollRay);
			this.numActivePointers = 0;
			this.rayPtr.type = POINTER_INFO.POINTER_TYPE.RAY;
			this.rayPtr.id = -1;
			this.rayPtr.rayDepth = this.rayDepth;
			this.rayPtr.layerMask = this.rayMask;
			this.rayPtr.camera = this.rayCamera;
			break;
		case UIManager.POINTER_TYPE.MOUSE_AND_RAY:
			this.pointerPoller = new UIManager.PointerPollerDelegate(this.PollMouseRay);
			this.activePointers[0] = 0;
			this.numActivePointers = 1;
			for (int n = 0; n < this.uiCameras.Length; n++)
			{
				this.pointers[n, 0].id = 0;
				this.pointers[n, 0].rayDepth = this.uiCameras[n].rayDepth;
				this.pointers[n, 0].layerMask = this.uiCameras[n].mask;
				this.pointers[n, 0].camera = this.uiCameras[n].camera;
				this.pointers[n, 0].type = POINTER_INFO.POINTER_TYPE.MOUSE;
			}
			this.rayPtr.id = -1;
			this.rayPtr.type = POINTER_INFO.POINTER_TYPE.RAY;
			this.rayPtr.rayDepth = this.rayDepth;
			this.rayPtr.layerMask = this.rayMask;
			this.rayPtr.camera = this.rayCamera;
			break;
		case UIManager.POINTER_TYPE.TOUCHPAD_AND_RAY:
			this.pointerPoller = new UIManager.PointerPollerDelegate(this.PollTouchpadRay);
			for (int num = 0; num < this.uiCameras.Length; num++)
			{
				for (int num2 = 0; num2 < this.numPointers; num2++)
				{
					this.pointers[num, num2].id = num2;
					this.pointers[num, num2].rayDepth = this.uiCameras[num].rayDepth;
					this.pointers[num, num2].layerMask = this.uiCameras[num].mask;
					this.pointers[num, num2].camera = this.uiCameras[num].camera;
					this.pointers[num, num2].type = POINTER_INFO.POINTER_TYPE.TOUCHPAD;
				}
			}
			this.rayPtr.id = -1;
			this.rayPtr.type = POINTER_INFO.POINTER_TYPE.RAY;
			this.rayPtr.rayDepth = this.rayDepth;
			this.rayPtr.layerMask = this.rayMask;
			this.rayPtr.camera = this.rayCamera;
			break;
		}
	}

	public void SetNonUIHitDelegate(UIManager.PointerInfoDelegate del)
	{
		this.informNonUIHit = del;
	}

	public void AddNonUIHitDelegate(UIManager.PointerInfoDelegate del)
	{
		this.informNonUIHit = (UIManager.PointerInfoDelegate)Delegate.Combine(this.informNonUIHit, del);
	}

	public void RemoveNonUIHitDelegate(UIManager.PointerInfoDelegate del)
	{
		this.informNonUIHit = (UIManager.PointerInfoDelegate)Delegate.Remove(this.informNonUIHit, del);
	}

	public void AddMouseTouchPtrListener(UIManager.PointerInfoDelegate del)
	{
		this.mouseTouchListeners = (UIManager.PointerInfoDelegate)Delegate.Combine(this.mouseTouchListeners, del);
	}

	public void AddRayPtrListener(UIManager.PointerInfoDelegate del)
	{
		this.rayListeners = (UIManager.PointerInfoDelegate)Delegate.Combine(this.rayListeners, del);
	}

	public void RemoveMouseTouchPtrListener(UIManager.PointerInfoDelegate del)
	{
		this.mouseTouchListeners = (UIManager.PointerInfoDelegate)Delegate.Remove(this.mouseTouchListeners, del);
	}

	public void RemoveRayPtrListener(UIManager.PointerInfoDelegate del)
	{
		this.rayListeners = (UIManager.PointerInfoDelegate)Delegate.Remove(this.rayListeners, del);
	}

	protected void AddNonUIHit(int ptrIndex, int camIndex)
	{
		if (this.informNonUIHit == null)
		{
			return;
		}
		if (camIndex == -1)
		{
			this.rayIsNonUIHit = true;
			return;
		}
		if (this.usedPointers[ptrIndex])
		{
			return;
		}
		if (this.usedNonUIHits[ptrIndex])
		{
			return;
		}
		this.usedNonUIHits[ptrIndex] = true;
		this.nonUIHits[this.numNonUIHits] = new UIManager.NonUIHitInfo(ptrIndex, camIndex);
		this.numNonUIHits++;
	}

	protected void CallNonUIHitDelegate()
	{
		if (this.informNonUIHit == null)
		{
			return;
		}
		for (int i = 0; i < this.numNonUIHits; i++)
		{
			UIManager.NonUIHitInfo nonUIHitInfo = this.nonUIHits[i];
			this.usedNonUIHits[nonUIHitInfo.ptrIndex] = false;
			if (!this.usedPointers[nonUIHitInfo.ptrIndex])
			{
				this.informNonUIHit(this.pointers[nonUIHitInfo.camIndex, nonUIHitInfo.ptrIndex]);
			}
		}
		if (this.rayIsNonUIHit)
		{
			this.informNonUIHit(this.rayPtr);
		}
	}

	public bool DidPointerHitUI(int id)
	{
		if (this.lastUpdateFrame != Time.frameCount)
		{
			this.Update();
		}
		if (id == -1)
		{
			return this.rayPtr.targetObj != null;
		}
		Mathf.Clamp(id, 0, this.usedPointers.Length - 1);
		return this.usedPointers[id];
	}

	public void AddCamera(Camera cam, LayerMask mask, float depth, int index)
	{
		EZCameraSettings[] array = new EZCameraSettings[this.uiCameras.Length + 1];
		index = Mathf.Clamp(index, 0, this.uiCameras.Length + 1);
		int i = 0;
		int num = 0;
		while (i < array.Length)
		{
			if (i == index)
			{
				array[i] = new EZCameraSettings();
				array[i].camera = cam;
				array[i].mask = mask;
				array[i].rayDepth = depth;
				num++;
			}
			else
			{
				array[i] = this.uiCameras[num];
			}
			i++;
			num++;
		}
		this.uiCameras = array;
		this.SetupPointers();
	}

	public void RemoveCamera(int index)
	{
		EZCameraSettings[] array = new EZCameraSettings[this.uiCameras.Length - 1];
		index = Mathf.Clamp(index, 0, this.uiCameras.Length);
		int i = 0;
		int num = 0;
		while (i < this.uiCameras.Length)
		{
			if (i == index)
			{
				num++;
			}
			else
			{
				array[i] = this.uiCameras[num];
			}
			i++;
			num++;
		}
		this.uiCameras = array;
		this.SetupPointers();
	}

	public void ReplaceCamera(int index, Camera cam)
	{
		index = Mathf.Clamp(index, 0, this.uiCameras.Length);
		this.uiCameras[index].camera = cam;
		this.SetupPointers();
	}

	public void OnLevelWasLoaded(int level)
	{
		for (int i = 0; i < this.uiCameras.Length; i++)
		{
			if (this.uiCameras[i].camera == null)
			{
				this.uiCameras[i].camera = Camera.main;
			}
		}
		if (this.rayCamera == null)
		{
			this.rayCamera = Camera.main;
		}
		if (this.focusObj == null)
		{
			this.FocusObject = null;
		}
		this.blockInput = false;
		this.inputLockCount = 0;
	}

	protected void BeginDrag(ref POINTER_INFO curPtr)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		if (curPtr.targetObj == null)
		{
			return;
		}
		curPtr.targetObj.OnEZDragDrop(new EZDragDropParams(EZDragDropEvent.Begin, curPtr.targetObj, curPtr));
		curPtr.targetObj.DragUpdatePosition(curPtr);
	}

	private int CompareZOrder(RaycastHit a, RaycastHit b)
	{
		return a.transform.position.z.CompareTo(b.transform.position.z);
	}

	protected void DoDragUpdate(POINTER_INFO curPtr)
	{
		IUIObject targetObj = curPtr.targetObj;
		if (targetObj == null)
		{
			return;
		}
		if (!targetObj.DragUpdatePosition(curPtr))
		{
			curPtr.targetObj.SetDragging(false);
			curPtr.targetObj = null;
			return;
		}
		RaycastHit[] array = Physics.RaycastAll(curPtr.ray, curPtr.rayDepth, curPtr.layerMask);
		bool flag = false;
		this.list.Clear();
		if (0 < array.Length)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].transform.gameObject.GetComponent<UIScrollList>())
				{
					this.list.Add(array[i]);
				}
			}
			this.list.Sort(new Comparison<RaycastHit>(this.CompareZOrder));
		}
		for (int j = 0; j < this.list.Count; j++)
		{
			RaycastHit raycastHit = this.list[j];
			if (!(null == raycastHit.transform))
			{
				if (raycastHit.transform.position.z != targetObj.transform.position.z)
				{
					targetObj.DropTarget = raycastHit.transform.gameObject;
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			targetObj.DropTarget = null;
		}
		if (null == targetObj.gameObject)
		{
			curPtr.targetObj.SetDragging(false);
			curPtr.targetObj = null;
			return;
		}
		POINTER_INFO.INPUT_EVENT evt = curPtr.evt;
		if (evt != POINTER_INFO.INPUT_EVENT.NO_CHANGE)
		{
			if (evt == POINTER_INFO.INPUT_EVENT.RELEASE)
			{
				targetObj.gameObject.SendMessage("OnEZDragDrop", new EZDragDropParams(EZDragDropEvent.Dropped, targetObj, curPtr), SendMessageOptions.DontRequireReceiver);
				return;
			}
			if (evt != POINTER_INFO.INPUT_EVENT.DRAG)
			{
				targetObj.gameObject.SendMessage("OnEZDragDrop", new EZDragDropParams(EZDragDropEvent.Dropped, targetObj, curPtr), SendMessageOptions.DontRequireReceiver);
				return;
			}
		}
		targetObj.gameObject.SendMessage("OnEZDragDrop", new EZDragDropParams(EZDragDropEvent.Update, targetObj, curPtr), SendMessageOptions.DontRequireReceiver);
	}

	public virtual bool Update()
	{
		if ((TsPlatform.IsWeb || TsPlatform.IsEditor) && this.focusObj != null && NkInputManager.imeCompositionMode == IMECompositionMode.Off && !((IKeyFocusable)this.focusObj).NumberMode)
		{
			NkInputManager.imeCompositionMode = IMECompositionMode.On;
		}
		if (this.bKeyTimer)
		{
			if (this.siNewKey == this.siOldKey)
			{
				this.siKeyTimer++;
			}
			else
			{
				this.siOldKey = this.siNewKey;
				this.siKeyTimer = 1;
			}
		}
		else
		{
			this.siKeyTimer = 0;
		}
		if (this.pointerPoller != null)
		{
			this.pointerPoller();
		}
		if (this.focusObj != null && !this.bfocusOn)
		{
			if (NkInputManager.inputString.Length > 0 && NkInputManager.inputString[0] != '\n' && NkInputManager.inputString[0] != '\r')
			{
				this.checkReturn = false;
				if (0f < this.firstKeyTime && Time.realtimeSinceStartup - this.firstKeyTime < 0.2f)
				{
					return false;
				}
			}
			if (this.checkReturn)
			{
				if (NkInputManager.inputString.Length <= 0)
				{
					this.checkReturn = false;
					return false;
				}
				if (NkInputManager.inputString[0] == '\n' || NkInputManager.inputString[0] == '\r')
				{
					return false;
				}
			}
			this.PollKeyboard();
		}
		else
		{
			this.bfocusOn = false;
		}
		this.DispatchInput();
		if (this.focusObj != null && (TsPlatform.IsWeb || TsPlatform.IsEditor))
		{
			((IKeyFocusable)this.focusObj).ToggleCaretShow();
		}
		return this.clickUI;
	}

	public void ClearInputQueue()
	{
		this.inputQueue.Clear();
	}

	protected void DispatchInput()
	{
		this.numNonUIHits = 0;
		this.rayIsNonUIHit = false;
		for (int i = 0; i < this.usedPointers.Length; i++)
		{
			this.usedPointers[i] = false;
		}
		if (TsPlatform.IsWeb)
		{
			if (TsPlatform.IsWeb)
			{
				if (this.mouseTouchListeners != null)
				{
					this.DispatchHelper(ref this.pointers[0, 0], 0);
					if (this.mouseTouchListeners != null)
					{
						this.mouseTouchListeners(this.pointers[0, 0]);
					}
				}
				else
				{
					this.DispatchHelper(ref this.pointers[0, 0], 0);
				}
			}
		}
		else
		{
			if (0 >= this.inputQueue.Count)
			{
				return;
			}
			if (this.mouseTouchListeners != null)
			{
				for (int j = 0; j < this.numActivePointers; j++)
				{
					for (int k = 0; k < this.uiCameras.Length; k++)
					{
						if (this.uiCameras[k].camera.gameObject.activeInHierarchy)
						{
							POINTER_INFO ptr = this.inputQueue.Dequeue();
							this.DispatchHelper(ref ptr, 0);
							if (this.mouseTouchListeners != null)
							{
								this.mouseTouchListeners(ptr);
							}
							if (TsPlatform.IsWeb)
							{
								this.pointers[0, 0].Copy(ptr);
							}
							else
							{
								this.pointers[k, ptr.fingerID].Copy(ptr);
							}
						}
					}
				}
			}
			else
			{
				for (int l = 0; l < this.numActivePointers; l++)
				{
					for (int m = 0; m < this.uiCameras.Length; m++)
					{
						if (this.uiCameras[m].camera.gameObject.activeInHierarchy)
						{
							POINTER_INFO ptr2 = this.inputQueue.Dequeue();
							this.DispatchHelper(ref ptr2, 0);
							if (TsPlatform.IsWeb)
							{
								this.pointers[0, 0].Copy(ptr2);
							}
							else
							{
								this.pointers[m, ptr2.fingerID].Copy(ptr2);
							}
						}
					}
				}
			}
		}
		if (this.pointerType == UIManager.POINTER_TYPE.RAY || this.pointerType == UIManager.POINTER_TYPE.MOUSE_AND_RAY || this.pointerType == UIManager.POINTER_TYPE.TOUCHPAD_AND_RAY)
		{
			this.DispatchHelper(ref this.rayPtr, -1);
			if (this.rayListeners != null)
			{
				this.rayListeners(this.rayPtr);
			}
		}
		this.CallNonUIHitDelegate();
	}

	protected void DispatchHelper(ref POINTER_INFO curPtr, int camIndex)
	{
		if (curPtr.targetObj != null && curPtr.targetObj.IsDragging())
		{
			this.DoDragUpdate(curPtr);
			this.clickUI = true;
		}
		else
		{
			switch (curPtr.evt)
			{
			case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
				if (this.FocusObject == null)
				{
					this.clickUI = false;
				}
				break;
			case POINTER_INFO.INPUT_EVENT.PRESS:
			case POINTER_INFO.INPUT_EVENT.DOUBLE_PRESS:
			case POINTER_INFO.INPUT_EVENT.RIGHT_PRESS:
				if (Physics.Raycast(curPtr.ray, out this.hit, curPtr.rayDepth, curPtr.layerMask))
				{
					this.DragUpUI = true;
					if (TsPlatform.IsMobile)
					{
						this.pressUI = true;
					}
					this.clickUI = true;
					this.tempObj = (IUIObject)this.hit.collider.gameObject.GetComponent("IUIObject");
					if (this.tempObj == null)
					{
						this.AddNonUIHit(curPtr.id, camIndex);
						if (this.warnOnNonUiHits)
						{
							this.LogNonUIObjErr(this.hit.collider.gameObject);
						}
					}
					curPtr.hitInfo = this.hit;
					if (this.tempObj != curPtr.targetObj && curPtr.targetObj != null)
					{
						this.tempPtr.Copy(curPtr);
						this.tempPtr.evt = POINTER_INFO.INPUT_EVENT.MOVE_OFF;
						if (!this.blockInput)
						{
							curPtr.targetObj.OnInput(this.tempPtr);
						}
					}
					if (!this.blockInput)
					{
						curPtr.targetObj = this.tempObj;
					}
					else
					{
						if (curPtr.targetObj != null)
						{
							this.tempPtr.Copy(curPtr);
							this.tempPtr.evt = POINTER_INFO.INPUT_EVENT.RELEASE_OFF;
							curPtr.targetObj.OnInput(this.tempPtr);
						}
						curPtr.targetObj = null;
					}
					if (curPtr.targetObj != null)
					{
						if (curPtr.targetObj != this.focusObj && curPtr.type == POINTER_INFO.POINTER_TYPE.RAY == this.focusWithRay)
						{
							if (curPtr.targetObj.GotFocus())
							{
								this.FocusObject = curPtr.targetObj;
							}
							else
							{
								this.FocusObject = null;
							}
						}
						if (!this.blockInput)
						{
							curPtr.targetObj.OnInput(curPtr);
							if (curPtr.targetObj is UITextField)
							{
								this.insert = ((UITextField)curPtr.targetObj).GetInsertPos();
							}
						}
					}
					else if (curPtr.type == POINTER_INFO.POINTER_TYPE.RAY == this.focusWithRay)
					{
						this.FocusObject = null;
					}
				}
				else
				{
					this.DragUpUI = false;
					curPtr.hitInfo = default(RaycastHit);
					if (this.blockInput && curPtr.targetObj != null)
					{
						this.tempPtr.Copy(curPtr);
						this.tempPtr.evt = POINTER_INFO.INPUT_EVENT.RELEASE_OFF;
						curPtr.targetObj.OnInput(this.tempPtr);
					}
					curPtr.targetObj = null;
					if (curPtr.type == POINTER_INFO.POINTER_TYPE.RAY == this.focusWithRay)
					{
						this.FocusObject = null;
					}
				}
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.RIGHT_RELEASE:
			case POINTER_INFO.INPUT_EVENT.TAP:
			case POINTER_INFO.INPUT_EVENT.RIGHT_TAP:
			case POINTER_INFO.INPUT_EVENT.LONG_TAP:
			case POINTER_INFO.INPUT_EVENT.DRAG:
				if (curPtr.evt == POINTER_INFO.INPUT_EVENT.RELEASE || curPtr.evt == POINTER_INFO.INPUT_EVENT.TAP || curPtr.evt == POINTER_INFO.INPUT_EVENT.RIGHT_RELEASE || curPtr.evt == POINTER_INFO.INPUT_EVENT.RIGHT_TAP || curPtr.evt == POINTER_INFO.INPUT_EVENT.LONG_TAP)
				{
					this.tempObj = null;
					if (Physics.Raycast(curPtr.ray, out this.hit, curPtr.rayDepth, curPtr.layerMask))
					{
						if (TsPlatform.IsMobile)
						{
							this.pressUI = true;
						}
						this.clickUI = true;
						this.DragUpUI = true;
						this.tempObj = (IUIObject)this.hit.collider.gameObject.GetComponent("IUIObject");
						curPtr.hitInfo = this.hit;
						if (this.tempObj == null)
						{
							this.AddNonUIHit(curPtr.id, camIndex);
						}
					}
					else
					{
						this.DragUpUI = false;
						curPtr.hitInfo = default(RaycastHit);
					}
					if (this.tempObj != curPtr.targetObj)
					{
						if (curPtr.targetObj != null)
						{
							this.tempPtr.Copy(curPtr);
							if (curPtr.evt == POINTER_INFO.INPUT_EVENT.RELEASE || curPtr.evt == POINTER_INFO.INPUT_EVENT.RIGHT_RELEASE)
							{
								this.tempPtr.evt = POINTER_INFO.INPUT_EVENT.RELEASE_OFF;
							}
							else
							{
								this.tempPtr.evt = curPtr.evt;
							}
							curPtr.targetObj.OnInput(this.tempPtr);
							if (curPtr.id >= 0)
							{
								this.usedPointers[curPtr.id] = true;
							}
							if (!this.blockInput)
							{
								curPtr.targetObj = this.tempObj;
							}
						}
						if (this.tempObj != null && curPtr.evt != POINTER_INFO.INPUT_EVENT.TAP && curPtr.evt != POINTER_INFO.INPUT_EVENT.RIGHT_TAP && !this.blockInput)
						{
							this.tempObj.OnInput(curPtr);
						}
					}
					else if (curPtr.targetObj != null)
					{
						curPtr.targetObj.OnInput(curPtr);
						if (curPtr.id >= 0)
						{
							this.usedPointers[curPtr.id] = true;
						}
					}
					if (curPtr.type == POINTER_INFO.POINTER_TYPE.TOUCHPAD)
					{
						curPtr.targetObj = null;
					}
				}
				else if (Physics.Raycast(curPtr.ray, out this.hit, curPtr.rayDepth, curPtr.layerMask))
				{
					this.DragUpUI = true;
					this.clickUI = true;
					curPtr.hitInfo = this.hit;
					if (curPtr.targetObj == null)
					{
						this.AddNonUIHit(curPtr.id, camIndex);
					}
					if (curPtr.targetObj != null && !this.blockInput)
					{
						curPtr.targetObj.OnInput(curPtr);
						if (curPtr.targetObj.IsDraggable && !curPtr.isTap)
						{
							if (!NkInputManager.GetMouseButton(1) && curPtr.evt != POINTER_INFO.INPUT_EVENT.RIGHT_RELEASE && curPtr.evt != POINTER_INFO.INPUT_EVENT.RIGHT_TAP && curPtr.evt != POINTER_INFO.INPUT_EVENT.RIGHT_PRESS)
							{
								this.BeginDrag(ref curPtr);
							}
						}
					}
				}
				else
				{
					this.DragUpUI = false;
					curPtr.hitInfo = default(RaycastHit);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
			case POINTER_INFO.INPUT_EVENT.MOUSE_WHEEL:
			{
				this.tempObj = null;
				bool flag = Physics.Raycast(curPtr.ray, out this.hit, curPtr.rayDepth, curPtr.layerMask);
				if (flag)
				{
					this.clickUI = true;
					this.DragUpUI = true;
					this.tempObj = (IUIObject)this.hit.collider.gameObject.GetComponent("IUIObject");
					curPtr.hitInfo = this.hit;
					if (this.tempObj == null)
					{
						this.AddNonUIHit(curPtr.id, camIndex);
						if (this.warnOnNonUiHits)
						{
							this.LogNonUIObjErr(this.hit.collider.gameObject);
						}
					}
					if (!curPtr.active)
					{
						if (curPtr.targetObj != this.tempObj && curPtr.targetObj != null)
						{
							this.tempPtr.Copy(curPtr);
							this.tempPtr.evt = POINTER_INFO.INPUT_EVENT.MOVE_OFF;
							if (!this.blockInput)
							{
								curPtr.targetObj.OnInput(this.tempPtr);
							}
						}
						if (!this.blockInput)
						{
							curPtr.targetObj = this.tempObj;
							if (this.tempObj != null)
							{
								curPtr.targetObj.OnInput(curPtr);
							}
						}
					}
					else if (curPtr.targetObj != null && !this.blockInput)
					{
						curPtr.targetObj.OnInput(curPtr);
					}
				}
				else
				{
					this.DragUpUI = false;
					curPtr.hitInfo = default(RaycastHit);
					if (curPtr.targetObj != null && !curPtr.active)
					{
						curPtr.evt = POINTER_INFO.INPUT_EVENT.MOVE_OFF;
						curPtr.targetObj.OnInput(curPtr);
					}
					if (!curPtr.active)
					{
						curPtr.targetObj = null;
					}
					if (this.FocusObject == null)
					{
						this.clickUI = false;
					}
				}
				break;
			}
			}
		}
		if (curPtr.targetObj != null)
		{
			if (curPtr.id >= 0)
			{
				this.usedPointers[curPtr.id] = true;
			}
			if (curPtr.evt != POINTER_INFO.INPUT_EVENT.NO_CHANGE)
			{
				this.clickUI = true;
			}
		}
	}

	protected void PollMouse()
	{
		this.pointers[0, 0].evt = POINTER_INFO.INPUT_EVENT.NO_CHANGE;
		this.PollMouse(ref this.pointers[0, 0]);
		for (int i = 1; i < this.uiCameras.Length; i++)
		{
			if (this.uiCameras[i].camera.gameObject.activeInHierarchy)
			{
				this.pointers[i, 0].Reuse(this.pointers[0, 0]);
				this.pointers[i, 0].prevRay = this.pointers[i, 0].ray;
				this.pointers[i, 0].ray = this.uiCameras[i].camera.ScreenPointToRay(this.pointers[i, 0].devicePos);
			}
		}
	}

	protected void PollMouseAndTouchpad()
	{
		if (TsPlatform.IsMobile)
		{
			this.PollTouchpad();
			if (TsPlatform.IsEditor)
			{
				this.numActivePointers++;
			}
		}
		else
		{
			this.numActivePointers = 1;
		}
		if (TsPlatform.IsWeb)
		{
			int num = this.numTouches - 1;
			this.activePointers[this.numActivePointers - 1] = num;
			this.pointers[0, num].evt = POINTER_INFO.INPUT_EVENT.NO_CHANGE;
			this.PollMouse(ref this.pointers[0, num]);
			for (int i = 1; i < this.uiCameras.Length; i++)
			{
				if (this.uiCameras[i].camera.gameObject.activeInHierarchy)
				{
					this.pointers[i, num].Reuse(this.pointers[0, num]);
					this.pointers[i, num].prevRay = this.pointers[i, num].ray;
					this.pointers[i, num].ray = this.uiCameras[i].camera.ScreenPointToRay(this.pointers[i, num].devicePos);
				}
			}
		}
	}

	protected bool PollMouseDown(ref POINTER_INFO curPtr)
	{
		this.down = Input.GetMouseButtonDown(0);
		this.rightdown = Input.GetMouseButtonDown(1);
		if (this.down && !curPtr.active)
		{
			if (curPtr.clickTime > 0f && Time.time - curPtr.clickTime < 0.3f)
			{
				curPtr.Reset(this.curActionID++);
				curPtr.evt = POINTER_INFO.INPUT_EVENT.DOUBLE_PRESS;
				curPtr.active = true;
				curPtr.inputDelta = NkInputManager.mousePosition - curPtr.devicePos;
				curPtr.origPos = NkInputManager.mousePosition;
				curPtr.isTap = true;
				curPtr.clickTime = Time.time;
			}
			else
			{
				curPtr.Reset(this.curActionID++);
				curPtr.evt = POINTER_INFO.INPUT_EVENT.PRESS;
				curPtr.active = true;
				curPtr.inputDelta = NkInputManager.mousePosition - curPtr.devicePos;
				curPtr.origPos = NkInputManager.mousePosition;
				curPtr.isTap = true;
				curPtr.clickTime = Time.time;
			}
			return true;
		}
		if (this.rightdown && !curPtr.active)
		{
			curPtr.Reset(this.curActionID++);
			curPtr.evt = POINTER_INFO.INPUT_EVENT.RIGHT_PRESS;
			curPtr.active = true;
			curPtr.inputDelta = NkInputManager.mousePosition - curPtr.devicePos;
			curPtr.origPos = NkInputManager.mousePosition;
			curPtr.isTap = true;
			return true;
		}
		return false;
	}

	protected bool PollMouseUp(ref POINTER_INFO curPtr)
	{
		if (curPtr.active)
		{
			if (NkInputManager.GetMouseButtonUp(0))
			{
				curPtr.inputDelta = NkInputManager.mousePosition - curPtr.devicePos;
				curPtr.devicePos = NkInputManager.mousePosition;
				if (curPtr.isTap)
				{
					this.tempVec = curPtr.origPos - curPtr.devicePos;
					if (Mathf.Abs(this.tempVec.x) > this.dragThreshold || Mathf.Abs(this.tempVec.y) > this.dragThreshold)
					{
						curPtr.isTap = false;
					}
				}
				if (curPtr.isTap && Time.time - curPtr.clickTime > 2f)
				{
					curPtr.evt = POINTER_INFO.INPUT_EVENT.LONG_TAP;
				}
				else if (curPtr.isTap)
				{
					curPtr.evt = POINTER_INFO.INPUT_EVENT.TAP;
				}
				else
				{
					curPtr.evt = POINTER_INFO.INPUT_EVENT.RELEASE;
				}
				curPtr.active = false;
				return true;
			}
			if (NkInputManager.GetMouseButtonUp(1))
			{
				curPtr.inputDelta = NkInputManager.mousePosition - curPtr.devicePos;
				curPtr.devicePos = NkInputManager.mousePosition;
				if (curPtr.isTap)
				{
					this.tempVec = curPtr.origPos - curPtr.devicePos;
					if (Mathf.Abs(this.tempVec.x) > this.dragThreshold || Mathf.Abs(this.tempVec.y) > this.dragThreshold)
					{
						curPtr.isTap = false;
					}
				}
				if (curPtr.isTap)
				{
					curPtr.evt = POINTER_INFO.INPUT_EVENT.RIGHT_TAP;
				}
				else
				{
					curPtr.evt = POINTER_INFO.INPUT_EVENT.RIGHT_RELEASE;
				}
				curPtr.active = false;
				return true;
			}
		}
		return false;
	}

	protected bool PollMouseOthers(ref POINTER_INFO curPtr)
	{
		if (NkInputManager.mousePosition != curPtr.devicePos)
		{
			if (curPtr.active)
			{
				curPtr.evt = POINTER_INFO.INPUT_EVENT.DRAG;
				curPtr.inputDelta = NkInputManager.mousePosition - curPtr.devicePos;
				curPtr.devicePos = NkInputManager.mousePosition;
				if (curPtr.isTap)
				{
					this.tempVec = curPtr.origPos - curPtr.devicePos;
					if (Mathf.Abs(this.tempVec.x) > this.dragThreshold || Mathf.Abs(this.tempVec.y) > this.dragThreshold)
					{
						curPtr.isTap = false;
					}
				}
			}
			else
			{
				curPtr.evt = POINTER_INFO.INPUT_EVENT.MOVE;
				curPtr.inputDelta = NkInputManager.mousePosition - curPtr.devicePos;
				curPtr.devicePos = NkInputManager.mousePosition;
			}
		}
		else
		{
			curPtr.evt = POINTER_INFO.INPUT_EVENT.NO_CHANGE;
			curPtr.inputDelta = Vector3.zero;
		}
		if (NkInputManager.GetAxisRaw("Mouse ScrollWheel") != 0f)
		{
			curPtr.evt = POINTER_INFO.INPUT_EVENT.MOUSE_WHEEL;
		}
		return true;
	}

	protected void PollMouse(ref POINTER_INFO curPtr)
	{
		if (this.PollMouseDown(ref curPtr))
		{
			curPtr.devicePos = NkInputManager.mousePosition;
			curPtr.prevRay = curPtr.ray;
			curPtr.ray = this.uiCameras[0].camera.ScreenPointToRay(curPtr.devicePos);
		}
		else if (this.PollMouseUp(ref curPtr))
		{
			curPtr.devicePos = NkInputManager.mousePosition;
			curPtr.prevRay = curPtr.ray;
			curPtr.ray = this.uiCameras[0].camera.ScreenPointToRay(curPtr.devicePos);
		}
		else if (this.PollMouseOthers(ref curPtr))
		{
			curPtr.devicePos = NkInputManager.mousePosition;
			curPtr.prevRay = curPtr.ray;
			curPtr.ray = this.uiCameras[0].camera.ScreenPointToRay(curPtr.devicePos);
		}
		if (TsPlatform.IsMobile)
		{
			default(POINTER_INFO).Copy(curPtr);
			this.inputQueue.Enqueue(curPtr);
		}
	}

	protected void PollTouchpad()
	{
		if (TsPlatform.IsMobile)
		{
			this.numActivePointers = Mathf.Min(this.numTouches, NkInputManager.touchCount);
			for (int i = 0; i < this.numActivePointers; i++)
			{
				Touch touch = NkInputManager.GetTouch(i);
				int num = touch.fingerId;
				if (num >= this.numTouchPointers)
				{
					num = this.numTouchPointers - 1;
				}
				this.activePointers[i] = num;
				switch (touch.phase)
				{
				case TouchPhase.Began:
					this.pointers[0, num].Reset(this.curActionID++);
					this.pointers[0, num].fingerID = this.activePointers[i];
					this.pointers[0, num].evt = POINTER_INFO.INPUT_EVENT.PRESS;
					this.pointers[0, num].active = true;
					this.pointers[0, num].inputDelta = Vector3.zero;
					this.pointers[0, num].origPos = touch.position;
					this.pointers[0, num].isTap = true;
					this.pointers[0, num].clickTime = Time.time;
					break;
				case TouchPhase.Moved:
					this.pointers[0, num].evt = POINTER_INFO.INPUT_EVENT.DRAG;
					this.pointers[0, num].inputDelta = touch.deltaPosition;
					this.pointers[0, num].devicePos = touch.position;
					if (this.pointers[0, num].isTap)
					{
						this.tempVec = this.pointers[0, num].origPos - this.pointers[0, num].devicePos;
						if (Mathf.Abs(this.tempVec.x) > this.dragThreshold || Mathf.Abs(this.tempVec.y) > this.dragThreshold)
						{
							this.pointers[0, num].isTap = false;
						}
					}
					break;
				case TouchPhase.Stationary:
					this.pointers[0, num].evt = POINTER_INFO.INPUT_EVENT.NO_CHANGE;
					this.pointers[0, num].inputDelta = Vector3.zero;
					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					if (Time.time - this.pointers[0, num].clickTime > 2f)
					{
						this.pointers[0, num].evt = POINTER_INFO.INPUT_EVENT.LONG_TAP;
					}
					else if (this.pointers[0, num].isTap)
					{
						this.pointers[0, num].evt = POINTER_INFO.INPUT_EVENT.TAP;
					}
					else if (this.dragClick)
					{
						this.pointers[0, num].evt = POINTER_INFO.INPUT_EVENT.TAP;
						this.dragClick = false;
					}
					else
					{
						this.pointers[0, num].evt = POINTER_INFO.INPUT_EVENT.RELEASE;
					}
					this.pointers[0, num].inputDelta = touch.deltaPosition;
					this.pointers[0, num].active = false;
					break;
				}
				this.pointers[0, num].devicePos = touch.position;
				this.pointers[0, num].prevRay = this.pointers[0, num].ray;
				this.pointers[0, num].ray = this.uiCameras[0].camera.ScreenPointToRay(this.pointers[0, num].devicePos);
				POINTER_INFO item = default(POINTER_INFO);
				item.Copy(this.pointers[0, num]);
				this.inputQueue.Enqueue(item);
			}
			for (int j = 1; j < this.uiCameras.Length; j++)
			{
				for (int k = 0; k < this.numActivePointers; k++)
				{
					int num2 = this.activePointers[k];
					this.pointers[j, num2].Reuse(this.pointers[0, num2]);
					this.pointers[j, num2].prevRay = this.pointers[j, num2].ray;
					this.pointers[j, num2].ray = this.uiCameras[j].camera.ScreenPointToRay(this.pointers[j, num2].devicePos);
				}
			}
		}
	}

	protected void PollRay()
	{
		if (this.actionAxis.Length != 0)
		{
			this.rayActive = NkInputManager.GetButton(this.actionAxis);
		}
		else
		{
			this.rayActive = (this.rayState != UIManager.RAY_ACTIVE_STATE.Inactive);
			if (this.rayState == UIManager.RAY_ACTIVE_STATE.Momentary)
			{
				this.rayState = UIManager.RAY_ACTIVE_STATE.Inactive;
			}
		}
		if (this.rayActive && this.rayPtr.active)
		{
			if (this.raycastingTransform.forward != this.rayPtr.ray.direction || this.raycastingTransform.position != this.rayPtr.ray.origin)
			{
				this.rayPtr.evt = POINTER_INFO.INPUT_EVENT.DRAG;
				this.tempVec = this.raycastingTransform.position + this.raycastingTransform.forward * this.rayDepth;
				this.rayPtr.inputDelta = this.tempVec - this.rayPtr.devicePos;
				this.rayPtr.devicePos = this.tempVec;
				if (this.rayPtr.isTap)
				{
					this.tempVec = this.rayPtr.origPos - this.rayPtr.devicePos;
					if (this.tempVec.sqrMagnitude > this.rayDragThreshold * this.rayDragThreshold)
					{
						this.rayPtr.isTap = false;
					}
				}
			}
			else
			{
				this.rayPtr.evt = POINTER_INFO.INPUT_EVENT.NO_CHANGE;
				this.rayPtr.inputDelta = Vector3.zero;
			}
		}
		else if (this.rayActive && !this.rayPtr.active)
		{
			this.rayPtr.Reset(this.curActionID++);
			this.rayPtr.evt = POINTER_INFO.INPUT_EVENT.PRESS;
			this.rayPtr.active = true;
			this.rayPtr.origPos = this.raycastingTransform.position + this.raycastingTransform.forward * this.rayDepth;
			this.rayPtr.inputDelta = this.rayPtr.origPos - this.rayPtr.devicePos;
			this.rayPtr.devicePos = this.rayPtr.origPos;
			this.rayPtr.isTap = true;
		}
		else if (!this.rayActive && this.rayPtr.active)
		{
			if (this.rayPtr.isTap)
			{
				this.rayPtr.evt = POINTER_INFO.INPUT_EVENT.TAP;
			}
			else
			{
				this.rayPtr.evt = POINTER_INFO.INPUT_EVENT.RELEASE;
			}
			this.tempVec = this.raycastingTransform.position + this.raycastingTransform.forward * this.rayDepth;
			this.rayPtr.inputDelta = this.tempVec - this.rayPtr.devicePos;
			this.rayPtr.devicePos = this.tempVec;
			this.rayPtr.active = false;
		}
		else if (!this.rayActive && NkInputManager.mousePosition != this.rayPtr.devicePos)
		{
			this.rayPtr.evt = POINTER_INFO.INPUT_EVENT.MOVE;
			this.tempVec = this.raycastingTransform.position + this.raycastingTransform.forward * this.rayDepth;
			this.rayPtr.inputDelta = this.tempVec - this.rayPtr.devicePos;
			this.rayPtr.devicePos = this.tempVec;
		}
		else
		{
			this.rayPtr.evt = POINTER_INFO.INPUT_EVENT.NO_CHANGE;
			this.rayPtr.inputDelta = Vector3.zero;
		}
		this.rayPtr.prevRay = this.rayPtr.ray;
		this.rayPtr.ray = new Ray(this.raycastingTransform.position, this.raycastingTransform.forward);
	}

	protected void PollMouseRay()
	{
		this.PollMouse();
		this.PollRay();
	}

	protected void PollTouchpadRay()
	{
		this.PollTouchpad();
		this.PollRay();
	}

	protected static int FindInsertionPoint(string before, string after)
	{
		if (before == null || after == null)
		{
			return 0;
		}
		int num = 0;
		while (num < before.Length && num < after.Length)
		{
			if (before[num] != after[num])
			{
				return num + 1;
			}
			num++;
		}
		return after.Length;
	}

	protected void PollKeyboard()
	{
		if (TsPlatform.IsMobile)
		{
			if (!Application.isEditor)
			{
				if (this.iKeyboard == null)
				{
					return;
				}
				if (this.focusObj == null)
				{
					return;
				}
				if (this.iKeyboard.done)
				{
					this.controlText = this.iKeyboard.text;
					if (this.focusObj is UITextField)
					{
						this.controlText = ((UITextField)this.focusObj).SetInputText(this.controlText, ref this.insert);
					}
					else
					{
						this.controlText = ((IKeyFocusable)this.focusObj).SetInputText(this.controlText, ref this.insert);
					}
					((IKeyFocusable)this.focusObj).Commit();
					this.FocusObject = null;
					return;
				}
				if (this.controlText == this.iKeyboard.text)
				{
					return;
				}
				string before = this.controlText;
				this.controlText = this.iKeyboard.text;
				this.insert = UIManager.FindInsertionPoint(before, this.controlText);
				((IKeyFocusable)this.focusObj).SetInputText(this.controlText, ref this.insert);
			}
			else
			{
				this.ProcessKeyboard();
			}
		}
		else
		{
			if (this.focusObj == null && !this.m_bActiveFocus)
			{
				return;
			}
			if (TsPlatform.IsWeb || TsPlatform.IsEditor)
			{
				if (NkInputManager.imeCompositionMode == IMECompositionMode.On)
				{
					this.compositionText = NkInputManager.compositionString;
				}
			}
			this.ProcessKeyboard();
			if (NkInputManager.GetKeyDown(KeyCode.Home))
			{
				this.controlText = ((IKeyFocusable)this.focusObj).Content;
				this.insert = 0;
				((IKeyFocusable)this.focusObj).SetInputText(this.controlText, ref this.insert);
			}
			else if (NkInputManager.GetKeyDown(KeyCode.End))
			{
				this.controlText = ((IKeyFocusable)this.focusObj).Content;
				this.insert = this.controlText.Length;
				((IKeyFocusable)this.focusObj).SetInputText(this.controlText, ref this.insert);
			}
			else if (NkInputManager.GetKey(KeyCode.RightArrow))
			{
				if (this.siKeyTimer == 0 || this.siKeyTimer >= 10)
				{
					this.controlText = ((IKeyFocusable)this.focusObj).Content;
					this.insert = Mathf.Min(this.controlText.Length, this.insert + 1);
					((IKeyFocusable)this.focusObj).SetInputText(this.controlText, ref this.insert);
				}
				this.bKeyTimer = true;
				this.siNewKey = 1;
			}
			else if (NkInputManager.GetKey(KeyCode.LeftArrow))
			{
				if (this.siKeyTimer == 0 || this.siKeyTimer >= 10)
				{
					this.controlText = ((IKeyFocusable)this.focusObj).Content;
					this.insert = Mathf.Max(0, this.insert - 1);
					((IKeyFocusable)this.focusObj).SetInputText(this.controlText, ref this.insert);
				}
				this.bKeyTimer = true;
				this.siNewKey = 2;
			}
			else if (NkInputManager.GetKey(KeyCode.UpArrow))
			{
				if (this.siKeyTimer == 0 || this.siKeyTimer >= 10)
				{
					((IKeyFocusable)this.focusObj).GoUp();
				}
				this.bKeyTimer = true;
				this.siNewKey = 4;
			}
			else if (NkInputManager.GetKey(KeyCode.DownArrow))
			{
				if (this.siKeyTimer == 0 || this.siKeyTimer >= 10)
				{
					((IKeyFocusable)this.focusObj).GoDown();
				}
				this.bKeyTimer = true;
				this.siNewKey = 5;
			}
			else if (NkInputManager.GetKey(KeyCode.Delete))
			{
				if (this.siKeyTimer == 0 || this.siKeyTimer >= 10)
				{
					this.controlText = ((IKeyFocusable)this.focusObj).Content;
					if (this.insert < this.sb.Length && this.controlText.Length > 0)
					{
						if (this.sb.Length > 0)
						{
							this.sb.Replace(this.sb.ToString(), this.controlText);
						}
						else
						{
							this.sb.Append(this.controlText);
						}
						this.sb.Remove(this.insert, 1);
						((IKeyFocusable)this.focusObj).OriginalContent = this.sb.ToString();
						((IKeyFocusable)this.focusObj).SetInputText(this.sb.ToString(), ref this.insert);
					}
				}
				this.bKeyTimer = true;
				this.siNewKey = 3;
			}
			else
			{
				this.bKeyTimer = false;
			}
		}
	}

	protected void ProcessKeyboard()
	{
		if (this.focusObj == null)
		{
			return;
		}
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			if (NkInputManager.inputString.Length == 0)
			{
				return;
			}
		}
		else if (NkInputManager.inputString.Length == 0 && this.compositionText.Length == 0)
		{
			if (this.bLastWord)
			{
				this.bLastWord = false;
				this.oldCompStatus = 0;
				this.controlText = ((IKeyFocusable)this.focusObj).OriginalContent;
				this.insert = Mathf.Clamp(this.insert, 0, this.controlText.Length);
				this.controlText = ((IKeyFocusable)this.focusObj).SetInputText(this.controlText, ref this.insert);
			}
			else if (this.bLinkText)
			{
				this.bLinkText = false;
				this.insert = Mathf.Clamp(this.insert, 0, this.controlText.Length);
				this.controlText = ((IKeyFocusable)this.focusObj).SetInputText(this.controlText, ref this.insert);
			}
			return;
		}
		this.bLastWord = true;
		if (0 < NkInputManager.inputString.Length)
		{
			this.controlText = ((IKeyFocusable)this.focusObj).OriginalContent;
			this.insert = Mathf.Clamp(this.insert, 0, this.controlText.Length);
			if (this.insert == 0 && NkInputManager.inputString.IndexOf('\b') == 0)
			{
				return;
			}
			if (this.sb.Length > 0)
			{
				this.sb.Replace(this.sb.ToString(), this.controlText);
			}
			else
			{
				this.sb.Append(this.controlText);
			}
			string inputString = NkInputManager.inputString;
			for (int i = 0; i < inputString.Length; i++)
			{
				char c = inputString[i];
				if (((IKeyFocusable)this.focusObj).NumberMode || !NkInputManager.GetKey(KeyCode.LeftShift) || NkInputManager.GetKeyDown(KeyCode.Alpha1) || NkInputManager.GetKeyDown(KeyCode.Alpha2) || NkInputManager.GetKeyDown(KeyCode.Alpha3) || NkInputManager.GetKeyDown(KeyCode.Alpha4) || NkInputManager.GetKeyDown(KeyCode.Alpha5) || NkInputManager.GetKeyDown(KeyCode.Alpha6) || NkInputManager.GetKeyDown(KeyCode.Alpha7) || NkInputManager.GetKeyDown(KeyCode.Alpha8) || NkInputManager.GetKeyDown(KeyCode.Alpha9))
				{
				}
				if (c == '\b')
				{
					this.insert = Mathf.Max(0, this.insert - 1);
					if (this.insert < this.sb.Length)
					{
						this.sb.Remove(this.insert, 1);
					}
				}
				else if (c == '\r')
				{
					if (((IKeyFocusable)this.focusObj).EnterMode)
					{
						this.lineCount = 0;
						string text = this.sb.ToString();
						for (int j = 0; j < text.Length; j++)
						{
							char c2 = text[j];
							if (c2 == '\n')
							{
								this.lineCount++;
							}
						}
						this.lineCount++;
						if (((IKeyFocusable)this.focusObj).MaxLineCount > this.lineCount || ((IKeyFocusable)this.focusObj).MaxLineCount == 0)
						{
							this.sb.Insert(this.insert, '\n');
							this.insert++;
						}
					}
					else
					{
						((IKeyFocusable)this.focusObj).Commit();
					}
				}
				else if (this.controlText.Length < this.kbInfo.maxLength || this.kbInfo.maxLength == 0)
				{
					if (((IKeyFocusable)this.focusObj).NumberMode)
					{
						int num = 0;
						if (int.TryParse(c.ToString(), out num))
						{
							this.sb.Insert(this.insert, c);
						}
					}
					else
					{
						this.sb.Insert(this.insert, c);
					}
					this.insert++;
				}
			}
			if (((IKeyFocusable)this.focusObj).NumberMode)
			{
				if (this.sb.Length > 0)
				{
					long num2 = long.Parse(this.sb.ToString());
					if (((IKeyFocusable)this.focusObj).MaxValue < num2)
					{
						num2 = ((IKeyFocusable)this.focusObj).MaxValue;
					}
					else if (((IKeyFocusable)this.focusObj).MinValue > num2)
					{
						num2 = ((IKeyFocusable)this.focusObj).MinValue;
					}
					this.controlText = num2.ToString();
				}
				else
				{
					this.controlText = "0";
					this.sb.Insert(0, '0');
					this.insert++;
				}
			}
			else
			{
				this.controlText = this.sb.ToString();
			}
			if (this.focusObj != null)
			{
				((IKeyFocusable)this.focusObj).OriginalContent = this.controlText;
			}
		}
		else
		{
			this.controlText = ((IKeyFocusable)this.focusObj).OriginalContent;
			this.insert = Mathf.Clamp(this.insert, 0, this.controlText.Length);
			if (this.sb.Length > 0)
			{
				this.sb.Replace(this.sb.ToString(), this.controlText);
			}
			else
			{
				this.sb.Append(this.controlText);
			}
		}
		if (TsPlatform.IsWeb || TsPlatform.IsEditor)
		{
			if (0 < this.compositionText.Length && (this.kbInfo.maxLength >= this.controlText.Length || this.kbInfo.maxLength == 0))
			{
				this.oldCompStatus = this.compStatus;
				string value = this.compositionText;
				this.sb.Insert(this.insert, value);
				this.controlText = this.sb.ToString();
				this.insert = Mathf.Clamp(this.insert, 0, this.controlText.Length);
				this.bMaxLength = true;
			}
			else
			{
				this.bMaxLength = false;
			}
		}
		if (NkInputManager.inputString.Length > 0)
		{
			if (this.focusObj != null && NkInputManager.imeCompositionMode == IMECompositionMode.On)
			{
				this.controlText = ((IKeyFocusable)this.focusObj).SetInputText(this.controlText, ref this.insert);
			}
		}
		else if (this.focusObj != null && NkInputManager.imeCompositionMode == IMECompositionMode.On)
		{
			this.controlText = ((IKeyFocusable)this.focusObj).SetInputTextNoComposition(this.controlText, ref this.insert);
		}
	}

	public void Detarget(IUIObject obj)
	{
		this.Retarget(obj, null);
	}

	public void Retarget(IUIObject oldObj, IUIObject newObj)
	{
		if (this.uiCameras == null)
		{
			return;
		}
		for (int i = 0; i < this.numActivePointers; i++)
		{
			int j = 0;
			while (j < this.uiCameras.Length)
			{
				if (this.uiCameras[j].camera != null && this.uiCameras[j].camera.gameObject.activeInHierarchy && this.pointers[j, this.activePointers[i]].targetObj != null)
				{
					if (this.pointers[j, this.activePointers[i]].targetObj == oldObj)
					{
						this.pointers[j, this.activePointers[i]].targetObj = newObj;
						return;
					}
					break;
				}
				else
				{
					j++;
				}
			}
		}
		if (this.rayPtr.targetObj == oldObj)
		{
			this.rayPtr.targetObj = newObj;
		}
	}

	public bool GetPointer(IUIObject obj, out POINTER_INFO ptr)
	{
		if (this.uiCameras == null)
		{
			ptr = default(POINTER_INFO);
			return false;
		}
		for (int i = 0; i < this.numActivePointers; i++)
		{
			int j = 0;
			while (j < this.uiCameras.Length)
			{
				if (this.uiCameras[j].camera != null && this.uiCameras[j].camera.gameObject.activeInHierarchy && this.pointers[j, this.activePointers[i]].targetObj != null)
				{
					if (this.pointers[j, this.activePointers[i]].targetObj == obj)
					{
						ptr = this.pointers[j, this.activePointers[i]];
						return true;
					}
					break;
				}
				else
				{
					j++;
				}
			}
		}
		if (this.rayPtr.targetObj == obj)
		{
			ptr = this.rayPtr;
			return true;
		}
		ptr = default(POINTER_INFO);
		return false;
	}

	public void LockInput()
	{
	}

	public void UnlockInput()
	{
	}

	public void GetIOSIMEKeyboard(string str)
	{
		this.controlText = str;
		if (this.focusObj != null)
		{
			this.controlText = ((IKeyFocusable)this.focusObj).SetInputText(str, ref this.insert);
			((IKeyFocusable)this.focusObj).Commit();
		}
		this.FocusObject = null;
	}

	protected void LogNonUIObjErr(GameObject obj)
	{
	}

	private int GetNumber(char num)
	{
		int num2 = 0;
		char[] array = this.cNumCode;
		for (int i = 0; i < array.Length; i++)
		{
			char c = array[i];
			if (c == num)
			{
				return num2;
			}
			num2++;
		}
		return 8;
	}

	public void RequestDownload(PostProcPerItem callbackDelegate, object obj)
	{
	}
}
