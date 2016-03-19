using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TsMobileGlowEffect : MonoBehaviour
{
	public Color glowTint = new Color(1f, 1f, 1f, 0f);

	public Shader compositeShader;

	private Material m_CompositeMaterial;

	protected Material compositeMaterial
	{
		get
		{
			if (this.m_CompositeMaterial == null)
			{
				this.m_CompositeMaterial = new Material(this.compositeShader);
				this.m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_CompositeMaterial;
		}
	}

	protected void OnDisable()
	{
		if (this.m_CompositeMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.m_CompositeMaterial);
		}
	}

	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (!this.compositeMaterial.shader.isSupported)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0);
		Graphics.Blit(source, temporary);
		Graphics.Blit(source, destination);
		this.compositeMaterial.color = new Color(this.glowTint.r, this.glowTint.g, this.glowTint.b, this.glowTint.a / 4f);
		Graphics.Blit(temporary, destination, this.compositeMaterial);
		RenderTexture.ReleaseTemporary(temporary);
	}
}
