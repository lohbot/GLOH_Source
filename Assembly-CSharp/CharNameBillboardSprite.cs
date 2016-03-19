using System;
using UnityEngine;

public class CharNameBillboardSprite : MonoBehaviour
{
	private bool bReadyData;

	private bool bRenderEnabled;

	private bool bRenderTarget;

	private bool bRenderText;

	private bool bRenderPlane;

	private bool bReadyShow;

	private bool bRenderRank;

	private float fCreateTime;

	public void Init()
	{
		this.bReadyData = false;
		this.bRenderEnabled = false;
		this.bRenderTarget = false;
		this.bRenderText = false;
		this.bRenderPlane = false;
		this.bReadyShow = false;
		this.bRenderRank = false;
	}

	public float Get_Bound_Size()
	{
		Transform transform = base.transform.FindChild("NameText");
		if (transform != null)
		{
			SpriteText component = transform.GetComponent<SpriteText>();
			if (component != null)
			{
				return component.GetTextWidth() / 2f;
			}
		}
		return 0f;
	}

	public Vector3 Get_Text_Scale()
	{
		Vector3 localScale = new Vector3(0.2f, 0.2f, 1f);
		Transform transform = base.transform.FindChild("NameText");
		if (transform != null)
		{
			SpriteText component = transform.GetComponent<SpriteText>();
			if (component != null)
			{
				localScale = component.transform.localScale;
			}
		}
		return localScale;
	}

	public float Get_Plane_Height()
	{
		float result = 0f;
		Transform transform = base.transform.FindChild("NamePlane");
		if (transform != null)
		{
			SimpleSprite component = transform.GetComponent<SimpleSprite>();
			if (component != null)
			{
				Transform transform2 = base.transform.FindChild("NameText");
				if (transform2 != null)
				{
					SpriteText component2 = transform2.GetComponent<SpriteText>();
					if (component2)
					{
						result = Math.Abs(component2.BottomRight.y - component2.TopLeft.y);
					}
				}
			}
		}
		return result;
	}

	public float Get_Plane_Width()
	{
		float result = 0f;
		Transform transform = base.transform.FindChild("NamePlane");
		if (transform != null)
		{
			SimpleSprite component = transform.GetComponent<SimpleSprite>();
			if (component != null)
			{
				Transform transform2 = base.transform.FindChild("NameText");
				if (transform2 != null)
				{
					SpriteText component2 = transform2.GetComponent<SpriteText>();
					if (component2)
					{
						result = Math.Abs(component2.BottomRight.x - component2.TopLeft.x);
					}
				}
			}
		}
		return result;
	}

	public Vector3 GetPosition()
	{
		return base.transform.position;
	}

	public void SetPosition(Vector3 pos)
	{
		base.transform.position = pos;
	}

	public void AlignCenter()
	{
		Transform transform = base.transform.FindChild("NameText");
		if (transform != null)
		{
			MeshRenderer component = transform.GetComponent<MeshRenderer>();
			if (component)
			{
				base.transform.position = component.bounds.center;
			}
		}
	}

	public void SetText(string text)
	{
		Transform transform = base.transform.FindChild("NameText");
		if (transform != null)
		{
			SpriteText component = transform.GetComponent<SpriteText>();
			if (component != null)
			{
				component.SetFont(NrTSingleton<UIManager>.Instance.defaultFontMaterial);
				component.Text = text;
			}
		}
		if (text.Length > 0)
		{
			this.fCreateTime = Time.time;
			this.bReadyData = true;
		}
		else
		{
			this.fCreateTime = 0f;
			this.bReadyData = false;
		}
	}

	public void SetTextSize(float textsize)
	{
		Transform transform = base.transform.FindChild("NameText");
		if (transform != null)
		{
			SpriteText component = transform.GetComponent<SpriteText>();
			if (component != null)
			{
				component.characterSize = textsize;
			}
		}
	}

	public void SetPlaneKey(string imageKey)
	{
		Transform transform = base.transform.FindChild("NamePlane");
		if (transform != null)
		{
			SimpleSprite component = transform.GetComponent<SimpleSprite>();
			if (component != null)
			{
				float num = 1.5f;
				float num2 = 8f;
				Transform transform2 = base.transform.FindChild("NameText");
				if (transform2 != null)
				{
					SpriteText component2 = transform2.GetComponent<SpriteText>();
					if (component2)
					{
						float num3 = Math.Abs(component2.BottomRight.x - component2.TopLeft.x);
						float num4 = Math.Abs(component2.BottomRight.y - component2.TopLeft.y);
						component.Setup(num3 + num2 * 2f, num4 * num, imageKey);
						component.Hide(false);
						return;
					}
				}
				component.Hide(true);
			}
		}
	}

	private void Update()
	{
		if (this.bRenderEnabled != this.bRenderTarget)
		{
			if (this.fCreateTime > 0f && Time.time - this.fCreateTime >= 2f)
			{
				this.bReadyShow = true;
			}
			if (this.bReadyShow)
			{
				this.SetCurrentShowHide(this.bRenderTarget);
			}
		}
	}

	public void ShowTextAndPlane(bool bShowText, bool bShowPlane, bool bShowMark)
	{
		float num = 1f;
		float num2 = 1f;
		if (!this.bReadyData)
		{
			bShowText = false;
			bShowPlane = false;
			bShowMark = false;
		}
		Transform transform = base.transform.FindChild("NameText");
		if (transform != null)
		{
			MeshRenderer component = transform.GetComponent<MeshRenderer>();
			if (component)
			{
				component.enabled = false;
			}
			num = transform.localScale.x;
			num2 = transform.localScale.y;
			this.bRenderText = bShowText;
		}
		Transform transform2 = base.transform.FindChild("NamePlane");
		if (transform2 != null)
		{
			MeshRenderer component2 = transform2.GetComponent<MeshRenderer>();
			if (component2)
			{
				component2.enabled = false;
			}
			if (bShowPlane)
			{
				transform2.localScale = new Vector3(transform2.localScale.x * num, transform2.localScale.y * num2, transform2.localScale.z);
			}
			this.bRenderPlane = bShowPlane;
		}
		Transform transform3 = base.transform.FindChild("CharRank");
		if (transform3 != null)
		{
			MeshRenderer component3 = transform3.GetComponent<MeshRenderer>();
			if (component3)
			{
				component3.enabled = false;
			}
			if (this.bRenderRank)
			{
				transform3.localScale = new Vector3(transform3.localScale.x * num, transform3.localScale.y * num2, transform3.localScale.z);
			}
			this.bRenderRank = bShowMark;
		}
	}

	private void SetCurrentShowHide(bool bShow)
	{
		Transform transform = base.transform.FindChild("NameText");
		if (transform != null)
		{
			MeshRenderer component = transform.GetComponent<MeshRenderer>();
			if (component)
			{
				component.enabled = (bShow && this.bRenderText);
			}
		}
		Transform transform2 = base.transform.FindChild("NamePlane");
		if (transform2 != null)
		{
			MeshRenderer component2 = transform2.GetComponent<MeshRenderer>();
			if (component2)
			{
				component2.enabled = (bShow && this.bRenderPlane);
			}
		}
		Transform transform3 = base.transform.FindChild("CharRank");
		if (transform3 != null)
		{
			MeshRenderer component3 = transform3.GetComponent<MeshRenderer>();
			if (component3)
			{
				component3.enabled = (bShow && this.bRenderRank);
			}
		}
		this.bRenderEnabled = bShow;
	}

	public void SetShowHide(bool bShow)
	{
		this.bRenderTarget = bShow;
		if (!bShow)
		{
			this.SetCurrentShowHide(bShow);
		}
	}
}
