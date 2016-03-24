using System;
using UnityEngine;
using UnityForms;

public class UILabelAnimationManager : NrTSingleton<UILabelAnimationManager>
{
	private UILabelAnimationManager()
	{
	}

	public void PrevDataDelete(Label text)
	{
		if (this.IsNull(text))
		{
			return;
		}
		UILabelStepByStepAni component = text.transform.gameObject.GetComponent<UILabelStepByStepAni>();
		if (component == null)
		{
			return;
		}
		component.PrevValueDelete();
	}

	public void TextAniSetting(Label text, UILabelStepByStepAniInfo aniInfo)
	{
		if (this.IsNull(text))
		{
			return;
		}
		UILabelStepByStepAni uILabelStepByStepAni = text.transform.gameObject.AddComponent<UILabelStepByStepAni>();
		uILabelStepByStepAni._loopTime = aniInfo.loopTime;
		uILabelStepByStepAni._loopInterval = aniInfo.loopInterval;
		uILabelStepByStepAni._nextValueStopInterval = aniInfo.nextValueStopInterval;
		uILabelStepByStepAni._reverse = aniInfo.reverse;
		uILabelStepByStepAni._changePartUpdate = aniInfo.changePartUpdate;
		uILabelStepByStepAni._useComma = aniInfo.useComma;
	}

	public void TextUpdateAndPlayAni(Label text, string targetText)
	{
		if (!this.IsStepByStepAniSetting(text))
		{
			return;
		}
		UILabelStepByStepAni component = text.transform.gameObject.GetComponent<UILabelStepByStepAni>();
		component.Clear();
		text.SetText(targetText);
	}

	private bool IsNull(Label text)
	{
		if (text == null)
		{
			Debug.LogError("ERROR, UILabelAnimationManager.cs, IsNull(), text is Null");
			return true;
		}
		return false;
	}

	private bool IsStepByStepAniSetting(Label text)
	{
		if (text == null)
		{
			Debug.LogError("ERROR, UILabelAnimationManager.cs, IsStepByStepAniSetting(), text is Null");
			return false;
		}
		UILabelStepByStepAni component = text.GetComponent<UILabelStepByStepAni>();
		if (component == null)
		{
			Debug.LogError("ERROR, UILabelAnimationManager.cs, IsNoProblem(), textAni is Null");
			return false;
		}
		return true;
	}
}
