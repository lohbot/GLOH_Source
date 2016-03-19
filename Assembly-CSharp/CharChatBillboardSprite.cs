using System;
using UnityEngine;
using UnityForms;

public class CharChatBillboardSprite : MonoBehaviour
{
	private bool bReadyData;

	private bool bPlaneScaleChanged;

	private bool bRenderEnabled;

	private bool bRenderTarget;

	private bool bRenderText;

	private bool bRenderPlane;

	private bool bReadyShow;

	private float fEmoticonScale = 0.06f;

	private float fBackImageScaleX = 0.06f;

	private float fBackImageScaleY = 0.06f;

	private UIListItemContainer emoticonContainer;

	private float fCreateTime;

	public UIListItemContainer EmoticonContainer
	{
		get
		{
			return this.emoticonContainer;
		}
	}

	public void Init()
	{
		this.bReadyData = false;
		this.bPlaneScaleChanged = false;
		this.bRenderEnabled = false;
		this.bRenderTarget = false;
		this.bRenderText = false;
		this.bRenderPlane = false;
		this.bReadyShow = false;
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
		Debug.Log("AlignCenter() = " + Time.time);
		Transform transform = base.transform.FindChild("ChatText");
		if (transform != null)
		{
			MeshRenderer component = transform.GetComponent<MeshRenderer>();
			if (component)
			{
				base.transform.position = component.bounds.center;
			}
		}
	}

	public void SetFontEffect(SpriteText.Font_Effect FEffect)
	{
		Transform transform = base.transform.FindChild("ChatText");
		if (transform != null)
		{
			this.emoticonContainer = transform.GetComponent<UIListItemContainer>();
			if (this.emoticonContainer == null)
			{
				this.emoticonContainer = transform.gameObject.AddComponent<UIListItemContainer>();
			}
			this.emoticonContainer.FontEffect = FEffect;
		}
	}

	public void SetText(string text)
	{
		Transform transform = base.transform.FindChild("ChatText");
		if (transform != null)
		{
			this.emoticonContainer = transform.GetComponent<UIListItemContainer>();
			if (this.emoticonContainer == null)
			{
				this.emoticonContainer = transform.gameObject.AddComponent<UIListItemContainer>();
			}
			this.emoticonContainer.Delete();
			float num = 0f;
			EmoticonInfo.ParseAdEmoticon(ref this.emoticonContainer, text, this.fEmoticonScale, ref num);
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
		Transform transform = base.transform.FindChild("ChatText");
		if (transform != null)
		{
			SpriteText component = transform.GetComponent<SpriteText>();
			if (component != null)
			{
				component.characterSize = textsize;
			}
		}
	}

	public void SetTextColor(Color color)
	{
		Transform transform = base.transform.FindChild("ChatText");
		if (transform != null)
		{
			MeshRenderer component = transform.GetComponent<MeshRenderer>();
			if (component)
			{
				component.material.color = color;
			}
		}
	}

	public void SetPlaneKey(string imageKey, ref float addY)
	{
		Transform transform = base.transform.FindChild("ChatPlane");
		if (transform != null)
		{
			SimpleSprite component = transform.GetComponent<SimpleSprite>();
			if (component != null)
			{
				Transform transform2 = base.transform.FindChild("ChatText");
				if (transform2 != null)
				{
					UIListItemContainer component2 = transform2.GetComponent<UIListItemContainer>();
					if (null != component2)
					{
						Vector2 vector = (Vector2)component2.Data;
						float num = vector.x + 20f;
						float h = vector.y + 10f;
						component.Setup(num, h, imageKey);
						component.SetColor(new Color(1f, 1f, 1f, 0.8f));
						component.transform.rotation = base.transform.rotation;
						component.transform.localScale = new Vector3(this.fBackImageScaleX, this.fBackImageScaleY, 1f);
						component2.transform.localPosition = new Vector3(-(num * this.fEmoticonScale) / 2f + 20f * this.fEmoticonScale / 2f, vector.y * this.fEmoticonScale / 2f, 0f);
					}
				}
				else
				{
					component.Hide(true);
				}
			}
		}
	}

	private void Update()
	{
		if (this.bRenderEnabled != this.bRenderTarget)
		{
			if (this.fCreateTime > 0f && Time.time - this.fCreateTime >= 1f)
			{
				this.bReadyShow = true;
			}
			if (this.bReadyShow)
			{
				this.SetCurrentShowHide(this.bRenderTarget);
			}
		}
	}

	public void ShowTextAndPlane(bool bShowText, bool bShowPlane)
	{
		float num = 1f;
		float num2 = 1f;
		if (!this.bReadyData)
		{
			bShowText = false;
			bShowPlane = false;
		}
		Transform transform = base.transform.FindChild("ChatText");
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
		Transform transform2 = base.transform.FindChild("ChatPlane");
		if (transform2 != null)
		{
			if (bShowPlane && !this.bPlaneScaleChanged)
			{
				transform2.localScale = new Vector3(transform2.localScale.x * num, transform2.localScale.y * num2, transform2.localScale.z);
				this.bPlaneScaleChanged = true;
			}
			this.bRenderPlane = bShowPlane;
		}
		Transform transform3 = base.transform.FindChild("ChatPlaneArrow");
		if (transform3 != null)
		{
			if (bShowPlane && !this.bPlaneScaleChanged)
			{
				transform3.localScale = new Vector3(transform2.localScale.x * num, transform2.localScale.y * num2, transform2.localScale.z);
				this.bPlaneScaleChanged = true;
			}
			this.bRenderPlane = bShowPlane;
		}
	}

	private void SetCurrentShowHide(bool bShow)
	{
		Transform transform = base.transform.FindChild("ChatPlane");
		if (transform != null)
		{
			MeshRenderer component = transform.GetComponent<MeshRenderer>();
			if (component)
			{
				component.enabled = (bShow && this.bRenderPlane);
			}
		}
		Transform transform2 = base.transform.FindChild("ChatPlaneArrow");
		if (transform2 != null)
		{
			MeshRenderer component2 = transform2.GetComponent<MeshRenderer>();
			if (component2)
			{
				component2.enabled = (bShow && this.bRenderPlane);
			}
		}
		Transform transform3 = base.transform.FindChild("ChatText");
		if (transform3 != null)
		{
			UIListItemContainer component3 = transform3.GetComponent<UIListItemContainer>();
			if (null != component3)
			{
				component3.Visible = (bShow && this.bRenderText);
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
