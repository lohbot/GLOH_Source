using System;
using UnityEngine;

public class FX_PT_Spiral : MonoBehaviour
{
	public const int Min_numArms = 1;

	public const int Max_numArms = 20;

	public const int Min_numPPA = 1;

	public const int Max_numPPA = 800;

	public const float Min_partSep = -1f;

	public const float Max_partSep = 1f;

	public const float Min_turnDist = -1f;

	public const float Max_turnDist = 1f;

	public const float Min_vertDist = -1f;

	public const float Max_vertDist = 1f;

	public const float Min_originOffset = -10f;

	public const float Max_originOffset = 10f;

	public const float Min_turnSpeed = -200f;

	public const float Max_turnSpeed = 200f;

	public const float Min_fade = 0f;

	public const float Max_fade = 1f;

	public const float Min_size = -0.1f;

	public const float Max_size = 0.1f;

	public Transform particleEffect;

	public int numberOfArms = 1;

	public int particlesPerArm = 200;

	public float particleSeparation = 0.05f;

	public float turnDistance = 0.5f;

	public float verticalTurnDistance;

	public float originOffset;

	public float turnSpeed;

	public float fadeValue;

	public float sizeValue;

	public int numberOfSpawns = 9999999;

	public float spawnRate = 5f;

	private float timeOfLastSpawn = -1000f;

	private int spawnCount;

	private int totParticles;

	private SpiralSettings defaultSettings;

	public void Start()
	{
		this.defaultSettings = this.getSettings();
	}

	private void SpawnEffect()
	{
		Transform transform = UnityEngine.Object.Instantiate(this.particleEffect, base.transform.position, base.transform.rotation) as Transform;
		transform.parent = base.gameObject.transform;
		ParticleEmitter component = transform.GetComponent<ParticleEmitter>();
		ParticleAnimator component2 = component.transform.GetComponent<ParticleAnimator>();
		if (component2 != null)
		{
			component2.autodestruct = true;
		}
		component.Emit(this.numberOfArms * this.particlesPerArm);
		Particle[] particles = component.particles;
		float num = 6.28318548f / (float)this.numberOfArms;
		for (int i = 0; i < this.numberOfArms; i++)
		{
			float num2 = 0f;
			float f = (float)i * num;
			for (int j = 0; j < this.particlesPerArm; j++)
			{
				int num3 = i * this.particlesPerArm + j;
				float num4 = this.originOffset + this.turnDistance * num2;
				Vector3 position = transform.localPosition;
				position.x += num4 * Mathf.Cos(num2);
				position.z += num4 * Mathf.Sin(num2);
				float x = position.x * Mathf.Cos(f) + position.z * Mathf.Sin(f);
				float z = -position.x * Mathf.Sin(f) + position.z * Mathf.Cos(f);
				position.x = x;
				position.z = z;
				position.y += (float)j * this.verticalTurnDistance;
				if (component.useWorldSpace)
				{
					position = base.transform.TransformPoint(position);
				}
				particles[num3].position = position;
				num2 += this.particleSeparation;
				particles[num3].energy = particles[num3].energy - (float)j * this.fadeValue;
				particles[num3].size = particles[num3].size - (float)j * this.sizeValue;
			}
		}
		component.particles = particles;
	}

	private void Update()
	{
		base.transform.Rotate(base.transform.up * Time.deltaTime * -this.turnSpeed, Space.World);
	}

	private void LateUpdate()
	{
		float num = Time.time - this.timeOfLastSpawn;
		if (num >= this.spawnRate && this.spawnCount < this.numberOfSpawns)
		{
			this.SpawnEffect();
			this.timeOfLastSpawn = Time.time;
			this.spawnCount++;
		}
	}

	public SpiralSettings getSettings()
	{
		SpiralSettings result;
		result.numArms = this.numberOfArms;
		result.numPPA = this.particlesPerArm;
		result.partSep = this.particleSeparation;
		result.turnDist = this.turnDistance;
		result.vertDist = this.verticalTurnDistance;
		result.originOffset = this.originOffset;
		result.turnSpeed = this.turnSpeed;
		result.fade = this.fadeValue;
		result.size = this.sizeValue;
		return result;
	}

	public SpiralSettings resetEffect(bool killCurrent, SpiralSettings settings)
	{
		if (killCurrent)
		{
			this.killCurrentEffects();
		}
		this.numberOfArms = settings.numArms;
		this.particlesPerArm = settings.numPPA;
		this.particleSeparation = settings.partSep;
		this.turnDistance = settings.turnDist;
		this.verticalTurnDistance = settings.vertDist;
		this.originOffset = settings.originOffset;
		this.turnSpeed = settings.turnSpeed;
		this.fadeValue = settings.fade;
		this.sizeValue = settings.size;
		this.SpawnEffect();
		this.timeOfLastSpawn = Time.time;
		this.spawnCount++;
		return this.getSettings();
	}

	public SpiralSettings resetEffectToDefaults(bool killCurrent)
	{
		return this.resetEffect(killCurrent, this.defaultSettings);
	}

	public SpiralSettings randomizeEffect(bool killCurrent)
	{
		if (killCurrent)
		{
			this.killCurrentEffects();
		}
		this.numberOfArms = UnityEngine.Random.Range(1, 21);
		this.particlesPerArm = UnityEngine.Random.Range(1, 801);
		this.particleSeparation = UnityEngine.Random.Range(-1f, 1f);
		this.turnDistance = UnityEngine.Random.Range(-1f, 1f);
		this.verticalTurnDistance = UnityEngine.Random.Range(-1f, 1f);
		this.originOffset = UnityEngine.Random.Range(-10f, 10f);
		this.turnSpeed = UnityEngine.Random.Range(-200f, 200f);
		this.fadeValue = UnityEngine.Random.Range(0f, 1f);
		this.sizeValue = UnityEngine.Random.Range(-0.1f, 0.1f);
		this.SpawnEffect();
		this.timeOfLastSpawn = Time.time;
		this.spawnCount++;
		return this.getSettings();
	}

	private void killCurrentEffects()
	{
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		ParticleEmitter[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			ParticleEmitter particleEmitter = array[i];
			Debug.Log("resetEffect killing: " + particleEmitter.name);
			ParticleAnimator component = particleEmitter.transform.GetComponent<ParticleAnimator>();
			if (component != null)
			{
				component.autodestruct = true;
			}
			Particle[] particles = particleEmitter.particles;
			for (int j = 0; j < particles.Length; j++)
			{
				particles[j].energy = 0.1f;
			}
			particleEmitter.particles = particles;
		}
	}
}
