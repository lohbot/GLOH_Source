using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TsEffectTimeControl : MonoBehaviour
{
	[Serializable]
	public class cMeshType
	{
		[SerializeField]
		private float StartTime;

		[SerializeField]
		private int LoopCount = 1;

		[SerializeField]
		private float AnimationTime;

		private int _PlayingCount;

		[SerializeField]
		private GameObject _effectObject;

		public float fStartTime
		{
			get
			{
				return this.StartTime;
			}
			set
			{
				this.StartTime = value;
			}
		}

		public int iLoopCount
		{
			get
			{
				return this.LoopCount;
			}
			set
			{
				this.LoopCount = value;
			}
		}

		public float fAnimationTime
		{
			get
			{
				return this.AnimationTime;
			}
			set
			{
				this.AnimationTime = value;
			}
		}

		public GameObject EffectObject
		{
			get
			{
				return this._effectObject;
			}
		}

		public cMeshType(GameObject TargetObject)
		{
			this._effectObject = TargetObject;
			Animation[] componentsInChildren = TargetObject.GetComponentsInChildren<Animation>();
			Animation[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Animation animation = array[i];
				if (animation.clip.length > this.AnimationTime)
				{
					this.AnimationTime = animation.clip.frameRate;
				}
				animation.wrapMode = WrapMode.Once;
				animation.clip.wrapMode = WrapMode.Once;
			}
		}

		[DebuggerHidden]
		public IEnumerator Update()
		{
			TsEffectTimeControl.cMeshType.<Update>c__Iterator6E <Update>c__Iterator6E = new TsEffectTimeControl.cMeshType.<Update>c__Iterator6E();
			<Update>c__Iterator6E.<>f__this = this;
			return <Update>c__Iterator6E;
		}
	}

	[Serializable]
	public class cParticleType
	{
		[SerializeField]
		private float StartTime;

		[SerializeField]
		private float EmissionTime;

		[SerializeField]
		private bool OneShot;

		[SerializeField]
		private GameObject _effectObject;

		public float fStartTime
		{
			get
			{
				return this.StartTime;
			}
			set
			{
				this.StartTime = value;
			}
		}

		public float fEmissionTime
		{
			get
			{
				return this.EmissionTime;
			}
			set
			{
				this.EmissionTime = value;
			}
		}

		public bool bOneShot
		{
			get
			{
				return this.OneShot;
			}
			set
			{
				this.OneShot = value;
			}
		}

		public GameObject EffectObject
		{
			get
			{
				return this._effectObject;
			}
		}

		public cParticleType(GameObject TargetObject)
		{
			this._effectObject = TargetObject;
			this._effectObject.particleEmitter.enabled = false;
			this._effectObject.SetActive(true);
		}

		[DebuggerHidden]
		public IEnumerator Update()
		{
			TsEffectTimeControl.cParticleType.<Update>c__Iterator6F <Update>c__Iterator6F = new TsEffectTimeControl.cParticleType.<Update>c__Iterator6F();
			<Update>c__Iterator6F.<>f__this = this;
			return <Update>c__Iterator6F;
		}
	}

	[Serializable]
	public class cTiledAnimation
	{
		[SerializeField]
		private float StartTime;

		[SerializeField]
		private int LoopCount = 1;

		[SerializeField]
		private GameObject _effectObject;

		[SerializeField]
		private FX_TEX_TiledAnimation refTiledAni;

		public float fStartTime
		{
			get
			{
				return this.StartTime;
			}
			set
			{
				this.StartTime = value;
			}
		}

		public int iLoopCount
		{
			get
			{
				return this.LoopCount;
			}
			set
			{
				this.LoopCount = value;
			}
		}

		public GameObject EffectObject
		{
			get
			{
				return this._effectObject;
			}
		}

		public cTiledAnimation(GameObject TargetObject)
		{
			this._effectObject = TargetObject;
			if (this.EffectObject != null)
			{
				this.refTiledAni = this.EffectObject.GetComponent<FX_TEX_TiledAnimation>();
				if (this.refTiledAni != null)
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						" refTiledAni name = ",
						this.refTiledAni.name,
						"    refTiledAni lifeTime =",
						this.refTiledAni.lifetime
					}));
					this.refTiledAni.StartTime = this.StartTime;
					this.refTiledAni.LoopCount = this.LoopCount;
				}
			}
		}

		private void Update()
		{
		}
	}

	public List<TsEffectTimeControl.cMeshType> listMeshEffect = new List<TsEffectTimeControl.cMeshType>();

	public List<TsEffectTimeControl.cParticleType> listParticleEffect = new List<TsEffectTimeControl.cParticleType>();

	public List<TsEffectTimeControl.cTiledAnimation> listTiledAnimation = new List<TsEffectTimeControl.cTiledAnimation>();

	[SerializeField]
	public float fEffectMaxTime;

	private void Start()
	{
		this.FindEffectMaxTime();
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.StartEffect());
	}

	[DebuggerHidden]
	private IEnumerator StartEffect()
	{
		TsEffectTimeControl.<StartEffect>c__Iterator6D <StartEffect>c__Iterator6D = new TsEffectTimeControl.<StartEffect>c__Iterator6D();
		<StartEffect>c__Iterator6D.<>f__this = this;
		return <StartEffect>c__Iterator6D;
	}

	private void Update()
	{
	}

	public void TraversGameObject(GameObject Go)
	{
		for (int i = 0; i < Go.transform.childCount; i++)
		{
			GameObject gameObject = Go.transform.GetChild(i).gameObject;
			if (gameObject.transform.childCount > 0)
			{
				this.CollectEffectObject(gameObject);
				this.TraversGameObject(gameObject);
			}
			else
			{
				this.CollectEffectObject(gameObject);
			}
		}
		this.FindEffectMaxTime();
	}

	public void FindEffectMaxTime()
	{
		this.fEffectMaxTime = 0f;
		foreach (TsEffectTimeControl.cMeshType current in this.listMeshEffect)
		{
			if (current.iLoopCount == -1)
			{
				this.fEffectMaxTime = -1f;
				return;
			}
			float num = this.ComputeMeshTotalTime(current);
			if (num > this.fEffectMaxTime)
			{
				this.fEffectMaxTime = num;
			}
		}
		foreach (TsEffectTimeControl.cParticleType current2 in this.listParticleEffect)
		{
			if (current2.fEmissionTime == -1f)
			{
				this.fEffectMaxTime = -1f;
				break;
			}
			float num2 = this.ComputeParticleTotalTime(current2);
			if (num2 > this.fEffectMaxTime)
			{
				this.fEffectMaxTime = num2;
			}
		}
	}

	private void CollectEffectObject(GameObject Go)
	{
		if (Go == null)
		{
			return;
		}
		Animation component = Go.GetComponent<Animation>();
		if (component != null && component.clip.length > 0f)
		{
			bool flag = false;
			foreach (TsEffectTimeControl.cMeshType current in this.listMeshEffect)
			{
				if (current.EffectObject.name.Equals(Go.name))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.listMeshEffect.Add(new TsEffectTimeControl.cMeshType(Go));
			}
		}
		ParticleRenderer component2 = Go.GetComponent<ParticleRenderer>();
		if (component2 != null)
		{
			bool flag2 = false;
			foreach (TsEffectTimeControl.cParticleType current2 in this.listParticleEffect)
			{
				if (current2.EffectObject.name.Equals(Go.name))
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				this.listParticleEffect.Add(new TsEffectTimeControl.cParticleType(Go));
			}
		}
		FX_TEX_TiledAnimation component3 = Go.GetComponent<FX_TEX_TiledAnimation>();
		if (component3 != null)
		{
			bool flag3 = false;
			foreach (TsEffectTimeControl.cTiledAnimation current3 in this.listTiledAnimation)
			{
				if (!(current3.EffectObject == null))
				{
					if (current3.EffectObject.name.Equals(Go.name))
					{
						flag3 = true;
						break;
					}
				}
			}
			if (!flag3)
			{
				this.listTiledAnimation.Add(new TsEffectTimeControl.cTiledAnimation(Go));
			}
		}
	}

	public float ComputeMeshTotalTime(TsEffectTimeControl.cMeshType MeshEffect)
	{
		float fStartTime = MeshEffect.fStartTime;
		float num = 0f;
		int iLoopCount = MeshEffect.iLoopCount;
		if (MeshEffect.EffectObject == null)
		{
			return 0f;
		}
		GameObject effectObject = MeshEffect.EffectObject;
		Animation[] components = effectObject.GetComponents<Animation>();
		Animation[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			Animation animation = array[i];
			if (animation.clip.length > num)
			{
				num = animation.clip.length;
			}
		}
		return fStartTime + num * (float)iLoopCount;
	}

	public float ComputeParticleTotalTime(TsEffectTimeControl.cParticleType ParticleEffect)
	{
		float fStartTime = ParticleEffect.fStartTime;
		float fEmissionTime = ParticleEffect.fEmissionTime;
		float num = 0f;
		if (ParticleEffect.EffectObject == null)
		{
			return 0f;
		}
		GameObject effectObject = ParticleEffect.EffectObject;
		ParticleEmitter[] componentsInChildren = effectObject.GetComponentsInChildren<ParticleEmitter>();
		ParticleEmitter[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			ParticleEmitter particleEmitter = array[i];
			if (particleEmitter.maxEnergy > num)
			{
				num = particleEmitter.maxEnergy;
			}
		}
		return fStartTime + fEmissionTime + num;
	}
}
