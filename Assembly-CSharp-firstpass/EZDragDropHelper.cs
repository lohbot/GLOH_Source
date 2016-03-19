using System;
using UnityEngine;

public class EZDragDropHelper
{
	public IUIObject host;

	private Vector3 touchCompensationOffset = Vector3.zero;

	protected Vector3 dragOrigin;

	protected Vector3 dragOriginOffset;

	protected Plane dragPlane;

	protected bool isDragging;

	protected GameObject dropTarget;

	protected bool dropHandled;

	protected EZDragDropDelegate dragDropDelegate;

	private Plane DragPlane
	{
		get
		{
			return this.dragPlane;
		}
	}

	public GameObject DropTarget
	{
		get
		{
			return this.dropTarget;
		}
		set
		{
			if (value == this.host.gameObject)
			{
				return;
			}
			if (this.dropTarget != value)
			{
				if (this.dropTarget != null)
				{
					this.dropTarget.SendMessage("OnEZDragDrop", new EZDragDropParams(EZDragDropEvent.DragExit, this.host, default(POINTER_INFO)), SendMessageOptions.DontRequireReceiver);
				}
				if (value != null)
				{
					value.SendMessage("OnEZDragDrop", new EZDragDropParams(EZDragDropEvent.DragEnter, this.host, default(POINTER_INFO)), SendMessageOptions.DontRequireReceiver);
				}
				this.host.gameObject.SendMessage("OnEZDragDrop", new EZDragDropParams(EZDragDropEvent.DragExit, this.host, default(POINTER_INFO)), SendMessageOptions.DontRequireReceiver);
				this.dropTarget = value;
				this.host.gameObject.SendMessage("OnEZDragDrop", new EZDragDropParams(EZDragDropEvent.DragEnter, this.host, default(POINTER_INFO)), SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public bool DropHandled
	{
		get
		{
			return this.dropHandled;
		}
		set
		{
			this.dropHandled = value;
		}
	}

	public EZDragDropHelper(IUIObject h)
	{
		this.host = h;
	}

	public EZDragDropHelper()
	{
	}

	public bool IsDragging()
	{
		return this.isDragging;
	}

	public void SetDragging(bool value)
	{
		bool flag = this.isDragging;
		this.isDragging = value;
		if (flag)
		{
			this.CancelDrag();
		}
	}

	public bool DragUpdatePosition(POINTER_INFO ptr)
	{
		if (this == null)
		{
			return false;
		}
		float num;
		this.dragPlane.Raycast(ptr.ray, out num);
		this.host.transform.position = this.touchCompensationOffset + ptr.ray.origin + ptr.ray.direction * (num - this.host.DragOffset);
		return true;
	}

	public void CancelDrag()
	{
		if (this == null || !this.isDragging)
		{
			return;
		}
		EZDragDropParams eZDragDropParams = new EZDragDropParams(EZDragDropEvent.Cancelled, this.host, default(POINTER_INFO));
		if (this.dropTarget != null)
		{
			this.dropTarget.SendMessage("OnEZDragDrop", eZDragDropParams, SendMessageOptions.DontRequireReceiver);
		}
		if (this.dragDropDelegate != null)
		{
			this.dragDropDelegate(eZDragDropParams);
		}
		this.dropTarget = null;
		this.dropHandled = false;
		this.isDragging = false;
		AnimatePosition.Do(this.host.gameObject, EZAnimation.ANIM_MODE.To, this.dragOriginOffset, EZAnimation.GetInterpolator(this.host.CancelDragEasing), this.host.CancelDragDuration, 0f, null, new EZAnimation.CompletionDelegate(this.FinishCancelDrag));
		if (UIManager.Exists())
		{
			NrTSingleton<UIManager>.Instance.Detarget(this.host);
		}
	}

	protected void FinishCancelDrag(EZAnimation anim)
	{
		if (this == null)
		{
			return;
		}
		this.host.transform.localPosition = this.dragOrigin;
	}

	public void OnEZDragDrop(EZDragDropParams parms)
	{
		if (this == null)
		{
			return;
		}
		if (this.host == null)
		{
			return;
		}
		EZDragDropEvent evt = parms.evt;
		if (evt == EZDragDropEvent.Begin)
		{
			if (this.host.transform == null)
			{
				return;
			}
			Transform transform = this.host.transform;
			if (transform == null)
			{
				return;
			}
			this.dragOrigin = transform.localPosition;
			this.isDragging = true;
			this.dropHandled = false;
			this.dragPlane.SetNormalAndPosition(transform.TransformDirection(transform.forward * -1f), transform.position);
			Ray ray = parms.ptr.camera.ScreenPointToRay(parms.ptr.camera.WorldToScreenPoint(transform.position));
			float num;
			this.dragPlane.Raycast(ray, out num);
			this.dragOriginOffset = ray.origin + ray.direction * (num - this.host.DragOffset);
			if (transform.parent != null)
			{
				this.dragOriginOffset = transform.parent.InverseTransformPoint(this.dragOriginOffset);
			}
			this.touchCompensationOffset = new Vector3(-this.host.MouseOffset, this.host.MouseOffset, 0f);
		}
		if (this.dragDropDelegate != null)
		{
			this.dragDropDelegate(parms);
		}
		if (parms.evt == EZDragDropEvent.Dropped && parms.dragObj.Equals(this.host))
		{
			if (this.dropHandled)
			{
				this.isDragging = false;
				this.dropTarget = null;
			}
			else
			{
				this.CancelDrag();
			}
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
}
