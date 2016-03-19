using System;
using UnityEngine;

public class NkInputState
{
	public virtual IMECompositionMode imeCompositionMode
	{
		get
		{
			return Input.imeCompositionMode;
		}
		set
		{
			Input.imeCompositionMode = value;
		}
	}

	public virtual Vector3 mousePosition
	{
		get
		{
			return Input.mousePosition;
		}
	}

	public virtual string inputString
	{
		get
		{
			return Input.inputString;
		}
	}

	public virtual string compositionString
	{
		get
		{
			return Input.compositionString;
		}
	}

	public virtual int touchCount
	{
		get
		{
			return Input.touchCount;
		}
	}

	public virtual Touch[] touches
	{
		get
		{
			return Input.touches;
		}
	}

	public virtual bool anyKey
	{
		get
		{
			return Input.anyKey;
		}
	}

	public virtual bool anyKeyDown
	{
		get
		{
			return Input.anyKeyDown;
		}
	}

	public virtual float GetAxisRaw(string axisName)
	{
		return Input.GetAxisRaw(axisName);
	}

	public virtual float GetAxis(string axisName)
	{
		return Input.GetAxis(axisName);
	}

	public virtual bool GetButton(string buttonName)
	{
		return Input.GetButton(buttonName);
	}

	public virtual bool GetKeyDown(KeyCode eKey)
	{
		return TsPlatform.IsEditor && Input.GetKeyDown(eKey);
	}

	public virtual bool GetKeyUp(KeyCode eKey)
	{
		return TsPlatform.IsEditor && Input.GetKeyUp(eKey);
	}

	public virtual bool GetKey(KeyCode eKey)
	{
		return TsPlatform.IsEditor && Input.GetKey(eKey);
	}

	public virtual bool GetMouseButton(int button)
	{
		return Input.GetMouseButton(button);
	}

	public virtual bool GetMouseButtonDown(int button)
	{
		return Input.GetMouseButtonDown(button);
	}

	public virtual bool GetMouseButtonUp(int button)
	{
		return Input.GetMouseButtonUp(button);
	}

	public virtual Touch GetTouch(int index)
	{
		return Input.GetTouch(index);
	}
}
