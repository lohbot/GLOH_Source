using System;
using UnityEngine;

public class FX_ANI_Blender : MonoBehaviour
{
	private Animation _AnimationComponent;

	private void Start()
	{
		this._AnimationComponent = base.GetComponentInChildren<Animation>();
		this._AnimationComponent.wrapMode = WrapMode.Loop;
		this._AnimationComponent.Blend("loop", 0.5f, 0.1f);
		this._AnimationComponent.Blend("path", 0.5f, 0.1f);
		this._AnimationComponent["path"].normalizedTime = UnityEngine.Random.value;
	}
}
