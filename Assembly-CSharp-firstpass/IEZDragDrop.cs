using System;
using UnityEngine;

public interface IEZDragDrop
{
	object Data
	{
		get;
		set;
	}

	bool IsDraggable
	{
		get;
		set;
	}

	GameObject DropTarget
	{
		get;
		set;
	}

	bool DropHandled
	{
		get;
		set;
	}

	float DragOffset
	{
		get;
		set;
	}

	EZAnimation.EASING_TYPE CancelDragEasing
	{
		get;
		set;
	}

	float CancelDragDuration
	{
		get;
		set;
	}

	float MouseOffset
	{
		get;
		set;
	}

	bool IsDragging();

	void SetDragging(bool value);

	bool DragUpdatePosition(POINTER_INFO ptr);

	void CancelDrag();

	void OnEZDragDrop(EZDragDropParams parms);

	void AddDragDropDelegate(EZDragDropDelegate del);

	void RemoveDragDropDelegate(EZDragDropDelegate del);

	void SetDragDropDelegate(EZDragDropDelegate del);
}
