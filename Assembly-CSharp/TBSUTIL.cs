using System;
using UnityEngine;

public class TBSUTIL
{
	public static Color NewHSV(float r, float g, float b, float a)
	{
		return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
	}

	public static Color NewHSV(Color _RGB)
	{
		return new Color(_RGB.r / 255f, _RGB.g / 255f, _RGB.b / 255f, _RGB.a / 255f);
	}

	public static GameObject Attach(string _Name, GameObject _Parent)
	{
		GameObject gameObject = new GameObject(_Name);
		if (_Parent)
		{
			gameObject.transform.parent = _Parent.transform;
		}
		TBSUTIL.Initilize(gameObject);
		return gameObject;
	}

	public static GameObject Attach(GameObject _Child, GameObject _Parent)
	{
		_Child.transform.parent = _Parent.transform;
		return _Child;
	}

	public static int Abs(int _value)
	{
		return Mathf.Abs(_value);
	}

	public static float ClosetUnit(float _Value, float _ClosetUnit)
	{
		_Value = (float)((int)(_Value / _ClosetUnit));
		_Value *= _ClosetUnit;
		return _Value;
	}

	public static void Initilize(GameObject _Target)
	{
		Transform transform = _Target.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	public static void Rocate(GameObject _Src, GameObject _Des)
	{
		Transform transform = _Src.transform;
		Transform transform2 = _Des.transform;
		transform2.position = transform.position;
		transform2.rotation = transform.rotation;
	}

	public static void Rocate(Vector3 _Src, GameObject _Des)
	{
		Transform transform = _Des.transform;
		transform.position = _Src;
	}

	public static void SetShaderPropertyColor(GameObject _Des, string Property, Color _Color)
	{
		Renderer[] componentsInChildren = _Des.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			if (!(renderer.tag == TsTag.UI_BUNDLE.ToString()))
			{
				Material[] materials = renderer.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					Material material = materials[j];
					material.SetColor(Property, _Color);
				}
			}
		}
	}

	public static void SetShaderPropertyColor(GameObject _Des, string Property, Color _Color, bool _IncludeInactive)
	{
		Renderer[] componentsInChildren = _Des.GetComponentsInChildren<Renderer>(_IncludeInactive);
		Renderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			if (!(renderer.tag == TsTag.UI_BUNDLE.ToString()))
			{
				Material[] materials = renderer.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					Material material = materials[j];
					material.SetColor(Property, _Color);
				}
			}
		}
	}

	public static void SetShaderPropertyFloat(GameObject _Des, string Property, float Value)
	{
		Renderer[] componentsInChildren = _Des.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			if (!(renderer.tag == TsTag.UI_BUNDLE.ToString()))
			{
				Material[] materials = renderer.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					Material material = materials[j];
					material.SetFloat(Property, Value);
				}
			}
		}
	}

	public static void SetParticleEmitter(GameObject _Des, bool bOn)
	{
		if (null != _Des)
		{
			ParticleSystem[] componentsInChildren = _Des.GetComponentsInChildren<ParticleSystem>();
			ParticleSystem[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				ParticleSystem particleSystem = array[i];
				particleSystem.enableEmission = bOn;
			}
			ParticleEmitter[] componentsInChildren2 = _Des.GetComponentsInChildren<ParticleEmitter>();
			ParticleEmitter[] array2 = componentsInChildren2;
			for (int j = 0; j < array2.Length; j++)
			{
				ParticleEmitter particleEmitter = array2[j];
				particleEmitter.emit = bOn;
			}
		}
	}

	public static void ChangeShader(GameObject _Des, string shadername)
	{
		Shader shader = Shader.Find(shadername);
		if (shader == null)
		{
			return;
		}
		Renderer[] componentsInChildren = _Des.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			if (!(renderer.tag == TsTag.UI_BUNDLE.ToString()))
			{
				Material[] materials = renderer.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					Material material = materials[j];
					material.shader = shader;
				}
			}
		}
	}

	public static Transform TraceParent(Transform _Des)
	{
		if (null != _Des && null != _Des.parent)
		{
			return _Des.parent;
		}
		return null;
	}

	public static string TraceParentDebugName(GameObject _Des)
	{
		Transform transform = _Des.transform;
		string text = string.Empty;
		int num = 10;
		for (int i = 0; i < num; i++)
		{
			transform = TBSUTIL.TraceParent(transform);
			if (!(null != transform))
			{
				break;
			}
			text = string.Format("{0}:{1}", transform.name, text);
		}
		return text;
	}
}
