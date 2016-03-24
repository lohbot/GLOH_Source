using GAME;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TsBundle;
using TsLibs;
using UnityEngine;

public class NkCutScene_Camera_Manager
{
	public static NkCutScene_Camera_Manager _instance;

	public List<CutScene_Camera> _listCamera = new List<CutScene_Camera>();

	public float Duration;

	private float fStartTime;

	private TsWeakReference<maxCamera> m_TargetCamera;

	private int nReservationCharKind;

	private eCharAnimationType eReservationAni = eCharAnimationType.None;

	private bool bLoadComplete;

	private bool bHide;

	private int nHideAlly = -1;

	public static NkCutScene_Camera_Manager Instance
	{
		get
		{
			if (NkCutScene_Camera_Manager._instance != null)
			{
				return NkCutScene_Camera_Manager._instance;
			}
			NkCutScene_Camera_Manager._instance = new NkCutScene_Camera_Manager();
			return NkCutScene_Camera_Manager._instance;
		}
	}

	public int ReservationCharKind
	{
		get
		{
			return this.nReservationCharKind;
		}
		set
		{
			this.nReservationCharKind = value;
		}
	}

	public eCharAnimationType ReservationAni
	{
		get
		{
			return this.eReservationAni;
		}
	}

	public bool LoadComplete
	{
		get
		{
			return this.bLoadComplete;
		}
	}

	public void Clear()
	{
		this._listCamera.Clear();
		this.Duration = 0f;
		this.fStartTime = 0f;
		if (this.m_TargetCamera != null && this.m_TargetCamera.CastedTarget != null)
		{
			this.m_TargetCamera.CastedTarget.enabled = true;
			this.m_TargetCamera.CastedTarget.RestoreCameraInfo();
		}
		this.m_TargetCamera = null;
		this.nReservationCharKind = 0;
		this.eReservationAni = eCharAnimationType.None;
		this.bLoadComplete = false;
		Battle.BATTLE.GRID_MANAGER.ShowHideGrid(true);
		GameObject gameObject = GameObject.Find("UI Camera");
		if (gameObject != null)
		{
			Camera componentInChildren = gameObject.GetComponentInChildren<Camera>();
			if (componentInChildren != null)
			{
				componentInChildren.enabled = true;
			}
		}
		if (this.bHide)
		{
			NrTSingleton<NkBattleCharManager>.Instance.ChangeCharLayer((eBATTLE_ALLY)this.nHideAlly, -1, true, true);
			this.bHide = false;
			this.nHideAlly = -1;
		}
	}

	public void Update()
	{
		if (this.fStartTime == 0f)
		{
			return;
		}
		float num = Time.realtimeSinceStartup - this.fStartTime;
		for (int i = 0; i < this._listCamera.Count; i++)
		{
			if (this._listCamera[i] != null && this._listCamera[i].Update(num))
			{
				break;
			}
		}
		if (num > this.Duration)
		{
			this.Clear();
		}
	}

	public float GetDurationTotal()
	{
		return this.Duration;
	}

	public void StartCutScene()
	{
		this.fStartTime = Time.realtimeSinceStartup;
		maxCamera component = Camera.main.GetComponent<maxCamera>();
		if (component != null)
		{
			this.m_TargetCamera = component;
			this.m_TargetCamera.CastedTarget.BackUpCameraInfo();
			this.m_TargetCamera.CastedTarget.enabled = false;
		}
		if (this.bHide)
		{
			NrTSingleton<NkBattleCharManager>.Instance.ChangeCharLayer((eBATTLE_ALLY)this.nHideAlly, -1, false, true);
		}
		NrTSingleton<NkBattleCharManager>.Instance.AllCharNameHide(false);
		Battle.BATTLE.GRID_MANAGER.ShowHideGrid(false);
		GameObject gameObject = GameObject.Find("UI Camera");
		if (gameObject != null)
		{
			Camera componentInChildren = gameObject.GetComponentInChildren<Camera>();
			if (componentInChildren != null)
			{
				componentInChildren.enabled = false;
			}
		}
	}

	public void SortCameraTimeLine()
	{
		this._listCamera.Sort(new Comparison<CutScene_Camera>(this.CompareToFileTime));
	}

	private int CompareToFileTime(CutScene_Camera left, CutScene_Camera right)
	{
		return right._fFireTime.CompareTo(left._fFireTime);
	}

	public bool ReadCutScneData(string fileName, int nCharKind, eCharAnimationType eAni, int nHide, int nAlly)
	{
		this.Clear();
		string text = string.Empty;
		if (!NrTSingleton<NrGlobalReference>.Instance.isLoadWWW)
		{
			string text2 = Option.GetProtocolRootPath(Protocol.FILE);
			text2 = text2.Substring("file:///".Length, text2.Length - "file:///".Length);
			text = string.Format("{0}{1}{2}.xml", text2, CDefinePath.XMLPath(), fileName);
			this.ReadXML(text, null);
		}
		else
		{
			ItemType itemType;
			if (!NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				text = string.Format("{0}{1}.xml", CDefinePath.XMLPath(), fileName);
				itemType = ItemType.USER_STRING;
			}
			else
			{
				if (TsPlatform.IsMobile)
				{
					text = string.Format("{0}{1}_mobile{2}", CDefinePath.XMLBundlePath(), fileName, Option.extAsset);
				}
				else
				{
					text = string.Format("{0}{1}{2}", CDefinePath.XMLBundlePath(), fileName, Option.extAsset);
				}
				itemType = ItemType.USER_ASSETB;
			}
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(text, null);
			wWWItem.SetItemType(itemType);
			wWWItem.SetCallback(new PostProcPerItem(this.ReadXML), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		this.bLoadComplete = false;
		this.nReservationCharKind = nCharKind;
		this.eReservationAni = eAni;
		if (nHide != 0)
		{
			this.bHide = true;
			this.nHideAlly = nAlly;
		}
		return true;
	}

	public void ReadXML(IDownloadedItem wItem, object kParmObj)
	{
		XmlReader xmlReader = null;
		if (wItem.canAccessString)
		{
			MemoryStream stream = new MemoryStream(NrXmlSerializer.StringToUTF8ByteArray(wItem.safeString));
			xmlReader = XmlReader.Create(stream);
		}
		else if (wItem.canAccessAssetBundle)
		{
			GameObject gameObject = wItem.mainAsset as GameObject;
			if (null != gameObject)
			{
				TsGameDataAdapter component = gameObject.GetComponent<TsGameDataAdapter>();
				TsGameData gameData = component.GameData;
				MemoryStream stream2 = new MemoryStream(NrXmlSerializer.StringToUTF8ByteArray(gameData.serializeGameDatas[0]));
				xmlReader = XmlReader.Create(stream2);
			}
			wItem.unloadImmediate = true;
		}
		if (xmlReader == null)
		{
			return;
		}
		try
		{
			while (xmlReader.Read())
			{
				if (xmlReader.NodeType == XmlNodeType.Element && !xmlReader.IsEmptyElement)
				{
					if (xmlReader.Name == typeof(Camera).Name)
					{
						CutScene_Camera cutScene_Camera = new CutScene_Camera();
						cutScene_Camera.CameraName = xmlReader.GetAttribute("Name");
						int fov = 35;
						if (int.TryParse(xmlReader.GetAttribute("FOV"), out fov))
						{
							cutScene_Camera._fov = fov;
						}
						else
						{
							cutScene_Camera._fov = 35;
							Debug.LogError("Cannot find XML Setting : FOV Default Settings -> 35");
						}
						this.ReadCamera(xmlReader, ref cutScene_Camera);
						this._listCamera.Add(cutScene_Camera);
					}
					else if (xmlReader.Name == "USequenceData")
					{
						string attribute = xmlReader.GetAttribute("Duration");
						this.Duration = float.Parse(attribute);
					}
				}
			}
			this.SortCameraTimeLine();
		}
		catch (Exception ex)
		{
			TsLog.LogError(ex.Message + " " + ex.StackTrace, new object[0]);
			return;
		}
		if (this.nReservationCharKind == 0 && this.eReservationAni == eCharAnimationType.None)
		{
			this.StartCutScene();
		}
		this.bLoadComplete = true;
	}

	public void ReadXML(string path, object kParmObj)
	{
		string fileString = new TsDataReader
		{
			UseMD5 = true
		}.GetFileString(path);
		MemoryStream stream = new MemoryStream(NrXmlSerializer.StringToUTF8ByteArray(fileString));
		XmlReader xmlReader = XmlReader.Create(stream);
		if (xmlReader == null)
		{
			return;
		}
		try
		{
			while (xmlReader.Read())
			{
				if (xmlReader.NodeType == XmlNodeType.Element && !xmlReader.IsEmptyElement)
				{
					if (xmlReader.Name == typeof(Camera).Name)
					{
						CutScene_Camera cutScene_Camera = new CutScene_Camera();
						cutScene_Camera.CameraName = xmlReader.GetAttribute("Name");
						int fov = 35;
						if (int.TryParse(xmlReader.GetAttribute("FOV"), out fov))
						{
							cutScene_Camera._fov = fov;
						}
						else
						{
							cutScene_Camera._fov = 35;
							Debug.LogError("Cannot find XML Setting : FOV Default Settings -> 35");
						}
						this.ReadCamera(xmlReader, ref cutScene_Camera);
						this._listCamera.Add(cutScene_Camera);
					}
					else if (xmlReader.Name == "USequenceData")
					{
						string attribute = xmlReader.GetAttribute("Duration");
						this.Duration = float.Parse(attribute);
					}
				}
			}
			this.SortCameraTimeLine();
		}
		catch (Exception ex)
		{
			TsLog.LogError(ex.Message + " " + ex.StackTrace, new object[0]);
			return;
		}
		if (this.nReservationCharKind != 0 || this.eReservationAni == eCharAnimationType.None)
		{
		}
		this.bLoadComplete = true;
	}

	public bool ReadCamera(XmlReader reader, ref CutScene_Camera camera)
	{
		bool flag = true;
		try
		{
			while (flag)
			{
				if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
				{
					if (reader.Name.CompareTo("localPosition") == 0)
					{
						this.ReadLocalPosition(reader, ref camera);
					}
					else if (reader.Name.CompareTo("localRotation") == 0)
					{
						this.ReadLocalRotation(reader, ref camera);
					}
				}
				else if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.Read();
					break;
				}
				flag = reader.Read();
			}
		}
		catch (Exception ex)
		{
			TsLog.LogError(ex.Message + " " + ex.StackTrace, new object[0]);
			return false;
		}
		return true;
	}

	public bool ReadLocalPosition(XmlReader reader, ref CutScene_Camera camera)
	{
		string s = string.Empty;
		bool flag = true;
		try
		{
			while (flag)
			{
				if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement && reader.Name.CompareTo("localPosition") == 0)
				{
					s = reader.GetAttribute("x");
					camera._StartPosition.x = float.Parse(s);
					s = reader.GetAttribute("y");
					camera._StartPosition.y = float.Parse(s);
					s = reader.GetAttribute("z");
					camera._StartPosition.z = float.Parse(s);
					this.ReadIntermalCurve(reader, ref camera._curvePosition[0]);
					this.ReadIntermalCurve(reader, ref camera._curvePosition[1]);
					this.ReadIntermalCurve(reader, ref camera._curvePosition[2]);
					int length = camera._curvePosition[0].length;
					camera._fFireTime = camera._curvePosition[0][0].time;
					camera._fDuration = camera._curvePosition[0][length - 1].time - camera._fFireTime;
				}
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					break;
				}
				flag = reader.Read();
			}
		}
		catch (Exception ex)
		{
			TsLog.LogError(ex.Message + " " + ex.StackTrace, new object[0]);
			return false;
		}
		return true;
	}

	public bool ReadLocalRotation(XmlReader reader, ref CutScene_Camera camera)
	{
		string s = string.Empty;
		bool flag = true;
		try
		{
			while (flag)
			{
				if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement && reader.Name.CompareTo("localRotation") == 0)
				{
					s = reader.GetAttribute("x");
					camera._StartRotation.x = float.Parse(s);
					s = reader.GetAttribute("y");
					camera._StartRotation.y = float.Parse(s);
					s = reader.GetAttribute("z");
					camera._StartRotation.z = float.Parse(s);
					s = reader.GetAttribute("w");
					camera._StartRotation.w = float.Parse(s);
					this.ReadIntermalCurve(reader, ref camera._curveRotation[0]);
					this.ReadIntermalCurve(reader, ref camera._curveRotation[1]);
					this.ReadIntermalCurve(reader, ref camera._curveRotation[2]);
					this.ReadIntermalCurve(reader, ref camera._curveRotation[3]);
					int length = camera._curvePosition[0].length;
					camera._fDuration = camera._curvePosition[0][length - 1].time;
				}
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					break;
				}
				reader.Read();
			}
		}
		catch (Exception ex)
		{
			TsLog.LogError(ex.Message + " " + ex.StackTrace, new object[0]);
			return false;
		}
		return true;
	}

	public bool ReadIntermalCurve(XmlReader reader, ref AnimationCurve anicurve)
	{
		try
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element && reader.Name.CompareTo("USInternalCurve") == 0)
				{
					anicurve = new AnimationCurve();
					this.ReadAnimationCurve(reader, ref anicurve);
				}
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.Read();
					break;
				}
			}
		}
		catch (Exception ex)
		{
			TsLog.LogError(ex.Message + " " + ex.StackTrace, new object[0]);
			return false;
		}
		return true;
	}

	public bool ReadAnimationCurve(XmlReader reader, ref AnimationCurve anicurve)
	{
		try
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.Name.CompareTo("AnimationCurve") == 0)
					{
						string attribute = reader.GetAttribute("preWrapMode");
						if (!string.IsNullOrEmpty(attribute))
						{
							anicurve.preWrapMode = this.ConvertWrapMode(attribute);
						}
						string attribute2 = reader.GetAttribute("postWrapMode");
						if (!string.IsNullOrEmpty(attribute2))
						{
							anicurve.postWrapMode = this.ConvertWrapMode(attribute2);
						}
					}
					else if (reader.Name.CompareTo("Keyframe") == 0)
					{
						Keyframe key = default(Keyframe);
						if (this.ReadKeyFrame(reader, ref key))
						{
							anicurve.AddKey(key);
						}
					}
				}
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.Read();
					break;
				}
			}
		}
		catch (Exception ex)
		{
			TsLog.LogError(ex.Message + " " + ex.StackTrace, new object[0]);
			return false;
		}
		return true;
	}

	private WrapMode ConvertWrapMode(string wrapmode)
	{
		IEnumerator enumerator = Enum.GetValues(typeof(WrapMode)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				WrapMode wrapMode = (WrapMode)((int)enumerator.Current);
				if (wrapmode == wrapMode.ToString())
				{
					return wrapMode;
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return WrapMode.Default;
	}

	public bool ReadKeyFrame(XmlReader reader, ref Keyframe keyframe)
	{
		try
		{
			string s = string.Empty;
			s = reader.GetAttribute("inTangent");
			keyframe.inTangent = float.Parse(s);
			s = reader.GetAttribute("outTangent");
			keyframe.outTangent = float.Parse(s);
			s = reader.GetAttribute("tangentMode");
			keyframe.tangentMode = int.Parse(s);
			s = reader.GetAttribute("time");
			keyframe.time = float.Parse(s);
			s = reader.GetAttribute("value");
			keyframe.value = float.Parse(s);
		}
		catch (Exception ex)
		{
			TsLog.LogError(ex.Message + " " + ex.StackTrace, new object[0]);
			return false;
		}
		return true;
	}
}
