using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("EZ GUI/Utility/EZ Screen Placement"), ExecuteInEditMode]
[Serializable]
public class EZScreenPlacement : MonoBehaviour, IUseCamera
{
	public enum HORIZONTAL_ALIGN
	{
		NONE,
		SCREEN_LEFT,
		SCREEN_RIGHT,
		SCREEN_CENTER,
		OBJECT
	}

	public enum VERTICAL_ALIGN
	{
		NONE,
		SCREEN_TOP,
		SCREEN_BOTTOM,
		SCREEN_CENTER,
		OBJECT
	}

	[Serializable]
	public class RelativeTo
	{
		public EZScreenPlacement.HORIZONTAL_ALIGN horizontal = EZScreenPlacement.HORIZONTAL_ALIGN.SCREEN_LEFT;

		public EZScreenPlacement.VERTICAL_ALIGN vertical = EZScreenPlacement.VERTICAL_ALIGN.SCREEN_TOP;

		protected EZScreenPlacement script;

		public EZScreenPlacement Script
		{
			get
			{
				return this.script;
			}
			set
			{
				this.Script = value;
			}
		}

		public RelativeTo(EZScreenPlacement sp, EZScreenPlacement.RelativeTo rt)
		{
			this.script = sp;
			this.Copy(rt);
		}

		public RelativeTo(EZScreenPlacement sp)
		{
			this.script = sp;
		}

		public bool Equals(EZScreenPlacement.RelativeTo rt)
		{
			return rt != null && this.horizontal == rt.horizontal && this.vertical == rt.vertical;
		}

		public void Copy(EZScreenPlacement.RelativeTo rt)
		{
			if (rt == null)
			{
				return;
			}
			this.horizontal = rt.horizontal;
			this.vertical = rt.vertical;
		}
	}

	public Camera renderCamera;

	public Vector3 screenPos = Vector3.forward;

	public EZScreenPlacement.RelativeTo relativeTo;

	public Transform relativeObject;

	public bool alwaysRecursive = true;

	protected Vector2 screenSize;

	protected EZScreenPlacementMirror mirror = new EZScreenPlacementMirror();

	protected bool m_awake;

	protected bool m_started;

	public Camera RenderCamera
	{
		get
		{
			return this.renderCamera;
		}
		set
		{
			this.SetCamera(value);
		}
	}

	private void Awake()
	{
		if (this.m_awake)
		{
			return;
		}
		this.m_awake = true;
		IUseCamera useCamera = (IUseCamera)base.GetComponent("IUseCamera");
		if (useCamera != null)
		{
			this.renderCamera = useCamera.RenderCamera;
		}
		if (this.renderCamera == null)
		{
			this.renderCamera = Camera.main;
		}
		if (this.relativeTo == null)
		{
			this.relativeTo = new EZScreenPlacement.RelativeTo(this);
		}
		else if (this.relativeTo.Script != this)
		{
			EZScreenPlacement.RelativeTo relativeTo = new EZScreenPlacement.RelativeTo(this, this.relativeTo);
			this.relativeTo = relativeTo;
		}
	}

	public void Start()
	{
		if (this.m_started)
		{
			return;
		}
		this.m_started = true;
		if (this.renderCamera != null)
		{
			this.screenSize.x = this.renderCamera.pixelWidth;
			this.screenSize.y = this.renderCamera.pixelHeight;
		}
		this.PositionOnScreenRecursively();
	}

	public void PositionOnScreenRecursively()
	{
		if (!this.m_started)
		{
			this.Start();
		}
		if (this.relativeObject != null)
		{
			EZScreenPlacement eZScreenPlacement = this.relativeObject.GetComponent(typeof(EZScreenPlacement)) as EZScreenPlacement;
			if (eZScreenPlacement != null)
			{
				eZScreenPlacement.PositionOnScreenRecursively();
			}
		}
		this.PositionOnScreen();
	}

	public Vector3 ScreenPosToLocalPos(Vector3 screenPos)
	{
		return base.transform.InverseTransformPoint(this.ScreenPosToWorldPos(screenPos));
	}

	public Vector3 ScreenPosToParentPos(Vector3 screenPos)
	{
		return this.ScreenPosToLocalPos(screenPos) + base.transform.localPosition;
	}

	public Vector3 ScreenPosToWorldPos(Vector3 screenPos)
	{
		if (!this.m_started)
		{
			this.Start();
		}
		if (this.renderCamera == null)
		{
			TsLog.LogError("Render camera not yet assigned to EZScreenPlacement component of \"" + base.name + "\" when attempting to call PositionOnScreen()", new object[0]);
			return base.transform.position;
		}
		Vector3 vector = this.renderCamera.WorldToScreenPoint(base.transform.position);
		Vector3 position = screenPos;
		switch (this.relativeTo.horizontal)
		{
		case EZScreenPlacement.HORIZONTAL_ALIGN.NONE:
			position.x = vector.x;
			break;
		case EZScreenPlacement.HORIZONTAL_ALIGN.SCREEN_RIGHT:
			position.x = this.screenSize.x + position.x;
			break;
		case EZScreenPlacement.HORIZONTAL_ALIGN.SCREEN_CENTER:
			position.x = this.screenSize.x * 0.5f + position.x;
			break;
		case EZScreenPlacement.HORIZONTAL_ALIGN.OBJECT:
			if (this.relativeObject != null)
			{
				position.x = this.renderCamera.WorldToScreenPoint(this.relativeObject.position).x + position.x;
			}
			else
			{
				position.x = vector.x;
			}
			break;
		}
		switch (this.relativeTo.vertical)
		{
		case EZScreenPlacement.VERTICAL_ALIGN.NONE:
			position.y = vector.y;
			break;
		case EZScreenPlacement.VERTICAL_ALIGN.SCREEN_TOP:
			position.y = this.screenSize.y + position.y;
			break;
		case EZScreenPlacement.VERTICAL_ALIGN.SCREEN_CENTER:
			position.y = this.screenSize.y * 0.5f + position.y;
			break;
		case EZScreenPlacement.VERTICAL_ALIGN.OBJECT:
			if (this.relativeObject != null)
			{
				position.y = this.renderCamera.WorldToScreenPoint(this.relativeObject.position).y + position.y;
			}
			else
			{
				position.y = vector.y;
			}
			break;
		}
		return this.renderCamera.ScreenToWorldPoint(position);
	}

	public void PositionOnScreen()
	{
		if (!this.m_awake)
		{
			return;
		}
		base.transform.position = this.ScreenPosToWorldPos(this.screenPos);
		base.SendMessage("OnReposition", SendMessageOptions.DontRequireReceiver);
	}

	public void PositionOnScreen(int x, int y, float depth)
	{
		this.PositionOnScreen(new Vector3((float)x, (float)y, depth));
	}

	public void PositionOnScreen(Vector3 pos)
	{
		this.screenPos = pos;
		this.PositionOnScreen();
	}

	public void SetCamera()
	{
		this.SetCamera(this.renderCamera);
	}

	public void SetCamera(Camera c)
	{
		if (c == null)
		{
			return;
		}
		this.renderCamera = c;
		this.screenSize.x = this.renderCamera.pixelWidth;
		this.screenSize.y = this.renderCamera.pixelHeight;
		if (this.alwaysRecursive || (Application.isEditor && !Application.isPlaying))
		{
			this.PositionOnScreenRecursively();
		}
		else
		{
			this.PositionOnScreen();
		}
	}

	public void WorldToScreenPos(Vector3 worldPos)
	{
		if (this.renderCamera == null)
		{
			return;
		}
		Vector3 vector = this.renderCamera.WorldToScreenPoint(worldPos);
		switch (this.relativeTo.horizontal)
		{
		case EZScreenPlacement.HORIZONTAL_ALIGN.SCREEN_LEFT:
			this.screenPos.x = vector.x;
			break;
		case EZScreenPlacement.HORIZONTAL_ALIGN.SCREEN_RIGHT:
			this.screenPos.x = vector.x - this.renderCamera.pixelWidth;
			break;
		case EZScreenPlacement.HORIZONTAL_ALIGN.SCREEN_CENTER:
			this.screenPos.x = vector.x - this.renderCamera.pixelWidth / 2f;
			break;
		case EZScreenPlacement.HORIZONTAL_ALIGN.OBJECT:
			if (this.relativeObject != null)
			{
				Vector3 vector2 = this.renderCamera.WorldToScreenPoint(this.relativeObject.transform.position);
				this.screenPos.x = vector.x - vector2.x;
			}
			break;
		}
		switch (this.relativeTo.vertical)
		{
		case EZScreenPlacement.VERTICAL_ALIGN.SCREEN_TOP:
			this.screenPos.y = vector.y - this.renderCamera.pixelHeight;
			break;
		case EZScreenPlacement.VERTICAL_ALIGN.SCREEN_BOTTOM:
			this.screenPos.y = vector.y;
			break;
		case EZScreenPlacement.VERTICAL_ALIGN.SCREEN_CENTER:
			this.screenPos.y = vector.y - this.renderCamera.pixelHeight / 2f;
			break;
		case EZScreenPlacement.VERTICAL_ALIGN.OBJECT:
			if (this.relativeObject != null)
			{
				Vector3 vector3 = this.renderCamera.WorldToScreenPoint(this.relativeObject.transform.position);
				this.screenPos.y = vector.y - vector3.y;
			}
			break;
		}
		this.screenPos.z = vector.z;
		if (this.alwaysRecursive)
		{
			this.PositionOnScreenRecursively();
		}
		else
		{
			this.PositionOnScreen();
		}
	}

	public static bool TestDepenency(EZScreenPlacement sp)
	{
		if (sp.relativeObject == null)
		{
			return true;
		}
		List<EZScreenPlacement> list = new List<EZScreenPlacement>();
		list.Add(sp);
		EZScreenPlacement eZScreenPlacement = sp.relativeObject.GetComponent(typeof(EZScreenPlacement)) as EZScreenPlacement;
		while (eZScreenPlacement != null)
		{
			if (list.Contains(eZScreenPlacement))
			{
				return false;
			}
			list.Add(eZScreenPlacement);
			if (eZScreenPlacement.relativeObject == null)
			{
				return true;
			}
			eZScreenPlacement = (eZScreenPlacement.relativeObject.GetComponent(typeof(EZScreenPlacement)) as EZScreenPlacement);
		}
		return true;
	}

	public virtual void DoMirror()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.mirror == null)
		{
			this.mirror = new EZScreenPlacementMirror();
			this.mirror.Mirror(this);
		}
		this.mirror.Validate(this);
		if (this.mirror.DidChange(this))
		{
			this.SetCamera(this.renderCamera);
			this.mirror.Mirror(this);
		}
	}

	public virtual void OnDrawGizmosSelected()
	{
		this.DoMirror();
	}

	public virtual void OnDrawGizmos()
	{
		this.DoMirror();
	}
}
