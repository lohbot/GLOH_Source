using System;
using UnityEngine;

public interface IUIListObject : IEZDragDrop, IUIObject
{
	bool Managed
	{
		get;
	}

	string Text
	{
		get;
		set;
	}

	SpriteText TextObj
	{
		get;
	}

	bool IsContainer();

	void FindOuterEdges();

	Vector2 TopLeftEdge();

	Vector2 BottomRightEdge();

	void Hide(bool tf);

	Rect3D GetClippingRect();

	void SetClippingRect(Rect3D value);

	bool IsClipped();

	void SetClipped(bool value);

	void Unclip();

	void UpdateCollider();

	void SetList(UIScrollList c);

	int GetIndex();

	void SetIndex(int value);

	bool IsSelected();

	void SetSelected(bool value);

	bool IsLocked();

	void SetLocked(bool value);

	void Delete();

	void DeleteAnim();
}
