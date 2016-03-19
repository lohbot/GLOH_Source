using System;
using UnityEngine;

namespace UnityForms
{
	public class TextArea : UITextField
	{
		public UISlider slider;

		private float scrollPos;

		public new static TextArea Create(string name, Vector3 pos)
		{
			return (TextArea)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(TextArea));
		}

		public override void Start()
		{
			if (this.m_started)
			{
				return;
			}
			base.Start();
			if (this.spriteText == null)
			{
				this.Text = string.Empty;
			}
			if (this.spriteText != null)
			{
				this.spriteText.password = this.password;
				this.spriteText.maskingCharacter = this.maskingCharacter;
				this.spriteText.multiline = this.multiline;
				this.origTextPos = this.spriteText.transform.localPosition;
				base.SetMargins(this.margins);
			}
			this.insert = this.Text.Length;
			if (Application.isPlaying)
			{
				if (base.collider == null)
				{
					this.AddCollider();
				}
				if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
				{
					if (this.showCaretOnMobile)
					{
						base.CreateCaret();
					}
				}
				else
				{
					base.CreateCaret();
				}
			}
			this.cachedPos = base.transform.position;
			this.cachedRot = base.transform.rotation;
			this.cachedScale = base.transform.lossyScale;
			base.CalcClippingRect();
			if (this.managed && this.m_hidden)
			{
				this.Hide(true);
			}
			Color color = this.color;
			color.a = 0.8f;
			this.SetColor(color);
		}

		public void AddSliderDelegate()
		{
			if (this.slider)
			{
				this.slider.Start();
				this.slider.useknobButton = false;
				this.slider.AddValueChangedDelegate(new EZValueChangedDelegate(this.SliderMoved));
			}
		}

		public override void GoUp()
		{
			if (this.caret == null || this.spriteText == null)
			{
				return;
			}
			Vector3 vector = this.spriteText.GetInsertionPointPos(this.spriteText.PlainIndexToDisplayIndex(this.insert));
			vector += this.spriteText.transform.up * this.spriteText.LineSpan * this.spriteText.transform.lossyScale.y;
			this.insert = this.spriteText.DisplayIndexToPlainIndex(this.spriteText.GetNearestInsertionPoint(vector));
			NrTSingleton<UIManager>.Instance.InsertionPoint = this.insert;
			vector = base.transform.InverseTransformPoint(this.spriteText.GetInsertionPointPos(this.spriteText.PlainIndexToDisplayIndex(this.insert)));
			Vector3 vector2 = vector + Vector3.up * this.spriteText.BaseHeight * this.spriteText.transform.localScale.y;
			if (this.multiline && vector2.y > this.marginTopLeft.y)
			{
				this.spriteText.transform.localPosition -= Vector3.up * this.spriteText.LineSpan;
				base.PositionCaret(false);
				this.spriteText.SetClippingRect(this.clientClippingRect);
				this.slider.ClickUpButtonDonotCallChangeDelegate(null);
			}
		}

		public override void GoDown()
		{
			if (this.caret == null || this.spriteText == null)
			{
				return;
			}
			Vector3 vector = this.spriteText.GetInsertionPointPos(this.spriteText.PlainIndexToDisplayIndex(this.insert));
			vector -= this.spriteText.transform.up * this.spriteText.LineSpan * this.spriteText.transform.lossyScale.y;
			this.insert = this.spriteText.DisplayIndexToPlainIndex(this.spriteText.GetNearestInsertionPoint(vector));
			NrTSingleton<UIManager>.Instance.InsertionPoint = this.insert;
			vector = base.transform.InverseTransformPoint(this.spriteText.GetInsertionPointPos(this.spriteText.PlainIndexToDisplayIndex(this.insert)));
			if (this.multiline && vector.y < this.marginBottomRight.y)
			{
				this.spriteText.transform.localPosition += Vector3.up * this.spriteText.LineSpan;
				base.PositionCaret(false);
				this.spriteText.SetClippingRect(this.clientClippingRect);
				this.slider.ClickDownButtonDonotCallChangeDelegate(null);
			}
		}

		public override void CallSliderChangeDelegate(bool upButton)
		{
			if (upButton)
			{
				this.slider.ClickUpButtonDonotCallChangeDelegate(null);
			}
			else
			{
				this.slider.ClickDownButtonDonotCallChangeDelegate(null);
			}
		}

		private void SliderMoved(IUIObject slider)
		{
			if (this.slider && this.scrollPos != ((UISlider)slider).Value)
			{
				this.scrollPos = ((UISlider)slider).Value;
				this.slider.CallChangeDelegate = false;
				this.slider.Value = this.scrollPos;
				if (this.slider.clickButton)
				{
					Vector3 vector = this.spriteText.GetInsertionPointPos(this.spriteText.PlainIndexToDisplayIndex(this.insert));
					vector += this.spriteText.transform.up * this.spriteText.LineSpan * this.spriteText.transform.lossyScale.y;
					this.insert = this.spriteText.DisplayIndexToPlainIndex(this.spriteText.GetNearestInsertionPoint(vector));
					NrTSingleton<UIManager>.Instance.InsertionPoint = this.insert;
					vector = base.transform.InverseTransformPoint(this.spriteText.GetInsertionPointPos(this.spriteText.PlainIndexToDisplayIndex(this.insert)));
					if (this.multiline)
					{
						this.spriteText.transform.localPosition -= Vector3.up * this.spriteText.LineSpan * (float)this.slider.data;
						base.PositionCaret(false);
						this.spriteText.SetClippingRect(this.clientClippingRect);
						return;
					}
				}
				else
				{
					Vector3 vector2 = this.spriteText.GetInsertionPointPos(this.spriteText.PlainIndexToDisplayIndex(this.insert));
					vector2 -= this.spriteText.transform.up * this.spriteText.LineSpan * this.spriteText.transform.lossyScale.y;
					this.insert = this.spriteText.DisplayIndexToPlainIndex(this.spriteText.GetNearestInsertionPoint(vector2));
					NrTSingleton<UIManager>.Instance.InsertionPoint = this.insert;
					if (this.caret == null || this.spriteText == null)
					{
						return;
					}
					vector2 = base.transform.InverseTransformPoint(this.spriteText.GetInsertionPointPos(this.spriteText.PlainIndexToDisplayIndex(this.insert)));
					if (this.multiline)
					{
						this.spriteText.transform.localPosition += Vector3.up * this.spriteText.LineSpan * (float)this.slider.data;
						base.PositionCaret(false);
						this.spriteText.SetClippingRect(this.clientClippingRect);
					}
				}
			}
		}

		public void Update()
		{
			if (this.spriteText && this.slider)
			{
				if (this.spriteText.TotalHeight > this.height)
				{
					this.slider.lineHeight = this.spriteText.TotalHeight / this.fontSize - this.height / this.fontSize;
					this.slider.totalLineCount = this.spriteText.TotalHeight / this.fontSize;
					if (!this.slider.Visible)
					{
						this.slider.CallChangeDelegate = false;
						this.slider.Visible = true;
						this.slider.Value = 1f;
					}
				}
				else
				{
					if (this.slider.Visible)
					{
						this.slider.Visible = false;
					}
					this.slider.lineHeight = 0f;
				}
			}
		}
	}
}
