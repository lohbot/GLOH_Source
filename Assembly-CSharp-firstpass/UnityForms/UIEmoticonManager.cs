using GameMessage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityForms
{
	public class UIEmoticonManager : NrTSingleton<UIEmoticonManager>
	{
		private Dictionary<string, UIEmoticonInfo> uiEmoticonDictionary;

		private Dictionary<string, string> _EmoticonENGName;

		private bool bLoad;

		public Dictionary<string, UIEmoticonInfo> UIEmoticonDictionary
		{
			get
			{
				return this.uiEmoticonDictionary;
			}
		}

		public Dictionary<string, string> UIEmoticonENGKey
		{
			get
			{
				return this._EmoticonENGName;
			}
		}

		private UIEmoticonManager()
		{
			this.uiEmoticonDictionary = new Dictionary<string, UIEmoticonInfo>();
			this._EmoticonENGName = new Dictionary<string, string>();
		}

		public bool LoadUIEmoticonDictionary()
		{
			if (this.bLoad)
			{
				return false;
			}
			string path = NrTSingleton<UIDataManager>.Instance.FilePath + "UIEmoticonDictionary" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
			TextAsset textAsset = (TextAsset)CResources.Load(path);
			if (null == textAsset)
			{
				TsLog.Log("Failed LoadUIEmoticonDictionary", new object[0]);
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
				string[] array2 = array[i].Split(separator);
				UIEmoticonInfo uIEmoticonInfo = new UIEmoticonInfo();
				uIEmoticonInfo.infoLoader.Material = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<UIDataManager>.Instance.FilePath, "Material/", array2[4]);
				uIEmoticonInfo.infoLoader.UVs = new Rect(float.Parse(array2[5]), float.Parse(array2[6]), float.Parse(array2[7]), float.Parse(array2[8]));
				if (a == array2[9].TrimEnd(new char[0]))
				{
					uIEmoticonInfo.infoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_MIN;
				}
				else if (a2 == array2[10].TrimEnd(new char[0]))
				{
					uIEmoticonInfo.infoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_1x3;
				}
				else if (a3 == array2[11].TrimEnd(new char[0]))
				{
					uIEmoticonInfo.infoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_3x1;
				}
				else if (a4 == array2[12].TrimEnd(new char[0]))
				{
					uIEmoticonInfo.infoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_3x3;
				}
				else
				{
					TsLog.Log("Don't Set  " + i, new object[0]);
				}
				uIEmoticonInfo.infoLoader.ButtonCount = byte.Parse(array2[10]);
				if (0 >= uIEmoticonInfo.infoLoader.ButtonCount)
				{
					TsLog.Log(string.Concat(new object[]
					{
						"Wrong Value ButtonCount :",
						array2[10],
						"/",
						i
					}), new object[0]);
					uIEmoticonInfo.infoLoader.ButtonCount = 4;
				}
				uIEmoticonInfo.delays[0] = float.Parse(array2[11]);
				uIEmoticonInfo.delays[1] = float.Parse(array2[12]);
				uIEmoticonInfo.delays[2] = float.Parse(array2[13]);
				if (!this.uiEmoticonDictionary.ContainsKey("^" + MsgHandler.HandleReturn<string>("GetTextFromInterface", new object[]
				{
					array2[0]
				})))
				{
					this.uiEmoticonDictionary.Add("^" + MsgHandler.HandleReturn<string>("GetTextFromInterface", new object[]
					{
						array2[0]
					}), uIEmoticonInfo);
					this._EmoticonENGName.Add(array2[2], array2[0]);
				}
				else
				{
					TsLog.Log(string.Concat(new object[]
					{
						"Ű���� �ߺ��Դϴ� = ",
						array2[0],
						" ",
						Time.time
					}), new object[0]);
				}
			}
			Resources.UnloadAsset(textAsset);
			CResources.Delete(path);
			this.bLoad = true;
			return true;
		}

		public bool FindUIEmoticonDictionary(string key, ref UIEmoticonInfo data)
		{
			if (this.uiEmoticonDictionary.ContainsKey(key))
			{
				data = this.uiEmoticonDictionary[key];
				return true;
			}
			data = null;
			return false;
		}

		public UIEmoticonInfo FindUIEmoticonDictionary(string key)
		{
			if (this.uiEmoticonDictionary.ContainsKey(key))
			{
				return this.uiEmoticonDictionary[key];
			}
			return null;
		}
	}
}
