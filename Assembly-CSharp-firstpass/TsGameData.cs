using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TsGameData
{
	public enum FORMAT
	{
		Unknown,
		TABLE,
		NDT,
		XML
	}

	public string CreateAssetBundleName = string.Empty;

	public long tsGameDataVersion;

	public int GameDataColumCount;

	public List<string> serializeGameDatas = new List<string>();

	public TsGameData.FORMAT Format
	{
		get
		{
			string[] array = this.CreateAssetBundleName.Split(new char[]
			{
				':'
			});
			if (array.Length != 0)
			{
				string text = array[0].ToLower();
				string text2 = text;
				switch (text2)
				{
				case "table":
					return TsGameData.FORMAT.TABLE;
				case "ndt":
					return TsGameData.FORMAT.NDT;
				case "xml":
					return TsGameData.FORMAT.XML;
				}
			}
			return TsGameData.FORMAT.Unknown;
		}
	}

	public string GameDataFileName
	{
		get
		{
			string[] array = this.CreateAssetBundleName.Split(new char[]
			{
				':'
			});
			if (array.Length != 0)
			{
				return array[1];
			}
			return this.CreateAssetBundleName;
		}
	}

	public int RowCount
	{
		get
		{
			return this.GameDataColumCount;
		}
	}

	public int ColumnCount
	{
		get
		{
			if (this.serializeGameDatas.Count != 0 && this.GameDataColumCount != 0)
			{
				return this.serializeGameDatas.Count / this.GameDataColumCount;
			}
			return 0;
		}
	}

	public long DataRevision
	{
		get
		{
			return this.tsGameDataVersion;
		}
		set
		{
			this.tsGameDataVersion = value;
		}
	}

	public string FileName
	{
		get
		{
			string[] array = this.CreateAssetBundleName.Split(new char[]
			{
				':'
			});
			if (array.Length == 1)
			{
				return array[0];
			}
			if (1 < array.Length)
			{
				return array[1];
			}
			return "(noname)";
		}
	}

	public override string ToString()
	{
		return string.Format("TsGameData[{0}] Row/Col[{1}/{2}] Ver[{3}]", new object[]
		{
			this.CreateAssetBundleName,
			this.RowCount,
			this.ColumnCount,
			this.DataRevision
		});
	}

	public bool SetGameData(List<string> listGameData, string createFileName, long nVersion, int nRowCount)
	{
		if (listGameData.Count <= 0 || string.IsNullOrEmpty(createFileName))
		{
			Debug.Log(" GameData is Empty!!!!!!!!!   FileName = " + createFileName);
			return false;
		}
		this.serializeGameDatas.Clear();
		this.serializeGameDatas.AddRange(listGameData);
		this.CreateAssetBundleName = createFileName;
		this.tsGameDataVersion = nVersion;
		this.GameDataColumCount = nRowCount;
		return true;
	}

	public bool SetGameData(TsGameData gameData)
	{
		if (gameData == null)
		{
			Debug.Log(" TsGameDataAdapter setGameData Failed !!!  GameData is null");
			return false;
		}
		return this.SetGameData(gameData.serializeGameDatas, gameData.CreateAssetBundleName, 0L, gameData.GameDataColumCount);
	}
}
