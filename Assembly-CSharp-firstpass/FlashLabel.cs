using GameMessage;
using System;
using UnityEngine;

public class FlashLabel : UIListItemContainer
{
	public float width;

	public float realHight;

	public float height;

	private Vector3 tempLocation = default(Vector3);

	public float lineSpacing = 1.1f;

	public SpriteText.Anchor_Pos anchor;

	public SpriteText.Alignment_Type alignment;

	private string szColor = string.Empty;

	private float fAddAutoAplha;

	private float fTotalApha;

	private bool bAutoAlpha;

	public float Height
	{
		get
		{
			return this.realHight;
		}
	}

	public float AddAutoAlpha
	{
		set
		{
			this.fAddAutoAplha = value;
		}
	}

	public bool AutoAplha
	{
		get
		{
			return this.bAutoAlpha;
		}
		set
		{
			this.bAutoAlpha = value;
		}
	}

	public float LineSpacing
	{
		get
		{
			return this.lineSpacing;
		}
		set
		{
			this.lineSpacing = value;
		}
	}

	public SpriteText.Anchor_Pos Anchor
	{
		get
		{
			return this.anchor;
		}
		set
		{
			this.anchor = value;
		}
	}

	public SpriteText.Alignment_Type Alignment
	{
		get
		{
			return this.alignment;
		}
		set
		{
			this.alignment = value;
		}
	}

	public override EZTransitionList[] Transitions
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public override string[] States
	{
		get
		{
			return null;
		}
	}

	public string FontColor
	{
		get
		{
			return this.szColor;
		}
		set
		{
			this.szColor = value;
		}
	}

	public bool UpdateAutoAplha()
	{
		if (!this.bAutoAlpha)
		{
			return false;
		}
		if (this.fTotalApha > 1f)
		{
			return false;
		}
		this.fTotalApha += this.fAddAutoAplha;
		base.SetAlpha(this.fTotalApha);
		return true;
	}

	public override void OnInput(POINTER_INFO ptr)
	{
	}

	public void SetLocation(float x, float y, float z)
	{
		this.tempLocation.x = x;
		this.tempLocation.y = -y;
		this.tempLocation.z = z;
		base.transform.localPosition = this.tempLocation;
	}

	public void SetLocation(int x, int y, int z)
	{
		this.tempLocation.x = (float)x;
		this.tempLocation.y = (float)(-(float)y);
		this.tempLocation.z = (float)z;
		base.transform.localPosition = this.tempLocation;
	}

	public void SetLocation(float x, float y)
	{
		this.tempLocation.x = x;
		this.tempLocation.y = -y;
		this.tempLocation.z = base.transform.localPosition.z;
		base.transform.localPosition = this.tempLocation;
	}

	public void SetLocation(int x, int y)
	{
		this.tempLocation.x = (float)x;
		this.tempLocation.y = (float)(-(float)y);
		this.tempLocation.z = base.transform.localPosition.z;
		base.transform.localPosition = this.tempLocation;
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

	public bool SetFlashLabel(string str)
	{
		if (this == null)
		{
			TsLog.LogWarning("UIListItemContainer NULL", new object[0]);
			return false;
		}
		this.DeleteListItemContainer();
		bool result = MsgHandler.Handle("ParseEmoticonFlashLabel", new object[]
		{
			this,
			str,
			this.width,
			this.height,
			(int)base.FontSize,
			this.lineSpacing,
			this.anchor,
			this.FontColor
		});
		if (this.bAutoAlpha)
		{
			this.fTotalApha = 0f;
			base.SetAlpha(this.fTotalApha);
		}
		this.FindOuterEdges();
		this.realHight = Mathf.Abs(this.BottomRightEdge().y - this.TopLeftEdge().y);
		return result;
	}

	public void ClearList()
	{
		base.DeleteListItemContainer();
	}

	public static FlashLabel Create(string name, Vector3 pos)
	{
		return (FlashLabel)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(FlashLabel));
	}

	public static FlashLabel Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (FlashLabel)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(FlashLabel));
	}

	public override EZTransitionList GetTransitions(int index)
	{
		return null;
	}
}
