using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TsBundle;
using UnityEngine;

public class NmLocalFileList : MonoBehaviour
{
	public class BundleFileInfo
	{
		public string strFileName = string.Empty;

		public string strFolderName = string.Empty;

		public BundleFileInfo(string strFile, string strFolder)
		{
			this.strFileName = strFile;
			this.strFolderName = strFolder;
		}

		public string GetFullPath()
		{
			return string.Format("{0}/{1}", this.strFolderName, this.strFileName);
		}
	}

	public string strBaseBundleFolder = "D:/AT2_WEB/BUNDLE";

	public string strDefaultSearchPattern = "*.assetbundle";

	public string strCurrentBundleFolder = string.Empty;

	private bool bFadeIn;

	private bool m_bEnableOutLine;

	private NmLocalFileList.BundleFileInfo CurrentBundleFileInfo;

	public List<NmLocalFileList.BundleFileInfo> lstBundleFileInfo;

	public GameObject goClone;

	public List<string> lstAniName = new List<string>();

	public Dictionary<string, GameObject> dicAniObject = new Dictionary<string, GameObject>();

	public maxCamera maxCam;

	public Rect rectPosition = new Rect(0f, 0f, 200f, 300f);

	public Vector2 v2ScrollPostion = Vector2.zero;

	private GUIStyle styleButton;

	private bool m_bInit;

	public void Awake()
	{
		if (!this.m_bInit)
		{
			NrTSingleton<NrGlobalReference>.Instance.Init();
			this.m_bInit = true;
		}
		this.rectPosition = new Rect(0f, 0f, 400f, (float)Screen.height * 0.7f);
		this.AttachCamera();
	}

	public void AttachCamera()
	{
		GameObject gameObject = GameObject.Find("Main Camera");
		if (null == gameObject)
		{
			gameObject = new GameObject("Main Camera");
			gameObject.tag = "MainCamera";
		}
		this.maxCam = gameObject.AddComponent<maxCamera>();
		this.maxCam.UseCameraLevel = false;
		this.maxCam.m_bToolCamera = true;
		this.maxCam.fieldOfView = 35f;
		this.maxCam.currentDistance = (this.maxCam.desiredDistance = (this.maxCam.distance = 5f));
		this.maxCam.minDistance = 0.1f;
		this.maxCam.maxDistance = 1000f;
	}

	private void Update()
	{
		if (this.maxCam)
		{
			if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				maxCamera expr_2A_cp_0 = this.maxCam;
				expr_2A_cp_0.targetOffset.y = expr_2A_cp_0.targetOffset.y - 0.1f;
			}
			if (Input.GetKeyUp(KeyCode.DownArrow))
			{
				maxCamera expr_55_cp_0 = this.maxCam;
				expr_55_cp_0.targetOffset.y = expr_55_cp_0.targetOffset.y + 0.1f;
			}
		}
		if (this.bFadeIn)
		{
			this.FadeIn();
		}
	}

	private void OnGUI_AssetViewer(int nID)
	{
		this.v2ScrollPostion = GUILayout.BeginScrollView(this.v2ScrollPostion, new GUILayoutOption[0]);
		if (this.lstBundleFileInfo != null)
		{
			this.styleButton = new GUIStyle("button");
			this.styleButton.alignment = TextAnchor.MiddleLeft;
			foreach (NmLocalFileList.BundleFileInfo current in this.lstBundleFileInfo)
			{
				if (this.CurrentBundleFileInfo == current)
				{
					GUILayout.Label(current.strFileName, new GUILayoutOption[0]);
				}
				else if (GUILayout.Button(current.strFileName, this.styleButton, new GUILayoutOption[0]))
				{
					this.RequestBundelDownload(current);
					this.CurrentBundleFileInfo = current;
				}
			}
		}
		GUILayout.EndScrollView();
		if (GUILayout.Button("Open Folder", new GUILayoutOption[0]))
		{
			this.OpenAssetBundelFolder();
		}
		GUI.DragWindow();
	}

	private void OnGUI_AniList()
	{
		if (null == this.goClone)
		{
			return;
		}
		float left = (float)(Screen.width - 100);
		float top = 0f;
		GUILayout.BeginArea(new Rect(left, top, 100f, (float)Screen.height - 40f));
		foreach (string current in this.lstAniName)
		{
			if (GUILayout.Button(current, new GUILayoutOption[0]))
			{
				GameObject gameObject = this.dicAniObject[current];
				if (gameObject != null)
				{
					Animation componentInChildren = gameObject.GetComponentInChildren<Animation>();
					if (null == componentInChildren)
					{
						return;
					}
					if (componentInChildren.isPlaying)
					{
						componentInChildren.Stop();
					}
					componentInChildren.Play(current);
				}
			}
		}
		GUILayout.EndArea();
	}

	private void OnGui_OutLine()
	{
		if (null == this.goClone)
		{
			return;
		}
		float left = (float)(Screen.width - 100);
		float top = (float)(Screen.height - 200);
		GUILayout.BeginArea(new Rect(left, top, 100f, 200f));
		this.m_bEnableOutLine = GUILayout.Toggle(this.m_bEnableOutLine, "Show OutLine", new GUILayoutOption[0]);
		GUILayout.EndArea();
	}

	private void OpenAssetBundelFolder()
	{
		if (string.IsNullOrEmpty(this.strCurrentBundleFolder))
		{
			return;
		}
		string[] array = this._GetFiles(this.strCurrentBundleFolder, this.strDefaultSearchPattern, SearchOption.AllDirectories);
		if (array == null || 0 >= array.Length)
		{
			return;
		}
		this.lstBundleFileInfo = new List<NmLocalFileList.BundleFileInfo>();
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i];
			FileInfo fileInfo = new FileInfo(text);
			NmLocalFileList.BundleFileInfo item = new NmLocalFileList.BundleFileInfo(fileInfo.Name, text.Substring(text.IndexOf('\\') + 1));
			this.lstBundleFileInfo.Add(item);
		}
		this.strCurrentBundleFolder += "/";
		Option.localWWW = true;
		Option.SetProtocolRootPath(Protocol.FILE, "/" + this.strCurrentBundleFolder);
	}

	private void RequestBundelDownload(NmLocalFileList.BundleFileInfo kBundleFileInfo)
	{
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(kBundleFileInfo.strFolderName, null);
		if (wWWItem == null)
		{
			return;
		}
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this._OnCompletedDownload), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, false);
	}

	private void _OnCompletedDownload(IDownloadedItem wItem, object obj)
	{
		GameObject gameObject = NkUtil.FindOrCreate("BasePos");
		if (gameObject)
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		UnityEngine.Debug.Log(wItem.assetPath + "/" + Time.time);
		GameObject gameObject2 = wItem.mainAsset as GameObject;
		if (gameObject2 == null)
		{
			base.StartCoroutine(this.StartMapLoad(wItem.url));
		}
		else
		{
			this.goClone = (GameObject)UnityEngine.Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity);
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.goClone);
			}
			this._GetAnimationList();
			gameObject = NkUtil.FindOrCreate("BasePos");
			this.Awake();
			this.goClone.transform.parent = gameObject.transform;
			if (null == this.maxCam)
			{
				return;
			}
			this.maxCam.target = this.goClone.transform;
			this.maxCam.minDistance = 0.1f;
			this.maxCam.maxDistance = 1000f;
			this.bFadeIn = true;
		}
	}

	[DebuggerHidden]
	private IEnumerator StartMapLoad(string szurl)
	{
		NmLocalFileList.<StartMapLoad>c__Iterator6C <StartMapLoad>c__Iterator6C = new NmLocalFileList.<StartMapLoad>c__Iterator6C();
		<StartMapLoad>c__Iterator6C.szurl = szurl;
		<StartMapLoad>c__Iterator6C.<$>szurl = szurl;
		<StartMapLoad>c__Iterator6C.<>f__this = this;
		return <StartMapLoad>c__Iterator6C;
	}

	private void _GetAnimationList()
	{
		this.lstAniName.Clear();
		this.dicAniObject.Clear();
		if (null == this.goClone)
		{
			return;
		}
		Animation[] componentsInChildren = this.goClone.GetComponentsInChildren<Animation>();
		if (componentsInChildren != null && componentsInChildren.Length > 0)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Animation animation = componentsInChildren[i];
				foreach (AnimationState animationState in animation)
				{
					if (!this.lstAniName.Contains(animationState.clip.name))
					{
						this.lstAniName.Add(animationState.clip.name);
						this.dicAniObject.Add(animationState.clip.name, animation.gameObject);
					}
				}
			}
		}
		Animator[] componentsInChildren2 = this.goClone.GetComponentsInChildren<Animator>();
		if (componentsInChildren2 != null && componentsInChildren2.Length > 0)
		{
			Animator[] array = componentsInChildren2;
			for (int j = 0; j < array.Length; j++)
			{
				Animator animator = array[j];
				for (int i = 0; i < animator.layerCount; i++)
				{
					AnimationInfo[] currentAnimationClipState = animator.GetCurrentAnimationClipState(i);
					AnimationInfo[] array2 = currentAnimationClipState;
					for (int k = 0; k < array2.Length; k++)
					{
						AnimationInfo animationInfo = array2[k];
						this.lstAniName.Add(animationInfo.clip.name);
						this.dicAniObject.Add(animationInfo.clip.name, animator.gameObject);
					}
				}
			}
		}
	}

	private string[] _GetFiles(string strPath, string strPattern, SearchOption eOption)
	{
		string[] result = null;
		if (eOption != SearchOption.TopDirectoryOnly)
		{
			if (eOption == SearchOption.AllDirectories)
			{
				result = this._GetFilesAll(strPath, strPattern);
			}
		}
		else
		{
			result = Directory.GetFiles(strPath, strPattern);
		}
		return result;
	}

	private string[] _GetFilesAll(string strPath, string strPattern)
	{
		List<string> list = new List<string>();
		string[] directories = Directory.GetDirectories(strPath);
		if (directories != null && 0 < directories.Length)
		{
			string[] array = directories;
			for (int i = 0; i < array.Length; i++)
			{
				string strPath2 = array[i];
				list.AddRange(this._GetFilesAll(strPath2, strPattern));
			}
		}
		list.AddRange(Directory.GetFiles(strPath, strPattern));
		return list.ToArray();
	}

	private void FadeIn()
	{
		GameObject x = this.goClone;
		if (x == null)
		{
			return;
		}
		this.bFadeIn = false;
	}
}
