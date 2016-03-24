using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityForms;

public class UILabelStepByStepAni : UILabelAnimationBase
{
	[SerializeField]
	public float _loopTime = -1f;

	[SerializeField]
	public float _loopInterval = 0.01f;

	[SerializeField]
	public float _nextValueStopInterval = 0.3f;

	[SerializeField]
	public bool _reverse;

	[SerializeField]
	public bool _changePartUpdate;

	private int _stopLabelDirectionCount;

	private float _stopLabelNewstTime;

	private void Awake()
	{
		this._label = base.GetComponent<Label>();
		this._prevValue = string.Empty;
		this._nextValue = string.Empty;
		this._currentState = UILabelAnimationBase.STATE.WAIT;
		this._directionCoroutineName = "StepByStepNumberDirection";
	}

	private void Update()
	{
		base.AddCommaProcess();
		if (this._currentState == UILabelAnimationBase.STATE.DIRECTION)
		{
			return;
		}
		if (!base.IsTextParseIntAvaliable(this._label.spriteText.Text))
		{
			return;
		}
		if (!base.IsTextChange())
		{
			return;
		}
		this.StartDirection();
	}

	private void StartDirection()
	{
		this._currentState = UILabelAnimationBase.STATE.DIRECTION;
		this._nextValue = this._label.spriteText.Text;
		this._stopLabelDirectionCount = 0;
		this._stopLabelDirectionCount = this.CalculateStartStopLabelDirectionCount();
		base.PlayLoopSound();
		base.StartCoroutine(this._directionCoroutineName);
	}

	[DebuggerHidden]
	private IEnumerator StepByStepNumberDirection()
	{
		UILabelStepByStepAni.<StepByStepNumberDirection>c__Iterator63 <StepByStepNumberDirection>c__Iterator = new UILabelStepByStepAni.<StepByStepNumberDirection>c__Iterator63();
		<StepByStepNumberDirection>c__Iterator.<>f__this = this;
		return <StepByStepNumberDirection>c__Iterator;
	}

	private void StopNumberStepByStep()
	{
		if (base.PureValueLength(this._label.spriteText.Text) <= this._stopLabelDirectionCount || base.PureValueLength(this._nextValue) <= this._stopLabelDirectionCount)
		{
			return;
		}
		char[] array = this._label.spriteText.Text.ToCharArray();
		char[] array2 = this._nextValue.ToCharArray();
		if (array == null || array2 == null)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < array.Length; i++)
		{
			int num3;
			if (this._reverse)
			{
				num3 = i;
			}
			else
			{
				num3 = array.Length - 1 - i;
			}
			if (this._stopLabelDirectionCount == num2)
			{
				break;
			}
			if (int.TryParse(array[num3].ToString(), out num))
			{
				array[num3] = this._nextValue[num3];
				num2++;
			}
		}
		this._label.spriteText.Text = new string(array);
		this.UpdateStopLabelDirectionCount();
	}

	private void UpdateStopLabelDirectionCount()
	{
		if (!this.IsChangeLabelIntervalExceed())
		{
			return;
		}
		this._stopLabelNewstTime = Time.realtimeSinceStartup;
		this._stopLabelDirectionCount++;
	}

	private bool IsChangeLabelIntervalExceed()
	{
		float num = Time.realtimeSinceStartup - this._stopLabelNewstTime;
		return num >= this._nextValueStopInterval;
	}

	private int CalculateStartStopLabelDirectionCount()
	{
		if (!this._changePartUpdate)
		{
			return 0;
		}
		string text = base.PureValue(this._prevValue);
		string text2 = base.PureValue(this._nextValue);
		if (text.Length < text2.Length)
		{
			return 0;
		}
		if (this._nextValue.Length < this._prevValue.Length)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < this._prevValue.Length; i++)
		{
			int index;
			if (this._reverse)
			{
				index = i;
			}
			else
			{
				index = this._prevValue.Length - 1 - i;
			}
			if (text[index] != text2[index])
			{
				break;
			}
			num++;
		}
		return num;
	}
}
