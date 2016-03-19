using System;
using UnityEngine;

public class NmAnimationEvent : MonoBehaviour
{
	public void NrAnimationEndCallbackEvt(AnimationEvent kAnimationEvt)
	{
		Nr3DCharBase nr3DCharBase = NrTSingleton<Nr3DCharSystem>.Instance.Get3DChar(kAnimationEvt.intParameter);
		if (nr3DCharBase == null)
		{
			Debug.LogError("3DChar not found!");
			Debug.Break();
		}
		nr3DCharBase.StartIdleAnimation(true);
	}
}
