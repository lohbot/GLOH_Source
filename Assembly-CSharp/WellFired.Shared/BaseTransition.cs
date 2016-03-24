using System;
using System.Collections.Generic;
using UnityEngine;

namespace WellFired.Shared
{
	public class BaseTransition
	{
		private List<Camera> additionalSourceCameras = new List<Camera>();

		private List<Camera> additionalDestinationCameras = new List<Camera>();

		private Camera sourceCamera;

		private Camera destinationCamera;

		private Material renderMaterial;

		private RenderTexture introRenderTexture;

		private RenderTexture outroRenderTexture;

		private bool shouldRender;

		private bool prevIntroCameraState;

		private bool prevOutroCameraState;

		private float ratio;

		private RenderTexture IntroRenderTexture
		{
			get
			{
				if (this.introRenderTexture == null)
				{
					this.introRenderTexture = new RenderTexture((int)TransitionHelper.MainGameViewSize.x, (int)TransitionHelper.MainGameViewSize.y, 24);
				}
				return this.introRenderTexture;
			}
			set
			{
			}
		}

		private RenderTexture OutroRenderTexture
		{
			get
			{
				if (this.outroRenderTexture == null)
				{
					this.outroRenderTexture = new RenderTexture((int)TransitionHelper.MainGameViewSize.x, (int)TransitionHelper.MainGameViewSize.y, 24);
				}
				return this.outroRenderTexture;
			}
			set
			{
			}
		}

		public Camera SourceCamera
		{
			get
			{
				return this.sourceCamera;
			}
			set
			{
				this.sourceCamera = value;
			}
		}

		public void InitializeTransition(Camera sourceCamera, Camera destinationCamera, List<Camera> additionalSourceCameras, List<Camera> additionalDestinationCameras, TypeOfTransition transitionType)
		{
			if (sourceCamera == null || destinationCamera == null)
			{
				Debug.LogError(string.Format("Cannot create a transition with sourceCamera({0}) and destinationCamera({1}), one of them is null", sourceCamera, destinationCamera));
			}
			this.renderMaterial = new Material(Resources.Load(string.Format("Transitions/WellFired{0}", transitionType.ToString()), typeof(Material)) as Material);
			if (this.renderMaterial == null)
			{
				Debug.LogError(string.Format("Couldn't load render material for {0}", transitionType));
			}
			this.sourceCamera = sourceCamera;
			this.destinationCamera = destinationCamera;
			this.additionalSourceCameras = additionalSourceCameras;
			this.additionalDestinationCameras = additionalDestinationCameras;
			this.prevIntroCameraState = this.sourceCamera.enabled;
			this.prevOutroCameraState = this.destinationCamera.enabled;
		}

		public void ProcessTransitionFromOnGUI()
		{
			if (!this.shouldRender)
			{
				return;
			}
			this.renderMaterial.SetTexture("_SecondTex", this.OutroRenderTexture);
			this.renderMaterial.SetFloat("_Alpha", this.ratio);
			Graphics.Blit(this.IntroRenderTexture, null, this.renderMaterial);
		}

		public void ProcessEventFromNoneOnGUI(float deltaTime, float duration)
		{
			this.sourceCamera.enabled = false;
			this.destinationCamera.enabled = false;
			this.sourceCamera.targetTexture = this.IntroRenderTexture;
			this.sourceCamera.Render();
			for (int i = 0; i < this.additionalSourceCameras.Count; i++)
			{
				Camera camera = this.additionalSourceCameras[i];
				if (camera)
				{
					camera.targetTexture = this.IntroRenderTexture;
					camera.Render();
				}
			}
			this.destinationCamera.targetTexture = this.OutroRenderTexture;
			this.destinationCamera.Render();
			for (int j = 0; j < this.additionalDestinationCameras.Count; j++)
			{
				Camera camera2 = this.additionalDestinationCameras[j];
				if (camera2)
				{
					camera2.targetTexture = this.OutroRenderTexture;
					camera2.Render();
				}
			}
			this.ratio = 1f - deltaTime / duration;
			this.shouldRender = true;
		}

		public void TransitionComplete()
		{
			this.shouldRender = false;
			this.destinationCamera.enabled = true;
			this.destinationCamera.targetTexture = null;
			this.sourceCamera.enabled = false;
			this.sourceCamera.targetTexture = null;
			foreach (Camera current in this.additionalSourceCameras)
			{
				if (current)
				{
					current.targetTexture = null;
				}
			}
			foreach (Camera current2 in this.additionalDestinationCameras)
			{
				if (current2)
				{
					current2.targetTexture = null;
				}
			}
		}

		public void RevertTransition()
		{
			if (this.sourceCamera != null)
			{
				this.sourceCamera.enabled = this.prevIntroCameraState;
				this.sourceCamera.targetTexture = null;
			}
			if (this.destinationCamera != null)
			{
				this.destinationCamera.enabled = this.prevOutroCameraState;
				this.destinationCamera.targetTexture = null;
			}
			foreach (Camera current in this.additionalSourceCameras)
			{
				if (current)
				{
					current.targetTexture = null;
				}
			}
			foreach (Camera current2 in this.additionalDestinationCameras)
			{
				if (current2)
				{
					current2.targetTexture = null;
				}
			}
			UnityEngine.Object.DestroyImmediate(this.IntroRenderTexture);
			UnityEngine.Object.DestroyImmediate(this.OutroRenderTexture);
			this.shouldRender = false;
		}
	}
}
