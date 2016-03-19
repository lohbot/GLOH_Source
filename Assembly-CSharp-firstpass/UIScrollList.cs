using GameMessage;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Scroll List")]
public class UIScrollList : MonoBehaviour, IEZDragDrop, IUIObject
{
	public enum ORIENTATION
	{
		HORIZONTAL,
		VERTICAL,
		LINE
	}

	public enum DIRECTION
	{
		TtoB_LtoR,
		BtoT_RtoL
	}

	public enum ALIGNMENT
	{
		LEFT_TOP,
		CENTER,
		RIGHT_BOTTOM
	}

	public enum SLIDERPOSITION
	{
		RIGHT,
		RIGHTOUT,
		LEFT
	}

	protected delegate float ItemAlignmentDel(IUIListObject item);

	protected delegate bool SnapCoordProc(float val);

	protected const float reboundSpeed = 1f;

	protected const float overscrollAllowance = 0.5f;

	protected const float scrollDecelCoef = 0.1f;

	protected const float lowPassKernelWidthInSeconds = 0.045f;

	protected const float backgroundColliderOffset = 0f;

	private const float scrollStopThreshold = 0.0001f;

	private int layer;

	public bool touchScroll;

	public float scrollWheelFactor = 100f;

	public bool snap;

	public float minSnapDuration = 1f;

	public EZAnimation.EASING_TYPE snapEasing = EZAnimation.EASING_TYPE.ExponentialOut;

	public UISlider slider;

	public UIScrollList.SLIDERPOSITION sliderposition;

	public UIScrollList.ORIENTATION orientation;

	public UIScrollList.DIRECTION direction;

	public UIScrollList.ALIGNMENT alignment = UIScrollList.ALIGNMENT.CENTER;

	public Vector2 viewableArea;

	private Vector3 position = Vector3.zero;

	protected Rect3D clientClippingRect;

	public float itemSpacing;

	public bool spacingAtEnds = true;

	public float extraEndSpacing;

	public bool activateWhenAdding = true;

	public bool clipContents = true;

	public bool clipWhenMoving;

	public float dragThreshold = float.NaN;

	public GameObject[] sceneItems = new GameObject[0];

	public PrefabListItem[] prefabItems = new PrefabListItem[0];

	public MonoBehaviour scriptWithMethodToInvoke;

	public string methodToInvokeOnSelect;

	public SpriteManager manager;

	public bool detargetOnDisable;

	public EZAnimation.EASING_TYPE positionEasing = EZAnimation.EASING_TYPE.ExponentialOut;

	public float positionEaseDuration = 0.5f;

	public float positionEaseDelay;

	public bool blockInputWhileEasing = true;

	public int clearChunkSize = 5;

	protected bool doItemEasing;

	protected bool doPosEasing;

	protected List<EZAnimation> itemEasers = new List<EZAnimation>();

	protected EZAnimation scrollPosAnim;

	[HideInInspector]
	[NonSerialized]
	public bool repositionOnEnable = true;

	protected float contentExtents;

	protected IUIListObject selectedItem;

	protected IUIListObject mouseItem;

	public float scrollPos;

	protected GameObject mover;

	protected List<IUIListObject> items = new List<IUIListObject>();

	protected List<IUIListObject> visibleItems = new List<IUIListObject>();

	protected List<IUIListObject> tempVisItems = new List<IUIListObject>();

	private int maxMultiSelectNum = 10;

	private bool multiSelectMode;

	private bool autoScroll;

	protected Dictionary<int, IUIListObject> selectedItems = new Dictionary<int, IUIListObject>();

	protected bool m_controlIsEnabled = true;

	protected IUIContainer container;

	protected EZInputDelegate inputDelegate;

	protected EZValueChangedDelegate changeDelegate;

	protected EZValueChangedDelegate mouseOverDelegate;

	protected EZValueChangedDelegate mouseOutDelegate;

	protected EZValueChangedDelegate rightMouseDelegate;

	protected EZValueChangedDelegate doubleClickDelegate;

	protected EZValueChangedDelegate slidePosChangeDelegate;

	protected EZValueChangedDelegate longTapDelegate;

	protected Vector3 cachedPos;

	protected Quaternion cachedRot;

	protected Vector3 cachedScale;

	protected Vector2 cachedviewableArea;

	protected bool m_started;

	protected bool m_awake;

	public float lineHeight = 20f;

	public bool positionItemsImmediately = true;

	protected List<IUIListObject> newItems = new List<IUIListObject>();

	protected bool itemsInserted;

	public bool isScrolling;

	protected bool noTouch = true;

	protected float lowPassFilterFactor;

	private float scrollInertia;

	protected float scrollMax;

	private float scrollDelta;

	private float scrollStopThresholdLog = Mathf.Log10(0.0001f);

	private float lastTime;

	private float timeDelta;

	private float inertiaLerpTime;

	private float inertiaLerpInterval = 0.06f;

	private float amtOfPlay;

	private float autoScrollDuration;

	private float autoScrollStart;

	private float autoScrollPos;

	private float autoScrollDelta;

	private float autoScrollTime;

	private bool autoScrolling;

	public bool listMoved;

	private EZAnimation.Interpolator autoScrollInterpolator;

	protected bool bShow;

	public bool line;

	public int columnCount = 1;

	public float addy;

	private bool listDrag = true;

	public static string backButtonName = "00back";

	public static string selectImageName = "select";

	public static string lockImageName = "lock";

	private bool hideSlider;

	private bool useBoxCollider = true;

	protected bool bLabelScroll;

	public string m_strScrollBg = string.Empty;

	public string m_strScrollCon = string.Empty;

	public string m_strScrollArr = string.Empty;

	private bool m_bDragList;

	protected int maxLine;

	private bool m_bOverViewMode;

	protected bool m_bReserve = true;

	private bool useScrollLine = true;

	private int count;

	public bool changeScrollPos;

	public bool chatLabelScroll;

	private bool rightMouseSelect;

	public bool overList;

	public bool bCalcClipping;

	public object data;

	protected EZDragDropDelegate dragDropDelegate;

	public virtual int Layer
	{
		get
		{
			return this.layer;
		}
		set
		{
			this.layer = value;
		}
	}

	public virtual bool Visible
	{
		get
		{
			return base.gameObject.activeInHierarchy;
		}
		set
		{
			if (base.gameObject.activeInHierarchy != value)
			{
				base.gameObject.SetActive(value);
				if (this.slider)
				{
					if (this.changeScrollPos)
					{
						this.slider.Visible = value;
					}
					else
					{
						this.slider.Visible = false;
					}
				}
			}
		}
	}

	public int MaxMultiSelectNum
	{
		get
		{
			return this.maxMultiSelectNum;
		}
		set
		{
			this.maxMultiSelectNum = value;
		}
	}

	public bool AutoScroll
	{
		set
		{
			this.autoScroll = value;
		}
	}

	public Dictionary<int, IUIListObject> SelectedItems
	{
		get
		{
			return this.selectedItems;
		}
	}

	public bool UseBoxCollider
	{
		set
		{
			this.useBoxCollider = value;
		}
	}

	public bool HideSlider
	{
		set
		{
			if (this.slider)
			{
				this.slider.Visible = !value;
			}
			this.hideSlider = value;
		}
	}

	public bool ListDrag
	{
		set
		{
			this.listDrag = value;
		}
	}

	public EZValueChangedDelegate SelectionChange
	{
		get
		{
			return this.changeDelegate;
		}
		set
		{
			this.changeDelegate = value;
		}
	}

	public EZValueChangedDelegate MouseOver
	{
		get
		{
			return this.mouseOverDelegate;
		}
		set
		{
			this.mouseOverDelegate = value;
		}
	}

	public EZValueChangedDelegate MouseOut
	{
		get
		{
			return this.mouseOutDelegate;
		}
		set
		{
			this.mouseOutDelegate = value;
		}
	}

	public EZValueChangedDelegate SlidePosChange
	{
		get
		{
			return this.slidePosChangeDelegate;
		}
		set
		{
			this.slidePosChangeDelegate = value;
		}
	}

	public bool DRAGLIST
	{
		get
		{
			return this.m_bDragList;
		}
		set
		{
			this.m_bDragList = value;
		}
	}

	public bool OverViewMode
	{
		get
		{
			return this.m_bOverViewMode;
		}
		set
		{
			this.m_bOverViewMode = value;
		}
	}

	public bool Reserve
	{
		set
		{
			this.m_bReserve = value;
		}
	}

	public float ScrollPosition
	{
		get
		{
			return this.scrollPos;
		}
		set
		{
			this.ScrollListTo(value);
		}
	}

	public bool UseScrollLine
	{
		set
		{
			this.useScrollLine = value;
		}
	}

	public bool RightMouseSelect
	{
		set
		{
			this.rightMouseSelect = value;
		}
	}

	public float ContentExtents
	{
		get
		{
			return this.contentExtents;
		}
	}

	public UIListItemContainer SelectedItem
	{
		get
		{
			return (UIListItemContainer)this.selectedItem;
		}
		set
		{
			if (this.selectedItem != null)
			{
				((UIListItemContainer)this.selectedItem).SetSelected(false);
			}
			if (value == null)
			{
				this.selectedItem = null;
				return;
			}
			this.selectedItem = value;
			((UIListItemContainer)this.selectedItem).SetSelected(true);
		}
	}

	public IUIListObject MouseItem
	{
		get
		{
			return this.mouseItem;
		}
	}

	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	public bool controlIsEnabled
	{
		get
		{
			return this.m_controlIsEnabled;
		}
		set
		{
			this.m_controlIsEnabled = value;
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].controlIsEnabled = value;
			}
		}
	}

	public virtual bool DetargetOnDisable
	{
		get
		{
			return this.DetargetOnDisable;
		}
		set
		{
			this.DetargetOnDisable = value;
		}
	}

	public virtual IUIContainer Container
	{
		get
		{
			return this.container;
		}
		set
		{
			if (value != this.container)
			{
				if (this.container != null)
				{
					this.RemoveItemsFromContainer();
				}
				this.container = value;
				this.AddItemsToContainer();
			}
			else
			{
				this.container = value;
			}
		}
	}

	public object Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
		}
	}

	public bool IsDraggable
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public float DragOffset
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float MouseOffset
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public EZAnimation.EASING_TYPE CancelDragEasing
	{
		get
		{
			return EZAnimation.EASING_TYPE.Default;
		}
		set
		{
		}
	}

	public float CancelDragDuration
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public GameObject DropTarget
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool DropHandled
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool GetMultiSelectMode()
	{
		return this.multiSelectMode;
	}

	public void SetMultiSelectMode(bool value)
	{
		this.multiSelectMode = value;
		if (!value)
		{
			this.ClearSelectedItems();
		}
		else if (this.selectedItem != null)
		{
			((UIListItemContainer)this.selectedItem).SetSelected(false);
			this.selectedItem = null;
		}
	}

	public void RemoveSelectedItems(UIListItemContainer obj)
	{
		if (this.SelectedItems.ContainsKey(obj.GetIndex()))
		{
			obj.SetSelected(false);
			this.SelectedItems.Remove(obj.GetIndex());
		}
	}

	public void ClearSelectedItems()
	{
		if (this.selectedItem != null)
		{
			((UIListItemContainer)this.selectedItem).SetSelected(false);
			this.selectedItem = null;
		}
		using (Dictionary<int, IUIListObject>.ValueCollection.Enumerator enumerator = this.selectedItems.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIListItemContainer uIListItemContainer = (UIListItemContainer)enumerator.Current;
				uIListItemContainer.SetSelected(false);
			}
		}
		this.selectedItems.Clear();
	}

	public float MoverPosY()
	{
		return this.mover.transform.localPosition.y;
	}

	private void Awake()
	{
		if (this.m_awake)
		{
			return;
		}
		this.m_awake = true;
		this.mover = new GameObject();
		this.mover.gameObject.layer = GUICamera.UILayer;
		this.mover.name = "Mover";
		this.mover.transform.parent = base.transform;
		this.mover.transform.localPosition = Vector3.zero;
		this.mover.transform.localRotation = Quaternion.identity;
		this.mover.transform.localScale = Vector3.one;
		if (this.direction == UIScrollList.DIRECTION.BtoT_RtoL)
		{
			this.scrollPos = 1f;
		}
		this.autoScrollInterpolator = EZAnimation.GetInterpolator(this.snapEasing);
		this.lowPassFilterFactor = this.inertiaLerpInterval / 0.045f;
	}

	public void Start()
	{
		if (this.m_started)
		{
			return;
		}
		this.m_started = true;
		this.lastTime = Time.realtimeSinceStartup;
		this.cachedPos = base.transform.position;
		this.cachedRot = base.transform.rotation;
		this.cachedScale = base.transform.lossyScale;
		this.CalcClippingRect();
		if (this.slider != null)
		{
			this.slider.AddValueChangedDelegate(new EZValueChangedDelegate(this.SliderMoved));
			this.slider.SetList(this);
		}
		bool flag = false;
		if (base.collider == null && this.touchScroll && this.useBoxCollider)
		{
			flag = true;
		}
		if (flag)
		{
			BoxCollider boxCollider = (BoxCollider)base.gameObject.AddComponent(typeof(BoxCollider));
			boxCollider.size = new Vector3(this.viewableArea.x, this.viewableArea.y, 0f);
			boxCollider.center = Vector3.forward * 0f;
			boxCollider.isTrigger = true;
		}
		for (int i = 0; i < this.sceneItems.Length; i++)
		{
			if (this.sceneItems[i] != null)
			{
				this.AddItem(this.sceneItems[i]);
			}
		}
		for (int j = 0; j < this.prefabItems.Length; j++)
		{
			if (this.prefabItems[j] != null)
			{
				if (this.prefabItems[j].item == null)
				{
					if (this.prefabItems[0].item != null)
					{
						this.CreateItem(this.prefabItems[0].item, (!(this.prefabItems[j].itemText == string.Empty)) ? this.prefabItems[j].itemText : null);
					}
				}
				else
				{
					this.CreateItem(this.prefabItems[j].item, (!(this.prefabItems[j].itemText == string.Empty)) ? this.prefabItems[j].itemText : null);
				}
			}
		}
		if (float.IsNaN(this.dragThreshold))
		{
			this.dragThreshold = NrTSingleton<UIManager>.Instance.dragThreshold;
		}
	}

	public void RemoveBoxCollider()
	{
		BoxCollider boxCollider = (BoxCollider)base.gameObject.GetComponent(typeof(BoxCollider));
		if (null != boxCollider)
		{
			boxCollider.size = new Vector2(0f, 0f);
		}
	}

	public void MakeBoxCollider()
	{
		BoxCollider boxCollider = (BoxCollider)base.gameObject.AddComponent(typeof(BoxCollider));
		boxCollider.size = new Vector3(this.viewableArea.x, this.viewableArea.y, 0.001f);
		boxCollider.center = Vector3.forward * 0f;
		boxCollider.isTrigger = true;
	}

	protected void CalcClippingRect()
	{
		this.clientClippingRect.FromPoints(new Vector3(-this.viewableArea.x * 0.5f, this.viewableArea.y * 0.5f), new Vector3(this.viewableArea.x * 0.5f, this.viewableArea.y * 0.5f), new Vector3(-this.viewableArea.x * 0.5f, -this.viewableArea.y * 0.5f));
		this.clientClippingRect.MultFast(base.transform.localToWorldMatrix);
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].TextObj != null)
			{
				this.items[i].TextObj.SetClippingRect(this.clientClippingRect);
			}
		}
	}

	public void SliderMoved(IUIObject slider)
	{
		if (this.scrollPos != ((UISlider)slider).Value)
		{
			this.ScrollListTo_Internal(((UISlider)slider).Value);
		}
	}

	public void SliderInputDel(ref POINTER_INFO ptr)
	{
	}

	protected void ScrollListTo_Internal(float pos)
	{
		if (float.IsNaN(pos))
		{
			return;
		}
		if (this.orientation == UIScrollList.ORIENTATION.VERTICAL)
		{
			float d = (this.direction != UIScrollList.DIRECTION.TtoB_LtoR) ? -1f : 1f;
			this.mover.transform.localPosition = Vector3.up * d * Mathf.Clamp(this.amtOfPlay, 0f, this.amtOfPlay) * pos;
		}
		else
		{
			float d2 = (this.direction != UIScrollList.DIRECTION.TtoB_LtoR) ? 1f : -1f;
			this.mover.transform.localPosition = Vector3.right * d2 * Mathf.Clamp(this.amtOfPlay, 0f, this.amtOfPlay) * pos;
		}
		this.scrollPos = pos;
		this.ClipItems();
		if (this.slider != null)
		{
			this.slider.Value = this.scrollPos;
		}
		if (this.slidePosChangeDelegate != null)
		{
			this.slidePosChangeDelegate(this);
		}
	}

	public void ScrollListTo_Internal_DonotClipItems(float pos)
	{
		if (float.IsNaN(pos))
		{
			return;
		}
		if (this.orientation == UIScrollList.ORIENTATION.VERTICAL)
		{
			float d = (this.direction != UIScrollList.DIRECTION.TtoB_LtoR) ? -1f : 1f;
			this.mover.transform.localPosition = Vector3.up * d * Mathf.Clamp(this.amtOfPlay, 0f, this.amtOfPlay) * pos;
		}
		else
		{
			float d2 = (this.direction != UIScrollList.DIRECTION.TtoB_LtoR) ? 1f : -1f;
			this.mover.transform.localPosition = Vector3.right * d2 * Mathf.Clamp(this.amtOfPlay, 0f, this.amtOfPlay) * pos;
		}
		this.scrollPos = pos;
		if (this.slider != null)
		{
			this.slider.Value = this.scrollPos;
		}
	}

	public void ScrollListTo(float pos)
	{
		this.scrollInertia = 0f;
		this.scrollDelta = 0f;
		this.isScrolling = false;
		this.autoScrolling = false;
		this.ScrollListTo_Internal(pos);
	}

	public void ScrollToItem(IUIListObject item, float scrollTime, EZAnimation.EASING_TYPE easing)
	{
		if (this.newItems.Count != 0)
		{
			if (this.itemsInserted || this.doItemEasing)
			{
				this.RepositionItems();
			}
			else
			{
				this.PositionNewItems();
			}
			this.itemsInserted = false;
			this.newItems.Clear();
		}
		if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
			{
				this.autoScrollPos = Mathf.Clamp01(item.transform.localPosition.x / this.amtOfPlay);
			}
			else
			{
				this.autoScrollPos = Mathf.Clamp01(-item.transform.localPosition.x / this.amtOfPlay);
			}
		}
		else if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
		{
			this.autoScrollPos = Mathf.Clamp01((-item.transform.localPosition.y + this.lineHeight / 2f + this.itemSpacing / 2f) / this.amtOfPlay);
		}
		else
		{
			this.autoScrollPos = Mathf.Clamp01((item.transform.localPosition.y - this.lineHeight / 2f - this.itemSpacing / 2f) / this.amtOfPlay);
		}
		this.autoScrollInterpolator = EZAnimation.GetInterpolator(easing);
		this.autoScrollStart = this.scrollPos;
		this.autoScrollDelta = this.autoScrollPos - this.scrollPos;
		this.autoScrollDuration = scrollTime;
		this.autoScrollTime = 0f;
		this.autoScrolling = true;
		this.scrollDelta = 0f;
		this.isScrolling = false;
		if (0f < this.autoScrollPos)
		{
			this.ReserveMakeItem();
		}
	}

	public void ScrollToItem(int index, float scrollTime, EZAnimation.EASING_TYPE easing)
	{
		if (index < 0 || index >= this.items.Count)
		{
			return;
		}
		this.ScrollToItem(this.items[index], scrollTime, easing);
	}

	public void ScrollToItem(IUIListObject item, float scrollTime)
	{
		this.ScrollToItem(item, scrollTime, this.snapEasing);
	}

	public void ScrollToItem(int index, float scrollTime)
	{
		this.ScrollToItem(index, scrollTime, this.snapEasing);
	}

	public void SetViewableAreaPixelDimensions(Camera cam, int width, int height)
	{
		Plane plane = new Plane(cam.transform.forward, cam.transform.position);
		float distanceToPoint = plane.GetDistanceToPoint(base.transform.position);
		float num = Vector3.Distance(cam.ScreenToWorldPoint(new Vector3(0f, 1f, distanceToPoint)), cam.ScreenToWorldPoint(new Vector3(0f, 0f, distanceToPoint)));
		this.viewableArea = new Vector2((float)width * num, (float)height * num);
		this.CalcClippingRect();
		this.RepositionItems();
	}

	public void ResizeViewableArea(float width, float height)
	{
		float x = this.GetLocation().x - this.GetSize().x / 2f;
		float y = -(this.GetLocation().y + this.GetSize().y / 2f);
		this.viewableArea = new Vector2(width, height);
		this.SetVectorLocation(x, y, base.transform.localPosition.z);
		BoxCollider boxCollider = (BoxCollider)base.gameObject.GetComponent(typeof(BoxCollider));
		if (boxCollider != null)
		{
			boxCollider.size = new Vector3(this.viewableArea.x, this.viewableArea.y, 0f);
		}
		this.CalcClippingRect();
		this.RepositionItems();
	}

	public void InsertItem(IUIListObject item, int position)
	{
		this.InsertItem(item, position, null, false);
	}

	public void InsertItem(IUIListObject item, int position, bool doEasing)
	{
		this.InsertItem(item, position, null, doEasing);
	}

	public void InsertItem(IUIListObject item, int position, string text, bool doEasing)
	{
		if (position >= this.items.Count)
		{
			this.doItemEasing = false;
		}
		else
		{
			this.doItemEasing = doEasing;
		}
		this.doPosEasing = doEasing;
		if (!this.m_awake)
		{
			this.Awake();
		}
		if (!this.m_started)
		{
			this.Start();
		}
		if (this.activateWhenAdding && !((Component)item).gameObject.activeInHierarchy)
		{
			((Component)item).gameObject.SetActive(true);
		}
		if (!base.gameObject.activeInHierarchy)
		{
			((Component)item).gameObject.SetActive(false);
		}
		item.gameObject.layer = base.gameObject.layer;
		if (this.container != null)
		{
			this.container.AddChild(item.gameObject);
		}
		item.transform.parent = this.mover.transform;
		item.transform.localRotation = Quaternion.identity;
		item.transform.localScale = Vector3.one;
		item.transform.localPosition = Vector3.zero;
		item.SetList(this);
		item.FindOuterEdges();
		if (this.bLabelScroll && Mathf.Abs(item.BottomRightEdge().y) + 5f > this.viewableArea.y && this.slider)
		{
			this.slider.Visible = true;
		}
		item.UpdateCollider();
		position = Mathf.Clamp(position, 0, this.items.Count);
		if (this.clipContents)
		{
			item.Hide(true);
			if (!item.Managed)
			{
				item.gameObject.SetActive(false);
			}
		}
		if (position == this.items.Count)
		{
			item.SetIndex(this.items.Count);
			this.items.Add(item);
			this.DonotCountRepositionItems();
		}
		else
		{
			this.items.Insert(position, item);
			this.PositionItems();
		}
		this.CalcSliderCount();
	}

	public void PositionInsertItem(IUIListObject item, int position, string text, bool doEasing)
	{
		if (position >= this.items.Count)
		{
			this.doItemEasing = false;
		}
		else
		{
			this.doItemEasing = doEasing;
		}
		this.doPosEasing = doEasing;
		if (!this.m_awake)
		{
			this.Awake();
		}
		if (!this.m_started)
		{
			this.Start();
		}
		if (this.activateWhenAdding && !((Component)item).gameObject.activeInHierarchy)
		{
			((Component)item).gameObject.SetActive(true);
		}
		if (!base.gameObject.activeInHierarchy)
		{
			((Component)item).gameObject.SetActive(false);
		}
		item.gameObject.layer = base.gameObject.layer;
		if (this.container != null)
		{
			this.container.AddChild(item.gameObject);
		}
		item.transform.parent = this.mover.transform;
		item.transform.localRotation = Quaternion.identity;
		item.transform.localScale = Vector3.one;
		item.transform.localPosition = Vector3.zero;
		item.SetList(this);
		position = Mathf.Clamp(position, 0, this.items.Count);
		if (this.clipContents)
		{
			item.Hide(true);
			if (!item.Managed)
			{
				item.gameObject.SetActive(false);
			}
		}
		item.SetIndex(position);
		this.newItems.Add(item);
		if (position != this.items.Count)
		{
			this.itemsInserted = true;
			this.items.Insert(position, item);
			if (this.visibleItems.Count == 0)
			{
				this.visibleItems.Add(item);
			}
			else if (item.GetIndex() > 0)
			{
				int num = this.visibleItems.IndexOf(this.items[item.GetIndex() - 1]);
				if (num == -1)
				{
					if (this.visibleItems[0].GetIndex() >= item.GetIndex())
					{
						this.visibleItems.Insert(0, item);
					}
					else
					{
						this.visibleItems.Add(item);
					}
				}
				else
				{
					this.visibleItems.Insert(num + 1, item);
				}
			}
		}
		else
		{
			this.items.Add(item);
			this.visibleItems.Add(item);
		}
		if (this.positionItemsImmediately)
		{
			if (this.itemsInserted || this.doItemEasing)
			{
				this.RepositionItems();
				this.itemsInserted = false;
				this.newItems.Clear();
			}
			else
			{
				this.PositionNewItems();
			}
		}
	}

	protected void PositionNewItems()
	{
		IUIListObject iUIListObject = null;
		float num = 0f;
		for (int i = 0; i < this.newItems.Count; i++)
		{
			if (this.newItems[i] != null)
			{
				int index = this.newItems[i].GetIndex();
				IUIListObject iUIListObject2 = this.items[index];
				iUIListObject2.FindOuterEdges();
				iUIListObject2.UpdateCollider();
				float x = 0f;
				float y = 0f;
				bool flag = false;
				if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
				{
					if (index > 0)
					{
						flag = true;
						iUIListObject = this.items[index - 1];
						if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
						{
							x = iUIListObject.transform.localPosition.x + iUIListObject.BottomRightEdge().x + this.itemSpacing - iUIListObject2.TopLeftEdge().x;
						}
						else
						{
							x = iUIListObject.transform.localPosition.x - iUIListObject.BottomRightEdge().x - this.itemSpacing + iUIListObject2.TopLeftEdge().x;
						}
					}
					else
					{
						if (this.spacingAtEnds)
						{
							flag = true;
						}
						if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
						{
							x = this.viewableArea.x * -0.5f - iUIListObject2.TopLeftEdge().x + ((!this.spacingAtEnds) ? 0f : this.itemSpacing) + this.extraEndSpacing;
						}
						else
						{
							x = this.viewableArea.x * 0.5f - iUIListObject2.BottomRightEdge().x - ((!this.spacingAtEnds) ? 0f : this.itemSpacing) - this.extraEndSpacing;
						}
					}
					switch (this.alignment)
					{
					case UIScrollList.ALIGNMENT.LEFT_TOP:
						y = this.viewableArea.y * 0.5f - iUIListObject2.TopLeftEdge().y;
						break;
					case UIScrollList.ALIGNMENT.CENTER:
						y = 0f;
						break;
					case UIScrollList.ALIGNMENT.RIGHT_BOTTOM:
						y = this.viewableArea.y * -0.5f - iUIListObject2.BottomRightEdge().y;
						break;
					}
					num += iUIListObject2.BottomRightEdge().x - iUIListObject2.TopLeftEdge().x + ((!flag || iUIListObject == null) ? 0f : this.itemSpacing);
				}
				else
				{
					if (index > 0)
					{
						flag = true;
						iUIListObject = this.items[index - 1];
						if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
						{
							y = iUIListObject.transform.localPosition.y + iUIListObject.BottomRightEdge().y - this.itemSpacing - iUIListObject2.TopLeftEdge().y;
						}
						else
						{
							y = iUIListObject.transform.localPosition.y - iUIListObject.BottomRightEdge().y + this.itemSpacing + iUIListObject2.TopLeftEdge().y;
						}
					}
					else
					{
						if (this.spacingAtEnds)
						{
							flag = true;
						}
						if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
						{
							y = this.viewableArea.y * 0.5f - iUIListObject2.TopLeftEdge().y - ((!this.spacingAtEnds) ? 0f : this.itemSpacing) - this.extraEndSpacing;
						}
						else
						{
							y = this.viewableArea.y * -0.5f - iUIListObject2.BottomRightEdge().y + ((!this.spacingAtEnds) ? 0f : this.itemSpacing) + this.extraEndSpacing;
						}
					}
					switch (this.alignment)
					{
					case UIScrollList.ALIGNMENT.LEFT_TOP:
						x = this.viewableArea.x * -0.5f - iUIListObject2.TopLeftEdge().x;
						break;
					case UIScrollList.ALIGNMENT.CENTER:
						x = 0f;
						break;
					case UIScrollList.ALIGNMENT.RIGHT_BOTTOM:
						x = this.viewableArea.x * 0.5f - iUIListObject2.BottomRightEdge().x;
						break;
					}
					num += iUIListObject2.TopLeftEdge().y - iUIListObject2.BottomRightEdge().y + ((!flag || iUIListObject == null) ? 0f : this.itemSpacing);
				}
				iUIListObject2.transform.localPosition = new Vector3(x, y, 0f);
			}
		}
		this.UpdateContentExtents(num);
		this.ClipItems();
		this.newItems.Clear();
	}

	public void InsertItemDonotPosionUpdate(IUIListObject item, int position, string text, bool indexPosition = true)
	{
		if (!this.m_awake)
		{
			this.Awake();
		}
		if (!this.m_started)
		{
			this.Start();
		}
		if (this.activateWhenAdding && !((Component)item).gameObject.activeInHierarchy)
		{
			((Component)item).gameObject.SetActive(true);
		}
		if (!base.gameObject.activeInHierarchy)
		{
			((Component)item).gameObject.SetActive(false);
		}
		item.gameObject.layer = base.gameObject.layer;
		if (this.container != null)
		{
			this.container.AddChild(item.gameObject);
		}
		item.transform.parent = this.mover.transform;
		item.transform.localRotation = Quaternion.identity;
		item.transform.localScale = Vector3.one;
		item.transform.localPosition = Vector3.zero;
		item.SetList(this);
		item.FindOuterEdges();
		if (this.bLabelScroll && Mathf.Abs(item.BottomRightEdge().y) > this.viewableArea.y && this.slider)
		{
			this.slider.Visible = true;
		}
		item.UpdateCollider();
		position = Mathf.Clamp(position, 0, this.items.Count);
		if (this.clipContents)
		{
			item.Hide(true);
			if (!item.Managed)
			{
				item.gameObject.SetActive(false);
			}
		}
		item.SetIndex(position);
		this.items.Insert(position, item);
		if (indexPosition)
		{
			if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
			{
				this.PositionHorizontally(true, position);
			}
			else if (!this.line)
			{
				this.PositionVertically(true, position);
			}
		}
		this.CalcSliderCount();
	}

	public void CalcSliderCount()
	{
		if (null != this.slider && this.useScrollLine)
		{
			if (this.line)
			{
				this.slider.lineHeight = (float)(this.items.Count / this.columnCount - (int)(this.viewableArea.y / this.lineHeight));
			}
			else
			{
				this.slider.lineHeight = (float)(this.items.Count - (int)(this.viewableArea.y / this.lineHeight));
			}
			if (0f >= this.slider.lineHeight)
			{
				this.slider.lineHeight = 1f;
			}
		}
		else if (null != this.slider)
		{
			this.slider.lineHeight = this.lineHeight;
		}
	}

	public void SetItem(IUIListObject item, int position)
	{
		if (!this.m_awake)
		{
			this.Awake();
		}
		if (!this.m_started)
		{
			this.Start();
		}
		if (this.activateWhenAdding && !((Component)item).gameObject.activeInHierarchy)
		{
			((Component)item).gameObject.SetActive(true);
		}
		if (!base.gameObject.activeInHierarchy)
		{
			((Component)item).gameObject.SetActive(false);
		}
		item.gameObject.layer = base.gameObject.layer;
		item.transform.parent = this.mover.transform;
		item.transform.localRotation = Quaternion.identity;
		item.transform.localScale = Vector3.one;
		item.transform.localPosition = Vector3.zero;
		item.SetList(this);
		item.FindOuterEdges();
		item.UpdateCollider();
		if (this.clipContents)
		{
			item.Hide(true);
			if (!item.Managed)
			{
				item.gameObject.SetActive(false);
			}
		}
		if (this.GetItem(position) == null)
		{
			if (this.container != null)
			{
				this.container.AddChild(item.gameObject);
			}
			this.items.Insert(position, item);
		}
		else
		{
			this.items[position] = item;
		}
		this.PositionItems();
	}

	public void AddItem(GameObject itemGO)
	{
		IUIListObject iUIListObject = (IUIListObject)itemGO.GetComponent(typeof(IUIListObject));
		if (iUIListObject == null)
		{
			TsLog.LogWarning(string.Concat(new string[]
			{
				"GameObject \"",
				itemGO.name,
				"\" does not contain any list item component suitable to be added to scroll list \"",
				base.name,
				"\"."
			}), new object[0]);
			return;
		}
		this.AddItem(iUIListObject, null);
	}

	public void AddItem(IUIListObject item)
	{
		this.AddItem(item, null);
	}

	public void AddItem(IUIListObject item, string text)
	{
		if (!this.m_awake)
		{
			this.Awake();
		}
		if (!this.m_started)
		{
			this.Start();
		}
		this.InsertItem(item, this.items.Count, text, false);
	}

	public IUIListObject CreateItem(GameObject prefab)
	{
		if (!this.m_awake)
		{
			this.Awake();
		}
		if (!this.m_started)
		{
			this.Start();
		}
		return this.CreateItem(prefab, this.items.Count, null);
	}

	public IUIListObject CreateItem(GameObject prefab, string text)
	{
		if (!this.m_awake)
		{
			this.Awake();
		}
		if (!this.m_started)
		{
			this.Start();
		}
		return this.CreateItem(prefab, this.items.Count, text);
	}

	public IUIListObject CreateItem(GameObject prefab, int position)
	{
		return this.CreateItem(prefab, position, null);
	}

	public IUIListObject CreateItem(GameObject prefab, int position, string text)
	{
		IUIListObject iUIListObject = (IUIListObject)prefab.GetComponent(typeof(IUIListObject));
		if (iUIListObject == null)
		{
			return null;
		}
		GameObject gameObject;
		if (this.manager != null)
		{
			if (iUIListObject.IsContainer())
			{
				gameObject = (GameObject)UnityEngine.Object.Instantiate(prefab);
				Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(SpriteRoot));
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.manager.AddSprite((SpriteRoot)componentsInChildren[i]);
				}
			}
			else
			{
				SpriteRoot spriteRoot = this.manager.CreateSprite(prefab);
				if (spriteRoot == null)
				{
					return null;
				}
				gameObject = spriteRoot.gameObject;
			}
		}
		else
		{
			gameObject = (GameObject)UnityEngine.Object.Instantiate(prefab);
		}
		iUIListObject = (IUIListObject)gameObject.GetComponent(typeof(IUIListObject));
		if (iUIListObject == null)
		{
			return null;
		}
		this.InsertItem(iUIListObject, position, text, false);
		return iUIListObject;
	}

	protected void UpdateContentExtents(float change)
	{
		float num = this.amtOfPlay;
		float num2 = ((!this.spacingAtEnds) ? 0f : (this.itemSpacing * 0.5f)) + this.extraEndSpacing * 2f;
		this.contentExtents += change;
		if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			this.amtOfPlay = this.contentExtents + num2 - this.viewableArea.x;
			this.scrollMax = this.viewableArea.x / (this.contentExtents + num2 - this.viewableArea.x) * 0.5f;
		}
		else
		{
			this.amtOfPlay = this.contentExtents + num2 - this.viewableArea.y;
			this.scrollMax = this.viewableArea.y / (this.contentExtents + num2 - this.viewableArea.y) * 0.5f;
		}
		float num3 = num * this.scrollPos / this.amtOfPlay;
		if (this.doPosEasing && num3 > 1f)
		{
			this.scrollPosAnim = AnimatePosition.Do(base.gameObject, EZAnimation.ANIM_MODE.By, Vector3.zero, new EZAnimation.Interpolator(this.ScrollPosInterpolator), this.positionEaseDuration, this.positionEaseDelay, null, new EZAnimation.CompletionDelegate(this.OnPosEasingDone));
			this.scrollPosAnim.Data = new Vector2(num3, 1f - num3);
			this.itemEasers.Add(this.scrollPosAnim);
		}
		else
		{
			this.ScrollListTo_Internal(Mathf.Clamp01(num3));
		}
		this.doPosEasing = false;
	}

	protected float ScrollPosInterpolator(float time, float start, float delta, float duration)
	{
		Vector2 vector = (Vector2)this.scrollPosAnim.Data;
		this.ScrollListTo_Internal(EZAnimation.GetInterpolator(this.positionEasing)(time, vector.x, vector.y, duration));
		if (time >= duration)
		{
			this.scrollPosAnim = null;
		}
		return start;
	}

	protected void OnPosEasingDone(EZAnimation anim)
	{
		if (anim == null)
		{
			return;
		}
		this.itemEasers.Remove(anim);
	}

	protected float GetYCentered(IUIListObject item)
	{
		return 0f;
	}

	protected float GetYAlignTop(IUIListObject item)
	{
		return this.viewableArea.y * 0.5f - item.TopLeftEdge().y;
	}

	protected float GetYAlignBottom(IUIListObject item)
	{
		return this.viewableArea.y * -0.5f - item.BottomRightEdge().y;
	}

	protected float GetXCentered(IUIListObject item)
	{
		return 0f;
	}

	protected float GetXAlignLeft(IUIListObject item)
	{
		return this.viewableArea.x * -0.5f - item.TopLeftEdge().x;
	}

	protected float GetXAlignRight(IUIListObject item)
	{
		return this.viewableArea.x * 0.5f - item.BottomRightEdge().x;
	}

	public void PositionItems()
	{
		if (this.itemEasers.Count > 0)
		{
			for (int i = 0; i < this.itemEasers.Count; i++)
			{
				this.itemEasers[i].CompletedDelegate = null;
				this.itemEasers[i].End();
			}
			this.itemEasers.Clear();
		}
		if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			this.PositionHorizontally(false);
		}
		else
		{
			this.PositionVertically(false);
		}
		this.UpdateContentExtents(0f);
		this.ClipItems();
	}

	public void RepositionItems()
	{
		if (0 >= this.items.Count)
		{
			return;
		}
		if (this.itemEasers.Count > 0)
		{
			for (int i = 0; i < this.itemEasers.Count; i++)
			{
				this.itemEasers[i].CompletedDelegate = null;
				this.itemEasers[i].End();
			}
			this.itemEasers.Clear();
		}
		if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			this.PositionHorizontally(true);
		}
		else
		{
			this.PositionVertically(true);
		}
		this.UpdateContentExtents(0f);
		this.ClipItems();
	}

	public void DonotCountRepositionItems()
	{
		if (0 >= this.items.Count)
		{
			return;
		}
		if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			this.PositionHorizontally(true);
		}
		else
		{
			this.PositionVertically(true);
		}
		this.UpdateContentExtents(0f);
		this.ClipItems();
	}

	public void RepositionItemsDonotClipItems(float scrollPos)
	{
		if (0 >= this.items.Count)
		{
			return;
		}
		if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			this.PositionHorizontally(true);
		}
		else
		{
			this.PositionVertically(true);
		}
		float num = ((!this.spacingAtEnds) ? 0f : (this.itemSpacing * 2f)) + this.extraEndSpacing * 2f;
		if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			this.amtOfPlay = this.contentExtents + num - this.viewableArea.x;
			this.scrollMax = this.viewableArea.x / (this.contentExtents + num - this.viewableArea.x) * 0.5f;
		}
		else
		{
			this.amtOfPlay = this.contentExtents + num - this.viewableArea.y;
			this.scrollMax = this.viewableArea.y / (this.contentExtents + num - this.viewableArea.y) * 0.5f;
		}
		this.ScrollListTo(scrollPos);
	}

	protected void PositionHorizontally(bool updateExtents, int index)
	{
		this.contentExtents = 0f;
		UIScrollList.ItemAlignmentDel itemAlignmentDel;
		switch (this.alignment)
		{
		case UIScrollList.ALIGNMENT.LEFT_TOP:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetYAlignTop);
			break;
		case UIScrollList.ALIGNMENT.CENTER:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetYCentered);
			break;
		case UIScrollList.ALIGNMENT.RIGHT_BOTTOM:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetYAlignBottom);
			break;
		default:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetYCentered);
			break;
		}
		if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
		{
			float num = this.viewableArea.x * -0.5f;
			if (updateExtents)
			{
				this.items[index].FindOuterEdges();
			}
			this.items[index].transform.localPosition = new Vector3(num - this.items[index].TopLeftEdge().x, itemAlignmentDel(this.items[index]), -0.1f);
			float num2 = this.items[index].BottomRightEdge().x - this.items[index].TopLeftEdge().x + this.itemSpacing;
			this.contentExtents += num2;
			num += num2;
			this.items[index].SetIndex(index);
			if (!this.spacingAtEnds)
			{
				this.contentExtents -= this.itemSpacing;
			}
		}
		else
		{
			float num = this.viewableArea.x * 0.5f;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (updateExtents)
				{
					this.items[i].FindOuterEdges();
				}
				this.items[index].transform.localPosition = new Vector3(num - this.items[index].BottomRightEdge().x, itemAlignmentDel(this.items[index]), -0.1f);
				float num2 = this.items[index].BottomRightEdge().x - this.items[index].TopLeftEdge().x + this.itemSpacing;
				this.contentExtents += num2;
				num -= num2;
				this.items[index].SetIndex(index);
			}
			if (!this.spacingAtEnds)
			{
				this.contentExtents -= this.itemSpacing;
			}
		}
	}

	protected void PositionHorizontally(bool updateExtents)
	{
		this.contentExtents = 0f;
		UIScrollList.ItemAlignmentDel itemAlignmentDel;
		switch (this.alignment)
		{
		case UIScrollList.ALIGNMENT.LEFT_TOP:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetYAlignTop);
			break;
		case UIScrollList.ALIGNMENT.CENTER:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetYCentered);
			break;
		case UIScrollList.ALIGNMENT.RIGHT_BOTTOM:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetYAlignBottom);
			break;
		default:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetYCentered);
			break;
		}
		if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
		{
			float num = this.viewableArea.x * -0.5f;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (updateExtents)
				{
					this.items[i].FindOuterEdges();
				}
				Vector3 vector = new Vector3(num - this.items[i].TopLeftEdge().x, itemAlignmentDel(this.items[i]), -0.1f);
				if (this.doItemEasing)
				{
					if (this.newItems.Contains(this.items[i]))
					{
						this.items[i].transform.localPosition = vector;
					}
					else
					{
						this.itemEasers.Add(AnimatePosition.Do(this.items[i].gameObject, EZAnimation.ANIM_MODE.To, vector, EZAnimation.GetInterpolator(this.positionEasing), this.positionEaseDuration, this.positionEaseDelay, null, new EZAnimation.CompletionDelegate(this.OnPosEasingDone)));
					}
				}
				else
				{
					this.items[i].transform.localPosition = vector;
				}
				float num2 = this.items[i].BottomRightEdge().x - this.items[i].TopLeftEdge().x + this.itemSpacing;
				this.contentExtents += num2;
				num += num2;
				this.items[i].SetIndex(i);
			}
			if (!this.spacingAtEnds)
			{
				this.contentExtents -= this.itemSpacing;
			}
		}
		else
		{
			float num = this.viewableArea.x * 0.5f;
			for (int j = 0; j < this.items.Count; j++)
			{
				if (updateExtents)
				{
					this.items[j].FindOuterEdges();
				}
				Vector3 vector = new Vector3(num - this.items[j].BottomRightEdge().x, itemAlignmentDel(this.items[j]), -0.1f);
				if (this.doItemEasing)
				{
					if (this.newItems.Contains(this.items[j]))
					{
						this.items[j].transform.localPosition = vector;
					}
					else
					{
						this.itemEasers.Add(AnimatePosition.Do(this.items[j].gameObject, EZAnimation.ANIM_MODE.To, vector, EZAnimation.GetInterpolator(this.positionEasing), this.positionEaseDuration, this.positionEaseDelay, null, new EZAnimation.CompletionDelegate(this.OnPosEasingDone)));
					}
				}
				else
				{
					this.items[j].transform.localPosition = vector;
				}
				float num2 = this.items[j].BottomRightEdge().x - this.items[j].TopLeftEdge().x + this.itemSpacing;
				this.contentExtents += num2;
				num -= num2;
				this.items[j].SetIndex(j);
			}
			if (!this.spacingAtEnds)
			{
				this.contentExtents -= this.itemSpacing;
			}
		}
	}

	protected void PositionVertically(bool updateExtents, int index)
	{
		float num = 0f;
		this.contentExtents = 0f;
		UIScrollList.ItemAlignmentDel itemAlignmentDel;
		switch (this.alignment)
		{
		case UIScrollList.ALIGNMENT.LEFT_TOP:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetXAlignLeft);
			break;
		case UIScrollList.ALIGNMENT.CENTER:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetXCentered);
			break;
		case UIScrollList.ALIGNMENT.RIGHT_BOTTOM:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetXAlignRight);
			break;
		default:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetXCentered);
			break;
		}
		if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
		{
			float num2 = this.viewableArea.y * 0.5f;
			for (int i = 0; i < this.items.Count - 1; i++)
			{
				num = this.items[i].TopLeftEdge().y - this.items[i].BottomRightEdge().y + this.itemSpacing;
				num2 -= num;
			}
			this.contentExtents += num;
			this.items[index].transform.localPosition = new Vector3(itemAlignmentDel(this.items[index]), num2 - this.items[index].TopLeftEdge().y, -0.1f);
			this.items[index].SetIndex(index);
			if (!this.spacingAtEnds)
			{
				this.contentExtents -= this.itemSpacing;
			}
		}
		else
		{
			float num2 = this.viewableArea.y * -0.5f;
			for (int j = 0; j < this.items.Count - 1; j++)
			{
				num = this.items[j].TopLeftEdge().y - this.items[j].BottomRightEdge().y + this.itemSpacing;
				num2 += num;
			}
			this.contentExtents += num;
			this.items[index].transform.localPosition = new Vector3(itemAlignmentDel(this.items[index]), num2 - this.items[index].BottomRightEdge().y, -0.1f);
			this.items[index].SetIndex(index);
			if (!this.spacingAtEnds)
			{
				this.contentExtents -= this.itemSpacing;
			}
		}
	}

	protected void PositionVertically(bool updateExtents)
	{
		this.contentExtents = 0f;
		UIScrollList.ItemAlignmentDel itemAlignmentDel;
		switch (this.alignment)
		{
		case UIScrollList.ALIGNMENT.LEFT_TOP:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetXAlignLeft);
			break;
		case UIScrollList.ALIGNMENT.CENTER:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetXCentered);
			break;
		case UIScrollList.ALIGNMENT.RIGHT_BOTTOM:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetXAlignRight);
			break;
		default:
			itemAlignmentDel = new UIScrollList.ItemAlignmentDel(this.GetXCentered);
			break;
		}
		if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
		{
			if (this.line)
			{
				float num = this.viewableArea.y * 0.5f;
				float num2 = this.viewableArea.x * -0.5f;
				for (int i = 0; i < this.items.Count; i++)
				{
					if (i % this.columnCount == 0)
					{
						num2 = this.viewableArea.x * -0.5f;
					}
					if (i % this.columnCount == 0 && i != 0)
					{
						float num3 = this.items[i].TopLeftEdge().y - this.items[i].BottomRightEdge().y + this.itemSpacing;
						num -= num3;
					}
					if ((i + 1) % this.columnCount == 0)
					{
						this.contentExtents += this.items[i].TopLeftEdge().y - this.items[i].BottomRightEdge().y + this.itemSpacing;
					}
					this.items[i].transform.localPosition = new Vector3(num2 - this.items[i].TopLeftEdge().x, num - this.items[i].TopLeftEdge().y, -0.1f);
					float num4 = this.items[i].BottomRightEdge().x - this.items[i].TopLeftEdge().x + this.itemSpacing;
					num2 += num4;
					this.items[i].SetIndex(i);
				}
			}
			else
			{
				float num = this.viewableArea.y * 0.5f;
				for (int j = 0; j < this.items.Count; j++)
				{
					if (updateExtents)
					{
						this.items[j].FindOuterEdges();
					}
					Vector3 vector = new Vector3(itemAlignmentDel(this.items[j]), num - this.items[j].TopLeftEdge().y, -0.1f);
					if (this.doItemEasing)
					{
						if (this.newItems.Contains(this.items[j]))
						{
							this.items[j].transform.localPosition = vector;
						}
						else
						{
							this.itemEasers.Add(AnimatePosition.Do(this.items[j].gameObject, EZAnimation.ANIM_MODE.To, vector, EZAnimation.GetInterpolator(this.positionEasing), this.positionEaseDuration, this.positionEaseDelay, null, new EZAnimation.CompletionDelegate(this.OnPosEasingDone)));
						}
					}
					else
					{
						this.items[j].transform.localPosition = vector;
					}
					float num3 = this.items[j].TopLeftEdge().y - this.items[j].BottomRightEdge().y + this.itemSpacing;
					this.contentExtents += num3;
					num -= num3;
					this.items[j].SetIndex(j);
				}
			}
			if (!this.spacingAtEnds)
			{
				this.contentExtents -= this.itemSpacing;
			}
		}
		else
		{
			float num = this.viewableArea.y * -0.5f;
			for (int k = 0; k < this.items.Count; k++)
			{
				if (updateExtents)
				{
					this.items[k].FindOuterEdges();
				}
				Vector3 vector = new Vector3(itemAlignmentDel(this.items[k]), num - this.items[k].BottomRightEdge().y, -0.1f);
				if (this.doItemEasing)
				{
					if (this.newItems.Contains(this.items[k]))
					{
						this.items[k].transform.localPosition = vector;
					}
					else
					{
						this.itemEasers.Add(AnimatePosition.Do(this.items[k].gameObject, EZAnimation.ANIM_MODE.To, vector, EZAnimation.GetInterpolator(this.positionEasing), this.positionEaseDuration, this.positionEaseDelay, null, new EZAnimation.CompletionDelegate(this.OnPosEasingDone)));
					}
				}
				else
				{
					this.items[k].transform.localPosition = vector;
				}
				float num3 = this.items[k].TopLeftEdge().y - this.items[k].BottomRightEdge().y + this.itemSpacing;
				this.contentExtents += num3;
				num += num3;
				this.items[k].SetIndex(k);
			}
			if (!this.spacingAtEnds)
			{
				this.contentExtents -= this.itemSpacing;
			}
		}
	}

	public void ClipItems()
	{
		if (this.mover == null || this.items.Count < 1 || !base.gameObject.activeInHierarchy || !this.clipContents)
		{
			return;
		}
		IUIListObject iUIListObject = null;
		if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			float x = this.mover.transform.localPosition.x;
			float num = this.viewableArea.x * -0.5f - x;
			float num2 = this.viewableArea.x * 0.5f - x;
			int i = (int)((float)(this.items.Count - 1) * Mathf.Clamp01(this.scrollPos));
			if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
			{
				float x2 = this.items[i].transform.localPosition.x;
				if (this.items[i].BottomRightEdge().x + x2 >= num)
				{
					for (i--; i > -1; i--)
					{
						x2 = this.items[i].transform.localPosition.x;
						if (this.items[i].BottomRightEdge().x + x2 < num)
						{
							break;
						}
					}
					iUIListObject = this.items[i + 1];
				}
				else
				{
					while (i < this.items.Count)
					{
						x2 = this.items[i].transform.localPosition.x;
						if (this.items[i].BottomRightEdge().x + x2 >= num)
						{
							iUIListObject = this.items[i];
							break;
						}
						i++;
					}
				}
				if (iUIListObject != null)
				{
					this.tempVisItems.Add(iUIListObject);
					if (!iUIListObject.gameObject.activeInHierarchy)
					{
						iUIListObject.gameObject.SetActive(true);
					}
					iUIListObject.Hide(false);
					iUIListObject.SetClippingRect(this.clientClippingRect);
					x2 = iUIListObject.transform.localPosition.x;
					if (iUIListObject.BottomRightEdge().x + x2 < num2)
					{
						for (i = iUIListObject.GetIndex() + 1; i < this.items.Count; i++)
						{
							x2 = this.items[i].transform.localPosition.x;
							if (this.items[i].BottomRightEdge().x + x2 >= num2)
							{
								if (!this.items[i].gameObject.activeInHierarchy)
								{
									this.items[i].gameObject.SetActive(true);
								}
								this.items[i].Hide(false);
								this.items[i].SetClippingRect(this.clientClippingRect);
								this.tempVisItems.Add(this.items[i]);
								break;
							}
							if (!this.items[i].gameObject.activeInHierarchy)
							{
								this.items[i].gameObject.SetActive(true);
							}
							this.items[i].Hide(false);
							this.items[i].SetClipped(false);
							this.tempVisItems.Add(this.items[i]);
						}
					}
				}
			}
			else
			{
				float x2 = this.items[i].transform.localPosition.x;
				if (this.items[i].TopLeftEdge().x + x2 <= num2)
				{
					for (i--; i > -1; i--)
					{
						x2 = this.items[i].transform.localPosition.x;
						if (this.items[i].TopLeftEdge().x + x2 > num2)
						{
							break;
						}
					}
					iUIListObject = this.items[i + 1];
				}
				else
				{
					while (i < this.items.Count)
					{
						x2 = this.items[i].transform.localPosition.x;
						if (this.items[i].TopLeftEdge().x + x2 <= num2)
						{
							iUIListObject = this.items[i];
							break;
						}
						i++;
					}
				}
				if (iUIListObject != null)
				{
					this.tempVisItems.Add(iUIListObject);
					if (!iUIListObject.gameObject.activeInHierarchy)
					{
						iUIListObject.gameObject.SetActive(true);
					}
					iUIListObject.Hide(false);
					iUIListObject.SetClippingRect(this.clientClippingRect);
					x2 = iUIListObject.transform.localPosition.x;
					if (iUIListObject.TopLeftEdge().x + x2 > num)
					{
						for (i = iUIListObject.GetIndex() + 1; i < this.items.Count; i++)
						{
							x2 = this.items[i].transform.localPosition.x;
							if (this.items[i].TopLeftEdge().x + x2 <= num)
							{
								if (!this.items[i].gameObject.activeInHierarchy)
								{
									this.items[i].gameObject.SetActive(true);
								}
								this.items[i].Hide(false);
								this.items[i].SetClippingRect(this.clientClippingRect);
								this.tempVisItems.Add(this.items[i]);
								break;
							}
							if (!this.items[i].gameObject.activeInHierarchy)
							{
								this.items[i].gameObject.SetActive(true);
							}
							this.items[i].Hide(false);
							this.items[i].SetClipped(false);
							this.tempVisItems.Add(this.items[i]);
						}
					}
				}
			}
		}
		else
		{
			float y = this.mover.transform.localPosition.y;
			float num3 = this.viewableArea.y * 0.5f - y;
			float num4 = this.viewableArea.y * -0.5f - y;
			int j = (int)((float)(this.items.Count - 1) * Mathf.Clamp01(this.scrollPos));
			if (this.direction == UIScrollList.DIRECTION.TtoB_LtoR)
			{
				float y2 = this.items[j].transform.localPosition.y;
				if (this.items[j].BottomRightEdge().y + y2 <= num3)
				{
					for (j--; j > -1; j--)
					{
						y2 = this.items[j].transform.localPosition.y;
						if (this.items[j].BottomRightEdge().y + y2 > num3)
						{
							break;
						}
					}
					iUIListObject = this.items[j + 1];
				}
				else
				{
					while (j < this.items.Count)
					{
						y2 = this.items[j].transform.localPosition.y;
						if (this.items[j].BottomRightEdge().y + y2 <= num3)
						{
							iUIListObject = this.items[j];
							break;
						}
						j++;
					}
				}
				if (iUIListObject != null)
				{
					this.tempVisItems.Add(iUIListObject);
					if (!iUIListObject.gameObject.activeInHierarchy)
					{
						iUIListObject.gameObject.SetActive(true);
					}
					iUIListObject.Hide(false);
					iUIListObject.SetClippingRect(this.clientClippingRect);
					y2 = iUIListObject.transform.localPosition.y;
					if (iUIListObject.BottomRightEdge().y + y2 > num4)
					{
						for (j = iUIListObject.GetIndex() + 1; j < this.items.Count; j++)
						{
							y2 = this.items[j].transform.localPosition.y;
							float num5 = this.items[j].BottomRightEdge().y + y2;
							int num6 = j % this.columnCount;
							bool flag = !this.line || this.columnCount - 1 == num6;
							if (num5 <= num4 && flag)
							{
								if (!this.items[j].gameObject.activeInHierarchy)
								{
									this.items[j].gameObject.SetActive(true);
								}
								this.items[j].Hide(false);
								this.items[j].SetClippingRect(this.clientClippingRect);
								this.tempVisItems.Add(this.items[j]);
								this.changeScrollPos = true;
								break;
							}
							if (num5 <= num4 && this.columnCount - 1 != num6)
							{
								if (!this.items[j].gameObject.activeInHierarchy)
								{
									this.items[j].gameObject.SetActive(true);
								}
								this.items[j].Hide(false);
								this.items[j].SetClippingRect(this.clientClippingRect);
								this.tempVisItems.Add(this.items[j]);
							}
							else if (num5 > num4)
							{
								if (!this.items[j].gameObject.activeInHierarchy)
								{
									this.items[j].gameObject.SetActive(true);
								}
								this.items[j].Hide(false);
								this.items[j].SetClippingRect(this.clientClippingRect);
								this.tempVisItems.Add(this.items[j]);
							}
							else
							{
								if (!this.items[j].gameObject.activeInHierarchy)
								{
									this.items[j].gameObject.SetActive(true);
								}
								this.items[j].Hide(false);
								this.items[j].SetClipped(false);
								this.tempVisItems.Add(this.items[j]);
							}
						}
					}
				}
			}
			else
			{
				float y2 = this.items[j].transform.localPosition.y;
				if (this.items[j].TopLeftEdge().y + y2 >= num4)
				{
					for (j--; j > -1; j--)
					{
						y2 = this.items[j].transform.localPosition.y;
						if (this.items[j].TopLeftEdge().y + y2 < num4)
						{
							break;
						}
					}
					iUIListObject = this.items[j + 1];
				}
				else
				{
					while (j < this.items.Count)
					{
						y2 = this.items[j].transform.localPosition.y;
						if (this.items[j].TopLeftEdge().y + y2 >= num4)
						{
							iUIListObject = this.items[j];
							break;
						}
						j++;
					}
				}
				if (iUIListObject != null)
				{
					this.tempVisItems.Add(iUIListObject);
					if (!iUIListObject.gameObject.activeInHierarchy)
					{
						iUIListObject.gameObject.SetActive(true);
					}
					iUIListObject.Hide(false);
					iUIListObject.SetClippingRect(this.clientClippingRect);
					y2 = iUIListObject.transform.localPosition.y;
					if (iUIListObject.TopLeftEdge().y + y2 < num3)
					{
						for (j = iUIListObject.GetIndex() + 1; j < this.items.Count; j++)
						{
							y2 = this.items[j].transform.localPosition.y;
							if (this.items[j].TopLeftEdge().y + y2 >= num3)
							{
								if (!this.items[j].gameObject.activeInHierarchy)
								{
									this.items[j].gameObject.SetActive(true);
								}
								this.items[j].Hide(false);
								this.items[j].SetClippingRect(this.clientClippingRect);
								this.tempVisItems.Add(this.items[j]);
								break;
							}
							if (!this.items[j].gameObject.activeInHierarchy)
							{
								this.items[j].gameObject.SetActive(true);
							}
							this.items[j].Hide(false);
							this.items[j].SetClipped(false);
							this.tempVisItems.Add(this.items[j]);
						}
					}
				}
			}
		}
		if (iUIListObject == null)
		{
			return;
		}
		IUIListObject iUIListObject2 = this.tempVisItems[this.tempVisItems.Count - 1];
		if (this.visibleItems.Count > 0)
		{
			if (this.visibleItems[0].GetIndex() > iUIListObject2.GetIndex() || this.visibleItems[this.visibleItems.Count - 1].GetIndex() < iUIListObject.GetIndex())
			{
				for (int k = 0; k < this.visibleItems.Count; k++)
				{
					this.visibleItems[k].Hide(true);
					if (!this.visibleItems[k].Managed)
					{
						this.visibleItems[k].gameObject.SetActive(false);
					}
					this.changeScrollPos = true;
				}
			}
			else
			{
				for (int l = 0; l < this.visibleItems.Count; l++)
				{
					if (this.visibleItems[l].GetIndex() >= iUIListObject.GetIndex())
					{
						break;
					}
					this.visibleItems[l].Hide(true);
					if (!this.visibleItems[l].Managed)
					{
						this.visibleItems[l].gameObject.SetActive(false);
					}
					this.changeScrollPos = true;
				}
				for (int m = this.visibleItems.Count - 1; m > -1; m--)
				{
					if (this.visibleItems[m].GetIndex() <= iUIListObject2.GetIndex())
					{
						break;
					}
					this.visibleItems[m].Hide(true);
					if (!this.visibleItems[m].Managed)
					{
						this.visibleItems[m].gameObject.SetActive(false);
					}
					this.changeScrollPos = true;
				}
			}
		}
		if (!this.hideSlider)
		{
			if (this.chatLabelScroll)
			{
				if (0 < iUIListObject.GetIndex())
				{
					this.changeScrollPos = true;
				}
				if (this.changeScrollPos && null != this.slider && !this.slider.Visible)
				{
					this.slider.Visible = true;
				}
			}
			else if (this.items.Count > this.tempVisItems.Count)
			{
				if (null != this.slider && !this.slider.Visible)
				{
					this.slider.Visible = true;
					this.count = this.tempVisItems.Count;
				}
				this.changeScrollPos = true;
			}
			else if (null != this.slider && this.slider.Visible)
			{
				if (this.items.Count <= this.count)
				{
					this.slider.Visible = false;
				}
			}
		}
		List<IUIListObject> list = this.visibleItems;
		this.visibleItems = this.tempVisItems;
		this.tempVisItems = list;
		this.tempVisItems.Clear();
	}

	public void DidSelect(UIListItemContainer item)
	{
		if (this.multiSelectMode)
		{
			if (this.selectedItems.ContainsKey(item.GetIndex()))
			{
				item.SetSelected(false);
				this.selectedItems.Remove(item.GetIndex());
			}
			else
			{
				if (this.maxMultiSelectNum <= this.selectedItems.Count)
				{
					return;
				}
				if (!this.selectedItems.ContainsKey(item.GetIndex()))
				{
					item.SetSelected(true);
					this.selectedItems.Add(item.GetIndex(), item);
				}
			}
		}
		else
		{
			if (this.selectedItem != null && this.selectedItem != item)
			{
				((UIListItemContainer)this.selectedItem).SetSelected(false);
			}
			this.selectedItem = item;
			item.SetSelected(true);
		}
	}

	public void DidLongClick(IUIListObject item)
	{
		if (this.selectedItem != null && this.selectedItem != item)
		{
			((UIListItemContainer)this.selectedItem).SetSelected(false);
		}
		this.selectedItem = item;
		((UIListItemContainer)item).SetSelected(true);
		if (this.longTapDelegate != null)
		{
			this.longTapDelegate(this);
		}
	}

	public void DidClick(IUIListObject item)
	{
		if (this.scriptWithMethodToInvoke != null)
		{
			this.scriptWithMethodToInvoke.Invoke(this.methodToInvokeOnSelect, 0f);
		}
		if (this.changeDelegate != null)
		{
			this.changeDelegate(this);
		}
		MsgHandler.Handle("ListSound", new object[0]);
	}

	public void DidDoubleClick(IUIListObject item)
	{
		if (this.doubleClickDelegate != null)
		{
			this.doubleClickDelegate(this);
		}
	}

	public void DidMouseOver(IUIListObject item)
	{
		this.mouseItem = item;
		if (this.mouseOverDelegate != null)
		{
			this.mouseOverDelegate(this);
		}
	}

	public void DidMouseOut(IUIListObject item)
	{
		this.mouseItem = item;
		if (this.mouseOutDelegate != null)
		{
			this.mouseOutDelegate(this);
		}
	}

	public void DirOverView(IUIListObject item)
	{
	}

	public void DirOverViewClear(IUIListObject obj)
	{
	}

	public void DidRightMouse(IUIListObject item)
	{
		if (this.rightMouseSelect)
		{
			if (this.selectedItem != null)
			{
				if (this.selectedItem != item)
				{
					((UIListItemContainer)this.selectedItem).SetSelected(false);
				}
				this.selectedItem = item;
				((UIListItemContainer)item).SetSelected(true);
			}
			else
			{
				this.selectedItem = item;
				((UIListItemContainer)item).SetSelected(true);
			}
		}
		this.mouseItem = item;
		if (this.rightMouseDelegate != null)
		{
			this.rightMouseDelegate(this);
		}
	}

	protected virtual void ReserveMakeItem()
	{
	}

	public void ListDragged(POINTER_INFO ptr)
	{
		if (!this.touchScroll || !this.controlIsEnabled || !this.listDrag)
		{
			return;
		}
		this.ReserveMakeItem();
		this.autoScrolling = false;
		Plane plane = default(Plane);
		if (Mathf.Approximately(ptr.inputDelta.sqrMagnitude, 0f))
		{
			this.scrollDelta = 0f;
			return;
		}
		this.listMoved = true;
		plane.SetNormalAndPosition(this.mover.transform.forward * -1f, this.mover.transform.position);
		float d;
		plane.Raycast(ptr.ray, out d);
		Vector3 a = ptr.ray.origin + ptr.ray.direction * d;
		plane.Raycast(ptr.prevRay, out d);
		Vector3 b = ptr.prevRay.origin + ptr.prevRay.direction * d;
		a = base.transform.InverseTransformPoint(a);
		b = base.transform.InverseTransformPoint(b);
		Vector3 vector = a - b;
		if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			this.scrollDelta = -vector.x / this.amtOfPlay;
		}
		else
		{
			this.scrollDelta = vector.y / this.amtOfPlay;
		}
		float num = this.scrollPos + this.scrollDelta;
		if (num > 1f)
		{
			this.scrollDelta *= Mathf.Clamp01(1f - (num - 1f) / this.scrollMax);
		}
		else if (num < 0f)
		{
			this.scrollDelta *= Mathf.Clamp01(1f + num / this.scrollMax);
		}
		if (this.direction == UIScrollList.DIRECTION.BtoT_RtoL)
		{
			this.scrollDelta *= -1f;
		}
		this.ScrollListTo_Internal(this.scrollPos + this.scrollDelta);
		this.noTouch = false;
		this.isScrolling = true;
	}

	public void PointerReleased()
	{
		this.noTouch = true;
		if (this.scrollInertia != 0f)
		{
			this.scrollDelta = this.scrollInertia;
		}
		this.scrollInertia = 0f;
		if (this.snap && this.listMoved)
		{
			this.CalcSnapItem();
		}
		this.listMoved = false;
	}

	public void OnEnable()
	{
		base.gameObject.SetActive(true);
		this.ClipItems();
	}

	protected virtual void OnDisable()
	{
		if (Application.isPlaying)
		{
			if (EZAnimator.Exists())
			{
				EZAnimator.instance.Stop(base.gameObject);
				EZAnimator.instance.Stop(this);
			}
			if (this.detargetOnDisable && UIManager.Exists())
			{
				NrTSingleton<UIManager>.Instance.Detarget(this);
			}
		}
	}

	public bool IsClickKnob()
	{
		return this.slider && null != this.slider.GetKnob() && this.slider.GetKnob().IsClickKnob();
	}

	public void SetSelectedItem(int index)
	{
		if (index < 0 || index >= this.items.Count)
		{
			if (this.selectedItem != null)
			{
				((UIListItemContainer)this.selectedItem).SetSelected(false);
			}
			this.selectedItem = null;
			return;
		}
		IUIListObject iUIListObject = this.items[index];
		if (this.selectedItem != null)
		{
			((UIListItemContainer)this.selectedItem).SetSelected(false);
		}
		this.selectedItem = iUIListObject;
		((UIListItemContainer)iUIListObject).SetSelected(true);
		if (this.autoScroll)
		{
			this.ScrollToItem(this.selectedItem, 0.1f);
		}
	}

	public IUIListObject GetItem(int index)
	{
		if (this.items.Count == 0)
		{
			return null;
		}
		if (index < 0 || index >= this.items.Count)
		{
			return null;
		}
		return this.items[index];
	}

	public IUIListObject GetSelectItem()
	{
		if (this.items.Count == 0)
		{
			return null;
		}
		if (this.SelectedItem == null)
		{
			return null;
		}
		int index = this.SelectedItem.GetIndex();
		if (index < 0 || index >= this.items.Count)
		{
			return null;
		}
		return this.items[index];
	}

	public void FadeList(float dur)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)this.items[i];
			if (null != uIListItemContainer)
			{
				uIListItemContainer.FadeListItemContainer(dur);
			}
		}
	}

	public void SetAlphaList(float a)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			UIListItemContainer uIListItemContainer = (UIListItemContainer)this.items[i];
			if (null != uIListItemContainer)
			{
				uIListItemContainer.SetAlphaList(a);
			}
		}
	}

	private void DestroyIUI(IUIListObject items)
	{
		UnityEngine.Object.Destroy(items.gameObject);
	}

	public void RemoveItem(int index, bool destroy)
	{
		if (index < 0 || index >= this.items.Count)
		{
			return;
		}
		this.items[index].DeleteAnim();
		if (this.container != null)
		{
			this.container.RemoveChild(this.items[index].gameObject);
		}
		if (this.selectedItem == this.items[index])
		{
			this.selectedItem = null;
			((UIListItemContainer)this.items[index]).SetSelected(false);
		}
		if (this.multiSelectMode && this.selectedItems.ContainsKey(index))
		{
			this.selectedItems[index].Visible = false;
			this.selectedItems.Remove(index);
		}
		this.visibleItems.Remove(this.items[index]);
		if (destroy)
		{
			this.items[index].Delete();
			this.DestroyIUI(this.items[index]);
		}
		else
		{
			this.items[index].transform.parent = null;
			this.items[index].gameObject.SetActive(false);
		}
		this.items.RemoveAt(index);
		this.PositionItems();
	}

	public void RemoveItem(int index, bool destroy, bool doEasing)
	{
		if (index < 0 || index >= this.items.Count)
		{
			return;
		}
		this.items[index].DeleteAnim();
		if (index == this.items.Count - 1)
		{
			this.doItemEasing = false;
		}
		else
		{
			this.doItemEasing = doEasing;
		}
		this.doPosEasing = doEasing;
		if (this.container != null)
		{
			this.container.RemoveChild(this.items[index].gameObject);
		}
		if (this.selectedItem == this.items[index])
		{
			this.selectedItem = null;
			((UIListItemContainer)this.items[index]).SetSelected(false);
		}
		this.visibleItems.Remove(this.items[index]);
		if (destroy)
		{
			this.items[index].Delete();
			UnityEngine.Object.Destroy(this.items[index].gameObject);
		}
		else
		{
			this.items[index].transform.parent = null;
			this.items[index].gameObject.SetActive(false);
		}
		this.items.RemoveAt(index);
		this.PositionItems();
	}

	public void RemoveItemDonotPositionUpdate(int index, bool destroy)
	{
		if (index < 0 || index >= this.items.Count)
		{
			return;
		}
		this.items[index].DeleteAnim();
		if (this.container != null)
		{
			this.container.RemoveChild(this.items[index].gameObject);
		}
		if (this.selectedItem == this.items[index])
		{
			this.selectedItem = null;
			((UIListItemContainer)this.items[index]).SetSelected(false);
		}
		if (this.multiSelectMode && this.selectedItems.ContainsKey(index))
		{
			this.selectedItems[index].Visible = false;
			this.selectedItems.Remove(index);
		}
		this.visibleItems.Remove(this.items[index]);
		if (destroy)
		{
			this.items[index].Delete();
			this.DestroyIUI(this.items[index]);
		}
		else
		{
			this.items[index].transform.parent = null;
			this.items[index].gameObject.SetActive(false);
		}
		this.items.RemoveAt(index);
	}

	public void RemoveItem(IUIListObject item, bool destroy)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i] == item)
			{
				this.RemoveItem(i, destroy);
				return;
			}
		}
	}

	public void ClearList(bool destroy)
	{
		this.RemoveItemsFromContainer();
		this.selectedItem = null;
		this.mouseItem = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			this.items[i].transform.parent = null;
			this.items[i].DeleteAnim();
			if (destroy)
			{
				this.DestroyIUI(this.items[i]);
			}
			else
			{
				this.items[i].gameObject.SetActive(false);
			}
		}
		this.visibleItems.Clear();
		this.items.Clear();
		if (this.multiSelectMode)
		{
			this.selectedItems.Clear();
		}
		this.PositionItems();
		this.count = 0;
		if (this.slider)
		{
			this.slider.Visible = false;
		}
	}

	public void OnInput(POINTER_INFO ptr)
	{
		if (!this.m_controlIsEnabled)
		{
			if (this.Container != null)
			{
				ptr.callerIsControl = true;
				this.Container.OnInput(ptr);
			}
			return;
		}
		if (Vector3.SqrMagnitude(ptr.origPos - ptr.devicePos) > this.dragThreshold * this.dragThreshold)
		{
			ptr.isTap = false;
			if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP || ptr.evt == POINTER_INFO.INPUT_EVENT.LONG_TAP)
			{
				ptr.evt = POINTER_INFO.INPUT_EVENT.RELEASE;
			}
		}
		else
		{
			ptr.isTap = true;
		}
		if (this.inputDelegate != null)
		{
			this.inputDelegate(ref ptr);
		}
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
			if (ptr.active)
			{
				this.ListDragged(ptr);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			this.PointerReleased();
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			this.overList = false;
			break;
		case POINTER_INFO.INPUT_EVENT.DRAG:
			this.overList = false;
			this.ListDragged(ptr);
			break;
		case POINTER_INFO.INPUT_EVENT.MOUSE_WHEEL:
			this.overList = true;
			break;
		}
		if (this.Container != null)
		{
			ptr.callerIsControl = true;
			this.Container.OnInput(ptr);
		}
	}

	public void ScrollWheel(float amt)
	{
		if (this.direction == UIScrollList.DIRECTION.BtoT_RtoL)
		{
			amt *= -1f;
		}
		this.ScrollListTo(Mathf.Clamp01(this.scrollPos - amt * this.scrollWheelFactor / this.amtOfPlay));
	}

	public virtual void Update()
	{
		if (this.newItems.Count != 0)
		{
			if (this.itemsInserted || this.doItemEasing)
			{
				this.RepositionItems();
			}
			else
			{
				this.PositionNewItems();
			}
			this.itemsInserted = false;
			this.newItems.Clear();
		}
		this.timeDelta = Time.realtimeSinceStartup - this.lastTime;
		this.lastTime = Time.realtimeSinceStartup;
		this.inertiaLerpTime += this.timeDelta;
		if (this.cachedPos != base.transform.position || this.cachedRot != base.transform.rotation || this.cachedScale != base.transform.lossyScale || this.cachedviewableArea != this.viewableArea || this.bCalcClipping)
		{
			this.cachedPos = base.transform.position;
			this.cachedRot = base.transform.rotation;
			this.cachedScale = base.transform.lossyScale;
			this.cachedviewableArea = this.viewableArea;
			this.CalcClippingRect();
			if (this.clipWhenMoving)
			{
				this.ClipItems();
				this.clipWhenMoving = false;
			}
		}
		if (this.itemEasers.Count > 0)
		{
			this.ClipItems();
		}
		if (TsPlatform.IsWeb)
		{
			if (this.overList && null != this.slider)
			{
				if (this.line)
				{
					if (0f > NkInputManager.GetAxis("Mouse ScrollWheel"))
					{
						this.slider.ClickDownButton(null);
					}
					else if (0f < NkInputManager.GetAxis("Mouse ScrollWheel"))
					{
						this.slider.ClickUpButton(null);
					}
				}
				else if (NkInputManager.GetAxis("Mouse ScrollWheel") != 0f)
				{
					if (this.slider.Visible)
					{
						this.ScrollWheel(NkInputManager.GetAxis("Mouse ScrollWheel"));
					}
				}
			}
			else if (null == this.slider)
			{
			}
		}
		if (!this.noTouch && this.inertiaLerpTime >= this.inertiaLerpInterval)
		{
			this.scrollInertia = Mathf.Lerp(this.scrollInertia, this.scrollDelta, this.lowPassFilterFactor);
			this.scrollDelta = 0f;
			this.inertiaLerpTime %= this.inertiaLerpInterval;
		}
		if (this.isScrolling && this.noTouch && !this.autoScrolling)
		{
			this.scrollDelta -= this.scrollDelta * 0.1f;
			if (this.scrollPos < 0f)
			{
				this.scrollPos -= this.scrollPos * 1f * (this.timeDelta / 0.166f);
				this.scrollDelta *= Mathf.Clamp01(1f + this.scrollPos / this.scrollMax);
			}
			else if (this.scrollPos > 1f)
			{
				this.scrollPos -= (this.scrollPos - 1f) * 1f * (this.timeDelta / 0.166f);
				this.scrollDelta *= Mathf.Clamp01(1f - (this.scrollPos - 1f) / this.scrollMax);
			}
			if (Mathf.Abs(this.scrollDelta) < 0.0001f)
			{
				this.scrollDelta = 0f;
				if (this.scrollPos > -0.0001f && this.scrollPos < 0.0001f)
				{
					this.scrollPos = Mathf.Clamp01(this.scrollPos);
				}
			}
			this.ScrollListTo_Internal(this.scrollPos + this.scrollDelta);
			if (this.scrollPos >= 0f && this.scrollPos <= 1.001f && this.scrollDelta == 0f)
			{
				this.isScrolling = false;
			}
		}
		else if (this.autoScrolling)
		{
			this.autoScrollTime += this.timeDelta;
			if (this.autoScrollTime >= this.autoScrollDuration)
			{
				this.autoScrolling = false;
				this.scrollPos = this.autoScrollPos;
			}
			else
			{
				this.scrollPos = this.autoScrollInterpolator(this.autoScrollTime, this.autoScrollStart, this.autoScrollDelta, this.autoScrollDuration);
			}
			this.ScrollListTo_Internal(this.scrollPos);
		}
	}

	protected void CalcSnapItem()
	{
		int num = 1;
		if (this.items.Count < 1)
		{
			return;
		}
		float num2;
		float num3;
		if (Mathf.Approximately(this.scrollDelta, 0f))
		{
			num2 = 1f;
			num3 = this.scrollPos;
		}
		else
		{
			num3 = this.scrollPos + this.scrollDelta / 0.1f;
			float num4 = Mathf.Abs(this.scrollDelta);
			num2 = Time.fixedDeltaTime * (this.scrollStopThresholdLog - Mathf.Log10(num4)) / Mathf.Log10((num4 - num4 * 0.1f) / num4);
			num2 = Mathf.Max(num2, this.minSnapDuration);
		}
		if (num3 >= 1f || num3 <= 0f)
		{
			if (num3 <= 0f)
			{
				this.ScrollToItem(0, num2);
			}
			else
			{
				this.ScrollToItem(this.items.Count - 1, num2);
			}
			return;
		}
		int num5 = (int)Mathf.Clamp((float)(this.items.Count - 1) * num3, 0f, (float)(this.items.Count - 1));
		if (this.orientation == UIScrollList.ORIENTATION.HORIZONTAL)
		{
			float num6 = (this.direction != UIScrollList.DIRECTION.TtoB_LtoR) ? 1f : -1f;
			IUIListObject iUIListObject = this.items[num5];
			float num7 = Mathf.Abs(num3 + num6 * iUIListObject.transform.localPosition.x / this.amtOfPlay);
			if (num5 + num < this.items.Count)
			{
				IUIListObject iUIListObject2 = this.items[num5 + num];
				float num8 = Mathf.Abs(num3 + num6 * iUIListObject2.transform.localPosition.x / this.amtOfPlay);
				if (num8 < num7)
				{
					num7 = num8;
					iUIListObject = iUIListObject2;
					num5 += num;
				}
				else
				{
					num = -1;
				}
			}
			else
			{
				num = -1;
			}
			int num9 = num5 + num;
			while (num9 > -1 && num9 < this.items.Count)
			{
				float num8 = Mathf.Abs(num3 + num6 * this.items[num9].transform.localPosition.x / this.amtOfPlay);
				if (num8 >= num7)
				{
					break;
				}
				num7 = num8;
				iUIListObject = this.items[num9];
				num9 += num;
			}
			this.ScrollToItem(iUIListObject, num2);
		}
		else
		{
			float num10 = (this.direction != UIScrollList.DIRECTION.TtoB_LtoR) ? -1f : 1f;
			IUIListObject iUIListObject = this.items[num5];
			float num7 = Mathf.Abs(num3 + num10 * iUIListObject.transform.localPosition.y / this.amtOfPlay);
			if (num5 + num < this.items.Count)
			{
				IUIListObject iUIListObject2 = this.items[num5 + num];
				float num8 = Mathf.Abs(num3 + num10 * iUIListObject2.transform.localPosition.y / this.amtOfPlay);
				if (num8 < num7)
				{
					num7 = num8;
					iUIListObject = iUIListObject2;
					num5 += num;
				}
				else
				{
					num = -1;
				}
			}
			else
			{
				num = -1;
			}
			int num11 = num5 + num;
			while (num11 > -1 && num11 < this.items.Count)
			{
				float num8 = Mathf.Abs(num3 + num10 * this.items[num11].transform.localPosition.y / this.amtOfPlay);
				if (num8 >= num7)
				{
					break;
				}
				num7 = num8;
				iUIListObject = this.items[num11];
				num11 += num;
			}
			this.ScrollToItem(iUIListObject, num2);
		}
	}

	protected void AddItemsToContainer()
	{
		if (this.container == null)
		{
			return;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			this.container.AddChild(this.items[i].gameObject);
		}
	}

	protected void RemoveItemsFromContainer()
	{
		if (this.container == null)
		{
			return;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			this.container.RemoveChild(this.items[i].gameObject);
		}
	}

	public bool RequestContainership(IUIContainer cont)
	{
		Transform parent = base.transform.parent;
		Transform transform = ((Component)cont).transform;
		while (parent != null)
		{
			if (parent == transform)
			{
				this.container = cont;
				return true;
			}
			if (parent.gameObject.GetComponent("IUIContainer") != null)
			{
				return false;
			}
			parent = parent.parent;
		}
		return false;
	}

	public bool GotFocus()
	{
		return false;
	}

	public void SetInputDelegate(EZInputDelegate del)
	{
		this.inputDelegate = del;
	}

	public void AddInputDelegate(EZInputDelegate del)
	{
		this.inputDelegate = (EZInputDelegate)Delegate.Combine(this.inputDelegate, del);
	}

	public void RemoveInputDelegate(EZInputDelegate del)
	{
		this.inputDelegate = (EZInputDelegate)Delegate.Remove(this.inputDelegate, del);
	}

	public void SetValueChangedDelegate(EZValueChangedDelegate del)
	{
		this.changeDelegate = del;
	}

	public void AddValueChangedDelegate(EZValueChangedDelegate del)
	{
		this.changeDelegate = (EZValueChangedDelegate)Delegate.Combine(this.changeDelegate, del);
	}

	public void RemoveValueChangedDelegate(EZValueChangedDelegate del)
	{
		this.changeDelegate = (EZValueChangedDelegate)Delegate.Remove(this.changeDelegate, del);
	}

	public void SetMouseOverDelegate(EZValueChangedDelegate del)
	{
		this.mouseOverDelegate = del;
	}

	public void AddMouseOverDelegate(EZValueChangedDelegate del)
	{
		this.mouseOverDelegate = (EZValueChangedDelegate)Delegate.Combine(this.mouseOverDelegate, del);
	}

	public void RemoveMouseOverDelegate(EZValueChangedDelegate del)
	{
		this.mouseOverDelegate = (EZValueChangedDelegate)Delegate.Remove(this.mouseOverDelegate, del);
	}

	public void SetMouseOutDelegate(EZValueChangedDelegate del)
	{
		this.mouseOutDelegate = del;
	}

	public void AddMouseOutDelegate(EZValueChangedDelegate del)
	{
		this.mouseOutDelegate = (EZValueChangedDelegate)Delegate.Combine(this.mouseOutDelegate, del);
	}

	public void RemoveMouseOutDelegate(EZValueChangedDelegate del)
	{
		this.mouseOutDelegate = (EZValueChangedDelegate)Delegate.Remove(this.mouseOutDelegate, del);
	}

	public void SetRightMouseDelegate(EZValueChangedDelegate del)
	{
		this.rightMouseDelegate = del;
	}

	public void AddRightMouseDelegate(EZValueChangedDelegate del)
	{
		this.rightMouseDelegate = (EZValueChangedDelegate)Delegate.Combine(this.rightMouseDelegate, del);
	}

	public void RemoveRightMouseDelegate(EZValueChangedDelegate del)
	{
		this.rightMouseDelegate = (EZValueChangedDelegate)Delegate.Remove(this.rightMouseDelegate, del);
	}

	public void SetDoubleClickDelegate(EZValueChangedDelegate del)
	{
		this.doubleClickDelegate = del;
	}

	public void AddDoubleClickDelegate(EZValueChangedDelegate del)
	{
		this.doubleClickDelegate = (EZValueChangedDelegate)Delegate.Combine(this.doubleClickDelegate, del);
	}

	public void RemoveDoubleClickDelegate(EZValueChangedDelegate del)
	{
		this.doubleClickDelegate = (EZValueChangedDelegate)Delegate.Remove(this.doubleClickDelegate, del);
	}

	public void AddLongTapDelegate(EZValueChangedDelegate del)
	{
		this.longTapDelegate = (EZValueChangedDelegate)Delegate.Combine(this.longTapDelegate, del);
	}

	public void RemoveLongTapDelegate(EZValueChangedDelegate del)
	{
		this.longTapDelegate = (EZValueChangedDelegate)Delegate.Remove(this.longTapDelegate, del);
	}

	public bool IsDragging()
	{
		return false;
	}

	public void SetDragging(bool value)
	{
	}

	public bool DragUpdatePosition(POINTER_INFO ptr)
	{
		return true;
	}

	public void CancelDrag()
	{
	}

	public void OnEZDragDrop(EZDragDropParams parms)
	{
		if (this.dragDropDelegate != null)
		{
			this.dragDropDelegate(parms);
		}
	}

	public void AddDragDropDelegate(EZDragDropDelegate del)
	{
		this.dragDropDelegate = (EZDragDropDelegate)Delegate.Combine(this.dragDropDelegate, del);
	}

	public void RemoveDragDropDelegate(EZDragDropDelegate del)
	{
		this.dragDropDelegate = (EZDragDropDelegate)Delegate.Remove(this.dragDropDelegate, del);
	}

	public void SetDragDropDelegate(EZDragDropDelegate del)
	{
		this.dragDropDelegate = del;
	}

	private void OnDrawGizmosSelected()
	{
		Vector3 vector = base.transform.position - base.transform.TransformDirection(Vector3.right * this.viewableArea.x * 0.5f * base.transform.lossyScale.x) + base.transform.TransformDirection(Vector3.up * this.viewableArea.y * 0.5f * base.transform.lossyScale.y);
		Vector3 vector2 = base.transform.position - base.transform.TransformDirection(Vector3.right * this.viewableArea.x * 0.5f * base.transform.lossyScale.x) - base.transform.TransformDirection(Vector3.up * this.viewableArea.y * 0.5f * base.transform.lossyScale.y);
		Vector3 vector3 = base.transform.position + base.transform.TransformDirection(Vector3.right * this.viewableArea.x * 0.5f * base.transform.lossyScale.x) - base.transform.TransformDirection(Vector3.up * this.viewableArea.y * 0.5f * base.transform.lossyScale.y);
		Vector3 vector4 = base.transform.position + base.transform.TransformDirection(Vector3.right * this.viewableArea.x * 0.5f * base.transform.lossyScale.x) + base.transform.TransformDirection(Vector3.up * this.viewableArea.y * 0.5f * base.transform.lossyScale.y);
		Gizmos.color = new Color(1f, 0f, 0.5f, 1f);
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, vector4);
		Gizmos.DrawLine(vector4, vector);
	}

	public static UIScrollList Create(string name, Vector3 pos)
	{
		return (UIScrollList)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIScrollList));
	}

	public static UIScrollList Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIScrollList)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIScrollList));
	}

	public void SetLocationZ(float z)
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, z);
	}

	public void SetVectorLocation(float x, float y, float z)
	{
		float x2 = this.viewableArea.x / 2f + x;
		float y2 = -(this.viewableArea.y / 2f + y);
		base.transform.localPosition = new Vector3(x2, y2, z);
		this.SetScrollBar(x, y);
	}

	public Vector3 GetVectorLocation()
	{
		return base.transform.localPosition;
	}

	public void SetLocation(float x, float y, float z)
	{
		float x2 = this.viewableArea.x / 2f + x;
		float y2 = -(this.viewableArea.y / 2f + y);
		this.position.x = x2;
		this.position.y = y2;
		this.position.z = z;
		base.transform.localPosition = this.position;
		this.SetScrollBar(x, -y);
	}

	public void SetLocation(float x, float y)
	{
		float x2 = this.viewableArea.x / 2f + x;
		float y2 = -(this.viewableArea.y / 2f + y);
		this.position.x = x2;
		this.position.y = y2;
		this.position.z = base.transform.localPosition.z;
		base.transform.localPosition = this.position;
		this.SetScrollBar(x, -y);
	}

	public void SetLocation(int x, int y)
	{
		float x2 = this.viewableArea.x / 2f + (float)x;
		float y2 = -(this.viewableArea.y / 2f + (float)y);
		this.position.x = x2;
		this.position.y = y2;
		this.position.z = base.transform.localPosition.z;
		base.transform.localPosition = this.position;
		this.SetScrollBar((float)x, (float)(-(float)y));
	}

	public Vector3 GetLocation()
	{
		return base.transform.localPosition;
	}

	public float GetLocationX()
	{
		return base.transform.localPosition.x;
	}

	public float GetLocationY()
	{
		return -base.transform.localPosition.y;
	}

	public Vector3 GetRealLocation()
	{
		Vector3 zero = Vector3.zero;
		zero.x = base.transform.localPosition.x - this.viewableArea.x / 2f;
		zero.y = -(base.transform.localPosition.y + this.viewableArea.y / 2f);
		zero.z = base.transform.localPosition.z;
		return zero;
	}

	public void SetSize(float x, float y)
	{
		this.viewableArea.x = x;
		this.viewableArea.y = y;
		this.changeScrollPos = false;
		if (this.slider)
		{
			this.slider.Visible = false;
		}
		this.CalcClippingRect();
		this.RepositionItems();
		this.ScrollPosition = 1f;
		this.SetScrollBar(x, -y);
		BoxCollider boxCollider = base.transform.GetComponent(typeof(BoxCollider)) as BoxCollider;
		if (boxCollider != null)
		{
			boxCollider.size = new Vector3(x, y, 0f);
			boxCollider.center = new Vector3(0f, 0f, 0f);
		}
	}

	public Vector2 GetSize()
	{
		return this.viewableArea;
	}

	public void SetScrollBar(float x, float y)
	{
		if (this.slider == null)
		{
			return;
		}
		float width;
		if (this.orientation == UIScrollList.ORIENTATION.VERTICAL)
		{
			width = this.viewableArea.y;
		}
		else
		{
			width = this.viewableArea.x;
		}
		this.slider.SetSize(width, this.slider.height);
		if (this.sliderposition == UIScrollList.SLIDERPOSITION.RIGHT)
		{
			this.slider.transform.localPosition = new Vector3(x + this.viewableArea.x - 20f - 6f, y, base.transform.localPosition.z - 0.02f);
		}
		else if (this.sliderposition == UIScrollList.SLIDERPOSITION.RIGHTOUT)
		{
			this.slider.transform.localPosition = new Vector3(x + this.viewableArea.x + 20f - 6f, y, base.transform.localPosition.z - 0.02f);
		}
		else
		{
			this.slider.transform.localPosition = new Vector3(x + 20f - 6f, y, base.transform.localPosition.z - 0.02f);
		}
	}

	public void ShowSlider(bool show, UIScrollList.SLIDERPOSITION postion)
	{
		this.sliderposition = postion;
	}

	public void AddSliderDelegate()
	{
		if (this.slider)
		{
			this.slider.Start();
			this.slider.AddValueChangedDelegate(new EZValueChangedDelegate(this.SliderMoved));
			this.slider.SetList(this);
		}
	}

	public void SetAlpha(float _alpha)
	{
		if (this.items.Count <= 0)
		{
			TsLog.LogWarning("KYT : List Index Error", new object[0]);
			return;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			for (int j = 0; j < this.items[i].transform.childCount; j++)
			{
				Transform child = this.items[i].transform.GetChild(j);
				if (null != child)
				{
					AutoSpriteControlBase component = child.GetComponent<AutoSpriteControlBase>();
					if (null != component)
					{
						component.SetAlpha(_alpha);
					}
				}
			}
		}
	}

	public void SetAlphaNoBG(float _alpha)
	{
		if (this.items.Count <= 0)
		{
			TsLog.LogWarning("KYT : List Index Error", new object[0]);
			return;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			for (int j = 0; j < this.items[i].transform.childCount; j++)
			{
				Transform child = this.items[i].transform.GetChild(j);
				if (null != child)
				{
					AutoSpriteControlBase component = child.GetComponent<AutoSpriteControlBase>();
					if (null != component && component.name != "back")
					{
						component.SetAlpha(_alpha);
					}
				}
			}
		}
	}

	public void SetAlphaColum(int index, float _alpha)
	{
		if (this.items.Count <= 0 || (index < 0 && index > this.items.Count))
		{
			TsLog.LogWarning("KYT : List Index Error", new object[0]);
			return;
		}
		for (int i = 0; i < this.items[index].transform.childCount; i++)
		{
			Transform child = this.items[index].transform.GetChild(i);
			if (null != child)
			{
				AutoSpriteControlBase component = child.GetComponent<AutoSpriteControlBase>();
				if (null != component)
				{
					component.SetAlpha(_alpha);
				}
			}
		}
	}

	public void SetAlphaColumNoBG(int index, float _alpha)
	{
		if (this.items.Count <= 0 || (index < 0 && index > this.items.Count))
		{
			TsLog.LogWarning("KYT : List Index Error", new object[0]);
			return;
		}
		for (int i = 0; i < this.items[index].transform.childCount; i++)
		{
			Transform child = this.items[index].transform.GetChild(i);
			if (null != child)
			{
				AutoSpriteControlBase component = child.GetComponent<AutoSpriteControlBase>();
				if (null != component && component.name != "back")
				{
					component.SetAlpha(_alpha);
				}
			}
		}
	}

	public void SetAlphaTarget(int index, int target, float _alpha)
	{
		if (this.items.Count <= 0 || (index < 0 && index > this.items.Count))
		{
			TsLog.LogWarning("KYT : List Index Error", new object[0]);
			return;
		}
		Transform child = this.items[index].transform.GetChild(target + 1);
		if (null != child)
		{
			AutoSpriteControlBase component = child.GetComponent<AutoSpriteControlBase>();
			if (null != component && component.name != "back")
			{
				component.SetAlpha(_alpha);
			}
		}
	}

	virtual GameObject get_gameObject()
	{
		return base.gameObject;
	}

	virtual Transform get_transform()
	{
		return base.transform;
	}

	virtual string get_name()
	{
		return base.name;
	}
}
