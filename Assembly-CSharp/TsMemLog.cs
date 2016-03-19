using System;
using System.Collections.Generic;
using UnityEngine;

public class TsMemLog
{
	private string title_;

	private int mem_;

	private int obj_;

	private int go_;

	private int comp_;

	private int texture_;

	public TsMemLog(string title)
	{
		this.title_ = title;
		this.mem_ = (int)Profiler.usedHeapSize;
		this.obj_ = UnityEngine.Object.FindObjectsOfType(typeof(UnityEngine.Object)).Length;
		this.go_ = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)).Length;
		this.comp_ = UnityEngine.Object.FindObjectsOfType(typeof(Component)).Length;
		this.texture_ = UnityEngine.Object.FindObjectsOfType(typeof(Component)).Length;
	}

	public static void Report()
	{
		Debug.Log(string.Concat(new object[]
		{
			"TsMemLog::Report\n\tHeap(",
			Profiler.usedHeapSize / 1048576u,
			")\n\tObject(",
			UnityEngine.Object.FindObjectsOfType(typeof(UnityEngine.Object)).Length,
			")\n\tGameObject(",
			UnityEngine.Object.FindObjectsOfType(typeof(GameObject)).Length,
			")"
		}));
	}

	public static void ObjectByType()
	{
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(UnityEngine.Object));
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		UnityEngine.Object[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			UnityEngine.Object @object = array2[i];
			if (!dictionary.ContainsKey(@object.GetType().FullName))
			{
				dictionary.Add(@object.GetType().FullName, 0);
			}
			Dictionary<string, int> dictionary2;
			Dictionary<string, int> expr_4E = dictionary2 = dictionary;
			string text;
			string expr_5C = text = @object.GetType().FullName;
			int num = dictionary2[text];
			expr_4E[expr_5C] = num + 1;
		}
		List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
		foreach (KeyValuePair<string, int> current in dictionary)
		{
			list.Add(current);
		}
		list.Sort((KeyValuePair<string, int> k1, KeyValuePair<string, int> k2) => k2.Value.CompareTo(k1.Value));
		string text2 = "TsMemLog::ObjectByType";
		foreach (KeyValuePair<string, int> current2 in list)
		{
			string text = text2;
			text2 = string.Concat(new object[]
			{
				text,
				"\n\t",
				current2.Key,
				" (",
				current2.Value,
				")"
			});
		}
		Debug.Log(text2);
	}

	public static Dictionary<Type, List<UnityEngine.Object>> ResourceByType()
	{
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object));
		Dictionary<Type, int> dictionary = new Dictionary<Type, int>();
		Dictionary<Type, List<UnityEngine.Object>> dictionary2 = new Dictionary<Type, List<UnityEngine.Object>>();
		UnityEngine.Object[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			UnityEngine.Object @object = array2[i];
			if (!dictionary.ContainsKey(@object.GetType()))
			{
				dictionary.Add(@object.GetType(), 0);
				dictionary2.Add(@object.GetType(), new List<UnityEngine.Object>());
			}
			Dictionary<Type, int> dictionary3;
			Dictionary<Type, int> expr_5D = dictionary3 = dictionary;
			Type type;
			Type expr_66 = type = @object.GetType();
			int num = dictionary3[type];
			expr_5D[expr_66] = num + 1;
			dictionary2[@object.GetType()].Add(@object);
		}
		List<KeyValuePair<Type, int>> list = new List<KeyValuePair<Type, int>>();
		foreach (KeyValuePair<Type, int> current in dictionary)
		{
			list.Add(current);
		}
		list.Sort((KeyValuePair<Type, int> k1, KeyValuePair<Type, int> k2) => k2.Value.CompareTo(k1.Value));
		string text = "TsMemLog::ResourceByType";
		foreach (KeyValuePair<Type, int> current2 in list)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"\n\t",
				current2.Key.FullName,
				" (",
				current2.Value,
				")"
			});
		}
		Debug.Log(text);
		return dictionary2;
	}

	public static void AssetBundle()
	{
		string text = "TsMemLog::AssetBundle";
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(AssetBundle));
		UnityEngine.Object[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			UnityEngine.Object @object = array2[i];
			AssetBundle assetBundle = @object as AssetBundle;
			if (assetBundle.mainAsset)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n\t",
					assetBundle.mainAsset.GetType(),
					"  ",
					assetBundle.mainAsset.name
				});
			}
			else
			{
				text = text + "\n\t" + assetBundle.ToString();
			}
		}
		Debug.Log(text);
		NrTSingleton<NrDebugConsole>.Instance.Print(text);
	}

	public static void Resource(string typename)
	{
		string text = "TsMemLog::Resouce Typed " + typename;
		Type type = Type.GetType(typename);
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(type);
		UnityEngine.Object[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			UnityEngine.Object @object = array2[i];
			text = text + "\n\t" + @object.ToString();
		}
		Debug.Log(text);
		NrTSingleton<NrDebugConsole>.Instance.Print(text);
		NrTSingleton<NrLogSystem>.Instance.AddString(text, true);
	}

	public static void TextureByFormat()
	{
		string text = "TsMemLog::TextureByFormat";
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(Texture2D));
		UnityEngine.Object[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			UnityEngine.Object @object = array2[i];
			Texture2D texture2D = @object as Texture2D;
			if (texture2D)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n\tTX : ",
					texture2D.name,
					" w:",
					texture2D.width,
					" h:",
					texture2D.height,
					" format:",
					texture2D.format
				});
			}
		}
		Debug.Log(text);
		NrTSingleton<NrDebugConsole>.Instance.Print(text);
	}
}
