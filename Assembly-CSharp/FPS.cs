using System;
using UnityEngine;

public class FPS : MonoBehaviour
{
	public int x = 5;

	public int y = 5;

	public float updateInterval = 0.5f;

	private double lastInterval;

	private float frames;

	private float fps;

	public bool m_bCharacterPositionUI;

	private void Start()
	{
		this.lastInterval = (double)Time.realtimeSinceStartup;
		this.frames = 0f;
	}

	private void Update()
	{
		this.frames += 1f;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if ((double)realtimeSinceStartup > this.lastInterval + (double)this.updateInterval)
		{
			this.fps = (float)((double)this.frames / ((double)realtimeSinceStartup - this.lastInterval));
			this.frames = 0f;
			this.lastInterval = (double)realtimeSinceStartup;
		}
	}

	public float GetFPS()
	{
		return this.fps;
	}
}
