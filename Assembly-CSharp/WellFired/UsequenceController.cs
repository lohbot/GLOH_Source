using System;
using UnityEngine;

namespace WellFired
{
	public class UsequenceController : MonoBehaviour
	{
		private enum LoadProgress
		{
			BUNDLE_LOAD,
			BUNDLE_EDIT,
			PLAY,
			END_PLAY
		}

		private struct camInfo
		{
			public Camera cam;

			public bool enabled;
		}

		private UsequenceController.LoadProgress sequenceProgress;

		private UsequenceBundleLoader Loader;

		private USSequencer _sequencer;

		private GameObject _destObj;

		public bool skip;

		private UsequenceController.camInfo[] camInfos;

		private void Update()
		{
			switch (this.sequenceProgress)
			{
			case UsequenceController.LoadProgress.BUNDLE_LOAD:
				this.LoadBundle();
				break;
			case UsequenceController.LoadProgress.BUNDLE_EDIT:
				if (this.IsDoneEdit())
				{
					if (this.skip)
					{
						this.sequenceProgress = UsequenceController.LoadProgress.END_PLAY;
						return;
					}
					this._sequencer.Play();
					this.sequenceProgress = UsequenceController.LoadProgress.PLAY;
				}
				break;
			case UsequenceController.LoadProgress.PLAY:
				if (this.skip)
				{
					this.sequenceProgress = UsequenceController.LoadProgress.END_PLAY;
					return;
				}
				if (!this.IsPlaying())
				{
					this.sequenceProgress = UsequenceController.LoadProgress.END_PLAY;
				}
				break;
			case UsequenceController.LoadProgress.END_PLAY:
				this.Dispose();
				break;
			}
		}

		private void LoadBundle()
		{
			try
			{
				if (this.Loader.DoneLoadBundle())
				{
					this._destObj = this.Loader.InstantiateFromLoad(false, null);
					if (this._destObj == null)
					{
						this.DestroyByNull("_destObj");
					}
					else
					{
						this._destObj.name = this._destObj.name.Replace("(Clone)", string.Empty);
						this._sequencer = this._destObj.GetComponentInChildren<USSequencer>();
						if (this._sequencer == null)
						{
							UnityEngine.Object.DestroyImmediate(this._destObj);
							this.DestroyByNull("_sequencer");
						}
						else
						{
							this.sequenceProgress = UsequenceController.LoadProgress.BUNDLE_EDIT;
						}
					}
				}
			}
			catch (Exception arg)
			{
				this.Loader.Dispose(true);
				Debug.Log("로드 오브젝트 처리 실패: " + arg);
			}
		}

		private void DestroyByNull(string nullTarget)
		{
			Debug.LogError(nullTarget + " 이 NULL 값 입니다.");
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}

		private bool IsDoneEdit()
		{
			UsequenceObjectReloader componentInChildren = this._sequencer.transform.root.GetComponentInChildren<UsequenceObjectReloader>();
			return componentInChildren.JobComplete;
		}

		private bool IsPlaying()
		{
			return !(this._sequencer == null) && this._sequencer.IsPlaying;
		}

		private void Dispose()
		{
			this._sequencer.Stop();
			if (this._destObj != null)
			{
				UsequenceObjectReloader component = this._destObj.GetComponent<UsequenceObjectReloader>();
				component.Dispose();
				UnityEngine.Object.Destroy(this._destObj);
			}
			this.Loader.Dispose(true);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void StartCutScene(string strPath)
		{
			this.CutSceneEnvironmentSet();
			this.Loader = new UsequenceBundleLoader();
			this.Loader.Init();
			this.Loader.SetAsset(strPath);
		}

		public void EndCutScene()
		{
			this._sequencer.Stop();
			this.CutSceneEnvironmentRevert();
			this.Dispose();
		}

		private void CutSceneEnvironmentSet()
		{
			this.UIWindows_Set();
			this.Sounds_Set();
			this.SetCamInfos();
		}

		private void CutSceneEnvironmentRevert()
		{
			this.UIWindows_Revert();
			this.Sounds_Revert();
			this.RevertCamInfos();
		}

		private void SetCamInfos()
		{
			int allCamerasCount = Camera.allCamerasCount;
			this.camInfos = new UsequenceController.camInfo[allCamerasCount];
			Camera[] allCameras = Camera.allCameras;
			for (int i = 0; i < allCamerasCount; i++)
			{
				UsequenceController.camInfo camInfo = default(UsequenceController.camInfo);
				camInfo.cam = allCameras[i];
				camInfo.enabled = allCameras[i].enabled;
				this.camInfos[i] = camInfo;
			}
		}

		private void RevertCamInfos()
		{
			if (this.camInfos == null)
			{
				return;
			}
			int num = this.camInfos.Length;
			for (int i = 0; i < num; i++)
			{
				this.camInfos[i].cam.enabled = this.camInfos[i].enabled;
				this.camInfos[i].cam = null;
			}
			this.camInfos = null;
		}

		private void UIWindows_Set()
		{
		}

		private void UIWindows_Revert()
		{
		}

		private void Sounds_Set()
		{
		}

		private void Sounds_Revert()
		{
		}
	}
}
