using System;
using UnityEngine;

public class FX_PT_Blink : MonoBehaviour
{
	[SerializeField]
	public float duration = 2f;

	private float _fLerp;

	private Color[] _ArryColor;

	private Color[] _ArryTransColor = new Color[5];

	private ParticleAnimator _ParticleAnimator;

	private float[] fRandTime = new float[5];

	private void Start()
	{
		this._ParticleAnimator = base.GetComponent<ParticleAnimator>();
		if (this._ParticleAnimator == null)
		{
			Debug.Log("Not Found ParticleAnimatior! don't use BlinkScript");
		}
		this._ArryColor = this._ParticleAnimator.colorAnimation;
		for (int i = 0; i < this.fRandTime.Length; i++)
		{
			this.fRandTime[i] = UnityEngine.Random.value;
		}
	}

	private void Update()
	{
		for (int i = 0; i < this._ArryColor.Length; i++)
		{
			this._fLerp = Mathf.PingPong(Time.time * this.fRandTime[i], this.duration) / this.duration;
			this._ArryTransColor[i] = Color.Lerp(this._ArryColor[i], Color.clear, this._fLerp);
			this._ParticleAnimator.colorAnimation = this._ArryTransColor;
		}
	}
}
