using System;
using System.Collections.Generic;
using UnityEngine;

public class AssetViewer : MonoBehaviour
{
	private static AssetViewer Instance;

	public string URL_PATH = string.Empty;

	public GameObject MainObject;

	public bool bPathLoad = true;

	public GUIStyle ActionStyle;

	private List<string> AniList = new List<string>();

	private List<GameObject> AniObj = new List<GameObject>();

	public string MainInfo = "INFO:";

	public Rect position;

	public static AssetViewer GetInstance()
	{
		return AssetViewer.Instance;
	}

	private void Awake()
	{
		AssetViewer.Instance = this;
		this.InitCamera();
		this.MainObject = new GameObject("MainObject");
	}

	private void Start()
	{
	}

	private void InitCamera()
	{
	}

	public void LoadObject(string _Paht)
	{
		if (0 < _Paht.Length)
		{
			if (this.MainObject)
			{
				UnityEngine.Object.Destroy(this.MainObject);
				this.MainObject = new GameObject("MainObject");
			}
			this.AniList.Clear();
			this.AniObj.Clear();
			WBundleLoader.Load(this.MainObject, _Paht);
			this.bPathLoad = false;
		}
	}

	private void PathLoadGUI()
	{
		GUILayout.BeginArea(new Rect((float)(Screen.width / 2) * 0.8f, (float)(Screen.height / 2) * 0.8f, 200f, 200f));
		this.URL_PATH = GUILayout.TextField(this.URL_PATH, new GUILayoutOption[0]);
		if (GUILayout.Button("LOAD", new GUILayoutOption[0]))
		{
			this.LoadObject(this.URL_PATH);
		}
		GUILayout.EndArea();
	}

	private void RegistAniList()
	{
		if (this.AniList.Count == 0 && this.MainObject)
		{
			Animation[] componentsInChildren = this.MainObject.GetComponentsInChildren<Animation>();
			if (componentsInChildren.Length > 0)
			{
				Animation[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					Animation animation = array[i];
					foreach (AnimationState animationState in animation)
					{
						this.AniList.Add(animationState.clip.name);
					}
					this.AniObj.Add(animation.gameObject);
				}
				if (0 < this.AniObj.Count && 0 < this.AniList.Count)
				{
					GameObject gameObject = this.AniObj[0].gameObject;
					Animation componentInChildren = gameObject.GetComponentInChildren<Animation>();
					this.MainInfo = string.Concat(new object[]
					{
						"INFO:",
						gameObject.name,
						" Ani:",
						componentInChildren.GetClipCount()
					});
				}
			}
			else
			{
				Animator[] componentsInChildren2 = this.MainObject.GetComponentsInChildren<Animator>();
				if (componentsInChildren2.Length > 0)
				{
					Animator[] array2 = componentsInChildren2;
					for (int j = 0; j < array2.Length; j++)
					{
						Animator animator = array2[j];
						for (int k = 0; k < animator.layerCount; k++)
						{
							AnimationInfo[] currentAnimationClipState = animator.GetCurrentAnimationClipState(k);
							AnimationInfo[] array3 = currentAnimationClipState;
							for (int l = 0; l < array3.Length; l++)
							{
								AnimationInfo animationInfo = array3[l];
								this.AniList.Add(animationInfo.clip.name);
							}
							this.AniObj.Add(animator.gameObject);
						}
					}
					if (0 < this.AniObj.Count && 0 < this.AniList.Count)
					{
						GameObject gameObject2 = this.AniObj[0].gameObject;
						Animator componentInChildren2 = gameObject2.GetComponentInChildren<Animator>();
						this.MainInfo = string.Concat(new object[]
						{
							"INFO:",
							gameObject2.name,
							" Ani:",
							componentInChildren2.layerCount
						});
					}
				}
			}
		}
	}

	private void InspectorGUI()
	{
		float left = (float)(Screen.width - 100);
		float top = 0f;
		GUILayout.BeginArea(new Rect(left, top, 100f, (float)Screen.height - 40f));
		if (0 < this.AniList.Count)
		{
			foreach (string current in this.AniList)
			{
				if (GUILayout.Button(current, new GUILayoutOption[0]))
				{
					foreach (GameObject current2 in this.AniObj)
					{
						if (current2.animation[current])
						{
							current2.animation.Play(current);
						}
					}
				}
			}
		}
		GUILayout.EndArea();
	}

	private void MainInfoGUI()
	{
		float num = 400f;
		float left = (float)(Screen.width / 2) - num / 2f;
		float top = (float)(Screen.height - 20);
		GUILayout.BeginArea(new Rect(left, top, num, 20f));
		GUILayout.Label(this.MainInfo, new GUILayoutOption[0]);
		GUILayout.EndArea();
	}

	private void Update()
	{
		if (WBundleLoader.IsComplete(base.gameObject))
		{
			this.RegistAniList();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.URL_PATH = string.Empty;
			FTPTreeGUI.isVisible = (this.bPathLoad = false);
			this.InitCamera();
		}
		if (Input.GetKeyUp(KeyCode.Insert))
		{
			this.bPathLoad = !this.bPathLoad;
		}
		if (Input.GetKeyUp(KeyCode.Home))
		{
			FTPTreeGUI.isVisible = !FTPTreeGUI.isVisible;
		}
	}
}
