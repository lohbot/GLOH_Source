using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityForms
{
	public class UIImageInfoManager : NrTSingleton<UIImageInfoManager>
	{
		private Dictionary<string, UIBaseInfoLoader> uiImageDictionary;

		private bool bLoad;

		private UIImageInfoManager()
		{
			this.uiImageDictionary = new Dictionary<string, UIBaseInfoLoader>();
			this.LoadUIImageDictionary();
		}

		private bool LoadUIImageDictionary()
		{
			if (this.bLoad)
			{
				return false;
			}
			string path = NrTSingleton<UIDataManager>.Instance.FilePath + "UIImageDictionary" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
			TextAsset textAsset = (TextAsset)CResources.Load(path);
			if (null == textAsset)
			{
				TsLog.Log("Failed UIImageDictionary", new object[0]);
				return false;
			}
			char[] separator = new char[]
			{
				'\t'
			};
			string a = "1:1";
			string a2 = "1:3";
			string a3 = "3:1";
			string a4 = "3:3";
			string[] array = textAsset.text.Split(new char[]
			{
				'\n'
			});
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length != 0)
				{
					string[] array2 = array[i].Split(separator);
					if (array2.Length > 1)
					{
						UIBaseInfoLoader uIBaseInfoLoader = new UIBaseInfoLoader();
						uIBaseInfoLoader.Material = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<UIDataManager>.Instance.FilePath, "Material/", array2[1]);
						uIBaseInfoLoader.UVs = new Rect(float.Parse(array2[2]), float.Parse(array2[3]), float.Parse(array2[4]), float.Parse(array2[5]));
						if (a == array2[6].TrimEnd(new char[0]))
						{
							uIBaseInfoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_MIN;
						}
						else if (a2 == array2[6].TrimEnd(new char[0]))
						{
							uIBaseInfoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_1x3;
						}
						else if (a3 == array2[6].TrimEnd(new char[0]))
						{
							uIBaseInfoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_3x1;
						}
						else if (a4 == array2[6].TrimEnd(new char[0]))
						{
							uIBaseInfoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_3x3;
						}
						else
						{
							TsLog.Log("Don't Set : {0} / {1}", new object[]
							{
								array2[0],
								i
							});
						}
						uIBaseInfoLoader.ButtonCount = byte.Parse(array2[7]);
						if (0 >= uIBaseInfoLoader.ButtonCount)
						{
							TsLog.Log(string.Concat(new object[]
							{
								"Wrong Value ButtonCount :",
								array2[7],
								"/",
								i
							}), new object[0]);
							uIBaseInfoLoader.ButtonCount = 4;
						}
						if (array2.Length > 8)
						{
							uIBaseInfoLoader.Pattern = (int.Parse(array2[8]) > 0);
						}
						string text = array2[0].Trim();
						uIBaseInfoLoader.StyleName = text;
						if (!this.uiImageDictionary.ContainsKey(text))
						{
							this.uiImageDictionary.Add(text, uIBaseInfoLoader);
						}
						else
						{
							TsLog.Log(string.Concat(new object[]
							{
								"This Key Value Is Overlap = ",
								text,
								" ",
								Time.time
							}), new object[0]);
						}
					}
				}
			}
			Resources.UnloadAsset(textAsset);
			CResources.Delete(path);
			this.bLoad = true;
			return true;
		}

		public bool FindUIImageDictionary(string key, ref UIBaseInfoLoader data)
		{
			key = key.Trim();
			if (this.uiImageDictionary.ContainsKey(key))
			{
				if (data != null)
				{
					data.Set(this.uiImageDictionary[key]);
				}
				else
				{
					data = new UIBaseInfoLoader();
					data.Set(this.uiImageDictionary[key]);
				}
				return true;
			}
			return false;
		}

		public UIBaseInfoLoader FindUIImageDictionary(string key)
		{
			if (this.uiImageDictionary.ContainsKey(key))
			{
				return this.uiImageDictionary[key];
			}
			return null;
		}
	}
}
