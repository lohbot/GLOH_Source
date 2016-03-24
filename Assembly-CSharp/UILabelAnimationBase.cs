using System;
using UnityEngine;
using UnityForms;

[RequireComponent(typeof(Label))]
public abstract class UILabelAnimationBase : MonoBehaviour
{
	protected enum STATE
	{
		WAIT,
		DIRECTION
	}

	[SerializeField]
	public bool _useComma;

	protected Label _label;

	protected string _prevValue = string.Empty;

	protected string _nextValue = string.Empty;

	protected UILabelAnimationBase.STATE _currentState;

	protected string _directionCoroutineName = string.Empty;

	public void PrevValueDelete()
	{
		this._prevValue = string.Empty;
	}

	public void Clear()
	{
		if (this._currentState == UILabelAnimationBase.STATE.WAIT)
		{
			return;
		}
		this.StopDirectionCoroutine();
		this._currentState = UILabelAnimationBase.STATE.WAIT;
		this._label.spriteText.Text = this.PureValue(this._nextValue);
		this._prevValue = this.PureValue(this._label.spriteText.Text);
		this._label.spriteText.Text = this.AddComma(this._label.spriteText.Text);
		this.StopLoopSound();
	}

	protected bool IsTextParseIntAvaliable(string text)
	{
		long num = -1L;
		string s = this.RemoveComma(text);
		return long.TryParse(s, out num);
	}

	protected bool IsTextChange()
	{
		return !(this._label == null) && !this._prevValue.Equals(this.PureValue(this._label.spriteText.Text));
	}

	protected bool IsDirectionTimeOver(float directionStartTime, float directionLoopTime)
	{
		float num = Time.realtimeSinceStartup - directionStartTime;
		return directionLoopTime < num;
	}

	protected void LabelChangeRandom()
	{
		if (this._nextValue.Length <= 0)
		{
			return;
		}
		int num = 0;
		this._label.spriteText.Text = string.Empty;
		for (int i = 0; i < this._nextValue.Length; i++)
		{
			if (int.TryParse(this._nextValue[i].ToString(), out num))
			{
				SpriteText expr_5B = this._label.spriteText;
				expr_5B.Text += UnityEngine.Random.Range(0, 10).ToString();
			}
			else
			{
				SpriteText expr_8B = this._label.spriteText;
				expr_8B.Text += this._nextValue[i];
			}
		}
	}

	protected void AddCommaProcess()
	{
		if (this._label.spriteText.Text.Contains(","))
		{
			return;
		}
		this._label.spriteText.Text = this.AddComma(this._label.spriteText.Text);
	}

	protected string AddComma(string text)
	{
		if (!this._useComma)
		{
			return text;
		}
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}
		int num = 3;
		string text2 = string.Empty;
		for (int i = 0; i < text.Length; i++)
		{
			int index = text.Length - i - 1;
			if (i != 0 && i % num == 0)
			{
				text2 = "," + text2;
			}
			text2 = text[index] + text2;
		}
		return text2;
	}

	protected string RemoveComma(string text)
	{
		string text2 = string.Empty;
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] != ',')
			{
				text2 += text[i];
			}
		}
		return text2;
	}

	protected int PureValueLength(string text)
	{
		return this.RemoveComma(text).Length;
	}

	protected string PureValue(string text)
	{
		return this.RemoveComma(text);
	}

	protected void PlayLoopSound()
	{
	}

	protected void StopLoopSound()
	{
	}

	protected void PlayEndSound()
	{
	}

	private void StopDirectionCoroutine()
	{
		base.StopCoroutine(this._directionCoroutineName);
	}
}
