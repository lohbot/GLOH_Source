using System;
using UnityEngine;

[RequireComponent(typeof(GUIText))]
public class TsPrintProperties : MonoBehaviour
{
	private GameObject[] m_objWhole;

	private int m_nVisibleObject;

	private float m_fUpdateInterval = 1f;

	private float m_fAccum;

	private int m_nFrames;

	private float m_fTimeLeft;

	private void Start()
	{
		if (!base.guiText)
		{
			MonoBehaviour.print("Frames Per Second needs a GUIText component!");
			base.enabled = false;
			return;
		}
		this.m_fTimeLeft = this.m_fUpdateInterval;
	}

	private void Update()
	{
		this.m_fTimeLeft -= Time.deltaTime;
		this.m_fAccum += Time.timeScale / Time.deltaTime;
		this.m_nFrames++;
		if (this.m_fTimeLeft <= 0f)
		{
			string text = string.Concat(new string[]
			{
				"System Infomation\r\nProcessor : ",
				SystemInfo.processorCount.ToString("d"),
				"\r\nMemory : ",
				SystemInfo.systemMemorySize.ToString("d"),
				"\r\nVGA Memory : ",
				SystemInfo.graphicsMemorySize.ToString("d"),
				"\r\nDevice : ",
				SystemInfo.graphicsDeviceName,
				"\r\nShader : v ",
				SystemInfo.graphicsShaderLevel.ToString("d")
			});
			this.m_objWhole = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			this.m_nVisibleObject = 0;
			GameObject[] objWhole = this.m_objWhole;
			for (int i = 0; i < objWhole.Length; i++)
			{
				GameObject gameObject = objWhole[i];
				Renderer component = gameObject.GetComponent<Renderer>();
				if (component && component.isVisible)
				{
					this.m_nVisibleObject++;
				}
			}
			base.guiText.text = string.Concat(new string[]
			{
				text,
				"\r\nFPS : ",
				(this.m_fAccum / (float)this.m_nFrames).ToString("f2"),
				"\r\nVisible Object : ",
				this.m_nVisibleObject.ToString("d")
			});
			this.m_fTimeLeft = this.m_fUpdateInterval;
			this.m_fAccum = 0f;
			this.m_nFrames = 0;
		}
	}
}
