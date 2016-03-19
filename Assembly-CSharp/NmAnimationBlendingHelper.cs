using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Animation/BlendHelper %b")]
public class NmAnimationBlendingHelper : MonoBehaviour
{
	public static readonly float DEFAULT_BLENDING_TIME = 0.1f;

	[SerializeField]
	public List<BlendData> _BlendDataList = new List<BlendData>();

	private float _beginBlendingTime = 3.40282347E+38f;

	private float _endBlendingTime = 3.40282347E+38f;

	private WrapMode _srcWarpMode;

	private string _sourceAni = string.Empty;

	private string _targetAni = string.Empty;

	public float _fBlendingTime = NmAnimationBlendingHelper.DEFAULT_BLENDING_TIME;

	public BlendData[] BlendDataArray
	{
		get
		{
			return this._BlendDataList.ToArray();
		}
	}

	public void OnSimulation(string _source, string _target, float bLendTime)
	{
		if (base.animation[_source] == null)
		{
			return;
		}
		this._sourceAni = _source;
		this._targetAni = _target;
		this._fBlendingTime = bLendTime;
		AnimationState animationState = base.animation[this._sourceAni];
		this._srcWarpMode = animationState.wrapMode;
		float length = animationState.length;
		this._beginBlendingTime = Time.time + length - this._fBlendingTime;
		base.animation.Play(_source);
	}

	public void Update()
	{
		if (Time.time >= this._beginBlendingTime)
		{
			base.animation.CrossFade(this._targetAni, this._fBlendingTime);
			this._beginBlendingTime = 3.40282347E+38f;
			this._endBlendingTime = Time.time + base.animation[this._targetAni].length;
		}
		if (Time.time >= this._endBlendingTime)
		{
			AnimationState animationState = base.animation[this._sourceAni];
			animationState.wrapMode = this._srcWarpMode;
			this._endBlendingTime = 3.40282347E+38f;
		}
	}

	public float GetBlendingTime(string aniClipNameSource, string aniClipNameTaget)
	{
		foreach (BlendData current in this._BlendDataList)
		{
			if (current._strSourceName.Equals(aniClipNameSource) && current._strTagetName.Equals(aniClipNameTaget))
			{
				return current._fBlendingTime;
			}
		}
		return NmAnimationBlendingHelper.DEFAULT_BLENDING_TIME;
	}
}
