using System;

public struct EZDragDropParams
{
	public EZDragDropEvent evt;

	public IUIObject dragObj;

	public POINTER_INFO ptr;

	public EZDragDropParams(EZDragDropEvent e, IUIObject obj, POINTER_INFO p)
	{
		this.evt = e;
		this.dragObj = obj;
		this.ptr = p;
	}
}
