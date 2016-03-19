using System;
using UnityEngine;

public class TsGameDataAdapter : MonoBehaviour
{
	[SerializeField]
	private TsGameData _gameData = new TsGameData();

	public TsGameData GameData
	{
		get
		{
			return this._gameData;
		}
	}
}
