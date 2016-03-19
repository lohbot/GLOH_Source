using System;
using UnityEngine;

public class UIScrollKnob : UIButton
{
	protected Vector3 origPos;

	protected UISlider slider;

	protected float maxScrollPos;

	protected Plane ctrlPlane;

	protected Vector2 colliderSizeFactor;

	protected float colliderExtent;

	private float dist;

	private Vector3 inputPoint;

	private Vector3 newPos;

	private Vector3 prevPoint;

	private bool clickKnob;

	private float moveValue;

	public bool IsClickKnob()
	{
		return this.clickKnob;
	}

	protected override void Awake()
	{
		base.Awake();
		base.EffectAni = false;
		this.origPos = base.transform.localPosition;
	}

	public override void OnInput(ref POINTER_INFO ptr)
	{
		base.OnInput(ref ptr);
		if (!this.m_controlIsEnabled)
		{
			return;
		}
		POINTER_INFO.INPUT_EVENT evt = ptr.evt;
		switch (evt)
		{
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			if (null != this.slider.list)
			{
				this.slider.list.overList = false;
			}
			this.clickKnob = false;
			this.moveValue = 0f;
			return;
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			IL_33:
			if (evt != POINTER_INFO.INPUT_EVENT.PRESS)
			{
				this.clickKnob = false;
				return;
			}
			this.prevPoint = this.GetLocalInputPoint(ptr.ray);
			this.clickKnob = true;
			this.moveValue = 0f;
			return;
		case POINTER_INFO.INPUT_EVENT.DRAG:
		{
			if (!this.slider.useknobButton)
			{
				return;
			}
			this.inputPoint = this.GetLocalInputPoint(ptr.ray);
			float scrollPos = this.GetScrollPos();
			this.dist = this.inputPoint.x - this.prevPoint.x;
			this.prevPoint = this.inputPoint;
			this.newPos = base.transform.localPosition;
			this.newPos.x = Mathf.Clamp(this.newPos.x + this.dist, this.origPos.x, this.origPos.x + this.maxScrollPos);
			base.transform.localPosition = this.newPos;
			this.prevPoint.x = Mathf.Clamp(this.prevPoint.x, this.origPos.x - this.colliderExtent, this.origPos.x + this.colliderExtent + this.maxScrollPos);
			this.moveValue = Mathf.Abs(this.GetScrollPos() - scrollPos);
			float num = this.maxScrollPos / this.slider.totalLineCount;
			this.moveValue *= num;
			if (this.moveValue > 1f)
			{
				this.moveValue = 1f;
			}
			if (this.dist > 0f)
			{
				this.slider.clickButton = false;
			}
			else
			{
				this.slider.clickButton = true;
			}
			this.slider.ScrollKnobMoved(this, this.GetScrollPos());
			this.clickKnob = true;
			return;
		}
		case POINTER_INFO.INPUT_EVENT.MOUSE_WHEEL:
			if (null != this.slider.list)
			{
				this.slider.list.overList = true;
			}
			return;
		}
		goto IL_33;
	}

	public void SetStartPos(Vector3 startPos)
	{
		this.origPos = startPos;
	}

	protected Vector3 GetLocalInputPoint(Ray ray)
	{
		this.ctrlPlane.SetNormalAndPosition(base.transform.forward * -1f, base.transform.position);
		this.ctrlPlane.Raycast(ray, out this.dist);
		return base.transform.parent.InverseTransformPoint(ray.origin + ray.direction * this.dist);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);
		if (!(s is UIScrollKnob))
		{
			return;
		}
		UIScrollKnob uIScrollKnob = (UIScrollKnob)s;
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			this.origPos = uIScrollKnob.origPos;
			this.ctrlPlane = uIScrollKnob.ctrlPlane;
			this.slider = uIScrollKnob.slider;
		}
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.maxScrollPos = uIScrollKnob.maxScrollPos;
			this.colliderSizeFactor = uIScrollKnob.colliderSizeFactor;
		}
	}

	public void SetColliderSizeFactor(Vector2 csf)
	{
		this.colliderSizeFactor = csf;
	}

	public override void UpdateCollider()
	{
		base.UpdateCollider();
		if (!(base.collider is BoxCollider) || base.IsHidden())
		{
			return;
		}
		BoxCollider boxCollider = (BoxCollider)base.collider;
		boxCollider.size = new Vector3(boxCollider.size.x * this.colliderSizeFactor.x, boxCollider.size.y * this.colliderSizeFactor.y, 0.001f);
		this.colliderExtent = boxCollider.size.x * 0.5f;
	}

	public float GetScrollPos()
	{
		return (base.transform.localPosition.x - this.origPos.x) / this.maxScrollPos;
	}

	public void SetPosition(float pos)
	{
		base.transform.localPosition = this.origPos + Vector3.right * this.maxScrollPos * pos;
	}

	public void SetSlider(UISlider s)
	{
		this.slider = s;
	}

	public UISlider GetSlider()
	{
		return this.slider;
	}

	public void SetMaxScroll(float max)
	{
		this.maxScrollPos = max;
	}

	public void SetupAppearance()
	{
		this.Start();
		this.InitUVs();
		this.UpdateUVs();
	}

	public new static UIScrollKnob Create(string name, Vector3 pos)
	{
		return (UIScrollKnob)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIScrollKnob));
	}

	public new static UIScrollKnob Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIScrollKnob)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIScrollKnob));
	}

	public void UpButtonScroll(float count)
	{
		if (count == 0f)
		{
			return;
		}
		float num = this.maxScrollPos / count;
		this.newPos = base.transform.localPosition;
		this.newPos.x = Mathf.Clamp(this.newPos.x - num, this.origPos.x, this.origPos.x + this.maxScrollPos);
		base.transform.localPosition = this.newPos;
		this.slider.clickButton = true;
		this.slider.ScrollKnobMoved(this, this.GetScrollPos());
	}

	public void DownButtonScroll(float count)
	{
		if (count == 0f)
		{
			return;
		}
		float num = this.maxScrollPos / count;
		this.newPos = base.transform.localPosition;
		this.newPos.x = Mathf.Clamp(this.newPos.x + num, this.origPos.x, this.origPos.x + this.maxScrollPos);
		base.transform.localPosition = this.newPos;
		this.slider.clickButton = false;
		this.slider.ScrollKnobMoved(this, this.GetScrollPos());
	}
}
