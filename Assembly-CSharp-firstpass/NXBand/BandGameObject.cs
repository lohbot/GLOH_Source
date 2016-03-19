using NetServ.Net.Json;
using System;
using System.IO;
using UnityEngine;

namespace NXBand
{
	public class BandGameObject : MonoBehaviour
	{
		private static string REQUEST_CODE = "requestCode";

		private static string RESULT_CODE = "resultCode";

		private static string RESULT = "result";

		private BandResultHandler _resultHandler;

		public void setHandler(BandResultHandler resultHandler)
		{
			this._resultHandler = resultHandler;
		}

		public void onBandResult(string jsonResult)
		{
			JsonParser jsonParser = new JsonParser(new StringReader(jsonResult), true);
			JsonObject jsonObject = jsonParser.ParseObject();
			RequestCode reqCode = (RequestCode)((JsonNumber)jsonObject[BandGameObject.REQUEST_CODE]).Value;
			int num = -1;
			NXBandResult result = null;
			if (jsonObject.ContainsKey(BandGameObject.RESULT))
			{
				JsonObject jsonObject2 = (JsonObject)jsonObject[BandGameObject.RESULT];
				if (jsonObject2.ContainsKey("result_data"))
				{
					result = NXBandResult.makeFromJson(reqCode, (JsonObject)jsonObject2["result_data"]);
				}
				if (jsonObject2.ContainsKey("result_code"))
				{
					num = (int)((JsonNumber)((JsonObject)jsonObject[BandGameObject.RESULT])["result_code"]).Value;
				}
			}
			if (num < 0)
			{
				num = (int)((JsonNumber)jsonObject[BandGameObject.RESULT_CODE]).Value;
			}
			if (this._resultHandler != null)
			{
				BandResultHandler resultHandler = this._resultHandler;
				this._resultHandler = null;
				resultHandler.onResult(reqCode, num, result);
			}
		}
	}
}
