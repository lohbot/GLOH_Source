using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[AddComponentMenu("TsScripts/TsDistCulling")]
public class TsDistCulling : MonoBehaviour
{
	private const int LAYER_COUNT = 32;

	private const string DEFAULT_XML = "XML/TsDistanceCulling";

	private float[] m_ScaledDist = new float[32];

	[SerializeField]
	private Camera m_TargetCamera;

	[SerializeField]
	private float[] m_Distances = new float[32];

	[SerializeField]
	private bool m_AutoLoading = true;

	public float this[int index]
	{
		get
		{
			return this.m_Distances[index];
		}
		set
		{
			this.m_Distances[index] = value;
		}
	}

	public bool AutoLoading
	{
		get
		{
			return this.m_AutoLoading;
		}
		set
		{
			this.m_AutoLoading = value;
		}
	}

	public Camera TargetCamera
	{
		get
		{
			return this.m_TargetCamera;
		}
		set
		{
			this.m_TargetCamera = value;
		}
	}

	private void Awake()
	{
		for (int i = 0; i < this.m_Distances.Length; i++)
		{
			this.m_Distances[i] = 0f;
		}
	}

	private void Start()
	{
		if (this.m_AutoLoading)
		{
			this.Load();
		}
		else
		{
			this.Apply();
		}
	}

	private void OnEnable()
	{
		this.Apply();
	}

	public void SetValue(int nLayer, float fDist)
	{
		try
		{
			this.m_Distances[nLayer] = fDist;
			this.Apply();
		}
		catch (IndexOutOfRangeException arg)
		{
			Debug.LogError("TsDistCulling.SetValue() => failed (" + arg + ")");
		}
	}

	public void Apply()
	{
		if (this.m_TargetCamera == null)
		{
			this.m_TargetCamera = Camera.main;
		}
		if (this.m_TargetCamera)
		{
			this.m_TargetCamera.layerCullDistances = this.m_Distances;
		}
	}

	public void Apply(float scale)
	{
		if (this.m_TargetCamera == null)
		{
			this.m_TargetCamera = Camera.main;
		}
		for (int i = 0; i < this.m_Distances.Length; i++)
		{
			this.m_ScaledDist[i] = this.m_Distances[i] * scale;
		}
		this.m_TargetCamera.layerCullDistances = this.m_ScaledDist;
	}

	private bool _CopyFrom(ref float[] srcLayerDist)
	{
		if (this.m_Distances.Length != srcLayerDist.Length)
		{
			return false;
		}
		for (int i = 0; i < this.m_Distances.Length; i++)
		{
			this.m_Distances[i] = srcLayerDist[i];
		}
		return true;
	}

	public bool Load(Stream stream)
	{
		bool flag = false;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(float[]));
			float[] array = xmlSerializer.Deserialize(stream) as float[];
			flag = this._CopyFrom(ref array);
			if (flag)
			{
				this.Apply();
			}
		}
		finally
		{
			if (stream != null)
			{
				stream.Close();
			}
		}
		return flag;
	}

	public bool Load()
	{
		TextAsset textAsset = Resources.Load("XML/TsDistanceCulling", typeof(TextAsset)) as TextAsset;
		return textAsset != null && this.Load(new MemoryStream(textAsset.bytes));
	}

	public bool Save(Stream stream)
	{
		bool result = false;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(float[]));
			xmlSerializer.Serialize(new XmlTextWriter(stream, Encoding.UTF8)
			{
				Formatting = Formatting.Indented,
				IndentChar = '\t',
				Indentation = 1
			}, this.m_Distances);
			result = true;
		}
		finally
		{
			if (stream != null)
			{
				stream.Close();
			}
		}
		return result;
	}

	public bool Save()
	{
		Directory.CreateDirectory("Resources");
		string path = "Assets/Resources/XML/TsDistanceCulling.xml";
		return this.Save(new FileStream(path, FileMode.Create, FileAccess.Write));
	}
}
